using System.Collections.Generic;

namespace ArmSoft.SME.Core.AddIn;

public class ErrorResponse
{
	public int? Code { get; set; }

	public string Title { get; set; }

	public string Error { get; set; }

	public string StackTrace { get; set; }

	public List<ErrorDetail> ErrorDetails { get; set; }

	public bool IsRestException { get; set; }

	public bool Visible { get; set; } = true;

	public string FieldName { get; set; }
}
