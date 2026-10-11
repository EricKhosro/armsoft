namespace ArmSoft.OnlineBanking.Common.Enums;

public enum RateSuggestionStateGroup : byte
{
	NotSet,
	AssignedToAnotherOperator,
	New,
	Assigned,
	InProcess,
	WaitingToBeSigned,
	Completed
}
