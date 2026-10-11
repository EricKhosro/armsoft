namespace ArmSoft.OpenBanking.Api.Common;

public class SignatureOptions
{
	public string Description { get; set; } = string.Empty;

	public string Challenge { get; set; } = string.Empty;

	public string SecurityProvider { get; set; } = string.Empty;

	public string TokenSerial { get; set; } = string.Empty;

	public string SignatureMode { get; set; } = string.Empty;

	public int SecurityProviderCodeLength { get; set; }
}
