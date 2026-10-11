using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
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
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents.Goods;

[DocumentWindow(33)]
public class InvoiceForPayWindow : MTInvoiceBaseWindow<InvoiceForPay>
{
	public class InvoiceForPaySpecificationRow : MTInvoiceBaseSpecificationRow
	{
		static InvoiceForPaySpecificationRow()
		{
			Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>> invoiceBasePropertyValidators = MTInvoiceBaseSpecificationRow.GetInvoiceBasePropertyValidators();
			DocumentGridRow.RegisterPropertyValidators(typeof(InvoiceForPaySpecificationRow), cacheValidationResults: true, invoiceBasePropertyValidators);
		}
	}

	protected delegate void RowInfoChangeEventHandler(InvoiceForPaySpecificationRow Row);

	private delegate void DelegateSetRowInfoToInfoPanel(InvoiceForPaySpecificationRow Row);

	private delegate void DelegateClearInfoPanel();

	private DropDownControl _buyer;

	private TextEditExt _barCode;

	private Button _codeEnter;

	private DropDownControl _currCode;

	private ExchangeRateControl _currExChngRate;

	public DropDownControl _contract;

	private DateEditControl _contractDate;

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

	private TextEditExt _buyerPassport;

	private DropDownControl _vatType;

	private DropDownControl _priceListType;

	private PopupCalcEditExt _discountPercent;

	private DropDownControl _storage;

	private PopupCalcEditExt _vatSumma;

	private PopupCalcEditExt _totalSumma;

	private LayoutGroup _infoDisplay;

	private TextEditExt _infoItemName;

	private InfoDisplayControls _infoDisplayControls;

	private TextEdit _infoFormula;

	private bool _isChanged = true;

	private SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs> _focusedRowHandleChangedEvent;

	private DocumentGridControl _invoiceGrid;

	private TableView _invoiceTableView;

	private BarEditItemExt _filteredSearch;

	public BarEditItemExt _discountPriceType;

	private ObservableCollection<InvoiceForPaySpecificationRow> _invoicesList;

	private DocumentGridColumn _itemTypeCol;

	private DocumentGridColumn _storageCol;

	private DocumentGridColumn _itemCodeCol;

	private DocumentGridColumn _itemNameCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _unitCol;

	private DocumentGridColumn _priceCol;

	private DocumentGridColumn _discountCol;

	private DocumentGridColumn _discountPriceCol;

	private DocumentGridColumn _summaCol;

	private DocumentGridColumn _vatCol;

	private Dictionary<(int, string), decimal> _availabilityByGoodId = new Dictionary<(int, string), decimal>();

	private string _mPriceListType = Settings.ParametersService.DefaultSalePrice();

	private string _priceListTypeRoundDgr = "";

	private readonly bool _isUsingDiscount = Settings.ParametersService.DiscountUse();

	private readonly bool _isCalcDiscountAuto = Settings.ParametersService.CalcDiscountAuto();

	private readonly bool _alwaysCalcDiscountAuto = Settings.ParametersService.AlwaysCalcDiscountAuto();

	private readonly bool _allowChangeSalePrices = Settings.ParametersService.AllowChangeSalePrices();

	private readonly bool _allowChangeDiscountPrices = Settings.ParametersService.AllowChangeDiscountPrice();

	private readonly bool _allowChangePriceType = Settings.ParametersService.AllowChangePriceType();

	private readonly bool _allowChangeDiscountPriceType = Settings.ParametersService.AllowChangeDiscountPriceType();

	private DXTabControl _tabs;

	private int _priceListTabControlIndex;

	private bool _validateTotalSumm;

	private int _maxRowId;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private ReportBarButtonItem _barButtonJumpTo;

	public bool _bForB2BDocLoad;

	public string _discountPriceListType = string.Empty;

	public string _discountPriceListVATType = string.Empty;

	public string _discountPriceListCurr = string.Empty;

	public string _discountPriceTypeRoundDegree = string.Empty;

	public decimal _discountPriceListCurrRate = 1m;

	public decimal _discountPriceListCurrBase = 1m;

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

	protected event RowInfoChangeEventHandler RowInfoChangeHandler;

	public InvoiceForPayWindow()
	{
	}

	public InvoiceForPayWindow(InvoiceForPay doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Date_EditValueChanged, this).Handler;
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_buyer = AddDropDownControl(null, ((PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"]).PartnerFieldDefinition, new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, "", isFilterBySuppliers: false, isFilterByCustomers: true), allowAdditionButtons: true, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_buyer.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Buyer_EditValueChanged, this).Handler;
		LayoutGroupExt container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barCode = AddStringControl(container2, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch);
		_barCode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_codeEnter = AddButton(container2, null, text, null, text, isTabStop: true);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		_tabs = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.General, "General");
		_currCode = AddDropDownControl(container3, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_currCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrCode_EditValueChanged, this).Handler;
		_currExChngRate = AddExchangeRateControl(container3, Definition.Fields["ExchangeRate"]);
		_contract = AddDropDownControl(container3, ((PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"]).ContractFieldDefinition, new Contracts("", "", isExceptCurrCodeNCC: false, isFilterBySuppliers: false, isFilterByCustomers: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_contract.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Contract_EditValueChanged, this).Handler;
		_contract.IsEnabled = false;
		_contractDate = AddDateControl(container3, Definition.Fields["ContractDate"]);
		_comment = AddComboBoxControl(container3, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container3, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		LayoutGroup container4 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.ItemList, "PriceList");
		_priceListTabControlIndex = _tabs.Items.Count - 1;
		LayoutGroupExt container5 = AddLayoutGroup(container4, LayoutGroupView.Group, Orientation.Horizontal);
		_vatType = AddDropDownControl(container5, Definition.Fields["VATType"], new VATTypes(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_vatType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(VATType_EditValueChanged, this).Handler;
		_priceListType = AddDropDownControl(container4, Definition.Fields["PriceListType"], new PriceTypes(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_priceListType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PriceListType_EditValueChanged, this).Handler;
		_discountPercent = AddNumericControl(container5, Definition.Fields["Discount"], useCurrencyFormat: false, HorizontalAlignment.Right);
		_discountPercent.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DiscountPercent_EditValueChanged, this).Handler;
		_storage = AddDropDownControl(container4, Definition.Fields["Storage"], new Storages(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_storage.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged, this).Handler;
		if (_bForB2BDocLoad)
		{
			_storage.Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_storage.Parent).Visibility = Visibility.Collapsed;
			_barCode.Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_barCode.Parent).Visibility = Visibility.Collapsed;
			_codeEnter.Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_codeEnter.Parent).Visibility = Visibility.Collapsed;
		}
		_invoiceGrid = AddGrid(container4, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_invoiceGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTInvoiceGrid_AddingRow, this).Handler;
		_invoiceGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTInvoiceGrid_AddingRowWithDialog, this).Handler;
		_invoiceGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTInvoiceGrid_DuplicatingRowWithDialog, this).Handler;
		_invoiceGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTInvoiceGrid_ClickDropDown, this).Handler;
		_invoiceGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTInvoiceGrid_DeletedRow, this).Handler;
		_invoiceGrid.DeletingRow += new SafeDocumentEventHandler<DocumentGridDeletingRowEventArgs>(MTInvoiceGrid_DeletingRow, this).Handler;
		_invoiceGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTInvoiceGrid_DuplicatingRow, this).Handler;
		_invoiceGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(InvoiceForPayGrid_RefreshButtons, this).Handler;
		_invoiceTableView = _invoiceGrid.View;
		_focusedRowHandleChangedEvent = new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTInvoiceTableView_FocusedRowHandleChangedEventArgs, this);
		_invoiceTableView.FocusedRowHandleChanged += _focusedRowHandleChangedEvent.Handler;
		_invoiceTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTInvoiceTableView_CellValueChanged, this).Handler;
		_invoiceTableView.ShowingEditor += new SafeDocumentEventHandler<ShowingEditorEventArgs>(MTInvoiceTableView_ShowingEditor, this).Handler;
		_invoiceGrid.ToolTip = ArmSoft.SME.Common.Resources.ItemList;
		if (_isUsingDiscount && !_anyAutoDiscountModeIsEnabled)
		{
			_discountPriceType = _invoiceGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.DiscountPriceType, ArmSoft.SME.Trade.Common.Resources.DiscountPriceType, 130.0);
			_discountPriceType.Click += new SafeDocumentEventHandler<RoutedEventArgs>(DiscountPriceType_Click, this).Handler;
		}
		_filteredSearch = _invoiceGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_invoiceGrid.AddRowIndicator(35);
		_itemTypeCol = _invoiceGrid.AddSmartEnumDropDownColumn(Definition.Grids["MTList"].Columns["ItemType"]);
		_itemTypeCol.Width = 55.0;
		_storageCol = _invoiceGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Storage"]);
		_itemCodeCol = _invoiceGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["ItemCode"], new GoodsFilter(), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_itemCodeCol.Width = 125.0;
		if (_bForB2BDocLoad)
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
		_vatCol = _invoiceGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["VAT"]);
		_vatCol.Width = 45.0;
		foreach (GridColumn column in _invoiceGrid.Grid.Columns)
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
		if (!_isUsingDiscount)
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
		_invoicesList = new ObservableCollection<InvoiceForPaySpecificationRow>();
		_vatSumma = AddNumericControl(container4, Definition.Fields["SummVAT"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_totalSumma = AddNumericControl(container4, Definition.Fields["Summ"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_totalSumma.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(TotalSumma_EditValueChanging, this).Handler;
		if (Settings.Localization == Localizations.Armenia)
		{
			_vatType.Code = "1";
		}
		else
		{
			_vatType.Code = "2";
		}
		_infoDisplayControls = new InfoDisplayControls(this, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay, isShowItemPictureOnInfoPanel: false, isShowStorageAvailabilityOnInfoPanel: true, isShowWholeAvailabilityOnInfoPanel: true);
		_infoDisplayControls.ConstructUI(container4);
		_infoDisplay = _infoDisplayControls.InfoDisplay;
		_infoItemName = _infoDisplayControls.InfoItemName;
		_infoFormula = _infoDisplayControls.InfoFormula;
		LayoutGroup container6 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.Supplier, "Supplier");
		_supplierName = AddStringControl(container6, Definition.Fields["SupplierName"], HorizontalAlignment.Stretch);
		_supplierLegalAddress = AddStringControl(container6, Definition.Fields["SupplierLegalAddress"], HorizontalAlignment.Stretch);
		_supplierBusinessAddress = AddDropDownCommentControl(container6, Definition.Fields["SupplierBusinessAddress"], new Storages(), HorizontalAlignment.Stretch);
		_supplierBusinessAddress.EditKeyValueChanged += new SafeDocumentEventHandler<object>(SupplierBusinessAddress_EditKeyValueChanged, this).Handler;
		_supplierBusinessAddress.MaxLength = 100;
		_supplierSettlAcc = AddDropDownControl(container6, Definition.Fields["SupplierSettlAcc"], new AccountInBanks(isKeySettlAccount: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_supplierSettlAcc.CheckValue = false;
		_supplierTaxCode = AddStringControl(container6, Definition.Fields["SupplierTaxCode"], HorizontalAlignment.Stretch);
		IEnumerable<string> itemsSource = SmartEnum<ManagerPositionsSE, string>.List.Select((ManagerPositionsSE row) => row.Caption);
		_supplierMngPost = AddComboBoxControl(container6, Definition.Fields["SupplierMngPost"], itemsSource, HorizontalAlignment.Stretch);
		_supplierMngName = AddStringControl(container6, Definition.Fields["SupplierMngName"], HorizontalAlignment.Stretch);
		IEnumerable<string> itemsSource2 = SmartEnum<ChiefAccPositionsSE, string>.List.Select((ChiefAccPositionsSE row) => row.Caption);
		_supplierChiefAccPost = AddComboBoxControl(container6, Definition.Fields["SupplierChiefAccPost"], itemsSource2, HorizontalAlignment.Stretch);
		_supplierChiefAccName = AddStringControl(container6, Definition.Fields["SupplierChiefAccName"], HorizontalAlignment.Stretch);
		LayoutGroup container7 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.Buyer, "Buyer");
		_buyerName = AddStringControl(container7, Definition.Fields["BuyerName"], HorizontalAlignment.Stretch);
		_buyerLegalAddress = AddStringControl(container7, Definition.Fields["BuyerLegalAddress"], HorizontalAlignment.Stretch);
		_buyerBusinessAddress = AddComboBoxControl(container7, Definition.Fields["BuyerBusinessAddress"], null, HorizontalAlignment.Stretch);
		_buyerSettlAcc = AddDropDownControl(container7, Definition.Fields["BuyerSettlAcc"], new PartnerAccountInBank(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_buyerSettlAcc.CheckValue = false;
		_buyerTaxCode = AddStringControl(container7, Definition.Fields["BuyerTaxCode"], HorizontalAlignment.Stretch);
		_buyerMngPost = AddComboBoxControl(container7, Definition.Fields["BuyerMngPost"], itemsSource, HorizontalAlignment.Stretch);
		_buyerMngName = AddStringControl(container7, Definition.Fields["BuyerMngName"], HorizontalAlignment.Stretch);
		_buyerChiefAccPost = AddComboBoxControl(container7, Definition.Fields["BuyerChiefAccPost"], itemsSource2, HorizontalAlignment.Stretch);
		_buyerChiefAccName = AddStringControl(container7, Definition.Fields["BuyerChiefAccName"], HorizontalAlignment.Stretch);
		_buyerStateRegNum = AddStringControl(container7, Definition.Fields["BuyerStateRegNum"], HorizontalAlignment.Stretch);
		_buyerPassport = AddStringControl(container7, Definition.Fields["BuyerPassport"], HorizontalAlignment.Stretch);
		RowInfoChangeHandler += ChangeRowInfo;
	}

	private void ChangeRowInfo(InvoiceForPaySpecificationRow Row = null)
	{
		if (Row == null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearInfoPanel(CleanInfoPanel), Array.Empty<object>());
			return;
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToInfoPanel(SetRowInfoToPanel), new object[1] { Row });
	}

	private void SetRowInfoToPanel(InvoiceForPaySpecificationRow row)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(row.ItemCode))
			{
				CleanInfoPanel();
				return;
			}
			_isChanged = IsChanged;
			decimal num = (row.Quantity * row.DiscountPrice).RoundAs(2);
			_infoItemName.Text = $"{row.ItemCode.Trim()} - {row.ItemName.Trim()}";
			_infoItemName.Text = $"{row.ItemCode.Trim()} - {row.ItemName.Trim()}";
			TextEdit infoFormula = _infoFormula;
			InlineArray4<object> buffer = default;
			buffer[0] = row.Quantity.CurrencyFormat(16, (short)((!(row.Quantity == (decimal)(int)row.Quantity)) ? 3 : 0));
			buffer[1] = row.UnitBrief.Trim();
			buffer[2] = row.DiscountPrice.CurrencyFormat(16, (short)((!(row.DiscountPrice == (decimal)(int)row.DiscountPrice)) ? 2 : 0));
			buffer[3] = num.CurrencyFormat(16, (short)((!(num == (decimal)(int)num)) ? 2 : 0));
			infoFormula.Text = string.Format("{0} {1} x {2} = {3}", (ReadOnlySpan<object?>)buffer);
			SetAvailabilityInfoToPanel(row);
			IsChanged = _isChanged;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SetAvailabilityInfoToPanel(InvoiceForPaySpecificationRow row)
	{
		_infoDisplayControls.ClearInfoAvailability();
		if (row.ItemType == GoodsTypesSE.Good.Value)
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
				_infoDisplayControls.SetAccUnitBrief(row.UnitBrief.Trim());
				_infoDisplayControls.SetWholeBalance(MaterialHelpers.GetGoodRemains(row.UpdateRemainsOnInfoPanel, row.ItemID, row.UnitCode, ref _availabilityByGoodId));
				_infoDisplayControls.SetWholeUnitBrief(row.UnitBrief.Trim());
				row.UpdateRemainsOnInfoPanel = false;
			}
		}
	}

	private void CleanInfoPanel()
	{
		_isChanged = IsChanged;
		_infoItemName.Text = "";
		_infoFormula.Text = "";
		_infoDisplayControls.ClearInfoAvailability();
		IsChanged = _isChanged;
	}

	private void FillSupplierRekvizits()
	{
		_supplierName.EditValue = Settings.ParametersService.CompanyName();
		_supplierLegalAddress.EditValue = Settings.ParametersService.CompanyAddress();
		_supplierTaxCode.EditValue = Settings.ParametersService.TaxCode();
		_supplierBusinessAddress.Text = Settings.ParametersService.CompanyBusinessAddress();
		_supplierSettlAcc.Code = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: true);
		_supplierMngPost.EditValue = Settings.ParametersService.ManagerPost();
		_supplierMngName.EditValue = Settings.ParametersService.ManagerName();
		_supplierChiefAccPost.EditValue = Settings.ParametersService.ChiefAccountantPost();
		_supplierChiefAccName.EditValue = Settings.ParametersService.ChiefAccountantName();
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.PartnerContractGroup.PartnerCode = _buyer.Code;
		PO.PartnerContractGroup.ContractCode = _contract.Code;
		PO.DiscountPriceListType = DiscountPriceListType;
		PO.MTList.Clear();
		foreach (InvoiceForPaySpecificationRow invoices in _invoicesList)
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
				Summa = invoices.Summa,
				DiscountSumma = invoices.DiscountSumma,
				VAT = invoices.VAT.Value,
				IsPartySelected = invoices.IsPartySelected
			};
			PO.MTList.Add(item);
		}
		PO.MaxRowID = _maxRowId;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_buyer.Code = PO.PartnerContractGroup.PartnerCode;
		_contract.Code = PO.PartnerContractGroup.ContractCode;
		_invoicesList.Clear();
		decimal num = 0m;
		_focusedRowHandleChangedEvent.HandleOnLoadingDocumentToControls = true;
		_discountPriceListType = PO.DiscountPriceListType;
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT in PO.MTList)
		{
			InvoiceForPaySpecificationRow invoiceForPaySpecificationRow = new InvoiceForPaySpecificationRow
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
				Summa = mT.Summa,
				DiscountSumma = mT.DiscountSumma,
				VAT = mT.VAT,
				IsPartySelected = mT.IsPartySelected
			};
			_invoicesList.Add(invoiceForPaySpecificationRow);
			mT.IsStorageRequired = false;
			num += Math.Abs(invoiceForPaySpecificationRow.Discount);
		}
		_maxRowId = PO.MaxRowID;
		_invoiceGrid.ItemsSource = _invoicesList;
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
		{
			_priceCol.Header = ArmSoft.SME.Common.Resources.Price;
			_discountPriceCol.Header = ArmSoft.SME.Common.Resources.DiscountedPrice;
			_summaCol.Header = ArmSoft.SME.Common.Resources.Summa;
			_currExChngRate.Rate = 1m;
			_currExChngRate.Base = 1m;
		}
		else
		{
			_priceCol.Header = ArmSoft.SME.Common.Resources.FCPrice;
			_discountPriceCol.Header = ArmSoft.SME.Trade.Common.Resources.FCDiscPrice;
			_summaCol.Header = ArmSoft.SME.Common.Resources.FCSumma;
		}
		if (!_isUsingDiscount && num == 0m)
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
			_discountCol.AllowFocus = false;
			_discountPriceCol.ReadOnly = true;
			_discountPriceCol.AllowFocus = false;
		}
		VATType_EditValueChangedForLoad();
		BindPartnerDataViews();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num2 = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num2);
		CollectionsMarshal.SetCount(list, num2);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString());
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		if (_viewMode != WindowViewMode.Copy)
		{
			string text = Settings.ParametersService.DefaultStorage();
			if (!string.IsNullOrWhiteSpace(text))
			{
				_storage.Code = text;
			}
			_priceListType.Code = _mPriceListType;
			FillSupplierRekvizits();
			DiscountPriceListType = Settings.ParametersService.DefaultDiscountPrice();
		}
		if (!string.IsNullOrWhiteSpace(Settings.ParametersService.VATType()))
		{
			_vatType.Code = Settings.ParametersService.VATType();
		}
		if (PO.DocDate == Settings.HostingEnvironment.CurrentDate)
		{
			CurrCodeValueChanged();
		}
		_priceListType.IsReadOnly = !_allowChangePriceType;
	}

	protected override bool AllowNewAfterSave()
	{
		return !_bForB2BDocLoad;
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_invoicesList.Clear();
		_vatSumma.EditValue = 0m;
		_totalSumma.EditValue = 0m;
		_owner.Code = Settings.LoginService.UserName;
		_priceListType.IsReadOnly = !_allowChangePriceType;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_vatSumma.IsReadOnly = true;
			_supplierName.IsReadOnly = true;
			_supplierLegalAddress.IsReadOnly = true;
			_supplierTaxCode.IsReadOnly = true;
			_currExChngRate.IsReadOnly = _currCode.Code == "AMD";
			_contract.IsEnabled = !string.IsNullOrEmpty(_buyer.Code);
		}
	}

	private void CalcSummAndSummVAT()
	{
		decimal num = 0m;
		decimal num2 = 0m;
		_validateTotalSumm = false;
		if (_vatType.Code.Trim().InList("1", "2"))
		{
			foreach (InvoiceForPaySpecificationRow invoices in _invoicesList)
			{
				num += PriceHelper.CalcVATAmountByType(invoices.Summa, _vatType.Code, invoices.VAT.Value);
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
			_vatSumma.EditValue = 0m;
			foreach (InvoiceForPaySpecificationRow invoices2 in _invoicesList)
			{
				num2 += invoices2.Summa;
			}
			_totalSumma.EditValue = num2;
		}
		_validateTotalSumm = true;
	}

	private void CalcInvoiceTotalAndVATSumm(ref decimal TotalSumm, ref decimal VATSumm)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		if (_vatType.Code.Trim().InList("1", "2"))
		{
			foreach (InvoiceForPaySpecificationRow invoices in _invoicesList)
			{
				num += PriceHelper.CalcVATAmountByType(invoices.Summa, _vatType.Code, invoices.VAT.Value);
				num2 += invoices.Summa;
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
		foreach (InvoiceForPaySpecificationRow invoices2 in _invoicesList)
		{
			num2 += invoices2.Summa;
		}
		TotalSumm = num2;
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!StoredInDb && !string.IsNullOrWhiteSpace(_storage.Code))
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
			if (item != null && !string.IsNullOrWhiteSpace(item.Address))
			{
				_supplierBusinessAddress.Text = item.Address;
			}
		}
		RowInfoChangeHandler?.Invoke((InvoiceForPaySpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void VATType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		VATType_EditValueChangedForLoad();
		CalcSummAndSummVAT();
		_vatSumma.DoValidate();
		RowInfoChangeHandler?.Invoke((InvoiceForPaySpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void VATType_EditValueChangedForLoad()
	{
		bool flag = _vatType.Code.InList("1", "2");
		_invoiceGrid.Grid.Columns["VAT"].Visible = flag;
		_invoiceGrid.Grid.Columns["VAT"].ShowInColumnChooser = flag;
	}

	private void CurrCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
		{
			_currExChngRate.IsReadOnly = true;
			_priceCol.Header = ArmSoft.SME.Common.Resources.Price;
			_discountPriceCol.Header = ArmSoft.SME.Common.Resources.DiscountedPrice;
			_summaCol.Header = ArmSoft.SME.Common.Resources.Summa;
			_currExChngRate.Rate = 1m;
			_currExChngRate.Base = 1m;
		}
		else
		{
			_currExChngRate.IsReadOnly = false;
			_priceCol.Header = ArmSoft.SME.Common.Resources.FCPrice;
			_discountPriceCol.Header = ArmSoft.SME.Trade.Common.Resources.FCDiscPrice;
			_summaCol.Header = ArmSoft.SME.Common.Resources.FCSumma;
			CurrCodeValueChanged();
		}
		_currExChngRate.DoValidate();
		RowInfoChangeHandler?.Invoke((InvoiceForPaySpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_date.DoValidate() && _date.EditValue.HasValue)
		{
			CurrCodeValueChanged();
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

	private void Buyer_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_buyer.Code) && _buyer.DoValidate())
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_buyer.Code, throwExceptionNotExits: true);
			if (Settings.Localization == Localizations.Armenia || !_buyerName.IsReadOnly)
			{
				_buyerName.Text = item.Caption;
			}
			_buyerLegalAddress.Text = item.Address;
			_buyerBusinessAddress.Text = item.BusinessAddress;
			_buyerSettlAcc.Code = TradeSettings.TradeCacheService.Partner.GetItem(item.PartCode).SettlAcc;
			BindPartnerDataViews(item);
			if (Settings.Localization == Localizations.Armenia || !_buyerTaxCode.IsReadOnly)
			{
				_buyerTaxCode.Text = item.TaxCode;
			}
			_buyerMngPost.EditValue = item.ManagerPost;
			_buyerMngName.Text = item.ManagerFIO;
			_buyerChiefAccPost.EditValue = item.ChiefaccPost;
			_buyerChiefAccName.Text = item.ChiefaccFIO;
			_buyerStateRegNum.Text = item.PartStateRegister;
			_buyerPassport.Text = item.Passport;
			_contract.IsReadOnly = false;
			_contract.DataView = new Contracts(item.PartCode);
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item);
			_contract.Code = defaultContract.Contract;
			_contract.IsEnabled = true;
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(item.PartCode, _contract.Code);
			if (contract != null)
			{
				_contractDate.EditValue = contract.ContractDate;
			}
		}
		else
		{
			if (Settings.Localization == Localizations.Armenia || !_buyerName.IsReadOnly)
			{
				_buyerName.Text = "";
			}
			_buyerLegalAddress.Text = "";
			_buyerBusinessAddress.Text = "";
			_buyerSettlAcc.Code = "";
			if (Settings.Localization == Localizations.Armenia || !_buyerTaxCode.IsReadOnly)
			{
				_buyerTaxCode.Text = "";
			}
			_buyerMngPost.EditValue = "";
			_buyerMngName.Text = "";
			_buyerChiefAccPost.EditValue = "";
			_buyerChiefAccName.Text = "";
			_buyerStateRegNum.Text = "";
			_buyerPassport.Text = "";
			_contract.Code = "";
			_contract.IsReadOnly = true;
			BindPartnerDataViews();
		}
	}

	private void BindPartnerDataViews(PartnerCacheRow partnerCacheRow = null)
	{
		if (!string.IsNullOrWhiteSpace(_buyer.Code) && _buyer.DoValidate())
		{
			if (partnerCacheRow == null)
			{
				partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(_buyer.Code, throwExceptionNotExits: true);
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

	private void Contract_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_buyer.Code);
		if (item == null)
		{
			return;
		}
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(item.PartCode, _contract.Code);
		if (contract == null)
		{
			return;
		}
		_contractDate.EditValue = contract.ContractDate;
		if (string.IsNullOrWhiteSpace(_contract.Code) || !_contract.DoValidate())
		{
			return;
		}
		if (_isUsingDiscount)
		{
			_discountPercent.EditValue = contract.Percent;
		}
		if (_invoicesList.Count == 0 && !string.IsNullOrWhiteSpace(contract.PriceType))
		{
			_priceListType.Code = contract.PriceType;
			if (PriceHelper.GetPriceListRowVATType(_priceListType.Code, out var priceListRowVATType, out var _))
			{
				_vatType.Code = priceListRowVATType;
			}
			_currCode.Code = contract.CurrCode;
		}
	}

	private void MTInvoiceGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		_invoicesList.Add(new InvoiceForPaySpecificationRow
		{
			IsStorageRequired = false,
			ItemType = ItemTypesSE.Goods.Value,
			Storage = _storage.Code,
			Discount = _discountPercent.Value,
			VAT = _vatType.Code.InList("1", "2"),
			RowID = _maxRowId
		});
		_maxRowId++;
		e.RowNumber = _invoicesList.Count - 1;
		RowInfoChangeHandler?.Invoke((InvoiceForPaySpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void MTInvoiceGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
	}

	private void MTInvoiceGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
	}

	private void MTInvoiceGrid_EditingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
	}

	private void MTInvoiceGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			InvoiceForPaySpecificationRow invoiceForPaySpecificationRow = (InvoiceForPaySpecificationRow)_invoiceGrid.Grid.GetRow(e.RowHandle);
			if (e.Column == _itemCodeCol)
			{
				if (invoiceForPaySpecificationRow.ItemType == Convert.ToString(InvoiceRowTypeSE.Services.Value))
				{
					GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, InvoiceRowTypeSE.Services.Value, "", isShowOnlyInPriceList: true);
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
					bool showAll = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(invoiceForPaySpecificationRow.Storage);
					GoodsAvailabilityFilter goodsAvailabilityFilter = new GoodsAvailabilityFilter(_date.EditValue.Value, invoiceForPaySpecificationRow.Storage, forRetail: false, includeServices: false, showAll);
					ModalBrowser modalBrowser2 = new ModalBrowser(goodsAvailabilityFilter)
					{
						LookUpValue = e.Value,
						Owner = e.Owner,
						Left = e.Left,
						Top = e.Top
					};
					if (modalBrowser2.Show())
					{
						e.NewValue = goodsAvailabilityFilter.ValueMember(modalBrowser2.SelectedRowIndex);
					}
				}
				CalcSummAndSummVAT();
			}
			else if (e.Column == _storageCol)
			{
				if (invoiceForPaySpecificationRow == null || invoiceForPaySpecificationRow.ItemType != ItemTypesSE.Goods.Value)
				{
					return;
				}
				Storages storages = new Storages();
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
		CalcSummAndSummVAT();
		_invoiceGrid.Grid.RefreshData();
		RowInfoChangeHandler?.Invoke((InvoiceForPaySpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void MTInvoiceGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		InvoiceForPaySpecificationRow mtInvGridRow = (InvoiceForPaySpecificationRow)_invoiceGrid.Grid.GetRow(e.RowHandle);
		if (mtInvGridRow != null)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
		}
		RowInfoChangeHandler?.Invoke((InvoiceForPaySpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void MTInvoiceTableView_FocusedRowHandleChangedEventArgs(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _invoiceTableView.FocusedRowHandle;
		InvoiceForPaySpecificationRow invoiceForPaySpecificationRow = (InvoiceForPaySpecificationRow)_invoiceGrid.Grid.GetRow(focusedRowHandle);
		if (invoiceForPaySpecificationRow != null)
		{
			ChangeItemCodeColumnAdditonalView(invoiceForPaySpecificationRow.ItemType, invoiceForPaySpecificationRow.ItemCode);
		}
		RowInfoChangeHandler?.Invoke((InvoiceForPaySpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void ChangeItemCodeColumnAdditonalView(string TypeValue, string ItemCode)
	{
		ItemTypesSE itemTypesSE = ItemTypesSE.All;
		if (TypeValue == ItemTypesSE.Goods.Value)
		{
			itemTypesSE = ItemTypesSE.Goods;
		}
		else if (TypeValue == ItemTypesSE.Services.Value)
		{
			itemTypesSE = ItemTypesSE.Services;
		}
		_itemCodeCol.DataView = new GoodsFilter(shortMode: true, itemTypesSE.Value, "", isShowOnlyInPriceList: true);
		_unitCodeCol.DataView = new MTQntUnits(ItemCode, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", itemTypesSE.Value);
	}

	private void MTInvoiceTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		InvoiceForPaySpecificationRow mtInvGridRow = (InvoiceForPaySpecificationRow)_invoiceGrid.Grid.GetRow(e.RowHandle);
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
		if (e.Column == _itemTypeCol)
		{
			if (text != text2)
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
				mtInvGridRow.UnitCode = "";
				mtInvGridRow.UnitBrief = "";
				mtInvGridRow.IsPartySelected = !(mtInvGridRow.ItemType == ItemTypesSE.Goods.Value);
				if (text == ItemTypesSE.Goods.Value)
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
				}
				if (text2 != ItemTypesSE.Goods.Value)
				{
					mtInvGridRow.IsPartySelected = true;
				}
				if (text2 != ItemTypesSE.Goods.Value && text2 != Convert.ToString(InvoiceRowTypeSE.Services.Value))
				{
					mtInvGridRow.ItemType = text;
				}
				if (mtInvGridRow.ItemType == ItemTypesSE.Goods.Value && string.IsNullOrWhiteSpace(mtInvGridRow.Storage))
				{
					mtInvGridRow.Storage = _storage.Code;
				}
				ChangeItemCodeColumnAdditonalView(text2, "");
				RowInfoChangeHandler?.Invoke(mtInvGridRow);
			}
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
					mtInvGridRow.IsPartySelected = !(mtInvGridRow.ItemType == ItemTypesSE.Goods.Value);
				}
				else if (text.Trim() != e.Value.ToString().Trim())
				{
					if (mtInvGridRow.ItemType == ItemTypesSE.Goods.Value && !string.IsNullOrWhiteSpace(text) && text.Trim() != e.Value.ToString().Trim())
					{
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
						mtInvGridRow.IsPartySelected = false;
					}
					string text3 = e.Value.ToString();
					foreach (InvoiceForPaySpecificationRow invoices in _invoicesList)
					{
						if (mtInvGridRow != invoices && text3.TrimEnd() == invoices.ItemCode.TrimEnd() && invoices.ItemType == mtInvGridRow.ItemType)
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
								mtInvGridRow.UnitCode = "";
								mtInvGridRow.UnitBrief = "";
								mtInvGridRow.IsPartySelected = !(mtInvGridRow.ItemType == ItemTypesSE.Goods.Value);
								_invoiceGrid.Grid.RefreshData();
								return;
							}
							break;
						}
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
					if (_isUsingDiscount)
					{
						if (string.IsNullOrWhiteSpace(DiscountPriceListType))
						{
							mtInvGridRow.Discount = _discountPercent.Value;
							if (mtInvGridRow.Discount == 0m || item.Discount != 0m)
							{
								mtInvGridRow.Discount = item.Discount;
							}
							mtInvGridRow.DiscountPrice = PriceHelper.CalculateDiscountPrice(mtInvGridRow.Price, mtInvGridRow.Discount);
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
					mtInvGridRow.Summa = PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, mtInvGridRow.Quantity);
					mtInvGridRow.IsPartySelected = false;
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
				mtInvGridRow.IsPartySelected = !(mtInvGridRow.ItemType == ItemTypesSE.Goods.Value);
			}
			finally
			{
				RowInfoChangeHandler?.Invoke(mtInvGridRow);
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
						mtInvGridRow.Price = PriceHelper.GetPriceForUnit(text, Convert.ToString(e.Value), mtInvGridRow.Price, mtInvGridRow.ItemCode);
					}
					else
					{
						PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_priceListType.Code, mtInvGridRow.ItemID, Convert.ToString(e.Value), _date.EditValue.Value, _currCode.Code);
						if (price.Price != 0m)
						{
							mtInvGridRow.Price = price.Price;
						}
						else
						{
							mtInvGridRow.Price = PriceHelper.GetPriceForUnit(text, Convert.ToString(e.Value), mtInvGridRow.Price, mtInvGridRow.ItemCode);
						}
					}
					mtInvGridRow.Price = Methods.RoundPrice(mtInvGridRow.Price, _priceListTypeRoundDgr);
					mtInvGridRow.DiscountPrice = (mtInvGridRow.Price - mtInvGridRow.Price * mtInvGridRow.Discount / 100m).RoundAs(2);
					mtInvGridRow.Summa = (mtInvGridRow.DiscountPrice * mtInvGridRow.Quantity).RoundAs(2);
				}
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
			finally
			{
				RowInfoChangeHandler?.Invoke(mtInvGridRow);
			}
		}
		else if (e.Column == _storageCol)
		{
			if (mtInvGridRow.ItemType == ItemTypesSE.Goods.Value && mtInvGridRow.IsPartySelected == true)
			{
				PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
				mtInvGridRow.IsPartySelected = false;
				RowInfoChangeHandler?.Invoke(mtInvGridRow);
			}
		}
		else if (e.Column == _quantityCol)
		{
			if (mtInvGridRow.ItemType == ItemTypesSE.Goods.Value && Convert.ToDecimal(e.OldValue) != Convert.ToDecimal(e.Value) && Convert.ToDecimal(e.OldValue) != 0m && mtInvGridRow.IsPartySelected.HasValue && mtInvGridRow.IsPartySelected.Value)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ForRowSelectedParties, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
				mtInvGridRow.IsPartySelected = false;
			}
			mtInvGridRow.Summa = (Convert.ToDecimal(e.Value) * mtInvGridRow.DiscountPrice).RoundAs(2);
			RowInfoChangeHandler?.Invoke(mtInvGridRow);
		}
		else if (e.Column == _priceCol)
		{
			mtInvGridRow.Price = Convert.ToDecimal(e.Value);
			mtInvGridRow.DiscountPrice = (mtInvGridRow.Price - mtInvGridRow.Price * mtInvGridRow.Discount / 100m).RoundAs(2);
			mtInvGridRow.Summa = (mtInvGridRow.DiscountPrice * mtInvGridRow.Quantity).RoundAs(2);
			if (!string.IsNullOrWhiteSpace(DiscountPriceListType) && !(mtInvGridRow.Price == 0m) && mtInvGridRow.DiscountPrice == 0m)
			{
				mtInvGridRow.Discount = (100m * (mtInvGridRow.Price - mtInvGridRow.DiscountPrice) / mtInvGridRow.Price).RoundAs(2);
			}
			_priceListTypeRoundDgr = "";
			CalculateGridRowSummaAndDiscountSumma(ref mtInvGridRow, string.IsNullOrWhiteSpace(DiscountPriceListType) || mtInvGridRow.DiscountPrice == 0m);
			_priceListTypeRoundDgr = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(_priceListType.Code);
			mtInvGridRow.Summa = PriceHelper.CalculateSum(mtInvGridRow.DiscountPrice, mtInvGridRow.Quantity);
			RowInfoChangeHandler?.Invoke(mtInvGridRow);
		}
		else if (e.Column == _discountCol)
		{
			mtInvGridRow.DiscountPrice = (mtInvGridRow.Price - mtInvGridRow.Price * mtInvGridRow.Discount / 100m).RoundAs(2);
			mtInvGridRow.Summa = (mtInvGridRow.DiscountPrice * mtInvGridRow.Quantity).RoundAs(2);
		}
		else if (e.Column == _discountPriceCol)
		{
			mtInvGridRow.DiscountPrice = Convert.ToDecimal(e.Value);
			mtInvGridRow.Summa = (mtInvGridRow.DiscountPrice * mtInvGridRow.Quantity).RoundAs(2);
			if (mtInvGridRow.Price == 0m)
			{
				mtInvGridRow.Price = (mtInvGridRow.DiscountPrice * 100m / (100m - mtInvGridRow.Discount)).RoundAs(2);
			}
			else
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
			RowInfoChangeHandler?.Invoke(mtInvGridRow);
		}
		else if (e.Column == _summaCol)
		{
			mtInvGridRow.Summa = Convert.ToDecimal(e.Value);
			if (mtInvGridRow.Quantity > 0m)
			{
				if (mtInvGridRow.Discount == 100m)
				{
					mtInvGridRow.DiscountPrice = 0m.RoundAs(2);
					mtInvGridRow.Price = 0m.RoundAs(2);
				}
				else
				{
					mtInvGridRow.DiscountPrice = (mtInvGridRow.Summa / mtInvGridRow.Quantity).RoundAs(2);
					mtInvGridRow.Price = (_isUsingDiscount ? (mtInvGridRow.DiscountPrice * 100m / (100m - mtInvGridRow.Discount)).RoundAs(2) : mtInvGridRow.DiscountPrice);
				}
			}
			RowInfoChangeHandler?.Invoke(mtInvGridRow);
		}
		_invoiceGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
		RowInfoChangeHandler?.Invoke(mtInvGridRow);
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

	private void MTInvoiceTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (e.Row == null)
		{
			return;
		}
		if (e.Column == _storageCol)
		{
			if (((InvoiceForPaySpecificationRow)e.Row).ItemType == ItemTypesSE.Services.Value)
			{
				e.Cancel = true;
			}
		}
		else if (e.Column == _unitCol)
		{
			_ = (InvoiceForPaySpecificationRow)e.Row;
			e.Cancel = true;
		}
		else if (e.Column == _itemNameCol && ((InvoiceForPaySpecificationRow)e.Row).ItemType == ItemTypesSE.Goods.Value)
		{
			e.Cancel = true;
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
			string barCode = _barCode.Text.Trim();
			if (Settings.ParametersService.UsePartnerEmployeeSearchNewMode())
			{
				if (DocumentHelpers.IsValidPartnerEmployeeCardCode(barCode, Settings.ParametersService.PartnerEmployeeCardCodePrefixes(), Settings.ParametersService.UsePartnerCardLenght(), Settings.ParametersService.PartnerCardLenght()))
				{
					_barCode.Text = "";
					_barCode.Focus();
					DocumentHelpers.GetPartnerEmployeeDataByBarCode(barCode, out var partnerCacheRow, out var partnerContract, out var _);
					FillPartnerEmployeeData(barCode, partnerCacheRow, partnerContract);
					return;
				}
			}
			else if (MCRInfo.GetInstance().IsValidCard(barCode))
			{
				_barCode.Text = "";
				_barCode.Focus();
				DocumentHelpers.GetPartnerEmployeeDataByBarCode(MCRInfo.GetInstance().GetCardNumber(barCode), out var partnerCacheRow2, out var partnerContract2, out var _);
				FillPartnerEmployeeData(barCode, partnerCacheRow2, partnerContract2);
				return;
			}
			decimal qty = 1m;
			string unitCode = string.Empty;
			string unitBrief = string.Empty;
			MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out qty, out unitCode, out unitBrief);
			if (materialCacheRow == null)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeNotExist, barCode));
			}
			if (_tabs.SelectedIndex != _priceListTabControlIndex)
			{
				_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
			}
			AddMTToList(materialCacheRow, unitCode, unitBrief, qty);
			_barCode.Text = "";
			CalcSummAndSummVAT();
			_vatSumma.DoValidate();
			ControlsHelper.SetFocus(_barCode);
			RowInfoChangeHandler?.Invoke((InvoiceForPaySpecificationRow)_invoiceTableView.Grid.CurrentItem);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddMTToList(MaterialCacheRow mtRow, string unitCode = "", string unitBrief = "", decimal quantity = 1m)
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		for (int i = 0; i <= _invoicesList.Count - 1; i++)
		{
			if (mtRow.Id != _invoicesList[i].ItemID || !(_storage.Code == _invoicesList[i].Storage) || !_invoicesList[i].IsPartySelected.HasValue || _invoicesList[i].IsPartySelected.Value || !(_invoicesList[i].UnitCode == (string.IsNullOrWhiteSpace(unitCode) ? mtRow.Unit : unitCode)))
			{
				continue;
			}
			_invoicesList[i].ItemCode = mtRow.MTCode;
			_invoicesList[i].ItemID = mtRow.Id;
			_invoicesList[i].ItemName = mtRow.Caption;
			_invoicesList[i].UnitBrief = (string.IsNullOrWhiteSpace(unitBrief) ? mtRow.UnitRow.Brief : unitBrief);
			_invoicesList[i].Quantity += quantity;
			if (_invoicesList[i].Price == 0m)
			{
				_invoicesList[i].Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, _invoicesList[i].ItemID, _invoicesList[i].UnitCode, _date.EditValue.Value, _currCode.Code);
				_invoicesList[i].Price = Methods.RoundPrice(_invoicesList[i].Price, _priceListTypeRoundDgr);
			}
			if (_isUsingDiscount)
			{
				if (string.IsNullOrWhiteSpace(DiscountPriceListType))
				{
					_invoicesList[i].Discount = _discountPercent.Value;
					if (_invoicesList[i].Discount == 0m || mtRow.Discount != 0m)
					{
						_invoicesList[i].Discount = mtRow.Discount;
					}
					_invoicesList[i].DiscountPrice = PriceHelper.CalculateDiscountPrice(_invoicesList[i].Price, _invoicesList[i].Discount);
				}
				else
				{
					InvoiceForPaySpecificationRow invoiceForPayGridRow = _invoicesList[i];
					CalculateGridRowDiscountPriceInner(ref invoiceForPayGridRow);
					CalculateGridRowSummaAndDiscountSumma(ref invoiceForPayGridRow, invoiceForPayGridRow.DiscountPrice == 0m);
				}
			}
			else
			{
				_invoicesList[i].DiscountPrice = _invoicesList[i].Price;
			}
			_invoicesList[i].Summa = PriceHelper.CalculateSum(_invoicesList[i].DiscountPrice, _invoicesList[i].Quantity);
			int rowHandleByListIndex = _invoiceGrid.Grid.GetRowHandleByListIndex(i);
			_invoiceGrid.Grid.RefreshRow(rowHandleByListIndex);
			return;
		}
		InvoiceForPaySpecificationRow invoiceForPayGridRow2 = new InvoiceForPaySpecificationRow();
		invoiceForPayGridRow2.IsStorageRequired = false;
		invoiceForPayGridRow2.ItemType = ItemTypesSE.Goods.Value;
		invoiceForPayGridRow2.Storage = _storage.Code;
		invoiceForPayGridRow2.IsPartySelected = false;
		invoiceForPayGridRow2.ItemID = mtRow.Id;
		invoiceForPayGridRow2.ItemCode = mtRow.MTCode;
		invoiceForPayGridRow2.ItemName = mtRow.Caption;
		invoiceForPayGridRow2.UnitBrief = (string.IsNullOrWhiteSpace(unitBrief) ? mtRow.UnitRow.Brief : unitBrief);
		invoiceForPayGridRow2.UnitCode = (string.IsNullOrWhiteSpace(unitCode) ? mtRow.Unit : unitCode);
		invoiceForPayGridRow2.VAT = mtRow.VAT;
		invoiceForPayGridRow2.Quantity = quantity;
		invoiceForPayGridRow2.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mtRow.Id, invoiceForPayGridRow2.UnitCode, _date.EditValue.Value, _currCode.Code);
		invoiceForPayGridRow2.Price = Methods.RoundPrice(invoiceForPayGridRow2.Price, _priceListTypeRoundDgr);
		if (_isUsingDiscount)
		{
			if (string.IsNullOrWhiteSpace(DiscountPriceListType))
			{
				invoiceForPayGridRow2.Discount = _discountPercent.Value;
				if (invoiceForPayGridRow2.Discount == 0m)
				{
					invoiceForPayGridRow2.Discount = mtRow.Discount;
				}
				else if (mtRow.Discount != 0m)
				{
					invoiceForPayGridRow2.Discount = mtRow.Discount;
				}
				invoiceForPayGridRow2.DiscountPrice = PriceHelper.CalculateDiscountPrice(invoiceForPayGridRow2.Price, invoiceForPayGridRow2.Discount);
			}
			else
			{
				CalculateGridRowDiscountPriceInner(ref invoiceForPayGridRow2);
				CalculateGridRowSummaAndDiscountSumma(ref invoiceForPayGridRow2, invoiceForPayGridRow2.DiscountPrice == 0m);
			}
		}
		else
		{
			invoiceForPayGridRow2.DiscountPrice = invoiceForPayGridRow2.Price;
		}
		invoiceForPayGridRow2.Summa = PriceHelper.CalculateSum(invoiceForPayGridRow2.DiscountPrice, invoiceForPayGridRow2.Quantity);
		invoiceForPayGridRow2.RowID = _maxRowId;
		_invoicesList.Add(invoiceForPayGridRow2);
		_maxRowId++;
		int rowHandleByListIndex2 = _invoiceGrid.Grid.GetRowHandleByListIndex(_invoicesList.Count - 1);
		_invoiceGrid.Grid.RefreshRow(rowHandleByListIndex2);
		_invoiceGrid.ConfigureButtons();
		RowInfoChangeHandler?.Invoke(invoiceForPayGridRow2);
	}

	private void MTInvoiceGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		InvoiceForPaySpecificationRow invoiceForPaySpecificationRow = (InvoiceForPaySpecificationRow)_invoiceGrid.Grid.GetRow(e.RowHandle);
		if (invoiceForPaySpecificationRow != null)
		{
			InvoiceForPaySpecificationRow invoiceForPaySpecificationRow2 = new InvoiceForPaySpecificationRow();
			invoiceForPaySpecificationRow.Clone(invoiceForPaySpecificationRow2);
			invoiceForPaySpecificationRow2.IsStorageRequired = false;
			invoiceForPaySpecificationRow2.IsPartySelected = !(invoiceForPaySpecificationRow2.ItemType == ItemTypesSE.Goods.Value);
			invoiceForPaySpecificationRow2.RowID = _maxRowId;
			_invoicesList.Add(invoiceForPaySpecificationRow2);
			_maxRowId++;
			e.NewRowNumber = _invoicesList.Count - 1;
			CalcSummAndSummVAT();
		}
	}

	private void InvoiceForPayGrid_RefreshButtons(object sender, EventArgs e)
	{
		if (_isUsingDiscount && !_anyAutoDiscountModeIsEnabled)
		{
			_discountPriceType.IsEnabled = _invoiceGrid.AllowAdd && !_invoiceGrid.IsReadOnly;
		}
	}

	protected override void OnF2()
	{
		if (!_barCode.IsReadOnly)
		{
			ControlsHelper.SetFocus(_barCode);
		}
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
				string cardNumber = MCRInfo.GetInstance().GetCardNumber(MCRData);
				DocumentHelpers.GetPartnerEmployeeDataByBarCode(cardNumber, out var partnerCacheRow, out var partnerContract, out var _);
				FillPartnerEmployeeData(cardNumber, partnerCacheRow, partnerContract);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void TotalSumma_EditValueChanging(object sender, EditValueChangedEventArgs e)
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
			if (_currExChngRate.DoValidate() && Math.Abs(Convert.ToDecimal(e.NewValue) - TotalSumm) * _currExChngRate.Rate / _currExChngRate.Base > 100m)
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

	private void PriceListType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = false;
		string priceListRowVATType = "";
		_priceListTypeRoundDgr = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(_priceListType.Code);
		string priceListRowCurr;
		if (!string.IsNullOrWhiteSpace(_priceListType.Code) && _priceListType.DoValidate())
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
				if (PriceHelper.GetPriceListRowVATType(_priceListType.Code, out priceListRowVATType, out priceListRowCurr))
				{
					_vatType.Code = priceListRowVATType;
				}
			}
		}
		if (flag)
		{
			UpdateMTInvoicePrices(_date.EditValue.Value, _priceListType.Code, _currExChngRate.Rate, _currExChngRate.Base);
			PriceHelper.GetPriceListRowVATType(_priceListType.Code, out priceListRowVATType, out priceListRowCurr);
			if (priceListRowVATType != _vatType.Code)
			{
				VATTypeValidate(priceListRowVATType, _vatType.Code);
			}
			if (!string.IsNullOrWhiteSpace(DiscountPriceListType))
			{
				for (int i = 0; i < _invoicesList.Count; i++)
				{
					InvoiceForPaySpecificationRow invoiceForPayGridRow = _invoicesList[i];
					CalculateGridRowDiscountPrice(ref invoiceForPayGridRow);
					CalculateGridRowSummaAndDiscountSumma(ref invoiceForPayGridRow, invoiceForPayGridRow.DiscountPrice == 0m);
				}
			}
			else
			{
				CalculateDiscount(_invoicesList, Convert.ToDecimal(_discountPercent.Value), gDiscount: false, calcInvFee: true, _vatType.Code);
			}
			CalcSummAndSummVAT();
			_invoiceGrid.Grid.RefreshData();
		}
		RowInfoChangeHandler?.Invoke((InvoiceForPaySpecificationRow)_invoiceTableView.Grid.CurrentItem);
	}

	private void UpdateMTInvoicePrices(DateOnly DocDate, string PriceListType, decimal CurrRate, decimal CurrBase)
	{
		ExchangeRate exchangeRate = new ExchangeRate();
		if (!_invoicesList.Any())
		{
			return;
		}
		PriceHelper.GetPriceListRowVATType(PriceListType, out var _, out var priceListRowCurr);
		if (priceListRowCurr != "AMD")
		{
			exchangeRate = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(priceListRowCurr, DocDate);
		}
		foreach (InvoiceForPaySpecificationRow invoices in _invoicesList)
		{
			PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(PriceListType, invoices.ItemID, invoices.UnitCode, DocDate);
			if (priceListRowCurr == _currCode.Code)
			{
				invoices.Price = price.Price;
				invoices.Price = Methods.RoundPrice(invoices.Price, _priceListTypeRoundDgr);
			}
			else if (priceListRowCurr == "AMD")
			{
				invoices.Price = (price.Price / CurrRate * CurrBase).RoundAs(2);
			}
			else
			{
				invoices.Price = (price.Price * exchangeRate.Rate / exchangeRate.Base / CurrRate * CurrBase).RoundAs(2);
			}
			invoices.DiscountPrice = (invoices.Price - invoices.Price * invoices.Discount / 100m).RoundAs(2);
			invoices.Summa = (invoices.DiscountPrice * invoices.Quantity).RoundAs(2);
		}
	}

	private void VATTypeValidate(string VATTypeOldValue, string VATTypeNewValue)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		_validateTotalSumm = false;
		if (_invoicesList.Count > 0)
		{
			switch (VATTypeNewValue)
			{
			case "1":
				if (VATTypeOldValue == "3" || VATTypeOldValue == "4")
				{
					foreach (InvoiceForPaySpecificationRow invoices in _invoicesList)
					{
						num += PriceHelper.CalcVATAmountByType(invoices.Summa, "1", invoices.VAT.Value);
						num2 += invoices.Summa;
					}
				}
				else
				{
					foreach (InvoiceForPaySpecificationRow invoices2 in _invoicesList)
					{
						invoices2.Price -= PriceHelper.CalcVATAmountByType(invoices2.Price, VATTypeOldValue, invoices2.VAT.Value);
						invoices2.Price = Methods.RoundPrice(invoices2.Price, _priceListTypeRoundDgr);
						invoices2.DiscountPrice = (invoices2.Price * (1m - invoices2.Discount / 100m)).RoundAs(2);
						invoices2.Summa = (invoices2.DiscountPrice * invoices2.Quantity).RoundAs(2);
						num += PriceHelper.CalcVATAmountByType(invoices2.Summa, "1", invoices2.VAT.Value);
						num2 += invoices2.Summa;
					}
				}
				_vatSumma.EditValue = num;
				_totalSumma.EditValue = num2 + num;
				break;
			case "2":
				switch (VATTypeOldValue)
				{
				case "3":
				case "4":
					foreach (InvoiceForPaySpecificationRow invoices3 in _invoicesList)
					{
						num += PriceHelper.CalcVATAmountByType(invoices3.Summa, "1", invoices3.VAT.Value);
						invoices3.Price += PriceHelper.CalcVATAmountByType(invoices3.Price, "1", invoices3.VAT.Value);
						invoices3.Price = Methods.RoundPrice(invoices3.Price, _priceListTypeRoundDgr);
						invoices3.DiscountPrice = (invoices3.Price * (1m - invoices3.Discount / 100m)).RoundAs(2);
						invoices3.Summa = (invoices3.DiscountPrice * invoices3.Quantity).RoundAs(2);
						num2 += invoices3.Summa;
					}
					_vatSumma.EditValue = num;
					_totalSumma.EditValue = num2;
					break;
				case "1":
					foreach (InvoiceForPaySpecificationRow invoices4 in _invoicesList)
					{
						num += PriceHelper.CalcVATAmountByType(invoices4.Summa, "1", invoices4.VAT.Value);
						invoices4.Price += PriceHelper.CalcVATAmountByType(invoices4.Price, "1", invoices4.VAT.Value);
						invoices4.Price = Methods.RoundPrice(invoices4.Price, _priceListTypeRoundDgr);
						invoices4.DiscountPrice = (invoices4.Price * (1m - invoices4.Discount / 100m)).RoundAs(2);
						invoices4.Summa = (invoices4.DiscountPrice * invoices4.Quantity).RoundAs(2);
						num2 += invoices4.Summa;
					}
					_vatSumma.EditValue = num;
					_totalSumma.EditValue = num2;
					break;
				}
				break;
			case "3":
			case "4":
				if (VATTypeOldValue == "2")
				{
					foreach (InvoiceForPaySpecificationRow invoices5 in _invoicesList)
					{
						invoices5.Price -= PriceHelper.CalcVATAmountByType(invoices5.Price, VATTypeOldValue, invoices5.VAT.Value);
						invoices5.Price = Methods.RoundPrice(invoices5.Price, _priceListTypeRoundDgr);
						invoices5.DiscountPrice = (invoices5.Price * (1m - invoices5.Discount / 100m)).RoundAs(2);
						invoices5.Summa = (invoices5.DiscountPrice * invoices5.Quantity).RoundAs(2);
						num2 += invoices5.Summa;
					}
					_vatSumma.EditValue = 0m;
					_totalSumma.EditValue = num2;
				}
				else
				{
					if (!(VATTypeOldValue == "1"))
					{
						break;
					}
					foreach (InvoiceForPaySpecificationRow invoices6 in _invoicesList)
					{
						num2 += invoices6.Summa;
					}
					_vatSumma.EditValue = 0m;
					_totalSumma.EditValue = num2;
				}
				break;
			}
		}
		_validateTotalSumm = true;
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
				InvoiceForPaySpecificationRow invoiceForPayGridRow = _invoicesList[i];
				CalculateGridRowDiscountPriceInner(ref invoiceForPayGridRow);
				CalculateGridRowSummaAndDiscountSumma(ref invoiceForPayGridRow, calcDiscountPrice: false);
			}
		}
	}

	private void CalculateGridRowDiscountPriceInner(ref InvoiceForPaySpecificationRow invoiceForPayGridRow)
	{
		if (_isUsingDiscount || GetDiscountUse())
		{
			PricesDataProvider.DataRow dataRow = null;
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(invoiceForPayGridRow.ItemCode);
			if (!string.IsNullOrWhiteSpace(DiscountPriceListType))
			{
				dataRow = TradeSettings.CreateTradeClient().CommonServices.GetPrice(DiscountPriceListType, invoiceForPayGridRow.ItemID, invoiceForPayGridRow.UnitCode, _date.EditValue.Value);
			}
			if (dataRow != null && dataRow.Price != 0m)
			{
				if (_discountPriceListCurr == _currCode.Code)
				{
					invoiceForPayGridRow.DiscountPrice = dataRow.Price;
				}
				else if (_discountPriceListCurr == "AMD")
				{
					invoiceForPayGridRow.DiscountPrice = (dataRow.Price / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
				}
				else
				{
					invoiceForPayGridRow.DiscountPrice = (dataRow.Price * _discountPriceListCurrRate / _discountPriceListCurrBase / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
				}
				if (_discountPriceListVATType != _vatType.Code && invoiceForPayGridRow.VAT.Value)
				{
					string discountPriceListVATType = _discountPriceListVATType;
					if (!(discountPriceListVATType == "1"))
					{
						if (discountPriceListVATType == "2" && _vatType.Code.InList("1", "3", "4"))
						{
							invoiceForPayGridRow.DiscountPrice -= PriceHelper.CalcVATAmountByType(invoiceForPayGridRow.DiscountPrice, "2", invoiceForPayGridRow.VAT.Value);
						}
					}
					else if (_vatType.Code == "2")
					{
						invoiceForPayGridRow.DiscountPrice += PriceHelper.CalcVATAmountByType(invoiceForPayGridRow.DiscountPrice, "1", invoiceForPayGridRow.VAT.Value);
					}
				}
				invoiceForPayGridRow.DiscountPrice = Methods.RoundPrice(invoiceForPayGridRow.DiscountPrice, _discountPriceTypeRoundDegree);
				if (!string.IsNullOrWhiteSpace(_priceListType.Code))
				{
					if (invoiceForPayGridRow.Price == 0m)
					{
						invoiceForPayGridRow.Price = invoiceForPayGridRow.DiscountPrice;
					}
					else
					{
						invoiceForPayGridRow.Discount = (100m * (invoiceForPayGridRow.Price - invoiceForPayGridRow.DiscountPrice) / invoiceForPayGridRow.Price).RoundAs(2);
					}
				}
			}
			else if (_discountPercent.Value != 0m)
			{
				invoiceForPayGridRow.Discount = _discountPercent.Value;
				invoiceForPayGridRow.DiscountPrice = Methods.RoundPrice(invoiceForPayGridRow.Price * (100m - invoiceForPayGridRow.Discount) / 100m, _priceListTypeRoundDgr);
			}
			else if (item.Discount != 0m)
			{
				invoiceForPayGridRow.Discount = item.Discount;
				invoiceForPayGridRow.DiscountPrice = Methods.RoundPrice(invoiceForPayGridRow.Price * (100m - invoiceForPayGridRow.Discount) / 100m, _priceListTypeRoundDgr);
			}
			else
			{
				invoiceForPayGridRow.DiscountPrice = invoiceForPayGridRow.Price;
				invoiceForPayGridRow.Discount = 0m;
			}
		}
		else
		{
			invoiceForPayGridRow.DiscountPrice = invoiceForPayGridRow.Price;
			invoiceForPayGridRow.Discount = 0m;
		}
	}

	private void CalculateGridRowSummaAndDiscountSumma(ref InvoiceForPaySpecificationRow invoiceForPayGridRow, bool calcDiscountPrice = true)
	{
		if (_isUsingDiscount || GetDiscountUse())
		{
			if (calcDiscountPrice)
			{
				invoiceForPayGridRow.DiscountPrice = Methods.RoundPrice(invoiceForPayGridRow.Price * (100m - invoiceForPayGridRow.Discount) / 100m, _priceListTypeRoundDgr);
			}
			invoiceForPayGridRow.Summa = PriceHelper.CalculateSum(invoiceForPayGridRow.DiscountPrice, invoiceForPayGridRow.Quantity);
			invoiceForPayGridRow.DiscountSumma = PriceHelper.CalculateSum(invoiceForPayGridRow.Price, invoiceForPayGridRow.Quantity) - invoiceForPayGridRow.Summa;
		}
		else
		{
			invoiceForPayGridRow.DiscountPrice = invoiceForPayGridRow.Price;
			invoiceForPayGridRow.Summa = PriceHelper.CalculateSum(invoiceForPayGridRow.Price, invoiceForPayGridRow.Quantity);
			invoiceForPayGridRow.DiscountSumma = 0m;
		}
	}

	private void CalculateGridRowDiscountPrice(ref InvoiceForPaySpecificationRow invoiceForPayGridRow)
	{
		if (string.IsNullOrWhiteSpace(DiscountPriceListType))
		{
			invoiceForPayGridRow.DiscountPrice = 0m;
		}
		else
		{
			CalculateGridRowDiscountPriceInner(ref invoiceForPayGridRow);
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
			FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
			{
				FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true),
				SearchDataView = new GoodsWithPrices(_date.EditValue.Value, _priceListType.Code, string.Empty),
				ResultsList = new MTInvoiceBaseFilteredSearchList<InvoiceForPaySpecificationRow>(_currCode.Code, _vatType.Code, _date.EditValue.Value)
			}, this, isBasketMode: true);
			if (filteredSearchWindow.ShowDialog().Value)
			{
				foreach (InvoiceForPaySpecificationRow item in filteredSearchWindow.BasketResult)
				{
					InvoiceForPaySpecificationRow invoiceForPayGridRow = item;
					invoiceForPayGridRow.IsStorageRequired = false;
					if (invoiceForPayGridRow.ItemType == ItemTypesSE.Goods.Value)
					{
						invoiceForPayGridRow.Storage = _storage.Code;
					}
					if (!_isUsingDiscount)
					{
						invoiceForPayGridRow.Discount = 0m;
					}
					else if (invoiceForPayGridRow.Discount == 0m && _discountPercent.Value != 0m)
					{
						invoiceForPayGridRow.Discount = _discountPercent.Value;
					}
					else if (!string.IsNullOrWhiteSpace(DiscountPriceListType))
					{
						CalculateGridRowDiscountPrice(ref invoiceForPayGridRow);
						CalculateGridRowSummaAndDiscountSumma(ref invoiceForPayGridRow, invoiceForPayGridRow.DiscountPrice == 0m);
					}
					invoiceForPayGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, invoiceForPayGridRow.ItemID, invoiceForPayGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
					invoiceForPayGridRow.Price = Methods.RoundPrice(invoiceForPayGridRow.Price, _priceListTypeRoundDgr);
					invoiceForPayGridRow.DiscountPrice = (invoiceForPayGridRow.Price - invoiceForPayGridRow.Price * invoiceForPayGridRow.Discount / 100m).RoundAs(2);
					invoiceForPayGridRow.Summa = (invoiceForPayGridRow.DiscountPrice * invoiceForPayGridRow.Quantity).RoundAs(2);
					invoiceForPayGridRow.Discount = 0m;
					invoiceForPayGridRow.RowID = _maxRowId;
					_invoicesList.Add(invoiceForPayGridRow);
					_maxRowId++;
				}
				_invoiceGrid.Grid.RefreshData();
				CalcSummAndSummVAT();
			}
			RowInfoChangeHandler?.Invoke((InvoiceForPaySpecificationRow)_invoiceTableView.Grid.CurrentItem);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barButtonJumpTo = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
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
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
	}

	protected override void CreateChildDocument(short childDocType, InvoiceForPay baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		CreateBasedOnDocHelper.CreateDocument((ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)(SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_invoiceGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double DefaultWidth, double DefaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(DefaultWidth, DefaultHeight, rk);
		_invoiceGrid.RestoreSettings(rk);
	}

	protected override bool AllowPrint()
	{
		return true;
	}

	protected override bool AllowPrintOnNew()
	{
		return true;
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
				GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value);
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
			RowInfoChangeHandler?.Invoke(_invoicesList[_invoicesList.Count - 1]);
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

	private void FillPartnerEmployeeData(string cardNumber, PartnerCacheRow partnerCacheRow, PartnerCacheRow.ContractRow contractRow)
	{
		if (partnerCacheRow == null)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.PartnerCodeIsNotExist, cardNumber));
		}
		_buyer.Code = partnerCacheRow.PartCode;
		_contract.Code = contractRow.Contract;
		_currCode.Code = contractRow.CurrCode;
	}

	private void DiscountPercent_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue == null)
		{
			return;
		}
		if (Convert.ToDecimal(e.NewValue.ToString()) != Convert.ToDecimal(e.OldValue.ToString()) && _invoicesList.Count > 0)
		{
			if (Convert.ToDecimal(e.OldValue) != 0m || GetDiscountUse())
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ComfirmDiscountChange, default(ReadOnlySpan<object>)), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					CalculateDiscount(_invoicesList, Convert.ToDecimal(e.NewValue));
					CalcSummAndSummVAT();
					_invoiceGrid.Grid.RefreshData();
				}
				else
				{
					_discountPercent.EditValueChanged -= DiscountPercent_EditValueChanged;
					_discountPercent.EditValue = Convert.ToDecimal(e.OldValue);
					_discountPercent.DoValidate();
					_discountPercent.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DiscountPercent_EditValueChanged, this).Handler;
				}
			}
			else
			{
				CalculateDiscount(_invoicesList, Convert.ToDecimal(e.NewValue));
				CalcSummAndSummVAT();
				_invoiceGrid.Grid.RefreshData();
			}
		}
		else
		{
			CalculateDiscount(_invoicesList, Convert.ToDecimal(e.NewValue));
			CalcSummAndSummVAT();
			_invoiceGrid.Grid.RefreshData();
		}
	}

	private bool GetDiscountUse()
	{
		decimal num = 0m;
		foreach (InvoiceForPaySpecificationRow invoices in _invoicesList)
		{
			num += Math.Abs(invoices.Price - invoices.DiscountPrice);
		}
		return !Convert.ToBoolean(num == 0m);
	}
}
