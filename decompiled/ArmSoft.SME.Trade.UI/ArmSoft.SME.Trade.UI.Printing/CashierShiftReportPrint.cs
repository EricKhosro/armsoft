using System;
using ArmSoft.SME.Common.Printing;
using ArmSoft.SME.Trade.Common.Models.ASReport;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(104)]
public class CashierShiftReportPrint : TradeDocumentPrintWrapper
{
	[PrintDataMember("CashierShiftReport")]
	public CashierShiftReportResult CashierShiftReport { get; set; }

	[PrintDataMember("Cash")]
	public string CashDesk { get; set; }

	[PrintDataMember("CashierShiftNum")]
	public string CashierShiftNumber { get; set; }

	[PrintDataMember("CashierShiftStart")]
	public DateTime StartDate { get; set; }

	[PrintDataMember("CashierShiftEnd")]
	public DateTime? EndDate { get; set; }
}
