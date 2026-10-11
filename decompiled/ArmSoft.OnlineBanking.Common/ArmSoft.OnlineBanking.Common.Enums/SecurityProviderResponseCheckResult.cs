namespace ArmSoft.OnlineBanking.Common.Enums;

public enum SecurityProviderResponseCheckResult : byte
{
	Valid = 1,
	Invalid,
	Repeated,
	Expired
}
