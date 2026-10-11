using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class CancelRequestApplicationForEdit : DocumentForEdit<CancelRequestApplication>
{
	public string TargetType { get; set; } = string.Empty;

	public string DisplayTargetType { get; set; } = string.Empty;

	public string TargetNumber { get; set; } = string.Empty;

	public DateTime TargetDate { get; set; }

	public decimal? TargetAmount { get; set; }

	public string TargetCurrency { get; set; } = string.Empty;
}
