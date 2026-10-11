using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using AppLife.Api;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.AppManager;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Helpers.Backups;
using ArmSoft.SME.Core.UI.VersionManagement;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ArmSoft.SME.Core.UI.Services;

public class AppUpdateService : IAppUpdateService
{
	public bool UpdateAvailable { get; set; }

	public void TryUpdateAppFiles(bool updateRequired = false)
	{
		bool flag = false;
		try
		{
			bool updateApplied = false;
			((DispatcherObject)Settings.HostingEnvironment).Dispatcher.Invoke((Action)(() =>
			{
				Mouse.OverrideCursor = Cursors.Wait;
				AppUpdateWindow appUpdateWindow = new AppUpdateWindow();
				Mouse.OverrideCursor = null;
				if (appUpdateWindow.ShowDialog().Value)
				{
					Settings.HostingEnvironment.DisableTimerPing();
					ApplyUpdateOptions options = ApplyUpdateOptions.AutoClose;
					Dictionary<string, string> initialSharedProperties = new Dictionary<string, string> { 
					{
						"AppPath",
						AppDomain.CurrentDomain.BaseDirectory
					} };
					appUpdateWindow.Controller.ApplyUpdate(options, initialSharedProperties);
					Settings.HostingEnvironment.Close();
					updateApplied = true;
				}
			}));
			if (!updateApplied & updateRequired)
			{
				Settings.LoginService.Logout();
				flag = true;
				throw new ASException(string.Format(Resources.ExeVersOldNeedUpdate, Settings.AppVersionString));
			}
		}
		catch (Exception ex)
		{
			if (flag)
			{
				throw;
			}
			Settings.ErrorHandler(ex);
		}
	}

	private static void TryUpdateDb()
	{
		string text = Settings.CreateApiClient().AppManager.CheckDbUpgrade();
		bool flag = true;
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		if (ConfigHelper.ASAdminLogin)
		{
			throw new RESTException(string.Format(Resources.DbVersOldNeedOldUI, text));
		}
		try
		{
			BackupRestoreRequest backupRestoreRequest = new BackupRestoreRequest();
			backupRestoreRequest.Type = OperationTypes.Backup;
			backupRestoreRequest.ServiceAddress = Settings.LoginService.EdgeServiceAddress;
			backupRestoreRequest.DatabaseName = Settings.LoginService.Database;
			backupRestoreRequest.DoNotArchiveBackup = true;
			backupRestoreRequest.FileName = $"{Settings.LoginService.Database} {DateTime.Now.FormatYYYYMMDD()} {DateTime.Now.FormatHHMMSS()}";
			backupRestoreRequest.Comment = string.Format(Resources.VersionChangeToX, text);
			new BackupRestoreProgressWindow(backupRestoreRequest, isAutoClose: true).ShowDialog();
		}
		catch
		{
			string text2 = string.Format(Resources.BackUpError, Settings.LoginService.Database);
			Log.Error(text2, $"Database {Settings.LoginService.Database}");
			if (MessageBox.Show(text2 + " \r\n" + Resources.AskContinue, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
			{
				flag = false;
			}
		}
		if (flag)
		{
			Settings.CreateApiClient().AppManager.DeleteJobs();
			DBUpgradeRequest request = new DBUpgradeRequest();
			DataProcessingRequestHelper.Execute(DPRType.Other, "DbUpgrade", request, 1, 0, null, null, Resources.DBVersionUpgrade, hideAbortButton: true);
			SQLiteStorageService requiredService = Settings.ServiceProvider.GetRequiredService<SQLiteStorageService>();
			requiredService.StoreInUserBasedLocalStorage = false;
			requiredService.DeleteDatabase();
		}
		else
		{
			Settings.HostingEnvironment.LogOut(bSaveSettings: false);
		}
	}

	private static void TryUpdateUI()
	{
		IUIManager requiredService = Settings.ServiceProvider.GetRequiredService<IUIManager>();
		if (requiredService.CheckUpgrade())
		{
			new UIUpgrade().ShowMultilineProgressWindow();
		}
		if (new Version(Settings.ParametersService.DBVersionNew()) < requiredService.ServiceMinVersion)
		{
			throw new ASException(string.Format(Resources.ExeVersNewerThenServiceVersion, requiredService.VersionInDB));
		}
	}

	public void TryUpdateApp()
	{
		TryUpdateDb();
		TryUpdateUI();
	}
}
