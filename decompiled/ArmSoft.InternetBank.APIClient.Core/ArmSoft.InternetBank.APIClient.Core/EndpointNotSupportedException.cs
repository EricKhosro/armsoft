using System;

namespace ArmSoft.InternetBank.APIClient.Core;

public class EndpointNotSupportedException : NotFoundException
{
	public EndpointNotSupportedException()
	{
	}

	public EndpointNotSupportedException(string message)
		: base(message)
	{
	}

	public EndpointNotSupportedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public EndpointNotSupportedException(string message, string content, Exception innerException)
		: base(content, message, innerException)
	{
	}
}
