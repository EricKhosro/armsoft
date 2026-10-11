namespace ArmSoft.Cloud.Client.AS7X;

public class UserLoginRequestV2
{
	public short Application { get; set; }

	public short Localization { get; set; }

	public string Workstation { get; set; }

	public int ProcessId { get; set; }
}
