namespace ArmSoft.SME.Common.Models.Authentication;

public class AuthenticatePublicApiResponse : AuthenticateResponse
{
	public int DatabaseId { get; set; }

	public string ServiceAddress { get; set; }
}
