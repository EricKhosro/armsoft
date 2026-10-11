namespace ArmSoft.Cloud.Client.AS7X;

public class UserDBSelectionRequest
{
	public int DBId { get; set; }

	public short Localization { get; set; }

	public string Workstation { get; set; }

	public int ProcessId { get; set; }
}
