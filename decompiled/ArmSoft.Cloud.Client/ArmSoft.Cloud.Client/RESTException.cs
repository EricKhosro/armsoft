using System;
using System.Net;
using System.Runtime.InteropServices;

namespace ArmSoft.Cloud.Client;

[ClassInterface(ClassInterfaceType.AutoDual)]
[ComVisible(true)]
[Guid("27D51039-BA62-4BE0-865B-CE8A3A027645")]
public class RESTException : Exception
{
	[ComVisible(false)]
	public HttpStatusCode StatusCode { get; private set; }

	public string StatusCodeString => StatusCode.ToString();

	public string Code { get; private set; }

	public string Response { get; private set; }

	public string ServerStackTrace { get; private set; }

	public RESTException(string message)
		: base(message)
	{
		Response = "";
		Code = "";
		ServerStackTrace = "";
	}

	public RESTException(string message, HttpStatusCode statusCode, string response)
		: base(message)
	{
		StatusCode = statusCode;
		Response = response;
		Code = "";
		ServerStackTrace = "";
	}

	public RESTException(string message, HttpStatusCode statusCode, string response, Exception innerException)
		: base(message, innerException)
	{
		StatusCode = statusCode;
		Response = response;
		Code = "";
		ServerStackTrace = "";
	}

	public RESTException(ErrorResponse errorResponse, HttpStatusCode statusCode, string response)
		: base(errorResponse.Error)
	{
		StatusCode = statusCode;
		Response = response;
		Code = ((errorResponse.Code == null) ? "" : errorResponse.Code);
		ServerStackTrace = ((errorResponse.StackTrace == null) ? "" : errorResponse.StackTrace);
	}
}
