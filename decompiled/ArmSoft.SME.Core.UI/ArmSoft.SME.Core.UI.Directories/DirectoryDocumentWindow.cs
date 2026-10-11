using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Directories;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Models.UserAccess;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.Directories;

public class DirectoryDocumentWindow<T> : POWindow<T>, IDirectoryDocumentWindow where T : ArmSoft.SME.Common.Models.Directories.Directory, new()
{
	public delegate void CallErrorOnSaveExceptionEventEventHandler(ASException ex);

	protected DirectoryDefinition _definition;

	protected bool _codeIsManualChanged;

	protected bool _codeMagicClickEvenTimes;

	protected ButtonEditExt _code;

	public override bool StoredInDb => PO.StoredInDb;

	public override byte State => DocumentStateSE.Processed;

	public override DirectoryDefinition Definition
	{
		get
		{
			if (_definition == null)
			{
				DefinitionMappingsService.IsDirectoryDocumentWindow(((object)this).GetType(), out var dirType);
				DefinitionMappingsService.TryResolveDirectoryDefinition(dirType, out _definition);
			}
			return _definition;
		}
	}

	public event CallErrorOnSaveExceptionEventEventHandler CallErrorOnSaveExceptionEvent;

	public DirectoryDocumentWindow()
	{
		_ribbonPageForOther.IsVisible = false;
		_barButtonShowISN.IsVisible = false;
	}

	public DirectoryDocumentWindow(T doc)
	{
		_ribbonPageForOther.IsVisible = false;
		_barButtonShowISN.IsVisible = false;
		PO = doc;
	}

	protected override void GetPO()
	{
		PO = Settings.DirectoryService.Create<T>(Definition.POType);
	}

	protected override void OnNew()
	{
		base.OnNew();
		SetNextCode();
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		if (WriteInHistory)
		{
			HistoryHelpers.WriteDirectoryItemInHistory(PO);
		}
	}

	protected override void OnNewAfterSave()
	{
		PO = Settings.DirectoryService.Create<T>(Definition.POType);
		base.OnNewAfterSave();
		SetNextCode();
	}

	protected override void SetEvents()
	{
		base.SetEvents();
		if (_definition.NextCodeParamId.HasValue)
		{
			_code.MagicClick += new SafeEventHandler<MagicClickEventArgs>(Code_MagicClick).Handler;
		}
	}

	public override void Show(WindowViewMode? viewMode = null, bool disableGridValidators = false)
	{
		WindowViewMode valueOrDefault = viewMode.GetValueOrDefault();
		if (!viewMode.HasValue)
		{
			valueOrDefault = ((!StoredInDb) ? WindowViewMode.NewCreate : WindowViewMode.Edit);
			viewMode = valueOrDefault;
		}
		POPermission directoryPermission = Settings.UserAccessService.GetDirectoryPermission(PO.POType);
		if ((viewMode == WindowViewMode.NewCreate || viewMode == WindowViewMode.NewAfterSave) && !directoryPermission.AllowAdd)
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.DontHavePermissionToCreatePO, Definition.Caption), 20008);
		}
		if (viewMode == WindowViewMode.Edit && !directoryPermission.AllowEdit)
		{
			viewMode = WindowViewMode.View;
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{PO.POName} ({Definition.POType}) directory window show with {viewMode} view mode start");
		}
		base.Show(viewMode, disableGridValidators);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{PO.POName} ({Definition.POType}) directory window show with {viewMode} view mode finished");
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDirectoryDefinition))
		{
			CallsRecordingHelper.ExportDirectoryWindowDefinition(PO.POName, Caption, isDialogWindow: false, GetControlsInfoForRecord(withoutValues: true));
		}
	}

	protected override void ReLoad()
	{
		if (PO.Id > 0)
		{
			PO = Settings.DirectoryService.Load<T>(PO.POType, PO.Id);
		}
		else
		{
			PO = Settings.DirectoryService.Load<T>(PO.POType, PO.Code);
		}
		IsChanged = false;
	}

	protected override bool InnerSave(bool isAsDraft = false)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{PO.POName} ({Definition.POType}) directory window save start");
		}
		DirStoreRequest sr = new DirStoreRequest();
		try
		{
			if (InnerValidate(out var errorMgs, isAsDraft))
			{
				bool isNewDocument = !StoredInDb;
				FillDocument(sr);
				if (BeforeStore(sr))
				{
					if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDirectory))
					{
						CallsRecordingHelper.ExportDirectoryWindow(PO.POName, this);
					}
					PO = Settings.DirectoryService.StoreAndLoad(PO, sr);
					AfterStore(sr, isNewDocument);
					UpdateDataViewPanel(DVPID);
					if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
					{
						CallsRecordingHelper.RecordCallStepInFile($"{PO.POName} ({Definition.POType}) directory window save finished");
					}
					return true;
				}
				if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
				{
					CallsRecordingHelper.RecordCallStepInFile($"{PO.POName} ({Definition.POType}) directory window save finished");
				}
				return false;
			}
			throw new ASException(string.IsNullOrWhiteSpace(errorMgs) ? ArmSoft.SME.Common.Resources.ErrorInSave : errorMgs);
		}
		catch (ASException ex)
		{
			Settings.ErrorHandler(ex);
			if (ex.FieldName != null)
			{
				_setFocusAfterInvalidSave = ControlByName(ex.FieldName);
				ControlsHelper.SetFocus(_setFocusAfterInvalidSave);
			}
			CallErrorOnSaveExceptionEvent?.Invoke(ex);
			return false;
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
			return false;
		}
	}

	protected virtual void FillDocument(DirStoreRequest sr)
	{
		base.FillDocument(sr);
	}

	protected virtual void AfterStore(DirStoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
		if (WriteInHistory)
		{
			HistoryHelpers.WriteDirectoryItemInHistory(PO);
		}
	}

	protected virtual bool BeforeStore(DirStoreRequest sr)
	{
		return true;
	}

	protected void Code_MagicClick(object sender, MagicClickEventArgs e)
	{
		SetNextCode();
		if (_code.IsEditorActive)
		{
			_code.ClearError();
		}
		if (!_code.DoValidate())
		{
			_code.EditValue = "";
		}
		_code.SelectAll();
		_codeMagicClickEvenTimes = !_codeMagicClickEvenTimes;
	}

	protected void SetNextCode()
	{
		if (_definition.NextCodeParamId.HasValue)
		{
			_codeIsManualChanged = _code.EditText.ToString().Trim() != _code.Text.Trim();
			string text = (_codeIsManualChanged ? _code.EditText.ToString().Trim() : "");
			NextCodeOfferSource offerSource = ((!_codeMagicClickEvenTimes && string.IsNullOrWhiteSpace(text)) ? NextCodeOfferSource.FromParam : NextCodeOfferSource.MaxCode);
			_code.EditValue = Settings.DirectoryService.GetNextCode(_definition.POType, offerSource, text, _code.MaxLength);
		}
	}

	protected override void BarButtonExportXML_ItemClick(object sender, ItemClickEventArgs e)
	{
		ValidateWithNoneAndAlsoFillDocumentForPrint(isForExportToXML: true);
		string fileNameFromExportDialog = Settings.CommonCoreHelper.GetFileNameFromExportDialog(".xml", "XML Files (.xml)|*.xml", "Export_" + DateTime.Now.ToString("MMdd_HHmm"));
		if (!string.IsNullOrWhiteSpace(fileNameFromExportDialog))
		{
			using (FileStream fileStream = new FileStream(fileNameFromExportDialog, FileMode.Create))
			{
				ExchangeRoutes exchange = Settings.CreateApiClient().Exchange;
				short pOType = Definition.POType;
				int num = 1;
				List<string> list = new List<string>(num);
				CollectionsMarshal.SetCount(list, num);
				CollectionsMarshal.AsSpan(list)[0] = _code.Text;
				fileStream.Write(exchange.ExportDirectory(pOType, list));
			}
		}
	}

	bool? IDirectoryDocumentWindow.ShowDialog()
	{
		return ((ThemedWindow)this).ShowDialog();
	}
}
