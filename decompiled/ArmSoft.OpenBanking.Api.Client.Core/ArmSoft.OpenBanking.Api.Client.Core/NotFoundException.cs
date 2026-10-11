using System.Net;

namespace ArmSoft.OpenBanking.Api.Client.Core;

public class NotFoundException : ApiException
{
	public NotFoundException(string? content)
		: base(HttpStatusCode.NotFound, content)
	{
	}

	public NotFoundException(string responseCode, string? content, string message)
		: base(HttpStatusCode.NotFound, content, responseCode, message)
	{
	}
}
