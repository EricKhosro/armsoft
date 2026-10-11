using System;

namespace ArmSoft.SME.Trade.Common.Models.ASReport;

public class PartCashBankBonusTAccRepParam
{
	public CommonConst.AccountingsTypes AccType { get; set; }

	public DateOnly BeginDate { get; set; }

	public DateOnly EndDate { get; set; }

	public string PartCode { get; set; }

	public string AccountCode { get; set; }

	public string CurrCode { get; set; }

	public bool GroupBy { get; set; }
}
