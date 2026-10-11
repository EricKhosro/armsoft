namespace ArmSoft.Cloud.Client.AS7X;

public class UserDBSelectionResponse
{
	public bool IsAdmin { get; set; }

	public KeyInfo Key { get; set; }

	public DBConnectionInfo DBInfo { get; set; }
}
