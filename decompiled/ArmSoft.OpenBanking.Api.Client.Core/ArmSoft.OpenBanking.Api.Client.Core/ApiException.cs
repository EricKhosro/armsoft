using System;
using System.Net;

namespace ArmSoft.OpenBanking.Api.Client.Core;

public class ApiException : Exception
{
	public HttpStatusCode StatusCode { get; private set; }

	public string? Content { get; private set; }

	public string ResponseCode { get; private set; }

	public ApiException(HttpStatusCode statusCode, string? content, string responseCode, string message, Exception? innerException = null)
		: base(message, innerException)
	{
		StatusCode = statusCode;
		Content = content;
		ResponseCode = responseCode;
	}

	public ApiException(HttpStatusCode statusCode, string? content)
		: this(statusCode, content, string.Empty, string.Empty)
	{
	}
}
