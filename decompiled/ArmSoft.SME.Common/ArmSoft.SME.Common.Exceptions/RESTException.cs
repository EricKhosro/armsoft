using System;
using System.Net;

namespace ArmSoft.SME.Common.Exceptions;

public class RESTException : Exception
{
	public int? Code { get; set; }

	public string Title { get; set; }

	public string FieldName { get; set; }

	public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;

	public RESTException()
	{
	}

	public RESTException(string message)
		: base(message)
	{
	}

	public RESTException(string message, string fieldName)
		: this(message)
	{
		FieldName = fieldName;
	}

	public RESTException(string message, HttpStatusCode statusCode)
		: this(message)
	{
		StatusCode = statusCode;
	}

	public RESTException(string message, HttpStatusCode statusCode, int? code)
		: this(message, statusCode)
	{
		Code = code;
	}

	public RESTException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
