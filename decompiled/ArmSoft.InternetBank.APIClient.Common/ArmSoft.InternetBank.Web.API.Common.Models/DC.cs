using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class DC : Application
{
	public string CardDesign { get; set; }

	public int? CardDesignId { get; set; }

	public string Currency { get; set; }

	public CardTypeGlobal CardTypeGlobal { get; set; }

	public string CardType { get; set; }

	public string CardTypeDisplayName { get; set; }
}
