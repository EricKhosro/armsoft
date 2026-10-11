using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.ASReport;
using ArmSoft.SME.Trade.Data.Reports;
using ArmSoft.SME.Trade.UI.DataView;

namespace ArmSoft.SME.Trade.UI.ReportViewers.Partner;

public class PartnerTAccountReportViewer : PartCashBankTAccRepViewerBase
{
	public PartnerTAccountReportViewer(List<AccRestRow> selectedRows = null)
		: base(CommonConst.AccountingsTypes.PartnerContract)
	{
		IsForSelectedRows = selectedRows != null && selectedRows.Count > 1;
		SelectedRows = selectedRows;
		if (!IsForSelectedRows && selectedRows != null)
		{
			PartnerCode = selectedRows[0].PartnerCode;
			ContractCode = selectedRows[0].AccCode;
			CurrCode = selectedRows[0].CurrCode;
		}
		InnerCaption = (IsForSelectedRows ? ArmSoft.SME.Trade.Common.Resources.PartnersTAccount : string.Format(ArmSoft.SME.Trade.Common.Resources.TAccountByPartner, ""));
		InnerName = "PartnerTAccount";
		HelpId = "Purchases_Reports_PartnerReports_PartnerCard";
	}

	protected override void FillContextMenu(object rowInfo)
	{
		if (GroupByContract && Settings.UserAccessService.GetProviderPermission(10052).AllowView)
		{
			AddContextMenuFunction(ArmSoft.SME.Common.Resources.Details, OpenPartOpJournalConst);
		}
	}

	public override void ExecuteDPR()
	{
		_reportRequest = new PartCashBankBonusTAccRepParam
		{
			AccType = AccType,
			BeginDate = DateBegin,
			EndDate = DateEnd,
			CurrCode = CurrCode,
			GroupBy = GroupByContract
		};
		_reportResult = new Dictionary<string, PartCashBankBonusTAccRepResult>();
		if (!IsForSelectedRows)
		{
			_reportRequest.PartCode = PartnerCode;
			_reportRequest.AccountCode = ContractCode;
			_reportResult.Add(PartnerCode + ContractCode, DataProcessingRequestHelper.Execute<PartCashBankBonusTAccRepParam, PartCashBankBonusTAccRepResult>(DPRType.Report, Name, _reportRequest));
			return;
		}
		foreach (AccRestRow selectedRow in SelectedRows)
		{
			_reportRequest.PartCode = selectedRow.PartnerCode;
			_reportRequest.AccountCode = selectedRow.AccCode;
			_reportResult.Add(selectedRow.PartnerCode + selectedRow.AccCode, DataProcessingRequestHelper.ExecuteThroughHandler<PartCashBankBonusTAccRepParam, PartCashBankBonusTAccRepResult>(DPRType.Report, Name, _reportRequest).Item2);
		}
	}

	private void OpenPartOpJournalConst(object rowInfo)
	{
		if (rowInfo != null)
		{
			PartCashBankBonusTAccRepResult.PartCashBankBonusTAccRow partCashBankBonusTAccRow = (PartCashBankBonusTAccRepResult.PartCashBankBonusTAccRow)rowInfo;
			PartnersOperations partnersOperations = new PartnersOperations();
			partnersOperations.StartDate = DateBegin;
			partnersOperations.EndDate = DateEnd;
			partnersOperations.PartnerCode = partCashBankBonusTAccRow.PartnerCode;
			partnersOperations.Contract = partCashBankBonusTAccRow.ContractCode;
			partnersOperations.CurrCode = partCashBankBonusTAccRow.CurrCode;
			partnersOperations.AllowOperation = true;
			partnersOperations.ReportCurrTypes = ReportCurrTypes;
			partnersOperations.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
		}
	}
}
