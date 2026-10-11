namespace ArmSoft.OnlineBanking.Common.Enums;

public enum OperationProcessingResult : byte
{
	NotSet,
	OperationAlreadyRejected,
	OperationAlreadyCompleted,
	OperationProcessingFailed,
	OperationTimeExpired
}
