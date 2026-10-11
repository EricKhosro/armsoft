using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class UserVerificationMethod
{
	public UserVerificationType Type { get; set; }

	public string DisplayText { get; set; }

	public string Info { get; set; }
}
