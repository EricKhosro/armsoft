using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Error;

namespace ArmSoft.SME.Client;

public class ApiException : Exception
{
	public int StatusCode { get; private set; }

	public int? Code { get; set; }

	public string Response { get; private set; }

	public string Title { get; set; }

	public List<ErrorDetail> ErrorDetails { get; set; }

	public bool IsRestException { get; set; }

	public string FieldName { get; set; }

	public bool Visible { get; set; }

	public string ServiceStackTrace { get; set; }

	public string InnerMessage { get; set; }

	public ApiException(string message)
		: base(message)
	{
		Response = "";
	}

	public ApiException(string message, int statusCode, string response, Exception innerException)
		: base(message, innerException)
	{
		StatusCode = statusCode;
		Response = response;
	}

	public ApiException(ErrorResponse errorResponse, int statusCode, string response)
		: base(errorResponse.Error ?? string.Empty)
	{
		StatusCode = statusCode;
		Response = response;
		Code = errorResponse.Code;
	}
}
