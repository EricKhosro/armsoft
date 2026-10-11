using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CardType
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string EName { get; set; }

	public bool IsDigital { get; set; }

	public CardTypeGlobal GlobalCardType { get; set; }
}
