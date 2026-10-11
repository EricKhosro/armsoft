using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.Cloud.Client.Edge;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.DPR;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Helpers.Backups;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Core.UI.DBIntegrity;

public class DBArchivationDialogWindow : DialogWindow
{
	public class LogRow : IDXDataErrorInfo
	{
		public DateTime Date { get; set; }

		public string Message { get; set; }

		public bool Error { get; set; }

		public void GetError(DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (Error)
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = Message;
			}
		}

		public void GetPropertyError(string propertyName, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
		}
	}

	private readonly DateEditControl _date;

	private readonly CheckEditExt _createFirstDb;

	private readonly TextEditExt _firstDbName;

	private readonly TextEditExt _firstDbComment;

	private readonly CheckEditExt _createSecondDb;

	private readonly TextEditExt _secondDbName;

	private readonly TextEditExt _secondDbComment;

	private readonly DocumentGridControl _logGrid;

	private readonly DocumentGridColumn _dateCol;

	private readonly DocumentGridColumn _messageCol;

	private readonly TableView _logView;

	private readonly BindingList<LogRow> _logRows;

	private readonly string _dprName;

	private CreateDatabaseManager _createDatabaseManager;

	private BackupsManager _backupsManager;

	private int _firstDbId;

	private int _secondDbId;

	private string _fileName;

	private string _dbTemplate;

	private string _edgeService;

	public DBArchivationDialogWindow(string dprName)
		: base(Settings.HostingEnvironment)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		_dprName = dprName;
		Title = ArmSoft.SME.Common.Resources.Archive;
		Icon = ImageService.GetImage("database_lightning_16.svg");
		HelpId = "AdminTools_Archive";
		ResizeMode = ResizeMode.CanResize;
		SizeToContent = SizeToContent.Manual;
		Width = SizeCalculator.Width(800.0);
		Height = SizeCalculator.Height(600.0);
		_date = AddDateControl("ArchivationDate", ArmSoft.SME.Common.Resources.DateWord, useLongDate: false, isRequired: true);
		_createFirstDb = AddBooleanControl("FirstDb", ArmSoft.SME.Common.Resources.CreateFirstDb);
		_createFirstDb.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CreateFirstDb_EditValueChanged).Handler;
		_firstDbName = AddStringControl("FirstDbName", ArmSoft.SME.Common.Resources.ArchivationFirstDBName, 20, isRequired: true);
		_firstDbComment = AddStringControl("FirstDbComment", ArmSoft.SME.Common.Resources.ArchivationFirstDBComment, 50);
		_createSecondDb = AddBooleanControl("SecondDb", ArmSoft.SME.Common.Resources.CreateSecondDb);
		_createSecondDb.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CreateSecondDb_EditValueChanged).Handler;
		_secondDbName = AddStringControl("SecondDbName", ArmSoft.SME.Common.Resources.ArchivationSecondDBName, 20);
		_secondDbComment = AddStringControl("SecondDbComment", ArmSoft.SME.Common.Resources.ArchivationSecondDBComment, 50);
		LayoutGroupExt layoutGroupExt = AddVerticalLayoutGroup(null, LayoutGroupView.GroupBox, ArmSoft.SME.Common.Resources.ArchivationResults);
		layoutGroupExt.UseLayoutRounding = true;
		_logGrid = AddGridControl(layoutGroupExt, "LogGrid");
		_logGrid.AllowAdd = false;
		_logGrid.AllowDelete = false;
		_logGrid.AllowDuplicate = false;
		_logGrid.ShowButtons = false;
		_logGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
		_logGrid.VerticalAlignment = VerticalAlignment.Stretch;
		_dateCol = _logGrid.AddDateColumn("Date", "Date", useLongDate: true);
		_dateCol.FixedWidth = true;
		_dateCol.EditSettings = new DateEditSettings
		{
			Mask = "hh:mm:ss",
			MaskUseAsDisplayFormat = true
		};
		_dateCol.AllowEditing = DefaultBoolean.False;
		_messageCol = _logGrid.AddStringColumn("Message", "Message", 550);
		_messageCol.AllowEditing = DefaultBoolean.False;
		_logRows = new BindingList<LogRow>();
		_logGrid.ItemsSource = _logRows;
		_logView = _logGrid.View;
		_logView.AllowMoveColumnToDropArea = false;
		_logView.IsColumnMenuEnabled = false;
		_logView.IsRowCellMenuEnabled = true;
		_logView.ShowColumnHeaders = false;
		_logView.AllowColumnFiltering = false;
		_logView.EnterMoveNextColumn = true;
		_logView.ShowFocusedRectangle = false;
		_logView.ShowGroupPanel = false;
		_logView.AllowBestFit = true;
		_logView.AutoWidth = true;
		_logView.EditorButtonShowMode = EditorButtonShowMode.ShowOnlyInEditor;
		_logView.NewItemRowPosition = NewItemRowPosition.None;
		_logView.NavigationStyle = GridViewNavigationStyle.Cell;
		_firstDbName.Validate += new SafeEventHandler<ValidationEventArgs>(DbName_Validate).Handler;
		_secondDbName.Validate += new SafeEventHandler<ValidationEventArgs>(DbName_Validate).Handler;
		Loaded += new SafeEventHandler<RoutedEventArgs>(DatabaseArchivationWindow_Loaded).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(DatabaseArchivationWindow_Closing).Handler;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(DBArchivation_WindowValidation).Handler;
	}

	private async void DBArchivation_WindowValidation(object sender, DialogWindowValidationArgs e)
	{
		_ = 1;
		try
		{
			if (!_createFirstDb.IsChecked.Value && !_createSecondDb.IsChecked.Value)
			{
				e.isValid = false;
				return;
			}
			SetReadOnly(isReadOnly: true);
			AddToLog(ArmSoft.SME.Common.Resources.ArchivationStart);
			ArchivationRequestModel request = new ArchivationRequestModel
			{
				ArchivationDate = _date.EditValue.Value,
				FirstDbName = _firstDbName.Text,
				SecondDbName = _secondDbName.Text,
				MainDb = true
			};
			ArchivationResponseModel archivationResponseModel = DataProcessingRequestHelper.Execute<ArchivationRequestModel, ArchivationResponseModel>(DPRType.Other, _dprName, request, 1, 0, null, null, null, null, hideAbortButton: false, null, null, 900.0);
			request.MainDb = false;
			AddRowsInfoToLog(archivationResponseModel.RowInfo, archivationResponseModel.Succeeded);
			archivationResponseModel.RowInfo = new List<string>();
			if (!archivationResponseModel.Succeeded)
			{
				SetReadOnly(isReadOnly: false);
				e.isValid = false;
				return;
			}
			CheckDatabases();
			bool flag = _createFirstDb.IsChecked.Value;
			if (flag)
			{
				flag = !(await CreateAndRestoreDatabase(_firstDbName.Text, firstDb: true, _firstDbId));
			}
			if (flag)
			{
				SetReadOnly(isReadOnly: false);
				e.isValid = false;
				return;
			}
			flag = _createSecondDb.IsChecked.Value;
			if (flag)
			{
				flag = !(await CreateAndRestoreDatabase(_secondDbName.Text, firstDb: false, _secondDbId));
			}
			if (flag)
			{
				SetReadOnly(isReadOnly: false);
				e.isValid = false;
				return;
			}
			if (_createFirstDb.IsChecked.Value)
			{
				AddToLog(ArmSoft.SME.Common.Resources.ArchivationStart + " - " + _firstDbName.Text);
				request.FirstDb = true;
				ApiClient apiClient = CreateApiClientForOtherDb(_firstDbId);
				apiClient.CacheRoutes.RefreshServiceCaches();
				archivationResponseModel = DataProcessingRequestHelper.Execute<ArchivationRequestModel, ArchivationResponseModel>(DPRType.Other, _dprName, request, 1, 0, null, null, null, null, hideAbortButton: false, null, apiClient, 900.0);
				request.FirstDb = false;
				AddRowsInfoToLog(archivationResponseModel.RowInfo, archivationResponseModel.Succeeded);
				archivationResponseModel.RowInfo = new List<string>();
				if (!archivationResponseModel.Succeeded)
				{
					SetReadOnly(isReadOnly: false);
					e.isValid = false;
					return;
				}
			}
			if (_createSecondDb.IsChecked.Value)
			{
				AddToLog(ArmSoft.SME.Common.Resources.ArchivationStart + " - " + _secondDbName.Text);
				request.SecondDb = true;
				ApiClient apiClient2 = CreateApiClientForOtherDb(_secondDbId);
				apiClient2.CacheRoutes.RefreshServiceCaches();
				archivationResponseModel = DataProcessingRequestHelper.Execute<ArchivationRequestModel, ArchivationResponseModel>(DPRType.Other, _dprName, request, 1, 0, null, null, null, null, hideAbortButton: false, null, apiClient2, 900.0);
				AddRowsInfoToLog(archivationResponseModel.RowInfo, archivationResponseModel.Succeeded);
				if (!archivationResponseModel.Succeeded)
				{
					SetReadOnly(isReadOnly: false);
					e.isValid = false;
					return;
				}
			}
			AddToLog(ArmSoft.SME.Common.Resources.ArchivationSuccessful);
		}
		catch (Exception ex)
		{
			AddToLog(ex.Message, error: true);
		}
		SetReadOnly(isReadOnly: false);
		e.isValid = false;
	}

	private void CheckDatabases()
	{
		if (!Settings.LoginService.IsDBAdmin || Settings.LoginService.DemoMode)
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.DontHavePermissionToCreatePO, ArmSoft.SME.Common.Resources.Database));
		}
		using CloudClient cloudClient = Settings.CloudClient;
		DatabaseReservationRequest databaseReservationRequest = new DatabaseReservationRequest
		{
			Key = Settings.LoginService.Key,
			PSMode = Settings.LoginService.PSMode,
			AppService = Settings.LoginService.ServiceAddress
		};
		if (_createFirstDb.IsChecked.Value)
		{
			string text = cloudClient.AS7X.CheckCloudDatabaseName(_firstDbName.Text);
			if (!string.IsNullOrEmpty(text))
			{
				throw new ASException(text);
			}
			databaseReservationRequest.DbName = _firstDbName.Text;
			databaseReservationRequest.Caption = _firstDbComment.Text;
			DatabaseReservationResponse databaseReservationResponse = cloudClient.AS7X.ReserveCloudDatabase(databaseReservationRequest);
			_firstDbId = databaseReservationResponse.DbId;
			_dbTemplate = databaseReservationResponse.SuggestedTemplate;
			_edgeService = databaseReservationResponse.EdgeService;
		}
		if (_createSecondDb.IsChecked.Value)
		{
			string text = cloudClient.AS7X.CheckCloudDatabaseName(_secondDbName.Text);
			if (!string.IsNullOrEmpty(text))
			{
				throw new ASException(text);
			}
			databaseReservationRequest.DbName = _secondDbName.Text;
			databaseReservationRequest.Caption = _secondDbComment.Text;
			DatabaseReservationResponse databaseReservationResponse2 = cloudClient.AS7X.ReserveCloudDatabase(databaseReservationRequest);
			_secondDbId = databaseReservationResponse2.DbId;
			_dbTemplate = databaseReservationResponse2.SuggestedTemplate;
			_edgeService = databaseReservationResponse2.EdgeService;
		}
	}

	private async Task<bool> CreateAndRestoreDatabase(string dbName, bool firstDb, int dbId)
	{
		AddToLog(string.Format(ArmSoft.SME.Common.Resources.ArchivationDBCreation, dbName));
		DatabaseCreateRequest request = new DatabaseCreateRequest
		{
			DbId = dbId,
			Template = _dbTemplate
		};
		_createDatabaseManager = new CreateDatabaseManager(request, _edgeService);
		if (!(await CreateDatabase()))
		{
			return false;
		}
		DatabaseHelper.UpdateUsersRoles(dbId, DatabaseHelper.GetDatabaseUserRoles(Settings.LoginService.DatabaseId));
		if (firstDb || (!_createFirstDb.IsChecked.Value && _createSecondDb.IsChecked.Value))
		{
			_fileName = $"{Settings.LoginService.Database} {DateTime.Now.FormatYYYYMMDD()} {DateTime.Now.FormatHHMMSS()}";
			BackupRestoreRequest backupRestoreRequest = new BackupRestoreRequest
			{
				Type = OperationTypes.Backup,
				DatabaseName = Settings.LoginService.Database,
				FileName = _fileName,
				DoNotArchiveBackup = true,
				Comment = Settings.LoginService.Database + "_Duplicate"
			};
			_backupsManager = new BackupsManager(backupRestoreRequest);
			if (!(await StartBackupRestore(backupRestoreRequest.Type)))
			{
				return false;
			}
			_fileName = _fileName + "." + Settings.NewBackupExtension;
		}
		BackupRestoreRequest backupRestoreRequest2 = new BackupRestoreRequest
		{
			Type = OperationTypes.Restore,
			DatabaseName = dbName,
			FileName = _fileName,
			DoNotArchiveBackup = true,
			Comment = ArmSoft.SME.Common.Resources.Archive
		};
		_backupsManager = new BackupsManager(backupRestoreRequest2);
		if (!(await StartBackupRestore(backupRestoreRequest2.Type)))
		{
			return false;
		}
		if (!firstDb || !_createSecondDb.IsChecked.Value)
		{
			BackupsManager.DeleteBackup(_fileName, Settings.LoginService.DatabaseId);
		}
		return true;
	}

	private async Task<bool> CreateDatabase()
	{
		try
		{
			await _createDatabaseManager.CreateDatabaseAsync();
			return true;
		}
		catch (Exception ex)
		{
			AddToLog(ex.Message, error: true);
			return false;
		}
	}

	private async Task<bool> StartBackupRestore(OperationTypes opType)
	{
		_ = 1;
		try
		{
			switch (opType)
			{
			case OperationTypes.Backup:
				await _backupsManager.CreateBackupAsync();
				break;
			case OperationTypes.Restore:
				await _backupsManager.RestoreBackupAsync();
				break;
			}
			return true;
		}
		catch (Exception ex)
		{
			AddToLog(ex.Message, error: true);
			return false;
		}
	}

	private void DbName_Validate(object sender, ValidationEventArgs e)
	{
		if (!(sender is TextEditExt { IsRequired: not false }))
		{
			return;
		}
		string text = e.Value.ToString();
		if (string.IsNullOrWhiteSpace(text))
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.FieldIsEmptyWithoutName;
			return;
		}
		text = text.Trim();
		byte[] bytes = Encoding.Unicode.GetBytes(text);
		for (int i = 0; i <= bytes.Length - 1; i += 2)
		{
			if ((short)((bytes[i + 1] << 8) | bytes[i]) > 127)
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.InvalidDBName;
			}
		}
	}

	private void CreateFirstDb_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		CreateFirstDb_EditValueChanged();
	}

	private void CreateFirstDb_EditValueChanged()
	{
		_firstDbName.IsRequired = _createFirstDb.IsChecked.Value;
		_firstDbName.IsReadOnly = !_createFirstDb.IsChecked.Value;
		_firstDbName.DoValidate();
		_firstDbComment.IsRequired = _createFirstDb.IsChecked.Value;
		_firstDbComment.IsReadOnly = !_createFirstDb.IsChecked.Value;
		_firstDbComment.DoValidate();
	}

	private void CreateSecondDb_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		CreateSecondDb_EditValueChanged();
	}

	private void CreateSecondDb_EditValueChanged()
	{
		_secondDbName.IsRequired = _createSecondDb.IsChecked.Value;
		_secondDbName.IsReadOnly = !_createSecondDb.IsChecked.Value;
		_secondDbName.DoValidate();
		_secondDbComment.IsRequired = _createSecondDb.IsChecked.Value;
		_secondDbComment.IsReadOnly = !_createSecondDb.IsChecked.Value;
		_secondDbComment.DoValidate();
	}

	private void DatabaseArchivationWindow_Loaded(object sender, RoutedEventArgs e)
	{
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		ControlsHelper.SetFocus(_date);
		_firstDbComment.EditValue = DatabaseHelper.GetCloudDatabase(Settings.LoginService.DatabaseId).Caption;
		_secondDbComment.EditValue = _firstDbComment.EditValue;
		_createFirstDb.IsChecked = true;
		_createSecondDb.IsChecked = true;
	}

	private void SetReadOnly(bool isReadOnly)
	{
		SetControlsReadonly(isReadOnly);
		_okButton.IsEnabled = !isReadOnly;
		_cancelButton.IsEnabled = !isReadOnly;
		if (!isReadOnly)
		{
			CreateFirstDb_EditValueChanged();
			CreateSecondDb_EditValueChanged();
		}
	}

	private void AddToLogInner(LogRow row)
	{
		_logRows.Add(row);
		int rowHandleByVisibleIndex = _logView.Grid.GetRowHandleByVisibleIndex(_logRows.Count - 1);
		_logView.MoveFocusedRow(_logView.Grid.GetRowVisibleIndexByHandle(rowHandleByVisibleIndex));
	}

	private void AddToLog(string msg, bool error = false, DateTime? date = null)
	{
		((DispatcherObject)this).Dispatcher.Invoke((Delegate)new Action<LogRow>(AddToLogInner), (DispatcherPriority)7, new object[1]
		{
			new LogRow
			{
				Date = (date.HasValue ? date.Value : DateTime.Now),
				Message = msg,
				Error = error
			}
		});
	}

	private void AddRowsInfoToLog(List<string> rowInfo, bool succeeded)
	{
		if (rowInfo == null || rowInfo.Count == 0)
		{
			return;
		}
		for (int i = 0; i < rowInfo.Count; i++)
		{
			string text = rowInfo[i];
			int num = text.IndexOf(',');
			bool error = i == rowInfo.Count - 1 && !succeeded;
			if (num >= 0 && DateTime.TryParse(text.Substring(0, num), out var result))
			{
				AddToLog(text.Substring(num + 1), error, result);
			}
			else
			{
				AddToLog(text, error);
			}
		}
	}

	private void DatabaseArchivationWindow_Closing(object sender, CancelEventArgs e)
	{
		if (!_cancelButton.IsEnabled)
		{
			e.Cancel = true;
		}
	}

	private static ApiClient CreateApiClientForOtherDb(int databaseId)
	{
		Settings.ThrowExceptionIfOffline();
		return new ApiClient(Settings.LoginService.ServiceAddress, ApplicationServiceConfig.GetTokenForOtherDb(databaseId), Settings.HttpClient, null);
	}
}
