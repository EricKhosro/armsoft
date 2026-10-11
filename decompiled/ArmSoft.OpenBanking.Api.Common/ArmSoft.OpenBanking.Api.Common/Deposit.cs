using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class Deposit : BaseContract
{
	public required string Category { get; set; }

	public required string DisplayCategory { get; set; }

	public bool IsReplenishable { get; set; }

	public bool IsCapitalizable { get; set; }

	public bool IsExtendable { get; set; }

	public InterestPayPeriod PayPeriod { get; set; }
}
