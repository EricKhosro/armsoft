namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ParamGetSignatureOptionsForNewSecurityProvider
{
	public string SecurityProviderName { get; set; }

	public string TokenSerial { get; set; }

	public string SignatureMode { get; set; }
}
