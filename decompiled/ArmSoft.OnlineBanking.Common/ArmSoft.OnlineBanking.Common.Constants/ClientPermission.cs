using ArmSoft.OnlineBanking.Common.Attributes;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Constants;

public static class ClientPermission
{
	[Permission(Group = PermissionGroup.Documents, Order = 1)]
	public const string DocumentsViewList = "GVDOCS";

	[Permission(Group = PermissionGroup.Documents, Order = 2)]
	public const string DocumentsAdd = "GADOC";

	[Permission(Group = PermissionGroup.Documents, Order = 3)]
	public const string DocumentsModify = "GMDOC";

	[Permission(Group = PermissionGroup.Documents, Order = 4)]
	public const string DocumentsDelete = "GDDOC";

	[Permission(Group = PermissionGroup.Documents, Order = 5)]
	public const string DocumentsImport = "GIDOC";

	[Permission(Group = PermissionGroup.Documents, Order = 6)]
	public const string DocumentsModifyImported = "GMIDOC";

	[Permission(Group = PermissionGroup.Documents, Order = 7)]
	public const string DocumentsSendToEdit = "GODOCSTE";

	[Permission(Group = PermissionGroup.Messages, Order = 1)]
	public const string MessagesViewList = "GVMSGS";

	[Permission(Group = PermissionGroup.Messages, Order = 2)]
	public const string MessagesAdd = "GAMSG";

	[Permission(Group = PermissionGroup.Messages, Order = 3)]
	public const string MessagesModify = "GMMSG";

	[Permission(Group = PermissionGroup.Messages, Order = 4)]
	public const string MessagesDelete = "GDMSG";

	[Permission(Group = PermissionGroup.Messages, Order = 5)]
	public const string MessagesSendToEdit = "GOMSGSTE";

	[Permission(Group = PermissionGroup.Signatures, Order = 1)]
	public const string FirstSignature = "GOSGNFRST";

	[Permission(Group = PermissionGroup.Signatures, Order = 2)]
	public const string SecondSignature = "GOSGNSEC";

	[Permission(Group = PermissionGroup.Signatures, Order = 3)]
	public const string ThirdSignature = "GOSGNTHRD";

	[Permission(Group = PermissionGroup.Signatures, Order = 4)]
	public const string DocumentsSign = "GODOCSIGN";

	[Permission(Group = PermissionGroup.Signatures, Order = 5)]
	public const string DocumentsSignGroup = "GODOCSGGR";

	[Permission(Group = PermissionGroup.Signatures, Order = 6)]
	public const string MessagesSign = "GOMSGSIGN";

	public const string OverviewAccounts = "GVACCTS";

	public const string OverviewCards = "GVCARDS";

	public const string OverviewDeposits = "GVDEPOS";

	public const string OverviewBonds = "GVBONDS";

	public const string OverviewLoans = "GVLOANS";

	public const string OverviewRates = "GVRATES";

	public const string OverviewVehicles = "GVVEHICLES";

	public const string OverviewMerchants = "GVMERCHANTS";

	[Permission(Group = PermissionGroup.ClientAdministrator, Order = 1)]
	public const string ClientAdminUsersViewList = "GVUSERS";

	[Permission(Group = PermissionGroup.ClientAdministrator, Order = 2)]
	public const string ClientAdminUsersViewDetails = "GVUDET";

	[Permission(Group = PermissionGroup.ClientAdministrator, Order = 3)]
	public const string ClientAdminUsersAdd = "GAUSER";

	[Permission(Group = PermissionGroup.ClientAdministrator, Order = 4)]
	public const string ClientAdminUsersModify = "GMUSER";

	[Permission(Group = PermissionGroup.ClientAdministrator, Order = 5)]
	public const string ClientAdminClientModify = "CAMCLIENT";

	[Permission(Group = PermissionGroup.ClientAdministrator, Order = 6)]
	public const string ClientAdminAccessKeysGenerate = "CAGKEYS";

	[Permission(Group = PermissionGroup.Other, Order = 1)]
	public const string OverviewClientReports = "GVCLREP";

	[Permission(Group = PermissionGroup.Other, Order = 2)]
	public const string OverviewFactoringSubcontracts = "GVFACSC";

	[Permission(Group = PermissionGroup.SignableFilesClient, Module = "AmeriaEkengSign", Order = 1)]
	public const string ViewSignableFiles = "SFCVIEW";

	[Permission(Group = PermissionGroup.SignableFilesClient, Module = "AmeriaEkengSign", Order = 2)]
	public const string UploadSignedFile = "SFCUP";
}
