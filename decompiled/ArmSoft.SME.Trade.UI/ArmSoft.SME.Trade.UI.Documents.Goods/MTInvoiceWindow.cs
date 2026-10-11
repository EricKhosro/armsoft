using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.BackgroundWorks;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.StandartWindow;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.CustomTypes;
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
using DevExpress.Xpf.Ribbon;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents.Goods;

[DocumentWindow(8)]
public class MTInvoiceWindow : MTInvoiceBaseWindow<MTInvoice>
{
	public class MTInvoiceSpecificationRow : MTInvoiceBaseSpecificationRow
	{
		[ObservableProperty]
		private bool markingHasMismatch;

		internal bool _isUpdatingMarking;

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
		public bool MarkingHasMismatch
		{
			get
			{
				return markingHasMismatch;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(markingHasMismatch, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MarkingHasMismatch);
					markingHasMismatch = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MarkingHasMismatch);
				}
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
					OnEnvFeeSummaChanged(value);
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
					OnEnvFeePercentChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.EnvFeePercent);
				}
			}
		}

		public MTInvoiceSpecificationRow()
		{
		}

		public MTInvoiceSpecificationRow(MTInvoiceWindow docWindow)
			: base((TradeDocumentWindow<MTInvoice>)docWindow)
		{
		}

		static MTInvoiceSpecificationRow()
		{
			Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>> invoiceBasePropertyValidators = MTInvoiceBaseSpecificationRow.GetInvoiceBasePropertyValidators();
			invoiceBasePropertyValidators.Add("Storage", (DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
			{
				ValidateStorage((MTInvoiceSpecificationRow)row, info);
			});
			if (Settings.ParametersService.UseMarking())
			{
				invoiceBasePropertyValidators.Add("MarkingStatus", (DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
				{
					ValidateMarkingListCount((MTInvoiceSpecificationRow)row, info);
				});
			}
			DocumentGridRow.RegisterPropertyValidators(typeof(MTInvoiceSpecificationRow), cacheValidationResults: true, invoiceBasePropertyValidators);
		}

		private static void ValidateStorage(MTInvoiceBaseSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (row.ItemType == GoodsTypesSE.Good.Value)
			{
				TradePropertyValidator.ValidateStorage(row.Storage, info, ArmSoft.SME.Common.Resources.Storage, isRequired: true);
			}
		}

		private static void ValidateMarkingListCount(MTInvoiceSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (row._isUpdatingMarking)
			{
				return;
			}
			row._isUpdatingMarking = true;
			row.MarkingHasMismatch = false;
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(row.ItemCode);
			if (row.IsRowMarkable && item != null && !item.IsWeight && (decimal)row.MarkingList.Count != row.Quantity)
			{
				if ((decimal)row.MarkingList.Count > row.Quantity)
				{
					info.ErrorType = ErrorType.Critical;
					info.ErrorText = ArmSoft.SME.Common.Resources.GreaterMarkingListCount;
				}
				if (item.CheckMarking)
				{
					row.MarkingHasMismatch = true;
				}
			}
			row._isUpdatingMarking = false;
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnEnvFeeSummaChanged(decimal value)
		{
			ResetForceValidationBit("EnvFeeSumma");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnEnvFeePercentChanged(decimal value)
		{
			ResetForceValidationBit("EnvFeePercent");
		}
	}

	protected delegate void RowInfoChangeEventHandler(MTInvoiceSpecificationRow row);

	private delegate void DelegateSetRowInfoToInfoPanel(MTInvoiceSpecificationRow row);

	private delegate void DelegateClearInfoPanel();

	private DropDownCommentControl _baseDocumentControl;

	private TextEditExt _barCode;

	private Button _codeEnter;

	private PartnerContractGroupControls _buyerContractContainer;

	private TextEditExt _supBookN;

	private PopupCalcEditExt _bookPage;

	private PopupCalcEditExt _bookRow;

	private DropDownControl _exportedType;

	private TextEditExt _taxInvoiceSeries;

	private TextEditExt _taxInvoiceNumber;

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

	public DropDownControl _currCode;

	private ExchangeRateControl _currExChngRate;

	public DropDownControl _vatType;

	public DropDownControl _dealType;

	private string _dealTypeKey;

	public DropDownControl _priceListType;

	public PopupCalcEditExt _discountPercent;

	public DropDownControl _storage;

	private PopupCalcEditExt _vatSumma;

	private PopupCalcEditExt _totalSumma;

	public DocumentGridControl _invoiceGrid;

	private TableView _invoiceTableView;

	private BarEditItemExt _viewAvailAndTurn;

	private BarEditItemExt _complect;

	private BarEditItemExt _discount;

	public BarEditItemExt _discountPriceType;

	private BarEditItemExt _filteredSearch;

	private BarEditItemExt _actualQuantitiesCheck;

	private BarEditItemExt _selectMarking;

	public ObservableCollection<MTInvoiceSpecificationRow> _invoicesList;

	private DocumentGridColumn _itemTypeCol;

	private DocumentGridColumn _storageCol;

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

	private DocumentGridColumn _rowMarkingListCol;

	private DocumentGridColumn _isRowMarkable;

	private bool _useMarking = Settings.ParametersService.UseMarking();

	private bool _immediatlyOpenMarkingWindow = Settings.ParametersService.OpenMarkingWindowImmediatly();

	private string _realText = "";

	private string _rowAddMode = Settings.ParametersService.ScannerMode();

	public string _discountPriceListType = string.Empty;

	public string _discountPriceListVATType = string.Empty;

	public string _discountPriceListCurr = string.Empty;

	public string _discountPriceTypeRoundDegree = string.Empty;

	public decimal _discountPriceListCurrRate = 1m;

	public decimal _discountPriceListCurrBase = 1m;

	private bool _isAskQuestionConfirmSummChange = true;

	private bool _paramUseDiscount = Settings.ParametersService.DiscountUse();

	private bool _isCalcDiscountAuto = Settings.ParametersService.CalcDiscountAuto();

	private bool _alwaysCalcDiscountAuto = Settings.ParametersService.AlwaysCalcDiscountAuto();

	private bool _useDiscountExtension = Settings.ParametersService.DiscountExtensionUse();

	public decimal _useDiscount;

	private bool _allowChangeSalePrices = Settings.ParametersService.AllowChangeSalePrices();

	private bool _allowChangeDiscountPrices = Settings.ParametersService.AllowChangeDiscountPrice();

	private bool _allowChangePriceType = Settings.ParametersService.AllowChangePriceType();

	private bool _allowChangeDiscountPriceType = Settings.ParametersService.AllowChangeDiscountPriceType();

	private DiscountsCalculator _discountsCalculator;

	private DXTabControl _tabs;

	private int _priceListTabControlIndex;

	private string _priceListTypeRoundDgr = "";

	private bool _validateTotalSumm;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	protected BarButtonItem _barButtonPrintLabel;

	protected BarButtonItem _barButtonImportFromFile;

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonMTSalesAnalysis;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	private BarButtonItem _barButtonDocumentSettings;

	private BarCheckItem _barShowDiscCalcSteps;

	private RibbonPageGroup _ribbonPageDiscounts;

	protected LayoutGroup _infoDisplay;

	protected TextEditExt _infoItemName;

	protected TextEditExt _infoFormula;

	protected InfoDisplayControls _infoDisplayControls;

	protected bool _isChanged = true;

	protected Dictionary<(int, string), decimal> _availabilityByGoodId = new Dictionary<(int, string), decimal>();

	private SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs> _focusedRowHandleChangedEvent;

	public int _maxRowID;

	private string _goodsTypes_Goods = GoodsTypesSE.Good.Value;

	public bool _forB2BDocLoad;

	private string _oldDocCurrCode;

	private decimal _oldDocCurrExChngRate = 1m;

	private decimal _oldDocCurrExChngBase = 1m;

	protected Guid _baseISN = Guid.Empty;

	protected TradeDocument _baseDocument;

	private bool _showGoodsListWIthPrices = true;

	public bool _useEnvFees = true;

	private LayoutGroup _lgDiscountLogs;

	private DocumentGridControl _discountlogsGrid;

	private TableView _discountLogsTableView;

	private BindingList<DiscountLogRow> _discountLogsList;

	private bool _showDiscCalcSteps;

	private bool _readOnlyMarkingWindow;

	private bool _anyAutoDiscountModeIsEnabled
	{
		get
		{
			if (!_isCalcDiscountAuto)
			{
				return _alwaysCalcDiscountAuto;
			}
			return true;
		}
	}

	private string DiscountPriceListType
	{
		get
		{
			return _discountPriceListType;
		}
		set
		{
			_discountPriceListType = value;
			if (string.IsNullOrWhiteSpace(_discountPriceListType))
			{
				_discountPriceTypeRoundDegree = string.Empty;
				_discountPriceListVATType = string.Empty;
				_discountPriceListCurr = string.Empty;
				_discountPriceListCurrRate = 1m;
				_discountPriceListCurrBase = 1m;
				return;
			}
			_discountPriceTypeRoundDegree = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(_discountPriceListType);
			ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_discountPriceListType, out _discountPriceListVATType, out _discountPriceListCurr);
			_discountPriceListCurrRate = 1m;
			_discountPriceListCurrBase = 1m;
			if (!(_discountPriceListCurr == _currCode.Code) && !(_discountPriceListCurr == "AMD"))
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_discountPriceListCurr, _date.EditValue.Value);
				if (exchangeRates != null)
				{
					_discountPriceListCurrRate = exchangeRates.Rate;
					_discountPriceListCurrBase = exchangeRates.Base;
				}
			}
		}
	}

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

	protected event RowInfoChangeEventHandler _rowInfoChange;

	protected override void InnerInitialize()
	{
		_discountsCalculator = new DiscountsCalculator(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice);
		_discountsCalculator.ShowMessageOnDiscount += ShowShowMessageOnDiscount;
	}

	private void ShowShowMessageOnDiscount(string message)
	{
		ArmSoft.SME.Core.UI.MessageBox.Show(message, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
	}

	public MTInvoiceWindow()
	{
	}

	public MTInvoiceWindow(MTInvoice doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		AddShowDiscCalcItemToRibbon();
		LayoutGroup container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Date_EditValueChanged, this).Handler;
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
		_baseDocumentControl.ShowClearButton = true;
		_baseDocumentControl.ShowSearchButton = true;
		_baseDocumentControl.IsTextEditable = false;
		_baseDocumentControl.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(BaseDocumentControl_ClickDropDown, this).Handler;
		_baseDocumentControl.EditKeyValueChanged += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentControl_EditKeyValueChanged, this).Handler;
		_baseDocumentControl.BtnClearClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentDelete_Click, this).Handler;
		_baseDocumentControl.BtnSearchClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentSearch_Click, this).Handler;
		LayoutGroup container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barCode = AddStringControl(container2, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 110, HorizontalAlignment.Stretch);
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		_barCode.PreviewTextInput += new SafeDocumentEventHandler<TextCompositionEventArgs>(BarCode_PreviewTextInput, this).Handler;
		_barCode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_codeEnter = AddButton(container2, string.Empty, text, string.Empty, text, isTabStop: true);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		_tabs = AddTabControl(null);
		LayoutGroup layoutGroup = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.General, "General");
		_buyerContractContainer = AddPartnerContractGroupControls(layoutGroup, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, null, null);
		_buyerContractContainer.PartnerCodeEditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(BuyerCode_EditValueChanged, this).Handler;
		_buyerContractContainer.PartnerCodeEditAdditionalValueChanged += new SafeDocumentEventHandler<EventArgs>(BuyerCode_EditAdditionalValueChanged, this).Handler;
		_buyerContractContainer.ContractCodeEditValueChanged += new SafeDocumentEventHandler<EventArgs>(ContractCode_EditValueChanged, this).Handler;
		LayoutGroup container3 = AddLayoutGroup(layoutGroup, LayoutGroupView.Group, Orientation.Horizontal);
		_supBookN = AddStringControl(container3, Definition.Fields["BookNum"]);
		_bookPage = AddNumericControl(container3, Definition.Fields["BookPage"]);
		_bookRow = AddNumericControl(container3, Definition.Fields["BookLine"]);
		_exportedType = AddSmartEnumDropDownControl(layoutGroup, Definition.Fields["ExportedType"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		LayoutGroup container4 = AddLayoutGroup(layoutGroup, LayoutGroupView.Group, Orientation.Horizontal);
		_taxInvoiceSeries = AddStringControl(container4, Definition.Fields["TaxInvoiceSeries"]);
		_taxInvoiceNumber = AddStringControl(container4, Definition.Fields["TaxInvoiceNumber"]);
		_taxInvoiceDate = AddDateControl(layoutGroup, Definition.Fields["InvoiceDate"]);
		_salesConsultant = AddDropDownControl(layoutGroup, Definition.Fields["SalesConsultant"], new Employees(isOnlySalesConsultant: true), allowAdditionButtons: true);
		List<string> captionsList = TradeSettings.TradeCacheService.StandardComments.GetCaptionsList();
		_comment = AddComboBoxControl(layoutGroup, Definition.Fields["Comment"], captionsList, HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(layoutGroup, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		LayoutGroup container5 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.ItemList, "MtList");
		_priceListTabControlIndex = _tabs.Items.Count - 1;
		LayoutGroup container6 = AddLayoutGroup(container5, LayoutGroupView.Group, Orientation.Horizontal);
		LayoutGroup container7 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Vertical);
		LayoutGroup container8 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Vertical);
		_currCode = AddDropDownControl(container7, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_currCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrCode_EditValueChanged, this).Handler;
		_discountPercent = AddNumericControl(container8, Definition.Fields["Discount"], useCurrencyFormat: false, HorizontalAlignment.Right);
		_discountPercent.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DiscountPercent_EditValueChanged, this).Handler;
		_currExChngRate = AddExchangeRateControl(container7, Definition.Fields["ExchangeRate"]);
		_currExChngRate.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrExChngRate_EditValueChanged, this).Handler;
		_vatType = AddDropDownControl(container7, Definition.Fields["VATType"], new VATTypes(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_vatType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(VATType_EditValueChanged, this).Handler;
		_dealType = AddDropDownControl(container7, Definition.Fields["DealType"], new SmartEnum(typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI));
		_dealType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DealType_EditValueChanged, this).Handler;
		_dealType.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(DealType_ClickDropDown, this).Handler;
		_priceListType = AddDropDownControl(container7, Definition.Fields["PriceListType"], new PriceTypes(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_priceListType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PriceListType_EditValueChanged, this).Handler;
		_storage = AddDropDownControl(container7, Definition.Fields["Storage"], new Storages(false), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_storage.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged, this).Handler;
		if (_forB2BDocLoad)
		{
			_storage.Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_storage.Parent).Visibility = Visibility.Collapsed;
			_barCode.Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_barCode.Parent).Visibility = Visibility.Collapsed;
			_codeEnter.Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_codeEnter.Parent).Visibility = Visibility.Collapsed;
		}
		_invoiceGrid = AddGrid(container5, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_invoiceGrid.AllowMoveUp = true;
		_invoiceGrid.AllowMoveDown = true;
		_invoiceGrid.AllowDuplicate = true;
		text = ArmSoft.SME.Common.Resources.Marking;
		_selectMarking = _invoiceGrid.AddButton("datamatrix_blue_16.svg", text, text, 136.0);
		_selectMarking.Click += new SafeDocumentEventHandler<RoutedEventArgs>(SelectMarking_Click, this).Handler;
		_invoiceGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTInvoiceGrid_AddingRow, this).Handler;
		_invoiceGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTInvoiceGrid_AddingRowWithDialog, this).Handler;
		_invoiceGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTInvoiceGrid_DuplicatingRowWithDialog, this).Handler;
		_invoiceGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(MTInvoiceGrid_EditingRowWithDialog, this).Handler;
		_invoiceGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTInvoiceGrid_ClickDropDown, this).Handler;
		_invoiceGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTInvoiceGrid_DeletedRow, this).Handler;
		_invoiceGrid.DeletingRow += new SafeDocumentEventHandler<DocumentGridDeletingRowEventArgs>(MTInvoiceGrid_DeletingRow, this).Handler;
		_invoiceGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTInvoiceGrid_DuplicatingRow, this).Handler;
		_invoiceGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTInvoiceGrid_MovingRowUp, this).Handler;
		_invoiceGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTInvoiceGrid_MovingRowDown, this).Handler;
		_invoiceGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTInvoiceGrid_RefreshButtons, this).Handler;
		_invoiceTableView = _invoiceGrid.View;
		_focusedRowHandleChangedEvent = new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTInvoiceTableView_FocusedRowHandleChanged, this);
		_invoiceTableView.FocusedRowHandleChanged += _focusedRowHandleChangedEvent.Handler;
		_invoiceTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTInvoiceTableView_CellValueChanged, this).Handler;
		_invoiceTableView.ShowingEditor += new SafeDocumentEventHandler<ShowingEditorEventArgs>(MTInvoiceTableView_ShowingEditor, this).Handler;
		_viewAvailAndTurn = _invoiceGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.AvailAndTurn, ArmSoft.SME.Trade.Common.Resources.AvailabilityAndTurn, 130.0);
		_viewAvailAndTurn.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ViewAvailAndTurn_Click, this).Handler;
		_complect = _invoiceGrid.AddButton("fill_down_16.svg", ArmSoft.SME.Common.Resources.Complect, ArmSoft.SME.Common.Resources.Complect, 90.0);
		_complect.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Complect_Click, this).Handler;
		if (_paramUseDiscount)
		{
			_discount = _invoiceGrid.AddButton("fill_down_16.svg", ArmSoft.SME.Trade.Common.Resources.GiveDiscount, ArmSoft.SME.Trade.Common.Resources.GiveDiscount, 90.0);
			_discount.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Discount_Click, this).Handler;
			if (!_anyAutoDiscountModeIsEnabled)
			{
				_discountPriceType = _invoiceGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.DiscountPriceType, ArmSoft.SME.Trade.Common.Resources.DiscountPriceType, 130.0);
				_discountPriceType.Click += new SafeDocumentEventHandler<RoutedEventArgs>(DiscountPriceType_Click, this).Handler;
			}
		}
		_filteredSearch = _invoiceGrid.AddButton(ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_actualQuantitiesCheck = _invoiceGrid.AddButton("cheked_all_16.svg", ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck, ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck, 225.0);
		_actualQuantitiesCheck.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ActualQuantitiesCheck_Click, this).Handler;
		_invoiceGrid.AddRowIndicator(35);
		_itemTypeCol = _invoiceGrid.AddSmartEnumDropDownColumn(Definition.Grids["MTList"].Columns["ItemType"]);
		_itemTypeCol.Width = 55.0;
		_storageCol = _invoiceGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Storage"]);
		_itemCodeCol = _invoiceGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["ItemCode"], new GoodsFilter(shortMode: true, "0", "", isShowOnlyInPriceList: true), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_itemCodeCol.Width = 125.0;
		if (_forB2BDocLoad)
		{
			_storageCol.Visible = false;
			_itemCodeCol.Visible = false;
		}
		_itemNameCol = _invoiceGrid.AddStringColumn(Definition.Grids["MTList"].Columns["ItemName"]);
		_itemNameCol.Width = 225.0;
		_unitCodeCol = _invoiceGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"], trimValue: false);
		_unitCol = _invoiceGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]);
		_unitCol.AllowEditing = DefaultBoolean.False;
		_quantityCol = _invoiceGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_priceCol = _invoiceGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Price"]);
		_discountCol = _invoiceGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Discount"]);
		_discountPriceCol = _invoiceGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["DiscountPrice"]);
		_summaCol = _invoiceGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Summa"], showTotal: true);
		if (_paramUseDiscount)
		{
			_summaCol.Header = ArmSoft.SME.Trade.Common.Resources.DiscountedSumm;
		}
		_envFeePercentCol = _invoiceGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["EnvFeePercent"]);
		_envFeeSummaCol = _invoiceGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["EnvFeeSumma"], showTotal: true);
		_vatCol = _invoiceGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["VAT"]);
		_vatCol.Width = 45.0;
		_dealTypeCol = _invoiceGrid.AddDropDownColumn("DealTypeCaption", ArmSoft.SME.Common.Resources.DealType, 1, trimValue: false);
		((ButtonEditSettings)_dealTypeCol.EditSettings).IsTextEditable = false;
		_rowMarkingListCol = _invoiceGrid.AddDropDownColumn("MarkingStatus", ArmSoft.SME.Common.Resources.Marking, 19, trimValue: false, GlyphKind.Regular);
		((ButtonEditSettings)_rowMarkingListCol.EditSettings).IsTextEditable = false;
		_isRowMarkable = _invoiceGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["IsRowMarkable"]);
		_isRowMarkable.ReadOnly = true;
		_isRowMarkable.TabStop = false;
		_isRowMarkable.AllowFocus = false;
		_isRowMarkable.Visible = false;
		if (!_useMarking)
		{
			_rowMarkingListCol.Visible = false;
			_rowMarkingListCol.ShowInColumnChooser = false;
			_selectMarking.IsVisible = false;
		}
		_partyCol = _invoiceGrid.AddDropDownColumn("PartyMethodName", ArmSoft.SME.Common.Resources.Party, 19, trimValue: false, GlyphKind.Regular);
		((ButtonEditSettings)_partyCol.EditSettings).IsTextEditable = false;
		_rowDescriptionCol = _invoiceGrid.AddStringColumn(Definition.Grids["MTList"].Columns["RowDescription"]);
		foreach (FormatConditionBase item in ConditionalFormattingHelper.ConvertConditions(CreateFormatingList()))
		{
			_invoiceGrid.View.AddFormatCondition(item);
		}
		foreach (GridColumn column in _invoiceGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		if (!_paramUseDiscount)
		{
			if (!_allowChangeSalePrices)
			{
				setNonEditable(_priceCol);
				setNonEditable(_summaCol);
			}
		}
		else if (!_allowChangeSalePrices)
		{
			setNonEditable(_priceCol);
			if (!_allowChangeDiscountPrices)
			{
				setNonEditable(_discountCol);
				setNonEditable(_discountPriceCol);
				setNonEditable(_summaCol);
			}
		}
		else if (!_allowChangeDiscountPrices)
		{
			setNonEditable(_discountCol);
			setNonEditable(_discountPriceCol);
			setNonEditable(_summaCol);
		}
		_invoicesList = new ObservableCollection<MTInvoiceSpecificationRow>();
		_infoDisplayControls = new InfoDisplayControls(this, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice, isShowItemPictureOnInfoPanel: false, isShowStorageAvailabilityOnInfoPanel: true, isShowWholeAvailabilityOnInfoPanel: true, isShowPriceAvailabilityOnInfoPanel: false, showDescriptionOnInfoPanel: false, enableSubstituteItems: false, 24.0);
		_infoDisplayControls.ConstructUI(container5);
		_infoDisplay = _infoDisplayControls.InfoDisplay;
		_infoItemName = _infoDisplayControls.InfoItemName;
		_infoFormula = _infoDisplayControls.InfoFormula;
		_infoDisplay.Collapsed += new SafeDocumentEventHandler<EventArgs>(InfoDisplay_Collapsed, this).Handler;
		_vatSumma = AddNumericControl(container5, Definition.Fields["SummVAT"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_totalSumma = AddNumericControl(container5, Definition.Fields["Summ"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_totalSumma.IsRequired = !Settings.ParametersService.AllowUse100PercentSale();
		_totalSumma.EditValueChanging += new SafeDocumentEventHandler<EditValueChangingEventArgs>(TotalSumma_EditValueChanged, this).Handler;
		_buyerContractContainer.DocSummaControl = _totalSumma;
		_buyerContractContainer.DocCurrCodeControl = _currCode;
		_lgDiscountLogs = AddTabbedGroup(_tabs, ArmSoft.SME.Trade.Common.Resources.DiscountLogsJournal, "DiscountLogs");
		_discountlogsGrid = AddGrid(_lgDiscountLogs, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_discountlogsGrid.Name = "DiscLogsGrid";
		_discountlogsGrid.AllowAdd = false;
		_discountlogsGrid.AllowDelete = false;
		_discountlogsGrid.AllowDuplicate = false;
		_discountlogsGrid.ShowButtons = false;
		_discountlogsGrid.AllowEdit = false;
		_discountlogsGrid.IsVisibleViewEditButton = false;
		_discountlogsGrid.ToolTip = ArmSoft.SME.Trade.Common.Resources.DiscountLogsJournal;
		_discountlogsGrid.AddNumberColumn("RowNumber", ArmSoft.SME.Trade.Common.Resources.RowNumber, 5, 0);
		_discountlogsGrid.AddStringColumn("GoodCode", ArmSoft.SME.Common.Resources.Code, 20).Width = 125.0;
		_discountlogsGrid.AddStringColumn("GoodCaption", ArmSoft.SME.Common.Resources.Name, 200).Width = 225.0;
		_discountlogsGrid.AddStringColumn("DiscountGroupCode", ArmSoft.SME.Trade.Common.Resources.DiscountGroup, 5);
		_discountlogsGrid.AddNumberColumn("DiscountGroupPriority", ArmSoft.SME.Trade.Common.Resources.Priority, 3, 0, allowNegativeNumbers: false, showTotal: false, useCurrencyFormat: false);
		_discountlogsGrid.AddStringColumn("GroupMergeMethodCaption", ArmSoft.SME.Trade.Common.Resources.DiscountsMergeMethod, 200).Width = 225.0;
		_discountlogsGrid.AddStringColumn("DiscountCode", ArmSoft.SME.Trade.Common.Resources.Discount, 5);
		_discountlogsGrid.AddStringColumn("DiscountCaption", ArmSoft.SME.Trade.Common.Resources.DiscountCaption, 5);
		_discountlogsGrid.AddNumberColumn("DiscountPriority", ArmSoft.SME.Trade.Common.Resources.Priority, 3, 0, allowNegativeNumbers: false, showTotal: false, useCurrencyFormat: false);
		_discountlogsGrid.AddNumberColumn("DiscountedSum", ArmSoft.SME.Trade.Common.Resources.DiscountSumm, 16, 2);
		_discountlogsGrid.AddNumberColumn("Percent", ArmSoft.SME.Common.Resources.DiscountPercent, 6, 2, allowNegativeNumbers: true);
		_discountLogsList = new BindingList<DiscountLogRow>();
		_discountlogsGrid.ItemsSource = _discountLogsList;
		_discountlogsGrid.View.AutoWidth = true;
		_discountlogsGrid.IsRequired = false;
		_discountLogsTableView = _discountlogsGrid.View;
		_discountLogsTableView.AllowMoveColumnToDropArea = false;
		_discountLogsTableView.IsColumnMenuEnabled = false;
		_discountLogsTableView.AllowSorting = false;
		_discountLogsTableView.AllowEditing = false;
		if (_showDiscCalcSteps)
		{
			(_lgDiscountLogs.Parent as DXTabItem).Visibility = Visibility.Visible;
		}
		else
		{
			(_lgDiscountLogs.Parent as DXTabItem).Visibility = Visibility.Collapsed;
		}
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
		_buyerIDDocType = AddSmartEnumDropDownControl(container10, Definition.Fields["BuyerIDDocType"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_buyerPassport = AddStringControl(container10, Definition.Fields["BuyerPassport"], HorizontalAlignment.Stretch);
		_buyerAdditionalData = AddComboBoxControl(container10, Definition.Fields["BuyerAdditionalData"], captionsList, HorizontalAlignment.Stretch);
		_buyerShippingMethod = AddComboBoxControl(container10, Definition.Fields["BuyerShippingMethod"], captionsList, HorizontalAlignment.Stretch);
		LayoutGroup layoutGroup2 = AddLayoutGroup(container10, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Common.Resources.Additional);
		IEnumerable<string> itemsSource3 = from row in TradeSettings.TradeCacheService.Partner.GetList()
			select row.Caption;
		_byWhomFio = AddComboBoxControl(layoutGroup2, Definition.Fields["ByWhomFio"], itemsSource3, HorizontalAlignment.Stretch);
		_authorizationNumber = AddStringControl(layoutGroup2, Definition.Fields["AuthorizationNumber"], HorizontalAlignment.Stretch);
		_authorizationDate = AddDateControl(layoutGroup2, Definition.Fields["AuthorizationDate"]);
		layoutGroup2.IsCollapsible = true;
		layoutGroup2.IsCollapsed = true;
		_tabs.SelectedContainer = (DXTabItem)(object)layoutGroup.Parent;
		_infoDisplay.IsCollapsed = _infoDisplayControls.InfoPanelCollapsed;
		_rowInfoChange += ChangeRowInfo;
		static void setNonEditable(DocumentGridColumn control)
		{
			control.AllowEditing = DefaultBoolean.False;
			control.AllowFocus = false;
		}
	}

	private List<AsFormatting> CreateFormatingList()
	{
		List<AsFormatting> list = new List<AsFormatting>();
		if (_useMarking)
		{
			UserDefinedFormat customFormat = new UserDefinedFormat
			{
				IconUri = "datamatrix_blue_16.svg"
			};
			AsCondititionFormat item = new AsCondititionFormat
			{
				Expression = "[Quantity] <> 0.0m And [IsRowMarkable] = True",
				FieldName = "Quantity",
				FormatName = "Custom",
				ApplyToRow = false,
				CustomFormat = customFormat,
				IsFixed = true
			};
			list.Add(item);
			customFormat = new UserDefinedFormat
			{
				IconUri = "datamatrix_red_16.svg"
			};
			item = new AsCondititionFormat
			{
				Expression = "[MarkingHasMismatch] = True",
				FieldName = "Quantity",
				FormatName = "Custom",
				ApplyToRow = false,
				CustomFormat = customFormat,
				IsFixed = true
			};
			list.Add(item);
			customFormat = (Settings.IsDarkTheme ? new UserDefinedFormat
			{
				ForegroundColor = "#FF6666",
				BackgroundColor = "#663333"
			} : new UserDefinedFormat
			{
				ForegroundColor = "#E50000",
				BackgroundColor = "#FFCCCC"
			});
			item = new AsCondititionFormat
			{
				Expression = "[MarkingHasMismatch] = True",
				FieldName = "MarkingStatus",
				FormatName = "Custom",
				ApplyToRow = false,
				CustomFormat = customFormat,
				IsFixed = true
			};
			list.Add(item);
		}
		return list;
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

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.BaseISN = BaseISN;
		PO.DiscountPriceListType = DiscountPriceListType;
		PO.MTList.Clear();
		foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
		{
			MTInvoiceBase.MTInvoiceSpecificationRow item = new MTInvoiceBase.MTInvoiceSpecificationRow
			{
				RowID = invoices.RowID,
				RowNum = invoices.RowNum,
				ItemID = invoices.ItemID,
				ItemCode = invoices.ItemCode,
				ItemType = invoices.ItemType,
				IsStorageRequired = invoices.IsStorageRequired,
				Storage = invoices.Storage,
				ItemName = invoices.ItemName,
				UnitCode = invoices.UnitCode,
				UnitBrief = invoices.UnitBrief,
				Quantity = invoices.Quantity,
				Price = invoices.Price,
				Discount = invoices.Discount,
				DiscountPrice = invoices.DiscountPrice,
				UsedBonus = invoices.UsedBonus,
				AccumulatedBonus = invoices.AccumulatedBonus,
				Summa = invoices.Summa,
				DiscountSumma = invoices.DiscountSumma,
				BeginSumma = invoices.BeginSumma,
				BarCode = invoices.BarCode,
				EnvFeePercent = invoices.EnvFeePercent,
				EnvFeeSumma = invoices.EnvFeeSumma,
				VAT = invoices.VAT,
				DealType = invoices.DealType,
				IsPartySelected = invoices.IsPartySelected,
				RowDescription = invoices.RowDescription,
				IsRowMarkable = invoices.IsRowMarkable,
				MarkingStatus = invoices.MarkingStatus,
				MarkingList = invoices.MarkingList,
				IsDeleted = invoices.IsDeleted,
				IsGift = invoices.IsGift,
				RowISN = invoices.RowISN
			};
			PO.MTList.Add(item);
		}
		PO.MaxRowID = _maxRowID;
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
		_discountPriceListType = PO.DiscountPriceListType;
		_invoicesList.Clear();
		_useDiscount = 0m;
		_focusedRowHandleChangedEvent.HandleOnLoadingDocumentToControls = true;
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT in PO.MTList)
		{
			MTInvoiceSpecificationRow mTInvoiceSpecificationRow = new MTInvoiceSpecificationRow(this)
			{
				RowID = mT.RowID,
				RowNum = mT.RowNum,
				ItemID = mT.ItemID,
				ItemCode = mT.ItemCode,
				ItemType = mT.ItemType,
				IsStorageRequired = mT.IsStorageRequired,
				Storage = mT.Storage,
				ItemName = mT.ItemName,
				UnitCode = mT.UnitCode,
				UnitBrief = mT.UnitBrief,
				Quantity = mT.Quantity,
				Price = mT.Price,
				Discount = mT.Discount,
				DiscountPrice = mT.DiscountPrice,
				UsedBonus = mT.UsedBonus,
				AccumulatedBonus = mT.AccumulatedBonus,
				Summa = mT.Summa,
				DiscountSumma = mT.DiscountSumma,
				BeginSumma = mT.BeginSumma,
				BarCode = mT.BarCode,
				EnvFeePercent = mT.EnvFeePercent,
				EnvFeeSumma = mT.EnvFeeSumma,
				VAT = mT.VAT.Value,
				DealType = mT.DealType,
				IsPartySelected = mT.IsPartySelected,
				IsDeleted = mT.IsDeleted,
				IsGift = mT.IsGift,
				IsRowMarkable = mT.IsRowMarkable,
				MarkingStatus = mT.MarkingStatus,
				RowDescription = mT.RowDescription,
				RowISN = mT.RowISN
			};
			if (mT.MarkingList != null && _viewMode != WindowViewMode.Copy)
			{
				foreach (string marking in mT.MarkingList)
				{
					mTInvoiceSpecificationRow.MarkingList.Add(marking);
				}
			}
			ChangeMarkingStatus(mTInvoiceSpecificationRow);
			_invoicesList.Add(mTInvoiceSpecificationRow);
			_useDiscount += Math.Abs(mTInvoiceSpecificationRow.Discount);
		}
		_invoiceGrid.ItemsSource = _invoicesList;
		_maxRowID = PO.MaxRowID;
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
		{
			_priceCol.Header = ArmSoft.SME.Common.Resources.Price;
			_discountPriceCol.Header = ArmSoft.SME.Common.Resources.DiscountedPrice;
			_summaCol.Header = (_paramUseDiscount ? ArmSoft.SME.Trade.Common.Resources.DiscountedSumm : ArmSoft.SME.Common.Resources.Summa);
			_currExChngRate.Rate = 1m;
			_currExChngRate.Base = 1m;
		}
		else
		{
			_priceCol.Header = ArmSoft.SME.Common.Resources.FCPrice;
			_discountPriceCol.Header = ArmSoft.SME.Trade.Common.Resources.FCDiscPrice;
			_summaCol.Header = ArmSoft.SME.Common.Resources.FCSumma;
		}
		if (!_paramUseDiscount && _useDiscount == 0m)
		{
			((LayoutItemWithRequired)(object)_discountPercent.Parent).Visibility = Visibility.Collapsed;
			_discountCol.Visible = false;
			_discountCol.ShowInColumnChooser = false;
			_discountPriceCol.Visible = false;
			_discountPriceCol.ShowInColumnChooser = false;
		}
		if (_anyAutoDiscountModeIsEnabled)
		{
			_discountPercent.IsReadOnly = true;
			_discountCol.ReadOnly = true;
			_discountPriceCol.ReadOnly = true;
			_summaCol.ReadOnly = _paramUseDiscount;
		}
		else
		{
			_discountPercent.IsReadOnly = !_allowChangeDiscountPrices;
		}
		_useEnvFees = Settings.ParametersService.EnvironmentalFeePayer() || GetEnvFeeUse();
		if (!_useEnvFees)
		{
			_envFeePercentCol.Visible = false;
			_envFeeSummaCol.Visible = false;
			_envFeePercentCol.ShowInColumnChooser = false;
			_envFeeSummaCol.ShowInColumnChooser = false;
		}
		if (_useMarking || GetMarkingUse())
		{
			_rowMarkingListCol.Visible = true;
			_rowMarkingListCol.ShowInColumnChooser = true;
			_selectMarking.IsVisible = true;
		}
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
		_buyerContractContainer.ValueChangedForLoadDoc();
		CalculatePartSumma();
		BindPartnerDataViews();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString().PadLeft(2, '0'));
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
	}

	private bool GetMarkingUse()
	{
		return _invoicesList.Any((MTInvoiceSpecificationRow x) => x.MarkingList.Count != 0);
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		if (_viewMode != WindowViewMode.Copy)
		{
			StoragesCacheRow storagesCacheRow = null;
			if (BaseDocument == null && string.IsNullOrWhiteSpace(_storage.Code))
			{
				string text = Settings.ParametersService.DefaultStorage();
				if (!string.IsNullOrWhiteSpace(text))
				{
					storagesCacheRow = TradeSettings.TradeCacheService.Storage.GetItem(text);
					if (storagesCacheRow != null && !storagesCacheRow.Retail && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
					{
						_storage.Code = text;
					}
				}
			}
			else
			{
				storagesCacheRow = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
			}
			if (_baseISN == Guid.Empty)
			{
				string text2 = Settings.ParametersService.DefaultCustomer();
				if (!string.IsNullOrWhiteSpace(text2))
				{
					PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text2);
					if (item != null && item.Customer)
					{
						_buyerContractContainer.PartnerCode.Code = text2;
					}
				}
			}
			_salesConsultant.Code = Settings.ParametersService.DefaultSalesConsultant();
			if (_baseDocument == null)
			{
				_priceListType.Code = Settings.ParametersService.DefaultSalePrice();
			}
			DiscountPriceListType = Settings.ParametersService.DefaultDiscountPrice();
			if (BaseDocument == null && !string.IsNullOrWhiteSpace(Settings.ParametersService.VATType()))
			{
				_vatType.Code = Settings.ParametersService.VATType();
			}
			FillSupplierRekvizits();
			if (storagesCacheRow != null && !string.IsNullOrWhiteSpace(storagesCacheRow.Address))
			{
				_supplierBusinessAddress.Text = storagesCacheRow.Address;
			}
			if (Settings.ParametersService.VATType().InList("1", "2") == _vatType.Code.InList("1", "2"))
			{
				_dealType.Code = Settings.ParametersService.DealType();
			}
		}
		if (PO.DocDate == Settings.HostingEnvironment.CurrentDate)
		{
			CurrCodeValueChanged();
		}
		_priceListType.IsReadOnly = !_allowChangePriceType;
	}

	protected override bool AllowNewAfterSave()
	{
		return !_forB2BDocLoad;
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_taxInvoiceSeries.Text = "";
		_taxInvoiceNumber.Text = "";
		_taxInvoiceDate.EditValue = null;
		_invoicesList.Clear();
		_maxRowID = 0;
		CalcSummAndSummVAT();
		_owner.Code = Settings.LoginService.UserName;
		_discountLogsList.Clear();
		_priceListType.IsReadOnly = !_allowChangeSalePrices;
		_buyerContractContainer.CalculatePartDebtRem();
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode == WindowViewMode.View)
		{
			return;
		}
		_vatSumma.IsReadOnly = true;
		_totalSumma.IsReadOnly = true;
		_supplierName.IsReadOnly = true;
		_supplierLegalAddress.IsReadOnly = true;
		_supplierTaxCode.IsReadOnly = true;
		bool flag = State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value || State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		bool flag2 = (_readOnlyMarkingWindow = _viewMode == WindowViewMode.Edit && !flag);
		_date.IsReadOnly = flag2;
		_storage.IsReadOnly = flag2;
		_barCode.IsReadOnly = flag2;
		_codeEnter.IsEnabled = !flag2;
		_currCode.IsReadOnly = flag2;
		_currExChngRate.IsReadOnly = flag2 || _currCode.Code == "AMD";
		_buyerContractContainer.IsReadOnly = flag2;
		_baseDocumentControl.IsReadOnly = flag2;
		if (_anyAutoDiscountModeIsEnabled)
		{
			_discountPercent.IsReadOnly = true;
		}
		else
		{
			_discountPercent.IsReadOnly = flag2 || !_allowChangeDiscountPrices;
		}
		_vatType.IsReadOnly = flag2;
		_dealType.IsReadOnly = flag2;
		if (flag2)
		{
			_priceListType.IsReadOnly = true;
		}
		else
		{
			_priceListType.IsReadOnly = !_allowChangePriceType;
		}
		foreach (GridColumn column in _invoiceGrid.Grid.Columns)
		{
			if (((object)column).Equals((object?)_discountCol))
			{
				if (_anyAutoDiscountModeIsEnabled)
				{
					column.ReadOnly = true;
				}
				else
				{
					column.ReadOnly = flag2;
				}
			}
			else
			{
				column.ReadOnly = flag2;
			}
		}
		_invoiceGrid.AllowAdd = !flag2;
		_invoiceGrid.AllowEdit = !flag2;
		_invoiceGrid.AllowDelete = !flag2;
		_invoiceGrid.AllowDuplicate = !flag2;
		_invoiceGrid.AllowMoveUp = !flag2;
		_invoiceGrid.AllowMoveDown = !flag2;
		_partyCol.ReadOnly = false;
		((ButtonEditSettings)_partyCol.EditSettings).IsTextEditable = false;
		_rowMarkingListCol.ReadOnly = false;
		((ButtonEditSettings)_rowMarkingListCol.EditSettings).IsTextEditable = false;
	}

	private void MTInvoiceGrid_RefreshButtons(object sender, EventArgs e)
	{
		_complect.IsEnabled = _invoiceGrid.AllowAdd && !_invoiceGrid.IsReadOnly;
		_filteredSearch.IsEnabled = _invoiceGrid.AllowAdd && !_invoiceGrid.IsReadOnly;
		_actualQuantitiesCheck.IsEnabled = _invoiceGrid.AllowAdd && !_invoiceGrid.IsReadOnly;
		if (_paramUseDiscount)
		{
			_discount.IsEnabled = _invoiceGrid.AllowAdd && !_invoiceGrid.IsReadOnly;
			if (!_anyAutoDiscountModeIsEnabled)
			{
				_discountPriceType.IsEnabled = _discount.IsEnabled;
			}
		}
	}

	protected override bool BeforeStore(StoreRequest sr)
	{
		if (sr.State != (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft && !string.IsNullOrWhiteSpace(_buyerContractContainer.PartnerCode.Code) && !string.IsNullOrWhiteSpace(_buyerContractContainer.Contract.Code))
		{
			PO.IsCheckLimitAnswerYes = false;
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_buyerContractContainer.PartnerCode.Code);
			int intPartID = -1;
			PartnerCacheRow.ContractRow contractRow = null;
			if (item != null)
			{
				intPartID = item.Id;
				contractRow = TradeSettings.TradeCacheService.Partner.GetContract(item, _buyerContractContainer.Contract.Code);
			}
			if (contractRow != null && contractRow.ControlReceivables)
			{
				if ((TradeSettings.CreateTradeClient().CommonServices.GetCurrentPartnerRemByContract(intPartID, contractRow.Contract, "", PO.ISN).SummCurr + Convert.ToDecimal(_buyerContractContainer.SummaPart.EditValue)).RoundAs(2) > contractRow.ReceivablesLimit)
				{
					if (!Settings.ParametersService.AllowExceedDebtLimit())
					{
						ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ReceivablesExceedDenied, contractRow.Contract), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
						return false;
					}
					if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ReceivablesConfirmExceed, contractRow.Contract), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
					{
						return false;
					}
					PO.IsCheckLimitAnswerYes = true;
				}
			}
			else
			{
				PO.IsCheckLimitAnswerYes = true;
			}
		}
		if (!PO.StoredInDb || PO.State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft || PO.State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
		{
			CalculateDiscountAuto();
			FillDocument(sr);
		}
		return true;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
	}

	private void CalcSummAndSummVAT()
	{
		decimal num = 0m;
		decimal num2 = 0m;
		_validateTotalSumm = false;
		if (_vatType.Code.Trim().InList("1", "2"))
		{
			foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
			{
				if (invoices.Summa != 0m && invoices.VAT.Value)
				{
					num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(invoices.Summa, _vatType.Code, invoices.VAT.Value);
				}
				num2 += invoices.Summa;
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
			foreach (MTInvoiceSpecificationRow invoices2 in _invoicesList)
			{
				num2 += invoices2.Summa;
			}
			_vatSumma.EditValue = 0m;
			_totalSumma.EditValue = num2;
		}
		CalculatePartSumma();
		_validateTotalSumm = true;
	}

	private void CalcInvoiceTotalAndVATSumm(ref decimal totalSumm, ref decimal vatSumm)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
		{
			num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(invoices.Summa, _vatType.Code, invoices.VAT.Value);
			num2 += invoices.Summa;
		}
		if (_vatType.Code.Trim().InList("1", "2"))
		{
			vatSumm = num;
			totalSumm = num2;
			if (_vatType.Code.Trim() == "1")
			{
				totalSumm += vatSumm;
			}
		}
		else
		{
			vatSumm = 0m;
			totalSumm = num2;
		}
	}

	private void CalculateDiscountAuto()
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (_invoicesList.Count == 0 || _useDiscountExtension || !_anyAutoDiscountModeIsEnabled)
			{
				return;
			}
			AutoCalculateDiscount();
			_discountLogsList.Clear();
			foreach (DiscountLogRow discountLogs in PO.DiscountLogsList)
			{
				_discountLogsList.Add(discountLogs);
			}
			CalcSummAndSummVAT();
			_invoiceGrid.Grid.RefreshData();
			_rowInfoChange?.Invoke((MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem);
			ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
			{
				Caption = Caption,
				HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification),
				Image = ImageService.GetImage("accept_32.svg"),
				Text = string.Format(ArmSoft.SME.Common.Resources.DiscountCalculated, default(ReadOnlySpan<object>))
			};
			toastNotificationViewModel.HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification);
			toastNotificationViewModel.HistoryItem.Description = toastNotificationViewModel.Text;
			toastNotificationViewModel.HistoryItem.Details = toastNotificationViewModel.Text;
			toastNotificationViewModel.HistoryItem.Icon = "accept_16.svg";
			toastNotificationViewModel.HistoryItem.Id = "Discount";
			Settings.HostingEnvironment.ShowNotificationAsync(toastNotificationViewModel);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void BuyerCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		BuyerCodeEditValueChanged();
		CalculatePartSumma();
	}

	private void BuyerCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		BuyerCodeEditValueChanged();
		CalculatePartSumma();
	}

	private void ContractCode_EditValueChanged(object sender, EventArgs e)
	{
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_buyerContractContainer.PartnerCode.Code);
		if (item != null)
		{
			ContractCodeEditValueChanged(item.Id);
		}
		CalculatePartSumma();
	}

	private void BuyerCodeEditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_buyerContractContainer.PartnerCode.Code) && _buyerContractContainer.PartnerCode.DoValidate())
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_buyerContractContainer.PartnerCode.Code, throwExceptionNotExits: true);
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
			return;
		}
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
		_invoiceGrid.Grid.RefreshData();
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		if (_invoicesList.Count == 0 || (_paramUseDiscount && _anyAutoDiscountModeIsEnabled))
		{
			_discountPercent.EditValue = 0m;
		}
		BindPartnerDataViews();
	}

	private void BindPartnerDataViews(PartnerCacheRow partnerCacheRow = null)
	{
		if (!string.IsNullOrWhiteSpace(_buyerContractContainer.PartnerCode.Code) && _buyerContractContainer.PartnerCode.DoValidate())
		{
			if (partnerCacheRow == null)
			{
				partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(_buyerContractContainer.PartnerCode.Code, throwExceptionNotExits: true);
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

	private void ContractCodeEditValueChanged(int partnerID)
	{
		if (string.IsNullOrWhiteSpace(_buyerContractContainer.Contract.Code) || !_buyerContractContainer.Contract.DoValidate())
		{
			return;
		}
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(partnerID, _buyerContractContainer.Contract.Code);
		if (contract == null)
		{
			return;
		}
		if (_paramUseDiscount)
		{
			_discountPercent.EditValue = contract.Percent;
		}
		if (_invoicesList.Count == 0 && !string.IsNullOrWhiteSpace(contract.PriceType))
		{
			_priceListType.Code = contract.PriceType;
			if (ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_priceListType.Code, out var priceListRowVATType, out var _))
			{
				_vatType.Code = priceListRowVATType;
			}
			_currCode.Code = contract.CurrCode;
		}
		_invoiceGrid.Grid.RefreshData();
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = true;
		bool flag2 = false;
		if (!string.IsNullOrWhiteSpace((string)e.NewValue))
		{
			if (!PO.StoredInDb)
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem((string)e.NewValue);
				if (item != null)
				{
					if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable((string)e.NewValue))
					{
						e.Handled = true;
						_storage.Code = (string)e.OldValue;
						ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, e.NewValue), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					}
					else if (!string.IsNullOrWhiteSpace(item.Address))
					{
						_supplierBusinessAddress.Text = item.Address;
					}
				}
			}
			if ((from row in _invoicesList
				where (row.ItemType == _goodsTypes_Goods) & (row.Storage != "") & (row.Storage != _storage.Code)
				select row.Storage).Any())
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ChangeStorageValues, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
				{
					flag = false;
					_storage.Code = (string)e.OldValue;
				}
				else
				{
					foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
					{
						if (invoices.ItemType == _goodsTypes_Goods && !invoices.IsPartySelected == false)
						{
							ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.PartiesValuesChanged, ArmSoft.SME.Common.Resources.Storage), Caption, MessageBoxButton.OK, MessageBoxImage.Asterisk);
							flag2 = true;
							break;
						}
					}
				}
			}
			if (flag)
			{
				foreach (MTInvoiceSpecificationRow invoices2 in _invoicesList)
				{
					if (invoices2.ItemType == _goodsTypes_Goods)
					{
						invoices2.Storage = _storage.Code;
						if (flag2)
						{
							invoices2.IsPartySelected = false;
						}
					}
				}
				PO.MTExpDocSelectedPartiesList.Clear();
			}
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			_invoiceGrid.Grid.RefreshData();
			_priceListTypeRoundDgr = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(_priceListType.Code);
		}
		_rowInfoChange?.Invoke((MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void VATType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		VATType_EditValueChangedForLoad();
		VATTypeValidate((string)e.OldValue, _vatType.Code);
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		_dealType.DataView = new SmartEnum((!_vatType.Code.InList("1", "2")) ? typeof(AccDealTypesSE) : typeof(InvoiceDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
		if (((e.OldValue.ToString().Trim() == "1" || e.OldValue.ToString().Trim() == "2") && (e.NewValue.ToString().Trim() == "3" || e.NewValue.ToString().Trim() == "4")) || ((e.OldValue.ToString().Trim() == "3" || e.OldValue.ToString().Trim() == "4") && (e.NewValue.ToString().Trim() == "1" || e.NewValue.ToString().Trim() == "2")))
		{
			_dealType.Code = string.Empty;
			_dealTypeKey = string.Empty;
			foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
			{
				invoices.DealType = string.Empty;
			}
		}
		_invoiceGrid.Grid.RefreshData();
		_rowInfoChange?.Invoke((MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void VATType_EditValueChangedForLoad()
	{
		bool flag = _vatType.Code.InList("1", "2");
		_vatCol.Visible = flag;
		_vatCol.ShowInColumnChooser = flag;
		if (!flag)
		{
			_vatSumma.EditValue = 0m;
		}
		_dealType.DataView = new SmartEnum(_vatType.Code.InList("1", "2") ? typeof(InvoiceDealTypesSE) : typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
	}

	private void DealType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_dealType.Code))
		{
			SmartEnumDataProvider.DataRow dataRow = (SmartEnumDataProvider.DataRow)_dealType.DataView.GetRowByValue(_dealType.Code);
			if (dataRow != null)
			{
				_dealTypeKey = dataRow.Code;
				foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
				{
					if (_vatType.Code.InList("3", "4") || !invoices.VAT.Value)
					{
						invoices.DealType = dataRow.Code;
					}
					else
					{
						invoices.DealType = string.Empty;
					}
				}
			}
		}
		else
		{
			_dealTypeKey = string.Empty;
			foreach (MTInvoiceSpecificationRow invoices2 in _invoicesList)
			{
				invoices2.DealType = string.Empty;
			}
		}
		_invoiceGrid.Grid.RefreshData();
	}

	private void DealType_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		_dealType.DataView = new SmartEnum(_vatType.Code.InList("1", "2") ? typeof(InvoiceDealTypesSE) : typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
	}

	private void CurrCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = false;
		if (_isAskQuestionConfirmSummChange && _totalSumma.Value != 0m && !string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate() && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			flag = true;
		}
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
		{
			_currExChngRate.IsReadOnly = true;
			_priceCol.Header = ArmSoft.SME.Common.Resources.Price;
			_discountPriceCol.Header = ArmSoft.SME.Common.Resources.DiscountedPrice;
			_summaCol.Header = (_paramUseDiscount ? ArmSoft.SME.Trade.Common.Resources.DiscountedSumm : ArmSoft.SME.Common.Resources.Summa);
			_currExChngRate.Rate = 1m;
			_currExChngRate.Base = 1m;
		}
		else
		{
			_currExChngRate.IsReadOnly = PO.StoredInDb && State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
			_priceCol.Header = ArmSoft.SME.Common.Resources.FCPrice;
			_discountPriceCol.Header = ArmSoft.SME.Trade.Common.Resources.FCDiscPrice;
			_summaCol.Header = ArmSoft.SME.Common.Resources.FCSumma;
			try
			{
				if (!_date.DoValidate())
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				_currExChngRate.Rate = exchangeRates.Rate;
				_currExChngRate.Base = exchangeRates.Base;
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
			CurrCodeValueChanged();
		}
		_currExChngRate.DoValidate();
		if (flag)
		{
			UpdatePricesForCurr();
		}
		if (!string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate())
		{
			_oldDocCurrCode = _currCode.Code;
			_oldDocCurrExChngRate = _currExChngRate.Rate;
			_oldDocCurrExChngBase = _currExChngRate.Base;
		}
		_rowInfoChange?.Invoke((MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void UpdatePricesForCurr()
	{
		foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
		{
			if (_oldDocCurrCode == "AMD")
			{
				if (_currExChngRate.Rate != 0m)
				{
					ExchangeRate exchangeRate = new ExchangeRate
					{
						Rate = _currExChngRate.Rate,
						Base = _currExChngRate.Base
					};
					invoices.Price = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(invoices.Price, exchangeRate);
				}
			}
			else if (!(_currExChngRate.Rate == 0m) && !(_oldDocCurrExChngBase == 0m))
			{
				invoices.Price = (invoices.Price * _oldDocCurrExChngRate / _oldDocCurrExChngBase / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
			}
			invoices.Price = Methods.RoundPrice(invoices.Price, _priceListTypeRoundDgr);
			invoices.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(invoices.Price, invoices.Quantity);
		}
		CalculateDiscount(_invoicesList, _discountPercent.Value, gDiscount: false, calcInvFee: true, _vatType.Code);
		CalcSummAndSummVAT();
		_invoiceGrid.Grid.RefreshData();
	}

	private void CurrExChngRate_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_currExChngRate.DoValidate())
		{
			CalculatePartSumma();
		}
	}

	private void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_date.DoValidate() && _date.EditValue.HasValue)
		{
			CurrCodeValueChanged();
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
		}
	}

	private void CurrCodeValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.Code != "AMD" && _date.DoValidate() && _currCode.DoValidate() && !_currExChngRate.IsReadOnly)
		{
			try
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				_currExChngRate.Rate = exchangeRates.Rate;
				_currExChngRate.Base = exchangeRates.Base;
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}
	}

	private void MTInvoiceGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		_invoicesList.Add(new MTInvoiceSpecificationRow(this)
		{
			IsStorageRequired = false,
			ItemType = _goodsTypes_Goods,
			Storage = _storage.Code,
			Discount = _discountPercent.Value,
			VAT = _vatType.Code.InList("1", "2"),
			RowID = _maxRowID,
			DealType = (_vatType.Code.InList("1", "2") ? string.Empty : _dealTypeKey),
			IsPartySelected = false
		});
		_maxRowID++;
		e.RowNumber = _invoicesList.Count - 1;
		_rowInfoChange?.Invoke((MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void MTInvoiceGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(DialogWindowMode.NewModeWithNext, this);
		goodsSelectPartyWindow.ShowDialog();
		int addedRows = goodsSelectPartyWindow.AddedRows;
		if (addedRows != 0)
		{
			_maxRowID += addedRows;
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			_invoiceGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			RowInfoChangeEventHandler rowInfoChangeEventHandler = _rowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTInvoiceSpecificationRow> invoicesList = _invoicesList;
				rowInfoChangeEventHandler(invoicesList[invoicesList.Count - 1]);
			}
		}
	}

	private void MTInvoiceGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTInvoiceSpecificationRow mTInvoiceSpecificationRow = (MTInvoiceSpecificationRow)_invoiceGrid.Grid.GetRow(e.RowHandle);
		if (mTInvoiceSpecificationRow == null)
		{
			return;
		}
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(DialogWindowMode.NewModeWithNext, this, mTInvoiceSpecificationRow);
		goodsSelectPartyWindow.ShowDialog();
		int addedRows = goodsSelectPartyWindow.AddedRows;
		if (addedRows != 0)
		{
			_maxRowID += addedRows;
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			_invoiceGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			RowInfoChangeEventHandler rowInfoChangeEventHandler = _rowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTInvoiceSpecificationRow> invoicesList = _invoicesList;
				rowInfoChangeEventHandler(invoicesList[invoicesList.Count - 1]);
			}
		}
	}

	private void MTInvoiceGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		MTInvoiceSpecificationRow mTInvoiceSpecificationRow = (MTInvoiceSpecificationRow)_invoiceGrid.Grid.GetRow(e.RowHandle);
		if (mTInvoiceSpecificationRow == null)
		{
			return;
		}
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(e.AllowEdit ? DialogWindowMode.EditMode : DialogWindowMode.ViewMode, this, mTInvoiceSpecificationRow);
		e.RowEdited = Convert.ToBoolean(goodsSelectPartyWindow.ShowDialog());
		if (e.AllowEdit && e.RowEdited)
		{
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			_invoiceGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			_rowInfoChange?.Invoke(mTInvoiceSpecificationRow);
		}
	}

	private void MTInvoiceGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTInvoiceSpecificationRow mTInvoiceSpecificationRow = (MTInvoiceSpecificationRow)_invoiceGrid.Grid.GetRow(e.RowHandle);
		if (mTInvoiceSpecificationRow != null)
		{
			MTInvoiceSpecificationRow mTInvoiceSpecificationRow2 = new MTInvoiceSpecificationRow(this);
			mTInvoiceSpecificationRow.Clone(mTInvoiceSpecificationRow2);
			mTInvoiceSpecificationRow2.IsPartySelected = ((mTInvoiceSpecificationRow2.ItemType != _goodsTypes_Goods) ? ((bool?)null) : new bool?(false));
			mTInvoiceSpecificationRow2.RowID = _maxRowID;
			_invoicesList.Add(mTInvoiceSpecificationRow2);
			_maxRowID++;
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			_invoiceGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			RowInfoChangeEventHandler rowInfoChangeEventHandler = _rowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTInvoiceSpecificationRow> invoicesList = _invoicesList;
				rowInfoChangeEventHandler(invoicesList[invoicesList.Count - 1]);
			}
		}
	}

	private void MTInvoiceGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_invoiceGrid.Grid.SortInfo.Count > 0 || _invoiceTableView.Grid.VisibleRowCount != _invoicesList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTInvoiceSpecificationRow mTInvoiceSpecificationRow = (MTInvoiceSpecificationRow)_invoiceGrid.Grid.GetRow(e.RowHandle);
		if (mTInvoiceSpecificationRow != null)
		{
			_invoicesList.RemoveAt(e.RowHandle);
			_invoicesList.Insert(e.RowHandle - 1, mTInvoiceSpecificationRow);
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
		}
		_invoiceGrid.Grid.RefreshData();
	}

	private void MTInvoiceGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_invoiceGrid.Grid.SortInfo.Count > 0 || _invoiceTableView.Grid.VisibleRowCount != _invoicesList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTInvoiceSpecificationRow mTInvoiceSpecificationRow = (MTInvoiceSpecificationRow)_invoiceGrid.Grid.GetRow(e.RowHandle);
		if (mTInvoiceSpecificationRow != null)
		{
			_invoicesList.RemoveAt(e.RowHandle);
			_invoicesList.Insert(e.RowHandle + 1, mTInvoiceSpecificationRow);
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
		}
		_invoiceGrid.Grid.RefreshData();
	}

	private void MTInvoiceGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			MTInvoiceSpecificationRow mTInvoiceSpecificationRow = (MTInvoiceSpecificationRow)_invoiceGrid.Grid.GetRow(e.RowHandle);
			if (mTInvoiceSpecificationRow == null)
			{
				return;
			}
			if (e.Column == _itemCodeCol)
			{
				if (mTInvoiceSpecificationRow.ItemType == GoodsTypesSE.Service.Value)
				{
					GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, GoodsTypesSE.Service.Value, "", isShowOnlyInPriceList: true);
					ModalBrowser modalBrowser = new ModalBrowser(goodsFilter)
					{
						LookUpValue = e.Value,
						Owner = e.Owner,
						Left = e.Left,
						Top = e.Top
					};
					if (modalBrowser.Show())
					{
						e.NewValue = goodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
					}
				}
				else
				{
					if (!_date.DoValidate() || !_date.EditValue.HasValue)
					{
						ControlsHelper.SetFocus(_date);
						throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
					}
					bool showAll = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(mTInvoiceSpecificationRow.Storage);
					IDropDownView dropDownView = null;
					dropDownView = ((string.IsNullOrWhiteSpace(_priceListType.Code) || !_showGoodsListWIthPrices) ? ((IDropDownView)new GoodsAvailabilityFilter(_date.EditValue.Value, mTInvoiceSpecificationRow.Storage, forRetail: false, includeServices: false, showAll)) : ((IDropDownView)new GoodsAvailabilityFilterForInvoices(_date.EditValue.Value, mTInvoiceSpecificationRow.Storage, _priceListType.Code, retail: false, "", "", extendedView: true, isFilteredSearchView: false, showAvailability: true, showAll: false)));
					ModalBrowser modalBrowser2 = new ModalBrowser(dropDownView)
					{
						LookUpValue = e.Value,
						Owner = e.Owner,
						Left = e.Left,
						Top = e.Top
					};
					if (modalBrowser2.Show())
					{
						e.NewValue = dropDownView.ValueMember(modalBrowser2.SelectedRowIndex);
					}
				}
				if (_paramUseDiscount && _alwaysCalcDiscountAuto)
				{
					On_DiscountClick();
				}
				CalcSummAndSummVAT();
			}
			else if (e.Column == _storageCol)
			{
				if (mTInvoiceSpecificationRow.ItemType != _goodsTypes_Goods)
				{
					return;
				}
				Storages storages = new Storages(false);
				ModalBrowser modalBrowser3 = new ModalBrowser(storages)
				{
					LookUpValue = e.Value,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser3.Show())
				{
					e.NewValue = storages.ValueMember(modalBrowser3.SelectedRowIndex);
				}
				if (_paramUseDiscount && _alwaysCalcDiscountAuto)
				{
					On_DiscountClick();
				}
			}
			else if (e.Column == _dealTypeCol)
			{
				if (mTInvoiceSpecificationRow == null || (_vatType.Code != "3" && _vatType.Code != "4" && mTInvoiceSpecificationRow.VAT.Value))
				{
					return;
				}
				SmartEnum smartEnum = new SmartEnum(_vatType.Code.InList("1", "2") ? typeof(InvoiceDealTypesSE) : typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
				ModalBrowser modalBrowser4 = new ModalBrowser(smartEnum)
				{
					LookUpValue = e.Value,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser4.Show())
				{
					mTInvoiceSpecificationRow.DealType = (string)smartEnum.KeyMember(modalBrowser4.SelectedRowIndex);
				}
			}
			else if (e.Column == _partyCol)
			{
				if (mTInvoiceSpecificationRow == null || mTInvoiceSpecificationRow.ItemType != _goodsTypes_Goods)
				{
					return;
				}
				if (!_date.DoValidate() || !_date.EditValue.HasValue)
				{
					ControlsHelper.SetFocus(_date);
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				if (string.IsNullOrWhiteSpace(mTInvoiceSpecificationRow.Storage))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyStorage);
				}
				if (string.IsNullOrWhiteSpace(mTInvoiceSpecificationRow.ItemCode))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
				}
				decimal quantity = (mTInvoiceSpecificationRow.Quantity * ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetMTQntUnitCoeff(mTInvoiceSpecificationRow.ItemCode, mTInvoiceSpecificationRow.UnitCode)).RoundAs(3);
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTInvoiceSpecificationRow.ItemCode);
				bool isReadOnly = PO.StoredInDb && !IsDraftOrDeleted;
				MTSelectPartyWindow mTSelectPartyWindow = new MTSelectPartyWindow(mTInvoiceSpecificationRow.Storage, _date.EditValue.Value, mTInvoiceSpecificationRow.ItemCode, item.Unit, quantity, mTInvoiceSpecificationRow.RowID, isReadOnly)
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
						mTInvoiceSpecificationRow.ItemID = item.Id;
						mTInvoiceSpecificationRow.ItemName = item.Caption;
						mTInvoiceSpecificationRow.ItemCode = mTSelectPartyWindow.MTCode;
						mTInvoiceSpecificationRow.UnitCode = item.Unit;
						mTInvoiceSpecificationRow.UnitBrief = item.UnitRow.Brief;
						mTInvoiceSpecificationRow.Quantity = mTSelectPartyWindow.TotalExpQty;
						mTInvoiceSpecificationRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mTInvoiceSpecificationRow.DiscountPrice, mTInvoiceSpecificationRow.Quantity);
						if (_useEnvFees)
						{
							mTInvoiceSpecificationRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mTInvoiceSpecificationRow.Summa, mTInvoiceSpecificationRow.EnvFeePercent, mTInvoiceSpecificationRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
						}
						mTInvoiceSpecificationRow.IsPartySelected = mTSelectPartyWindow.IsPartySelected.Value;
						mTInvoiceSpecificationRow.ValidateWithoutResponse();
					}
				}
			}
			else if (e.Column == _rowMarkingListCol)
			{
				if (mTInvoiceSpecificationRow == null || mTInvoiceSpecificationRow.ItemType != GoodsTypesSE.Good.Value)
				{
					return;
				}
				OpenMarkingSelectWindow(mTInvoiceSpecificationRow);
			}
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			_invoiceGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTInvoiceGrid_DeletedRow(object sender, EventArgs e)
	{
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		CalcSummAndSummVAT();
		_invoiceGrid.Grid.RefreshData();
		_rowInfoChange?.Invoke((MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void MTInvoiceGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		MTInvoiceSpecificationRow mtInvGridRow = (MTInvoiceSpecificationRow)_invoiceGrid.Grid.GetRow(e.RowHandle);
		if (mtInvGridRow != null)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
		}
		_rowInfoChange?.Invoke((MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void MTInvoiceTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _invoiceTableView.FocusedRowHandle;
		MTInvoiceSpecificationRow mTInvoiceSpecificationRow = (MTInvoiceSpecificationRow)_invoiceGrid.Grid.GetRow(focusedRowHandle);
		if (mTInvoiceSpecificationRow != null)
		{
			ChangeItemCodeColumnAdditonalView(mTInvoiceSpecificationRow.ItemType, mTInvoiceSpecificationRow.ItemCode);
		}
		_rowInfoChange?.Invoke(mTInvoiceSpecificationRow);
	}

	private void ChangeItemCodeColumnAdditonalView(string typeValue, string itemCode)
	{
		ItemTypesSE itemTypesSE = ItemTypesSE.All;
		if (typeValue == _goodsTypes_Goods)
		{
			itemTypesSE = ItemTypesSE.Goods;
		}
		else if (typeValue == ItemTypesSE.Services.Value)
		{
			itemTypesSE = ItemTypesSE.Services;
		}
		_itemCodeCol.DataView = new GoodsFilter(shortMode: true, itemTypesSE.Value, "", isShowOnlyInPriceList: true);
		_unitCodeCol.DataView = new MTQntUnits(itemCode, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", itemTypesSE.Value);
	}

	private void MTInvoiceTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTInvoiceSpecificationRow mtInvGridRow = (MTInvoiceSpecificationRow)_invoiceGrid.Grid.GetRow(e.RowHandle);
		if (mtInvGridRow == null)
		{
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		if (e.OldValue != null)
		{
			text = e.OldValue.ToString();
		}
		if (e.Value != null)
		{
			text2 = e.Value.ToString();
		}
		if (e.Column == _itemTypeCol && text != text2)
		{
			mtInvGridRow.Storage = "";
			mtInvGridRow.ItemCode = "";
			mtInvGridRow.ItemID = -1;
			mtInvGridRow.ItemName = "";
			mtInvGridRow.Quantity = 0m;
			mtInvGridRow.Summa = 0m;
			mtInvGridRow.Price = 0m;
			mtInvGridRow.DiscountPrice = 0m;
			mtInvGridRow.Discount = 0m;
			if (_useEnvFees)
			{
				mtInvGridRow.EnvFeePercent = 0m;
				mtInvGridRow.EnvFeeSumma = 0m;
			}
			mtInvGridRow.UnitCode = "";
			mtInvGridRow.UnitBrief = "";
			mtInvGridRow.IsPartySelected = false;
			mtInvGridRow.RowDescription = "";
			if (text == _goodsTypes_Goods)
			{
				PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
			}
			if (text2 != _goodsTypes_Goods)
			{
				mtInvGridRow.IsPartySelected = null;
			}
			if (text2 != _goodsTypes_Goods && text2 != GoodsTypesSE.Service.Value)
			{
				mtInvGridRow.ItemType = text;
			}
			if (mtInvGridRow.ItemType == _goodsTypes_Goods && string.IsNullOrWhiteSpace(mtInvGridRow.Storage))
			{
				mtInvGridRow.Storage = _storage.Code;
			}
			ChangeItemCodeColumnAdditonalView(text2, "");
			_rowInfoChange?.Invoke(mtInvGridRow);
		}
		if (e.Column == _storageCol)
		{
			if (!string.IsNullOrWhiteSpace(text2) && TradeSettings.TradeCacheService.Storage.GetItem(text2) != null)
			{
				if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text2))
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, text2), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					mtInvGridRow.Storage = text;
				}
				else if (mtInvGridRow.ItemType == _goodsTypes_Goods && mtInvGridRow.IsPartySelected == true)
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
					mtInvGridRow.IsPartySelected = false;
				}
			}
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			_rowInfoChange?.Invoke(mtInvGridRow);
		}
		else if (e.Column == _itemCodeCol)
		{
			try
			{
				if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
					mtInvGridRow.ItemID = -1;
					mtInvGridRow.ItemCode = "";
					mtInvGridRow.ItemName = "";
					mtInvGridRow.UnitCode = "";
					mtInvGridRow.UnitBrief = "";
					mtInvGridRow.Price = 0m;
					mtInvGridRow.Discount = 0m;
					mtInvGridRow.DiscountPrice = 0m;
					mtInvGridRow.Summa = 0m;
					if (_useEnvFees)
					{
						mtInvGridRow.EnvFeePercent = 0m;
						mtInvGridRow.EnvFeeSumma = 0m;
					}
					mtInvGridRow.IsPartySelected = false;
					mtInvGridRow.RowDescription = "";
					mtInvGridRow.MarkingList.Clear();
					mtInvGridRow.MarkingStatus = "";
				}
				else
				{
					if (text != text2 && mtInvGridRow.ItemType == _goodsTypes_Goods)
					{
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
						mtInvGridRow.IsPartySelected = false;
					}
					string text3 = e.Value.ToString();
					foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
					{
						if (mtInvGridRow != invoices && string.Equals(text3, invoices.ItemCode, StringComparison.OrdinalIgnoreCase))
						{
							if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, text3), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
							{
								mtInvGridRow.ItemCode = "";
								mtInvGridRow.ItemID = -1;
								mtInvGridRow.ItemCode = "";
								mtInvGridRow.ItemName = "";
								mtInvGridRow.Quantity = 0m;
								mtInvGridRow.Summa = 0m;
								mtInvGridRow.Price = 0m;
								mtInvGridRow.Discount = 0m;
								mtInvGridRow.DiscountPrice = 0m;
								if (_useEnvFees)
								{
									mtInvGridRow.EnvFeePercent = 0m;
									mtInvGridRow.EnvFeeSumma = 0m;
								}
								mtInvGridRow.UnitCode = "";
								mtInvGridRow.UnitBrief = "";
								mtInvGridRow.IsPartySelected = false;
								mtInvGridRow.RowDescription = "";
								return;
							}
							break;
						}
					}
					if (string.IsNullOrWhiteSpace(text3))
					{
						throw new Exception();
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text3, throwExceptionNotExits: true);
					mtInvGridRow.ItemName = item.Caption;
					mtInvGridRow.ItemCode = text3;
					mtInvGridRow.ItemID = item.Id;
					mtInvGridRow.ItemType = item.Type;
					mtInvGridRow.UnitBrief = item.UnitRow.Brief;
					mtInvGridRow.UnitCode = item.Unit;
					_unitCodeCol.DataView = new MTQntUnits(item.MTCode);
					mtInvGridRow.VAT = item.VAT;
					mtInvGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mtInvGridRow.ItemID, mtInvGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
					mtInvGridRow.Price = Methods.RoundPrice(mtInvGridRow.Price, _priceListTypeRoundDgr);
					if (_paramUseDiscount && !_alwaysCalcDiscountAuto)
					{
						if (string.IsNullOrWhiteSpace(DiscountPriceListType))
						{
							mtInvGridRow.Discount = _discountPercent.Value;
							if (mtInvGridRow.Discount == 0m || item.Discount != 0m)
							{
								mtInvGridRow.Discount = item.Discount;
							}
							mtInvGridRow.DiscountPrice = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateDiscountPrice(mtInvGridRow.Price, mtInvGridRow.Discount);
						}
						else
						{
							CalculateGridRowDiscountPrice(ref mtInvGridRow);
							CalculateGridRowSummaAndDiscountSumma(ref mtInvGridRow, mtInvGridRow.DiscountPrice == 0m);
						}
					}
					else
					{
						mtInvGridRow.DiscountPrice = mtInvGridRow.Price;
					}
					mtInvGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, mtInvGridRow.Quantity);
					if (_useEnvFees && item.Type != GoodsTypesSE.Service.Value)
					{
						mtInvGridRow.EnvFeePercent = item.EcoFeeRate;
						mtInvGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvGridRow.Summa, mtInvGridRow.EnvFeePercent, mtInvGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
					}
					if (!string.IsNullOrWhiteSpace(_dealTypeKey))
					{
						if (_vatType.Code == "3" || _vatType.Code == "4" || !item.VAT)
						{
							mtInvGridRow.DealType = _dealTypeKey;
						}
						else
						{
							mtInvGridRow.DealType = string.Empty;
						}
					}
					mtInvGridRow.IsPartySelected = ((mtInvGridRow.ItemType != _goodsTypes_Goods) ? ((bool?)null) : new bool?(false));
				}
			}
			catch
			{
				mtInvGridRow.ItemName = "";
				mtInvGridRow.ItemID = -1;
				mtInvGridRow.UnitCode = "";
				mtInvGridRow.UnitBrief = "";
				mtInvGridRow.Quantity = 0m;
				mtInvGridRow.Price = 0m;
				mtInvGridRow.Discount = 0m;
				mtInvGridRow.DiscountPrice = 0m;
				mtInvGridRow.Summa = 0m;
				mtInvGridRow.IsPartySelected = false;
				mtInvGridRow.RowDescription = "";
				mtInvGridRow.MarkingList.Clear();
				mtInvGridRow.MarkingStatus = "";
			}
			finally
			{
				_rowInfoChange?.Invoke(mtInvGridRow);
			}
		}
		else if (e.Column == _unitCodeCol)
		{
			try
			{
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()) && !string.IsNullOrWhiteSpace(mtInvGridRow.ItemCode))
				{
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(e.Value.ToString(), throwExceptionNotExits: true);
					mtInvGridRow.UnitBrief = ((item2 == null) ? "" : item2.Brief);
					if (string.IsNullOrWhiteSpace(_priceListType.Code))
					{
						mtInvGridRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(text, e.Value.ToString(), mtInvGridRow.Price, mtInvGridRow.ItemCode);
					}
					else
					{
						decimal priceForGivenPriceListAndVATType = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mtInvGridRow.ItemID, e.Value.ToString(), _date.EditValue.Value, _currCode.Code);
						if (priceForGivenPriceListAndVATType != 0m)
						{
							mtInvGridRow.Price = priceForGivenPriceListAndVATType;
						}
						else
						{
							mtInvGridRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(text, e.Value.ToString(), mtInvGridRow.Price, mtInvGridRow.ItemCode);
						}
					}
					mtInvGridRow.Price = Methods.RoundPrice(mtInvGridRow.Price, _priceListTypeRoundDgr);
					mtInvGridRow.DiscountPrice = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateDiscountPrice(mtInvGridRow.Price, mtInvGridRow.Discount);
					mtInvGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, mtInvGridRow.Quantity);
					if (_useEnvFees && mtInvGridRow.ItemType == _goodsTypes_Goods)
					{
						mtInvGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvGridRow.Summa, mtInvGridRow.EnvFeePercent, mtInvGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
					}
				}
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
			finally
			{
				_rowInfoChange?.Invoke(mtInvGridRow);
			}
		}
		else if (e.Column == _quantityCol)
		{
			if (mtInvGridRow.ItemType == _goodsTypes_Goods && Convert.ToDecimal(e.OldValue) != Convert.ToDecimal(e.Value) && Convert.ToDecimal(e.OldValue) != 0m)
			{
				if (mtInvGridRow.IsPartySelected == true)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ForRowSelectedParties, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				}
				PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
				mtInvGridRow.IsPartySelected = false;
			}
			mtInvGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, Convert.ToDecimal(e.Value));
			if (_useEnvFees)
			{
				mtInvGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvGridRow.Summa, mtInvGridRow.EnvFeePercent, mtInvGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
			_rowInfoChange?.Invoke(mtInvGridRow);
		}
		else if (e.Column == _priceCol)
		{
			mtInvGridRow.Price = Convert.ToDecimal(e.Value);
			mtInvGridRow.DiscountPrice = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateDiscountPrice(mtInvGridRow.Price, mtInvGridRow.Discount);
			mtInvGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, mtInvGridRow.Quantity);
			if (!string.IsNullOrWhiteSpace(DiscountPriceListType) && !(mtInvGridRow.Price == 0m) && mtInvGridRow.DiscountPrice == 0m)
			{
				mtInvGridRow.Discount = (100m * (mtInvGridRow.Price - mtInvGridRow.DiscountPrice) / mtInvGridRow.Price).RoundAs(2);
			}
			_priceListTypeRoundDgr = "";
			CalculateGridRowSummaAndDiscountSumma(ref mtInvGridRow, string.IsNullOrWhiteSpace(DiscountPriceListType) || mtInvGridRow.DiscountPrice == 0m);
			_priceListTypeRoundDgr = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(_priceListType.Code);
			mtInvGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, mtInvGridRow.Quantity);
			if (_useEnvFees && mtInvGridRow.ItemType == _goodsTypes_Goods)
			{
				mtInvGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvGridRow.Summa, mtInvGridRow.EnvFeePercent, mtInvGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
			_rowInfoChange?.Invoke(mtInvGridRow);
		}
		else if (e.Column == _discountCol)
		{
			mtInvGridRow.DiscountPrice = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateDiscountPrice(mtInvGridRow.Price, mtInvGridRow.Discount);
			mtInvGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, mtInvGridRow.Quantity);
			if (_useEnvFees && mtInvGridRow.ItemType == _goodsTypes_Goods)
			{
				mtInvGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvGridRow.Summa, mtInvGridRow.EnvFeePercent, mtInvGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
			_rowInfoChange?.Invoke(mtInvGridRow);
		}
		else if (e.Column == _discountPriceCol)
		{
			mtInvGridRow.DiscountPrice = Convert.ToDecimal(e.Value);
			mtInvGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, mtInvGridRow.Quantity);
			if (mtInvGridRow.Price != 0m)
			{
				mtInvGridRow.Discount = (100m * (mtInvGridRow.Price - mtInvGridRow.DiscountPrice) / mtInvGridRow.Price).RoundAs(2);
				if (mtInvGridRow.Discount / 1000m >= 1m || mtInvGridRow.Discount / 1000m <= -1m)
				{
					mtInvGridRow.Discount = 0m;
					mtInvGridRow.DiscountPrice = mtInvGridRow.Price;
					mtInvGridRow.DiscountSumma = 0m;
					mtInvGridRow.Summa = mtInvGridRow.Price * mtInvGridRow.Quantity;
				}
			}
			if (_useEnvFees && mtInvGridRow.ItemType == _goodsTypes_Goods)
			{
				mtInvGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvGridRow.Summa, mtInvGridRow.EnvFeePercent, mtInvGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
			_rowInfoChange?.Invoke(mtInvGridRow);
		}
		else if (e.Column == _summaCol)
		{
			mtInvGridRow.Summa = Convert.ToDecimal(e.Value);
			if (mtInvGridRow.Quantity > 0m)
			{
				mtInvGridRow.DiscountPrice = (mtInvGridRow.Summa / mtInvGridRow.Quantity).RoundAs(2);
				if (!_paramUseDiscount)
				{
					mtInvGridRow.Price = (mtInvGridRow.Summa / mtInvGridRow.Quantity).RoundAs(2);
				}
			}
			if (mtInvGridRow.Price != 0m)
			{
				mtInvGridRow.Discount = (100m * (mtInvGridRow.Price - mtInvGridRow.DiscountPrice) / mtInvGridRow.Price).RoundAs(2);
			}
			_rowInfoChange?.Invoke(mtInvGridRow);
			if (_useEnvFees && mtInvGridRow.ItemType == _goodsTypes_Goods)
			{
				mtInvGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvGridRow.Summa, mtInvGridRow.EnvFeePercent, mtInvGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
		}
		else if (e.Column == _envFeePercentCol)
		{
			mtInvGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvGridRow.Summa, Convert.ToDecimal(e.Value), mtInvGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
		}
		else if (e.Column == _envFeeSummaCol)
		{
			if (mtInvGridRow.Summa == 0m || Convert.ToDecimal(e.Value) == 0m)
			{
				mtInvGridRow.EnvFeePercent = 0m;
			}
			else
			{
				decimal summa = mtInvGridRow.Summa;
				if (_vatType.Code.Trim() == "2" && mtInvGridRow.VAT.Value)
				{
					summa -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtInvGridRow.Summa, _vatType.Code, mtInvGridRow.VAT.Value);
				}
				summa -= Convert.ToDecimal(e.Value);
				if (summa == 0m)
				{
					mtInvGridRow.EnvFeePercent = 0m;
				}
				else
				{
					mtInvGridRow.EnvFeePercent = (Convert.ToDecimal(e.Value) * 100m / summa).RoundAs(2);
				}
			}
		}
		else if (e.Column == _vatCol)
		{
			if (_useEnvFees && mtInvGridRow.ItemType == _goodsTypes_Goods)
			{
				mtInvGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvGridRow.Summa, mtInvGridRow.EnvFeePercent, (bool)e.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
			if ((bool)e.Value)
			{
				mtInvGridRow.DealType = string.Empty;
			}
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		_invoiceGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
		ChangeMarkingStatus(mtInvGridRow);
		_rowInfoChange?.Invoke(mtInvGridRow);
	}

	private void MTInvoiceTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (e.Row == null)
		{
			return;
		}
		if (e.Column == _storageCol || e.Column == _partyCol || e.Column == _rowMarkingListCol)
		{
			if (((MTInvoiceSpecificationRow)e.Row).ItemType == GoodsTypesSE.Service.Value)
			{
				e.Cancel = true;
			}
		}
		else if (e.Column == _itemNameCol)
		{
			if (((MTInvoiceSpecificationRow)e.Row).ItemType == _goodsTypes_Goods)
			{
				e.Cancel = true;
			}
		}
		else if ((e.Column == _envFeePercentCol || e.Column == _envFeeSummaCol) && ((MTInvoiceSpecificationRow)e.Row).ItemType == GoodsTypesSE.Service.Value)
		{
			e.Cancel = true;
		}
	}

	private void SelectMarking_Click(object sender, RoutedEventArgs e)
	{
		if (_invoiceTableView.Grid.CurrentItem == null)
		{
			return;
		}
		MTInvoiceSpecificationRow mTInvoiceSpecificationRow = (MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem;
		if (mTInvoiceSpecificationRow == null)
		{
			return;
		}
		if (!MarkingHelpers.IsGoodMarkable(mTInvoiceSpecificationRow.ItemCode, _useMarking, mTInvoiceSpecificationRow.MarkingList.Count != 0, _date.EditValue.Value, out var isRowMarkable))
		{
			if (string.IsNullOrWhiteSpace(mTInvoiceSpecificationRow.ItemCode))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
			}
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotMarkable, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
			return;
		}
		mTInvoiceSpecificationRow.IsRowMarkable = isRowMarkable;
		List<string> allMarkings = _invoicesList.SelectMany((MTInvoiceSpecificationRow row) => row.MarkingList).ToList();
		MarkingSelectWindow markingSelectWindow = new MarkingSelectWindow(this, _readOnlyMarkingWindow, mTInvoiceSpecificationRow.IsRowMarkable, mTInvoiceSpecificationRow.ItemCode, mTInvoiceSpecificationRow.ItemName, mTInvoiceSpecificationRow.MarkingList, allMarkings, mTInvoiceSpecificationRow.Quantity);
		if (markingSelectWindow.ShowDialog().Value && !markingSelectWindow.IsViewOnlyMode)
		{
			mTInvoiceSpecificationRow.MarkingList.Clear();
			foreach (MarkingSelectWindow.MarkingRow marking in markingSelectWindow.MarkingList)
			{
				mTInvoiceSpecificationRow.MarkingList.Add(marking.Marking);
			}
			if (mTInvoiceSpecificationRow.Quantity < (decimal)mTInvoiceSpecificationRow.MarkingList.Count)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTInvoiceSpecificationRow.ItemCode);
				if (item == null || !item.IsWeight)
				{
					mTInvoiceSpecificationRow.Quantity = mTInvoiceSpecificationRow.MarkingList.Count;
					ChangeRowQty(mTInvoiceSpecificationRow);
				}
			}
			ChangeMarkingStatus(mTInvoiceSpecificationRow);
		}
		CalcSummAndSummVAT();
		_rowInfoChange?.Invoke(mTInvoiceSpecificationRow);
	}

	private void ChangeRowQty(MTInvoiceSpecificationRow focusedRow)
	{
		if (focusedRow.IsPartySelected.HasValue && focusedRow.IsPartySelected.Value)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ForRowSelectedParties, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == focusedRow.RowID);
			focusedRow.IsPartySelected = false;
		}
		focusedRow.Summa = (focusedRow.Quantity * focusedRow.DiscountPrice).RoundAs(2);
		focusedRow.DiscountSumma = ((focusedRow.Quantity * focusedRow.Price).RoundAs(2) - focusedRow.Summa).RoundAs(2);
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
	}

	private void ChangeMarkingStatus(MTInvoiceSpecificationRow row)
	{
		if (row != null)
		{
			if (MarkingHelpers.IsGoodMarkable(row.ItemCode, _useMarking, row.MarkingList.Count != 0, _date.EditValue.Value, out var isRowMarkable))
			{
				row.MarkingStatus = MarkingHelper.GetMarkingStatus(row.MarkingList.Count, row.Quantity);
			}
			else
			{
				row.MarkingStatus = "";
			}
			row.IsRowMarkable = isRowMarkable;
		}
	}

	private void OpenMarkingSelectWindow(MTInvoiceSpecificationRow mtInvGridRow)
	{
		if (!MarkingHelpers.IsGoodMarkable(mtInvGridRow.ItemCode, _useMarking, mtInvGridRow.MarkingList.Count != 0, _date.EditValue.Value, out var isRowMarkable))
		{
			if (string.IsNullOrWhiteSpace(mtInvGridRow.ItemCode))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
			}
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotMarkable, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
			return;
		}
		mtInvGridRow.IsRowMarkable = isRowMarkable;
		List<string> allMarkings = _invoicesList.SelectMany((MTInvoiceSpecificationRow row) => row.MarkingList).ToList();
		MarkingSelectWindow markingSelectWindow = new MarkingSelectWindow(this, _readOnlyMarkingWindow, mtInvGridRow.IsRowMarkable, mtInvGridRow.ItemCode, mtInvGridRow.ItemName, mtInvGridRow.MarkingList, allMarkings, mtInvGridRow.Quantity);
		if (!markingSelectWindow.ShowDialog().Value || markingSelectWindow.IsViewOnlyMode)
		{
			return;
		}
		mtInvGridRow.MarkingList.Clear();
		foreach (MarkingSelectWindow.MarkingRow marking in markingSelectWindow.MarkingList)
		{
			mtInvGridRow.MarkingList.Add(marking.Marking);
		}
		if (mtInvGridRow.Quantity < (decimal)mtInvGridRow.MarkingList.Count)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtInvGridRow.ItemCode);
			if (item == null || !item.IsWeight)
			{
				mtInvGridRow.Quantity = mtInvGridRow.MarkingList.Count;
				ChangeRowQty(mtInvGridRow);
			}
		}
		ChangeMarkingStatus(mtInvGridRow);
	}

	private void SupplierBusinessAddress_EditKeyValueChanged(object sender, object keyValue)
	{
		if (keyValue != null)
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(keyValue.ToString());
			if (item != null)
			{
				_supplierBusinessAddress.Text = item.Address;
			}
		}
	}

	private void ViewAvailAndTurn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			MTInvoiceSpecificationRow mTInvoiceSpecificationRow = (MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem;
			if (mTInvoiceSpecificationRow != null)
			{
				new MTAvailAndTurnWindow(DialogWindowMode.ViewMode, this, _storage.Code, "", mTInvoiceSpecificationRow.ItemID, mTInvoiceSpecificationRow.ItemCode, mTInvoiceSpecificationRow.UnitCode, _buyerContractContainer.PartnerCode.Code).Invoke();
			}
			else
			{
				new MTAvailAndTurnWindow(DialogWindowMode.ViewMode, this, _storage.Code, "", -1, string.Empty, string.Empty, _buyerContractContainer.PartnerCode.Code).Invoke();
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
			FillComplectRowsWindow<MTInvoiceSpecificationRow> fillComplectRowsWindow = new FillComplectRowsWindow<MTInvoiceSpecificationRow>(this, PO, _date.EditValue.Value, _storage.Code, _paramUseDiscount, _anyAutoDiscountModeIsEnabled, _useDiscount, _discountPercent.Value, _maxRowID, _invoicesList, _invoiceGrid, _priceListType.Code, _vatType.Code, _currCode.Code);
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
					MTInvoiceSpecificationRow mTInvoiceSpecificationRow = (_invoiceGrid.ItemsSource as IEnumerable<MTInvoiceSpecificationRow>)?.FirstOrDefault((MTInvoiceSpecificationRow x) => x.RowID == i);
					if (mTInvoiceSpecificationRow != null)
					{
						if (_vatType.Code.InList("3", "4") || !mTInvoiceSpecificationRow.VAT.Value)
						{
							mTInvoiceSpecificationRow.DealType = _dealTypeKey;
						}
						else
						{
							mTInvoiceSpecificationRow.DealType = string.Empty;
						}
					}
				}
			}
			_maxRowID += addedRows;
			_invoiceGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void Discount_Click(object sender, RoutedEventArgs e)
	{
		On_DiscountClick();
	}

	private void DiscountPriceType_Click(object sender, RoutedEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow(this);
		DropDownControl dropDownControl = new DropDownControl();
		dialogWindow.Name = "DiscountPriceType";
		dialogWindow.Caption = ArmSoft.SME.Trade.Common.Resources.DiscountPriceType;
		dropDownControl = dialogWindow.AddDropDownControl("DiscountPriceType", ArmSoft.SME.Trade.Common.Resources.PriceType, new PriceTypes());
		dropDownControl.Code = DiscountPriceListType;
		dropDownControl.IsReadOnly = !_allowChangeDiscountPriceType;
		if (dialogWindow.ShowDialog().Value)
		{
			DiscountPriceListType = dropDownControl.Code;
			bool flag = true;
			if (_totalSumma.Value != 0m && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangeDiscountPrices, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
			{
				flag = false;
			}
			if (flag)
			{
				DiscountPriceListTypeEditValueChanged();
			}
		}
	}

	private void DiscountPriceListTypeEditValueChanged()
	{
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		UpdateDiscountPrices();
		CalcSummAndSummVAT();
		_invoiceGrid.Grid.RefreshData();
	}

	private void UpdateDiscountPrices()
	{
		if (_currCode.Code != "AMD")
		{
			TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
		}
		if (_invoicesList.Count != 0)
		{
			for (int i = 0; i < _invoicesList.Count; i++)
			{
				MTInvoiceSpecificationRow mtInvGridRow = _invoicesList[i];
				CalculateGridRowDiscountPriceInner(ref mtInvGridRow);
				CalculateGridRowSummaAndDiscountSumma(ref mtInvGridRow, calcDiscountPrice: false);
			}
		}
	}

	private void CalculateDiscount(CommonConst.JSDiscType discType = CommonConst.JSDiscType.ProgCalc)
	{
		switch (discType)
		{
		case CommonConst.JSDiscType.DiscountPercent:
			foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
			{
				invoices.DiscountPrice = (invoices.Price * (1m - invoices.Discount / 100m)).RoundAs(2);
				invoices.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(invoices.DiscountPrice, invoices.Quantity);
				invoices.DiscountSumma = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(invoices.Price, invoices.Quantity) - invoices.Summa;
			}
			break;
		case CommonConst.JSDiscType.DiscountPrice:
			foreach (MTInvoiceSpecificationRow invoices2 in _invoicesList)
			{
				if (invoices2.Price != 0m)
				{
					invoices2.Discount = (100m * (invoices2.Price - invoices2.DiscountPrice) / invoices2.Price).RoundAs(2);
				}
				else
				{
					invoices2.Discount = 0m;
				}
				invoices2.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(invoices2.DiscountPrice, invoices2.Quantity);
				invoices2.DiscountSumma = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(invoices2.Price, invoices2.Quantity) - invoices2.Summa;
			}
			break;
		case CommonConst.JSDiscType.DiscountSumm:
			foreach (MTInvoiceSpecificationRow invoices3 in _invoicesList)
			{
				if (invoices3.Quantity != 0m)
				{
					invoices3.DiscountPrice = (invoices3.Summa / invoices3.Quantity).RoundAs(2);
					if (invoices3.Price != 0m)
					{
						invoices3.Discount = (100m * (invoices3.Price - invoices3.DiscountPrice) / invoices3.Price).RoundAs(2);
					}
					else
					{
						invoices3.Discount = 0m;
					}
					invoices3.DiscountSumma = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(invoices3.Price, invoices3.Quantity) - invoices3.Summa;
				}
			}
			break;
		case CommonConst.JSDiscType.ProgCalc:
			foreach (MTInvoiceSpecificationRow invoices4 in _invoicesList)
			{
				invoices4.Discount = Convert.ToDecimal(_discountPercent.EditValue);
				invoices4.DiscountPrice = (invoices4.Price * (1m - Convert.ToDecimal(_discountPercent.EditValue) / 100m)).RoundAs(2);
				invoices4.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(invoices4.DiscountPrice, invoices4.Quantity);
				invoices4.DiscountSumma = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(invoices4.Price, invoices4.Quantity) - invoices4.Summa;
			}
			break;
		}
		CalcSummAndSummVAT();
		_invoiceGrid.Grid.RefreshData();
	}

	private void CalculateGridRowDiscountPrice(ref MTInvoiceSpecificationRow mtInvGridRow)
	{
		if (string.IsNullOrWhiteSpace(DiscountPriceListType))
		{
			mtInvGridRow.DiscountPrice = 0m;
		}
		else
		{
			CalculateGridRowDiscountPriceInner(ref mtInvGridRow);
		}
	}

	private void CalculateGridRowDiscountPriceInner(ref MTInvoiceSpecificationRow mtInvGridRow)
	{
		if (_paramUseDiscount || GetDiscountUse())
		{
			PricesDataProvider.DataRow dataRow = null;
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtInvGridRow.ItemCode);
			if (!string.IsNullOrWhiteSpace(DiscountPriceListType))
			{
				dataRow = TradeSettings.CreateTradeClient().CommonServices.GetPrice(DiscountPriceListType, mtInvGridRow.ItemID, mtInvGridRow.UnitCode, _date.EditValue.Value);
			}
			if (dataRow != null && dataRow.Price != 0m)
			{
				if (_discountPriceListCurr == _currCode.Code)
				{
					mtInvGridRow.DiscountPrice = dataRow.Price;
				}
				else if (_discountPriceListCurr == "AMD")
				{
					mtInvGridRow.DiscountPrice = (dataRow.Price / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
				}
				else
				{
					mtInvGridRow.DiscountPrice = (dataRow.Price * _discountPriceListCurrRate / _discountPriceListCurrBase / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
				}
				if (_discountPriceListVATType != _vatType.Code && mtInvGridRow.VAT.Value)
				{
					string discountPriceListVATType = _discountPriceListVATType;
					if (!(discountPriceListVATType == "1"))
					{
						if (discountPriceListVATType == "2" && _vatType.Code.InList("1", "3", "4"))
						{
							mtInvGridRow.DiscountPrice -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtInvGridRow.DiscountPrice, "2", mtInvGridRow.VAT.Value);
						}
					}
					else if (_vatType.Code == "2")
					{
						mtInvGridRow.DiscountPrice += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtInvGridRow.DiscountPrice, "1", mtInvGridRow.VAT.Value);
					}
				}
				mtInvGridRow.DiscountPrice = Methods.RoundPrice(mtInvGridRow.DiscountPrice, _discountPriceTypeRoundDegree);
				if (!string.IsNullOrWhiteSpace(_priceListType.Code))
				{
					if (mtInvGridRow.Price == 0m)
					{
						mtInvGridRow.Price = mtInvGridRow.DiscountPrice;
					}
					else
					{
						mtInvGridRow.Discount = (100m * (mtInvGridRow.Price - mtInvGridRow.DiscountPrice) / mtInvGridRow.Price).RoundAs(2);
					}
				}
			}
			else if (_discountPercent.Value != 0m)
			{
				mtInvGridRow.Discount = _discountPercent.Value;
				mtInvGridRow.DiscountPrice = Methods.RoundPrice(mtInvGridRow.Price * (100m - mtInvGridRow.Discount) / 100m, _priceListTypeRoundDgr);
			}
			else if (item.Discount != 0m)
			{
				mtInvGridRow.Discount = item.Discount;
				mtInvGridRow.DiscountPrice = Methods.RoundPrice(mtInvGridRow.Price * (100m - mtInvGridRow.Discount) / 100m, _priceListTypeRoundDgr);
			}
			else
			{
				mtInvGridRow.DiscountPrice = mtInvGridRow.Price;
				mtInvGridRow.Discount = 0m;
			}
		}
		else
		{
			mtInvGridRow.DiscountPrice = mtInvGridRow.Price;
			mtInvGridRow.Discount = 0m;
		}
	}

	private void CalculateGridRowSummaAndDiscountSumma(ref MTInvoiceSpecificationRow mtInvGridRow, bool calcDiscountPrice = true)
	{
		if (_paramUseDiscount || GetDiscountUse())
		{
			if (calcDiscountPrice)
			{
				mtInvGridRow.DiscountPrice = Methods.RoundPrice(mtInvGridRow.Price * (100m - mtInvGridRow.Discount) / 100m, _priceListTypeRoundDgr);
			}
			mtInvGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, mtInvGridRow.Quantity);
			mtInvGridRow.DiscountSumma = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.Price, mtInvGridRow.Quantity) - mtInvGridRow.Summa;
		}
		else
		{
			mtInvGridRow.DiscountPrice = mtInvGridRow.Price;
			mtInvGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.Price, mtInvGridRow.Quantity);
			mtInvGridRow.DiscountSumma = 0m;
		}
	}

	private void CalculatePartSumma()
	{
		decimal summaNCC = _totalSumma.Value;
		if (_currExChngRate.Base != 0m)
		{
			ExchangeRate exchangeRate = new ExchangeRate
			{
				Rate = _currExChngRate.Rate,
				Base = _currExChngRate.Base
			};
			summaNCC = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(_totalSumma.Value, exchangeRate);
		}
		_buyerContractContainer.CalculatePartSumma(summaNCC);
	}

	private decimal CalcBeginSumm()
	{
		decimal result = 0m;
		foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
		{
			if (!invoices.IsDeleted && invoices.ItemID != -1)
			{
				result += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(invoices.Price, invoices.Quantity);
			}
		}
		return result;
	}

	private bool GetEnvFeeUse()
	{
		decimal num = 0m;
		foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
		{
			num += invoices.EnvFeeSumma;
		}
		return !Convert.ToBoolean(num == 0m);
	}

	private void CalculateEnvironmentalFees()
	{
		foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
		{
			if (invoices.ItemType == _goodsTypes_Goods)
			{
				invoices.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(invoices.Summa, invoices.EnvFeePercent, invoices.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			}
		}
	}

	private void TotalSumma_EditValueChanged(object sender, EditValueChangingEventArgs e)
	{
		decimal totalSumm = 0m;
		decimal vatSumm = 0m;
		if (!_validateTotalSumm)
		{
			return;
		}
		try
		{
			CalcInvoiceTotalAndVATSumm(ref totalSumm, ref vatSumm);
			if (_currExChngRate.DoValidate() && Math.Abs(Convert.ToDecimal(e.NewValue) - totalSumm) * _currExChngRate.Rate / _currExChngRate.Base > 100m)
			{
				e.IsCancel = true;
				e.Handled = true;
				_totalSumma.Value = Convert.ToDecimal(e.OldValue);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BigDifference, 100m));
			}
			_vatSumma.Value = vatSumm + Convert.ToDecimal(e.NewValue) - totalSumm;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void PriceListType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = false;
		string priceListRowVATType = "";
		_priceListTypeRoundDgr = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(_priceListType.Code);
		string priceListRowCurr;
		if (_isAskQuestionConfirmSummChange && !string.IsNullOrWhiteSpace(_priceListType.Code) && _priceListType.DoValidate())
		{
			if (_totalSumma.Value != 0m)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
				if (ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_priceListType.Code, out priceListRowVATType, out priceListRowCurr))
				{
					_vatType.Code = priceListRowVATType;
				}
			}
		}
		if (flag)
		{
			if (!_date.DoValidate())
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			UpdateMTInvoicePrices(_date.EditValue.Value, _priceListType.Code, _currExChngRate.Rate, _currExChngRate.Base);
			ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_priceListType.Code, out priceListRowVATType, out priceListRowCurr);
			if (priceListRowVATType != _vatType.Code)
			{
				VATTypeValidate(priceListRowVATType, _vatType.Code);
			}
			if (!string.IsNullOrWhiteSpace(DiscountPriceListType))
			{
				for (int i = 0; i < _invoicesList.Count; i++)
				{
					MTInvoiceSpecificationRow mtInvGridRow = _invoicesList[i];
					CalculateGridRowDiscountPrice(ref mtInvGridRow);
					CalculateGridRowSummaAndDiscountSumma(ref mtInvGridRow, mtInvGridRow.DiscountPrice == 0m);
				}
			}
			else
			{
				CalculateDiscount(_invoicesList, Convert.ToDecimal(_discountPercent.Value), gDiscount: false, calcInvFee: true, _vatType.Code);
			}
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			CalcSummAndSummVAT();
			_invoiceGrid.Grid.RefreshData();
		}
		_rowInfoChange?.Invoke((MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void UpdateMTInvoicePrices(DateOnly docDate, string priceListType, decimal currRate, decimal currBase)
	{
		ExchangeRate exchangeRate = new ExchangeRate();
		ExchangeRate exchangeRate2 = new ExchangeRate();
		exchangeRate2.Rate = currRate;
		exchangeRate2.Base = currBase;
		string priceListRowVATType = "";
		string priceListRowCurr = "";
		if (!_invoicesList.Any())
		{
			return;
		}
		ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(priceListType, out priceListRowVATType, out priceListRowCurr);
		if (priceListRowCurr != "AMD")
		{
			exchangeRate = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(priceListRowCurr, docDate);
		}
		foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
		{
			PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(priceListType, invoices.ItemID, invoices.UnitCode, docDate);
			if (priceListRowCurr == _currCode.Code)
			{
				invoices.Price = price.Price;
				invoices.Price = Methods.RoundPrice(invoices.Price, _priceListTypeRoundDgr);
			}
			else if (priceListRowCurr == "AMD")
			{
				invoices.Price = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(price.Price, exchangeRate2);
			}
			else
			{
				invoices.Price = (price.Price * exchangeRate.Rate / exchangeRate.Base / currRate * currBase).RoundAs(2);
			}
		}
	}

	private void VATTypeValidate(string vatTypeOldValue, string vatTypeNewValue)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		_validateTotalSumm = false;
		if (_invoicesList.Count > 0)
		{
			switch (vatTypeNewValue)
			{
			case "1":
				if (vatTypeOldValue == "3" || vatTypeOldValue == "4")
				{
					foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
					{
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(invoices.Summa, "1", invoices.VAT.Value);
						num2 += invoices.Summa;
					}
				}
				else
				{
					foreach (MTInvoiceSpecificationRow invoices2 in _invoicesList)
					{
						if (invoices2.VAT.Value)
						{
							invoices2.Price -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(invoices2.Price, vatTypeOldValue, invoices2.VAT.Value);
							invoices2.Price = Methods.RoundPrice(invoices2.Price, _priceListTypeRoundDgr);
							invoices2.DiscountPrice = (invoices2.Price * (1m - invoices2.Discount / 100m)).RoundAs(2);
							invoices2.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(invoices2.DiscountPrice, invoices2.Quantity);
							num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(invoices2.Summa, "1", invoices2.VAT.Value);
						}
						num2 += invoices2.Summa;
					}
				}
				_vatSumma.EditValue = num;
				_totalSumma.EditValue = num2 + num;
				break;
			case "2":
				switch (vatTypeOldValue)
				{
				case "3":
				case "4":
					foreach (MTInvoiceSpecificationRow invoices3 in _invoicesList)
					{
						if (invoices3.VAT.Value)
						{
							num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(invoices3.Summa, "1", invoices3.VAT.Value);
							invoices3.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(invoices3.Price, "1", invoices3.VAT.Value);
							invoices3.Price = Methods.RoundPrice(invoices3.Price, _priceListTypeRoundDgr);
							invoices3.DiscountPrice = (invoices3.Price * (1m - invoices3.Discount / 100m)).RoundAs(2);
							invoices3.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(invoices3.DiscountPrice, invoices3.Quantity);
						}
						num2 += invoices3.Summa;
					}
					_vatSumma.EditValue = num;
					_totalSumma.EditValue = num2;
					break;
				case "1":
					foreach (MTInvoiceSpecificationRow invoices4 in _invoicesList)
					{
						if (invoices4.VAT.Value)
						{
							num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(invoices4.Summa, "1", invoices4.VAT.Value);
							invoices4.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(invoices4.Price, "1", invoices4.VAT.Value);
							invoices4.Price = Methods.RoundPrice(invoices4.Price, _priceListTypeRoundDgr);
							invoices4.DiscountPrice = (invoices4.Price * (1m - invoices4.Discount / 100m)).RoundAs(2);
							invoices4.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(invoices4.DiscountPrice, invoices4.Quantity);
						}
						num2 += invoices4.Summa;
					}
					_vatSumma.EditValue = num;
					_totalSumma.EditValue = num2;
					break;
				}
				break;
			case "3":
			case "4":
				if (vatTypeOldValue == "2")
				{
					foreach (MTInvoiceSpecificationRow invoices5 in _invoicesList)
					{
						if (invoices5.VAT.Value)
						{
							invoices5.Price -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(invoices5.Price, vatTypeOldValue, invoices5.VAT.Value);
							invoices5.Price = Methods.RoundPrice(invoices5.Price, _priceListTypeRoundDgr);
							invoices5.DiscountPrice = (invoices5.Price * (1m - invoices5.Discount / 100m)).RoundAs(2);
							invoices5.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(invoices5.DiscountPrice, invoices5.Quantity);
						}
						num2 += invoices5.Summa;
					}
					_vatSumma.EditValue = 0m;
					_totalSumma.EditValue = num2;
				}
				else
				{
					if (!(vatTypeOldValue == "1"))
					{
						break;
					}
					foreach (MTInvoiceSpecificationRow invoices6 in _invoicesList)
					{
						num2 += invoices6.Summa;
					}
					_vatSumma.EditValue = 0m;
					_totalSumma.EditValue = num2;
				}
				break;
			}
		}
		if (_useEnvFees && (vatTypeNewValue == "2" || vatTypeOldValue == "2"))
		{
			CalculateEnvironmentalFees();
		}
		CalculatePartSumma();
		_validateTotalSumm = true;
	}

	private void On_DiscountClick()
	{
		if (_discount == null || !_discount.IsEnabled)
		{
			return;
		}
		if (_invoicesList.Count != 0)
		{
			if (!_useDiscountExtension)
			{
				bool flag = true;
				if (!_anyAutoDiscountModeIsEnabled)
				{
					flag = ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ComfirmDiscountCalc, default(ReadOnlySpan<object>)), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes;
				}
				if (flag)
				{
					AutoCalculateDiscount();
					_discountLogsList.Clear();
					foreach (DiscountLogRow discountLogs in PO.DiscountLogsList)
					{
						_discountLogsList.Add(discountLogs);
					}
					CalcSummAndSummVAT();
					_invoiceGrid.Grid.RefreshData();
					_rowInfoChange?.Invoke((MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem);
				}
			}
			else
			{
				try
				{
					FillDocument(new StoreRequest());
				}
				catch (Exception ex)
				{
					Settings.ErrorHandler(ex);
				}
			}
		}
		else
		{
			_discountLogsList.Clear();
		}
		_invoiceTableView.CommitEditing();
	}

	private void AutoCalculateDiscount()
	{
		try
		{
			for (int i = _invoicesList.Count - 1; i >= 0; i += -1)
			{
				if (_invoicesList[i].IsGift)
				{
					_invoicesList.RemoveAt(i);
				}
			}
			ExchangeRate exchangeRate = new ExchangeRate();
			exchangeRate.Rate = PO.ExchangeRate.Rate;
			exchangeRate.Base = PO.ExchangeRate.Base;
			_discountsCalculator.DocInfo = new DocumentInfo
			{
				DocDate = _date.EditValue.Value,
				PartnerCode = _buyerContractContainer.PartnerCode.Code,
				PartnerContract = _buyerContractContainer.Contract.Code,
				CurrBase = PO.ExchangeRate.Base,
				CurrRate = PO.ExchangeRate.Rate,
				CurrCode = _currCode.Code,
				TotalAmount = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(CalcBeginSumm(), exchangeRate),
				VATType = _vatType.Code,
				PriceType = _priceListType.Code,
				PriceListTypeRoundDgr = _priceListTypeRoundDgr,
				Storage = _storage.Code,
				SaleConsultant = _salesConsultant.Code,
				Comment = _comment.Text,
				Rows = _invoicesList
			};
			_discountsCalculator.InitializeAndCalculate();
			if (_discountsCalculator.Log != null)
			{
				PO.DiscountLogsList = _discountsCalculator.Log;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (!_date.DoValidate())
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			PO.DocDate = _date.EditValue.Value;
			PO.VATType = _vatType.Code;
			PO.CurrCode = _currCode.Code;
			FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
			{
				FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true),
				SearchDataView = new GoodsAvailabilityFilterForInvoices(_date.EditValue.Value, _storage.Code, _priceListType.Code, retail: false, "", "", extendedView: true, isFilteredSearchView: true),
				ResultsList = new MTInvoiceBaseFilteredSearchList<MTInvoiceSpecificationRow>(_currCode.Code, _vatType.Code, _date.EditValue.Value, showSalePrice: true, _priceListType.Code)
			}, this, isBasketMode: true);
			if (filteredSearchWindow.ShowDialog().Value)
			{
				foreach (MTInvoiceSpecificationRow item in filteredSearchWindow.BasketResult)
				{
					MTInvoiceSpecificationRow mtInvGridRow = new MTInvoiceSpecificationRow(this)
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
					if (mtInvGridRow.ItemType == _goodsTypes_Goods)
					{
						mtInvGridRow.Storage = _storage.Code;
					}
					if (!_paramUseDiscount)
					{
						mtInvGridRow.Discount = 0m;
					}
					else if (mtInvGridRow.Discount == 0m && _discountPercent.Value != 0m)
					{
						mtInvGridRow.Discount = _discountPercent.Value;
					}
					else if (!string.IsNullOrWhiteSpace(DiscountPriceListType))
					{
						CalculateGridRowDiscountPrice(ref mtInvGridRow);
						CalculateGridRowSummaAndDiscountSumma(ref mtInvGridRow, mtInvGridRow.DiscountPrice == 0m);
					}
					mtInvGridRow.DiscountPrice = (mtInvGridRow.Price - mtInvGridRow.Price * mtInvGridRow.Discount / 100m).RoundAs(2);
					mtInvGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, mtInvGridRow.Quantity);
					if (_useEnvFees && mtInvGridRow.ItemType != GoodsTypesSE.Service.Value)
					{
						mtInvGridRow.EnvFeePercent = TradeSettings.TradeCacheService.Material.GetItem(mtInvGridRow.ItemCode)?.EcoFeeRate ?? 0m;
						mtInvGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvGridRow.Summa, mtInvGridRow.EnvFeePercent, mtInvGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
					}
					else
					{
						mtInvGridRow.EnvFeePercent = 0m;
					}
					if (!string.IsNullOrWhiteSpace(_dealTypeKey))
					{
						if (_vatType.Code == "3" || _vatType.Code == "4" || !Convert.ToBoolean(mtInvGridRow.VAT))
						{
							mtInvGridRow.DealType = _dealTypeKey;
						}
						else
						{
							mtInvGridRow.DealType = string.Empty;
						}
					}
					mtInvGridRow.RowID = _maxRowID;
					_invoicesList.Add(mtInvGridRow);
					_rowInfoChange?.Invoke(mtInvGridRow);
					_maxRowID++;
				}
				if (_paramUseDiscount && _alwaysCalcDiscountAuto)
				{
					On_DiscountClick();
				}
				_invoiceGrid.Grid.RefreshData();
				CalcSummAndSummVAT();
			}
			_rowInfoChange?.Invoke((MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem);
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
			ActualQuantitiesCheckWindow actualQuantitiesCheckWindow = new ActualQuantitiesCheckWindow(this, _invoicesList, _date.EditValue.Value, _docNum.EditValue.ToString());
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
						for (int i = 0; i <= _invoicesList.Count - 1; i++)
						{
							if (item2.MTID != _invoicesList[i].ItemID || !(item2.UnitCode == _invoicesList[i].UnitCode))
							{
								continue;
							}
							MTInvoiceSpecificationRow mTInvoiceSpecificationRow = _invoicesList[i];
							decimal num = mTInvoiceSpecificationRow.Quantity + item2.VarianceQuantity;
							TradeSettings.TradeCacheService.Material.GetItem(item2.MTCode);
							MTInvoiceSpecificationRow mtInvGridRow = (MTInvoiceSpecificationRow)_invoiceGrid.Grid.GetRowByListIndex(i);
							if (num < 0m)
							{
								mTInvoiceSpecificationRow.Quantity = 0m;
								PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
								mtInvGridRow.IsPartySelected = ((mtInvGridRow.ItemType != _goodsTypes_Goods) ? ((bool?)null) : new bool?(false));
								mtInvGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, mTInvoiceSpecificationRow.Quantity);
								if (_useEnvFees && mtInvGridRow.ItemType == _goodsTypes_Goods)
								{
									mtInvGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvGridRow.Summa, mtInvGridRow.EnvFeePercent, mtInvGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
								}
								item2.VarianceQuantity = num;
								continue;
							}
							mTInvoiceSpecificationRow.Quantity = item2.ActualQuantity;
							PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
							mtInvGridRow.IsPartySelected = ((mtInvGridRow.ItemType != _goodsTypes_Goods) ? ((bool?)null) : new bool?(false));
							mtInvGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, mTInvoiceSpecificationRow.Quantity);
							if (_useEnvFees && mtInvGridRow.ItemType == _goodsTypes_Goods)
							{
								mtInvGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvGridRow.Summa, mtInvGridRow.EnvFeePercent, mtInvGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
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
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			_invoiceGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonMTSalesAnalysis.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonPrintLabel.ConvertForToolbar("t_action_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonImportFromFile.ConvertForToolbar("t_import_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonDocumentSettings.ConvertForToolbar("t_page_white_gear_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barButtonDocumentSettings = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.Settings,
			Glyph = ImageService.GetImage("page_white_gear_16.svg"),
			LargeGlyph = ImageService.GetImage("page_white_gear_32.svg")
		};
		_barButtonDocumentSettings.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonDocumentSettings_ItemClick, this).Handler;
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonDocumentSettings.CreateLink());
		_barButtonJumpTo = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barButtonPrintLabel = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.PrintLabel,
			KeyGesture = new KeyGesture((Key)55, (ModifierKeys)2),
			Glyph = ImageService.GetImage("action_16.svg"),
			LargeGlyph = ImageService.GetImage("action_32.svg")
		};
		_barManager.Items.Add(_barButtonPrintLabel);
		_barButtonPrintLabel.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonPrintLabel_ItemClick, this).Handler;
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonPrintLabel.CreateLink());
		_barButtonImportFromFile = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.Import,
			KeyGesture = new KeyGesture((Key)52, (ModifierKeys)2),
			Glyph = ImageService.GetImage("Import_16.svg"),
			LargeGlyph = ImageService.GetImage("Import_32.svg")
		};
		_barManager.Items.Add(_barButtonImportFromFile);
		_barButtonImportFromFile.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonImportFromFile_ItemClick, this).Handler;
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonImportFromFile.CreateLink());
		_barButtonMTSalesAnalysis = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepSalesAnalysis.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis);
		_barButtonMTSalesAnalysis.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonGoodsOperationsJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperations);
		_barButtonGoodsOperationsJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonPartnerOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		_barButtonPartnerOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
	}

	protected override void SetReportBarButtonsProperties()
	{
		base.SetReportBarButtonsProperties();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString().PadLeft(2, '0'));
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
		ReportBarButtonItem barButtonMTSalesAnalysis = _barButtonMTSalesAnalysis;
		num = 3;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
		span2[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span2[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span2[2] = new PropertyWithValue("ShowNCCVATSumm", true.ToString());
		barButtonMTSalesAnalysis.PropertyWithValues = list2;
		ReportBarButtonItem barButtonGoodsOperationsJournal = _barButtonGoodsOperationsJournal;
		num = 2;
		List<PropertyWithValue> list3 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<PropertyWithValue> span3 = CollectionsMarshal.AsSpan(list3);
		span3[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span3[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonGoodsOperationsJournal.PropertyWithValues = list3;
		ReportBarButtonItem barButtonPartnerOperationJournal = _barButtonPartnerOperationJournal;
		num = 3;
		List<PropertyWithValue> list4 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list4, num);
		Span<PropertyWithValue> span4 = CollectionsMarshal.AsSpan(list4);
		span4[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span4[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span4[2] = new PropertyWithValue("ShowLinks", true.ToString());
		barButtonPartnerOperationJournal.PropertyWithValues = list4;
	}

	private void AddShowDiscCalcItemToRibbon()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		_barShowDiscCalcSteps = new BarCheckItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.ShowDiscCalcData,
			Glyph = ImageService.GetImage("table_discount_16.svg"),
			LargeGlyph = ImageService.GetImage("table_discount_32.svg"),
			IsVisible = true,
			IsThreeState = false
		};
		_ribbonPageDiscounts = new RibbonPageGroup
		{
			Caption = ArmSoft.SME.Trade.Common.Resources.Discounts
		};
		_ribbonPageDiscounts.ItemLinks.Add(_barShowDiscCalcSteps.CreateLink());
		AddNewItemOnRibbonPageHome(_ribbonPageDiscounts);
		_barShowDiscCalcSteps.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarShowDiscCalcSteps_CheckedChanged, this).Handler;
	}

	private void BarShowDiscCalcSteps_CheckedChanged(object sender, ItemClickEventArgs e)
	{
		_showDiscCalcSteps = _barShowDiscCalcSteps.IsChecked.Value;
		if (_showDiscCalcSteps)
		{
			((DXTabItem)(object)_lgDiscountLogs.Parent).Visibility = Visibility.Visible;
			_tabs.SelectedContainer = (DXTabItem)(object)_lgDiscountLogs.Parent;
		}
		else
		{
			((DXTabItem)(object)_lgDiscountLogs.Parent).Visibility = Visibility.Collapsed;
		}
	}

	private void BarButtonDocumentSettings_ItemClick(object sender, ItemClickEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Common.Resources.Settings,
			Owner = this
		};
		dialogWindow.AddBooleanControl(null, "_showGoodsListWIthPrices", ArmSoft.SME.Trade.Common.Resources.ShowGoodsListWithPrices).IsChecked = _showGoodsListWIthPrices;
		if (Convert.ToBoolean(dialogWindow.ShowDialog()))
		{
			_showGoodsListWIthPrices = (bool)dialogWindow["_showGoodsListWIthPrices"];
		}
	}

	protected void BarButtonPrintLabel_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (_invoicesList.Count > 0)
		{
			FillDocument(new StoreRequest());
			new PrintLabelDialog(this, PO).Invoke();
		}
	}

	private void BarButtonImportFromFile_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			if (_date.IsReadOnly || !_date.DoValidate())
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(_storage.Code))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrorStorageFromFile);
			}
			if (_invoicesList.Count <= 0 || ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.LoadMaterialsList, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.No)
			{
				_invoicesList.Clear();
				PO.MTExpDocSelectedPartiesList.Clear();
				_maxRowID = 0;
				TerminalHelper.TerminalImporter terminalImporter = new TerminalHelper.TerminalImporter(this);
				if (terminalImporter.ImportFromFile((decimal quantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow) =>
				{
					AddMTToListNewRow(goodRow, goodBarcodeRow.QntUnit, TradeSettings.TradeCacheService.QntUnit.GetItem(goodBarcodeRow.QntUnit).Brief, quantity);
				}))
				{
					CalcSummAndSummVAT();
					terminalImporter.ShowReport();
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void CreateChildDocument(short childDocType, MTInvoice baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		CreateBasedOnDocHelper.CreateDocument((ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)(SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
	}

	protected override bool AllowPrint()
	{
		return true;
	}

	protected override bool AllowPrintOnNew()
	{
		return true;
	}

	protected override bool AllowChangeGridMode()
	{
		return true;
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		rk.SetValue("InfoPanelCollapsed", _infoDisplayControls.InfoPanelCollapsed ? "1" : "0");
		rk.SetValue("_showDiscCalcSteps", _showDiscCalcSteps ? "1" : "0");
		rk.SetValue("_showGoodsListWIthPrices", _showGoodsListWIthPrices ? "1" : "0");
		_invoiceGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		if (rk != null)
		{
			_infoDisplayControls.InfoPanelCollapsed = rk.GetValue("InfoPanelCollapsed", false).ToString() == "1";
			_showDiscCalcSteps = rk.GetValue("_showDiscCalcSteps", false).ToString() == "1";
			_showGoodsListWIthPrices = rk.GetValue("_showGoodsListWIthPrices", true).ToString() == "1";
			_barShowDiscCalcSteps.IsChecked = _showDiscCalcSteps;
			if (_showDiscCalcSteps)
			{
				(_lgDiscountLogs.Parent as DXTabItem).Visibility = Visibility.Visible;
			}
			_invoiceGrid.RestoreSettings(rk);
		}
	}

	private void FillPartnerEmployeeData(string cardnumber, PartnerCacheRow partner, PartnerCacheRow.ContractRow contractRow)
	{
		if (partner == null)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.PartnerCodeIsNotExist, cardnumber));
		}
		_buyerContractContainer.PartnerCode.Code = partner.PartCode;
		_buyerContractContainer.Contract.Code = contractRow.Contract;
		_currCode.Code = contractRow.CurrCode;
	}

	private void DiscountPercent_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue == null)
		{
			return;
		}
		if (Convert.ToDecimal(e.NewValue.ToString()) != Convert.ToDecimal(e.OldValue.ToString()) && _invoicesList.Count > 0 && !_anyAutoDiscountModeIsEnabled)
		{
			if (Convert.ToDecimal(e.OldValue) != 0m || GetDiscountUse())
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ComfirmDiscountChange, default(ReadOnlySpan<object>)), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					CalculateDiscount(_invoicesList, Convert.ToDecimal(e.NewValue), gDiscount: true, calcInvFee: true, _vatType.Code);
					CalcSummAndSummVAT();
					_invoiceGrid.Grid.RefreshData();
				}
				else
				{
					_discountPercent.EditValue = Convert.ToDecimal(e.OldValue);
					_discountPercent.DoValidate();
				}
			}
			else
			{
				CalculateDiscount(_invoicesList, Convert.ToDecimal(e.NewValue), gDiscount: true, calcInvFee: true, _vatType.Code);
				CalcSummAndSummVAT();
				_invoiceGrid.Grid.RefreshData();
			}
		}
		else
		{
			CalculateDiscount(_invoicesList, Convert.ToDecimal(e.NewValue), gDiscount: true, calcInvFee: true, _vatType.Code);
			CalcSummAndSummVAT();
			_invoiceGrid.Grid.RefreshData();
		}
	}

	protected override void CalculateDiscount(MTInvoiceBaseSpecificationRow row, bool calcInvFee, string vatType)
	{
		base.CalculateDiscount(row, calcInvFee, vatType);
		MTInvoiceSpecificationRow mTInvoiceSpecificationRow = (MTInvoiceSpecificationRow)row;
		if (calcInvFee && row.ItemType == _goodsTypes_Goods)
		{
			mTInvoiceSpecificationRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(row.Summa, mTInvoiceSpecificationRow.EnvFeePercent, row.VAT.Value, vatType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
		}
	}

	private bool GetDiscountUse()
	{
		decimal num = 0m;
		foreach (MTInvoiceSpecificationRow invoices in _invoicesList)
		{
			num += Math.Abs(invoices.Price - invoices.DiscountPrice);
		}
		return !Convert.ToBoolean(num == 0m);
	}

	public override void OnScanner(string scannerData)
	{
		if (!_barCode.IsReadOnly)
		{
			_barCode.Text = MarkingHelper.CleanScannerSymbols(scannerData);
			ScannerAdd();
		}
	}

	public override void OnMagneticCardReader(string mcrData)
	{
		try
		{
			if (!string.IsNullOrWhiteSpace(mcrData) && MCRInfo.GetInstance().IsValidCard(mcrData))
			{
				string cardNumber = MCRInfo.GetInstance().GetCardNumber(mcrData);
				ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(cardNumber, out var partnerCacheRow, out var partnerContract, out var _);
				FillPartnerEmployeeData(cardNumber, partnerCacheRow, partnerContract);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void OnF2()
	{
		if (!_barCode.IsReadOnly)
		{
			ControlsHelper.SetFocus(_barCode);
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
				IDropDownView dropDownView = null;
				dropDownView = ((string.IsNullOrWhiteSpace(_priceListType.Code) || !_showGoodsListWIthPrices) ? ((IDropDownView)new GoodsFilter(shortMode: true, _goodsTypes_Goods)) : ((IDropDownView)new GoodsWithPrices(_date.EditValue.Value, _priceListType.Code, "")));
				ModalBrowser modalBrowser = new ModalBrowser(dropDownView)
				{
					Owner = this,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};
				if (!modalBrowser.Show())
				{
					_barCode.Text = "";
					_realText = "";
					ControlsHelper.SetFocus(_barCode);
					return;
				}
				text = dropDownView.ValueMember(modalBrowser.SelectedRowIndex);
			}
			if (text.Length > 20)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrMTCodeLen, 20, text));
			}
			MTInvoiceSpecificationRow mTInvoiceSpecificationRow = null;
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
			if (item == null)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTEgineInavidMaterial, text));
			}
			mTInvoiceSpecificationRow = AddMTToList(item);
			if (_tabs.SelectedIndex != _priceListTabControlIndex)
			{
				_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
			}
			_barCode.Text = "";
			_realText = "";
			if (_immediatlyOpenMarkingWindow && MarkingHelpers.IsGoodMarkable(mTInvoiceSpecificationRow.ItemCode, _useMarking, mTInvoiceSpecificationRow.MarkingList.Count != 0, _date.EditValue.Value, out var isRowMarkable))
			{
				mTInvoiceSpecificationRow.IsRowMarkable = isRowMarkable;
				List<string> allMarkings = _invoicesList.SelectMany((MTInvoiceSpecificationRow row) => row.MarkingList).ToList();
				MarkingSelectWindow markingSelectWindow = new MarkingSelectWindow(this, _readOnlyMarkingWindow, mTInvoiceSpecificationRow.IsRowMarkable, mTInvoiceSpecificationRow.ItemCode, mTInvoiceSpecificationRow.ItemName, mTInvoiceSpecificationRow.MarkingList, allMarkings, mTInvoiceSpecificationRow.Quantity);
				if (markingSelectWindow.ShowDialog().Value && !markingSelectWindow.IsViewOnlyMode)
				{
					mTInvoiceSpecificationRow.MarkingList.Clear();
					foreach (MarkingSelectWindow.MarkingRow marking in markingSelectWindow.MarkingList)
					{
						mTInvoiceSpecificationRow.MarkingList.Add(marking.Marking);
					}
					if (mTInvoiceSpecificationRow.Quantity < (decimal)mTInvoiceSpecificationRow.MarkingList.Count)
					{
						MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(mTInvoiceSpecificationRow.ItemCode);
						if (item2 == null || !item2.IsWeight)
						{
							mTInvoiceSpecificationRow.Quantity = mTInvoiceSpecificationRow.MarkingList.Count;
							ChangeRowQty(mTInvoiceSpecificationRow);
						}
					}
					ChangeMarkingStatus(mTInvoiceSpecificationRow);
				}
			}
			CalcSummAndSummVAT();
			_vatSumma.DoValidate();
			ControlsHelper.SetFocus(_barCode);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CodeEnter_Click(object sender, RoutedEventArgs e)
	{
		On_CodeEnterClick();
	}

	private void BarCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		_realText += e.Text;
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
				e.Handled = true;
				_barCode.Text = MarkingHelper.CleanScannerSymbols(_realText.Contains('\u001d') ? _realText : _barCode.Text);
				ScannerAdd();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			_realText = "";
		}
	}

	private void ScannerAdd()
	{
		_barCode.DoValidate();
		try
		{
			MTInvoiceSpecificationRow mTInvoiceSpecificationRow = new MTInvoiceSpecificationRow();
			string barCode = _barCode.Text.Trim();
			if (Settings.ParametersService.UsePartnerEmployeeSearchNewMode())
			{
				if (ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.IsValidPartnerEmployeeCardCode(barCode, Settings.ParametersService.PartnerEmployeeCardCodePrefixes(), Settings.ParametersService.UsePartnerCardLenght(), Settings.ParametersService.PartnerCardLenght()))
				{
					_barCode.Text = "";
					_realText = "";
					_barCode.Focus();
					ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(barCode, out var partnerCacheRow, out var partnerContract, out var _);
					FillPartnerEmployeeData(barCode, partnerCacheRow, partnerContract);
					return;
				}
			}
			else if (MCRInfo.GetInstance().IsValidCard(barCode))
			{
				_barCode.Text = "";
				_realText = "";
				_barCode.Focus();
				ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(MCRInfo.GetInstance().GetCardNumber(barCode), out var partnerCacheRow2, out var partnerContract2, out var _);
				FillPartnerEmployeeData(barCode, partnerCacheRow2, partnerContract2);
				return;
			}
			decimal qty = 1m;
			string unitCode = "";
			string unitBrief = "";
			if (barCode.Length < 29)
			{
				MaterialCacheRow materialCacheRow = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.TryGetItemByBarCode(ref barCode, out qty, out unitCode, out unitBrief);
				if (materialCacheRow == null)
				{
					ControlsHelper.SetFocus(_barCode);
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeNotExist, barCode));
				}
				if (_useMarking && !materialCacheRow.IsWeight && materialCacheRow.CheckMarking)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ScanDataMatrix);
				}
				if (_tabs.SelectedIndex != _priceListTabControlIndex)
				{
					_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
				}
				mTInvoiceSpecificationRow = AddMTToList(materialCacheRow, unitCode, unitBrief, qty);
				_barCode.Text = "";
				_realText = "";
				if (_useMarking && _immediatlyOpenMarkingWindow && MarkingHelpers.IsGoodMarkable(mTInvoiceSpecificationRow.ItemCode, _useMarking, mTInvoiceSpecificationRow.MarkingList.Any(), _date.EditValue.Value, out var isRowMarkable))
				{
					mTInvoiceSpecificationRow.IsRowMarkable = isRowMarkable;
					List<string> allMarkings = _invoicesList.SelectMany((MTInvoiceSpecificationRow row) => row.MarkingList).ToList();
					MarkingSelectWindow markingSelectWindow = new MarkingSelectWindow(this, _readOnlyMarkingWindow, mTInvoiceSpecificationRow.IsRowMarkable, mTInvoiceSpecificationRow.ItemCode, mTInvoiceSpecificationRow.ItemName, mTInvoiceSpecificationRow.MarkingList, allMarkings, mTInvoiceSpecificationRow.Quantity);
					if (markingSelectWindow.ShowDialog().Value && !markingSelectWindow.IsViewOnlyMode)
					{
						mTInvoiceSpecificationRow.MarkingList.Clear();
						foreach (MarkingSelectWindow.MarkingRow marking in markingSelectWindow.MarkingList)
						{
							mTInvoiceSpecificationRow.MarkingList.Add(marking.Marking);
						}
						if (mTInvoiceSpecificationRow.Quantity < (decimal)mTInvoiceSpecificationRow.MarkingList.Count)
						{
							MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTInvoiceSpecificationRow.ItemCode);
							if (item == null || !item.IsWeight)
							{
								mTInvoiceSpecificationRow.Quantity = mTInvoiceSpecificationRow.MarkingList.Count;
								ChangeRowQty(mTInvoiceSpecificationRow);
							}
						}
					}
				}
			}
			else
			{
				if (!_useMarking)
				{
					_barCode.Text = "";
					_realText = "";
					_barCode.Focus();
					throw new ASException(ArmSoft.SME.Common.Resources.MarkingNotActive);
				}
				string eanBarCode = "";
				string text = _barCode.Text;
				MaterialCacheRow goodFromScanedMark = GetGoodFromScanedMark(ref eanBarCode, out var skipMarking);
				if (skipMarking)
				{
					return;
				}
				if (goodFromScanedMark == null)
				{
					_barCode.Text = "";
					_realText = "";
					ControlsHelper.SetFocus(_barCode);
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodesNotExist, eanBarCode));
				}
				if (!MarkingHelpers.IsGoodMarkable(goodFromScanedMark.MTCode, _useMarking, markingListHasValue: false, _date.EditValue.Value, out var isRowMarkable2))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.CPAEmptyOrNotMarkable);
				}
				mTInvoiceSpecificationRow.IsRowMarkable = isRowMarkable2;
				mTInvoiceSpecificationRow = AddMTToList(goodFromScanedMark);
				if (mTInvoiceSpecificationRow != null && mTInvoiceSpecificationRow.ItemType == GoodsTypesSE.Good.Value && !string.IsNullOrWhiteSpace(eanBarCode) && _useMarking && MarkingHelpers.IsGoodMarkable(mTInvoiceSpecificationRow.ItemCode, _useMarking, mTInvoiceSpecificationRow.MarkingList.Count != 0, _date.EditValue.Value, out isRowMarkable2))
				{
					mTInvoiceSpecificationRow.IsRowMarkable = isRowMarkable2;
					mTInvoiceSpecificationRow.MarkingList.Add(text);
				}
				_barCode.Text = "";
				_realText = "";
				if (_tabs.SelectedIndex != _priceListTabControlIndex)
				{
					_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
				}
			}
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			CalcSummAndSummVAT();
			_vatSumma.DoValidate();
			ChangeMarkingStatus(mTInvoiceSpecificationRow);
			_rowInfoChange?.Invoke(mTInvoiceSpecificationRow);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private MaterialCacheRow GetGoodFromScanedMark(ref string eanBarCode, out bool skipMarking)
	{
		skipMarking = false;
		if (_useMarking)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(_barCode.Text);
			for (int i = 0; i <= bytes.Length - 1; i += 2)
			{
				if (((bytes[i + 1] << 8) | bytes[i]) > 127)
				{
					_barCode.Text = "";
					_realText = "";
					ControlsHelper.SetFocus(_barCode);
					throw new ASException(ArmSoft.SME.Common.Resources.KeyboardLanguageForMarkingScanShouldBeEnglish);
				}
			}
			MTInvoiceSpecificationRow invoiceRow = _invoicesList.FirstOrDefault((MTInvoiceSpecificationRow r) => r.MarkingList.Contains(_barCode.Text));
			if (invoiceRow != null)
			{
				ScannerMessageBoxWindow scannerMessageBoxWindow = new ScannerMessageBoxWindow(ArmSoft.SME.Trade.Common.Resources.ConfirmDeleteDataMatrix, this);
				if (scannerMessageBoxWindow.ShowDialog().Value)
				{
					invoiceRow.MarkingList.Remove(_barCode.Text);
					if (invoiceRow.Quantity >= 1m && !invoiceRow.IsDeleted && !invoiceRow.IsGift)
					{
						MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(invoiceRow.ItemCode);
						if (item == null || !item.IsWeight)
						{
							invoiceRow.Quantity--;
							if (invoiceRow.Quantity == 0m)
							{
								PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == invoiceRow.RowID);
								_invoicesList.Remove(invoiceRow);
								invoiceRow = null;
							}
							else
							{
								ChangeRowQty(invoiceRow);
							}
							CalcSummAndSummVAT();
							if (_currCode.Code == "AMD")
							{
								PO.SummNCC = _totalSumma.Value;
							}
							else
							{
								ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
								PO.ExchangeRate.Rate = exchangeRates.Rate;
								PO.ExchangeRate.Base = exchangeRates.Base;
								PO.SummNCC = (_totalSumma.Value * PO.ExchangeRate.Rate / PO.ExchangeRate.Base).RoundAs(2);
							}
						}
					}
					ChangeMarkingStatus(invoiceRow);
					_rowInfoChange?.Invoke(invoiceRow);
				}
				_barCode.Text = "";
				_realText = "";
				ControlsHelper.SetFocus(_barCode);
				if (!string.IsNullOrWhiteSpace(scannerMessageBoxWindow.ScannerData))
				{
					_barCode.Text = scannerMessageBoxWindow.ScannerData;
					ScannerAdd();
				}
				skipMarking = true;
				return null;
			}
			MarkingHelper.ValidateMarkingCode(_barCode.Text);
			eanBarCode = MarkingHelpers.ExtractGTINFromDataMatrix(_barCode.Text, out var good);
			return good;
		}
		return null;
	}

	private MTInvoiceSpecificationRow AddMTToList(MaterialCacheRow goodsRow, string unitCode = "", string unitBrief = "", decimal quantity = 1m)
	{
		MTInvoiceSpecificationRow mTInvoiceSpecificationRow = ((!(_rowAddMode == "3")) ? AddNewRowOrMergeInner(goodsRow, _rowAddMode == "2", string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity) : AddMTToListNewRow(goodsRow, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity));
		ChangeMarkingStatus(mTInvoiceSpecificationRow);
		_rowInfoChange?.Invoke(mTInvoiceSpecificationRow);
		return mTInvoiceSpecificationRow;
	}

	private MTInvoiceSpecificationRow AddNewRowOrMergeInner(MaterialCacheRow goodsRow, bool forFocusedRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTInvoiceSpecificationRow mTInvoiceSpecificationRow = null;
		if (forFocusedRow && _invoicesList.Count != 0)
		{
			mTInvoiceSpecificationRow = (MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem;
			if (mTInvoiceSpecificationRow == null)
			{
				return null;
			}
		}
		for (int i = 0; i <= _invoicesList.Count - 1; i++)
		{
			if ((!forFocusedRow || mTInvoiceSpecificationRow.RowID == _invoicesList[i].RowID) && goodsRow.Id == _invoicesList[i].ItemID && unitCode == _invoicesList[i].UnitCode && (_invoicesList[i].ItemType == GoodsTypesSE.Service.Value || (_invoicesList[i].ItemType == _goodsTypes_Goods && _storage.Code == _invoicesList[i].Storage && !_invoicesList[i].IsPartySelected.Value)))
			{
				FillMTInvGridRow(_invoicesList[i], goodsRow, unitCode, unitBrief, quantity);
				int rowHandleByListIndex = _invoiceGrid.Grid.GetRowHandleByListIndex(i);
				POWindow<MTInvoice>.SelectCellInGrid(_invoiceTableView, _invoiceGrid, rowHandleByListIndex, "Quantity");
				return (MTInvoiceSpecificationRow)_invoiceTableView.Grid.CurrentItem;
			}
		}
		return AddMTToListNewRow(goodsRow, unitCode, unitBrief, quantity);
	}

	private MTInvoiceSpecificationRow AddMTToListNewRow(MaterialCacheRow sourceRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTInvoiceSpecificationRow mTInvoiceSpecificationRow = new MTInvoiceSpecificationRow(this);
		FillMTInvGridRow(mTInvoiceSpecificationRow, sourceRow, unitCode, unitBrief, quantity);
		mTInvoiceSpecificationRow.RowID = _maxRowID;
		_invoicesList.Add(mTInvoiceSpecificationRow);
		_maxRowID++;
		int rowHandleByListIndex = _invoiceGrid.Grid.GetRowHandleByListIndex(_invoicesList.Count - 1);
		POWindow<MTInvoice>.SelectCellInGrid(_invoiceTableView, _invoiceGrid, rowHandleByListIndex, "Quantity");
		_invoiceGrid.ConfigureButtons();
		_rowInfoChange?.Invoke(mTInvoiceSpecificationRow);
		return mTInvoiceSpecificationRow;
	}

	private void FillMTInvGridRow(MTInvoiceSpecificationRow mtInvGridRow, MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		if (goodsRow == null)
		{
			return;
		}
		mtInvGridRow.ItemType = goodsRow.Type;
		if (mtInvGridRow.ItemType == _goodsTypes_Goods)
		{
			mtInvGridRow.Storage = _storage.Code;
			mtInvGridRow.IsPartySelected = false;
		}
		else
		{
			mtInvGridRow.Storage = "";
		}
		mtInvGridRow.ItemCode = goodsRow.MTCode;
		mtInvGridRow.ItemID = goodsRow.Id;
		mtInvGridRow.ItemName = goodsRow.Caption;
		mtInvGridRow.UnitCode = unitCode;
		mtInvGridRow.UnitBrief = unitBrief;
		mtInvGridRow.VAT = goodsRow.VAT;
		mtInvGridRow.Quantity += quantity;
		if (mtInvGridRow.Price == 0m)
		{
			mtInvGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mtInvGridRow.ItemID, mtInvGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
			mtInvGridRow.Price = Methods.RoundPrice(mtInvGridRow.Price, _priceListTypeRoundDgr);
		}
		if (_paramUseDiscount)
		{
			if (string.IsNullOrWhiteSpace(DiscountPriceListType))
			{
				mtInvGridRow.Discount = _discountPercent.Value;
				if (mtInvGridRow.Discount == 0m || goodsRow.Discount != 0m)
				{
					mtInvGridRow.Discount = goodsRow.Discount;
				}
				mtInvGridRow.DiscountPrice = (mtInvGridRow.Price - mtInvGridRow.Price * mtInvGridRow.Discount / 100m).RoundAs(2);
			}
			else
			{
				CalculateGridRowDiscountPriceInner(ref mtInvGridRow);
				CalculateGridRowSummaAndDiscountSumma(ref mtInvGridRow, mtInvGridRow.DiscountPrice == 0m);
			}
		}
		else
		{
			mtInvGridRow.DiscountPrice = mtInvGridRow.Price;
		}
		mtInvGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, mtInvGridRow.Quantity);
		if (Settings.ParametersService.EnvironmentalFeePayer() && mtInvGridRow.ItemType == _goodsTypes_Goods)
		{
			mtInvGridRow.EnvFeePercent = goodsRow.EcoFeeRate;
			mtInvGridRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mtInvGridRow.Summa, mtInvGridRow.EnvFeePercent, mtInvGridRow.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
		}
		if (!string.IsNullOrWhiteSpace(_dealTypeKey))
		{
			if (_vatType.Code == "3" || _vatType.Code == "4" || !goodsRow.VAT)
			{
				mtInvGridRow.DealType = _dealTypeKey;
			}
			else
			{
				mtInvGridRow.DealType = string.Empty;
			}
		}
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
		_currCode.Code = BaseDocument.CurrCode;
		if ((BaseDocument.POType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value) & (BaseDocument.POType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value))
		{
			ExchangeRate exchangeRate = new ExchangeRate();
			_currExChngRate.Rate = exchangeRate.Rate;
			_currExChngRate.Base = exchangeRate.Base;
			_buyerContractContainer.PartnerCode.Code = BaseDocument.PartnerContractGroup.PartnerCode;
			_buyerContractContainer.Contract.Code = BaseDocument.PartnerContractGroup.ContractCode;
			if (!string.IsNullOrEmpty(_buyerContractContainer.PartnerCode.Code))
			{
				PartnerCacheRow itemById = TradeSettings.TradeCacheService.Partner.GetItemById(BaseDocument.PartnerID);
				if (itemById != null)
				{
					_buyerName.Text = itemById.Caption;
					_buyerLegalAddress.Text = itemById.Address;
					_buyerBusinessAddress.Text = itemById.BusinessAddress;
					_buyerSettlAcc.Code = itemById.SettlAcc;
					_buyerTaxCode.Text = itemById.TaxCode;
					_buyerMngPost.Text = itemById.ManagerPost;
					_buyerMngName.Text = itemById.ManagerFIO;
					_buyerChiefAccPost.Text = itemById.ChiefaccPost;
					_buyerChiefAccName.Text = itemById.ChiefaccFIO;
					_buyerStateRegNum.Text = itemById.PartStateRegister;
					_buyerIDDocType.Code = itemById.IDDocType;
					_buyerPassport.Text = itemById.Passport;
				}
			}
			_comment.EditValue = BaseDocument.Comment;
		}
		short pOType = BaseDocument.POType;
		if (pOType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value)
		{
			MTIncome mTIncome = (MTIncome)BaseDocument;
			_invoicesList.Clear();
			_maxRowID = 0;
			_storage.Code = mTIncome.Storage;
			_vatType.Code = Settings.ParametersService.VATType();
			_priceListType.Code = Settings.ParametersService.DefaultSalePrice();
			PO.MTExpDocSelectedPartiesList.Clear();
			foreach (MTIncomeBase.MTIncomeSpecificationRow mT in mTIncome.MTList)
			{
				MTInvoiceSpecificationRow mTInvoiceSpecificationRow = new MTInvoiceSpecificationRow(this)
				{
					RowID = _maxRowID,
					ItemType = _goodsTypes_Goods,
					Storage = BaseDocument.Storage,
					ItemID = mT.MTID,
					ItemCode = mT.MTCode,
					ItemName = mT.MTName,
					UnitCode = mT.UnitCode,
					UnitBrief = mT.UnitBrief,
					Quantity = mT.Quantity,
					VAT = mT.VAT
				};
				mTInvoiceSpecificationRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mTInvoiceSpecificationRow.ItemID, mTInvoiceSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
				mTInvoiceSpecificationRow.DiscountPrice = mTInvoiceSpecificationRow.Price;
				mTInvoiceSpecificationRow.Summa = (mTInvoiceSpecificationRow.Quantity * mTInvoiceSpecificationRow.Price).RoundAs(2);
				mTInvoiceSpecificationRow.IsPartySelected = false;
				_invoicesList.Add(mTInvoiceSpecificationRow);
				_maxRowID++;
			}
		}
		else if (pOType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value)
		{
			MTIncomeRetail mTIncomeRetail = (MTIncomeRetail)BaseDocument;
			_invoicesList.Clear();
			_maxRowID = 0;
			_storage.Code = mTIncomeRetail.Storage;
			_vatType.Code = Settings.ParametersService.VATType();
			_priceListType.Code = Settings.ParametersService.DefaultSalePrice();
			PO.MTExpDocSelectedPartiesList.Clear();
			foreach (MTIncomeBase.MTIncomeSpecificationRow mT2 in mTIncomeRetail.MTList)
			{
				MTInvoiceSpecificationRow mTInvoiceSpecificationRow2 = new MTInvoiceSpecificationRow(this)
				{
					ItemType = _goodsTypes_Goods,
					Storage = BaseDocument.Storage,
					ItemID = mT2.MTID,
					ItemCode = mT2.MTCode,
					ItemName = mT2.MTName,
					UnitCode = mT2.UnitCode,
					UnitBrief = mT2.UnitBrief,
					Quantity = mT2.Quantity,
					VAT = mT2.VAT
				};
				mTInvoiceSpecificationRow2.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mTInvoiceSpecificationRow2.ItemID, mTInvoiceSpecificationRow2.UnitCode, _date.EditValue.Value, _currCode.Code);
				mTInvoiceSpecificationRow2.DiscountPrice = mTInvoiceSpecificationRow2.Price;
				mTInvoiceSpecificationRow2.Summa = (mTInvoiceSpecificationRow2.Quantity * mTInvoiceSpecificationRow2.Price).RoundAs(2);
				mTInvoiceSpecificationRow2.IsPartySelected = false;
				mTInvoiceSpecificationRow2.RowID = _maxRowID;
				_invoicesList.Add(mTInvoiceSpecificationRow2);
				_maxRowID++;
			}
		}
		else if (pOType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTaking.Value)
		{
			MTStockTaking mTStockTaking = (MTStockTaking)BaseDocument;
			_invoicesList.Clear();
			_maxRowID = 0;
			_storage.Code = mTStockTaking.Storage;
			PO.MTExpDocSelectedPartiesList.Clear();
			MaterialCacheRow materialCacheRow = null;
			foreach (MTStockTakingBase.MTStockTakingSpecificationRow mT3 in mTStockTaking.MTList)
			{
				if (mT3.VarianceQuantity < 0m)
				{
					MTInvoiceSpecificationRow mTInvoiceSpecificationRow3 = new MTInvoiceSpecificationRow(this)
					{
						Storage = mTStockTaking.Storage,
						ItemType = _goodsTypes_Goods,
						ItemID = mT3.MTID,
						ItemCode = mT3.MTCode,
						ItemName = mT3.MTName
					};
					materialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(mTInvoiceSpecificationRow3.ItemCode);
					if (materialCacheRow != null)
					{
						mTInvoiceSpecificationRow3.VAT = materialCacheRow.VAT;
					}
					mTInvoiceSpecificationRow3.Quantity = Math.Abs(mT3.VarianceQuantity);
					mTInvoiceSpecificationRow3.UnitCode = mT3.UnitCode;
					mTInvoiceSpecificationRow3.UnitBrief = mT3.UnitBrief;
					mTInvoiceSpecificationRow3.IsPartySelected = false;
					mTInvoiceSpecificationRow3.RowID = _maxRowID;
					_maxRowID++;
					_invoicesList.Add(mTInvoiceSpecificationRow3);
				}
			}
		}
		else if (pOType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay.Value)
		{
			InvoiceForPay invoiceForPay = (InvoiceForPay)BaseDocument;
			_invoicesList.Clear();
			_maxRowID = 0;
			_storage.Code = invoiceForPay.Storage;
			_vatType.Code = invoiceForPay.VATType;
			_priceListType.Code = invoiceForPay.PriceListType;
			PO.MTExpDocSelectedPartiesList.Clear();
			foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT4 in invoiceForPay.MTList)
			{
				MTInvoiceSpecificationRow mTInvoiceSpecificationRow4 = new MTInvoiceSpecificationRow(this)
				{
					RowID = _maxRowID,
					Storage = mT4.Storage,
					IsPartySelected = ((mT4.ItemType == _goodsTypes_Goods) ? new bool?(false) : ((bool?)null)),
					ItemType = mT4.ItemType,
					ItemID = mT4.ItemID,
					ItemCode = mT4.ItemCode,
					ItemName = mT4.ItemName,
					UnitCode = mT4.UnitCode,
					UnitBrief = mT4.UnitBrief,
					Quantity = mT4.Quantity,
					VAT = mT4.VAT.Value,
					Price = mT4.Price,
					Discount = mT4.Discount,
					DiscountPrice = mT4.DiscountPrice,
					Summa = mT4.Summa
				};
				if (_useEnvFees)
				{
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTInvoiceSpecificationRow4.ItemCode);
					if (item != null)
					{
						mTInvoiceSpecificationRow4.EnvFeePercent = item.EcoFeeRate;
						mTInvoiceSpecificationRow4.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mTInvoiceSpecificationRow4.Summa, mTInvoiceSpecificationRow4.EnvFeePercent, mTInvoiceSpecificationRow4.VAT.Value, _vatType.Code, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
					}
				}
				_maxRowID++;
				_invoicesList.Add(mTInvoiceSpecificationRow4);
			}
		}
		else
		{
			short num = pOType;
			if (num == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value || num == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value)
			{
				MTMoveWithConfirmationBase mTMoveWithConfirmationBase = (MTMoveWithConfirmationBase)BaseDocument;
				_invoicesList.Clear();
				_maxRowID = 0;
				StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(mTMoveWithConfirmationBase.StorageIncome);
				if (item2 != null && item2.Retail)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.CreateBasedMTInvoiceError);
				}
				_storage.Code = mTMoveWithConfirmationBase.StorageIncome;
				PO.MTExpDocSelectedPartiesList.Clear();
				MaterialCacheRow materialCacheRow2 = null;
				foreach (MTMoveBase.MTMoveSpecificationRow mT5 in mTMoveWithConfirmationBase.MTList)
				{
					MTInvoiceSpecificationRow mTInvoiceSpecificationRow5 = new MTInvoiceSpecificationRow(this);
					mTInvoiceSpecificationRow5.Storage = mTMoveWithConfirmationBase.StorageIncome;
					mTInvoiceSpecificationRow5.ItemType = _goodsTypes_Goods;
					mTInvoiceSpecificationRow5.IsPartySelected = false;
					mTInvoiceSpecificationRow5.ItemID = mT5.MTID;
					mTInvoiceSpecificationRow5.ItemCode = mT5.MTCode;
					mTInvoiceSpecificationRow5.ItemName = mT5.MTName;
					materialCacheRow2 = TradeSettings.TradeCacheService.Material.GetItem(mTInvoiceSpecificationRow5.ItemCode);
					if (materialCacheRow2 != null)
					{
						mTInvoiceSpecificationRow5.VAT = materialCacheRow2.VAT;
					}
					mTInvoiceSpecificationRow5.UnitCode = mT5.UnitCode;
					mTInvoiceSpecificationRow5.UnitBrief = mT5.UnitBrief;
					mTInvoiceSpecificationRow5.Quantity = mT5.Quantity;
					if (_paramUseDiscount)
					{
						mTInvoiceSpecificationRow5.Discount = materialCacheRow2.Discount;
					}
					else
					{
						mTInvoiceSpecificationRow5.Discount = 0m;
					}
					mTInvoiceSpecificationRow5.EnvFeePercent = materialCacheRow2.EcoFeeRate;
					mTInvoiceSpecificationRow5.RowID = _maxRowID;
					_maxRowID++;
					_invoicesList.Add(mTInvoiceSpecificationRow5);
				}
			}
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		CalcSummAndSummVAT();
		_invoiceGrid.Grid.RefreshData();
	}

	private void InfoDisplay_Collapsed(object sender, EventArgs e)
	{
		_infoDisplayControls.InfoPanelCollapsed = true;
	}

	private void CleanInfoPanel()
	{
		_isChanged = IsChanged;
		_infoItemName.Text = "";
		_infoFormula.Text = "";
		_infoDisplayControls.ClearInfoAvailability();
		IsChanged = _isChanged;
	}

	private void SetAvailabilityInfoToPanel(MTInvoiceSpecificationRow row)
	{
		_infoDisplayControls.ClearInfoAvailability();
		if (row.ItemType == _goodsTypes_Goods)
		{
			if (string.IsNullOrWhiteSpace(row.UnitCode))
			{
				_infoDisplayControls.SetStringAccBalance("-");
				_infoDisplayControls.SetStringAccUnitBrief("");
				_infoDisplayControls.SetStringWholeBalance("-");
				_infoDisplayControls.SetStringWholeUnitBrief("");
			}
			else
			{
				InfoDisplayControls infoDisplayControls = _infoDisplayControls;
				decimal accBalance;
				if (string.IsNullOrWhiteSpace(_storage.Code))
				{
					accBalance = (string.IsNullOrWhiteSpace(row.Storage) ? 0m : MaterialHelpers.GetGoodRemains(row.UpdateRemainsOnInfoPanel, row.ItemID, row.UnitCode, ref _availabilityByGoodId, row.Storage));
				}
				else
				{
					accBalance = MaterialHelpers.GetGoodRemains(row.UpdateRemainsOnInfoPanel, row.ItemID, row.UnitCode, ref _availabilityByGoodId, _storage.Code);
				}
				infoDisplayControls.SetAccBalance(accBalance);
				_infoDisplayControls.SetAccUnitBrief(row.UnitBrief);
				_infoDisplayControls.SetWholeBalance(MaterialHelpers.GetGoodRemains(row.UpdateRemainsOnInfoPanel, row.ItemID, row.UnitCode, ref _availabilityByGoodId));
				_infoDisplayControls.SetWholeUnitBrief(row.UnitBrief);
				row.UpdateRemainsOnInfoPanel = false;
			}
		}
	}

	private void SetRowInfoToPanel(MTInvoiceSpecificationRow row)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(row.ItemCode))
			{
				CleanInfoPanel();
				return;
			}
			_isChanged = IsChanged;
			decimal num = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(row.DiscountPrice, row.Quantity);
			_infoItemName.Text = $"{row.ItemCode} - {row.ItemName}";
			TextEditExt infoFormula = _infoFormula;
			InlineArray4<object> buffer = default;
			buffer[0] = row.Quantity.CurrencyFormat(16, Convert.ToInt16((!(row.Quantity != row.Quantity)) ? 3 : 0));
			buffer[1] = row.UnitBrief;
			buffer[2] = row.DiscountPrice.CurrencyFormat(16, Convert.ToInt16((!(row.DiscountPrice != row.DiscountPrice)) ? 2 : 0));
			buffer[3] = num.CurrencyFormat(16, Convert.ToInt16((!(num != decimal.Floor(num))) ? 2 : 0));
			infoFormula.Text = string.Format("{0} {1} x {2} = {3}", (ReadOnlySpan<object?>)buffer);
			SetAvailabilityInfoToPanel(row);
			IsChanged = _isChanged;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ChangeRowInfo(MTInvoiceSpecificationRow row = null)
	{
		if (row == null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearInfoPanel(CleanInfoPanel), Array.Empty<object>());
			return;
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToInfoPanel(SetRowInfoToPanel), new object[1] { row });
	}
}
