using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CreditPermission
{
	public string ContractNumber { get; set; }

	public CreditAccessLevels AccessLevel { get; set; }
}
