namespace ArmSoft.OpenBanking.Api.Client.Core;

public class ClientVersionException : ForbiddenException
{
	public ClientVersionException(string? content)
		: base("unsupported_version", content, "The installed version of the client is outdated")
	{
	}
}
