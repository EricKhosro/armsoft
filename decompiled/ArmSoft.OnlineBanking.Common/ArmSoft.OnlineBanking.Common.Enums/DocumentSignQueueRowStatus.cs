namespace ArmSoft.OnlineBanking.Common.Enums;

public enum DocumentSignQueueRowStatus : byte
{
	New,
	Started,
	Signed,
	SignFailed,
	Ignored
}
