using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class IssuedBond
{
	public string IssuanceNumber { get; set; }

	public BondIssuanceType IssuanceType { get; set; }

	public long Count { get; set; }

	public long AllocatedCount { get; set; }

	public string Currency { get; set; }

	public decimal InterestRate { get; set; }

	public DateTime MaturityDate { get; set; }

	public DateTime IssueDate { get; set; }

	public DateTime? AllocatedEndDate { get; set; }

	public decimal Volume { get; set; }

	public decimal AllocatedVolume { get; set; }

	public decimal NominalValue { get; set; }

	public decimal UnitPrice { get; set; }

	public InterestPayPeriod PayPeriod { get; set; }
}
