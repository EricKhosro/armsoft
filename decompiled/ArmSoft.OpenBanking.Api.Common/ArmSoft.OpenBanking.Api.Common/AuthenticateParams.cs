namespace ArmSoft.OpenBanking.Api.Common;

public class AuthenticateParams
{
	public string Username { get; set; } = string.Empty;

	public string Password { get; set; } = string.Empty;

	public string? OTP { get; set; }

	public bool IsOtpGeneratorIntegrated { get; set; }
}
