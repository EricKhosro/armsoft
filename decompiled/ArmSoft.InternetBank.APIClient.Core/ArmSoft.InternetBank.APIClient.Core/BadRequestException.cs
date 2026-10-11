using System;
using System.Net;

namespace ArmSoft.InternetBank.APIClient.Core;

public class BadRequestException : ApiException
{
	public string ErrorCode { get; private set; }

	public BadRequestException(string errorCode)
		: base(HttpStatusCode.BadRequest)
	{
		ErrorCode = errorCode;
	}

	public BadRequestException(string errorCode, string message)
		: base(HttpStatusCode.BadRequest, message)
	{
		ErrorCode = errorCode;
	}

	public BadRequestException(string errorCode, string message, Exception innerException)
		: base(HttpStatusCode.BadRequest, message, innerException)
	{
		ErrorCode = errorCode;
	}

	public BadRequestException(string errorCode, string content, string message, Exception innerException)
		: base(HttpStatusCode.BadRequest, content, message, innerException)
	{
		ErrorCode = errorCode;
	}
}
