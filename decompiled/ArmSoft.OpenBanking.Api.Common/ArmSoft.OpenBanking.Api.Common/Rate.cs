using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class Rate
{
	public DateTime? RateDate { get; set; }

	public string Currency1 { get; set; } = string.Empty;

	public string Currency2 { get; set; } = string.Empty;

	public decimal RateValue { get; set; }

	public string AmountCurrency { get; set; } = string.Empty;

	public short RateFor { get; set; }

	public string RateLabel { get; set; } = string.Empty;
}
