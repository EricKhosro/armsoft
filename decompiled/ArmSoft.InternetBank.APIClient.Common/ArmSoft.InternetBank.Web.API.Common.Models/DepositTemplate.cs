using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class DepositTemplate
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string Currency { get; set; }

	public decimal FixedRate { get; set; }

	public int RateDeviation { get; set; }

	public string DisplayFixedRate { get; set; }

	public string FloatingRate { get; set; }

	public string FloatingRateCode { get; set; }

	public string PassOverDirection { get; set; }

	public string PassOverType { get; set; }

	public InterestPayPeriod PayPeriodCode { get; set; }

	public string PayPeriod { get; set; }

	public decimal AmountLower { get; set; }

	public decimal AmountUpper { get; set; }

	public int PeriodLower { get; set; }

	public int PeriodUpper { get; set; }

	public string PeriodLowerText { get; set; }

	public string PeriodUpperText { get; set; }

	public bool Capitalize { get; set; }

	public bool IsCapitalizeRequired { get; set; }

	public bool Replenish { get; set; }

	public decimal Interest { get; set; }

	public decimal Profit { get; set; }

	public decimal MaxWithdrawablePercent { get; set; }

	public bool IsWithdrawable { get; set; }

	public bool IsTerminatable { get; set; }
}
