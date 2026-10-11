using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class UserInfoForClientRegistration
{
	public string Passport { get; set; }

	public string Mobile { get; set; }

	public string Email { get; set; }

	public string VerificationCodeForEmail { get; set; }

	public string VerificationCodeForMobile { get; set; }

	public string Firstname { get; set; }

	public string Lastname { get; set; }

	public string Username { get; set; }

	public string Password { get; set; }

	public BankClientRegistrationInformationOption ClientsBankInformationOption { get; set; }

	public string ClientsBankInformation { get; set; }

	public bool AcceptTerms { get; set; }

	public bool IsRegistrationForBankLegalClient { get; set; }

	public UserInfoForClientRegistration()
	{
		Passport = string.Empty;
		Mobile = string.Empty;
		Email = string.Empty;
		ClientsBankInformation = string.Empty;
	}
}
