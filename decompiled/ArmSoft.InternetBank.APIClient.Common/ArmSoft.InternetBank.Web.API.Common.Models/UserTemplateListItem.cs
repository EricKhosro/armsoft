using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class UserTemplateListItem
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public bool IsMobileRequired { get; set; }

	public bool IsEmailRequired { get; set; }

	public bool IsSerialTokenRequired { get; set; }

	public PasswordProvidingMethod PasswordProvidingMethod { get; set; }
}
