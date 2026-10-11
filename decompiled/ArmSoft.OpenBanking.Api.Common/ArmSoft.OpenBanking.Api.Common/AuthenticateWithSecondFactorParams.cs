namespace ArmSoft.OpenBanking.Api.Common;

public class AuthenticateWithSecondFactorParams : UserProcessActionParams
{
	public string OTP { get; set; } = string.Empty;

	public bool IsOtpGeneratorIntegrated { get; set; }
}
