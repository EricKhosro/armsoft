using System.Net;

namespace ArmSoft.OpenBanking.Api.Client.Core;

public class UnauthorizedException : ApiException
{
	public UnauthorizedException(string? content)
		: base(HttpStatusCode.Unauthorized, content)
	{
	}

	public UnauthorizedException(string responseCode, string? content, string message)
		: base(HttpStatusCode.Unauthorized, content, responseCode, message)
	{
	}
}
