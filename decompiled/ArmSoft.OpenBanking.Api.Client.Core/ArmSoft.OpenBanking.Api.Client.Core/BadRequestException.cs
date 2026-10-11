using System.Net;

namespace ArmSoft.OpenBanking.Api.Client.Core;

public class BadRequestException : ApiException
{
	public BadRequestException(string? content)
		: base(HttpStatusCode.BadRequest, content)
	{
	}

	public BadRequestException(string responseCode, string? content, string message)
		: base(HttpStatusCode.BadRequest, content, responseCode, message)
	{
	}
}
