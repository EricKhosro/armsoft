using System;
using System.Net;

namespace ArmSoft.InternetBank.APIClient.Core;

public class UnexpectedException : ApiException
{
	public UnexpectedException(HttpStatusCode statusCode)
		: base(statusCode)
	{
	}

	public UnexpectedException(HttpStatusCode statusCode, string message)
		: base(statusCode, message)
	{
	}

	public UnexpectedException(HttpStatusCode statusCode, string message, Exception innerException)
		: base(statusCode, message, innerException)
	{
	}

	public UnexpectedException(HttpStatusCode statusCode, string content, string message, Exception innerException)
		: base(statusCode, content, message, innerException)
	{
	}
}
