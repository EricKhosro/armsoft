using System.ComponentModel;

namespace ArmSoft.InternetBank.Web.API.Common;

public class OuterTransferCommissionParams
{
	public string PayerAccount { get; set; }

	public string ReceiverAccount { get; set; }

	public decimal Amount { get; set; }

	public string Currency { get; set; }

	public bool IsUrgentTransfer { get; set; }

	public string DocumentType { get; set; }

	[DefaultValue("")]
	public string ChargeAccountCurrency { get; set; }

	public OuterTransferCommissionParams()
	{
		IsUrgentTransfer = false;
	}
}
