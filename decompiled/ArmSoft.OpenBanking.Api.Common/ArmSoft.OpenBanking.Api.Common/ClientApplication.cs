namespace ArmSoft.OpenBanking.Api.Common;

public class ClientApplication
{
	public required string Id { get; set; }

	public required string Name { get; set; }

	public string? ReturnUrlHost { get; set; }

	public required SystemType System { get; set; }
}
