using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class AccountListItem
{
	public int Id { get; set; }

	public string Number { get; set; } = string.Empty;

	public string Currency { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public AccountType Type { get; set; }

	public string DisplayType { get; set; } = string.Empty;

	public string? Note { get; set; }

	public decimal? Balance { get; set; }

	public decimal? ExpectedBalance { get; set; }

	public decimal InterestRate { get; set; }

	public bool IsGoalAccount { get; set; }

	public decimal? GoalAmount { get; set; }

	public AccountAccessLevels AccessLevels { get; set; }

	public string? AccessType { get; set; }

	public DateTime? ClosingDate { get; set; }
}
