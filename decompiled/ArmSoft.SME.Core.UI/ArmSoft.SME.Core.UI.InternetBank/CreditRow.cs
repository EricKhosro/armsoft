using System;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class CreditRow
{
	public string DisplayContract { get; set; }

	public string CreditCode { get; set; }

	public string DisplayCategory { get; set; }

	public string Currency { get; set; }

	public DateOnly OriginalDate { get; set; }

	public DateOnly MaturityDate { get; set; }

	public decimal? CalculatedInterest { get; set; }

	public string InterestRate { get; set; }

	public decimal? OriginalPrincipal { get; set; }

	public decimal? OutstandingPrincipal { get; set; }

	public DateOnly? ClosingDate { get; set; }
}
