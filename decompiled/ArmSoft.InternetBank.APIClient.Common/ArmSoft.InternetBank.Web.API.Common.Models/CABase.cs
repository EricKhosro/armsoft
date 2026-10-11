namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CABase : Application
{
	public string CustomizationName { get; set; }

	public string Description { get; set; }

	public bool IsEditable { get; set; }

	public bool IsDetailsFieldShown { get; set; }

	public bool IsDateEnabled { get; set; }
}
