namespace ArmSoft.OpenBanking.Api.Common;

public class AuthenticationParams
{
	public required string ClientId { get; set; }

	public required string Secret { get; set; }

	public required string AuthenticationCode { get; set; }
}
