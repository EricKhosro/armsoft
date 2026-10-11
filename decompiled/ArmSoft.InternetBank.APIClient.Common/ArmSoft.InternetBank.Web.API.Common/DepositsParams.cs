using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class DepositsParams
{
	public DepositAccessLevels DesiredAccess { get; set; }

	public DepositsParams()
	{
		DesiredAccess = DepositAccessLevels.View;
	}
}
