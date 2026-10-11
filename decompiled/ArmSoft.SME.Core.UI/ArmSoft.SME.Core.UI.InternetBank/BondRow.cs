using System;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class BondRow
{
	public string Isin { get; set; }

	public string DisplayContract { get; set; }

	public DateOnly OriginalDate { get; set; }

	public DateOnly MaturityDate { get; set; }

	public string Currency { get; set; }

	public string InterestRate { get; set; }

	public DateOnly? ClosingDate { get; set; }

	public decimal? OutstandingPrincipal { get; set; }

	public decimal? CalculatedInterest { get; set; }
}
