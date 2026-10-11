using System;

namespace ArmSoft.InternetBank.APIClient.Core;

public class ClientVersionException : BadRequestException
{
	public ClientVersionException()
		: base("unsupported_version")
	{
	}

	public ClientVersionException(string message)
		: base("unsupported_version", message)
	{
	}

	public ClientVersionException(string message, Exception innerException)
		: base("unsupported_version", message, innerException)
	{
	}
}
