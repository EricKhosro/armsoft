using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetDepositOnlineDataListParams
{
	public required List<int> Ids { get; set; }

	public DepositAccessLevels DesiredAccess { get; set; }

	public bool OnlyActive { get; set; }

	public bool IncludeSummary { get; set; }
}
