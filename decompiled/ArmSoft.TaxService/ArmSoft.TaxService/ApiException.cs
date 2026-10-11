using System;
using System.Collections.Generic;
using ArmSoft.TaxService.Models;

namespace ArmSoft.TaxService;

public class ApiException : Exception
{
	public int StatusCode { get; set; }

	public string Code { get; private set; }

	public string Response { get; private set; }

	public List<FailedCodes> FailedCodes { get; set; }

	public ApiException(string message)
		: base(Resources.Get(ResourceKey.EInvoicingError) + message)
	{
		Response = "";
		Code = "TaxServiceError";
	}

	public ApiException(string code, string message)
		: base(Resources.Get(ResourceKey.EInvoicingError) + message)
	{
		Response = "";
		Code = (string.IsNullOrWhiteSpace(code) ? "TaxServiceError" : code);
	}

	public ApiException(string message, int statusCode, string response, Exception innerException)
		: base(Resources.Get(ResourceKey.EInvoicingError) + message, innerException)
	{
		StatusCode = statusCode;
		Response = response;
		Code = "TaxServiceError";
	}

	public ApiException(ErrorResponse errorResponse, int statusCode, string response)
		: base((errorResponse.Error == null) ? string.Empty : (Resources.Get(ResourceKey.EInvoicingError) + errorResponse.Error))
	{
		StatusCode = statusCode;
		Response = response;
		Code = (string.IsNullOrWhiteSpace(errorResponse.Code) ? "TaxServiceError" : errorResponse.Code);
	}

	public ApiException(string code, string message, List<FailedCodes> failedCodes)
		: base(Resources.Get(ResourceKey.EInvoicingError) + message)
	{
		Code = (string.IsNullOrWhiteSpace(code) ? "TaxServiceError" : code);
		FailedCodes = failedCodes;
	}
}
