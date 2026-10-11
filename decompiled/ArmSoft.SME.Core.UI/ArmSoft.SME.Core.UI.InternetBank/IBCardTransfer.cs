namespace ArmSoft.SME.Core.UI.InternetBank;

public class IBCardTransfer : IBPayment
{
	public string ReceiverCardNumber { get; set; }

	public string ReceiverCountryCode { get; set; }

	public decimal CommissionAmount { get; set; }

	public string ChargeAcc { get; set; }
}
