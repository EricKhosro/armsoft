namespace ArmSoft.Cloud.Client.AS7X;

public class KeyRegistrationRequest
{
	public string KeySerialNumber { get; set; }

	public string KeyInfo { get; set; }

	public short Localization { get; set; }

	public short Application { get; set; }
}
