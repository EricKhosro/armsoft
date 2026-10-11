namespace ArmSoft.OpenBanking.Api.Common;

public class VerifyClientResponse
{
	public required string ClientApplicationName { get; set; }

	public required SystemType ClientApplicationSystemType { get; set; }

	public string? ClientApplicationUrlHost { get; set; }
}
