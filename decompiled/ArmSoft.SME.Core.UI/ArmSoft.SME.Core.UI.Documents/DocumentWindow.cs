using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Documents;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Models.UserAccess;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Ribbon;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Documents;

public abstract class DocumentWindow<T> : POWindow<T> where T : Document, new()
{
	public delegate void RefreshRequiredEntriesGridsEventHandler();

	public delegate void InnerValidateEntriesControlEventHandler(Control myControl, StoreRequest sr);

	public delegate void RefreshSystemGeneratedEntriesEventHandler(ASException ex);

	public delegate void ClearEntriesRowsEventHandler();

	public delegate void CallErrorOnSaveExceptionEventEventHandler(ASException ex);

	protected DocDefinition _definition;

	protected ButtonEditExt _docNum;

	public DateEditControl _date;

	protected BarButtonItem _barButtonCreateBasedOnDoc;

	protected BarButtonItem _barButtonDocumentLinkedList;

	protected RibbonPage _ribbonPageIB;

	protected RibbonPageGroup _ribbonPageGroupIBActions;

	protected RibbonPage _ribbonPageEInvoicing;

	protected RibbonPageGroup _ribbonPageGroupEInvoicing;

	protected BarButtonItem _barButtonExportToBank;

	protected BarButtonItem _barButtonRefreshIBState;

	protected BarButtonItem _barButtonSign;

	protected BarButtonItem _barButtonDeleteFromIB;

	protected BarButtonItem _barButtonCancel;

	protected BarButtonItem _barButtonExportToEInvoicing;

	protected BarButtonItem _barButtonEInvoicingSettings;

	protected BarButtonItem _barButtonExportedInvoiceButton;

	protected DocNumOfferSource _docNumOfferSource = DocNumOfferSource.FromParam;

	public override bool StoredInDb => PO.StoredInDb;

	public override byte State => PO.State;

	public bool IsDraftOrDeleted
	{
		get
		{
			if (State != DocumentStateSE.Draft.Value)
			{
				return State == DocumentStateSE.Deleted.Value;
			}
			return true;
		}
	}

	protected override bool WriteInHistory => true;

	public override DocDefinition Definition
	{
		get
		{
			if (_definition == null)
			{
				DefinitionMappingsService.IsDocumentWindow(((object)this).GetType(), out var docType);
				DefinitionMappingsService.TryResolveDocumentDefinition(docType, out _definition);
			}
			return _definition;
		}
	}

	protected virtual bool AllowCreateChildDoc { get; }

	protected virtual bool ShowLinkedDocsFunction { get; } = true;

	public event RefreshRequiredEntriesGridsEventHandler RefreshRequiredEntriesGrids;

	public event InnerValidateEntriesControlEventHandler InnerValidateEntriesControl;

	public event RefreshSystemGeneratedEntriesEventHandler RefreshSystemGeneratedEntries;

	public event ClearEntriesRowsEventHandler ClearEntriesRows;

	public event CallErrorOnSaveExceptionEventEventHandler CallErrorOnSaveExceptionEvent;

	public DocumentWindow()
	{
	}

	public DocumentWindow(T doc)
		: base(doc)
	{
		PO = doc;
	}

	protected override void OnNew()
	{
		base.OnNew();
		SetNextDocumentNumber();
	}

	protected override void OnNewAfterSave()
	{
		PO = Settings.DocumentService.Create<T>(Definition.POType);
		base.OnNewAfterSave();
		SetNextDocumentNumber();
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		if (WriteInHistory)
		{
			HistoryHelpers.WriteDocumentItemInHistory(PO);
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
		if (PO.POType > 0)
		{
			POPermission documentPermission = Settings.UserAccessService.GetDocumentPermission(PO.POType);
			if ((viewMode == WindowViewMode.NewCreate || viewMode == WindowViewMode.NewAfterSave) && !documentPermission.AllowAdd)
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.DontHavePermissionToCreatePO, Definition.Caption), 20008);
			}
			if (viewMode == WindowViewMode.Edit)
			{
				if (!documentPermission.AllowEdit && documentPermission.AllowView)
				{
					viewMode = WindowViewMode.View;
				}
				else
				{
					try
					{
						if (PO.DocDate != DateOnly.MinValue)
						{
							Settings.CommonCoreHelper.CheckWorkingPeriod(PO.DocDate);
						}
					}
					catch
					{
						viewMode = WindowViewMode.View;
					}
				}
			}
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{PO.POName} ({Definition.POType}) document window show with {viewMode} view mode start");
		}
		base.Show(viewMode, disableGridValidators);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{PO.POName} ({Definition.POType}) document window show with {viewMode} view mode finished");
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDocumentDefinition))
		{
			CallsRecordingHelper.ExportDocumetWindowDefinition(PO.POName, Caption, GetControlsInfoForRecord(withoutValues: true));
		}
	}

	protected override void ConstructUI()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		base.ConstructUI();
		List<SmartEnumDataProvider.DataRow> childDocTypes = Settings.DocumentService.GetChildDocTypes(_definition, checkViewAccess: false, checkAddAccess: true);
		bool flag = childDocTypes != null && childDocTypes.Count > 0;
		if (AllowCreateChildDoc & flag)
		{
			CreateRibbonPageGroupActions();
			_barButtonCreateBasedOnDoc = new BarButtonItem
			{
				Content = ArmSoft.SME.Common.Resources.CreateBasedOnDoc,
				KeyGesture = HotKeys.WithModifier.Control_Shift_Y,
				Glyph = ImageService.GetImage("doc_based_on_16.svg"),
				LargeGlyph = ImageService.GetImage("doc_based_on_32.svg")
			};
			_barManager.Items.Add(_barButtonCreateBasedOnDoc);
			_ribbonPageGroupActions.ItemLinks.Add(_barButtonCreateBasedOnDoc.CreateLink());
			_barButtonCreateBasedOnDoc.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonCreateBasedOnDoc_ItemClick, this).Handler;
		}
		if (!ShowLinkedDocsFunction)
		{
			return;
		}
		List<SmartEnumDataProvider.DataRow> childDocTypes2 = Settings.DocumentService.GetChildDocTypes(_definition, checkViewAccess: true);
		if (childDocTypes2 == null || childDocTypes2.Count <= 0)
		{
			List<SmartEnumDataProvider.DataRow> baseDocTypes = Settings.DocumentService.GetBaseDocTypes(_definition, checkViewAccess: true);
			if (baseDocTypes == null || baseDocTypes.Count <= 0)
			{
				return;
			}
		}
		CreateRibbonPageGroupReports();
		_barButtonDocumentLinkedList = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.LinkedDoc,
			Glyph = ImageService.GetImage("report_link_16.svg"),
			LargeGlyph = ImageService.GetImage("report_link_32.svg")
		};
		_barManager.Items.Add(_barButtonDocumentLinkedList);
		_ribbonPageGroupReports.ItemLinks.Add(_barButtonDocumentLinkedList.CreateLink());
		_barButtonDocumentLinkedList.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonDocumentLinkedList_ItemClick, this).Handler;
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		if (_barButtonDocumentLinkedList != null)
		{
			_barButtonDocumentLinkedList.IsEnabled = isEnabled;
		}
		if (_barButtonExportToBank != null)
		{
			_barButtonExportToBank.IsEnabled = _viewMode != WindowViewMode.View;
		}
		if (_barButtonRefreshIBState != null)
		{
			_barButtonRefreshIBState.IsEnabled = _viewMode != WindowViewMode.View;
		}
		if (_barButtonSign != null)
		{
			_barButtonSign.IsEnabled = _viewMode != WindowViewMode.View;
		}
		if (_barButtonDeleteFromIB != null)
		{
			_barButtonDeleteFromIB.IsEnabled = _viewMode != WindowViewMode.View;
		}
		if (_barButtonCancel != null)
		{
			_barButtonCancel.IsEnabled = _viewMode != WindowViewMode.View;
		}
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		if (_barButtonExportToEInvoicing != null)
		{
			Ribbon.ToolbarItemLinks.Add(_barButtonExportToEInvoicing.ConvertForToolbar("t_doc_export_16.svg"));
			Ribbon.ToolbarItemLinks.Add(_barButtonEInvoicingSettings.ConvertForToolbar("t_page_white_gear_16.svg"));
			Ribbon.ToolbarItemLinks.Add(_barButtonExportedInvoiceButton.ConvertForToolbar("t_TaxService_16.svg"));
		}
		if (_barButtonCreateBasedOnDoc != null)
		{
			Ribbon.ToolbarItemLinks.Add(_barButtonCreateBasedOnDoc.ConvertForToolbar("t_doc_based_on_16.svg"));
		}
		if (_barButtonDocumentLinkedList != null)
		{
			Ribbon.ToolbarItemLinks.Add(_barButtonDocumentLinkedList.ConvertForToolbar("t_report_link_16.svg"));
		}
		if (_barButtonExportToBank != null)
		{
			Ribbon.ToolbarItemLinks.Add(_barButtonExportToBank.ConvertForToolbar("t_doc_export_16.svg"));
		}
		if (_barButtonRefreshIBState != null)
		{
			Ribbon.ToolbarItemLinks.Add(_barButtonRefreshIBState.ConvertForToolbar("t_doc_update_16.svg"));
		}
		if (_barButtonSign != null)
		{
			Ribbon.ToolbarItemLinks.Add(_barButtonSign.ConvertForToolbar("t_doc_accept_16.svg"));
		}
		if (_barButtonDeleteFromIB != null)
		{
			Ribbon.ToolbarItemLinks.Add(_barButtonDeleteFromIB.ConvertForToolbar("t_doc_delete_16.svg"));
		}
		if (_barButtonCancel != null)
		{
			Ribbon.ToolbarItemLinks.Add(_barButtonCancel.ConvertForToolbar("t_doc_cancel_16.svg"));
		}
	}

	protected void CreateRibbonPageGroupIBActions()
	{
		if (_ribbonPageIB == null)
		{
			AddIBRibbonPage();
		}
		if (_ribbonPageGroupIBActions == null)
		{
			_ribbonPageGroupIBActions = new RibbonPageGroup
			{
				AllowCollapse = false,
				Caption = ArmSoft.SME.Common.Resources.Operations
			};
			_ribbonPageIB.Groups.Add(_ribbonPageGroupIBActions);
		}
	}

	protected void AddIBRibbonPage()
	{
		if (_ribbonPageIB == null)
		{
			_ribbonPageIB = new RibbonPage
			{
				CaptionTemplate = (DataTemplate)Settings.GenericResources["RibbonPageHeaderTemplate"]
			};
		}
		Ribbon.Categories[0].Pages.Add(_ribbonPageIB);
		_ribbonPageIB.Caption = ArmSoft.SME.Common.Resources.InternetBank;
	}

	protected void AddIBBarButtons()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		_barButtonExportToBank = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.Send,
			Glyph = ImageService.GetImage("doc_export_16.svg"),
			LargeGlyph = ImageService.GetImage("doc_export_32.svg")
		};
		_ribbonPageGroupIBActions.ItemLinks.Add(_barButtonExportToBank.CreateLink());
		_barButtonRefreshIBState = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.Refresh,
			Glyph = ImageService.GetImage("doc_update_16.svg"),
			LargeGlyph = ImageService.GetImage("doc_update_32.svg")
		};
		_ribbonPageGroupIBActions.ItemLinks.Add(_barButtonRefreshIBState.CreateLink());
		_barButtonSign = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.SignPayment,
			Glyph = ImageService.GetImage("doc_accept_16.svg"),
			LargeGlyph = ImageService.GetImage("doc_accept_32.svg")
		};
		_ribbonPageGroupIBActions.ItemLinks.Add(_barButtonSign.CreateLink());
		_barButtonDeleteFromIB = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.Delete,
			Glyph = ImageService.GetImage("doc_delete_16.svg"),
			LargeGlyph = ImageService.GetImage("doc_delete_32.svg")
		};
		_ribbonPageGroupIBActions.ItemLinks.Add(_barButtonDeleteFromIB.CreateLink());
		_barButtonCancel = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.CancelPayment,
			Glyph = ImageService.GetImage("doc_cancel_16.svg"),
			LargeGlyph = ImageService.GetImage("doc_cancel_32.svg")
		};
		_ribbonPageGroupIBActions.ItemLinks.Add(_barButtonCancel.CreateLink());
		_barButtonExportToBank.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonExportToBank_ItemClick).Handler;
		_barButtonRefreshIBState.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonRefreshIBState_ItemClick).Handler;
		_barButtonSign.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonSign_ItemClick).Handler;
		_barButtonDeleteFromIB.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonDeleteFromIB_ItemClick).Handler;
		_barButtonCancel.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonCancel_ItemClick).Handler;
	}

	protected void CreateRibbonPageGroupEInvoicing()
	{
		if (_ribbonPageEInvoicing == null)
		{
			AddEInvoicingRibbonPage();
		}
		if (_ribbonPageGroupEInvoicing == null)
		{
			_ribbonPageGroupEInvoicing = new RibbonPageGroup
			{
				AllowCollapse = false,
				Caption = ArmSoft.SME.Common.Resources.Operations
			};
			_ribbonPageEInvoicing.Groups.Add(_ribbonPageGroupEInvoicing);
		}
	}

	protected void AddEInvoicingRibbonPage()
	{
		if (_ribbonPageEInvoicing == null)
		{
			_ribbonPageEInvoicing = new RibbonPage
			{
				CaptionTemplate = (DataTemplate)Settings.GenericResources["RibbonPageHeaderTemplate"]
			};
		}
		Ribbon.Categories[0].Pages.Add(_ribbonPageEInvoicing);
		_ribbonPageEInvoicing.Caption = ArmSoft.SME.Common.Resources.EInvoicing;
	}

	protected void AddEInvoicingBarButtons()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		_barButtonExportToEInvoicing = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.Export,
			Glyph = ImageService.GetImage("doc_export_16.svg"),
			LargeGlyph = ImageService.GetImage("doc_export_32.svg")
		};
		_ribbonPageGroupEInvoicing.ItemLinks.Add(_barButtonExportToEInvoicing.CreateLink());
		_barButtonExportToEInvoicing.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonExportToEInvoicing_ItemClick).Handler;
		_barButtonEInvoicingSettings = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.Settings,
			Glyph = ImageService.GetImage("page_white_gear_16.svg"),
			LargeGlyph = ImageService.GetImage("page_white_gear_32.svg")
		};
		_ribbonPageGroupEInvoicing.ItemLinks.Add(_barButtonEInvoicingSettings.CreateLink());
		_barButtonEInvoicingSettings.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonEInvoicingSettings_ItemClick).Handler;
		_barButtonExportedInvoiceButton = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.SubmitedInvoice,
			Glyph = ImageService.GetImage("TaxService_line_16.svg"),
			LargeGlyph = ImageService.GetImage("TaxService_32.svg")
		};
		_ribbonPageGroupEInvoicing.ItemLinks.Add(_barButtonExportedInvoiceButton.CreateLink());
		_barButtonExportedInvoiceButton.ItemClick += new SafeEventHandler<RoutedEventArgs>(BarButtonExportedInvoiceButton_ItemClick).Handler;
	}

	protected override bool AllowDraft()
	{
		return Definition.AllowStoreAsDraft;
	}

	protected override void BarButtonShowISN_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			MessageBox.Show(PO.ISN.ToString(), ArmSoft.SME.Common.Resources.DocumentISN);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void GetPO()
	{
		PO = Settings.DocumentService.Create<T>(Definition.POType);
	}

	protected override void RefreshRequiredGrids()
	{
		base.RefreshRequiredGrids();
		RefreshRequiredEntriesGrids?.Invoke();
	}

	protected override void ReLoad()
	{
		PO = Settings.DocumentService.Load<T>(PO.ISN, Definition.POType, withoutAccessCheck: true);
		IsChanged = false;
	}

	protected override bool InnerSave(bool isAsDraft = false)
	{
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile($"{PO.POName} ({Definition.POType}) document window save start");
		}
		StoreRequest storeRequest = new StoreRequest
		{
			State = (isAsDraft ? DocumentStateSE.Draft : DocumentStateSE.Processed)
		};
		bool flag = false;
		try
		{
			if (InnerValidate(out var errorMgs, storeRequest.State == (byte)DocumentStateSE.Draft))
			{
				bool isNewDocument = !StoredInDb;
				storeRequest.DocNumOfferSource = _docNumOfferSource;
				FillDocument(storeRequest);
				if (BeforeStore(storeRequest))
				{
					if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDocument))
					{
						CallsRecordingHelper.ExportDocumentWindow(PO.POName, this);
					}
					T val = Settings.DocumentService.StoreAndLoad(PO, storeRequest, this);
					if (val == null)
					{
						flag = false;
					}
					else
					{
						PO = val;
						flag = true;
					}
					if (storeRequest.StoreMode == StoreMode.All)
					{
						AfterStore(storeRequest, isNewDocument);
					}
					UpdateDataViewPanel(DVPID);
					if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
					{
						CallsRecordingHelper.RecordCallStepInFile($"{PO.POName} ({Definition.POType}) document window save finished");
					}
					return flag;
				}
				if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
				{
					CallsRecordingHelper.RecordCallStepInFile($"{PO.POName} ({Definition.POType}) document window save finished");
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
				if (storeRequest.RegenerateEntries)
				{
					RefreshSystemGeneratedEntries?.Invoke(ex);
				}
			}
			else if (ex.Message == ArmSoft.SME.Common.Resources.ErrorInSave)
			{
				ClearEntriesRows?.Invoke();
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

	protected override bool CustomControlValidate(Control control, bool isAsDraft)
	{
		base.CustomControlValidate(control, isAsDraft);
		StoreRequest sr = new StoreRequest
		{
			State = (isAsDraft ? DocumentStateSE.Draft : DocumentStateSE.Processed)
		};
		InnerValidateEntriesControl?.Invoke(control, sr);
		return true;
	}

	protected virtual void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
	}

	protected virtual void AfterStore(StoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
		if (WriteInHistory)
		{
			HistoryHelpers.WriteDocumentItemInHistory(PO);
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		dvp.Update(PO.ISN);
	}

	protected virtual bool BeforeStore(StoreRequest sr)
	{
		return true;
	}

	protected override void OnPrintPreview(TemplatesCacheRow template, PrintMode mode)
	{
		if (template != null)
		{
			FillDocument(new StoreRequest());
			base.OnPrintPreview(template, mode);
			Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(PO, mode, template.Name);
		}
	}

	protected virtual void BarButtonCreateBasedOnDoc_ItemClick(object sender, ItemClickEventArgs e)
	{
		if ((!PO.StoredInDb || IsChanged) && (MessageBox.Show(ArmSoft.SME.Common.Resources.ForActionMustSaveDoc, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes || !SaveDocument()))
		{
			return;
		}
		ContextMenu contextMenu = new ContextMenu
		{
			FontFamily = Settings.FontFamily,
			FontSize = Settings.GetFontSize()
		};
		List<SmartEnumDataProvider.DataRow> childDocTypes = Settings.DocumentService.GetChildDocTypes(_definition, checkViewAccess: false, checkAddAccess: true);
		if (childDocTypes.Count == 1)
		{
			CreateChildDocument(short.Parse(childDocTypes.First().Code), PO);
			return;
		}
		foreach (SmartEnumDataProvider.DataRow item in childDocTypes)
		{
			MenuItem menuItem = new MenuItem
			{
				Header = item.Caption,
				Tag = item.Code
			};
			menuItem.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CreateChildDocument, this).Handler;
			contextMenu.Items.Add(menuItem);
		}
		contextMenu.ToolTip = ArmSoft.SME.Common.Resources.NewDocuments;
		contextMenu.PlacementTarget = this;
		contextMenu.Placement = PlacementMode.Center;
		contextMenu.VerticalOffset = 0.0;
		contextMenu.HorizontalOffset = 0.0;
		contextMenu.IsOpen = true;
	}

	protected void CreateChildDocument(object sender, RoutedEventArgs e)
	{
		MenuItem menuItem = (MenuItem)sender;
		CreateChildDocument(short.Parse(menuItem.Tag.ToString()), PO);
	}

	protected virtual void CreateChildDocument(short childDocType, T baseDoc)
	{
	}

	protected virtual void BarButtonDocumentLinkedList_ItemClick(object sender, ItemClickEventArgs e)
	{
	}

	protected void DocNum_MagicClick(object sender, MagicClickEventArgs e)
	{
		if (_docNum.IsEditorActive)
		{
			_docNum.ClearError();
		}
		SetNextDocumentNumber();
		_docNum.DoValidate();
		_docNum.SelectAll();
	}

	protected void DocNum_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_docNumOfferSource = DocNumOfferSource.None;
	}

	protected virtual void SetNextDocumentNumber()
	{
		if (_definition.NextDocNumParamId.HasValue)
		{
			KeyValuePair<string, DocNumOfferSource> nextDocNum = Settings.DocumentService.GetNextDocNum(_definition.POType, NextDocNumberRelatedInfo());
			_docNum.EditValue = nextDocNum.Key;
			_docNumOfferSource = nextDocNum.Value;
		}
	}

	protected virtual string NextDocNumberRelatedInfo()
	{
		return "";
	}

	protected override void BarButtonExportXML_ItemClick(object sender, ItemClickEventArgs e)
	{
		ValidateWithNoneAndAlsoFillDocumentForPrint(isForExportToXML: true);
		string fileNameFromExportDialog = Settings.CommonCoreHelper.GetFileNameFromExportDialog(".xml", "XML Files (.xml)|*.xml", "Export_" + DateTime.Now.ToString("MMdd_HHmm"));
		if (!string.IsNullOrWhiteSpace(fileNameFromExportDialog))
		{
			using (FileStream fileStream = new FileStream(fileNameFromExportDialog, FileMode.Create))
			{
				fileStream.Write(Settings.CreateApiClient().Exchange.ExportDocument(Definition.POType, POISN()));
			}
		}
	}

	protected virtual void BarButtonExportToBank_ItemClick(object sender, ItemClickEventArgs e)
	{
	}

	protected virtual void BarButtonRefreshIBState_ItemClick(object sender, ItemClickEventArgs e)
	{
	}

	protected virtual void BarButtonSign_ItemClick(object sender, ItemClickEventArgs e)
	{
	}

	protected virtual void BarButtonDeleteFromIB_ItemClick(object sender, ItemClickEventArgs e)
	{
	}

	protected virtual void BarButtonCancel_ItemClick(object sender, ItemClickEventArgs e)
	{
	}

	protected virtual void BarButtonExportToEInvoicing_ItemClick(object sender, ItemClickEventArgs e)
	{
	}

	protected virtual void BarButtonEInvoicingSettings_ItemClick(object sender, ItemClickEventArgs e)
	{
	}

	protected virtual void BarButtonExportedInvoiceButton_ItemClick(object sender, RoutedEventArgs e)
	{
	}

	protected virtual bool CheckDocsSavedModeBeforeOperation()
	{
		if (IsChanged || !PO.StoredInDb || IsDraftOrDeleted)
		{
			if (MessageBox.Show(ArmSoft.SME.Common.Resources.SaveAndContinue, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
			{
				return false;
			}
			if (!SaveDocument())
			{
				return false;
			}
		}
		return true;
	}
}
