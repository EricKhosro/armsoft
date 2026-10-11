using System.ComponentModel;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetDepositsParams
{
	[DefaultValue(DepositAccessLevels.View)]
	public DepositAccessLevels DesiredAccess { get; set; } = DepositAccessLevels.View;

	public bool OnlyActive { get; set; } = true;
}
