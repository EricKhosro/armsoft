namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ParamsForRegistrationButton
{
	public string UserRegistrationLink { get; set; }

	public string UserRegistrationLinkText { get; set; }

	public bool AllowBankClientRegistration { get; set; }

	public bool AllowBankLegalClientRegistration { get; set; }
}
