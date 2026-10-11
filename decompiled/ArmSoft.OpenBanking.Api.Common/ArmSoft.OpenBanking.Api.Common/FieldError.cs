namespace ArmSoft.OpenBanking.Api.Common;

public class FieldError
{
	public string Collection { get; set; } = string.Empty;

	public int? Line { get; set; }

	public string Field { get; set; } = string.Empty;

	public string Field2 { get; set; } = string.Empty;

	public string Message { get; set; } = string.Empty;

	public string DecodedMessage { get; set; } = string.Empty;

	public string Reason { get; set; } = string.Empty;
}
