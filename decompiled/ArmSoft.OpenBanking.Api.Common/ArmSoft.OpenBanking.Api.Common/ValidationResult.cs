using System.Collections.Generic;

namespace ArmSoft.OpenBanking.Api.Common;

public class ValidationResult
{
	public ErrorType Type { get; set; }

	public List<FieldError> FieldErrors { get; set; } = new List<FieldError>();
}
