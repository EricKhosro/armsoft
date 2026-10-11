using System.Collections.Generic;
using System.ComponentModel;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetCreditsParams
{
	[DefaultValue(CreditAccessLevels.View)]
	public CreditAccessLevels DesiredAccess { get; set; } = CreditAccessLevels.View;

	public List<CreditType>? CreditTypes { get; set; } = new List<CreditType>
	{
		CreditType.Standard,
		CreditType.Line,
		CreditType.Overdraft,
		CreditType.Leasing,
		CreditType.Factoring
	};

	public List<CreditRelationType> RelationTypes { get; set; } = new List<CreditRelationType>
	{
		CreditRelationType.Owner,
		CreditRelationType.Codebtor
	};

	public bool OnlyActive { get; set; } = true;
}
