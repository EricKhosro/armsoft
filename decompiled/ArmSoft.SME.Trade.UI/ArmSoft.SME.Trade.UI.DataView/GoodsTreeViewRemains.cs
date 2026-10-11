using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Converters;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.ReportViewers.Goods;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10050)]
public class GoodsTreeViewRemains : TradeDataView<GoodsRemainsTreeViewDataProvider.DataRow, GoodsRemainsTreeViewDataProvider.Param>
{
	public class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly GoodsTreeViewRemains _dataView;

		private readonly DateEditControl _remDate;

		private readonly MultiSelectDropDownControl _storage;

		private readonly DropDownControl _mtGroup;

		private readonly MultiSelectDropDownControl _mtCode;

		private readonly CheckEditExt _showByAditionalQntUnits;

		private readonly CheckEditExt _showSumms;

		private readonly CheckEditExt _showSummsWithVAT;

		private readonly CheckEditExt _showCurrSumms;

		private readonly CheckEditExt _showByDifferentParties;

		private readonly CheckEditExt _detailByStorage;

		private readonly DropDownControl _minQtys;

		private readonly MultiSelectDropDownControl _priceListTypes;

		private readonly GoodsFilter _goods;

		public DataViewDialog(GoodsTreeViewRemains dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			_goods = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: true);
			Caption = (_dataView.ShowByDifferentParties ? ArmSoft.SME.Trade.Common.Resources.MatRepAvailByParties : ArmSoft.SME.Trade.Common.Resources.MatRepAvail);
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_remDate = AddDateControl(container, "RemDate", ArmSoft.SME.Common.Resources.DateWord, useLongDate: false, isRequired: true, !useParamValues);
			_remDate.EditValue = (useParamValues ? dataView.RemDate : Settings.HostingEnvironment.CurrentDate);
			AddSpecialDateButton(container, _remDate);
			DXTabControl tc = AddTabControl(null);
			LayoutGroup layoutGroup = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.General);
			string storage = ArmSoft.SME.Common.Resources.Storage;
			bool? checkViewOrOperationAccess = true;
			_storage = AddMultiSelectDropDownControl(layoutGroup, "Storage", storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(null, checkViewOrOperationAccess), isRequired: false, !useParamValues);
			_storage.SelectedString = (useParamValues ? _dataView.Storage : string.Empty);
			_mtGroup = AddDropDownControl(layoutGroup, "MTGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_mtGroup.Code = (useParamValues ? _dataView.MTGroup : string.Empty);
			_mtGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MtGroup_EditValueChanged).Handler;
			_mtCode = AddMultiSelectDropDownControl(layoutGroup, "MTCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, _goods, isRequired: false, !useParamValues);
			_mtCode.SelectedString = (useParamValues ? _dataView.MTCode : string.Empty);
			AddLine(layoutGroup);
			_showByAditionalQntUnits = AddBooleanControl(layoutGroup, "ShowByAditionalQntUnits", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, !useParamValues);
			_showByAditionalQntUnits.IsChecked = useParamValues && _dataView.ShowByAditionalQntUnits;
			_showSumms = AddBooleanControl(layoutGroup, "ShowSumms", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, !useParamValues);
			_showSumms.IsChecked = useParamValues && _dataView.ShowSumms;
			_showSummsWithVAT = AddBooleanControl(layoutGroup, "ShowSummsWithVAT", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, !useParamValues);
			_showSummsWithVAT.IsChecked = useParamValues && _dataView.ShowSummsWithVAT;
			_showCurrSumms = AddBooleanControl(layoutGroup, "ShowCurrSumms", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, dataView._goodsCurr), !useParamValues);
			_showCurrSumms.IsChecked = useParamValues && _dataView.ShowCurrSumms;
			_showByDifferentParties = AddBooleanControl(layoutGroup, "ShowByDifferentParties", ArmSoft.SME.Common.Resources.ByDiffParties, !useParamValues);
			_showByDifferentParties.IsChecked = useParamValues && _dataView.ShowByDifferentParties;
			_showByDifferentParties.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowByDifferentParties_EditValueChanged).Handler;
			AddLine(layoutGroup);
			_detailByStorage = AddBooleanControl(layoutGroup, "DetailByStorage", ArmSoft.SME.Trade.Common.Resources.DetailByStorage, !useParamValues);
			_detailByStorage.IsChecked = useParamValues && _dataView.DetailByStorage;
			_detailByStorage.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DetailByStorage_EditValueChanged).Handler;
			_minQtys = AddDropDownControl(layoutGroup, "MinQtys", ArmSoft.SME.Common.Resources.MinQtys, new SmartEnum(typeof(AvailabilityMinQtysSE)), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_minQtys.Code = (useParamValues ? _dataView.MinQtys : string.Empty);
			AddLine(layoutGroup);
			_priceListTypes = AddMultiSelectDropDownControl(layoutGroup, "PriceListTypes", ArmSoft.SME.Trade.Common.Resources.ShowPricesByPT, new ArmSoft.SME.Trade.UI.DropDown.PriceTypes(), isRequired: false, !useParamValues);
			_priceListTypes.SelectedString = (useParamValues ? _dataView.PriceListTypes : string.Empty);
			if (!isRefreshMode)
			{
				AddColumnsSelectionTab(tc, layoutGroup, null);
			}
		}

		private void MtGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue)
			{
				_goods.Group = (string)e.NewValue;
			}
		}

		private void ShowByDifferentParties_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (_showByDifferentParties.IsChecked.HasValue && _showByDifferentParties.IsChecked.Value)
			{
				_detailByStorage.IsChecked = true;
				_minQtys.Code = "";
			}
		}

		private void DetailByStorage_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (_detailByStorage.IsChecked.HasValue && !_detailByStorage.IsChecked.Value)
			{
				_showByDifferentParties.IsChecked = false;
			}
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_remDate.EditValue.Value);
				base.DialogValidation(args);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.RemDate = _remDate.EditValue.Value;
			_dataView.Storage = _storage.SelectedString;
			_dataView.MTGroup = _mtGroup.Code;
			_dataView.MTCode = _mtCode.SelectedString;
			_dataView.ShowByAditionalQntUnits = _showByAditionalQntUnits.IsChecked.Value;
			_dataView.ShowSumms = _showSumms.IsChecked.Value;
			_dataView.ShowSummsWithVAT = _showSummsWithVAT.IsChecked.Value;
			_dataView.ShowCurrSumms = _showCurrSumms.IsChecked.Value;
			_dataView.ShowByDifferentParties = _showByDifferentParties.IsChecked.Value;
			_dataView.DetailByStorage = _detailByStorage.IsChecked.Value;
			_dataView.MinQtys = _minQtys.Code;
			_dataView.PriceListTypes = _priceListTypes.SelectedString;
		}
	}

	private readonly string _goodsCurr;

	private readonly bool _showSumms;

	private List<string> _dynamicColumns = new List<string>();

	private List<ViewColumn> potentiallyColumns;

	public override string HelpId => "Purchases_Reports_Remains_Tree_View";

	public override bool DialogSupported => true;

	public override bool AllowAdd => false;

	public override bool AllowEdit => false;

	public override bool AllowDelete => false;

	public override List<ViewColumn> PotentiallyColumns => potentiallyColumns;

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public DateOnly RemDate
	{
		get
		{
			return (DateOnly)Parameters["RemDate"].Value;
		}
		set
		{
			Parameters["RemDate"].Value = value;
		}
	}

	public string Storage
	{
		get
		{
			return (string)Parameters["Storage"].Value;
		}
		set
		{
			Parameters["Storage"].Value = value;
		}
	}

	public string MTGroup
	{
		get
		{
			return (string)Parameters["MTGroup"].Value;
		}
		set
		{
			Parameters["MTGroup"].Value = value;
		}
	}

	public string MTCode
	{
		get
		{
			return (string)Parameters["MTCode"].Value;
		}
		set
		{
			Parameters["MTCode"].Value = value;
		}
	}

	public bool ShowByAditionalQntUnits
	{
		get
		{
			return (bool)Parameters["ShowByAditionalQntUnits"].Value;
		}
		set
		{
			Parameters["ShowByAditionalQntUnits"].Value = value;
		}
	}

	public bool ShowSumms
	{
		get
		{
			return (bool)Parameters["ShowSumms"].Value;
		}
		set
		{
			Parameters["ShowSumms"].Value = value;
		}
	}

	public bool ShowSummsWithVAT
	{
		get
		{
			return (bool)Parameters["ShowSummsWithVAT"].Value;
		}
		set
		{
			Parameters["ShowSummsWithVAT"].Value = value;
		}
	}

	public bool ShowCurrSumms
	{
		get
		{
			return (bool)Parameters["ShowCurrSumms"].Value;
		}
		set
		{
			Parameters["ShowCurrSumms"].Value = value;
		}
	}

	public bool ShowByDifferentParties
	{
		get
		{
			return (bool)Parameters["ShowByDifferentParties"].Value;
		}
		set
		{
			Parameters["ShowByDifferentParties"].Value = value;
		}
	}

	public bool DetailByStorage
	{
		get
		{
			return (bool)Parameters["DetailByStorage"].Value;
		}
		set
		{
			Parameters["DetailByStorage"].Value = value;
		}
	}

	public string MinQtys
	{
		get
		{
			return (string)Parameters["MinQtys"].Value;
		}
		set
		{
			Parameters["MinQtys"].Value = value;
		}
	}

	public string PriceListTypes
	{
		get
		{
			return (string)Parameters["PriceListTypes"].Value;
		}
		set
		{
			Parameters["PriceListTypes"].Value = value;
		}
	}

	public GoodsTreeViewRemains()
	{
		_showSumms = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		_goodsCurr = Settings.ParametersService.MTCurr();
		DataProvider = new GoodsRemainsTreeViewDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("RemDate", ArmSoft.SME.Common.Resources.DateWord, Settings.HostingEnvironment.CurrentDate, "dd/MM/yy");
		AddParameter("Storage", ArmSoft.SME.Common.Resources.Storage, string.Empty);
		AddParameter("MTGroup", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("MTCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, string.Empty);
		AddParameter("ShowByAditionalQntUnits", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, false);
		AddParameter("ShowSumms", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, false);
		AddParameter("ShowSummsWithVAT", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, false);
		AddParameter("ShowCurrSumms", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, _goodsCurr), false);
		AddParameter("ShowByDifferentParties", ArmSoft.SME.Common.Resources.ByDiffParties, false);
		AddParameter("DetailByStorage", ArmSoft.SME.Trade.Common.Resources.DetailByStorage, false);
		AddParameter("MinQtys", ArmSoft.SME.Common.Resources.MinQtys, string.Empty);
		AddParameter("PriceListTypes", ArmSoft.SME.Trade.Common.Resources.ShowPricesByPT, string.Empty);
		FillColumnsFromDataProvider();
		Columns["ImageExists"].Converter = new BooleanToImageExists();
		potentiallyColumns = new List<ViewColumn>();
		potentiallyColumns.AddRange(Columns.ViewList);
		potentiallyColumns.AddRange(MaterialCache.GetDataViewColumns(ArmSoft.SME.Trade.Common.Resources.MaterialValue, "", ""));
		potentiallyColumns.AddRange(ProducerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Producer, "", ""));
	}

	protected override void SetColumnsProperties()
	{
		base.SetColumnsProperties();
		foreach (ViewColumn view in Columns.ViewList)
		{
			if (view.Name.Substring(0, 3) == "PT_" || view.Name.Substring(0, 3) == "PS_")
			{
				Columns[view.Name].IsSystem = true;
				Columns[view.Name].FooterSum = view.Name.Substring(0, 3) == "PS_";
			}
		}
		Columns["Storage"].IsSystem = true;
		Columns["StorageCaption"].IsSystem = true;
		Columns["Cost"].IsSystem = true;
		Columns["CostSumm"].IsSystem = true;
		Columns["CostWithVAT"].IsSystem = true;
		Columns["CostSummWithVAT"].IsSystem = true;
		Columns["CurrCost"].IsSystem = true;
		Columns["CurrCostSumm"].IsSystem = true;
		Columns["IncomeDate"].IsSystem = true;
		Columns["PartyPartCaption"].IsSystem = true;
		Columns["PartyPartTaxCode"].IsSystem = true;
		Columns["PartyContractCaption"].IsSystem = true;
		Columns["PartyDocType"].IsSystem = true;
		Columns["PartyDocNum"].IsSystem = true;
		Columns["PartyDocUsableDate"].IsSystem = true;
		Columns["PartyDocCountry"].IsSystem = true;
		Columns["PartyDocProducer"].IsSystem = true;
		Columns["PartyDocPropertyType1"].IsSystem = true;
		Columns["PartyDocPropertyValue1"].IsSystem = true;
		Columns["PartyDocPropertyType2"].IsSystem = true;
		Columns["PartyDocPropertyValue2"].IsSystem = true;
		Columns["GroupCodeCaption"].IsSystem = true;
		Columns["OrderComment"].Visible = false;
		Columns["OrderQty"].Visible = false;
		Columns["MaxQty"].Visible = false;
	}

	public override string KeyFieldName()
	{
		return "KeyFieldForTree";
	}

	public override string ParentFieldName()
	{
		return "Parent";
	}

	public override string IsLeafFieldName()
	{
		return "Leaf";
	}

	public override bool TreeViewAllowed(ref bool setIsDefault)
	{
		setIsDefault = true;
		return true;
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		if (!TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableStorage())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.AccessToStoragesDenied, Settings.LoginService.UserName), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return null;
		}
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		_dynamicColumns = PriceListTypes.SplitSelected().SelectMany((string priceType) => new string[2]
		{
			"PT_" + priceType,
			"PS_" + priceType
		}).ToList();
		foreach (ViewColumn view in Columns.ViewList)
		{
			if (view.Name.Substring(0, 3) == "PT_" || view.Name.Substring(0, 3) == "PS_")
			{
				dvp.Set_ColumnVisible(view.Name, _dynamicColumns.Contains(view.Name));
			}
		}
		dvp.RemoveGrouping();
		dvp.Set_ColumnVisible("MinQty", !string.IsNullOrWhiteSpace(MinQtys));
		dvp.Set_ColumnVisible("DiffQty", !string.IsNullOrWhiteSpace(MinQtys));
		dvp.Set_ColumnVisible("IncomeDate", ShowByDifferentParties && Settings.ParametersService.ShowPartyIncomeDate());
		dvp.Set_ColumnVisible("PartyPartCaption", ShowByDifferentParties && Settings.ParametersService.ShowPartySUPPLIER());
		dvp.Set_ColumnVisible("PartyPartTaxCode", ShowByDifferentParties && Settings.ParametersService.ShowPartnerTaxCode());
		dvp.Set_ColumnVisible("PartyContractCaption", ShowByDifferentParties && Settings.ParametersService.ShowPartyCONTRACT());
		dvp.Set_ColumnVisible("PartyDocType", ShowByDifferentParties && Settings.ParametersService.ShowPartyDocType());
		dvp.Set_ColumnVisible("PartyDocNum", ShowByDifferentParties && Settings.ParametersService.ShowPartyDocNum());
		dvp.Set_ColumnVisible("PartyDocUsableDate", ShowByDifferentParties && Settings.ParametersService.ShowPartyUSEABLEDATE());
		dvp.Set_ColumnVisible("PartyDocCountry", ShowByDifferentParties && Settings.ParametersService.ShowPartyCountry());
		dvp.Set_ColumnVisible("PartyDocProducer", ShowByDifferentParties && Settings.ParametersService.ShowPartyProducer());
		dvp.Set_ColumnVisible("PartyDocPropertyType1", ShowByDifferentParties && Settings.ParametersService.ShowPartyProperty1());
		dvp.Set_ColumnVisible("PartyDocPropertyValue1", ShowByDifferentParties && Settings.ParametersService.ShowPartyProperty1());
		dvp.Set_ColumnVisible("PartyDocPropertyType2", ShowByDifferentParties && Settings.ParametersService.ShowPartyProperty2());
		dvp.Set_ColumnVisible("PartyDocPropertyValue2", ShowByDifferentParties && Settings.ParametersService.ShowPartyProperty2());
		dvp.Set_ColumnVisible("GroupCodeCaption", value: false);
		dvp.Set_ColumnVisible("Cost", ShowSumms);
		dvp.Set_ColumnVisible("CostSumm", ShowSumms);
		dvp.Set_ColumnVisible("CostWithVAT", ShowSummsWithVAT);
		dvp.Set_ColumnVisible("CostSummWithVAT", ShowSummsWithVAT);
		dvp.Set_ColumnVisible("CurrCost", ShowCurrSumms);
		dvp.Set_ColumnVisible("CurrCostSumm", ShowCurrSumms);
		dvp.Set_ColumnVisible("Storage", DetailByStorage || !string.IsNullOrWhiteSpace(Storage));
		dvp.Set_ColumnVisible("StorageCaption", DetailByStorage || !string.IsNullOrWhiteSpace(Storage));
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("MTCode");
			dvp.Set_ColumnSortOrder("Storage");
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.RemDate = RemDate;
		ParametersProvider.Storage = Storage;
		ParametersProvider.MTGroup = MTGroup;
		ParametersProvider.MTCode = MTCode;
		ParametersProvider.ShowByAditionalQntUnits = ShowByAditionalQntUnits;
		ParametersProvider.ShowSumms = ShowSumms;
		ParametersProvider.ShowSummsWithVAT = ShowSummsWithVAT;
		ParametersProvider.ShowCurrSumms = ShowCurrSumms;
		ParametersProvider.ShowByDifferentParties = ShowByDifferentParties;
		ParametersProvider.DetailByStorage = DetailByStorage;
		ParametersProvider.MinQtys = MinQtys;
		ParametersProvider.PriceListTypes = PriceListTypes;
		ParametersProvider.SubTotalType = "4";
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		GoodsRemainsTreeViewDataProvider.DataRow dataRow = (GoodsRemainsTreeViewDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow == null)
		{
			return;
		}
		if (dataRow.Leaf)
		{
			if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepTAccount).AllowView)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowMTTAccount", ArmSoft.SME.Trade.Common.Resources.TAccount, ShowMTTAccount, "", "", storeOpContext: true);
			}
			if (DetailByStorage || !string.IsNullOrWhiteSpace(Storage))
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "CreateDoc", ArmSoft.SME.Common.Resources.CreateDoc, CreateDoc, "doc_new_16.svg");
			}
		}
		Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "PrintLabel", ArmSoft.SME.Trade.Common.Resources.PrintLabel, PrintLabel);
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		GoodsRemainsTreeViewDataProvider.DataRow dataRow = row as GoodsRemainsTreeViewDataProvider.DataRow;
		dataRow.MaterialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(dataRow.MTCode);
		dataRow.ProducerCacheRow = TradeSettings.TradeCacheService.Producer.GetItem(dataRow.MaterialCacheRow?.Producer ?? "");
	}

	private void ShowMTTAccount()
	{
		List<GoodsRemainsTreeViewDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRemainsTreeViewDataProvider.DataRow>();
		MTAccountReportViewer mTAccountReportViewer;
		if (selectedOrFocusedRows.Count == 1)
		{
			mTAccountReportViewer = new MTAccountReportViewer();
			mTAccountReportViewer.GoodCode = selectedOrFocusedRows[0].MTCode;
			if (DetailByStorage)
			{
				mTAccountReportViewer.Storage = selectedOrFocusedRows[0].Storage;
			}
		}
		else
		{
			List<ListOfSelectedMaterialStorage> list = new List<ListOfSelectedMaterialStorage>();
			foreach (GoodsRemainsTreeViewDataProvider.DataRow item in selectedOrFocusedRows)
			{
				ListOfSelectedMaterialStorage listOfSelectedMaterialStorage = new ListOfSelectedMaterialStorage();
				listOfSelectedMaterialStorage.GoodCode = item.MTCode;
				if (DetailByStorage)
				{
					listOfSelectedMaterialStorage.Storage = item.Storage;
				}
				list.Add(listOfSelectedMaterialStorage);
			}
			mTAccountReportViewer = new MTAccountReportViewer(isForSelectedRows: true, list);
		}
		mTAccountReportViewer.ShowFromTurns = true;
		mTAccountReportViewer.ShowAltUnit = ShowByAditionalQntUnits;
		mTAccountReportViewer.ShowSumms = ShowSumms;
		mTAccountReportViewer.ShowSummsWithVAT = ShowSummsWithVAT;
		mTAccountReportViewer.ShowCurrSumms = ShowCurrSumms;
		mTAccountReportViewer.ShowParties = ShowByDifferentParties;
		mTAccountReportViewer.StartDate = Settings.HostingEnvironment.DateBegin;
		mTAccountReportViewer.EndDate = Settings.HostingEnvironment.DateEnd;
		mTAccountReportViewer.Show(useParamValues: true);
	}

	private void PrintLabel()
	{
		List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow> list = new List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow>();
		ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow goodLabelRow = new ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow();
		foreach (GoodsRemainsTreeViewDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRemainsTreeViewDataProvider.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.MTCode);
			if (item != null && item.Type == GoodsTypesSE.Good)
			{
				goodLabelRow = InitContextFunctionsHelper.BuildLabelGoodRow(item);
				goodLabelRow.Quantity = selectedOrFocusedRow.Quantity;
				list.Add(goodLabelRow);
			}
		}
		new PrintLabelDialog(null, null, list).Invoke();
	}

	private void CreateDoc()
	{
		if (!ShowByDifferentParties && !DetailByStorage)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidCreateDocFromGoodsAvail);
		}
		int num = 10;
		List<short> list = new List<short>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<short> span = CollectionsMarshal.AsSpan(list);
		span[0] = DocumentTypeSE.MTInvoice.Value;
		span[1] = DocumentTypeSE.MTMove.Value;
		span[2] = DocumentTypeSE.MTMoveWithConfirmation.Value;
		span[3] = DocumentTypeSE.MTIncome.Value;
		span[4] = DocumentTypeSE.MTIncomeRetail.Value;
		span[5] = DocumentTypeSE.MTExpense.Value;
		span[6] = DocumentTypeSE.MTBill.Value;
		span[7] = DocumentTypeSE.MTReturnToSupplier.Value;
		span[8] = DocumentTypeSE.MTStockTaking.Value;
		span[9] = DocumentTypeSE.InvoiceForPay.Value;
		List<short> list2 = list;
		if (!Settings.ParametersService.MTMoveWithConfirmation())
		{
			list2.Remove(DocumentTypeSE.MTMoveWithConfirmation.Value);
		}
		if (ShowByDifferentParties || !ShowSumms)
		{
			list2.Remove(DocumentTypeSE.MTStockTaking.Value);
		}
		DocumentTypeSE documentTypeSE = InitContextFunctionsHelper.ChooseDocTypeFromMenu(list2);
		if (!(documentTypeSE == null))
		{
			bool fillAvailQty = InitContextFunctionsHelper.FillAllAvailabilityAvailabilityOrTurnOver(documentTypeSE);
			if ((short)documentTypeSE == DocumentTypeSE.MTInvoice.Value)
			{
				InitContextFunctionsHelper.CreateNewMTInvoiceDoc(fillAvailQty, RemDate, ShowByDifferentParties);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTMove.Value)
			{
				InitContextFunctionsHelper.CreateNewMTMoveDoc(fillAvailQty, RemDate, ShowByDifferentParties);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTMoveWithConfirmation.Value)
			{
				InitContextFunctionsHelper.CreateNewMTMoveWithConfirmationDoc(fillAvailQty, RemDate, ShowByDifferentParties);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTIncome.Value)
			{
				InitContextFunctionsHelper.CreateNewMTIncomeDoc(RemDate);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTIncomeRetail.Value)
			{
				InitContextFunctionsHelper.CreateNewMTIncomeRetailDoc(RemDate, isFromRetail: false);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTExpense.Value)
			{
				InitContextFunctionsHelper.CreateNewMTExpenseDoc(fillAvailQty, RemDate, ShowByDifferentParties);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTBill.Value)
			{
				InitContextFunctionsHelper.CreateNewMTBillDoc(fillAvailQty, RemDate, ShowByDifferentParties);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTReturnToSupplier.Value)
			{
				InitContextFunctionsHelper.CreateNewMTReturnToSupplierDoc(fillAvailQty, RemDate, ShowByDifferentParties);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.InvoiceForPay.Value)
			{
				InitContextFunctionsHelper.CreateNewInvoiceForPayDoc(fillAvailQty, isFromTurnOver: false);
			}
		}
	}

	public override ToolTip ToolTipContent(object sender)
	{
		GoodsRemainsTreeViewDataProvider.DataRow dataRow = (GoodsRemainsTreeViewDataProvider.DataRow)((Image)sender).Tag;
		if (dataRow == null)
		{
			return null;
		}
		int mTID = dataRow.MTID;
		if (dataRow.ImageExists && dataRow.Image == null)
		{
			dataRow.Image = Settings.ImagesService.GetItemImage(ImageItemTypeSE.Materials.Value, TradeSettings.TradeCacheService.Material.GetQntUnit(mTID, dataRow.UnitCode)?.ImageGUID.ToString());
		}
		if (dataRow.Image != null && dataRow.Image.Length != 0)
		{
			LayoutItemWithRequired layoutItemWithRequired = new LayoutItemWithRequired();
			layoutItemWithRequired.LabelPosition = LayoutItemLabelPosition.Top;
			layoutItemWithRequired.Label = dataRow.Caption;
			ImageEdit imageEdit = new ImageEdit();
			imageEdit.IsReadOnly = true;
			imageEdit.ShowLoadDialogOnClickMode = ShowLoadDialogOnClickMode.Never;
			imageEdit.EditMode = EditMode.InplaceInactive;
			imageEdit.EditValue = dataRow.Image;
			imageEdit.Width = 200.0;
			imageEdit.Height = 200.0;
			imageEdit.VerticalAlignment = VerticalAlignment.Stretch;
			imageEdit.HorizontalAlignment = HorizontalAlignment.Stretch;
			layoutItemWithRequired.Content = imageEdit;
			return new ToolTip
			{
				Content = layoutItemWithRequired
			};
		}
		return new ToolTip
		{
			Content = ArmSoft.SME.Common.Resources.NoImage
		};
	}
}
