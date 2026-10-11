namespace ArmSoft.OpenBanking.Api.Common;

public class RefreshRequestParams
{
	public required string ClientId { get; set; }

	public required string Secret { get; set; }

	public required string RefreshToken { get; set; }
}
