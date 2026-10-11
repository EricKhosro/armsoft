using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetCreditOnlineDataListParams
{
	public required List<int> Ids { get; set; }

	public CreditAccessLevels DesiredAccess { get; set; }

	public List<CreditRelationType> RelationTypes { get; set; } = new List<CreditRelationType>
	{
		CreditRelationType.Owner,
		CreditRelationType.Codebtor
	};

	public bool OnlyActive { get; set; }

	public bool IncludeSummary { get; set; }
}
