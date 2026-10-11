namespace ArmSoft.SME.Core.UI.InternetBank;

public class ChallengeRow
{
	public string Description { get; set; }

	public string Challenge { get; set; }

	public string SecurityProvider { get; set; }

	public string TokenSerial { get; set; }

	public string SignatureMode { get; set; }

	public int SecurityProviderCodeLength { get; set; }
}
