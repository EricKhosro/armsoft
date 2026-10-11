using System.Net;

namespace ArmSoft.OpenBanking.Api.Client.Core;

public class ForbiddenException : ApiException
{
	public ForbiddenException(string? content)
		: base(HttpStatusCode.Forbidden, content)
	{
	}

	public ForbiddenException(string responseCode, string? content, string message)
		: base(HttpStatusCode.Forbidden, content, responseCode, message)
	{
	}
}
