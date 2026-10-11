namespace ArmSoft.OnlineBanking.Common.Enums;

public enum ClientLimitCategory : byte
{
	Limits = 1,
	Exceptions,
	LimitsWithoutSecurityProvider,
	ExceptionsWithoutSecurityProvider
}
