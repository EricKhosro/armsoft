using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AccountPermission
{
	public string Number { get; set; }

	public AccountAccessLevels AccessLevel { get; set; }
}
