using System;

namespace ArmSoft.InternetBank.APIClient.Core;

public class ApiVersionException : BadRequestException
{
	public string CurrentApiVersion { get; private set; }

	public string MinimalSupportedApiVersion { get; private set; }

	public ApiVersionException(string currentApiVersion, string minimalSupportedApiVersion)
		: base("unsupported_version")
	{
		CurrentApiVersion = currentApiVersion;
		MinimalSupportedApiVersion = minimalSupportedApiVersion;
	}

	public ApiVersionException(string currentApiVersion, string minimalSupportedApiVersion, string message)
		: base("unsupported_version", message)
	{
		CurrentApiVersion = currentApiVersion;
		MinimalSupportedApiVersion = minimalSupportedApiVersion;
	}

	public ApiVersionException(string currentApiVersion, string minimalSupportedApiVersion, string message, Exception innerException)
		: base("unsupported_version", message, innerException)
	{
		CurrentApiVersion = currentApiVersion;
		MinimalSupportedApiVersion = minimalSupportedApiVersion;
	}
}
