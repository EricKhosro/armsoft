using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class DepositPermission
{
	public string ContractNumber { get; set; }

	public DepositAccessLevels AccessLevel { get; set; }
}
