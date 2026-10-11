using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class AuthenticationResult
{
	public AuthenticationStatus Status { get; set; }

	public string? SecurityProvider { get; set; }

	public int SecurityProviderCodeLength { get; set; }

	public string? Username => Identity?.Username;

	public string? AuthenticationMode { get; set; }

	public string? TokenSerial { get; set; }

	public string? Challenge { get; set; }

	public string? OtpDescription { get; set; }

	public ChangePasswordReason ChangePasswordReason { get; set; }

	public SystemVerification? SystemVerification { get; set; }

	public string AccessToken { get; set; } = string.Empty;

	public string RefreshToken { get; set; } = string.Empty;

	public Identity? Identity { get; set; }
}
