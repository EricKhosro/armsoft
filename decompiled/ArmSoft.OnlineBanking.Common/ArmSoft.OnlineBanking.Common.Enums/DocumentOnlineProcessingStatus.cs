namespace ArmSoft.OnlineBanking.Common.Enums;

public enum DocumentOnlineProcessingStatus : byte
{
	NotSet,
	NotApplicable,
	Started,
	CommissionChanged,
	DraftCreated,
	Succeeded,
	Failed
}
