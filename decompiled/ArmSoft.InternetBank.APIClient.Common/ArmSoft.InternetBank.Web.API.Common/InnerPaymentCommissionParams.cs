using System.ComponentModel;

namespace ArmSoft.InternetBank.Web.API.Common;

public class InnerPaymentCommissionParams
{
	public string PayerAccount { get; set; }

	public string ReceiverAccount { get; set; }

	public decimal Amount { get; set; }

	public string Currency { get; set; }

	[DefaultValue("")]
	public string ChargeAccountCurrency { get; set; }
}
