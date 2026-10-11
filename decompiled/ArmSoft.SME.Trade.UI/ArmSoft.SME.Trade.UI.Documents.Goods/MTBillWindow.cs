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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Ardalis.SmartEnum;
using ArmSoft.Arcus2Client;
using ArmSoft.Cloud.Client.Discount;
using ArmSoft.ECRConnector;
using ArmSoft.PossoftPayClient;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Data.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.CustomTypes;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DataView;
using ArmSoft.SME.Trade.UI.Directories;
using ArmSoft.SME.Trade.UI.Directory;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.Models;
using ArmSoft.SME.Trade.UI.OtherWindows;
using ArmSoft.SME.Trade.UI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Mvvm.Native;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Ribbon;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents.Goods;

[DocumentWindow(18)]
public class MTBillWindow : MTInvoiceBaseWindow<MTBill>
{
	public class MTBillSpecificationRow : MTInvoiceBaseSpecificationRow, IMarkableRow
	{
		[ObservableProperty]
		private bool markingHasMismatch;

		internal bool _isUpdatingMarking;

		public string PartyMethodName
		{
			get
			{
				if (!IsPartySelected.HasValue)
				{
					return "";
				}
				if (IsPartySelected.Value)
				{
					return ArmSoft.SME.Common.Resources.PartySelected;
				}
				return ArmSoft.SME.Common.Resources.AutoSelection;
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

		static MTBillSpecificationRow()
		{
			Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>> invoiceBasePropertyValidators = MTInvoiceBaseSpecificationRow.GetInvoiceBasePropertyValidators();
			invoiceBasePropertyValidators.Add("Storage", (DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
			{
				ValidateStorage((MTBillSpecificationRow)row, info);
			});
			if (Settings.ParametersService.UseMarking())
			{
				invoiceBasePropertyValidators.Add("MarkingStatus", (DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
				{
					ValidateMarkingListCount((MTBillSpecificationRow)row, info);
				});
			}
			DocumentGridRow.RegisterPropertyValidators(typeof(MTBillSpecificationRow), cacheValidationResults: true, invoiceBasePropertyValidators);
		}

		private static void ValidateStorage(MTInvoiceBaseSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (row.ItemType == GoodsTypesSE.Good.Value)
			{
				TradePropertyValidator.ValidateStorage(row.Storage, info, ArmSoft.SME.Common.Resources.Storage, isRequired: true);
			}
		}

		private static void ValidateMarkingListCount(MTBillSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
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
	}

	private delegate void RowInfoChangeEventHandler(MTBillSpecificationRow row);

	private delegate void DelegateSetRowInfoToInfoPanel(MTBillSpecificationRow row);

	private delegate void DelegateSetRowInfoToPoleDisplay(MTBillSpecificationRow row);

	private delegate void DelegateSetTotalInfoToPoleDisplay(decimal total, decimal discountSum, decimal cashSum, decimal nonCashSum, decimal usedBonusSum, decimal fromPrePaySumm, decimal creditSaleSumm, decimal sumForPay);

	private delegate void DelegateSetRowCountInfoToPoleDisplay(int rowCount);

	private delegate void DelegateSetBuyerInfoToPoleDisplay(string buyerCode, string buyerName, string contractCode, decimal currentAccumulatedBonus, decimal currentBonusRemain);

	private delegate void DelegateSetChangeInfoToPoleDisplay(decimal change);

	private delegate void DelegateClearInfoPanel();

	private delegate void DelegateClearPoleDisplay(bool clearAll, string messageForPoleDisplay = "");

	private bool _allowChangeDate = Settings.ParametersService.AllowChangeDateInBill();

	private readonly bool _mtBillSimpleMode = Settings.ParametersService.MTBillSimpleMode();

	private bool _paramUseDiscount = Settings.ParametersService.DiscountUse();

	private bool _isCalcDiscountAuto = Settings.ParametersService.CalcDiscountAuto();

	private bool _alwaysCalcDiscountAuto = Settings.ParametersService.AlwaysCalcDiscountAuto();

	private bool _allowChangeSalePrices = Settings.ParametersService.AllowChangeSalePrices();

	private bool _allowChangePriceType = Settings.ParametersService.AllowChangePriceType();

	private bool _allowChangeDiscountPriceType = Settings.ParametersService.AllowChangeDiscountPriceType();

	private bool _allowChangeDiscountPrices = Settings.ParametersService.AllowChangeDiscountPrice();

	private string _defaultMTCheckDiscountPriceType = Settings.ParametersService.DefaultMTCheckDiscountPriceType();

	public decimal _usedDiscount;

	private bool _useDiscountExtension = Settings.ParametersService.DiscountExtensionUse();

	private bool _useBonusExtension = Settings.ParametersService.BonusExtensionUse();

	private bool _useMarking = Settings.ParametersService.UseMarking();

	private bool _allowUseBonus = Settings.ParametersService.UseBonus();

	private string _bonusAccumulationMethod = Settings.ParametersService.BonusAccumulationMethod();

	private string _bonusAccumulationBase = Settings.ParametersService.BonusAccumulationBase();

	private string _bonusType = Settings.ParametersService.BonusType();

	private bool _calcBonusForDiscountedItems = Settings.ParametersService.AccumulateBonusForDiscountedGoods();

	private decimal _bonusAccumulationBaseSumm = Settings.ParametersService.BonusAccumulationConditionSumGreater();

	private DateOnly? _bonusAccumulationPeriodStart = Settings.ParametersService.BonusAccumulationConditionPeriodStart();

	private DateOnly? _bonusAccumulationPeriodEnd = Settings.ParametersService.BonusAccumulationConditionPeriodEnd();

	private bool _storedBonus;

	private bool _noCalcBonusesWhenUseBonus = Settings.ParametersService.NoCalcBonusesWhenRePay();

	private decimal _bonusUseMaxPercent = Settings.ParametersService.BonusUseMaxPercent();

	private bool _isAllowChangeStorageInBill = Settings.ParametersService.AllowChangeStorageInBill();

	private bool _isAllowChangeCashInBill = Settings.ParametersService.AllowChangeCashInBill();

	private bool _isAllowChangeBuyerInBill = Settings.ParametersService.AllowChangeBuyerInBill();

	private bool _isAllowChangeSaleConsultantInBill = Settings.ParametersService.AllowChangeSaleConsultantInBill();

	private bool _isAllowChangeDiscountInBill = Settings.ParametersService.AllowChangeDiscountInBill();

	private bool _isAllowChangeDocNumberInBill = Settings.ParametersService.AllowChangeDocNumberInBill();

	private bool _isAllowChangeGoodRowInBill = Settings.ParametersService.AllowChangeMTBillGoodRow();

	private bool _isShowUnitCodeColInBill = Settings.ParametersService.ShowUnitCodeColInBill();

	private bool _isShowDiscountPercentColInBill = Settings.ParametersService.ShowDiscountPercentColInBill();

	private bool _isShowDiscountedPriceColInBill = Settings.ParametersService.ShowDiscountedPriceColInBill();

	private bool _isShowDiscountSummColInBill = Settings.ParametersService.ShowDiscountSummColInBill();

	private bool _isShowVATColInBill = Settings.ParametersService.ShowVATColInBill();

	private bool _isShowPartyColInBill = Settings.ParametersService.ShowPartyColInBill();

	private bool _isShowDescrColInBill = Settings.ParametersService.ShowRowDescrColInBill();

	private bool _isShowInfoPanel = Settings.ParametersService.ShowMTBillInfoPanel();

	private bool _isShowItemPictureOnInfoPanel = Settings.ParametersService.ShowItemPictureOnInfoPanel();

	private bool _isShowStorageAvailabilityOnInfoPanel = Settings.ParametersService.ShowStorageAvailabilityOnInfoPanel();

	private bool _isShowWholeAvailabilityOnInfoPanel = Settings.ParametersService.ShowWholeAvailabilityOnInfoPanel();

	private bool _isShowPoleDisplay = Settings.ParametersService.ShowPoleDisplay();

	private bool _isShowStorageColInMTBill = Settings.ParametersService.MTBillAllowDifferentStorages();

	private bool _isShowAccumulatedBonusColInMTBill = Settings.ParametersService.ShowAccumulatedBonusColInBill();

	private bool _isShowUsedBonusColInMTBill = Settings.ParametersService.ShowUsedBonusColInBill();

	private string _rowAddMode = Settings.ParametersService.ScannerMode();

	private bool _connectTob2barmsoftDiscountServer = Settings.ParametersService.ConnectTob2barmsoftDiscountServer();

	private string _prepaymentPartnerCode = "";

	private string _prepaymentContractCode = "";

	private short _nonCashPaymentMethod;

	private bool _partClickDropDown;

	private bool _partReadMagneticCard;

	public string _vatType;

	public string _priceListType = Settings.ParametersService.DefaultMTCheckPriceType();

	public string _priceListTypeRoundDgr = "";

	public string _discountPriceListType = "";

	public string _discountPriceListVATType = "";

	public string _discountPriceListCurr = "";

	public string _discountPriceTypeRoundDegree = "";

	public decimal _discountPriceListCurrRate = 1m;

	public decimal _discountPriceListCurrBase = 1m;

	public decimal _docCurrRate = 1m;

	public decimal _docCurrBase = 1m;

	private decimal _vATSumm;

	private decimal _tmpDiscountSumm;

	private decimal _sumFromRound;

	private decimal _sumAddBonus;

	private bool _applyLastPrice;

	private bool _searchBarCodeInProperty1;

	public bool _distributByFEFO;

	private bool _immediatlyEditRow;

	private bool _immediatlyOpenMarkingWindow = Settings.ParametersService.OpenMarkingWindowImmediatly();

	private string _realText = "";

	public bool UseBuyerAddOrEditQuickMode;

	private string _ecrCRN;

	private Dictionary<(int, string), decimal> _availabilityByGoodId = new Dictionary<(int, string), decimal>();

	private ImageEdit _itemImage;

	private bool _isPartContractWithBonus;

	private bool _isPartBonusCardValid;

	private decimal _partContractBonusPercentOrPoint;

	private bool _isChanged;

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private ReportBarButtonItem _barButtonJumpTo;

	private BarButtonItem _barButtonPrintLabel;

	private BarButtonItem _barButtonImportFromFile;

	private BarButtonItem _barButtonDocumentSettings;

	private ReportBarButtonItem _barButtonMTSalesAnalysis;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private ReportBarButtonItem _barButtonGoodsRetailOperationJournal;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	private ReportBarButtonItem _barButtonBonusOperationJournal;

	private ReportBarButtonItem _barButtonCashOperationJournal;

	private BarStaticItem _barStatusConnectToDS;

	private bool _showDiscCalcSteps;

	private BarCheckItem _barShowDiscCalcSteps;

	private RibbonPageGroup _ribbonPageDiscounts;

	private ECR _ecr;

	private DiscountsCalculator _discountsCalculator;

	private decimal _changeOrPaySum;

	private TextEditExt _ecrCheckNum;

	private TextEditExt _prePaymentECRCheckNum;

	private LayoutGroup _paymentSystemLayout;

	private TextEditExt _nonCashPaymentId;

	private ImageEdit _paymentStatusImage;

	private bool _paymentStatus;

	private MTBill _clonedBill;

	private TextEditExt _inputValue;

	public PopupCalcEditExt _discountPercent;

	private SafeDocumentEventHandler<EditValueChangedEventArgs> _discountPercent_EditValueChangedHandler;

	private PopupCalcEditExt _summaCash;

	private PopupCalcEditExt _summaPlCard;

	private PopupCalcEditExt _summaCredit;

	private SafeDocumentEventHandler<EditValueChangedEventArgs> _otherPaymentTypes_EditValueChangedHandler;

	private PopupCalcEditExt _summaPayByBonus;

	private PopupCalcEditExt _summaPrePayment;

	private PopupCalcEditExt _summa;

	private PopupCalcEditExt _summaChange;

	private PopupCalcEditExt _addedBonus;

	private BarEditItemExt _increase;

	private BarEditItemExt _decrease;

	private BarEditItemExt _weight;

	private Button _salePrice;

	private BarEditItemExt _discount;

	private BarEditItemExt _complect;

	private Button _barCodeEnter;

	private Button _codeEnter;

	private Button _mtListLikeName;

	private Button _qtyEnter;

	private Button _printBill;

	private Button _printECRCheck;

	private Button _plCardPayment;

	private Button _credit;

	private BarEditItemExt _prices;

	private Button _useBonus;

	private BarEditItemExt _calcBonus;

	private BarEditItemExt _addBonus;

	private BarEditItemExt _mtFavorites;

	private Button _returnBill;

	private Button _eMarkReceipt;

	private Button _actualQuantitiesCheck;

	private BarEditItemExt _selectMarking;

	private Button _usePrePayment;

	private BarEditItemExt _filteredSearch;

	private BarEditItemExt _substituteItems;

	private Button _summaCashButton;

	private Button _erasePayments;

	private TextEditExt _infoItemName;

	private TextEditExt _infoFormula;

	private ImageEdit _hintImage;

	private TextEditExt _substitutionInfo;

	private InfoDisplayControls _infoDisplayControls;

	private TextEditExt _infoDescription;

	private SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs> _focusedRowHandleChangedEvent;

	private DropDownControl _currCode;

	public DropDownControl _storage;

	private DropDownControl _cashDesk;

	private TextEditExt _cashierShift;

	private DropDownControl _partner;

	private CheckEditExt _printPartnerTinOnCheck;

	private DropDownControl _salesConsultant;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	public DocumentGridControl _mtBillGrid;

	private TableView _mtBillTableView;

	public ObservableCollection<MTBillSpecificationRow> _mtBillList;

	private DocumentGridColumn _storageCol;

	private DocumentGridColumn _barCodeCol;

	private DocumentGridColumn _itemCodeCol;

	private DocumentGridColumn _itemNameCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _unitCol;

	private DocumentGridColumn _priceCol;

	private DocumentGridColumn _discountCol;

	private DocumentGridColumn _discountPriceCol;

	private DocumentGridColumn _summaCol;

	private DocumentGridColumn _discountSummCol;

	private DocumentGridColumn _usedBonusSummCol;

	private DocumentGridColumn _vatCol;

	private DocumentGridColumn _accumulatedBonusCol;

	private DocumentGridColumn _partyCol;

	private DocumentGridColumn _canceledCol;

	private DocumentGridColumn _isGiftCol;

	private DocumentGridColumn _rowDescCol;

	private DocumentGridColumn _rowMarkingListCol;

	private DocumentGridColumn _isRowMarkable;

	private LayoutGroup _lgDiscountLogs;

	private DocumentGridControl _discountlogsGrid;

	private TableView _discountLogsTableView;

	private BindingList<DiscountLogRow> _discountLogsList;

	public int _maxRowID;

	private bool _mDiscountUse;

	private string _contract;

	private string _partCode = "";

	public bool _retail;

	private decimal _summaValue;

	private decimal _addedBonusValue;

	private decimal _summaCashValue;

	private decimal _summaPlCardValue;

	private decimal _summaPayByBonusValue;

	private decimal _summaCreditValue;

	private decimal _summaPrePaymentValue;

	private decimal _calculatedBonusValue;

	private decimal _summaCashSubtractBonus;

	private decimal _summaPlCardSubtractBonus;

	private decimal _summaPrePaymentSubtractBonus;

	private decimal _mQty_Weight;

	private bool _readOnlyAfterCheckPrint;

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

	private double _fontSizeLarge => _mtBillSimpleMode ? 24 : 22;

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
				_discountPriceTypeRoundDegree = "";
				_discountPriceListVATType = "";
				_discountPriceListCurr = "";
				_discountPriceListCurrRate = 1m;
				_discountPriceListCurrBase = 1m;
				return;
			}
			_discountPriceTypeRoundDegree = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(_discountPriceListType);
			PriceHelper.GetPriceListRowVATType(_discountPriceListType, out _discountPriceListVATType, out _discountPriceListCurr);
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

	private bool _paymentAccepted
	{
		get
		{
			return _paymentStatus;
		}
		set
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_paymentStatus = value;
			if (value)
			{
				_paymentStatusImage.Source = ImageService.GetImage("check_20.svg");
			}
			else
			{
				_paymentStatusImage.Source = ImageService.GetImage("check_disable_20.svg");
			}
		}
	}

	private event RowInfoChangeEventHandler RowInfoChange;

	public MTBillWindow()
	{
	}

	public MTBillWindow(MTBill doc)
		: base(doc)
	{
	}

	protected override void InnerInitialize()
	{
		_discountsCalculator = new DiscountsCalculator(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill);
		_discountsCalculator.ShowMessageOnDiscount += ShowShowMessageOnDiscount;
		_discountsCalculator.AddGiftGoodsToDoc += AddGiftGoodsToDoc;
		_discountsCalculator.SelectAndAddGiftGoodsToDoc += SelectAndAddGiftGoodsToDoc;
	}

	protected override bool AllowChangeGridMode()
	{
		return true;
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		string text = "";
		AddShowDiscCalcItemToRibbon();
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		LayoutGroupExt container2 = AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Vertical);
		LayoutGroupExt container3 = AddLayoutGroup(container2, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container3, Definition.Fields["DocDate"]);
		_date.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Date_EditValueChanged, this).Handler;
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_docNum = (_mtBillSimpleMode ? AddButtonEditControl(container2, Definition.Fields["DocNum"], 1) : AddButtonEditControl(container3, Definition.Fields["DocNum"], 1));
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_ecrCheckNum = AddStringControl(container2, Definition.Fields["ECRCheckNum"]);
		_ecrCheckNum.Width = 223.0;
		_prePaymentECRCheckNum = AddStringControl(container2, Definition.Fields["PrePaymentECRCheckNum"]);
		_prePaymentECRCheckNum.Width = 223.0;
		_paymentSystemLayout = AddLayoutGroup(container2, LayoutGroupView.Group, Orientation.Horizontal);
		_nonCashPaymentId = AddStringControl(_paymentSystemLayout, Definition.Fields["PaymentId"]);
		_nonCashPaymentId.Width = 223.0;
		_paymentStatusImage = AddImageControl(_paymentSystemLayout, 20.0, 20.0, isTabStop: false, showBorder: false, showMenu: false, ShowMenuMode.Hover, ShowLoadDialogOnClickMode.Never, Stretch.Uniform, VerticalAlignment.Center, HorizontalAlignment.Left, "PaymentStatusImage");
		_currCode = AddDropDownControl(container2, Definition.Fields["CurrCode"], new ArmSoft.SME.Core.UI.DropDowns.Currency(shortMode: true));
		_currCode.IsEnabled = false;
		_storage = AddDropDownControl(container2, Definition.Fields["Storage"], new ArmSoft.SME.Trade.UI.DropDown.Storages(_isShowStorageColInMTBill ? new bool?(false) : ((bool?)null)));
		_storage.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged, this)
		{
			HandleOnLoadingDocumentToControls = true
		}.Handler;
		if (!_isShowStorageColInMTBill && Settings.ParametersService.ShowStorageInMTBill())
		{
			_storage.IsRequired = true;
		}
		_cashDesk = AddDropDownControl(container2, Definition.Fields["CashDesk"], new ArmSoft.SME.Trade.UI.DropDown.Cashes(withoutCheckAccess: true));
		_cashDesk.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CashDesk_EditValueChanged, this).Handler;
		_cashierShift = AddStringControl(container2, Definition.Fields["CashierShiftNumber"]);
		_cashierShift.Width = 223.0;
		_partner = AddDropDownControl(container2, ArmSoft.SME.Common.Resources.Buyer, isRequired: false, new ArmSoft.SME.Trade.UI.DropDown.Contracts("", "AMD", isExceptCurrCodeNCC: false, isFilterBySuppliers: false, isFilterByCustomers: true, isValueMemberPartner: true), allowAdditionButtons: true, HorizontalAlignment.Left, "", "Buyer");
		_partner.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(Partner_ClickDropDown, this).Handler;
		_partner.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Partner_EditValueChanged, this).Handler;
		_printPartnerTinOnCheck = AddBooleanControl(container2, Definition.Fields["PrintPartnerTinOnCheck"]);
		_salesConsultant = AddDropDownControl(container2, Definition.Fields["SalesConsultant"], new ArmSoft.SME.Trade.UI.DropDown.Employees(isOnlySalesConsultant: true), allowAdditionButtons: true);
		LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Vertical);
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Right;
		_discountPercent = AddNumericControl(layoutGroupExt, Definition.Fields["Discount"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_discountPercent_EditValueChangedHandler = new SafeDocumentEventHandler<EditValueChangedEventArgs>(DiscountPercent_EditValueChanged, this);
		_discountPercent.EditValueChanged += _discountPercent_EditValueChangedHandler.Handler;
		_summa = AddNumericControl(layoutGroupExt, Definition.Fields["Summ"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_summa.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Summa_EditValueChanged, this).Handler;
		((LayoutItemWithRequired)(object)_summa.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summa.Parent).FontWeight = FontWeights.Bold;
		_summa.IsTabStop = false;
		_summa.IsRequired = !Settings.ParametersService.AllowUse100PercentSale();
		_addedBonus = AddNumericControl(layoutGroupExt, Definition.Fields["SumAddedBonus"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_addedBonus.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(AddedBonus_EditValueChanged, this).Handler;
		((LayoutItemWithRequired)(object)_addedBonus.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_addedBonus.Parent).FontWeight = FontWeights.Bold;
		_addedBonus.IsTabStop = false;
		((LayoutItemWithRequired)(object)_addedBonus.Parent).Visibility = Visibility.Collapsed;
		_summaCash = AddNumericControl(layoutGroupExt, Definition.Fields["SummaCash"], useCurrencyFormat: true, HorizontalAlignment.Right, "SummaCash");
		((LayoutItemWithRequired)(object)_summaCash.Parent).Label = ArmSoft.SME.Trade.Common.Resources.CashPaymentForPrint + ((string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillSummaCashHotKey()) || _anyAutoDiscountModeIsEnabled) ? "" : ("(" + Settings.ParametersService.MTBillSummaCashHotKey() + ")"));
		_summaCash.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SummaCash_EditValueChanged, this).Handler;
		if (_anyAutoDiscountModeIsEnabled)
		{
			_summaCash.IsTabStop = false;
		}
		((LayoutItemWithRequired)(object)_summaCash.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summaCash.Parent).FontWeight = FontWeights.Bold;
		_otherPaymentTypes_EditValueChangedHandler = new SafeDocumentEventHandler<EditValueChangedEventArgs>(OtherPaymentTypes_EditValueChanged, this);
		_summaPlCard = AddNumericControl(layoutGroupExt, Definition.Fields["SummaPlCard"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_summaPlCard.EditValueChanged += _otherPaymentTypes_EditValueChangedHandler.Handler;
		_summaPlCard.IsTabStop = false;
		((LayoutItemWithRequired)(object)_summaPlCard.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summaPlCard.Parent).FontWeight = FontWeights.Bold;
		((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Collapsed;
		_summaPayByBonus = AddNumericControl(layoutGroupExt, Definition.Fields["SummaPayByBonus"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_summaPayByBonus.EditValueChanged += _otherPaymentTypes_EditValueChangedHandler.Handler;
		((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).FontWeight = FontWeights.Bold;
		_summaPayByBonus.IsTabStop = false;
		((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
		_summaCredit = AddNumericControl(layoutGroupExt, Definition.Fields["SummaCredit"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_summaCredit.EditValueChanged += _otherPaymentTypes_EditValueChangedHandler.Handler;
		_summaCredit.IsTabStop = false;
		((LayoutItemWithRequired)(object)_summaCredit.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summaCredit.Parent).FontWeight = FontWeights.Bold;
		((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Collapsed;
		_summaPrePayment = AddNumericControl(layoutGroupExt, Definition.Fields["SummaPrePayment"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_summaPrePayment.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SummaPrePayment_EditValueChanged, this).Handler;
		_summaPrePayment.IsTabStop = false;
		((LayoutItemWithRequired)(object)_summaPrePayment.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summaPrePayment.Parent).FontWeight = FontWeights.Bold;
		((LayoutItemWithRequired)(object)_summaPrePayment.Parent).Visibility = Visibility.Collapsed;
		_summaChange = AddNumericControl(layoutGroupExt, Definition.Fields["SummaChange"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_summaChange.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SummaChange_EditValueChanged, this).Handler;
		((LayoutItemWithRequired)(object)_summaChange.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summaChange.Parent).FontWeight = FontWeights.Bold;
		_summaChange.IsTabStop = false;
		_summaCash.Width = 250.0;
		_summa.Foreground = _summaCash.Foreground;
		_summa.Width = _summaCash.Width;
		_summaChange.Foreground = _summaCash.Foreground;
		_summaChange.Width = _summaCash.Width;
		_summaCredit.Foreground = _summaCash.Foreground;
		_summaCredit.Width = _summaCash.Width;
		_summaPlCard.Foreground = _summaCash.Foreground;
		_summaPlCard.Width = _summaCash.Width;
		_summaPayByBonus.Foreground = _summaCash.Foreground;
		_summaPayByBonus.Width = _summaCash.Width;
		_summaPrePayment.Foreground = _summaCash.Foreground;
		_summaPrePayment.Width = _summaCash.Width;
		_addedBonus.Foreground = _summaCash.Foreground;
		_addedBonus.Width = _summaCash.Width;
		BindingOperations.ClearBinding((DependencyObject)(object)(LayoutItemWithRequired)(object)_summa.Parent, LayoutItemWithRequired.IsReadOnlyProperty);
		LayoutItemWithRequired layoutItemWithRequired = (LayoutItemWithRequired)(object)_summaChange.Parent;
		BindingOperations.ClearBinding((DependencyObject)(object)layoutItemWithRequired, LayoutItemWithRequired.IsReadOnlyProperty);
		layoutItemWithRequired.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#A9A9A9 ");
		LayoutItemWithRequired layoutItemWithRequired2 = (LayoutItemWithRequired)(object)_summaCredit.Parent;
		BindingOperations.ClearBinding((DependencyObject)(object)layoutItemWithRequired2, LayoutItemWithRequired.IsReadOnlyProperty);
		layoutItemWithRequired2.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#1a73e8");
		LayoutItemWithRequired layoutItemWithRequired3 = (LayoutItemWithRequired)(object)_summaPlCard.Parent;
		BindingOperations.ClearBinding((DependencyObject)(object)layoutItemWithRequired3, LayoutItemWithRequired.IsReadOnlyProperty);
		layoutItemWithRequired3.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#1a73e8");
		LayoutItemWithRequired layoutItemWithRequired4 = (LayoutItemWithRequired)(object)_summaPayByBonus.Parent;
		BindingOperations.ClearBinding((DependencyObject)(object)layoutItemWithRequired4, LayoutItemWithRequired.IsReadOnlyProperty);
		layoutItemWithRequired4.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#1a73e8");
		LayoutItemWithRequired layoutItemWithRequired5 = (LayoutItemWithRequired)(object)_summaPrePayment.Parent;
		BindingOperations.ClearBinding((DependencyObject)(object)layoutItemWithRequired5, LayoutItemWithRequired.IsReadOnlyProperty);
		layoutItemWithRequired5.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#1a73e8");
		BindingOperations.ClearBinding((DependencyObject)(object)(LayoutItemWithRequired)(object)_addedBonus.Parent, LayoutItemWithRequired.IsReadOnlyProperty);
		LayoutItemWithRequired layoutItemWithRequired6 = (LayoutItemWithRequired)(object)_summaCash.Parent;
		layoutItemWithRequired6.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#1a73e8");
		BindingOperations.ClearBinding((DependencyObject)(object)layoutItemWithRequired6, LayoutItemWithRequired.IsReadOnlyProperty);
		int num = (_mtBillSimpleMode ? 135 : 136);
		LayoutGroupExt layoutGroupExt2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_inputValue = AddStringControl(layoutGroupExt2, ArmSoft.SME.Trade.Common.Resources.InputBoxCaption + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillInputValueHotKey()) ? "" : (" (" + Settings.ParametersService.MTBillInputValueHotKey() + ")")), isRequired: false, 110, HorizontalAlignment.Stretch, "", "InputValue");
		_inputValue.PreviewTextInput += new SafeDocumentEventHandler<TextCompositionEventArgs>(InputValue_PreviewTextInput, this).Handler;
		_inputValue.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(InputValue_KeyDown, this).Handler;
		text = ArmSoft.SME.Trade.Common.Resources.cmdBarCodeEnter + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillBarCodeEnterHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillBarCodeEnterHotKey() + ")"));
		_barCodeEnter = AddButton(layoutGroupExt2, "", text, "", text, isTabStop: true);
		_barCodeEnter.Width = num;
		_barCodeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(BarCodeEnter_Click, this).Handler;
		text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillCodeEnterHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillCodeEnterHotKey() + ")"));
		_codeEnter = AddButton(layoutGroupExt2, "", text, "", text, isTabStop: true);
		_codeEnter.Width = num;
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		text = ArmSoft.SME.Trade.Common.Resources.cmdNameEnter + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillMTListLikeNameHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillMTListLikeNameHotKey() + ")"));
		_mtListLikeName = AddButton(layoutGroupExt2, "", text, "", text, isTabStop: true);
		_mtListLikeName.Width = num;
		_mtListLikeName.Click += new SafeDocumentEventHandler<RoutedEventArgs>(MTListLikeName_Click, this).Handler;
		layoutGroupExt = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Right;
		text = ArmSoft.SME.Trade.Common.Resources.cmdQtyEnter + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillQtyEnterHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillQtyEnterHotKey() + ")"));
		_qtyEnter = AddButton(_mtBillSimpleMode ? layoutGroupExt : layoutGroupExt2, "", text, "", text, isTabStop: true);
		_qtyEnter.Width = num;
		_qtyEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(QtyEnter_Click, this).Handler;
		if (!_mtBillSimpleMode)
		{
			if (_useMarking && Settings.ParametersService.EMarkReceiptShowButton())
			{
				text = ArmSoft.SME.Trade.Common.Resources.EMarkReceiptButton + (string.IsNullOrWhiteSpace(Settings.ParametersService.EMarkReceiptHotKey()) ? "" : ("(" + Settings.ParametersService.EMarkReceiptHotKey() + ")"));
				_eMarkReceipt = AddButton(layoutGroupExt, "emark_16.svg", text, "", text, isTabStop: true);
				_eMarkReceipt.Width = 150.0;
				_eMarkReceipt.Click += new SafeDocumentEventHandler<RoutedEventArgs>(EMarkReceipt_Click, this).Handler;
			}
			if (Settings.ParametersService.ActualQuantitiesCheckShowButton())
			{
				text = ArmSoft.SME.Trade.Common.Resources.AQCheck + (string.IsNullOrWhiteSpace(Settings.ParametersService.ActualQuantitiesCheckHotKey()) ? "" : ("(" + Settings.ParametersService.ActualQuantitiesCheckHotKey() + ")"));
				_actualQuantitiesCheck = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
				_actualQuantitiesCheck.Width = num;
				_actualQuantitiesCheck.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ActualQuantitiesCheck_Click, this).Handler;
			}
			if (Settings.ParametersService.MTBillReturnBillShowButton())
			{
				text = ArmSoft.SME.Trade.Common.Resources.DocMTBillReturn;
				_returnBill = AddButton(layoutGroupExt, "", text, "", ArmSoft.SME.Trade.Common.Resources.DocMTBillReturn + "(Shift+F12)", isTabStop: true);
				_returnBill.Width = num;
				_returnBill.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ReturnBill_Click, this).Handler;
			}
		}
		if (_allowChangeSalePrices)
		{
			text = ArmSoft.SME.Trade.Common.Resources.SalePriceShort + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillSalePriceHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillSalePriceHotKey() + ")"));
			_salePrice = AddButton(_mtBillSimpleMode ? layoutGroupExt : layoutGroupExt2, "", text, "", text, isTabStop: true);
			_salePrice.IsTabStop = true;
			_salePrice.Width = num;
			_salePrice.Click += new SafeDocumentEventHandler<RoutedEventArgs>(SalePrice_Click, this).Handler;
		}
		if (_anyAutoDiscountModeIsEnabled)
		{
			text = ArmSoft.SME.Trade.Common.Resources.ErasePayments + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillErasePaymentsHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillErasePaymentsHotKey() + ")"));
			_erasePayments = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
			_erasePayments.Width = num;
			_erasePayments.Click += new SafeDocumentEventHandler<RoutedEventArgs>(BE_ErasePayments_Click, this).Handler;
			text = ArmSoft.SME.Trade.Common.Resources.CashPaymentForPrint + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillSummaCashHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillSummaCashHotKey() + ")"));
			_summaCashButton = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
			_summaCashButton.Width = num;
			_summaCashButton.Click += new SafeDocumentEventHandler<RoutedEventArgs>(SummaCash_Click, this).Handler;
		}
		if (!_mtBillSimpleMode)
		{
			if (Settings.ParametersService.MTBillUsePrePaymentShowButton())
			{
				text = ArmSoft.SME.Trade.Common.Resources.UsePrePaymentShort + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillUsePrePaymentHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillUsePrePaymentHotKey() + ")"));
				_usePrePayment = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
				_usePrePayment.Width = num;
				_usePrePayment.Click += new SafeDocumentEventHandler<RoutedEventArgs>(UsePrePayment_Click, this).Handler;
			}
			if (Settings.ParametersService.MTBillPlCardPaymentShowButton())
			{
				text = ArmSoft.SME.Trade.Common.Resources.PlasticCartPayment + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillPlCardPaymentHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillPlCardPaymentHotKey() + ")"));
				_plCardPayment = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
				_plCardPayment.Width = num;
				_plCardPayment.Click += new SafeDocumentEventHandler<RoutedEventArgs>(PlCardPayment_Click, this).Handler;
			}
			if (Settings.ParametersService.MTBillCreditShowButton())
			{
				text = ArmSoft.SME.Trade.Common.Resources.CreditSaleShort + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillCreditHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillCreditHotKey() + ")"));
				_credit = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
				_credit.Width = num;
				_credit.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Credit_Click, this).Handler;
			}
			if (_allowUseBonus)
			{
				text = ArmSoft.SME.Trade.Common.Resources.BonusUseShort + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillUseBonusHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillUseBonusHotKey() + ")"));
				_useBonus = AddButton(layoutGroupExt, "", text, "", text);
				_useBonus.IsTabStop = true;
				_useBonus.Width = num;
				_useBonus.Click += new SafeDocumentEventHandler<RoutedEventArgs>(UseBonus_Click, this).Handler;
			}
		}
		if (Settings.ParametersService.MTBillPrintECRCheckShowButton())
		{
			text = ArmSoft.SME.Common.Resources.ECRC + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillPrintECRCheckHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillPrintECRCheckHotKey() + ")"));
			_printECRCheck = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
			_printECRCheck.Width = num;
			_printECRCheck.Click += new SafeDocumentEventHandler<RoutedEventArgs>(PrintECRCheck_Click, this).Handler;
		}
		if (Settings.ParametersService.MTBillPrintBillShowButton())
		{
			text = ArmSoft.SME.Trade.Common.Resources.PrintCheck + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillPrintBillHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillPrintBillHotKey() + ")"));
			_printBill = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
			_printBill.Width = num;
			_printBill.Click += new SafeDocumentEventHandler<RoutedEventArgs>(PrintBill_Click, this).Handler;
		}
		_mtBillGrid = AddGrid(null, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_mtBillGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTBillGrid_AddingRow, this).Handler;
		_mtBillGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTBillGrid_DuplicatingRow, this).Handler;
		_mtBillGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTBillGrid_MovingRowUp, this).Handler;
		_mtBillGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTBillGrid_MovingRowDown, this).Handler;
		_mtBillGrid.DeletingRow += new SafeDocumentEventHandler<DocumentGridDeletingRowEventArgs>(MTBillGrid_DeletingRow, this).Handler;
		_mtBillGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTBillGrid_DeletedRow, this).Handler;
		_mtBillGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTBillGrid_AddingRowWithDialog, this).Handler;
		_mtBillGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTBillGrid_DuplicatingRowWithDialog, this).Handler;
		_mtBillGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(MTBillGrid_EditingRowWithDialog, this).Handler;
		_mtBillGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTBillGrid_ClickDropDown, this).Handler;
		_mtBillGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTBillGrid_RefreshButtons, this).Handler;
		_mtBillGrid.HasRowDeletedColumn = true;
		text = ArmSoft.SME.Trade.Common.Resources.Increase;
		_increase = _mtBillGrid.AddButton("", text, text, num);
		_increase.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Increase_Click, this).Handler;
		text = ArmSoft.SME.Trade.Common.Resources.Decrease;
		_decrease = _mtBillGrid.AddButton("", text, text, num);
		_decrease.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Decrease_Click, this).Handler;
		if (!string.IsNullOrWhiteSpace(ElScaleInfo.GetInstance().ElScaleType))
		{
			text = ArmSoft.SME.Trade.Common.Resources.Weight + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillWeightHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillWeightHotKey() + ")"));
			_weight = _mtBillGrid.AddButton("", text, text, num);
			_weight.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Weight_Click, this).Handler;
		}
		if (!_mtBillSimpleMode)
		{
			if (Settings.ParametersService.MTBillComplectShowButton())
			{
				text = ArmSoft.SME.Common.Resources.Complect + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillComplectHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillComplectHotKey() + ")"));
				_complect = _mtBillGrid.AddButton("", text, text, num);
				_complect.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Complect_Click, this).Handler;
			}
			if (_paramUseDiscount)
			{
				text = ArmSoft.SME.Trade.Common.Resources.GiveDiscount + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillDiscountHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillDiscountHotKey() + ")"));
				_discount = _mtBillGrid.AddButton("", text, text, num);
				_discount.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Discount_Click, this).Handler;
			}
			if (_allowUseBonus)
			{
				text = ArmSoft.SME.Trade.Common.Resources.CalcBonusesShort + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillCalcBonusHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillCalcBonusHotKey() + ")"));
				_calcBonus = _mtBillGrid.AddButton("", text, text, num);
				_calcBonus.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CalcBonus_Click, this).Handler;
				text = ArmSoft.SME.Trade.Common.Resources.AddToBonusShort + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillAddToBonusHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillAddToBonusHotKey() + ")"));
				_addBonus = _mtBillGrid.AddButton("", text, text, num);
				_addBonus.Click += new SafeDocumentEventHandler<RoutedEventArgs>(AddBonus_Click, this).Handler;
			}
		}
		if (Settings.ParametersService.MTBillPricesShowButton())
		{
			text = ArmSoft.SME.Common.Resources.Prices + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillPricesHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillPricesHotKey() + ")"));
			_prices = _mtBillGrid.AddButton("", text, text, num);
			_prices.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Prices_Click, this).Handler;
		}
		if (!_mtBillSimpleMode && Settings.ParametersService.MTBillFilteredSearchShowButton())
		{
			text = ArmSoft.SME.Trade.Common.Resources.GroupAddition + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillFilteredSearchHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillFilteredSearchHotKey() + ")"));
			_filteredSearch = _mtBillGrid.AddButton("", text, text, num);
			_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		}
		if (Settings.ParametersService.MTBillFavoritesShowButton())
		{
			text = ArmSoft.SME.Common.Resources.RibbonPageFavorites + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillFavoritesHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillFavoritesHotKey() + ")"));
			_mtFavorites = _mtBillGrid.AddButton("", text, text, num);
			_mtFavorites.Click += new SafeDocumentEventHandler<RoutedEventArgs>(MTFavorites_Click, this).Handler;
		}
		if (Settings.ParametersService.EnableSubstituteItems() && Settings.ParametersService.MTBillSubstituteItemsShowButton())
		{
			text = ArmSoft.SME.Trade.Common.Resources.Substitute + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillSubstituteItemsHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillSubstituteItemsHotKey() + ")"));
			_substituteItems = _mtBillGrid.AddButton("", text, text, num);
			_substituteItems.Click += new SafeDocumentEventHandler<RoutedEventArgs>(SubstituteItems_Click, this).Handler;
		}
		text = ArmSoft.SME.Common.Resources.Marking;
		_selectMarking = _mtBillGrid.AddButton("datamatrix_blue_16.svg", text, text, num);
		_selectMarking.Click += new SafeDocumentEventHandler<RoutedEventArgs>(SelectMarking_Click, this).Handler;
		_mtBillGrid.AllowMoveUp = true;
		_mtBillGrid.AllowMoveDown = true;
		_mtBillGrid.View.AutoWidth = Settings.ParametersService.MTBillGridAutoWidth();
		_mtBillTableView = _mtBillGrid.View;
		_focusedRowHandleChangedEvent = new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTBillTableView_FocusedRowHandleChanged, this);
		_mtBillTableView.FocusedRowHandleChanged += _focusedRowHandleChangedEvent.Handler;
		_mtBillTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTBillTableView_CellValueChanged, this).Handler;
		_mtBillTableView.ShowingEditor += new SafeDocumentEventHandler<ShowingEditorEventArgs>(MTBillTableView_ShowingEditor, this).Handler;
		_mtBillTableView.MouseLeftButtonDown += new SafeDocumentEventHandler<MouseButtonEventArgs>(MTBillTableView_MouseLeftButtonDown, this).Handler;
		_mtBillGrid.AddRowIndicator(35);
		if (!_isAllowChangeGoodRowInBill)
		{
			_canceledCol = _mtBillGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["IsDeleted"]);
			_canceledCol.ReadOnly = true;
			_canceledCol.TabStop = false;
			_canceledCol.AllowFocus = false;
			_canceledCol.Width = 35.0;
		}
		if (HasDeletedRow())
		{
			_isAllowChangeGoodRowInBill = false;
		}
		_isGiftCol = _mtBillGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["IsGift"]);
		_isGiftCol.ReadOnly = true;
		_isGiftCol.TabStop = false;
		_isGiftCol.AllowFocus = false;
		_isGiftCol.Visible = false;
		_storageCol = _mtBillGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Storage"], trimValue: false);
		_barCodeCol = _mtBillGrid.AddDropDownCommentColumn(Definition.Grids["MTList"].Columns["BarCode"], new ArmSoft.SME.Trade.UI.DropDown.MTBarCodes(isShowQntUnitCaption: false, showBarCodeFirst: true, isShowOnlyInclInPriceList: false, showAlsoMTQntUnitsEmptyBarCodes: true), 20);
		_barCodeCol.IsOwnProcessingClickDropDown = true;
		_barCodeCol.Width = 120.0;
		_itemCodeCol = _mtBillGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["ItemCode"], new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_itemCodeCol.Width = 125.0;
		_itemNameCol = _mtBillGrid.AddStringColumn(Definition.Grids["MTList"].Columns["ItemName"]);
		_itemNameCol.Width = 225.0;
		_unitCodeCol = _mtBillGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"], trimValue: false);
		_unitCol = _mtBillGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]);
		_unitCol.AllowEditing = DefaultBoolean.False;
		_unitCol.AllowFocus = false;
		_quantityCol = _mtBillGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], _isAllowChangeGoodRowInBill);
		_priceCol = _mtBillGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Price"], showTotal: false, useCurrencyFormat: true, isOwnProcessingClickDropDown: true);
		_discountCol = _mtBillGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Discount"]);
		_discountPriceCol = _mtBillGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["DiscountPrice"]);
		_summaCol = _mtBillGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Summa"], _isAllowChangeGoodRowInBill);
		if (_paramUseDiscount)
		{
			_summaCol.Header = ArmSoft.SME.Trade.Common.Resources.DiscountedSumm;
		}
		_discountSummCol = _mtBillGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["DiscountSumma"], _isAllowChangeGoodRowInBill);
		_discountSummCol.AllowEditing = DefaultBoolean.False;
		_discountSummCol.AllowFocus = false;
		if (!_paramUseDiscount)
		{
			_discountSummCol.Visible = false;
			_discountSummCol.ShowInColumnChooser = false;
		}
		_usedBonusSummCol = _mtBillGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["UsedBonus"], _isAllowChangeGoodRowInBill);
		_usedBonusSummCol.AllowEditing = DefaultBoolean.False;
		_usedBonusSummCol.AllowFocus = false;
		_rowMarkingListCol = _mtBillGrid.AddDropDownColumn("MarkingStatus", ArmSoft.SME.Common.Resources.Marking, 19, trimValue: false, GlyphKind.Regular);
		((ButtonEditSettings)_rowMarkingListCol.EditSettings).IsTextEditable = false;
		_isRowMarkable = _mtBillGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["IsRowMarkable"]);
		_isRowMarkable.ReadOnly = true;
		_isRowMarkable.TabStop = false;
		_isRowMarkable.AllowFocus = false;
		_isRowMarkable.Visible = false;
		_vatCol = _mtBillGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["VAT"]);
		_vatCol.Width = 45.0;
		_accumulatedBonusCol = _mtBillGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["AccumulatedBonus"], _isAllowChangeGoodRowInBill);
		_accumulatedBonusCol.AllowEditing = DefaultBoolean.False;
		_accumulatedBonusCol.AllowFocus = false;
		_partyCol = _mtBillGrid.AddDropDownColumn("PartyMethodName", ArmSoft.SME.Common.Resources.Party, 19, trimValue: false, GlyphKind.Regular);
		((ButtonEditSettings)_partyCol.EditSettings).IsTextEditable = false;
		_rowDescCol = _mtBillGrid.AddStringColumn(Definition.Grids["MTList"].Columns["RowDescription"]);
		_rowDescCol.AllowEditing = DefaultBoolean.True;
		foreach (FormatConditionBase item in ConditionalFormattingHelper.ConvertConditions(CreateFormatingList()))
		{
			_mtBillGrid.View.AddFormatCondition(item);
		}
		if (IsOfflineDoc)
		{
			_retail = false;
		}
		else if (string.IsNullOrWhiteSpace(PO.Storage))
		{
			_retail = !string.IsNullOrWhiteSpace(Settings.ParametersService.DefaultStorage()) && TradeSettings.TradeCacheService.Storage.GetItem(Settings.ParametersService.DefaultStorage()).Retail;
		}
		else
		{
			_retail = TradeSettings.TradeCacheService.Storage.GetItem(PO.Storage).Retail;
		}
		if (HasDeletedRow())
		{
			_isAllowChangeGoodRowInBill = Settings.ParametersService.AllowChangeMTBillGoodRow();
		}
		if (!_isAllowChangeGoodRowInBill)
		{
			foreach (GridColumn column in _mtBillGrid.Grid.Columns)
			{
				column.ReadOnly = true;
			}
			_mtBillGrid.AllowAdd = false;
			_mtBillGrid.AllowEdit = false;
			_mtBillGrid.AllowDuplicate = false;
		}
		if (Settings.ParametersService.ShowASInfoInMTBill())
		{
			DocumentHelpers.ShowASInfoInGrid(_mtBillGrid);
		}
		foreach (GridColumn column2 in _mtBillGrid.Grid.Columns)
		{
			column2.AllowSorting = DefaultBoolean.True;
			column2.AllowColumnFiltering = DefaultBoolean.True;
			column2.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_mtBillList = new ObservableCollection<MTBillSpecificationRow>();
		if (_isShowInfoPanel)
		{
			_infoDisplayControls = AddInfoDisplayControls(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill, _isShowItemPictureOnInfoPanel, _isShowStorageAvailabilityOnInfoPanel, _isShowWholeAvailabilityOnInfoPanel, isShowPriceAvailabilityOnInfoPanel: false, Settings.ParametersService.ShowDescriptionOnInfoPanel(), Settings.ParametersService.EnableSubstituteItems(), _mtBillSimpleMode ? 24 : 22);
			_infoItemName = _infoDisplayControls.InfoItemName;
			_infoFormula = _infoDisplayControls.InfoFormula;
			_infoDescription = _infoDisplayControls.InfoDescription;
			_substitutionInfo = _infoDisplayControls.SubstitutionInfo;
			_hintImage = _infoDisplayControls.HintImage;
			_itemImage = _infoDisplayControls.ItemImage;
		}
		LayoutGroupExt container4 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_comment = AddComboBoxControl(container4, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs()) ? HorizontalAlignment.Stretch : HorizontalAlignment.Left);
		_owner = AddDropDownControl(container4, Definition.Fields["Owner"], new UsersFilter(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Right);
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.ParametersService.ShowOwnerInMTBill())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		else if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		else
		{
			_owner.IsEnabled = Settings.ParametersService.AllowChangeOwnerInMTBill();
		}
		_lgDiscountLogs = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Horizontal, ArmSoft.SME.Trade.Common.Resources.DiscountLogsJournal);
		_lgDiscountLogs.IsCollapsible = true;
		_lgDiscountLogs.IsCollapsed = true;
		_discountlogsGrid = AddGrid(_lgDiscountLogs, ArmSoft.SME.Trade.Common.Resources.DiscountLogsJournal, HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_discountlogsGrid.AllowAdd = false;
		_discountlogsGrid.AllowDelete = false;
		_discountlogsGrid.AllowDuplicate = false;
		_discountlogsGrid.ShowButtons = false;
		_discountlogsGrid.AllowEdit = false;
		_discountlogsGrid.IsVisibleViewEditButton = false;
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
		_discountLogsTableView = _discountlogsGrid.View;
		_discountLogsTableView.AllowMoveColumnToDropArea = false;
		_discountLogsTableView.IsColumnMenuEnabled = false;
		_discountLogsTableView.AllowSorting = false;
		_discountLogsTableView.AllowEditing = false;
		_lgDiscountLogs.Visibility = ((!_showDiscCalcSteps) ? Visibility.Collapsed : Visibility.Visible);
		if (!_mtBillSimpleMode)
		{
			if (!Settings.ParametersService.ShowStorageInMTBill())
			{
				((LayoutItemWithRequired)(object)_storage.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				_storage.IsEnabled = _isAllowChangeStorageInBill;
			}
			if (!Settings.ParametersService.ShowCashInMTBill())
			{
				((LayoutItemWithRequired)(object)_cashDesk.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				_cashDesk.IsEnabled = _isAllowChangeCashInBill;
			}
			if (!Settings.ParametersService.ShowCashierShiftNumInMTBill())
			{
				((LayoutItemWithRequired)(object)_cashierShift.Parent).Visibility = Visibility.Collapsed;
			}
			_partner.IsEnabled = _isAllowChangeBuyerInBill;
			if (!Settings.ParametersService.ShowPrintExpJustifyingCheckInMTBill())
			{
				((LayoutItemWithRequired)(object)_printPartnerTinOnCheck.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				_printPartnerTinOnCheck.IsEnabled = Settings.ParametersService.AllowChangePrintExpJustifyingCheckInMTBill();
			}
			if (!Settings.ParametersService.ShowSalesConsultantInMTBill())
			{
				((LayoutItemWithRequired)(object)_salesConsultant.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				_salesConsultant.IsEnabled = _isAllowChangeSaleConsultantInBill;
			}
			if (!Settings.ParametersService.ShowDiscountInMTBill())
			{
				((LayoutItemWithRequired)(object)_discountPercent.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				_discountPercent.IsEnabled = _isAllowChangeDiscountInBill;
			}
			_docNum.IsEnabled = _isAllowChangeDocNumberInBill;
			if (!Settings.ParametersService.ShowCommentInMTBill())
			{
				((LayoutItemWithRequired)(object)_comment.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				_comment.IsEnabled = Settings.ParametersService.AllowChangeCommentInMTBill();
			}
		}
		else
		{
			_cashDesk.IsEnabled = false;
			_docNum.IsEnabled = false;
			_date.IsEnabled = false;
		}
		if (!_isShowStorageColInMTBill)
		{
			_storageCol.Visible = false;
			_storageCol.ShowInColumnChooser = false;
		}
		if (!_isShowUnitCodeColInBill)
		{
			_unitCodeCol.Visible = false;
			_unitCodeCol.ShowInColumnChooser = false;
		}
		if (!_isShowDiscountPercentColInBill)
		{
			_discountCol.Visible = false;
			_discountCol.ShowInColumnChooser = false;
		}
		if (!_isShowDiscountedPriceColInBill)
		{
			_discountPriceCol.Visible = false;
			_discountPriceCol.ShowInColumnChooser = false;
		}
		if (!_isShowDiscountSummColInBill)
		{
			_discountSummCol.Visible = false;
			_discountSummCol.ShowInColumnChooser = false;
		}
		if (!_isShowVATColInBill)
		{
			_vatCol.Visible = false;
			_vatCol.ShowInColumnChooser = false;
		}
		if (!_isShowPartyColInBill)
		{
			_partyCol.Visible = false;
			_partyCol.ShowInColumnChooser = false;
		}
		if (!_isShowDescrColInBill)
		{
			_rowDescCol.Visible = false;
			_rowDescCol.ShowInColumnChooser = false;
		}
		if (!_useMarking)
		{
			_rowMarkingListCol.Visible = false;
			_rowMarkingListCol.ShowInColumnChooser = false;
			_selectMarking.IsVisible = false;
		}
		ControlsHelper.SetFocus(_inputValue);
		PreviewKeyDown += new SafeDocumentEventHandler<KeyEventArgs>(DocumentWindow_PreviewKeyDown, this).Handler;
		RowInfoChange += OnRowInfoChange;
	}

	private List<AsFormatting> CreateFormatingList()
	{
		List<AsFormatting> list = new List<AsFormatting>();
		UserDefinedFormat customFormat = new UserDefinedFormat
		{
			IconUri = "gift_small_16.svg"
		};
		AsCondititionFormat item = new AsCondititionFormat
		{
			Expression = "[IsGift] = True",
			FieldName = "ItemCode",
			FormatName = "Custom",
			ApplyToRow = false,
			CustomFormat = customFormat,
			IsFixed = false
		};
		list.Add(item);
		if (_useMarking)
		{
			customFormat = new UserDefinedFormat
			{
				IconUri = "datamatrix_blue_16.svg"
			};
			AsCondititionFormat item2 = new AsCondititionFormat
			{
				Expression = "[Quantity] <> 0.0m And [IsRowMarkable] = True",
				FieldName = "Quantity",
				FormatName = "Custom",
				ApplyToRow = false,
				CustomFormat = customFormat,
				IsFixed = true
			};
			list.Add(item2);
			customFormat = new UserDefinedFormat
			{
				IconUri = "datamatrix_red_16.svg"
			};
			item2 = new AsCondititionFormat
			{
				Expression = "[MarkingHasMismatch] = True",
				FieldName = "Quantity",
				FormatName = "Custom",
				ApplyToRow = false,
				CustomFormat = customFormat,
				IsFixed = true
			};
			list.Add(item2);
			customFormat = (Settings.IsDarkTheme ? new UserDefinedFormat
			{
				ForegroundColor = "#FF6666",
				BackgroundColor = "#663333"
			} : new UserDefinedFormat
			{
				ForegroundColor = "#E50000",
				BackgroundColor = "#FFCCCC"
			});
			item2 = new AsCondititionFormat
			{
				Expression = "[MarkingHasMismatch] = True",
				FieldName = "MarkingStatus",
				FormatName = "Custom",
				ApplyToRow = false,
				CustomFormat = customFormat,
				IsFixed = true
			};
			list.Add(item2);
		}
		return list;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_vatType = PO.VATType;
		_ecrCRN = PO.ECRCRN;
		_nonCashPaymentMethod = PO.NonCashPaymentMethod;
		if (!POSTerminalHelpers.IsActivePOSTerminal(_cashDesk.Code) && string.IsNullOrWhiteSpace(_nonCashPaymentId.Text))
		{
			_paymentSystemLayout.Visibility = Visibility.Collapsed;
		}
		_paymentAccepted = PO.PaymentAccepted;
		_partner.Code = PO.PartnerContractGroup.PartnerCode;
		_contract = PO.PartnerContractGroup.ContractCode;
		Partner_EditValueChanged();
		_prepaymentPartnerCode = PO.PrepaymentGroup.PartnerCode;
		_prepaymentContractCode = PO.PrepaymentGroup.ContractCode;
		if (_currCode.Code == "AMD")
		{
			((LayoutItemWithRequired)(object)_currCode.Parent).Visibility = Visibility.Collapsed;
		}
		_summaCash.Value = PO.SummaCash.Value;
		if (PO.PriceListType != "")
		{
			_priceListType = PO.PriceListType;
		}
		if (PO.DiscountPriceListType != "")
		{
			_discountPriceListType = PO.DiscountPriceListType;
		}
		_sumAddBonus = PO.SumAddedBonus.Value;
		UpdateChangeOrPaySum();
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(PO.PartnerContractGroup.PartnerCode, PO.PartnerContractGroup.ContractCode);
		if (contract != null)
		{
			_isPartContractWithBonus = contract.IsBonusCard;
			if (_bonusType == BonusTypeSE.ByPercent.Value)
			{
				_partContractBonusPercentOrPoint = contract.Bonus;
			}
			else
			{
				_partContractBonusPercentOrPoint = contract.Point;
			}
		}
		if (_summaPlCard.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Visible;
		}
		if (_summaCredit.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Visible;
		}
		if (_summaPrePayment.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaPrePayment.Parent).Visibility = Visibility.Visible;
		}
		if (_addedBonus.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_addedBonus.Parent).Visibility = Visibility.Visible;
		}
		_mtBillList.Clear();
		decimal num = 0m;
		decimal num2 = 0m;
		_focusedRowHandleChangedEvent.HandleOnLoadingDocumentToControls = true;
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT in PO.MTList)
		{
			MTBillSpecificationRow mTBillSpecificationRow = new MTBillSpecificationRow
			{
				RowID = mT.RowID,
				ItemID = mT.ItemID,
				ItemType = mT.ItemType,
				Storage = mT.Storage,
				BarCode = mT.BarCode,
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
				IsRowMarkable = mT.IsRowMarkable,
				VAT = mT.VAT.Value,
				AccumulatedBonus = mT.AccumulatedBonus,
				RowDescription = mT.RowDescription,
				MarkingStatus = mT.MarkingStatus,
				MarkingList = mT.MarkingList,
				IsGift = mT.IsGift,
				IsDeleted = mT.IsDeleted,
				IsStorageRequired = mT.IsStorageRequired,
				IsPartySelected = mT.IsPartySelected,
				UsedBonus = mT.UsedBonus,
				RowISN = mT.RowISN
			};
			if (!mT.IsDeleted)
			{
				_usedDiscount += Math.Abs(mT.Discount);
				num += Math.Abs(mT.UsedBonus);
				num2 += Math.Abs(mT.AccumulatedBonus);
			}
			if (_viewMode == WindowViewMode.Copy)
			{
				mTBillSpecificationRow.MarkingList.Clear();
			}
			ChangeMarkingStatus(mTBillSpecificationRow);
			_mtBillList.Add(mTBillSpecificationRow);
		}
		_mtBillGrid.Grid.RefreshData();
		_maxRowID = PO.MaxRowID;
		if (_mtBillList.Count > 0 && (from row in _mtBillList
			where !string.IsNullOrWhiteSpace(row.Storage)
			select row.Storage).Distinct().Count() > 1)
		{
			_storageCol.Visible = true;
			_storageCol.ShowInColumnChooser = true;
			_isShowStorageColInMTBill = true;
		}
		_mtBillGrid.ItemsSource = _mtBillList;
		_storedBonus = _allowUseBonus || _summaPayByBonus.Value != 0m || num != 0m || num2 != 0m;
		_discountLogsList.Clear();
		foreach (DiscountLogRow discountLogs in PO.DiscountLogsList)
		{
			_discountLogsList.Add(discountLogs);
		}
		_accumulatedBonusCol.Visible = _isShowAccumulatedBonusColInMTBill && _storedBonus;
		_accumulatedBonusCol.ShowInColumnChooser = _accumulatedBonusCol.Visible;
		_usedBonusSummCol.Visible = _isShowUsedBonusColInMTBill && _storedBonus;
		_usedBonusSummCol.ShowInColumnChooser = _usedBonusSummCol.Visible;
		if (!_allowUseBonus && _summaPayByBonus.Value == 0m && !_storedBonus)
		{
			((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
		}
		else if (_summaPayByBonus.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Visible;
		}
		else
		{
			((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
		}
		if (_paramUseDiscount || GetDiscountUse())
		{
			_mDiscountUse = true;
			((LayoutItemWithRequired)(object)_discountPercent.Parent).Visibility = Visibility.Visible;
		}
		else
		{
			_mDiscountUse = false;
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
			_discountCol.AllowFocus = false;
			_discountPriceCol.ReadOnly = true;
			_discountPriceCol.AllowFocus = false;
			if (_paramUseDiscount)
			{
				_summaCol.ReadOnly = true;
				_summaCol.AllowFocus = false;
			}
			else
			{
				_summaCol.ReadOnly = false;
			}
		}
		if (_useMarking || GetMarkingUse())
		{
			_rowMarkingListCol.Visible = true;
			_rowMarkingListCol.ShowInColumnChooser = true;
			_selectMarking.IsVisible = true;
		}
		if (_vatType == "3" || _vatType == "4")
		{
			_vatCol.Visible = false;
			_vatCol.ShowInColumnChooser = false;
		}
		if (!(PO.BaseISN == Guid.Empty) && !PO.StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
		}
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num3 = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num3);
		CollectionsMarshal.SetCount(list, num3);
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
			SetControlsDisabledAndReadOnly();
			if (!IsOfflineDoc)
			{
				_printECRCheck.IsEnabled = string.IsNullOrWhiteSpace(_ecrCheckNum.Text) || _ecrCheckNum.Text == "XXXXXXXX";
			}
		}
		else
		{
			SetReadOnlyInner();
			SetOnlineModeDocReadOnlyControls();
		}
	}

	private void SetReadOnlyInner()
	{
		_date.IsReadOnly = !_allowChangeDate;
		_ecrCheckNum.IsReadOnly = true;
		_prePaymentECRCheckNum.IsReadOnly = true;
		_nonCashPaymentId.IsReadOnly = true;
		_currCode.IsReadOnly = true;
		_cashierShift.IsReadOnly = true;
		_summa.IsReadOnly = true;
		_addedBonus.IsReadOnly = true;
		_summaCash.IsReadOnly = _anyAutoDiscountModeIsEnabled;
		_summaPlCard.IsReadOnly = true;
		_summaPayByBonus.IsReadOnly = true;
		_summaCredit.IsReadOnly = true;
		_summaPrePayment.IsReadOnly = true;
		_summaChange.IsReadOnly = true;
		_owner.IsReadOnly = !Settings.ParametersService.AllowChangeOwnerInMTBill();
		if (!_mtBillSimpleMode)
		{
			_storage.IsReadOnly = !_isAllowChangeStorageInBill;
			_cashDesk.IsReadOnly = !_isAllowChangeCashInBill;
			_partner.IsReadOnly = !_isAllowChangeBuyerInBill || !string.IsNullOrWhiteSpace(_ecrCheckNum.Text);
			_printPartnerTinOnCheck.IsReadOnly = !Settings.ParametersService.AllowChangePrintExpJustifyingCheckInMTBill();
			_salesConsultant.IsReadOnly = !_isAllowChangeSaleConsultantInBill;
			_discountPercent.IsReadOnly = !_isAllowChangeDiscountInBill;
			_docNum.IsReadOnly = !_isAllowChangeDocNumberInBill;
			_comment.IsReadOnly = !Settings.ParametersService.AllowChangeCommentInMTBill();
		}
		else
		{
			_cashDesk.IsReadOnly = true;
			_docNum.IsReadOnly = true;
			_date.IsReadOnly = true;
		}
	}

	private void SetControlsDisabledAndReadOnly()
	{
		if (_usePrePayment != null)
		{
			_usePrePayment.IsEnabled = false;
		}
		if (_substituteItems != null)
		{
			_substituteItems.IsEnabled = false;
		}
		if (_summaCashButton != null)
		{
			_summaCashButton.IsEnabled = false;
		}
		if (_erasePayments != null)
		{
			_erasePayments.IsEnabled = false;
		}
		if (_eMarkReceipt != null)
		{
			_eMarkReceipt.IsEnabled = false;
		}
		if (_actualQuantitiesCheck != null)
		{
			_actualQuantitiesCheck.IsEnabled = false;
		}
		if (_lgDiscountLogs != null)
		{
			_lgDiscountLogs.Visibility = Visibility.Collapsed;
		}
		if (_salePrice != null)
		{
			_salePrice.IsEnabled = false;
		}
		if (_barCodeEnter != null)
		{
			_barCodeEnter.IsEnabled = false;
		}
		if (_codeEnter != null)
		{
			_codeEnter.IsEnabled = false;
		}
		if (_mtListLikeName != null)
		{
			_mtListLikeName.IsEnabled = false;
		}
		if (_qtyEnter != null)
		{
			_qtyEnter.IsEnabled = false;
		}
		if (_printBill != null)
		{
			_printBill.IsEnabled = true;
		}
		if (_printECRCheck != null)
		{
			_printECRCheck.IsEnabled = false;
		}
		if (_plCardPayment != null)
		{
			_plCardPayment.IsEnabled = false;
		}
		if (_credit != null)
		{
			_credit.IsEnabled = false;
		}
		if (_useBonus != null)
		{
			_useBonus.IsEnabled = false;
		}
		if (_returnBill != null)
		{
			_returnBill.IsEnabled = false;
		}
		if (_printPartnerTinOnCheck != null)
		{
			_printPartnerTinOnCheck.IsEnabled = false;
		}
		if (_discountlogsGrid != null)
		{
			_discountlogsGrid.IsEnabled = false;
		}
		if (_discountLogsTableView != null)
		{
			_discountLogsTableView.IsEnabled = false;
		}
		if (_weight != null)
		{
			_weight.IsReadOnly = true;
			_weight.IsEnabled = false;
		}
		if (_infoFormula != null)
		{
			_infoFormula.IsReadOnly = true;
			_infoFormula.IsEnabled = false;
		}
		if (_infoItemName != null)
		{
			_infoItemName.IsReadOnly = true;
			_infoItemName.IsEnabled = false;
		}
		if (_hintImage != null)
		{
			_hintImage.IsReadOnly = true;
			_hintImage.IsEnabled = false;
		}
		if (_substitutionInfo != null)
		{
			_substitutionInfo.IsEnabled = false;
			_substitutionInfo.IsReadOnly = true;
		}
		if (_infoDescription != null)
		{
			_infoDescription.IsReadOnly = true;
			_infoDescription.IsEnabled = false;
		}
		if (_inputValue != null)
		{
			_inputValue.IsReadOnly = true;
			_inputValue.IsEnabled = false;
		}
		if (_discountPercent != null)
		{
			_discountPercent.IsReadOnly = true;
		}
		if (_summaCash != null)
		{
			_summaCash.IsReadOnly = true;
		}
		if (_summaPlCard != null)
		{
			_summaPlCard.IsReadOnly = true;
		}
		if (_summaCredit != null)
		{
			_summaCredit.IsReadOnly = true;
		}
		if (_summaPayByBonus != null)
		{
			_summaPayByBonus.IsReadOnly = true;
		}
		if (_summaPrePayment != null)
		{
			_summaPrePayment.IsReadOnly = true;
		}
		if (_summa != null)
		{
			_summa.IsReadOnly = true;
		}
		if (_summaChange != null)
		{
			_summaChange.IsReadOnly = true;
		}
		if (_addedBonus != null)
		{
			_addedBonus.IsReadOnly = true;
		}
		if (_increase != null)
		{
			_increase.IsReadOnly = true;
			_increase.IsEnabled = false;
		}
		if (_decrease != null)
		{
			_decrease.IsReadOnly = true;
			_decrease.IsEnabled = false;
		}
		if (_discount != null)
		{
			_discount.IsReadOnly = true;
			_discount.IsEnabled = false;
		}
		if (_complect != null)
		{
			_complect.IsReadOnly = true;
			_complect.IsEnabled = false;
		}
		if (_prices != null)
		{
			_prices.IsReadOnly = true;
			_prices.IsEnabled = false;
		}
		if (_calcBonus != null)
		{
			_calcBonus.IsReadOnly = true;
			_calcBonus.IsEnabled = false;
		}
		if (_addBonus != null)
		{
			_addBonus.IsReadOnly = true;
			_addBonus.IsEnabled = false;
		}
		if (_mtFavorites != null)
		{
			_mtFavorites.IsReadOnly = true;
			_mtFavorites.IsEnabled = false;
		}
		if (_selectMarking != null)
		{
			_selectMarking.IsReadOnly = true;
			_selectMarking.IsEnabled = false;
		}
		if (_filteredSearch != null)
		{
			_filteredSearch.IsReadOnly = true;
			_filteredSearch.IsEnabled = false;
		}
		if (_barSubItemJournals != null)
		{
			_barSubItemJournals.IsEnabled = false;
		}
		if (_barButtonPrintLabel != null)
		{
			_barButtonPrintLabel.IsEnabled = false;
		}
		if (_barButtonImportFromFile != null)
		{
			_barButtonImportFromFile.IsEnabled = false;
		}
		if (_barButtonDocumentSettings != null)
		{
			_barButtonDocumentSettings.IsEnabled = false;
		}
		if (_barButtonJumpTo != null)
		{
			_barButtonJumpTo.IsEnabled = false;
		}
		if (_barButtonMTSalesAnalysis != null)
		{
			_barButtonMTSalesAnalysis.IsEnabled = false;
		}
		if (_barButtonGoodsOperationsJournal != null)
		{
			_barButtonGoodsOperationsJournal.IsEnabled = false;
		}
		if (_barButtonGoodsRetailOperationJournal != null)
		{
			_barButtonGoodsRetailOperationJournal.IsEnabled = false;
		}
		if (_barButtonPartnerOperationJournal != null)
		{
			_barButtonPartnerOperationJournal.IsEnabled = false;
		}
		if (_barButtonBonusOperationJournal != null)
		{
			_barButtonBonusOperationJournal.IsEnabled = false;
		}
		if (_barButtonCashOperationJournal != null)
		{
			_barButtonCashOperationJournal.IsEnabled = false;
		}
		if (_barStatusConnectToDS != null)
		{
			_barStatusConnectToDS.IsEnabled = false;
		}
	}

	private void SetOnlineModeDocReadOnlyControls()
	{
		bool flag = State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value || State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
		_readOnlyAfterCheckPrint = flag2 || !string.IsNullOrWhiteSpace(PO.ECRCheckNum);
		if (_anyAutoDiscountModeIsEnabled && !_readOnlyAfterCheckPrint)
		{
			AutoCalcDiscountAdjustControlsAccessibility();
			_discountPercent.IsReadOnly = true;
		}
		else
		{
			if (!_allowChangeDate)
			{
				_date.IsReadOnly = true;
			}
			else
			{
				_date.IsReadOnly = _readOnlyAfterCheckPrint;
			}
			if (_isAllowChangeStorageInBill)
			{
				_storage.IsReadOnly = _readOnlyAfterCheckPrint;
			}
			if (_isAllowChangeCashInBill)
			{
				_cashDesk.IsReadOnly = _readOnlyAfterCheckPrint;
			}
			if (_isAllowChangeBuyerInBill)
			{
				_partner.IsReadOnly = flag2;
			}
			_summaCash.IsReadOnly = _readOnlyAfterCheckPrint;
			if (_isAllowChangeSaleConsultantInBill)
			{
				_salesConsultant.IsReadOnly = flag2;
			}
			_mtBillGrid.AllowAdd = !_readOnlyAfterCheckPrint && _isAllowChangeGoodRowInBill;
			_mtBillGrid.AllowEdit = !_readOnlyAfterCheckPrint && _isAllowChangeGoodRowInBill;
			_mtBillGrid.AllowDuplicate = !_readOnlyAfterCheckPrint && _isAllowChangeGoodRowInBill;
			_mtBillGrid.AllowDelete = !_readOnlyAfterCheckPrint;
			_mtBillGrid.AllowMoveUp = !_readOnlyAfterCheckPrint;
			_mtBillGrid.AllowMoveDown = !_readOnlyAfterCheckPrint;
			foreach (GridColumn column in _mtBillGrid.Grid.Columns)
			{
				column.ReadOnly = _readOnlyAfterCheckPrint || !_isAllowChangeGoodRowInBill;
			}
			_barCodeEnter.IsEnabled = !_readOnlyAfterCheckPrint;
			_codeEnter.IsEnabled = !_readOnlyAfterCheckPrint;
			_mtListLikeName.IsEnabled = !_readOnlyAfterCheckPrint;
			_qtyEnter.IsEnabled = !_readOnlyAfterCheckPrint;
			if (_allowChangeSalePrices)
			{
				_salePrice.IsEnabled = !_readOnlyAfterCheckPrint;
			}
			if (_isAllowChangeDiscountInBill)
			{
				_discountPercent.IsReadOnly = _readOnlyAfterCheckPrint;
			}
		}
		if (_printECRCheck != null)
		{
			_printECRCheck.IsEnabled = string.IsNullOrWhiteSpace(_ecrCheckNum.Text);
			_printECRCheck.IsEnabled = _printECRCheck.IsEnabled || _ecrCheckNum.Text == "XXXXXXXX" || (string.IsNullOrWhiteSpace(_prePaymentECRCheckNum.Text) && Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.DiscountAndPrePayment.Value && CalcBonusFromRows() + _addedBonusValue > 0m) || _prePaymentECRCheckNum.Text == "XXXXXXXX";
		}
		if (_erasePayments != null)
		{
			_erasePayments.IsEnabled = !_readOnlyAfterCheckPrint;
		}
		if (_isAllowChangeDocNumberInBill)
		{
			_docNum.IsReadOnly = flag2;
		}
		_inputValue.IsReadOnly = _readOnlyAfterCheckPrint;
		_partyCol.ReadOnly = false;
		((ButtonEditSettings)_partyCol.EditSettings).IsTextEditable = false;
		_rowMarkingListCol.ReadOnly = false;
		((ButtonEditSettings)_rowMarkingListCol.EditSettings).IsTextEditable = false;
		_printPartnerTinOnCheck.IsReadOnly = !string.IsNullOrWhiteSpace(_ecrCheckNum.Text);
	}

	private void MTBillGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		MTBillSpecificationRow MTBillGridRow = (MTBillSpecificationRow)_mtBillGrid.Grid.GetRow(e.RowHandle);
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		string storage = ((!string.IsNullOrWhiteSpace(MTBillGridRow.Storage)) ? MTBillGridRow.Storage : _storage.Code);
		if (e.Column == _itemCodeCol)
		{
			ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter goodsAvailabilityFilter = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter(_date.EditValue.Value, storage, _retail, !_retail);
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
		else if (e.Column == _storageCol)
		{
			if (MTBillGridRow.ItemType != GoodsTypesSE.Good.Value)
			{
				return;
			}
			ArmSoft.SME.Trade.UI.DropDown.Storages storages = new ArmSoft.SME.Trade.UI.DropDown.Storages(false);
			ModalBrowser modalBrowser2 = new ModalBrowser(storages)
			{
				LookUpValue = e.Value,
				Owner = e.Owner,
				Left = e.Left,
				Top = e.Top
			};
			if (modalBrowser2.Show())
			{
				e.NewValue = storages.ValueMember(modalBrowser2.SelectedRowIndex);
			}
		}
		else if (e.Column == _barCodeCol)
		{
			if (MTBillGridRow == null)
			{
				return;
			}
			ArmSoft.SME.Trade.UI.DropDown.MTBarCodes mTBarCodes = new ArmSoft.SME.Trade.UI.DropDown.MTBarCodes(isShowQntUnitCaption: false, showBarCodeFirst: true, Settings.ParametersService.ShowMTInclInPrList(), showAlsoMTQntUnitsEmptyBarCodes: true);
			ModalBrowser modalBrowser3 = new ModalBrowser(mTBarCodes)
			{
				LookUpValue = e.Value,
				Owner = e.Owner,
				Left = e.Left,
				Top = e.Top
			};
			if (modalBrowser3.Show())
			{
				e.NewValue = mTBarCodes.ValueMember(modalBrowser3.SelectedRowIndex);
				MTBillGridRow.ItemCode = mTBarCodes.CommentMember(modalBrowser3.SelectedRowIndex);
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(MTBillGridRow.ItemCode);
				MTBillGridRow.ItemType = item.Type;
				MTBillGridRow.ItemName = item.Caption;
				MTBillGridRow.ItemID = item.Id;
				MTBarCodeDataProvider.DataRow rowByIndex = mTBarCodes.GetRowByIndex(modalBrowser3.SelectedRowIndex);
				MTBillGridRow.BarCode = rowByIndex.BarCode;
				string qntUnit;
				if (string.IsNullOrWhiteSpace(MTBillGridRow.BarCode))
				{
					qntUnit = rowByIndex.QntUnit;
				}
				else
				{
					TradeSettings.TradeCacheService.Material.GetItemByBarCode(MTBillGridRow.BarCode, out qntUnit);
					if (qntUnit == null)
					{
						qntUnit = item.Unit;
					}
				}
				MTBillGridRow.UnitCode = qntUnit;
				_unitCodeCol.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(item.MTCode);
				MTBillGridRow.VAT = item.VAT;
				MTBillGridRow.Discount = _discountPercent.Value;
				if (!_retail || MTBillGridRow.ItemType == GoodsTypesSE.Service.Value)
				{
					MTBillGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, MTBillGridRow.ItemID, MTBillGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
					MTBillGridRow.Price = Methods.RoundPrice(MTBillGridRow.Price, _priceListTypeRoundDgr);
					if (!_paramUseDiscount || !_alwaysCalcDiscountAuto)
					{
						CalculateGridRowDiscountPrice(ref MTBillGridRow);
						CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, MTBillGridRow.DiscountPrice == 0m);
					}
				}
				else
				{
					MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(MTBillGridRow.ItemID, MTBillGridRow.UnitCode);
					if (qntUnit2 != null)
					{
						MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
						{
							MTCode = MTBillGridRow.ItemCode,
							Storage = MTBillGridRow.Storage,
							Date = _date.EditValue,
							UnitCoef = qntUnit2.Coef
						};
						List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
						if (rows.Count > 0)
						{
							MTBillGridRow.Price = Methods.RoundPrice(rows[0].Price, _priceListTypeRoundDgr);
						}
					}
					if (!string.IsNullOrWhiteSpace(_priceListType))
					{
						if (_paramUseDiscount)
						{
							if (!_alwaysCalcDiscountAuto)
							{
								if (string.IsNullOrWhiteSpace(DiscountPriceListType))
								{
									MTBillGridRow.DiscountPrice = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, MTBillGridRow.ItemID, MTBillGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
									MTBillGridRow.DiscountPrice = Methods.RoundPrice(MTBillGridRow.DiscountPrice, _priceListTypeRoundDgr);
									if (MTBillGridRow.Price == 0m)
									{
										MTBillGridRow.Discount = _discountPercent.Value;
										if (MTBillGridRow.Discount == 0m)
										{
											MTBillGridRow.Discount = item.Discount;
										}
										else if (item.Discount != 0m)
										{
											MTBillGridRow.Discount = item.Discount;
										}
										if (MTBillGridRow.Discount != 0m)
										{
											MTBillGridRow.Price = (MTBillGridRow.DiscountPrice * 100m / (100m - MTBillGridRow.Discount)).RoundAs(2);
										}
										else
										{
											MTBillGridRow.Price = MTBillGridRow.DiscountPrice;
										}
									}
									else
									{
										MTBillGridRow.Discount = (100m * (MTBillGridRow.Price - MTBillGridRow.DiscountPrice) / MTBillGridRow.Price).RoundAs(2);
									}
									CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, calcDiscountPrice: false);
								}
								else
								{
									CalculateGridRowDiscountPrice(ref MTBillGridRow);
									CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, MTBillGridRow.DiscountPrice == 0m);
								}
							}
						}
						else
						{
							CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, calcDiscountPrice: false);
						}
					}
					else if (!_paramUseDiscount || !_alwaysCalcDiscountAuto)
					{
						CalculateGridRowDiscountPrice(ref MTBillGridRow);
						CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, MTBillGridRow.DiscountPrice == 0m);
					}
				}
				QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(MTBillGridRow.UnitCode);
				MTBillGridRow.UnitBrief = ((item2 != null) ? item2.Brief : "");
				if (MTBillGridRow.ItemType == GoodsTypesSE.Good.Value)
				{
					MTBillGridRow.IsPartySelected = false;
				}
				else
				{
					MTBillGridRow.IsPartySelected = null;
				}
				if (_paramUseDiscount && _alwaysCalcDiscountAuto)
				{
					On_DiscountClick();
				}
			}
		}
		else if (e.Column == _priceCol)
		{
			if (_retail)
			{
				if (MTBillGridRow == null)
				{
					return;
				}
				if (MTBillGridRow.ItemID != -1 && TradeSettings.TradeCacheService.Material.GetItem(MTBillGridRow.ItemCode).Type == GoodsTypesSE.Good.Value)
				{
					MaterialCacheRow.QntUnitRow qntUnit3 = TradeSettings.TradeCacheService.Material.GetQntUnit(MTBillGridRow.ItemID, MTBillGridRow.UnitCode);
					if (qntUnit3 != null)
					{
						MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(MTBillGridRow.ItemCode, qntUnit3.Coef, MTBillGridRow.Storage, _date.EditValue.Value);
						ModalBrowser modalBrowser4 = new ModalBrowser(mTRetailQtyAndPrice)
						{
							LookUpValue = MTBillGridRow.UnitCode,
							Owner = e.Owner,
							Left = e.Left,
							Top = e.Top
						};
						if (modalBrowser4.Show())
						{
							MTBillGridRow.Price = Methods.RoundPrice(decimal.Parse(mTRetailQtyAndPrice.CommentMember(modalBrowser4.SelectedRowIndex)), _priceListTypeRoundDgr);
							if (!_paramUseDiscount || !_alwaysCalcDiscountAuto)
							{
								CalculateGridRowDiscountPrice(ref MTBillGridRow);
								CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, MTBillGridRow.DiscountPrice == 0m);
							}
							else
							{
								On_DiscountClick();
							}
						}
					}
				}
			}
		}
		else if (e.Column == _partyCol)
		{
			if (MTBillGridRow == null || MTBillGridRow.ItemType != GoodsTypesSE.Good.Value)
			{
				return;
			}
			OpenMTSelectPartyWindow(MTBillGridRow);
		}
		else if (e.Column == _rowMarkingListCol)
		{
			if (MTBillGridRow == null || MTBillGridRow.ItemType != GoodsTypesSE.Good.Value)
			{
				return;
			}
			OpenMarkingSelectWindow(MTBillGridRow);
		}
		CalcSummAndSummVAT();
		UpdateChangeOrPaySum();
		ChangeMarkingStatus(MTBillGridRow);
		RowInfoChange?.Invoke(MTBillGridRow);
	}

	private void MTBillGrid_RefreshButtons(object sender, EventArgs e)
	{
		if (IsOfflineDoc || _viewMode == WindowViewMode.View)
		{
			return;
		}
		if (!_mtBillSimpleMode && (_readOnlyAfterCheckPrint || !_anyAutoDiscountModeIsEnabled))
		{
			if (_complect != null)
			{
				_complect.IsEnabled = !_readOnlyAfterCheckPrint;
			}
			if (_paramUseDiscount)
			{
				_discount.IsEnabled = !_readOnlyAfterCheckPrint;
			}
			if (_weight != null)
			{
				_weight.IsEnabled = !_readOnlyAfterCheckPrint;
			}
			if (!_mtBillSimpleMode && _allowUseBonus)
			{
				_calcBonus.IsEnabled = !_readOnlyAfterCheckPrint;
			}
			if (_filteredSearch != null)
			{
				_filteredSearch.IsEnabled = !_readOnlyAfterCheckPrint;
			}
			_increase.IsEnabled = !_readOnlyAfterCheckPrint;
			_decrease.IsEnabled = !_readOnlyAfterCheckPrint;
			if (_mtFavorites != null)
			{
				_mtFavorites.IsEnabled = !_readOnlyAfterCheckPrint;
			}
			if (_substituteItems != null)
			{
				_substituteItems.IsEnabled = !_readOnlyAfterCheckPrint;
			}
			if (_eMarkReceipt != null)
			{
				_eMarkReceipt.IsEnabled = !_readOnlyAfterCheckPrint;
			}
			if (_actualQuantitiesCheck != null)
			{
				_actualQuantitiesCheck.IsEnabled = !_readOnlyAfterCheckPrint;
			}
		}
		else if (_anyAutoDiscountModeIsEnabled)
		{
			AutoCalcDiscountAdjustControlsAccessibility(OnlyGridButtons: true);
		}
	}

	private void MTBillTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTBillSpecificationRow mtBillGridRow = (MTBillSpecificationRow)_mtBillGrid.Grid.GetRow(e.RowHandle);
		if (mtBillGridRow == null)
		{
			return;
		}
		string text = "";
		if (e.OldValue != null)
		{
			text = e.OldValue.ToString();
		}
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return;
		}
		if (e.Column == _itemCodeCol)
		{
			try
			{
				if (e.Value == null)
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtBillGridRow.RowID);
					mtBillGridRow.Storage = "";
					mtBillGridRow.BarCode = "";
					mtBillGridRow.ItemID = -1;
					mtBillGridRow.ItemCode = "";
					mtBillGridRow.ItemName = "";
					mtBillGridRow.UnitBrief = "";
					mtBillGridRow.UnitCode = "";
					mtBillGridRow.Quantity = 0m;
					mtBillGridRow.Price = 0m;
					mtBillGridRow.Summa = 0m;
					mtBillGridRow.DiscountSumma = 0m;
					mtBillGridRow.IsPartySelected = null;
					mtBillGridRow.MarkingList.Clear();
					mtBillGridRow.MarkingStatus = "";
				}
				else
				{
					if (mtBillGridRow.IsPartySelected == true)
					{
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtBillGridRow.RowID);
					}
					ChangeItemCodeValue(e.Value.ToString(), mtBillGridRow);
					if (_distributByFEFO && mtBillGridRow.ItemType == ItemTypesSE.Goods.Value)
					{
						DistributeQtyByExpiredDate(mtBillGridRow.RowID, mtBillGridRow.Storage, mtBillGridRow.ItemCode, mtBillGridRow.UnitCode, mtBillGridRow.Quantity);
						mtBillGridRow.IsPartySelected = true;
					}
				}
			}
			catch (Exception)
			{
				mtBillGridRow.Storage = "";
				mtBillGridRow.BarCode = "";
				mtBillGridRow.ItemID = -1;
				mtBillGridRow.ItemCode = "";
				mtBillGridRow.ItemName = "";
				mtBillGridRow.UnitBrief = "";
				mtBillGridRow.UnitCode = "";
				mtBillGridRow.Quantity = 0m;
				mtBillGridRow.Price = 0m;
				mtBillGridRow.Summa = 0m;
				mtBillGridRow.DiscountSumma = 0m;
				mtBillGridRow.IsPartySelected = null;
				mtBillGridRow.MarkingList.Clear();
				mtBillGridRow.MarkingStatus = "";
			}
		}
		else if (e.Column == _storageCol)
		{
			if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()) && TradeSettings.TradeCacheService.Storage.GetItem(e.Value.ToString()) != null)
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(e.Value.ToString());
				if (item != null && item.Retail)
				{
					mtBillGridRow.Storage = "";
				}
				else if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(e.Value.ToString()))
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, e.Value.ToString()), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					mtBillGridRow.Storage = text;
				}
				else if (mtBillGridRow.ItemType == GoodsTypesSE.Good.Value && mtBillGridRow.IsPartySelected == true)
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtBillGridRow.RowID);
					mtBillGridRow.IsPartySelected = false;
				}
			}
		}
		else if (e.Column == _barCodeCol)
		{
			if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
			{
				MaterialCacheRow itemByBarCode = TradeSettings.TradeCacheService.Material.GetItemByBarCode(e.Value.ToString(), out var qntUnit);
				if (itemByBarCode != null)
				{
					if (_paramUseDiscount && !_alwaysCalcDiscountAuto)
					{
						mtBillGridRow.Discount = _discountPercent.Value;
						if (string.IsNullOrWhiteSpace(DiscountPriceListType) && (mtBillGridRow.Discount == 0m || itemByBarCode.Discount != 0m))
						{
							mtBillGridRow.Discount = itemByBarCode.Discount;
						}
					}
					if (string.IsNullOrWhiteSpace(mtBillGridRow.BarCode))
					{
						mtBillGridRow.BarCode = itemByBarCode.BarCode;
					}
					mtBillGridRow.ItemType = itemByBarCode.Type;
					mtBillGridRow.ItemName = itemByBarCode.Caption;
					mtBillGridRow.ItemID = itemByBarCode.Id;
					mtBillGridRow.ItemCode = itemByBarCode.MTCode;
					mtBillGridRow.UnitCode = qntUnit;
					_unitCodeCol.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(itemByBarCode.MTCode);
					mtBillGridRow.VAT = itemByBarCode.VAT;
					if (string.IsNullOrWhiteSpace(mtBillGridRow.Storage) && itemByBarCode.Type == GoodsTypesSE.Good.Value)
					{
						mtBillGridRow.Storage = _storage.Code;
					}
					else if (itemByBarCode.Type == GoodsTypesSE.Service.Value)
					{
						mtBillGridRow.IsStorageRequired = false;
					}
					if (!_retail || mtBillGridRow.ItemType == GoodsTypesSE.Service.Value)
					{
						mtBillGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, mtBillGridRow.ItemID, mtBillGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
						mtBillGridRow.Price = Methods.RoundPrice(mtBillGridRow.Price, _priceListTypeRoundDgr);
						if (!_paramUseDiscount || !_alwaysCalcDiscountAuto)
						{
							CalculateGridRowDiscountPrice(ref mtBillGridRow);
							CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, mtBillGridRow.DiscountPrice == 0m);
						}
					}
					else
					{
						MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtBillGridRow.ItemID, mtBillGridRow.UnitCode);
						if (qntUnit2 != null)
						{
							MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
							{
								MTCode = mtBillGridRow.ItemCode,
								UnitCoef = qntUnit2.Coef,
								Storage = mtBillGridRow.Storage,
								Date = _date.EditValue.Value
							};
							List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
							if (rows.Count > 0)
							{
								mtBillGridRow.Price = Methods.RoundPrice(rows[0].Price, _priceListTypeRoundDgr);
							}
							_priceCol.DataView = new MTRetailQtyAndPrice(mtBillGridRow.ItemCode, qntUnit2.Coef, mtBillGridRow.Storage, _date.EditValue.Value);
						}
						if (!string.IsNullOrWhiteSpace(_priceListType))
						{
							if (_paramUseDiscount)
							{
								if (!_alwaysCalcDiscountAuto)
								{
									if (string.IsNullOrWhiteSpace(DiscountPriceListType))
									{
										mtBillGridRow.DiscountPrice = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, mtBillGridRow.ItemID, mtBillGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
										mtBillGridRow.DiscountPrice = Methods.RoundPrice(mtBillGridRow.DiscountPrice, _priceListTypeRoundDgr);
										if (mtBillGridRow.Price == 0m)
										{
											if (mtBillGridRow.Discount != 0m)
											{
												mtBillGridRow.Price = (mtBillGridRow.DiscountPrice * 100m / (100m - mtBillGridRow.Discount)).RoundAs(2);
											}
											else
											{
												mtBillGridRow.Price = mtBillGridRow.DiscountPrice;
											}
										}
										else
										{
											mtBillGridRow.Discount = (100m * (mtBillGridRow.Price - mtBillGridRow.DiscountPrice) / mtBillGridRow.Price).RoundAs(2);
										}
										CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, calcDiscountPrice: false);
									}
									else
									{
										CalculateGridRowDiscountPrice(ref mtBillGridRow);
										CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, mtBillGridRow.DiscountPrice == 0m);
									}
								}
							}
							else
							{
								CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, calcDiscountPrice: false);
							}
						}
						else if (!_paramUseDiscount || !_alwaysCalcDiscountAuto)
						{
							CalculateGridRowDiscountPrice(ref mtBillGridRow);
							CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, mtBillGridRow.DiscountPrice == 0m);
						}
					}
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(mtBillGridRow.UnitCode);
					mtBillGridRow.UnitBrief = ((item2 != null) ? item2.Brief : "");
					if (mtBillGridRow.ItemType == GoodsTypesSE.Good.Value)
					{
						mtBillGridRow.IsPartySelected = false;
					}
					else
					{
						mtBillGridRow.Storage = "";
						mtBillGridRow.IsPartySelected = null;
					}
					mtBillGridRow.MarkingList.Clear();
					mtBillGridRow.MarkingStatus = "";
				}
			}
		}
		else if (e.Column == _unitCodeCol)
		{
			if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()) && !string.IsNullOrWhiteSpace(mtBillGridRow.ItemCode))
			{
				UnitChanged(e.Value.ToString(), text, ref mtBillGridRow);
			}
		}
		else if (e.Column == _quantityCol)
		{
			if ((decimal)e.OldValue != (decimal)e.Value && (decimal)e.OldValue != 0m)
			{
				if (mtBillGridRow.ItemType == GoodsTypesSE.Good.Value)
				{
					if (_distributByFEFO)
					{
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtBillGridRow.RowID);
						DistributeQtyByExpiredDate(mtBillGridRow.RowID, mtBillGridRow.Storage, mtBillGridRow.ItemCode, mtBillGridRow.UnitCode, (decimal)e.Value);
						mtBillGridRow.IsPartySelected = true;
					}
					else if (mtBillGridRow.IsPartySelected == true)
					{
						ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ForRowSelectedParties, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtBillGridRow.RowID);
						mtBillGridRow.IsPartySelected = false;
					}
				}
				mtBillGridRow.Quantity = (decimal)e.Value;
				if ((!_retail || mtBillGridRow.ItemType == GoodsTypesSE.Service.Value) && mtBillGridRow.Price == 0m && _priceListType != "")
				{
					mtBillGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, mtBillGridRow.ItemID, mtBillGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
					mtBillGridRow.Price = Methods.RoundPrice(mtBillGridRow.Price, _priceListTypeRoundDgr);
				}
			}
			else if ((decimal)e.OldValue != (decimal)e.Value && (decimal)e.OldValue == 0m && _distributByFEFO && mtBillGridRow.ItemType == ItemTypesSE.Goods.Value)
			{
				DistributeQtyByExpiredDate(mtBillGridRow.RowID, mtBillGridRow.Storage, mtBillGridRow.ItemCode, mtBillGridRow.UnitCode, (decimal)e.Value);
				mtBillGridRow.IsPartySelected = true;
			}
			mtBillGridRow.Summa = ((decimal)e.Value * mtBillGridRow.DiscountPrice).RoundAs(2);
			CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, calcDiscountPrice: false);
		}
		else if (e.Column == _priceCol)
		{
			if ((decimal)e.OldValue != (decimal)e.Value)
			{
				mtBillGridRow.Price = Methods.RoundPrice((decimal)e.Value, _priceListTypeRoundDgr);
				if (!string.IsNullOrWhiteSpace(DiscountPriceListType) && mtBillGridRow.Price != 0m && mtBillGridRow.DiscountPrice != 0m)
				{
					mtBillGridRow.Discount = (100m * (mtBillGridRow.Price - mtBillGridRow.DiscountPrice) / mtBillGridRow.Price).RoundAs(2);
				}
				CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, string.IsNullOrWhiteSpace(DiscountPriceListType) || mtBillGridRow.DiscountPrice == 0m);
			}
		}
		else if (e.Column == _discountCol)
		{
			CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow);
		}
		else if (e.Column == _discountPriceCol)
		{
			mtBillGridRow.DiscountPrice = Methods.RoundPrice((decimal)e.Value, _priceListTypeRoundDgr);
			if (mtBillGridRow.Price != 0m)
			{
				mtBillGridRow.Discount = (100m * (mtBillGridRow.Price - mtBillGridRow.DiscountPrice) / mtBillGridRow.Price).RoundAs(2);
				CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, calcDiscountPrice: false);
				if (mtBillGridRow.Discount / 1000m >= 1m || mtBillGridRow.Discount / 1000m <= -1m)
				{
					mtBillGridRow.Discount = 0m;
					mtBillGridRow.DiscountPrice = mtBillGridRow.Price;
					mtBillGridRow.DiscountSumma = 0m;
					mtBillGridRow.Summa = mtBillGridRow.Price * mtBillGridRow.Quantity;
				}
			}
		}
		else if (e.Column == _summaCol)
		{
			if (mtBillGridRow.Quantity != 0m)
			{
				mtBillGridRow.DiscountPrice = Methods.RoundPrice((mtBillGridRow.Summa / mtBillGridRow.Quantity).RoundAs(2), _priceListTypeRoundDgr);
				if (!_paramUseDiscount)
				{
					mtBillGridRow.Price = mtBillGridRow.DiscountPrice;
				}
			}
			if (!(mtBillGridRow.Price == 0m) && _paramUseDiscount)
			{
				mtBillGridRow.Discount = (100m * (mtBillGridRow.Price - mtBillGridRow.DiscountPrice) / mtBillGridRow.Price).RoundAs(2);
				mtBillGridRow.DiscountSumma = ((mtBillGridRow.Quantity * mtBillGridRow.Price).RoundAs(2) - mtBillGridRow.Summa).RoundAs(2);
			}
			if (!_paramUseDiscount && mtBillGridRow.Quantity != 0m)
			{
				mtBillGridRow.Price = (mtBillGridRow.Summa / mtBillGridRow.Quantity).RoundAs(2);
				mtBillGridRow.Price = Methods.RoundPrice(mtBillGridRow.Price, _priceListTypeRoundDgr);
			}
			if (mtBillGridRow.Discount == 0m)
			{
				mtBillGridRow.DiscountSumma = 0m;
			}
			else
			{
				mtBillGridRow.DiscountSumma = ((mtBillGridRow.Quantity * mtBillGridRow.Price).RoundAs(2) - mtBillGridRow.Summa).RoundAs(2);
			}
		}
		else if (e.Column == _discountSummCol)
		{
			mtBillGridRow.Summa = (mtBillGridRow.Quantity * mtBillGridRow.Price - mtBillGridRow.DiscountSumma).RoundAs(2);
			mtBillGridRow.DiscountPrice = Methods.RoundPrice((mtBillGridRow.Summa / mtBillGridRow.Quantity).RoundAs(2), _priceListTypeRoundDgr);
			if (!(mtBillGridRow.Price == 0m))
			{
				mtBillGridRow.Discount = (100m * (mtBillGridRow.Price - mtBillGridRow.DiscountPrice) / mtBillGridRow.Price).RoundAs(2);
			}
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		CalculateDocumentSumma();
		ChangeMarkingStatus(mtBillGridRow);
		RowInfoChange?.Invoke(mtBillGridRow);
	}

	private void MTBillTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (e.Row != null)
		{
			MTBillSpecificationRow mTBillSpecificationRow = (MTBillSpecificationRow)e.Row;
			if (mTBillSpecificationRow.IsGift)
			{
				e.Cancel = true;
			}
			else if (mTBillSpecificationRow.ItemType == GoodsTypesSE.Service.Value && (e.Column == _storageCol || e.Column == _partyCol || e.Column == _rowMarkingListCol))
			{
				e.Cancel = true;
			}
			else if (e.Column == _itemNameCol && mTBillSpecificationRow.ItemType == GoodsTypesSE.Good.Value)
			{
				e.Cancel = true;
			}
		}
	}

	private void MTBillTableView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		TableView view = (TableView)sender;
		TableView tableView = view;
		object originalSource = e.OriginalSource;
		TableViewHitInfo tableViewHitInfo = tableView.CalcHitInfo((DependencyObject)((originalSource is DependencyObject) ? originalSource : null));
		if (!tableViewHitInfo.InRowCell)
		{
			return;
		}
		object row = _mtBillGrid.Grid.GetRow(tableViewHitInfo.RowHandle);
		MTBillSpecificationRow row2 = row as MTBillSpecificationRow;
		if (row2 == null || row2.ItemType != GoodsTypesSE.Good.Value || tableViewHitInfo.Column == null)
		{
			return;
		}
		GridColumn column = tableViewHitInfo.Column;
		if (column != _partyCol && column != _rowMarkingListCol)
		{
			return;
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
		{
			try
			{
				if (column == _partyCol)
				{
					OpenMTSelectPartyWindow(row2);
				}
				else if (column == _rowMarkingListCol)
				{
					OpenMarkingSelectWindow(row2);
				}
				view.CommitEditing();
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}), (DispatcherPriority)4, Array.Empty<object>());
	}

	private void OpenMTSelectPartyWindow(MTBillSpecificationRow mtBillGridRow)
	{
		if (string.IsNullOrWhiteSpace(mtBillGridRow.ItemCode))
		{
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtBillGridRow.ItemCode);
		decimal quantity = (mtBillGridRow.Quantity * PriceHelper.GetMTQntUnitCoeff(mtBillGridRow.ItemCode, mtBillGridRow.UnitCode)).RoundAs(3);
		bool isReadOnly = StoredInDb && !IsDraftOrDeleted;
		if (string.IsNullOrWhiteSpace(mtBillGridRow.Storage))
		{
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyStorage);
		}
		MTSelectPartyWindow mTSelectPartyWindow = new MTSelectPartyWindow(mtBillGridRow.Storage, _date.EditValue.Value, mtBillGridRow.ItemCode, item.Unit, quantity, mtBillGridRow.RowID, isReadOnly)
		{
			MTExpDocSelectedPartiesList = PO.MTExpDocSelectedPartiesList
		};
		mTSelectPartyWindow.Load();
		mTSelectPartyWindow.Owner = this;
		if (!mTSelectPartyWindow.ShowDialog().Value)
		{
			return;
		}
		PO.MTExpDocSelectedPartiesList = mTSelectPartyWindow.MTExpDocSelectedPartiesList;
		if (mTSelectPartyWindow.IsPartySelected.HasValue)
		{
			mtBillGridRow.ItemCode = mTSelectPartyWindow.MTCode;
			mtBillGridRow.ItemID = item.Id;
			mtBillGridRow.ItemName = item.Caption;
			mtBillGridRow.Quantity = mTSelectPartyWindow.TotalExpQty;
			string unitCode = mtBillGridRow.UnitCode;
			mtBillGridRow.UnitCode = item.Unit;
			mtBillGridRow.UnitBrief = item.UnitRow.Brief;
			if (_distributByFEFO)
			{
				UnitChanged(item.Unit, unitCode, ref mtBillGridRow, distributeparties: false);
			}
			mtBillGridRow.Summa = (mtBillGridRow.Quantity * mtBillGridRow.DiscountPrice).RoundAs(2);
			mtBillGridRow.IsPartySelected = mTSelectPartyWindow.IsPartySelected;
			mtBillGridRow.ValidateWithoutResponse();
			_mtBillGrid.Grid.RefreshData();
			PO.MTExpDocSelectedPartiesList = mTSelectPartyWindow.MTExpDocSelectedPartiesList;
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
	}

	private void OpenMarkingSelectWindow(MTBillSpecificationRow mtBillGridRow)
	{
		if (!MarkingHelpers.IsGoodMarkable(mtBillGridRow.ItemCode, _useMarking, mtBillGridRow.MarkingList.Count != 0, _date.EditValue.Value, out var isRowMarkable))
		{
			if (string.IsNullOrWhiteSpace(mtBillGridRow.ItemCode))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
			}
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotMarkable, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
			return;
		}
		mtBillGridRow.IsRowMarkable = isRowMarkable;
		List<string> allMarkings = _mtBillList.SelectMany((MTBillSpecificationRow row) => row.MarkingList).ToList();
		MarkingSelectWindow markingSelectWindow = new MarkingSelectWindow(this, _readOnlyAfterCheckPrint, mtBillGridRow.IsRowMarkable, mtBillGridRow.ItemCode, mtBillGridRow.ItemName, mtBillGridRow.MarkingList, allMarkings, mtBillGridRow.Quantity);
		if (!markingSelectWindow.ShowDialog().Value || markingSelectWindow.IsViewOnlyMode)
		{
			return;
		}
		mtBillGridRow.MarkingList.Clear();
		foreach (MarkingSelectWindow.MarkingRow marking in markingSelectWindow.MarkingList)
		{
			mtBillGridRow.MarkingList.Add(marking.Marking);
		}
		if (mtBillGridRow.Quantity < (decimal)mtBillGridRow.MarkingList.Count)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtBillGridRow.ItemCode);
			if (item == null || !item.IsWeight)
			{
				mtBillGridRow.Quantity = mtBillGridRow.MarkingList.Count;
				ChangeRowQty(mtBillGridRow);
			}
		}
		ChangeMarkingStatus(mtBillGridRow);
	}

	private void ChangeItemCodeValue(string newCode, MTBillSpecificationRow row, bool preservePreviousUnit = false, bool isSubstitution = false)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(newCode);
		if (item == null)
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(row.Storage) && item.Type == GoodsTypesSE.Good.Value)
		{
			row.Storage = _storage.Code;
		}
		else if (item.Type == GoodsTypesSE.Service.Value)
		{
			row.IsStorageRequired = false;
		}
		if (_paramUseDiscount && !_alwaysCalcDiscountAuto)
		{
			row.Discount = _discountPercent.Value;
			if (string.IsNullOrWhiteSpace(DiscountPriceListType) && (row.Discount == 0m || item.Discount != 0m))
			{
				row.Discount = item.Discount;
			}
		}
		row.BarCode = item.BarCode;
		row.ItemType = item.Type;
		row.ItemName = item.Caption;
		row.ItemID = item.Id;
		row.ItemCode = item.MTCode;
		if (!preservePreviousUnit)
		{
			row.UnitBrief = item.UnitRow.Brief;
			row.UnitCode = item.Unit;
		}
		_unitCodeCol.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(item.MTCode);
		row.VAT = item.VAT;
		if (!_retail || row.ItemType == GoodsTypesSE.Service.Value)
		{
			if (!isSubstitution)
			{
				row.UnitCode = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(row.ItemID, _priceListType, _date.EditValue.Value);
				if (string.IsNullOrWhiteSpace(row.UnitCode))
				{
					row.UnitCode = item.Unit;
				}
				else
				{
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(row.UnitCode);
					row.UnitBrief = ((item2 != null) ? item2.Brief : "");
				}
			}
			row.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, row.ItemID, row.UnitCode, _date.EditValue.Value, _currCode.Code);
			row.Price = Methods.RoundPrice(row.Price, _priceListTypeRoundDgr);
			if (!_paramUseDiscount || !_alwaysCalcDiscountAuto)
			{
				CalculateGridRowDiscountPrice(ref row);
				CalculateGridRowSummaAndDiscountSumma(ref row, row.DiscountPrice == 0m);
			}
		}
		else
		{
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(row.ItemID, row.UnitCode);
			if (qntUnit != null)
			{
				MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
				{
					MTCode = row.ItemCode,
					UnitCoef = qntUnit.Coef,
					Storage = row.Storage,
					Date = _date.EditValue
				};
				List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
				if (rows.Count > 0)
				{
					row.Price = Methods.RoundPrice(rows[0].Price, _priceListTypeRoundDgr);
				}
			}
			if (!string.IsNullOrWhiteSpace(_priceListType))
			{
				if (_paramUseDiscount)
				{
					if (!_alwaysCalcDiscountAuto)
					{
						if (string.IsNullOrWhiteSpace(DiscountPriceListType))
						{
							row.DiscountPrice = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, row.ItemID, row.UnitCode, _date.EditValue.Value, _currCode.Code);
							row.DiscountPrice = Methods.RoundPrice(row.DiscountPrice, _priceListTypeRoundDgr);
							if (row.Price == 0m)
							{
								if (row.Discount != 0m)
								{
									row.Price = (row.DiscountPrice * 100m / (100m - row.Discount)).RoundAs(2);
								}
								else
								{
									row.Price = row.DiscountPrice;
								}
							}
							else
							{
								row.Discount = (100m * (row.Price - row.DiscountPrice) / row.Price).RoundAs(2);
							}
							CalculateGridRowSummaAndDiscountSumma(ref row, calcDiscountPrice: false);
						}
						else
						{
							CalculateGridRowDiscountPrice(ref row);
							CalculateGridRowSummaAndDiscountSumma(ref row, row.DiscountPrice == 0m);
						}
					}
				}
				else
				{
					CalculateGridRowSummaAndDiscountSumma(ref row, calcDiscountPrice: false);
				}
			}
			else if (!_paramUseDiscount || !_alwaysCalcDiscountAuto)
			{
				CalculateGridRowDiscountPrice(ref row);
				CalculateGridRowSummaAndDiscountSumma(ref row, row.DiscountPrice == 0m);
			}
		}
		if (row.ItemType == GoodsTypesSE.Good.Value)
		{
			row.IsPartySelected = false;
		}
		else
		{
			row.Storage = "";
			row.IsPartySelected = null;
		}
		row.MarkingList.Clear();
		row.MarkingStatus = "";
	}

	private bool GetMarkingUse()
	{
		return _mtBillList.Any((MTBillSpecificationRow x) => x.MarkingList.Count != 0);
	}

	private bool GetDiscountUse()
	{
		decimal num = 0m;
		foreach (MTBillSpecificationRow mtBill in _mtBillList)
		{
			num += mtBill.DiscountSumma;
		}
		return num != 0m;
	}

	private bool HasDeletedRow()
	{
		return PO.MTList.Any((MTInvoiceBase.MTInvoiceSpecificationRow row) => row.IsDeleted);
	}

	protected override void CreateChildDocument(short childDocType, MTBill baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		CreateBasedOnDocHelper.CreateDocument((ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)(SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
	}

	private void CreateBasedOnDoc()
	{
		Document document = Settings.DocumentService.Load(PO.BaseISN, withoutAccessCheck: true);
		if (!_allowChangeDate)
		{
			_date.EditValue = DateTime.Now.ToDateOnly();
		}
		else
		{
			_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		}
		short pOType = document.POType;
		if (pOType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value || pOType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value)
		{
			AddRowForBaseMTMove(document);
		}
		else if (pOType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay.Value)
		{
			AddRowForBaseInvoiceForPay(document);
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		CalculateDocumentSumma();
		_mtBillGrid.Grid.RefreshData();
	}

	private void AddRowForBaseInvoiceForPay(Document baseDocument)
	{
		InvoiceForPay invoiceForPay = (InvoiceForPay)baseDocument;
		_comment.EditValue = invoiceForPay.Comment;
		if (string.IsNullOrWhiteSpace(invoiceForPay.Storage))
		{
			_storage.Code = Settings.ParametersService.DefaultStorage();
		}
		else
		{
			_storage.Code = invoiceForPay.Storage;
		}
		if (!string.IsNullOrWhiteSpace(invoiceForPay.PartnerContractGroup.PartnerCode))
		{
			if (!string.IsNullOrWhiteSpace(invoiceForPay.PartnerContractGroup.ContractCode))
			{
				_contract = invoiceForPay.PartnerContractGroup.ContractCode;
			}
			else
			{
				PO.PartnerContractGroup.ContractCode = TradeSettings.TradeCacheService.Partner.GetDefaultContract(invoiceForPay.PartnerContractGroup.PartnerCode)?.Contract ?? "";
			}
		}
		_partner.Code = invoiceForPay.PartnerContractGroup.PartnerCode;
		_discountPercent.Value = invoiceForPay.Discount;
		_mtBillList.Clear();
		_maxRowID = 0;
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT in invoiceForPay.MTList)
		{
			MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(mT.ItemID, mT.UnitCode);
			MTBillSpecificationRow mtBillGridRow = new MTBillSpecificationRow
			{
				Storage = _storage.Code,
				ItemType = mT.ItemType,
				IsPartySelected = mT.IsPartySelected,
				ItemID = mT.ItemID,
				ItemCode = mT.ItemCode,
				ItemName = mT.ItemName,
				UnitCode = mT.UnitCode,
				UnitBrief = mT.UnitBrief,
				Quantity = mT.Quantity,
				Price = mT.Price,
				BarCode = ((barCode == null) ? "" : barCode.BarCode),
				VAT = mT.VAT.Value,
				Discount = mT.Discount,
				DiscountPrice = mT.DiscountPrice,
				DiscountSumma = mT.DiscountSumma,
				Summa = mT.Summa,
				RowDescription = mT.RowDescription,
				RowID = _maxRowID
			};
			if (!_paramUseDiscount || !_alwaysCalcDiscountAuto)
			{
				CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow);
			}
			ChangeMarkingStatus(mtBillGridRow);
			_mtBillList.Add(mtBillGridRow);
			_maxRowID++;
		}
	}

	private void AddRowForBaseMTMove(Document baseDocument)
	{
		MTMoveWithConfirmationBase mTMoveWithConfirmationBase = (MTMoveWithConfirmationBase)baseDocument;
		_comment.EditValue = mTMoveWithConfirmationBase.Comment;
		_storage.Code = mTMoveWithConfirmationBase.StorageIncome;
		bool flag = TradeSettings.TradeCacheService.Storage.GetItem(mTMoveWithConfirmationBase.StorageIncome)?.Retail ?? false;
		_mtBillList.Clear();
		_maxRowID = 0;
		foreach (MTMoveBase.MTMoveSpecificationRow mT in mTMoveWithConfirmationBase.MTList)
		{
			MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(mT.MTID, mT.UnitCode);
			MTBillSpecificationRow mtBillGridRow = new MTBillSpecificationRow
			{
				Storage = mTMoveWithConfirmationBase.StorageIncome,
				ItemType = "1",
				IsPartySelected = false,
				ItemID = mT.MTID,
				ItemCode = mT.MTCode,
				ItemName = mT.MTName,
				UnitCode = mT.UnitCode,
				UnitBrief = mT.UnitBrief,
				Quantity = mT.Quantity
			};
			if (flag)
			{
				mtBillGridRow.Price = mT.Price.Value;
			}
			else
			{
				mtBillGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, mtBillGridRow.ItemID, mtBillGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
				mtBillGridRow.Price = Methods.RoundPrice(mtBillGridRow.Price, _priceListTypeRoundDgr);
			}
			mtBillGridRow.BarCode = ((barCode == null) ? "" : barCode.BarCode);
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtBillGridRow.ItemCode);
			if (item != null)
			{
				mtBillGridRow.VAT = item.VAT;
			}
			if (_paramUseDiscount)
			{
				if (!_alwaysCalcDiscountAuto)
				{
					mtBillGridRow.Discount = item.Discount;
					mtBillGridRow.DiscountPrice = (mtBillGridRow.Price - mtBillGridRow.Price * mtBillGridRow.Discount / 100m).RoundAs(2);
				}
			}
			else
			{
				mtBillGridRow.Discount = 0m;
				mtBillGridRow.DiscountPrice = mtBillGridRow.Price;
			}
			if (!_paramUseDiscount || !_alwaysCalcDiscountAuto)
			{
				CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow);
			}
			mtBillGridRow.RowID = _maxRowID;
			ChangeMarkingStatus(mtBillGridRow);
			_mtBillList.Add(mtBillGridRow);
			_maxRowID++;
		}
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.PartnerContractGroup.PartnerCode = _partner.Code;
		PO.PartnerContractGroup.ContractCode = _contract;
		PO.SummaCash = _summaCash.Value;
		PO.PaymentAccepted = _paymentAccepted;
		PO.VATType = Methods.VATAndRetailVATMapping(_vatType, oppositeMapping: false);
		PO.NonCashPaymentMethod = _nonCashPaymentMethod;
		PO.PrepaymentGroup.PartnerCode = _prepaymentPartnerCode;
		PO.PrepaymentGroup.ContractCode = _prepaymentContractCode;
		PO.ECRCRN = _ecrCRN;
		PO.PriceListType = _priceListType;
		PO.DiscountPriceListType = DiscountPriceListType;
		PO.SumFromRound = _sumFromRound;
		PO.SumAddedBonus = _sumAddBonus;
		PO.SummVAT = _vATSumm;
		PO.PrePaymentCashAmount = _summaCashSubtractBonus;
		PO.PrePaymentCardAmount = _summaPlCardSubtractBonus;
		PO.MaxRowID = _maxRowID;
		PO.MTList.Clear();
		foreach (MTBillSpecificationRow mtBill in _mtBillList)
		{
			PO.MTList.Add(new MTInvoiceBase.MTInvoiceSpecificationRow
			{
				RowID = mtBill.RowID,
				ItemID = mtBill.ItemID,
				ItemType = mtBill.ItemType,
				Storage = mtBill.Storage,
				BarCode = mtBill.BarCode,
				ItemCode = mtBill.ItemCode,
				ItemName = mtBill.ItemName,
				UnitCode = mtBill.UnitCode,
				UnitBrief = mtBill.UnitBrief,
				Quantity = mtBill.Quantity,
				Price = mtBill.Price,
				Discount = mtBill.Discount,
				DiscountPrice = mtBill.DiscountPrice,
				Summa = mtBill.Summa,
				DiscountSumma = mtBill.DiscountSumma,
				IsRowMarkable = mtBill.IsRowMarkable,
				VAT = mtBill.VAT,
				AccumulatedBonus = mtBill.AccumulatedBonus,
				RowDescription = mtBill.RowDescription,
				MarkingStatus = mtBill.MarkingStatus,
				MarkingList = mtBill.MarkingList,
				IsGift = mtBill.IsGift,
				IsDeleted = mtBill.IsDeleted,
				IsPartySelected = mtBill.IsPartySelected,
				UsedBonus = mtBill.UsedBonus,
				RowISN = mtBill.RowISN
			});
		}
	}

	protected override bool BeforeStore(StoreRequest sr)
	{
		if (sr.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value)
		{
			if (_anyAutoDiscountModeIsEnabled && IsUnderpaid())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PaymentsSmallerThanTotal, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return false;
			}
			if (_nonCashPaymentMethod != 0 && _summaPlCard.Value == 0m)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.WrongNonCashSum, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return false;
			}
			if (_bonusUseMaxPercent < 100m && (_summaPayByBonus.Value * 100m / _summa.Value).RoundAs(2) > _bonusUseMaxPercent)
			{
				decimal num = (_summa.Value * _bonusUseMaxPercent / 100m).RoundAs(2);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.UsedMaxBonusValidate, num));
			}
			decimal num2 = _summaPrePayment.Value;
			if (!string.IsNullOrWhiteSpace(_prepaymentPartnerCode) && (!(_prepaymentPartnerCode == _partner.Code) || !(_prepaymentContractCode == PO.PartnerContractGroup.ContractCode)))
			{
				num2 = 0m;
				PO.IsCheckLimitAnswerYesforOtherPartnerPrepayment = false;
				if (_summaPrePayment.Value > 0m)
				{
					if (!string.IsNullOrWhiteSpace(_prepaymentPartnerCode))
					{
						PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_prepaymentPartnerCode);
						PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_prepaymentPartnerCode, _prepaymentContractCode);
						int intPartID = item?.Id ?? (-1);
						if (contract != null && contract.ControlReceivables && (TradeSettings.CreateTradeClient().CommonServices.GetCurrentPartnerRemByContract(intPartID, contract.Contract, "", PO.ISN).SummCurr + _summaPrePayment.Value).RoundAs(2) > contract.ReceivablesLimit)
						{
							if (!Settings.ParametersService.AllowExceedDebtLimit())
							{
								ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ReceivablesExceedDenied, _prepaymentContractCode), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
								return false;
							}
							if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ReceivablesConfirmExceed, contract.Contract), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
							{
								return false;
							}
							PO.IsCheckLimitAnswerYesforOtherPartnerPrepayment = true;
						}
					}
				}
				else
				{
					PO.IsCheckLimitAnswerYesforOtherPartnerPrepayment = true;
				}
			}
			if (!string.IsNullOrWhiteSpace(_partner.Code) && !string.IsNullOrWhiteSpace(_contract))
			{
				PO.IsCheckLimitAnswerYes = false;
				if (_summaCredit.Value + num2 > 0m)
				{
					PartnerCacheRow item2 = TradeSettings.TradeCacheService.Partner.GetItem(_partner.Code);
					PartnerCacheRow.ContractRow contract2 = TradeSettings.TradeCacheService.Partner.GetContract(_partner.Code, _contract);
					int intPartID2 = item2?.Id ?? (-1);
					if (contract2 != null && contract2.ControlReceivables)
					{
						if ((TradeSettings.CreateTradeClient().CommonServices.GetCurrentPartnerRemByContract(intPartID2, contract2.Contract, "", PO.ISN).SummCurr + _summaCredit.Value + num2).RoundAs(2) > contract2.ReceivablesLimit)
						{
							if (!Settings.ParametersService.AllowExceedDebtLimit())
							{
								ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ReceivablesExceedDenied, _contract), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
								return false;
							}
							if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ReceivablesConfirmExceed, contract2.Contract), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
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
			}
			if (sr.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && _summaPayByBonus.Value != 0m)
			{
				CheckUsedBonusSum(_summaPayByBonus.Value);
			}
		}
		return true;
	}

	protected override void AfterStore(StoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
		try
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partCode, _contract);
			if (string.IsNullOrWhiteSpace(_partCode) || contract == null || !contract.IsBonusCard || string.IsNullOrWhiteSpace(contract.DiscountCard) || !_connectTob2barmsoftDiscountServer)
			{
				return;
			}
			if (State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value)
			{
				if (PO.TransactionISN == Guid.Empty)
				{
					CardTransactionRequest cardTransactionRequest = new CardTransactionRequest();
					cardTransactionRequest.CardNumber = contract.DiscountCard;
					cardTransactionRequest.DocInfo = new CardTransactionRequestDocInfo
					{
						ISN = PO.ISN,
						Type = DocType.Sale,
						Date = PO.DocDate.ToDateTime(),
						Number = PO.DocNum,
						Amount = PO.Summ.Value,
						CashCode = PO.CashDesk,
						ShiftNumber = PO.CashierShiftNumber,
						User = Settings.LoginService.UserName
					};
					cardTransactionRequest.Rows = new List<CardTransactionRow>();
					decimal? summaAccumulatedBonus = PO.SummaAccumulatedBonus;
					if (!((summaAccumulatedBonus.GetValueOrDefault() == 0m) & summaAccumulatedBonus.HasValue))
					{
						cardTransactionRequest.Rows.Add(new CardTransactionRow
						{
							Op = OpType.Accumulate,
							Amount = PO.SummaAccumulatedBonus.Value
						});
					}
					summaAccumulatedBonus = PO.SumAddedBonus;
					if (!((summaAccumulatedBonus.GetValueOrDefault() == 0m) & summaAccumulatedBonus.HasValue))
					{
						cardTransactionRequest.Rows.Add(new CardTransactionRow
						{
							Op = OpType.Accumulate,
							Amount = PO.SumAddedBonus.Value
						});
					}
					summaAccumulatedBonus = PO.SummaPayByBonus;
					if (!((summaAccumulatedBonus.GetValueOrDefault() == 0m) & summaAccumulatedBonus.HasValue))
					{
						cardTransactionRequest.Rows.Add(new CardTransactionRow
						{
							Op = OpType.Use,
							Amount = -PO.SummaPayByBonus.Value
						});
					}
					CardTransaction cardTransaction = DiscountServerService.RecordDiscountServerTransaction(cardTransactionRequest);
					if (cardTransaction != null)
					{
						PO.TransactionISN = cardTransaction.ISN;
						int num = 1;
						List<PropertyWithValue> list = new List<PropertyWithValue>(num);
						CollectionsMarshal.SetCount(list, num);
						CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("TransactionISN", PO.TransactionISN.ToString());
						List<PropertyWithValue> properties = list;
						sr.LogComment = $"{ArmSoft.SME.Trade.Common.Resources.RecordInDiscountServer} - {PO.TransactionISN}";
						PO.TS = Settings.DocumentService.EditFields(PO.ISN, properties, sr);
					}
				}
			}
			else if (State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Processed.Value)
			{
				DiscountServerService.DeleteTransactionsByISN(PO.TransactionISN);
				PO.TransactionISN = Guid.Empty;
				int num = 1;
				List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
				CollectionsMarshal.SetCount(list2, num);
				CollectionsMarshal.AsSpan(list2)[0] = new PropertyWithValue("TransactionISN", PO.TransactionISN.ToString());
				List<PropertyWithValue> properties2 = list2;
				sr.LogComment = $"{ArmSoft.SME.Trade.Common.Resources.DeleteFromDiscountServer} - {PO.TransactionISN}";
				PO.TS = Settings.DocumentService.EditFields(PO.ISN, properties2, sr);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		rk.SetValue("SearchBarCodeInProperty1", _searchBarCodeInProperty1 ? "1" : "0");
		rk.SetValue("ImmediatlyEditRow", _immediatlyEditRow ? "1" : "0");
		rk.SetValue("DistributByFEFO", _distributByFEFO ? "1" : "0");
		rk.SetValue("ApplyLastPrice", _applyLastPrice ? "1" : "0");
		rk.SetValue("bUseBuyerAddOrEditQuickMode", UseBuyerAddOrEditQuickMode ? "1" : "0");
		rk.SetValue("ShowDiscCalcSteps", _showDiscCalcSteps ? "1" : "0");
		_mtBillGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		if (rk != null)
		{
			_searchBarCodeInProperty1 = rk.GetValue("SearchBarCodeInProperty1", false).ToString() == "1";
			_immediatlyEditRow = rk.GetValue("ImmediatlyEditRow", false).ToString() == "1";
			_distributByFEFO = rk.GetValue("DistributByFEFO", false).ToString() == "1";
			_applyLastPrice = rk.GetValue("ApplyLastPrice", false).ToString() == "1";
			if (_lgDiscountLogs != null)
			{
				_showDiscCalcSteps = rk.GetValue("ShowDiscCalcSteps", false).ToString() == "1";
			}
			UseBuyerAddOrEditQuickMode = rk.GetValue("bUseBuyerAddOrEditQuickMode", false).ToString() == "1";
			_barShowDiscCalcSteps.IsChecked = _showDiscCalcSteps;
			_mtBillGrid?.RestoreSettings(rk);
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

	private void PrintCheck()
	{
		_printECRCheck.IsEnabled = false;
		if (_ecrCheckNum.Text == "XXXXXXXX" || _prePaymentECRCheckNum.Text == "XXXXXXXX")
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRMessageForXXXXXXXX, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			_printECRCheck.IsEnabled = true;
			return;
		}
		try
		{
			CashCacheRow cashRow = TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk.Code);
			if (!ECRPreliminaryChecksAndInitalization(cashRow))
			{
				_printECRCheck.IsEnabled = true;
				return;
			}
			bool num = !string.IsNullOrWhiteSpace(_ecrCheckNum.Text) && _ecrCheckNum.Text != "XXXXXXXX";
			_summaValue = _summa.Value;
			_addedBonusValue = _addedBonus.Value;
			_summaCashValue = _summaCash.Value;
			_summaPlCardValue = _summaPlCard.Value;
			_summaPayByBonusValue = _summaPayByBonus.Value;
			_summaCreditValue = _summaCredit.Value;
			_summaPrePaymentValue = _summaPrePayment.Value;
			_calculatedBonusValue = 0m;
			_summaCashSubtractBonus = 0m;
			_summaPlCardSubtractBonus = 0m;
			_summaPrePaymentSubtractBonus = 0m;
			if (Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.DiscountAndPrePayment.Value)
			{
				if (CalcBonusFromRows() + _addedBonus.Value > 0m)
				{
					_summaCashValue -= _summaChange.Value;
				}
				CalcAllSummaBasedFromCalculatedBonuses();
			}
			bool bonus = Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.DiscountAndPrePayment.Value && _calculatedBonusValue != 0m && (_summaPlCardSubtractBonus > 0m || _summaCashSubtractBonus > 0m);
			if (((PO.POSTerminalType == "1") & bonus) && _summaPlCardSubtractBonus != 0m)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.CanNotPrintBonusAsPerPaymentWithECRPOS);
			}
			_clonedBill = (MTBill)Settings.DocumentService.Load(POISN(), withoutAccessCheck: true);
			_clonedBill.SumAddedBonus = _addedBonusValue;
			MTBill clonedBill = _clonedBill;
			decimal summaPrePaymentValue = _summaPrePaymentValue;
			decimal? summaPayByBonus = _clonedBill.SummaPayByBonus;
			clonedBill.SummaPrePayment = (decimal?)summaPrePaymentValue - summaPayByBonus;
			bool flag;
			if (!num)
			{
				Utilities.PrintReceiptRequest printReceiptRequest = CreatePrintRequest(cashRow);
				if (printReceiptRequest == null)
				{
					_printECRCheck.IsEnabled = true;
					return;
				}
				long poscrn = 0L;
				PosTerminalResponse posTerminalResponse = PayWithPOSTerminalIfChosen(ref poscrn);
				if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN))
				{
					printReceiptRequest.rrn = posTerminalResponse.RRN;
					if (!string.IsNullOrWhiteSpace(posTerminalResponse.TerminalId))
					{
						printReceiptRequest.terminalId = posTerminalResponse.TerminalId;
					}
				}
				ECRPrintCheckWindow eCRPrintCheckWindow = new ECRPrintCheckWindow(this, _ecr, cashRow.ECROperator, cashRow.ECROperatorPassword, cashRow.ShowECRTracking, printReceiptRequest, ECRPrintCheckWindow.ECRCheckType.Standard, (ECRPrintCheckWindow window, Utilities.PrintReceiptResponse response) =>
				{
					if (!bonus || Settings.ParametersService.PrintPrepaymentCheckSeparately())
					{
						ECRHelpers.PrintOnOtherPrinter(window, response, cashRow, bonus ? _clonedBill : PO, $"{ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value}.3");
					}
				}, (string checkNumber, string errorMessage) =>
				{
					int num3 = 1;
					List<PropertyWithValue> list2 = new List<PropertyWithValue>(num3);
					CollectionsMarshal.SetCount(list2, num3);
					CollectionsMarshal.AsSpan(list2)[0] = new PropertyWithValue("ECRCheckNum", checkNumber);
					List<PropertyWithValue> list3 = list2;
					if (bonus)
					{
						list3.Add(new PropertyWithValue("PrePaymentECRCheckNum", string.IsNullOrWhiteSpace(checkNumber) ? checkNumber : "XXXXXXXX"));
						_prePaymentECRCheckNum.Text = (string.IsNullOrWhiteSpace(checkNumber) ? checkNumber : "XXXXXXXX");
						PO.PrePaymentECRCheckNum = (string.IsNullOrWhiteSpace(checkNumber) ? checkNumber : "XXXXXXXX");
					}
					PO.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, errorMessage, POISN(), list3, PO.TS);
					_ecrCheckNum.Text = checkNumber;
					PO.ECRCheckNum = checkNumber;
				});
				flag = eCRPrintCheckWindow.Succeeded;
				if (flag)
				{
					ConfirmPOSTerminalPaymentIfDone(posTerminalResponse, savePaymentInfo: false, 0L);
					UpdateDocAndWindowAfterCheckPrint(eCRPrintCheckWindow.CheckNumber, eCRPrintCheckWindow.ECRCrn, forBonus: false, bonus);
					TradeSettings.CreateTradeClient().CommonServices.SetCompanyData(_cashDesk.Code, eCRPrintCheckWindow.TaxCode, eCRPrintCheckWindow.CompanyName, eCRPrintCheckWindow.CompanyAddress, eCRPrintCheckWindow.ECRCrn);
				}
				else
				{
					_printECRCheck.IsEnabled = true;
					CancelPOSTerminalPaymentIfDone(posTerminalResponse, poscrn);
				}
			}
			else
			{
				flag = true;
			}
			if (!(flag & bonus))
			{
				return;
			}
			Utilities.PrintReceiptRequest request = CreatePrePaymentRequest();
			PO.PrePaymentCashAmount = _summaCashSubtractBonus;
			PO.PrePaymentCardAmount = _summaPlCardSubtractBonus;
			ECRPrintCheckWindow eCRPrintCheckWindow2 = new ECRPrintCheckWindow(this, _ecr, cashRow.ECROperator, cashRow.ECROperatorPassword, cashRow.ShowECRTracking, request, ECRPrintCheckWindow.ECRCheckType.PrePayment, (ECRPrintCheckWindow window, Utilities.PrintReceiptResponse response) =>
			{
				if (!Settings.ParametersService.PrintPrepaymentCheckSeparately())
				{
					_clonedBill.PrePaymentECRCheckNum = PO.PrePaymentECRCheckNum;
					_clonedBill.ECRCheckNum = PO.ECRCheckNum;
				}
				ECRHelpers.PrintOnOtherPrinter(window, response, cashRow, Settings.ParametersService.PrintPrepaymentCheckSeparately() ? PO : _clonedBill, Settings.ParametersService.PrintPrepaymentCheckSeparately() ? $"{ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value}.5" : $"{ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value}.3");
			}, (string checkNumber, string errorMessage) =>
			{
				int num3 = 1;
				List<PropertyWithValue> list2 = new List<PropertyWithValue>(num3);
				CollectionsMarshal.SetCount(list2, num3);
				CollectionsMarshal.AsSpan(list2)[0] = new PropertyWithValue("PrePaymentECRCheckNum", checkNumber);
				List<PropertyWithValue> editedProperties2 = list2;
				if (checkNumber != "XXXXXXXX")
				{
					PO.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, errorMessage, POISN(), editedProperties2, PO.TS);
					_prePaymentECRCheckNum.Text = checkNumber;
					PO.PrePaymentECRCheckNum = checkNumber;
				}
			});
			if (eCRPrintCheckWindow2.Succeeded)
			{
				UpdateDocAndWindowAfterCheckPrint(eCRPrintCheckWindow2.CheckNumber, eCRPrintCheckWindow2.ECRCrn, forBonus: true);
			}
		}
		catch (Exception ex) when ((ex is Arcus2Exception || ex is PossoftPayClientException) ? true : false)
		{
			int num2 = 1;
			List<PropertyWithValue> list = new List<PropertyWithValue>(num2);
			CollectionsMarshal.SetCount(list, num2);
			CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("ECRCheckNum", "");
			List<PropertyWithValue> editedProperties = list;
			PO.TS = ECRHelpers.ChangeDocECRCheckInfo("", ex.Message, POISN(), editedProperties, PO.TS);
			_ecrCheckNum.Text = "";
			PO.ECRCheckNum = "";
			Settings.ErrorHandler(new ASException(ex.Message));
			_printECRCheck.IsEnabled = true;
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
			_printECRCheck.IsEnabled = true;
		}
	}

	private void CalcAllSummaBasedFromCalculatedBonuses()
	{
		_calculatedBonusValue += _addedBonusValue;
		_addedBonusValue = 0m;
		_summaPrePaymentValue += _summaPayByBonusValue;
		_summaPayByBonusValue = 0m;
		_calculatedBonusValue += CalcBonusFromRows();
		if (_summaCashValue >= _calculatedBonusValue)
		{
			_summaCashSubtractBonus = _calculatedBonusValue;
			_summaCashValue -= _calculatedBonusValue;
			return;
		}
		_summaCashSubtractBonus = _summaCashValue;
		_calculatedBonusValue -= _summaCashValue;
		_summaCashValue = 0m;
		if (_summaPlCardValue >= _calculatedBonusValue)
		{
			_summaPlCardSubtractBonus = _calculatedBonusValue;
			_summaPlCardValue -= _calculatedBonusValue;
			return;
		}
		_summaPlCardSubtractBonus = _summaPlCardValue;
		_calculatedBonusValue -= _summaPlCardValue;
		_summaPlCardValue = 0m;
		if (_summaPrePaymentValue >= _calculatedBonusValue)
		{
			_summaPrePaymentSubtractBonus = _calculatedBonusValue;
			_summaPrePaymentValue -= _calculatedBonusValue;
		}
		else
		{
			_summaCashSubtractBonus += _calculatedBonusValue;
			_summaCreditValue -= _calculatedBonusValue;
		}
	}

	private decimal CalcBonusFromRows()
	{
		return PO.MTList.Where((MTInvoiceBase.MTInvoiceSpecificationRow r) => !r.IsDeleted).Sum((MTInvoiceBase.MTInvoiceSpecificationRow r) => r.AccumulatedBonus);
	}

	private Utilities.PrintReceiptRequest CreatePrintRequest(CashCacheRow cashRow)
	{
		string departmentWithVAT = "";
		string departmentWithoutVAT = "";
		bool isDocumentWithVAT = _vatType.InList("1", "2", "5");
		decimal summaCashValue = _summaCashValue;
		decimal summaPlCardValue = _summaPlCardValue;
		decimal d = 0m;
		if (Settings.ParametersService.ECRCheckPrintCreditAsCash())
		{
			summaCashValue += _summaCreditValue;
		}
		else if (Settings.ParametersService.ECRCheckPrintCreditAsPartial())
		{
			d = _summaCreditValue;
		}
		if (Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.Cash.Value)
		{
			summaCashValue += _summaPayByBonusValue;
		}
		if (!TryGetECRDepartments(cashRow, isDocumentWithVAT, ref departmentWithVAT, ref departmentWithoutVAT))
		{
			return null;
		}
		List<Utilities.Item> items = CreateECRItemsForGoodsMode(isDocumentWithVAT, departmentWithVAT, departmentWithoutVAT);
		List<string> eMarks = CreateECREMarksList();
		int? paymentSystem = null;
		if (PO.POSTerminalType == POSTerminalTypesSE.ECRPOS)
		{
			paymentSystem = NonCashPaymentHelpers.GetECRPaymentSystem((ArmSoft.SME.Common.Constants.Common.NonCashPaymentType)_nonCashPaymentMethod);
		}
		return new Utilities.PrintReceiptRequest
		{
			mode = 2,
			paidAmount = decimal.ToDouble(summaCashValue),
			partialAmount = decimal.ToDouble(d),
			prePaymentAmount = decimal.ToDouble(_summaPrePaymentValue),
			paidAmountCard = decimal.ToDouble(summaPlCardValue),
			useExtPOS = (PO.POSTerminalType != POSTerminalTypesSE.ECRPOS.Value),
			PaymentSystem = paymentSystem,
			partnerTin = GetPartnerTin(),
			items = items,
			eMarks = eMarks
		};
	}

	private Utilities.PrintReceiptRequest CreatePrePaymentRequest()
	{
		int? paymentSystem = null;
		if (PO.POSTerminalType == "1")
		{
			paymentSystem = NonCashPaymentHelpers.GetECRPaymentSystem((ArmSoft.SME.Common.Constants.Common.NonCashPaymentType)PO.NonCashPaymentMethod);
		}
		return new Utilities.PrintReceiptRequest
		{
			mode = 3,
			paidAmount = decimal.ToDouble(_summaCashSubtractBonus),
			partialAmount = 0.0,
			prePaymentAmount = 0.0,
			paidAmountCard = decimal.ToDouble(_summaPlCardSubtractBonus),
			PaymentSystem = paymentSystem,
			useExtPOS = (PO.POSTerminalType != "1"),
			partnerTin = GetPartnerTin()
		};
	}

	private List<string> CreateECREMarksList()
	{
		List<string> list = new List<string>();
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT in PO.MTList)
		{
			if (mT.IsDeleted)
			{
				continue;
			}
			foreach (string marking in mT.MarkingList)
			{
				list.Add(marking);
			}
		}
		return list;
	}

	private void UpdateDocAndWindowAfterCheckPrint(string checkNumber, string ecrCRN, bool forBonus = false, bool fillXInBonus = false)
	{
		if (fillXInBonus)
		{
			_prePaymentECRCheckNum.Text = "XXXXXXXX";
			PO.PrePaymentECRCheckNum = "XXXXXXXX";
		}
		if (forBonus)
		{
			_prePaymentECRCheckNum.Text = checkNumber;
			PO.PrePaymentECRCheckNum = checkNumber;
		}
		else
		{
			_ecrCheckNum.Text = checkNumber;
			PO.ECRCheckNum = checkNumber;
		}
		PO.ECRCRN = ecrCRN;
		PO.IsCheckLimitAnswerYes = true;
		int num = 2;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("ECRCRN", PO.ECRCRN);
		span[1] = new PropertyWithValue("IsCheckLimitAnswerYes", Convert.ToString(PO.IsCheckLimitAnswerYes));
		List<PropertyWithValue> list2 = list;
		if (forBonus)
		{
			list2.Add(new PropertyWithValue("PrePaymentECRCheckNum", PO.PrePaymentECRCheckNum));
		}
		else
		{
			list2.Add(new PropertyWithValue("ECRCheckNum", PO.ECRCheckNum));
		}
		PO.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, "", POISN(), list2, PO.TS);
		LoadDataToControls();
		IsChanged = false;
		UpdateDataViewPanel(DVPID);
		SetControlsReadonly();
		if (Settings.ParametersService.PrintECRCheckAndNew() && !Settings.ParametersService.PrintECRAndProductCheck() && !fillXInBonus)
		{
			_viewMode = WindowViewMode.NewAfterSave;
			OnNewAfterSaveUpper();
		}
	}

	private bool ECRPreliminaryChecksAndInitalization(CashCacheRow cashRow)
	{
		if (CalcMTBillListRowCount(includeDeleted: false) <= 0)
		{
			return false;
		}
		if (_date.EditValue != TradeSettings.CreateTradeClient().CommonServices.GetServerDate().ToDateOnly())
		{
			throw new ASException(ArmSoft.SME.Common.Resources.DocDateNotCurrent);
		}
		if (cashRow == null)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.ValidEmptyCash);
		}
		if (string.IsNullOrWhiteSpace(cashRow.ECROperator) || string.IsNullOrWhiteSpace(cashRow.ECROperatorPassword))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidateECROperatorOrECROperatorPin, _cashDesk.Code), ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (!cashRow.ECROperator.IsNumeric())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.MustBeSaveDocBeforePrintECRCheck, ArmSoft.SME.Common.Resources.PrintECRCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if ((!StoredInDb || IsChanged || State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value) && !SaveDocument())
		{
			return false;
		}
		if (_anyAutoDiscountModeIsEnabled && IsUnderpaid())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PaymentsSmallerThanTotal, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
			return false;
		}
		if (_printPartnerTinOnCheck.IsChecked.HasValue && _printPartnerTinOnCheck.IsChecked.Value)
		{
			if (Settings.ParametersService.ECRCheckPrintCreditAsPartial())
			{
				if (_summaCash.Value != 0m || _summaPayByBonus.Value != 0m)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
					return false;
				}
			}
			else if (_summaCash.Value != 0m || _summaCredit.Value != 0m || _summaPayByBonus.Value != 0m)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return false;
			}
		}
		else
		{
			string cashSummLimitType = cashRow.CashSummLimitType;
			if (cashSummLimitType == CashLimitsSE.OnlyNonCash.Value)
			{
				if (Settings.ParametersService.ECRCheckPrintCreditAsPartial())
				{
					if (_summaCash.Value != 0m || _summaPayByBonus.Value != 0m)
					{
						ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
						return false;
					}
				}
				else if (_summaCash.Value != 0m || _summaCredit.Value != 0m || _summaPayByBonus.Value != 0m)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
					return false;
				}
			}
			else if (cashSummLimitType == CashLimitsSE.WithLimit.Value && _summa.Value > 300000m)
			{
				if (Settings.ParametersService.ECRCheckPrintCreditAsPartial())
				{
					if (_summaCash.Value != 0m || _summaPayByBonus.Value != 0m)
					{
						ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
						return false;
					}
				}
				else if (_summaCash.Value != 0m || _summaCredit.Value != 0m || _summaPayByBonus.Value != 0m)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
					return false;
				}
			}
		}
		ECRHelpers.InitECR(ref _ecr, _cashDesk.Code);
		if (_ecr == null)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		return true;
	}

	private bool TryGetECRDepartments(CashCacheRow cashRow, bool isDocumentWithVAT, ref string departmentWithVAT, ref string departmentWithoutVAT)
	{
		bool flag = false;
		bool flag2 = false;
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT in PO.MTList)
		{
			if (!mT.IsDeleted)
			{
				flag = flag || mT.VAT.Value;
				flag2 = flag2 || !mT.VAT.Value;
				if (flag & flag2)
				{
					break;
				}
			}
		}
		if (flag & isDocumentWithVAT)
		{
			if (string.IsNullOrWhiteSpace(cashRow.ECRDepWithVAT))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ECRDepWithVATNotSet, cashRow.ECRDepWithVAT, cashRow.ECROperator), ArmSoft.SME.Common.Resources.PrintECRCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return false;
			}
			if (!cashRow.ECRDepWithVAT.IsNumeric())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ECRInitFailed, default(ReadOnlySpan<object>)), ArmSoft.SME.Common.Resources.PrintECRCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return false;
			}
			departmentWithVAT = cashRow.ECRDepWithVAT;
		}
		if (flag2 || !isDocumentWithVAT)
		{
			if (string.IsNullOrWhiteSpace(cashRow.ECRDepWithOutVAT))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ECRDepWithOutVATNotSet, cashRow.ECRDepWithOutVAT, cashRow.ECROperator), ArmSoft.SME.Common.Resources.PrintECRCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return false;
			}
			if (!cashRow.ECRDepWithOutVAT.IsNumeric())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ECRInitFailed, default(ReadOnlySpan<object>)), ArmSoft.SME.Common.Resources.PrintECRCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return false;
			}
			departmentWithoutVAT = cashRow.ECRDepWithOutVAT;
		}
		return true;
	}

	private string GetPartnerTin()
	{
		if (!_printPartnerTinOnCheck.IsChecked.HasValue || !_printPartnerTinOnCheck.IsChecked.Value)
		{
			return null;
		}
		string result = "";
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partCode);
		if (item != null)
		{
			result = item.TaxCode;
		}
		return result;
	}

	private List<Utilities.Item> CreateECRItemsForGoodsMode(bool isDocumentWithVAT, string departmentWithVAT, string departmentWithoutVAT)
	{
		List<Utilities.Item> list = new List<Utilities.Item>();
		for (int i = 0; i < PO.MTList.Count; i++)
		{
			MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow = PO.MTList[i];
			if (mTInvoiceSpecificationRow.IsDeleted)
			{
				continue;
			}
			Utilities.Item item = new Utilities.Item();
			item.dep = Convert.ToInt32((mTInvoiceSpecificationRow.VAT.Value & isDocumentWithVAT) ? departmentWithVAT : departmentWithoutVAT);
			item.qty = mTInvoiceSpecificationRow.Quantity;
			decimal num = mTInvoiceSpecificationRow.DiscountPrice;
			decimal discount = mTInvoiceSpecificationRow.Discount;
			decimal num2 = mTInvoiceSpecificationRow.Summa;
			decimal num3 = mTInvoiceSpecificationRow.DiscountSumma;
			if (Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.DiscountAndPrePayment.Value)
			{
				num = (mTInvoiceSpecificationRow.DiscountPrice - mTInvoiceSpecificationRow.AccumulatedBonus / mTInvoiceSpecificationRow.Quantity).RoundAs(2);
				discount = 100m - (num / mTInvoiceSpecificationRow.Price * 100m).RoundAs(2);
				num2 = (mTInvoiceSpecificationRow.Summa - mTInvoiceSpecificationRow.AccumulatedBonus).RoundAs(2);
				num3 = (mTInvoiceSpecificationRow.DiscountSumma + mTInvoiceSpecificationRow.AccumulatedBonus).RoundAs(2);
			}
			_clonedBill.MTList[i].DiscountPrice = num;
			_clonedBill.MTList[i].Discount = discount;
			_clonedBill.MTList[i].Summa = num2;
			_clonedBill.MTList[i].DiscountSumma = num3;
			if (Settings.ParametersService.PrintECRCheckDiscount())
			{
				item.price = mTInvoiceSpecificationRow.Price;
				if (num3 != 0m)
				{
					string text = Settings.ParametersService.ECRDiscountType();
					if (text == ECRRowDiscountTypesSE.Percent.Value)
					{
						item.discount = discount;
						item.discountType = 1;
					}
					else if (text == ECRRowDiscountTypesSE.Price.Value)
					{
						item.discount = mTInvoiceSpecificationRow.Price - ((Settings.ParametersService.AllowChangeDiscountPrice() || mTInvoiceSpecificationRow.Quantity == 0m) ? num : (num2 / mTInvoiceSpecificationRow.Quantity).RoundAs(2));
						item.discountType = 2;
					}
					else
					{
						item.discount = num3;
						item.discountType = 4;
					}
				}
			}
			else if (Settings.ParametersService.AllowChangeDiscountPrice() || mTInvoiceSpecificationRow.Quantity == 0m)
			{
				item.price = num;
			}
			else
			{
				item.price = (num2 / mTInvoiceSpecificationRow.Quantity).RoundAs(2);
			}
			MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(mTInvoiceSpecificationRow.ItemCode);
			if (item2 == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NotInMaterials, mTInvoiceSpecificationRow.ItemCode));
			}
			item.productCode = ECRHelpers.GetItemCode(Settings.ParametersService.ASECRCheckItemCode(), item2);
			item.productName = ECRHelpers.GetItemName(Settings.ParametersService.ASECRCheckItemName(), item2, mTInvoiceSpecificationRow.ItemName);
			item.adgCode = item2.CPACode;
			if (string.IsNullOrWhiteSpace(item.adgCode))
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.CPAClassification));
			}
			if (item2.CPACode.Replace(".", "").Length == item2.CPACode.Length)
			{
				if (item2.CPACode.Length >= 4)
				{
					item.adgCode = item2.CPACode.Substring(0, 4);
				}
			}
			else if (item2.CPACode.Length >= 5)
			{
				item.adgCode = item2.CPACode.Substring(0, 5);
			}
			item.unit = mTInvoiceSpecificationRow.UnitBrief;
			if (_useMarking && !item2.IsWeight && item2.CheckMarking && (decimal)mTInvoiceSpecificationRow.MarkingList.Count != mTInvoiceSpecificationRow.Quantity && MarkingHelpers.IsGoodMarkable(mTInvoiceSpecificationRow.ItemCode, _useMarking, mTInvoiceSpecificationRow.MarkingList.Count != 0, _date.EditValue.Value, out var _))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MarkingHasMismatch, mTInvoiceSpecificationRow.ItemCode));
			}
			list.Add(item);
		}
		return list;
	}

	private PosTerminalResponse PayWithPOSTerminalIfChosen(ref long poscrn)
	{
		if (_summaPlCard.Value == 0m)
		{
			return null;
		}
		if (PO.PaymentAccepted && PO.POSTerminalType.InList("2", "4"))
		{
			return new PosTerminalResponse(PO.PaymentId.Contains(':') ? PO.PaymentId.Substring(PO.PaymentId.IndexOf(':') + 1).Trim() : PO.PaymentId, PO.POSTerminalID);
		}
		if (PO.POSTerminalType == "2")
		{
			return Arcus2Helpers.CheckAndPurchase(PO.POSTerminalID, _summaPlCard.Value);
		}
		if (PO.POSTerminalType == "4")
		{
			return PossoftPayClientHelper.Purchase(out poscrn, PO.POSTerminalAddress, _summaPlCard.Value);
		}
		return null;
	}

	private void ConfirmPOSTerminalPaymentIfDone(PosTerminalResponse posTerminalResponse, bool savePaymentInfo = false, long posCRN = 0L)
	{
		if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN) && (!(PO.POSTerminalType != "2") || !(PO.POSTerminalType != "4")) && !PO.PaymentAccepted)
		{
			UpdatePaymentId(posTerminalResponse.RRN, savePaymentInfo ? posTerminalResponse.TerminalId : "", (PO.POSTerminalType == "4") ? posCRN : 0);
		}
	}

	private void CancelPOSTerminalPaymentIfDone(PosTerminalResponse posTerminalResponse, long poscrn)
	{
		if (posTerminalResponse == null || string.IsNullOrWhiteSpace(posTerminalResponse.RRN))
		{
			return;
		}
		bool flag = false;
		if (PO.POSTerminalType.InList("2", "4"))
		{
			if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.CancelPaymentQuestion, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
			{
				ConfirmPOSTerminalPaymentIfDone(posTerminalResponse, savePaymentInfo: true, poscrn);
				return;
			}
			flag = true;
		}
		if (PO.POSTerminalType == "2")
		{
			Arcus2Helpers.Cancel(PO.POSTerminalID, _summaPlCard.Value, posTerminalResponse.RRN);
		}
		else if (PO.POSTerminalType == "4")
		{
			if (poscrn == 0L && !string.IsNullOrWhiteSpace(PO.POSCRN))
			{
				long.TryParse(PO.POSCRN, out poscrn);
			}
			PossoftPayClientHelper.Cancel(poscrn, PO.POSTerminalAddress);
		}
		else if (flag && PO.PaymentAccepted)
		{
			UpdatePaymentInfoIfCancelled();
		}
	}

	private void UpdatePaymentId(string paymentId, string posTerminalId = "", long posCRN = 0L)
	{
		try
		{
			string text = (SmartEnum<POSTerminalTypesSE, string>.TryFromValue(PO.POSTerminalType, out var result) ? (result.CaptionShort + ": ") : "");
			_nonCashPaymentId.Text = text + paymentId;
			_paymentAccepted = true;
			PO.PaymentId = _nonCashPaymentId.Text;
			PO.PaymentAccepted = true;
			if (!string.IsNullOrWhiteSpace(posTerminalId))
			{
				PO.POSTerminalID = posTerminalId;
			}
			if (posCRN != 0L)
			{
				PO.POSCRN = posCRN.ToString();
			}
			int num = 2;
			List<PropertyWithValue> list = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list, num);
			Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
			span[0] = new PropertyWithValue("PaymentId", PO.PaymentId);
			span[1] = new PropertyWithValue("PaymentAccepted", PO.PaymentAccepted.ToString());
			List<PropertyWithValue> properties = list;
			StoreRequest sr = new StoreRequest
			{
				IsDataChanged = true,
				LogComment = _nonCashPaymentId.Text,
				WithoutGrids = true,
				WithoutValidation = true,
				WithoutAccessCheck = true,
				RegenerateEntries = false
			};
			PO.TS = Settings.DocumentService.EditFields(PO.ISN, properties, sr);
			LoadDataToControls();
			IsChanged = false;
			UpdateDataViewPanel(DVPID);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void UpdatePaymentInfoIfCancelled()
	{
		try
		{
			_nonCashPaymentId.Text = "";
			_paymentAccepted = false;
			PO.PaymentId = _nonCashPaymentId.Text;
			PO.PaymentAccepted = _paymentAccepted;
			PO.POSTerminalID = "";
			PO.POSCRN = "";
			int num = 4;
			List<PropertyWithValue> list = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list, num);
			Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
			span[0] = new PropertyWithValue("PaymentId", PO.PaymentId);
			span[1] = new PropertyWithValue("PaymentAccepted", PO.PaymentAccepted.ToString());
			span[2] = new PropertyWithValue("POSTerminalID", PO.POSTerminalID);
			span[3] = new PropertyWithValue("POSCRN", PO.POSCRN);
			List<PropertyWithValue> properties = list;
			StoreRequest sr = new StoreRequest
			{
				IsDataChanged = true,
				LogComment = string.Format("{0}: {1}", SmartEnum<NonCashPaymentMethodsSE, short>.TryFromValue(PO.NonCashPaymentMethod, out var result) ? result.Caption : "", ArmSoft.SME.Trade.Common.Resources.IsCanceled),
				WithoutGrids = true,
				WithoutValidation = true,
				WithoutAccessCheck = true,
				RegenerateEntries = false
			};
			PO.TS = Settings.DocumentService.EditFields(POISN(), properties, sr);
			LoadDataToControls();
			IsChanged = false;
			UpdateDataViewPanel(DVPID);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ChangeMarkingStatus(MTBillSpecificationRow row)
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

	private void ShowShowMessageOnDiscount(string msg)
	{
		ArmSoft.SME.Core.UI.MessageBox.Show(msg, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
	}

	private void AddGiftGoodsToDoc(List<DiscountCacheRow> docDiscounts, List<MTComplectsPartsDataProvider.DataRow> docGiftSets)
	{
		foreach (DiscountCacheRow docDiscount in docDiscounts)
		{
			if (docDiscount.UseType == UsageMethodsSE.Gift.Value)
			{
				MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(docDiscount.GiftGoodID);
				if (itemById != null)
				{
					AddGiftItemToList(itemById, docDiscount.GiftPrice, docDiscount.GiftQuantity);
				}
			}
		}
		foreach (MTComplectsPartsDataProvider.DataRow docGiftSet in docGiftSets)
		{
			MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(docGiftSet.MTSourceID);
			if (itemById != null)
			{
				AddGiftItemToList(itemById, docGiftSet.Summ, docGiftSet.QTY);
			}
		}
	}

	private void SelectAndAddGiftGoodsToDoc(List<DiscountCacheRow> docDiscounts, Dictionary<string, List<MTComplectsPartsDataProvider.DataRow>> docGiftSets)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		List<GiftSetRowsDataProvider.DataRow> list = new List<GiftSetRowsDataProvider.DataRow>();
		foreach (DiscountCacheRow docDiscount in docDiscounts)
		{
			if (!(docDiscount.UseType == UsageMethodsSE.GiftSet.Value) || !docDiscount.SelectGift || !docGiftSets.TryGetValue(docDiscount.Code, out var value))
			{
				continue;
			}
			IDropDownView dropDownView = new GiftSetRows(value);
			ModalBrowser modalBrowser = new ModalBrowser(dropDownView)
			{
				Owner = this
			};
			modalBrowser.Caption = modalBrowser.Caption + " (" + docDiscount.Caption + ")";
			modalBrowser.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			while (modalBrowser.SelectedRowIndex == -1)
			{
				if (!modalBrowser.Show())
				{
					continue;
				}
				GiftSetRowsDataProvider.DataRow dataRow = (GiftSetRowsDataProvider.DataRow)dropDownView.GetRowByValue(dropDownView.ValueMember(modalBrowser.SelectedRowIndex));
				if (dataRow != null)
				{
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(dataRow.MTCode);
					if (item != null)
					{
						dictionary.Add(docDiscount.Code, item.Id);
						list.Add(dataRow);
					}
				}
			}
		}
		foreach (GiftSetRowsDataProvider.DataRow item3 in list)
		{
			_discountsCalculator.GenerateNewGiftDiscountsForDocument(dictionary);
			_discountsCalculator.CalculateNewGeneratedGiftDiscount(dictionary);
			MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(item3.MTCode);
			if (item2 != null)
			{
				AddGiftItemToList(item2, item3.Summ, item3.Quantity);
			}
		}
	}

	private void AddGiftItemToList(MaterialCacheRow goodsRow, decimal price, decimal qty)
	{
		MTBillSpecificationRow mtBillGridRow = new MTBillSpecificationRow();
		mtBillGridRow.IsGift = true;
		FillMTBillGridRow(mtBillGridRow, goodsRow, goodsRow.Unit, price, qty);
		mtBillGridRow.RowID = _maxRowID;
		_mtBillList.Add(mtBillGridRow);
		if (_distributByFEFO && mtBillGridRow.ItemType == ItemTypesSE.Goods.Value)
		{
			DistributeQtyByExpiredDate(mtBillGridRow.RowID, mtBillGridRow.Storage, mtBillGridRow.ItemCode, mtBillGridRow.UnitCode, mtBillGridRow.Quantity, goodsRow.Unit);
			mtBillGridRow.IsPartySelected = false;
		}
		_maxRowID++;
		_mtBillTableView.Grid.CurrentItem = mtBillGridRow;
		if (!_retail || mtBillGridRow.ItemType == GoodsTypesSE.Service.Value)
		{
			CalculateGridRowDiscountPrice(ref mtBillGridRow);
			CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, mtBillGridRow.DiscountPrice == 0m);
		}
		else
		{
			CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, calcDiscountPrice: false);
		}
		_mtBillGrid.ConfigureButtons();
		CalculateDocumentSumma();
	}

	private void CheckUsedBonusSum(decimal usedBonusSum)
	{
		if (_connectTob2barmsoftDiscountServer)
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partner.Code, _contract);
			if (contract != null && usedBonusSum > DiscountServerService.GetCardBalance(contract.DiscountCard))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.UsedBonusValidate);
			}
		}
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_connectTob2barmsoftDiscountServer = Settings.ParametersService.ConnectTob2barmsoftDiscountServer();
		_owner.Code = Settings.LoginService.UserName;
		_date.EditValue = ((!_allowChangeDate) ? DateTime.Today.ToDateOnly() : Settings.HostingEnvironment.CurrentDate);
		if (string.IsNullOrWhiteSpace(_cashDesk.Code))
		{
			_cashDesk.Code = Settings.ParametersService.DefaultCash();
			if (!POSTerminalHelpers.IsActivePOSTerminal(_cashDesk.Code) && string.IsNullOrWhiteSpace(_nonCashPaymentId.Text))
			{
				_paymentSystemLayout.Visibility = Visibility.Collapsed;
			}
			else
			{
				_paymentSystemLayout.Visibility = Visibility.Visible;
			}
		}
		SetCashierShiftNumber();
		DiscountPriceListType = _defaultMTCheckDiscountPriceType;
		if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(Settings.ParametersService.DefaultStorage()))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, Settings.ParametersService.DefaultStorage()), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
			_storage.Code = "";
		}
		else if (string.IsNullOrWhiteSpace(_storage.Code))
		{
			_storage.Code = Settings.ParametersService.DefaultStorage();
		}
		if (_viewMode != WindowViewMode.Copy)
		{
			_sumFromRound = 0m;
			_currCode.Code = (_retail ? "AMD" : Settings.ParametersService.DefaultMTBillCurr());
			_salesConsultant.Code = Settings.ParametersService.DefaultSalesConsultant();
			if (!string.IsNullOrWhiteSpace(_cashDesk.Code))
			{
				CashCacheRow.NonCashInfo nonCashInfo = TradeSettings.TradeCacheService.Cash.GetNonCashInfoListByCashCode(_cashDesk.Code).FirstOrDefault((CashCacheRow.NonCashInfo x) => x.Type == 5);
				if (nonCashInfo != null)
				{
					PO.POSTerminalType = nonCashInfo.Device;
					PO.POSTerminalID = nonCashInfo.POSTerminalID;
					PO.POSTerminalAddress = nonCashInfo.POSTerminalAddress;
					PO.BankAcquiring.PartnerCode = nonCashInfo.Partner;
					PO.BankAcquiring.ContractCode = nonCashInfo.Contract;
				}
			}
			else
			{
				PO.POSTerminalType = POSTerminalTypesSE.None.Value;
				PO.POSTerminalID = "";
				PO.POSTerminalAddress = "";
			}
		}
		else
		{
			decimal num = 0m;
			if (!_isShowStorageColInMTBill)
			{
				_isShowStorageColInMTBill = false;
				foreach (MTBillSpecificationRow mtBill in _mtBillList)
				{
					mtBill.Storage = _storage.Code;
					mtBill.AccumulatedBonus = 0m;
					mtBill.UsedBonus = 0m;
					if (!mtBill.IsDeleted)
					{
						num += mtBill.Summa;
					}
				}
			}
			else
			{
				foreach (MTBillSpecificationRow mtBill2 in _mtBillList)
				{
					mtBill2.AccumulatedBonus = 0m;
					mtBill2.UsedBonus = 0m;
					if (!mtBill2.IsDeleted)
					{
						num += mtBill2.Summa;
					}
				}
			}
			_summaPayByBonus.EditValue = 0m;
			if (!string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillRoundDegree()))
			{
				_summa.Value = num;
				_sumFromRound = Methods.RoundPrice(num, Settings.ParametersService.MTBillRoundDegree(), Settings.ParametersService.MTBillSummRoundUp()) - _summa.Value;
				_summa.EditValue = _summa.Value + _sumFromRound;
			}
			if (!string.IsNullOrWhiteSpace(_cashDesk.Code))
			{
				CashCacheRow.NonCashInfo nonCashInfo2 = TradeSettings.TradeCacheService.Cash.GetNonCashInfoListByCashCode(_cashDesk.Code).FirstOrDefault((CashCacheRow.NonCashInfo x) => x.Type == 5);
				if (nonCashInfo2 != null)
				{
					PO.POSTerminalType = nonCashInfo2.Device;
					PO.POSTerminalID = nonCashInfo2.POSTerminalID;
					PO.POSTerminalAddress = nonCashInfo2.POSTerminalAddress;
					PO.BankAcquiring.PartnerCode = nonCashInfo2.Partner;
					PO.BankAcquiring.ContractCode = nonCashInfo2.Contract;
				}
			}
			else
			{
				PO.POSTerminalType = POSTerminalTypesSE.None.Value;
				PO.POSTerminalID = "";
				PO.POSTerminalAddress = "";
			}
			UpdateChangeOrPaySum();
			if (_distributByFEFO)
			{
				PO.MTExpDocSelectedPartiesList.Clear();
				for (int num2 = 0; num2 < _mtBillList.Count; num2++)
				{
					if (_mtBillList[num2].ItemType == GoodsTypesSE.Good.Value)
					{
						DistributeQtyByExpiredDate(_mtBillList[num2].RowID, _storage.Code, _mtBillList[num2].ItemCode, _mtBillList[num2].UnitCode, _mtBillList[num2].Quantity);
						_mtBillList[num2].IsPartySelected = true;
					}
				}
			}
		}
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD")
		{
			_currCode.Code = "AMD";
			PO.ExchangeRate.Rate = 1m;
			PO.ExchangeRate.Base = 1m;
		}
		else
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			PO.ExchangeRate.Rate = exchangeRates.Rate;
			PO.ExchangeRate.Base = exchangeRates.Base;
		}
		if (_mtBillSimpleMode)
		{
			HideFieldsForMTBillSimpleMode();
		}
		InitPoleDisplay(_viewMode == WindowViewMode.Copy);
	}

	private void HideFieldsForMTBillSimpleMode()
	{
		((LayoutItemWithRequired)(object)_currCode.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_storage.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_partner.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_salesConsultant.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_discountPercent.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_comment.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_printPartnerTinOnCheck.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		if (_summaCredit.Value == 0m)
		{
			_summaCredit.Visibility = Visibility.Collapsed;
		}
		if (_summaPlCard.Value == 0m)
		{
			_summaPlCard.Visibility = Visibility.Collapsed;
		}
		if (_summaPayByBonus.Value == 0m)
		{
			_summaPayByBonus.Visibility = Visibility.Collapsed;
		}
		_accumulatedBonusCol.Visible = false;
		_accumulatedBonusCol.ShowInColumnChooser = false;
		_usedBonusSummCol.Visible = false;
		_usedBonusSummCol.ShowInColumnChooser = false;
	}

	protected override void OnNewAfterSave()
	{
		_storedBonus = _allowUseBonus;
		_connectTob2barmsoftDiscountServer = Settings.ParametersService.ConnectTob2barmsoftDiscountServer();
		_ecrCheckNum.Text = "";
		_prePaymentECRCheckNum.Text = "";
		base.OnNewAfterSave();
		_mtBillList.Clear();
		_maxRowID = 0;
		_sumFromRound = 0m;
		_partner.Code = "";
		_paymentAccepted = false;
		_nonCashPaymentId.Text = "";
		_nonCashPaymentMethod = NonCashPaymentMethodsSE.None.Value;
		_salesConsultant.Code = Settings.ParametersService.DefaultSalesConsultant();
		if (!string.IsNullOrWhiteSpace(_cashDesk.Code))
		{
			CashCacheRow.NonCashInfo nonCashInfo = TradeSettings.TradeCacheService.Cash.GetNonCashInfoListByCashCode(_cashDesk.Code).FirstOrDefault((CashCacheRow.NonCashInfo x) => x.Type == 5);
			if (nonCashInfo != null)
			{
				PO.POSTerminalType = nonCashInfo.Device;
				PO.POSTerminalID = nonCashInfo.POSTerminalID;
				PO.POSTerminalAddress = nonCashInfo.POSTerminalAddress;
				PO.BankAcquiring.PartnerCode = nonCashInfo.Partner;
				PO.BankAcquiring.ContractCode = nonCashInfo.Contract;
			}
		}
		else
		{
			PO.POSTerminalType = POSTerminalTypesSE.None.Value;
			PO.POSTerminalID = "";
			PO.POSTerminalAddress = "";
		}
		SetCashierShiftNumber();
		PO.PartnerContractGroup.ContractCode = _contract;
		_discountPercent.EditValue = 0m;
		_vatType = PO.VATType;
		_priceListType = Settings.ParametersService.DefaultMTCheckPriceType();
		_summa.EditValue = 0m;
		_summaCash.EditValue = 0m;
		_summaChange.EditValue = 0m;
		_summaCredit.EditValue = 0m;
		_summaPlCard.EditValue = 0m;
		_summaPayByBonus.EditValue = 0m;
		_addedBonus.EditValue = 0m;
		_sumAddBonus = 0m;
		_summaPrePayment.EditValue = 0m;
		_printPartnerTinOnCheck.IsChecked = false;
		_comment.Text = "";
		((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_addedBonus.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_summaChange.Parent).Label = ArmSoft.SME.Trade.Common.Resources.ForPay;
		((LayoutItemWithRequired)(object)_summaChange.Parent).Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326");
		_summaChange.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326");
		_accumulatedBonusCol.Visible = _isShowAccumulatedBonusColInMTBill && _storedBonus;
		_accumulatedBonusCol.ShowInColumnChooser = _accumulatedBonusCol.Visible;
		_usedBonusSummCol.Visible = _isShowUsedBonusColInMTBill && _storedBonus;
		_usedBonusSummCol.ShowInColumnChooser = _usedBonusSummCol.Visible;
		_discountLogsList.Clear();
		DiscountPriceListType = _defaultMTCheckDiscountPriceType;
		if (_mtBillSimpleMode)
		{
			HideFieldsForMTBillSimpleMode();
		}
		_prepaymentPartnerCode = "";
		_prepaymentContractCode = "";
		_owner.Code = Settings.LoginService.UserName;
		if (_isShowInfoPanel)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearInfoPanel(CleanInfoPanel), Array.Empty<object>());
		}
		InitPoleDisplay();
		_setFocusOnActivation = _inputValue;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_connectTob2barmsoftDiscountServer = Settings.ParametersService.ConnectTob2barmsoftDiscountServer();
		if (_mtBillSimpleMode)
		{
			HideFieldsForMTBillSimpleMode();
		}
		InitPoleDisplay(onEdit: true);
	}

	private void Partner_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		IDropDownView dataView = _partner.DataView;
		ModalBrowser modalBrowser = new ModalBrowser(dataView)
		{
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			ContractDataProvider.DataRow.Key key = (ContractDataProvider.DataRow.Key)dataView.KeyMember(modalBrowser.SelectedRowIndex);
			if (key != null)
			{
				_contract = key.Contract;
				PO.PartnerContractGroup.ContractCode = _contract;
				_partCode = key.PartCode;
				if (!_readOnlyAfterCheckPrint)
				{
					PartnerCacheRow.ContractRow contractRow = null;
					if (!string.IsNullOrWhiteSpace(dataView.ValueMember(modalBrowser.SelectedRowIndex)))
					{
						contractRow = TradeSettings.TradeCacheService.Partner.GetContract(_partCode, _contract, out var partnerCacheRow);
						if (!PO.StoredInDb)
						{
							ValidateBonusCard(partnerCacheRow, contractRow);
						}
					}
					if (contractRow != null)
					{
						if (_bonusType == BonusTypeSE.ByPercent.Value)
						{
							_partContractBonusPercentOrPoint = contractRow.Bonus;
						}
						else
						{
							_partContractBonusPercentOrPoint = contractRow.Point;
						}
						if (!string.IsNullOrWhiteSpace(contractRow.PriceType) && _priceListType != contractRow.PriceType)
						{
							_priceListType = contractRow.PriceType;
							PriceListTypeEditValueChanged(_priceListType);
						}
					}
					if (_paramUseDiscount && _alwaysCalcDiscountAuto)
					{
						On_DiscountClick();
					}
					foreach (MTBillSpecificationRow mtBill in _mtBillList)
					{
						mtBill.AccumulatedBonus = 0m;
						mtBill.UsedBonus = 0m;
					}
					_summaPayByBonus.EditValue = 0m;
					((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
					UpdateChangeOrPaySum();
					if (!string.IsNullOrWhiteSpace(_prepaymentPartnerCode) && _prepaymentPartnerCode == _partCode && _prepaymentContractCode == PO.PartnerContractGroup.ContractCode)
					{
						_prepaymentPartnerCode = "";
						_prepaymentContractCode = "";
					}
				}
			}
			_partClickDropDown = true;
			_partner.Code = dataView.ValueMember(modalBrowser.SelectedRowIndex);
		}
		e.Cancel = true;
	}

	private void Partner_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		Partner_EditValueChanged();
	}

	private void Partner_EditValueChanged()
	{
		PartnerCacheRow partnerCacheRow = null;
		if (!string.IsNullOrWhiteSpace(_partner.Code))
		{
			partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(_partner.Code);
		}
		if (partnerCacheRow != null)
		{
			PartnerCacheRow.ContractRow contractRow = (IsLoadingDataToControls ? TradeSettings.TradeCacheService.Partner.GetContract(_partner.Code, PO.PartnerContractGroup.ContractCode, out var partnerCacheRow2) : TradeSettings.TradeCacheService.Partner.GetDefaultContract(_partner.Code, "AMD", out partnerCacheRow2));
			if (!_readOnlyAfterCheckPrint)
			{
				if (contractRow != null)
				{
					if (!_partClickDropDown && !_partReadMagneticCard)
					{
						if (!PO.StoredInDb && !IsOfflineDoc)
						{
							ValidateBonusCard(partnerCacheRow2, contractRow);
						}
						if (_bonusType == BonusTypeSE.ByPercent.Value)
						{
							_partContractBonusPercentOrPoint = contractRow.Bonus;
						}
						else
						{
							_partContractBonusPercentOrPoint = contractRow.Point;
						}
						if (!string.IsNullOrWhiteSpace(contractRow.PriceType) && _priceListType != contractRow.PriceType)
						{
							_priceListType = contractRow.PriceType;
							PriceListTypeEditValueChanged(_priceListType);
						}
						PO.PartnerContractGroup.ContractCode = contractRow.Contract;
						_contract = PO.PartnerContractGroup.ContractCode;
						_partCode = partnerCacheRow2.PartCode;
						if (!IsLoadingDataToControls)
						{
							if (_paramUseDiscount)
							{
								_discountPercent.EditValue = contractRow.Percent;
							}
							foreach (MTBillSpecificationRow mtBill in _mtBillList)
							{
								mtBill.AccumulatedBonus = 0m;
								mtBill.UsedBonus = 0m;
							}
							_summaPayByBonus.EditValue = 0m;
							((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
						}
					}
					else
					{
						PO.PartnerContractGroup.ContractCode = _contract;
						contractRow = TradeSettings.TradeCacheService.Partner.GetContract(_partner.Code, _contract, out partnerCacheRow2);
						if (!PO.StoredInDb)
						{
							ValidateBonusCard(partnerCacheRow2, contractRow);
						}
						if (contractRow != null)
						{
							if (_bonusType == BonusTypeSE.ByPercent.Value)
							{
								_partContractBonusPercentOrPoint = contractRow.Bonus;
							}
							else
							{
								_partContractBonusPercentOrPoint = contractRow.Point;
							}
							if (_paramUseDiscount)
							{
								_discountPercent.EditValue = contractRow.Percent;
							}
							if (!string.IsNullOrWhiteSpace(contractRow.PriceType) && _priceListType != contractRow.PriceType)
							{
								_priceListType = contractRow.PriceType;
								PriceListTypeEditValueChanged(_priceListType);
							}
							_partCode = partnerCacheRow2.PartCode;
							if (!IsLoadingDataToControls)
							{
								foreach (MTBillSpecificationRow mtBill2 in _mtBillList)
								{
									mtBill2.AccumulatedBonus = 0m;
									mtBill2.UsedBonus = 0m;
								}
								_summaPayByBonus.EditValue = 0m;
								((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
							}
						}
						_partClickDropDown = false;
						_partReadMagneticCard = false;
					}
					if (!string.IsNullOrWhiteSpace(_prepaymentPartnerCode) && _prepaymentPartnerCode == _partner.Code && _prepaymentContractCode == PO.PartnerContractGroup.ContractCode)
					{
						_prepaymentPartnerCode = "";
						_prepaymentContractCode = "";
					}
				}
				else
				{
					PO.PartnerContractGroup.ContractCode = "";
					if (_mtBillList.Count == 0 && !IsLoadingDataToControls)
					{
						_discountPercent.EditValue = 0m;
					}
				}
			}
			else if (contractRow != null)
			{
				PO.PartnerContractGroup.ContractCode = contractRow.Contract;
				_contract = PO.PartnerContractGroup.ContractCode;
				_partCode = partnerCacheRow2.PartCode;
			}
		}
		else
		{
			PO.PartnerContractGroup.ContractCode = "";
			_sumAddBonus = 0m;
			if (!_readOnlyAfterCheckPrint)
			{
				_isPartContractWithBonus = false;
				_partContractBonusPercentOrPoint = 0m;
				_contract = PO.PartnerContractGroup.ContractCode;
				_partCode = "";
				if (!IsLoadingDataToControls)
				{
					if (_mtBillList.Count == 0 || (_paramUseDiscount && _anyAutoDiscountModeIsEnabled))
					{
						_discountPercent.EditValue = 0m;
					}
					foreach (MTBillSpecificationRow mtBill3 in _mtBillList)
					{
						mtBill3.AccumulatedBonus = 0m;
						mtBill3.UsedBonus = 0m;
					}
					_summaPayByBonus.EditValue = 0m;
					_summaCredit.EditValue = 0m;
					((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
				}
			}
		}
		UpdateChangeOrPaySum();
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		SetBuyerInfoToPoleDisplay();
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (IsOfflineDoc)
		{
			return;
		}
		bool flag = true;
		if (!string.IsNullOrWhiteSpace(e.NewValue.ToString()) && TradeSettings.TradeCacheService.Storage.GetItem(e.NewValue.ToString()) != null && !_storage.IsReadOnly)
		{
			if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(e.NewValue.ToString()))
			{
				e.Handled = true;
				_storage.Code = e.OldValue.ToString();
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, e.NewValue.ToString()), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			if (_mtBillList.Count != 0 && !_isShowStorageColInMTBill)
			{
				if (_mtBillList.Any((MTBillSpecificationRow row) => row.ItemType == GoodsTypesSE.Good.Value && !string.IsNullOrWhiteSpace(row.Storage) && row.Storage != _storage.Code) && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ChangeStorageValues, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
				{
					flag = false;
					_storage.Code = (string)e.OldValue;
				}
				if (flag)
				{
					foreach (MTBillSpecificationRow mtBill in _mtBillList)
					{
						if (mtBill.ItemType == GoodsTypesSE.Good.Value)
						{
							mtBill.Storage = (string)e.NewValue;
						}
					}
					if (_paramUseDiscount && _alwaysCalcDiscountAuto)
					{
						On_DiscountClick();
					}
					CalculateDocumentSumma();
				}
			}
		}
		if (((string)e.NewValue).TrimStart() == ((string)e.OldValue).TrimStart() || !_storage.DoValidate())
		{
			return;
		}
		_retail = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code)?.Retail ?? false;
		if (_retail)
		{
			_priceListTypeRoundDgr = "";
		}
		else
		{
			_priceListTypeRoundDgr = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(_priceListType);
		}
		if (_retail)
		{
			_priceCol.AllowEditing = DefaultBoolean.False;
		}
		else if (_allowChangeSalePrices)
		{
			_priceCol.AllowEditing = DefaultBoolean.True;
		}
		if (_mtBillList.Count != 0)
		{
			if (!_distributByFEFO)
			{
				foreach (MTBillSpecificationRow mtBill2 in _mtBillList)
				{
					if (mtBill2.ItemType == GoodsTypesSE.Good.Value && mtBill2.IsPartySelected != false)
					{
						ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.PartiesValuesChanged, ArmSoft.SME.Common.Resources.Storage), Caption, MessageBoxButton.OK, MessageBoxImage.Asterisk);
						break;
					}
				}
			}
			if (_distributByFEFO)
			{
				PO.MTExpDocSelectedPartiesList.Clear();
				for (int num = 0; num <= _mtBillList.Count - 1; num++)
				{
					if (_mtBillList[num].ItemType == GoodsTypesSE.Good.Value)
					{
						DistributeQtyByExpiredDate(_mtBillList[num].RowID, _storage.Code, _mtBillList[num].ItemCode, _mtBillList[num].UnitCode, _mtBillList[num].Quantity);
						_mtBillList[num].IsPartySelected = true;
					}
				}
			}
			else
			{
				for (int num2 = 0; num2 <= _mtBillList.Count - 1; num2++)
				{
					if (_mtBillList[num2].ItemType == GoodsTypesSE.Good.Value)
					{
						_mtBillList[num2].IsPartySelected = false;
					}
				}
				PO.MTExpDocSelectedPartiesList.Clear();
			}
			if (_retail)
			{
				foreach (MTBillSpecificationRow mtBill3 in _mtBillList)
				{
					if (mtBill3.ItemType == GoodsTypesSE.Good.Value)
					{
						mtBill3.Price = 0m;
						mtBill3.DiscountPrice = 0m;
						mtBill3.Summa = 0m;
						mtBill3.DiscountSumma = 0m;
					}
				}
				if (_paramUseDiscount && _alwaysCalcDiscountAuto)
				{
					On_DiscountClick();
				}
			}
		}
		if (!_paramUseDiscount)
		{
			if (!_allowChangeSalePrices)
			{
				_priceCol.AllowEditing = DefaultBoolean.False;
				_priceCol.AllowFocus = false;
				_summaCol.AllowEditing = DefaultBoolean.False;
				_summaCol.AllowFocus = false;
			}
			else if (_retail)
			{
				_priceCol.AllowEditing = DefaultBoolean.False;
				_priceCol.AllowFocus = true;
				_summaCol.AllowEditing = DefaultBoolean.False;
				_summaCol.AllowFocus = false;
			}
			else
			{
				_priceCol.AllowEditing = DefaultBoolean.True;
				_priceCol.AllowFocus = true;
				_summaCol.AllowEditing = DefaultBoolean.True;
				_summaCol.AllowFocus = true;
			}
		}
		else if (!_allowChangeSalePrices && !_allowChangeDiscountPrices)
		{
			_priceCol.AllowEditing = DefaultBoolean.False;
			_priceCol.AllowFocus = false;
			_discountCol.AllowEditing = DefaultBoolean.False;
			_discountCol.AllowFocus = false;
			_discountPriceCol.AllowEditing = DefaultBoolean.False;
			_discountPriceCol.AllowFocus = false;
			_summaCol.AllowEditing = DefaultBoolean.False;
			_summaCol.AllowFocus = false;
		}
		else if (_allowChangeSalePrices && !_allowChangeDiscountPrices)
		{
			if (_retail)
			{
				_priceCol.AllowEditing = DefaultBoolean.False;
			}
			else
			{
				_priceCol.AllowEditing = DefaultBoolean.True;
			}
			_priceCol.AllowFocus = true;
			_discountCol.AllowEditing = DefaultBoolean.False;
			_discountCol.AllowFocus = false;
			_discountPriceCol.AllowEditing = DefaultBoolean.False;
			_discountPriceCol.AllowFocus = false;
			_summaCol.AllowEditing = DefaultBoolean.False;
			_summaCol.AllowFocus = false;
		}
		else if (!_allowChangeSalePrices && _allowChangeDiscountPrices)
		{
			_priceCol.AllowEditing = DefaultBoolean.False;
			_priceCol.AllowFocus = false;
			_discountCol.AllowEditing = DefaultBoolean.True;
			_discountCol.AllowFocus = true;
			_discountPriceCol.AllowEditing = DefaultBoolean.True;
			_discountPriceCol.AllowFocus = true;
			_summaCol.AllowEditing = DefaultBoolean.True;
			_summaCol.AllowFocus = true;
		}
		else
		{
			if (_retail)
			{
				_priceCol.AllowEditing = DefaultBoolean.False;
			}
			else
			{
				_priceCol.AllowEditing = DefaultBoolean.True;
			}
			_priceCol.AllowFocus = true;
			_discountCol.AllowEditing = DefaultBoolean.True;
			_discountCol.AllowFocus = true;
			_discountPriceCol.AllowEditing = DefaultBoolean.True;
			_discountPriceCol.AllowFocus = true;
			_summaCol.AllowEditing = DefaultBoolean.True;
			_summaCol.AllowFocus = true;
		}
	}

	private void CashDesk_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_cashDesk.Code) && TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk.Code) != null && !TradeSettings.CreateTradeClient().CommonServices.IsCashDeskAvailable((string)e.NewValue))
		{
			e.Handled = true;
			_cashDesk.Code = (string)e.OldValue;
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.CashDeskNotAvailForUser, Settings.LoginService.UserName, (string)e.NewValue), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return;
		}
		if ((string)e.NewValue != (string)e.OldValue && _viewMode != WindowViewMode.Copy)
		{
			SetPOSTerminalInfo();
		}
		SetCashierShiftNumber();
		SetNextDocumentNumber();
	}

	protected override string NextDocNumberRelatedInfo()
	{
		return _cashDesk.Code;
	}

	private void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			return;
		}
		if (!string.IsNullOrWhiteSpace(_currCode.Code))
		{
			try
			{
				DiscountPriceListType = (_anyAutoDiscountModeIsEnabled ? "" : _defaultMTCheckDiscountPriceType);
				if (!string.IsNullOrWhiteSpace(_discountPriceListCurr) && _discountPriceListCurr != _currCode.Code && _discountPriceListCurr != "AMD")
				{
					ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_discountPriceListCurr, _date.EditValue.Value);
					_discountPriceListCurrRate = exchangeRates.Rate;
					_discountPriceListCurrBase = exchangeRates.Base;
				}
				if (_currCode.Code != "AMD")
				{
					ExchangeRate exchangeRates2 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
					_docCurrRate = exchangeRates2.Rate;
					_docCurrBase = exchangeRates2.Base;
				}
			}
			catch
			{
			}
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		if (string.IsNullOrWhiteSpace(_partner.Code) || PO.StoredInDb)
		{
			return;
		}
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partner.Code, PO.PartnerContractGroup.ContractCode, out var partnerCacheRow);
		try
		{
			if (TradeSettings.CreateTradeClient().CommonServices.BonusCardValidation(partnerCacheRow.PartCode, contract.Contract, _date.EditValue, Settings.ParametersService.ConnectTob2barmsoftDiscountServer()))
			{
				return;
			}
			if (contract.BonusCardCanceled)
			{
				_partner.Code = "";
				if (_allowUseBonus && contract.IsBonusCard)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsCanceled);
				}
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsCanceled);
			}
			_partner.Code = "";
			if (_allowUseBonus && contract.IsBonusCard && Settings.ParametersService.ConnectTob2barmsoftDiscountServer())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsNotValidForBSUser);
			}
			if (_allowUseBonus && contract.IsBonusCard)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsNotValid);
			}
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsNotValid);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ReturnBill_Click(object sender, RoutedEventArgs e)
	{
		if (Settings.UserAccessService.GetDocumentPermission(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn).AllowAdd)
		{
			if (!_date.DoValidate() || !_date.EditValue.HasValue)
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			Guid guid = Guid.Empty;
			DocumentsJournal documentsJournal = new DocumentsJournal(this, ReportTypesSE.BillDocument, new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill })
			{
				StartDate = _date.EditValue.Value.AddDays(-30),
				EndDate = _date.EditValue,
				PartnerCode = _partner.Code,
				DocState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Processed.ValueUI
			};
			ModalBrowser modalBrowser = new ModalBrowser(documentsJournal);
			modalBrowser.Owner = this;
			modalBrowser.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			if (modalBrowser.Show())
			{
				guid = (Guid)documentsJournal.KeyMember(modalBrowser.SelectedRowIndex);
			}
			if (guid != Guid.Empty)
			{
				CreateBasedOnDocHelper.CreateDocument(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn, guid, DVPID);
			}
			return;
		}
		throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.UserCannotCreateBillReturn, Settings.LoginService.UserName));
	}

	private void EMarkReceipt_Click(object sender, RoutedEventArgs e)
	{
		EMarkReceipt();
	}

	private void EMarkReceipt()
	{
		MarkingHelpers.ExportEMarkReceipts(this, _readOnlyAfterCheckPrint, forPOS: false, _storage.Code);
	}

	private void ActualQuantitiesCheck_Click(object sender, RoutedEventArgs e)
	{
		ValidateDocFields();
		if ((!PO.StoredInDb || IsChanged) && !SaveDocument(isAsDraft: true))
		{
			return;
		}
		ActualQuantitiesCheckWindow actualQuantitiesCheckWindow = new ActualQuantitiesCheckWindow(this, _mtBillList, _date.EditValue.Value, _docNum.EditValue.ToString());
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
					for (int i = 0; i <= _mtBillList.Count - 1; i++)
					{
						MTBillSpecificationRow mTBillSpecificationRow = _mtBillList[i];
						if (item2.MTID != mTBillSpecificationRow.ItemID || !(item2.UnitCode == mTBillSpecificationRow.UnitCode))
						{
							continue;
						}
						decimal num = mTBillSpecificationRow.Quantity + item2.VarianceQuantity;
						TradeSettings.TradeCacheService.Material.GetItem(item2.MTCode);
						MTBillSpecificationRow MTBillGridRow = (MTBillSpecificationRow)_mtBillGrid.Grid.GetRowByListIndex(i);
						if (num < 0m)
						{
							mTBillSpecificationRow.Quantity = 0m;
							if (MTBillGridRow.ItemType == GoodsTypesSE.Good.Value)
							{
								if (_distributByFEFO)
								{
									PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == MTBillGridRow.RowID);
									DistributeQtyByExpiredDate(MTBillGridRow.RowID, MTBillGridRow.Storage, MTBillGridRow.ItemCode, MTBillGridRow.UnitCode, mTBillSpecificationRow.Quantity);
									MTBillGridRow.IsPartySelected = true;
								}
								else
								{
									PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == MTBillGridRow.RowID);
									MTBillGridRow.IsPartySelected = false;
								}
							}
							MTBillGridRow.Quantity = mTBillSpecificationRow.Quantity;
							if ((!_retail || MTBillGridRow.ItemType == GoodsTypesSE.Service.Value) && MTBillGridRow.Price == 0m && _priceListType != "")
							{
								MTBillGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, MTBillGridRow.ItemID, MTBillGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
								MTBillGridRow.Price = Methods.RoundPrice(MTBillGridRow.Price, _priceListTypeRoundDgr);
							}
							MTBillGridRow.Summa = (mTBillSpecificationRow.Quantity * MTBillGridRow.DiscountPrice).RoundAs(2);
							CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, calcDiscountPrice: false);
							item2.VarianceQuantity = num;
							continue;
						}
						mTBillSpecificationRow.Quantity = item2.ActualQuantity;
						if (MTBillGridRow.ItemType == GoodsTypesSE.Good.Value)
						{
							if (_distributByFEFO)
							{
								PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == MTBillGridRow.RowID);
								DistributeQtyByExpiredDate(MTBillGridRow.RowID, MTBillGridRow.Storage, MTBillGridRow.ItemCode, MTBillGridRow.UnitCode, mTBillSpecificationRow.Quantity);
								MTBillGridRow.IsPartySelected = true;
							}
							else
							{
								PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == MTBillGridRow.RowID);
								MTBillGridRow.IsPartySelected = false;
							}
						}
						MTBillGridRow.Quantity = mTBillSpecificationRow.Quantity;
						if ((!_retail || MTBillGridRow.ItemType == GoodsTypesSE.Service.Value) && MTBillGridRow.Price == 0m && _priceListType != "")
						{
							MTBillGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, MTBillGridRow.ItemID, MTBillGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
							MTBillGridRow.Price = Methods.RoundPrice(MTBillGridRow.Price, _priceListTypeRoundDgr);
						}
						MTBillGridRow.Summa = (mTBillSpecificationRow.Quantity * MTBillGridRow.DiscountPrice).RoundAs(2);
						CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, calcDiscountPrice: false);
						break;
					}
				}
				else
				{
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(item2.MTCode);
					_mQty_Weight = item2.VarianceQuantity;
					AddItemToList(item, item2.MTCode, barCode: false, 0m, item2.UnitCode);
				}
			}
			IsChanged = true;
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		_mtBillGrid.ConfigureButtons();
		CalculateDocumentSumma();
	}

	private void SelectMarking_Click(object sender, RoutedEventArgs e)
	{
		if (_mtBillTableView.Grid.CurrentItem == null)
		{
			return;
		}
		MTBillSpecificationRow mTBillSpecificationRow = (MTBillSpecificationRow)_mtBillTableView.Grid.CurrentItem;
		if (mTBillSpecificationRow == null)
		{
			return;
		}
		if (!MarkingHelpers.IsGoodMarkable(mTBillSpecificationRow.ItemCode, _useMarking, mTBillSpecificationRow.MarkingList.Count != 0, _date.EditValue.Value, out var isRowMarkable))
		{
			if (string.IsNullOrWhiteSpace(mTBillSpecificationRow.ItemCode))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
			}
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotMarkable, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
			return;
		}
		mTBillSpecificationRow.IsRowMarkable = isRowMarkable;
		List<string> allMarkings = _mtBillList.SelectMany((MTBillSpecificationRow row) => row.MarkingList).ToList();
		MarkingSelectWindow markingSelectWindow = new MarkingSelectWindow(this, _readOnlyAfterCheckPrint, mTBillSpecificationRow.IsRowMarkable, mTBillSpecificationRow.ItemCode, mTBillSpecificationRow.ItemName, mTBillSpecificationRow.MarkingList, allMarkings, mTBillSpecificationRow.Quantity);
		if (markingSelectWindow.ShowDialog().Value && !markingSelectWindow.IsViewOnlyMode)
		{
			mTBillSpecificationRow.MarkingList.Clear();
			foreach (MarkingSelectWindow.MarkingRow marking in markingSelectWindow.MarkingList)
			{
				mTBillSpecificationRow.MarkingList.Add(marking.Marking);
			}
			if (mTBillSpecificationRow.Quantity < (decimal)mTBillSpecificationRow.MarkingList.Count)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTBillSpecificationRow.ItemCode);
				if (item == null || !item.IsWeight)
				{
					mTBillSpecificationRow.Quantity = mTBillSpecificationRow.MarkingList.Count;
					ChangeRowQty(mTBillSpecificationRow);
				}
			}
			ChangeMarkingStatus(mTBillSpecificationRow);
		}
		CalcSummAndSummVAT();
		UpdateChangeOrPaySum();
		RowInfoChange?.Invoke(mTBillSpecificationRow);
	}

	private void SetPOSTerminalInfo()
	{
		CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk.Code);
		CashCacheRow.NonCashInfo nonCashInfo = null;
		if (item != null)
		{
			nonCashInfo = TradeSettings.TradeCacheService.Cash.GetNonCashInfoListByCashCode(_cashDesk.Code).FirstOrDefault((CashCacheRow.NonCashInfo x) => x.Type == 5);
		}
		else
		{
			PO.POSTerminalType = "0";
			PO.BankAcquiring.PartnerCode = "";
			PO.BankAcquiring.ContractCode = "";
			PO.POSTerminalID = "";
			PO.POSTerminalAddress = "";
			_nonCashPaymentMethod = 0;
		}
		if (nonCashInfo != null)
		{
			PO.POSTerminalType = nonCashInfo.Device;
			PO.POSTerminalID = nonCashInfo.POSTerminalID;
			PO.POSTerminalAddress = nonCashInfo.POSTerminalAddress;
			PO.BankAcquiring.PartnerCode = nonCashInfo.Partner;
			PO.BankAcquiring.ContractCode = nonCashInfo.Contract;
			_nonCashPaymentMethod = 0;
		}
	}

	private void InputValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		_realText += e.Text;
	}

	private void InputValue_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0 && !_inputValue.IsReadOnly && !string.IsNullOrWhiteSpace(_inputValue.Text))
			{
				e.Handled = true;
				_inputValue.Text = MarkingHelper.CleanScannerSymbols(_realText.Contains('\u001d') ? _realText : _inputValue.Text);
				On_BarCodeEnterClick();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			_realText = "";
		}
	}

	private void Summa_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_isShowPoleDisplay)
		{
			int num = CalcMTBillListRowCount(includeDeleted: false);
			if (num == 0)
			{
				ClearPoleDisplay(string.IsNullOrWhiteSpace(_partCode));
				return;
			}
			SetTotalInfoToPoleDisplay();
			SetRowCountInfoToPoleDisplay(num);
		}
	}

	protected override void OnClosed(EventArgs e)
	{
		if (_isShowPoleDisplay && TradeSettings.PoleDisplay != null)
		{
			ClearPoleDisplay(clearAll: true);
		}
		base.OnClosed(e);
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		if (_isAllowChangeGoodRowInBill || (PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && (_mtBillList.Count == 0 || PO.StoredInDb)) || !IsChanged)
		{
			base.OnClosing(e);
		}
		else
		{
			e.Cancel = true;
		}
	}

	private void FillPartnerEmployeeData(PartnerCacheRow partner, PartnerCacheRow.ContractRow contractRow, EmployeeCacheRow employee)
	{
		if (partner == null || contractRow == null)
		{
			_salesConsultant.Code = employee.EmployeeCode;
		}
		else
		{
			_isPartBonusCardValid = (!contractRow.BonusCardValidPeriodStart.HasValue || (contractRow.BonusCardValidPeriodStart.HasValue && contractRow.BonusCardValidPeriodStart <= _date.EditValue)) && (!contractRow.BonusCardValidPeriodEnd.HasValue || (contractRow.BonusCardValidPeriodEnd.HasValue && contractRow.BonusCardValidPeriodEnd >= _date.EditValue));
			if (Settings.ParametersService.ConnectTob2barmsoftDiscountServer())
			{
				_isPartBonusCardValid = _isPartBonusCardValid && contractRow.BonusCardValidPeriodStart.HasValue;
			}
			if (!PO.StoredInDb && PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value)
			{
				if (contractRow.BonusCardCanceled)
				{
					if (_allowUseBonus && contractRow.IsBonusCard)
					{
						throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsCanceled);
					}
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsCanceled);
				}
				if (!_isPartBonusCardValid)
				{
					_partner.Code = "";
					if (_allowUseBonus && contractRow.IsBonusCard && Settings.ParametersService.ConnectTob2barmsoftDiscountServer())
					{
						throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsNotValidForBSUser);
					}
					if (_allowUseBonus && contractRow.IsBonusCard)
					{
						throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsNotValid);
					}
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsNotValid);
				}
			}
			_contract = contractRow.Contract;
			if (_paramUseDiscount)
			{
				_discountPercent.EditValue = contractRow.Percent;
			}
			_partReadMagneticCard = true;
			_partner.Code = partner.PartCode;
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
	}

	private void ValidateBonusCard(PartnerCacheRow partnerCacheRow, PartnerCacheRow.ContractRow contractRow)
	{
		if (TradeSettings.CreateTradeClient().CommonServices.BonusCardValidation(partnerCacheRow.PartCode, contractRow.Contract, _date.EditValue, Settings.ParametersService.ConnectTob2barmsoftDiscountServer()))
		{
			return;
		}
		if (contractRow.BonusCardCanceled)
		{
			_partner.Code = "";
			if (_allowUseBonus && contractRow.IsBonusCard)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsCanceled);
			}
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsCanceled);
		}
		_partner.Code = "";
		if (_allowUseBonus && contractRow.IsBonusCard && Settings.ParametersService.ConnectTob2barmsoftDiscountServer())
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsNotValidForBSUser);
		}
		if (_allowUseBonus && contractRow.IsBonusCard)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsNotValid);
		}
		throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsNotValid);
	}

	private bool PriceListTypeEditValueChanged(string priceListType)
	{
		bool flag = false;
		decimal currRate = 1m;
		decimal currBase = 1m;
		if (!string.IsNullOrWhiteSpace(priceListType))
		{
			if (_summa.Value != 0m && !IsLoadingDataToControls)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (!_date.DoValidate() || !_date.EditValue.HasValue)
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			PriceHelper.GetPriceListRowVATType(priceListType, out var priceListRowVATType, out var priceListRowCurr);
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				PO.ExchangeRate.Rate = exchangeRates.Rate;
				PO.ExchangeRate.Base = exchangeRates.Base;
			}
			string round = TradeSettings.TradeCacheService.PriceType.GetItem(priceListType).Round;
			UpdateMTInvoicePrices(_date.EditValue.Value, priceListRowCurr, round, currRate, currBase);
			if (priceListRowVATType != Methods.VATAndRetailVATMapping(_vatType, oppositeMapping: true))
			{
				VATTypeValidate(priceListRowVATType);
			}
			if (!_retail && string.IsNullOrWhiteSpace(DiscountPriceListType))
			{
				CalculateDiscount(_mtBillList, _discountPercent.Value, gDiscount: false);
			}
			if (!IsLoadingDataToControls)
			{
				CalcSummAndSummVAT();
			}
		}
		return flag;
	}

	private void UpdateMTInvoicePrices(DateOnly docDate, string priceTypeCurr, string priceTypeRoundDegree, decimal currRate, decimal currBase)
	{
		if (_mtBillList.Count == 0)
		{
			return;
		}
		decimal num = 1m;
		decimal num2 = 1m;
		if (priceTypeCurr != _currCode.Code && priceTypeCurr != "AMD")
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(priceTypeCurr, docDate);
			num = exchangeRates.Rate;
			num2 = exchangeRates.Base;
		}
		foreach (MTBillSpecificationRow mtBill in _mtBillList)
		{
			PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_priceListType, mtBill.ItemID, mtBill.UnitCode, docDate);
			if (priceTypeCurr == _currCode.Code)
			{
				if (_retail)
				{
					if (string.IsNullOrWhiteSpace(DiscountPriceListType))
					{
						mtBill.DiscountPrice = price.Price;
					}
				}
				else
				{
					mtBill.Price = price.Price;
				}
			}
			else if (priceTypeCurr == "AMD")
			{
				if (_retail)
				{
					if (string.IsNullOrWhiteSpace(DiscountPriceListType))
					{
						mtBill.DiscountPrice = (price.Price / currRate * currBase).RoundAs(2);
					}
				}
				else
				{
					mtBill.Price = (price.Price / currRate * currBase).RoundAs(2);
				}
			}
			else if (_retail)
			{
				if (string.IsNullOrWhiteSpace(DiscountPriceListType))
				{
					mtBill.DiscountPrice = (price.Price * num / num2 / currRate * currBase).RoundAs(2);
				}
			}
			else
			{
				mtBill.Price = (price.Price * num / num2 / currRate * currBase).RoundAs(2);
			}
			if (_retail)
			{
				if (_paramUseDiscount)
				{
					mtBill.DiscountPrice = Methods.RoundPrice(mtBill.DiscountPrice, priceTypeRoundDegree);
					if (mtBill.Price == 0m && string.IsNullOrWhiteSpace(DiscountPriceListType))
					{
						MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtBill.ItemCode);
						mtBill.Discount = _discountPercent.Value;
						if (mtBill.Discount == 0m)
						{
							mtBill.Discount = item.Discount;
						}
						else if (item.Discount != 0m)
						{
							mtBill.Discount = item.Discount;
						}
						if (mtBill.Discount != 0m)
						{
							mtBill.Price = (mtBill.DiscountPrice * 100m / (100m - mtBill.Discount)).RoundAs(2);
						}
						else
						{
							mtBill.Price = mtBill.DiscountPrice;
						}
					}
					else
					{
						mtBill.Discount = (100m * (mtBill.Price - mtBill.DiscountPrice) / mtBill.Price).RoundAs(2);
					}
				}
				mtBill.Summa = (mtBill.Quantity * mtBill.DiscountPrice).RoundAs(2);
				mtBill.DiscountSumma = ((mtBill.Quantity * mtBill.Price).RoundAs(2) - mtBill.Summa).RoundAs(2);
			}
			else
			{
				mtBill.Price = Methods.RoundPrice(mtBill.Price, priceTypeRoundDegree);
				mtBill.Summa = (mtBill.Price * mtBill.Quantity).RoundAs(2);
			}
		}
	}

	private void VATTypeValidate(string priceListRowVATType)
	{
		decimal vATSumm = 0m;
		decimal num = 0m;
		if (_mtBillList.Count <= 0)
		{
			return;
		}
		if (!(priceListRowVATType == "1"))
		{
			if (priceListRowVATType == "2")
			{
				foreach (MTBillSpecificationRow mtBill in _mtBillList)
				{
					num += mtBill.Summa;
					if (!_retail)
					{
						if (_vatType.InList("1", "3", "4"))
						{
							mtBill.Price -= PriceHelper.CalcVATAmountByType(mtBill.Price, "5", mtBill.VAT.Value);
							mtBill.Price = Methods.RoundPrice(mtBill.Price, _priceListTypeRoundDgr);
						}
						mtBill.Summa = (mtBill.Quantity * mtBill.Price).RoundAs(2);
					}
				}
				_vATSumm = 0m;
				_summa.EditValue = num;
			}
		}
		else
		{
			foreach (MTBillSpecificationRow mtBill2 in _mtBillList)
			{
				vATSumm += PriceHelper.CalcVATAmountByType(mtBill2.Summa, "1", mtBill2.VAT.Value);
				if (!_retail)
				{
					if ((_vatType == "2") | (_vatType == "5"))
					{
						mtBill2.Price += PriceHelper.CalcVATAmountByType(mtBill2.Price, "1", mtBill2.VAT.Value);
						mtBill2.Price = Methods.RoundPrice(mtBill2.Price, _priceListTypeRoundDgr);
					}
					mtBill2.Summa = (mtBill2.Quantity * mtBill2.Price).RoundAs(2);
				}
				num += mtBill2.Summa;
			}
			_vATSumm = vATSumm;
			_summa.EditValue = num;
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
	}

	private void SetCashierShiftNumber()
	{
		if (!string.IsNullOrWhiteSpace(_cashDesk.Code))
		{
			_cashierShift.Text = TradeSettings.CreateTradeClient().CommonServices.GetOpenCashierShiftNumberByCashDesk(_cashDesk.Code);
		}
		else
		{
			_cashierShift.Text = "";
		}
	}

	private void ValidateDocFields()
	{
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		if (string.IsNullOrWhiteSpace(_storage.Code) && !_isShowStorageColInMTBill)
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

	private void MTBillGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		if (!_date.DoValidate() || _date == null)
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		_mtBillList.Add(new MTBillSpecificationRow
		{
			ItemType = GoodsTypesSE.Good.Value,
			Storage = _storage.Code,
			Discount = _discountPercent.Value,
			IsPartySelected = false,
			RowID = _maxRowID
		});
		_maxRowID++;
		e.RowNumber = _mtBillList.Count - 1;
	}

	private void UnitChanged(string unitCodeNew, string unitCodeOld, ref MTBillSpecificationRow mtBillGridRow, bool distributeparties = true)
	{
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(unitCodeNew);
		if (item == null)
		{
			mtBillGridRow.UnitBrief = "";
			mtBillGridRow.BarCode = "";
			return;
		}
		mtBillGridRow.UnitBrief = item.Brief;
		MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(mtBillGridRow.ItemCode, throwExceptionNotExits: true);
		MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtBillGridRow.ItemID, item.Code);
		MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(mtBillGridRow.ItemID, item.Code);
		mtBillGridRow.BarCode = ((barCode == null) ? "" : barCode.BarCode);
		if (!_retail || item2.Type == GoodsTypesSE.Service.Value)
		{
			if (!string.IsNullOrWhiteSpace(_priceListType))
			{
				decimal num = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, mtBillGridRow.ItemID, item.Code, _date.EditValue.Value, _currCode.Code);
				if (num == 0m)
				{
					num = PriceHelper.GetPriceForUnit(unitCodeOld, unitCodeNew, mtBillGridRow.Price, mtBillGridRow.ItemCode);
				}
				mtBillGridRow.Price = Methods.RoundPrice(num, _priceListTypeRoundDgr);
			}
			else
			{
				MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtBillGridRow.ItemID, unitCodeOld);
				if (barCode != null && qntUnit2 != null)
				{
					mtBillGridRow.Price = (mtBillGridRow.Price / qntUnit2.Coef * qntUnit.Coef).RoundAs(2);
					mtBillGridRow.Price = Methods.RoundPrice(mtBillGridRow.Price, _priceListTypeRoundDgr);
				}
			}
			if (!_paramUseDiscount || !_alwaysCalcDiscountAuto)
			{
				CalculateGridRowDiscountPrice(ref mtBillGridRow);
				CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, mtBillGridRow.DiscountPrice == 0m);
			}
		}
		else
		{
			mtBillGridRow.Price = PriceHelper.GetPriceForUnit(unitCodeOld, unitCodeNew, mtBillGridRow.Price, mtBillGridRow.ItemCode);
			if (!string.IsNullOrWhiteSpace(_priceListType))
			{
				if (_paramUseDiscount)
				{
					if (!_alwaysCalcDiscountAuto)
					{
						if (string.IsNullOrWhiteSpace(DiscountPriceListType))
						{
							mtBillGridRow.DiscountPrice = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, mtBillGridRow.ItemID, mtBillGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
							mtBillGridRow.DiscountPrice = Methods.RoundPrice(mtBillGridRow.DiscountPrice, _priceListTypeRoundDgr);
							if (mtBillGridRow.Price == 0m)
							{
								mtBillGridRow.Discount = _discountPercent.Value;
								if (mtBillGridRow.Discount == 0m)
								{
									mtBillGridRow.Discount = item2.Discount;
								}
								else if (item2.Discount != 0m)
								{
									mtBillGridRow.Discount = item2.Discount;
								}
								if (mtBillGridRow.Discount != 0m)
								{
									mtBillGridRow.Price = (mtBillGridRow.DiscountPrice * 100m / (100m - mtBillGridRow.Discount)).RoundAs(2);
								}
								else
								{
									mtBillGridRow.Price = mtBillGridRow.DiscountPrice;
								}
							}
							else
							{
								mtBillGridRow.Discount = (100m * (mtBillGridRow.Price - mtBillGridRow.DiscountPrice) / mtBillGridRow.Price).RoundAs(2);
							}
							CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, calcDiscountPrice: false);
						}
						else
						{
							CalculateGridRowDiscountPrice(ref mtBillGridRow);
							CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, mtBillGridRow.DiscountPrice == 0m);
						}
					}
				}
				else
				{
					CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, calcDiscountPrice: false);
				}
			}
			else if (!_paramUseDiscount || !_alwaysCalcDiscountAuto)
			{
				CalculateGridRowDiscountPrice(ref mtBillGridRow);
				CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, mtBillGridRow.DiscountPrice == 0m);
			}
		}
		if ((_distributByFEFO & distributeparties) && mtBillGridRow.ItemType == ItemTypesSE.Goods.Value)
		{
			int rowId = mtBillGridRow.RowID;
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == rowId);
			DistributeQtyByExpiredDate(mtBillGridRow.RowID, mtBillGridRow.Storage, mtBillGridRow.ItemCode, mtBillGridRow.UnitCode, mtBillGridRow.Quantity);
			mtBillGridRow.IsPartySelected = true;
		}
	}

	private void MTBillGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTBillSpecificationRow mTBillSpecificationRow = (MTBillSpecificationRow)_mtBillGrid.Grid.GetRow(e.RowHandle);
		if (mTBillSpecificationRow != null)
		{
			MTBillSpecificationRow mTBillSpecificationRow2 = new MTBillSpecificationRow();
			mTBillSpecificationRow.Clone(mTBillSpecificationRow2);
			mTBillSpecificationRow2.UpdateRemainsOnInfoPanel = true;
			mTBillSpecificationRow2.AccumulatedBonus = 0m;
			mTBillSpecificationRow2.UsedBonus = 0m;
			mTBillSpecificationRow2.IsPartySelected = ((mTBillSpecificationRow2.ItemType == GoodsTypesSE.Good.Value) ? new bool?(false) : ((bool?)null));
			mTBillSpecificationRow2.RowID = _maxRowID;
			_mtBillList.Add(mTBillSpecificationRow2);
			if (_distributByFEFO && mTBillSpecificationRow2.ItemType == ItemTypesSE.Goods.Value)
			{
				DistributeQtyByExpiredDate(mTBillSpecificationRow2.RowID, _storage.Code, mTBillSpecificationRow2.ItemCode, mTBillSpecificationRow2.UnitCode, mTBillSpecificationRow2.Quantity);
				mTBillSpecificationRow2.IsPartySelected = true;
			}
			_maxRowID++;
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			e.NewRowNumber = _mtBillList.Count - 1;
			CalculateDocumentSumma();
			ChangeRowInfo(mTBillSpecificationRow2);
		}
	}

	private void MTBillGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtBillGrid.Grid.SortInfo.Count > 0 || _mtBillTableView.Grid.VisibleRowCount != _mtBillList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTBillSpecificationRow mTBillSpecificationRow = (MTBillSpecificationRow)_mtBillGrid.Grid.GetRow(e.RowHandle);
		if (mTBillSpecificationRow != null)
		{
			_mtBillList.RemoveAt(e.RowHandle);
			_mtBillList.Insert(e.RowHandle - 1, mTBillSpecificationRow);
			On_DiscountClick();
		}
	}

	private void MTBillGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtBillGrid.Grid.SortInfo.Count > 0 || _mtBillTableView.Grid.VisibleRowCount != _mtBillList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTBillSpecificationRow mTBillSpecificationRow = (MTBillSpecificationRow)_mtBillGrid.Grid.GetRow(e.RowHandle);
		if (mTBillSpecificationRow != null)
		{
			_mtBillList.RemoveAt(e.RowHandle);
			_mtBillList.Insert(e.RowHandle + 1, mTBillSpecificationRow);
			On_DiscountClick();
		}
	}

	private void MTBillGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		MTBillSpecificationRow MTBillGridRow = (MTBillSpecificationRow)_mtBillGrid.Grid.GetRow(e.RowHandle);
		if (MTBillGridRow.IsGift)
		{
			e.Cancel = true;
		}
		if (_isAllowChangeGoodRowInBill)
		{
			if (MTBillGridRow != null)
			{
				PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == MTBillGridRow.RowID);
				RowInfoChange?.Invoke(null);
			}
			if (!_allowUseBonus)
			{
				return;
			}
			foreach (MTBillSpecificationRow mtBill in _mtBillList)
			{
				mtBill.UsedBonus = 0m;
			}
			_summaPayByBonus.EditValue = 0m;
			((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
			UpdateChangeOrPaySum();
			return;
		}
		e.Cancel = true;
		if (MTBillGridRow != null)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == MTBillGridRow.RowID);
			MTBillGridRow.IsDeleted = !MTBillGridRow.IsDeleted;
			MTBillGridRow.AccumulatedBonus = 0m;
			MTBillGridRow.UsedBonus = 0m;
			RowInfoChange?.Invoke(null);
		}
		if (_allowUseBonus)
		{
			foreach (MTBillSpecificationRow mtBill2 in _mtBillList)
			{
				mtBill2.UsedBonus = 0m;
			}
			_summaPayByBonus.EditValue = 0m;
			((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
			UpdateChangeOrPaySum();
		}
		CalculateDocumentSumma();
	}

	private void MTBillGrid_DeletedRow(object sender, EventArgs e)
	{
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		CalculateDocumentSumma();
	}

	private void MTBillGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateDocFields();
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
			_mtBillGrid.Grid.RefreshData();
			CalculateDocumentSumma();
			RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTBillSpecificationRow> mtBillList = _mtBillList;
				rowInfoChangeEventHandler(mtBillList[mtBillList.Count - 1]);
			}
		}
	}

	private void MTBillGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTBillSpecificationRow mTBillSpecificationRow = (MTBillSpecificationRow)_mtBillGrid.Grid.GetRow(e.RowHandle);
		if (mTBillSpecificationRow == null)
		{
			return;
		}
		ValidateDocFields();
		GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(DialogWindowMode.NewModeWithNext, this, mTBillSpecificationRow);
		goodsSelectPartyWindow.ShowDialog();
		int addedRows = goodsSelectPartyWindow.AddedRows;
		if (addedRows != 0)
		{
			_maxRowID += addedRows;
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			_mtBillGrid.Grid.RefreshData();
			CalculateDocumentSumma();
			RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTBillSpecificationRow> mtBillList = _mtBillList;
				rowInfoChangeEventHandler(mtBillList[mtBillList.Count - 1]);
			}
		}
	}

	private void MTBillGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		MTBillSpecificationRow mTBillSpecificationRow = (MTBillSpecificationRow)_mtBillGrid.Grid.GetRow(e.RowHandle);
		if (mTBillSpecificationRow != null)
		{
			ValidateDocFields();
			bool editMode = !mTBillSpecificationRow.IsDeleted && !mTBillSpecificationRow.IsGift && e.AllowEdit;
			e.RowEdited = EditRowWithDialog(editMode, e.AllowEdit, mTBillSpecificationRow);
		}
	}

	private bool EditRowWithDialog(bool editMode, bool allowEdit, MTBillSpecificationRow sourceRow)
	{
		bool flag = Convert.ToBoolean(new GoodsSelectPartyWindow(editMode ? DialogWindowMode.EditMode : DialogWindowMode.ViewMode, this, sourceRow).ShowDialog());
		if (_distributByFEFO && sourceRow.IsPartySelected.HasValue && sourceRow.IsPartySelected.Value && _distributByFEFO)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == sourceRow.RowID);
			DistributeQtyByExpiredDate(sourceRow.RowID, sourceRow.Storage, sourceRow.ItemCode, sourceRow.UnitCode, sourceRow.Quantity);
			sourceRow.IsPartySelected = true;
		}
		if (allowEdit & flag)
		{
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			_mtBillGrid.Grid.RefreshData();
			CalculateDocumentSumma();
			RowInfoChange?.Invoke(sourceRow);
		}
		return flag;
	}

	private void DistributeQtyByExpiredDate(int rowID, string storage, string goodCode, string unitCode, decimal quantity, string baseUnit = "")
	{
		try
		{
			ValidateDocFields();
			if (_isShowStorageColInMTBill && string.IsNullOrWhiteSpace(storage))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyStorage);
			}
			decimal num = quantity;
			List<PartyRow> list = new List<PartyRow>();
			List<PartyRow> list2 = new List<PartyRow>();
			List<PartyRow> list3 = new List<PartyRow>();
			bool flag = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(storage);
			if (string.IsNullOrWhiteSpace(goodCode))
			{
				return;
			}
			GoodsEngineServicesRoutes.PartyRowAndExpMethod parties = TradeSettings.CreateTradeClient().GoodsEngineServices.GetParties(storage, goodCode, _date.EditValue.Value);
			baseUnit = TradeSettings.TradeCacheService.Material.GetItem(goodCode).Unit;
			quantity = (quantity * PriceHelper.GetMTQntUnitCoeff(goodCode, unitCode)).RoundAs(3);
			num = quantity;
			unitCode = baseUnit;
			foreach (PartyRow partyRow in parties.partyRows)
			{
				partyRow.ExpQty = 0m;
				partyRow.GridRow = rowID;
				if (!partyRow.UseableDate.HasValue)
				{
					list2.Add(partyRow);
				}
				else
				{
					list3.Add(partyRow);
				}
			}
			list.AddRange(from ListRow in list3
				orderby ListRow.UseableDate, ListRow.IncomeDate, ListRow.OrderNum, ListRow.Row
				select ListRow);
			if (list != null)
			{
				foreach (PartyRow item in list)
				{
					if (!flag)
					{
						if (item.Qty > 0m)
						{
							if (num >= item.Qty)
							{
								item.ExpQty = item.Qty;
								num -= item.Qty;
							}
							else
							{
								item.ExpQty = num;
								num = 0m;
							}
						}
					}
					else if (item.Qty > 0m)
					{
						if (num >= item.Qty)
						{
							item.ExpQty = item.Qty;
							num -= item.Qty;
						}
						else
						{
							item.ExpQty = num;
							num = 0m;
						}
					}
				}
				if (((num > 0m) & flag) && parties.partyRows.Count >= 1)
				{
					List<PartyRow> partyRows = parties.partyRows;
					partyRows[partyRows.Count - 1].ExpQty += num;
				}
			}
			if (num > 0m && list2 != null)
			{
				foreach (PartyRow item2 in list2)
				{
					if (!flag)
					{
						if (item2.Qty > 0m)
						{
							if (num >= item2.Qty)
							{
								item2.ExpQty = item2.Qty;
								num -= item2.Qty;
							}
							else
							{
								item2.ExpQty = num;
								num = 0m;
							}
						}
					}
					else if (num >= item2.Qty)
					{
						List<PartyRow> partyRows2 = parties.partyRows;
						if (item2 == partyRows2[partyRows2.Count - 1])
						{
							item2.ExpQty = num;
							num = 0m;
						}
						else
						{
							item2.ExpQty = item2.Qty;
							num -= item2.Qty;
						}
					}
					else
					{
						item2.ExpQty = num;
						num = 0m;
					}
				}
			}
			List<PartyRow> list4 = list2;
			int num2 = list.Count + list4.Count;
			List<PartyRow> list5 = new List<PartyRow>(num2);
			CollectionsMarshal.SetCount(list5, num2);
			Span<PartyRow> span = CollectionsMarshal.AsSpan(list5);
			int num3 = 0;
			Span<PartyRow> span2 = CollectionsMarshal.AsSpan(list);
			span2.CopyTo(span.Slice(num3, span2.Length));
			num3 += span2.Length;
			Span<PartyRow> span3 = CollectionsMarshal.AsSpan(list4);
			span3.CopyTo(span.Slice(num3, span3.Length));
			num3 += span3.Length;
			parties.partyRows = list5;
			List<PartyRow> list6 = new List<PartyRow>();
			if (parties.partyRows.Count > 1)
			{
				if (parties.expMethod == "2")
				{
					list6.AddRange(from ListRow in parties.partyRows
						orderby ListRow.IncomeDate descending, ListRow.OrderNum, ListRow.Row
						select ListRow);
				}
				else
				{
					list6.AddRange(from ListRow in parties.partyRows
						orderby ListRow.IncomeDate, ListRow.OrderNum, ListRow.Row
						select ListRow);
				}
			}
			else
			{
				list6 = parties.partyRows;
			}
			foreach (PartyRow partyRow2 in parties.partyRows)
			{
				PO.MTExpDocSelectedPartiesList.Add(partyRow2.Clone());
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTBillTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _mtBillTableView.FocusedRowHandle;
		MTBillSpecificationRow mTBillSpecificationRow = (MTBillSpecificationRow)_mtBillGrid.Grid.GetRow(focusedRowHandle);
		if (mTBillSpecificationRow != null)
		{
			ChangeColumnAdditonalView(mTBillSpecificationRow.ItemCode);
			RowInfoChange?.Invoke(mTBillSpecificationRow);
		}
	}

	private void ChangeColumnAdditonalView(string itemCode)
	{
		_unitCodeCol.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(itemCode);
	}

	private void OnPlus()
	{
		IncreaseDecreaseRowQty();
	}

	private void OnMinus()
	{
		IncreaseDecreaseRowQty(increase: false);
	}

	protected override void OnF2()
	{
		HotKeyRedirector("F2");
	}

	protected override void OnF3()
	{
		HotKeyRedirector("F3");
	}

	protected override void OnF4()
	{
		HotKeyRedirector("F4");
	}

	protected override void OnF6()
	{
		HotKeyRedirector("F6");
	}

	protected override void OnF7()
	{
		HotKeyRedirector("F7");
	}

	protected override void OnF8()
	{
		HotKeyRedirector("F8");
	}

	protected override void OnF9()
	{
		HotKeyRedirector("F9");
	}

	protected override void OnF11()
	{
		HotKeyRedirector("F11");
	}

	protected override void OnShiftF8()
	{
		HotKeyRedirector("Shift+F8");
	}

	protected override void OnShiftF9()
	{
		HotKeyRedirector("Shift+F9");
	}

	protected override void OnCtrlF9()
	{
		HotKeyRedirector("Ctrl+F9");
	}

	protected override void OnCtrlF11()
	{
		HotKeyRedirector("Ctrl+F11");
	}

	protected override void OnCtrl1()
	{
		HotKeyRedirector("Ctrl+1");
	}

	private void DocumentWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Invalid comparison between Unknown and I4
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Invalid comparison between Unknown and I4
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Invalid comparison between Unknown and I4
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Invalid comparison between Unknown and I4
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Invalid comparison between Unknown and I4
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Invalid comparison between Unknown and I4
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Invalid comparison between Unknown and I4
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Invalid comparison between Unknown and I4
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Invalid comparison between Unknown and I4
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Invalid comparison between Unknown and I4
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Invalid comparison between Unknown and I4
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Invalid comparison between Unknown and I4
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Invalid comparison between Unknown and I4
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Invalid comparison between Unknown and I4
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Invalid comparison between Unknown and I4
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Invalid comparison between Unknown and I4
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Invalid comparison between Unknown and I4
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Invalid comparison between Unknown and I4
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Invalid comparison between Unknown and I4
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Invalid comparison between Unknown and I4
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Invalid comparison between Unknown and I4
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Invalid comparison between Unknown and I4
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Invalid comparison between Unknown and I4
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Invalid comparison between Unknown and I4
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Invalid comparison between Unknown and I4
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Invalid comparison between Unknown and I4
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Invalid comparison between Unknown and I4
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Invalid comparison between Unknown and I4
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Invalid comparison between Unknown and I4
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Invalid comparison between Unknown and I4
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Invalid comparison between Unknown and I4
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Invalid comparison between Unknown and I4
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Invalid comparison between Unknown and I4
		if (!_comment.IsKeyboardFocusWithin && !_docNum.IsKeyboardFocusWithin && !_discountPercent.IsKeyboardFocusWithin)
		{
			if ((int)e.Key == 85 && (int)Keyboard.Modifiers == 0)
			{
				OnPlus();
				e.Handled = true;
			}
			else if ((int)e.Key == 87 && (int)Keyboard.Modifiers == 0)
			{
				OnMinus();
				e.Handled = true;
			}
			else if ((int)e.Key == 91 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+F2");
				e.Handled = true;
			}
			else if ((int)e.Key == 92 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+F3");
				e.Handled = true;
			}
			else if ((int)e.Key == 94 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+F5");
				e.Handled = true;
			}
			else if ((int)e.Key == 36 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+2");
				e.Handled = true;
			}
			else if ((int)e.Key == 37 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+3");
				e.Handled = true;
			}
			else if ((int)e.Key == 38 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+4");
				e.Handled = true;
			}
			else if ((int)e.Key == 39 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+5");
				e.Handled = true;
			}
			else if ((int)e.Key == 40 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+6");
				e.Handled = true;
			}
			else if ((int)e.Key == 41 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+7");
				e.Handled = true;
			}
			else if ((int)e.Key == 42 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+8");
				e.Handled = true;
			}
			else if ((int)e.Key == 43 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+9");
				e.Handled = true;
			}
			else if ((int)e.Key == 91 && (int)Keyboard.Modifiers == 4)
			{
				HotKeyRedirector("Shift+F2");
				e.Handled = true;
			}
			else if ((int)e.Key == 92 && (int)Keyboard.Modifiers == 4)
			{
				HotKeyRedirector("Shift+F3");
				e.Handled = true;
			}
			else if ((int)e.Key == 94 && (int)Keyboard.Modifiers == 4)
			{
				HotKeyRedirector("Shift+F5");
				e.Handled = true;
			}
			else if ((int)e.Key == 95 && (int)Keyboard.Modifiers == 4)
			{
				HotKeyRedirector("Shift+F6");
				e.Handled = true;
			}
			else if ((int)e.Key == 96 && (int)Keyboard.Modifiers == 4)
			{
				HotKeyRedirector("Shift+F7");
				e.Handled = true;
			}
			else if ((int)e.Key == 100 && (int)Keyboard.Modifiers == 4)
			{
				HotKeyRedirector("Shift+F11");
				e.Handled = true;
			}
			else if ((int)e.Key == 101 && (int)Keyboard.Modifiers == 4)
			{
				HotKeyRedirector("Shift+F12");
				e.Handled = true;
			}
		}
	}

	private void HotKeyRedirector(string hotKey)
	{
		if (hotKey == Settings.ParametersService.MTBillInputValueHotKey())
		{
			if (!_inputValue.IsReadOnly)
			{
				ControlsHelper.SetFocus(_inputValue);
			}
		}
		else if (hotKey == Settings.ParametersService.MTBillSummaCashHotKey())
		{
			if (_anyAutoDiscountModeIsEnabled)
			{
				On_SummaCashClick();
			}
			else if (!_summaCash.IsReadOnly)
			{
				ControlsHelper.SetFocus(_summaCash);
			}
		}
		else if (hotKey == Settings.ParametersService.MTBillSetFocusOnGridHotKey())
		{
			ControlsHelper.SetFocus(_mtBillGrid.Grid);
		}
		else if (hotKey == Settings.ParametersService.MTBillBarCodeEnterHotKey())
		{
			On_BarCodeEnterClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillCodeEnterHotKey())
		{
			On_CodeEnterClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillMTListLikeNameHotKey())
		{
			On_MTListLikeNameClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillSalePriceHotKey())
		{
			On_SalePriceClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillUsePrePaymentHotKey())
		{
			On_UsePrePaymentClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillPlCardPaymentHotKey())
		{
			On_PlCardPaymentClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillCreditHotKey())
		{
			On_CreditClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillUseBonusHotKey())
		{
			On_UseBonusClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillPrintECRCheckHotKey())
		{
			On_PrintECRCheckClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillPrintBillHotKey())
		{
			On_PrintMTBillClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillWeightHotKey())
		{
			On_WeightClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillComplectHotKey())
		{
			On_ComplectClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillDiscountHotKey())
		{
			On_DiscountClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillCalcBonusHotKey())
		{
			On_CalcBonusClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillPricesHotKey())
		{
			On_PricesClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillQtyEnterHotKey())
		{
			On_QtyEnterClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillFavoritesHotKey())
		{
			On_MTFavoritesClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillFilteredSearchHotKey())
		{
			On_FilteredSearchClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillErasePaymentsHotKey())
		{
			On_ErasePaymentsClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillAddToBonusHotKey())
		{
			On_AddBonusClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillSubstituteItemsHotKey())
		{
			On_SubstituteItemsClick();
		}
		else if (hotKey == Settings.ParametersService.UnitCodeSwitchHotKey())
		{
			OnSwitchUnit();
		}
		else if (hotKey == Settings.ParametersService.EMarkReceiptHotKey())
		{
			EMarkReceipt();
		}
	}

	private void Increase_Click(object sender, RoutedEventArgs e)
	{
		IncreaseDecreaseRowQty();
	}

	private void Decrease_Click(object sender, RoutedEventArgs e)
	{
		IncreaseDecreaseRowQty(increase: false);
	}

	private void Discount_Click(object sender, RoutedEventArgs e)
	{
		On_DiscountClick();
	}

	private void Weight_Click(object sender, RoutedEventArgs e)
	{
		On_WeightClick();
	}

	private void SalePrice_Click(object sender, RoutedEventArgs e)
	{
		On_SalePriceClick();
	}

	private void Complect_Click(object sender, RoutedEventArgs e)
	{
		On_ComplectClick();
	}

	private void BarCodeEnter_Click(object sender, RoutedEventArgs e)
	{
		On_BarCodeEnterClick();
	}

	private void CodeEnter_Click(object sender, RoutedEventArgs e)
	{
		On_CodeEnterClick();
	}

	private void MTListLikeName_Click(object sender, RoutedEventArgs e)
	{
		On_MTListLikeNameClick();
	}

	private void QtyEnter_Click(object sender, RoutedEventArgs e)
	{
		On_QtyEnterClick();
	}

	private void PrintECRCheck_Click(object sender, RoutedEventArgs e)
	{
		On_PrintECRCheckClick();
	}

	private void PrintBill_Click(object sender, RoutedEventArgs e)
	{
		On_PrintMTBillClick();
	}

	private void UseBonus_Click(object sender, RoutedEventArgs e)
	{
		On_UseBonusClick();
	}

	private void Credit_Click(object sender, RoutedEventArgs e)
	{
		On_CreditClick();
	}

	private void PlCardPayment_Click(object sender, RoutedEventArgs e)
	{
		On_PlCardPaymentClick();
	}

	private void CalcBonus_Click(object sender, RoutedEventArgs e)
	{
		On_CalcBonusClick();
	}

	private void AddBonus_Click(object sender, RoutedEventArgs e)
	{
		On_AddBonusClick();
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		On_FilteredSearchClick();
	}

	private void MTFavorites_Click(object sender, RoutedEventArgs e)
	{
		On_MTFavoritesClick();
	}

	private void Prices_Click(object sender, RoutedEventArgs e)
	{
		On_PricesClick();
	}

	private void UsePrePayment_Click(object sender, RoutedEventArgs e)
	{
		On_UsePrePaymentClick();
	}

	private void SummaCash_Click(object sender, RoutedEventArgs e)
	{
		On_SummaCashClick();
	}

	private void BE_ErasePayments_Click(object sender, RoutedEventArgs e)
	{
		On_ErasePaymentsClick();
	}

	private void SubstituteItems_Click(object sender, RoutedEventArgs e)
	{
		On_SubstituteItemsClick();
	}

	private void On_CodeEnterClick()
	{
		if (_codeEnter == null || !_codeEnter.IsEnabled)
		{
			return;
		}
		_inputValue.DoValidate();
		_mQty_Weight = 1m;
		bool flag = false;
		string text = _inputValue.Text.Trim();
		if (string.IsNullOrWhiteSpace(text))
		{
			if (!_date.DoValidate() || !_date.EditValue.HasValue)
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices goodsAvailabilityFilterForInvoices = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices(_date.EditValue.Value, _storage.Code, _priceListType, _retail);
			ModalBrowser modalBrowser = new ModalBrowser(goodsAvailabilityFilterForInvoices)
			{
				Owner = this,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};
			if (!modalBrowser.Show())
			{
				_inputValue.Text = "";
				_realText = "";
				ControlsHelper.SetFocus(_inputValue);
				return;
			}
			text = goodsAvailabilityFilterForInvoices.ValueMember(modalBrowser.SelectedRowIndex);
			flag = true;
		}
		if (!flag)
		{
			if (Settings.ParametersService.UsePartnerEmployeeSearchNewMode())
			{
				if (DocumentHelpers.IsValidPartnerEmployeeCardCode(text, Settings.ParametersService.PartnerEmployeeCardCodePrefixes(), Settings.ParametersService.UsePartnerCardLenght(), Settings.ParametersService.PartnerCardLenght()))
				{
					_inputValue.Text = "";
					_realText = "";
					_inputValue.Focus();
					if (!DocumentHelpers.GetPartnerEmployeeDataByBarCode(text, out var partnerCacheRow, out var partnerContract, out var employee, "AMD", hideExceptions: true))
					{
						AddNewBuyer(text);
					}
					else
					{
						FillPartnerEmployeeData(partnerCacheRow, partnerContract, employee);
					}
					return;
				}
			}
			else if (MCRInfo.GetInstance().IsValidCard(text))
			{
				_inputValue.Text = "";
				_realText = "";
				_inputValue.Focus();
				string cardNumber = MCRInfo.GetInstance().GetCardNumber(text);
				if (!DocumentHelpers.GetPartnerEmployeeDataByBarCode(cardNumber, out var partnerCacheRow2, out var partnerContract2, out var employee2, "AMD", hideExceptions: true))
				{
					AddNewBuyer(cardNumber);
				}
				else
				{
					FillPartnerEmployeeData(partnerCacheRow2, partnerContract2, employee2);
				}
				return;
			}
		}
		if (text.Length > 20)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrMTCodeLen, 20, text));
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
		if (item == null)
		{
			ControlsHelper.SetFocus(_inputValue);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text));
		}
		MTBillSpecificationRow mtBillGridRow = AddItemToList(item, text, barCode: false);
		if (mtBillGridRow.ItemType == GoodsTypesSE.Good.Value && mtBillGridRow != null && _distributByFEFO)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtBillGridRow.RowID);
			DistributeQtyByExpiredDate(mtBillGridRow.RowID, _storage.Code, item.MTCode, item.Unit, mtBillGridRow.Quantity, item.Unit);
			mtBillGridRow.IsPartySelected = true;
			if (_immediatlyEditRow)
			{
				EditRowWithDialog(editMode: true, allowEdit: true, mtBillGridRow);
			}
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		if (!_immediatlyOpenMarkingWindow || !MarkingHelpers.IsGoodMarkable(mtBillGridRow.ItemCode, _useMarking, mtBillGridRow.MarkingList.Count != 0, _date.EditValue.Value, out var isRowMarkable))
		{
			return;
		}
		mtBillGridRow.IsRowMarkable = isRowMarkable;
		List<string> allMarkings = _mtBillList.SelectMany((MTBillSpecificationRow row) => row.MarkingList).ToList();
		MarkingSelectWindow markingSelectWindow = new MarkingSelectWindow(this, _readOnlyAfterCheckPrint, mtBillGridRow.IsRowMarkable, mtBillGridRow.ItemCode, mtBillGridRow.ItemName, mtBillGridRow.MarkingList, allMarkings, mtBillGridRow.Quantity);
		if (markingSelectWindow.ShowDialog().Value && !markingSelectWindow.IsViewOnlyMode)
		{
			mtBillGridRow.MarkingList.Clear();
			foreach (MarkingSelectWindow.MarkingRow marking in markingSelectWindow.MarkingList)
			{
				mtBillGridRow.MarkingList.Add(marking.Marking);
			}
			if (mtBillGridRow.Quantity < (decimal)mtBillGridRow.MarkingList.Count)
			{
				MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(mtBillGridRow.ItemCode);
				if (item2 == null || !item2.IsWeight)
				{
					mtBillGridRow.Quantity = mtBillGridRow.MarkingList.Count;
					ChangeRowQty(mtBillGridRow);
				}
			}
			ChangeMarkingStatus(mtBillGridRow);
		}
		CalcSummAndSummVAT();
		UpdateChangeOrPaySum();
	}

	private void On_BarCodeEnterClick()
	{
		if (_barCodeEnter == null || !_barCodeEnter.IsEnabled)
		{
			return;
		}
		_inputValue.DoValidate();
		_mQty_Weight = 1m;
		MTBillSpecificationRow mtBillGridRow = new MTBillSpecificationRow();
		string barCode = _inputValue.Text.Trim();
		bool flag = false;
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		if (string.IsNullOrWhiteSpace(barCode))
		{
			ArmSoft.SME.Trade.UI.DropDown.MTBarCodes mTBarCodes = new ArmSoft.SME.Trade.UI.DropDown.MTBarCodes(isShowQntUnitCaption: false, showBarCodeFirst: true, Settings.ParametersService.ShowMTInclInPrList(), showAlsoMTQntUnitsEmptyBarCodes: true);
			ModalBrowser modalBrowser = new ModalBrowser(mTBarCodes)
			{
				Owner = this,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};
			if (!modalBrowser.Show())
			{
				_inputValue.Text = "";
				_realText = "";
				ControlsHelper.SetFocus(_inputValue);
				return;
			}
			barCode = mTBarCodes.ValueMember(modalBrowser.SelectedRowIndex);
			string text = mTBarCodes.CommentMember(modalBrowser.SelectedRowIndex);
			if (string.IsNullOrWhiteSpace(barCode))
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
				mtBillGridRow = AddItemToList(item, text, barCode: false, 0m, mTBarCodes.GetRowByIndex(modalBrowser.SelectedRowIndex).QntUnit);
				if (mtBillGridRow.ItemType == GoodsTypesSE.Good.Value && mtBillGridRow != null && _distributByFEFO)
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtBillGridRow.RowID);
					DistributeQtyByExpiredDate(mtBillGridRow.RowID, _storage.Code, mtBillGridRow.ItemCode, mtBillGridRow.UnitCode, mtBillGridRow.Quantity, item.Unit);
					mtBillGridRow.IsPartySelected = true;
				}
				if (_paramUseDiscount && _alwaysCalcDiscountAuto)
				{
					On_DiscountClick();
				}
				return;
			}
			flag = true;
		}
		if (!flag)
		{
			if (Settings.ParametersService.UsePartnerEmployeeSearchNewMode())
			{
				if (DocumentHelpers.IsValidPartnerEmployeeCardCode(barCode, Settings.ParametersService.PartnerEmployeeCardCodePrefixes(), Settings.ParametersService.UsePartnerCardLenght(), Settings.ParametersService.PartnerCardLenght()))
				{
					_inputValue.Text = "";
					_realText = "";
					_inputValue.Focus();
					if (!DocumentHelpers.GetPartnerEmployeeDataByBarCode(barCode, out var partnerCacheRow, out var partnerContract, out var employee, "AMD", hideExceptions: true))
					{
						AddNewBuyer(barCode);
					}
					else
					{
						FillPartnerEmployeeData(partnerCacheRow, partnerContract, employee);
					}
					return;
				}
			}
			else if (MCRInfo.GetInstance().IsValidCard(barCode))
			{
				_inputValue.Text = "";
				_realText = "";
				_inputValue.Focus();
				string cardNumber = MCRInfo.GetInstance().GetCardNumber(barCode);
				if (!DocumentHelpers.GetPartnerEmployeeDataByBarCode(cardNumber, out var partnerCacheRow2, out var partnerContract2, out var employee2, "AMD", hideExceptions: true))
				{
					AddNewBuyer(cardNumber);
				}
				else
				{
					FillPartnerEmployeeData(partnerCacheRow2, partnerContract2, employee2);
				}
				return;
			}
		}
		if (barCode.Length < 29)
		{
			if (_searchBarCodeInProperty1 && !flag)
			{
				BarCodeEnterClickOnProperty1(barCode);
			}
			else
			{
				MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out _mQty_Weight, out var _, out var _);
				if (materialCacheRow == null)
				{
					ControlsHelper.SetFocus(_inputValue);
					if (barCode.Length >= 2 && (barCode.Substring(0, 2) == "28" || barCode.Substring(0, 2) == "26"))
					{
						if (barCode.Length >= 13)
						{
							throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeNotExist, barCode.Substring(2, 5)));
						}
						throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.WrongMinLengthOfBarCode, (short)13, barCode));
					}
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeNotExist, barCode));
				}
				if (_useMarking && !materialCacheRow.IsWeight && materialCacheRow.CheckMarking)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ScanDataMatrix);
				}
				mtBillGridRow = AddItemToList(materialCacheRow, barCode);
				if (mtBillGridRow.ItemType == GoodsTypesSE.Good.Value && mtBillGridRow != null && _distributByFEFO)
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtBillGridRow.RowID);
					DistributeQtyByExpiredDate(mtBillGridRow.RowID, _storage.Code, materialCacheRow.MTCode, mtBillGridRow.UnitCode, mtBillGridRow.Quantity, materialCacheRow.Unit);
					mtBillGridRow.IsPartySelected = true;
					if (_immediatlyEditRow)
					{
						EditRowWithDialog(editMode: true, allowEdit: true, mtBillGridRow);
					}
				}
			}
			if (_useMarking && _immediatlyOpenMarkingWindow && MarkingHelpers.IsGoodMarkable(mtBillGridRow.ItemCode, _useMarking, mtBillGridRow.MarkingList.Any(), _date.EditValue.Value, out var isRowMarkable))
			{
				mtBillGridRow.IsRowMarkable = isRowMarkable;
				List<string> allMarkings = _mtBillList.SelectMany((MTBillSpecificationRow row) => row.MarkingList).ToList();
				MarkingSelectWindow markingSelectWindow = new MarkingSelectWindow(this, _readOnlyAfterCheckPrint, mtBillGridRow.IsRowMarkable, mtBillGridRow.ItemCode, mtBillGridRow.ItemName, mtBillGridRow.MarkingList, allMarkings, mtBillGridRow.Quantity);
				if (markingSelectWindow.ShowDialog().Value && !markingSelectWindow.IsViewOnlyMode)
				{
					mtBillGridRow.MarkingList.Clear();
					foreach (MarkingSelectWindow.MarkingRow marking in markingSelectWindow.MarkingList)
					{
						mtBillGridRow.MarkingList.Add(marking.Marking);
					}
					if (mtBillGridRow.Quantity < (decimal)mtBillGridRow.MarkingList.Count)
					{
						MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(mtBillGridRow.ItemCode);
						if (item2 == null || !item2.IsWeight)
						{
							mtBillGridRow.Quantity = mtBillGridRow.MarkingList.Count;
							ChangeRowQty(mtBillGridRow);
						}
					}
					CalcSummAndSummVAT();
					UpdateChangeOrPaySum();
				}
			}
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
		}
		else
		{
			if (!_useMarking)
			{
				_inputValue.Text = "";
				_realText = "";
				_inputValue.Focus();
				throw new ASException(ArmSoft.SME.Common.Resources.MarkingNotActive);
			}
			string eanBarCode = "";
			string text2 = _inputValue.Text;
			MaterialCacheRow goodFromScanedMark = GetGoodFromScanedMark(ref eanBarCode, out var skipMarking);
			if (skipMarking)
			{
				return;
			}
			if (goodFromScanedMark == null)
			{
				_inputValue.Text = "";
				_realText = "";
				ControlsHelper.SetFocus(_inputValue);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodesNotExist, eanBarCode));
			}
			if (!MarkingHelpers.IsGoodMarkable(goodFromScanedMark.MTCode, _useMarking, markingListHasValue: false, _date.EditValue.Value, out var isRowMarkable2))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.CPAEmptyOrNotMarkable);
			}
			mtBillGridRow.IsRowMarkable = isRowMarkable2;
			mtBillGridRow = AddItemToList(goodFromScanedMark, eanBarCode);
			if (mtBillGridRow.ItemType == GoodsTypesSE.Good.Value)
			{
				if (_distributByFEFO)
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtBillGridRow.RowID);
					DistributeQtyByExpiredDate(mtBillGridRow.RowID, _storage.Code, goodFromScanedMark.MTCode, mtBillGridRow.UnitCode, mtBillGridRow.Quantity, goodFromScanedMark.Unit);
					mtBillGridRow.IsPartySelected = true;
					if (_immediatlyEditRow)
					{
						EditRowWithDialog(editMode: true, allowEdit: true, mtBillGridRow);
					}
				}
				if (!string.IsNullOrWhiteSpace(eanBarCode) && _useMarking && MarkingHelpers.IsGoodMarkable(mtBillGridRow.ItemCode, _useMarking, mtBillGridRow.MarkingList.Count != 0, _date.EditValue.Value, out isRowMarkable2))
				{
					mtBillGridRow.IsRowMarkable = isRowMarkable2;
					mtBillGridRow.MarkingList.Add(text2);
				}
			}
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
		}
		ChangeMarkingStatus(mtBillGridRow);
		RowInfoChange?.Invoke(mtBillGridRow);
	}

	private MaterialCacheRow GetGoodFromScanedMark(ref string eanBarCode, out bool skipMarking)
	{
		skipMarking = false;
		if (_useMarking)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(_inputValue.Text);
			for (int i = 0; i <= bytes.Length - 1; i += 2)
			{
				if (((bytes[i + 1] << 8) | bytes[i]) > 127)
				{
					_inputValue.Text = "";
					_realText = "";
					ControlsHelper.SetFocus(_inputValue);
					throw new ASException(ArmSoft.SME.Common.Resources.KeyboardLanguageForMarkingScanShouldBeEnglish);
				}
			}
			MTBillSpecificationRow mtBillRow = _mtBillList.FirstOrDefault((MTBillSpecificationRow r) => r.MarkingList.Contains(_inputValue.Text));
			if (mtBillRow != null)
			{
				ScannerMessageBoxWindow scannerMessageBoxWindow = new ScannerMessageBoxWindow(ArmSoft.SME.Trade.Common.Resources.ConfirmDeleteDataMatrix, this);
				if (scannerMessageBoxWindow.ShowDialog().Value)
				{
					mtBillRow.MarkingList.Remove(_inputValue.Text);
					if (mtBillRow.Quantity >= 1m && !mtBillRow.IsDeleted && !mtBillRow.IsGift)
					{
						MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtBillRow.ItemCode);
						if ((item == null || !item.IsWeight) && _isAllowChangeGoodRowInBill)
						{
							mtBillRow.Quantity--;
							if (mtBillRow.Quantity == 0m)
							{
								PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtBillRow.RowID);
								_mtBillList.Remove(mtBillRow);
								mtBillRow = null;
							}
							else
							{
								ChangeRowQty(mtBillRow);
							}
							CalculateDocumentSumma();
						}
					}
					ChangeMarkingStatus(mtBillRow);
					RowInfoChange?.Invoke(mtBillRow);
				}
				_inputValue.Text = "";
				_realText = "";
				ControlsHelper.SetFocus(_inputValue);
				if (!string.IsNullOrWhiteSpace(scannerMessageBoxWindow.ScannerData))
				{
					_inputValue.Text = scannerMessageBoxWindow.ScannerData;
					On_BarCodeEnterClick();
				}
				skipMarking = true;
				return null;
			}
			MarkingHelper.ValidateMarkingCode(_inputValue.Text);
			eanBarCode = MarkingHelpers.ExtractGTINFromDataMatrix(_inputValue.Text, out var good);
			return good;
		}
		return null;
	}

	private void On_QtyEnterClick()
	{
		if (_qtyEnter == null || !_qtyEnter.IsEnabled)
		{
			return;
		}
		_inputValue.DoValidate();
		if (_mtBillTableView.Grid.CurrentItem == null || ((MTBillSpecificationRow)_mtBillTableView.Grid.CurrentItem).IsGift)
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(_inputValue.Text))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.InputBoxIsEmpty);
		}
		string text = _inputValue.Text.Trim();
		bool flag = text.StartsWith("/");
		if (flag)
		{
			text = text.Substring("/".Length);
		}
		if (!text.IsNumeric())
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
		}
		if (!decimal.TryParse(text, out var result))
		{
			return;
		}
		decimal num = result.RoundAs(3);
		if (num < 0m)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidateQuantIsPositive);
		}
		if (num.ToString("#").Length > 9)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.ErrNumberOfDigitsBeforeDecimalPart);
		}
		MTBillSpecificationRow focusedRow = (MTBillSpecificationRow)_mtBillTableView.Grid.CurrentItem;
		if (!focusedRow.IsGift && !_isAllowChangeGoodRowInBill && num < focusedRow.Quantity)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.InputQtyMustBeGreater);
		}
		focusedRow.Quantity = num;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(focusedRow.ItemCode);
		if (focusedRow.ItemType == GoodsTypesSE.Good.Value)
		{
			if (_distributByFEFO)
			{
				PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == focusedRow.ItemID);
				DistributeQtyByExpiredDate(focusedRow.RowID, focusedRow.Storage, focusedRow.ItemCode, focusedRow.UnitCode, focusedRow.Quantity, item.Unit);
				focusedRow.IsPartySelected = true;
			}
			else if (focusedRow.IsPartySelected == true)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ForRowSelectedParties, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == focusedRow.ItemID);
				focusedRow.IsPartySelected = false;
			}
		}
		if (item != null)
		{
			if (flag)
			{
				string text2 = "";
				if (focusedRow.UnitCode == item.AltUnit)
				{
					text2 = item.Unit;
				}
				else if (focusedRow.UnitCode == item.Unit)
				{
					text2 = item.AltUnit;
				}
				UnitChanged(text2, focusedRow.UnitCode, ref focusedRow);
				focusedRow.UnitCode = text2;
			}
			else
			{
				CalculateGridRowSummaAndDiscountSumma(ref focusedRow, calcDiscountPrice: false);
			}
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		CalculateDocumentSumma();
		ChangeMarkingStatus(focusedRow);
		RowInfoChange?.Invoke(focusedRow);
		_inputValue.Text = "";
		_realText = "";
		ControlsHelper.SetFocus(_inputValue);
	}

	private void On_MTListLikeNameClick()
	{
		if (_mtListLikeName == null || !_mtListLikeName.IsEnabled)
		{
			return;
		}
		_inputValue.DoValidate();
		_mQty_Weight = 1m;
		string likeName = _inputValue.Text.Trim();
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices goodsAvailabilityFilterForInvoices = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices(_date.EditValue.Value, _storage.Code, _priceListType, _retail, likeName);
		ModalBrowser modalBrowser = new ModalBrowser(goodsAvailabilityFilterForInvoices)
		{
			Owner = this,
			WindowStartupLocation = WindowStartupLocation.CenterOwner
		};
		if (modalBrowser.Show(upClick: true))
		{
			string text = goodsAvailabilityFilterForInvoices.ValueMember(modalBrowser.SelectedRowIndex);
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
			if (item == null)
			{
				ControlsHelper.SetFocus(_inputValue);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text));
			}
			MTBillSpecificationRow mtBillGridRow = AddItemToList(item, text, barCode: false);
			if (mtBillGridRow.ItemType == GoodsTypesSE.Good.Value && mtBillGridRow != null && _distributByFEFO)
			{
				PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtBillGridRow.RowID);
				DistributeQtyByExpiredDate(mtBillGridRow.RowID, _storage.Code, item.MTCode, item.Unit, mtBillGridRow.Quantity, item.Unit);
				mtBillGridRow.IsPartySelected = true;
				if (_immediatlyEditRow)
				{
					EditRowWithDialog(editMode: true, allowEdit: true, mtBillGridRow);
				}
			}
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
		}
		else
		{
			_inputValue.Text = "";
			_realText = "";
			ControlsHelper.SetFocus(_inputValue);
		}
	}

	private void On_CalcBonusClick()
	{
		if (_calcBonus == null || !_calcBonus.IsEnabled)
		{
			return;
		}
		if (!string.IsNullOrWhiteSpace(_partner.Code))
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partner.Code, _contract);
			if (contract != null)
			{
				_isPartContractWithBonus = contract.IsBonusCard;
				if (_bonusType == BonusTypeSE.ByPercent.Value)
				{
					_partContractBonusPercentOrPoint = contract.Bonus;
				}
				else
				{
					_partContractBonusPercentOrPoint = contract.Point;
				}
			}
		}
		if (_isPartContractWithBonus)
		{
			if (_noCalcBonusesWhenUseBonus && (decimal)_summaPayByBonus.EditValue != 0m)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCalcValidate);
			}
			if (CalcMTBillListRowCount(includeDeleted: false) == 0)
			{
				return;
			}
			if (!_useBonusExtension)
			{
				if (_bonusAccumulationBase == BonusAccumulationBaseSE.WithoutCondition.Value)
				{
					CalcBonuses();
				}
				else if (_bonusAccumulationBase == BonusAccumulationBaseSE.OneTimeSaleSumm.Value)
				{
					if (_currCode.Code == "AMD")
					{
						if (_summa.Value >= _bonusAccumulationBaseSumm)
						{
							CalcBonuses();
						}
						else
						{
							foreach (MTBillSpecificationRow mtBill in _mtBillList)
							{
								mtBill.AccumulatedBonus = 0m;
								mtBill.UsedBonus = 0m;
							}
						}
					}
					else
					{
						ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
						PO.ExchangeRate.Rate = exchangeRates.Rate;
						PO.ExchangeRate.Base = exchangeRates.Base;
						if ((_summa.Value * PO.ExchangeRate.Rate / PO.ExchangeRate.Base).RoundAs(2) >= _bonusAccumulationBaseSumm)
						{
							CalcBonuses();
						}
						else
						{
							foreach (MTBillSpecificationRow mtBill2 in _mtBillList)
							{
								mtBill2.AccumulatedBonus = 0m;
								mtBill2.UsedBonus = 0m;
							}
						}
					}
				}
				else
				{
					DateOnly startDate = _bonusAccumulationPeriodStart ?? ArmSoft.SME.Common.Constants.Common.MinOPDateOnly;
					DateOnly endDate = _bonusAccumulationPeriodEnd ?? _date.EditValue.Value;
					PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partCode);
					decimal num = 0m;
					if (item != null)
					{
						num = TradeSettings.CreateTradeClient().CommonServices.GetMTSaleSumByPartnerAndContract(CommonConst.SummaTypes.NCCSummWithVAT, startDate, endDate, item.Id, _contract, PO.ISN);
					}
					if (num >= _bonusAccumulationBaseSumm)
					{
						CalcBonuses(num);
					}
					else
					{
						foreach (MTBillSpecificationRow mtBill3 in _mtBillList)
						{
							mtBill3.AccumulatedBonus = 0m;
							mtBill3.UsedBonus = 0m;
						}
					}
				}
				MTBillSpecificationRow row = (MTBillSpecificationRow)_mtBillGrid.Grid.CurrentItem;
				RowInfoChange?.Invoke(row);
				SetBuyerInfoToPoleDisplay();
			}
			else
			{
				FillDocument(new StoreRequest());
			}
			return;
		}
		foreach (MTBillSpecificationRow mtBill4 in _mtBillList)
		{
			mtBill4.AccumulatedBonus = 0m;
			mtBill4.UsedBonus = 0m;
		}
		SetBuyerInfoToPoleDisplay();
		throw new ASException(ArmSoft.SME.Trade.Common.Resources.ContractForCalcBonus);
	}

	private void On_UseBonusClick()
	{
		if (_useBonus == null || !_useBonus.IsEnabled)
		{
			return;
		}
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partCode, _contract);
		if (contract != null)
		{
			if (_connectTob2barmsoftDiscountServer)
			{
				if (string.IsNullOrWhiteSpace(contract.DiscountCard))
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Trade.Common.Resources.DiscountCardCode));
				}
				if (DiscountServerService.GetCardInfo(contract.DiscountCard) == null)
				{
					return;
				}
			}
			_isPartContractWithBonus = contract.IsBonusCard;
			if (_bonusType == BonusTypeSE.ByPercent.Value)
			{
				_partContractBonusPercentOrPoint = contract.Bonus;
			}
			else
			{
				_partContractBonusPercentOrPoint = contract.Point;
			}
		}
		if (_isPartContractWithBonus)
		{
			BonusUse();
			return;
		}
		throw new ASException(ArmSoft.SME.Trade.Common.Resources.ContractForUseBonus);
	}

	private void On_AddBonusClick()
	{
		if (_useBonus == null || !_useBonus.IsEnabled)
		{
			return;
		}
		if (!string.IsNullOrWhiteSpace(_partner.Code))
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partner.Code, _contract);
			if (contract != null)
			{
				_isPartContractWithBonus = contract.IsBonusCard;
			}
		}
		if (!_isPartContractWithBonus)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ContractForCalcBonus);
		}
		bool flag = _changeOrPaySum >= 0m && _sumAddBonus == 0m;
		DialogWindow dialogWindow = new DialogWindow(this)
		{
			Caption = ArmSoft.SME.Trade.Common.Resources.AddToBonus
		};
		PopupCalcEditExt popupCalcEditExt = dialogWindow.AddNumericControl("BonusSum", ArmSoft.SME.Common.Resources.Summa, 16, 2);
		popupCalcEditExt.EditValue = (flag ? _summaChange.EditValue : ((object)_sumAddBonus));
		if (_readOnlyAfterCheckPrint)
		{
			dialogWindow.Mode = DialogWindowMode.ViewMode;
			popupCalcEditExt.IsReadOnly = true;
		}
		if (dialogWindow.ShowDialog().Value)
		{
			_sumAddBonus = popupCalcEditExt.Value;
			_addedBonus.EditValue = _sumAddBonus;
			UpdateChangeOrPaySum();
		}
	}

	private void On_ComplectClick()
	{
		if (_complect == null || !_complect.IsEnabled)
		{
			return;
		}
		FillComplectRowsWindow<MTBillSpecificationRow> fillComplectRowsWindow = new FillComplectRowsWindow<MTBillSpecificationRow>(this, PO, _date.EditValue.Value, _storage.Code, _paramUseDiscount, _anyAutoDiscountModeIsEnabled, _usedDiscount, _discountPercent.Value, _maxRowID, _mtBillList, _mtBillGrid, _priceListType, _vatType, _currCode.Code);
		fillComplectRowsWindow.ShowDialog();
		int addedRows = fillComplectRowsWindow.AddedRows;
		if (addedRows == 0)
		{
			return;
		}
		_maxRowID += addedRows;
		if (_distributByFEFO)
		{
			for (int i = 0; i <= addedRows - 1; i++)
			{
				MTBillSpecificationRow mTBillSpecificationRow = _mtBillList[_mtBillList.Count - 1 - i];
				DistributeQtyByExpiredDate(mTBillSpecificationRow.RowID, _storage.Code, mTBillSpecificationRow.ItemCode, mTBillSpecificationRow.UnitCode, mTBillSpecificationRow.Quantity);
				mTBillSpecificationRow.IsPartySelected = true;
			}
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		CalcSummAndSummVAT();
		UpdateChangeOrPaySum();
		foreach (MTBillSpecificationRow mtBill in _mtBillList)
		{
			ChangeMarkingStatus(mtBill);
		}
		RowInfoChange?.Invoke(_mtBillList[_mtBillList.Count - 1]);
	}

	private void On_DiscountClick()
	{
		if (_discount == null || !_discount.IsEnabled)
		{
			return;
		}
		if (CalcMTBillListRowCount(includeDeleted: false) != 0)
		{
			if (!_useDiscountExtension)
			{
				bool flag = true;
				if (!_anyAutoDiscountModeIsEnabled)
				{
					flag = ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ComfirmDiscountCalc, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes;
				}
				if (!flag)
				{
					return;
				}
				if (_currCode.Code == "AMD")
				{
					PO.ExchangeRate.Rate = 1m;
					PO.ExchangeRate.Base = 1m;
				}
				else
				{
					if (!_date.DoValidate() || !_date.EditValue.HasValue)
					{
						ControlsHelper.SetFocus(_date);
						throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
					}
					ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
					PO.ExchangeRate.Rate = exchangeRates.Rate;
					PO.ExchangeRate.Base = exchangeRates.Base;
				}
				AutoCalculateDiscount();
				_discountLogsList.Clear();
				foreach (DiscountLogRow discountLogs in PO.DiscountLogsList)
				{
					_discountLogsList.Add(discountLogs);
				}
				CalcSummAndSummVAT();
				UpdateChangeOrPaySum();
				MTBillSpecificationRow row = (MTBillSpecificationRow)_mtBillGrid.Grid.CurrentItem;
				RowInfoChange?.Invoke(row);
			}
			else
			{
				FillDocument(new StoreRequest());
			}
		}
		else
		{
			_discountLogsList.Clear();
		}
	}

	private void AutoCalculateDiscount()
	{
		for (int num = _mtBillList.Count - 1; num >= 0; num--)
		{
			if (_mtBillList[num].IsGift)
			{
				_mtBillList.RemoveAt(num);
			}
		}
		_discountsCalculator.DocInfo = new DocumentInfo
		{
			DocDate = _date.EditValue.Value,
			PartnerCode = _partner.Code,
			PartnerContract = _contract,
			CurrBase = PO.ExchangeRate.Base,
			CurrRate = PO.ExchangeRate.Rate,
			CurrCode = _currCode.Code,
			TotalAmount = (CalcBeginSumm() * PO.ExchangeRate.Rate / PO.ExchangeRate.Base).RoundAs(2),
			VATType = _vatType,
			PriceType = _priceListType,
			Storage = _storage.Code,
			Cash = _cashDesk.Code,
			SaleConsultant = _salesConsultant.Code,
			PriceListTypeRoundDgr = _priceListTypeRoundDgr,
			Rows = _mtBillList,
			Comment = _comment.Text
		};
		_discountsCalculator.InitializeAndCalculate();
		if (_discountsCalculator.Log != null)
		{
			PO.DiscountLogsList = _discountsCalculator.Log;
		}
	}

	private void On_PlCardPaymentClick()
	{
		if (_plCardPayment == null || !_plCardPayment.IsEnabled)
		{
			return;
		}
		decimal num = 0m;
		decimal num2 = 0m;
		if (_paramUseDiscount && _anyAutoDiscountModeIsEnabled && !PaymentIsInProcess())
		{
			CalculateDiscountAuto();
		}
		num = ((!(_summaPlCard.Value != 0m)) ? RemainSumForCardPayment() : _summaPlCard.Value);
		decimal num3 = 1m;
		decimal num4 = 1m;
		if (_currCode.Code != "AMD")
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			num3 = exchangeRates.Rate;
			num4 = exchangeRates.Base;
			num = (num * num3 / num4).RoundAs(2);
		}
		if (string.IsNullOrWhiteSpace(PO.BankAcquiring.PartnerCode) && string.IsNullOrWhiteSpace(PO.BankAcquiring.ContractCode))
		{
			if (!string.IsNullOrWhiteSpace(_cashDesk.Code))
			{
				CashCacheRow.NonCashInfo nonCashInfo = TradeSettings.TradeCacheService.Cash.GetNonCashInfoListByCashCode(_cashDesk.Code).FirstOrDefault((CashCacheRow.NonCashInfo x) => x.Type == 5);
				if (nonCashInfo != null)
				{
					PO.POSTerminalType = nonCashInfo.Device;
					PO.POSTerminalID = nonCashInfo.POSTerminalID;
					PO.POSTerminalAddress = nonCashInfo.POSTerminalAddress;
					if (!string.IsNullOrWhiteSpace(nonCashInfo.Partner))
					{
						PO.BankAcquiring.PartnerCode = nonCashInfo.Partner;
						PO.BankAcquiring.ContractCode = nonCashInfo.Contract;
					}
				}
			}
			else
			{
				PO.POSTerminalType = POSTerminalTypesSE.None.Value;
				PO.POSTerminalID = "";
				PO.POSTerminalAddress = "";
			}
		}
		if (_readOnlyAfterCheckPrint)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates2 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				num3 = exchangeRates2.Rate;
				num4 = exchangeRates2.Base;
				num2 = (_summaPlCard.Value * num3 / num4).RoundAs(2);
			}
			else
			{
				num2 = _summaPlCard.Value;
			}
		}
		decimal plCardPaySum = (_readOnlyAfterCheckPrint ? num2 : num);
		NonCashPaymentDialog nonCashPaymentDialog = new NonCashPaymentDialog(this, plCardPaySum, RemainSumForCardPayment(), _nonCashPaymentMethod, _readOnlyAfterCheckPrint, PO.BankAcquiring.PartnerCode, PO.BankAcquiring.ContractCode, _cashDesk.Code, PO.POSTerminalType, _partner.Code, PO.POSTerminalID, PO.POSTerminalAddress);
		if (nonCashPaymentDialog.ShowDialog().Value)
		{
			decimal num5 = 0m;
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates3 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				num3 = exchangeRates3.Rate;
				num4 = exchangeRates3.Base;
				num5 = (nonCashPaymentDialog.EnteredNonCashSum * num4 / num3).RoundAs(2);
			}
			else
			{
				num5 = nonCashPaymentDialog.EnteredNonCashSum;
			}
			if (_nonCashPaymentMethod != nonCashPaymentDialog.NonCashPaymentMethod || _summaPlCard.Value != num5)
			{
				_nonCashPaymentId.Text = "";
			}
			_summaPlCard.EditValue = num5;
			UpdateChangeOrPaySum();
			_nonCashPaymentMethod = nonCashPaymentDialog.NonCashPaymentMethod;
			PO.BankAcquiring.PartnerCode = nonCashPaymentDialog.EnteredBankAcquiringPartnerCode;
			PO.BankAcquiring.ContractCode = nonCashPaymentDialog.EnteredBankAcquiringContractCode;
			PO.POSTerminalType = nonCashPaymentDialog.POSTerminalType;
			PO.POSTerminalID = nonCashPaymentDialog.POSTerminalID;
			PO.POSTerminalAddress = nonCashPaymentDialog.POSTerminalAddress;
		}
		if (_summaPlCard.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Visible;
		}
		else
		{
			((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Collapsed;
		}
	}

	private void On_PricesClick()
	{
		if (_prices == null || !_prices.IsEnabled)
		{
			return;
		}
		bool flag = State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value || State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		bool flag2 = (_viewMode == WindowViewMode.Edit && !flag) || !string.IsNullOrWhiteSpace(PO.ECRCheckNum) || PO.PaymentAccepted;
		PricesDialog pricesDialog = new PricesDialog(this, _vatType, _priceListType, _priceListTypeRoundDgr, DiscountPriceListType, _retail, _paramUseDiscount, _allowChangePriceType, _allowChangeDiscountPrices, _allowChangeDiscountPriceType && !_anyAutoDiscountModeIsEnabled, !flag2);
		if (pricesDialog.ShowDialog().Value)
		{
			_priceListType = pricesDialog.PriceListType;
			_priceListTypeRoundDgr = pricesDialog.PriceListTypeRoundDgr;
			_vatType = pricesDialog.VATType;
			DiscountPriceListType = pricesDialog.DiscountPriceListType;
			bool questionAnswerIsYes = PriceListTypeEditValueChanged(_priceListType);
			VATTypeEditValueChanged();
			if (_paramUseDiscount && !_anyAutoDiscountModeIsEnabled)
			{
				DiscountPriceListTypeEditValueChanged(questionAnswerIsYes);
			}
			else if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
		}
		UpdateChangeOrPaySum();
		_mtBillTableView.CommitEditing();
	}

	private void DiscountPriceListTypeEditValueChanged(bool QuestionAnswerIsYes)
	{
		if (!QuestionAnswerIsYes)
		{
			if (_summa.Value != 0m)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangeDiscountPrices, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					QuestionAnswerIsYes = true;
				}
			}
			else
			{
				QuestionAnswerIsYes = true;
			}
		}
		if (QuestionAnswerIsYes)
		{
			if (!_date.DoValidate() || !_date.EditValue.HasValue)
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			UpdateDiscountPrices();
			CalcSummAndSummVAT();
		}
	}

	private void UpdateDiscountPrices()
	{
		if (_currCode.Code != "AMD")
		{
			TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
		}
		if (_mtBillList.Count != 0)
		{
			for (int i = 0; i < _mtBillList.Count; i++)
			{
				MTBillSpecificationRow MTBillGridRow = _mtBillList[i];
				CalculateGridRowDiscountPriceInner(ref MTBillGridRow);
				CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, calcDiscountPrice: false);
			}
		}
	}

	private void VATTypeEditValueChanged()
	{
		bool flag = _vatType.InList("1", "2", "5");
		if (_isShowVATColInBill)
		{
			_vatCol.Visible = flag;
			_vatCol.ShowInColumnChooser = flag;
		}
		if (!flag)
		{
			_vATSumm = 0m;
		}
		if (!_retail)
		{
			CalculateDiscount(_mtBillList, _discountPercent.Value, gDiscount: false);
		}
		CalcSummAndSummVAT();
	}

	private void On_UsePrePaymentClick()
	{
		if (_usePrePayment == null || !_usePrePayment.IsEnabled)
		{
			return;
		}
		if (_paramUseDiscount && _anyAutoDiscountModeIsEnabled && !PaymentIsInProcess())
		{
			CalculateDiscountAuto();
		}
		decimal num = 0m;
		decimal num2 = ((!(_summaPrePayment.Value != 0m)) ? RemainSumForPrePayment() : _summaPrePayment.Value);
		if (_currCode.Code != "AMD")
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			decimal rate = exchangeRates.Rate;
			decimal num3 = exchangeRates.Base;
			num2 = (num2 * rate / num3).RoundAs(2);
		}
		if (_readOnlyAfterCheckPrint)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates2 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				decimal rate = exchangeRates2.Rate;
				decimal num3 = exchangeRates2.Base;
				num = (_summaPrePayment.Value * rate / num3).RoundAs(2);
			}
			else
			{
				num = _summaPrePayment.Value;
			}
		}
		decimal prePaymentSum = (_readOnlyAfterCheckPrint ? num : num2);
		UsePrepaymentDialog usePrepaymentDialog = new UsePrepaymentDialog(this, prePaymentSum, RemainSumForPrePayment(), _partner.Code, _contract, _readOnlyAfterCheckPrint, _prepaymentPartnerCode, _prepaymentContractCode, _summaChange.Value);
		if (usePrepaymentDialog.ShowDialog().Value)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates3 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				decimal rate = exchangeRates3.Rate;
				decimal num3 = exchangeRates3.Base;
				_summaPrePayment.EditValue = (usePrepaymentDialog.EnteredPrePaymentSum * num3 / rate).RoundAs(2);
			}
			else
			{
				_summaPrePayment.EditValue = usePrepaymentDialog.EnteredPrePaymentSum;
			}
			_prepaymentPartnerCode = usePrepaymentDialog.PrepaymentPartnerCode;
			_prepaymentContractCode = usePrepaymentDialog.PrepaymentContractCode;
			UpdateChangeOrPaySum();
		}
		if (_summaPrePayment.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaPrePayment.Parent).Visibility = Visibility.Visible;
		}
		else
		{
			((LayoutItemWithRequired)(object)_summaPrePayment.Parent).Visibility = Visibility.Collapsed;
		}
	}

	private void On_ErasePaymentsClick()
	{
		if (_erasePayments == null || !_erasePayments.IsEnabled)
		{
			return;
		}
		_summaCash.Value = 0m;
		_summaCredit.Value = 0m;
		_summaPayByBonus.Value = 0m;
		_summaPlCard.Value = 0m;
		_addedBonus.Value = 0m;
		_sumAddBonus = 0m;
		_nonCashPaymentMethod = 0;
		_summaPrePayment.Value = 0m;
		((LayoutItemWithRequired)(object)_summaPrePayment.Parent).Visibility = Visibility.Collapsed;
		_nonCashPaymentId.Text = "";
		foreach (MTBillSpecificationRow mtBill in _mtBillList)
		{
			mtBill.UsedBonus = 0m;
		}
		((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_addedBonus.Parent).Visibility = Visibility.Collapsed;
		UpdateChangeOrPaySum();
	}

	private void On_SummaCashClick()
	{
		if (_summaCashButton == null || !_summaCashButton.IsEnabled)
		{
			return;
		}
		if (_paramUseDiscount && _anyAutoDiscountModeIsEnabled && !PaymentIsInProcess())
		{
			CalculateDiscountAuto();
		}
		decimal num = 0m;
		decimal num2 = ((!(_summaCash.Value != 0m)) ? RemainSumForCashPayment() : _summaCash.Value);
		if (_currCode.Code != "AMD")
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			decimal rate = exchangeRates.Rate;
			decimal num3 = exchangeRates.Base;
			num2 = (num2 * rate / num3).RoundAs(2);
		}
		if (_readOnlyAfterCheckPrint)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates2 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				decimal rate = exchangeRates2.Rate;
				decimal num3 = exchangeRates2.Base;
				num = (_summaCash.Value * rate / num3).RoundAs(2);
			}
			else
			{
				num = _summaCash.Value;
			}
		}
		decimal cashSum = (_readOnlyAfterCheckPrint ? num : num2);
		CashPaymentDialog cashPaymentDialog = new CashPaymentDialog(this, cashSum, RemainSumForCashPayment(), _readOnlyAfterCheckPrint);
		if (cashPaymentDialog.ShowDialog().Value)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates3 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				decimal rate = exchangeRates3.Rate;
				decimal num3 = exchangeRates3.Base;
				_summaCash.EditValue = (cashPaymentDialog.EnteredCashSum * num3 / rate).RoundAs(2);
			}
			else
			{
				_summaCash.EditValue = cashPaymentDialog.EnteredCashSum;
			}
			UpdateChangeOrPaySum();
		}
	}

	private void On_CreditClick()
	{
		if (_credit == null || !_credit.IsEnabled)
		{
			return;
		}
		decimal num = 0m;
		if ((string.IsNullOrWhiteSpace(_partner.Code) || !_partner.DoValidate()) && !_readOnlyAfterCheckPrint)
		{
			ControlsHelper.SetFocus(_partner);
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.CreditSaleBuyer);
		}
		if (_paramUseDiscount && _anyAutoDiscountModeIsEnabled && !PaymentIsInProcess())
		{
			CalculateDiscountAuto();
		}
		decimal num2 = ((!(_summaCredit.Value != 0m)) ? RemainSumForCreditPayment() : _summaCredit.Value);
		if (_currCode.Code != "AMD")
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			decimal rate = exchangeRates.Rate;
			decimal num3 = exchangeRates.Base;
			num2 = (num2 * rate / num3).RoundAs(2);
		}
		if (_readOnlyAfterCheckPrint)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates2 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				decimal rate = exchangeRates2.Rate;
				decimal num3 = exchangeRates2.Base;
				num = (_summaCredit.Value * rate / num3).RoundAs(2);
			}
			else
			{
				num = _summaCredit.Value;
			}
		}
		decimal creditSum = (_readOnlyAfterCheckPrint ? num : num2);
		CreditSaleDialog creditSaleDialog = new CreditSaleDialog(this, creditSum, RemainSumForCreditPayment(), _partner.Code, _contract, _readOnlyAfterCheckPrint);
		if (creditSaleDialog.ShowDialog().Value)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates3 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				decimal rate = exchangeRates3.Rate;
				decimal num3 = exchangeRates3.Base;
				_summaCredit.EditValue = (creditSaleDialog.EnteredCreditSum * num3 / rate).RoundAs(2);
			}
			else
			{
				_summaCredit.EditValue = creditSaleDialog.EnteredCreditSum;
			}
			UpdateChangeOrPaySum();
		}
		if (_summaCredit.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Visible;
		}
		else
		{
			((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Collapsed;
		}
	}

	private void On_FilteredSearchClick()
	{
		if (_filteredSearch == null || !_filteredSearch.IsEnabled)
		{
			return;
		}
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
		{
			FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true),
			SearchDataView = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices(_date.EditValue.Value, _storage.Code, _priceListType, _retail, "", "", extendedView: true, isFilteredSearchView: true),
			ResultsList = new MTInvoiceBaseFilteredSearchList<MTBillSpecificationRow>(_currCode.Code, _vatType, _date.EditValue.Value, !_retail, _priceListType, _priceListTypeRoundDgr, _priceCol.AllowEditing != DefaultBoolean.False)
		}, this, isBasketMode: true);
		if (!filteredSearchWindow.ShowDialog().Value)
		{
			return;
		}
		foreach (MTBillSpecificationRow item2 in filteredSearchWindow.BasketResult)
		{
			MTBillSpecificationRow MTBillGridRow = item2;
			MTBillGridRow.IsGift = false;
			MTBillGridRow.IsDeleted = false;
			if (MTBillGridRow.ItemType == GoodsTypesSE.Good.Value)
			{
				MTBillGridRow.Storage = _storage.Code;
			}
			if (!_paramUseDiscount)
			{
				MTBillGridRow.Discount = 0m;
			}
			else if (MTBillGridRow.Discount == 0m && _discountPercent.Value != 0m)
			{
				MTBillGridRow.Discount = _discountPercent.Value;
			}
			if (!_retail || MTBillGridRow.ItemType == GoodsTypesSE.Service.Value)
			{
				string mTQntUnitByMTIDPriceTypeAndDocDate = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(MTBillGridRow.ItemID, _priceListType, _date.EditValue.Value);
				if (!string.IsNullOrWhiteSpace(mTQntUnitByMTIDPriceTypeAndDocDate))
				{
					MTBillGridRow.UnitCode = mTQntUnitByMTIDPriceTypeAndDocDate;
					QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(MTBillGridRow.UnitCode);
					MTBillGridRow.UnitBrief = ((item == null) ? "" : item.Brief);
				}
				if (!_paramUseDiscount || !_alwaysCalcDiscountAuto)
				{
					CalculateGridRowDiscountPrice(ref MTBillGridRow);
					CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, MTBillGridRow.DiscountPrice == 0m);
				}
			}
			else
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(MTBillGridRow.ItemID, MTBillGridRow.UnitCode);
				if (qntUnit != null)
				{
					MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
					{
						MTCode = MTBillGridRow.ItemCode,
						Storage = _storage.Code,
						Date = _date.EditValue,
						UnitCoef = qntUnit.Coef
					};
					List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
					if (rows.Count > 0)
					{
						MTBillGridRow.Price = Methods.RoundPrice(rows[0].Price, _priceListTypeRoundDgr);
					}
				}
				if (!string.IsNullOrWhiteSpace(_priceListType))
				{
					if (_paramUseDiscount)
					{
						if (!_alwaysCalcDiscountAuto)
						{
							if (string.IsNullOrWhiteSpace(DiscountPriceListType))
							{
								MTBillGridRow.DiscountPrice = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, MTBillGridRow.ItemID, MTBillGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
								MTBillGridRow.DiscountPrice = Methods.RoundPrice(MTBillGridRow.DiscountPrice, _priceListTypeRoundDgr);
								if (MTBillGridRow.Price == 0m)
								{
									MTBillGridRow.Price = ((MTBillGridRow.Discount != 0m) ? (MTBillGridRow.DiscountPrice * 100m / (100m - MTBillGridRow.Discount)).RoundAs(2) : MTBillGridRow.DiscountPrice);
								}
								else
								{
									MTBillGridRow.Discount = (100m * (MTBillGridRow.Price - MTBillGridRow.DiscountPrice) / MTBillGridRow.Price).RoundAs(2);
								}
								CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, calcDiscountPrice: false);
							}
							else
							{
								CalculateGridRowDiscountPrice(ref MTBillGridRow);
								CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, MTBillGridRow.DiscountPrice == 0m);
							}
						}
					}
					else
					{
						CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, calcDiscountPrice: false);
					}
				}
				else if (!_paramUseDiscount || !_alwaysCalcDiscountAuto)
				{
					CalculateGridRowDiscountPrice(ref MTBillGridRow);
					CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, MTBillGridRow.DiscountPrice == 0m);
				}
			}
			MTBillGridRow.RowID = _maxRowID;
			_mtBillList.Add(MTBillGridRow);
			if (MTBillGridRow.ItemType == GoodsTypesSE.Good.Value && _distributByFEFO)
			{
				DistributeQtyByExpiredDate(MTBillGridRow.RowID, _storage.Code, MTBillGridRow.ItemCode, MTBillGridRow.UnitCode, MTBillGridRow.Quantity);
				MTBillGridRow.IsPartySelected = true;
			}
			_maxRowID++;
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		CalcSummAndSummVAT();
		UpdateChangeOrPaySum();
	}

	private void On_MTFavoritesClick()
	{
		if (_mtFavorites != null && _mtFavorites.IsEnabled)
		{
			if (!TradeSettings.CreateTradeClient().CommonServices.FavoriteMTsExists(Settings.LoginService.UserId))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.MTFavoriteInstruction, ArmSoft.SME.Common.Resources.RibbonPageFavorites, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			_mQty_Weight = 1m;
			MTFavoritesBrowser mTFavoritesBrowser = new MTFavoritesBrowser(new ArmSoft.SME.Trade.UI.DropDown.MTFavorites(Settings.LoginService.UserId, onlyMaterials: false, onlyGroups: false, _priceListType, _date.EditValue.Value, _vatType, _currCode.Code), AddFromFavorites, _priceListType, _date.EditValue.Value, showTree: false, showCardView: true);
			mTFavoritesBrowser.Owner = this;
			mTFavoritesBrowser.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			mTFavoritesBrowser.Show();
		}
	}

	private void AddFromFavorites(string itemCode, string itemUnitCode)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(itemCode);
		if (item == null)
		{
			ControlsHelper.SetFocus(_mtBillGrid.Grid);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, itemCode));
		}
		MTBillSpecificationRow MTBillGridRow = AddItemToList(item, itemCode, barCode: false, 0m, itemUnitCode);
		if (MTBillGridRow.ItemType == GoodsTypesSE.Good.Value && MTBillGridRow != null && _distributByFEFO)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == MTBillGridRow.RowID);
			DistributeQtyByExpiredDate(MTBillGridRow.RowID, _storage.Code, item.MTCode, MTBillGridRow.UnitCode, MTBillGridRow.Quantity, item.Unit);
			MTBillGridRow.IsPartySelected = true;
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
	}

	private void On_PrintMTBillClick()
	{
		if (_printBill != null && _printBill.IsEnabled)
		{
			PrintMTBillClick();
		}
	}

	private void PrintMTBillClick()
	{
		if (!PO.StoredInDb || PO.State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft || PO.State == (byte)ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted)
		{
			PO.PartnerID = -1;
			FillDocument(new StoreRequest());
		}
		if (!Settings.ParametersService.PrintMTCheckAndNew() || (PO.StoredInDb && !IsChanged) || SaveDocument())
		{
			PrintMTBill();
			if (Settings.ParametersService.PrintMTCheckAndNew())
			{
				_viewMode = WindowViewMode.NewAfterSave;
				OnNewAfterSaveUpper();
			}
		}
	}

	private void PrintMTBill()
	{
		FillDocument(new StoreRequest());
		string text = Settings.ParametersService.MTCheckTemplate();
		if (string.IsNullOrWhiteSpace(text))
		{
			text = Settings.CoreCacheService.Template.GetDefaultPrintTemplate(short.Parse(ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTBill.Value), throwExceptionNotExits: true).Name;
		}
		Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(PO, PrintMode.PrintDirect, text, CheckPrinterInfo.GetInstance().CheckPrinterName);
	}

	private void On_SubstituteItemsClick()
	{
		if (_substituteItems == null || !_substituteItems.IsEnabled)
		{
			return;
		}
		MTBillSpecificationRow focusedRow = (MTBillSpecificationRow)_mtBillTableView.Grid.CurrentItem;
		if (focusedRow == null)
		{
			return;
		}
		if (!TradeSettings.TradeCacheService.Material.HasSubstitution(focusedRow.ItemCode))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ThereIsNoSubstitution, focusedRow.ItemCode), ArmSoft.SME.Trade.Common.Resources.SubstituteItems, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return;
		}
		string substituteItemsGroup = TradeSettings.TradeCacheService.Material.GetItem(focusedRow.ItemCode).SubstituteItemsGroup;
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices goodsAvailabilityFilterForInvoices = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices(_date.EditValue.Value, _storage.Code, _priceListType, _retail, "", substituteItemsGroup);
		ModalBrowser modalBrowser = new ModalBrowser(goodsAvailabilityFilterForInvoices)
		{
			Owner = this,
			WindowStartupLocation = WindowStartupLocation.CenterOwner
		};
		if (!modalBrowser.Show())
		{
			return;
		}
		string text = goodsAvailabilityFilterForInvoices.ValueMember(modalBrowser.SelectedRowIndex);
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
		if (item == null)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text));
		}
		bool flag = TradeSettings.TradeCacheService.Material.GetQntUnit(item.Id, focusedRow.UnitCode) != null;
		ChangeItemCodeValue(item.MTCode, focusedRow, flag, isSubstitution: true);
		if (!flag)
		{
			focusedRow.Quantity = 0m;
			CalculateGridRowSummaAndDiscountSumma(ref focusedRow, calcDiscountPrice: false);
		}
		if (_distributByFEFO && focusedRow.ItemType == GoodsTypesSE.Good.Value && focusedRow.Quantity != 0m)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == focusedRow.RowID);
			DistributeQtyByExpiredDate(focusedRow.RowID, focusedRow.Storage, focusedRow.ItemCode, focusedRow.UnitCode, focusedRow.Quantity, item.Unit);
			focusedRow.IsPartySelected = true;
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		CalculateDocumentSumma();
		ChangeMarkingStatus(focusedRow);
		RowInfoChange?.Invoke(focusedRow);
	}

	private void OnSwitchUnit()
	{
		MTBillSpecificationRow selectedRow = (MTBillSpecificationRow)_mtBillGrid.Grid.SelectedItem;
		if (selectedRow == null)
		{
			return;
		}
		List<MaterialCacheRow.QntUnitRow> qntUnits = TradeSettings.TradeCacheService.Material.GetItem(selectedRow.ItemCode).QntUnits;
		if (qntUnits.Count > 1)
		{
			int num = qntUnits.IndexOf((MaterialCacheRow.QntUnitRow x) => x.QntUnit == selectedRow.UnitCode);
			string unitCode = selectedRow.UnitCode;
			if (num + 1 == qntUnits.Count)
			{
				selectedRow.UnitCode = qntUnits[0].QntUnit;
				UnitChanged(selectedRow.UnitCode, unitCode, ref selectedRow);
			}
			else
			{
				selectedRow.UnitCode = qntUnits[num + 1].QntUnit;
				UnitChanged(selectedRow.UnitCode, unitCode, ref selectedRow);
			}
			ControlsHelper.SetFocus(_mtBillTableView);
			CalculateDocumentSumma();
			ChangeRowInfo(selectedRow);
			_mtBillGrid.Grid.RefreshData();
		}
	}

	private void On_WeightClick()
	{
		if (_weight == null || !_weight.IsEnabled || _mtBillTableView.Grid.CurrentItem == null)
		{
			return;
		}
		MTBillSpecificationRow mTBillSpecificationRow = (MTBillSpecificationRow)_mtBillTableView.Grid.CurrentItem;
		if (mTBillSpecificationRow != null && !mTBillSpecificationRow.IsGift && mTBillSpecificationRow.UnitCode == WeightItemInfo.GetInstance().Unit)
		{
			decimal weight = ScaleConnector.GetWeight();
			mTBillSpecificationRow.Quantity = weight;
			mTBillSpecificationRow.Summa = (mTBillSpecificationRow.Quantity * mTBillSpecificationRow.Price * (100m - mTBillSpecificationRow.Discount) / 100m).RoundAs(2);
			mTBillSpecificationRow.DiscountSumma = ((mTBillSpecificationRow.Quantity * mTBillSpecificationRow.Price).RoundAs(2) - mTBillSpecificationRow.Summa).RoundAs(2);
			if (_paramUseDiscount && _alwaysCalcDiscountAuto)
			{
				On_DiscountClick();
			}
			CalculateDocumentSumma();
			ChangeMarkingStatus(mTBillSpecificationRow);
			RowInfoChange?.Invoke(mTBillSpecificationRow);
		}
	}

	private void On_SalePriceClick()
	{
		if (_salePrice == null || !_salePrice.IsEnabled)
		{
			return;
		}
		if (!_isAllowChangeGoodRowInBill)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.AccessDenied, 20008);
		}
		_inputValue.DoValidate();
		if (_mtBillTableView.Grid.CurrentItem == null)
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(_inputValue.Text))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.InputBoxIsEmpty);
		}
		if (!_inputValue.Text.Trim().IsNumeric())
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
		}
		if (!decimal.TryParse(_inputValue.Text.Trim(), out var result))
		{
			return;
		}
		decimal num = result.RoundAs(2);
		if (num < 0m)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidatePriceIsPositive);
		}
		if (num.ToString("#").Length > 9)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.ErrNumberOfDigitsBeforeDecimalPart);
		}
		MTBillSpecificationRow MTBillGridRow = (MTBillSpecificationRow)_mtBillTableView.Grid.CurrentItem;
		if (!MTBillGridRow.IsGift)
		{
			if (!_retail || MTBillGridRow.ItemType == GoodsTypesSE.Service.Value)
			{
				MTBillGridRow.Price = num;
				if (TradeSettings.TradeCacheService.Material.GetItem(MTBillGridRow.ItemCode) != null && (!_paramUseDiscount || !_alwaysCalcDiscountAuto))
				{
					CalculateGridRowDiscountPrice(ref MTBillGridRow);
					CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, MTBillGridRow.DiscountPrice == 0m);
				}
			}
			else
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(MTBillGridRow.ItemID, MTBillGridRow.UnitCode);
				if (qntUnit == null)
				{
					return;
				}
				MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
				{
					MTCode = MTBillGridRow.ItemCode,
					Storage = _storage.Code,
					Date = _date.EditValue,
					UnitCoef = qntUnit.Coef
				};
				List<MTRetailQtyAndPriceDataProvider.DataRow> MTRetailQtyAndPrices = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
				if (IsPriceFromMTRetailQtyAndPrices(num, ref MTRetailQtyAndPrices))
				{
					MTBillGridRow.Price = num;
					if (TradeSettings.TradeCacheService.Material.GetItem(MTBillGridRow.ItemCode) != null && (!_paramUseDiscount || !_alwaysCalcDiscountAuto))
					{
						CalculateGridRowDiscountPrice(ref MTBillGridRow);
						CalculateGridRowSummaAndDiscountSumma(ref MTBillGridRow, MTBillGridRow.DiscountPrice == 0m);
					}
				}
				else
				{
					MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(MTBillGridRow.ItemCode, qntUnit.Coef, _storage.Code, _date.EditValue.Value);
					mTRetailQtyAndPrice.LoadData();
					ModalBrowser modalBrowser = new ModalBrowser(mTRetailQtyAndPrice)
					{
						Owner = this,
						WindowStartupLocation = WindowStartupLocation.CenterOwner
					};
					if (!modalBrowser.Show())
					{
						_inputValue.Text = "";
						_realText = "";
						ControlsHelper.SetFocus(_inputValue);
						return;
					}
					num = decimal.Parse(mTRetailQtyAndPrice.CommentMember(modalBrowser.SelectedRowIndex));
					MTBillGridRow.Price = num;
				}
			}
		}
		if (_paramUseDiscount && _alwaysCalcDiscountAuto)
		{
			On_DiscountClick();
		}
		CalculateDocumentSumma();
		RowInfoChange?.Invoke(MTBillGridRow);
		_inputValue.Text = "";
		_realText = "";
		ControlsHelper.SetFocus(_inputValue);
	}

	public void On_PrintECRCheckClick()
	{
		if (_printECRCheck != null && _printECRCheck.IsEnabled)
		{
			PrintCheck();
			if (Settings.ParametersService.PrintECRAndProductCheck() && !Settings.ParametersService.PrintECRCheckOnCheckPrinter())
			{
				PrintMTBillClick();
			}
		}
	}

	private void DiscountPercent_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue == null)
		{
			return;
		}
		if ((decimal)e.NewValue != (decimal)e.OldValue && CalcMTBillListRowCount(includeDeleted: false) > 0 && !_anyAutoDiscountModeIsEnabled)
		{
			if ((decimal)e.OldValue != 0m)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ComfirmDiscountChange, default(ReadOnlySpan<object>)), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					CalculateDiscount();
					return;
				}
				_discountPercent.EditValueChanged -= _discountPercent_EditValueChangedHandler.Handler;
				_discountPercent.EditValue = (decimal)e.NewValue;
				_discountPercent.DoValidate();
				_discountPercent.EditValueChanged += _discountPercent_EditValueChangedHandler.Handler;
			}
			else
			{
				CalculateDiscount();
			}
		}
		else
		{
			CalculateDiscount();
		}
	}

	private void SummaCash_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if ((decimal)e.NewValue != (decimal)e.OldValue)
		{
			UpdateChangeOrPaySum();
			if (_anyAutoDiscountModeIsEnabled)
			{
				AutoCalcDiscountAdjustControlsAccessibility();
			}
		}
	}

	private void OtherPaymentTypes_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if ((decimal)e.NewValue != (decimal)e.OldValue && _anyAutoDiscountModeIsEnabled)
		{
			AutoCalcDiscountAdjustControlsAccessibility();
		}
	}

	private void SummaPrePayment_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if ((decimal)e.NewValue != (decimal)e.OldValue && _anyAutoDiscountModeIsEnabled)
		{
			AutoCalcDiscountAdjustControlsAccessibility();
		}
	}

	private void SummaChange_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if ((decimal)e.NewValue < 0m)
		{
			_summaChange.Foreground = Brushes.Red;
		}
		else
		{
			_summaChange.Foreground = _summaCash.Foreground;
		}
		if (_anyAutoDiscountModeIsEnabled)
		{
			AutoCalcDiscountAdjustControlsAccessibility();
		}
		if (_isShowPoleDisplay)
		{
			int num = CalcMTBillListRowCount(includeDeleted: false);
			if (num == 0)
			{
				ClearPoleDisplay(string.IsNullOrWhiteSpace(_partCode));
				return;
			}
			SetTotalInfoToPoleDisplay();
			SetRowCountInfoToPoleDisplay(num);
		}
	}

	private void AddedBonus_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if ((decimal)e.NewValue != 0m)
		{
			((LayoutItemWithRequired)(object)_addedBonus.Parent).Visibility = Visibility.Visible;
		}
		else
		{
			((LayoutItemWithRequired)(object)_addedBonus.Parent).Visibility = Visibility.Collapsed;
		}
	}

	private void CalculateDiscount(CommonConst.JSDiscType discountType = CommonConst.JSDiscType.ProgCalc)
	{
		switch (discountType)
		{
		case CommonConst.JSDiscType.DiscountPercent:
			foreach (MTBillSpecificationRow mtBill in _mtBillList)
			{
				mtBill.DiscountPrice = Methods.RoundPrice((mtBill.Price * (1m - mtBill.Discount / 100m)).RoundAs(2), _priceListTypeRoundDgr);
				mtBill.Summa = (mtBill.Quantity * mtBill.DiscountPrice).RoundAs(2);
				mtBill.DiscountSumma = (mtBill.Quantity * mtBill.Price).RoundAs(2) - mtBill.Summa;
			}
			break;
		case CommonConst.JSDiscType.DiscountPrice:
			foreach (MTBillSpecificationRow mtBill2 in _mtBillList)
			{
				if (mtBill2.Price != 0m)
				{
					mtBill2.Discount = (100m * (mtBill2.Price - mtBill2.DiscountPrice) / mtBill2.Price).RoundAs(2);
				}
				else
				{
					mtBill2.Discount = 0m;
				}
				mtBill2.Summa = (mtBill2.Quantity * mtBill2.DiscountPrice).RoundAs(2);
				mtBill2.DiscountSumma = (mtBill2.Quantity * mtBill2.Price).RoundAs(2) - mtBill2.Summa;
			}
			break;
		case CommonConst.JSDiscType.DiscountSumm:
			foreach (MTBillSpecificationRow mtBill3 in _mtBillList)
			{
				if (mtBill3.Quantity != 0m)
				{
					mtBill3.DiscountPrice = Methods.RoundPrice((mtBill3.Summa / mtBill3.Quantity).RoundAs(2), _priceListTypeRoundDgr);
					if (!(mtBill3.Price == 0m))
					{
						mtBill3.Discount = (100m * (mtBill3.Price - mtBill3.DiscountPrice) / mtBill3.Price).RoundAs(2);
					}
					else
					{
						mtBill3.Discount = 0m;
					}
					mtBill3.DiscountSumma = (mtBill3.Quantity * mtBill3.Price).RoundAs(2) - mtBill3.Summa;
				}
			}
			break;
		case CommonConst.JSDiscType.ProgCalc:
			foreach (MTBillSpecificationRow mtBill4 in _mtBillList)
			{
				mtBill4.Discount = _discountPercent.Value;
				mtBill4.DiscountPrice = Methods.RoundPrice((mtBill4.Price * (1m - _discountPercent.Value / 100m)).RoundAs(2), _priceListTypeRoundDgr);
				mtBill4.Summa = (mtBill4.Quantity * mtBill4.DiscountPrice).RoundAs(2);
				mtBill4.DiscountSumma = (mtBill4.Quantity * mtBill4.Price).RoundAs(2) - mtBill4.Summa;
			}
			break;
		}
		CalcSummAndSummVAT();
		UpdateChangeOrPaySum();
		if (_currCode.Code == "AMD")
		{
			PO.SummNCC = _summa.Value;
			return;
		}
		ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
		PO.ExchangeRate.Rate = exchangeRates.Rate;
		PO.ExchangeRate.Base = exchangeRates.Base;
		PO.SummNCC = (_summa.Value * PO.ExchangeRate.Rate / PO.ExchangeRate.Base).RoundAs(2);
	}

	private void FillMTBillGridRow(MTBillSpecificationRow gridRow, MaterialCacheRow goodsRow, string unitCode, decimal price, decimal quantity = -1m)
	{
		gridRow.ItemType = goodsRow.Type;
		if (goodsRow.Type == GoodsTypesSE.Good.Value)
		{
			gridRow.Storage = _storage.Code;
			gridRow.IsPartySelected = false;
		}
		else
		{
			gridRow.Storage = "";
			gridRow.IsPartySelected = null;
		}
		gridRow.ItemID = goodsRow.Id;
		gridRow.ItemCode = goodsRow.MTCode;
		gridRow.ItemName = goodsRow.Caption;
		gridRow.BarCode = goodsRow.BarCode;
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(unitCode);
		gridRow.UnitCode = unitCode;
		gridRow.UnitBrief = ((item == null) ? "" : item.Brief);
		gridRow.VAT = goodsRow.VAT;
		if (quantity == -1m)
		{
			gridRow.Quantity = _mQty_Weight;
		}
		else
		{
			gridRow.Quantity = quantity;
		}
		gridRow.Price = price;
		if (_paramUseDiscount && !_alwaysCalcDiscountAuto)
		{
			gridRow.Discount = _discountPercent.Value;
			if (string.IsNullOrWhiteSpace(DiscountPriceListType) && (gridRow.Discount == 0m || goodsRow.Discount != 0m))
			{
				gridRow.Discount = goodsRow.Discount;
			}
		}
		if (string.IsNullOrWhiteSpace(_priceListType) || !_retail || gridRow.ItemType == GoodsTypesSE.Service.Value || !_mDiscountUse)
		{
			return;
		}
		if (!string.IsNullOrWhiteSpace(DiscountPriceListType))
		{
			CalculateGridRowDiscountPrice(ref gridRow);
			return;
		}
		gridRow.DiscountPrice = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, gridRow.ItemID, gridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
		gridRow.DiscountPrice = Methods.RoundPrice(gridRow.DiscountPrice, _priceListTypeRoundDgr);
		if (gridRow.Price != 0m)
		{
			gridRow.Discount = (100m * (gridRow.Price - gridRow.DiscountPrice) / gridRow.Price).RoundAs(2);
		}
		else
		{
			gridRow.Price = ((gridRow.Discount != 0m) ? (gridRow.DiscountPrice * 100m / (100m - gridRow.Discount)).RoundAs(2) : gridRow.DiscountPrice);
		}
	}

	private void UpdateChangeOrPaySum()
	{
		_changeOrPaySum = PaidSum() - _summa.Value;
		_summaChange.EditValue = Math.Abs(_changeOrPaySum);
		if (_changeOrPaySum <= 0m)
		{
			((LayoutItemWithRequired)(object)_summaChange.Parent).Label = ArmSoft.SME.Trade.Common.Resources.ForPay;
			((LayoutItemWithRequired)(object)_summaChange.Parent).Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326");
			_summaChange.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326");
		}
		else
		{
			((LayoutItemWithRequired)(object)_summaChange.Parent).Label = ArmSoft.SME.Trade.Common.Resources.SummaChange;
			((LayoutItemWithRequired)(object)_summaChange.Parent).Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#A9A9A9");
			_summaChange.Foreground = _summaCash.Foreground;
		}
	}

	private void IncreaseDecreaseRowQty(bool increase = true)
	{
		try
		{
			if (!_mtBillGrid.AllowEdit && !(!_isAllowChangeGoodRowInBill & increase))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.AccessDenied, 20008);
			}
			if (_mtBillTableView.Grid.CurrentItem == null)
			{
				return;
			}
			MTBillSpecificationRow focusedRow = (MTBillSpecificationRow)_mtBillTableView.Grid.CurrentItem;
			if (focusedRow == null || focusedRow.IsGift)
			{
				return;
			}
			if (_mtBillTableView.IsEditing)
			{
				_mtBillTableView.CommitEditing();
			}
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
			{
				focusedRow.Quantity += (decimal)(increase ? 1 : (-1));
				if (focusedRow.Quantity <= 0m)
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == focusedRow.RowID);
					_mtBillList.Remove(focusedRow);
					focusedRow = null;
				}
				else
				{
					ChangeRowQty(focusedRow);
				}
				CalculateDocumentSumma();
				ChangeMarkingStatus(focusedRow);
				RowInfoChange?.Invoke(focusedRow);
			}), Array.Empty<object>());
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ChangeRowQty(MTBillSpecificationRow focusedRow)
	{
		if (_distributByFEFO)
		{
			if (focusedRow.ItemType == GoodsTypesSE.Good.Value)
			{
				PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == focusedRow.RowID);
				DistributeQtyByExpiredDate(focusedRow.RowID, focusedRow.Storage, focusedRow.ItemCode, focusedRow.UnitCode, focusedRow.Quantity);
				focusedRow.IsPartySelected = true;
			}
		}
		else if (focusedRow.IsPartySelected.HasValue && focusedRow.IsPartySelected.Value)
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

	private void CalculateGridRowDiscountPrice(ref MTBillSpecificationRow MTBillGridRow)
	{
		if (string.IsNullOrWhiteSpace(DiscountPriceListType))
		{
			MTBillGridRow.DiscountPrice = 0m;
		}
		else
		{
			CalculateGridRowDiscountPriceInner(ref MTBillGridRow);
		}
	}

	private void CalculateGridRowDiscountPriceInner(ref MTBillSpecificationRow MTBillGridRow)
	{
		if (!_mDiscountUse)
		{
			MTBillGridRow.DiscountPrice = MTBillGridRow.Price;
			MTBillGridRow.Discount = 0m;
			return;
		}
		PricesDataProvider.DataRow dataRow = null;
		if (!string.IsNullOrWhiteSpace(DiscountPriceListType))
		{
			dataRow = TradeSettings.CreateTradeClient().CommonServices.GetPrice(DiscountPriceListType, MTBillGridRow.ItemID, MTBillGridRow.UnitCode, _date.EditValue.Value);
		}
		if (dataRow == null || dataRow.Price == 0m)
		{
			if (!(_discountPercent.Value == 0m))
			{
				MTBillGridRow.Discount = _discountPercent.Value;
				MTBillGridRow.DiscountPrice = Methods.RoundPrice(MTBillGridRow.Price * (100m - MTBillGridRow.Discount) / 100m, _priceListTypeRoundDgr);
			}
			else
			{
				MTBillGridRow.DiscountPrice = MTBillGridRow.Price;
				MTBillGridRow.Discount = 0m;
			}
			return;
		}
		if (_discountPriceListCurr == _currCode.Code)
		{
			MTBillGridRow.DiscountPrice = dataRow.Price;
		}
		else if (_discountPriceListCurr == "AMD")
		{
			MTBillGridRow.DiscountPrice = (dataRow.Price / _docCurrRate * _docCurrBase).RoundAs(2);
		}
		else
		{
			MTBillGridRow.DiscountPrice = (dataRow.Price * _discountPriceListCurrRate / _discountPriceListCurrBase / _docCurrRate * _docCurrBase).RoundAs(2);
		}
		if (Methods.VATAndRetailVATMapping(_discountPriceListVATType, oppositeMapping: true) != _vatType && MTBillGridRow.VAT.Value)
		{
			switch (_discountPriceListVATType)
			{
			case "1":
				if (_vatType.InList("2", "5"))
				{
					MTBillGridRow.DiscountPrice += PriceHelper.CalcVATAmountByType(MTBillGridRow.DiscountPrice, "1", MTBillGridRow.VAT.Value);
				}
				break;
			case "2":
				if (_vatType.InList("1", "3", "4"))
				{
					MTBillGridRow.DiscountPrice -= PriceHelper.CalcVATAmountByType(MTBillGridRow.DiscountPrice, "5", MTBillGridRow.VAT.Value);
				}
				break;
			case "5":
				if (_vatType.InList("1", "3", "4"))
				{
					MTBillGridRow.DiscountPrice -= PriceHelper.CalcVATAmountByType(MTBillGridRow.DiscountPrice, "5", MTBillGridRow.VAT.Value);
				}
				break;
			}
		}
		MTBillGridRow.DiscountPrice = Methods.RoundPrice(MTBillGridRow.DiscountPrice, _discountPriceTypeRoundDegree);
		if (MTBillGridRow.Price == 0m)
		{
			MTBillGridRow.Price = MTBillGridRow.DiscountPrice;
		}
		else
		{
			MTBillGridRow.Discount = (100m * (MTBillGridRow.Price - MTBillGridRow.DiscountPrice) / MTBillGridRow.Price).RoundAs(2);
		}
	}

	private void CalculateGridRowSummaAndDiscountSumma(ref MTBillSpecificationRow mtBillGridRow, bool calcDiscountPrice = true)
	{
		if (_mDiscountUse)
		{
			if (calcDiscountPrice)
			{
				if (!string.IsNullOrWhiteSpace(_priceListTypeRoundDgr))
				{
					mtBillGridRow.DiscountPrice = Methods.RoundPrice(mtBillGridRow.Price * (100m - mtBillGridRow.Discount) / 100m, _priceListTypeRoundDgr);
				}
				else
				{
					mtBillGridRow.DiscountPrice = (mtBillGridRow.Price * (100m - mtBillGridRow.Discount) / 100m).RoundAs(2);
				}
			}
			mtBillGridRow.Summa = (mtBillGridRow.Quantity * mtBillGridRow.DiscountPrice).RoundAs(2);
			mtBillGridRow.DiscountSumma = ((mtBillGridRow.Quantity * mtBillGridRow.Price).RoundAs(2) - mtBillGridRow.Summa).RoundAs(2);
		}
		else
		{
			mtBillGridRow.DiscountPrice = mtBillGridRow.Price;
			mtBillGridRow.Summa = (mtBillGridRow.Quantity * mtBillGridRow.Price).RoundAs(2);
			mtBillGridRow.DiscountSumma = 0m;
		}
	}

	private static bool IsPriceFromMTRetailQtyAndPrices(decimal Price, ref List<MTRetailQtyAndPriceDataProvider.DataRow> MTRetailQtyAndPrices)
	{
		bool result = false;
		foreach (MTRetailQtyAndPriceDataProvider.DataRow MTRetailQtyAndPrice in MTRetailQtyAndPrices)
		{
			if (MTRetailQtyAndPrice.Price == Price)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private void CalculateDocumentSumma()
	{
		CalcSummAndSummVAT();
		UpdateChangeOrPaySum();
		if (_currCode.Code == "AMD")
		{
			PO.SummNCC = _summa.Value;
			return;
		}
		ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
		PO.ExchangeRate.Rate = exchangeRates.Rate;
		PO.ExchangeRate.Base = exchangeRates.Base;
		PO.SummNCC = (_summa.Value * PO.ExchangeRate.Rate / PO.ExchangeRate.Base).RoundAs(2);
	}

	private void CalculateDiscountAuto()
	{
		On_DiscountClick();
		TradeNotificationHelper.ShowTradeDefaultNotification(Caption, string.Format(ArmSoft.SME.Common.Resources.DiscountCalculated, default(ReadOnlySpan<object>)), "Discount");
	}

	private int CalcMTBillListRowCount(bool includeDeleted)
	{
		if (!includeDeleted)
		{
			return _mtBillList.Count((MTBillSpecificationRow x) => !x.IsDeleted);
		}
		return _mtBillList.Count;
	}

	private void CalcSummAndSummVAT()
	{
		decimal vATSumm = 0m;
		decimal num = 0m;
		_tmpDiscountSumm = 0m;
		foreach (MTBillSpecificationRow mtBill in _mtBillList)
		{
			if (!mtBill.IsDeleted)
			{
				num += mtBill.Summa;
				_tmpDiscountSumm += mtBill.DiscountSumma;
				vATSumm += PriceHelper.CalcVATAmountByType(mtBill.Summa, _vatType, mtBill.VAT.Value);
			}
		}
		decimal num2 = num;
		if (!string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillRoundDegree()))
		{
			num2 = Methods.RoundPrice(num, Settings.ParametersService.MTBillRoundDegree(), Settings.ParametersService.MTBillSummRoundUp());
		}
		_sumFromRound = num2 - num;
		_summa.EditValue = num2;
		_vATSumm = vATSumm;
	}

	private decimal CalcBeginSumm()
	{
		decimal result = 0m;
		foreach (MTBillSpecificationRow mtBill in _mtBillList)
		{
			if (!mtBill.IsDeleted && mtBill.ItemID != -1)
			{
				result += (mtBill.Quantity * mtBill.Price).RoundAs(2);
			}
		}
		return result;
	}

	public override void OnScanner(string scannerData)
	{
		if (!_readOnlyAfterCheckPrint)
		{
			_inputValue.Text = MarkingHelper.CleanScannerSymbols(scannerData);
			On_BarCodeEnterClick();
		}
	}

	public override void OnMagneticCardReader(string mcrData)
	{
		try
		{
			if (!string.IsNullOrWhiteSpace(mcrData) && !_readOnlyAfterCheckPrint && MCRInfo.GetInstance().IsValidCard(mcrData))
			{
				string cardNumber = MCRInfo.GetInstance().GetCardNumber(mcrData);
				if (!DocumentHelpers.GetPartnerEmployeeDataByBarCode(cardNumber, out var partnerCacheRow, out var partnerContract, out var employee, "AMD", hideExceptions: true))
				{
					AddNewBuyer(cardNumber);
				}
				else
				{
					FillPartnerEmployeeData(partnerCacheRow, partnerContract, employee);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private MTBillSpecificationRow AddItemToList(MaterialCacheRow materialCacheRow, string value, bool barCode = true, decimal retailPrice = 0m, string qntUnit = "")
	{
		MTBillSpecificationRow mtBillGridRow = null;
		int count = _mtBillList.Count;
		int num = ((!(_rowAddMode == "3")) ? AddNewRowOrMergeInner(materialCacheRow, value, ref mtBillGridRow, barCode, retailPrice, _rowAddMode == "2", qntUnit) : AddNewRowInner(materialCacheRow, value, ref mtBillGridRow, barCode, retailPrice, qntUnit));
		if (num == -1)
		{
			return null;
		}
		_mtBillTableView.Grid.CurrentItem = mtBillGridRow;
		bool flag = count != _mtBillList.Count;
		if (string.IsNullOrWhiteSpace(_priceListType) || (flag && (!_retail || mtBillGridRow.ItemType == GoodsTypesSE.Service.Value)))
		{
			CalculateGridRowDiscountPrice(ref mtBillGridRow);
			CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, mtBillGridRow.DiscountPrice == 0m);
		}
		else
		{
			CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, calcDiscountPrice: false);
		}
		_mtBillGrid.ConfigureButtons();
		CalculateDocumentSumma();
		_inputValue.Text = "";
		_realText = "";
		ControlsHelper.SetFocus(_inputValue);
		ChangeMarkingStatus(mtBillGridRow);
		RowInfoChange?.Invoke(mtBillGridRow);
		return mtBillGridRow;
	}

	private int AddNewRowInner(MaterialCacheRow materialCacheRow, string value, ref MTBillSpecificationRow mtBillGridRow, bool isBarCode = true, decimal retailPrice = 0m, string qntUnit = "")
	{
		decimal price = 0m;
		string qntUnit2 = "";
		if (materialCacheRow == null)
		{
			_inputValue.Text = "";
			_realText = "";
			ControlsHelper.SetFocus(_inputValue);
			return -1;
		}
		string text = null;
		if (isBarCode)
		{
			if (TradeSettings.TradeCacheService.Material.GetItemByBarCode(value, out qntUnit2) != null)
			{
				text = value;
			}
		}
		else
		{
			qntUnit2 = (string.IsNullOrWhiteSpace(qntUnit) ? materialCacheRow.Unit : qntUnit);
			text = TradeSettings.TradeCacheService.Material.GetBarCode(materialCacheRow.Id, qntUnit2)?.BarCode;
		}
		if (!_retail || materialCacheRow.Type == GoodsTypesSE.Service.Value)
		{
			if (_applyLastPrice && !isBarCode)
			{
				price = PriceHelper.GetPriceCalcForUnitByPriceList(_priceListType, _vatType, materialCacheRow.MTCode, qntUnit2, _date.EditValue.Value, _currCode.Code);
			}
			else
			{
				price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, materialCacheRow.Id, qntUnit2, _date.EditValue.Value, _currCode.Code);
				if (price == 0m && materialCacheRow.Unit != materialCacheRow.AltUnit)
				{
					price = PriceHelper.GetPriceCalcForUnitByPriceList(_priceListType, _vatType, materialCacheRow.MTCode, qntUnit2, _date.EditValue.Value, _currCode.Code);
				}
			}
			price = Methods.RoundPrice(price, _priceListTypeRoundDgr);
		}
		else
		{
			MaterialCacheRow.QntUnitRow qntUnit3 = TradeSettings.TradeCacheService.Material.GetQntUnit(materialCacheRow.Id, qntUnit2);
			if (qntUnit3 != null)
			{
				if (retailPrice == 0m)
				{
					MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(materialCacheRow.MTCode, qntUnit3.Coef, _storage.Code, _date.EditValue.Value);
					mTRetailQtyAndPrice.LoadData();
					if (mTRetailQtyAndPrice.RowsCount == 1)
					{
						price = decimal.Parse(mTRetailQtyAndPrice.CommentMember(0));
					}
					else if (mTRetailQtyAndPrice.RowsCount > 1)
					{
						ModalBrowser modalBrowser = new ModalBrowser(mTRetailQtyAndPrice)
						{
							Owner = this,
							WindowStartupLocation = WindowStartupLocation.CenterOwner
						};
						if (!modalBrowser.Show())
						{
							_inputValue.Text = "";
							_realText = "";
							ControlsHelper.SetFocus(_inputValue);
							return -1;
						}
						price = decimal.Parse(mTRetailQtyAndPrice.CommentMember(modalBrowser.SelectedRowIndex));
					}
				}
				else
				{
					price = retailPrice;
				}
			}
		}
		mtBillGridRow = new MTBillSpecificationRow();
		FillMTBillGridRow(mtBillGridRow, materialCacheRow, qntUnit2, price);
		if (text != null)
		{
			mtBillGridRow.BarCode = text;
		}
		mtBillGridRow.RowID = _maxRowID;
		_mtBillList.Add(mtBillGridRow);
		_maxRowID++;
		return _mtBillGrid.Grid.GetRowHandleByListIndex(_mtBillList.Count - 1);
	}

	private int AddNewRowOrMergeInner(MaterialCacheRow materialCacheRow, string value, ref MTBillSpecificationRow mtBillGridRow, bool barCode = true, decimal retailPrice = 0m, bool forFocusedRow = false, string qntUnit = "")
	{
		decimal num = 0m;
		int result = -1;
		MTBillSpecificationRow mTBillSpecificationRow = null;
		if (forFocusedRow && CalcMTBillListRowCount(includeDeleted: true) != 0)
		{
			mTBillSpecificationRow = (MTBillSpecificationRow)_mtBillTableView.Grid.CurrentItem;
			if (mTBillSpecificationRow == null)
			{
				return -1;
			}
		}
		for (int i = 0; i <= _mtBillList.Count - 1; i++)
		{
			MTBillSpecificationRow mTBillSpecificationRow2 = _mtBillList[i];
			if ((forFocusedRow && mTBillSpecificationRow.RowID != mTBillSpecificationRow2.RowID) || mTBillSpecificationRow2.IsDeleted || mTBillSpecificationRow2.IsGift || ((!barCode || !(mTBillSpecificationRow2.BarCode.Trim() == value.Trim())) && (barCode || !(mTBillSpecificationRow2.ItemCode == value) || (!string.IsNullOrWhiteSpace(qntUnit) && !(mTBillSpecificationRow2.UnitCode == qntUnit)))))
			{
				continue;
			}
			if (!_retail || materialCacheRow.Type == GoodsTypesSE.Service.Value)
			{
				mTBillSpecificationRow2.Quantity += _mQty_Weight;
				mtBillGridRow = _mtBillList[i];
				result = _mtBillGrid.Grid.GetRowHandleByListIndex(i);
				break;
			}
			MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(materialCacheRow.Id, materialCacheRow.Unit);
			if (qntUnit2 == null)
			{
				break;
			}
			if (retailPrice == 0m)
			{
				MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(materialCacheRow.MTCode, qntUnit2.Coef, _storage.Code, _date.EditValue.Value);
				mTRetailQtyAndPrice.LoadData();
				if (mTRetailQtyAndPrice.RowsCount == 1)
				{
					num = decimal.Parse(mTRetailQtyAndPrice.CommentMember(0));
				}
				else if (mTRetailQtyAndPrice.RowsCount > 1)
				{
					ModalBrowser modalBrowser = new ModalBrowser(mTRetailQtyAndPrice)
					{
						Owner = this,
						WindowStartupLocation = WindowStartupLocation.CenterOwner
					};
					if (!modalBrowser.Show())
					{
						_inputValue.Text = "";
						_realText = "";
						ControlsHelper.SetFocus(_inputValue);
						return -1;
					}
					num = decimal.Parse(mTRetailQtyAndPrice.CommentMember(modalBrowser.SelectedRowIndex));
				}
			}
			else
			{
				num = retailPrice;
			}
			if (num == mTBillSpecificationRow2.Price)
			{
				mTBillSpecificationRow2.Quantity += _mQty_Weight;
				mtBillGridRow = _mtBillList[i];
				result = _mtBillGrid.Grid.GetRowHandleByListIndex(i);
				break;
			}
			int j = 0;
			for (int num2 = _mtBillList.Count - 1; j <= num2; j++)
			{
				MTBillSpecificationRow mTBillSpecificationRow3 = _mtBillList[j];
				if (mTBillSpecificationRow3.ItemID == materialCacheRow.Id && mTBillSpecificationRow3.UnitCode == materialCacheRow.Unit && mTBillSpecificationRow3.Price == num)
				{
					mTBillSpecificationRow3.Quantity += _mQty_Weight;
					mtBillGridRow = _mtBillList[j];
					result = _mtBillGrid.Grid.GetRowHandleByListIndex(j);
					break;
				}
			}
			if (mtBillGridRow == null)
			{
				mtBillGridRow = new MTBillSpecificationRow();
				FillMTBillGridRow(mtBillGridRow, materialCacheRow, materialCacheRow.Unit, num);
				MaterialCacheRow.BarCodeRow barCode2 = TradeSettings.TradeCacheService.Material.GetBarCode(materialCacheRow.Id, materialCacheRow.Unit);
				if (barCode2 != null)
				{
					mtBillGridRow.BarCode = barCode2.BarCode;
				}
				mtBillGridRow.RowID = _maxRowID;
				_mtBillList.Add(mtBillGridRow);
				_maxRowID++;
				result = _mtBillGrid.Grid.GetRowHandleByListIndex(_mtBillList.Count - 1);
			}
			break;
		}
		if (mtBillGridRow == null || mtBillGridRow.IsDeleted || mtBillGridRow.IsGift)
		{
			result = AddNewRowInner(materialCacheRow, value, ref mtBillGridRow, barCode, retailPrice, qntUnit);
		}
		return result;
	}

	private void AddNewMTFromFile(MaterialCacheRow materialCacheRow, string unitCode, decimal quantity)
	{
		int count = _mtBillList.Count;
		decimal price = 0m;
		if (!_retail)
		{
			price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, materialCacheRow.Id, unitCode, _date.EditValue.Value, _currCode.Code);
			price = Methods.RoundPrice(price, _priceListTypeRoundDgr);
		}
		else if (TradeSettings.TradeCacheService.Material.GetQntUnit(materialCacheRow.Id, unitCode) != null)
		{
			MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
			{
				MTCode = materialCacheRow.MTCode,
				Storage = _storage.Code,
				Date = _date.EditValue,
				UnitCoef = materialCacheRow.Coeff
			};
			List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
			if (rows.Count > 0)
			{
				price = rows[0].Price;
			}
		}
		MTBillSpecificationRow mtBillGridRow = AddMTToListNewRow(materialCacheRow, unitCode, quantity, price);
		if (_distributByFEFO && mtBillGridRow.ItemType == ItemTypesSE.Goods.Value)
		{
			DistributeQtyByExpiredDate(mtBillGridRow.RowID, mtBillGridRow.Storage, mtBillGridRow.ItemCode, mtBillGridRow.UnitCode, quantity, materialCacheRow.Unit);
			mtBillGridRow.IsPartySelected = true;
		}
		int rowHandleByListIndex = _mtBillGrid.Grid.GetRowHandleByListIndex(_mtBillList.Count - 1);
		bool flag = count != _mtBillList.Count;
		if (string.IsNullOrWhiteSpace(_priceListType) || (flag && !_retail))
		{
			CalculateGridRowDiscountPrice(ref mtBillGridRow);
			CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, mtBillGridRow.DiscountPrice == 0m);
		}
		else
		{
			CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, calcDiscountPrice: false);
		}
		POWindow<MTBill>.SelectCellInGrid(_mtBillTableView, _mtBillGrid, rowHandleByListIndex, "Quantity");
		_mtBillGrid.ConfigureButtons();
	}

	private MTBillSpecificationRow AddMTToListNewRow(MaterialCacheRow materialCacheRow, string unitCode, decimal quantity, decimal price)
	{
		MTBillSpecificationRow mTBillSpecificationRow = new MTBillSpecificationRow();
		FillMTBillGridRow(mTBillSpecificationRow, materialCacheRow, unitCode, price, quantity);
		MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(materialCacheRow.Id, unitCode);
		if (barCode != null)
		{
			mTBillSpecificationRow.BarCode = barCode.BarCode;
		}
		mTBillSpecificationRow.RowID = _maxRowID;
		_mtBillList.Add(mTBillSpecificationRow);
		_maxRowID++;
		return mTBillSpecificationRow;
	}

	private bool AddNewBuyer(string cardNumber)
	{
		bool result = false;
		if (!Settings.ParametersService.AllowToAddNewBuyerWithNotRegisteredBuyerCard() || !_isAllowChangeBuyerInBill)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidCardNotExist, cardNumber));
		}
		if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.AcceptToAddNewBuyerWithCardCode, cardNumber), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
		{
			return false;
		}
		DirectoryDocumentWindow<Partner> directoryDocumentWindow = ((!UseBuyerAddOrEditQuickMode) ? ((DirectoryDocumentWindow<Partner>)(PartnerDocumentWindow)Settings.DirectoryService.CreatePOWindow(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Partner)) : ((DirectoryDocumentWindow<Partner>)new PartnerDocumentQuickModeWindow((Partner)Settings.DirectoryService.Create(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Partner))));
		Partner.PartnerContractRow partnerContractRow = new Partner.PartnerContractRow();
		partnerContractRow.Contract = ArmSoft.SME.Common.Extensions.StringExtensions.NextDocNum(new string('0', 3), 3);
		partnerContractRow.DiscountCard = cardNumber;
		partnerContractRow.IsDefault = true;
		partnerContractRow.Caption = ArmSoft.SME.Trade.Common.Resources.DefaultContract;
		partnerContractRow.CurrCode = "AMD";
		directoryDocumentWindow.PO.PartnerContracts.Add(partnerContractRow);
		directoryDocumentWindow.Owner = Owner;
		directoryDocumentWindow.Top = Top;
		directoryDocumentWindow.Left = Left;
		if (directoryDocumentWindow.ShowDialog().Value)
		{
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(directoryDocumentWindow.PO.Code, out var partnerCacheRow);
			if (defaultContract != null)
			{
				if (defaultContract.IsBonusCard && !string.IsNullOrWhiteSpace(defaultContract.DiscountCard) && _connectTob2barmsoftDiscountServer)
				{
					DiscountServerService.CreateCardInfo(partnerCacheRow, defaultContract);
					TradeSettings.CreateTradeClient().CommonServices.EditDiscountServerData(partnerCacheRow.Id, defaultContract.Contract, exportedToDiscountServer: true);
				}
				DocumentHelpers.GetPartnerEmployeeDataByBarCode(cardNumber, out var partnerCacheRow2, out var partnerContract, out var employee, "AMD", hideExceptions: true);
				FillPartnerEmployeeData(partnerCacheRow2, partnerContract, employee);
				result = true;
			}
		}
		return result;
	}

	private void BarCodeEnterClickOnProperty1(string barCode)
	{
		string text = "";
		if (string.IsNullOrWhiteSpace(barCode))
		{
			return;
		}
		GoodsPartiesAvailibilityListByProperty1DataProvider.Param param = new GoodsPartiesAvailibilityListByProperty1DataProvider.Param
		{
			PartiesStorage = _storage.Code,
			PartiesProperty1Value = barCode,
			PartiesDate = _date.EditValue.Value
		};
		List<GoodsPartiesAvailibilityListByProperty1DataProvider.DataRow> rows = new GoodsPartiesAvailibilityListByProperty1DataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
		if (rows.Count == 0)
		{
			ControlsHelper.SetFocus(_inputValue);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NoAvailablePartiesWithBarcodeInProperty1, barCode));
		}
		if (rows.Count == 1)
		{
			GoodsPartiesAvailibilityListByProperty1DataProvider.DataRow row = rows.FirstOrDefault();
			AddGoodByProperty1ToMTExpDocSelectedPartiesListAndItemList(row);
			return;
		}
		GoodsPartiesAvailabilityListByProperty1 goodsPartiesAvailabilityListByProperty = new GoodsPartiesAvailabilityListByProperty1(_storage.Code, barCode, _date.EditValue.Value);
		ModalBrowser modalBrowser = new ModalBrowser(goodsPartiesAvailabilityListByProperty)
		{
			Owner = this,
			WindowStartupLocation = WindowStartupLocation.CenterOwner
		};
		if (modalBrowser.Show())
		{
			text = goodsPartiesAvailabilityListByProperty.ValueMember(modalBrowser.SelectedRowIndex);
			GoodsPartiesAvailibilityListByProperty1DataProvider.DataRow rowByValue = goodsPartiesAvailabilityListByProperty.GetRowByValue(text);
			if (!string.IsNullOrWhiteSpace(text))
			{
				AddGoodByProperty1ToMTExpDocSelectedPartiesListAndItemList(rowByValue);
			}
		}
		else
		{
			_inputValue.Text = "";
			_realText = "";
			ControlsHelper.SetFocus(_inputValue);
		}
	}

	private void AddGoodByProperty1ToMTExpDocSelectedPartiesListAndItemList(GoodsPartiesAvailibilityListByProperty1DataProvider.DataRow row)
	{
		bool flag = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(_storage.Code);
		PartyRow partyRow = new PartyRow
		{
			GoodID = row.GoodID,
			GridRow = _maxRowID,
			Base = row.PartyBase,
			Row = row.PartyRow,
			Qty = row.Quantity,
			ExpQty = 1m,
			Comment = row.Comment,
			Cost = row.Cost
		};
		if (partyRow.ExpQty == partyRow.Qty)
		{
			partyRow.ExpSumma = partyRow.Summa;
		}
		else if (!flag)
		{
			partyRow.ExpSumma = (partyRow.Cost * partyRow.ExpQty).RoundAs(2);
		}
		else if (partyRow.ExpQty < partyRow.Qty)
		{
			partyRow.ExpSumma = (partyRow.Cost * partyRow.ExpQty).RoundAs(2);
		}
		else
		{
			partyRow.ExpSumma = (partyRow.Cost * partyRow.Qty).RoundAs(2);
		}
		partyRow.IncomeDate = row.IncomeDate.Value;
		PO.MTExpDocSelectedPartiesList.Add(partyRow);
		AddItemToListByProperty1(row.GoodID);
	}

	private void AddItemToListByProperty1(int goodID)
	{
		MTBillSpecificationRow mtBillGridRow = null;
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(goodID);
		if (itemById != null && AddNewRowInner(itemById, "", ref mtBillGridRow, isBarCode: false) != -1)
		{
			_mtBillTableView.Grid.CurrentItem = mtBillGridRow;
			if (string.IsNullOrWhiteSpace(_priceListType) || !_retail)
			{
				CalculateGridRowDiscountPrice(ref mtBillGridRow);
				CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, mtBillGridRow.DiscountPrice == 0m);
			}
			else
			{
				CalculateGridRowSummaAndDiscountSumma(ref mtBillGridRow, calcDiscountPrice: false);
			}
			_mtBillGrid.ConfigureButtons();
			CalculateDocumentSumma();
			_inputValue.Text = "";
			_realText = "";
			ControlsHelper.SetFocus(_inputValue);
			ChangeMarkingStatus(mtBillGridRow);
			RowInfoChange?.Invoke(mtBillGridRow);
		}
	}

	private void BonusUse()
	{
		if ((string.IsNullOrWhiteSpace(_partner.Code) || !_partner.DoValidate()) && !_readOnlyAfterCheckPrint)
		{
			ControlsHelper.SetFocus(_partner);
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.CreditSaleBuyer);
		}
		if (_paramUseDiscount && _anyAutoDiscountModeIsEnabled && !PaymentIsInProcess())
		{
			CalculateDiscountAuto();
		}
		decimal num = _summa.Value - _summaCash.Value - _summaPlCard.Value;
		num -= _summaPrePayment.Value;
		decimal num2;
		if (!(_currCode.Code != "AMD"))
		{
			if (num > 0m)
			{
				num2 = ((!(_summaPayByBonus.Value != 0m)) ? (_summaChange.Value - _sumAddBonus) : _summaPayByBonus.Value);
			}
			else
			{
				num2 = 0m;
			}
		}
		else
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			if (num > 0m)
			{
				num2 = ((!(_summaPayByBonus.Value != 0m)) ? (_summaChange.Value * exchangeRates.Rate / exchangeRates.Base).RoundAs(2) : _summaPayByBonus.Value);
			}
			else
			{
				num2 = 0m;
			}
		}
		BonusUseDialog bonusUseDialog;
		if (_readOnlyAfterCheckPrint)
		{
			bonusUseDialog = new BonusUseDialog(this, _summaPayByBonus.Value, _summa.Value, _partner.Code, _contract, _readOnlyAfterCheckPrint, connectTob2barmsoftDiscountServer: false, _bonusUseMaxPercent);
		}
		else
		{
			decimal summaForPay = ((_bonusUseMaxPercent < 100m) ? (_summa.Value * _bonusUseMaxPercent / 100m).RoundAs(2) : num2);
			bonusUseDialog = new BonusUseDialog(this, summaForPay, _summa.Value, _partner.Code, _contract, _readOnlyAfterCheckPrint, _connectTob2barmsoftDiscountServer, _bonusUseMaxPercent);
		}
		if (!bonusUseDialog.ShowDialog().Value)
		{
			return;
		}
		decimal usedBonusSumm = bonusUseDialog.UsedBonusSumm;
		if (usedBonusSumm != 0m)
		{
			decimal value = _summa.Value;
			decimal num3 = 0m;
			foreach (MTBillSpecificationRow mtBill in _mtBillList)
			{
				if (!mtBill.IsDeleted && !mtBill.IsGift && value != 0m)
				{
					mtBill.UsedBonus = (mtBill.Summa / value * usedBonusSumm).RoundAs(2);
					if (_noCalcBonusesWhenUseBonus)
					{
						mtBill.AccumulatedBonus = 0m;
					}
					num3 += mtBill.UsedBonus;
				}
			}
			decimal num4 = usedBonusSumm - num3;
			if (num4 != 0m && CalcMTBillListRowCount(includeDeleted: false) > 0)
			{
				for (int num5 = _mtBillList.Count - 1; num5 >= 0; num5--)
				{
					if (!_mtBillList[num5].IsDeleted && !_mtBillList[num5].IsGift)
					{
						_mtBillList[_mtBillList.Count - 1].UsedBonus += num4;
						break;
					}
				}
			}
			((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Visible;
		}
		else
		{
			foreach (MTBillSpecificationRow mtBill2 in _mtBillList)
			{
				mtBill2.UsedBonus = 0m;
			}
			((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
		}
		_summaPayByBonus.Value = usedBonusSumm;
		UpdateChangeOrPaySum();
		MTBillSpecificationRow row = (MTBillSpecificationRow)_mtBillGrid.Grid.CurrentItem;
		RowInfoChange?.Invoke(row);
	}

	private decimal CalcBonuses(decimal saleSum = 0m)
	{
		decimal num = 0m;
		switch (_bonusAccumulationMethod)
		{
		case "1":
			foreach (MTBillSpecificationRow item2 in _mtBillList.Where((MTBillSpecificationRow r) => !r.IsDeleted && !r.IsGift))
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(item2.ItemCode);
				if (item == null)
				{
					continue;
				}
				bool flag = _calcBonusForDiscountedItems || item2.Discount == 0m;
				if (item.IgnoreBonus)
				{
					flag = false;
				}
				if (flag)
				{
					if (_bonusType == BonusTypeSE.ByPercent.Value)
					{
						item2.AccumulatedBonus = (item2.Summa * item.Bonus / 100m).RoundAs(2);
					}
					else
					{
						item2.AccumulatedBonus = (item2.Quantity * item.Point).RoundAs(2);
					}
					num += item2.AccumulatedBonus;
				}
				else
				{
					num -= item2.AccumulatedBonus;
					item2.AccumulatedBonus = 0m;
				}
			}
			break;
		case "2":
		{
			decimal num5 = 0m;
			num = TradeSettings.CreateTradeClient().CommonServices.CalculateBonusByScale(_bonusType, (saleSum > 0m) ? saleSum : _summa.Value);
			if (!(num != -1m))
			{
				break;
			}
			foreach (MTBillSpecificationRow item3 in _mtBillList.Where((MTBillSpecificationRow r) => !r.IsDeleted && !r.IsGift))
			{
				if ((_calcBonusForDiscountedItems || item3.Discount == 0m) && !TradeSettings.TradeCacheService.Material.IsBonusIgnoredForItem(item3.ItemCode))
				{
					if (_bonusType == BonusTypeSE.ByPercent.Value)
					{
						item3.AccumulatedBonus = (item3.Summa * num / 100m).RoundAs(2);
					}
					else
					{
						item3.AccumulatedBonus = (item3.Summa / _summa.Value * num).RoundAs(2);
						num5 += item3.AccumulatedBonus;
					}
				}
				else if (_bonusType == BonusTypeSE.ByPercent.Value)
				{
					item3.AccumulatedBonus = 0m;
				}
				else
				{
					num5 -= item3.AccumulatedBonus;
					item3.AccumulatedBonus = 0m;
				}
				if (!(_bonusType != BonusTypeSE.ByPercent.Value))
				{
					continue;
				}
				decimal num6 = num - num5;
				if (!(num6 != 0m) || _mtBillList.Count <= 0)
				{
					continue;
				}
				for (int num7 = 0; num7 <= _mtBillList.Count - 1; num7++)
				{
					if (!_mtBillList[num7].IsDeleted && !_mtBillList[num7].IsGift && _mtBillList[num7].AccumulatedBonus > 0m)
					{
						_mtBillList[_mtBillList.Count - 1].AccumulatedBonus += num6;
					}
				}
			}
			num = _mtBillList.Where((MTBillSpecificationRow r) => !r.IsDeleted && !r.IsGift).Sum((MTBillSpecificationRow r) => r.AccumulatedBonus);
			break;
		}
		case "3":
			if (_bonusType == BonusTypeSE.ByPercent.Value)
			{
				foreach (MTBillSpecificationRow item4 in _mtBillList.Where((MTBillSpecificationRow r) => !r.IsDeleted && !r.IsGift))
				{
					bool flag = _calcBonusForDiscountedItems || item4.Discount == 0m;
					if (TradeSettings.TradeCacheService.Material.IsBonusIgnoredForItem(item4.ItemCode))
					{
						flag = false;
					}
					if (flag)
					{
						item4.AccumulatedBonus = (item4.Summa * _partContractBonusPercentOrPoint / 100m).RoundAs(2);
						num += item4.AccumulatedBonus;
					}
					else
					{
						num -= item4.AccumulatedBonus;
						item4.AccumulatedBonus = 0m;
					}
				}
			}
			else
			{
				if (!(_partContractBonusPercentOrPoint != 0m))
				{
					break;
				}
				decimal num2 = 0m;
				decimal num3 = 0m;
				decimal value = _summa.Value;
				foreach (MTBillSpecificationRow mtBill in _mtBillList)
				{
					bool flag = _calcBonusForDiscountedItems || mtBill.Discount == 0m;
					if (TradeSettings.TradeCacheService.Material.IsBonusIgnoredForItem(mtBill.ItemCode))
					{
						flag = false;
					}
					if (flag)
					{
						if (value != 0m)
						{
							mtBill.AccumulatedBonus = (mtBill.Summa / value * _partContractBonusPercentOrPoint).RoundAs(2);
							num3 += mtBill.AccumulatedBonus;
						}
					}
					else if (value != 0m)
					{
						num -= mtBill.AccumulatedBonus;
						mtBill.AccumulatedBonus = 0m;
					}
				}
				num2 = _partContractBonusPercentOrPoint - num3;
				if (!(num2 != 0m) || CalcMTBillListRowCount(includeDeleted: false) <= 0)
				{
					break;
				}
				for (int num4 = _mtBillList.Count - 1; num4 >= 0; num4--)
				{
					if (!_mtBillList[num4].IsDeleted && !_mtBillList[num4].IsGift && _mtBillList[num4].AccumulatedBonus > 0m)
					{
						_mtBillList[_mtBillList.Count - 1].AccumulatedBonus += num2;
						break;
					}
				}
			}
			break;
		}
		return num;
	}

	private void AutoCalcDiscountAdjustControlsAccessibility(bool OnlyGridButtons = false)
	{
		bool flag = PaymentIsInProcess();
		_increase.IsEnabled = !flag;
		_decrease.IsEnabled = !flag;
		if (_weight != null)
		{
			_weight.IsEnabled = !flag;
		}
		if (_complect != null)
		{
			_complect.IsEnabled = !flag;
		}
		if (_discount != null)
		{
			_discount.IsEnabled = !flag;
		}
		if (_calcBonus != null)
		{
			_calcBonus.IsEnabled = !flag;
		}
		if (_complect != null)
		{
			_complect.IsEnabled = !flag;
		}
		if (_prices != null)
		{
			_prices.IsEnabled = !flag;
		}
		if (_filteredSearch != null)
		{
			_filteredSearch.IsEnabled = !flag;
		}
		if (_mtFavorites != null)
		{
			_mtFavorites.IsEnabled = !flag;
		}
		if (_substituteItems != null)
		{
			_substituteItems.IsEnabled = !flag;
		}
		if (_eMarkReceipt != null)
		{
			_eMarkReceipt.IsEnabled = !flag;
		}
		if (_actualQuantitiesCheck != null)
		{
			_actualQuantitiesCheck.IsEnabled = !flag;
		}
		if (OnlyGridButtons)
		{
			return;
		}
		_mtBillGrid.AllowAdd = !flag && _isAllowChangeGoodRowInBill;
		_mtBillGrid.AllowEdit = !flag && _isAllowChangeGoodRowInBill;
		_mtBillGrid.AllowDuplicate = !flag && _isAllowChangeGoodRowInBill;
		_mtBillGrid.AllowDelete = !flag;
		_mtBillGrid.AllowMoveUp = !flag;
		_mtBillGrid.AllowMoveDown = !flag;
		foreach (GridColumn item in _mtBillGrid.Grid.Columns.Except(new DocumentGridColumn[3] { _discountCol, _discountPriceCol, _summaCol }))
		{
			item.ReadOnly = flag || !_isAllowChangeGoodRowInBill;
		}
		_date.IsReadOnly = flag || !_allowChangeDate;
		_storage.IsReadOnly = flag || !_isAllowChangeStorageInBill;
		_cashDesk.IsReadOnly = flag || !_isAllowChangeCashInBill;
		_partner.IsReadOnly = flag || !_isAllowChangeBuyerInBill || !string.IsNullOrWhiteSpace(_ecrCheckNum.Text);
		_salesConsultant.IsReadOnly = flag || !_isAllowChangeSaleConsultantInBill;
		_owner.IsReadOnly = flag;
		_barCodeEnter.IsEnabled = !flag;
		_codeEnter.IsEnabled = !flag;
		_mtListLikeName.IsEnabled = !flag;
		_qtyEnter.IsEnabled = !flag;
		if (_salePrice != null)
		{
			_salePrice.IsEnabled = !flag;
		}
	}

	private bool PaymentIsInProcess()
	{
		return PaidSum() > 0m;
	}

	private bool IsUnderpaid()
	{
		return _summa.Value > PaidSum();
	}

	private decimal PaidSum()
	{
		return _summaCash.Value + _summaCredit.Value + _summaPayByBonus.Value + _summaPlCard.Value - _sumAddBonus + _summaPrePayment.Value;
	}

	private decimal RemainSumForCreditPayment()
	{
		decimal num = _summa.Value - _summaCash.Value - _summaPayByBonus.Value - _summaPlCard.Value - _summaPrePayment.Value;
		if (!(num <= 0m))
		{
			return num;
		}
		return 0m;
	}

	private decimal RemainSumForCardPayment()
	{
		decimal num = _summa.Value - _summaCash.Value - _summaPayByBonus.Value - _summaCredit.Value - _summaPrePayment.Value;
		if (!(num <= 0m))
		{
			return num;
		}
		return 0m;
	}

	private decimal RemainSumForCashPayment()
	{
		decimal num = _summa.Value - _summaCredit.Value - _summaPayByBonus.Value - _summaPlCard.Value + _sumAddBonus - _summaPrePayment.Value;
		if (!(num <= 0m))
		{
			return num;
		}
		return 0m;
	}

	private decimal RemainSumForPrePayment()
	{
		decimal num = _summa.Value - _summaCash.Value - _summaPayByBonus.Value - _summaPlCard.Value - _summaCredit.Value;
		if (!(num <= 0m))
		{
			return num;
		}
		return 0m;
	}

	private void OnRowInfoChange(MTBillSpecificationRow row)
	{
		if (_isShowInfoPanel || _isShowPoleDisplay)
		{
			ChangeRowInfo(row);
		}
	}

	private void ChangeRowInfo(MTBillSpecificationRow row = null)
	{
		if (row == null)
		{
			if (_isShowInfoPanel)
			{
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearInfoPanel(CleanInfoPanel), Array.Empty<object>());
			}
			if (_isShowPoleDisplay && TradeSettings.PoleDisplay != null)
			{
				ClearPoleDisplay(string.IsNullOrWhiteSpace(_partCode));
				if (CalcMTBillListRowCount(includeDeleted: false) != 0)
				{
					SetTotalInfoToPoleDisplay();
				}
			}
		}
		else
		{
			if (_isShowInfoPanel)
			{
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToInfoPanel(SetRowInfoToPanel), new object[1] { row });
			}
			if (_isShowPoleDisplay && TradeSettings.PoleDisplay != null)
			{
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToPoleDisplay(TradeSettings.PoleDisplay.SetRowInfo), new object[1] { row });
			}
		}
	}

	private void SetRowInfoToPanel(MTBillSpecificationRow row)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(row.ItemName))
			{
				CleanInfoPanel();
				return;
			}
			short num = 2;
			_isChanged = IsChanged;
			decimal num2 = (row.Quantity * row.DiscountPrice).RoundAs(2);
			_infoItemName.Text = row.ItemName;
			TextEditExt infoFormula = _infoFormula;
			InlineArray4<object> buffer = default;
			buffer[0] = row.Quantity.CurrencyFormat(16, (short)((!(row.Quantity == decimal.Floor(row.Quantity))) ? 3 : 0));
			buffer[1] = row.UnitBrief;
			buffer[2] = row.DiscountPrice.CurrencyFormat(16, (short)((!(row.DiscountPrice == decimal.Floor(row.DiscountPrice))) ? num : 0));
			buffer[3] = num2.CurrencyFormat(16, (short)((!(num2 == decimal.Floor(num2))) ? num : 0));
			infoFormula.Text = string.Format("{0} {1} x {2} = {3}", (ReadOnlySpan<object?>)buffer);
			if (Settings.ParametersService.EnableSubstituteItems())
			{
				if (TradeSettings.TradeCacheService.Material.HasSubstitution(row.ItemCode))
				{
					_substitutionInfo.Text = ArmSoft.SME.Trade.Common.Resources.SubstitutionsExists;
					((LayoutGroup)(object)_hintImage.Parent).Visibility = Visibility.Visible;
				}
				else
				{
					_substitutionInfo.Text = "";
					((LayoutGroup)(object)_hintImage.Parent).Visibility = Visibility.Collapsed;
				}
			}
			if ((_isShowStorageAvailabilityOnInfoPanel || _isShowWholeAvailabilityOnInfoPanel) && !IsOfflineDoc)
			{
				SetAvailabilityInfoToPanel(row);
			}
			SetDescriptionInfoPanel(row);
			if (_isShowItemPictureOnInfoPanel)
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(row.ItemID, row.UnitCode);
				if (qntUnit != null && qntUnit.ImageGUID.HasValue)
				{
					if (!_images.TryGetValue(qntUnit.ImageGUID.Value, out var value))
					{
						value = Settings.ImagesService.GetItemImage(ArmSoft.SME.Trade.Common.SmartEnums.ImageItemTypeSE.Materials.Value, qntUnit.ImageGUID.ToString());
						_images.Add(qntUnit.ImageGUID.Value, value);
					}
					_itemImage.EditValue = value;
					_itemImage.UpdateLayout();
				}
			}
			IsChanged = _isChanged;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SetAvailabilityInfoToPanel(MTBillSpecificationRow row)
	{
		_infoDisplayControls.ClearInfoAvailability();
		if (row.ItemType == GoodsTypesSE.Good.Value)
		{
			if (_isShowStorageAvailabilityOnInfoPanel)
			{
				_infoDisplayControls.SetAccBalance(MaterialHelpers.GetGoodRemains(row.UpdateRemainsOnInfoPanel, row.ItemID, row.UnitCode, ref _availabilityByGoodId, row.Storage));
				_infoDisplayControls.SetAccUnitBrief(row.UnitBrief);
			}
			if (_isShowWholeAvailabilityOnInfoPanel)
			{
				_infoDisplayControls.SetWholeBalance(MaterialHelpers.GetGoodRemains(row.UpdateRemainsOnInfoPanel, row.ItemID, row.UnitCode, ref _availabilityByGoodId));
				_infoDisplayControls.SetWholeUnitBrief(row.UnitBrief);
			}
		}
		row.UpdateRemainsOnInfoPanel = false;
	}

	private void SetDescriptionInfoPanel(MTBillSpecificationRow row)
	{
		if (Settings.ParametersService.ShowDescriptionOnInfoPanel())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(row.ItemCode);
			_infoDescription.Text = ((item == null) ? "" : item.Description);
		}
	}

	private void CleanInfoPanel()
	{
		_infoItemName.Text = "";
		_infoFormula.Text = "";
		if (_substitutionInfo != null)
		{
			_substitutionInfo.Text = "";
		}
		_infoDisplayControls.ClearInfoAvailability();
		if (_infoDescription != null)
		{
			_infoDescription.Text = "";
		}
		if (_itemImage != null)
		{
			_itemImage.EditValue = null;
			_itemImage.UpdateLayout();
		}
	}

	private void SetTotalInfoToPoleDisplay()
	{
		if (TradeSettings.PoleDisplay != null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetTotalInfoToPoleDisplay(TradeSettings.PoleDisplay.SetTotalInfo), new object[8]
			{
				_summa.Value,
				_tmpDiscountSumm,
				_summaCash.Value,
				_summaPlCard.Value,
				_summaPayByBonus.Value,
				_summaPrePayment.Value,
				_summaCredit.Value,
				PaidSum() - _summa.Value
			});
		}
	}

	private void SetRowCountInfoToPoleDisplay(int rowCount = 0)
	{
		if (TradeSettings.PoleDisplay != null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowCountInfoToPoleDisplay(TradeSettings.PoleDisplay.SetRowCountInfo), new object[1] { rowCount });
		}
	}

	private void InitPoleDisplay(bool onEdit = false)
	{
		if (_isShowPoleDisplay)
		{
			if (TradeSettings.PoleDisplay == null || PresentationSource.FromVisual(TradeSettings.PoleDisplay) == null)
			{
				TradeSettings.PoleDisplay = new PoleDisplayWindow();
			}
			else if (onEdit)
			{
				SetTotalInfoToPoleDisplay();
			}
			else
			{
				ClearPoleDisplay(clearAll: true);
			}
			TradeSettings.PoleDisplay.Show();
		}
	}

	private void ClearPoleDisplay(bool clearAll)
	{
		if (TradeSettings.PoleDisplay != null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearPoleDisplay(TradeSettings.PoleDisplay.ClearScreen), new object[2] { clearAll, "" });
		}
	}

	private void SetBuyerInfoToPoleDisplay()
	{
		if (TradeSettings.PoleDisplay == null)
		{
			return;
		}
		decimal num = 0m;
		decimal num2 = 0m;
		if (_allowUseBonus)
		{
			num = _mtBillList.Sum((MTBillSpecificationRow row) => row.AccumulatedBonus).RoundAs(2);
			if (!string.IsNullOrWhiteSpace(_contract))
			{
				decimal num3 = 0m;
				if (_connectTob2barmsoftDiscountServer)
				{
					PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partCode, _contract);
					if (contract != null)
					{
						num3 = DiscountServerService.GetCardBalance(contract.DiscountCard);
					}
				}
				else
				{
					TradeSettings.TradeCacheService.Partner.GetContract(_partCode, _contract, out var partnerCacheRow);
					num3 = TradeSettings.CreateTradeClient().CommonServices.GetAccRem(CommonConst.AccountingsTypes.Bonus, _contract, partnerCacheRow.Id, _currCode.Code, _date.EditValue.Value).SummNcc;
				}
				num2 = num3;
			}
		}
		string text = "";
		if (!string.IsNullOrWhiteSpace(_partCode))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partCode);
			if (item != null)
			{
				text = item.Caption;
			}
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetBuyerInfoToPoleDisplay(TradeSettings.PoleDisplay.SetBuyerInfo), new object[5] { _partCode, text, _contract, num, num2 });
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
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		BarSubItem barSubItem = new BarSubItem();
		_barButtonJumpTo = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref barSubItem);
		_barButtonPrintLabel = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.PrintLabel,
			KeyGesture = new KeyGesture((Key)55, (ModifierKeys)2),
			Glyph = ImageService.GetImage("action_16.svg"),
			LargeGlyph = ImageService.GetImage("action_32.svg")
		};
		_barButtonPrintLabel.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonPrintLabel_ItemClick, this).Handler;
		_barManager.Items.Add(_barButtonPrintLabel);
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonPrintLabel.CreateLink());
		Ribbon.ToolbarItemLinks.Add(_barButtonPrintLabel.CreateLink());
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
		_barButtonDocumentSettings = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.Settings,
			Glyph = ImageService.GetImage("page_white_gear_16.svg"),
			LargeGlyph = ImageService.GetImage("page_white_gear_32.svg")
		};
		_barButtonDocumentSettings.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonDocumentSettings_ItemClick, this).Handler;
		_barManager.Items.Add(_barButtonDocumentSettings);
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonDocumentSettings.CreateLink());
		Ribbon.ToolbarItemLinks.Add(_barButtonDocumentSettings.CreateLink());
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("report_16.svg");
		_barButtonMTSalesAnalysis = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepSalesAnalysis.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis);
		_barButtonMTSalesAnalysis.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonGoodsOperationsJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperations);
		_barButtonGoodsOperationsJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonGoodsRetailOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepOperationsRetail.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperationsRetail);
		_barButtonGoodsRetailOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonPartnerOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		_barButtonPartnerOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonBonusOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.BonusOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.BonusOp);
		_barButtonBonusOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonCashOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.CashOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.KassaOp);
		_barButtonCashOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		if (_connectTob2barmsoftDiscountServer)
		{
			_barStatusConnectToDS = new BarStaticItem
			{
				ItemMinWidth = 165.0,
				AutoSizeMode = BarItemAutoSizeMode.Fill,
				ContentAlignment = HorizontalAlignment.Center,
				ContentTemplate = GetStatusBarContentTemplate()
			};
			_barManager.Items.Add(_barStatusConnectToDS);
			_ribbonStatusBar.RightItemLinks.AddWithSeparator(_barStatusConnectToDS.CreateLink());
		}
	}

	protected override void SetReportBarButtonsProperties()
	{
		base.SetReportBarButtonsProperties();
		_barButtonJumpTo.PropertyWithValues = new List<PropertyWithValue>
		{
			new PropertyWithValue("DocTypeParam", PO.POType.ToString()),
			new PropertyWithValue("StartDate", PO.DocDate.ToString()),
			new PropertyWithValue("EndDate", PO.DocDate.ToString()),
			new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString())
		};
		_barButtonMTSalesAnalysis.PropertyWithValues = new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", PO.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption()),
			new PropertyWithValue("ShowNCCVATSumm", true.ToString())
		};
		_barButtonGoodsOperationsJournal.PropertyWithValues = new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", PO.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption())
		};
		_barButtonGoodsRetailOperationJournal.PropertyWithValues = new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", PO.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption())
		};
		_barButtonPartnerOperationJournal.PropertyWithValues = new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", PO.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption()),
			new PropertyWithValue("ShowLinks", true.ToString())
		};
		_barButtonBonusOperationJournal.PropertyWithValues = new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", PO.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption())
		};
		_barButtonCashOperationJournal.PropertyWithValues = new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", PO.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption())
		};
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
		_barShowDiscCalcSteps.CheckedChanged += new SafeDocumentEventHandler<ItemClickEventArgs>(BarShowDiscCalcSteps_CheckedChanged, this).Handler;
		_ribbonPageDiscounts = new RibbonPageGroup
		{
			Caption = ArmSoft.SME.Trade.Common.Resources.Discounts
		};
		_ribbonPageDiscounts.ItemLinks.Add(_barShowDiscCalcSteps.CreateLink());
		AddNewItemOnRibbonPageHome(_ribbonPageDiscounts);
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonMTSalesAnalysis.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
		_barButtonGoodsRetailOperationJournal.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
		_barButtonBonusOperationJournal.IsEnabled = isEnabled;
		_barButtonCashOperationJournal.IsEnabled = isEnabled;
	}

	public DataTemplate GetStatusBarContentTemplate()
	{
		return (DataTemplate)Application.Current.FindResource("MyStatusBarContentTemplate");
	}

	private void BarButtonDocumentSettings_ItemClick(object sender, ItemClickEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Common.Resources.Settings,
			Owner = this
		};
		LayoutGroupExt layoutGroupExt = dialogWindow.AddHorizontalLayoutGroup();
		layoutGroupExt.Orientation = Orientation.Vertical;
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt.Header = ArmSoft.SME.Trade.Common.Resources.MaterialsAndServices;
		layoutGroupExt.View = LayoutGroupView.GroupBox;
		LayoutGroupExt layoutGroupExt2 = dialogWindow.AddHorizontalLayoutGroup();
		layoutGroupExt2.Orientation = Orientation.Vertical;
		layoutGroupExt2.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt2.Header = ArmSoft.SME.Common.Resources.Partners;
		layoutGroupExt2.View = LayoutGroupView.GroupBox;
		CheckEditExt checkEditExt = dialogWindow.AddBooleanControl(layoutGroupExt, "ApplyLastPrice", ArmSoft.SME.Trade.Common.Resources.ApplyLastPrice);
		checkEditExt.IsChecked = _applyLastPrice;
		CheckEditExt checkEditExt2 = dialogWindow.AddBooleanControl(layoutGroupExt, "ImmediatlyEditRow", ArmSoft.SME.Trade.Common.Resources.ImmediatlyEditRow);
		checkEditExt2.IsChecked = _immediatlyEditRow;
		CheckEditExt checkEditExt3 = dialogWindow.AddBooleanControl(layoutGroupExt, "SearchBarCodeInProperty1", ArmSoft.SME.Trade.Common.Resources.SearchBarCodeInPartiesWithProperty1);
		checkEditExt3.IsChecked = _searchBarCodeInProperty1;
		CheckEditExt checkEditExt4 = dialogWindow.AddBooleanControl(layoutGroupExt, "DistributByFEFO", ArmSoft.SME.Trade.Common.Resources.FEFODistibuteLong);
		checkEditExt4.IsChecked = _distributByFEFO;
		CheckEditExt checkEditExt5 = dialogWindow.AddBooleanControl(layoutGroupExt2, "bUseBuyerAddOrEditQuickMode", ArmSoft.SME.Trade.Common.Resources.UseBuyerAddOrEditQuickMode);
		checkEditExt5.IsChecked = UseBuyerAddOrEditQuickMode;
		if (dialogWindow.ShowDialog().Value)
		{
			_applyLastPrice = checkEditExt.IsChecked.Value;
			_searchBarCodeInProperty1 = checkEditExt3.IsChecked.Value;
			_immediatlyEditRow = checkEditExt2.IsChecked.Value;
			_distributByFEFO = checkEditExt4.IsChecked.Value;
			UseBuyerAddOrEditQuickMode = checkEditExt5.IsChecked.Value;
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
			if (_mtBillList.Count <= 0 || ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.LoadMaterialsList, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.No)
			{
				_mtBillList.Clear();
				PO.MTExpDocSelectedPartiesList.Clear();
				_maxRowID = 0;
				TerminalHelper.TerminalImporter terminalImporter = new TerminalHelper.TerminalImporter(this);
				if (terminalImporter.ImportFromFile((decimal quantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow) =>
				{
					AddNewMTFromFile(goodRow, goodBarcodeRow.QntUnit, quantity);
				}))
				{
					CalculateDocumentSumma();
					terminalImporter.ShowReport();
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void BarShowDiscCalcSteps_CheckedChanged(object sender, ItemClickEventArgs e)
	{
		_showDiscCalcSteps = _barShowDiscCalcSteps.IsChecked.Value;
		if (_showDiscCalcSteps)
		{
			_lgDiscountLogs.Visibility = Visibility.Visible;
		}
		else
		{
			_lgDiscountLogs.Visibility = Visibility.Collapsed;
		}
	}

	protected void BarButtonPrintLabel_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (_mtBillList.Count > 0)
		{
			FillDocument(new StoreRequest());
			new PrintLabelDialog(this, PO).Invoke();
		}
	}
}
