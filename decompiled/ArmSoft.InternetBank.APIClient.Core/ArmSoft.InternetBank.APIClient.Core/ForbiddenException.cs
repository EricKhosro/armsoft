using System;
using System.Net;

namespace ArmSoft.InternetBank.APIClient.Core;

public class ForbiddenException : ApiException
{
	public ForbiddenException()
		: base(HttpStatusCode.Forbidden)
	{
	}

	public ForbiddenException(string message)
		: base(HttpStatusCode.Forbidden, message)
	{
	}

	public ForbiddenException(string message, Exception innerException)
		: base(HttpStatusCode.Forbidden, message, innerException)
	{
	}

	public ForbiddenException(string message, string content, Exception innerException)
		: base(HttpStatusCode.Forbidden, content, message, innerException)
	{
	}
}
