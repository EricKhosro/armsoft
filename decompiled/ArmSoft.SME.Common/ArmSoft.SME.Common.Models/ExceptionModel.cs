using System.Collections.Generic;
using System.Net;
using ArmSoft.SME.Common.Models.Error;

namespace ArmSoft.SME.Common.Models;

public class ExceptionModel
{
	public int? Code { get; set; }

	public HttpStatusCode StatusCode { get; set; }

	public string Message { get; set; }

	public string FieldName { get; set; }

	public bool IsRestException { get; set; }

	public string StackTrace { get; set; }

	public List<ErrorDetail> ErrorDetails { get; set; }
}
