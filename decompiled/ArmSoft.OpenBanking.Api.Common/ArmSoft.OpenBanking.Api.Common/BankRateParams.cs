using System;
using System.ComponentModel;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class BankRateParams
{
	public string Currency1 { get; set; } = string.Empty;

	public string Currency2 { get; set; } = string.Empty;

	[DefaultValue(null)]
	public DateTime? Date { get; set; }

	[DefaultValue(DealingRateType.NonCash)]
	public DealingRateType RateType { get; set; }
}
