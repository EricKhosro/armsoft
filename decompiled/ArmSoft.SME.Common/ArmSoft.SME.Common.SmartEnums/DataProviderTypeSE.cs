using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Base)]
public class DataProviderTypeSE : ASSmartEnum<DataProviderTypeSE, short>
{
	public static readonly DataProviderTypeSE Currencies = new DataProviderTypeSE("Currencies", 2, () => Resources.Curres);

	public static readonly DataProviderTypeSE ExchangeRates = new DataProviderTypeSE("ExchangeRates", 3, () => Resources.ExchangeRates);

	public static readonly DataProviderTypeSE Banks = new DataProviderTypeSE("Banks", 1, () => Resources.Banks);

	public static readonly DataProviderTypeSE DocsLog = new DataProviderTypeSE("DocsLog", 4, () => Resources.DocumentsLog);

	public static readonly DataProviderTypeSE DirsLog = new DataProviderTypeSE("DirsLog", 5, () => Resources.DirectoriesLog);

	public static readonly DataProviderTypeSE Templates = new DataProviderTypeSE("Templates", 6, () => Resources.PrintTemplates);

	public static readonly DataProviderTypeSE OnlineReportTemplates = new DataProviderTypeSE("OnlineReportTemplates", 34, () => Resources.OnlineReportsTemplates);

	public static readonly DataProviderTypeSE DatabaseLog = new DataProviderTypeSE("DatabaseLog", 7, () => Resources.UsersLoginHistory);

	public static readonly DataProviderTypeSE Databases = new DataProviderTypeSE("Databases", 8, () => Resources.Databases);

	public static readonly DataProviderTypeSE Backups = new DataProviderTypeSE("Backups", 23, () => Resources.Backups);

	public static readonly DataProviderTypeSE Users = new DataProviderTypeSE("Users", 12, () => Resources.Users);

	public static readonly DataProviderTypeSE UsersAccess = new DataProviderTypeSE("UsersAccess", 13, () => Resources.UserAccess);

	public static readonly DataProviderTypeSE DatabaseConnections = new DataProviderTypeSE("DatabaseConnections", 9, () => Resources.DatabaseConnections);

	public static readonly DataProviderTypeSE DirectoryLog = new DataProviderTypeSE("DirectoryLog", 10, () => Resources.History);

	public static readonly DataProviderTypeSE DocumentLinkedList = new DataProviderTypeSE("DocumentLinkedList", 11, () => Resources.LinkedDoc);

	public static readonly DataProviderTypeSE CPAClassifications = new DataProviderTypeSE("CPAClassifications", 14, () => Resources.CPAClassifications);

	public static readonly DataProviderTypeSE DataViewLayouts = new DataProviderTypeSE("DataViewLayouts", 24, () => Resources.DataViewLayouts);

	public static readonly DataProviderTypeSE OnlineLicenses = new DataProviderTypeSE("OnlineLicenses", 25, () => Resources.OnlineLicenses);

	public static readonly DataProviderTypeSE DocumentLog = new DataProviderTypeSE("DocumentLog", 26, () => Resources.History);

	public static readonly DataProviderTypeSE ApiKeys = new DataProviderTypeSE("ApiKeys", 38, () => Resources.ApiKeys);

	public static readonly DataProviderTypeSE TaxServiceTaxInvoicesSubmitted = new DataProviderTypeSE("TaxServiceTaxInvoicesSubmitted", 19, () => Resources.TaxInvoices + " (" + Resources.EInvoicingSubmitted + ")");

	public static readonly DataProviderTypeSE TaxServiceTaxInvoicesReceived = new DataProviderTypeSE("TaxServiceTaxInvoicesReceived", 17, () => Resources.TaxInvoices + " (" + Resources.EInvoicingReceived + ")");

	public static readonly DataProviderTypeSE TaxServiceTaxInvoicesDraft = new DataProviderTypeSE("TaxServiceTaxInvoicesDraft", 15, () => Resources.TaxInvoices + " (" + Resources.EInvoicingDraft + ")");

	public static readonly DataProviderTypeSE TaxServiceTaxInvoicesCancelled = new DataProviderTypeSE("TaxServiceTaxInvoicesCancelled", 36, () => Resources.TaxInvoices + " (" + Resources.EInvoicingCancelled + ")");

	public static readonly DataProviderTypeSE TaxServiceAccDocsSubmitted = new DataProviderTypeSE("TaxServiceAccDocsSubmitted", 20, () => Resources.TaxAccDocs + " (" + Resources.EInvoicingSubmitted + ")");

	public static readonly DataProviderTypeSE TaxServiceAccDocsReceived = new DataProviderTypeSE("TaxServiceAccDocsReceived", 18, () => Resources.TaxAccDocs + " (" + Resources.EInvoicingReceived + ")");

	public static readonly DataProviderTypeSE TaxServiceAccDocsDraft = new DataProviderTypeSE("TaxServiceAccDocsDraft", 16, () => Resources.TaxAccDocs + " (" + Resources.EInvoicingDraft + ")");

	public static readonly DataProviderTypeSE TaxServiceAccDocsCancelled = new DataProviderTypeSE("TaxServiceAccDocsCancelled", 37, () => Resources.TaxAccDocs + " (" + Resources.EInvoicingCancelled + ")");

	public static readonly DataProviderTypeSE IBTransactions = new DataProviderTypeSE("IBTransactions", 21, () => Resources.IBTransactions);

	public static readonly DataProviderTypeSE IBBankAccountsRemains = new DataProviderTypeSE("IBBankAccountsRemains", 22, () => Resources.AccountsBalance);

	public static readonly DataProviderTypeSE DataViewDefinitions = new DataProviderTypeSE("DataViewDefinitions", 30, () => Resources.DataViewsExtensions);

	public static readonly DataProviderTypeSE AWPDefinitions = new DataProviderTypeSE("AWPDefinitions", 27, () => Resources.AWPDefinitions);

	public static readonly DataProviderTypeSE UserScriptsDefinitions = new DataProviderTypeSE("UserScriptsDefinitions", 28, () => Resources.UserScriptsDefinitions);

	public static readonly DataProviderTypeSE Trees = new DataProviderTypeSE("Trees", 29, () => Resources.TreeDefs);

	public static readonly DataProviderTypeSE TreeNodes = new DataProviderTypeSE("TreeNodes", 31, () => Resources.TreeDefs);

	public static readonly DataProviderTypeSE TreeLog = new DataProviderTypeSE("TreeLog", 33, () => Resources.History);

	public static readonly DataProviderTypeSE ContextFunctionDefinitions = new DataProviderTypeSE("ContextFunctionDefinitions", 32, () => Resources.UserOperations);

	public static readonly DataProviderTypeSE DocumentExtensions = new DataProviderTypeSE("DocumentExtensions", 35, () => Resources.DocumentsExtensions);

	public static readonly DataProviderTypeSE DirectoryExtensions = new DataProviderTypeSE("DirectoryExtensions", 41, () => Resources.DirectoriesExtensions);

	public static readonly DataProviderTypeSE UserResources = new DataProviderTypeSE("UserResources", 39, () => Resources.UserResources);

	public static readonly DataProviderTypeSE DialogValueTemplates = new DataProviderTypeSE("DialogValueTemplates", 40, () => Resources.DialogValuesGroups);

	public const short BanksConst = 1;

	public const short CurrenciesConst = 2;

	public const short ExchangeRatesConst = 3;

	public const short DocsLogConst = 4;

	public const short DirsLogConst = 5;

	public const short TemplatesConst = 6;

	public const short DatabaseLogConst = 7;

	public const short DatabasesConst = 8;

	public const short DatabaseConnectionsConst = 9;

	public const short DirectoryLogConst = 10;

	public const short DocumentLinkedListConst = 11;

	public const short UsersConst = 12;

	public const short UsersAccessConst = 13;

	public const short CPAClassificationsConst = 14;

	public const short DraftTaxInvoicesConst = 15;

	public const short DraftAccDocsConst = 16;

	public const short ReceivedTaxInvoicesConst = 17;

	public const short ReceivedAccDocsConst = 18;

	public const short SubmittedTaxInvoicesConst = 19;

	public const short SubmittedAccDocsConst = 20;

	public const short IBTransactionsConst = 21;

	public const short IBBankAccountsRemainsConst = 22;

	public const short BackupsConst = 23;

	public const short DataViewLayoutsConst = 24;

	public const short OnlineLicensesConst = 25;

	public const short DocumentLogConst = 26;

	public const short AWPDefinitionsConst = 27;

	public const short UserScriptsDefinitionsConst = 28;

	public const short TreesConst = 29;

	public const short DataViewDefinitionsConst = 30;

	public const short TreeNodesConst = 31;

	public const short ContextFunctionDefinitionsConst = 32;

	public const short TreeLogConst = 33;

	public const short OnlineReportTemplatesConst = 34;

	public const short DocumentExtensionsConst = 35;

	public const short CancelledTaxInvoicesConst = 36;

	public const short CancelledAccDocsConst = 37;

	public const short ApiKeysConst = 38;

	public const short UserResourcesConst = 39;

	public const short DialogValueTemplatesConst = 40;

	public const short DirectoryExtensionsConst = 41;

	private DataProviderTypeSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
