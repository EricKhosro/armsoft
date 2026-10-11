using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class User
{
	public int UserId { get; set; }

	public string ClientCode { get; set; }

	public string UserName { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string Mobile { get; set; }

	public string Email { get; set; }

	public string Passport { get; set; }

	public bool LoginAllowed { get; set; }

	public PasswordProvidingMethod PasswordProvidingMethod { get; set; }

	public string Password { get; set; }

	public bool AllowUserVerificationByEmail { get; set; }

	public bool AllowUserVerificationBySMS { get; set; }

	public string SecurityProvider { get; set; }

	public string TokenSerial { get; set; }

	public decimal? MaxLimitCreateAmount { get; set; }

	public decimal? MaxLimitSignAmount { get; set; }

	public List<string> Roles { get; set; }

	public bool IsLimited { get; set; }

	public bool IsLoginToCorporateApiAllowed { get; set; }

	public bool IsLoginFromCreditPlatformAllowed { get; set; }

	public bool? IsSecurityProviderRequired { get; set; }

	public string TemplateName { get; set; }

	public LocalizableString TemplateDescription { get; set; }

	public List<string> AllowedSecurityProviders { get; set; }

	public List<AccountPermission> AccountPermissions { get; set; }

	public List<CardPermission> CardPermissions { get; set; }

	public List<CreditPermission> CreditPermissions { get; set; }

	public List<DepositPermission> DepositPermissions { get; set; }

	public List<BondPermission> BondPermissions { get; set; }

	public List<PurchasedSecurityPermission> PurchasedSecurityPermissions { get; set; }

	public User()
	{
	}

	public User(string clientCode, string userName, string firstName, string lastName, bool allowUserVerificationByEmail, bool allowUserVerificationBySMS, List<string> roles, PasswordProvidingMethod passwordProvidingMethod, string password = null, string mobile = null, string email = null, bool loginAllowed = true, string securityProvider = null, string tokenSerial = null, decimal? maxLimitCreateAmount = null, decimal? maxLimitSignAmount = null, bool isLimited = false, List<string> allowedSecurityProviders = null, bool? isSecurityProviderRequired = null, string passport = null, bool isLoginToCorporateApiAllowed = false, string templateName = null, LocalizableString templateDescription = null, List<AccountPermission> accountPermissions = null, List<CardPermission> cardPermissions = null, List<DepositPermission> depositPermissions = null, List<BondPermission> bondPermissions = null, List<PurchasedSecurityPermission> purchasedSecurityPermissions = null, List<CreditPermission> creditPermissions = null)
	{
		UserId = -1;
		ClientCode = clientCode;
		UserName = userName;
		FirstName = firstName;
		LastName = lastName;
		AllowUserVerificationByEmail = allowUserVerificationByEmail;
		AllowUserVerificationBySMS = allowUserVerificationBySMS;
		Roles = roles;
		PasswordProvidingMethod = passwordProvidingMethod;
		Password = password;
		Passport = passport;
		Mobile = mobile;
		Email = email;
		LoginAllowed = loginAllowed;
		SecurityProvider = securityProvider;
		TokenSerial = tokenSerial;
		MaxLimitCreateAmount = maxLimitCreateAmount;
		MaxLimitSignAmount = maxLimitSignAmount;
		IsLimited = isLimited;
		AllowedSecurityProviders = allowedSecurityProviders;
		IsLoginToCorporateApiAllowed = isLoginToCorporateApiAllowed;
		IsSecurityProviderRequired = isSecurityProviderRequired;
		TemplateName = templateName;
		TemplateDescription = templateDescription;
		AccountPermissions = accountPermissions;
		CardPermissions = cardPermissions;
		BondPermissions = bondPermissions;
		PurchasedSecurityPermissions = purchasedSecurityPermissions;
		DepositPermissions = depositPermissions;
		CreditPermissions = creditPermissions;
	}
}
