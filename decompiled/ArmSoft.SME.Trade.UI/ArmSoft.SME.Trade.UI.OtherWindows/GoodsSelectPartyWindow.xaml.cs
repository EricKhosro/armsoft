using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Data.DropDowns;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Documents;
using ArmSoft.SME.Trade.UI.Documents.Goods;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public partial class GoodsSelectPartyWindow : ThemedWindow, IComponentConnector
{
	private string _expMethod = "";

	private DocumentGridControl _partiesGrid;

	private TableView _partiesTableView;

	private List<PartyRow> _mtCodeSelectedPartiesList;

	private readonly List<PartyRow> _mtExpDocSelectedPartiesList;

	private Guid _partyBase;

	private int _partyRowId;

	private const string _registryKey = "GoodsSelectPartyWindow";

	private const double _defaultWidth = 796.0;

	private const double _defaultHeight = 530.0;

	private DropDownControl _itemType;

	private DropDownControl _group;

	private DropDownControl _storage;

	private DropDownControl _itemCode;

	private TextEditExt _itemName;

	private DropDownControl _unit;

	private TextEditExt _rowDesc;

	private PopupCalcEditExt _quantity;

	private PopupCalcEditExt _initPrice;

	private PopupCalcEditExt _discountPercent;

	private PopupCalcEditExt _discountPrice;

	private PopupCalcEditExt _summa;

	private PopupCalcEditExt _envFeePercent;

	private PopupCalcEditExt _envFeeSumma;

	private DropDownControl _dealType;

	private PopupCalcEditExt _salePrice;

	private PopupCalcEditExt _saleSumma;

	private PopupCalcEditExt _usedBonus;

	private PopupCalcEditExt _accumulatedBonus;

	private CheckEditExt _vat;

	private CheckEditExt _party;

	private LayoutItemWithRequired _liParty;

	private Button _fefo;

	private DocumentGridColumn _availableQuantityCol;

	private DocumentGridColumn _expQuantityCol;

	private readonly DialogWindowMode _mode;

	private readonly DateOnly _docDate;

	private readonly string _docStorage;

	private readonly string _currCode = "AMD";

	private readonly decimal _docDiscountPercent;

	private readonly string _discountPriceListType = "";

	private readonly string _discountPriceTypeRoundDegree = "";

	private readonly string _discountPriceListVATType = "";

	private readonly string _discountPriceListCurr = "";

	private readonly decimal _discountPriceListCurrRate = 1m;

	private readonly decimal _discountPriceListCurrBase = 1m;

	private readonly decimal _docCurrRate = 1m;

	private readonly decimal _docCurrBase = 1m;

	private int _maxRowId;

	private int _rowId;

	private string _oldItemCode = "";

	private readonly string _complectCode = "";

	private readonly short _docType;

	private readonly DocumentGridControl _mtDocGrid;

	private bool _isBaseUnit;

	private readonly bool _showPartySUPPLIER = Settings.ParametersService.ShowPartySUPPLIER();

	private readonly bool _showPartyCONTRACT = Settings.ParametersService.ShowPartyCONTRACT();

	private readonly bool _showPartyUSEABLEDATE = Settings.ParametersService.ShowPartyUSEABLEDATE();

	private readonly bool _showPartyCountry = Settings.ParametersService.ShowPartyCountry();

	private readonly bool _showPartyProducer = Settings.ParametersService.ShowPartyProducer();

	private readonly bool _showPartyProperty1 = Settings.ParametersService.ShowPartyProperty1();

	private readonly bool _showPartyProperty2 = Settings.ParametersService.ShowPartyProperty2();

	private readonly bool _showCost = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();

	private bool _existPriceSummControls;

	private bool _storageEditable;

	private bool _storageRetail;

	private bool _storageAllowNegExp;

	private readonly bool _showInPriceList = Settings.ParametersService.ShowMTInclInPrList();

	private bool _existDiscountPercentControl;

	private bool _existDiscountPriceControl;

	private bool _existDropDownClickForPriceControl;

	private bool _existDropDownClickForSalePriceControl;

	private bool _existBonusControl;

	private readonly bool _storedInDbAndAlsoNotIsDraft;

	private bool _allowAddRowAndNotViewMode;

	private bool _docTypeHasDiscount;

	private bool _editableMode;

	private bool _changedQuantityFromGrid;

	private readonly bool _sourceRowIsNull;

	private readonly bool _vatEnabled;

	private readonly string _vatType = "";

	private readonly string _docDealType = "";

	private readonly bool _useEnvFees;

	private string _iItemTypeCode;

	private readonly bool _useDiscount;

	private readonly bool _calcDiscountAuto;

	private readonly decimal _usedDiscount;

	private readonly bool _allowChangeSalePrices;

	private readonly bool _showDiscountPercentColInBill = true;

	private readonly bool _showDiscountedPriceColInBill = true;

	private readonly bool _showVATColInBill = true;

	private readonly bool _showPartyColInBill = true;

	private readonly bool _showStorageColInBill = true;

	private readonly bool _showAccumulatedBonusColInMTBill = true;

	private readonly bool _showUsedBonusColInMTBill = true;

	private readonly bool _useBonus;

	private readonly string _bonusType = Settings.ParametersService.BonusType();

	private readonly bool _distrByFEFO;

	private readonly string _priceListType = "";

	private readonly string _priceListTypeRoundDgr = "";

	private readonly bool _storageIncomeRetail;

	private readonly MTBillWindow _mtBill;

	private readonly ObservableCollection<MTBillWindow.MTBillSpecificationRow> _mtBillList;

	private readonly MTBillWindow.MTBillSpecificationRow _mtBillSourceRow;

	private readonly MTInvoiceWindow _mtInvoice;

	private readonly ObservableCollection<MTInvoiceWindow.MTInvoiceSpecificationRow> _mtInvoicesList;

	private readonly MTInvoiceWindow.MTInvoiceSpecificationRow _mtInvoiceSourceRow;

	private readonly MTInvoiceRetailWindow _mtInvoiceRetail;

	private readonly ObservableCollection<MTInvoiceRetailWindow.MTInvoiceRetailSpecificationRow> _mtInvoiceRetailList;

	private readonly MTInvoiceRetailWindow.MTInvoiceRetailSpecificationRow _mtInvoiceRetailSourceRow;

	private readonly MTMoveWindow _mtMove;

	private readonly ObservableCollection<MTMoveWindow.MTMoveSpecificationRow> _mtMoveList;

	private readonly MTMoveWindow.MTMoveSpecificationRow _mtMoveSourceRow;

	private readonly MTMoveWithConfirmationWindow _mtMoveWithConfirmation;

	private readonly ObservableCollection<MTMoveWithConfirmationWindow.MTMoveSpecificationRow> _mtMoveWithConfirmationList;

	private readonly MTMoveWithConfirmationWindow.MTMoveSpecificationRow _mtMoveWithConfirmationSourceRow;

	private readonly MTExpenseWindow _mtExpense;

	private readonly ObservableCollection<MTExpenseWindow.MTExpenseSpecificationRow> _mtExpenseList;

	private readonly MTExpenseWindow.MTExpenseSpecificationRow _mtExpenseSourceRow;

	private readonly MTComplectationWindow _mtComplectation;

	private readonly ObservableCollection<MTComplectationWindow.MTComplectationSpecificationRow> _mtComplectationList;

	private readonly MTComplectationWindow.MTComplectationSpecificationRow _mtComplectationSourceRow;

	private readonly MTReturnToSupplierWindow _mtReturnToSupplier;

	private readonly ObservableCollection<MTReturnToSupplierWindow.MTRetToSupSpecificationRow> _mtReturnToSupplierList;

	private readonly MTReturnToSupplierWindow.MTRetToSupSpecificationRow _mtReturnToSupplierSourceRow;

	private readonly bool _allowChangeDiscountPrices = Settings.ParametersService.AllowChangeDiscountPrice();

	public int AddedRows { get; set; }

	public GoodsSelectPartyWindow(DialogWindowMode mode, MTBillWindow mtBillWindow, MTBillWindow.MTBillSpecificationRow sourceRow = null)
	{
		InitializeComponent();
		Owner = mtBillWindow;
		WindowStartupLocation = WindowStartupLocation.CenterOwner;
		_mode = mode;
		_mtBill = mtBillWindow;
		_docDate = mtBillWindow._date.EditValue.Value;
		_docStorage = mtBillWindow._storage.Code;
		_storageRetail = mtBillWindow._retail;
		_priceListType = mtBillWindow._priceListType;
		if (!_storageRetail)
		{
			_priceListTypeRoundDgr = TradeSettings.TradeCacheService.PriceType.GetItem(_priceListType)?.Round ?? "";
		}
		_docDiscountPercent = mtBillWindow._discountPercent.Value;
		_discountPriceListType = mtBillWindow._discountPriceListType;
		_discountPriceTypeRoundDegree = mtBillWindow._discountPriceTypeRoundDegree;
		_discountPriceListVATType = mtBillWindow._discountPriceListVATType;
		_discountPriceListCurr = mtBillWindow._discountPriceListCurr;
		_discountPriceListCurrRate = mtBillWindow._discountPriceListCurrRate;
		_discountPriceListCurrBase = mtBillWindow._discountPriceListCurrBase;
		_docCurrRate = mtBillWindow._docCurrRate;
		_docCurrBase = mtBillWindow._docCurrBase;
		_vatEnabled = true;
		_useDiscount = Settings.ParametersService.DiscountUse();
		_calcDiscountAuto = Settings.ParametersService.CalcDiscountAuto();
		_usedDiscount = mtBillWindow._usedDiscount;
		_allowChangeSalePrices = Settings.ParametersService.AllowChangeSalePrices();
		_showDiscountPercentColInBill = Settings.ParametersService.ShowDiscountPercentColInBill();
		_showDiscountedPriceColInBill = Settings.ParametersService.ShowDiscountedPriceColInBill();
		_showVATColInBill = Settings.ParametersService.ShowVATColInBill();
		_showPartyColInBill = Settings.ParametersService.ShowPartyColInBill();
		_showStorageColInBill = Settings.ParametersService.MTBillAllowDifferentStorages();
		_distrByFEFO = mtBillWindow._distributByFEFO;
		_useBonus = Settings.ParametersService.UseBonus();
		_showAccumulatedBonusColInMTBill = Settings.ParametersService.ShowAccumulatedBonusColInBill();
		_showUsedBonusColInMTBill = Settings.ParametersService.ShowUsedBonusColInBill();
		_vatType = (string.IsNullOrWhiteSpace(mtBillWindow._vatType) ? "5" : mtBillWindow._vatType);
		_currCode = "AMD";
		_storedInDbAndAlsoNotIsDraft = mtBillWindow.PO.StoredInDb && mtBillWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && mtBillWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		_maxRowId = mtBillWindow._maxRowID;
		_mtBillList = mtBillWindow._mtBillList;
		_mtDocGrid = mtBillWindow._mtBillGrid;
		_mtExpDocSelectedPartiesList = new List<PartyRow>();
		foreach (PartyRow mTExpDocSelectedParties in mtBillWindow.PO.MTExpDocSelectedPartiesList)
		{
			_mtExpDocSelectedPartiesList.Add(mTExpDocSelectedParties.Clone());
		}
		_docType = mtBillWindow.PO.POType;
		Owner = mtBillWindow;
		_mtBillSourceRow = sourceRow;
		_sourceRowIsNull = sourceRow == null;
		InnerInitialize();
		ASFontHelper.SetFontSettings(this);
	}

	public GoodsSelectPartyWindow(DialogWindowMode mode, MTInvoiceWindow invoiceWindow, MTInvoiceWindow.MTInvoiceSpecificationRow sourceRow = null)
	{
		InitializeComponent();
		_mode = mode;
		_mtInvoice = invoiceWindow;
		_docDate = invoiceWindow._date.EditValue.Value;
		_docStorage = invoiceWindow._storage.Code;
		_currCode = invoiceWindow._currCode.Code;
		_priceListType = invoiceWindow._priceListType.Code;
		_priceListTypeRoundDgr = TradeSettings.TradeCacheService.PriceType.GetItem(_priceListType)?.Round ?? "";
		_docDiscountPercent = invoiceWindow._discountPercent.Value;
		_discountPriceListType = invoiceWindow._discountPriceListType;
		_discountPriceTypeRoundDegree = invoiceWindow._discountPriceTypeRoundDegree;
		_discountPriceListVATType = invoiceWindow._discountPriceListVATType;
		_discountPriceListCurr = invoiceWindow._discountPriceListCurr;
		_discountPriceListCurrRate = invoiceWindow._discountPriceListCurrRate;
		_discountPriceListCurrBase = invoiceWindow._discountPriceListCurrBase;
		_vatType = invoiceWindow._vatType.Code;
		_vatEnabled = _vatType.InList("1", "2");
		_docDealType = invoiceWindow._dealType.Code;
		_useEnvFees = invoiceWindow._useEnvFees;
		_useDiscount = Settings.ParametersService.DiscountUse();
		_calcDiscountAuto = Settings.ParametersService.CalcDiscountAuto();
		_usedDiscount = invoiceWindow._useDiscount;
		_allowChangeSalePrices = Settings.ParametersService.AllowChangeSalePrices();
		_storedInDbAndAlsoNotIsDraft = invoiceWindow.PO.StoredInDb && invoiceWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && invoiceWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		_maxRowId = invoiceWindow._maxRowID;
		_mtInvoicesList = invoiceWindow._invoicesList;
		_mtDocGrid = invoiceWindow._invoiceGrid;
		_mtExpDocSelectedPartiesList = new List<PartyRow>();
		foreach (PartyRow mTExpDocSelectedParties in invoiceWindow.PO.MTExpDocSelectedPartiesList)
		{
			_mtExpDocSelectedPartiesList.Add(mTExpDocSelectedParties.Clone());
		}
		_docType = invoiceWindow.PO.POType;
		Owner = invoiceWindow;
		_mtInvoiceSourceRow = sourceRow;
		_sourceRowIsNull = sourceRow == null;
		InnerInitialize();
	}

	public GoodsSelectPartyWindow(DialogWindowMode mode, MTInvoiceRetailWindow invoiceRetailWindow, MTInvoiceRetailWindow.MTInvoiceRetailSpecificationRow sourceRow = null)
	{
		InitializeComponent();
		_mode = mode;
		_docDate = invoiceRetailWindow._date.EditValue.Value;
		_docStorage = invoiceRetailWindow._storage.Code;
		_storageRetail = true;
		_priceListType = Settings.ParametersService.DefaultSalePrice();
		_docDiscountPercent = invoiceRetailWindow._discountPercent.Value;
		_vatEnabled = invoiceRetailWindow._vatType.Code.InList("1", "2");
		_vatType = "2";
		_useDiscount = Settings.ParametersService.DiscountUse();
		_calcDiscountAuto = Settings.ParametersService.CalcDiscountAuto();
		_usedDiscount = invoiceRetailWindow.UsedDiscount;
		_allowChangeSalePrices = Settings.ParametersService.AllowChangeSalePrices();
		_useEnvFees = invoiceRetailWindow._useEnvFees;
		_docDealType = invoiceRetailWindow._dealType.Code;
		_currCode = "AMD";
		_storedInDbAndAlsoNotIsDraft = invoiceRetailWindow.PO.StoredInDb && invoiceRetailWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && invoiceRetailWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		_maxRowId = invoiceRetailWindow._maxRowID;
		_mtInvoiceRetailList = invoiceRetailWindow._invoicesRetailList;
		_mtDocGrid = invoiceRetailWindow._invoiceRetailGrid;
		_mtInvoiceRetail = invoiceRetailWindow;
		_mtExpDocSelectedPartiesList = new List<PartyRow>();
		foreach (PartyRow mTExpDocSelectedParties in invoiceRetailWindow.PO.MTExpDocSelectedPartiesList)
		{
			_mtExpDocSelectedPartiesList.Add(mTExpDocSelectedParties.Clone());
		}
		_docType = invoiceRetailWindow.PO.POType;
		Owner = invoiceRetailWindow;
		_mtInvoiceRetailSourceRow = sourceRow;
		_sourceRowIsNull = sourceRow == null;
		InnerInitialize();
	}

	public GoodsSelectPartyWindow(DialogWindowMode mode, MTMoveWithConfirmationWindow moveWithConfirmationWindow, MTMoveWithConfirmationWindow.MTMoveSpecificationRow sourceRow = null)
	{
		InitializeComponent();
		_mode = mode;
		_distrByFEFO = moveWithConfirmationWindow._distributeByFEFO;
		_docDate = moveWithConfirmationWindow._date.EditValue.Value;
		_docStorage = moveWithConfirmationWindow._storageExpense.Code;
		_storageRetail = moveWithConfirmationWindow._storageExpenseIsRetail;
		_storageIncomeRetail = moveWithConfirmationWindow._storageIncomeIsRetail;
		_priceListType = moveWithConfirmationWindow._pricesTypes.Code;
		_allowChangeSalePrices = Settings.ParametersService.AllowChangeSalePrices();
		_vatType = "2";
		_currCode = "AMD";
		_storedInDbAndAlsoNotIsDraft = moveWithConfirmationWindow.PO.StoredInDb && moveWithConfirmationWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && moveWithConfirmationWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		_maxRowId = moveWithConfirmationWindow._maxRowID;
		_mtMoveWithConfirmationList = moveWithConfirmationWindow._mtMoveWithConfirmationList;
		_mtDocGrid = moveWithConfirmationWindow._mtMoveWithConfirmationGrid;
		_mtMoveWithConfirmation = moveWithConfirmationWindow;
		_mtExpDocSelectedPartiesList = new List<PartyRow>();
		foreach (PartyRow mTExpDocSelectedParties in moveWithConfirmationWindow.PO.MTExpDocSelectedPartiesList)
		{
			_mtExpDocSelectedPartiesList.Add(mTExpDocSelectedParties.Clone());
		}
		_docType = moveWithConfirmationWindow.PO.POType;
		Owner = moveWithConfirmationWindow;
		_mtMoveWithConfirmationSourceRow = sourceRow;
		_sourceRowIsNull = sourceRow == null;
		InnerInitialize();
	}

	public GoodsSelectPartyWindow(DialogWindowMode mode, MTMoveWindow moveWindow, MTMoveWindow.MTMoveSpecificationRow sourceRow = null)
	{
		InitializeComponent();
		_mode = mode;
		_distrByFEFO = moveWindow._distributeByFEFO;
		_docDate = moveWindow._date.EditValue.Value;
		_docStorage = moveWindow._storageExpense.Code;
		_storageRetail = moveWindow._storageExpenseIsRetail;
		_storageIncomeRetail = moveWindow._storageIncomeIsRetail;
		_priceListType = moveWindow._pricesTypes.Code;
		_allowChangeSalePrices = Settings.ParametersService.AllowChangeSalePrices();
		_vatType = "2";
		_currCode = "AMD";
		_storedInDbAndAlsoNotIsDraft = moveWindow.PO.StoredInDb && moveWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && moveWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		_maxRowId = moveWindow._maxRowID;
		_mtMoveList = moveWindow._mtMoveList;
		_mtDocGrid = moveWindow._mtMoveGrid;
		_mtMove = moveWindow;
		_mtExpDocSelectedPartiesList = new List<PartyRow>();
		foreach (PartyRow mTExpDocSelectedParties in moveWindow.PO.MTExpDocSelectedPartiesList)
		{
			_mtExpDocSelectedPartiesList.Add(mTExpDocSelectedParties.Clone());
		}
		_docType = moveWindow.PO.POType;
		Owner = moveWindow;
		_mtMoveSourceRow = sourceRow;
		_sourceRowIsNull = sourceRow == null;
		InnerInitialize();
	}

	public GoodsSelectPartyWindow(DialogWindowMode mode, MTExpenseWindow expenseWindow, MTExpenseWindow.MTExpenseSpecificationRow sourceRow = null)
	{
		InitializeComponent();
		_mtExpense = expenseWindow;
		_distrByFEFO = expenseWindow._distributeByFEFO;
		_mode = mode;
		_docStorage = expenseWindow._storage.Code;
		_allowChangeSalePrices = Settings.ParametersService.AllowChangeSalePrices();
		_storedInDbAndAlsoNotIsDraft = expenseWindow.PO.StoredInDb && expenseWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && expenseWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		_maxRowId = expenseWindow._maxRowID;
		_mtExpenseList = expenseWindow._mtExpenseList;
		_mtDocGrid = expenseWindow._mtExpenseGrid;
		_mtExpDocSelectedPartiesList = new List<PartyRow>();
		foreach (PartyRow mTExpDocSelectedParties in expenseWindow.PO.MTExpDocSelectedPartiesList)
		{
			_mtExpDocSelectedPartiesList.Add(mTExpDocSelectedParties.Clone());
		}
		_docType = expenseWindow.PO.POType;
		_docDate = expenseWindow._date.EditValue.Value;
		Owner = expenseWindow;
		_mtExpenseSourceRow = sourceRow;
		_sourceRowIsNull = sourceRow == null;
		InnerInitialize();
	}

	public GoodsSelectPartyWindow(DialogWindowMode mode, MTComplectationWindow complectationWindow, MTComplectationWindow.MTComplectationSpecificationRow sourceRow = null)
	{
		InitializeComponent();
		_mode = mode;
		_mtComplectation = complectationWindow;
		_docDate = complectationWindow._date.EditValue.Value;
		_docStorage = complectationWindow._storageExpense.Code;
		_storageRetail = false;
		_complectCode = complectationWindow._complect.Code;
		_storedInDbAndAlsoNotIsDraft = complectationWindow.PO.StoredInDb && complectationWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && complectationWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		_maxRowId = complectationWindow._maxRowID;
		_mtComplectationList = complectationWindow._mtComplectPartsList;
		_mtDocGrid = complectationWindow._mtComplectPartsGrid;
		_mtExpDocSelectedPartiesList = new List<PartyRow>();
		foreach (PartyRow mTExpDocSelectedParties in complectationWindow.PO.MTExpDocSelectedPartiesList)
		{
			_mtExpDocSelectedPartiesList.Add(mTExpDocSelectedParties.Clone());
		}
		_docType = complectationWindow.PO.POType;
		Owner = complectationWindow;
		_mtComplectationSourceRow = sourceRow;
		_sourceRowIsNull = sourceRow == null;
		InnerInitialize();
	}

	public GoodsSelectPartyWindow(DialogWindowMode mode, MTReturnToSupplierWindow returnToSupplierWindow, MTReturnToSupplierWindow.MTRetToSupSpecificationRow sourceRow = null)
	{
		InitializeComponent();
		_mode = mode;
		_mtReturnToSupplier = returnToSupplierWindow;
		_docDate = returnToSupplierWindow._date.EditValue.Value;
		_docStorage = returnToSupplierWindow._storage.Code;
		_currCode = returnToSupplierWindow._currCode.Code;
		_priceListType = returnToSupplierWindow._priceListType.Code;
		_vatType = returnToSupplierWindow._vatType.Code;
		_vatEnabled = _vatType.InList("1", "2");
		_docDiscountPercent = returnToSupplierWindow._discountPercent.Value;
		_useDiscount = Settings.ParametersService.AllowDiscountInPurchase();
		_usedDiscount = returnToSupplierWindow._useDiscount;
		_storedInDbAndAlsoNotIsDraft = returnToSupplierWindow.PO.StoredInDb && returnToSupplierWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && returnToSupplierWindow.PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		_maxRowId = returnToSupplierWindow._maxRowID;
		_mtReturnToSupplierList = returnToSupplierWindow._mtRetToSupList;
		_mtDocGrid = returnToSupplierWindow._mtRetToSupGrid;
		_mtExpDocSelectedPartiesList = new List<PartyRow>();
		foreach (PartyRow mTExpDocSelectedParties in returnToSupplierWindow.PO.MTExpDocSelectedPartiesList)
		{
			_mtExpDocSelectedPartiesList.Add(mTExpDocSelectedParties.Clone());
		}
		_docType = returnToSupplierWindow.PO.POType;
		Owner = returnToSupplierWindow;
		_mtReturnToSupplierSourceRow = sourceRow;
		_sourceRowIsNull = sourceRow == null;
		InnerInitialize();
	}

	private void InnerInitialize()
	{
		ImageSource icon = new ImageSourceConverter().ConvertFromString(Settings.AppIconPath) as ImageSource;
		Icon = icon;
		AddedRows = 0;
		_allowAddRowAndNotViewMode = _mtDocGrid.AllowAdd && _mode != DialogWindowMode.ViewMode;
		_docTypeHasDiscount = _docType.InList(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value);
		_existDiscountPercentControl = _docTypeHasDiscount && (_useDiscount || _usedDiscount != 0m) && (!_calcDiscountAuto || _mode != DialogWindowMode.NewModeWithNext);
		_existDiscountPriceControl = _docTypeHasDiscount && (_existDiscountPercentControl || _docDiscountPercent != 0m);
		_existBonusControl = _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && _useBonus;
		_storageEditable = _docType.InList(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value);
		if (_storageEditable)
		{
			_storageRetail = TradeSettings.TradeCacheService.Storage.GetItem(_docStorage)?.Retail ?? false;
		}
		_storageAllowNegExp = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(_docStorage);
		_existPriceSummControls = _docTypeHasDiscount || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value || (_storageRetail && _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense.Value) || ((_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value) && (_storageRetail || _storageIncomeRetail));
		LayoutControl.ScrollBars = ScrollBars.Auto;
		Title = GetTitle();
		NextButton.Content = ArmSoft.SME.Common.Resources.NextWord;
		OkButton.Content = ArmSoft.SME.Common.Resources.Ok;
		CancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		bool existPriceSummControls = _existPriceSummControls;
		LayoutGroup layoutGroup = null;
		LayoutGroup layoutGroup2 = null;
		LayoutGroup layoutGroup3 = null;
		if (existPriceSummControls)
		{
			layoutGroup = AddLayoutGroup(null);
			layoutGroup.VerticalAlignment = VerticalAlignment.Top;
			layoutGroup2 = AddLayoutGroup(layoutGroup, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.MaterialOrService);
			layoutGroup2.HorizontalAlignment = HorizontalAlignment.Stretch;
			layoutGroup2.VerticalAlignment = VerticalAlignment.Stretch;
			layoutGroup3 = AddLayoutGroup(layoutGroup, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.QuantityAndSum);
			layoutGroup3.HorizontalAlignment = HorizontalAlignment.Stretch;
			layoutGroup3.VerticalAlignment = VerticalAlignment.Stretch;
		}
		LayoutItemWithRequired layoutItemWithRequired;
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value)
		{
			_itemType = new DropDownControl();
			layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.TypeWord, layoutGroup2);
			CommonEventHandlers.AddIsRequiredBinding(_itemType, layoutItemWithRequired);
			_itemType.DataView = new SmartEnum(typeof(GoodsTypesSE));
			_itemType.Owner = this;
			_itemType.Width = _itemType.RecommendedWidth;
			_itemType.IsRequired = _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value;
			layoutItemWithRequired.Content = _itemType;
			_itemType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ItemType_EditValueChanged).Handler;
		}
		if (_allowAddRowAndNotViewMode)
		{
			_group = new DropDownControl();
			layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.Group, layoutGroup2);
			CommonEventHandlers.AddIsRequiredBinding(_group, layoutItemWithRequired);
			_group.DataView = new GoodsGroups();
			_group.Owner = this;
			_group.Width = _group.RecommendedWidth;
			layoutItemWithRequired.Content = _group;
			_group.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Group_EditValueChanged).Handler;
		}
		_storage = new DropDownControl();
		layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.Storage, existPriceSummControls ? layoutGroup2 : layoutGroup);
		CommonEventHandlers.AddIsRequiredBinding(_storage, layoutItemWithRequired);
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value || (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && _showStorageColInBill))
		{
			_storage.DataView = new Storages(false);
		}
		else
		{
			_storage.DataView = new Storages();
		}
		_storage.Owner = this;
		_storage.Width = _storage.RecommendedWidth;
		_storage.IsRequired = true;
		_storage.Code = _docStorage;
		if (_docType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value && _docType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value && (_docType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value || !_showStorageColInBill))
		{
			_storage.IsReadOnly = true;
			_storage.IsEnabled = false;
		}
		layoutItemWithRequired.Content = _storage;
		_storage.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged).Handler;
		_itemCode = new DropDownControl();
		layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.Code, existPriceSummControls ? layoutGroup2 : layoutGroup);
		CommonEventHandlers.AddIsRequiredBinding(_itemCode, layoutItemWithRequired);
		_itemCode.AllowAdditionButtons = true;
		_itemCode.CodeWidth = 8;
		_itemCode.ShowComment = !existPriceSummControls;
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value || !_allowAddRowAndNotViewMode)
		{
			_itemCode.DataView = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter();
		}
		else
		{
			bool includeServices = _docTypeHasDiscount || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value;
			_itemCode.DataView = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter(_docDate, _docStorage, _storageRetail, includeServices, _storageAllowNegExp);
		}
		_itemCode.Owner = this;
		_itemCode.Width = _itemCode.RecommendedWidth;
		_itemCode.IsRequired = true;
		layoutItemWithRequired.Content = _itemCode;
		_itemCode.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(ItemCode_ClickDropDown).Handler;
		_itemCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ItemCode_EditValueChanged).Handler;
		_itemCode.EditAdditionalValueChanged += new SafeEventHandler<EventArgs>(ItemCode_EditAdditionalValueChanged).Handler;
		layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.Name, existPriceSummControls ? layoutGroup2 : layoutGroup);
		_itemName = new TextEditExt();
		CommonEventHandlers.AddIsRequiredBinding(_itemName, layoutItemWithRequired);
		_itemName.IsRequired = true;
		_itemName.IsReadOnly = true;
		_itemName.IsEnabled = _docTypeHasDiscount;
		_itemName.MaxLength = (_docTypeHasDiscount ? 255 : 200);
		_itemName.MinWidth = 20.0;
		_itemName.Width = SizeCalculator.WidthForString(_itemName.MaxLength);
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
		{
			_itemName.Focusable = !_itemName.IsReadOnly;
		}
		layoutItemWithRequired.Content = _itemName;
		layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.Unit, existPriceSummControls ? layoutGroup2 : layoutGroup);
		_unit = new DropDownControl();
		CommonEventHandlers.AddIsRequiredBinding(_unit, layoutItemWithRequired);
		if (!_allowAddRowAndNotViewMode)
		{
			_unit.DataView = new QntUnits();
		}
		else
		{
			_unit.DataView = new MTQntUnits("", isShowQntUnitCaption: false, isShowOnlyComplects: false, "", _docTypeHasDiscount ? ItemTypesSE.All.Value : ItemTypesSE.Goods.Value);
		}
		_unit.Owner = this;
		_unit.Width = _unit.RecommendedWidth;
		_unit.IsRequired = true;
		layoutItemWithRequired.Content = _unit;
		_unit.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(Unit_ClickDropDown).Handler;
		_unit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Unit_EditValueChanged).Handler;
		layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.Quantity, existPriceSummControls ? layoutGroup3 : layoutGroup);
		_quantity = new PopupCalcEditExt();
		CommonEventHandlers.AddIsRequiredBinding(_quantity, layoutItemWithRequired);
		_quantity.Mask = DecimalExtension.GetCurrencyFormat(13, 3);
		_quantity.Precision = 3;
		_quantity.IsRequired = true;
		_quantity.Width = SizeCalculator.WidthForNumber(13);
		layoutItemWithRequired.Content = _quantity;
		_quantity.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Quantity_EditValueChanged).Handler;
		if (_existPriceSummControls)
		{
			string text = "";
			if (_docType.InList(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value))
			{
				text = ArmSoft.SME.Common.Resources.MTTAccSalePrice;
			}
			else
			{
				text = ArmSoft.SME.Common.Resources.Price;
				if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value && _currCode != "AMD")
				{
					text = ArmSoft.SME.Common.Resources.FCPrice;
				}
			}
			layoutItemWithRequired = AddLayoutItem(text, layoutGroup3);
			_initPrice = new PopupCalcEditExt();
			CommonEventHandlers.AddIsRequiredBinding(_initPrice, layoutItemWithRequired);
			_initPrice.Mask = DecimalExtension.GetCurrencyFormat(16, 2);
			_initPrice.Precision = 2;
			_initPrice.IsRequired = true;
			_initPrice.Width = SizeCalculator.WidthForNumber(16);
			if (_allowAddRowAndNotViewMode)
			{
				if (_docType.InList(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value) && _storageRetail)
				{
					_existDropDownClickForPriceControl = true;
					_initPrice.AllowDefaultButton = false;
					_initPrice.IsReadOnly = true;
					ButtonInfo buttonInfo = new ButtonInfo
					{
						GlyphKind = GlyphKind.DropDown
					};
					buttonInfo.Click += InitPrice_ColumnDropDownClick;
					_initPrice.Buttons.Add(buttonInfo);
				}
				else if (_docType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value && !_allowChangeSalePrices)
				{
					_initPrice.IsReadOnly = true;
					_initPrice.IsRequired = false;
				}
			}
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
			{
				_initPrice.Focusable = !_initPrice.IsReadOnly;
			}
			layoutItemWithRequired.Content = _initPrice;
			_initPrice.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(InitPrice_EditValueChanged).Handler;
			_initPrice.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(InitPrice_PreviewKeyDown).Handler;
			if (_existDiscountPercentControl)
			{
				layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.DiscountPercent, layoutGroup3);
				_discountPercent = new PopupCalcEditExt();
				CommonEventHandlers.AddIsRequiredBinding(_discountPercent, layoutItemWithRequired);
				_discountPercent.Mask = DecimalExtension.GetCurrencyFormat(6, 2);
				_discountPercent.Precision = 6;
				if (_docTypeHasDiscount)
				{
					_discountPercent.IsReadOnly = !_allowChangeDiscountPrices;
				}
				_discountPercent.AllowNegativeNumbers = true;
				_discountPercent.Width = SizeCalculator.WidthForNumber(6);
				_discountPercent.Value = _docDiscountPercent;
				layoutItemWithRequired.Content = _discountPercent;
				_discountPercent.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DiscountPercent_EditValueChanged).Handler;
				if (!_showDiscountPercentColInBill)
				{
					layoutItemWithRequired.Visibility = Visibility.Collapsed;
					_discountPercent.Visibility = Visibility.Collapsed;
				}
				if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
				{
					_discountPercent.Focusable = !_discountPercent.IsReadOnly;
				}
			}
			if (_existDiscountPriceControl)
			{
				layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.DiscountedPrice, layoutGroup3);
				_discountPrice = new PopupCalcEditExt();
				CommonEventHandlers.AddIsRequiredBinding(_discountPrice, layoutItemWithRequired);
				_discountPrice.Mask = DecimalExtension.GetCurrencyFormat(16, 2);
				_discountPrice.Precision = 2;
				_discountPrice.IsReadOnly = !_existDiscountPercentControl;
				if (_docTypeHasDiscount)
				{
					_discountPrice.IsReadOnly = !_allowChangeDiscountPrices;
				}
				else if (!_allowChangeSalePrices || _initPrice.IsReadOnly)
				{
					_discountPrice.IsReadOnly = !_allowChangeDiscountPrices;
				}
				_discountPrice.Width = SizeCalculator.WidthForNumber(16);
				if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
				{
					_discountPrice.Focusable = !_discountPrice.IsReadOnly;
				}
				layoutItemWithRequired.Content = _discountPrice;
				_discountPrice.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DiscountPrice_EditValueChanged).Handler;
				if (!_showDiscountedPriceColInBill)
				{
					layoutItemWithRequired.Visibility = Visibility.Collapsed;
					_discountPrice.Visibility = Visibility.Collapsed;
				}
			}
			string text2 = "";
			if (_docType.InList(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value))
			{
				text2 = ArmSoft.SME.Trade.Common.Resources.SaleSum;
			}
			else
			{
				text2 = (_existDiscountPriceControl ? ArmSoft.SME.Trade.Common.Resources.DiscountedSumm : ArmSoft.SME.Common.Resources.Summa);
				if (_docTypeHasDiscount && _currCode != "AMD")
				{
					text2 = ArmSoft.SME.Common.Resources.FCSumma;
				}
			}
			layoutItemWithRequired = AddLayoutItem(text2, layoutGroup3);
			_summa = new PopupCalcEditExt();
			CommonEventHandlers.AddIsRequiredBinding(_summa, layoutItemWithRequired);
			_summa.Mask = DecimalExtension.GetCurrencyFormat(16, 2);
			_summa.Precision = 2;
			_summa.IsRequired = !_existDiscountPercentControl;
			if (_summa.IsRequired && !_initPrice.IsRequired)
			{
				_summa.IsRequired = false;
			}
			if (_docType.InList(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value))
			{
				_summa.IsReadOnly = true;
			}
			else
			{
				_summa.IsReadOnly = _existDiscountPriceControl && !_existDiscountPercentControl && _docType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value;
			}
			if (!_summa.IsReadOnly && ((_docType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value && !_allowChangeSalePrices) || _initPrice.IsReadOnly))
			{
				_summa.IsReadOnly = true;
			}
			if (_docTypeHasDiscount)
			{
				if (_existDiscountPercentControl)
				{
					_summa.IsReadOnly = !_allowChangeDiscountPrices;
				}
				else
				{
					_summa.IsReadOnly = !_allowChangeSalePrices;
				}
			}
			_summa.Width = SizeCalculator.WidthForNumber(16);
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
			{
				_summa.Focusable = !_summa.IsReadOnly;
			}
			layoutItemWithRequired.Content = _summa;
			_summa.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Summa_EditValueChanged).Handler;
			if (_useEnvFees)
			{
				layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.EnvFeePercent, layoutGroup3);
				_envFeePercent = new PopupCalcEditExt();
				CommonEventHandlers.AddIsRequiredBinding(_envFeePercent, layoutItemWithRequired);
				_envFeePercent.Mask = DecimalExtension.GetCurrencyFormat(6, 2);
				_envFeePercent.Precision = 2;
				_envFeePercent.AllowNegativeNumbers = false;
				_envFeePercent.Width = SizeCalculator.WidthForNumber(6);
				layoutItemWithRequired.Content = _envFeePercent;
				_envFeePercent.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(EnvFeePercent_EditValueChanged).Handler;
				layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.EnvFeeSumma, layoutGroup3);
				_envFeeSumma = new PopupCalcEditExt();
				CommonEventHandlers.AddIsRequiredBinding(_envFeeSumma, layoutItemWithRequired);
				_envFeeSumma.Mask = DecimalExtension.GetCurrencyFormat(16, 2);
				_envFeeSumma.Precision = 2;
				_envFeeSumma.AllowNegativeNumbers = false;
				_envFeeSumma.Width = SizeCalculator.WidthForNumber(16);
				layoutItemWithRequired.Content = _envFeeSumma;
				_envFeeSumma.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(EnvFeeSumma_EditValueChanged).Handler;
			}
			if (_existBonusControl)
			{
				layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Trade.Common.Resources.BonusUse, layoutGroup3);
				_usedBonus = new PopupCalcEditExt();
				CommonEventHandlers.AddIsRequiredBinding(_usedBonus, layoutItemWithRequired);
				_usedBonus.Mask = DecimalExtension.GetCurrencyFormat(16, 2);
				_usedBonus.Precision = 2;
				_usedBonus.IsReadOnly = true;
				_usedBonus.Width = SizeCalculator.WidthForNumber(16);
				layoutItemWithRequired.Content = _usedBonus;
				if (!_showUsedBonusColInMTBill)
				{
					layoutItemWithRequired.Visibility = Visibility.Collapsed;
					_usedBonus.Visibility = Visibility.Collapsed;
				}
				if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
				{
					_usedBonus.Focusable = !_usedBonus.IsReadOnly;
				}
				layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Trade.Common.Resources.AccumulateBonus, layoutGroup3);
				_accumulatedBonus = new PopupCalcEditExt();
				CommonEventHandlers.AddIsRequiredBinding(_accumulatedBonus, layoutItemWithRequired);
				_accumulatedBonus.Mask = DecimalExtension.GetCurrencyFormat(16, 2);
				_accumulatedBonus.Precision = 2;
				_accumulatedBonus.IsReadOnly = true;
				_accumulatedBonus.Width = SizeCalculator.WidthForNumber(16);
				layoutItemWithRequired.Content = _accumulatedBonus;
				if (!_showAccumulatedBonusColInMTBill)
				{
					layoutItemWithRequired.Visibility = Visibility.Collapsed;
					_accumulatedBonus.Visibility = Visibility.Collapsed;
				}
				if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
				{
					_accumulatedBonus.Focusable = !_accumulatedBonus.IsReadOnly;
				}
			}
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
			{
				layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.MTTAccSalePrice, layoutGroup3);
				_salePrice = new PopupCalcEditExt();
				CommonEventHandlers.AddIsRequiredBinding(_salePrice, layoutItemWithRequired);
				_salePrice.Mask = DecimalExtension.GetCurrencyFormat(16, 2);
				_salePrice.Precision = 2;
				_salePrice.IsReadOnly = true;
				if (_allowAddRowAndNotViewMode)
				{
					_existDropDownClickForSalePriceControl = true;
					_salePrice.AllowDefaultButton = false;
					_salePrice.IsReadOnly = true;
					ButtonInfo buttonInfo2 = new ButtonInfo
					{
						GlyphKind = GlyphKind.DropDown
					};
					buttonInfo2.Click += SalePrice_ColumnDropDownClick;
					_salePrice.Buttons.Add(buttonInfo2);
				}
				_salePrice.Width = SizeCalculator.WidthForNumber(16);
				layoutItemWithRequired.Content = _salePrice;
				_salePrice.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(SalePrice_PreviewKeyDown).Handler;
				layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Trade.Common.Resources.SaleSum, layoutGroup3);
				_saleSumma = new PopupCalcEditExt();
				CommonEventHandlers.AddIsRequiredBinding(_saleSumma, layoutItemWithRequired);
				_saleSumma.Mask = DecimalExtension.GetCurrencyFormat(16, 2);
				_saleSumma.Precision = 2;
				_saleSumma.IsReadOnly = true;
				_saleSumma.Width = SizeCalculator.WidthForNumber(16);
				layoutItemWithRequired.Content = _saleSumma;
			}
			if ((_docTypeHasDiscount || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value) && _vatEnabled)
			{
				layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.OptionsVAT, layoutGroup3);
				_vat = new CheckEditExt();
				CommonEventHandlers.AddIsRequiredBinding(_vat, layoutItemWithRequired);
				_vat.IsChecked = false;
				_vat.Focusable = true;
				layoutItemWithRequired.Content = _vat;
				_vat.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(VAT_EditValueChanged).Handler;
				if (!_showVATColInBill)
				{
					layoutItemWithRequired.Visibility = Visibility.Collapsed;
					_vat.Visibility = Visibility.Collapsed;
				}
			}
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value)
			{
				layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.DealType, layoutGroup3);
				_dealType = new DropDownControl
				{
					Name = "DealType"
				};
				CommonEventHandlers.AddIsRequiredBinding(_dealType, layoutItemWithRequired);
				_dealType.AllowAdditionButtons = false;
				_dealType.CodeWidth = 1;
				_dealType.ShowComment = true;
				_dealType.DataView = new SmartEnum(_vatEnabled ? typeof(InvoiceDealTypesSE) : typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
				_dealType.Owner = this;
				_dealType.Width = _dealType.RecommendedWidth;
				_dealType.IsRequired = false;
				_dealType.Code = _docDealType;
				layoutItemWithRequired.Content = _dealType;
			}
		}
		else if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value)
		{
			layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.Summa, layoutGroup3);
			_summa = new PopupCalcEditExt();
			CommonEventHandlers.AddIsRequiredBinding(_summa, layoutItemWithRequired);
			_summa.Mask = DecimalExtension.GetCurrencyFormat(16, 2);
			_summa.Precision = 2;
			_summa.Width = SizeCalculator.WidthForNumber(16);
			layoutItemWithRequired.Content = _summa;
		}
		LayoutGroup container = AddLayoutGroup(null);
		layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.Party, container);
		if (!_showPartyColInBill)
		{
			layoutItemWithRequired.Visibility = Visibility.Collapsed;
		}
		_party = new CheckEditExt();
		CommonEventHandlers.AddIsRequiredBinding(_party, layoutItemWithRequired);
		_party.VerticalAlignment = VerticalAlignment.Center;
		_party.IsChecked = false;
		_party.Focusable = true;
		layoutItemWithRequired.Content = _party;
		_party.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Party_EditValueChanged).Handler;
		_liParty = AddLayoutItem("", container);
		_liParty.VerticalAlignment = VerticalAlignment.Center;
		_liParty.Focusable = false;
		if (!_showPartyColInBill)
		{
			_liParty.Visibility = Visibility.Collapsed;
			_party.Visibility = Visibility.Collapsed;
		}
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value)
		{
			layoutItemWithRequired = AddLayoutItem(ArmSoft.SME.Common.Resources.Description, layoutGroup3);
			_rowDesc = new TextEditExt();
			CommonEventHandlers.AddIsRequiredBinding(_rowDesc, layoutItemWithRequired);
			_rowDesc.MaxLength = 50;
			_rowDesc.MinWidth = 20.0;
			_rowDesc.Width = SizeCalculator.WidthForString(_rowDesc.MaxLength);
			layoutItemWithRequired.Content = _rowDesc;
		}
		if (_showPartyUSEABLEDATE)
		{
			_fefo = new Button
			{
				UseLayoutRounding = true,
				Content = ArmSoft.SME.Trade.Common.Resources.FEFODistibute
			};
			layoutItemWithRequired = AddLayoutItem("", container);
			layoutItemWithRequired.HorizontalAlignment = HorizontalAlignment.Center;
			layoutItemWithRequired.VerticalAlignment = VerticalAlignment.Center;
			layoutItemWithRequired.Content = _fefo;
			_fefo.Click += new SafeEventHandler<RoutedEventArgs>(FEFO_Click).Handler;
			_fefo.IsTabStop = true;
		}
		_partiesGrid = new DocumentGridControl
		{
			Owner = this,
			Name = "_partiesGrid"
		};
		LayoutControl.Children.Add(_partiesGrid);
		_partiesGrid.AllowAdd = false;
		_partiesGrid.AllowDelete = false;
		_partiesGrid.AllowDuplicate = false;
		_partiesGrid.ShowButtons = false;
		DocumentGridColumn documentGridColumn = _partiesGrid.AddDateColumn("IncomeDate", ArmSoft.SME.Common.Resources.IncomeDateShort);
		documentGridColumn.ShowInColumnChooser = false;
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn2 = _partiesGrid.AddNumberColumn("Cost", ArmSoft.SME.Common.Resources.MtCurCost, 16, 2);
		documentGridColumn2.ShowInColumnChooser = false;
		documentGridColumn2.AllowEditing = DefaultBoolean.False;
		_availableQuantityCol = _partiesGrid.AddNumberColumn("Qty", ArmSoft.SME.Common.Resources.Availability, 13, 3, allowNegativeNumbers: false, showTotal: true);
		_availableQuantityCol.ShowInColumnChooser = false;
		_availableQuantityCol.AllowEditing = DefaultBoolean.False;
		_expQuantityCol = _partiesGrid.AddNumberColumn("ExpQty", ArmSoft.SME.Common.Resources.Quantity, 13, 3, allowNegativeNumbers: false, showTotal: true);
		_expQuantityCol.AllowEditing = DefaultBoolean.False;
		_expQuantityCol.ShowInColumnChooser = false;
		DocumentGridColumn documentGridColumn3 = _partiesGrid.AddStringColumn("PartnerCode", ArmSoft.SME.Common.Resources.Supplier, 20);
		documentGridColumn3.Width = 100.0;
		documentGridColumn3.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn4 = _partiesGrid.AddStringColumn("Contract", ArmSoft.SME.Common.Resources.Contract, 50);
		documentGridColumn4.Width = 100.0;
		documentGridColumn4.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn5 = _partiesGrid.AddStringColumn("Comment", ArmSoft.SME.Common.Resources.IncomeDoc, 50);
		documentGridColumn5.ShowInColumnChooser = false;
		documentGridColumn5.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn6 = _partiesGrid.AddDateColumn("UseableDate", ArmSoft.SME.Trade.Common.Resources.UseLife);
		documentGridColumn6.AllowEditing = DefaultBoolean.False;
		documentGridColumn6.AllowSorting = DefaultBoolean.True;
		DocumentGridColumn documentGridColumn7 = _partiesGrid.AddStringColumn("Country", ArmSoft.SME.Common.Resources.Country, 50);
		documentGridColumn7.Width = 100.0;
		documentGridColumn7.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn8 = _partiesGrid.AddStringColumn("Producer", ArmSoft.SME.Common.Resources.Producer, 50);
		documentGridColumn8.Width = 100.0;
		documentGridColumn8.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn9 = _partiesGrid.AddStringColumn("Property1", ArmSoft.SME.Trade.Common.Resources.Properties, 50);
		documentGridColumn9.Width = 100.0;
		documentGridColumn9.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn10 = _partiesGrid.AddStringColumn("Property2", ArmSoft.SME.Trade.Common.Resources.Properties, 50);
		documentGridColumn10.Width = 100.0;
		documentGridColumn10.AllowEditing = DefaultBoolean.False;
		_partiesTableView = _partiesGrid.View;
		_partiesTableView.CellValueChanged += PartiesTableView_CellValueChanged;
		_partiesTableView.CellValueChanging += PartiesTableView_CellValueChanging;
		_partiesTableView.RowUpdated += new SafeEventHandler<RowEventArgs>(PartiesTableView_RowUpdated).Handler;
		_partiesGrid.EscapePressed += new SafeEventHandler<EventArgs>(PartiesGrid_EscapePressed).Handler;
		_partiesGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
		_partiesGrid.VerticalAlignment = VerticalAlignment.Stretch;
		if (!_showPartyColInBill)
		{
			_partiesGrid.Visibility = Visibility.Collapsed;
		}
		MinWidth = SizeCalculator.Width(680.0);
		if (_showPartyColInBill)
		{
			MinHeight = SizeCalculator.Height(430.0);
		}
		else
		{
			MinHeight = SizeCalculator.Height(260.0);
		}
		Load();
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value)
		{
			_summa.IsRequired = _itemType.Code == ItemTypesSE.Services.Value;
			_storage.IsRequired = !_summa.IsRequired;
			layoutItemWithRequired = (LayoutItemWithRequired)(object)_summa.Parent;
			if (_summa.IsRequired)
			{
				_summa.Visibility = Visibility.Visible;
				layoutItemWithRequired.Visibility = Visibility.Visible;
			}
			else
			{
				_summa.Visibility = Visibility.Collapsed;
				layoutItemWithRequired.Visibility = Visibility.Collapsed;
			}
		}
		if (!_allowAddRowAndNotViewMode)
		{
			NextButton.Visibility = Visibility.Collapsed;
			OkButton.Visibility = Visibility.Collapsed;
			SetReadOnly();
		}
		else if (_mode == DialogWindowMode.EditMode)
		{
			NextButton.Visibility = Visibility.Collapsed;
		}
		if (!_allowAddRowAndNotViewMode && !_sourceRowIsNull && string.IsNullOrWhiteSpace(_expMethod) && !string.IsNullOrWhiteSpace(_itemCode.Code))
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item != null)
			{
				_expMethod = item.ExpenseMethod;
			}
		}
		if (_showPartyColInBill)
		{
			ChangeExpMethod();
			_partiesGrid.ControlEnterPressed += new SafeEventHandler<EventArgs>(OnGridControlEnterPressed).Handler;
		}
		ASFontHelper.SetFontSettings(this);
		RestoreSettings();
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(GoodsSelectPartyWindow_PreviewKeyDown).Handler;
		NextButton.Click += new SafeEventHandler<RoutedEventArgs>(NextButton_Click).Handler;
		OkButton.Click += new SafeEventHandler<RoutedEventArgs>(OkButton_Click).Handler;
		Loaded += new SafeEventHandler<RoutedEventArgs>(GoodsSelectPartyWindow_Loaded).Handler;
		CancelButton.Click += new SafeEventHandler<RoutedEventArgs>(CancelButton_Click).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(GoodsSelectPartyWindow_Closing).Handler;
	}

	private void ChangeExpMethod()
	{
		bool flag = !string.IsNullOrWhiteSpace(_expMethod) && !(_expMethod == "3");
		_partiesGrid.Grid.Columns["PartnerCode"].Visible = flag && _showPartySUPPLIER;
		_partiesGrid.Grid.Columns["Contract"].Visible = flag && _showPartyCONTRACT;
		_partiesGrid.Grid.Columns["Comment"].Visible = flag;
		GridColumn gridColumn = _partiesGrid.Grid.Columns.FirstOrDefault((GridColumn c) => c.Name.Contains("UseableDate"));
		if (gridColumn != null)
		{
			gridColumn.Visible = flag && _showPartyUSEABLEDATE;
		}
		_partiesGrid.Grid.Columns["Country"].Visible = flag && _showPartyCountry;
		_partiesGrid.Grid.Columns["Producer"].Visible = flag && _showPartyProducer;
		_partiesGrid.Grid.Columns["Property1"].Visible = flag && _showPartyProperty1;
		_partiesGrid.Grid.Columns["Property2"].Visible = flag && _showPartyProperty2;
		_partiesGrid.Grid.Columns["Cost"].Visible = flag && _showCost;
	}

	private string GetTitle()
	{
		string text = "";
		if (SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>.TryFromValue(_docType, out var result))
		{
			text += result.FullCaption;
		}
		text = text + " [" + _docDate.FormatDDMMYY() + "]";
		switch (_mode)
		{
		case DialogWindowMode.NewMode:
		case DialogWindowMode.NewModeWithNext:
			text += ArmSoft.SME.Common.Resources.CaptionNew;
			break;
		case DialogWindowMode.EditMode:
			text += (_allowAddRowAndNotViewMode ? ArmSoft.SME.Common.Resources.CaptionEdit : ArmSoft.SME.Common.Resources.CaptionView);
			break;
		case DialogWindowMode.ViewMode:
			text += ArmSoft.SME.Common.Resources.CaptionView;
			break;
		case DialogWindowMode.DeleteMode:
			text += ArmSoft.SME.Common.Resources.CaptionDelete;
			break;
		}
		return text;
	}

	private void Load()
	{
		_iItemTypeCode = ItemTypesSE.Goods.Value;
		_editableMode = false;
		if (_allowAddRowAndNotViewMode)
		{
			_group.Code = "";
		}
		if (!_sourceRowIsNull)
		{
			string text = "";
			switch (_docType)
			{
			case 8:
			{
				InnerLoadForInvoice(out var isPartySelected2);
				text = (isPartySelected2 ? ArmSoft.SME.Common.Resources.PartySelected : ArmSoft.SME.Common.Resources.AutoSelection);
				break;
			}
			case 21:
			{
				InnerLoadForInvoiceRetail(out var isPartySelected4);
				text = (isPartySelected4 ? ArmSoft.SME.Common.Resources.PartySelected : ArmSoft.SME.Common.Resources.AutoSelection);
				break;
			}
			case 18:
			{
				InnerLoadForBill(out var isPartySelected3);
				text = (isPartySelected3 ? ArmSoft.SME.Common.Resources.PartySelected : ArmSoft.SME.Common.Resources.AutoSelection);
				break;
			}
			case 9:
				if (_mode == DialogWindowMode.NewModeWithNext)
				{
					_rowId = _maxRowId;
				}
				else
				{
					_rowId = _mtExpenseSourceRow.RowID;
					_oldItemCode = _mtExpenseSourceRow.MTCode;
				}
				_itemCode.Code = _mtExpenseSourceRow.MTCode;
				_itemName.Text = _mtExpenseSourceRow.MTName;
				_unit.Code = _mtExpenseSourceRow.UnitCode;
				_quantity.Value = _mtExpenseSourceRow.Quantity;
				text = _mtExpenseSourceRow.PartyMethodName;
				if (_mode == DialogWindowMode.NewModeWithNext && text == ArmSoft.SME.Common.Resources.PartySelected)
				{
					text = ArmSoft.SME.Common.Resources.AutoSelection;
				}
				if (_allowAddRowAndNotViewMode)
				{
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
					if (item != null)
					{
						_expMethod = item.ExpenseMethod;
						_isBaseUnit = item.Unit == _unit.Code;
						_unit.DataView = new MTQntUnits(_itemCode.Code);
					}
					else if (!string.IsNullOrWhiteSpace(_itemCode.Code))
					{
						throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _itemCode.Code));
					}
				}
				if (_existPriceSummControls)
				{
					_initPrice.Value = _mtExpenseSourceRow.SalePrice;
					_summa.Value = _mtExpenseSourceRow.Summa;
				}
				break;
			case 10:
			{
				InnerLoadForMTMove(out var isPartySelected);
				text = (isPartySelected ? ArmSoft.SME.Common.Resources.PartySelected : ArmSoft.SME.Common.Resources.AutoSelection);
				break;
			}
			case 39:
			{
				InnerLoadForMTMoveWithConfirmation(out var isPartySelected5);
				text = (isPartySelected5 ? ArmSoft.SME.Common.Resources.PartySelected : ArmSoft.SME.Common.Resources.AutoSelection);
				break;
			}
			case 11:
				_itemType.Code = _mtComplectationSourceRow.ItemType;
				_iItemTypeCode = _mtComplectationSourceRow.ItemType;
				if (_mode == DialogWindowMode.NewModeWithNext)
				{
					_rowId = _maxRowId;
				}
				else
				{
					_rowId = _mtComplectationSourceRow.RowID;
					_oldItemCode = _mtComplectationSourceRow.ItemCode;
				}
				_itemCode.Code = _mtComplectationSourceRow.ItemCode;
				_itemName.Text = _mtComplectationSourceRow.ItemName;
				_unit.Code = _mtComplectationSourceRow.UnitCode;
				_quantity.Value = _mtComplectationSourceRow.Quantity;
				if (_mtComplectationSourceRow.ItemType == ItemTypesSE.Services.Value)
				{
					_summa.Value = _mtComplectationSourceRow.Summa;
				}
				text = ((_mtComplectationSourceRow.IsPartySelected.HasValue && _mtComplectationSourceRow.IsPartySelected.Value) ? ArmSoft.SME.Common.Resources.PartySelected : ArmSoft.SME.Common.Resources.AutoSelection);
				if (_allowAddRowAndNotViewMode)
				{
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
					if (item != null)
					{
						_expMethod = item.ExpenseMethod;
						_isBaseUnit = item.Unit == _unit.Code;
						_unit.DataView = new MTQntUnits(_itemCode.Code);
					}
					else if (!string.IsNullOrWhiteSpace(_itemCode.Code))
					{
						throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _itemCode.Code));
					}
				}
				break;
			case 14:
				if (_mode == DialogWindowMode.NewModeWithNext)
				{
					_rowId = _maxRowId;
				}
				else
				{
					_rowId = _mtReturnToSupplierSourceRow.RowID;
					_oldItemCode = _mtReturnToSupplierSourceRow.MTCode;
				}
				_storage.Code = _mtReturnToSupplierSourceRow.Storage;
				if (_storage.Code != _docStorage)
				{
					_storageRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code)?.Retail ?? false;
					_storageAllowNegExp = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(_storage.Code);
				}
				_itemCode.Code = _mtReturnToSupplierSourceRow.MTCode;
				_itemName.Text = _mtReturnToSupplierSourceRow.MTName;
				_unit.Code = _mtReturnToSupplierSourceRow.UnitCode;
				_quantity.Value = _mtReturnToSupplierSourceRow.Quantity;
				_initPrice.Value = _mtReturnToSupplierSourceRow.Price;
				if (_existDiscountPercentControl)
				{
					_discountPercent.Value = _mtReturnToSupplierSourceRow.Discount;
				}
				if (_existDiscountPriceControl)
				{
					_discountPrice.Value = _mtReturnToSupplierSourceRow.DiscountPrice;
				}
				_summa.Value = _mtReturnToSupplierSourceRow.Summa;
				_salePrice.Value = _mtReturnToSupplierSourceRow.SalePrice;
				_saleSumma.Value = _mtReturnToSupplierSourceRow.SaleSumma;
				if (_vatEnabled)
				{
					_vat.IsChecked = _mtReturnToSupplierSourceRow.VAT;
				}
				text = (_mtReturnToSupplierSourceRow.IsPartySelected ? ArmSoft.SME.Common.Resources.PartySelected : ArmSoft.SME.Common.Resources.AutoSelection);
				if (_allowAddRowAndNotViewMode)
				{
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
					if (item != null)
					{
						_expMethod = item.ExpenseMethod;
						_isBaseUnit = item.Unit == _unit.Code;
						_unit.DataView = new MTQntUnits(_itemCode.Code);
					}
					else if (!string.IsNullOrWhiteSpace(_itemCode.Code))
					{
						throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _itemCode.Code));
					}
				}
				break;
			}
			if (_iItemTypeCode == ItemTypesSE.Goods.Value && _showPartyColInBill)
			{
				_party.IsChecked = text == ArmSoft.SME.Common.Resources.PartySelected;
				if (_storedInDbAndAlsoNotIsDraft || (_mode == DialogWindowMode.ViewMode && _party.IsChecked.Value))
				{
					_mtCodeSelectedPartiesList = _mtExpDocSelectedPartiesList.FindAll(FindGridRowInList) ?? new List<PartyRow>();
					_partiesGrid.ItemsSource = _mtCodeSelectedPartiesList;
				}
				else
				{
					_mtCodeSelectedPartiesList = FillPartiesGrid();
					if (!_party.IsChecked.Value)
					{
						if (!CanDivideByParties(_quantity.Value) && _mode != DialogWindowMode.ViewMode)
						{
							decimal num = Convert.ToDecimal(_availableQuantityCol.TotalSummaries[0].Value);
							ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.FormNegativeQuantityMessage(num - _quantity.Value, _storage.Code, _itemCode.Code, _docDate, _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value, Settings.ParametersService.EnableSubstituteItems()), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
							_quantity.Value = num;
							if (_docTypeHasDiscount)
							{
								decimal num2 = (_existDiscountPriceControl ? _discountPrice.Value : _initPrice.Value);
								_summa.Value = (num2 * _quantity.Value).RoundAs(2);
							}
						}
						_partiesGrid.ItemsSource = _mtCodeSelectedPartiesList;
					}
					else
					{
						List<PartyRow> list = _mtExpDocSelectedPartiesList.FindAll(FindGridRowInList);
						if (list.Count > 0)
						{
							foreach (PartyRow item2 in list)
							{
								_partyBase = item2.Base;
								_partyRowId = item2.Row;
								PartyRow partyRow = _mtCodeSelectedPartiesList.Find(FindRowInList);
								if (partyRow == null)
								{
									continue;
								}
								if (!_storageAllowNegExp)
								{
									if (partyRow.Qty >= item2.ExpQty)
									{
										partyRow.ExpQty = item2.ExpQty;
									}
									else
									{
										partyRow.ExpQty = partyRow.Qty;
									}
								}
								else
								{
									partyRow.ExpQty = item2.ExpQty;
								}
							}
							if (_showPartyColInBill)
							{
								_partiesGrid.Grid.RefreshData();
							}
							decimal num = Convert.ToDecimal(_expQuantityCol.TotalSummaries[0].Value);
							if (_quantity.Value != num)
							{
								ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.FormNegativeQuantityMessage(num - _quantity.Value, _storage.Code, _itemCode.Code, _docDate, _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value, Settings.ParametersService.EnableSubstituteItems()), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
								_quantity.Value = num;
								if (_docTypeHasDiscount)
								{
									decimal num3 = (_existDiscountPriceControl ? _discountPrice.Value : _initPrice.Value);
									_summa.Value = (num3 * _quantity.Value).RoundAs(2);
								}
							}
						}
					}
				}
			}
			if (!_docTypeHasDiscount && !_party.IsChecked.Value)
			{
				Party_Changed();
			}
			if (!_allowAddRowAndNotViewMode)
			{
				SetReadOnly();
			}
		}
		else
		{
			_rowId = _maxRowId;
			if (_docTypeHasDiscount || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value)
			{
				if ((_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value) && string.IsNullOrWhiteSpace(_itemType.Code))
				{
					_iItemTypeCode = ItemTypesSE.Goods.Value;
					_itemType.Code = _iItemTypeCode;
				}
				if (_existDiscountPercentControl)
				{
					_discountPercent.EditValue = _docDiscountPercent;
				}
			}
			else if (_showPartyColInBill)
			{
				_party.IsChecked = false;
				Party_Changed();
			}
		}
		_editableMode = true;
	}

	private void InnerLoadForInvoice(out bool isPartySelected)
	{
		if (_mode == DialogWindowMode.NewModeWithNext)
		{
			_rowId = _maxRowId;
		}
		else
		{
			_rowId = _mtInvoiceSourceRow.RowID;
			_oldItemCode = _mtInvoiceSourceRow.ItemCode;
		}
		_storage.Code = _mtInvoiceSourceRow.Storage;
		_itemType.Code = _mtInvoiceSourceRow.ItemType;
		if (_storage.Code != _docStorage)
		{
			_storageRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code)?.Retail ?? false;
			_storageAllowNegExp = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(_storage.Code);
		}
		_itemCode.Code = _mtInvoiceSourceRow.ItemCode;
		_itemName.Text = _mtInvoiceSourceRow.ItemName;
		_unit.Code = _mtInvoiceSourceRow.UnitCode;
		_quantity.Value = _mtInvoiceSourceRow.Quantity;
		_initPrice.Value = _mtInvoiceSourceRow.Price;
		if (_existDiscountPercentControl)
		{
			_discountPercent.Value = _mtInvoiceSourceRow.Discount;
		}
		if (_existDiscountPriceControl)
		{
			_discountPrice.Value = _mtInvoiceSourceRow.DiscountPrice;
		}
		if (_existBonusControl)
		{
			_usedBonus.Value = _mtInvoiceSourceRow.UsedBonus;
			_accumulatedBonus.Value = _mtInvoiceSourceRow.AccumulatedBonus;
		}
		_summa.Value = _mtInvoiceSourceRow.Summa;
		_iItemTypeCode = _mtInvoiceSourceRow.ItemType;
		isPartySelected = _mtInvoiceSourceRow.IsPartySelected.HasValue && _mtInvoiceSourceRow.IsPartySelected.Value;
		if (_vatEnabled)
		{
			_vat.IsChecked = _mtInvoiceSourceRow.VAT;
		}
		_rowDesc.Text = _mtInvoiceSourceRow.RowDescription;
		if (_useEnvFees)
		{
			_envFeePercent.Value = _mtInvoiceSourceRow.EnvFeePercent;
			_envFeeSumma.Value = _mtInvoiceSourceRow.EnvFeeSumma;
		}
		if (_vatEnabled)
		{
			if (_vat.IsChecked.Value)
			{
				_dealType.Code = "";
				_dealType.IsReadOnly = true;
			}
			else
			{
				_dealType.IsReadOnly = false;
				_dealType.Code = (from accDealTypesSE in SmartEnum<AccDealTypesSE, short>.List
					where accDealTypesSE.Value.ToString() == _mtInvoiceSourceRow.DealType
					select accDealTypesSE.ValueUI).FirstOrDefault()?.ToString() ?? "";
			}
		}
		else
		{
			_dealType.IsReadOnly = false;
			_dealType.Code = (from accDealTypesSE in SmartEnum<AccDealTypesSE, short>.List
				where accDealTypesSE.Value.ToString() == _mtInvoiceSourceRow.DealType
				select accDealTypesSE.ValueUI).FirstOrDefault()?.ToString() ?? "";
		}
		if (_allowAddRowAndNotViewMode)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item != null)
			{
				_expMethod = item.ExpenseMethod;
				_isBaseUnit = item.Unit == _unit.Code;
				_unit.DataView = new MTQntUnits(_itemCode.Code);
			}
			else if (!string.IsNullOrWhiteSpace(_itemCode.Code))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _itemCode.Code));
			}
		}
	}

	private void InnerLoadForInvoiceRetail(out bool isPartySelected)
	{
		if (_mode == DialogWindowMode.NewModeWithNext)
		{
			_rowId = _maxRowId;
		}
		else
		{
			_rowId = _mtInvoiceRetailSourceRow.RowID;
			_oldItemCode = _mtInvoiceRetailSourceRow.ItemCode;
		}
		_storage.Code = _mtInvoiceRetailSourceRow.Storage;
		_itemCode.Code = _mtInvoiceRetailSourceRow.ItemCode;
		_itemName.Text = _mtInvoiceRetailSourceRow.ItemName;
		_unit.Code = _mtInvoiceRetailSourceRow.UnitCode;
		_quantity.Value = _mtInvoiceRetailSourceRow.Quantity;
		_initPrice.Value = _mtInvoiceRetailSourceRow.Price;
		if (_existDiscountPercentControl)
		{
			_discountPercent.Value = _mtInvoiceRetailSourceRow.Discount;
		}
		if (_existDiscountPriceControl)
		{
			_discountPrice.Value = _mtInvoiceRetailSourceRow.DiscountPrice;
		}
		if (_existBonusControl)
		{
			_usedBonus.Value = _mtInvoiceRetailSourceRow.UsedBonus;
			_accumulatedBonus.Value = _mtInvoiceRetailSourceRow.AccumulatedBonus;
		}
		_summa.Value = _mtInvoiceRetailSourceRow.Summa;
		_iItemTypeCode = _mtInvoiceRetailSourceRow.ItemType;
		isPartySelected = _mtInvoiceRetailSourceRow.IsPartySelected.HasValue && _mtInvoiceRetailSourceRow.IsPartySelected.Value;
		if (_vatEnabled)
		{
			_vat.IsChecked = _mtInvoiceRetailSourceRow.VAT;
		}
		_rowDesc.Text = _mtInvoiceRetailSourceRow.RowDescription;
		if (_useEnvFees)
		{
			_envFeePercent.Value = _mtInvoiceRetailSourceRow.EnvFeePercent;
			_envFeeSumma.Value = _mtInvoiceRetailSourceRow.EnvFeeSumma;
		}
		if (_vatEnabled)
		{
			if (_vat.IsChecked.Value)
			{
				_dealType.Code = "";
				_dealType.IsReadOnly = true;
			}
			else
			{
				_dealType.IsReadOnly = false;
				_dealType.Code = (from accDealTypesSE in SmartEnum<AccDealTypesSE, short>.List
					where accDealTypesSE.Value.ToString() == _mtInvoiceRetailSourceRow.DealType
					select accDealTypesSE.ValueUI).FirstOrDefault()?.ToString() ?? "";
			}
		}
		else
		{
			_dealType.IsReadOnly = false;
			_dealType.Code = (from accDealTypesSE in SmartEnum<AccDealTypesSE, short>.List
				where accDealTypesSE.Value.ToString() == _mtInvoiceRetailSourceRow.DealType
				select accDealTypesSE.ValueUI).FirstOrDefault()?.ToString() ?? "";
		}
		if (_allowAddRowAndNotViewMode)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item != null)
			{
				_expMethod = item.ExpenseMethod;
				_isBaseUnit = item.Unit == _unit.Code;
				_unit.DataView = new MTQntUnits(_itemCode.Code);
			}
			else if (!string.IsNullOrWhiteSpace(_itemCode.Code))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _itemCode.Code));
			}
		}
	}

	private void InnerLoadForBill(out bool isPartySelected)
	{
		if (_mode == DialogWindowMode.NewModeWithNext)
		{
			_rowId = _maxRowId;
		}
		else
		{
			_rowId = _mtBillSourceRow.RowID;
			_oldItemCode = _mtBillSourceRow.ItemCode;
		}
		_storage.Code = _mtBillSourceRow.Storage;
		if (_showStorageColInBill)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtBillSourceRow.ItemCode);
			if (item != null)
			{
				if (item.Type == ItemTypesSE.Goods.Value)
				{
					_storage.IsRequired = true;
					_storage.IsReadOnly = false;
				}
				else
				{
					_storage.IsRequired = false;
					_storage.IsReadOnly = true;
				}
			}
			_itemCode.DataView = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter(_docDate, _storage.Code, _storageRetail, _docTypeHasDiscount, _storageAllowNegExp);
		}
		_itemCode.Code = _mtBillSourceRow.ItemCode;
		_itemName.Text = _mtBillSourceRow.ItemName;
		_unit.Code = _mtBillSourceRow.UnitCode;
		_quantity.Value = _mtBillSourceRow.Quantity;
		_initPrice.Value = _mtBillSourceRow.Price;
		if (_existDiscountPercentControl)
		{
			_discountPercent.Value = _mtBillSourceRow.Discount;
		}
		if (_existDiscountPriceControl)
		{
			_discountPrice.Value = _mtBillSourceRow.DiscountPrice;
		}
		if (_existBonusControl)
		{
			_usedBonus.Value = _mtBillSourceRow.UsedBonus;
			_accumulatedBonus.Value = _mtBillSourceRow.AccumulatedBonus;
		}
		_summa.Value = _mtBillSourceRow.Summa;
		_iItemTypeCode = _mtBillSourceRow.ItemType;
		isPartySelected = _mtBillSourceRow.IsPartySelected.HasValue && _mtBillSourceRow.IsPartySelected.Value;
		if (_vatEnabled)
		{
			_vat.IsChecked = _mtBillSourceRow.VAT;
		}
		if (_allowAddRowAndNotViewMode)
		{
			MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item2 != null)
			{
				_expMethod = item2.ExpenseMethod;
				_isBaseUnit = item2.Unit == _unit.Code;
				_unit.DataView = new MTQntUnits(_itemCode.Code);
			}
			else if (!string.IsNullOrWhiteSpace(_itemCode.Code))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _itemCode.Code));
			}
		}
	}

	private void InnerLoadForMTMove(out bool isPartySelected)
	{
		if (_mode == DialogWindowMode.NewModeWithNext)
		{
			_rowId = _maxRowId;
		}
		else
		{
			_rowId = _mtMoveSourceRow.RowID;
			_oldItemCode = _mtMoveSourceRow.MTCode;
		}
		_itemCode.Code = _mtMoveSourceRow.MTCode;
		_itemName.Text = _mtMoveSourceRow.MTName;
		_unit.Code = _mtMoveSourceRow.UnitCode;
		_quantity.Value = _mtMoveSourceRow.Quantity;
		isPartySelected = _mtMoveSourceRow.IsPartySelected;
		if (_allowAddRowAndNotViewMode)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item != null)
			{
				_expMethod = item.ExpenseMethod;
				_isBaseUnit = item.Unit == _unit.Code;
				_unit.DataView = new MTQntUnits(_itemCode.Code);
			}
			else if (!string.IsNullOrWhiteSpace(_itemCode.Code))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _itemCode.Code));
			}
		}
		_rowDesc.Text = _mtMoveSourceRow.RowDescription;
		if (_existPriceSummControls)
		{
			_initPrice.Value = _mtMoveSourceRow.Price;
			_summa.Value = _mtMoveSourceRow.Summa;
		}
	}

	private void InnerLoadForMTMoveWithConfirmation(out bool isPartySelected)
	{
		if (_mode == DialogWindowMode.NewModeWithNext)
		{
			_rowId = _maxRowId;
		}
		else
		{
			_rowId = _mtMoveWithConfirmationSourceRow.RowID;
			_oldItemCode = _mtMoveWithConfirmationSourceRow.MTCode;
		}
		_itemCode.Code = _mtMoveWithConfirmationSourceRow.MTCode;
		_itemName.Text = _mtMoveWithConfirmationSourceRow.MTName;
		_unit.Code = _mtMoveWithConfirmationSourceRow.UnitCode;
		_quantity.Value = _mtMoveWithConfirmationSourceRow.Quantity;
		isPartySelected = _mtMoveWithConfirmationSourceRow.IsPartySelected;
		if (_allowAddRowAndNotViewMode)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item != null)
			{
				_expMethod = item.ExpenseMethod;
				_isBaseUnit = item.Unit == _unit.Code;
				_unit.DataView = new MTQntUnits(_itemCode.Code);
			}
			else if (!string.IsNullOrWhiteSpace(_itemCode.Code))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _itemCode.Code));
			}
		}
		_rowDesc.Text = _mtMoveWithConfirmationSourceRow.RowDescription;
		if (_existPriceSummControls)
		{
			_initPrice.Value = _mtMoveWithConfirmationSourceRow.Price;
			_summa.Value = _mtMoveWithConfirmationSourceRow.Summa;
		}
	}

	private void SetReadOnly()
	{
		if (_docTypeHasDiscount || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
		{
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value)
			{
				_itemType.IsReadOnly = true;
			}
			_storage.IsReadOnly = true;
		}
		_itemCode.IsReadOnly = true;
		_itemName.IsReadOnly = true;
		_unit.IsReadOnly = true;
		_quantity.IsReadOnly = true;
		if (_existPriceSummControls)
		{
			_initPrice.IsReadOnly = true;
			if (_existDiscountPercentControl)
			{
				_discountPercent.IsReadOnly = true;
			}
			if (_existDiscountPriceControl)
			{
				_discountPrice.IsReadOnly = true;
			}
			_summa.IsReadOnly = true;
			if (_vatEnabled)
			{
				_vat.IsReadOnly = true;
			}
		}
		else if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value)
		{
			_itemType.IsReadOnly = true;
			_summa.IsReadOnly = true;
		}
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value)
		{
			_dealType.IsReadOnly = true;
		}
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value)
		{
			_rowDesc.IsReadOnly = true;
		}
		if (_docTypeHasDiscount && _useEnvFees)
		{
			_envFeePercent.IsReadOnly = true;
			_envFeeSumma.IsReadOnly = true;
		}
		if (_showPartyColInBill)
		{
			_party.IsReadOnly = true;
			_liParty.IsReadOnly = true;
			SetPartiesGridReadOnly();
		}
		if (_showPartyUSEABLEDATE)
		{
			_fefo.IsEnabled = false;
		}
	}

	private LayoutItemWithRequired AddLayoutItem(string caption, LayoutGroup container = null)
	{
		LayoutItemWithRequired layoutItemWithRequired = new LayoutItemWithRequired
		{
			Label = caption
		};
		if (container == null)
		{
			LayoutControl.Children.Add(layoutItemWithRequired);
		}
		else
		{
			container.Children.Add(layoutItemWithRequired);
		}
		layoutItemWithRequired.HorizontalAlignment = HorizontalAlignment.Left;
		return layoutItemWithRequired;
	}

	private LayoutGroup AddLayoutGroup(LayoutGroup container, LayoutGroupView view = LayoutGroupView.Group, Orientation orientation = Orientation.Horizontal, string header = "")
	{
		LayoutGroup layoutGroup = new LayoutGroup
		{
			View = view,
			Orientation = orientation
		};
		if (!string.IsNullOrWhiteSpace(header))
		{
			layoutGroup.Header = header;
		}
		if (container == null)
		{
			LayoutControl.Children.Add(layoutGroup);
		}
		else
		{
			container.Children.Add(layoutGroup);
		}
		return layoutGroup;
	}

	private void PartiesTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		if (e.Column != _expQuantityCol)
		{
			return;
		}
		for (int i = _mtCodeSelectedPartiesList.Count - 1; i >= 0; i += -1)
		{
			int rowHandleByListIndex = _partiesGrid.Grid.GetRowHandleByListIndex(i);
			if (rowHandleByListIndex != e.RowHandle)
			{
				continue;
			}
			if (!_storageAllowNegExp)
			{
				if (Convert.ToDecimal(e.Value) > _mtCodeSelectedPartiesList[i].Qty || Convert.ToDecimal(e.Value) < 0m)
				{
					_mtCodeSelectedPartiesList[i].ExpQty = _mtCodeSelectedPartiesList[i].Qty;
				}
			}
			else if (Convert.ToDecimal(e.Value) < 0m)
			{
				_mtCodeSelectedPartiesList[i].ExpQty = _mtCodeSelectedPartiesList[i].Qty;
			}
			_partiesGrid.Grid.RefreshRow(rowHandleByListIndex);
			break;
		}
	}

	private void PartiesTableView_CellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		_partiesTableView.CommitEditing();
	}

	private void PartiesTableView_RowUpdated(object sender, RowEventArgs e)
	{
		_partiesGrid.Grid.RefreshData();
		_changedQuantityFromGrid = true;
		_quantity.EditValue = Convert.ToDecimal(_expQuantityCol.TotalSummaries[0].Value);
		_changedQuantityFromGrid = false;
	}

	private void PartiesGrid_EscapePressed(object sender, EventArgs e)
	{
		DialogResult = false;
	}

	private void CancelButton_Click(object sender, RoutedEventArgs e)
	{
		DialogResult = false;
	}

	private void OnGridControlEnterPressed(object sender, EventArgs e)
	{
		_partiesTableView.CommitEditing();
	}

	private void RestoreSettings()
	{
		bool flag = false;
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\GoodsSelectPartyWindow", writable: false);
			if (registryKey == null)
			{
				flag = true;
			}
			else
			{
				Settings.RestoreWindowSize(this, registryKey, 796.0, 530.0);
				bool flag2 = registryKey.GetValue("State", "1").ToString() == "0";
				if (!_showPartyColInBill)
				{
					Height = SizeCalculator.Height(260.0);
				}
				if (flag2)
				{
					WindowState = WindowState.Maximized;
				}
				else
				{
					WindowState = WindowState.Normal;
				}
			}
		}
		catch (Exception)
		{
			flag = true;
		}
		if (flag)
		{
			Width = SizeCalculator.Width(796.0);
			Height = SizeCalculator.Height(530.0);
		}
	}

	private void StoreSettings()
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\GoodsSelectPartyWindow");
			if (registryKey != null)
			{
				if (WindowState == WindowState.Normal)
				{
					registryKey.SetValue("Width", Width.ToString());
					registryKey.SetValue("Height", Height.ToString());
					registryKey.SetValue("State", "1");
				}
				else if (WindowState == WindowState.Maximized)
				{
					registryKey.SetValue("State", "0");
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void GoodsSelectPartyWindow_Closing(object sender, CancelEventArgs e)
	{
		if (!e.Cancel)
		{
			StoreSettings();
		}
	}

	private void GoodsSelectPartyWindow_Loaded(object sender, RoutedEventArgs e)
	{
		_itemCode.InnerCodeEdit.Focus();
		if ((_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value || (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value && _itemType.Code == ItemTypesSE.Goods.Value)) && string.IsNullOrWhiteSpace(_storage.Code))
		{
			_storage.InnerCodeEdit.Focus();
		}
	}

	private void GoodsSelectPartyWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.Key == 93 && (int)Keyboard.Modifiers == 2)
			{
				e.Handled = true;
				Close();
			}
			else if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 2)
			{
				e.Handled = true;
				if (_mode == DialogWindowMode.ViewMode)
				{
					DialogResult = true;
				}
				else if (_mode == DialogWindowMode.EditMode)
				{
					OkButton_Click(sender, e);
				}
				else if (_mode == DialogWindowMode.NewModeWithNext)
				{
					NextButton_Click(sender, e);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ItemType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		CleanSelectedPartiesList(e.OldValue?.ToString());
		CleanAllFieldExceptStorage();
		if (_allowAddRowAndNotViewMode)
		{
			if (!string.IsNullOrWhiteSpace(_itemType.Code) && _itemType.DoValidate())
			{
				ChangeItemCodeColumnAdditonalView(isChangedTypeItem: true);
			}
		}
		else if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value && _itemType.Code == ItemTypesSE.Services.Value)
		{
			LayoutItemWithRequired layoutItemWithRequired = (LayoutItemWithRequired)(object)_summa.Parent;
			_summa.Visibility = Visibility.Visible;
			layoutItemWithRequired.Visibility = Visibility.Visible;
		}
		_itemType.DoValidate();
	}

	private void ChangeItemCodeColumnAdditonalView(bool isChangedTypeItem = false)
	{
		string text = ItemTypesSE.Goods.Value;
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value)
		{
			text = _itemType.Code;
		}
		if (!isChangedTypeItem)
		{
			return;
		}
		if (_docTypeHasDiscount || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value)
		{
			if (text == ItemTypesSE.Goods.Value)
			{
				_itemName.IsReadOnly = true;
				_itemName.IsEnabled = _mode == DialogWindowMode.ViewMode;
			}
			else
			{
				_itemName.IsReadOnly = false;
				_itemName.IsEnabled = true;
			}
		}
		if (_docTypeHasDiscount && _useEnvFees)
		{
			if (text == InvoiceRowTypeSE.Services.Value)
			{
				_envFeePercent.EditValue = 0m;
				_envFeeSumma.EditValue = 0m;
				_envFeePercent.IsReadOnly = true;
				_envFeeSumma.IsReadOnly = true;
				_envFeePercent.IsEnabled = _mode == DialogWindowMode.ViewMode;
				_envFeeSumma.IsEnabled = _mode == DialogWindowMode.ViewMode;
			}
			else
			{
				_envFeePercent.IsReadOnly = false;
				_envFeeSumma.IsReadOnly = false;
				_envFeePercent.IsEnabled = true;
				_envFeeSumma.IsEnabled = true;
			}
		}
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value)
		{
			_summa.IsRequired = text == ItemTypesSE.Services.Value;
			LayoutItemWithRequired layoutItemWithRequired = (LayoutItemWithRequired)(object)_summa.Parent;
			if (_summa.IsRequired)
			{
				_summa.Visibility = Visibility.Visible;
				layoutItemWithRequired.Visibility = Visibility.Visible;
			}
			else
			{
				_summa.Visibility = Visibility.Collapsed;
				layoutItemWithRequired.Visibility = Visibility.Collapsed;
			}
		}
		if (text == ItemTypesSE.Goods.Value)
		{
			_storage.IsRequired = true;
			_storage.IsReadOnly = false;
			_storage.IsEnabled = true;
			if (string.IsNullOrWhiteSpace(_storage.Code))
			{
				_storage.Code = _docStorage;
			}
			if (_showPartyColInBill)
			{
				_party.IsReadOnly = false;
				_party.IsEnabled = true;
			}
			_partiesGrid.IsReadOnly = false;
		}
		else
		{
			_storage.Code = "";
			_storage.IsReadOnly = true;
			_storage.IsEnabled = false;
			_storage.IsRequired = false;
			if (_showPartyColInBill)
			{
				_party.IsReadOnly = true;
				_party.IsEnabled = false;
				SetPartiesGridReadOnly(isChangeAvailableQuantityColVisibilty: false);
			}
			CleanParties();
		}
		_storage.DoValidate();
		if (_showPartyColInBill)
		{
			_party.IsChecked = false;
			Party_Changed();
		}
	}

	private void SetPartiesGridReadOnly(bool isChangeAvailableQuantityColVisibilty = true)
	{
		_partiesGrid.IsReadOnly = true;
		if (isChangeAvailableQuantityColVisibilty)
		{
			_availableQuantityCol.Visible = false;
		}
	}

	private void Group_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_group.DoValidate() && _allowAddRowAndNotViewMode)
		{
			ChangeItemCodeColumnAdditonalView();
		}
		if (!string.IsNullOrWhiteSpace(_group.Code) && !string.IsNullOrWhiteSpace(_itemCode.Code) && !_itemCode.DoValidate())
		{
			_itemCode.Code = "";
			_itemCode.DoValidate();
		}
	}

	private void ItemCode_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		string text = ItemTypesSE.Goods.Value;
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value)
		{
			text = _itemType.Code;
		}
		IDropDownView dropDownView;
		if (text == ItemTypesSE.Goods.Value)
		{
			dropDownView = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter(_docDate, _storage.Code, _storageRetail, _docTypeHasDiscount && _docType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value, _storageAllowNegExp, _group.Code);
		}
		else
		{
			dropDownView = ((!(text == ItemTypesSE.Services.Value)) ? new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, "0", _group.Code, _showInPriceList) : new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, ItemTypesSE.Services, _group.Code, _showInPriceList));
		}
		if (dropDownView != null)
		{
			ModalBrowser modalBrowser = new ModalBrowser(dropDownView)
			{
				LookUpValue = _itemCode.Code,
				Owner = e.Owner,
				Left = e.Left,
				Top = e.Top
			};
			if (modalBrowser.Show())
			{
				_itemCode.Code = dropDownView.ValueMember(modalBrowser.SelectedRowIndex);
			}
		}
		e.Cancel = true;
	}

	private void ItemCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_itemCode.Code) && _itemCode.DoValidate())
		{
			if (_editableMode)
			{
				try
				{
					int num = -1;
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
					if (item != null)
					{
						num = item.Id;
						_ = item.Unit;
					}
					_iItemTypeCode = item.Type;
					if (_docTypeHasDiscount && _existDiscountPercentControl)
					{
						_discountPercent.Value = _docDiscountPercent;
						if (_discountPercent.Value == 0m)
						{
							_discountPercent.Value = item.Discount;
						}
						else if (item.Discount != 0m)
						{
							_discountPercent.Value = item.Discount;
						}
					}
					if (item.Type == ItemTypesSE.Goods.Value)
					{
						UpdateMaterialData(item);
					}
					else
					{
						UpdateServiceData(item);
					}
					if (_docTypeHasDiscount && num != -1)
					{
						UpdateInvoiceSumm();
					}
					if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && _showStorageColInBill)
					{
						if (item.Type == ItemTypesSE.Goods.Value)
						{
							_storage.IsRequired = true;
							_storage.IsReadOnly = false;
						}
						else
						{
							_storage.IsRequired = false;
							_storage.IsReadOnly = true;
							_storage.Code = "";
						}
					}
					if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value)
					{
						_rowDesc.Text = "";
					}
				}
				catch (Exception ex)
				{
					Settings.ErrorHandler(ex);
				}
			}
		}
		else
		{
			_itemName.Text = "";
			_unit.Code = "";
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value)
			{
				_rowDesc.Text = "";
			}
			if (_allowAddRowAndNotViewMode)
			{
				_unit.DataView = new MTQntUnits("", isShowQntUnitCaption: false, isShowOnlyComplects: false, _group.Code, _docTypeHasDiscount ? ItemTypesSE.All.Value : ItemTypesSE.Goods.Value);
			}
			SetQuantityToZero();
		}
		CleanSelectedPartiesList();
	}

	private void ItemCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		try
		{
			int num = -1;
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item != null)
			{
				num = item.Id;
				_ = item.Unit;
			}
			if (item.Type == ItemTypesSE.Goods.Value)
			{
				UpdateMaterialData(item, isSetQuantityToZero: false);
			}
			else
			{
				UpdateServiceData(item);
			}
			if (_docTypeHasDiscount && num != -1)
			{
				UpdateInvoiceSumm();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void Unit_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_itemCode.Code) && _itemCode.DoValidate())
		{
			return;
		}
		MTQntUnits mTQntUnits = new MTQntUnits("", isShowQntUnitCaption: false, isShowOnlyComplects: false, _group.Code, _docTypeHasDiscount ? ItemTypesSE.All.Value : ItemTypesSE.Goods.Value);
		ModalBrowser modalBrowser = new ModalBrowser(mTQntUnits)
		{
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			MTQntUnitsDataProvider.DataRow.MTQntUnitsRowKey mTQntUnitsRowKey = (MTQntUnitsDataProvider.DataRow.MTQntUnitsRowKey)mTQntUnits.KeyMember(modalBrowser.SelectedRowIndex);
			if (mTQntUnitsRowKey != null)
			{
				MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(mTQntUnitsRowKey.MTID);
				if (itemById != null)
				{
					_itemCode.Code = itemById.MTCode;
					_itemCode.DoValidate();
				}
			}
			_unit.Code = mTQntUnits.ValueMember(modalBrowser.SelectedRowIndex);
		}
		e.Cancel = true;
	}

	private void Unit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_mtCodeSelectedPartiesList == null || _mtCodeSelectedPartiesList.Count <= 0 || string.IsNullOrWhiteSpace(_unit.Code) || !_unit.DoValidate())
		{
			return;
		}
		int goodID = _mtCodeSelectedPartiesList[0].GoodID;
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(goodID);
		if (itemById == null)
		{
			return;
		}
		_isBaseUnit = itemById.Unit == _unit.Code;
		string text = e.OldValue.ToString();
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		try
		{
			bool flag = itemById.Unit == text;
			decimal num = 0m;
			foreach (PartyRow mtCodeSelectedParties in _mtCodeSelectedPartiesList)
			{
				if (flag)
				{
					decimal coef = TradeSettings.TradeCacheService.Material.GetQntUnit(mtCodeSelectedParties.GoodID, _unit.Code, throwExceptionNotExits: true).Coef;
					mtCodeSelectedParties.Qty = (mtCodeSelectedParties.Qty / coef).RoundAs(3);
					continue;
				}
				if (_isBaseUnit)
				{
					decimal coef = TradeSettings.TradeCacheService.Material.GetQntUnit(mtCodeSelectedParties.GoodID, text, throwExceptionNotExits: true).Coef;
					mtCodeSelectedParties.Qty = (mtCodeSelectedParties.Qty * coef).RoundAs(3);
					continue;
				}
				decimal coef2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtCodeSelectedParties.GoodID, text, throwExceptionNotExits: true).Coef;
				num = TradeSettings.TradeCacheService.Material.GetQntUnit(mtCodeSelectedParties.GoodID, _unit.Code, throwExceptionNotExits: true).Coef;
				if (num != 0m)
				{
					mtCodeSelectedParties.Qty = (mtCodeSelectedParties.Qty / num * coef2).RoundAs(3);
				}
			}
			if (_distrByFEFO)
			{
				_party.IsChecked = true;
				DistributeQtyByExpiredDate(_quantity.Value);
			}
			if (_showPartyColInBill)
			{
				_partiesGrid.Grid.RefreshData();
			}
			if (!_existPriceSummControls)
			{
				return;
			}
			if (!_storageRetail)
			{
				if (!string.IsNullOrWhiteSpace(_priceListType))
				{
					decimal num2 = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, goodID, _unit.Code, _docDate, _currCode);
					if (num2 == 0m)
					{
						num2 = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(text, _unit.Code, _initPrice.Value, itemById.MTCode);
					}
					_initPrice.Value = Methods.RoundPrice(num2, _priceListTypeRoundDgr);
				}
				else
				{
					MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(goodID, e.OldValue.ToString());
					if (qntUnit != null)
					{
						_initPrice.Value = (_initPrice.Value / qntUnit.Coef * num).RoundAs(2);
						_initPrice.Value = Methods.RoundPrice(_initPrice.Value, _priceListTypeRoundDgr);
					}
				}
				return;
			}
			_initPrice.Value = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(text, _unit.Code, _initPrice.Value, itemById.MTCode);
			if (string.IsNullOrWhiteSpace(_priceListType) || !_useDiscount)
			{
				return;
			}
			if (_existDiscountPriceControl)
			{
				_discountPrice.Value = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, goodID, _unit.Code, _docDate, _currCode);
				_discountPrice.Value = Methods.RoundPrice(_discountPrice.Value, _priceListTypeRoundDgr);
			}
			if (!_existDiscountPercentControl)
			{
				return;
			}
			if (_initPrice.Value == 0m)
			{
				if (_discountPercent.Value == 0m)
				{
					_discountPercent.Value = itemById.Discount;
				}
				else if (itemById.Discount != 0m)
				{
					_discountPercent.Value = itemById.Discount;
				}
				if (_discountPercent.Value != 0m)
				{
					_initPrice.Value = (_discountPrice.Value * 100m / (100m - _discountPercent.Value)).RoundAs(2);
				}
				else
				{
					_initPrice.Value = _discountPrice.Value;
				}
			}
			else
			{
				_discountPercent.Value = (100m * (_initPrice.Value - _discountPrice.Value) / _initPrice.Value).RoundAs(2);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void UpdateMaterialData(MaterialCacheRow good, bool isSetQuantityToZero = true)
	{
		if (_docTypeHasDiscount)
		{
			_itemName.Text = (string.IsNullOrWhiteSpace(good.Caption) ? good.FullCaption : good.Caption);
			if (_useEnvFees)
			{
				_envFeePercent.EditValue = good.EcoFeeRate;
			}
		}
		else
		{
			_itemName.Text = good.Caption;
		}
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && !_storageRetail)
		{
			_unit.Code = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(good.Id, _priceListType, _docDate);
			if (string.IsNullOrWhiteSpace(_unit.Code))
			{
				_unit.Code = good.Unit;
			}
		}
		else
		{
			_unit.Code = good.Unit;
		}
		if (_vatEnabled)
		{
			_vat.IsChecked = good.VAT;
			if ((_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value) && !_vat.IsChecked.Value)
			{
				_dealType.Code = _docDealType;
			}
		}
		else if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value)
		{
			_dealType.Code = _docDealType;
		}
		_isBaseUnit = good.Unit == _unit.Code;
		if (_allowAddRowAndNotViewMode)
		{
			_unit.DataView = new MTQntUnits(_itemCode.Code);
			_unit.DoValidate();
		}
		if (_expMethod != good.ExpenseMethod && !isSetQuantityToZero && _showPartyColInBill)
		{
			_party.IsChecked = false;
			_party.IsReadOnly = false;
			_liParty.IsReadOnly = false;
		}
		_expMethod = good.ExpenseMethod;
		ChangeExpMethod();
		if (isSetQuantityToZero)
		{
			CleanSelectedPartiesList();
			_quantity.EditValue = 0m;
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
			{
				_salePrice.EditValue = 0m;
				_saleSumma.EditValue = 0m;
			}
			_mtCodeSelectedPartiesList = FillPartiesGrid();
		}
		if (_storageEditable)
		{
			if (string.IsNullOrWhiteSpace(_storage.Code))
			{
				_storage.Code = _docStorage;
			}
			_storage.IsReadOnly = false;
			_storage.IsEnabled = true;
			_storage.IsRequired = true;
		}
		if (!_existPriceSummControls)
		{
			return;
		}
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
		{
			if (_storageRetail)
			{
				_salePrice.EditValue = GetPriceForExpenseMove(good.Id, _unit.Code);
			}
		}
		else if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && !_storageRetail))
		{
			_initPrice.EditValue = GetPriceForGivenPriceList(good.Id, _unit.Code);
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && !_storageRetail))
			{
				_initPrice.EditValue = Methods.RoundPrice(Convert.ToDecimal(_initPrice.EditValue), _priceListTypeRoundDgr);
			}
		}
		else
		{
			_initPrice.EditValue = GetPriceForExpenseMove(good.Id, _unit.Code);
		}
	}

	private void UpdateServiceData(MaterialCacheRow service)
	{
		if (_docTypeHasDiscount)
		{
			_itemName.Text = (string.IsNullOrWhiteSpace(service.Caption) ? service.FullCaption : service.Caption);
		}
		else
		{
			_itemName.Text = service.Caption;
		}
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
		{
			_unit.Code = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(service.Id, _priceListType, _docDate);
			if (string.IsNullOrWhiteSpace(_unit.Code))
			{
				_unit.Code = service.Unit;
			}
		}
		else
		{
			_unit.Code = service.Unit;
		}
		if (_vatEnabled)
		{
			_vat.IsChecked = service.VAT;
			if ((_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value) && !_vat.IsChecked.Value)
			{
				_dealType.Code = _docDealType;
			}
		}
		_isBaseUnit = service.Unit == _unit.Code;
		if (_allowAddRowAndNotViewMode)
		{
			_unit.DataView = new MTQntUnits(_itemCode.Code);
			_unit.DoValidate();
		}
		if (_existPriceSummControls)
		{
			_initPrice.EditValue = GetPriceForGivenPriceList(service.Id, _unit.Code);
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && !_storageRetail)
			{
				_initPrice.EditValue = Methods.RoundPrice(Convert.ToDecimal(_initPrice.EditValue), _priceListTypeRoundDgr);
			}
		}
		if (_storageEditable)
		{
			_storage.Code = "";
			_storage.IsReadOnly = true;
			_storage.IsEnabled = false;
			_storage.IsRequired = false;
		}
		_expMethod = "";
		ChangeExpMethod();
	}

	private void UpdateInvoiceSumm()
	{
		decimal num;
		if (_existDiscountPriceControl)
		{
			if ((_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value) && !string.IsNullOrWhiteSpace(_discountPriceListType))
			{
				num = CalculateDiscountPrice();
			}
			else
			{
				num = ((!_existDiscountPercentControl) ? (_initPrice.Value - _initPrice.Value * _docDiscountPercent / 100m).RoundAs(2) : (_initPrice.Value - _initPrice.Value * _discountPercent.Value / 100m).RoundAs(2));
			}
			_discountPrice.EditValue = num;
		}
		else
		{
			num = _initPrice.Value;
		}
		UpdateSaleSumma(num);
		if (_useEnvFees)
		{
			_envFeeSumma.EditValue = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(_summa.Value, _envFeePercent.Value, _vatEnabled && _vat.IsChecked.Value, _vatType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
		}
	}

	private void CleanSelectedPartiesList(string itemTypeOldValue = "")
	{
		string text = ItemTypesSE.Goods.Value;
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value)
		{
			text = _itemType.Code;
		}
		else if (_docTypeHasDiscount)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item != null)
			{
				text = item.Type;
			}
		}
		if (_showPartyColInBill && (text == ItemTypesSE.Goods.Value || (itemTypeOldValue == ItemTypesSE.Goods.Value && _editableMode && _party.IsChecked.Value)))
		{
			_party.IsChecked = false;
		}
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_editableMode)
		{
			return;
		}
		if (!string.IsNullOrWhiteSpace(_storage.Code) && _storage.DoValidate())
		{
			_storageRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code)?.Retail ?? false;
			_storageAllowNegExp = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(_storage.Code);
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
			{
				_salePrice.EditValue = 0m;
				_saleSumma.EditValue = 0m;
			}
		}
		CleanSelectedPartiesList();
		_mtCodeSelectedPartiesList = FillPartiesGrid();
		if (_mtCodeSelectedPartiesList.Count > 0 && _quantity.Value > 0m && !CanDivideByParties(_quantity.Value))
		{
			SetQuantityToZero();
		}
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && _showStorageColInBill)
		{
			_itemCode.DataView = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter(_docDate, _storage.Code, _storageRetail, _docTypeHasDiscount, _storageAllowNegExp);
		}
	}

	private void SetQuantityToZero()
	{
		_quantity.EditValue = 0m;
		_quantity.DoValidate();
		if (_existPriceSummControls)
		{
			_initPrice.EditValue = 0m;
			if (_existDiscountPriceControl)
			{
				_discountPrice.EditValue = 0m;
			}
			_summa.EditValue = 0m;
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
			{
				_salePrice.EditValue = 0m;
				_saleSumma.EditValue = 0m;
			}
		}
		if (_mtCodeSelectedPartiesList != null)
		{
			foreach (PartyRow mtCodeSelectedParties in _mtCodeSelectedPartiesList)
			{
				mtCodeSelectedParties.ExpQty = 0m;
			}
		}
		if (_showPartyColInBill)
		{
			_partiesGrid.Grid.RefreshData();
		}
	}

	private void Quantity_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue == null || !_editableMode)
		{
			return;
		}
		string text = ItemTypesSE.Goods.Value;
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value)
		{
			text = _itemType.Code;
		}
		else if (_docTypeHasDiscount)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item != null)
			{
				text = item.Type;
			}
		}
		if (!_changedQuantityFromGrid)
		{
			if (text == ItemTypesSE.Goods.Value)
			{
				if (_showPartyColInBill && ((Convert.ToDecimal(e.OldValue) != Convert.ToDecimal(e.NewValue) && Convert.ToDecimal(e.OldValue) != 0m) || !_quantity.DoValidate()) && _party.IsChecked.HasValue && _party.IsChecked.Value)
				{
					if (_quantity.DoValidate())
					{
						ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ForRowSelectedParties, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
					}
					_party.IsChecked = false;
				}
				if (_showPartyColInBill && (!_quantity.DoValidate() || !CanDivideByParties(_quantity.Value)))
				{
					e.Handled = true;
					_quantity.Value = 0m;
					SetQuantityToZero();
				}
			}
			else if (!_quantity.DoValidate())
			{
				e.Handled = true;
				_quantity.Value = 0m;
				SetQuantityToZero();
			}
		}
		if (_existPriceSummControls)
		{
			_editableMode = false;
			decimal price = ((!_existDiscountPriceControl) ? _initPrice.Value : _discountPrice.Value);
			UpdateSaleSumma(price);
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
			{
				UpdateRetSaleSumma(_salePrice.Value);
			}
			_editableMode = true;
		}
		if (_docTypeHasDiscount && _useEnvFees && _iItemTypeCode == InvoiceRowTypeSE.Goods.Value)
		{
			_envFeeSumma.EditValue = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(_summa.Value, _envFeePercent.Value, _vatEnabled && _vat.IsChecked.Value, _vatType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			_envFeeSumma.DoValidate();
		}
	}

	private decimal GetPriceForExpenseMove(int mtID, string unitCode)
	{
		decimal result = 0m;
		if (_storageRetail)
		{
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtID, unitCode);
			if (qntUnit != null)
			{
				MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
				{
					MTCode = _itemCode.Code,
					UnitCoef = qntUnit.Coef,
					Storage = _storage.Code,
					Date = _docDate
				};
				List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
				if (rows.Count != 0)
				{
					return rows[0].Price;
				}
			}
		}
		else if ((_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value) && _storageIncomeRetail)
		{
			return GetPriceForGivenPriceList(mtID, unitCode);
		}
		return result;
	}

	private decimal GetPriceForGivenPriceList(int mtID, string unitCode)
	{
		return TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, mtID, unitCode, _docDate, _currCode);
	}

	private void InitPrice_ColumnDropDownClick(object sender, RoutedEventArgs e)
	{
		InitPrice_ColumnDropDownClick();
	}

	private void SalePrice_ColumnDropDownClick(object sender, RoutedEventArgs e)
	{
		SalePrice_ColumnDropDownClick();
	}

	private void InitPrice_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		if (_existDropDownClickForPriceControl && _allowAddRowAndNotViewMode && ((((int)e.Key == 20 || (int)e.Key == 93) && (int)Keyboard.Modifiers == 0) || ((int)e.Key == 26 && (int)Keyboard.Modifiers == 2)))
		{
			InitPrice_ColumnDropDownClick();
		}
	}

	private void SalePrice_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		if (_existDropDownClickForSalePriceControl && _allowAddRowAndNotViewMode && ((((int)e.Key == 20 || (int)e.Key == 93) && (int)Keyboard.Modifiers == 0) || ((int)e.Key == 26 && (int)Keyboard.Modifiers == 2)))
		{
			SalePrice_ColumnDropDownClick();
		}
	}

	private void InitPrice_ColumnDropDownClick()
	{
		try
		{
			if (string.IsNullOrWhiteSpace(_itemCode.Code) || !_itemCode.DoValidate() || string.IsNullOrWhiteSpace(_unit.Code) || !_unit.DoValidate())
			{
				return;
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item == null)
			{
				return;
			}
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(item.Id, _unit.Code);
			if (qntUnit != null)
			{
				MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(_itemCode.Code, qntUnit.Coef, _storage.Code, _docDate);
				ModalBrowser modalBrowser = new ModalBrowser(mTRetailQtyAndPrice)
				{
					LookUpValue = _itemCode.Code,
					Owner = this
				};
				double top = 0.0;
				double left = 0.0;
				GetCoordinatesForPopup_InitPrice(ref top, ref left);
				modalBrowser.Left = left;
				modalBrowser.Top = top;
				if (modalBrowser.Show())
				{
					_initPrice.EditValue = Convert.ToDecimal(mTRetailQtyAndPrice.CommentMember(modalBrowser.SelectedRowIndex));
					_initPrice.DoValidate();
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SalePrice_ColumnDropDownClick()
	{
		try
		{
			if (string.IsNullOrWhiteSpace(_itemCode.Code) || !_itemCode.DoValidate() || string.IsNullOrWhiteSpace(_unit.Code) || !_unit.DoValidate() || !_storageRetail)
			{
				return;
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item == null)
			{
				return;
			}
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(item.Id, _unit.Code);
			if (qntUnit != null)
			{
				MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(_itemCode.Code, qntUnit.Coef, _storage.Code, _docDate);
				ModalBrowser modalBrowser = new ModalBrowser(mTRetailQtyAndPrice)
				{
					LookUpValue = _itemCode.Code,
					Owner = this
				};
				double top = 0.0;
				double left = 0.0;
				GetCoordinatesForPopup_SalePrice(ref top, ref left);
				modalBrowser.Left = left;
				modalBrowser.Top = top;
				if (modalBrowser.Show())
				{
					_salePrice.EditValue = Convert.ToDecimal(mTRetailQtyAndPrice.CommentMember(modalBrowser.SelectedRowIndex));
					_salePrice.DoValidate();
					UpdateRetSaleSumma(_salePrice.Value);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void GetCoordinatesForPopup_InitPrice(ref double top, ref double left)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Point val = _initPrice.PointToScreen(new Point(0.0, 0.0));
		Matrix transformFromDevice = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
		Point val2 = transformFromDevice.Transform(val);
		top = val2.Y + _initPrice.ActualHeight;
		left = val2.X;
	}

	private void GetCoordinatesForPopup_SalePrice(ref double top, ref double left)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Point val = _salePrice.PointToScreen(new Point(0.0, 0.0));
		Matrix transformFromDevice = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
		Point val2 = transformFromDevice.Transform(val);
		top = val2.Y + _salePrice.ActualHeight;
		left = val2.X;
	}

	private void InitPrice_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue == null || !_editableMode)
		{
			return;
		}
		_editableMode = false;
		if (_existDiscountPriceControl)
		{
			if ((_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value) && !string.IsNullOrWhiteSpace(_discountPriceListType))
			{
				if (_initPrice.Value != 0m && _discountPrice.Value != 0m)
				{
					_discountPercent.EditValue = (100m * (_initPrice.Value - _discountPrice.Value) / _initPrice.Value).RoundAs(2);
				}
			}
			else
			{
				if (_existDiscountPercentControl)
				{
					_discountPrice.EditValue = CalcDiscountPrice(_initPrice.Value, _discountPercent.Value);
				}
				else
				{
					_discountPrice.EditValue = CalcDiscountPrice(_initPrice.Value, _docDiscountPercent);
				}
				_discountPrice.DoValidate();
			}
			UpdateSaleSumma(_discountPrice.Value);
		}
		else
		{
			UpdateSaleSumma(_initPrice.Value);
		}
		if (_useEnvFees && _iItemTypeCode != InvoiceRowTypeSE.Services.Value)
		{
			_envFeeSumma.EditValue = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(_summa.Value, _envFeePercent.Value, _vatEnabled && _vat.IsChecked.Value, _vatType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			_envFeeSumma.DoValidate();
		}
		_editableMode = true;
	}

	private void DiscountPercent_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue == null || !_editableMode)
		{
			return;
		}
		_editableMode = false;
		if (Convert.ToDecimal(e.NewValue) < -999.99m || Convert.ToDecimal(e.NewValue) > 100m)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidatePercent, (-999.99m).ToString(), 100m.ToString()), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
			e.Handled = true;
			_discountPercent.EditValue = Convert.ToDecimal(e.OldValue);
			_discountPercent.DoValidate();
		}
		else
		{
			decimal num = CalcDiscountPrice(_initPrice.Value, _discountPercent.Value);
			if ((_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value) && !_storageRetail)
			{
				num = Methods.RoundPrice(num, _priceListTypeRoundDgr);
			}
			_discountPrice.EditValue = num;
			_discountPrice.DoValidate();
			UpdateSaleSumma(num);
			if (_useEnvFees && _iItemTypeCode != InvoiceRowTypeSE.Services.Value)
			{
				_envFeeSumma.EditValue = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(_summa.Value, _envFeePercent.Value, _vatEnabled && _vat.IsChecked.Value, _vatType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
				_envFeeSumma.DoValidate();
			}
		}
		_editableMode = true;
	}

	private static decimal CalcDiscountPrice(decimal initPrice, decimal discountPercent)
	{
		return (initPrice - initPrice * discountPercent / 100m).RoundAs(2);
	}

	private decimal CalculateDiscountPrice()
	{
		PricesDataProvider.DataRow dataRow = null;
		if (!string.IsNullOrWhiteSpace(_discountPriceListType) && !string.IsNullOrWhiteSpace(_itemCode.Code))
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item != null)
			{
				dataRow = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_discountPriceListType, item.Id, _unit.Code, _docDate);
			}
		}
		if (dataRow != null && dataRow.Price != 0m)
		{
			decimal num;
			if (_discountPriceListCurr == _currCode)
			{
				num = dataRow.Price;
			}
			else
			{
				num = ((!(_discountPriceListCurr == "AMD")) ? (dataRow.Price * _discountPriceListCurrRate / _discountPriceListCurrBase / _docCurrRate * _docCurrBase).RoundAs(2) : (dataRow.Price / _docCurrRate * _docCurrBase).RoundAs(2));
			}
			if (_vatEnabled && _vat.IsChecked.Value && _discountPriceListVATType != _vatType)
			{
				string discountPriceListVATType = _discountPriceListVATType;
				if (!(discountPriceListVATType == "1"))
				{
					if (discountPriceListVATType == "2" && _vatType.InList("1", "3", "4"))
					{
						num -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(num, "2", _vat.IsChecked.Value);
					}
				}
				else if (_vatType == "2")
				{
					num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(num, "1", _vat.IsChecked.Value);
				}
			}
			return Methods.RoundPrice(num, _discountPriceTypeRoundDegree);
		}
		if (_discountPercent.Value != 0m)
		{
			return Methods.RoundPrice(_initPrice.Value * (100m - _docDiscountPercent) / 100m, _priceListTypeRoundDgr);
		}
		return _initPrice.Value;
	}

	private void DiscountPrice_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue == null || !_editableMode || _discountPrice.IsReadOnly)
		{
			return;
		}
		_editableMode = false;
		if (_existDiscountPercentControl)
		{
			decimal num = 0m;
			if (_initPrice.Value != 0m)
			{
				num = (100m * (_initPrice.Value - _discountPrice.Value) / _initPrice.Value).RoundAs(2);
			}
			if (num < -999.99m || num > 100m)
			{
				if ((_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value) && !_storageRetail)
				{
					_discountPrice.EditValue = Methods.RoundPrice(Convert.ToDecimal(e.OldValue), _priceListTypeRoundDgr);
				}
				else
				{
					_discountPrice.EditValue = Convert.ToDecimal(e.OldValue);
				}
				_discountPrice.DoValidate();
			}
			else
			{
				_discountPercent.EditValue = num;
				_discountPercent.DoValidate();
			}
		}
		UpdateSaleSumma(Convert.ToDecimal(_discountPrice.EditValue));
		if (_useEnvFees && _iItemTypeCode != InvoiceRowTypeSE.Services.Value)
		{
			_envFeeSumma.EditValue = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(_summa.Value, _envFeePercent.Value, _vatEnabled && _vat.IsChecked.Value, _vatType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			_envFeeSumma.DoValidate();
		}
		_editableMode = true;
	}

	private void Summa_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if ((!_docTypeHasDiscount && _docType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value) || e.NewValue == null || !_editableMode || !(_quantity.Value > 0m))
		{
			return;
		}
		_editableMode = false;
		decimal num = (_summa.Value / _quantity.Value).RoundAs(2);
		if (_existDiscountPriceControl)
		{
			_discountPrice.EditValue = num;
			_discountPrice.DoValidate();
			if (_initPrice.Value != 0m)
			{
				_discountPercent.EditValue = (100m * (_initPrice.Value - _discountPrice.Value) / _initPrice.Value).RoundAs(2);
			}
		}
		else
		{
			_initPrice.EditValue = num;
		}
		_initPrice.DoValidate();
		if (_useEnvFees && _iItemTypeCode != InvoiceRowTypeSE.Services.Value)
		{
			_envFeeSumma.EditValue = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(Convert.ToDecimal(e.NewValue), _envFeePercent.Value, _vatEnabled && _vat.IsChecked.Value, _vatType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			_envFeeSumma.DoValidate();
		}
		_editableMode = true;
	}

	private void EnvFeePercent_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			_envFeeSumma.EditValue = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(_summa.Value, Convert.ToDecimal(e.NewValue), _vatEnabled && _vat.IsChecked.Value, _vatType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			_envFeeSumma.DoValidate();
			_editableMode = true;
		}
	}

	private void EnvFeeSumma_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue == null || !_editableMode)
		{
			return;
		}
		_editableMode = false;
		if (_summa.Value == 0m || Convert.ToDecimal(e.NewValue) == 0m)
		{
			_envFeePercent.EditValue = 0m;
			_envFeePercent.DoValidate();
		}
		else
		{
			decimal value = _summa.Value;
			if (_vatEnabled && _vat.IsChecked.Value && _vatType == "2")
			{
				value -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(_summa.Value, _vatType, _vat.IsChecked.Value);
			}
			value -= Convert.ToDecimal(e.NewValue);
			if (value == 0m)
			{
				_envFeePercent.EditValue = 0m;
				_envFeePercent.DoValidate();
			}
			else
			{
				_envFeePercent.EditValue = (Convert.ToDecimal(e.NewValue) * 100m / value).RoundAs(2);
				_envFeePercent.DoValidate();
			}
		}
		_editableMode = true;
	}

	private void VAT_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode && _useEnvFees && _iItemTypeCode == InvoiceRowTypeSE.Goods.Value)
		{
			_editableMode = false;
			_envFeeSumma.EditValue = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(_summa.Value, _envFeePercent.Value, Convert.ToBoolean(e.NewValue), _vatType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
			_envFeeSumma.DoValidate();
			_editableMode = true;
		}
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value)
		{
			if (_vat.IsChecked.Value)
			{
				_dealType.IsReadOnly = true;
				_dealType.Code = "";
			}
			else
			{
				_dealType.IsReadOnly = false;
			}
		}
	}

	private void UpdateSaleSumma(decimal price)
	{
		_summa.EditValue = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(price, _quantity.Value);
		_summa.DoValidate();
	}

	private void UpdateRetSaleSumma(decimal price)
	{
		_saleSumma.EditValue = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(price, _quantity.Value);
		_saleSumma.DoValidate();
	}

	private void CleanAllFieldExceptStorage(bool isCleanItemCodeField = true)
	{
		if (isCleanItemCodeField)
		{
			_itemCode.Code = "";
			_itemName.Text = "";
			_unit.Code = "";
		}
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value)
		{
			_rowDesc.Text = "";
		}
		_quantity.EditValue = 0m;
		if (_existPriceSummControls)
		{
			_initPrice.EditValue = 0m;
			if (_existDiscountPriceControl)
			{
				_discountPrice.EditValue = 0m;
			}
			_summa.EditValue = 0m;
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
			{
				_salePrice.EditValue = 0m;
				_saleSumma.EditValue = 0m;
			}
		}
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value)
		{
			_summa.EditValue = 0m;
		}
		if (_showPartyColInBill)
		{
			_party.IsChecked = false;
			Party_Changed();
		}
		if (_docTypeHasDiscount)
		{
			if (_useEnvFees && _iItemTypeCode != InvoiceRowTypeSE.Services.Value)
			{
				_envFeePercent.EditValue = 0m;
				_envFeeSumma.EditValue = 0m;
			}
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value)
			{
				_dealType.Code = "";
			}
		}
		_mtCodeSelectedPartiesList = FillPartiesGrid();
	}

	private void Party_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		Party_Changed();
	}

	private void Party_Changed()
	{
		if (!_showPartyColInBill)
		{
			return;
		}
		string text = ItemTypesSE.Goods.Value;
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value)
		{
			text = _itemType.Code;
		}
		else if (_docTypeHasDiscount)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item != null)
			{
				text = item.Type;
			}
		}
		if (text == ItemTypesSE.Goods.Value)
		{
			if (_party.IsChecked.HasValue && _party.IsChecked.Value)
			{
				_expQuantityCol.AllowEditing = ((!_allowAddRowAndNotViewMode) ? DefaultBoolean.False : DefaultBoolean.Default);
				_liParty.Label = ArmSoft.SME.Common.Resources.PartySelected;
				if (_editableMode)
				{
					_partiesTableView.Grid.CurrentColumn = _expQuantityCol;
					_partiesTableView.ShowEditor();
				}
			}
			else
			{
				_liParty.Label = ArmSoft.SME.Common.Resources.AutoSelection;
				_expQuantityCol.AllowEditing = DefaultBoolean.False;
				if (_editableMode)
				{
					CleanParties();
					_mtCodeSelectedPartiesList = FillPartiesGrid();
					if (!CanDivideByParties(_quantity.Value))
					{
						SetQuantityToZero();
					}
				}
			}
		}
		else
		{
			_liParty.Label = "";
		}
		_liParty.IsReadOnly = _party.IsReadOnly;
	}

	private void CleanParties()
	{
		if (_showPartyColInBill)
		{
			_partiesGrid.ItemsSource = new List<PartyRow>();
		}
	}

	private List<PartyRow> FillPartiesGrid()
	{
		GoodsEngineServicesRoutes.PartyRowAndExpMethod partyRowAndExpMethod = new GoodsEngineServicesRoutes.PartyRowAndExpMethod();
		if (_showPartyColInBill)
		{
			try
			{
				string text = _docStorage;
				if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value && _iItemTypeCode == ItemTypesSE.Goods.Value)
				{
					text = _storage.Code;
				}
				else if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && _showStorageColInBill))
				{
					text = _storage.Code;
				}
				else if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier.Value)
				{
					text = _storage.Code;
				}
				if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(_itemCode.Code) && _itemCode.DoValidate())
				{
					partyRowAndExpMethod = TradeSettings.CreateTradeClient().GoodsEngineServices.GetParties(text, _itemCode.Code, _docDate);
					foreach (PartyRow partyRow in partyRowAndExpMethod.partyRows)
					{
						partyRow.GridRow = _rowId;
						if (!_isBaseUnit)
						{
							MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(partyRow.GoodID, _unit.Code);
							if (qntUnit != null)
							{
								partyRow.Qty = (partyRow.Qty / qntUnit.Coef).RoundAs(3);
							}
						}
					}
				}
				_partiesGrid.ItemsSource = partyRowAndExpMethod.partyRows ?? new List<PartyRow>();
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}
		return partyRowAndExpMethod.partyRows ?? new List<PartyRow>();
	}

	private bool CanDivideByParties(decimal quantity)
	{
		decimal num = quantity;
		if (_mtCodeSelectedPartiesList == null)
		{
			return false;
		}
		foreach (PartyRow mtCodeSelectedParties in _mtCodeSelectedPartiesList)
		{
			if (!_storageAllowNegExp)
			{
				if (mtCodeSelectedParties.Qty > 0m)
				{
					if (num >= mtCodeSelectedParties.Qty)
					{
						mtCodeSelectedParties.ExpQty = mtCodeSelectedParties.Qty;
						num -= mtCodeSelectedParties.Qty;
					}
					else
					{
						mtCodeSelectedParties.ExpQty = num;
						num = 0m;
					}
				}
			}
			else if (num >= mtCodeSelectedParties.Qty)
			{
				List<PartyRow> mtCodeSelectedPartiesList = _mtCodeSelectedPartiesList;
				if (mtCodeSelectedParties == mtCodeSelectedPartiesList[mtCodeSelectedPartiesList.Count - 1])
				{
					mtCodeSelectedParties.ExpQty = num;
					num = 0m;
				}
				else
				{
					mtCodeSelectedParties.ExpQty = mtCodeSelectedParties.Qty;
					num -= mtCodeSelectedParties.Qty;
				}
			}
			else
			{
				mtCodeSelectedParties.ExpQty = num;
				num = 0m;
			}
		}
		if (_showPartyColInBill)
		{
			_partiesGrid.ItemsSource = _mtCodeSelectedPartiesList;
			_partiesGrid.Grid.RefreshData();
		}
		if (_storageAllowNegExp)
		{
			return true;
		}
		if (Convert.ToDecimal(_availableQuantityCol.TotalSummaries[0].Value) < quantity)
		{
			return false;
		}
		return true;
	}

	private void DistributeQtyByExpiredDate(decimal quantity)
	{
		decimal num = quantity;
		List<PartyRow> list = new List<PartyRow>();
		List<PartyRow> list2 = new List<PartyRow>();
		List<PartyRow> list3 = new List<PartyRow>();
		bool flag = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(_docStorage);
		string text = "";
		if (_mtCodeSelectedPartiesList == null)
		{
			return;
		}
		_ = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code).Unit == _unit.Code;
		foreach (PartyRow mtCodeSelectedParties in _mtCodeSelectedPartiesList)
		{
			mtCodeSelectedParties.ExpQty = 0m;
			mtCodeSelectedParties.GridRow = _rowId;
			if (!mtCodeSelectedParties.UseableDate.HasValue)
			{
				list2.Add(mtCodeSelectedParties);
			}
			else
			{
				list3.Add(mtCodeSelectedParties);
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
			if ((num > 0m) & flag)
			{
				List<PartyRow> mtCodeSelectedPartiesList = _mtCodeSelectedPartiesList;
				mtCodeSelectedPartiesList[mtCodeSelectedPartiesList.Count - 1].ExpQty += num;
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
				else if (item2.Qty > 0m)
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
		_mtCodeSelectedPartiesList = list5;
		List<PartyRow> list6 = new List<PartyRow>();
		if (_mtCodeSelectedPartiesList.Count > 1)
		{
			if (text == "2")
			{
				list6.AddRange(from ListRow in _mtCodeSelectedPartiesList
					orderby ListRow.IncomeDate descending, ListRow.OrderNum, ListRow.Row
					select ListRow);
			}
			else
			{
				list6.AddRange(from ListRow in _mtCodeSelectedPartiesList
					orderby ListRow.IncomeDate, ListRow.OrderNum, ListRow.Row
					select ListRow);
			}
		}
		else
		{
			list6 = _mtCodeSelectedPartiesList;
		}
		if (_showPartyColInBill)
		{
			_partiesGrid.ItemsSource = list6;
			_partiesGrid.Grid.RefreshData();
		}
	}

	private bool FindGridRowInList(PartyRow row)
	{
		return row.GridRow == _rowId;
	}

	private bool FindRowInList(PartyRow row)
	{
		if (row.Base == _partyBase)
		{
			return row.Row == _partyRowId;
		}
		return false;
	}

	private void NextButton_Click(object sender, RoutedEventArgs e)
	{
		if (Validate())
		{
			Save();
			_editableMode = false;
			_rowId = _maxRowId;
			CleanAllFieldExceptStorage(isCleanItemCodeField: false);
			_itemCode.InnerCodeEdit.Focus();
			_editableMode = true;
		}
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		if (Validate())
		{
			Save();
			DialogResult = true;
			Close();
		}
	}

	private bool Validate()
	{
		try
		{
			if (!InnerValidate())
			{
				return false;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			return false;
		}
		return true;
	}

	private bool InnerValidate()
	{
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value)
		{
			if (!_itemType.DoValidate())
			{
				_itemType.InnerCodeEdit.Focus();
				return false;
			}
			if (_itemType.Code == ItemTypesSE.Goods.Value && !_storage.DoValidate())
			{
				_storage.InnerCodeEdit.Focus();
				return false;
			}
		}
		if (!_itemCode.DoValidate())
		{
			_itemCode.InnerCodeEdit.Focus();
			return false;
		}
		if (!_itemName.DoValidate())
		{
			_itemName.Focus();
			return false;
		}
		if (!_unit.DoValidate())
		{
			_unit.InnerCodeEdit.Focus();
			return false;
		}
		if (!_quantity.DoValidate())
		{
			_quantity.Focus();
			return false;
		}
		if (_existPriceSummControls)
		{
			if (!_initPrice.DoValidate())
			{
				_initPrice.Focus();
				return false;
			}
			if (_existDiscountPriceControl && !_discountPrice.DoValidate())
			{
				_discountPrice.Focus();
				return false;
			}
			if (!_summa.DoValidate())
			{
				_summa.Focus();
				return false;
			}
			if (_useEnvFees)
			{
				if (!_envFeePercent.DoValidate())
				{
					_envFeePercent.Focus();
					return false;
				}
				if (!_envFeeSumma.DoValidate())
				{
					_envFeeSumma.Focus();
					return false;
				}
			}
			if (_vatEnabled)
			{
				_vat.DoValidate();
			}
			if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value && _itemType.Code == ItemTypesSE.Goods.Value)
			{
				_partiesTableView.CommitEditing();
			}
			else if (_docTypeHasDiscount && _showPartyColInBill)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
				if (item != null && item.Type == ItemTypesSE.Goods.Value)
				{
					_partiesTableView.CommitEditing();
				}
			}
		}
		else
		{
			_partiesTableView.CommitEditing();
		}
		if (_showPartyColInBill)
		{
			_party.DoValidate();
		}
		switch (_docType)
		{
		case 8:
			if (!InnerCheckRepeatedInListInvoice())
			{
				return false;
			}
			break;
		case 21:
			if (!InnerCheckRepeatedInListInvoiceRetail())
			{
				return false;
			}
			break;
		case 18:
			if (!InnerCheckRepeatedInListBill())
			{
				return false;
			}
			break;
		case 9:
			foreach (MTExpenseWindow.MTExpenseSpecificationRow mtExpense in _mtExpenseList)
			{
				if (_itemCode.Code == mtExpense.MTCode && ((mtExpense.RowID != _rowId && _mode == DialogWindowMode.EditMode && _itemCode.Code != _oldItemCode) || _mode == DialogWindowMode.NewModeWithNext))
				{
					if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, _itemCode.Code), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
					{
						_itemCode.InnerCodeEdit.Focus();
						return false;
					}
					break;
				}
			}
			break;
		case 10:
			if (!InnerCheckRepeatedInMTMoveList())
			{
				return false;
			}
			break;
		case 39:
			if (!InnerCheckRepeatedInMTMoveWithConfirmationList())
			{
				return false;
			}
			break;
		case 11:
			foreach (MTComplectationWindow.MTComplectationSpecificationRow mtComplectation in _mtComplectationList)
			{
				if (_itemCode.Code == mtComplectation.ItemCode && ((mtComplectation.RowID != _rowId && _mode == DialogWindowMode.EditMode && _itemCode.Code != _oldItemCode) || _mode == DialogWindowMode.NewModeWithNext))
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.WrongComplectPart, _itemCode.Code, _complectCode), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					_itemCode.InnerCodeEdit.Focus();
					return false;
				}
			}
			break;
		}
		string text = "";
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value)
		{
			text = _itemType.Code;
		}
		else if (!_docTypeHasDiscount || (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value && _itemType.Code == ItemTypesSE.Goods.Value))
		{
			text = ItemTypesSE.Goods.Value;
		}
		else
		{
			MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item2 != null)
			{
				text = item2.Type;
			}
		}
		if (text == ItemTypesSE.Goods.Value)
		{
			if (_showPartyColInBill && !_storageAllowNegExp && Convert.ToDecimal(_expQuantityCol.TotalSummaries[0].Value) == 0m)
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldValueGraterZero, ArmSoft.SME.Common.Resources.Quantity));
			}
			return true;
		}
		return true;
	}

	private bool InnerCheckRepeatedInListInvoice()
	{
		foreach (MTInvoiceWindow.MTInvoiceSpecificationRow mtInvoices in _mtInvoicesList)
		{
			if (_itemCode.Code == mtInvoices.ItemCode && ((mtInvoices.RowID != _rowId && _mode == DialogWindowMode.EditMode && _itemCode.Code != _oldItemCode) || _mode == DialogWindowMode.NewModeWithNext))
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, _itemCode.Code), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
				{
					_itemCode.InnerCodeEdit.Focus();
					return false;
				}
				break;
			}
		}
		return true;
	}

	private bool InnerCheckRepeatedInListInvoiceRetail()
	{
		foreach (MTInvoiceRetailWindow.MTInvoiceRetailSpecificationRow mtInvoiceRetail in _mtInvoiceRetailList)
		{
			if (_itemCode.Code == mtInvoiceRetail.ItemCode && ((mtInvoiceRetail.RowID != _rowId && _mode == DialogWindowMode.EditMode && _itemCode.Code != _oldItemCode) || _mode == DialogWindowMode.NewModeWithNext))
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, _itemCode.Code), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
				{
					_itemCode.InnerCodeEdit.Focus();
					return false;
				}
				break;
			}
		}
		return true;
	}

	private bool InnerCheckRepeatedInListBill()
	{
		foreach (MTBillWindow.MTBillSpecificationRow mtBill in _mtBillList)
		{
			if (_itemCode.Code == mtBill.ItemCode && ((mtBill.RowID != _rowId && _mode == DialogWindowMode.EditMode && _itemCode.Code != _oldItemCode) || _mode == DialogWindowMode.NewModeWithNext))
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, _itemCode.Code), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
				{
					_itemCode.InnerCodeEdit.Focus();
					return false;
				}
				break;
			}
		}
		return true;
	}

	private bool InnerCheckRepeatedInMTMoveList()
	{
		foreach (MTMoveWindow.MTMoveSpecificationRow mtMove in _mtMoveList)
		{
			if (_itemCode.Code == mtMove.MTCode && ((mtMove.RowID != _rowId && _mode == DialogWindowMode.EditMode && _itemCode.Code != _oldItemCode) || _mode == DialogWindowMode.NewModeWithNext))
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, _itemCode.Code), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
				{
					_itemCode.InnerCodeEdit.Focus();
					return false;
				}
				break;
			}
		}
		return true;
	}

	private bool InnerCheckRepeatedInMTMoveWithConfirmationList()
	{
		foreach (MTMoveWithConfirmationWindow.MTMoveSpecificationRow mtMoveWithConfirmation in _mtMoveWithConfirmationList)
		{
			if (_itemCode.Code == mtMoveWithConfirmation.MTCode && ((mtMoveWithConfirmation.RowID != _rowId && _mode == DialogWindowMode.EditMode && _itemCode.Code != _oldItemCode) || _mode == DialogWindowMode.NewModeWithNext))
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, _itemCode.Code), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
				{
					_itemCode.InnerCodeEdit.Focus();
					return false;
				}
				break;
			}
		}
		return true;
	}

	private void Save()
	{
		int mtListCount = 0;
		switch (_docType)
		{
		case 8:
		{
			InnerSaveForInvoice(_mtInvoice, out var mtInvoiceRow, ref mtListCount);
			if (mtInvoiceRow.ItemType == ItemTypesSE.Goods.Value)
			{
				_mtExpDocSelectedPartiesList.RemoveAll(FindGridRowInList);
				if (_showPartyColInBill && _party.IsChecked.HasValue && _party.IsChecked.Value)
				{
					foreach (PartyRow mtCodeSelectedParties in _mtCodeSelectedPartiesList)
					{
						_mtExpDocSelectedPartiesList.Add(mtCodeSelectedParties.Clone());
					}
				}
				_mtInvoice.PO.MTExpDocSelectedPartiesList.Clear();
				foreach (PartyRow mtExpDocSelectedParties in _mtExpDocSelectedPartiesList)
				{
					_mtInvoice.PO.MTExpDocSelectedPartiesList.Add(mtExpDocSelectedParties.Clone());
				}
			}
			else if (_mode == DialogWindowMode.EditMode)
			{
				_mtInvoice.PO.MTExpDocSelectedPartiesList.RemoveAll(FindGridRowInList);
			}
			break;
		}
		case 21:
		{
			InnerSaveForInvoiceRetail(_mtInvoiceRetail, out var mtInvoiceRetailRow, ref mtListCount);
			if (mtInvoiceRetailRow.ItemType == ItemTypesSE.Goods.Value)
			{
				_mtExpDocSelectedPartiesList.RemoveAll(FindGridRowInList);
				if (_showPartyColInBill && _party.IsChecked.HasValue && _party.IsChecked.Value)
				{
					foreach (PartyRow mtCodeSelectedParties2 in _mtCodeSelectedPartiesList)
					{
						_mtExpDocSelectedPartiesList.Add(mtCodeSelectedParties2.Clone());
					}
				}
				_mtInvoiceRetail.PO.MTExpDocSelectedPartiesList.Clear();
				foreach (PartyRow mtExpDocSelectedParties2 in _mtExpDocSelectedPartiesList)
				{
					_mtInvoiceRetail.PO.MTExpDocSelectedPartiesList.Add(mtExpDocSelectedParties2.Clone());
				}
			}
			else if (_mode == DialogWindowMode.EditMode)
			{
				_mtInvoiceRetail.PO.MTExpDocSelectedPartiesList.RemoveAll(FindGridRowInList);
			}
			break;
		}
		case 18:
		{
			InnerSaveForBill(out var mtBillRow, ref mtListCount);
			if (mtBillRow.ItemType == ItemTypesSE.Goods.Value)
			{
				_mtExpDocSelectedPartiesList.RemoveAll(FindGridRowInList);
				if (_showPartyColInBill && _party.IsChecked.HasValue && _party.IsChecked.Value)
				{
					foreach (PartyRow mtCodeSelectedParties3 in _mtCodeSelectedPartiesList)
					{
						_mtExpDocSelectedPartiesList.Add(mtCodeSelectedParties3.Clone());
					}
				}
				_mtBill.PO.MTExpDocSelectedPartiesList.Clear();
				foreach (PartyRow mtExpDocSelectedParties3 in _mtExpDocSelectedPartiesList)
				{
					_mtBill.PO.MTExpDocSelectedPartiesList.Add(mtExpDocSelectedParties3.Clone());
				}
			}
			else if (_mode == DialogWindowMode.EditMode)
			{
				_mtBill.PO.MTExpDocSelectedPartiesList.RemoveAll(FindGridRowInList);
			}
			break;
		}
		case 9:
		{
			MTExpenseWindow.MTExpenseSpecificationRow mTExpenseSpecificationRow;
			if (_mode == DialogWindowMode.NewModeWithNext)
			{
				mTExpenseSpecificationRow = new MTExpenseWindow.MTExpenseSpecificationRow(_mtExpense);
				_mtExpenseList.Add(mTExpenseSpecificationRow);
				mtListCount = _mtExpenseList.Count - 1;
			}
			else
			{
				mTExpenseSpecificationRow = _mtExpenseSourceRow;
			}
			mTExpenseSpecificationRow.MTCode = _itemCode.Code;
			MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item2 != null)
			{
				mTExpenseSpecificationRow.MTID = item2.Id;
				mTExpenseSpecificationRow.MTCode = item2.MTCode;
			}
			mTExpenseSpecificationRow.MTName = _itemName.Text;
			mTExpenseSpecificationRow.UnitCode = _unit.Code;
			mTExpenseSpecificationRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code)?.Brief ?? "";
			mTExpenseSpecificationRow.Quantity = _quantity.Value;
			mTExpenseSpecificationRow.IsPartySelected = _party.IsChecked.HasValue && _party.IsChecked.Value;
			mTExpenseSpecificationRow.RowID = _rowId;
			_mtExpDocSelectedPartiesList.RemoveAll(FindGridRowInList);
			decimal summa = 0m;
			if (_party.IsChecked.HasValue && _party.IsChecked.Value)
			{
				foreach (PartyRow mtCodeSelectedParties4 in _mtCodeSelectedPartiesList)
				{
					PartyRow partyRow = mtCodeSelectedParties4.Clone();
					if (mtCodeSelectedParties4.ExpQty > 0m)
					{
						PartyRow partyRow3 = partyRow;
						decimal expSumma2;
						if (mtCodeSelectedParties4.ExpQty == mtCodeSelectedParties4.Qty)
						{
							expSumma2 = mtCodeSelectedParties4.Summa;
						}
						else
						{
							expSumma2 = ((!_storageAllowNegExp) ? ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtCodeSelectedParties4.Cost, mtCodeSelectedParties4.ExpQty) : ((mtCodeSelectedParties4.Cost * mtCodeSelectedParties4.ExpQty < mtCodeSelectedParties4.Qty) ? mtCodeSelectedParties4.ExpQty : mtCodeSelectedParties4.Qty).RoundAs(2));
						}
						partyRow3.ExpSumma = expSumma2;
						summa += partyRow.ExpSumma;
					}
					_mtExpDocSelectedPartiesList.Add(partyRow);
				}
			}
			mTExpenseSpecificationRow.Summa = summa;
			_mtExpense.PO.MTExpDocSelectedPartiesList.Clear();
			foreach (PartyRow mtExpDocSelectedParties4 in _mtExpDocSelectedPartiesList)
			{
				_mtExpense.PO.MTExpDocSelectedPartiesList.Add(mtExpDocSelectedParties4.Clone());
			}
			if (_existPriceSummControls)
			{
				mTExpenseSpecificationRow.SalePrice = _initPrice.Value;
				mTExpenseSpecificationRow.Summa = _summa.Value;
			}
			break;
		}
		case 10:
		{
			MTMoveWindow.MTMoveSpecificationRow mtMoveRow = null;
			InnerSaveForMTMove(_mtMove, ref mtMoveRow, ref mtListCount);
			_mtExpDocSelectedPartiesList.RemoveAll(FindGridRowInList);
			decimal summa2 = 0m;
			if (_party.IsChecked.HasValue && _party.IsChecked.Value)
			{
				foreach (PartyRow mtCodeSelectedParties5 in _mtCodeSelectedPartiesList)
				{
					PartyRow partyRow = mtCodeSelectedParties5.Clone();
					if (mtCodeSelectedParties5.ExpQty > 0m)
					{
						PartyRow partyRow4 = partyRow;
						decimal expSumma3;
						if (mtCodeSelectedParties5.ExpQty == mtCodeSelectedParties5.Qty)
						{
							expSumma3 = mtCodeSelectedParties5.Summa;
						}
						else
						{
							expSumma3 = ((!_storageAllowNegExp) ? (mtCodeSelectedParties5.Cost * mtCodeSelectedParties5.ExpQty).RoundAs(2) : ((mtCodeSelectedParties5.Cost * mtCodeSelectedParties5.ExpQty < mtCodeSelectedParties5.Qty) ? mtCodeSelectedParties5.ExpQty : mtCodeSelectedParties5.Qty).RoundAs(2));
						}
						partyRow4.ExpSumma = expSumma3;
						summa2 += partyRow.ExpSumma;
					}
					_mtExpDocSelectedPartiesList.Add(partyRow);
				}
			}
			mtMoveRow.Summa = summa2;
			_mtMove.PO.MTExpDocSelectedPartiesList.Clear();
			foreach (PartyRow mtExpDocSelectedParties5 in _mtExpDocSelectedPartiesList)
			{
				_mtMove.PO.MTExpDocSelectedPartiesList.Add(mtExpDocSelectedParties5.Clone());
			}
			if (_existPriceSummControls)
			{
				mtMoveRow.Price = _initPrice.Value;
				mtMoveRow.Summa = _summa.Value;
			}
			break;
		}
		case 39:
		{
			MTMoveWithConfirmationWindow.MTMoveSpecificationRow mtMoveWithConfirmationRow = null;
			InnerSaveForMTMoveWithConfirmation(_mtMoveWithConfirmation, ref mtMoveWithConfirmationRow, ref mtListCount);
			_mtExpDocSelectedPartiesList.RemoveAll(FindGridRowInList);
			decimal summa4 = 0m;
			if (_party.IsChecked.HasValue && _party.IsChecked.Value)
			{
				foreach (PartyRow mtCodeSelectedParties6 in _mtCodeSelectedPartiesList)
				{
					PartyRow partyRow = mtCodeSelectedParties6.Clone();
					if (mtCodeSelectedParties6.ExpQty > 0m)
					{
						PartyRow partyRow6 = partyRow;
						decimal expSumma5;
						if (mtCodeSelectedParties6.ExpQty == mtCodeSelectedParties6.Qty)
						{
							expSumma5 = mtCodeSelectedParties6.Summa;
						}
						else
						{
							expSumma5 = ((!_storageAllowNegExp) ? (mtCodeSelectedParties6.Cost * mtCodeSelectedParties6.ExpQty).RoundAs(2) : ((mtCodeSelectedParties6.Cost * mtCodeSelectedParties6.ExpQty < mtCodeSelectedParties6.Qty) ? mtCodeSelectedParties6.ExpQty : mtCodeSelectedParties6.Qty).RoundAs(2));
						}
						partyRow6.ExpSumma = expSumma5;
						summa4 += partyRow.ExpSumma;
					}
					_mtExpDocSelectedPartiesList.Add(partyRow);
				}
			}
			mtMoveWithConfirmationRow.Summa = summa4;
			_mtMoveWithConfirmation.PO.MTExpDocSelectedPartiesList.Clear();
			foreach (PartyRow mtExpDocSelectedParties6 in _mtExpDocSelectedPartiesList)
			{
				_mtMoveWithConfirmation.PO.MTExpDocSelectedPartiesList.Add(mtExpDocSelectedParties6.Clone());
			}
			if (_existPriceSummControls)
			{
				mtMoveWithConfirmationRow.Price = _initPrice.Value;
				mtMoveWithConfirmationRow.Summa = _summa.Value;
			}
			break;
		}
		case 11:
		{
			MTComplectationWindow.MTComplectationSpecificationRow mTComplectationSpecificationRow;
			if (_mode == DialogWindowMode.NewModeWithNext)
			{
				mTComplectationSpecificationRow = new MTComplectationWindow.MTComplectationSpecificationRow();
				_mtComplectationList.Add(mTComplectationSpecificationRow);
				mtListCount = _mtComplectationList.Count - 1;
			}
			else
			{
				mTComplectationSpecificationRow = _mtComplectationSourceRow;
			}
			MaterialCacheRow item3 = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item3 != null)
			{
				mTComplectationSpecificationRow.ItemType = item3.Type;
				mTComplectationSpecificationRow.ItemCode = _itemCode.Code;
				mTComplectationSpecificationRow.ItemID = item3.Id;
			}
			mTComplectationSpecificationRow.ItemName = _itemName.Text;
			mTComplectationSpecificationRow.UnitCode = _unit.Code;
			mTComplectationSpecificationRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code)?.Brief ?? "";
			mTComplectationSpecificationRow.Quantity = _quantity.Value;
			if (mTComplectationSpecificationRow.ItemType == ItemTypesSE.Goods.Value)
			{
				mTComplectationSpecificationRow.Summa = 0m;
				if (_showPartyColInBill)
				{
					mTComplectationSpecificationRow.IsPartySelected = _party.IsChecked.HasValue && _party.IsChecked.Value;
				}
				else
				{
					mTComplectationSpecificationRow.IsPartySelected = false;
				}
				_mtExpDocSelectedPartiesList.RemoveAll(FindGridRowInList);
				decimal summa3 = 0m;
				if (_party.IsChecked.HasValue && _party.IsChecked.Value)
				{
					foreach (PartyRow mtCodeSelectedParties7 in _mtCodeSelectedPartiesList)
					{
						PartyRow partyRow = mtCodeSelectedParties7.Clone();
						if (mtCodeSelectedParties7.ExpQty > 0m)
						{
							PartyRow partyRow5 = partyRow;
							decimal expSumma4;
							if (mtCodeSelectedParties7.ExpQty == mtCodeSelectedParties7.Qty)
							{
								expSumma4 = mtCodeSelectedParties7.Summa;
							}
							else
							{
								expSumma4 = ((!_storageAllowNegExp) ? ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtCodeSelectedParties7.Cost, mtCodeSelectedParties7.ExpQty) : ((mtCodeSelectedParties7.Cost * mtCodeSelectedParties7.ExpQty < mtCodeSelectedParties7.Qty) ? mtCodeSelectedParties7.ExpQty : mtCodeSelectedParties7.Qty).RoundAs(2));
							}
							partyRow5.ExpSumma = expSumma4;
							summa3 += partyRow.ExpSumma;
						}
						_mtExpDocSelectedPartiesList.Add(partyRow);
					}
				}
				mTComplectationSpecificationRow.Summa = summa3;
				_mtComplectation.PO.MTExpDocSelectedPartiesList.Clear();
				foreach (PartyRow mtExpDocSelectedParties7 in _mtExpDocSelectedPartiesList)
				{
					_mtComplectation.PO.MTExpDocSelectedPartiesList.Add(mtExpDocSelectedParties7.Clone());
				}
			}
			else
			{
				mTComplectationSpecificationRow.Summa = _summa.Value;
				mTComplectationSpecificationRow.IsPartySelected = null;
			}
			mTComplectationSpecificationRow.RowID = _rowId;
			break;
		}
		case 14:
		{
			MTReturnToSupplierWindow.MTRetToSupSpecificationRow mTRetToSupSpecificationRow;
			if (_mode == DialogWindowMode.NewModeWithNext)
			{
				mTRetToSupSpecificationRow = new MTReturnToSupplierWindow.MTRetToSupSpecificationRow();
				_mtReturnToSupplierList.Add(mTRetToSupSpecificationRow);
				mtListCount = _mtReturnToSupplierList.Count - 1;
			}
			else
			{
				mTRetToSupSpecificationRow = _mtReturnToSupplierSourceRow;
			}
			mTRetToSupSpecificationRow.Storage = _storage.Code;
			mTRetToSupSpecificationRow.MTCode = _itemCode.Code;
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
			if (item != null)
			{
				mTRetToSupSpecificationRow.MTID = item.Id;
			}
			mTRetToSupSpecificationRow.MTName = _itemName.Text;
			mTRetToSupSpecificationRow.UnitCode = _unit.Code;
			mTRetToSupSpecificationRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code)?.Brief ?? "";
			mTRetToSupSpecificationRow.Quantity = _quantity.Value;
			mTRetToSupSpecificationRow.Price = _initPrice.Value;
			if (_existDiscountPercentControl)
			{
				mTRetToSupSpecificationRow.Discount = _discountPercent.Value;
			}
			else if (_existDiscountPriceControl)
			{
				mTRetToSupSpecificationRow.Discount = _docDiscountPercent;
			}
			if (_existDiscountPriceControl)
			{
				mTRetToSupSpecificationRow.DiscountPrice = _discountPrice.Value;
			}
			else
			{
				mTRetToSupSpecificationRow.DiscountPrice = mTRetToSupSpecificationRow.Price;
			}
			mTRetToSupSpecificationRow.Summa = _summa.Value;
			mTRetToSupSpecificationRow.SalePrice = _salePrice.Value;
			mTRetToSupSpecificationRow.SaleSumma = _saleSumma.Value;
			if (_vatEnabled)
			{
				mTRetToSupSpecificationRow.VAT = Convert.ToBoolean(_vat.IsChecked);
			}
			mTRetToSupSpecificationRow.IsPartySelected = _party.IsChecked.HasValue && _party.IsChecked.Value;
			mTRetToSupSpecificationRow.RowID = _rowId;
			_mtExpDocSelectedPartiesList.RemoveAll(FindGridRowInList);
			decimal num = 0m;
			if (_party.IsChecked.HasValue && _party.IsChecked.Value)
			{
				foreach (PartyRow mtCodeSelectedParties8 in _mtCodeSelectedPartiesList)
				{
					PartyRow partyRow = mtCodeSelectedParties8.Clone();
					if (mtCodeSelectedParties8.ExpQty > 0m)
					{
						PartyRow partyRow2 = partyRow;
						decimal expSumma;
						if (mtCodeSelectedParties8.ExpQty == mtCodeSelectedParties8.Qty)
						{
							expSumma = mtCodeSelectedParties8.Summa;
						}
						else
						{
							expSumma = ((!_storageAllowNegExp) ? ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtCodeSelectedParties8.Cost, mtCodeSelectedParties8.ExpQty) : ((mtCodeSelectedParties8.Cost * mtCodeSelectedParties8.ExpQty < mtCodeSelectedParties8.Qty) ? mtCodeSelectedParties8.ExpQty : mtCodeSelectedParties8.Qty).RoundAs(2));
						}
						partyRow2.ExpSumma = expSumma;
						num += partyRow.ExpSumma;
					}
					_mtExpDocSelectedPartiesList.Add(partyRow);
				}
			}
			_mtReturnToSupplier.PO.MTExpDocSelectedPartiesList.Clear();
			foreach (PartyRow mtExpDocSelectedParties8 in _mtExpDocSelectedPartiesList)
			{
				_mtReturnToSupplier.PO.MTExpDocSelectedPartiesList.Add(mtExpDocSelectedParties8.Clone());
			}
			break;
		}
		}
		if (_mode == DialogWindowMode.NewModeWithNext)
		{
			DocumentGridAddingRowEventArgs e = new DocumentGridAddingRowEventArgs(_mtDocGrid);
			_maxRowId++;
			AddedRows++;
			e.RowNumber = mtListCount;
			_mtDocGrid.InnerAddRow(e);
		}
		_mtDocGrid.Grid.UpdateTotalSummary();
		_mtDocGrid.View.CommitEditing();
	}

	private void InnerSaveForInvoice(MTInvoiceWindow doc, out MTInvoiceWindow.MTInvoiceSpecificationRow mtInvoiceRow, ref int mtListCount)
	{
		if (_mode == DialogWindowMode.NewModeWithNext)
		{
			mtInvoiceRow = new MTInvoiceWindow.MTInvoiceSpecificationRow(doc);
			_mtInvoicesList.Add(mtInvoiceRow);
			mtListCount = _mtInvoicesList.Count - 1;
		}
		else
		{
			mtInvoiceRow = _mtInvoiceSourceRow;
		}
		mtInvoiceRow.Storage = _storage.Code;
		mtInvoiceRow.ItemCode = _itemCode.Code;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
		mtInvoiceRow.ItemType = item.Type;
		mtInvoiceRow.ItemID = item.Id;
		mtInvoiceRow.ItemName = _itemName.Text;
		mtInvoiceRow.UnitCode = _unit.Code;
		mtInvoiceRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code)?.Brief ?? "";
		mtInvoiceRow.RowDescription = _rowDesc.Text;
		mtInvoiceRow.Quantity = _quantity.Value;
		mtInvoiceRow.Price = _initPrice.Value;
		if (_existDiscountPercentControl)
		{
			mtInvoiceRow.Discount = _discountPercent.Value;
		}
		else if (_existDiscountPriceControl)
		{
			mtInvoiceRow.Discount = _docDiscountPercent;
		}
		if (_existDiscountPriceControl)
		{
			mtInvoiceRow.DiscountPrice = _discountPrice.Value;
		}
		else
		{
			mtInvoiceRow.DiscountPrice = mtInvoiceRow.Price;
		}
		mtInvoiceRow.Summa = _summa.Value;
		if (_useDiscount)
		{
			mtInvoiceRow.DiscountSumma = (mtInvoiceRow.Quantity * mtInvoiceRow.Price - mtInvoiceRow.Summa).RoundAs(2);
		}
		else
		{
			mtInvoiceRow.DiscountSumma = 0m;
		}
		if (_useEnvFees && mtInvoiceRow.ItemType == InvoiceRowTypeSE.Goods.Value)
		{
			mtInvoiceRow.EnvFeePercent = _envFeePercent.Value;
			mtInvoiceRow.EnvFeeSumma = _envFeeSumma.Value;
		}
		if (_vatEnabled)
		{
			mtInvoiceRow.VAT = Convert.ToBoolean(_vat.IsChecked);
		}
		string text;
		if (_vatEnabled)
		{
			text = (string.IsNullOrEmpty(_dealType.Code) ? null : (from invoiceDealTypesSE in SmartEnum<InvoiceDealTypesSE, short>.List
				where invoiceDealTypesSE.ValueUI == _dealType.Code
				select invoiceDealTypesSE.Value).FirstOrDefault().ToString());
		}
		else
		{
			text = (string.IsNullOrEmpty(_dealType.Code) ? null : (from accDealTypesSE in SmartEnum<AccDealTypesSE, short>.List
				where accDealTypesSE.ValueUI == _dealType.Code
				select accDealTypesSE.Value).FirstOrDefault().ToString());
		}
		if (text != null)
		{
			mtInvoiceRow.DealType = text;
		}
		if (mtInvoiceRow.ItemType == ItemTypesSE.Goods.Value)
		{
			if (_showPartyColInBill)
			{
				mtInvoiceRow.IsPartySelected = _party.IsChecked.HasValue && _party.IsChecked.Value;
			}
			else
			{
				mtInvoiceRow.IsPartySelected = false;
			}
		}
		else
		{
			mtInvoiceRow.IsPartySelected = null;
		}
		mtInvoiceRow.RowID = _rowId;
	}

	private void InnerSaveForInvoiceRetail(MTInvoiceRetailWindow doc, out MTInvoiceRetailWindow.MTInvoiceRetailSpecificationRow mtInvoiceRetailRow, ref int mtListCount)
	{
		if (_mode == DialogWindowMode.NewModeWithNext)
		{
			mtInvoiceRetailRow = new MTInvoiceRetailWindow.MTInvoiceRetailSpecificationRow(doc);
			_mtInvoiceRetailList.Add(mtInvoiceRetailRow);
			mtListCount = _mtInvoiceRetailList.Count - 1;
		}
		else
		{
			mtInvoiceRetailRow = _mtInvoiceRetailSourceRow;
		}
		mtInvoiceRetailRow.Storage = _docStorage;
		mtInvoiceRetailRow.ItemCode = _itemCode.Code;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
		mtInvoiceRetailRow.ItemType = item.Type;
		mtInvoiceRetailRow.ItemID = item.Id;
		mtInvoiceRetailRow.ItemName = _itemName.Text;
		mtInvoiceRetailRow.UnitCode = _unit.Code;
		mtInvoiceRetailRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code)?.Brief ?? "";
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value)
		{
			mtInvoiceRetailRow.RowDescription = _rowDesc.Text;
		}
		mtInvoiceRetailRow.Quantity = _quantity.Value;
		mtInvoiceRetailRow.Price = _initPrice.Value;
		if (_existDiscountPercentControl)
		{
			mtInvoiceRetailRow.Discount = _discountPercent.Value;
		}
		else if (_existDiscountPriceControl)
		{
			mtInvoiceRetailRow.Discount = _docDiscountPercent;
		}
		if (_existDiscountPriceControl)
		{
			mtInvoiceRetailRow.DiscountPrice = _discountPrice.Value;
		}
		else
		{
			mtInvoiceRetailRow.DiscountPrice = mtInvoiceRetailRow.Price;
		}
		mtInvoiceRetailRow.Summa = _summa.Value;
		if (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value || _docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value)
		{
			if (_useDiscount)
			{
				mtInvoiceRetailRow.DiscountSumma = (mtInvoiceRetailRow.Quantity * mtInvoiceRetailRow.Price - mtInvoiceRetailRow.Summa).RoundAs(2);
			}
			else
			{
				mtInvoiceRetailRow.DiscountSumma = 0m;
			}
		}
		if (_useEnvFees && mtInvoiceRetailRow.ItemType == InvoiceRowTypeSE.Goods.Value)
		{
			mtInvoiceRetailRow.EnvFeePercent = _envFeePercent.Value;
			mtInvoiceRetailRow.EnvFeeSumma = _envFeeSumma.Value;
		}
		if (_vatEnabled)
		{
			mtInvoiceRetailRow.VAT = Convert.ToBoolean(_vat.IsChecked);
		}
		string text;
		if (_vatEnabled)
		{
			text = (string.IsNullOrEmpty(_dealType.Code) ? null : (from invoiceDealTypesSE in SmartEnum<InvoiceDealTypesSE, short>.List
				where invoiceDealTypesSE.ValueUI == _dealType.Code
				select invoiceDealTypesSE.Value).FirstOrDefault().ToString());
		}
		else
		{
			text = (string.IsNullOrEmpty(_dealType.Code) ? null : (from accDealTypesSE in SmartEnum<AccDealTypesSE, short>.List
				where accDealTypesSE.ValueUI == _dealType.Code
				select accDealTypesSE.Value).FirstOrDefault().ToString());
		}
		if (text != null)
		{
			mtInvoiceRetailRow.DealType = text;
		}
		if (mtInvoiceRetailRow.ItemType == ItemTypesSE.Goods.Value)
		{
			if (_showPartyColInBill)
			{
				mtInvoiceRetailRow.IsPartySelected = _party.IsChecked.HasValue && _party.IsChecked.Value;
			}
			else
			{
				mtInvoiceRetailRow.IsPartySelected = false;
			}
		}
		else
		{
			mtInvoiceRetailRow.IsPartySelected = null;
		}
		mtInvoiceRetailRow.RowID = _rowId;
	}

	private void InnerSaveForBill(out MTBillWindow.MTBillSpecificationRow mtBillRow, ref int mtListCount)
	{
		if (_mode == DialogWindowMode.NewModeWithNext)
		{
			mtBillRow = new MTBillWindow.MTBillSpecificationRow();
			_mtBillList.Add(mtBillRow);
			mtListCount = _mtBillList.Count - 1;
		}
		else
		{
			mtBillRow = _mtBillSourceRow;
		}
		mtBillRow.Storage = ((_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || (_docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && _showStorageColInBill)) ? _storage.Code : _docStorage);
		mtBillRow.ItemCode = _itemCode.Code;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
		mtBillRow.ItemType = item.Type;
		mtBillRow.ItemID = item.Id;
		mtBillRow.ItemName = _itemName.Text;
		mtBillRow.UnitCode = _unit.Code;
		mtBillRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code)?.Brief ?? "";
		mtBillRow.Quantity = _quantity.Value;
		mtBillRow.Price = _initPrice.Value;
		if (_existDiscountPercentControl)
		{
			mtBillRow.Discount = _discountPercent.Value;
		}
		else if (_existDiscountPriceControl)
		{
			mtBillRow.Discount = _docDiscountPercent;
		}
		if (_existDiscountPriceControl)
		{
			mtBillRow.DiscountPrice = _discountPrice.Value;
		}
		else
		{
			mtBillRow.DiscountPrice = mtBillRow.Price;
		}
		mtBillRow.Summa = _summa.Value;
		if (_useDiscount)
		{
			mtBillRow.DiscountSumma = (mtBillRow.Quantity * mtBillRow.Price - mtBillRow.Summa).RoundAs(2);
		}
		else
		{
			mtBillRow.DiscountSumma = 0m;
		}
		if (_vatEnabled)
		{
			mtBillRow.VAT = Convert.ToBoolean(_vat.IsChecked);
		}
		if (mtBillRow.ItemType == ItemTypesSE.Goods.Value)
		{
			if (_showPartyColInBill)
			{
				mtBillRow.IsPartySelected = _party.IsChecked.HasValue && _party.IsChecked.Value;
			}
			else
			{
				mtBillRow.IsPartySelected = false;
			}
		}
		else
		{
			mtBillRow.IsPartySelected = null;
		}
		mtBillRow.RowID = _rowId;
		MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(mtBillRow.ItemID, mtBillRow.UnitCode);
		mtBillRow.BarCode = ((barCode == null) ? "" : barCode.BarCode);
		if (MarkingHelpers.IsGoodMarkable(mtBillRow.ItemCode, Settings.ParametersService.UseMarking(), markingListHasValue: false, _docDate, out var isRowMarkable))
		{
			mtBillRow.IsRowMarkable = isRowMarkable;
		}
	}

	private void InnerSaveForMTMove(MTMoveWindow doc, ref MTMoveWindow.MTMoveSpecificationRow mtMoveRow, ref int mtListCount)
	{
		if (_mode == DialogWindowMode.NewModeWithNext)
		{
			mtMoveRow = new MTMoveWindow.MTMoveSpecificationRow(doc);
			_mtMoveList.Add(mtMoveRow);
			mtListCount = _mtMoveList.Count - 1;
		}
		else
		{
			mtMoveRow = _mtMoveSourceRow;
		}
		mtMoveRow.MTCode = _itemCode.Code;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
		mtMoveRow.MTID = item.Id;
		mtMoveRow.MTCode = item.MTCode;
		mtMoveRow.MTName = _itemName.Text;
		mtMoveRow.UnitCode = _unit.Code;
		mtMoveRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code)?.Brief ?? "";
		mtMoveRow.RowDescription = _rowDesc.Text;
		mtMoveRow.Quantity = _quantity.Value;
		mtMoveRow.IsPartySelected = _party.IsChecked.HasValue && _party.IsChecked.Value;
		mtMoveRow.RowID = _rowId;
	}

	private void InnerSaveForMTMoveWithConfirmation(MTMoveWithConfirmationWindow doc, ref MTMoveWithConfirmationWindow.MTMoveSpecificationRow mtMoveWithConfirmationRow, ref int mtListCount)
	{
		if (_mode == DialogWindowMode.NewModeWithNext)
		{
			mtMoveWithConfirmationRow = new MTMoveWithConfirmationWindow.MTMoveSpecificationRow(doc);
			_mtMoveWithConfirmationList.Add(mtMoveWithConfirmationRow);
			mtListCount = _mtMoveWithConfirmationList.Count - 1;
		}
		else
		{
			mtMoveWithConfirmationRow = _mtMoveWithConfirmationSourceRow;
		}
		mtMoveWithConfirmationRow.MTCode = _itemCode.Code;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_itemCode.Code);
		mtMoveWithConfirmationRow.MTID = item.Id;
		mtMoveWithConfirmationRow.MTCode = item.MTCode;
		mtMoveWithConfirmationRow.MTName = _itemName.Text;
		mtMoveWithConfirmationRow.UnitCode = _unit.Code;
		mtMoveWithConfirmationRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code)?.Brief ?? "";
		mtMoveWithConfirmationRow.RowDescription = _rowDesc.Text;
		mtMoveWithConfirmationRow.Quantity = _quantity.Value;
		mtMoveWithConfirmationRow.IsPartySelected = _party.IsChecked.HasValue && _party.IsChecked.Value;
		mtMoveWithConfirmationRow.RowID = _rowId;
	}

	private void FEFO_Click(object sender, RoutedEventArgs e)
	{
		_party.IsChecked = true;
		DistributeQtyByExpiredDate(_quantity.Value);
	}
}
