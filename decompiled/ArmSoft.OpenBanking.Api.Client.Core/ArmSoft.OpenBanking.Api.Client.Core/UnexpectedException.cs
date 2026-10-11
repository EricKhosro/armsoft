using System.Net;

namespace ArmSoft.OpenBanking.Api.Client.Core;

public class UnexpectedException : ApiException
{
	public UnexpectedException(HttpStatusCode statusCode, string? content)
		: base(statusCode, content)
	{
	}

	public UnexpectedException(HttpStatusCode statusCode, string? content, string message)
		: base(statusCode, content, string.Empty, message)
	{
	}
}
