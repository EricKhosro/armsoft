namespace ArmSoft.InternetBank.Web.API.Common;

public class CardParams
{
	public bool IncludeAccountBalances { get; set; }

	public string MainCardNumber { get; set; }

	public int? SubUserId { get; set; }

	public CardParams()
	{
		IncludeAccountBalances = true;
		MainCardNumber = null;
	}
}
