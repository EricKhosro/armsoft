using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class PurchasedSecurityPermission
{
	public string ContractNumber { get; set; }

	public PurchasedSecurityAccessLevels AccessLevel { get; set; }
}
