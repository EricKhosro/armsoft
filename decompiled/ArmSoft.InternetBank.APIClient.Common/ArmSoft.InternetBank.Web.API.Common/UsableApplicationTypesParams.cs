using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class UsableApplicationTypesParams
{
	public CustomizationObjectType ObjectType { get; set; }

	public string CardType { get; set; }

	public string AccountType { get; set; }

	public string Currency { get; set; }

	public string ContractNumber { get; set; }
}
