namespace ArmSoft.InternetBank.Web.API.Common;

public class CommissionParams
{
	public string PayerAccount { get; set; }

	public string ReceiverAccount { get; set; }

	public decimal Amount { get; set; }

	public string Currency { get; set; }

	public string CreditCurrency { get; set; }

	public bool IsUrgentTransfer { get; set; }

	public CommissionParams()
	{
		IsUrgentTransfer = false;
	}
}
