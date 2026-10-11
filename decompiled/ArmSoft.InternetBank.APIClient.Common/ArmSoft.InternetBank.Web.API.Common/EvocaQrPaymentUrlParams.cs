namespace ArmSoft.InternetBank.Web.API.Common;

public class EvocaQrPaymentUrlParams
{
	public EvocaQrPaymentUrlType Type { get; set; }

	public string ReceiverId { get; set; }

	public decimal Amount { get; set; }

	public EvocaQrPaymentUrlParams()
	{
		Type = EvocaQrPaymentUrlType.NotSet;
		ReceiverId = string.Empty;
		Amount = 0m;
	}
}
