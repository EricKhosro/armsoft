namespace ArmSoft.InternetBank.Web.API.Common;

public class CashOutCommissionParams
{
	public string PayerAccount { get; set; }

	public decimal Amount { get; set; }

	public string Currency { get; set; }
}
