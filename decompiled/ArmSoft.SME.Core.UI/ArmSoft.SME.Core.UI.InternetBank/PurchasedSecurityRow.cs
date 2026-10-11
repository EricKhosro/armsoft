using System;

namespace ArmSoft.SME.Core.UI.InternetBank;

public class PurchasedSecurityRow
{
	public string ContractNumber { get; set; }

	public string IssuanceNumber { get; set; }

	public string Type { get; set; } = "";

	public string Currency { get; set; } = "";

	public decimal? InterestRate { get; set; }

	public decimal NominalValue { get; set; }

	public decimal FreePartAmount { get; set; }

	public int? Count { get; set; }

	public decimal PledgedPartAmount { get; set; }

	public DateOnly IssueDate { get; set; }

	public DateOnly PurchaseDate { get; set; }

	public DateOnly MaturityDate { get; set; }

	public string PayPeriodName { get; set; }
}
