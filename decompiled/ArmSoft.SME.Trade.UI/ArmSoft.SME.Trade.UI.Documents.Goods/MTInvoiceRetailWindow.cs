using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Trade.Client.Data.DropDowns;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.OtherWindows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents.Goods;

[DocumentWindow(21)]
public class MTInvoiceRetailWindow : MTInvoiceBaseWindow<MTInvoiceRetail>
{
	public class MTInvoiceRetailSpecificationRow : MTInvoiceBaseSpecificationRow
	{
		[ObservableProperty]
		private decimal envFeeSumma;

		[ObservableProperty]
		private decimal envFeePercent;

		public string PartyMethodName
		{
			get
			{
				if (!IsPartySelected.HasValue)
				{
					return string.Empty;
				}
				if (IsPartySelected.Value)
				{
					return ArmSoft.SME.Common.Resources.PartySelected;
				}
				return ArmSoft.SME.Common.Resources.AutoSelection;
			}
		}

		public string DealTypeCaption
		{
			get
			{
				if (string.IsNullOrWhiteSpace(DealType) || DocWindow == null)
				{
					return string.Empty;
				}
				if (Convert.ToString(DocWindow.GetControlValue("VATType")).InList("3", "4"))
				{
					return SmartEnum<AccDealTypesSE, short>.FromValue(Convert.ToInt16(DealType)).Caption;
				}
				return SmartEnum<InvoiceDealTypesSE, short>.FromValue(Convert.ToInt16(DealType)).Caption;
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal EnvFeeSumma
		{
			get
			{
				return envFeeSumma;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(envFeeSumma, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.EnvFeeSumma);
					envFeeSumma = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.EnvFeeSumma);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal EnvFeePercent
		{
			get
			{
				return envFeePercent;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(envFeePercent, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.EnvFeePercent);
					envFeePercent = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.EnvFeePercent);
				}
			}
		}

		public MTInvoiceRetailSpecificationRow()
		{
		}

		public MTInvoiceRetailSpecificationRow(MTInvoiceRetailWindow docWindow)
			: base((TradeDocumentWindow<MTInvoiceRetail>)docWindow)
		{
		}

		static MTInvoiceRetailSpecificationRow()
		{
			Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>> invoiceBasePropertyValidators = MTInvoiceBaseSpecificationRow.GetInvoiceBasePropertyValidators();
			invoiceBasePropertyValidators.Add("Storage", (DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
			{
				ValidateStorage((MTInvoiceRetailSpecificationRow)row, info);
			});
			DocumentGridRow.RegisterPropertyValidators(typeof(MTInvoiceRetailSpecificationRow), cacheValidationResults: true, invoiceBasePropertyValidators);
		}

		private static void ValidateStorage(MTInvoiceBaseSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (row.ItemType == GoodsTypesSE.Good.Value)
			{
				TradePropertyValidator.ValidateStorage(row.Storage, info, ArmSoft.SME.Common.Resources.Storage, isRequired: true);
			}
		}
	}

	private DropDownCommentControl _baseDocumentControl;

	public DropDownControl _storage;

	private TextEditExt _barCode;

	private Button _codeEnter;

	private PartnerContractGroupControls _partnerContractContainer;

	private DropDownControl _exportedType;

	private TextEditExt _taxInvoiceSeries;

	private ButtonEditExt _taxInvoiceNumber;

	private DateEditControl _taxInvoiceDate;

	private DropDownControl _salesConsultant;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private TextEditExt _supplierName;

	private TextEditExt _supplierLegalAddress;

	private DropDownCommentControl _supplierBusinessAddress;

	private DropDownControl _supplierSettlAcc;

	private TextEditExt _supplierTaxCode;

	private ComboBoxEditExt _supplierMngPost;

	private TextEditExt _supplierMngName;

	private ComboBoxEditExt _supplierChiefAccPost;

	private TextEditExt _supplierChiefAccName;

	private ComboBoxEditExt _supplierAdditionalData;

	private TextEditExt _buyerName;

	private TextEditExt _buyerLegalAddress;

	private ComboBoxEditExt _buyerBusinessAddress;

	private DropDownControl _buyerSettlAcc;

	private TextEditExt _buyerTaxCode;

	private ComboBoxEditExt _buyerMngPost;

	private TextEditExt _buyerMngName;

	private ComboBoxEditExt _buyerChiefAccPost;

	private TextEditExt _buyerChiefAccName;

	private TextEditExt _buyerStateRegNum;

	private DropDownControl _buyerIDDocType;

	private TextEditExt _buyerPassport;

	private ComboBoxEditExt _buyerAdditionalData;

	private ComboBoxEditExt _buyerShippingMethod;

	private ComboBoxEditExt _byWhomFio;

	private TextEditExt _authorizationNumber;

	private DateEditControl _authorizationDate;

	private DropDownControl _currCode;

	public DropDownControl _vatType;

	public DropDownControl _dealType;

	private string _dealTypeKey;

	public PopupCalcEditExt _discountPercent;

	private PopupCalcEditExt _vatSumma;

	private PopupCalcEditExt _totalSumma;

	private BarEditItemExt _viewAvailAndTurn;

	private BarEditItemExt _complect;

	private BarEditItemExt _discount;

	private BarEditItemExt _filteredSearch;

	private BarEditItemExt _actualQuantitiesCheck;

	public DocumentGridControl _invoiceRetailGrid;

	private TableView _invoiceRetailTableView;

	public ObservableCollection<MTInvoiceRetailSpecificationRow> _invoicesRetailList;

	private DocumentGridColumn _itemCodeCol;

	private DocumentGridColumn _itemNameCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _unitCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _priceCol;

	private DocumentGridColumn _discountCol;

	private DocumentGridColumn _discountPriceCol;

	private DocumentGridColumn _summaCol;

	private DocumentGridColumn _envFeePercentCol;

	private DocumentGridColumn _envFeeSummaCol;

	private DocumentGridColumn _vatCol;

	private DocumentGridColumn _dealTypeCol;

	private DocumentGridColumn _partyCol;

	private DocumentGridColumn _rowDescriptionCol;

	private DXTabControl _tabs;

	private int _priceListTabControlIndex;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private BarButtonItem _barButtonImportFromFile;

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonMTSalesAnalysis;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private ReportBarButtonItem _barButtonGoodsRetailOperationsJournal;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	private SafeDocumentEventHandler<EditValueChangedEventArgs> _discountPercentEventHandler;

	private bool _validateTotalSumm;

	public int _maxRowID;

	private Guid _baseISN = Guid.Empty;

	protected TradeDocument _baseDocument;

	private bool _useDiscount = Settings.ParametersService.DiscountUse();

	private static bool _calcDiscountAuto = Settings.ParametersService.CalcDiscountAuto();

	private bool _allowChangeSalePrices = Settings.ParametersService.AllowChangeSalePrices();

	private bool _allowChangeDiscountPrices = Settings.ParametersService.AllowChangeDiscountPrice();

	private string _priceListType = Settings.ParametersService.DefaultSalePrice();

	public decimal UsedDiscount;

	private string _rowAddMode = Settings.ParametersService.ScannerMode();

	public bool _useEnvFees { get; set; }

	private TradeDocument BaseDocument
	{
		get
		{
			return _baseDocument;
		}
		set
		{
			_baseDocument = value;
			if (_baseDocument == null)
			{
				_baseDocumentControl.Text = "";
			}
			else if (_baseDocument.POType != (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedTaxInvoice)
			{
				_baseDocumentControl.Text = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(_baseDocument, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum);
			}
			else
			{
				_baseDocumentControl.Text = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(_baseDocument, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum);
			}
		}
	}

	private Guid BaseISN
	{
		get
		{
			return _baseISN;
		}
		set
		{
			if (((IComparable)value).CompareTo((object?)_baseISN) == 0)
			{
				return;
			}
			_baseISN = value;
			if (_baseISN == Guid.Empty)
			{
				BaseDocument = null;
				return;
			}
			BaseDocument = (TradeDocument)Settings.DocumentService.Load(_baseISN, withoutAccessCheck: true);
			if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillUsingBaseQ, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
			{
				CreateBasedOnDoc();
			}
		}
	}

	public MTInvoiceRetailWindow()
	{
	}

	public MTInvoiceRetailWindow(MTInvoiceRetail doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		DocumentsJournal idv = new DocumentsJournal(this, ReportTypesSE.AllDocument, ((TradeDocDefinition)Definition).BaseDocTypes)
		{
			DocType = ((TradeDocDefinition)Definition).BaseDocTypes.First().ToString().PadLeft(2, '0'),
			DocState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.DraftOrProcessed.ValueUI
		};
		_baseDocumentControl = AddDropDownCommentControl(null, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: false, idv, 50, HorizontalAlignment.Stretch, "", "BaseDocumentControl");
		_baseDocumentControl.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(BaseDocumentControl_ClickDropDown, this).Handler;
		_baseDocumentControl.EditKeyValueChanged += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentControl_EditKeyValueChanged, this).Handler;
		_baseDocumentControl.BtnClearClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentDelete_Click, this).Handler;
		_baseDocumentControl.BtnSearchClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentSearch_Click, this).Handler;
		_baseDocumentControl.ShowClearButton = true;
		_baseDocumentControl.ShowSearchButton = true;
		_baseDocumentControl.IsTextEditable = false;
		_storage = AddDropDownControl(null, Definition.Fields["Storage"], new Storages(true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_storage.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged, this).Handler;
		LayoutGroupExt container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barCode = AddStringControl(container2, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch);
		_barCode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_codeEnter = AddButton(container2, null, text, null, text, isTabStop: true);
		_codeEnter.Margin = new Thickness(0.0, 0.0, 18.0, 0.0);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(BE_CodeEnter_Click, this).Handler;
		_tabs = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.General, "General");
		_partnerContractContainer = AddPartnerContractGroupControls(container3, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, null, null);
		_partnerContractContainer.PartnerCodeEditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PartnerCodeEditValueChanged, this).Handler;
		_partnerContractContainer.PartnerCodeEditAdditionalValueChanged += new SafeDocumentEventHandler<EventArgs>(PartnerCodeEditAdditionalValueChanged, this).Handler;
		_partnerContractContainer.ContractCodeEditValueChanged += new SafeDocumentEventHandler<EventArgs>(ContractCodeEditValueChanged, this).Handler;
		_exportedType = AddSmartEnumDropDownControl(container3, Definition.Fields["ExportedType"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_exportedType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(ExportedType_EditValueChanged, this).Handler;
		LayoutGroupExt container4 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_taxInvoiceSeries = AddStringControl(container4, Definition.Fields["TaxInvoiceSeries"]);
		_taxInvoiceNumber = AddButtonEditControl(container4, Definition.Fields["TaxInvoiceNumber"], 1);
		_taxInvoiceNumber.AllowMagicButton = true;
		_taxInvoiceNumber.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_taxInvoiceDate = AddDateControl(container3, Definition.Fields["InvoiceDate"]);
		_salesConsultant = AddDropDownControl(container3, Definition.Fields["SalesConsultant"], new Employees(), allowAdditionButtons: true);
		List<string> captionsList = TradeSettings.TradeCacheService.StandardComments.GetCaptionsList();
		_comment = AddComboBoxControl(container3, Definition.Fields["Comment"], captionsList, HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container3, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		LayoutGroup container5 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.ItemList, "MtList");
		_priceListTabControlIndex = _tabs.Items.Count - 1;
		LayoutGroupExt container6 = AddLayoutGroup(container5, LayoutGroupView.Group, Orientation.Horizontal);
		LayoutGroupExt container7 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Vertical);
		LayoutGroupExt container8 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Vertical);
		_currCode = AddDropDownControl(container7, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_discountPercent = AddNumericControl(container8, Definition.Fields["Discount"], useCurrencyFormat: false, HorizontalAlignment.Right);
		_discountPercentEventHandler = new SafeDocumentEventHandler<EditValueChangedEventArgs>(DiscountPercent_EditValueChanged, this);
		_discountPercent.EditValueChanged += _discountPercentEventHandler.Handler;
		_vatType = AddDropDownControl(container7, Definition.Fields["VATType"], new VATTypes(includeVATRetail: false, onlyVATRetailTypes: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_vatType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(VATType_EditValueChanged, this).Handler;
		_dealType = AddDropDownControl(container7, Definition.Fields["DealType"], new SmartEnum(typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI));
		_dealType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DealType_EditValueChanged, this).Handler;
		_dealType.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(DealType_ClickDropDown, this).Handler;
		_invoiceRetailGrid = AddGrid(container5, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_invoiceRetailGrid.IsRequired = true;
		_invoiceRetailGrid.AllowMoveUp = true;
		_invoiceRetailGrid.AllowMoveDown = true;
		_invoiceRetailGrid.AllowDuplicate = true;
		_invoiceRetailGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTinvoiceRetailGrid_AddingRow, this).Handler;
		_invoiceRetailGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTInvoiceRetailGrid_AddingRowWithDialog, this).Handler;
		_invoiceRetailGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTInvoiceRetailGrid_DuplicatingRowWithDialog, this).Handler;
		_invoiceRetailGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(MTInvoiceRetailGrid_EditingRowWithDialog, this).Handler;
		_invoiceRetailGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTInvoiceRetailGrid_ClickDropDown, this).Handler;
		_invoiceRetailGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTinvoiceRetailGrid_DeletedRow, this).Handler;
		_invoiceRetailGrid.DeletingRow += new SafeDocumentEventHandler<DocumentGridDeletingRowEventArgs>(MTinvoiceRetailGrid_DeletingRow, this).Handler;
		_invoiceRetailGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTinvoiceRetailGrid_DuplicatingRow, this).Handler;
		_invoiceRetailGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTinvoiceRetailGrid_MovingRowUp, this).Handler;
		_invoiceRetailGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTinvoiceRetailGrid_MovingRowDown, this).Handler;
		_invoiceRetailGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTInvoiceRetailGrid_RefreshButtons, this).Handler;
		_invoiceRetailTableView = _invoiceRetailGrid.View;
		_invoiceRetailTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTinvoiceRetailTableView_FocusedRowHandleChangedEventArgs, this).Handler;
		_invoiceRetailTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTInvoiceRetailTableView_CellValueChanged, this).Handler;
		_invoiceRetailTableView.ShowingEditor += new SafeDocumentEventHandler<ShowingEditorEventArgs>(MTInvoiceRetailTableView_ShowingEditor, this).Handler;
		_invoiceRetailGrid.ToolTip = ArmSoft.SME.Common.Resources.ItemList;
		_viewAvailAndTurn = _invoiceRetailGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.AvailAndTurn, ArmSoft.SME.Trade.Common.Resources.AvailabilityAndTurn, 130.0);
		_viewAvailAndTurn.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ViewAvailAndTurn_Click, this).Handler;
		_complect = _invoiceRetailGrid.AddButton("fill_down_16.svg", ArmSoft.SME.Common.Resources.Complect, ArmSoft.SME.Common.Resources.Complect, 90.0);
		_complect.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Complect_Click, this).Handler;
		if (_useDiscount)
		{
			_discount = _invoiceRetailGrid.AddButton("fill_down_16.svg", ArmSoft.SME.Trade.Common.Resources.GiveDiscount, ArmSoft.SME.Trade.Common.Resources.GiveDiscount, 90.0);
			_discount.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Discount_Click, this).Handler;
		}
		_filteredSearch = _invoiceRetailGrid.AddButton(ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, string.Empty, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_actualQuantitiesCheck = _invoiceRetailGrid.AddButton("cheked_all_16.svg", ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck, ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck, 255.0);
		_actualQuantitiesCheck.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ActualQuantitiesCheck_Click, this).Handler;
		_invoiceRetailGrid.AddRowIndicator(35);
		_itemCodeCol = _invoiceRetailGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["ItemCode"], new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, "0", "", isShowOnlyInPriceList: true), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_itemCodeCol.Width = 125.0;
		_itemNameCol = _invoiceRetailGrid.AddStringColumn(Definition.Grids["MTList"].Columns["ItemName"]);
		_itemNameCol.Width = 225.0;
		_unitCodeCol = _invoiceRetailGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"], trimValue: false);
		_unitCol = _invoiceRetailGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]);
		_unitCol.AllowEditing = DefaultBoolean.False;
		_quantityCol = _invoiceRetailGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_priceCol = _invoiceRetailGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Price"], showTotal: false, useCurrencyFormat: true, isOwnProcessingClickDropDown: true);
		_priceCol.AllowEditing = DefaultBoolean.False;
		_discountCol = _invoiceRetailGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Discount"]);
		_discountPriceCol = _invoiceRetailGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["DiscountPrice"]);
		_summaCol = _invoiceRetailGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Summa"], showTotal: true);
		_envFeePercentCol = _invoiceRetailGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["EnvFeePercent"]);
		_envFeeSummaCol = _invoiceRetailGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["EnvFeeSumma"], showTotal: true);
		_vatCol = _invoiceRetailGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["VAT"]);
		_vatCol.Width = 45.0;
		_dealTypeCol = _invoiceRetailGrid.AddDropDownColumn("DealTypeCaption", ArmSoft.SME.Common.Resources.DealType, 1, trimValue: false);
		((ButtonEditSettings)_dealTypeCol.EditSettings).IsTextEditable = false;
		_partyCol = _invoiceRetailGrid.AddDropDownColumn("PartyMethodName", ArmSoft.SME.Common.Resources.Party, 19, trimValue: false, GlyphKind.Regular);
		((ButtonEditSettings)_partyCol.EditSettings).IsTextEditable = false;
		_rowDescriptionCol = _invoiceRetailGrid.AddStringColumn(Definition.Grids["MTList"].Columns["RowDescription"]);
		_rowDescriptionCol.AllowEditing = DefaultBoolean.True;
		foreach (GridColumn column in _invoiceRetailGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		Action<DocumentGridColumn> action = (DocumentGridColumn control) =>
		{
			control.AllowEditing = DefaultBoolean.False;
			control.AllowFocus = false;
		};
		if (!_useDiscount)
		{
			if (!_allowChangeSalePrices)
			{
				action(_priceCol);
				action(_summaCol);
			}
		}
		else if (!_allowChangeSalePrices)
		{
			action(_priceCol);
			if (!_allowChangeDiscountPrices)
			{
				action(_discountCol);
				action(_discountPriceCol);
				action(_summaCol);
			}
		}
		else if (!_allowChangeDiscountPrices)
		{
			action(_discountCol);
			action(_discountPriceCol);
			action(_summaCol);
		}
		_invoicesRetailList = new ObservableCollection<MTInvoiceRetailSpecificationRow>();
		_vatSumma = AddNumericControl(container5, Definition.Fields["SummVAT"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_totalSumma = AddNumericControl(container5, Definition.Fields["Summ"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_totalSumma.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(TotalSumma_EditValueChanged, this).Handler;
		_totalSumma.IsRequired = !Settings.ParametersService.AllowUse100PercentSale();
		_partnerContractContainer.DocSummaControl = _totalSumma;
		_partnerContractContainer.DocCurrCodeControl = _currCode;
		LayoutGroup container9 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.Supplier, "Supplier");
		_supplierName = AddStringControl(container9, Definition.Fields["SupplierName"], HorizontalAlignment.Stretch);
		_supplierLegalAddress = AddStringControl(container9, Definition.Fields["SupplierLegalAddress"], HorizontalAlignment.Stretch);
		_supplierBusinessAddress = AddDropDownCommentControl(container9, Definition.Fields["SupplierBusinessAddress"], new Storages(), HorizontalAlignment.Stretch);
		_supplierBusinessAddress.EditKeyValueChanged += new SafeDocumentEventHandler<object>(SupplierBusinessAddress_EditKeyValueChanged, this).Handler;
		_supplierBusinessAddress.MaxLength = 100;
		_supplierSettlAcc = AddDropDownControl(container9, Definition.Fields["SupplierSettlAcc"], new AccountInBanks(isKeySettlAccount: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_supplierSettlAcc.CheckValue = false;
		_supplierTaxCode = AddStringControl(container9, Definition.Fields["SupplierTaxCode"], HorizontalAlignment.Stretch);
		IEnumerable<string> itemsSource = SmartEnum<ManagerPositionsSE, string>.List.Select((ManagerPositionsSE row) => row.Caption);
		_supplierMngPost = AddComboBoxControl(container9, Definition.Fields["SupplierMngPost"], itemsSource, HorizontalAlignment.Stretch);
		_supplierMngName = AddStringControl(container9, Definition.Fields["SupplierMngName"], HorizontalAlignment.Stretch);
		IEnumerable<string> itemsSource2 = SmartEnum<ChiefAccPositionsSE, string>.List.Select((ChiefAccPositionsSE row) => row.Caption);
		_supplierChiefAccPost = AddComboBoxControl(container9, Definition.Fields["SupplierChiefAccPost"], itemsSource2, HorizontalAlignment.Stretch);
		_supplierChiefAccName = AddStringControl(container9, Definition.Fields["SupplierChiefAccName"], HorizontalAlignment.Stretch);
		_supplierAdditionalData = AddComboBoxControl(container9, Definition.Fields["SupplierAdditionalData"], captionsList, HorizontalAlignment.Stretch);
		LayoutGroup container10 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.Buyer, "Buyer");
		_buyerName = AddStringControl(container10, Definition.Fields["BuyerName"], HorizontalAlignment.Stretch);
		_buyerLegalAddress = AddStringControl(container10, Definition.Fields["BuyerLegalAddress"], HorizontalAlignment.Stretch);
		_buyerBusinessAddress = AddComboBoxControl(container10, Definition.Fields["BuyerBusinessAddress"], null, HorizontalAlignment.Stretch);
		_buyerSettlAcc = AddDropDownControl(container10, Definition.Fields["BuyerSettlAcc"], new PartnerAccountInBank(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_buyerSettlAcc.CheckValue = false;
		_buyerTaxCode = AddStringControl(container10, Definition.Fields["BuyerTaxCode"], HorizontalAlignment.Stretch);
		_buyerMngPost = AddComboBoxControl(container10, Definition.Fields["BuyerMngPost"], itemsSource, HorizontalAlignment.Stretch);
		_buyerMngName = AddStringControl(container10, Definition.Fields["BuyerMngName"], HorizontalAlignment.Stretch);
		_buyerChiefAccPost = AddComboBoxControl(container10, Definition.Fields["BuyerChiefAccPost"], itemsSource2, HorizontalAlignment.Stretch);
		_buyerChiefAccName = AddStringControl(container10, Definition.Fields["BuyerChiefAccName"], HorizontalAlignment.Stretch);
		_buyerStateRegNum = AddStringControl(container10, Definition.Fields["BuyerStateRegNum"], HorizontalAlignment.Stretch);
		_buyerIDDocType = AddDropDownControl(container10, ArmSoft.SME.Trade.Common.Resources.IDDocTypes, isRequired: false, new SmartEnum(typeof(IdentityTypeSE)), allowAdditionButtons: false, HorizontalAlignment.Stretch);
		_buyerPassport = AddStringControl(container10, Definition.Fields["BuyerPassport"], HorizontalAlignment.Stretch);
		_buyerAdditionalData = AddComboBoxControl(container10, Definition.Fields["BuyerAdditionalData"], captionsList, HorizontalAlignment.Stretch);
		_buyerShippingMethod = AddComboBoxControl(container10, Definition.Fields["BuyerShippingMethod"], captionsList, HorizontalAlignment.Stretch);
		LayoutGroupExt layoutGroupExt = AddLayoutGroup(container10, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Common.Resources.Additional);
		layoutGroupExt.IsCollapsible = true;
		layoutGroupExt.IsCollapsed = true;
		IEnumerable<string> itemsSource3 = from row in TradeSettings.TradeCacheService.Partner.GetList()
			select row.Caption;
		_byWhomFio = AddComboBoxControl(layoutGroupExt, Definition.Fields["ByWhomFio"], itemsSource3, HorizontalAlignment.Stretch);
		_authorizationNumber = AddStringControl(layoutGroupExt, Definition.Fields["AuthorizationNumber"], HorizontalAlignment.Stretch);
		_authorizationDate = AddDateControl(layoutGroupExt, Definition.Fields["AuthorizationDate"]);
	}

	private void FillSupplierRekvizits()
	{
		_supplierName.Text = Settings.ParametersService.CompanyName();
		_supplierLegalAddress.Text = Settings.ParametersService.CompanyAddress();
		_supplierBusinessAddress.Text = Settings.ParametersService.CompanyBusinessAddress();
		_supplierSettlAcc.Code = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: true);
		_supplierTaxCode.EditValue = Settings.ParametersService.TaxCode();
		_supplierMngPost.EditValue = Settings.ParametersService.ManagerPost();
		_supplierMngName.EditValue = Settings.ParametersService.ManagerName();
		_supplierChiefAccPost.EditValue = Settings.ParametersService.ChiefAccountantPost();
		_supplierChiefAccName.EditValue = Settings.ParametersService.ChiefAccountantName();
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		if (_viewMode == WindowViewMode.Copy)
		{
			return;
		}
		string text = Settings.ParametersService.DefaultStorage();
		if (!string.IsNullOrWhiteSpace(text))
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(text);
			if (item != null && item.Retail && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
			{
				_storage.Code = text;
			}
		}
		if (_baseISN == Guid.Empty)
		{
			string text2 = Settings.ParametersService.DefaultCustomer();
			if (!string.IsNullOrWhiteSpace(text2))
			{
				PartnerCacheRow item2 = TradeSettings.TradeCacheService.Partner.GetItem(text2);
				if (item2 != null && item2.Customer)
				{
					_partnerContractContainer.PartnerCode.Code = text2;
				}
			}
		}
		_salesConsultant.Code = Settings.ParametersService.DefaultSalesConsultant();
		FillSupplierRekvizits();
		if (Settings.ParametersService.VATType().InList("1", "2") == _vatType.Code.InList("1", "2"))
		{
			_dealType.Code = Settings.ParametersService.DealType();
		}
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_invoicesRetailList.Clear();
		_maxRowID = 0;
		CalcSummAndSummVAT();
		_owner.Code = Settings.LoginService.UserName;
		_vatSumma.EditValue = 0m;
		_totalSumma.EditValue = 0m;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
	}

	private void CalcInvoiceTotalAndVATSumm(ref decimal TotalSumm, ref decimal VATSumm)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		if (_vatType.Code.Trim().InList("1", "2"))
		{
			foreach (MTInvoiceRetailSpecificationRow invoicesRetail in _invoicesRetailList)
			{
				num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(invoicesRetail.Summa, _vatType.Code, invoicesRetail.VAT.Value);
				num2 += invoicesRetail.Summa;
			}
			VATSumm = num;
			TotalSumm = num2;
			if (_vatType.Code.Trim() == "1")
			{
				TotalSumm += VATSumm;
			}
			return;
		}
		VATSumm = 0m;
		foreach (MTInvoiceRetailSpecificationRow invoicesRetail2 in _invoicesRetailList)
		{
			num2 += invoicesRetail2.Summa;
		}
		TotalSumm = num2;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_baseISN = PO.BaseISN;
		if (_baseISN == Guid.Empty)
		{
			BaseDocument = null;
		}
		else
		{
			BaseDocument = (TradeDocument)Settings.DocumentService.Load(_baseISN, withoutAccessCheck: true);
		}
		_invoiceRetailGrid.ItemsSource = _invoicesRetailList;
		_invoicesRetailList.Clear();
		_useEnvFees = Settings.ParametersService.EnvironmentalFeePayer() || GetEnvFeeUse();
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT in PO.MTList)
		{
			MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = new MTInvoiceRetailSpecificationRow(this)
			{
				RowID = mT.RowID,
				RowNum = mT.RowNum,
				ItemType = mT.ItemType,
				ItemID = mT.ItemID,
				ItemCode = mT.ItemCode,
				ItemName = mT.ItemName,
				UnitCode = mT.UnitCode,
				UnitBrief = mT.UnitBrief,
				Quantity = mT.Quantity,
				Price = mT.Price,
				Discount = mT.Discount,
				DiscountPrice = mT.DiscountPrice,
				Summa = mT.Summa,
				DiscountSumma = mT.DiscountSumma,
				EnvFeeSumma = mT.EnvFeeSumma,
				EnvFeePercent = mT.EnvFeePercent,
				VAT = mT.VAT.Value,
				DealType = mT.DealType,
				IsPartySelected = mT.IsPartySelected,
				RowDescription = mT.RowDescription,
				RowISN = mT.RowISN,
				Storage = mT.Storage
			};
			UsedDiscount += Math.Abs(mTInvoiceRetailSpecificationRow.Discount);
			_invoicesRetailList.Add(mTInvoiceRetailSpecificationRow);
		}
		_maxRowID = PO.MaxRowID;
		if (!(_baseISN == Guid.Empty) && !PO.StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
		}
		VATType_EditValueChangedForLoad();
		SmartEnumDataProvider.DataRow dataRow = (SmartEnumDataProvider.DataRow)_dealType.DataView.GetRowByValue(_dealType.Code);
		if (dataRow != null)
		{
			_dealTypeKey = dataRow.Code;
		}
		_partnerContractContainer.ValueChangedForLoadDoc();
		BindPartnerDataViews();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString());
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode == WindowViewMode.View)
		{
			return;
		}
		_currCode.IsReadOnly = true;
		_vatSumma.IsReadOnly = true;
		_totalSumma.IsReadOnly = true;
		bool flag = State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value || State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
		_date.IsReadOnly = flag2;
		_storage.IsReadOnly = flag2;
		_barCode.IsReadOnly = flag2;
		_codeEnter.IsEnabled = !flag2;
		_baseDocumentControl.IsReadOnly = flag2;
		_discountPercent.IsReadOnly = flag2 || _calcDiscountAuto;
		_vatType.IsReadOnly = flag2;
		_dealType.IsReadOnly = flag2;
		foreach (GridColumn column in _invoiceRetailGrid.Grid.Columns)
		{
			column.ReadOnly = flag2;
		}
		if (_calcDiscountAuto)
		{
			_discountCol.ReadOnly = true;
			_discountPriceCol.ReadOnly = true;
		}
		_invoiceRetailGrid.AllowAdd = !flag2;
		_invoiceRetailGrid.AllowEdit = !flag2;
		_invoiceRetailGrid.AllowDelete = !flag2;
		_invoiceRetailGrid.AllowDuplicate = !flag2;
		_invoiceRetailGrid.AllowMoveUp = !flag2;
		_invoiceRetailGrid.AllowMoveDown = !flag2;
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (StoredInDb || string.IsNullOrWhiteSpace(e.NewValue.ToString()))
		{
			return;
		}
		StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
		if (item == null)
		{
			return;
		}
		if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(e.NewValue.ToString()))
		{
			e.Handled = true;
			_storage.Code = e.OldValue.ToString();
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, e.NewValue.ToString()), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return;
		}
		foreach (MTInvoiceRetailSpecificationRow invoicesRetail in _invoicesRetailList)
		{
			invoicesRetail.Storage = item.Code;
		}
		if (!string.IsNullOrWhiteSpace(item.Address))
		{
			_supplierBusinessAddress.Text = item.Address;
		}
	}

	private void VATType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		VATType_EditValueChangedForLoad();
		VATTypeValidate(e.OldValue.ToString());
		_dealType.DataView = new SmartEnum(_vatType.Code.InList("2") ? typeof(InvoiceDealTypesSE) : typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
		if (((e.OldValue.ToString().Trim() == "3" || e.OldValue.ToString().Trim() == "4") && e.NewValue.ToString().Trim() == "2") || (e.OldValue.ToString().Trim() == "2" && (e.NewValue.ToString().Trim() == "3" || e.NewValue.ToString().Trim() == "4")))
		{
			_dealType.Code = string.Empty;
			_dealTypeKey = string.Empty;
			foreach (MTInvoiceRetailSpecificationRow invoicesRetail in _invoicesRetailList)
			{
				invoicesRetail.DealType = string.Empty;
			}
		}
		_invoiceRetailGrid.Grid.RefreshData();
	}

	private void VATType_EditValueChangedForLoad()
	{
		bool flag = _vatType.Code.InList("1", "2");
		_invoiceRetailGrid.Grid.Columns["VAT"].Visible = flag;
		_invoiceRetailGrid.Grid.Columns["VAT"].ShowInColumnChooser = flag;
		if (!flag)
		{
			_vatSumma.EditValue = 0m;
		}
		_dealType.DataView = new SmartEnum(_vatType.Code.InList("2") ? typeof(InvoiceDealTypesSE) : typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
	}

	private void VATTypeValidate(string vatTypeOldValue)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		_validateTotalSumm = false;
		if (_invoicesRetailList.Count > 0)
		{
			switch (_vatType.Code)
			{
			case "2":
				if (!(vatTypeOldValue == "3") && !(vatTypeOldValue == "4"))
				{
					break;
				}
				foreach (MTInvoiceRetailSpecificationRow invoicesRetail in _invoicesRetailList)
				{
					if (invoicesRetail.VAT.Value)
					{
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(invoicesRetail.Summa, "2", invoicesRetail.VAT.Value);
					}
					num2 += invoicesRetail.Summa;
				}
				_vatSumma.EditValue = num;
				_totalSumma.EditValue = num2;
				break;
			case "3":
			case "4":
				if (!(vatTypeOldValue == "2"))
				{
					break;
				}
				foreach (MTInvoiceRetailSpecificationRow invoicesRetail2 in _invoicesRetailList)
				{
					num2 += invoicesRetail2.Summa;
				}
				_vatSumma.EditValue = 0m;
				_totalSumma.EditValue = num2;
				break;
			}
		}
		if (_useEnvFees && (_vatType.Code.Trim() == "1" || vatTypeOldValue == "2"))
		{
			CalculateEnvironmentalFees();
		}
		CalculatePartSumma();
		_validateTotalSumm = true;
	}

	private void DealType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_dealType.Code))
		{
			SmartEnumDataProvider.DataRow dataRow = (SmartEnumDataProvider.DataRow)_dealType.DataView.GetRowByValue(_dealType.Code);
			if (dataRow != null)
			{
				_dealTypeKey = dataRow.Code;
				foreach (MTInvoiceRetailSpecificationRow invoicesRetail in _invoicesRetailList)
				{
					if (_vatType.Code.InList("3", "4") || !invoicesRetail.VAT.Value)
					{
						invoicesRetail.DealType = dataRow.Code;
					}
					else
					{
						invoicesRetail.DealType = string.Empty;
					}
				}
			}
		}
		else
		{
			_dealTypeKey = string.Empty;
			foreach (MTInvoiceRetailSpecificationRow invoicesRetail2 in _invoicesRetailList)
			{
				invoicesRetail2.DealType = string.Empty;
			}
		}
		_invoiceRetailGrid.Grid.RefreshData();
	}

	private void DealType_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		_dealType.DataView = new SmartEnum(_vatType.Code.InList("2") ? typeof(InvoiceDealTypesSE) : typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
	}

	private void ExportedType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_taxInvoiceNumber.IsEnabledMagicButton = _exportedType.Code != "1";
	}

	private void CalculatePartSumma()
	{
		decimal value = _totalSumma.Value;
		_partnerContractContainer.CalculatePartSumma(value);
	}

	private void CalcSummAndSummVAT()
	{
		decimal num = 0m;
		decimal num2 = 0m;
		_validateTotalSumm = false;
		if (_vatType.Code.Trim().InList("1", "2"))
		{
			foreach (MTInvoiceRetailSpecificationRow invoicesRetail in _invoicesRetailList)
			{
				if (invoicesRetail.Summa != 0m && invoicesRetail.VAT.Value)
				{
					num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(invoicesRetail.Summa, _vatType.Code, invoicesRetail.VAT.Value);
				}
				num2 += invoicesRetail.Summa;
			}
			_vatSumma.EditValue = num;
			_totalSumma.EditValue = num2;
			if (_vatType.Code.Trim() == "1")
			{
				_totalSumma.EditValue = Convert.ToDecimal(_totalSumma.EditValue) + Convert.ToDecimal(_vatSumma.EditValue);
			}
		}
		else
		{
			_vatSumma.EditValue = 0m;
			foreach (MTInvoiceRetailSpecificationRow invoicesRetail2 in _invoicesRetailList)
			{
				num2 += invoicesRetail2.Summa;
			}
			_totalSumma.EditValue = num2;
		}
		_validateTotalSumm = true;
	}

	private void PartnerCodeEditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		PartnerCodeEditValueChanged();
		CalculatePartSumma();
	}

	private void PartnerCodeEditAdditionalValueChanged(object sender, EventArgs e)
	{
		CalculatePartSumma();
	}

	private void ContractCodeEditValueChanged(object sender, EventArgs e)
	{
		CalculatePartSumma();
	}

	private void PartnerCodeEditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_partnerContractContainer.PartnerCode.Code) && _partnerContractContainer.PartnerCode.DoValidate())
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partnerContractContainer.PartnerCode.Code);
			if (item != null)
			{
				if (!_buyerName.IsReadOnly)
				{
					_buyerName.Text = item.Caption;
				}
				_buyerLegalAddress.Text = item.Address;
				_buyerBusinessAddress.Text = item.BusinessAddress;
				_buyerSettlAcc.Code = item.SettlAcc;
				BindPartnerDataViews(item);
				if (!_buyerTaxCode.IsReadOnly)
				{
					_buyerTaxCode.Text = item.TaxCode;
				}
				_buyerMngPost.EditValue = item.ManagerPost;
				_buyerMngName.Text = item.ManagerFIO;
				_buyerChiefAccPost.EditValue = item.ChiefaccPost;
				_buyerChiefAccName.Text = item.ChiefaccFIO;
				_buyerStateRegNum.Text = item.PartStateRegister;
				_buyerIDDocType.Code = item.IDDocType;
				_buyerPassport.Text = item.Passport;
			}
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(item, _partnerContractContainer.Contract.Code);
			if (contract != null && _useDiscount)
			{
				_discountPercent.Value = contract.Percent;
			}
		}
		else
		{
			if (!_buyerName.IsReadOnly)
			{
				_buyerName.Text = "";
			}
			_buyerLegalAddress.Text = "";
			_buyerBusinessAddress.Text = "";
			_buyerSettlAcc.Code = "";
			if (!_buyerTaxCode.IsReadOnly)
			{
				_buyerTaxCode.Text = "";
			}
			_buyerMngPost.EditValue = "";
			_buyerMngName.Text = "";
			_buyerChiefAccPost.EditValue = "";
			_buyerChiefAccName.Text = "";
			_buyerStateRegNum.Text = "";
			_buyerIDDocType.Code = "";
			_buyerPassport.Text = "";
			BindPartnerDataViews();
		}
	}

	private void BindPartnerDataViews(PartnerCacheRow partnerCacheRow = null)
	{
		if (!string.IsNullOrWhiteSpace(_partnerContractContainer.PartnerCode.Code) && _partnerContractContainer.PartnerCode.DoValidate())
		{
			if (partnerCacheRow == null)
			{
				partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(_partnerContractContainer.PartnerCode.Code, throwExceptionNotExits: true);
			}
			_buyerBusinessAddress.ItemsSource = TradeSettings.TradeCacheService.Partner.GetAllBusinessAddresses(partnerCacheRow);
			_buyerSettlAcc.DataView = new PartnerAccountInBank(partnerCacheRow.PartCode);
		}
		else
		{
			_buyerBusinessAddress.ItemsSource = null;
			_buyerSettlAcc.DataView = new PartnerAccountInBank();
		}
	}

	private void MTinvoiceRetailGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateDocFields();
		_invoicesRetailList.Add(new MTInvoiceRetailSpecificationRow(this)
		{
			ItemType = GoodsTypesSE.Good.Value,
			Storage = _storage.Code,
			Discount = _discountPercent.Value,
			IsPartySelected = false,
			DealType = _dealTypeKey,
			RowID = _maxRowID
		});
		_maxRowID++;
		e.RowNumber = _invoicesRetailList.Count - 1;
	}

	private void MTInvoiceRetailGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateDocFields();
		GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(DialogWindowMode.NewModeWithNext, this);
		goodsSelectPartyWindow.ShowDialog();
		int addedRows = goodsSelectPartyWindow.AddedRows;
		if (addedRows != 0)
		{
			_maxRowID += addedRows;
			_invoiceRetailGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			CalcDiscountByScale();
		}
	}

	private void MTInvoiceRetailGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = (MTInvoiceRetailSpecificationRow)_invoiceRetailGrid.Grid.GetRow(e.RowHandle);
		if (mTInvoiceRetailSpecificationRow != null)
		{
			ValidateDocFields();
			GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(DialogWindowMode.NewModeWithNext, this, mTInvoiceRetailSpecificationRow);
			goodsSelectPartyWindow.ShowDialog();
			int addedRows = goodsSelectPartyWindow.AddedRows;
			if (addedRows != 0)
			{
				_maxRowID += addedRows;
				_invoiceRetailGrid.Grid.RefreshData();
				CalcSummAndSummVAT();
				CalcDiscountByScale();
			}
		}
	}

	private void MTInvoiceRetailGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = (MTInvoiceRetailSpecificationRow)_invoiceRetailGrid.Grid.GetRow(e.RowHandle);
		if (mTInvoiceRetailSpecificationRow != null)
		{
			ValidateDocFields();
			GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(e.AllowEdit ? DialogWindowMode.EditMode : DialogWindowMode.ViewMode, this, mTInvoiceRetailSpecificationRow);
			e.RowEdited = Convert.ToBoolean(goodsSelectPartyWindow.ShowDialog());
			if (e.AllowEdit && e.RowEdited)
			{
				_invoiceRetailGrid.Grid.RefreshData();
				CalcSummAndSummVAT();
				CalcDiscountByScale();
			}
		}
	}

	private void MTInvoiceRetailTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTInvoiceRetailSpecificationRow mtInvRGridRow = (MTInvoiceRetailSpecificationRow)_invoiceRetailGrid.Grid.GetRow(e.RowHandle);
		if (mtInvRGridRow == null)
		{
			return;
		}
		string oldUnit = string.Empty;
		if (e.OldValue != null)
		{
			oldUnit = e.OldValue.ToString();
		}
		if (e.Column == _itemCodeCol)
		{
			try
			{
				if (e.Value == null)
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvRGridRow.RowID);
					mtInvRGridRow.ItemType = "";
					mtInvRGridRow.ItemID = -1;
					mtInvRGridRow.ItemCode = "";
					mtInvRGridRow.ItemName = "";
					mtInvRGridRow.UnitBrief = "";
					mtInvRGridRow.UnitCode = "";
					mtInvRGridRow.Price = 0m;
					mtInvRGridRow.Discount = 0m;
					mtInvRGridRow.DiscountPrice = 0m;
					mtInvRGridRow.Summa = 0m;
					if (_useEnvFees)
					{
						mtInvRGridRow.EnvFeePercent = 0m;
						mtInvRGridRow.EnvFeeSumma = 0m;
					}
					mtInvRGridRow.IsPartySelected = false;
					mtInvRGridRow.RowDescription = "";
				}
				else
				{
					if (e.OldValue != null && e.OldValue.ToString() != e.Value.ToString())
					{
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvRGridRow.RowID);
						mtInvRGridRow.ItemType = TradeSettings.TradeCacheService.Material.GetItem(e.Value.ToString()).Type;
						mtInvRGridRow.IsPartySelected = ((mtInvRGridRow.ItemType != GoodsTypesSE.Good.Value) ? ((bool?)null) : new bool?(false));
					}
					string text = e.Value.ToString();
					foreach (MTInvoiceRetailSpecificationRow invoicesRetail in _invoicesRetailList)
					{
						if (mtInvRGridRow != invoicesRetail && text.TrimEnd() == invoicesRetail.ItemCode.TrimEnd())
						{
							if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, text), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
							{
								mtInvRGridRow.ItemType = "";
								mtInvRGridRow.ItemName = "";
								mtInvRGridRow.ItemID = -1;
								mtInvRGridRow.ItemCode = "";
								mtInvRGridRow.Quantity = 0m;
								mtInvRGridRow.Summa = 0m;
								mtInvRGridRow.Discount = 0m;
								mtInvRGridRow.DiscountPrice = 0m;
								mtInvRGridRow.Price = 0m;
								if (_useEnvFees)
								{
									mtInvRGridRow.EnvFeePercent = 0m;
									mtInvRGridRow.EnvFeeSumma = 0m;
								}
								mtInvRGridRow.UnitCode = "";
								mtInvRGridRow.UnitBrief = "";
								mtInvRGridRow.IsPartySelected = false;
								mtInvRGridRow.RowDescription = "";
								_invoiceRetailGrid.Grid.RefreshData();
								return;
							}
							break;
						}
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text, throwExceptionNotExits: true);
					mtInvRGridRow.ItemName = item.Caption;
					mtInvRGridRow.ItemCode = text;
					mtInvRGridRow.ItemID = item.Id;
					mtInvRGridRow.ItemType = item.Type;
					mtInvRGridRow.UnitBrief = item.UnitRow.Brief;
					mtInvRGridRow.UnitCode = item.Unit;
					_unitCodeCol.DataView = new MTQntUnits(item.MTCode);
					mtInvRGridRow.VAT = item.VAT;
					if (mtInvRGridRow.ItemType == ItemTypesSE.Goods.Value)
					{
						mtInvRGridRow.IsPartySelected = false;
						MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtInvRGridRow.ItemID, mtInvRGridRow.UnitCode);
						if (qntUnit != null)
						{
							MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
							{
								MTCode = mtInvRGridRow.ItemCode,
								UnitCoef = qntUnit.Coef,
								Storage = _storage.Code,
								Date = _date.EditValue
							};
							List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
							if (rows.Count > 0)
							{
								mtInvRGridRow.Price = rows[0].Price;
							}
						}
					}
					else
					{
						mtInvRGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType.Code, mtInvRGridRow.ItemID, mtInvRGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
					}
					if (_useDiscount)
					{
						mtInvRGridRow.Discount = _discountPercent.Value;
						if (mtInvRGridRow.Discount == 0m || item.Discount != 0m)
						{
							mtInvRGridRow.Discount = item.Discount;
						}
					}
					mtInvRGridRow.DiscountPrice = (Convert.ToDecimal(mtInvRGridRow.Price) - Convert.ToDecimal(mtInvRGridRow.Price) * Convert.ToDecimal(mtInvRGridRow.Discount) / 100m).RoundAs(2);
					mtInvRGridRow.Summa = (Convert.ToDecimal(mtInvRGridRow.DiscountPrice) * Convert.ToDecimal(mtInvRGridRow.Quantity)).RoundAs(2);
					if (_useEnvFees && mtInvRGridRow.ItemType == ItemTypesSE.Goods)
					{
						mtInvRGridRow.EnvFeePercent = item.EcoFeeRate;
						mtInvRGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvRGridRow.Summa, mtInvRGridRow.EnvFeePercent, mtInvRGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
					}
					if (!string.IsNullOrWhiteSpace(_dealTypeKey))
					{
						if (_vatType.Code == "3" || _vatType.Code == "4" || !item.VAT)
						{
							mtInvRGridRow.DealType = _dealTypeKey;
						}
						else
						{
							mtInvRGridRow.DealType = string.Empty;
						}
					}
				}
			}
			catch
			{
				mtInvRGridRow.ItemType = "";
				mtInvRGridRow.ItemName = "";
				mtInvRGridRow.ItemID = -1;
				mtInvRGridRow.UnitBrief = "";
				mtInvRGridRow.UnitCode = "";
				mtInvRGridRow.Quantity = 0m;
				mtInvRGridRow.Price = 0m;
				mtInvRGridRow.Discount = 0m;
				mtInvRGridRow.DiscountPrice = 0m;
				mtInvRGridRow.Summa = 0m;
				if (_useEnvFees)
				{
					mtInvRGridRow.EnvFeePercent = 0m;
					mtInvRGridRow.EnvFeeSumma = 0m;
				}
				mtInvRGridRow.IsPartySelected = false;
				mtInvRGridRow.RowDescription = "";
			}
		}
		else if (e.Column == _unitCodeCol)
		{
			try
			{
				if (e.Value == null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(e.Value.ToString(), throwExceptionNotExits: true);
					mtInvRGridRow.UnitBrief = ((item2 == null) ? "" : item2.Brief);
					mtInvRGridRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(oldUnit, Convert.ToString(e.Value), mtInvRGridRow.Price, mtInvRGridRow.ItemCode);
					mtInvRGridRow.DiscountPrice = (Convert.ToDecimal(mtInvRGridRow.Price) - Convert.ToDecimal(mtInvRGridRow.Price) * Convert.ToDecimal(mtInvRGridRow.Discount) / 100m).RoundAs(2);
					mtInvRGridRow.Summa = (Convert.ToDecimal(mtInvRGridRow.DiscountPrice) * Convert.ToDecimal(mtInvRGridRow.Quantity)).RoundAs(2);
					if (_useEnvFees && mtInvRGridRow.ItemType == InvoiceRowTypeSE.Goods.Value)
					{
						mtInvRGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvRGridRow.Summa, mtInvRGridRow.EnvFeePercent, mtInvRGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
					}
				}
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}
		else if (e.Column == _quantityCol)
		{
			if (Convert.ToDecimal(e.OldValue) != Convert.ToDecimal(e.Value) && Convert.ToDecimal(e.OldValue) != 0m)
			{
				if (mtInvRGridRow.IsPartySelected == true)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ForRowSelectedParties, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				}
				PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvRGridRow.RowID);
				mtInvRGridRow.IsPartySelected = false;
			}
			mtInvRGridRow.Summa = (Convert.ToDecimal(e.Value) * Convert.ToDecimal(mtInvRGridRow.DiscountPrice)).RoundAs(2);
			if (_useEnvFees && mtInvRGridRow.ItemType == InvoiceRowTypeSE.Goods.Value)
			{
				mtInvRGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvRGridRow.Summa, mtInvRGridRow.EnvFeePercent, mtInvRGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
		}
		else if (e.Column == _priceCol)
		{
			mtInvRGridRow.Price = Convert.ToDecimal(e.Value);
			mtInvRGridRow.DiscountPrice = (Convert.ToDecimal(mtInvRGridRow.Price) - Convert.ToDecimal(mtInvRGridRow.Price) * Convert.ToDecimal(mtInvRGridRow.Discount) / 100m).RoundAs(2);
			mtInvRGridRow.Summa = (Convert.ToDecimal(mtInvRGridRow.DiscountPrice) * Convert.ToDecimal(mtInvRGridRow.Quantity)).RoundAs(2);
			if (_useEnvFees && mtInvRGridRow.ItemType == InvoiceRowTypeSE.Goods.Value)
			{
				mtInvRGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvRGridRow.Summa, mtInvRGridRow.EnvFeePercent, mtInvRGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
		}
		else if (e.Column == _discountCol)
		{
			mtInvRGridRow.DiscountPrice = (Convert.ToDecimal(mtInvRGridRow.Price) - Convert.ToDecimal(mtInvRGridRow.Price) * Convert.ToDecimal(mtInvRGridRow.Discount) / 100m).RoundAs(2);
			mtInvRGridRow.Summa = (Convert.ToDecimal(mtInvRGridRow.DiscountPrice) * Convert.ToDecimal(mtInvRGridRow.Quantity)).RoundAs(2);
			if (_useEnvFees && mtInvRGridRow.ItemType == InvoiceRowTypeSE.Goods.Value)
			{
				mtInvRGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvRGridRow.Summa, mtInvRGridRow.EnvFeePercent, mtInvRGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
		}
		else if (e.Column == _discountPriceCol)
		{
			mtInvRGridRow.DiscountPrice = Convert.ToDecimal(e.Value);
			mtInvRGridRow.Summa = (Convert.ToDecimal(mtInvRGridRow.DiscountPrice) * Convert.ToDecimal(mtInvRGridRow.Quantity)).RoundAs(2);
			if (mtInvRGridRow.Price != 0m)
			{
				mtInvRGridRow.Discount = (100m * (Convert.ToDecimal(mtInvRGridRow.Price) - Convert.ToDecimal(mtInvRGridRow.DiscountPrice)) / Convert.ToDecimal(mtInvRGridRow.Price)).RoundAs(2);
				if (Convert.ToDecimal(mtInvRGridRow.Discount) / 1000m >= 1m || Convert.ToDecimal(mtInvRGridRow.Discount) / 1000m <= -1m)
				{
					mtInvRGridRow.Discount = 0m;
					mtInvRGridRow.DiscountPrice = mtInvRGridRow.Price;
					mtInvRGridRow.DiscountSumma = 0m;
					mtInvRGridRow.Summa = mtInvRGridRow.Price * mtInvRGridRow.Quantity;
				}
			}
			if (_useEnvFees && mtInvRGridRow.ItemType == InvoiceRowTypeSE.Goods.Value)
			{
				mtInvRGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvRGridRow.Summa, mtInvRGridRow.EnvFeePercent, mtInvRGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
		}
		else if (e.Column == _summaCol)
		{
			mtInvRGridRow.Summa = Convert.ToDecimal(e.Value);
			if (mtInvRGridRow.Quantity > 0m)
			{
				mtInvRGridRow.DiscountPrice = (Convert.ToDecimal(mtInvRGridRow.Summa) / Convert.ToDecimal(mtInvRGridRow.Quantity)).RoundAs(2);
			}
			if (mtInvRGridRow.Price != 0m)
			{
				mtInvRGridRow.Discount = (100m * (Convert.ToDecimal(mtInvRGridRow.Price) - Convert.ToDecimal(mtInvRGridRow.DiscountPrice)) / Convert.ToDecimal(mtInvRGridRow.Price)).RoundAs(2);
			}
			if (_useEnvFees && mtInvRGridRow.ItemType == InvoiceRowTypeSE.Goods.Value)
			{
				mtInvRGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvRGridRow.Summa, mtInvRGridRow.EnvFeePercent, mtInvRGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
		}
		else if (e.Column == _envFeePercentCol)
		{
			mtInvRGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvRGridRow.Summa, Convert.ToDecimal(e.Value), mtInvRGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
		}
		else if (e.Column == _envFeeSummaCol)
		{
			if (mtInvRGridRow.Summa == 0m || Convert.ToDecimal(e.Value) == 0m)
			{
				mtInvRGridRow.EnvFeePercent = 0m;
			}
			else
			{
				decimal summa = mtInvRGridRow.Summa;
				if (_vatType.Code.Trim() == "2" && mtInvRGridRow.VAT.Value)
				{
					summa -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtInvRGridRow.Summa, _vatType.Code.Trim(), itemVAT: true);
				}
				summa -= Convert.ToDecimal(e.Value);
				if (summa == 0m)
				{
					mtInvRGridRow.EnvFeePercent = 0m;
				}
				else
				{
					mtInvRGridRow.EnvFeePercent = (Convert.ToDecimal(e.Value) * 100m / summa).RoundAs(2);
				}
			}
		}
		else if (e.Column == _vatCol)
		{
			if (_useEnvFees && mtInvRGridRow.ItemType == InvoiceRowTypeSE.Goods.Value)
			{
				mtInvRGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvRGridRow.Summa, mtInvRGridRow.EnvFeePercent, Convert.ToBoolean(e.Value), _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
			if ((bool)e.Value)
			{
				mtInvRGridRow.DealType = string.Empty;
			}
		}
		_invoiceRetailGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
		CalcDiscountByScale();
	}

	private void CalcDiscountByScale()
	{
		if (!_useDiscount || !_calcDiscountAuto)
		{
			return;
		}
		decimal num = CalculateDiscountByScale();
		if (num != -1m)
		{
			_discountPercent.Value = num;
			CalcSummAndSummVAT();
			if (_useEnvFees)
			{
				CalculateEnvironmentalFees();
			}
			_invoiceRetailGrid.Grid.RefreshData();
		}
	}

	private decimal CalculateDiscountByScale()
	{
		decimal num = TradeSettings.CreateTradeClient().CommonServices.CalculateDiscountByScale(_totalSumma.Value);
		if (num != -1m)
		{
			foreach (MTInvoiceRetailSpecificationRow invoicesRetail in _invoicesRetailList)
			{
				if (!invoicesRetail.IsDeleted)
				{
					invoicesRetail.Discount = num;
					invoicesRetail.DiscountPrice = Methods.RoundPrice((invoicesRetail.Price * (1m - num / 100m)).RoundAs(2), string.Empty);
					invoicesRetail.Summa = (invoicesRetail.Quantity * invoicesRetail.DiscountPrice).RoundAs(2);
					invoicesRetail.DiscountSumma = (invoicesRetail.Quantity * invoicesRetail.Price - invoicesRetail.Summa).RoundAs(2);
				}
			}
		}
		return num;
	}

	private void CalculateEnvironmentalFees()
	{
		foreach (MTInvoiceRetailSpecificationRow invoicesRetail in _invoicesRetailList)
		{
			if (invoicesRetail.ItemType == GoodsTypesSE.Good.Value)
			{
				invoicesRetail.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(invoicesRetail.Summa, invoicesRetail.EnvFeePercent, invoicesRetail.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
		}
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.BaseISN = _baseISN;
		PO.MTList.Clear();
		foreach (MTInvoiceRetailSpecificationRow invoicesRetail in _invoicesRetailList)
		{
			MTInvoiceBase.MTInvoiceSpecificationRow item = new MTInvoiceBase.MTInvoiceSpecificationRow
			{
				RowID = invoicesRetail.RowID,
				RowNum = invoicesRetail.RowNum,
				ItemType = invoicesRetail.ItemType,
				ItemID = invoicesRetail.ItemID,
				ItemCode = invoicesRetail.ItemCode,
				ItemName = invoicesRetail.ItemName,
				UnitCode = invoicesRetail.UnitCode,
				UnitBrief = invoicesRetail.UnitBrief,
				Quantity = invoicesRetail.Quantity,
				Price = invoicesRetail.Price,
				Discount = invoicesRetail.Discount,
				DiscountPrice = invoicesRetail.DiscountPrice,
				Summa = invoicesRetail.Summa,
				DiscountSumma = invoicesRetail.DiscountSumma,
				EnvFeeSumma = invoicesRetail.EnvFeeSumma,
				EnvFeePercent = invoicesRetail.EnvFeePercent,
				VAT = invoicesRetail.VAT,
				DealType = invoicesRetail.DealType,
				IsPartySelected = invoicesRetail.IsPartySelected,
				RowDescription = invoicesRetail.RowDescription,
				RowISN = invoicesRetail.RowISN
			};
			PO.MTList.Add(item);
		}
		PO.MaxRowID = _maxRowID;
	}

	protected override bool BeforeStore(StoreRequest sr)
	{
		if (sr.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && !string.IsNullOrWhiteSpace(_partnerContractContainer.PartnerCode.Code) && !string.IsNullOrWhiteSpace(_partnerContractContainer.Contract.Code))
		{
			PO.IsCheckLimitAnswerYes = false;
			int num = TradeSettings.TradeCacheService.Partner.GetItem(_partnerContractContainer.PartnerCode.Code)?.Id ?? (-1);
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(num, _partnerContractContainer.Contract.Code);
			if (contract != null && contract.ControlReceivables)
			{
				if ((TradeSettings.CreateTradeClient().CommonServices.GetCurrentPartnerRemByContract(num, contract.Contract, string.Empty, PO.ISN).SummCurr + (decimal)_partnerContractContainer.SummaPart.EditValue).RoundAs(2) > contract.ReceivablesLimit)
				{
					if (Settings.ParametersService.AllowExceedDebtLimit())
					{
						if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ReceivablesConfirmExceed, contract.Contract), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
						{
							return false;
						}
						return true;
					}
					ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ReceivablesExceedDenied, contract.Contract), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
					return false;
				}
			}
			else
			{
				PO.IsCheckLimitAnswerYes = true;
			}
		}
		return base.BeforeStore(sr);
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_invoiceRetailGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double DefaultWidth, double DefaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(DefaultWidth, DefaultHeight, rk);
		_invoiceRetailGrid.RestoreSettings(rk);
	}

	protected override bool AllowChangeGridMode()
	{
		return true;
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonImportFromFile.ConvertForToolbar("t_import_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barButtonJumpTo = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barButtonImportFromFile = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.Import,
			KeyGesture = new KeyGesture((Key)52, (ModifierKeys)2),
			Glyph = ImageService.GetImage("Import_16.svg"),
			LargeGlyph = ImageService.GetImage("Import_32.svg")
		};
		_barButtonImportFromFile.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonImportFromFile_ItemClick, this).Handler;
		_barManager.Items.Add(_barButtonImportFromFile);
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonImportFromFile.CreateLink());
		Ribbon.ToolbarItemLinks.Add(_barButtonImportFromFile.CreateLink());
		_barButtonGoodsOperationsJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperations, new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", PO.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption())
		});
		_barButtonGoodsOperationsJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonGoodsRetailOperationsJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepOperationsRetail.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperationsRetail, new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", PO.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption())
		});
		_barButtonGoodsRetailOperationsJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonPartnerOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnerOp, new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", PO.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption()),
			new PropertyWithValue("ShowLinks", true.ToString())
		});
		_barButtonPartnerOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonMTSalesAnalysis = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepSalesAnalysis.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis, new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", PO.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption()),
			new PropertyWithValue("ShowNCCVATSumm", true.ToString())
		});
		_barButtonMTSalesAnalysis.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
	}

	protected override void SetReportBarButtonsProperties()
	{
		base.SetReportBarButtonsProperties();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString());
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
		ReportBarButtonItem barButtonGoodsOperationsJournal = _barButtonGoodsOperationsJournal;
		num = 2;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
		span2[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span2[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonGoodsOperationsJournal.PropertyWithValues = list2;
		ReportBarButtonItem barButtonGoodsRetailOperationsJournal = _barButtonGoodsRetailOperationsJournal;
		num = 2;
		List<PropertyWithValue> list3 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<PropertyWithValue> span3 = CollectionsMarshal.AsSpan(list3);
		span3[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span3[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonGoodsRetailOperationsJournal.PropertyWithValues = list3;
		ReportBarButtonItem barButtonPartnerOperationJournal = _barButtonPartnerOperationJournal;
		num = 3;
		List<PropertyWithValue> list4 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list4, num);
		Span<PropertyWithValue> span4 = CollectionsMarshal.AsSpan(list4);
		span4[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span4[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span4[2] = new PropertyWithValue("ShowLinks", true.ToString());
		barButtonPartnerOperationJournal.PropertyWithValues = list4;
		ReportBarButtonItem barButtonMTSalesAnalysis = _barButtonMTSalesAnalysis;
		num = 3;
		List<PropertyWithValue> list5 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list5, num);
		Span<PropertyWithValue> span5 = CollectionsMarshal.AsSpan(list5);
		span5[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span5[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span5[2] = new PropertyWithValue("ShowNCCVATSumm", true.ToString());
		barButtonMTSalesAnalysis.PropertyWithValues = list5;
	}

	private void BarButtonImportFromFile_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (_date.IsReadOnly || !_date.DoValidate())
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(_storage.Code))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrorRetailStorageFromFile);
		}
		if (_invoicesRetailList.Count <= 0 || ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.LoadMaterialsList, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.No)
		{
			_invoicesRetailList.Clear();
			PO.MTExpDocSelectedPartiesList.Clear();
			_maxRowID = 0;
			TerminalHelper.TerminalImporter terminalImporter = new TerminalHelper.TerminalImporter(this);
			if (terminalImporter.ImportFromFile((decimal quantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow) =>
			{
				AddMTToListNewRow(goodRow, goodBarcodeRow.QntUnit, TradeSettings.TradeCacheService.QntUnit.GetItem(goodBarcodeRow.QntUnit).Brief, quantity);
			}))
			{
				terminalImporter.ShowReport();
			}
		}
	}

	protected override void CreateChildDocument(short childDocType, MTInvoiceRetail baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		switch (childDocType)
		{
		case 2:
		case 4:
		case 5:
		case 15:
		case 20:
			CreateBasedOnDocHelper.CreateDocument((ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)(SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
			break;
		}
	}

	protected override bool AllowPrint()
	{
		return true;
	}

	protected override bool AllowPrintOnNew()
	{
		return true;
	}

	private void BaseDocumentControl_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		DocumentsJournal documentsJournal = (DocumentsJournal)e.DataView;
		documentsJournal.DialogTop = e.Top;
		documentsJournal.DialogLeft = e.Left;
		if (!e.DataView.ShowDialog())
		{
			e.Cancel = true;
		}
	}

	private void BaseDocumentControl_EditKeyValueChanged(object sender, EditKeyValueChangedEventArgs e)
	{
		BaseISN = ((e.KeyValue != null) ? ((Guid)e.KeyValue) : Guid.Empty);
	}

	private void BaseDocumentDelete_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		_baseDocumentControl.KeyValue = Guid.Empty;
		_baseDocumentControl.DoValidate();
	}

	private void BaseDocumentSearch_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		if (BaseISN != Guid.Empty)
		{
			Settings.DocumentService.ShowWindow(BaseISN, WindowViewMode.Edit);
		}
	}

	private void CreateBasedOnDoc()
	{
		_comment.EditValue = BaseDocument.Comment;
		short pOType = BaseDocument.POType;
		if (pOType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTakingRetail.Value)
		{
			MTStockTakingRetail mTStockTakingRetail = (MTStockTakingRetail)BaseDocument;
			_storage.Code = mTStockTakingRetail.Storage;
			_invoicesRetailList.Clear();
			_maxRowID = 0;
			foreach (MTStockTakingBase.MTStockTakingSpecificationRow mT in mTStockTakingRetail.MTList)
			{
				if (mT.VarianceQuantity < 0m)
				{
					_maxRowID++;
					MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = new MTInvoiceRetailSpecificationRow(this)
					{
						Storage = _storage.Code,
						ItemType = GoodsTypesSE.Good.Value,
						ItemID = mT.MTID,
						ItemCode = mT.MTCode,
						ItemName = mT.MTName,
						UnitCode = mT.UnitCode,
						UnitBrief = mT.UnitBrief,
						Quantity = Math.Abs(mT.VarianceQuantity),
						Price = mT.Price,
						Discount = 0m,
						DiscountPrice = mT.Price,
						IsPartySelected = false,
						RowID = _maxRowID
					};
					mTInvoiceRetailSpecificationRow.Summa = (mTInvoiceRetailSpecificationRow.Quantity * mTInvoiceRetailSpecificationRow.Price).RoundAs(2);
					MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(mTInvoiceRetailSpecificationRow.ItemID);
					if (itemById != null)
					{
						mTInvoiceRetailSpecificationRow.VAT = itemById.VAT;
					}
					_invoicesRetailList.Add(mTInvoiceRetailSpecificationRow);
				}
			}
			CalcSummAndSummVAT();
			return;
		}
		short num = pOType;
		if (num == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value || num == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value)
		{
			MTMoveWithConfirmationBase mTMoveWithConfirmationBase = (MTMoveWithConfirmationBase)BaseDocument;
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(mTMoveWithConfirmationBase.StorageIncome);
			if (item == null || !item.Retail)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.CreateBasedMTInvoiceRetailError);
			}
			_storage.Code = mTMoveWithConfirmationBase.StorageIncome;
			_invoicesRetailList.Clear();
			_maxRowID = 0;
			foreach (MTMoveBase.MTMoveSpecificationRow mT2 in mTMoveWithConfirmationBase.MTList)
			{
				MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow2 = new MTInvoiceRetailSpecificationRow(this)
				{
					Storage = mTMoveWithConfirmationBase.StorageIncome,
					ItemType = GoodsTypesSE.Good.Value,
					IsPartySelected = true,
					ItemID = mT2.MTID,
					ItemCode = mT2.MTCode,
					ItemName = mT2.MTName,
					UnitCode = mT2.UnitCode,
					UnitBrief = mT2.UnitBrief,
					Quantity = mT2.Quantity,
					Price = mT2.Price.Value,
					RowID = _maxRowID
				};
				MaterialCacheRow itemById2 = TradeSettings.TradeCacheService.Material.GetItemById(mTInvoiceRetailSpecificationRow2.ItemID);
				if (itemById2 != null)
				{
					mTInvoiceRetailSpecificationRow2.VAT = itemById2.VAT;
				}
				if (_useDiscount)
				{
					mTInvoiceRetailSpecificationRow2.Discount = itemById2.Discount;
					mTInvoiceRetailSpecificationRow2.DiscountPrice = (mTInvoiceRetailSpecificationRow2.Price - mTInvoiceRetailSpecificationRow2.Price * mTInvoiceRetailSpecificationRow2.Discount / 100m).RoundAs(2);
				}
				else
				{
					mTInvoiceRetailSpecificationRow2.Discount = 0m;
					mTInvoiceRetailSpecificationRow2.DiscountPrice = mTInvoiceRetailSpecificationRow2.Price;
				}
				mTInvoiceRetailSpecificationRow2.Summa = (mTInvoiceRetailSpecificationRow2.DiscountPrice * mTInvoiceRetailSpecificationRow2.Quantity).RoundAs(2);
				if (_useEnvFees)
				{
					mTInvoiceRetailSpecificationRow2.EnvFeePercent = itemById2.EcoFeeRate;
					mTInvoiceRetailSpecificationRow2.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mTInvoiceRetailSpecificationRow2.Summa, mTInvoiceRetailSpecificationRow2.EnvFeePercent, mTInvoiceRetailSpecificationRow2.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
				}
				_maxRowID++;
				_invoicesRetailList.Add(mTInvoiceRetailSpecificationRow2);
			}
			CalcSummAndSummVAT();
		}
		else
		{
			PartnerCacheRow itemById3 = TradeSettings.TradeCacheService.Partner.GetItemById(BaseDocument.PartnerID);
			if (itemById3 != null && itemById3.Customer)
			{
				_partnerContractContainer.PartnerCode.Code = BaseDocument.PartnerContractGroup.PartnerCode;
				_partnerContractContainer.Contract.Code = BaseDocument.PartnerContractGroup.ContractCode;
				_buyerName.Text = itemById3.Caption;
				_buyerLegalAddress.Text = itemById3.Address;
				_buyerBusinessAddress.Text = itemById3.BusinessAddress;
				_buyerSettlAcc.Code = itemById3.SettlAcc;
				_buyerTaxCode.Text = itemById3.TaxCode;
				_buyerMngPost.Text = itemById3.ManagerPost;
				_buyerMngName.Text = itemById3.ManagerFIO;
				_buyerChiefAccPost.Text = itemById3.ChiefaccPost;
				_buyerChiefAccName.Text = itemById3.ChiefaccFIO;
				_buyerStateRegNum.Text = itemById3.PartStateRegister;
				_buyerIDDocType.Code = itemById3.IDDocType;
				_buyerPassport.Text = itemById3.Passport;
			}
		}
	}

	private void SupplierBusinessAddress_EditKeyValueChanged(object sender, object KeyValue)
	{
		if (KeyValue != null)
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(Convert.ToString(KeyValue));
			if (item != null)
			{
				_supplierBusinessAddress.Text = item.Address;
			}
		}
	}

	private void DiscountPercent_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue == null)
		{
			return;
		}
		if (Convert.ToDecimal(e.NewValue.ToString()) != Convert.ToDecimal(e.OldValue.ToString()) && _invoicesRetailList.Count > 0)
		{
			if (Convert.ToDecimal(e.OldValue) != 0m || GetDiscountUse())
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ComfirmDiscountChange, default(ReadOnlySpan<object>)), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					CalculateDiscount(ref _invoicesRetailList, Convert.ToDecimal(e.NewValue));
					CalcSummAndSummVAT();
					_invoiceRetailGrid.Grid.RefreshData();
				}
				else
				{
					_discountPercent.EditValueChanged -= _discountPercentEventHandler.Handler;
					_discountPercent.EditValue = Convert.ToDecimal(e.OldValue);
					_discountPercent.DoValidate();
					_discountPercent.EditValueChanged += _discountPercentEventHandler.Handler;
				}
			}
			else
			{
				CalculateDiscount(ref _invoicesRetailList, Convert.ToDecimal(e.NewValue));
				CalcSummAndSummVAT();
				_invoiceRetailGrid.Grid.RefreshData();
			}
		}
		else
		{
			CalculateDiscount(ref _invoicesRetailList, Convert.ToDecimal(e.NewValue));
			CalcSummAndSummVAT();
			_invoiceRetailGrid.Grid.RefreshData();
		}
	}

	private bool GetDiscountUse()
	{
		decimal num = 0m;
		foreach (MTInvoiceRetailSpecificationRow invoicesRetail in _invoicesRetailList)
		{
			num += Math.Abs(invoicesRetail.Price - invoicesRetail.DiscountPrice);
		}
		return !Convert.ToBoolean(num == 0m);
	}

	public override void OnScanner(string ScannerData)
	{
		if (!_barCode.IsReadOnly)
		{
			_barCode.Text = ScannerData;
			ScannerAdd();
		}
	}

	public override void OnMagneticCardReader(string MCRData)
	{
		try
		{
			if (!string.IsNullOrWhiteSpace(MCRData) && MCRInfo.GetInstance().IsValidCard(MCRData))
			{
				ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(MCRInfo.GetInstance().GetCardNumber(MCRData), out var partnerCacheRow, out var partnerContract, out var employee);
				FillPartnerEmployeeData(partnerCacheRow, partnerContract, employee);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void BarCode_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0 && !_barCode.IsReadOnly && !string.IsNullOrWhiteSpace(_barCode.Text))
			{
				ScannerAdd();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ScannerAdd()
	{
		_barCode.DoValidate();
		try
		{
			string barCode = _barCode.Text.Trim();
			if (Settings.ParametersService.UsePartnerEmployeeSearchNewMode())
			{
				if (ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.IsValidPartnerEmployeeCardCode(barCode, Settings.ParametersService.PartnerEmployeeCardCodePrefixes(), Settings.ParametersService.UsePartnerCardLenght(), Settings.ParametersService.PartnerCardLenght()))
				{
					_barCode.Text = string.Empty;
					_barCode.Focus();
					ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(barCode, out var partnerCacheRow, out var partnerContract, out var employee);
					FillPartnerEmployeeData(partnerCacheRow, partnerContract, employee);
				}
				else if (MCRInfo.GetInstance().IsValidCard(barCode))
				{
					_barCode.Text = "";
					_barCode.Focus();
					ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(MCRInfo.GetInstance().GetCardNumber(barCode), out var partnerCacheRow2, out var partnerContract2, out var employee2);
					FillPartnerEmployeeData(partnerCacheRow2, partnerContract2, employee2);
					return;
				}
			}
			MaterialCacheRow materialCacheRow = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.TryGetItemByBarCode(ref barCode, out var qty, out var unitCode, out var unitBrief);
			if (materialCacheRow == null)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeNotExist, barCode));
			}
			if (string.IsNullOrWhiteSpace(_storage.Code))
			{
				ControlsHelper.SetFocus(_storage);
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Storage));
			}
			if (!_storage.DoValidate())
			{
				ControlsHelper.SetFocus(_storage);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotExist, _storage.Code));
			}
			if (_tabs.SelectedIndex != _priceListTabControlIndex)
			{
				_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
			}
			AddMTToList(materialCacheRow, unitCode, unitBrief, qty);
			_barCode.Text = "";
			CalcSummAndSummVAT();
			_vatSumma.DoValidate();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddMTToList(MaterialCacheRow goodsRow, string unitCode = "", string unitBrief = "", decimal quantity = 1m)
	{
		if (_rowAddMode == "3")
		{
			AddMTToListNewRow(goodsRow, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity);
		}
		else
		{
			AddNewRowOrMergeInner(goodsRow, _rowAddMode == "2", string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity);
		}
	}

	private void AddMTToListNewRow(MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = new MTInvoiceRetailSpecificationRow(this);
		FillMTInvRGridRow(mTInvoiceRetailSpecificationRow, goodsRow, unitCode, unitBrief, quantity);
		mTInvoiceRetailSpecificationRow.RowID = _maxRowID;
		_invoicesRetailList.Add(mTInvoiceRetailSpecificationRow);
		_maxRowID++;
		int rowHandleByListIndex = _invoiceRetailGrid.Grid.GetRowHandleByListIndex(_invoicesRetailList.Count - 1);
		_invoiceRetailTableView.Grid.RefreshRow(rowHandleByListIndex);
		POWindow<MTInvoiceRetail>.SelectCellInGrid(_invoiceRetailTableView, _invoiceRetailGrid, rowHandleByListIndex, "Quantity");
		_invoiceRetailGrid.ConfigureButtons();
	}

	private void AddNewRowOrMergeInner(MaterialCacheRow goodsRow, bool forFocusedRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = null;
		if (forFocusedRow && _invoicesRetailList.Count != 0)
		{
			mTInvoiceRetailSpecificationRow = (MTInvoiceRetailSpecificationRow)_invoiceRetailTableView.Grid.CurrentItem;
			if (mTInvoiceRetailSpecificationRow == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _invoicesRetailList.Count - 1; i++)
		{
			if ((!forFocusedRow || mTInvoiceRetailSpecificationRow.RowID == _invoicesRetailList[i].RowID) && goodsRow.Id == _invoicesRetailList[i].ItemID && unitCode == _invoicesRetailList[i].UnitCode && (_invoicesRetailList[i].ItemType == GoodsTypesSE.Service.Value || (_invoicesRetailList[i].ItemType == GoodsTypesSE.Good.Value && _storage.Code == _invoicesRetailList[i].Storage && !_invoicesRetailList[i].IsPartySelected.Value)))
			{
				FillMTInvRGridRow(_invoicesRetailList[i], goodsRow, unitCode, unitBrief, quantity);
				int rowHandleByListIndex = _invoiceRetailGrid.Grid.GetRowHandleByListIndex(i);
				_invoiceRetailGrid.Grid.RefreshRow(rowHandleByListIndex);
				POWindow<MTInvoiceRetail>.SelectCellInGrid(_invoiceRetailTableView, _invoiceRetailGrid, rowHandleByListIndex, "Quantity");
				return;
			}
		}
		AddMTToListNewRow(goodsRow, unitCode, unitBrief, quantity);
	}

	private void ViewAvailAndTurn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = (MTInvoiceRetailSpecificationRow)_invoiceRetailTableView.Grid.CurrentItem;
			if (mTInvoiceRetailSpecificationRow != null)
			{
				new MTAvailAndTurnWindow(DialogWindowMode.ViewMode, this, _storage.Code, "", mTInvoiceRetailSpecificationRow.ItemID, mTInvoiceRetailSpecificationRow.ItemCode, mTInvoiceRetailSpecificationRow.UnitCode, _partnerContractContainer.PartnerCode.Code).Invoke();
			}
			else
			{
				new MTAvailAndTurnWindow(DialogWindowMode.ViewMode, this, _storage.Code, "", -1, string.Empty, string.Empty, _partnerContractContainer.PartnerCode.Code).Invoke();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void Complect_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_date.IsReadOnly)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(_storage.Code))
			{
				ControlsHelper.SetFocus(_storage);
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Storage));
			}
			if (!_storage.DoValidate())
			{
				ControlsHelper.SetFocus(_storage);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotExist, _storage.Code));
			}
			FillComplectRowsWindow<MTInvoiceRetailSpecificationRow> fillComplectRowsWindow = new FillComplectRowsWindow<MTInvoiceRetailSpecificationRow>(this, PO, _date.EditValue.Value, _storage.Code, _useDiscount, _calcDiscountAuto, _invoicesRetailList.Sum((MTInvoiceRetailSpecificationRow x) => x.Discount), _discountPercent.Value, _maxRowID, _invoicesRetailList, _invoiceRetailGrid);
			fillComplectRowsWindow.ShowDialog();
			int addedRows = fillComplectRowsWindow.AddedRows;
			if (addedRows == 0)
			{
				return;
			}
			if (!string.IsNullOrWhiteSpace(_dealTypeKey))
			{
				int i;
				for (i = _maxRowID; i < _maxRowID + addedRows; i++)
				{
					MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = (_invoiceRetailGrid.ItemsSource as IEnumerable<MTInvoiceRetailSpecificationRow>)?.FirstOrDefault((MTInvoiceRetailSpecificationRow x) => x.RowID == i);
					if (mTInvoiceRetailSpecificationRow != null)
					{
						if (_vatType.Code.InList("3", "4") || !mTInvoiceRetailSpecificationRow.VAT.Value)
						{
							mTInvoiceRetailSpecificationRow.DealType = _dealTypeKey;
						}
						else
						{
							mTInvoiceRetailSpecificationRow.DealType = string.Empty;
						}
					}
				}
			}
			_maxRowID += addedRows;
			_invoiceRetailGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void Discount_Click(object sender, RoutedEventArgs e)
	{
		if (_invoicesRetailList.Count != 0 && ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ComfirmDiscountChange, default(ReadOnlySpan<object>)), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			_discountPercent.EditValueChanged -= _discountPercentEventHandler.Handler;
			decimal num = CalculateDiscountByScale();
			if (num != -1m)
			{
				_discountPercent.Value = num;
			}
			_discountPercent.EditValueChanged += _discountPercentEventHandler.Handler;
			CalcSummAndSummVAT();
			_invoiceRetailGrid.Grid.RefreshData();
		}
	}

	private void FillMTInvRGridRow(MTInvoiceRetailSpecificationRow mtInvRGridRow, MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		if (goodsRow == null)
		{
			return;
		}
		mtInvRGridRow.ItemType = goodsRow.Type;
		mtInvRGridRow.Storage = _storage.Code;
		mtInvRGridRow.ItemCode = goodsRow.MTCode;
		mtInvRGridRow.ItemID = goodsRow.Id;
		mtInvRGridRow.ItemName = goodsRow.Caption;
		mtInvRGridRow.UnitCode = unitCode;
		mtInvRGridRow.UnitBrief = unitBrief;
		mtInvRGridRow.VAT = goodsRow.VAT;
		mtInvRGridRow.Quantity += quantity;
		if (mtInvRGridRow.Price == 0m)
		{
			if (mtInvRGridRow.ItemType == ItemTypesSE.Goods.Value)
			{
				mtInvRGridRow.IsPartySelected = false;
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtInvRGridRow.ItemID, mtInvRGridRow.UnitCode);
				if (qntUnit != null)
				{
					MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
					{
						MTCode = mtInvRGridRow.ItemCode,
						UnitCoef = qntUnit.Coef,
						Storage = _storage.Code,
						Date = _date.EditValue
					};
					List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
					if (rows.Count > 0)
					{
						mtInvRGridRow.Price = rows[0].Price;
					}
				}
			}
			else if (TradeSettings.TradeCacheService.Material.GetQntUnit(mtInvRGridRow.ItemID, mtInvRGridRow.UnitCode) != null)
			{
				mtInvRGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType.Code, mtInvRGridRow.ItemID, mtInvRGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
			}
		}
		if (_useDiscount)
		{
			mtInvRGridRow.Discount = _discountPercent.Value;
			if (mtInvRGridRow.Discount == 0m)
			{
				mtInvRGridRow.Discount = goodsRow.Discount;
			}
			else if (goodsRow.Discount != 0m)
			{
				mtInvRGridRow.Discount = goodsRow.Discount;
			}
		}
		mtInvRGridRow.DiscountPrice = (Convert.ToDecimal(mtInvRGridRow.Price) - Convert.ToDecimal(mtInvRGridRow.Price) * Convert.ToDecimal(mtInvRGridRow.Discount) / 100m).RoundAs(2);
		mtInvRGridRow.Summa = (Convert.ToDecimal(mtInvRGridRow.Quantity) * Convert.ToDecimal(mtInvRGridRow.DiscountPrice)).RoundAs(2);
		if (_useEnvFees && mtInvRGridRow.ItemType == ItemTypesSE.Goods.Value)
		{
			mtInvRGridRow.EnvFeePercent = goodsRow.EcoFeeRate;
			mtInvRGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvRGridRow.Summa, mtInvRGridRow.EnvFeePercent, mtInvRGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
		}
		if (!string.IsNullOrWhiteSpace(_dealTypeKey))
		{
			if (_vatType.Code == "3" || _vatType.Code == "4" || !goodsRow.VAT)
			{
				mtInvRGridRow.DealType = _dealTypeKey;
			}
			else
			{
				mtInvRGridRow.DealType = string.Empty;
			}
		}
	}

	private void TotalSumma_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		decimal TotalSumm = 0m;
		decimal VATSumm = 0m;
		if (!_validateTotalSumm)
		{
			return;
		}
		try
		{
			CalcInvoiceTotalAndVATSumm(ref TotalSumm, ref VATSumm);
			if (Math.Abs(Convert.ToDecimal(e.NewValue) - TotalSumm) > 100m)
			{
				e.Handled = true;
				_totalSumma.Value = Convert.ToDecimal(e.OldValue);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BigDifference, 100m));
			}
			_vatSumma.Value = VATSumm + Convert.ToDecimal(e.NewValue) - TotalSumm;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTinvoiceRetailGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_invoiceRetailGrid.Grid.SortInfo.Count > 0 || _invoiceRetailTableView.Grid.VisibleRowCount != _invoicesRetailList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = (MTInvoiceRetailSpecificationRow)_invoiceRetailGrid.Grid.GetRow(e.RowHandle);
		if (mTInvoiceRetailSpecificationRow != null)
		{
			_invoicesRetailList.RemoveAt(e.RowHandle);
			_invoicesRetailList.Insert(e.RowHandle - 1, mTInvoiceRetailSpecificationRow);
		}
		_invoiceRetailGrid.Grid.RefreshData();
	}

	private void MTinvoiceRetailGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_invoiceRetailGrid.Grid.SortInfo.Count > 0 || _invoiceRetailTableView.Grid.VisibleRowCount != _invoicesRetailList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = (MTInvoiceRetailSpecificationRow)_invoiceRetailGrid.Grid.GetRow(e.RowHandle);
		if (mTInvoiceRetailSpecificationRow != null)
		{
			_invoicesRetailList.RemoveAt(e.RowHandle);
			_invoicesRetailList.Insert(e.RowHandle + 1, mTInvoiceRetailSpecificationRow);
		}
		_invoiceRetailGrid.Grid.RefreshData();
	}

	private void MTInvoiceRetailGrid_RefreshButtons(object sender, EventArgs e)
	{
		if (_viewAvailAndTurn != null)
		{
			_viewAvailAndTurn.IsEnabled = Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepAvailability).AllowView || Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepAvailabilityTreeView).AllowView || Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations).AllowView || Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepSalesAnalysis).AllowView;
		}
	}

	private void ValidateEmptyDateAndStorage()
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		if (string.IsNullOrWhiteSpace(_storage.Code))
		{
			ControlsHelper.SetFocus(_storage);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyStorage);
		}
	}

	private void MTInvoiceRetailGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = (MTInvoiceRetailSpecificationRow)_invoiceRetailGrid.Grid.GetRow(e.RowHandle);
			if (mTInvoiceRetailSpecificationRow == null)
			{
				return;
			}
			if (e.Column == _itemCodeCol)
			{
				if (!_date.DoValidate() || !_date.EditValue.HasValue)
				{
					ControlsHelper.SetFocus(_date);
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				bool showAll = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(_storage.Code);
				ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter goodsAvailabilityFilter = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter(_date.EditValue.Value, _storage.Code, forRetail: true, includeServices: true, showAll);
				ModalBrowser modalBrowser = new ModalBrowser(goodsAvailabilityFilter)
				{
					LookUpValue = e.Value,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser.Show())
				{
					e.NewValue = goodsAvailabilityFilter.ValueMember(modalBrowser.SelectedRowIndex);
				}
			}
			else if (e.Column == _priceCol)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTInvoiceRetailSpecificationRow.ItemCode);
				if (item != null && item.Type == GoodsTypesSE.Good.Value)
				{
					MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(mTInvoiceRetailSpecificationRow.ItemCode);
					if (item2 != null)
					{
						MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(mTInvoiceRetailSpecificationRow.ItemCode, item2.Coeff, _storage.Code, _date.EditValue.Value);
						ModalBrowser modalBrowser2 = new ModalBrowser(mTRetailQtyAndPrice)
						{
							LookUpValue = mTInvoiceRetailSpecificationRow.UnitCode,
							Owner = e.Owner,
							Left = e.Left,
							Top = e.Top
						};
						if (modalBrowser2.Show())
						{
							e.NewValue = Convert.ToString(mTRetailQtyAndPrice.CommentMember(modalBrowser2.SelectedRowIndex));
						}
					}
				}
			}
			else if (e.Column == _dealTypeCol)
			{
				if (mTInvoiceRetailSpecificationRow == null || (_vatType.Code != "3" && _vatType.Code != "4" && mTInvoiceRetailSpecificationRow.VAT.Value))
				{
					return;
				}
				SmartEnum smartEnum = new SmartEnum(_vatType.Code.InList("1", "2") ? typeof(InvoiceDealTypesSE) : typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
				ModalBrowser modalBrowser3 = new ModalBrowser(smartEnum)
				{
					LookUpValue = e.Value,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser3.Show())
				{
					mTInvoiceRetailSpecificationRow.DealType = (string)smartEnum.KeyMember(modalBrowser3.SelectedRowIndex);
				}
			}
			else if (e.Column == _partyCol)
			{
				if (mTInvoiceRetailSpecificationRow.ItemType != ItemTypesSE.Goods.Value)
				{
					return;
				}
				ValidateDocFields();
				if (string.IsNullOrWhiteSpace(mTInvoiceRetailSpecificationRow.ItemCode))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
				}
				decimal quantity = (mTInvoiceRetailSpecificationRow.Quantity * ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetMTQntUnitCoeff(mTInvoiceRetailSpecificationRow.ItemCode, mTInvoiceRetailSpecificationRow.UnitCode)).RoundAs(3);
				MaterialCacheRow item3 = TradeSettings.TradeCacheService.Material.GetItem(mTInvoiceRetailSpecificationRow.ItemCode);
				MTSelectPartyWindow mTSelectPartyWindow = new MTSelectPartyWindow(_storage.Code, _date.EditValue.Value, mTInvoiceRetailSpecificationRow.ItemCode, item3.Unit, quantity, mTInvoiceRetailSpecificationRow.RowID, PO.StoredInDb && !IsDraftOrDeleted)
				{
					MTExpDocSelectedPartiesList = PO.MTExpDocSelectedPartiesList
				};
				mTSelectPartyWindow.Load();
				mTSelectPartyWindow.Owner = this;
				if (mTSelectPartyWindow.ShowDialog().Value)
				{
					PO.MTExpDocSelectedPartiesList = mTSelectPartyWindow.MTExpDocSelectedPartiesList;
					if (mTSelectPartyWindow.IsPartySelected.HasValue)
					{
						mTInvoiceRetailSpecificationRow.ItemID = item3.Id;
						mTInvoiceRetailSpecificationRow.ItemName = item3.Caption;
						mTInvoiceRetailSpecificationRow.ItemCode = mTSelectPartyWindow.MTCode;
						mTInvoiceRetailSpecificationRow.UnitCode = item3.Unit;
						mTInvoiceRetailSpecificationRow.UnitBrief = item3.UnitRow.Brief;
						mTInvoiceRetailSpecificationRow.Quantity = mTSelectPartyWindow.TotalExpQty;
						mTInvoiceRetailSpecificationRow.Summa = (mTInvoiceRetailSpecificationRow.Quantity * mTInvoiceRetailSpecificationRow.DiscountPrice).RoundAs(2);
						if (_useEnvFees)
						{
							mTInvoiceRetailSpecificationRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mTInvoiceRetailSpecificationRow.Summa, mTInvoiceRetailSpecificationRow.EnvFeePercent, mTInvoiceRetailSpecificationRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
						}
						mTInvoiceRetailSpecificationRow.IsPartySelected = mTSelectPartyWindow.IsPartySelected;
						mTInvoiceRetailSpecificationRow.ValidateWithoutResponse();
						_invoiceRetailGrid.Grid.RefreshRow(e.RowHandle);
					}
				}
			}
			_invoiceRetailGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ValidateDocFields()
	{
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		if (string.IsNullOrWhiteSpace(_storage.Code))
		{
			ControlsHelper.SetFocus(_storage);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyStorage);
		}
		if (!_storage.DoValidate())
		{
			ControlsHelper.SetFocus(_storage);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotExist, _storage.Code));
		}
	}

	private void MTinvoiceRetailGrid_DeletedRow(object sender, EventArgs e)
	{
		CalcSummAndSummVAT();
		_invoiceRetailGrid.Grid.RefreshData();
	}

	private void MTinvoiceRetailGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		MTInvoiceRetailSpecificationRow mtInvGridRow = (MTInvoiceRetailSpecificationRow)_invoiceRetailGrid.Grid.GetRow(e.RowHandle);
		if (mtInvGridRow != null)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
		}
	}

	private void MTinvoiceRetailTableView_FocusedRowHandleChangedEventArgs(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _invoiceRetailTableView.FocusedRowHandle;
		MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = (MTInvoiceRetailSpecificationRow)_invoiceRetailGrid.Grid.GetRow(focusedRowHandle);
		if (mTInvoiceRetailSpecificationRow != null)
		{
			ChangeItemCodeColumnAdditonalView(mTInvoiceRetailSpecificationRow.ItemCode);
		}
	}

	private void ChangeItemCodeColumnAdditonalView(string ItemCode)
	{
		_unitCodeCol.DataView = new MTQntUnits((!string.IsNullOrWhiteSpace(ItemCode)) ? ItemCode : string.Empty);
	}

	private void MTInvoiceRetailTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = (MTInvoiceRetailSpecificationRow)_invoiceRetailGrid.Grid.GetRow(e.RowHandle);
		if (mTInvoiceRetailSpecificationRow != null && !string.IsNullOrWhiteSpace(mTInvoiceRetailSpecificationRow.ItemCode))
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTInvoiceRetailSpecificationRow.ItemCode);
			if (item != null && e.Column == _priceCol && item.Type == ItemTypesSE.Services.Value)
			{
				e.Cancel = true;
			}
		}
	}

	private void MTinvoiceRetailGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = (MTInvoiceRetailSpecificationRow)_invoiceRetailGrid.Grid.GetRow(e.RowHandle);
		if (mTInvoiceRetailSpecificationRow != null)
		{
			MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow2 = new MTInvoiceRetailSpecificationRow(this);
			mTInvoiceRetailSpecificationRow.Clone(mTInvoiceRetailSpecificationRow2);
			mTInvoiceRetailSpecificationRow2.IsPartySelected = ((mTInvoiceRetailSpecificationRow2.ItemType == ItemTypesSE.Goods.Value) ? new bool?(false) : ((bool?)null));
			mTInvoiceRetailSpecificationRow2.RowID = _maxRowID;
			_invoicesRetailList.Add(mTInvoiceRetailSpecificationRow2);
			_maxRowID++;
			e.NewRowNumber = _invoicesRetailList.Count - 1;
			CalcSummAndSummVAT();
			CalcDiscountByScale();
		}
	}

	private bool GetEnvFeeUse()
	{
		decimal num = 0m;
		foreach (MTInvoiceRetailSpecificationRow invoicesRetail in _invoicesRetailList)
		{
			num += invoicesRetail.EnvFeeSumma;
		}
		return !Convert.ToBoolean(num == 0m);
	}

	protected override void OnF2()
	{
		if (!_barCode.IsReadOnly)
		{
			ControlsHelper.SetFocus(_barCode);
		}
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			ValidateDocFields();
			FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
			{
				FilterDataView = new GoodsGroups(),
				SearchDataView = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, ItemTypesSE.Goods),
				ResultsList = new MTInvoiceBaseFilteredSearchList<MTInvoiceRetailSpecificationRow>(_currCode.Code, _vatType.Code, _date.EditValue.Value)
			}, this, isBasketMode: true);
			if (!filteredSearchWindow.ShowDialog().Value)
			{
				return;
			}
			foreach (MTInvoiceRetailSpecificationRow item in filteredSearchWindow.BasketResult)
			{
				MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow2 = new MTInvoiceRetailSpecificationRow(this)
				{
					ItemType = item.ItemType,
					ItemID = item.ItemID,
					ItemCode = item.ItemCode,
					ItemName = item.ItemName,
					BarCode = item.BarCode,
					UnitCode = item.UnitCode,
					UnitBrief = item.UnitBrief,
					Discount = item.Discount,
					VAT = item.VAT,
					IsPartySelected = item.IsPartySelected,
					Price = item.Price,
					Quantity = item.Quantity,
					Summa = item.Summa
				};
				if (mTInvoiceRetailSpecificationRow2.ItemType == GoodsTypesSE.Good.Value)
				{
					mTInvoiceRetailSpecificationRow2.Storage = _storage.Code;
					MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mTInvoiceRetailSpecificationRow2.ItemCode, mTInvoiceRetailSpecificationRow2.UnitCode);
					if (qntUnit != null)
					{
						MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
						{
							MTCode = mTInvoiceRetailSpecificationRow2.ItemCode,
							UnitCoef = qntUnit.Coef,
							Storage = _storage.Code,
							Date = _date.EditValue
						};
						List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
						if (rows.Count > 0)
						{
							mTInvoiceRetailSpecificationRow2.Price = rows[0].Price;
						}
					}
				}
				else
				{
					mTInvoiceRetailSpecificationRow2.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType.Code, mTInvoiceRetailSpecificationRow2.ItemID, mTInvoiceRetailSpecificationRow2.UnitCode, _date.EditValue.Value, _currCode.Code);
				}
				if (!_useDiscount)
				{
					mTInvoiceRetailSpecificationRow2.Discount = 0m;
				}
				else if (mTInvoiceRetailSpecificationRow2.Discount == 0m && _discountPercent.Value != 0m)
				{
					mTInvoiceRetailSpecificationRow2.Discount = _discountPercent.Value;
				}
				mTInvoiceRetailSpecificationRow2.DiscountPrice = (mTInvoiceRetailSpecificationRow2.Price - mTInvoiceRetailSpecificationRow2.Price * mTInvoiceRetailSpecificationRow2.Discount / 100m).RoundAs(2);
				mTInvoiceRetailSpecificationRow2.Summa = (mTInvoiceRetailSpecificationRow2.DiscountPrice * mTInvoiceRetailSpecificationRow2.Quantity).RoundAs(2);
				if (_useEnvFees && mTInvoiceRetailSpecificationRow2.ItemType == GoodsTypesSE.Good.Value)
				{
					mTInvoiceRetailSpecificationRow2.EnvFeePercent = TradeSettings.TradeCacheService.Material.GetItem(mTInvoiceRetailSpecificationRow2.ItemCode)?.EcoFeeRate ?? 0m;
					mTInvoiceRetailSpecificationRow2.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mTInvoiceRetailSpecificationRow2.Summa, mTInvoiceRetailSpecificationRow2.EnvFeePercent, mTInvoiceRetailSpecificationRow2.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
				}
				if (!string.IsNullOrWhiteSpace(_dealTypeKey))
				{
					if (_vatType.Code == "3" || _vatType.Code == "4" || !Convert.ToBoolean(mTInvoiceRetailSpecificationRow2.VAT))
					{
						mTInvoiceRetailSpecificationRow2.DealType = _dealTypeKey;
					}
					else
					{
						mTInvoiceRetailSpecificationRow2.DealType = string.Empty;
					}
				}
				mTInvoiceRetailSpecificationRow2.RowID = _maxRowID;
				_invoicesRetailList.Add(mTInvoiceRetailSpecificationRow2);
				_maxRowID++;
			}
			_invoiceRetailGrid.Grid.RefreshData();
			if (_invoicesRetailList.Count > 0)
			{
				_storage.IsReadOnly = true;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ActualQuantitiesCheck_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if ((!PO.StoredInDb || IsChanged) && !SaveDocument(isAsDraft: true))
			{
				return;
			}
			ActualQuantitiesCheckWindow actualQuantitiesCheckWindow = new ActualQuantitiesCheckWindow(this, _invoicesRetailList, _date.EditValue.Value, _docNum.EditValue.ToString());
			if (actualQuantitiesCheckWindow.ShowDialog().Value)
			{
				foreach (ActualQuantitiesCheck.ActualQuantitiesCheckRow item2 in actualQuantitiesCheckWindow.QCResult)
				{
					if (item2.VarianceQuantity == 0m)
					{
						continue;
					}
					if (!item2.UserAdd)
					{
						for (int i = 0; i <= _invoicesRetailList.Count - 1; i++)
						{
							if (item2.MTID != _invoicesRetailList[i].ItemID || !(item2.UnitCode == _invoicesRetailList[i].UnitCode))
							{
								continue;
							}
							MTInvoiceRetailSpecificationRow mTInvoiceRetailSpecificationRow = _invoicesRetailList[i];
							decimal num = mTInvoiceRetailSpecificationRow.Quantity + item2.VarianceQuantity;
							TradeSettings.TradeCacheService.Material.GetItem(item2.MTCode);
							MTInvoiceRetailSpecificationRow mtInvRGridRow = (MTInvoiceRetailSpecificationRow)_invoiceRetailGrid.Grid.GetRowByListIndex(i);
							if (num < 0m)
							{
								mTInvoiceRetailSpecificationRow.Quantity = 0m;
								PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvRGridRow.RowID);
								mtInvRGridRow.IsPartySelected = ((mtInvRGridRow.ItemType != GoodsTypesSE.Good.Value) ? ((bool?)null) : new bool?(false));
								mtInvRGridRow.Summa = (mTInvoiceRetailSpecificationRow.Quantity * mtInvRGridRow.DiscountPrice).RoundAs(2);
								if (_useEnvFees && mtInvRGridRow.ItemType == InvoiceRowTypeSE.Goods.Value)
								{
									mtInvRGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvRGridRow.Summa, mtInvRGridRow.EnvFeePercent, mtInvRGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
								}
								item2.VarianceQuantity = num;
								continue;
							}
							mTInvoiceRetailSpecificationRow.Quantity = item2.ActualQuantity;
							PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvRGridRow.RowID);
							mtInvRGridRow.IsPartySelected = ((mtInvRGridRow.ItemType != GoodsTypesSE.Good.Value) ? ((bool?)null) : new bool?(false));
							mtInvRGridRow.Summa = (mTInvoiceRetailSpecificationRow.Quantity * mtInvRGridRow.DiscountPrice).RoundAs(2);
							if (_useEnvFees && mtInvRGridRow.ItemType == InvoiceRowTypeSE.Goods.Value)
							{
								mtInvRGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvRGridRow.Summa, mtInvRGridRow.EnvFeePercent, mtInvRGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
							}
							item2.VarianceQuantity = 0m;
							break;
						}
					}
					else
					{
						MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(item2.MTCode);
						AddNewRowOrMergeInner(item, forFocusedRow: false, item2.UnitCode, item2.UnitBrief, item2.VarianceQuantity);
					}
				}
				IsChanged = true;
			}
			_invoiceRetailGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void OnF8()
	{
		On_CodeEnterClick();
	}

	private void On_CodeEnterClick()
	{
		if (_codeEnter == null || !_codeEnter.IsEnabled)
		{
			return;
		}
		_barCode.DoValidate();
		try
		{
			string text = _barCode.Text.Trim();
			if (string.IsNullOrWhiteSpace(text))
			{
				if (!_date.DoValidate() || !_date.EditValue.HasValue)
				{
					ControlsHelper.SetFocus(_date);
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				ArmSoft.SME.Trade.UI.DropDown.GoodsFilter goodsFilter = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, GoodsTypesSE.Good.Value);
				ModalBrowser modalBrowser = new ModalBrowser(goodsFilter)
				{
					Owner = this,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};
				if (!modalBrowser.Show())
				{
					_barCode.Text = "";
					ControlsHelper.SetFocus(_barCode);
					return;
				}
				text = goodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
			}
			if (text.Length > 20)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrMTCodeLen, 20, text));
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
			if (item == null)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTEgineInavidMaterial, text));
			}
			AddMTToList(item);
			if (_tabs.SelectedIndex != _priceListTabControlIndex)
			{
				_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
			}
			_barCode.Text = "";
			CalcSummAndSummVAT();
			_vatSumma.DoValidate();
			ControlsHelper.SetFocus(_barCode);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void BE_CodeEnter_Click(object sender, RoutedEventArgs e)
	{
		On_CodeEnterClick();
	}

	private void FillPartnerEmployeeData(PartnerCacheRow partner, PartnerCacheRow.ContractRow contractRow, EmployeeCacheRow employee)
	{
		if (partner != null)
		{
			_partnerContractContainer.PartnerCode.Code = partner.PartCode;
			_partnerContractContainer.Contract.Code = contractRow.Contract;
			_currCode.Code = contractRow.CurrCode;
		}
		else
		{
			_salesConsultant.Code = employee.EmployeeCard;
		}
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonMTSalesAnalysis.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
		_barButtonGoodsRetailOperationsJournal.IsEnabled = isEnabled;
	}

	public void CalculateDiscount(ref ObservableCollection<MTInvoiceRetailSpecificationRow> MTList, decimal discount, bool gDiscount = true, bool calcInvFee = false, string vatType = "")
	{
		if (gDiscount)
		{
			foreach (MTInvoiceRetailSpecificationRow MT in MTList)
			{
				MT.Discount = discount;
				CalculateDiscount(MT, calcInvFee, vatType);
			}
			return;
		}
		foreach (MTInvoiceRetailSpecificationRow MT2 in MTList)
		{
			CalculateDiscount(MT2, calcInvFee, vatType);
		}
	}

	private void CalculateDiscount(MTInvoiceRetailSpecificationRow row, bool calcInvFee, string vatType)
	{
		row.DiscountPrice = (row.Price * (1m - row.Discount / 100m)).RoundAs(2);
		row.Summa = (row.Quantity * row.DiscountPrice).RoundAs(2);
		row.DiscountSumma = (row.Quantity * row.Price - row.Summa).RoundAs(2);
		if (calcInvFee)
		{
			row.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(row.Summa, row.EnvFeePercent, row.VAT.Value, vatType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
		}
	}
}
