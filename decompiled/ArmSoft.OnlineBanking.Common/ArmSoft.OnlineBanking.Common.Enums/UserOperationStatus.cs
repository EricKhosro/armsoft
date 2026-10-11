namespace ArmSoft.OnlineBanking.Common.Enums;

public enum UserOperationStatus : byte
{
	NotSet,
	Active,
	InProgress,
	Sent,
	Reactivated,
	Confirmed,
	Rejected,
	Failed,
	Inactive
}
