using ArmSoft.OnlineBanking.Common.Attributes;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Constants;

public static class MenuItem
{
	public const string Accounts = "Accounts";

	public const string Cards = "Cards";

	public const string AttachedCards = "AttachedCards";

	public const string Credits = "Credits";

	public const string Deposits = "Deposits";

	public const string Bonds = "Bonds";

	public const string PurchasedSecurities = "PurchasedSecurities";

	public const string MoneyInjunctions = "MoneyInjunctions";

	public const string Merchants = "Merchants";

	public const string PaymentsList = "PaymentsList";

	public const string ApplicationsList = "ApplicationsList";

	public const string CancelRequestsList = "CancelRequestsList";

	public const string DocumentsUserCanSign = "DocumentsUserCanSign";

	public const string Messages = "Messages";

	public const string OutgoingMessages = "OutgoingMessages";

	public const string IncomingMessages = "IncomingMessages";

	public const string PensionStatement = "PensionStatement";

	[MenuItemIgnore(DomainRestrictions.Operators)]
	public const string ClientReports = "ClientReports";

	public const string Payees = "Payees";

	public const string Vehicles = "Vehicles";

	[MenuItemIgnore(DomainRestrictions.All)]
	public const string HomePage = "HomePage";

	[MenuItemIgnore(DomainRestrictions.All)]
	public const string WelcomePage = "WelcomePage";

	[MenuItemIgnore(DomainRestrictions.All)]
	public const string News = "News";

	public const string Subusers = "Subusers";

	[MenuItemIgnore(DomainRestrictions.All)]
	public const string NewApplication = "NewApplication";

	public const string PeriodicPayments = "PeriodicPayments";

	public const string RateSuggestions = "RateSuggestions";

	public const string SignableFileDocument = "SignableFileDocument";

	public const string ClientFinanceDeclaration = "ClientFinanceDeclaration";

	public const string AmeriaEkengSign = "AmeriaEkengSign";

	public const string ClientDebts = "ClientDebts";

	public const string AccessKeyGeneration = "AccessKeyGeneration";

	[MenuItemIgnore(DomainRestrictions.All)]
	public const string AboutUs = "AboutUs";

	public const string PersonalPassport = "PersonalPassport";

	[MenuItemIgnore(DomainRestrictions.All)]
	public const string ImportXML = "ImportXML";

	[MenuItemIgnore(DomainRestrictions.Operators)]
	public const string AccessKeysGeneration = "AccessKeyGeneration";
}
