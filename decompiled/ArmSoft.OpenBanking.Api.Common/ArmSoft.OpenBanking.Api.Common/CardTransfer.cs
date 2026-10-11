namespace ArmSoft.OpenBanking.Api.Common;

public class CardTransfer : AccountPayment
{
	public string ReceiverCardNumber { get; set; } = string.Empty;

	public string ReceiverCountryCode { get; set; } = string.Empty;
}
