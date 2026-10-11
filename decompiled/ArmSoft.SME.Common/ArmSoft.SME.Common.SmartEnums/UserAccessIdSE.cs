using System.Collections.Generic;
using ArmSoft.SME.Common.Enums;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Base)]
public class UserAccessIdSE : ASSmartEnum<UserAccessIdSE, short>
{
	public static readonly UserAccessIdSE Currencies = new UserAccessIdSE("Currencies", 126, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Currencies);

	public static readonly UserAccessIdSE ExchangeRates = new UserAccessIdSE("ExchangeRates", 121, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.ExchangeRates);

	public static readonly UserAccessIdSE Banks = new UserAccessIdSE("Banks", 120, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Banks);

	public static readonly UserAccessIdSE DocLogs = new UserAccessIdSE("DocLogs", 357, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DocsLog);

	public static readonly UserAccessIdSE Templates = new UserAccessIdSE("Templates", 51, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Templates);

	public static readonly UserAccessIdSE DatabaseLog = new UserAccessIdSE("DatabaseLog", 65, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DatabaseLog);

	public static readonly UserAccessIdSE Databases = new UserAccessIdSE("Databases", 50, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Databases);

	public static readonly UserAccessIdSE DatabaseConnections = new UserAccessIdSE("DatabaseConnections", 53, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DatabaseConnections);

	public static readonly UserAccessIdSE Users = new UserAccessIdSE("Users", 48, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Users);

	public static readonly UserAccessIdSE RefreshServiceCaches = new UserAccessIdSE("RefreshServiceCaches", 1000, null, 0);

	public static readonly UserAccessIdSE CPAClassifications = new UserAccessIdSE("CPAClassifications", 509, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.CPAClassifications);

	public static readonly UserAccessIdSE Integrations = new UserAccessIdSE("Integrations", 1001, null, 0);

	public static readonly UserAccessIdSE ApiKeys = new UserAccessIdSE("ApiKeys", 1002, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.ApiKeys);

	public static readonly UserAccessIdSE Backups = new UserAccessIdSE("Backups", 49, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Backups);

	public static readonly UserAccessIdSE CheckIntegrity = new UserAccessIdSE("CheckIntegrity", 52, null, 0);

	public static readonly UserAccessIdSE Archive = new UserAccessIdSE("Archive", 54, null, 0);

	public static readonly UserAccessIdSE ImportData = new UserAccessIdSE("ImportData", 55, null, 0);

	public static readonly UserAccessIdSE ExportData = new UserAccessIdSE("ExportData", 56, null, 0);

	public static readonly UserAccessIdSE DirsLog = new UserAccessIdSE("DirsLog", 453, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DirsLog);

	public static readonly UserAccessIdSE Extensions = new UserAccessIdSE("Extensions", 420, null, 0);

	public static readonly UserAccessIdSE ImportExtensions = new UserAccessIdSE("ImportExtensions", 421, null, 0);

	public static readonly UserAccessIdSE DocumentsExtensions = new UserAccessIdSE("DocumentsExtensions", 422, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DocumentExtensions);

	public static readonly UserAccessIdSE DirectoriesExtensions = new UserAccessIdSE("DirectoriesExtensions", 1005, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DirectoryExtensions);

	public static readonly UserAccessIdSE DataViewsExtensions = new UserAccessIdSE("DataViewsExtensions", 423, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DataViewDefinitions);

	public static readonly UserAccessIdSE TreesDefinitions = new UserAccessIdSE("TreesDefinitions", 424, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Trees);

	public static readonly UserAccessIdSE UserScriptsDefinitions = new UserAccessIdSE("UserScriptsDefinitions", 435, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.UserScriptsDefinitions);

	public static readonly UserAccessIdSE AWPDefinitions = new UserAccessIdSE("AWPDefinitions", 454, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.AWPDefinitions);

	public static readonly UserAccessIdSE ContextFunctionDefinitions = new UserAccessIdSE("ContextFunctionDefinitions", 1007, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.ContextFunctionDefinitions);

	public static readonly UserAccessIdSE UserResources = new UserAccessIdSE("UserResources", 1006, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.UserResources);

	public static readonly UserAccessIdSE DialogValueTemplates = new UserAccessIdSE("DialogValueTemplates", 463, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DialogValueTemplates);

	public static readonly UserAccessIdSE MakeAssemblyFromExtensions = new UserAccessIdSE("MakeAssemblyFromExtensions", 425, null, 0);

	public static readonly UserAccessIdSE UserReportsDefinitions = new UserAccessIdSE("UserReportsDefinitions", 444, null, 0);

	public static readonly UserAccessIdSE UserRepTemplates = new UserAccessIdSE("UserRepTemplates", 445, null, 0);

	public static readonly UserAccessIdSE DebugCalls = new UserAccessIdSE("DebugCalls", 460, null, 0);

	public static readonly UserAccessIdSE OnlineLicenses = new UserAccessIdSE("OnlineLicenses", 451, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.OnlineLicenses);

	public static readonly UserAccessIdSE DataViewLayouts = new UserAccessIdSE("DataViewLayouts", 452, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DataViewLayouts);

	public static readonly UserAccessIdSE TaxService = new UserAccessIdSE("TaxService", 466, null, 0);

	public static readonly UserAccessIdSE TaxServiceTaxInvoices = new UserAccessIdSE("TaxServiceTaxInvoices", 467, null, 0);

	public static readonly UserAccessIdSE TaxServiceTaxInvoicesSubmitted = new UserAccessIdSE("TaxServiceTaxInvoicesSubmitted", 468, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceTaxInvoicesSubmitted);

	public static readonly UserAccessIdSE TaxServiceTaxInvoicesReceived = new UserAccessIdSE("TaxServiceTaxInvoicesReceived", 469, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceTaxInvoicesReceived);

	public static readonly UserAccessIdSE TaxServiceTaxInvoicesDraft = new UserAccessIdSE("TaxServiceTaxInvoicesDraft", 470, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceTaxInvoicesDraft);

	public static readonly UserAccessIdSE TaxServiceAccDocs = new UserAccessIdSE("TaxServiceAccDocs", 475, null, 0);

	public static readonly UserAccessIdSE TaxServiceAccDocsSubmitted = new UserAccessIdSE("TaxServiceAccDocsSubmitted", 476, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceAccDocsSubmitted);

	public static readonly UserAccessIdSE TaxServiceAccDocsReceived = new UserAccessIdSE("TaxServiceAccDocsReceived", 477, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceAccDocsReceived);

	public static readonly UserAccessIdSE TaxServiceAccDocsDraft = new UserAccessIdSE("TaxServiceAccDocsDraft", 478, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceAccDocsDraft);

	public static readonly UserAccessIdSE TaxServiceTaxInvoicesCancelled = new UserAccessIdSE("TaxServiceTaxInvoicesCancelled", 1003, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceTaxInvoicesCancelled);

	public static readonly UserAccessIdSE TaxServiceAccDocsCancelled = new UserAccessIdSE("TaxServiceAccDocsCancelled", 1004, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceAccDocsCancelled);

	public static readonly UserAccessIdSE DebugChangeDBVersion = new UserAccessIdSE("DebugChangeDBVersion", 395, null, 0);

	public static readonly UserAccessIdSE DebugExpPrintSubstitutions = new UserAccessIdSE("DebugExpPrintSubstitutions", 394, null, 0);

	public NavigationCommandRefObjectType? RefObjectType { get; private set; }

	public short RefObjectId { get; private set; }

	public List<short> SubsystemIds { get; set; }

	protected UserAccessIdSE(string name, short value, NavigationCommandRefObjectType? refObjectType = null, short refObjectId = 0, List<short> subsystemIds = null)
		: base(name, value)
	{
		RefObjectType = refObjectType;
		RefObjectId = refObjectId;
		SubsystemIds = subsystemIds;
	}
}
