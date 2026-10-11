namespace ArmSoft.OnlineBanking.Common.Enums;

public enum RateSuggestionState : byte
{
	NotSet,
	New,
	Assigned,
	Started,
	AcceptedByClient,
	Signed,
	ConfirmedByBank,
	RefusedByClient,
	RefusedByOperator,
	RefusedByBank,
	Expired
}
