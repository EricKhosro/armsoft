using System;
using System.Collections.Generic;
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
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.Data.Reports;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.ReportViewers.Partner;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView.Reports;

[DataView(10066)]
public class PartnersTurnOver : TradeDataView<PartnersTurnOverDataProvider.DataRow, PartnersTurnOverDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly PartnersTurnOver _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly PartnerContractGroupControls _partnerContractControl;

		private readonly DropDownControl _repCurrType;

		private readonly CheckEditExt _detailByContracts;

		private readonly CheckEditExt _detailsByOperations;

		private readonly CheckEditExt _showLinks;

		private readonly CheckEditExt _showAlsoZeroRows;

		public DataViewDialog(PartnersTurnOver dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = dataView.InnerCaption;
			Name = dataView.Name;
			HelpId = dataView.HelpId;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : Settings.HostingEnvironment.DateBegin);
			_endDate = AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : Settings.HostingEnvironment.DateEnd);
			AddSpecialDateButton(container, _startDate, _endDate);
			DXTabControl tc = AddTabControl(null);
			LayoutGroup layoutGroup = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.General);
			LayoutGroupExt layoutGroupExt = AddVerticalLayoutGroup(layoutGroup);
			_partnerContractControl = AddPartnerContractGroupControls();
			_partnerContractControl.ConstructDlgUI(layoutGroupExt, !useParamValues);
			if (useParamValues)
			{
				_partnerContractControl.PartnerGroup.Code = dataView.PartnerGroup;
				_partnerContractControl.PartnerCode.Code = dataView.PartnerCode;
				_partnerContractControl.Contract.Code = dataView.Contract;
				_partnerContractControl.ContractCurrCode.Code = dataView.CurrCode;
			}
			AddLine(layoutGroupExt);
			_repCurrType = AddDropDownControl(layoutGroupExt, "ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, new SmartEnum(typeof(ReportCurrTypesSE)), isRequired: true, allowAdditionButtons: false, !useParamValues);
			_repCurrType.Code = (useParamValues ? dataView.ReportCurrTypes : "1");
			_detailByContracts = AddBooleanControl(layoutGroupExt, "DetailsByContracts", ArmSoft.SME.Trade.Common.Resources.DetailByContrac, !useParamValues);
			_detailByContracts.EditValue = useParamValues && dataView.DetailsByContracts;
			_detailsByOperations = AddBooleanControl(layoutGroupExt, "DetailsByOperations", ArmSoft.SME.Trade.Common.Resources.OperationDetail, !useParamValues);
			_detailsByOperations.EditValue = useParamValues && dataView.DetailsByOperations;
			AddLine(layoutGroupExt);
			_showLinks = AddBooleanControl(layoutGroupExt, "ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, !useParamValues);
			_showLinks.EditValue = useParamValues && dataView.ShowLinks;
			AddLine(layoutGroupExt);
			_showAlsoZeroRows = AddBooleanControl(layoutGroupExt, "ShowAlsoZeroRows", ArmSoft.SME.Trade.Common.Resources.ShowAlsoZeroRows, !useParamValues);
			_showAlsoZeroRows.EditValue = useParamValues && dataView.ShowAlsoZeroRows;
			if (!isRefreshMode)
			{
				AddColumnsSelectionTab(tc, layoutGroup, null);
			}
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
				args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_endDate.EditValue.Value);
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
			_dataView.StartDate = _startDate.EditValue.Value;
			_dataView.EndDate = _endDate.EditValue.Value;
			_dataView.PartnerGroup = _partnerContractControl.PartnerGroup.Code;
			_dataView.PartnerCode = _partnerContractControl.PartnerCode.Code;
			_dataView.Contract = _partnerContractControl.Contract.Code;
			_dataView.CurrCode = _partnerContractControl.ContractCurrCode.Code;
			_dataView.ReportCurrTypes = _repCurrType.Code;
			_dataView.DetailsByContracts = _detailByContracts.IsChecked.Value;
			_dataView.DetailsByOperations = _detailsByOperations.IsChecked.Value;
			_dataView.ShowAlsoZeroRows = _showAlsoZeroRows.IsChecked.Value;
			_dataView.ShowLinks = _showLinks.IsChecked.Value;
		}
	}

	private List<ViewColumn> potentiallyColumns;

	public override string HelpId => "Purchases_Reports_PartnerReports_TurnOver";

	public override bool DialogSupported => true;

	public override string Caption => $"{base.Caption} - {StartDate.FormatDDMMYY()} - {EndDate.FormatDDMMYY()}";

	private string InnerCaption => base.Caption ?? "";

	public override List<ViewColumn> PotentiallyColumns => potentiallyColumns;

	public override LoadIndication LoadingIndication => LoadIndication.AlwaysInside;

	public DateOnly StartDate
	{
		get
		{
			return (DateOnly)Parameters["StartDate"].Value;
		}
		set
		{
			Parameters["StartDate"].Value = value;
		}
	}

	public DateOnly EndDate
	{
		get
		{
			return (DateOnly)Parameters["EndDate"].Value;
		}
		set
		{
			Parameters["EndDate"].Value = value;
		}
	}

	public string PartnerGroup
	{
		get
		{
			return (string)Parameters["PartnerGroup"].Value;
		}
		set
		{
			Parameters["PartnerGroup"].Value = value;
		}
	}

	public string PartnerCode
	{
		get
		{
			return (string)Parameters["PartnerCode"].Value;
		}
		set
		{
			Parameters["PartnerCode"].Value = value;
		}
	}

	public string CurrCode
	{
		get
		{
			return (string)Parameters["CurrCode"].Value;
		}
		set
		{
			Parameters["CurrCode"].Value = value;
		}
	}

	public string Contract
	{
		get
		{
			return (string)Parameters["Contract"].Value;
		}
		set
		{
			Parameters["Contract"].Value = value;
		}
	}

	public string ReportCurrTypes
	{
		get
		{
			return (string)Parameters["ReportCurrTypes"].Value;
		}
		set
		{
			Parameters["ReportCurrTypes"].Value = value;
		}
	}

	public bool DetailsByContracts
	{
		get
		{
			return (bool)Parameters["DetailsByContracts"].Value;
		}
		set
		{
			Parameters["DetailsByContracts"].Value = value;
		}
	}

	public bool DetailsByOperations
	{
		get
		{
			return (bool)Parameters["DetailsByOperations"].Value;
		}
		set
		{
			Parameters["DetailsByOperations"].Value = value;
		}
	}

	public bool ShowAlsoZeroRows
	{
		get
		{
			return (bool)Parameters["ShowAlsoZeroRows"].Value;
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
			return (bool)Parameters["ShowLinks"].Value;
		}
		set
		{
			Parameters["ShowLinks"].Value = value;
		}
	}

	public PartnersTurnOver()
	{
		DataProvider = new PartnersTurnOverDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("PartnerGroup", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Partner, string.Empty);
		AddParameter("Contract", ArmSoft.SME.Common.Resources.Contract, string.Empty);
		AddParameter("CurrCode", ArmSoft.SME.Common.Resources.Curr, string.Empty);
		AddParameter("ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, "1");
		AddParameter("DetailsByContracts", ArmSoft.SME.Trade.Common.Resources.DetailByContrac, false);
		AddParameter("DetailsByOperations", ArmSoft.SME.Trade.Common.Resources.OperationDetail, false);
		AddParameter("ShowAlsoZeroRows", ArmSoft.SME.Trade.Common.Resources.ShowAlsoZeroRows, false);
		AddParameter("ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, true);
		FillColumnsFromDataProvider();
		potentiallyColumns = new List<ViewColumn>();
		potentiallyColumns.AddRange(Columns.ViewList);
		potentiallyColumns.AddRange(PartnerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Partner, "", ""));
	}

	protected override void SetColumnsProperties()
	{
		base.SetColumnsProperties();
		Columns["StartRemNCC"].IsSystem = true;
		Columns["StartRemCurr"].IsSystem = true;
		Columns["TurnOverInNCC"].IsSystem = true;
		Columns["TurnOverInCurr"].IsSystem = true;
		Columns["TurnOverOutNCC"].IsSystem = true;
		Columns["TurnOverOutCurr"].IsSystem = true;
		Columns["SupRetSumNCC"].IsSystem = true;
		Columns["SupRetSumCurr"].IsSystem = true;
		Columns["RealizSumNCC"].IsSystem = true;
		Columns["RealizSumCurr"].IsSystem = true;
		Columns["OtherSumInNCC"].IsSystem = true;
		Columns["OtherSumInCurr"].IsSystem = true;
		Columns["CustPaySumNCC"].IsSystem = true;
		Columns["CustPaySumCurr"].IsSystem = true;
		Columns["CustRetSumNCC"].IsSystem = true;
		Columns["CustRetSumCurr"].IsSystem = true;
		Columns["MtInSumNCC"].IsSystem = true;
		Columns["MtInSumCurr"].IsSystem = true;
		Columns["OverHeadExpSumOutNCC"].IsSystem = true;
		Columns["OverHeadExpSumOutCurr"].IsSystem = true;
		Columns["OtherSumOutNCC"].IsSystem = true;
		Columns["OtherSumOutCurr"].IsSystem = true;
		Columns["EndRemNCC"].IsSystem = true;
		Columns["EndRemCurr"].IsSystem = true;
	}

	public override string GetDefaultFunction()
	{
		return "OpenPartnerCard";
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.PartRepCard).AllowView)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "OpenPartnerCard", ArmSoft.SME.Trade.Common.Resources.PartnerCard, OpenPartnerCard);
		}
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		PartnersTurnOverDataProvider.DataRow dataRow = row as PartnersTurnOverDataProvider.DataRow;
		dataRow.PartnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(dataRow.PartCode);
	}

	private void OpenPartnerCard()
	{
		List<AccRestRow> list = new List<AccRestRow>();
		foreach (PartnersTurnOverDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<PartnersTurnOverDataProvider.DataRow>())
		{
			AccRestRow accRestRow = new AccRestRow
			{
				PartID = selectedOrFocusedRow.PartID,
				PartnerCode = selectedOrFocusedRow.PartCode,
				AccCode = selectedOrFocusedRow.ContractCode
			};
			if (ReportCurrTypes != "1")
			{
				accRestRow.CurrCode = selectedOrFocusedRow.CurrCode;
			}
			list.Add(accRestRow);
		}
		PartnerTAccountReportViewer partnerTAccountReportViewer = new PartnerTAccountReportViewer(list);
		partnerTAccountReportViewer.ReportCurrTypes = ReportCurrTypes;
		partnerTAccountReportViewer.ShowFromTurns = true;
		partnerTAccountReportViewer.DateBegin = StartDate;
		partnerTAccountReportViewer.DateEnd = EndDate;
		partnerTAccountReportViewer.Show(useParamValues: true);
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.PartnerGroup = PartnerGroup;
		ParametersProvider.PartnerCode = PartnerCode;
		ParametersProvider.Contract = Contract;
		ParametersProvider.CurrCode = CurrCode;
		ParametersProvider.ReportCurrTypes = ReportCurrTypes;
		ParametersProvider.DetailsByContracts = DetailsByContracts;
		ParametersProvider.DetailsByOperations = DetailsByOperations;
		ParametersProvider.ShowAlsoZeroRows = ShowAlsoZeroRows;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		bool flag = !(ReportCurrTypes == "2");
		bool flag2 = !(ReportCurrTypes == "1");
		bool value = flag && DetailsByOperations;
		bool value2 = flag2 && DetailsByOperations;
		dvp.Set_ColumnVisible("StartRemNCC", flag);
		dvp.Set_ColumnVisible("TurnOverInNCC", flag && !DetailsByOperations);
		dvp.Set_ColumnVisible("TurnOverOutNCC", flag && !DetailsByOperations);
		dvp.Set_ColumnVisible("EndRemNCC", flag);
		dvp.Set_ColumnVisible("StartRemCurr", flag2);
		dvp.Set_ColumnVisible("TurnOverInCurr", flag2 && !DetailsByOperations);
		dvp.Set_ColumnVisible("TurnOverOutCurr", flag2 && !DetailsByOperations);
		dvp.Set_ColumnVisible("EndRemCurr", flag2);
		dvp.Set_ColumnVisible("SupRetSumNCC", value);
		dvp.Set_ColumnVisible("SupRetSumCurr", value2);
		dvp.Set_ColumnVisible("RealizSumNCC", value);
		dvp.Set_ColumnVisible("RealizSumCurr", value2);
		dvp.Set_ColumnVisible("OtherSumInNCC", value);
		dvp.Set_ColumnVisible("OtherSumInCurr", value2);
		dvp.Set_ColumnVisible("CustPaySumNCC", value);
		dvp.Set_ColumnVisible("CustPaySumCurr", value2);
		dvp.Set_ColumnVisible("CustRetSumNCC", value);
		dvp.Set_ColumnVisible("CustRetSumCurr", value2);
		dvp.Set_ColumnVisible("MtInSumNCC", value);
		dvp.Set_ColumnVisible("MtInSumCurr", value2);
		dvp.Set_ColumnVisible("OtherSumOutNCC", value);
		dvp.Set_ColumnVisible("OtherSumOutCurr", value2);
		dvp.Set_ColumnVisible("OverHeadExpSumOutNCC", value);
		dvp.Set_ColumnVisible("OverHeadExpSumOutCurr", value2);
		dvp.Set_ColumnVisible("ContractName", DetailsByContracts);
		dvp.Set_ColumnVisible("ContractCode", DetailsByContracts);
		dvp.Set_ColumnVisible("ContractName", DetailsByContracts);
		dvp.Set_ColumnVisible("CurrCode", DetailsByContracts | flag2);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("PartCode");
			if (DetailsByContracts)
			{
				dvp.Set_ColumnSortOrder("ContractCode");
			}
		}
		Columns.AddDirectoryLinkColumn(DirectoryTypeSE.Partner, "PartCode", "PartName", "", "", ShowLinks);
	}
}
