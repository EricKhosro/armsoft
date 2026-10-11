namespace ArmSoft.OpenBanking.Api.Common;

public class CreditAmountItem
{
	public string Currency { get; set; } = string.Empty;

	public decimal Amount { get; set; }

	public decimal AmountInPaymentCurrency { get; set; }
}
