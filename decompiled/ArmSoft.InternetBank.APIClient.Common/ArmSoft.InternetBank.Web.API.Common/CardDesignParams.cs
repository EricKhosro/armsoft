using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class CardDesignParams
{
	public string CardType { get; set; }

	public CardTypeGlobal? GlobalCardType { get; set; }

	public CardDesignParams()
	{
		CardType = null;
		GlobalCardType = null;
	}
}
