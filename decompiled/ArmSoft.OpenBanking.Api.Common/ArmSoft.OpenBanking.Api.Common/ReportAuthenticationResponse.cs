using System.Text.Json.Serialization;

namespace ArmSoft.OpenBanking.Api.Common;

public class ReportAuthenticationResponse
{
	[JsonPropertyName("access_token")]
	public required string AccessToken { get; set; }

	[JsonPropertyName("refresh_token")]
	public string? RefreshToken { get; set; }

	[JsonPropertyName("token_type")]
	public string TokenType { get; set; } = "Bearer";

	[JsonPropertyName("expires_in")]
	public int ExpiresIn { get; set; }

	public required string Username { get; set; }

	public required string BankCode { get; set; }

	public bool RequiresPasswordChange { get; set; }
}
