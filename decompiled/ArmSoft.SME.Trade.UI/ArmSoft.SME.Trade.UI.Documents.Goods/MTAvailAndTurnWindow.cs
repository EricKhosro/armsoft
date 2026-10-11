using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents.Goods;

public class MTAvailAndTurnWindow : DialogWindow
{
	private string _storage = "";

	private string _supplierCode = "";

	private int _supplierID = -1;

	private string _buyerCode = "";

	private int _buyerID = -1;

	private string _mtCode = "";

	private int _mtID = -1;

	private string _qntUnit = "";

	private decimal _qntUnitCoef = 1m;

	private DropDownControl _storageControl;

	private DropDownControl _supplier;

	private DropDownControl _buyer;

	private DropDownControl _mtGroup;

	private DropDownControl _mtCodeControl;

	private DropDownControl _qntUnitControl;

	private Button _refreshAvail;

	private Button _refreshPurchases;

	private Button _refreshSales;

	private DateEditControl _purchasesStartDate;

	private DateEditControl _purchasesEndDate;

	private DateEditControl _salesStartDate;

	private DateEditControl _salesEndDate;

	private DocumentGridControl _partiesGrid;

	private TableView _partiesTableView;

	private DocumentGridControl _purchasesGrid;

	private TableView _purchasesTableView;

	private DocumentGridControl _salesGrid;

	private TableView _salesTableView;

	private List<CurrentAvailGoodsByPartiesDataProvider.DataRow> _mtCurrentAvailByParties;

	private List<PurchasesByPartiesDataProvider.DataRow> _mtPurchasesByParties;

	private List<SalesByPartiesDataProvider.DataRow> _mtSalesByParties;

	public MTAvailAndTurnWindow(DialogWindowMode mode, Window owner, string storage, string supplierCode, int mtid, string mtCode, string qntUnit, string buyerCode = "")
		: base(owner)
	{
		Mode = mode;
		Name = "AvailAndTurn";
		Caption = ArmSoft.SME.Trade.Common.Resources.AvailabilityAndTurn;
		SizeToContent = SizeToContent.Manual;
		ResizeMode = ResizeMode.CanResizeWithGrip;
		Height = 500.0;
		Width = 1000.0;
		MinHeight = Height;
		MinWidth = Width;
		_registryKey = "MTAvailAndTurnWindow";
		RestoreSettings();
		_storage = storage;
		_mtCode = mtCode;
		_mtID = mtid;
		_qntUnit = qntUnit;
		_supplierCode = supplierCode;
		_buyerCode = buyerCode;
		if (!string.IsNullOrWhiteSpace(supplierCode))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(supplierCode);
			if (item != null)
			{
				_supplierID = item.Id;
			}
		}
		if (!string.IsNullOrWhiteSpace(buyerCode))
		{
			PartnerCacheRow item2 = TradeSettings.TradeCacheService.Partner.GetItem(buyerCode);
			if (item2 != null)
			{
				_buyerID = item2.Id;
			}
		}
		if (!string.IsNullOrWhiteSpace(qntUnit))
		{
			MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtid, qntUnit);
			if (qntUnit2 != null && qntUnit2.Coef != 0m)
			{
				_qntUnitCoef = qntUnit2.Coef;
			}
		}
		_mtCurrentAvailByParties = new List<CurrentAvailGoodsByPartiesDataProvider.DataRow>();
		_mtPurchasesByParties = new List<PurchasesByPartiesDataProvider.DataRow>();
		_mtSalesByParties = new List<SalesByPartiesDataProvider.DataRow>();
	}

	protected override void RestoreSettings(RegistryKey rk)
	{
		base.RestoreSettings(rk);
		if (rk != null)
		{
			Settings.RestoreWindowSize(this, rk, MinWidth, MinHeight);
			WindowState result = WindowState.Normal;
			if (Enum.TryParse<WindowState>(rk.GetValue("WindowState", WindowState.Normal).ToString(), out result))
			{
				WindowState = ((result != WindowState.Minimized) ? result : WindowState.Normal);
			}
		}
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		if (rk != null && WindowState != WindowState.Minimized)
		{
			rk.SetValue("Width", Width.ToString());
			rk.SetValue("Height", Height.ToString());
			rk.SetValue("WindowState", WindowState);
		}
	}

	public bool? Invoke()
	{
		if (!TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableStorage())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.AccessToStoragesDenied, Settings.LoginService.UserName), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return false;
		}
		string storage = ArmSoft.SME.Common.Resources.Storage;
		bool? checkViewOrOperationAccess = true;
		_storageControl = AddDropDownControl("Storage", storage, new Storages(null, checkViewOrOperationAccess));
		_supplier = AddDropDownControl("Supplier", ArmSoft.SME.Common.Resources.Supplier, new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, "", isFilterBySuppliers: true));
		_buyer = AddDropDownControl("Buyer", ArmSoft.SME.Common.Resources.Buyer, new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, "", isFilterBySuppliers: false, isFilterByCustomers: true));
		_mtGroup = AddDropDownControl("MTGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups());
		_mtGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTGroup_EditValueChanged).Handler;
		_mtCodeControl = AddDropDownControl("MTCode", ArmSoft.SME.Common.Resources.Code, new GoodsFilter(shortMode: true, ItemTypesSE.Goods), isRequired: true, allowAdditionButtons: true, storeValue: false, isShowComment: true, 8);
		_mtCodeControl.EditAdditionalValueChanged += new SafeEventHandler<EventArgs>(MTCode_EditAdditionalValueChanged).Handler;
		_mtCodeControl.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(MTCode_ClickDropDown).Handler;
		_qntUnitControl = AddDropDownControl("QntUnit", ArmSoft.SME.Trade.Common.Resources.QntUnit, new MTQntUnits(_mtCode, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", ItemTypesSE.Goods), isRequired: true);
		_qntUnitControl.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(QntUnit_ClickDropDown).Handler;
		_qntUnitControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(QntUnit_EditValueChanged).Handler;
		DXTabControl dXTabControl = AddTabControl(null);
		LayoutGroup layoutGroup = AddTabedGroup(dXTabControl, ArmSoft.SME.Common.Resources.Availability);
		layoutGroup.Name = "AvialTab";
		if (!Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailability).AllowView && !Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailabilityTreeView).AllowView)
		{
			((DXTabItem)dXTabControl.Items[0]).Visibility = Visibility.Collapsed;
		}
		_refreshAvail = AddButton(layoutGroup, "RefreshAvail", "refresh_32.svg", ArmSoft.SME.Common.Resources.Refresh);
		((LayoutItemWithRequired)(object)_refreshAvail.Parent).Label = "";
		_refreshAvail.IsTabStop = true;
		_refreshAvail.Click += new SafeEventHandler<RoutedEventArgs>(RefreshAvail_Click).Handler;
		_partiesGrid = AddGridControl(layoutGroup, "AvailPartiesGrid");
		_partiesGrid.Owner = this;
		_partiesGrid.AllowAdd = false;
		_partiesGrid.AllowDelete = false;
		_partiesGrid.AllowDuplicate = false;
		_partiesGrid.ShowButtons = false;
		_partiesGrid.AddStringColumn("Storage", ArmSoft.SME.Common.Resources.Storage, 5).AllowEditing = DefaultBoolean.False;
		_partiesGrid.AddNumberColumn("Qty", ArmSoft.SME.Common.Resources.Availability, 13, 3, allowNegativeNumbers: false, showTotal: true).AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn = _partiesGrid.AddNumberColumn("Cost", ArmSoft.SME.Common.Resources.MtCurCost, 16, 2);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Visible = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		DocumentGridColumn documentGridColumn2 = _partiesGrid.AddStringColumn("PartnerCode", ArmSoft.SME.Common.Resources.Supplier, 20);
		documentGridColumn2.Width = 100.0;
		documentGridColumn2.ReadOnly = true;
		documentGridColumn2.Visible = Settings.ParametersService.ShowPartySUPPLIER();
		DocumentGridColumn documentGridColumn3 = _partiesGrid.AddStringColumn("PartnerCaption", ArmSoft.SME.Common.Resources.Name, 50);
		documentGridColumn3.Width = 100.0;
		documentGridColumn3.ReadOnly = true;
		documentGridColumn3.Visible = Settings.ParametersService.ShowPartySUPPLIER();
		DocumentGridColumn documentGridColumn4 = _partiesGrid.AddStringColumn("Contract", ArmSoft.SME.Common.Resources.Contract, 50);
		documentGridColumn4.Width = 100.0;
		documentGridColumn4.ReadOnly = true;
		documentGridColumn4.Visible = Settings.ParametersService.ShowPartyCONTRACT();
		DocumentGridColumn documentGridColumn5 = _partiesGrid.AddDateColumn("IncomeDate", ArmSoft.SME.Common.Resources.IncomeDateShort);
		documentGridColumn5.AllowEditing = DefaultBoolean.False;
		documentGridColumn5.Visible = Settings.ParametersService.ShowPartyIncomeDate();
		DocumentGridColumn documentGridColumn6 = _partiesGrid.AddStringColumn("Comment", ArmSoft.SME.Common.Resources.IncomeDoc, 50);
		documentGridColumn6.Width = 100.0;
		documentGridColumn6.ReadOnly = true;
		DocumentGridColumn documentGridColumn7 = _partiesGrid.AddDateColumn("UseableDate", ArmSoft.SME.Trade.Common.Resources.UseLife);
		documentGridColumn7.ReadOnly = true;
		documentGridColumn7.Visible = Settings.ParametersService.ShowPartyUSEABLEDATE();
		DocumentGridColumn documentGridColumn8 = _partiesGrid.AddStringColumn("Country", ArmSoft.SME.Common.Resources.Country, 50);
		documentGridColumn8.Width = 100.0;
		documentGridColumn8.ReadOnly = true;
		documentGridColumn8.Visible = Settings.ParametersService.ShowPartyCountry();
		DocumentGridColumn documentGridColumn9 = _partiesGrid.AddStringColumn("Producer", ArmSoft.SME.Common.Resources.Producer, 50);
		documentGridColumn9.Width = 100.0;
		documentGridColumn9.ReadOnly = true;
		documentGridColumn9.Visible = Settings.ParametersService.ShowPartyProducer();
		DocumentGridColumn documentGridColumn10 = _partiesGrid.AddStringColumn("Property1", ArmSoft.SME.Trade.Common.Resources.Attribute1, 50);
		documentGridColumn10.Width = 100.0;
		documentGridColumn10.ReadOnly = true;
		documentGridColumn10.Visible = Settings.ParametersService.ShowPartyProperty1();
		DocumentGridColumn documentGridColumn11 = _partiesGrid.AddStringColumn("Property2", ArmSoft.SME.Trade.Common.Resources.Attribute2, 50);
		documentGridColumn11.Width = 100.0;
		documentGridColumn11.ReadOnly = true;
		documentGridColumn11.Visible = Settings.ParametersService.ShowPartyProperty2();
		foreach (GridColumn column in _partiesGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_partiesTableView = _partiesGrid.View;
		_partiesGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
		_partiesGrid.VerticalAlignment = VerticalAlignment.Stretch;
		_partiesGrid.IsReadOnly = true;
		MinWidth = SizeCalculator.Width(620.0);
		MinHeight = SizeCalculator.Height(250.0);
		LayoutGroup layoutGroup2 = AddTabedGroup(dXTabControl, ArmSoft.SME.Trade.Common.Resources.Purchases);
		layoutGroup2.Name = "PurchasesTab";
		if (!Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailability).AllowView)
		{
			((DXTabItem)dXTabControl.Items[1]).Visibility = Visibility.Collapsed;
		}
		LayoutGroupExt container = AddHorizontalLayoutGroup(layoutGroup2);
		_refreshPurchases = AddButton(container, "RefreshPurchaes", "refresh_32.svg", ArmSoft.SME.Common.Resources.Refresh);
		((LayoutItemWithRequired)(object)_refreshPurchases.Parent).Label = "";
		_refreshPurchases.IsTabStop = true;
		_refreshPurchases.Click += new SafeEventHandler<RoutedEventArgs>(RefreshPurchases_Click).Handler;
		_purchasesStartDate = AddDateControl(container, "PurchasesStartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, storeValue: false, ArmSoft.SME.Common.Resources.DatePeriodBegin);
		_purchasesEndDate = AddDateControl(container, "PurchasesEndDate", "", useLongDate: false, isRequired: true, storeValue: false, ArmSoft.SME.Common.Resources.DatePeriodEnd);
		AddSpecialDateButton(container, _purchasesStartDate, _purchasesEndDate);
		_purchasesGrid = AddGridControl(layoutGroup2, "PurchasesGrid");
		_purchasesGrid.Owner = this;
		_purchasesGrid.AllowAdd = false;
		_purchasesGrid.AllowDelete = false;
		_purchasesGrid.AllowDuplicate = false;
		_purchasesGrid.ShowButtons = false;
		_purchasesGrid.AddDateColumn("Date", ArmSoft.SME.Common.Resources.DateWord).AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn12 = _purchasesGrid.AddStringColumn("DocNum", ArmSoft.SME.Common.Resources.DocNumber, 12);
		documentGridColumn12.Width = 100.0;
		documentGridColumn12.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn13 = _purchasesGrid.AddStringColumn("DocTypeCaption", ArmSoft.SME.Trade.Common.Resources.MatRepOperationType, 50);
		documentGridColumn13.Width = 100.0;
		documentGridColumn13.AllowEditing = DefaultBoolean.False;
		_purchasesGrid.AddNumberColumn("Quantity", ArmSoft.SME.Common.Resources.Quantity, 13, 3, allowNegativeNumbers: false, showTotal: true).AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn14 = _purchasesGrid.AddNumberColumn("Cost", ArmSoft.SME.Common.Resources.Price + " " + ArmSoft.SME.Trade.Common.Resources.WithNDS, 16, 2);
		documentGridColumn14.AllowEditing = DefaultBoolean.False;
		documentGridColumn14.Visible = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		DocumentGridColumn documentGridColumn15 = _purchasesGrid.AddNumberColumn("CostSummWithVAT", ArmSoft.SME.Common.Resources.Summa + " " + ArmSoft.SME.Trade.Common.Resources.WithNDS, 16, 2, allowNegativeNumbers: false, showTotal: true);
		documentGridColumn15.AllowEditing = DefaultBoolean.False;
		documentGridColumn15.Visible = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		_purchasesGrid.AddStringColumn("Storage", ArmSoft.SME.Common.Resources.Storage, 5).AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn16 = _purchasesGrid.AddStringColumn("CorrObjectCode", ArmSoft.SME.Common.Resources.Supplier, 20);
		documentGridColumn16.Width = 100.0;
		documentGridColumn16.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn17 = _purchasesGrid.AddStringColumn("CorrObjectCaption", ArmSoft.SME.Common.Resources.Name, 50);
		documentGridColumn17.Width = 100.0;
		documentGridColumn17.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn18 = _purchasesGrid.AddStringColumn("PartyContractCaption", ArmSoft.SME.Common.Resources.Contract, 50);
		documentGridColumn18.Width = 100.0;
		documentGridColumn18.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn19 = _purchasesGrid.AddStringColumn("Comment", ArmSoft.SME.Common.Resources.Comment, 50);
		documentGridColumn19.Width = 100.0;
		documentGridColumn19.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn20 = _purchasesGrid.AddDateColumn("PartyDocUsableDate", ArmSoft.SME.Trade.Common.Resources.UseLife);
		documentGridColumn20.AllowEditing = DefaultBoolean.False;
		documentGridColumn20.Visible = Settings.ParametersService.ShowPartyUSEABLEDATE();
		DocumentGridColumn documentGridColumn21 = _purchasesGrid.AddStringColumn("PartyDocCountry", ArmSoft.SME.Common.Resources.Country, 50);
		documentGridColumn21.Width = 100.0;
		documentGridColumn21.AllowEditing = DefaultBoolean.False;
		documentGridColumn21.Visible = Settings.ParametersService.ShowPartyCountry();
		DocumentGridColumn documentGridColumn22 = _purchasesGrid.AddStringColumn("PartyDocProducer", ArmSoft.SME.Common.Resources.Producer, 50);
		documentGridColumn22.Width = 100.0;
		documentGridColumn22.Visible = Settings.ParametersService.ShowPartyProducer();
		DocumentGridColumn documentGridColumn23 = _purchasesGrid.AddStringColumn("PartyDocPropertyType1", ArmSoft.SME.Trade.Common.Resources.AttributeType1, 4);
		documentGridColumn23.Width = 100.0;
		documentGridColumn23.AllowEditing = DefaultBoolean.False;
		documentGridColumn23.Visible = Settings.ParametersService.ShowPartyProperty1();
		DocumentGridColumn documentGridColumn24 = _purchasesGrid.AddStringColumn("PartyDocPropertyValue1", ArmSoft.SME.Trade.Common.Resources.AttributeValue + " 1", 50);
		documentGridColumn24.Width = 100.0;
		documentGridColumn24.AllowEditing = DefaultBoolean.False;
		documentGridColumn24.Visible = Settings.ParametersService.ShowPartyProperty1();
		DocumentGridColumn documentGridColumn25 = _purchasesGrid.AddStringColumn("PartyDocPropertyType2", ArmSoft.SME.Trade.Common.Resources.AttributeType2, 4);
		documentGridColumn25.Width = 100.0;
		documentGridColumn25.AllowEditing = DefaultBoolean.False;
		documentGridColumn25.Visible = Settings.ParametersService.ShowPartyProperty2();
		DocumentGridColumn documentGridColumn26 = _purchasesGrid.AddStringColumn("PartyDocPropertyValue2", ArmSoft.SME.Trade.Common.Resources.AttributeValue + " 2", 50);
		documentGridColumn26.Width = 100.0;
		documentGridColumn26.AllowEditing = DefaultBoolean.False;
		documentGridColumn26.Visible = Settings.ParametersService.ShowPartyProperty2();
		foreach (GridColumn column2 in _purchasesGrid.Grid.Columns)
		{
			column2.AllowSorting = DefaultBoolean.True;
			column2.AllowColumnFiltering = DefaultBoolean.True;
			column2.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_purchasesTableView = _purchasesGrid.View;
		_purchasesGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
		_purchasesGrid.VerticalAlignment = VerticalAlignment.Stretch;
		LayoutGroup layoutGroup3 = AddTabedGroup(dXTabControl, ArmSoft.SME.Common.Resources.Sales);
		layoutGroup3.Name = "SalesTab";
		if (!Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailability).AllowView)
		{
			((DXTabItem)dXTabControl.Items[2]).Visibility = Visibility.Collapsed;
		}
		LayoutGroupExt container2 = AddHorizontalLayoutGroup(layoutGroup3);
		_refreshSales = AddButton(container2, "RefreshSales", "refresh_32.svg", ArmSoft.SME.Common.Resources.Refresh);
		((LayoutItemWithRequired)(object)_refreshSales.Parent).Label = "";
		_refreshSales.IsTabStop = true;
		_refreshSales.Click += new SafeEventHandler<RoutedEventArgs>(RefreshSales_Click).Handler;
		_salesStartDate = AddDateControl(container2, "SalesStartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, storeValue: false, ArmSoft.SME.Common.Resources.DatePeriodBegin);
		_salesEndDate = AddDateControl(container2, "SalesEndDate", "", useLongDate: false, isRequired: true, storeValue: false, ArmSoft.SME.Common.Resources.DatePeriodEnd);
		AddSpecialDateButton(container2, _salesStartDate, _salesEndDate);
		_salesGrid = AddGridControl(layoutGroup3, "SalesGrid");
		_salesGrid.Owner = this;
		_salesGrid.AllowAdd = false;
		_salesGrid.AllowDelete = false;
		_salesGrid.AllowDuplicate = false;
		_salesGrid.ShowButtons = false;
		_salesGrid.AddDateColumn("DocDate", ArmSoft.SME.Common.Resources.DateWord).AllowEditing = DefaultBoolean.False;
		_salesGrid.AddStringColumn("DocNum", ArmSoft.SME.Common.Resources.DocNumber, 12).AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn27 = _salesGrid.AddStringColumn("OpName", ArmSoft.SME.Trade.Common.Resources.MatRepOperationType, 1);
		documentGridColumn27.Width = 100.0;
		documentGridColumn27.AllowEditing = DefaultBoolean.False;
		_salesGrid.AddNumberColumn("Quantity", ArmSoft.SME.Common.Resources.Quantity, 13, 3, allowNegativeNumbers: false, showTotal: true).AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn28 = _salesGrid.AddNumberColumn("SalePriceWithVAT", ArmSoft.SME.Common.Resources.MTTAccSalePrice + " " + ArmSoft.SME.Trade.Common.Resources.WithNDS, 16, 2);
		documentGridColumn28.AllowEditing = DefaultBoolean.False;
		documentGridColumn28.Visible = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		DocumentGridColumn documentGridColumn29 = _salesGrid.AddNumberColumn("SaleSummWithVAT", ArmSoft.SME.Trade.Common.Resources.SaleSum + " " + ArmSoft.SME.Trade.Common.Resources.WithNDS, 16, 2, allowNegativeNumbers: false, showTotal: true);
		documentGridColumn29.AllowEditing = DefaultBoolean.False;
		documentGridColumn29.Visible = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		_salesGrid.AddStringColumn("Storage", ArmSoft.SME.Common.Resources.Storage, 5).AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn30 = _salesGrid.AddStringColumn("PartnerCode", ArmSoft.SME.Common.Resources.Buyer, 20);
		documentGridColumn30.Width = 100.0;
		documentGridColumn30.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn31 = _salesGrid.AddStringColumn("PartnerCaption", ArmSoft.SME.Common.Resources.Name, 50);
		documentGridColumn31.Width = 100.0;
		documentGridColumn31.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn32 = _salesGrid.AddStringColumn("Comment", ArmSoft.SME.Common.Resources.IncomeDoc, 50);
		documentGridColumn32.Width = 100.0;
		documentGridColumn32.AllowEditing = DefaultBoolean.False;
		_salesGrid.AddDateColumn("IncomeDate", ArmSoft.SME.Common.Resources.IncomeDateShort).AllowEditing = DefaultBoolean.False;
		_salesGrid.AddStringColumn("PartyDocNum", ArmSoft.SME.Common.Resources.DocNumber, 12).AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn33 = _salesGrid.AddStringColumn("PartyDocType", ArmSoft.SME.Trade.Common.Resources.MatRepOperationType, 1);
		documentGridColumn33.Width = 100.0;
		documentGridColumn33.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn34 = _salesGrid.AddStringColumn("PartyPartCaption", ArmSoft.SME.Common.Resources.Supplier, 50);
		documentGridColumn34.Width = 100.0;
		documentGridColumn34.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn35 = _salesGrid.AddStringColumn("PartyContractCaption", ArmSoft.SME.Common.Resources.Contract, 50);
		documentGridColumn35.Width = 100.0;
		documentGridColumn35.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn36 = _salesGrid.AddDateColumn("PartyDocUsableDate", ArmSoft.SME.Trade.Common.Resources.UseLife);
		documentGridColumn36.AllowEditing = DefaultBoolean.False;
		documentGridColumn36.Visible = Settings.ParametersService.ShowPartyUSEABLEDATE();
		DocumentGridColumn documentGridColumn37 = _salesGrid.AddStringColumn("PartyDocCountry", ArmSoft.SME.Common.Resources.Country, 50);
		documentGridColumn37.Width = 100.0;
		documentGridColumn37.AllowEditing = DefaultBoolean.False;
		documentGridColumn37.Visible = Settings.ParametersService.ShowPartyCountry();
		DocumentGridColumn documentGridColumn38 = _salesGrid.AddStringColumn("PartyDocProducer", ArmSoft.SME.Common.Resources.Producer, 50);
		documentGridColumn38.Width = 100.0;
		documentGridColumn38.Visible = Settings.ParametersService.ShowPartyProducer();
		DocumentGridColumn documentGridColumn39 = _salesGrid.AddStringColumn("PartyDocPropertyType1", ArmSoft.SME.Trade.Common.Resources.AttributeType1, 4);
		documentGridColumn39.Width = 100.0;
		documentGridColumn39.AllowEditing = DefaultBoolean.False;
		documentGridColumn39.Visible = Settings.ParametersService.ShowPartyProperty1();
		DocumentGridColumn documentGridColumn40 = _salesGrid.AddStringColumn("PartyDocPropertyValue1", ArmSoft.SME.Trade.Common.Resources.AttributeValue + " 1", 50);
		documentGridColumn40.Width = 100.0;
		documentGridColumn40.AllowEditing = DefaultBoolean.False;
		documentGridColumn40.Visible = Settings.ParametersService.ShowPartyProperty1();
		DocumentGridColumn documentGridColumn41 = _salesGrid.AddStringColumn("PartyDocPropertyType2", ArmSoft.SME.Trade.Common.Resources.AttributeType2, 4);
		documentGridColumn41.Width = 100.0;
		documentGridColumn41.AllowEditing = DefaultBoolean.False;
		documentGridColumn41.Visible = Settings.ParametersService.ShowPartyProperty2();
		DocumentGridColumn documentGridColumn42 = _salesGrid.AddStringColumn("PartyDocPropertyValue2", ArmSoft.SME.Trade.Common.Resources.AttributeValue + " 2", 50);
		documentGridColumn42.Width = 100.0;
		documentGridColumn42.AllowEditing = DefaultBoolean.False;
		documentGridColumn42.Visible = Settings.ParametersService.ShowPartyProperty2();
		foreach (GridColumn column3 in _salesGrid.Grid.Columns)
		{
			column3.AllowSorting = DefaultBoolean.True;
			column3.AllowColumnFiltering = DefaultBoolean.True;
			column3.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_salesTableView = _salesGrid.View;
		_salesGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
		_salesGrid.VerticalAlignment = VerticalAlignment.Stretch;
		_storageControl.Code = _storage;
		_supplier.Code = _supplierCode;
		_buyer.Code = _buyerCode;
		_mtCodeControl.Code = _mtCode;
		_qntUnitControl.Code = _qntUnit;
		_purchasesStartDate.EditValue = Settings.HostingEnvironment.DateBegin;
		_purchasesEndDate.EditValue = Settings.HostingEnvironment.DateEnd;
		_salesStartDate.EditValue = Settings.HostingEnvironment.DateBegin;
		_salesEndDate.EditValue = Settings.HostingEnvironment.DateEnd;
		_mtCodeControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTCode_EditValueChanged).Handler;
		_storageControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged).Handler;
		_supplier.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Supplier_EditValueChanged).Handler;
		_buyer.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Buyer_EditValueChanged).Handler;
		_purchasesStartDate.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PurchasesStartDate_EditValueChanged).Handler;
		_purchasesEndDate.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PurchasesEndDate_EditValueChanged).Handler;
		_salesStartDate.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SalesStartDate_EditValueChanged).Handler;
		_salesEndDate.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SalesEndDate_EditValueChanged).Handler;
		LoadGoodCurrentAvail();
		LoadGoodPurchases();
		LoadGoodSales();
		return ShowDialog();
	}

	private void SetReadOnly()
	{
		_mtGroup.IsReadOnly = true;
		_mtCodeControl.IsReadOnly = true;
		_qntUnitControl.IsReadOnly = true;
	}

	private void MTCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		UpdateMaterialData();
	}

	private void MTGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_mtGroup.DoValidate() && !string.IsNullOrWhiteSpace(_mtCodeControl.Code) && _mtCodeControl.DoValidate())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCodeControl.Code);
			if (item != null)
			{
				_qntUnitControl.DataView = new MTQntUnits(item.MTCode, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", ItemTypesSE.Goods);
			}
		}
		if (!string.IsNullOrWhiteSpace(_mtGroup.Code) && !string.IsNullOrWhiteSpace(_mtCodeControl.Code) && !_mtCodeControl.DoValidate())
		{
			CleanAllField();
		}
	}

	private void MTCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_mtCodeControl.Code) && _mtCodeControl.DoValidate())
		{
			_mtCode = _mtCodeControl.Code;
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode);
			if (item != null)
			{
				_mtID = item.Id;
				_qntUnitControl.DataView = new MTQntUnits(_mtCode, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", ItemTypesSE.Goods);
				_qntUnitControl.Code = item.Unit;
				_qntUnit = item.Unit;
			}
			LoadGoodCurrentAvail();
			LoadGoodPurchases();
			LoadGoodSales();
		}
		else
		{
			_qntUnitControl.DataView = new MTQntUnits("", isShowQntUnitCaption: false, isShowOnlyComplects: false, _mtGroup.Code, ItemTypesSE.Goods);
			_qntUnitControl.Code = "";
			_mtCurrentAvailByParties.Clear();
			_partiesGrid.ItemsSource = _mtCurrentAvailByParties;
			_partiesGrid.Grid.RefreshData();
			_mtPurchasesByParties.Clear();
			_purchasesGrid.ItemsSource = _mtPurchasesByParties;
			_purchasesGrid.Grid.RefreshData();
			_mtSalesByParties.Clear();
			_salesGrid.ItemsSource = _mtSalesByParties;
			_salesGrid.Grid.RefreshData();
		}
	}

	private void MTCode_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, _mtGroup.Code, isShowOnlyInPriceList: true);
		ModalBrowser modalBrowser = new ModalBrowser(goodsFilter)
		{
			LookUpValue = _mtCodeControl.Code,
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			_mtCodeControl.Code = goodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
		}
		e.Cancel = true;
	}

	private void QntUnit_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		MTQntUnits mTQntUnits = new MTQntUnits(_mtCode, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", ItemTypesSE.Goods);
		ModalBrowser modalBrowser = new ModalBrowser(mTQntUnits)
		{
			LookUpValue = _qntUnitControl.Code,
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			_qntUnitControl.Code = mTQntUnits.ValueMember(modalBrowser.SelectedRowIndex);
			if (string.IsNullOrWhiteSpace(_mtCodeControl.Code))
			{
				MTQntUnitDataProvider.DataRow.Key key = (MTQntUnitDataProvider.DataRow.Key)mTQntUnits.KeyMember(modalBrowser.SelectedRowIndex);
				if (key != null)
				{
					_mtID = key.MTID;
					MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(_mtID);
					if (itemById != null)
					{
						_mtCode = itemById.MTCode;
						_mtCodeControl.Code = _mtCode;
						_qntUnit = _qntUnitControl.Code;
						MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(_mtID, _qntUnitControl.Code);
						if (qntUnit != null && qntUnit.Coef != 0m)
						{
							_qntUnitCoef = qntUnit.Coef;
						}
					}
				}
			}
		}
		e.Cancel = true;
	}

	private void QntUnit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		decimal qntUnitCoef = _qntUnitCoef;
		if (!string.IsNullOrWhiteSpace(_qntUnitControl.Code) && _qntUnitControl.DoValidate() && !string.IsNullOrWhiteSpace(_mtCodeControl.Code) && _mtCodeControl.DoValidate())
		{
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(_mtID, _qntUnitControl.Code);
			if (qntUnit != null && qntUnit.Coef != 0m)
			{
				_qntUnitCoef = qntUnit.Coef;
				ReCalcAllGridsQty(qntUnitCoef, _qntUnitCoef);
			}
		}
	}

	private void UpdateMaterialData()
	{
		try
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCodeControl.Code);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _mtCodeControl.Code));
			}
			_qntUnitControl.DataView = new MTQntUnits(item.MTCode, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", ItemTypesSE.Goods);
			_qntUnitControl.Code = item.Unit;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CleanAllField(bool IsCleanItemCodeField = true)
	{
		if (IsCleanItemCodeField)
		{
			_mtCodeControl.Code = "";
			_qntUnitControl.Code = "";
		}
	}

	private void LoadGoodCurrentAvail()
	{
		try
		{
			_mtCurrentAvailByParties.Clear();
			if (_mtID == -1)
			{
				return;
			}
			CurrentAvailGoodsByPartiesDataProvider.Param param = new CurrentAvailGoodsByPartiesDataProvider.Param
			{
				Storage = _storage,
				MTID = _mtID,
				SupplierID = _supplierID
			};
			_mtCurrentAvailByParties = new CurrentAvailGoodsByPartiesDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
			if (!string.IsNullOrWhiteSpace(_qntUnitControl.Code))
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(_mtID, _qntUnitControl.Code);
				if (qntUnit != null && qntUnit.Coef != 0m)
				{
					_qntUnitCoef = qntUnit.Coef;
					foreach (CurrentAvailGoodsByPartiesDataProvider.DataRow mtCurrentAvailByParty in _mtCurrentAvailByParties)
					{
						mtCurrentAvailByParty.Qty = (mtCurrentAvailByParty.Qty / _qntUnitCoef).RoundAs(3);
						if (mtCurrentAvailByParty.Qty == 0m)
						{
							mtCurrentAvailByParty.Cost = 0m;
						}
						else
						{
							mtCurrentAvailByParty.Cost = (mtCurrentAvailByParty.Summa / mtCurrentAvailByParty.Qty).RoundAs(2);
						}
					}
				}
			}
			_partiesGrid.ItemsSource = _mtCurrentAvailByParties;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void LoadGoodPurchases()
	{
		try
		{
			_mtPurchasesByParties.Clear();
			if (_mtID == -1)
			{
				return;
			}
			if (_purchasesStartDate.EditValue.HasValue && _purchasesEndDate.EditValue.HasValue)
			{
				ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_purchasesStartDate.EditValue, _purchasesEndDate.EditValue);
				PurchasesByPartiesDataProvider.Param param = new PurchasesByPartiesDataProvider.Param
				{
					Storage = _storage,
					MTID = _mtID,
					SupplierID = _supplierID,
					StartDate = _purchasesStartDate.EditValue.Value,
					EndDate = _purchasesEndDate.EditValue.Value
				};
				_mtPurchasesByParties = new PurchasesByPartiesDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
			}
			if (!string.IsNullOrWhiteSpace(_qntUnitControl.Code))
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(_mtID, _qntUnitControl.Code);
				if (qntUnit != null && qntUnit.Coef != 0m)
				{
					_qntUnitCoef = qntUnit.Coef;
					foreach (PurchasesByPartiesDataProvider.DataRow mtPurchasesByParty in _mtPurchasesByParties)
					{
						mtPurchasesByParty.Quantity = (mtPurchasesByParty.Quantity / _qntUnitCoef).RoundAs(3);
						if (mtPurchasesByParty.Quantity == 0m)
						{
							mtPurchasesByParty.Cost = 0m;
						}
						else
						{
							mtPurchasesByParty.Cost = (mtPurchasesByParty.CostSummWithVAT / mtPurchasesByParty.Quantity).RoundAs(2);
						}
					}
				}
			}
			_purchasesGrid.ItemsSource = _mtPurchasesByParties;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void LoadGoodSales()
	{
		try
		{
			_mtSalesByParties.Clear();
			if (_mtID == -1)
			{
				return;
			}
			if (_salesStartDate.EditValue.HasValue && _salesEndDate.EditValue.HasValue)
			{
				ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_salesStartDate.EditValue, _salesEndDate.EditValue);
				SalesByPartiesDataProvider.Param param = new SalesByPartiesDataProvider.Param
				{
					Storage = _storage,
					MTID = _mtID,
					SupplierID = _supplierID,
					BuyerId = _buyerID,
					StartDate = _salesStartDate.EditValue.Value,
					EndDate = _salesEndDate.EditValue.Value
				};
				_mtSalesByParties = new SalesByPartiesDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
			}
			if (!string.IsNullOrWhiteSpace(_qntUnitControl.Code))
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(_mtID, _qntUnitControl.Code);
				if (qntUnit != null && qntUnit.Coef != 0m)
				{
					_qntUnitCoef = qntUnit.Coef;
					foreach (SalesByPartiesDataProvider.DataRow mtSalesByParty in _mtSalesByParties)
					{
						mtSalesByParty.Quantity = (mtSalesByParty.Quantity / _qntUnitCoef).RoundAs(3);
						if (mtSalesByParty.Quantity == 0m)
						{
							mtSalesByParty.SalePrice = 0m;
							mtSalesByParty.SalePriceWithVAT = 0m;
						}
						else
						{
							mtSalesByParty.SalePrice = (mtSalesByParty.SaleSumm / mtSalesByParty.Quantity).RoundAs(2);
							mtSalesByParty.SalePriceWithVAT = (mtSalesByParty.SaleSummWithVAT / mtSalesByParty.Quantity).RoundAs(2);
						}
					}
				}
			}
			_salesGrid.ItemsSource = _mtSalesByParties;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void RefreshAvail_Click(object sender, RoutedEventArgs e)
	{
		LoadGoodCurrentAvail();
	}

	private void RefreshPurchases_Click(object sender, RoutedEventArgs e)
	{
		LoadGoodPurchases();
	}

	private void RefreshSales_Click(object sender, RoutedEventArgs e)
	{
		LoadGoodSales();
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_storageControl.DoValidate())
		{
			_storage = _storageControl.Code;
			LoadGoodCurrentAvail();
			LoadGoodPurchases();
			LoadGoodSales();
		}
	}

	private void PurchasesStartDate_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		LoadGoodPurchases();
	}

	private void PurchasesEndDate_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		LoadGoodPurchases();
	}

	private void SalesStartDate_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		LoadGoodSales();
	}

	private void SalesEndDate_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		LoadGoodSales();
	}

	private void Supplier_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_supplier.DoValidate())
		{
			_supplierCode = _supplier.Code;
			_supplierID = TradeSettings.TradeCacheService.Partner.GetItem(_supplierCode)?.Id ?? (-1);
			LoadGoodCurrentAvail();
			LoadGoodPurchases();
			LoadGoodSales();
		}
	}

	private void Buyer_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_buyer.DoValidate())
		{
			_buyerCode = _buyer.Code;
			_buyerID = TradeSettings.TradeCacheService.Partner.GetItem(_buyerCode)?.Id ?? (-1);
			LoadGoodSales();
		}
	}

	private void ReCalcAllGridsQty(decimal OldQntunitCoef, decimal NewQntUnitCoef)
	{
		foreach (CurrentAvailGoodsByPartiesDataProvider.DataRow mtCurrentAvailByParty in _mtCurrentAvailByParties)
		{
			mtCurrentAvailByParty.Qty = (mtCurrentAvailByParty.Qty * OldQntunitCoef / NewQntUnitCoef).RoundAs(3);
			if (mtCurrentAvailByParty.Qty == 0m)
			{
				mtCurrentAvailByParty.Cost = 0m;
			}
			else
			{
				mtCurrentAvailByParty.Cost = (mtCurrentAvailByParty.Summa / mtCurrentAvailByParty.Qty).RoundAs(2);
			}
		}
		_partiesGrid.ItemsSource = _mtCurrentAvailByParties;
		_partiesGrid.Grid.RefreshData();
		foreach (PurchasesByPartiesDataProvider.DataRow mtPurchasesByParty in _mtPurchasesByParties)
		{
			mtPurchasesByParty.Quantity = (mtPurchasesByParty.Quantity / _qntUnitCoef).RoundAs(3);
			if (mtPurchasesByParty.Quantity == 0m)
			{
				mtPurchasesByParty.Cost = 0m;
			}
			else
			{
				mtPurchasesByParty.Cost = (mtPurchasesByParty.CostSummWithVAT / mtPurchasesByParty.Quantity).RoundAs(2);
			}
		}
		_purchasesGrid.ItemsSource = _mtPurchasesByParties;
		_purchasesGrid.Grid.RefreshData();
		foreach (SalesByPartiesDataProvider.DataRow mtSalesByParty in _mtSalesByParties)
		{
			mtSalesByParty.Quantity = (mtSalesByParty.Quantity * OldQntunitCoef / NewQntUnitCoef).RoundAs(3);
			if (mtSalesByParty.Quantity == 0m)
			{
				mtSalesByParty.SalePrice = 0m;
				mtSalesByParty.SalePriceWithVAT = 0m;
			}
			else
			{
				mtSalesByParty.SalePrice = (mtSalesByParty.SaleSumm / mtSalesByParty.Quantity).RoundAs(2);
				mtSalesByParty.SalePriceWithVAT = (mtSalesByParty.SaleSummWithVAT / mtSalesByParty.Quantity).RoundAs(2);
			}
		}
		_salesGrid.ItemsSource = _mtSalesByParties;
		_salesGrid.Grid.RefreshData();
	}
}
