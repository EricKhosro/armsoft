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
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.Data.Reports;
using ArmSoft.SME.Trade.UI.ReportViewers.Partner;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10053)]
public class BonusRemains : TradeDataView<BonusRemainsDataProvider.DataRow, BonusRemainsDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly BonusRemains _dataView;

		private readonly DateEditControl _remDate;

		private readonly PartnerContractGroupControls _partnerContractControl;

		private readonly CheckEditExt _detailsByContracts;

		private readonly CheckEditExt _showPartnerData;

		private readonly CheckEditExt _showLinks;

		public DataViewDialog(BonusRemains dataView, bool useParamValues, bool isRefreshMode)
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
			_remDate = AddDateControl(container, "Date", ArmSoft.SME.Common.Resources.DateWord, useLongDate: false, isRequired: true, !useParamValues);
			_remDate.EditValue = (useParamValues ? dataView.Date : Settings.HostingEnvironment.CurrentDate);
			AddSpecialDateButton(container, _remDate);
			LayoutGroupExt layoutGroupExt = AddVerticalLayoutGroup();
			_partnerContractControl = AddPartnerContractGroupControls();
			_partnerContractControl.ConstructDlgUI(layoutGroupExt, !useParamValues, "", isFilterBySuppliers: false, isFilterByCustomers: false, isShowContractCurrCode: false, isShowSummaDebt: false, "", "", showOnlyBonusContracts: true);
			if (useParamValues)
			{
				_partnerContractControl.PartnerGroup.Code = dataView.PartnerGroup;
				_partnerContractControl.PartnerCode.Code = dataView.PartnerCode;
				_partnerContractControl.Contract.Code = dataView.Contract;
			}
			AddLine(layoutGroupExt);
			_detailsByContracts = AddBooleanControl(layoutGroupExt, "DetailsByContracts", ArmSoft.SME.Trade.Common.Resources.DetailByContrac, storeValue: true);
			_detailsByContracts.IsChecked = useParamValues && dataView.DetailsByContracts;
			_showPartnerData = AddBooleanControl(layoutGroupExt, "ShowPartnerData", ArmSoft.SME.Trade.Common.Resources.ShowPartnerData, storeValue: true);
			_showPartnerData.IsChecked = useParamValues && dataView.ShowPartnerData;
			AddLine();
			_showLinks = AddBooleanControl("ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, !useParamValues);
			_showLinks.EditValue = useParamValues && dataView.ShowLinks;
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
			_dataView.Date = _remDate.EditValue.Value;
			_dataView.PartnerGroup = _partnerContractControl.PartnerGroup.Code;
			_dataView.PartnerCode = _partnerContractControl.PartnerCode.Code;
			_dataView.Contract = _partnerContractControl.Contract.Code;
			_dataView.DetailsByContracts = _detailsByContracts.IsChecked.Value;
			_dataView.ShowPartnerData = _showPartnerData.IsChecked.Value;
			_dataView.ShowLinks = _showLinks.IsChecked.Value;
		}
	}

	public override string HelpId => "Purchases_Reports_BonusReports_Remains";

	public override bool DialogSupported => true;

	public override string Caption => base.Caption + " - " + Date.FormatDDMMYY();

	private string InnerCaption => base.Caption ?? "";

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public DateOnly Date
	{
		get
		{
			return (DateOnly)Parameters["Date"].Value;
		}
		set
		{
			Parameters["Date"].Value = value;
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

	public bool ShowPartnerData
	{
		get
		{
			return (bool)Parameters["ShowPartnerData"].Value;
		}
		set
		{
			Parameters["ShowPartnerData"].Value = value;
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

	public BonusRemains()
	{
		DataProvider = new BonusRemainsDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("Date", ArmSoft.SME.Common.Resources.DateWord, Settings.HostingEnvironment.CurrentDate, "dd/MM/yy");
		AddParameter("PartnerGroup", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Code, string.Empty);
		AddParameter("Contract", ArmSoft.SME.Common.Resources.Contract, string.Empty);
		AddParameter("DetailsByContracts", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, false);
		AddParameter("ShowPartnerData", ArmSoft.SME.Trade.Common.Resources.ShowPartnerData, false);
		AddParameter("ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, true);
		FillColumnsFromDataProvider();
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	public override string GetDefaultFunction()
	{
		return "OpenBonusTAccount";
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.BonusTAccount).AllowView)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "OpenBonusTAccount", ArmSoft.SME.Trade.Common.Resources.BonusTAccount, OpenBonusTAccount);
		}
		if (Settings.UserAccessService.GetDocumentPermission(DocumentTypeSE.BonusGroupCorrect).AllowAdd && DetailsByContracts)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "CreateBonusGroupCorrect", ArmSoft.SME.Trade.Common.Resources.BonusGroupCorrect, CreateBonusGroupCorrect);
		}
	}

	private void OpenBonusTAccount()
	{
		List<AccRestRow> list = new List<AccRestRow>();
		foreach (BonusRemainsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<BonusRemainsDataProvider.DataRow>())
		{
			AccRestRow item = new AccRestRow
			{
				PartID = selectedOrFocusedRow.PartID,
				AccCode = selectedOrFocusedRow.ContractCode,
				PartnerCode = selectedOrFocusedRow.PartCode
			};
			list.Add(item);
		}
		BonusTAccountReportViewer bonusTAccountReportViewer = new BonusTAccountReportViewer(list);
		bonusTAccountReportViewer.DateBegin = Settings.HostingEnvironment.DateBegin;
		bonusTAccountReportViewer.DateEnd = Settings.HostingEnvironment.DateEnd;
		bonusTAccountReportViewer.Show(useParamValues: true);
	}

	private void CreateBonusGroupCorrect()
	{
		BonusGroupCorrect bonusGroupCorrect = Settings.DocumentService.Create<BonusGroupCorrect>(DocumentTypeSE.BonusGroupCorrect);
		bonusGroupCorrect.DocDate = Settings.HostingEnvironment.CurrentDate;
		foreach (BonusRemainsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<BonusRemainsDataProvider.DataRow>())
		{
			bonusGroupCorrect.PartRemList.Add(new PartnerBonusRemGroupCorrectBase.PartnerBonusRemSpecificationRow
			{
				Contract = selectedOrFocusedRow.ContractCode,
				PartCode = selectedOrFocusedRow.PartCode,
				PartName = selectedOrFocusedRow.PartName,
				PartID = selectedOrFocusedRow.PartID,
				Summa = -selectedOrFocusedRow.RemNCC,
				SummaNCC = -selectedOrFocusedRow.RemNCC
			});
		}
		Settings.DocumentService.CreateWindow(bonusGroupCorrect).Show();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("ContractCode", DetailsByContracts);
		dvp.Set_ColumnVisible("ContractName", DetailsByContracts);
		dvp.Set_ColumnVisible("ContractDate", DetailsByContracts);
		dvp.Set_ColumnVisible("TaxCode", ShowPartnerData);
		dvp.Set_ColumnVisible("Address", ShowPartnerData);
		dvp.Set_ColumnVisible("Phone", ShowPartnerData);
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

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.Date = Date;
		ParametersProvider.PartnerGroup = PartnerGroup;
		ParametersProvider.PartnerCode = PartnerCode;
		ParametersProvider.Contract = Contract;
		ParametersProvider.DetailsByContracts = DetailsByContracts;
		ParametersProvider.ShowPartnerData = ShowPartnerData;
	}
}
