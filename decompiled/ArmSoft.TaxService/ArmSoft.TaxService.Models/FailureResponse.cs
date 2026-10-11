using System.Collections.Generic;

namespace ArmSoft.TaxService.Models;

public class FailureResponse
{
	public string Code { get; set; }

	public string Message { get; set; }

	public List<FailedCodes> Codes { get; set; }
}
