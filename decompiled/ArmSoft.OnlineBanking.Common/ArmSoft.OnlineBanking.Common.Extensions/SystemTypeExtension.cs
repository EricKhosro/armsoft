using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Extensions;

public static class SystemTypeExtension
{
	public static bool IsWebApiClient(this SystemType systemType)
	{
		if (systemType != SystemType.WebApiClient && systemType != SystemType.ASBank && systemType != SystemType.ASEnterprise && systemType != SystemType.ASHumanResourceManagement && systemType != SystemType.ASAccountantApplication && systemType != SystemType.ASBookkeeper && systemType != SystemType.ASSalesManagement && systemType != SystemType.ASOrderApplication && systemType != SystemType.ASTrade && systemType != SystemType.ASShop && systemType != SystemType.ASMarketPlace)
		{
			return systemType == SystemType.ASFinMobile;
		}
		return true;
	}
}
