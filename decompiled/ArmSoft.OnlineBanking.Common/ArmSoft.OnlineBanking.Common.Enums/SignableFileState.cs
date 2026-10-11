namespace ArmSoft.OnlineBanking.Common.Enums;

public enum SignableFileState : byte
{
	New,
	SentToClient,
	SentToOperator,
	Uploaded,
	RejectedByOperator,
	RejectedByClient,
	Expired,
	Approved
}
