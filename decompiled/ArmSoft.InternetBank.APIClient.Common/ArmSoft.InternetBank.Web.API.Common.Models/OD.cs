using System;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class OD : Application
{
	public string PayerAcc { get; set; }

	public string PercentAcc { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public decimal Amount { get; set; }

	public string Currency { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime MaturityDate { get; set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public int DurationDays { get; set; }

	public decimal FixedRate { get; set; }

	public string DepositTemplateCode { get; set; }

	public string DepositTemplateName { get; set; }

	public DepositTemplate CurrentDepositTemplate { get; set; }

	public string DepositTermsURL { get; set; }

	public bool IsDepositTermExtendable { get; set; }

	public bool? IsDepositCapitalizable { get; set; }

	public string BranchCode { get; set; }

	public bool? IsDepositReplenishable { get; set; }

	public InterestPayPeriod PayPeriodCode { get; set; }

	public OD()
	{
		BranchCode = null;
	}
}
