using System;
using ArmSoft.OnlineBanking.Common.Attributes;

namespace ArmSoft.OnlineBanking.Common.Enums;

public enum UserActionType : byte
{
	[UserAction(UserActionCategory.NotSet, UserActionReason.NotSet)]
	NotSet = 0,
	[UserAction(UserActionCategory.LoginSucceeded, UserActionReason.Successfull)]
	LoginSucceeded = 1,
	[UserAction(UserActionCategory.LoginFailed, UserActionReason.InvalidTokenResponse)]
	LoginFailedInvalidTokenResponse = 2,
	[UserAction(UserActionCategory.LogOut, UserActionReason.Successfull)]
	LogOut = 3,
	[UserAction(UserActionCategory.PasswordChangeSucceeded, UserActionReason.Successfull)]
	PasswordChangeSucceeded = 4,
	[UserAction(UserActionCategory.PasswordChangeFailed, UserActionReason.InvalidOldPassword)]
	PasswordChangeFailedInvalidOldPassword = 5,
	[UserAction(UserActionCategory.LoginFailed, UserActionReason.InvalidPassword)]
	LoginFailedInvalidPassword = 7,
	[UserAction(UserActionCategory.PasswordResetSucceeded, UserActionReason.Successfull)]
	[Obsolete("This action is not supported")]
	PasswordResetSucceeded_Old = 8,
	[UserAction(UserActionCategory.UserLocked, UserActionReason.NotSet)]
	UserWasLocked = 9,
	[UserAction(UserActionCategory.LoginFailed, UserActionReason.NotSet)]
	LoginFailedMustChangePassword = 10,
	[UserAction(UserActionCategory.UserUnlocked, UserActionReason.NotSet)]
	UserWasAutoUnlocked = 11,
	[UserAction(UserActionCategory.UserUnlocked, UserActionReason.NotSet)]
	UserWasUnlocked = 12,
	[UserAction(UserActionCategory.UsernameChanged, UserActionReason.NotSet)]
	UsernameWasChanged = 13,
	[UserAction(UserActionCategory.LoginFailed, UserActionReason.UnallowedIP)]
	LoginFailedUnallowableIPAddress = 14,
	[UserAction(UserActionCategory.LoginFailed, UserActionReason.ExpiredPassword)]
	LoginFailedPasswordExpired = 15,
	[Obsolete("This action is not supported")]
	[UserAction(UserActionCategory.PasswordResetSucceeded, UserActionReason.Successfull)]
	PasswordResetByUserSucceeded_Old = 16,
	[UserAction(UserActionCategory.PasswordResetFailed, UserActionReason.InvalidInformation)]
	PasswordResetByUserFailedInvalidInfo = 17,
	[UserAction(UserActionCategory.UserUnlocked, UserActionReason.NotSet)]
	UserWasUnlockedByUser = 18,
	[Obsolete("This action is not supported")]
	[UserAction(UserActionCategory.ResetEmailSent, UserActionReason.NotSet)]
	ResetEmailWasSentToUser_Old = 19,
	[UserAction(UserActionCategory.SigningFailed, UserActionReason.InvalidTokenResponse)]
	SigningFailedInvalidTokenResponse = 20,
	[UserAction(UserActionCategory.PasswordResetSucceeded, UserActionReason.Successfull)]
	PasswordResetByAdminWithUsernameSucceeded = 21,
	[Obsolete("This action is not supported")]
	[UserAction(UserActionCategory.PasswordResetSucceeded, UserActionReason.Successfull)]
	PasswordResetByAdminWithPasswordPackageSucceeded_Old = 22,
	[UserAction(UserActionCategory.PasswordResetSucceeded, UserActionReason.Successfull)]
	PasswordResetByAdminWithNewPasswordSucceeded = 23,
	[UserAction(UserActionCategory.PasswordResetSucceeded, UserActionReason.Successfull)]
	PasswordResetByUserUsingEmailSucceeded = 24,
	[UserAction(UserActionCategory.PasswordResetSucceeded, UserActionReason.Successfull)]
	PasswordResetByUserUsingSMSSucceeded = 25,
	[UserAction(UserActionCategory.VerificationSucceeded, UserActionReason.Successfull)]
	UserVerificationUsingEmailSucceeded = 26,
	[UserAction(UserActionCategory.VerificationSucceeded, UserActionReason.Successfull)]
	UserVerificationUsingSMSSucceeded = 27,
	[UserAction(UserActionCategory.PasswordResetFailed, UserActionReason.InvalidVerificationCode)]
	PasswordResetByUserFailedInvalidVerificationCode = 28,
	[UserAction(UserActionCategory.VerificationFailed, UserActionReason.InvalidVerificationCode)]
	UserVerificationFailedInvalidVerificationCode = 29,
	[UserAction(UserActionCategory.VerificationSent, UserActionReason.NotSet)]
	VerificationEmailWasSentToUser = 30,
	[UserAction(UserActionCategory.VerificationSent, UserActionReason.NotSet)]
	VerificationSMSWasSentToUser = 31,
	[UserAction(UserActionCategory.LoginSucceeded, UserActionReason.Successfull)]
	LoginWithQrCodeSucceeded = 32,
	[UserAction(UserActionCategory.SecurityProviderActivationSucceeded, UserActionReason.Successfull)]
	SecurityProviderActivationSucceeded = 33,
	[UserAction(UserActionCategory.SecurityProviderDeactivationSucceeded, UserActionReason.Successfull)]
	SecurityProviderDeactivationSucceeded = 34,
	[UserAction(UserActionCategory.ClientNotActive, UserActionReason.NotSet)]
	ClientIsNotActive = 35,
	[UserAction(UserActionCategory.LoginSucceeded, UserActionReason.Successfull)]
	LoginWithFingerprintSucceeded = 36,
	[UserAction(UserActionCategory.LoginSucceeded, UserActionReason.Successfull)]
	LoginWithPinSucceeded = 37,
	[UserAction(UserActionCategory.SecurityProviderActivationFailed, UserActionReason.InvalidVerificationCode)]
	SecurityProviderActivationFailedInvalidVerificationCode = 38,
	[UserAction(UserActionCategory.SecurityProviderChanged, UserActionReason.NotSet)]
	SecurityProviderChangedByUser = 39,
	[UserAction(UserActionCategory.LoginSucceeded, UserActionReason.Successfull)]
	LoginFromAssociatedUserSucceeded = 40,
	[UserAction(UserActionCategory.LoginSucceeded, UserActionReason.Successfull)]
	AutologinSucceeded = 41,
	[UserAction(UserActionCategory.VerificationFailed, UserActionReason.ExpiredVerificationCode)]
	EmailVerificationFailedSystemVerificationCodeExpired = 42,
	[UserAction(UserActionCategory.VerificationFailed, UserActionReason.ExpiredVerificationCode)]
	PasswordResetFailedSystemVerificationCodeExpired = 43,
	[UserAction(UserActionCategory.SecurityProviderActivationFailed, UserActionReason.ExpiredVerificationCode)]
	SecurityProviderActivationFailedSystemVerificationCodeExpired = 44,
	[UserAction(UserActionCategory.VerificationFailed, UserActionReason.ExpiredVerificationCode)]
	SmsVerificationFailedSystemVerificationCodeExpired = 45,
	[UserAction(UserActionCategory.LoginSucceeded, UserActionReason.Successfull)]
	LoginWithAppAuthAccessKeySucceeded = 46,
	[UserAction(UserActionCategory.LoginFailed, UserActionReason.InvalidAccessKey)]
	LoginWithAppAuthAccessKeyFailedInvalidAccessKey = 47,
	[UserAction(UserActionCategory.UserLocked, UserActionReason.WrongPasswordsExcess)]
	UserWasLockedPasswordAttemptsCountExceeded = 48,
	[UserAction(UserActionCategory.UserLocked, UserActionReason.InvalidVerificationCode)]
	UserWasLockedInvalidVerificationCode = 49,
	[UserAction(UserActionCategory.RefreshSecurityProviderDataFailed, UserActionReason.ExpiredVerificationCode)]
	RefreshSecurityProviderDataFailedSystemVerificationCodeExpired = 50,
	[UserAction(UserActionCategory.RefreshSecurityProviderDataSucceeded, UserActionReason.Successfull)]
	RefreshSecurityProviderDataAndAutoLogin = 51,
	[UserAction(UserActionCategory.LoginFailed, UserActionReason.NotAllowed)]
	LoginFailedNotAllowed = 52,
	[UserAction(UserActionCategory.LoginFailed, UserActionReason.SystemCodeExpired)]
	LoginFailedSystemCodeExpired = 53,
	[UserAction(UserActionCategory.LoginFailed, UserActionReason.RepeatedSecurityProviderResponse)]
	LoginFailedSecurityProviderResponseRepeated = 54,
	[UserAction(UserActionCategory.LoginFailed, UserActionReason.ExpiredSecurityProviderResponse)]
	LoginFailedSecurityProviderResponseExpired = 55,
	[UserAction(UserActionCategory.RetrievingUserDataFailed, UserActionReason.SystemCodeExpired)]
	RetrievingUserInformationFailedSystemCodeInvalid = 56,
	[UserAction(UserActionCategory.RetrievingUserDataSucceeded, UserActionReason.Successfull)]
	RetrievingUserInformation = 57,
	[UserAction(UserActionCategory.UserLocked, UserActionReason.InvalidTokenResponse)]
	UserWasLockedOtpAttemptsCountExceeded = 58,
	[UserAction(UserActionCategory.SecurityProviderChangeFailed, UserActionReason.ExpiredVerificationCode)]
	SecurityProviderChangeFailedSystemVerificationCodeExpired = 59
}
