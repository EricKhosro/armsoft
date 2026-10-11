namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class UtilitySpecialSearchField
{
	public string Name { get; set; }

	public string DisplayName { get; set; }

	public bool IsRequired { get; set; }

	public int MaxLength { get; set; }

	public int Order { get; set; }

	public bool IsPhoneNumber { get; set; }
}
