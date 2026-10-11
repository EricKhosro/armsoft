using System.ComponentModel;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetBondsParams
{
	[DefaultValue(BondAccessLevels.View)]
	public BondAccessLevels DesiredAccess { get; set; } = BondAccessLevels.View;

	public bool OnlyActive { get; set; }
}
