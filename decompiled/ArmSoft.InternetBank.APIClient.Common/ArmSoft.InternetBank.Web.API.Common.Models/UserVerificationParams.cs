using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class UserVerificationParams
{
	public string Username { get; set; }

	public UserVerificationType VerificationType { get; set; }

	public string VerificationCode { get; set; }

	public string NewPassword { get; set; }

	public string NewSecurityProvider { get; set; }

	public string TokenSerial { get; set; }

	public string SecurityProviderType { get; set; }
}
