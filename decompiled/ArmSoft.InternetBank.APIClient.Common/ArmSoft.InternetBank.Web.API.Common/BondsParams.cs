using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class BondsParams
{
	public BondAccessLevels DesiredAccess { get; set; }

	public BondsParams()
	{
		DesiredAccess = BondAccessLevels.View;
	}
}
