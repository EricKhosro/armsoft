using System;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient.Core;

public class ApiValidationException : BadRequestException
{
	public Error Error { get; private set; }

	public ApiValidationException(Error error)
		: base(string.Empty)
	{
		Error = error;
	}

	public ApiValidationException(Error error, string message)
		: base(string.Empty, message)
	{
		Error = error;
	}

	public ApiValidationException(Error error, string message, Exception innerException)
		: base(string.Empty, message, innerException)
	{
		Error = error;
	}
}
