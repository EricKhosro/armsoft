using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public abstract class BaseLocalPayment : AccountPayment
{
	public string ReceiverAccount { get; set; } = string.Empty;

	public string TaxCode { get; set; } = string.Empty;

	public string SocialCard { get; set; } = string.Empty;

	public PaymentMode? PaymentMode { get; set; }

	public bool IsRateChanged { get; set; }

	public string? FixedCurrency { get; set; }

	public string CurrencyCredit { get; set; } = string.Empty;

	public decimal AmountCredit { get; set; }

	public decimal? RateValue { get; set; }

	public string? RateCurrency1 { get; set; }

	public short? RateFor { get; set; }

	public string? RateCurrency2 { get; set; }

	public decimal? BankRateValue { get; set; }

	public decimal? BankRateValueByDeviation { get; set; }

	public bool IsUrgentTransfer { get; set; }

	public int? RequestMoneyId { get; set; }
}
