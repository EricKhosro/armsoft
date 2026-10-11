using System;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class DepositRow
{
	public string DisplayContract { get; set; }

	public string DisplayCategory { get; set; }

	public string Currency { get; set; }

	public DateOnly OriginalDate { get; set; }

	public DateOnly MaturityDate { get; set; }

	public decimal OriginalPrincipal { get; set; }

	public string InterestRate { get; set; }

	public decimal? OutstandingPrincipal { get; set; }

	public decimal? CalculatedInterest { get; set; }

	public DateOnly? ClosingDate { get; set; }

	public bool IsReplenishable { get; set; }

	public bool IsCapitalizable { get; set; }

	public bool IsExtendable { get; set; }

	public string PayPeriodName { get; set; }
}
