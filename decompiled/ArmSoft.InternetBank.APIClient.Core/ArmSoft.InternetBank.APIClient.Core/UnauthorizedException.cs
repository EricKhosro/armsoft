using System;
using System.Net;

namespace ArmSoft.InternetBank.APIClient.Core;

public class UnauthorizedException : ApiException
{
	public UnauthorizedException()
		: base(HttpStatusCode.Unauthorized)
	{
	}

	public UnauthorizedException(string message)
		: base(HttpStatusCode.Unauthorized, message)
	{
	}

	public UnauthorizedException(string message, Exception innerException)
		: base(HttpStatusCode.Unauthorized, message, innerException)
	{
	}

	public UnauthorizedException(string message, string content, Exception innerException)
		: base(HttpStatusCode.Unauthorized, content, message, innerException)
	{
	}
}
