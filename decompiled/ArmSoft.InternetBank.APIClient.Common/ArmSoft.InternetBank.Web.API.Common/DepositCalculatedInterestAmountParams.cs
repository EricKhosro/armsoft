using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class DepositCalculatedInterestAmountParams
{
	public DateTime DateFrom { get; set; }

	public DateTime DateTo { get; set; }

	public decimal Amount { get; set; }

	public string Currency { get; set; }

	public bool Capitalize { get; set; }

	public InterestPayPeriod PayPeriod { get; set; }

	public string PassOverDirection { get; set; }

	public string PassOverType { get; set; }

	public string FloatingRateCode { get; set; }

	public int RateDivisor { get; set; }

	public decimal Rate { get; set; }
}
