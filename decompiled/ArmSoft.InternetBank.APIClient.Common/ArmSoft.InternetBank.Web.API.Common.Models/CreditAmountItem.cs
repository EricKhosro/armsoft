namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CreditAmountItem
{
	public string Currency { get; set; }

	public decimal Amount { get; set; }

	public decimal AmountInPaymentCurrency { get; set; }
}
