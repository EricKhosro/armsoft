namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class QrAuthenticationCheckResult
{
	public bool IsConfirmed { get; set; }

	public SystemVerification SystemVerification { get; set; }
}
