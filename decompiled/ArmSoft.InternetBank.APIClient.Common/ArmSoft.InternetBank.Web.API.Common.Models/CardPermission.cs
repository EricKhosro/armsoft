using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CardPermission
{
	public string Number { get; set; }

	public CardAccessLevels AccessLevel { get; set; }
}
