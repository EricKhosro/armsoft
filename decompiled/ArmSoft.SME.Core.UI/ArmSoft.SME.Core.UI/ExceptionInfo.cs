using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Error;

namespace ArmSoft.SME.Core.UI;

public class ExceptionInfo
{
	public string Title { get; set; }

	public string Message { get; set; }

	public string StackTrace { get; set; }

	public List<ErrorDetail> Details { get; set; }

	public bool IsRestException { get; set; }
}
