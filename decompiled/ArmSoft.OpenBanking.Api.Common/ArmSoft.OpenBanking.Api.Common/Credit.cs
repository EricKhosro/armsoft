using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class Credit : BaseContract
{
	public required string Category { get; set; }

	public required string DisplayCategory { get; set; }

	public CreditRelationType Relation { get; set; }

	public CreditType Type { get; set; }
}
