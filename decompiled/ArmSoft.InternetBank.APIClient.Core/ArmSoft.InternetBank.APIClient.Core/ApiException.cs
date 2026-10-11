using System;
using System.Net;

namespace ArmSoft.InternetBank.APIClient.Core;

public class ApiException : Exception
{
	public HttpStatusCode StatusCode { get; private set; }

	public string Content { get; private set; }

	public ApiException(HttpStatusCode statusCode)
	{
		StatusCode = statusCode;
	}

	public ApiException(HttpStatusCode statusCode, string message)
		: base(message)
	{
		StatusCode = statusCode;
	}

	public ApiException(HttpStatusCode statusCode, string message, Exception innerException)
		: base(message, innerException)
	{
		StatusCode = statusCode;
	}

	public ApiException(HttpStatusCode statusCode, string content, string message, Exception innerException)
		: base(message, innerException)
	{
		StatusCode = statusCode;
		Content = content;
	}
}
