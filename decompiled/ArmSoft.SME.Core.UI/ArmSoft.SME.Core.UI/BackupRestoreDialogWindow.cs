using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.Edge;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Helpers.Backups;
using DevExpress.Xpf.Editors;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI;

public class BackupRestoreDialogWindow : DialogWindow
{
	private readonly DropDownControl _backup;

	private readonly LayoutGroupExt _backupInfoLayout;

	private readonly string _serviceAddress;

	private readonly string _dbName;

	public BackupRestoreDialogWindow(string serviceAddress, string dbName)
	{
		Caption = ArmSoft.SME.Common.Resources.Restore;
		_serviceAddress = serviceAddress;
		_dbName = dbName;
		_backup = AddDropDownControl("Backup", ArmSoft.SME.Common.Resources.Backup, new Backups(_serviceAddress), isRequired: true);
		_backup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Backup_EditValueChanged).Handler;
		ButtonInfo buttonInfo = new ButtonInfo
		{
			GlyphKind = GlyphKind.Regular
		};
		buttonInfo.Click += new SafeEventHandler<RoutedEventArgs>(BrowseButton_Click).Handler;
		_backup.InnerCodeEdit.Buttons.Add(buttonInfo);
		_backupInfoLayout = AddHorizontalLayoutGroup();
		AddTextBlock(_backupInfoLayout, ArmSoft.SME.Common.Resources.InvalidBackup, TextBlockImage.Error);
		_backupInfoLayout.Visibility = Visibility.Collapsed;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(BackupRestore_WindowValidation).Handler;
	}

	private void Backup_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_backup.Code.EndsWith(Settings.NewBackupExtension))
		{
			_okButton.IsEnabled = true;
			_backupInfoLayout.Visibility = Visibility.Collapsed;
			return;
		}
		using EdgeAdminClient edgeAdminClient = new EdgeAdminClient(Settings.LoginService.EdgeServiceAddress, ADHelper.ASAdminB2CToken);
		int serverId = DatabaseHelper.GetCloudDatabase(Settings.LoginService.DatabaseId).ServerId;
		BackupInfoEx backupInfoEx = edgeAdminClient.GetBackupsInfoEx(serverId, Settings.LoginService.Key, _backup.Code).FirstOrDefault();
		bool flag = backupInfoEx != null && ((int)backupInfoEx.Appliaction != (int)Settings.SystemType || backupInfoEx.PSMode != Settings.LoginService.PSMode);
		_okButton.IsEnabled = !flag;
		_backupInfoLayout.Visibility = ((!flag) ? Visibility.Collapsed : Visibility.Visible);
	}

	private void BrowseButton_Click(object sender, RoutedEventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		string text = "*." + Settings.BackupExtension + ";*." + Settings.NewBackupExtension;
		openFileDialog.DefaultExt = Settings.NewBackupExtension;
		openFileDialog.Filter = "Backup Files (" + text + ")|" + text;
		if (!openFileDialog.ShowDialog().Value)
		{
			return;
		}
		Mouse.OverrideCursor = Cursors.Wait;
		try
		{
			UploadDownloadRequest request = new UploadDownloadRequest
			{
				ProgressStep = ProgressStep.ImportBackupFile,
				ServiceAddress = Settings.LoginService.ServiceAddress,
				SourceFileName = openFileDialog.FileName,
				DestinationFileName = openFileDialog.SafeFileName
			};
			FilesManager fileManager = new FilesManager(request);
			AsyncHelper.RunSync(async () =>
			{
				await fileManager.UploadBackupFileAsync();
			});
		}
		catch (Exception ex)
		{
			if (ex.InnerException != null)
			{
				throw ex.InnerException;
			}
			throw;
		}
		_backup.Code = openFileDialog.SafeFileName;
		Mouse.OverrideCursor = null;
	}

	private void BackupRestore_WindowValidation(object sender, DialogWindowValidationArgs args)
	{
		if (MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.DBCurDataRemove, _dbName), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			BackupRestoreRequest backupRestoreRequest = new BackupRestoreRequest();
			backupRestoreRequest.Type = OperationTypes.Restore;
			backupRestoreRequest.ServiceAddress = _serviceAddress;
			backupRestoreRequest.DatabaseName = _dbName;
			backupRestoreRequest.FileName = _backup.Code;
			backupRestoreRequest.DoNotArchiveBackup = _backup.Code.EndsWith(Settings.NewBackupExtension);
			new BackupRestoreProgressWindow(backupRestoreRequest).ShowDialog();
			if (Settings.LoginService.CacheStructChanged)
			{
				Settings.HostingEnvironment.LogOut(bSaveSettings: false);
				Settings.HostingEnvironment.LogIn(isOnline: true);
				args.isValid = false;
			}
		}
	}
}
