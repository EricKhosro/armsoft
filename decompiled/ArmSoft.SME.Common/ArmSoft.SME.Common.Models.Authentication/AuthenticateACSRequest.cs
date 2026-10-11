namespace ArmSoft.SME.Common.Models.Authentication;

public class AuthenticateACSRequest
{
	public string ADToken { get; set; }

	public int DBId { get; set; }

	public string CompName { get; set; }

	public AppType AppType { get; set; }

	public string AppVersion { get; set; }
}
