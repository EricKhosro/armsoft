namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ArCaP2PReceiverInfo
{
	public string CurrentBankArCaCode { get; set; }

	public string ReceiverBankArCaCode { get; set; }

	public string ReceiverBankName { get; set; }

	public string ReceiverName { get; set; }

	public string TransactionRelativeNumber { get; set; }
}
