using System;
using System.Net;

namespace ArmSoft.b2b.V2;

public class B2BException : Exception
{
	public int StatusCode { get; private set; }

	public int UserCode { get; private set; }

	public int Severity { get; private set; }

	public B2BException(string message, HttpStatusCode statusCode, Exception innerException)
		: base(message, innerException)
	{
		StatusCode = (int)statusCode;
		UserCode = 0;
		Severity = 0;
	}

	public B2BException(ErrorResponse error, HttpStatusCode statusCode)
		: base(string.IsNullOrEmpty(error.UserMessage) ? error.InternalMessage : error.UserMessage, (!string.IsNullOrEmpty(error.UserMessage)) ? new Exception(error.InternalMessage) : null)
	{
		StatusCode = (int)statusCode;
		UserCode = error.UserCode;
		Severity = error.Severity;
	}
}
