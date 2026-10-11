using System;
using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.Data.Reports;
using ArmSoft.SME.Trade.UI.ReportViewers.Partner;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10061)]
public class BonusTurnOver : TradeDataView<BonusTurnOverDataProvider.DataRow, BonusTurnOverDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly BonusTurnOver _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly PartnerContractGroupControls _partnerContractControl;

		private readonly CheckEditExt _detailByContracts;

		private readonly CheckEditExt _showLinks;

		public DataViewDialog(BonusTurnOver dataView, bool useParamValues, bool isRefreshMode)
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
			LayoutGroupExt lgPartCont = AddVerticalLayoutGroup();
			_partnerContractControl = AddPartnerContractGroupControls();
			_partnerContractControl.ConstructDlgUI(lgPartCont, !useParamValues, "", isFilterBySuppliers: false, isFilterByCustomers: false, isShowContractCurrCode: false, isShowSummaDebt: false, "", "", showOnlyBonusContracts: true);
			if (useParamValues)
			{
				_partnerContractControl.PartnerGroup.Code = dataView.PartnerGroup;
				_partnerContractControl.PartnerCode.Code = dataView.PartnerCode;
				_partnerContractControl.Contract.Code = dataView.Contract;
			}
			AddLine();
			_detailByContracts = AddBooleanControl("_detailByContracts", ArmSoft.SME.Trade.Common.Resources.DetailByContrac, !useParamValues);
			AddLine();
			_showLinks = AddBooleanControl("ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, !useParamValues);
			_showLinks.EditValue = useParamValues && dataView.ShowLinks;
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
			_dataView.DetailsByContracts = _detailByContracts.IsChecked.Value;
			_dataView.ShowLinks = _showLinks.IsChecked.Value;
		}
	}

	public override string HelpId => "Purchases_Reports_BonusReports_TurnOver";

	public override bool DialogSupported => true;

	public override string Caption => $"{base.Caption} - {StartDate.FormatDDMMYY()} - {EndDate.FormatDDMMYY()}";

	private string InnerCaption => base.Caption ?? "";

	public override LoadIndication LoadingIndication => LoadIndication.Always;

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

	public BonusTurnOver()
	{
		DataProvider = new BonusTurnOverDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("PartnerGroup", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Partner, string.Empty);
		AddParameter("Contract", ArmSoft.SME.Common.Resources.Contract, string.Empty);
		AddParameter("DetailsByContracts", ArmSoft.SME.Trade.Common.Resources.DetailByContrac, false);
		AddParameter("ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, true);
		FillColumnsFromDataProvider();
	}

	public override string GetDefaultFunction()
	{
		return "OpenBonusCard";
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.PartRepCard).AllowView)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "OpenBonusCard", ArmSoft.SME.Trade.Common.Resources.BonusTAccount, OpenBonusCard);
		}
	}

	private void OpenBonusCard()
	{
		List<AccRestRow> list = new List<AccRestRow>();
		foreach (BonusTurnOverDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<BonusTurnOverDataProvider.DataRow>())
		{
			AccRestRow item = new AccRestRow
			{
				PartID = selectedOrFocusedRow.PartID,
				PartnerCode = selectedOrFocusedRow.PartCode,
				AccCode = selectedOrFocusedRow.ContractCode
			};
			list.Add(item);
		}
		BonusTAccountReportViewer bonusTAccountReportViewer = new BonusTAccountReportViewer(list);
		bonusTAccountReportViewer.ShowFromTurns = true;
		bonusTAccountReportViewer.DateBegin = StartDate;
		bonusTAccountReportViewer.DateEnd = EndDate;
		bonusTAccountReportViewer.Show(useParamValues: true);
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
		ParametersProvider.DetailsByContracts = DetailsByContracts;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("ContractName", DetailsByContracts);
		dvp.Set_ColumnVisible("ContractCode", DetailsByContracts);
		Columns.AddDirectoryLinkColumn(DirectoryTypeSE.Partner, "PartCode", "PartName", "", "", ShowLinks);
	}
}
