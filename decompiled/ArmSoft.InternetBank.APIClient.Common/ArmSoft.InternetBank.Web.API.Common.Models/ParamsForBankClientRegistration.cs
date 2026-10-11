namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ParamsForBankClientRegistration
{
	public bool RequireEmail { get; set; }

	public bool RequireMobile { get; set; }

	public bool RequirePassport { get; set; }

	public bool VerifyEmail { get; set; }

	public bool VerifyMobile { get; set; }

	public string BankName { get; set; }

	public string BankClientRegistrationTermsURL1 { get; set; }

	public string BankClientRegistrationTermsText1 { get; set; }

	public string BankClientRegistrationTermsURL2 { get; set; }

	public string BankClientRegistrationTermsText2 { get; set; }

	public string BankClientRegistrationAcceptTermsText { get; set; }

	public string RegistrationLinkTextForNonBankClients { get; set; }

	public string BankClientRegistrationAdditionalInformation { get; set; }
}
