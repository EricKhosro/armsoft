namespace ArmSoft.InternetBank.Web.API.Common;

public class AttachSecurityProviderParams
{
	public string Username { get; set; }

	public string SecurityProviderType { get; set; }

	public bool? IsSecurityProviderRequired { get; set; }
}
