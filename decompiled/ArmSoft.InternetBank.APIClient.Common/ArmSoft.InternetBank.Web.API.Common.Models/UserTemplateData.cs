using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class UserTemplateData
{
	public PasswordProvidingMethod PasswordProvidingMethod { get; set; }

	public bool IsMobileRequired { get; set; }

	public bool IsEmailRequired { get; set; }
}
