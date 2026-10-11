namespace ArmSoft.OnlineBanking.Common.Enums;

public enum PasswordViolationReason : byte
{
	ContentPolicyViolation,
	HistoryPolicyViolation,
	UsernameSimilarityPolicyViolation
}
