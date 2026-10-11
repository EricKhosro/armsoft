namespace ArmSoft.Cloud.Client.AS7X;

public class UserLogoutRequest
{
	public int DBId { get; set; }

	public string Workstation { get; set; }

	public int ProcessId { get; set; }

	public string Key { get; set; }
}
