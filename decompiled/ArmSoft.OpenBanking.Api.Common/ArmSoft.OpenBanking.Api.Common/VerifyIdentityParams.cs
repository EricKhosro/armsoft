namespace ArmSoft.OpenBanking.Api.Common;

public class VerifyIdentityParams
{
	public required string ClientId { get; set; }

	public required string Secret { get; set; }

	public required string State { get; set; }
}
