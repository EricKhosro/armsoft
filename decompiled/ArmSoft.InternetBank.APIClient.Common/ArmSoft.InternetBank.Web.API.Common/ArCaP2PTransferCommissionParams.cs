namespace ArmSoft.InternetBank.Web.API.Common;

public class ArCaP2PTransferCommissionParams
{
	public string PayerAccount { get; set; }

	public decimal Amount { get; set; }

	public string Currency { get; set; }
}
