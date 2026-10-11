namespace ArmSoft.OpenBanking.Api.Common;

public enum AuthenticationStatus : byte
{
	Success,
	ChangePasswordRequired,
	SecureAuthenticationRequired,
	VerificationRequired
}
