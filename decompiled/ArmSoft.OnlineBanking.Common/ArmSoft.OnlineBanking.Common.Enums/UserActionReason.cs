namespace ArmSoft.OnlineBanking.Common.Enums;

public enum UserActionReason : byte
{
	NotSet,
	Successfull,
	InvalidTokenResponse,
	InvalidPassword,
	InvalidOldPassword,
	ExpiredPassword,
	WrongPasswordsExcess,
	UnallowedIP,
	InvalidInformation,
	InvalidVerificationCode,
	ExpiredVerificationCode,
	InvalidAccessKey,
	NotAllowed,
	SystemCodeExpired,
	RepeatedSecurityProviderResponse,
	ExpiredSecurityProviderResponse
}
