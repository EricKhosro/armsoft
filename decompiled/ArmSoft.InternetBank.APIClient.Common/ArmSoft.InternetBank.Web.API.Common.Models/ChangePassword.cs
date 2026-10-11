namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ChangePassword
{
	public string OldPassword { get; set; }

	public string NewPassword { get; set; }

	public bool UpdateLastLoginDate { get; set; }
}
