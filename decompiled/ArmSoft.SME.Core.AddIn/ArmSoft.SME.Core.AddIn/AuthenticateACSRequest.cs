namespace ArmSoft.SME.Core.AddIn;

public class AuthenticateACSRequest
{
	public string ADToken { get; set; }

	public int DBId { get; set; }

	public string CompName { get; set; }

	public string AppVersion { get; set; }
}
