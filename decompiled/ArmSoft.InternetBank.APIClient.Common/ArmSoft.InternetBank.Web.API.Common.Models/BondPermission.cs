using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class BondPermission
{
	public string ContractNumber { get; set; }

	public BondAccessLevels AccessLevel { get; set; }
}
