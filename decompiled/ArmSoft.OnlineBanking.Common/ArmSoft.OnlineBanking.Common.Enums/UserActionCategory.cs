namespace ArmSoft.OnlineBanking.Common.Enums;

public enum UserActionCategory : byte
{
	NotSet,
	LoginSucceeded,
	LoginFailed,
	LogOut,
	PasswordChangeSucceeded,
	PasswordChangeFailed,
	PasswordResetSucceeded,
	PasswordResetFailed,
	UserLocked,
	UserUnlocked,
	UsernameChanged,
	ResetEmailSent,
	VerificationSent,
	VerificationSucceeded,
	VerificationFailed,
	SecurityProviderActivationSucceeded,
	SecurityProviderActivationFailed,
	SecurityProviderDeactivationSucceeded,
	SecurityProviderChanged,
	RefreshSecurityProviderDataFailed,
	RefreshSecurityProviderDataSucceeded,
	ClientNotActive,
	SigningFailed,
	RetrievingUserDataFailed,
	RetrievingUserDataSucceeded,
	SecurityProviderChangeFailed
}
