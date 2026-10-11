namespace ArmSoft.SME.Common.Models.Parameter;

public class ParameterStoreRequestModel
{
	public short Id { get; set; }

	public string Value { get; set; }

	public int UserId { get; set; } = -1;

	public bool IsDefault { get; set; }
}
