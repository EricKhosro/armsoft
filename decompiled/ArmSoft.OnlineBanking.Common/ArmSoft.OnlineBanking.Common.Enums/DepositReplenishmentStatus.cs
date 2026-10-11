namespace ArmSoft.OnlineBanking.Common.Enums;

public enum DepositReplenishmentStatus : short
{
	Allowed,
	NotAllowed,
	AllowedUntilDate,
	MinimumRequiredAmount,
	MaximumExceededAmount,
	LimitExceeded
}
