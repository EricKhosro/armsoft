using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.ReportViewers.Goods;
using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10048)]
public class GoodsRetailTurnOver : TradeDataView<GoodsRetailTurnOverDataProvider.DataRow, GoodsRetailTurnOverDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly GoodsRetailTurnOver _dataView;

		private DateEditControl _startDate;

		private DateEditControl _endDate;

		private DropDownControl _subTotalType;

		private DropDownControl _goodGroup;

		private DropDownControl _good;

		private MultiSelectDropDownControl _storage;

		private CheckEditExt _showAltUnit;

		private CheckEditExt _showByPrices;

		private CheckEditExt _detailByStorages;

		private CheckEditExt _detailByOperations;

		private readonly GoodsFilter _goods;

		public DataViewDialog(GoodsRetailTurnOver dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = ArmSoft.SME.Trade.Common.Resources.MTRepRetailTurn;
			_goods = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: true);
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_endDate = AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			AddSpecialDateButton(container, _startDate, _endDate);
			DXTabControl tc = AddTabControl(null);
			LayoutGroup layoutGroup = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.General);
			_storage = AddMultiSelectDropDownControl(layoutGroup, "Storage", ArmSoft.SME.Common.Resources.Storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(true, true), isRequired: false, !useParamValues);
			_storage.SelectedString = (useParamValues ? dataView.Storage : string.Empty);
			_goodGroup = AddDropDownControl(layoutGroup, "Group", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_goodGroup.Code = (useParamValues ? dataView.GoodGroup : string.Empty);
			_goodGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GoodsGroup_EditValueChanged).Handler;
			_good = AddDropDownControl(layoutGroup, "GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, _goods, isRequired: false, allowAdditionButtons: false, !useParamValues);
			_good.Code = (useParamValues ? dataView.Good : string.Empty);
			AddLine(layoutGroup);
			_showAltUnit = AddBooleanControl(layoutGroup, "ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, !useParamValues);
			_showAltUnit.IsChecked = useParamValues && dataView.ShowAltUnit;
			AddLine(layoutGroup);
			_showByPrices = AddBooleanControl(layoutGroup, "ShowByPrices", ArmSoft.SME.Trade.Common.Resources.ByDifPrices, !useParamValues);
			_showByPrices.IsChecked = useParamValues && dataView.ShowByPrices;
			_detailByStorages = AddBooleanControl(layoutGroup, "DetailByStorage", ArmSoft.SME.Trade.Common.Resources.DetailByStorage, !useParamValues);
			_detailByStorages.IsChecked = useParamValues && dataView.DetailByStorage;
			_detailByOperations = AddBooleanControl(layoutGroup, "DetailByOper", ArmSoft.SME.Trade.Common.Resources.OperationDetail, !useParamValues);
			_detailByOperations.IsChecked = useParamValues && dataView.DetailByOper;
			AddLine(layoutGroup);
			_subTotalType = AddDropDownControl(layoutGroup, "SubTotalType", ArmSoft.SME.Common.Resources.ShowSubTotals, new SmartEnum(typeof(SubTotalTypesSE)), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_subTotalType.Code = (useParamValues ? dataView.SubTotalType : string.Empty);
			_subTotalType.EditValueChanged += SubTotalType_EditValueChanged;
			if (!isRefreshMode)
			{
				AddColumnsSelectionTab(tc, layoutGroup, null);
			}
		}

		private void GoodsGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue)
			{
				_goods.Group = (string)e.NewValue;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = _startDate.EditValue;
			_dataView.EndDate = _endDate.EditValue;
			_dataView.Storage = _storage.SelectedString;
			_dataView.GoodGroup = _goodGroup.Code;
			_dataView.Good = _good.Code;
			_dataView.ShowAltUnit = _showAltUnit.IsChecked.Value;
			_dataView.DetailByStorage = _detailByStorages.IsChecked.Value;
			_dataView.DetailByOper = _detailByOperations.IsChecked.Value;
			_dataView.SubTotalType = _subTotalType.Code;
			_dataView.ShowByPrices = _showByPrices.IsChecked.Value;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
				args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_endDate.EditValue.Value);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}

		private void SubTotalType_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(_subTotalType.Code) && _subTotalType.DoValidate())
			{
				if (_subTotalType.Code == "1")
				{
					_detailByStorages.IsChecked = true;
					_detailByStorages.IsReadOnly = true;
				}
				else
				{
					_detailByStorages.IsReadOnly = false;
				}
				_detailByOperations.IsChecked = false;
				_detailByOperations.IsReadOnly = true;
			}
			else
			{
				_detailByStorages.IsReadOnly = false;
				_detailByOperations.IsReadOnly = false;
			}
		}
	}

	private List<ViewColumn> potentiallyColumns;

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public override string HelpId => "Sales_Reports_TurnOverRetail";

	public override bool DialogSupported => true;

	public override string Caption => ArmSoft.SME.Trade.Common.Resources.MTRepRetailTurn + " " + StartDate.FormatDDMMYY() + " - " + EndDate.FormatDDMMYY();

	public override List<ViewColumn> PotentiallyColumns => potentiallyColumns;

	public DateOnly? StartDate
	{
		get
		{
			return (DateOnly?)Parameters["StartDate"].Value;
		}
		set
		{
			Parameters["StartDate"].Value = value;
		}
	}

	public DateOnly? EndDate
	{
		get
		{
			return (DateOnly?)Parameters["EndDate"].Value;
		}
		set
		{
			Parameters["EndDate"].Value = value;
		}
	}

	public string Storage
	{
		get
		{
			return Convert.ToString(Parameters["Storage"].Value);
		}
		set
		{
			Parameters["Storage"].Value = value;
		}
	}

	public string GoodGroup
	{
		get
		{
			return Convert.ToString(Parameters["Group"].Value);
		}
		set
		{
			Parameters["Group"].Value = value;
		}
	}

	public string Good
	{
		get
		{
			return Convert.ToString(Parameters["GoodCode"].Value);
		}
		set
		{
			Parameters["GoodCode"].Value = value;
		}
	}

	public bool ShowAltUnit
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowAltUnit"].Value);
		}
		set
		{
			Parameters["ShowAltUnit"].Value = value;
		}
	}

	public bool DetailByStorage
	{
		get
		{
			return Convert.ToBoolean(Parameters["DetailByStorage"].Value);
		}
		set
		{
			Parameters["DetailByStorage"].Value = value;
		}
	}

	public bool DetailByOper
	{
		get
		{
			return Convert.ToBoolean(Parameters["DetailByOper"].Value);
		}
		set
		{
			Parameters["DetailByOper"].Value = value;
		}
	}

	public string SubTotalType
	{
		get
		{
			return Convert.ToString(Parameters["SubTotalType"].Value);
		}
		set
		{
			Parameters["SubTotalType"].Value = value;
		}
	}

	public bool ShowByPrices
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowByPrices"].Value);
		}
		set
		{
			Parameters["ShowByPrices"].Value = value;
		}
	}

	public GoodsRetailTurnOver()
	{
		DataProvider = new GoodsRetailTurnOverDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("Storage", ArmSoft.SME.Common.Resources.Storage, "");
		AddParameter("Group", ArmSoft.SME.Common.Resources.Group, "");
		AddParameter("GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, "");
		AddParameter("ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, false);
		AddParameter("ShowByPrices", ArmSoft.SME.Trade.Common.Resources.ByDifPrices, false);
		AddParameter("DetailByStorage", ArmSoft.SME.Trade.Common.Resources.DetailByStorage, false);
		AddParameter("DetailByOper", ArmSoft.SME.Trade.Common.Resources.OperationDetail, false);
		AddParameter("SubTotalType", ArmSoft.SME.Common.Resources.ShowSubTotals, "");
		FillColumnsFromDataProvider();
		potentiallyColumns = new List<ViewColumn>();
		potentiallyColumns.AddRange(Columns.ViewList);
		potentiallyColumns.AddRange(MaterialCache.GetDataViewColumns(ArmSoft.SME.Trade.Common.Resources.MaterialValue, "", ""));
		potentiallyColumns.AddRange(ProducerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Producer, "", ""));
	}

	protected override void SetColumnsProperties()
	{
		base.SetColumnsProperties();
		Columns["Price"].IsSystem = true;
		Columns["QTYInIncome"].IsSystem = true;
		Columns["SumInIncome"].IsSystem = true;
		Columns["QTYInReturnFromCustomer"].IsSystem = true;
		Columns["SumInReturnFromCustomer"].IsSystem = true;
		Columns["QTYInMove"].IsSystem = true;
		Columns["SumInMove"].IsSystem = true;
		Columns["QTYInMTRetailRevaluation"].IsSystem = true;
		Columns["SumInMTRetailRevaluation"].IsSystem = true;
		Columns["QTYInMTRem"].IsSystem = true;
		Columns["SumInMTRem"].IsSystem = true;
		Columns["QTYOutSaleRetail"].IsSystem = true;
		Columns["SumOutSaleRetail"].IsSystem = true;
		Columns["QTYOutMTExpense"].IsSystem = true;
		Columns["SumOutMTExpense"].IsSystem = true;
		Columns["QTYOutMove"].IsSystem = true;
		Columns["SumOutMove"].IsSystem = true;
		Columns["QTYOutMTRetailRevaluation"].IsSystem = true;
		Columns["SumOutMTRetailRevaluation"].IsSystem = true;
		Columns["QtyIn"].IsSystem = true;
		Columns["SumIn"].IsSystem = true;
		Columns["QtyOut"].IsSystem = true;
		Columns["SumOut"].IsSystem = true;
		Columns["QTYInBillReturn"].IsSystem = true;
		Columns["SumInBillReturn"].IsSystem = true;
		Columns["GroupCodeCaption"].IsSystem = true;
		Columns["GoodID"].Visible = false;
		Columns["Unit"].Visible = false;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.RemoveGrouping();
		dvp.Set_ColumnVisible("QtyIn", !DetailByOper);
		dvp.Set_ColumnVisible("SumIn", !DetailByOper);
		dvp.Set_ColumnVisible("QtyOut", !DetailByOper);
		dvp.Set_ColumnVisible("SumOut", !DetailByOper);
		dvp.Set_ColumnVisible("Price", ShowByPrices);
		dvp.Set_ColumnVisible("GroupCodeCaption", value: false);
		dvp.Set_ColumnVisible("QTYInIncome", DetailByOper);
		dvp.Set_ColumnVisible("SumInIncome", DetailByOper);
		dvp.Set_ColumnVisible("QTYInReturnFromCustomer", DetailByOper);
		dvp.Set_ColumnVisible("SumInReturnFromCustomer", DetailByOper);
		dvp.Set_ColumnVisible("QTYInMove", DetailByOper);
		dvp.Set_ColumnVisible("SumInMove", DetailByOper);
		dvp.Set_ColumnVisible("QTYInMTRetailRevaluation", DetailByOper);
		dvp.Set_ColumnVisible("SumInMTRetailRevaluation", DetailByOper);
		dvp.Set_ColumnVisible("QTYInMTRem", DetailByOper);
		dvp.Set_ColumnVisible("SumInMTRem", DetailByOper);
		dvp.Set_ColumnVisible("QTYInBillReturn", DetailByOper);
		dvp.Set_ColumnVisible("SumInBillReturn", DetailByOper);
		dvp.Set_ColumnVisible("QTYOutSaleRetail", DetailByOper);
		dvp.Set_ColumnVisible("SumOutSaleRetail", DetailByOper);
		dvp.Set_ColumnVisible("QTYOutMTExpense", DetailByOper);
		dvp.Set_ColumnVisible("SumOutMTExpense", DetailByOper);
		dvp.Set_ColumnVisible("QTYOutMove", DetailByOper);
		dvp.Set_ColumnVisible("SumOutMove", DetailByOper);
		dvp.Set_ColumnVisible("QTYOutMTRetailRevaluation", DetailByOper);
		dvp.Set_ColumnVisible("SumOutMTRetailRevaluation", DetailByOper);
		dvp.Set_ColumnVisible("Storage", DetailByOper);
		dvp.Set_ColumnVisible("StorageCaption", DetailByOper);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("GoodCode");
			dvp.Set_ColumnSortOrder("Storage");
		}
		if (!string.IsNullOrWhiteSpace(SubTotalType))
		{
			dvp.Set_ColumnGroupSummary("QtyBegin", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumBegin", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("QtyIn", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumIn", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("QtyOut", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumOut", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("QtyEnd", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumEnd", SummaryItemType.Sum);
			switch (SubTotalType)
			{
			case "1":
				dvp.GroupByColumns("Storage");
				break;
			case "2":
				dvp.GroupByColumns("GroupCodeCaption");
				break;
			case "3":
				dvp.GroupByColumns("GoodCode");
				break;
			}
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowMTTAccount", ArmSoft.SME.Trade.Common.Resources.TAccount, ShowMTTAccount, "", "", storeOpContext: true);
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "PrintLabel", ArmSoft.SME.Trade.Common.Resources.PrintLabel, PrintLabel);
		}
	}

	private void PrintLabel()
	{
		List<Label.GoodLabelRow> list = new List<Label.GoodLabelRow>();
		Label.GoodLabelRow goodLabelRow = new Label.GoodLabelRow();
		foreach (GoodsRetailTurnOverDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailTurnOverDataProvider.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.GoodCode);
			if (item != null && item.Type == GoodsTypesSE.Good)
			{
				goodLabelRow = InitContextFunctionsHelper.BuildLabelGoodRow(item);
				list.Add(goodLabelRow);
			}
		}
		new PrintLabelDialog(null, null, list).Invoke();
	}

	private void ShowMTTAccount()
	{
		List<GoodsRetailTurnOverDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailTurnOverDataProvider.DataRow>();
		MTAccountReportViewer mTAccountReportViewer;
		if (selectedOrFocusedRows.Count == 1)
		{
			mTAccountReportViewer = new MTAccountReportViewer();
			mTAccountReportViewer.GoodCode = selectedOrFocusedRows[0].GoodCode;
			if (DetailByStorage)
			{
				mTAccountReportViewer.Storage = selectedOrFocusedRows[0].Storage;
			}
		}
		else
		{
			List<ListOfSelectedMaterialStorage> list = new List<ListOfSelectedMaterialStorage>();
			foreach (GoodsRetailTurnOverDataProvider.DataRow item in selectedOrFocusedRows)
			{
				ListOfSelectedMaterialStorage listOfSelectedMaterialStorage = new ListOfSelectedMaterialStorage();
				listOfSelectedMaterialStorage.GoodCode = item.GoodCode;
				if (DetailByStorage)
				{
					listOfSelectedMaterialStorage.Storage = item.Storage;
				}
				list.Add(listOfSelectedMaterialStorage);
			}
			mTAccountReportViewer = new MTAccountReportViewer(isForSelectedRows: true, list);
		}
		mTAccountReportViewer.ShowFromTurns = true;
		mTAccountReportViewer.StartDate = StartDate.Value;
		mTAccountReportViewer.EndDate = EndDate.Value;
		mTAccountReportViewer.ShowAltUnit = ShowAltUnit;
		mTAccountReportViewer.Show(useParamValues: true);
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		GoodsRetailTurnOverDataProvider.DataRow dataRow = row as GoodsRetailTurnOverDataProvider.DataRow;
		dataRow.MaterialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(dataRow.GoodCode);
		dataRow.MaterialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(dataRow.GoodCode);
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

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate.Value;
		ParametersProvider.EndDate = EndDate.Value;
		ParametersProvider.Storage = Storage;
		ParametersProvider.Group = GoodGroup;
		ParametersProvider.GoodCode = Good;
		ParametersProvider.ShowAltUnit = ShowAltUnit;
		ParametersProvider.DetailByStorage = DetailByStorage;
		ParametersProvider.DetailByOper = DetailByOper;
		ParametersProvider.ShowByPrices = ShowByPrices;
	}
}
