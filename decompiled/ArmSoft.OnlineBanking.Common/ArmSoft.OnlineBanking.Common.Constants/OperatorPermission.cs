using ArmSoft.OnlineBanking.Common.Attributes;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Constants;

public static class OperatorPermission
{
	[Permission(Group = PermissionGroup.Clients, Order = 1)]
	public const string ClientsViewList = "GVCLIENTS";

	[Permission(Group = PermissionGroup.Clients, Order = 2)]
	public const string ClientsViewAccounts = "GVCLACC";

	[Permission(Group = PermissionGroup.Clients, Order = 3)]
	public const string ClientsViewCards = "GVCLCARD";

	[Permission(Group = PermissionGroup.Clients, Order = 4)]
	public const string ClientsViewCredits = "GVCLCRED";

	[Permission(Group = PermissionGroup.Clients, Order = 5)]
	public const string ClientsViewDeposits = "GVCLDEP";

	[Permission(Group = PermissionGroup.Clients, Order = 6)]
	public const string ClientsViewBonds = "GVCLBOND";

	[Permission(Group = PermissionGroup.Clients, Order = 7)]
	public const string ClientsViewPurchasedSecurities = "GVCLPRSC";

	[Permission(Group = PermissionGroup.Clients, Order = 8)]
	public const string ClientsViewDocuments = "GVCLDOCS";

	[Permission(Group = PermissionGroup.Clients, Order = 9)]
	public const string ClientsViewMessages = "GVCLMSGS";

	[Permission(Group = PermissionGroup.Clients, Order = 10)]
	public const string ClientsChangeDocumentState = "GCCLDOCST";

	[Permission(Group = PermissionGroup.Clients, Order = 11)]
	public const string ClientsModify = "GMCLIENT";

	[Permission(Group = PermissionGroup.Clients, Order = 12)]
	public const string ClientViewOperationsLog = "GVCLOPLOG";

	[Permission(Group = PermissionGroup.Clients, Order = 13)]
	public const string ClientDeactivate = "CLDEAC";

	[Permission(Group = PermissionGroup.Clients, Order = 14)]
	public const string ClientsImport = "CLIMPORT";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 1)]
	public const string ClientUsersViewList = "GVCUSERS";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 2)]
	public const string ClientUsersViewDetails = "GVCUDET";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 3)]
	public const string ClientUsersAdd = "GACUSER";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 4)]
	public const string ClientUsersModify = "GMCUSER";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 5)]
	public const string ClientUsersDelete = "GDCUSER";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 6)]
	public const string ClientUsersResetPassword = "GOCURESET";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 7)]
	public const string ClientUsersResetSecurityProvider = "GOCURSECPR";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 8)]
	public const string ClientUsersDeactivateSecurityProvider = "GDCURSECPR";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 9)]
	public const string ClientUsersUnlock = "GOCUUNLOCK";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 10)]
	public const string ClientUsersAssignRoles = "GOCUROLES";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 11)]
	public const string ClientUsersModifyPermissionsAccounts = "GMCUACCP";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 12)]
	public const string ClientUsersModifyPermissionsCards = "GMCUCARDP";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 13)]
	public const string ClientUsersModifyPermissionsCredits = "GMCUCREDP";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 14)]
	public const string ClientUsersModifyPermissionsDeposits = "GMCUDEPP";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 15)]
	public const string ClientUsersModifyPermissionsBonds = "GMCUBOND";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 16)]
	public const string ClientUsersModifyPermissionsPurchasedSecurity = "GMCUPRSC";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 17)]
	public const string ClientUsersModifyPermissionsDocuments = "GMCUDOCP";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 18)]
	public const string ClientUsersViewAssociations = "GVCUASSC";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 19)]
	public const string ClientUsersManageAssociations = "GMCUASSC";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 20)]
	public const string ClientUsersViewOperationsLog = "GVCUOPLOG";

	[Permission(Group = PermissionGroup.ClientUsers, Order = 21)]
	public const string ClientUsersRemoveSecurityProvider = "GCURSECPR";

	[Permission(Group = PermissionGroup.OperatorUsers, Order = 1)]
	public const string OperatorUsersViewList = "GVOUSERS";

	[Permission(Group = PermissionGroup.OperatorUsers, Order = 2)]
	public const string OperatorUsersViewDetails = "GVOUDET";

	[Permission(Group = PermissionGroup.OperatorUsers, Order = 3)]
	public const string OperatorUsersAdd = "GAOUSER";

	[Permission(Group = PermissionGroup.OperatorUsers, Order = 4)]
	public const string OperatorUsersModify = "GMOUSER";

	[Permission(Group = PermissionGroup.OperatorUsers, Order = 5)]
	public const string OperatorUsersDelete = "GDOUSER";

	[Permission(Group = PermissionGroup.OperatorUsers, Order = 6)]
	public const string OperatorUsersResetPassword = "GOOURESET";

	[Permission(Group = PermissionGroup.OperatorUsers, Order = 7)]
	public const string OperatorUsersResetSecurityProvider = "GOOURSECPR";

	[Permission(Group = PermissionGroup.OperatorUsers, Order = 8)]
	public const string OperatorUsersDeactivateSecurityProvider = "GDOURSECPR";

	[Permission(Group = PermissionGroup.OperatorUsers, Order = 9)]
	public const string OperatorUsersUnlock = "GOOUUNLOCK";

	[Permission(Group = PermissionGroup.OperatorUsers, Order = 10)]
	public const string OperatorUsersAssignRoles = "GOOUROLES";

	[Permission(Group = PermissionGroup.OperatorUsers, Order = 11)]
	public const string OperatorUsersViewOperationsLog = "GVOUOPLOG";

	[Permission(Group = PermissionGroup.OperatorUsers, Order = 12)]
	public const string OperatorUsersViewLog = "GVOUVLOG";

	[Permission(Group = PermissionGroup.DigipassDevices, Order = 1)]
	public const string DigipassesViewList = "GVDPSS";

	[Permission(Group = PermissionGroup.DigipassDevices, Order = 2)]
	public const string DigipassesImport = "GODPSIMP";

	[Permission(Group = PermissionGroup.DigipassDevices, Order = 3)]
	public const string DigipassesUnlock = "GODPSUNLCK";

	[Permission(Group = PermissionGroup.DigipassDevices, Order = 4)]
	public const string DigipassesReset = "GODPSRESET";

	[Permission(Group = PermissionGroup.DigipassDevices, Order = 5)]
	public const string DigipassesDelete = "GDDPS";

	[Permission(Group = PermissionGroup.DigipassDevices, Order = 6)]
	public const string DigipassesChangeStatus = "GMDPS";

	[Permission(Group = PermissionGroup.Parameters, Order = 1)]
	public const string ParametersView = "GVPARAMS";

	[Permission(Group = PermissionGroup.Parameters, Order = 2)]
	public const string ParametersModifyGeneral = "GMPARGEN";

	[Permission(Group = PermissionGroup.Parameters, Order = 3)]
	public const string ParametersModifySecurity = "GMPARSEC";

	[Permission(Group = PermissionGroup.Parameters, Order = 4)]
	public const string ParametersModifyDigipass = "GMPARDPS";

	[Permission(Group = PermissionGroup.Parameters, Order = 5)]
	public const string ParametersModifyConfiguration = "GMPARCON";

	[Permission(Group = PermissionGroup.Roles, Order = 1)]
	public const string RolesViewList = "GVROLES";

	[Permission(Group = PermissionGroup.Roles, Order = 2)]
	public const string RolesViewDetails = "GVROLEDET";

	[Permission(Group = PermissionGroup.Roles, Order = 3)]
	public const string RolesAdd = "GAROLE";

	[Permission(Group = PermissionGroup.Roles, Order = 4)]
	public const string RolesModify = "GMROLE";

	[Permission(Group = PermissionGroup.Roles, Order = 5)]
	public const string RolesDelete = "GDROLE";

	[Permission(Group = PermissionGroup.Roles, Order = 6)]
	public const string RolesViewOperationsLog = "GVRLOPLOG";

	[Permission(Group = PermissionGroup.News, Order = 1)]
	public const string NewsViewList = "VNEWS";

	[Permission(Group = PermissionGroup.News, Order = 2)]
	public const string NewsAdd = "ADDNEWS";

	[Permission(Group = PermissionGroup.News, Order = 3)]
	public const string NewsModify = "MODNEWS";

	[Permission(Group = PermissionGroup.News, Order = 4)]
	public const string NewsDelete = "DELNEWS";

	[Permission(Group = PermissionGroup.News, Order = 5)]
	public const string NewsViewOperationsLog = "GVNSOPLOG";

	[Permission(Group = PermissionGroup.Verifications, Order = 1)]
	public const string VerificationsClientUsers = "VERUCLNT";

	[Permission(Group = PermissionGroup.Verifications, Order = 2)]
	public const string VerificationsBankUsers = "VERUBANK";

	[Permission(Group = PermissionGroup.Verifications, Order = 3)]
	public const string VerificationsViewRequests = "VERVRQST";

	[Permission(Group = PermissionGroup.Verifications, Order = 4)]
	public const string VerificationRegisterBankClientUser = "VERUREG";

	[Permission(Group = PermissionGroup.Verifications, Order = 5)]
	public const string VerificationsCreateUser = "VERUCREATE";

	[Permission(Group = PermissionGroup.Verifications, Order = 6)]
	public const string VerificationsModifyUser = "VERUEDIT";

	[Permission(Group = PermissionGroup.Verifications, Order = 7)]
	public const string VerificationsCreateUserTemplate = "VERUTCREAT";

	[Permission(Group = PermissionGroup.Verifications, Order = 8)]
	public const string VerificationsModifyUserTemplate = "VERUTEDIT";

	[Permission(Group = PermissionGroup.Verifications, Order = 9)]
	public const string VerificationsUserPasswordReset = "VERUPRST";

	[Permission(Group = PermissionGroup.Verifications, Order = 10)]
	public const string VerificationsUserSecurityProviderReset = "VERUSPRST";

	[Permission(Group = PermissionGroup.Verifications, Order = 11)]
	public const string VerificationsUserSecurityProviderDeactivate = "VERUSPD";

	[Permission(Group = PermissionGroup.Verifications, Order = 12)]
	public const string VerificationsUserUnlock = "VERUUNLK";

	[Permission(Group = PermissionGroup.Verifications, Order = 13)]
	public const string VerificationsModifyAssociations = "VERAEDIT";

	[Permission(Group = PermissionGroup.Verifications, Order = 14)]
	public const string VerificationsDigipassReset = "VERDPS";

	[Permission(Group = PermissionGroup.Verifications, Order = 15)]
	public const string VerificationsDigipassChangeStatus = "VERDPSCHS";

	[Permission(Group = PermissionGroup.Verifications, Order = 16)]
	public const string VerificationsDigipassDelete = "VERDPSD";

	[Permission(Group = PermissionGroup.Verifications, Order = 17)]
	public const string VerificationsAuthenticatorToken = "VERAUT";

	[Permission(Group = PermissionGroup.Verifications, Order = 18)]
	public const string VerificationsRoles = "VERROLES";

	[Permission(Group = PermissionGroup.Verifications, Order = 19)]
	public const string VerificationsParameters = "VERPARAM";

	[Permission(Group = PermissionGroup.Verifications, Order = 20)]
	public const string VerificationsNews = "VERNEWS";

	[Permission(Group = PermissionGroup.Verifications, Order = 21)]
	public const string VerificationsModifyClient = "VERCEDIT";

	[Permission(Group = PermissionGroup.Verifications, Order = 22)]
	public const string VerificationsDeactivateClient = "VERCDEAC";

	[Permission(Group = PermissionGroup.Verifications, Order = 23)]
	public const string VerificationsClientLimits = "VERCLEDIT";

	[Permission(Group = PermissionGroup.Customizations, Order = 1)]
	public const string CustomizationsViewList = "VCSTM";

	[Permission(Group = PermissionGroup.Customizations, Order = 2)]
	public const string CustomizationsAdd = "ADDCSTM";

	[Permission(Group = PermissionGroup.Customizations, Order = 3)]
	public const string CustomizationsModify = "MODCSTM";

	[Permission(Group = PermissionGroup.Customizations, Order = 4)]
	public const string CustomizationsDelete = "DELCSTM";

	[Permission(Group = PermissionGroup.Customizations, Order = 5)]
	public const string CustomDirectoriesManage = "MNGCSTM";

	[Permission(Group = PermissionGroup.Credentials, Order = 1)]
	public const string CredentialsView = "GVCRED";

	[Permission(Group = PermissionGroup.Credentials, Order = 2)]
	public const string CredentialsGenerate = "ADDCRED";

	[Permission(Group = PermissionGroup.Credentials, Order = 3)]
	public const string CredentialsModify = "MODCRED";

	[Permission(Group = PermissionGroup.Credentials, Order = 4)]
	public const string CredentialsDelete = "DELCRED";

	[Permission(Group = PermissionGroup.Agreements, Order = 1)]
	public const string ServiceAgreementsViewList = "VSAGR";

	[Permission(Group = PermissionGroup.Agreements, Order = 2)]
	public const string ServiceAgreementsAdd = "ADDSAGR";

	[Permission(Group = PermissionGroup.Agreements, Order = 3)]
	public const string ServiceAgreementsModify = "MODSAGR";

	[Permission(Group = PermissionGroup.Agreements, Order = 4)]
	public const string ServiceAgreementsDelete = "DELSAGR";

	[Permission(Group = PermissionGroup.MapPoints, Order = 1)]
	public const string MapPointsViewList = "VMAPP";

	[Permission(Group = PermissionGroup.MapPoints, Order = 2)]
	public const string MapPointsAdd = "ADDMAPP";

	[Permission(Group = PermissionGroup.MapPoints, Order = 3)]
	public const string MapPointsModify = "MODMAPP";

	[Permission(Group = PermissionGroup.MapPoints, Order = 4)]
	public const string MapPointsDelete = "DELMAPP";

	[Permission(Group = PermissionGroup.UserModules, Order = 1)]
	public const string UserModulesView = "VIEWUSMO";

	[Permission(Group = PermissionGroup.UserModules, Order = 2)]
	public const string UserModuleModify = "MODUSMO";

	[Permission(Group = PermissionGroup.Notifications, Order = 1)]
	public const string NotificationsViewList = "VNTF";

	[Permission(Group = PermissionGroup.Notifications, Order = 2)]
	public const string NotificationsAdd = "ADDNTF";

	[Permission(Group = PermissionGroup.Notifications, Order = 3)]
	public const string NotificationsModify = "MODNTF";

	[Permission(Group = PermissionGroup.Notifications, Order = 4)]
	public const string NotificationsDelete = "DELNTF";

	[Permission(Group = PermissionGroup.BankRequisites, Order = 1)]
	public const string BankRequisitesViewList = "VREQ";

	[Permission(Group = PermissionGroup.BankRequisites, Order = 2)]
	public const string BankRequisitesAdd = "ADDREQ";

	[Permission(Group = PermissionGroup.BankRequisites, Order = 3)]
	public const string BankRequisitesModify = "MODREQ";

	[Permission(Group = PermissionGroup.BankRequisites, Order = 4)]
	public const string BankRequisitesDelete = "DELREQ";

	[Permission(Group = PermissionGroup.Reports, Order = 1)]
	public const string ReportUsersView = "VREPU";

	[Permission(Group = PermissionGroup.Reports, Order = 2)]
	public const string ReportDocsView = "VREPD";

	[Permission(Group = PermissionGroup.Reports, Order = 3)]
	public const string ReportTurnoverView = "VREPT";

	[Permission(Group = PermissionGroup.Reports, Order = 4)]
	public const string ReportClientsTurnoverView = "VREPCT";

	[Permission(Group = PermissionGroup.Reports, Order = 5)]
	public const string ReportUserRegistrationsView = "VREPUR";

	[Permission(Group = PermissionGroup.Reports, Order = 6)]
	public const string ReportOperatorOperationsView = "VREPOPOPS";

	[Permission(Group = PermissionGroup.Reports, Order = 7)]
	public const string ReportDeviceActivationInfosView = "VREPDEACS";

	[Permission(Group = PermissionGroup.Reports, Order = 8)]
	public const string ReportPushNotificationsView = "VREPPUSHN";

	[Permission(Group = PermissionGroup.Reports, Order = 9)]
	public const string ReportWorkerDataView = "VRWORKER";

	[Permission(Group = PermissionGroup.ClientLimits, Order = 1)]
	public const string ClientLimitsViewList = "VIEWLIMIT";

	[Permission(Group = PermissionGroup.ClientLimits, Order = 2)]
	public const string ClientLimitsAdd = "ADDLIMIT";

	[Permission(Group = PermissionGroup.ClientLimits, Order = 3)]
	public const string ClientLimitsModify = "MODLIMIT";

	[Permission(Group = PermissionGroup.ClientLimits, Order = 4)]
	public const string ClientLimitsDelete = "DELLIMIT";

	[Permission(Group = PermissionGroup.ClientLimits, Order = 5)]
	public const string ClientLimitsViewOperationsLog = "GVCLLOPLOG";

	[Permission(Group = PermissionGroup.Utilities, Order = 1)]
	public const string UtilitiesModify = "MODUTIL";

	[Permission(Group = PermissionGroup.History, Order = 1)]
	public const string HistoryViewFullOperationsLog = "HVFLOG";

	[Permission(Group = PermissionGroup.DocumentTemplates, Order = 1)]
	public const string DocumentTemplatesView = "VIEWDT";

	[Permission(Group = PermissionGroup.DocumentTemplates, Order = 2)]
	public const string DocumentTemplatesAdd = "ADDDT";

	[Permission(Group = PermissionGroup.DocumentTemplates, Order = 3)]
	public const string DocumentTemplatesModify = "MODDT";

	[Permission(Group = PermissionGroup.DocumentTemplates, Order = 4)]
	public const string DocumentTemplatesDelete = "DELDT";

	[Permission(Group = PermissionGroup.UserTemplates, Order = 1)]
	public const string UserTemplatesView = "VIEWUT";

	[Permission(Group = PermissionGroup.UserTemplates, Order = 2)]
	public const string UserTemplatesAdd = "ADDUT";

	[Permission(Group = PermissionGroup.UserTemplates, Order = 3)]
	public const string UserTemplatesModify = "MODUT";

	[Permission(Group = PermissionGroup.UserTemplates, Order = 4)]
	public const string UserTemplatesDelete = "DELUT";

	[Permission(Group = PermissionGroup.UserTemplates, Order = 5)]
	public const string ClientUserTemplatesViewOperationsLog = "GVCUTOPLOG";

	[Permission(Group = PermissionGroup.PaymentGroups, Order = 1)]
	public const string PaymentGroupsView = "VIEWPG";

	[Permission(Group = PermissionGroup.PaymentGroups, Order = 2)]
	public const string PaymentGroupAdd = "ADDPG";

	[Permission(Group = PermissionGroup.PaymentGroups, Order = 3)]
	public const string PaymentGroupModify = "MODPG";

	[Permission(Group = PermissionGroup.PaymentGroups, Order = 4)]
	public const string PaymentGroupDelete = "DELPG";

	[Permission(Group = PermissionGroup.UserRegistration, Order = 1)]
	public const string BlockedUserRegistrationsView = "USRGVW";

	[Permission(Group = PermissionGroup.UserRegistration, Order = 2)]
	public const string UserRegistrationUnblock = "USRGUNBL";

	[Permission(Group = PermissionGroup.CorporateApi, Order = 1)]
	public const string BlockedCorporateApiAuthenticationAttemptsView = "CPAUTVW";

	[Permission(Group = PermissionGroup.CorporateApi, Order = 2)]
	public const string CorporateApiAuthenticationAttemptIpUnblock = "CPAUTUNBL";

	[Permission(Group = PermissionGroup.RateSuggestion, Order = 1)]
	public const string SuggestRateForClients = "SRFC";

	[Permission(Group = PermissionGroup.RateSuggestion, Order = 2)]
	public const string ViewAllRateSuggestions = "VARS";

	[Permission(Group = PermissionGroup.RateSuggestion, Order = 3)]
	public const string RejectAcceptedRateSuggestion = "RARS";

	[Permission(Group = PermissionGroup.RateSuggestion, Order = 4)]
	public const string RateSuggestionAvailabilityView = "VIEWRSA";

	[Permission(Group = PermissionGroup.RateSuggestion, Order = 5)]
	public const string RateSuggestionAvailabilityModify = "MODRSA";

	[Permission(Group = PermissionGroup.RateSuggestion, Order = 6)]
	public const string ManageRateSuggestions = "MARASA";

	[Permission(Group = PermissionGroup.SignableFiles, Module = "AmeriaEkengSign", Order = 1)]
	public const string ViewAllSignableFiles = "SFVIEW";

	[Permission(Group = PermissionGroup.SignableFiles, Module = "AmeriaEkengSign", Order = 2)]
	public const string UploadSignableFile = "SFUP";

	[Permission(Group = PermissionGroup.SignableFiles, Module = "AmeriaEkengSign", Order = 3)]
	public const string ApproveSignableFile = "SFAPP";

	[Permission(Group = PermissionGroup.MailTemplates, Order = 1)]
	public const string ViewMailTemplates = "VMAILTMP";

	[Permission(Group = PermissionGroup.Directories, Order = 1)]
	public const string ViewDirectories = "VDIRECTRS";

	[Permission(Group = PermissionGroup.ClientRegistrationConfiguration, Order = 1)]
	public const string ViewClientRegistrationConfiguration = "CLRGCNVW";

	[Permission(Group = PermissionGroup.ClientRegistrationConfiguration, Order = 2)]
	public const string AddClientRegistrationConfiguration = "CLRGCNAD";

	[Permission(Group = PermissionGroup.ClientRegistrationConfiguration, Order = 3)]
	public const string DeleteClientRegistrationConfiguration = "CLRGCNDL";

	[Permission(Group = PermissionGroup.AttachedCardTransfers, Order = 1)]
	public const string TransferToAttachedCard = "GACTRATC";
}
