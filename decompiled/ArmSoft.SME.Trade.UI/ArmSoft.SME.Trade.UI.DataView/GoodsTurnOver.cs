using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
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

[DataView(10041)]
public class GoodsTurnOver : TradeDataView<GoodsTurnOverDataProvider.DataRow, GoodsTurnOverDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly GoodsTurnOver _dataView;

		private DateEditControl _startDate;

		private DateEditControl _endDate;

		private DropDownControl _subTotalType;

		private DropDownControl _goodGroup;

		private DropDownControl _good;

		private MultiSelectDropDownControl storage;

		private CheckEditExt showAltUnit;

		private CheckEditExt showSumms;

		private CheckEditExt showSummsWithVAT;

		private CheckEditExt showSummsWithCurr;

		private CheckEditExt showParties;

		private CheckEditExt showLinks;

		private CheckEditExt showZeroContainRows;

		private CheckEditExt detailByStorages;

		private CheckEditExt detailByOperations;

		private readonly GoodsFilter goods;

		public DataViewDialog(GoodsTurnOver dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = ArmSoft.SME.Trade.Common.Resources.MatRepTurnOver;
			bool flag = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
			goods = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: true);
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
			string caption = ArmSoft.SME.Common.Resources.Storage;
			bool? checkViewOrOperationAccess = true;
			storage = AddMultiSelectDropDownControl(layoutGroup, "Storage", caption, new ArmSoft.SME.Trade.UI.DropDown.Storages(null, checkViewOrOperationAccess), isRequired: false, !useParamValues);
			storage.SelectedString = (useParamValues ? dataView.Storage : string.Empty);
			_goodGroup = AddDropDownControl(layoutGroup, "Group", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_goodGroup.Code = (useParamValues ? dataView.GoodGroup : string.Empty);
			_goodGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GoodsGroup_EditValueChanged).Handler;
			_good = AddDropDownControl(layoutGroup, "GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, goods, isRequired: false, allowAdditionButtons: false, !useParamValues);
			_good.Code = (useParamValues ? dataView.Good : string.Empty);
			AddLine(layoutGroup);
			showAltUnit = AddBooleanControl(layoutGroup, "ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, !useParamValues);
			showAltUnit.IsChecked = useParamValues && dataView.ShowAltUnit;
			showSumms = AddBooleanControl(layoutGroup, "ShowSumms", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
			showSumms.IsChecked = Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && useParamValues && dataView.ShowSumms;
			showSumms.IsReadOnly = !flag;
			showSummsWithVAT = AddBooleanControl(layoutGroup, "ShowSummsWithVAT", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
			showSummsWithVAT.IsChecked = Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && useParamValues && dataView.ShowSummsWithVAT;
			showSummsWithVAT.IsReadOnly = !flag;
			showSummsWithCurr = AddBooleanControl(layoutGroup, "ShowCurrSumms", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, Settings.ParametersService.MTCurr()), Settings.ParametersService.ShowGoodsCostInReportsAndDocs());
			showSummsWithCurr.IsChecked = Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && useParamValues && dataView.ShowCurrSumms;
			showSummsWithCurr.IsReadOnly = !flag;
			showParties = AddBooleanControl(layoutGroup, "ShowParties", ArmSoft.SME.Common.Resources.ByDiffParties, !useParamValues);
			showParties.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowByDifferentParties_EditValueChanged).Handler;
			AddLine(layoutGroup);
			detailByStorages = AddBooleanControl(layoutGroup, "DetailByStorage", ArmSoft.SME.Trade.Common.Resources.DetailByStorage, !useParamValues);
			detailByStorages.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DetailByStorage_EditValueChanged).Handler;
			detailByStorages.IsChecked = useParamValues && dataView.DetailByStorage;
			detailByOperations = AddBooleanControl(layoutGroup, "DetailByOper", ArmSoft.SME.Trade.Common.Resources.OperationDetail, !useParamValues);
			detailByOperations.IsChecked = useParamValues && dataView.DetailByOper;
			showParties.IsChecked = useParamValues && dataView.ShowParties;
			AddLine(layoutGroup);
			showLinks = AddBooleanControl(layoutGroup, "ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, !useParamValues);
			showLinks.IsChecked = useParamValues && dataView.ShowLinks;
			_subTotalType = AddDropDownControl(layoutGroup, "SubTotalType", ArmSoft.SME.Common.Resources.ShowSubTotals, new SmartEnum(typeof(SubTotalTypesSE)), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_subTotalType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SubTotalType_EditValueChanged).Handler;
			_subTotalType.Code = (useParamValues ? dataView.SubTotalType : string.Empty);
			AddLine(layoutGroup);
			showZeroContainRows = AddBooleanControl(layoutGroup, "ShowAlsoZeroRows", ArmSoft.SME.Trade.Common.Resources.ShowAlsoZeroRows, !useParamValues);
			showZeroContainRows.IsChecked = useParamValues && dataView.ShowAlsoZeroRows;
			if (!isRefreshMode)
			{
				AddColumnsSelectionTab(tc, layoutGroup, null);
			}
		}

		private void GoodsGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue)
			{
				goods.Group = (string)e.NewValue;
			}
		}

		private void ShowByDifferentParties_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (showParties.IsChecked.HasValue && showParties.IsChecked.Value)
			{
				detailByStorages.IsChecked = true;
			}
		}

		private void DetailByStorage_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (showParties.IsChecked.HasValue && !detailByStorages.IsChecked.Value)
			{
				showParties.IsChecked = false;
			}
		}

		private void SubTotalType_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(_subTotalType.Code) && _subTotalType.DoValidate())
			{
				if (_subTotalType.Code == "1")
				{
					detailByStorages.IsChecked = true;
					detailByStorages.IsReadOnly = true;
				}
				else
				{
					detailByStorages.IsReadOnly = false;
				}
				detailByOperations.IsChecked = false;
				detailByOperations.IsReadOnly = true;
			}
			else
			{
				detailByStorages.IsReadOnly = false;
				detailByOperations.IsReadOnly = false;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = _startDate.EditValue;
			_dataView.EndDate = _endDate.EditValue;
			_dataView.Storage = storage.SelectedString;
			_dataView.GoodGroup = _goodGroup.Code;
			_dataView.Good = _good.Code;
			_dataView.ShowParties = showParties.IsChecked.Value;
			_dataView.ShowAltUnit = showAltUnit.IsChecked.Value;
			_dataView.ShowSumms = showSumms.IsChecked.Value;
			_dataView.ShowSummsWithVAT = showSummsWithVAT.IsChecked.Value;
			_dataView.ShowCurrSumms = showSummsWithCurr.IsChecked.Value;
			_dataView.DetailByStorage = detailByStorages.IsChecked.Value;
			_dataView.DetailByOper = detailByOperations.IsChecked.Value;
			_dataView.SubTotalType = _subTotalType.Code;
			_dataView.ShowAlsoZeroRows = showZeroContainRows.IsChecked.Value;
			_dataView.ShowLinks = showLinks.IsChecked.Value;
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
	}

	private List<ViewColumn> potentiallyColumns;

	public override List<ViewColumn> PotentiallyColumns => potentiallyColumns;

	public override string HelpId => "Purchases_Reports_TurnOver";

	public override bool DialogSupported => true;

	public override string Caption => ArmSoft.SME.Trade.Common.Resources.MatRepTurnOver + " " + StartDate.FormatDDMMYY() + " - " + EndDate.FormatDDMMYY();

	public override LoadIndication LoadingIndication => LoadIndication.AlwaysInside;

	public override bool AllowEditSelected => Settings.UserAccessService.GetDirectoryPermission(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value).AllowEdit;

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

	public bool ShowSumms
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowSumms"].Value);
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
			return Convert.ToBoolean(Parameters["ShowSummsWithVAT"].Value);
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
			return Convert.ToBoolean(Parameters["ShowCurrSumms"].Value);
		}
		set
		{
			Parameters["ShowCurrSumms"].Value = value;
		}
	}

	public bool ShowParties
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowParties"].Value);
		}
		set
		{
			Parameters["ShowParties"].Value = value;
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

	public bool ShowAlsoZeroRows
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowAlsoZeroRows"].Value);
		}
		set
		{
			Parameters["ShowAlsoZeroRows"].Value = value;
		}
	}

	public bool ShowLinks
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowLinks"].Value);
		}
		set
		{
			Parameters["ShowLinks"].Value = value;
		}
	}

	public GoodsTurnOver()
	{
		DataProvider = new GoodsTurnOverDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("Storage", ArmSoft.SME.Common.Resources.Storage, "");
		AddParameter("Group", ArmSoft.SME.Common.Resources.Group, "");
		AddParameter("GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, "");
		AddParameter("ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, false);
		AddParameter("ShowSumms", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, false);
		AddParameter("ShowSummsWithVAT", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, false);
		AddParameter("ShowCurrSumms", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, Settings.ParametersService.MTCurr()), false);
		AddParameter("ShowParties", ArmSoft.SME.Common.Resources.ByDiffParties, false);
		AddParameter("ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, true);
		AddParameter("DetailByStorage", ArmSoft.SME.Trade.Common.Resources.DetailByStorage, false);
		AddParameter("DetailByOper", ArmSoft.SME.Trade.Common.Resources.OperationDetail, false);
		AddParameter("SubTotalType", ArmSoft.SME.Common.Resources.ShowSubTotals, "");
		AddParameter("ShowAlsoZeroRows", ArmSoft.SME.Trade.Common.Resources.ShowAlsoZeroRows, false);
		FillColumnsFromDataProvider();
		potentiallyColumns = new List<ViewColumn>();
		potentiallyColumns.AddRange(Columns.ViewList);
		potentiallyColumns.AddRange(MaterialCache.GetDataViewColumns(ArmSoft.SME.Trade.Common.Resources.MaterialValue, "", ""));
		potentiallyColumns.AddRange(ProducerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Producer, "", ""));
	}

	protected override void SetColumnsProperties()
	{
		Columns["SumBegin"].IsSystem = true;
		Columns["SumVATBegin"].IsSystem = true;
		Columns["SumCurrBegin"].IsSystem = true;
		Columns["QTYInIncome"].IsSystem = true;
		Columns["SumInIncome"].IsSystem = true;
		Columns["SumVatInIncome"].IsSystem = true;
		Columns["SumCurrInIncome"].IsSystem = true;
		Columns["QTYInReval"].IsSystem = true;
		Columns["SumInReval"].IsSystem = true;
		Columns["SumVatInReval"].IsSystem = true;
		Columns["SumCurrInReval"].IsSystem = true;
		Columns["SumInOverheadExp"].IsSystem = true;
		Columns["SumVatInOverheadExp"].IsSystem = true;
		Columns["SumCurrInOverheadExp"].IsSystem = true;
		Columns["QTYInReturnFromCustomer"].IsSystem = true;
		Columns["SumInReturnFromCustomer"].IsSystem = true;
		Columns["SumVatInReturnFromCustomer"].IsSystem = true;
		Columns["SumCurrInReturnFromCustomer"].IsSystem = true;
		Columns["QTYInMove"].IsSystem = true;
		Columns["SumInMove"].IsSystem = true;
		Columns["SumVatInMove"].IsSystem = true;
		Columns["SumCurrInMove"].IsSystem = true;
		Columns["QTYInComplectation"].IsSystem = true;
		Columns["SumInComplectation"].IsSystem = true;
		Columns["SumVatInComplectation"].IsSystem = true;
		Columns["SumCurrInComplectation"].IsSystem = true;
		Columns["QTYInDeComplectation"].IsSystem = true;
		Columns["SumInDeComplectation"].IsSystem = true;
		Columns["SumVatInDeComplectation"].IsSystem = true;
		Columns["SumCurrInDeComplectation"].IsSystem = true;
		Columns["QTYInIncomeRem"].IsSystem = true;
		Columns["SumInIncomeRem"].IsSystem = true;
		Columns["SumVatInIncomeRem"].IsSystem = true;
		Columns["SumCurrInIncomeRem"].IsSystem = true;
		Columns["QTYOutSale"].IsSystem = true;
		Columns["SumOutSale"].IsSystem = true;
		Columns["SumVatOutSale"].IsSystem = true;
		Columns["SumCurrOutSale"].IsSystem = true;
		Columns["QTYOutSaleRetail"].IsSystem = true;
		Columns["SumOutSaleRetail"].IsSystem = true;
		Columns["SumVatOutSaleRetail"].IsSystem = true;
		Columns["SumCurrOutSaleRetail"].IsSystem = true;
		Columns["QTYOutReturnToSupplier"].IsSystem = true;
		Columns["SumOutReturnToSupplier"].IsSystem = true;
		Columns["SumVatOutReturnToSupplier"].IsSystem = true;
		Columns["SumCurrOutReturnToSupplier"].IsSystem = true;
		Columns["QTYOutExpense"].IsSystem = true;
		Columns["SumOutExpense"].IsSystem = true;
		Columns["SumVatOutExpense"].IsSystem = true;
		Columns["SumCurrOutExpense"].IsSystem = true;
		Columns["QTYOutMove"].IsSystem = true;
		Columns["SumOutMove"].IsSystem = true;
		Columns["SumVatOutMove"].IsSystem = true;
		Columns["SumCurrOutMove"].IsSystem = true;
		Columns["QTYOutComplectation"].IsSystem = true;
		Columns["SumOutComplectation"].IsSystem = true;
		Columns["SumVatOutComplectation"].IsSystem = true;
		Columns["SumCurrOutComplectation"].IsSystem = true;
		Columns["QTYOutDeComplectation"].IsSystem = true;
		Columns["SumOutDeComplectation"].IsSystem = true;
		Columns["SumVatOutDeComplectation"].IsSystem = true;
		Columns["SumCurrOutDeComplectation"].IsSystem = true;
		Columns["QTYOutReval"].IsSystem = true;
		Columns["SumOutReval"].IsSystem = true;
		Columns["SumVatOutReval"].IsSystem = true;
		Columns["SumCurrOutReval"].IsSystem = true;
		Columns["QtyIn"].IsSystem = true;
		Columns["SumIn"].IsSystem = true;
		Columns["SumVATIn"].IsSystem = true;
		Columns["SumCurrIn"].IsSystem = true;
		Columns["QtyOut"].IsSystem = true;
		Columns["SumOut"].IsSystem = true;
		Columns["SumVATOut"].IsSystem = true;
		Columns["SumCurrOut"].IsSystem = true;
		Columns["SumEnd"].IsSystem = true;
		Columns["SumVATEnd"].IsSystem = true;
		Columns["SumCurrEnd"].IsSystem = true;
		Columns["IncomeDate"].IsSystem = true;
		Columns["PartyPartCaption"].IsSystem = true;
		Columns["PartyPartTaxCode"].IsSystem = true;
		Columns["PartyPartContract"].IsSystem = true;
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
		Columns["GoodID"].Visible = false;
		Columns["SaleSumBegin"].Visible = false;
		Columns["SaleSumCurrBegin"].Visible = false;
		Columns["SaleSumVATBegin"].Visible = false;
		Columns["PartyPartID"].Visible = false;
		Columns["PartyBase"].Visible = false;
		Columns["PartyRow"].Visible = false;
		Columns["Unit"].Visible = false;
	}

	public override string GetDefaultFunction()
	{
		return "ShowMTTAccount";
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.RemoveGrouping();
		dvp.Set_ColumnVisible("QtyIn", !DetailByOper);
		dvp.Set_ColumnVisible("SumIn", !DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVATIn", !DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrIn", !DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QtyOut", !DetailByOper);
		dvp.Set_ColumnVisible("SumOut", !DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVATOut", !DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrOut", !DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("SumBegin", ShowSumms);
		dvp.Set_ColumnVisible("SumVATBegin", ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrBegin", ShowCurrSumms);
		dvp.Set_ColumnVisible("GroupCodeCaption", value: false);
		dvp.Set_ColumnVisible("QTYInIncome", DetailByOper);
		dvp.Set_ColumnVisible("SumInIncome", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatInIncome", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrInIncome", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYInReval", DetailByOper);
		dvp.Set_ColumnVisible("SumInReval", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatInReval", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrInReval", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("SumInOverheadExp", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatInOverheadExp", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrInOverheadExp", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYInReturnFromCustomer", DetailByOper);
		dvp.Set_ColumnVisible("SumInReturnFromCustomer", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatInReturnFromCustomer", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrInReturnFromCustomer", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYInMove", DetailByOper);
		dvp.Set_ColumnVisible("SumInMove", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatInMove", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrInMove", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYInComplectation", DetailByOper);
		dvp.Set_ColumnVisible("SumInComplectation", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatInComplectation", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrInComplectation", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYInDeComplectation", DetailByOper);
		dvp.Set_ColumnVisible("SumInDeComplectation", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatInDeComplectation", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrInDeComplectation", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYInIncomeRem", DetailByOper);
		dvp.Set_ColumnVisible("SumInIncomeRem", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatInIncomeRem", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrInIncomeRem", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYOutSale", DetailByOper);
		dvp.Set_ColumnVisible("SumOutSale", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatOutSale", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrOutSale", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYOutSaleRetail", DetailByOper);
		dvp.Set_ColumnVisible("SumOutSaleRetail", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatOutSaleRetail", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrOutSaleRetail", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYOutReturnToSupplier", DetailByOper);
		dvp.Set_ColumnVisible("SumOutReturnToSupplier", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatOutReturnToSupplier", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrOutReturnToSupplier", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYOutExpense", DetailByOper);
		dvp.Set_ColumnVisible("SumOutExpense", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatOutExpense", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrOutExpense", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYOutMove", DetailByOper);
		dvp.Set_ColumnVisible("SumOutMove", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatOutMove", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrOutMove", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYOutComplectation", DetailByOper);
		dvp.Set_ColumnVisible("SumOutComplectation", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatOutComplectation", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrOutComplectation", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYOutDeComplectation", DetailByOper);
		dvp.Set_ColumnVisible("SumOutDeComplectation", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatOutDeComplectation", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrOutDeComplectation", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("QTYOutReval", DetailByOper);
		dvp.Set_ColumnVisible("SumOutReval", DetailByOper && ShowSumms);
		dvp.Set_ColumnVisible("SumVatOutReval", DetailByOper && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrOutReval", DetailByOper && ShowCurrSumms);
		dvp.Set_ColumnVisible("SumEnd", ShowSumms);
		dvp.Set_ColumnVisible("SumVATEnd", ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SumCurrEnd", ShowCurrSumms);
		dvp.Set_ColumnVisible("IncomeDate", ShowParties && Settings.ParametersService.ShowPartyIncomeDate());
		dvp.Set_ColumnVisible("PartyPartCaption", ShowParties && Settings.ParametersService.ShowPartySUPPLIER());
		dvp.Set_ColumnVisible("PartyPartTaxCode", ShowParties && Settings.ParametersService.ShowPartnerTaxCode());
		dvp.Set_ColumnVisible("PartyPartCaption", ShowParties && Settings.ParametersService.ShowPartySUPPLIER());
		dvp.Set_ColumnVisible("PartyPartContract", ShowParties && Settings.ParametersService.ShowPartyCONTRACT());
		dvp.Set_ColumnVisible("PartyDocType", ShowParties && Settings.ParametersService.ShowPartyDocType());
		dvp.Set_ColumnVisible("PartyDocNum", ShowParties && Settings.ParametersService.ShowPartyDocNum());
		dvp.Set_ColumnVisible("PartyDocUsableDate", ShowParties && Settings.ParametersService.ShowPartyUSEABLEDATE());
		dvp.Set_ColumnVisible("PartyDocCountry", ShowParties && Settings.ParametersService.ShowPartyCountry());
		dvp.Set_ColumnVisible("PartyDocProducer", ShowParties && Settings.ParametersService.ShowPartyProducer());
		dvp.Set_ColumnVisible("PartyDocPropertyType1", ShowParties && Settings.ParametersService.ShowPartyProperty1());
		dvp.Set_ColumnVisible("PartyDocPropertyValue1", ShowParties && Settings.ParametersService.ShowPartyProperty1());
		dvp.Set_ColumnVisible("PartyDocPropertyType2", ShowParties && Settings.ParametersService.ShowPartyProperty2());
		dvp.Set_ColumnVisible("PartyDocPropertyValue2", ShowParties && Settings.ParametersService.ShowPartyProperty2());
		dvp.Set_ColumnVisible("Storage", DetailByStorage);
		dvp.Set_ColumnVisible("StorageCaption", DetailByStorage);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("GoodCode");
			dvp.Set_ColumnSortOrder("Storage");
		}
		if (!string.IsNullOrWhiteSpace(SubTotalType))
		{
			dvp.Set_ColumnGroupSummary("QtyBegin", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumBegin", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumVATBegin", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumCurrBegin", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("QtyIn", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumIn", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumVATIn", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumCurrIn", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("QtyOut", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumOut", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumVATOut", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumCurrOut", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("QtyEnd", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumEnd", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumVATEnd", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SumCurrEnd", SummaryItemType.Sum);
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
		Columns.AddDirectoryLinkColumn(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods, "GoodCode", "Caption", "", ArmSoft.SME.Common.Resources.Product, ShowLinks);
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		GoodsTurnOverDataProvider.DataRow dataRow = row as GoodsTurnOverDataProvider.DataRow;
		dataRow.MaterialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(dataRow.GoodCode);
		dataRow.ProducerCacheRow = TradeSettings.TradeCacheService.Producer.GetItem(dataRow.MaterialCacheRow?.Producer);
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepTAccount).AllowView)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowMTTAccount", ArmSoft.SME.Trade.Common.Resources.TAccount, ShowMTTAccount, "", "", storeOpContext: true);
			}
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "CreateDoc", ArmSoft.SME.Common.Resources.CreateDoc, CreateDoc, "doc_new_16.svg");
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsBySegment).AllowView)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ChangeGoodsBySegments", ArmSoft.SME.Trade.Common.Resources.AddToSegment, ChangeGoodsBySegments);
			}
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "PrintLabel", ArmSoft.SME.Trade.Common.Resources.PrintLabel, PrintLabel);
		}
	}

	private void PrintLabel()
	{
		List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow> list = new List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow>();
		ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow goodLabelRow = new ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow();
		foreach (GoodsTurnOverDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsTurnOverDataProvider.DataRow>())
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

	private void CreateDoc()
	{
		try
		{
			if (!ShowParties && !DetailByStorage && (!Settings.UserAccessService.GetDocumentPermission(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value).AllowView || !Settings.UserAccessService.GetDocumentPermission(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveOrder.Value).AllowView))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidCreateDocFromGoodsTurnOver);
			}
			ContextMenu contextMenu = new ContextMenu();
			contextMenu.FontFamily = Settings.FontFamily;
			contextMenu.FontSize = Settings.GetFontSize();
			if (ShowParties || DetailByStorage)
			{
				AddAccesDocTypeInContextMenu(contextMenu);
			}
			else
			{
				MenuItem menuItem = new MenuItem
				{
					Header = ArmSoft.SME.Common.Resources.DocMTPurchaseOrder,
					Tag = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value
				};
				menuItem.Click += CreateNewDocuments;
				contextMenu.Items.Add(menuItem);
				menuItem = new MenuItem
				{
					Header = ArmSoft.SME.Common.Resources.DocMTPurchaseOrder + " " + ArmSoft.SME.Trade.Common.Resources.BySuppliers,
					Tag = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value
				};
				menuItem.Click += CreateNewMTPurchaseOrdersBySuppliersDoc;
				contextMenu.Items.Add(menuItem);
				menuItem = new MenuItem
				{
					Header = ArmSoft.SME.Trade.Common.Resources.MTMoveOrder,
					Tag = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveOrder.Value
				};
				menuItem.Click += CreateNewDocuments;
				contextMenu.Items.Add(menuItem);
			}
			contextMenu.ToolTip = ArmSoft.SME.Common.Resources.NewDocuments;
			contextMenu.PlacementTarget = Settings.HostingEnvironment.LayoutManager;
			contextMenu.Placement = PlacementMode.Center;
			contextMenu.VerticalOffset = 0.0;
			contextMenu.HorizontalOffset = 0.0;
			contextMenu.IsOpen = contextMenu.Items.Count > 0;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ChangeGoodsBySegments()
	{
		InitContextFunctionsHelper.ChangeGoodsBySegments((from p in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsTurnOverDataProvider.DataRow>()
			select p.GoodCode).ToList());
	}

	private void AddAccesDocTypeInContextMenu(ContextMenu contextMenu)
	{
		foreach (SmartEnumDataProvider.DataRow docType in Settings.DocumentService.GetDocTypes((from x in GetCanCreateDocumentTypes()
			select x.Value).ToHashSet()))
		{
			MenuItem menuItem = new MenuItem
			{
				Header = docType.Caption,
				Tag = docType.Code
			};
			menuItem.Click += CreateNewDocuments;
			contextMenu.Items.Add(menuItem);
			if (docType.Code.Equals(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value.ToString()))
			{
				menuItem = new MenuItem
				{
					Header = docType.Caption + " " + ArmSoft.SME.Trade.Common.Resources.BySuppliers,
					Tag = docType.Code
				};
				menuItem.Click += CreateNewMTPurchaseOrdersBySuppliersDoc;
				contextMenu.Items.Add(menuItem);
			}
		}
	}

	private List<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE> GetCanCreateDocumentTypes()
	{
		int num = 13;
		List<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE> list = new List<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE> span = CollectionsMarshal.AsSpan(list);
		span[0] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome;
		span[1] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier;
		span[2] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice;
		span[3] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill;
		span[4] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail;
		span[5] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense;
		span[6] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove;
		span[7] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation;
		span[8] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveOrder;
		span[9] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTaking;
		span[10] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTakingRetail;
		span[11] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder;
		span[12] = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay;
		List<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE> list2 = list;
		if (!Settings.ParametersService.MTMoveWithConfirmation())
		{
			list2.Remove(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation);
		}
		if (ShowParties)
		{
			list2.Remove(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTaking);
			list2.Remove(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTakingRetail);
		}
		return list2;
	}

	private void CreateNewDocuments(object sender, RoutedEventArgs e)
	{
		try
		{
			if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
			{
				ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE documentTypeSE = SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>.FromValue(Convert.ToInt16(((MenuItem)sender).Tag));
				CreateDocument(InitContextFunctionsHelper.FillAllAvailabilityAvailabilityOrTurnOver(documentTypeSE), documentTypeSE);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateDocument(bool fillAvailQty, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE docType)
	{
		switch (docType)
		{
		case 7:
			InitContextFunctionsHelper.CreateNewMTIncomeDoc(EndDate.Value, isFromTurnOver: true);
			break;
		case 14:
			InitContextFunctionsHelper.CreateNewMTReturnToSupplierDoc(fillAvailQty, EndDate.Value, ShowParties, isFromTurnOver: true);
			break;
		case 8:
			InitContextFunctionsHelper.CreateNewMTInvoiceDoc(fillAvailQty, EndDate.Value, ShowParties, isFromTurnOver: true);
			break;
		case 18:
			InitContextFunctionsHelper.CreateNewMTBillDoc(fillAvailQty, EndDate.Value, ShowParties, isFromTurnOver: true);
			break;
		case 21:
			InitContextFunctionsHelper.CreateNewMTInvoiceRetailDoc(fillAvailQty, EndDate.Value, ShowParties, isFromTurnOver: true);
			break;
		case 9:
			InitContextFunctionsHelper.CreateNewMTExpenseDoc(fillAvailQty, EndDate.Value, ShowParties, isFromTurnOver: true);
			break;
		case 10:
			InitContextFunctionsHelper.CreateNewMTMoveDoc(fillAvailQty, EndDate.Value, ShowParties, isFromTurnOver: true);
			break;
		case 39:
			InitContextFunctionsHelper.CreateNewMTMoveWithConfirmationDoc(fillAvailQty, EndDate.Value, ShowParties, isFromTurnOver: true);
			break;
		case 36:
			InitContextFunctionsHelper.CreateNewMTMoveOrderDoc(fillAvailQty, EndDate.Value, ShowParties, isFromTurnOver: true, "", DetailByOper);
			break;
		case 16:
			InitContextFunctionsHelper.CreateNewMTStockTakingDoc(fillAvailQty, EndDate.Value, isFromTurnOver: true);
			break;
		case 22:
			InitContextFunctionsHelper.CreateNewMTStockTakingRetailDoc(fillAvailQty, EndDate.Value, isFromTurnOver: true);
			break;
		case 32:
			CreateNewMTPurchaseOrderDoc();
			break;
		case 33:
			InitContextFunctionsHelper.CreateNewInvoiceForPayDoc(fillAvailQty, isFromTurnOver: true);
			break;
		}
	}

	private void CreateNewMTPurchaseOrdersBySuppliersDoc(object sender, RoutedEventArgs e)
	{
		try
		{
			bool flag = ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OpenPOJournal, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes;
			CreateMTPurchaseOrdersBySuppliers();
			if (flag)
			{
				PurchaseOrdersDocumentsJournalDataView purchaseOrdersDocumentsJournalDataView = new PurchaseOrdersDocumentsJournalDataView();
				purchaseOrdersDocumentsJournalDataView.StartDate = Settings.HostingEnvironment.CurrentDate;
				purchaseOrdersDocumentsJournalDataView.EndDate = Settings.HostingEnvironment.CurrentDate;
				purchaseOrdersDocumentsJournalDataView.DocOwner = Settings.CoreCacheService.User.GetNameById(Settings.LoginService.UserId);
				purchaseOrdersDocumentsJournalDataView.Show(useParamValues: true, openAsChild: false);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateMTPurchaseOrdersBySuppliers()
	{
		Dictionary<string, MTPurchaseOrder> dictionary = new Dictionary<string, MTPurchaseOrder>();
		MTPurchaseOrder mTPurchaseOrder = null;
		string text = "";
		string text2 = Settings.ParametersService.MTPurchaseOrderNumber();
		foreach (GoodsTurnOverDataProvider.DataRow row in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsTurnOverDataProvider.DataRow>())
		{
			if (!isCreateDoc(row))
			{
				continue;
			}
			IReadOnlyList<PartnerGoodCacheRow> list = TradeSettings.TradeCacheService.PartnerGood.GetList((PartnerGoodCacheRow p) => p.MTID == row.GoodID);
			if (list.Count > 0)
			{
				text = TradeSettings.TradeCacheService.Partner.GetItemById(list[0].PartID)?.PartCode;
			}
			if (dictionary.TryGetValue(text, out var value))
			{
				mTPurchaseOrder = value;
				AddCurrentRowToMTPurchaseOrderList(mTPurchaseOrder, row);
				continue;
			}
			mTPurchaseOrder = Settings.DocumentService.Create<MTPurchaseOrder>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder.Value);
			mTPurchaseOrder.Owner = Settings.LoginService.UserName;
			mTPurchaseOrder.OwnerID = Settings.LoginService.UserId;
			if (string.IsNullOrWhiteSpace(text))
			{
				mTPurchaseOrder.POStates = POStatesSE.EmptySupplier;
			}
			mTPurchaseOrder.DocDate = Settings.HostingEnvironment.CurrentDate;
			mTPurchaseOrder.DocNum = text2;
			text2 = ArmSoft.SME.Common.Extensions.StringExtensions.NextDocNum(mTPurchaseOrder.DocNum, 12);
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text);
			if (item != null)
			{
				mTPurchaseOrder.PartnerContractGroup.PartnerCode = item.PartCode;
				mTPurchaseOrder.PartnerName = item.Caption;
				mTPurchaseOrder.SupplierEMail = item.Email;
			}
			AddCurrentRowToMTPurchaseOrderList(mTPurchaseOrder, row);
			dictionary.Add(text, mTPurchaseOrder);
		}
		foreach (KeyValuePair<string, MTPurchaseOrder> item2 in dictionary)
		{
			mTPurchaseOrder = item2.Value;
			if (mTPurchaseOrder.MTList.Count > 0)
			{
				Settings.DocumentService.Store(mTPurchaseOrder, new StoreRequest
				{
					LogComment = ArmSoft.SME.Common.Resources.AutoCreatePurchase
				});
			}
		}
		Settings.ParametersService.MTPurchaseOrderNumber_set(text2);
		bool isCreateDoc(GoodsTurnOverDataProvider.DataRow dataRow)
		{
			if (dataRow != null)
			{
				if (!DetailByOper || !(dataRow.QTYOutSale + dataRow.QTYOutSaleRetail - dataRow.QTYInReturnFromCustomer > 0m))
				{
					if (!DetailByOper)
					{
						return dataRow.QtyOut > 0m;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	private void AddCurrentRowToMTPurchaseOrderList(MTPurchaseOrder doc, GoodsTurnOverDataProvider.DataRow sourceRow)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(sourceRow.GoodCode);
		if (item != null)
		{
			doc.MTList.Add(new MTPurchaseOrder.MTPurchaseOrderSpecificationRow
			{
				RowID = doc.MaxRowId,
				MTCode = item.MTCode,
				MTName = item.Caption,
				MTID = item.Id,
				UnitCode = sourceRow.Unit,
				Quantity = (DetailByOper ? (sourceRow.QTYOutSale + sourceRow.QTYOutSaleRetail - sourceRow.QTYInReturnFromCustomer) : sourceRow.QtyOut),
				UnitBrief = sourceRow.UnitBrief
			});
			doc.MaxRowId++;
		}
	}

	private void CreateNewMTPurchaseOrderDoc()
	{
		MTPurchaseOrder mTPurchaseOrder = null;
		if (!Settings.HostingEnvironment.CurrentViewPanel.GetSelectedAny())
		{
			GoodsTurnOverDataProvider.DataRow sourceRow = (GoodsTurnOverDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			mTPurchaseOrder = Settings.DocumentService.Create<MTPurchaseOrder>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder);
			mTPurchaseOrder.DocDate = Settings.HostingEnvironment.CurrentDate;
			mTPurchaseOrder.DocNum = Settings.ParametersService.MTPurchaseOrderNumber();
			mTPurchaseOrder.OwnerID = Settings.LoginService.UserId;
			mTPurchaseOrder.Owner = Settings.LoginService.UserName;
			AddCurrentRowToMTPurchaseOrderList(mTPurchaseOrder, sourceRow);
		}
		else
		{
			mTPurchaseOrder = Settings.DocumentService.Create<MTPurchaseOrder>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder);
			mTPurchaseOrder.DocDate = Settings.HostingEnvironment.CurrentDate;
			mTPurchaseOrder.DocNum = Settings.ParametersService.MTPurchaseOrderNumber();
			mTPurchaseOrder.OwnerID = Settings.LoginService.UserId;
			mTPurchaseOrder.Owner = Settings.LoginService.UserName;
			foreach (GoodsTurnOverDataProvider.DataRow selectedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<GoodsTurnOverDataProvider.DataRow>())
			{
				AddCurrentRowToMTPurchaseOrderList(mTPurchaseOrder, selectedRow);
			}
		}
		if (mTPurchaseOrder != null)
		{
			Settings.DocumentService.CreateWindow(mTPurchaseOrder).Show();
		}
	}

	private void ShowMTTAccount()
	{
		List<GoodsTurnOverDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsTurnOverDataProvider.DataRow>();
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
			foreach (GoodsTurnOverDataProvider.DataRow item in selectedOrFocusedRows)
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
		mTAccountReportViewer.ShowSumms = ShowSumms;
		mTAccountReportViewer.ShowSummsWithVAT = ShowSummsWithVAT;
		mTAccountReportViewer.ShowCurrSumms = ShowCurrSumms;
		mTAccountReportViewer.ShowParties = ShowParties;
		mTAccountReportViewer.Show(useParamValues: true);
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
		ParametersProvider.ShowParties = ShowParties;
		ParametersProvider.ShowAltUnit = ShowAltUnit;
		ParametersProvider.ShowSumms = ShowSumms;
		ParametersProvider.ShowSummsWithVAT = ShowSummsWithVAT;
		ParametersProvider.ShowCurrSumms = ShowCurrSumms;
		ParametersProvider.DetailByStorage = DetailByStorage;
		ParametersProvider.DetailByOper = DetailByOper;
		ParametersProvider.ShowAlsoZeroRows = ShowAlsoZeroRows;
	}

	protected override void AddControlsInEditSelectedDialog(DialogWindow dialog, out bool doRefresh)
	{
		base.AddControlsInEditSelectedDialog(dialog, out doRefresh);
		EditSelectedHelper.AddGoodsControls(dialog);
	}

	protected override void EditSelectedRows(List<PropertyWithValue> properties, bool doRefresh, bool withoutValidation = false)
	{
		List<GoodsTurnOverDataProvider.DataRow> selectedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<GoodsTurnOverDataProvider.DataRow>();
		List<int> list = new List<int>();
		foreach (GoodsTurnOverDataProvider.DataRow item in selectedRows)
		{
			list.Add(item.GoodID);
		}
		EditSelectedDirectories(properties, list, null, ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value, out var errorRepViewer, withoutValidation);
		if (errorRepViewer.HasErrors)
		{
			errorRepViewer.Show();
		}
	}
}
