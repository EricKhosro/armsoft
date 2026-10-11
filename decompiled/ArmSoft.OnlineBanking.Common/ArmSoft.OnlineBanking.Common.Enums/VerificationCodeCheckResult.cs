namespace ArmSoft.OnlineBanking.Common.Enums;

public enum VerificationCodeCheckResult : byte
{
	NotSet,
	Invalid,
	Expired,
	MaxVerificationAttemptsReached,
	OK
}
