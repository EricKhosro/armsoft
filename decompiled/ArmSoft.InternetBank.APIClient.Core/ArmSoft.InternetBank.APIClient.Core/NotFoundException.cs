using System;
using System.Net;

namespace ArmSoft.InternetBank.APIClient.Core;

public class NotFoundException : ApiException
{
	public NotFoundException()
		: base(HttpStatusCode.NotFound)
	{
	}

	public NotFoundException(string message)
		: base(HttpStatusCode.NotFound, message)
	{
	}

	public NotFoundException(string message, Exception innerException)
		: base(HttpStatusCode.NotFound, message, innerException)
	{
	}

	public NotFoundException(string message, string content, Exception innerException)
		: base(HttpStatusCode.NotFound, content, message, innerException)
	{
	}
}
