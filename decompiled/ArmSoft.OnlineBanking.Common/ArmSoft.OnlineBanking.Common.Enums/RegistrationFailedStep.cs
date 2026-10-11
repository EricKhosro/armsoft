namespace ArmSoft.OnlineBanking.Common.Enums;

public enum RegistrationFailedStep : byte
{
	NotSet,
	BankInformation,
	MobileVerification,
	EmailVerification,
	UsernamePassword,
	RequestVerification,
	BiometricCheck
}
