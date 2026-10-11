using System;
using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class Deposit
{
	public int Id { get; set; }

	public string ContractNo { get; set; }

	public string DisplayContract { get; set; }

	public string Category { get; set; }

	public string DisplayCategory { get; set; }

	public string Currency { get; set; }

	public string Name { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime OriginalDate { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime MaturityDate { get; set; }

	public decimal OriginalPrincipal { get; set; }

	public string InterestRate { get; set; }

	public string Note { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public decimal? OutstandingPrincipal { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public decimal? CalculatedInterest { get; set; }

	public decimal? TotalInterest { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime? ClosingDate { get; set; }

	public bool IsReplenishable { get; set; }

	public bool IsCapitalizable { get; set; }

	public bool IsExtendable { get; set; }

	public InterestPayPeriod PayPeriod { get; set; }

	public List<Link> Links { get; set; }

	public decimal MaxWithdrawablePercent { get; set; }

	public bool IsWithdrawable { get; set; }

	public bool IsTerminatable { get; set; }

	public string DepositReceiveAccount { get; set; }

	public string PercentAmountReceiveAccount { get; set; }
}
