using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViewsData;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Helpers.Backups;
using DevExpress.Data;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.DataViews;

[DataView(23)]
public class Backups : DataView<BackupsDataProvider.DataRow, BackupsDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly Backups _dataView;

		private readonly DropDownControl _mode;

		public DataViewDialog(Backups dataView, bool useParamValues)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = dataView.Caption;
			MinWidth = 600.0;
			_mode = AddDropDownControl("Mode", ArmSoft.SME.Common.Resources.VersionCapital, new SmartEnum(typeof(BackupModeSE)), isRequired: true, allowAdditionButtons: true);
			_mode.Code = (useParamValues ? dataView.Mode : BackupModeSE.NewMode.Value);
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.Mode = _mode.Code;
		}
	}

	public string ServiceAddress { get; set; }

	public override string HelpId => "AdminTools_Backups";

	public override bool AllowAdd => Mode == BackupModeSE.NewMode.Value;

	public override bool AllowDelete => true;

	public override bool AllowDeleteSelected => true;

	public override bool DialogSupported => true;

	public string Mode
	{
		get
		{
			return (string)Parameters["Mode"].Value;
		}
		set
		{
			Parameters["Mode"].Value = value;
		}
	}

	public Backups()
	{
		DataProvider = new BackupsDataProvider();
		FillColumnsFromDataProvider();
		AddParameter("Mode", Resources.Account, "");
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.Mode = Mode;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (Mode == BackupModeSE.NewMode.Value)
		{
			dvp.Set_ColumnVisible("CompressedSize", value: false);
			dvp.Set_ColumnVisible("DbSize", value: true);
			dvp.Set_ColumnVisible("IsAutoBackup", value: true);
			dvp.Set_ColumnVisible("IsDailyBackup", value: true);
			dvp.Set_ColumnVisible("IsMonthlyBackup", value: true);
			dvp.Set_ColumnVisible("IsAnnualBackup", value: true);
		}
		else
		{
			dvp.Set_ColumnVisible("DbSize", value: false);
			dvp.Set_ColumnVisible("CompressedSize", value: true);
			dvp.Set_ColumnVisible("IsAutoBackup", value: false);
			dvp.Set_ColumnVisible("IsDailyBackup", value: false);
			dvp.Set_ColumnVisible("IsMonthlyBackup", value: false);
			dvp.Set_ColumnVisible("IsAnnualBackup", value: false);
		}
		dvp.Set_ColumnSortOrder("DateCreated", ColumnSortOrder.Descending);
	}

	public override void InitContextFunctions()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			if ((BackupsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "Restore", Resources.Restore, Restore);
				Settings.HostingEnvironment.CurrentViewPanel.AddContextMenuGroup("Export", Resources.Export);
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("Export", "Download", Resources.File, Download, "usb_flash_drive_16.svg", "usb_flash_drive_32.svg");
			}
			if (Settings.HostingEnvironment.CurrentViewPanel.GetSelectedAny())
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("GroupExport", "GroupExport", Resources.GroupExport, GroupExport);
			}
			Settings.HostingEnvironment.CurrentViewPanel.AddContextMenuGroup("Import", Resources.Import);
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("Import", "Upload", Resources.File, Upload, "usb_flash_drive_16.svg", "usb_flash_drive_32.svg");
		}
	}

	public override void Add()
	{
		Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = Resources.CreateBackup,
			HelpId = HelpId
		};
		dialogWindow.AddDropDownControl("Name", Resources.Name, new ArmSoft.SME.Core.UI.DropDowns.Databases(ServiceAddress), isRequired: true).Code = Settings.LoginService.Database;
		dialogWindow.AddStringControl("Comment", Resources.Comment, 50, isRequired: true);
		Mouse.OverrideCursor = null;
		if (dialogWindow.ShowDialog().Value)
		{
			string text = (string)dialogWindow["Name"];
			string text2 = $"{text} {DateTime.Now.FormatYYYYMMDD()} {DateTime.Now.FormatHHMMSS()}";
			BackupRestoreRequest backupRestoreRequest = new BackupRestoreRequest();
			backupRestoreRequest.Type = OperationTypes.Backup;
			backupRestoreRequest.ServiceAddress = ServiceAddress;
			backupRestoreRequest.DatabaseName = text;
			backupRestoreRequest.FileName = text2;
			backupRestoreRequest.Comment = (string)dialogWindow["Comment"];
			backupRestoreRequest.IsAutoBackup = false;
			backupRestoreRequest.DoNotArchiveBackup = true;
			new BackupRestoreProgressWindow(backupRestoreRequest).ShowDialog();
			if (Settings.HostingEnvironment.CurrentViewPanel != null)
			{
				Settings.HostingEnvironment.CurrentViewPanel.Update(text2 + "." + Settings.NewBackupExtension);
			}
		}
	}

	public override void Delete()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			BackupsDataProvider.DataRow dataRow = (BackupsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null && MessageBox.Show(Resources.ConfirmBckpDel, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				BackupsManager.DeleteBackup(dataRow.FileName, dataRow.DbId);
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.FileName);
			}
		}
	}

	public override void DeleteSelected()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null || MessageBox.Show(Resources.ConfirmSelectedRowsDelete, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
		{
			return;
		}
		Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
		foreach (BackupsDataProvider.DataRow selectedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<BackupsDataProvider.DataRow>())
		{
			BackupsManager.DeleteBackup(selectedRow.FileName, selectedRow.DbId);
			Settings.HostingEnvironment.CurrentViewPanel.Update(selectedRow.FileName);
		}
		Mouse.OverrideCursor = null;
	}

	private void Restore()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		BackupsDataProvider.DataRow dataRow = (BackupsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null)
		{
			return;
		}
		if (!dataRow.DbName.Equals(Settings.LoginService.Database, StringComparison.CurrentCultureIgnoreCase))
		{
			throw new ASException(string.Format(Resources.CantRestoreDBFrAnDB, dataRow.DbName));
		}
		if (MessageBox.Show(string.Format(Resources.DBCurDataRemove, Settings.LoginService.Database), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			BackupRestoreRequest request = ((!(Mode == BackupModeSE.OldMode)) ? new BackupRestoreRequest
			{
				Type = OperationTypes.Restore,
				ServiceAddress = ServiceAddress,
				DatabaseName = dataRow.DbName,
				FileName = dataRow.FileName,
				DoNotArchiveBackup = true
			} : new BackupRestoreRequest
			{
				Type = OperationTypes.Restore,
				ServiceAddress = ServiceAddress,
				DatabaseName = dataRow.DbName,
				FileName = dataRow.FileName
			});
			new BackupRestoreProgressWindow(request).ShowDialog();
			if (!ConfigHelper.ASAdminLogin && Settings.LoginService.CacheStructChanged)
			{
				Settings.HostingEnvironment.LogOut(bSaveSettings: false);
				Settings.HostingEnvironment.LogIn(isOnline: true);
			}
		}
	}

	private void Download()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		BackupsDataProvider.DataRow dataRow = (BackupsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			string text = ((Mode == BackupModeSE.OldMode.Value) ? ("." + Settings.BackupExtension) : Settings.NewBackupExtension);
			Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
			{
				DefaultExt = text,
				Filter = "Backup Files (" + text + ")|*" + text,
				FileName = dataRow.FileName
			};
			if (saveFileDialog.ShowDialog().Value)
			{
				UploadDownloadRequest uploadDownloadRequest = new UploadDownloadRequest();
				uploadDownloadRequest.ProgressStep = ProgressStep.CopyToLocalFile;
				uploadDownloadRequest.ServiceAddress = ServiceAddress;
				uploadDownloadRequest.SourceFileName = dataRow.FileName;
				uploadDownloadRequest.DestinationFileName = saveFileDialog.FileName;
				new FileTransferProgressWindow(uploadDownloadRequest).ShowDialog();
			}
		}
	}

	private void GroupExport()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
		{
			Description = Resources.SelectLocalDefaultBackupPath
		};
		if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
		foreach (BackupsDataProvider.DataRow selectedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<BackupsDataProvider.DataRow>())
		{
			UploadDownloadRequest request = new UploadDownloadRequest
			{
				ProgressStep = ProgressStep.CopyToLocalFile,
				ServiceAddress = ServiceAddress,
				SourceFileName = selectedRow.FileName,
				DestinationFileName = folderBrowserDialog.SelectedPath + "\\" + selectedRow.FileName
			};
			FilesManager filesManager = new FilesManager(request);
			AsyncHelper.RunSync(async () => await filesManager.DownloadBackupFileAsync());
		}
		Mouse.OverrideCursor = null;
		MessageBox.Show(Resources.GroupExportOk + " " + folderBrowserDialog.SelectedPath);
	}

	private void Upload()
	{
		Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
		string text = (openFileDialog.DefaultExt = ((Mode == BackupModeSE.OldMode.Value) ? ("." + Settings.BackupExtension) : Settings.NewBackupExtension));
		openFileDialog.Filter = "Backup Files (" + text + ")|*" + text;
		if (openFileDialog.ShowDialog().Value)
		{
			UploadDownloadRequest uploadDownloadRequest = new UploadDownloadRequest();
			uploadDownloadRequest.ProgressStep = ProgressStep.ImportBackupFile;
			uploadDownloadRequest.ServiceAddress = ServiceAddress;
			uploadDownloadRequest.SourceFileName = openFileDialog.FileName;
			uploadDownloadRequest.DestinationFileName = openFileDialog.SafeFileName;
			new FileTransferProgressWindow(uploadDownloadRequest).ShowDialog();
			if (Settings.HostingEnvironment.CurrentViewPanel != null)
			{
				Settings.HostingEnvironment.CurrentViewPanel.Update(openFileDialog.SafeFileName);
			}
		}
	}
}
