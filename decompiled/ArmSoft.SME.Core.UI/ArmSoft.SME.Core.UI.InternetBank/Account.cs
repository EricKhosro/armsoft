using System.Collections.Generic;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class Account
{
	public int Id { get; set; }

	public string AccountNumber { get; set; }

	public string DisplayAccountNumber { get; set; }

	public string Currency { get; set; }

	public string Name { get; set; }

	public AccountType Type { get; set; }

	public string Note { get; set; }

	public string DisplayType { get; set; }

	public decimal? Balance { get; set; }

	public decimal? ExpectedBalance { get; set; }

	public decimal InterestRate { get; set; }

	public bool IsGoalAccount { get; set; }

	public decimal? GoalAmount { get; set; }

	public AccountAccessLevels AccessLevel { get; set; }

	public List<Link> Links { get; set; }
}
