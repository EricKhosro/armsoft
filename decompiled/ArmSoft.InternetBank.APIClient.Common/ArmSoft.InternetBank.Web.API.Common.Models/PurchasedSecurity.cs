using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class PurchasedSecurity
{
	public int Id { get; set; }

	public string ContractNumber { get; set; }

	public string IssuanceNumber { get; set; }

	public string Type { get; set; }

	public string Currency { get; set; }

	public decimal? InterestRate { get; set; }

	public int InterestRateDivisor { get; set; }

	public decimal NominalValue { get; set; }

	public int? Count { get; set; }

	public decimal FreePartAmount { get; set; }

	public decimal PledgedPartAmount { get; set; }

	public DateTime IssueDate { get; set; }

	public DateTime PurchaseDate { get; set; }

	public DateTime MaturityDate { get; set; }

	public PurchasedSecurityStatehood Statehood { get; set; }

	public InterestPayPeriod PayPeriod { get; set; }
}
