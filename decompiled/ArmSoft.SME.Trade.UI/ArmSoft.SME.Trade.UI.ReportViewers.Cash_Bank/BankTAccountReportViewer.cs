using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.ASReport;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.Data.Reports;
using ArmSoft.SME.Trade.UI.DataView;

namespace ArmSoft.SME.Trade.UI.ReportViewers.Cash_Bank;

public class BankTAccountReportViewer : PartCashBankTAccRepViewerBase
{
	public BankTAccountReportViewer(List<AccRestRow> selectedRows = null)
		: base(CommonConst.AccountingsTypes.Bank)
	{
		IsForSelectedRows = selectedRows != null && selectedRows.Count > 1;
		SelectedRows = selectedRows;
		if (!IsForSelectedRows && selectedRows != null)
		{
			PartnerCode = selectedRows[0].PartnerCode;
			AccountCode = selectedRows[0].AccCode;
			CurrCode = selectedRows[0].CurrCode;
		}
		InnerCaption = (IsForSelectedRows ? ArmSoft.SME.Trade.Common.Resources.BanksTAccount : string.Format(ArmSoft.SME.Trade.Common.Resources.TAccountByBank, ""));
		InnerName = "BankTAccount";
		HelpId = "Purchases_Reports_CashBankReports_BankTAccount";
	}

	public override void ExecuteDPR()
	{
		_reportRequest = new PartCashBankBonusTAccRepParam();
		_reportRequest.AccType = AccType;
		_reportRequest.BeginDate = DateBegin;
		_reportRequest.EndDate = DateEnd;
		_reportRequest.CurrCode = CurrCode;
		_reportRequest.GroupBy = GroupByContract;
		_reportResult = new Dictionary<string, PartCashBankBonusTAccRepResult>();
		if (!IsForSelectedRows)
		{
			_reportRequest.PartCode = PartnerCode;
			_reportRequest.AccountCode = AccountCode;
			_reportResult.Add(AccountCode, DataProcessingRequestHelper.Execute<PartCashBankBonusTAccRepParam, PartCashBankBonusTAccRepResult>(DPRType.Report, Name, _reportRequest));
			return;
		}
		foreach (AccRestRow selectedRow in SelectedRows)
		{
			_reportRequest.PartCode = PartnerCode;
			_reportRequest.AccountCode = selectedRow.AccCode;
			_reportResult.Add(selectedRow.AccCode, DataProcessingRequestHelper.ExecuteThroughHandler<PartCashBankBonusTAccRepParam, PartCashBankBonusTAccRepResult>(DPRType.Report, Name, _reportRequest).Item2);
		}
	}

	protected override void FillContextMenu(object rowInfo)
	{
		if (GroupByContract && Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.BankOperations).AllowView)
		{
			AddContextMenuFunction(ArmSoft.SME.Common.Resources.Details, OpenBankOpJournal);
		}
	}

	private void OpenBankOpJournal(object rowInfo)
	{
		if (rowInfo != null)
		{
			PartCashBankBonusTAccRepResult.PartCashBankBonusTAccRow partCashBankBonusTAccRow = (PartCashBankBonusTAccRepResult.PartCashBankBonusTAccRow)rowInfo;
			BankOperations bankOperations = new BankOperations();
			bankOperations.StartDate = DateBegin;
			bankOperations.EndDate = DateEnd;
			bankOperations.PartnerCode = partCashBankBonusTAccRow.PartnerCode;
			bankOperations.CurrCode = partCashBankBonusTAccRow.CurrCode;
			bankOperations.AllowOperation = true;
			bankOperations.ReportCurrTypes = ReportCurrTypes;
			bankOperations.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
		}
	}
}
