namespace ArmSoft.OnlineBanking.Common.Enums;

public enum RateSuggestionOperationType : byte
{
	NotSet,
	Created,
	Assigned,
	Suggested,
	AcceptedByClient,
	Signed,
	ConfirmedByBank,
	RefusedByClient,
	RefusedByOperator,
	RefusedByBank,
	Expired
}
