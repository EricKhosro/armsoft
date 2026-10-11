using System.Text.Json.Serialization;

namespace ArmSoft.OpenBanking.Api.Common;

public class AuthenticationResponse
{
	[JsonPropertyName("access_token")]
	public required string AccessToken { get; set; }

	[JsonPropertyName("refresh_token")]
	public required string RefreshToken { get; set; }

	[JsonPropertyName("bank_code")]
	public string? BankCode { get; set; }
}
