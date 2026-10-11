using System;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class ReconciliationCalcGridRow
{
	[PrintDataMember("RowN")]
	public int RowNumber { get; set; }

	[PrintDataMember("DateWord")]
	public DateOnly Date { get; set; }

	[PrintDataMember("BaseDoc")]
	public string DocDescr { get; set; }

	[PrintDataMember("Comment")]
	public string TransComment { get; set; }

	[PrintDataMember("DebtIncrease")]
	public decimal DBSumm { get; set; }

	[PrintDataMember("DebtDecrease")]
	public decimal CRSumm { get; set; }
}
