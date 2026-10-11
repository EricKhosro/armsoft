using System.Text;
using ArmSoft.OpenBanking.Api.Common;

namespace ArmSoft.OpenBanking.Api.Client.Core;

public class ValidationException : BadRequestException
{
	public ValidationResult ValidationResult { get; private set; }

	public ValidationException(ValidationResult validationResult, string responseCode, string? content, string message)
		: base(responseCode, content, message)
	{
		ValidationResult = validationResult;
	}

	public string GetDetails()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (FieldError fieldError in ValidationResult.FieldErrors)
		{
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(7, 3, stringBuilder2);
			handler.AppendFormatted(fieldError.Field);
			handler.AppendLiteral(" -> ");
			handler.AppendFormatted(fieldError.Reason);
			handler.AppendLiteral(" (");
			handler.AppendFormatted(fieldError.Message);
			handler.AppendLiteral(")");
			stringBuilder2.AppendLine(ref handler);
		}
		return stringBuilder.ToString();
	}
}
