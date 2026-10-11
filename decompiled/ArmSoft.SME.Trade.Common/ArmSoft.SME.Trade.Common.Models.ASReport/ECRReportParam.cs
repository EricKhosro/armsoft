using System;

namespace ArmSoft.SME.Trade.Common.Models.ASReport;

public class ECRReportParam
{
	public DateOnly StartDate { get; set; }

	public DateOnly EndDate { get; set; }

	public string CashDesk { get; set; }
}
