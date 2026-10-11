using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CardSetting
{
	public bool Receive3DSecureMessagesViaSms { get; set; }

	public CardReceiveTransactionInfoOption TransactionInfoReceivingOption { get; set; }

	public bool Receive3DSecureMessagesViaEmail { get; set; }

	public decimal? TransactionMinAmount { get; set; }

	public string Mobile { get; set; }

	public CardSmsFormat SmsFormat { get; set; }

	public CardSmsLanguage SmsLanguage { get; set; }

	public string Email { get; set; }

	public CardSetting()
	{
		Receive3DSecureMessagesViaSms = true;
		TransactionInfoReceivingOption = CardReceiveTransactionInfoOption.NotToReceive;
		Receive3DSecureMessagesViaEmail = false;
		TransactionMinAmount = null;
		Mobile = string.Empty;
		SmsFormat = CardSmsFormat.NotSet;
		SmsLanguage = CardSmsLanguage.NotSet;
		Email = string.Empty;
	}
}
