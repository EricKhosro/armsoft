using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class DigitalCardType
{
	public string Currency { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public CardTypeGlobal CardTypeGlobal { get; set; }

	public string ShortDescription { get; set; }

	public string Description { get; set; }
}
