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

namespace ArmSoft.SME.Trade.UI.ReportViewers.Partner;

public class BonusTAccountReportViewer : PartCashBankTAccRepViewerBase
{
	public BonusTAccountReportViewer(List<AccRestRow> selectedRows = null)
		: base(CommonConst.AccountingsTypes.Bonus)
	{
		IsForSelectedRows = selectedRows != null && selectedRows.Count > 1;
		SelectedRows = selectedRows;
		if (!IsForSelectedRows && selectedRows != null)
		{
			PartnerCode = selectedRows[0].PartnerCode;
			ContractCode = selectedRows[0].AccCode;
		}
		InnerCaption = (IsForSelectedRows ? ArmSoft.SME.Trade.Common.Resources.BonusTAccount : string.Format(ArmSoft.SME.Trade.Common.Resources.TAccountByBonus, ""));
		InnerName = "BonusTAccount";
		HelpId = "Purchases_Reports_BonusReports_BonusCard";
	}

	public override void ExecuteDPR()
	{
		_reportRequest = new PartCashBankBonusTAccRepParam();
		_reportRequest.AccType = AccType;
		_reportRequest.BeginDate = DateBegin;
		_reportRequest.EndDate = DateEnd;
		_reportRequest.CurrCode = "AMD";
		_reportRequest.GroupBy = GroupByContract;
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

	protected override void FillContextMenu(object rowInfo)
	{
		if (GroupByContract && Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.BonusOpJournal).AllowView)
		{
			AddContextMenuFunction(ArmSoft.SME.Common.Resources.Details, OpenBonusOpJournal);
		}
	}

	private void OpenBonusOpJournal(object rowInfo)
	{
		if (rowInfo != null)
		{
			PartCashBankBonusTAccRepResult.PartCashBankBonusTAccRow partCashBankBonusTAccRow = (PartCashBankBonusTAccRepResult.PartCashBankBonusTAccRow)rowInfo;
			BonusOperations bonusOperations = new BonusOperations();
			bonusOperations.StartDate = DateBegin;
			bonusOperations.EndDate = DateEnd;
			bonusOperations.PartnerCode = partCashBankBonusTAccRow.PartnerCode;
			bonusOperations.Contract = partCashBankBonusTAccRow.ContractCode;
			bonusOperations.AllowOperation = true;
			bonusOperations.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
		}
	}
}
