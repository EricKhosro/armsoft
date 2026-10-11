using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Special)]
[SmartEnumPartial(SmartEnumType.Base)]
public class UserAccessIdSE : ASSmartEnum<UserAccessIdSE, short>
{
	public static readonly UserAccessIdSE None;

	public static readonly UserAccessIdSE PurchaseDocuments;

	public static readonly UserAccessIdSE PurchaseSubDocuments;

	public static readonly UserAccessIdSE PurchaseJournals;

	public static readonly UserAccessIdSE PurchaseOperations;

	public static readonly UserAccessIdSE PurchaseReports;

	public static readonly UserAccessIdSE PurchasePartnersReports;

	public static readonly UserAccessIdSE PurchaseBonusReports;

	public static readonly UserAccessIdSE PurchaseCashBankReports;

	public static readonly UserAccessIdSE PurchaseDirectories;

	public static readonly UserAccessIdSE PurchaseGoodsDirectories;

	public static readonly UserAccessIdSE PurchasePartnersDirectories;

	public static readonly UserAccessIdSE PurchaseOtherDirectories;

	public static readonly UserAccessIdSE SalesSubDocuments;

	public static readonly UserAccessIdSE SaleDocuments;

	public static readonly UserAccessIdSE SaleJournals;

	public static readonly UserAccessIdSE SaleOperations;

	public static readonly UserAccessIdSE SaleReports;

	public static readonly UserAccessIdSE SalesRetailReports;

	public static readonly UserAccessIdSE SalesPartnersReports;

	public static readonly UserAccessIdSE SalesBonusReports;

	public static readonly UserAccessIdSE SalesCashBankReports;

	public static readonly UserAccessIdSE SaleDirectories;

	public static readonly UserAccessIdSE SalesGoodsDirectories;

	public static readonly UserAccessIdSE SalesPartnersDirectories;

	public static readonly UserAccessIdSE SalesOtherDirectories;

	public static readonly UserAccessIdSE PointOfSaleDocuments;

	public static readonly UserAccessIdSE PointOfSaleOperations;

	public static readonly UserAccessIdSE PointOfSaleReports;

	public static readonly UserAccessIdSE StorageDocuments;

	public static readonly UserAccessIdSE StorageSubDocuments;

	public static readonly UserAccessIdSE StorageJournals;

	public static readonly UserAccessIdSE StorageOperations;

	public static readonly UserAccessIdSE StorageReports;

	public static readonly UserAccessIdSE StorageRetailReports;

	public static readonly UserAccessIdSE StorageDirectories;

	public static readonly UserAccessIdSE StorageGoodsDirectories;

	public static readonly UserAccessIdSE CashBankDocuments;

	public static readonly UserAccessIdSE CashBankJournals;

	public static readonly UserAccessIdSE CashBankOperations;

	public static readonly UserAccessIdSE CashBankReports;

	public static readonly UserAccessIdSE CashBankPartnersReports;

	public static readonly UserAccessIdSE CashBankDirectories;

	public static readonly UserAccessIdSE CashBankPartnersDirectories;

	public static readonly UserAccessIdSE AnalysisReports;

	public static readonly UserAccessIdSE b2bJournals;

	public static readonly UserAccessIdSE b2bDirectories;

	public static readonly UserAccessIdSE b2bOperations;

	public static readonly UserAccessIdSE b2bSynchronization;

	public static readonly UserAccessIdSE b2bPublishWizard;

	public static readonly UserAccessIdSE eInvJournals;

	public static readonly UserAccessIdSE eInvWriteOffJournals;

	public static readonly UserAccessIdSE eInvReceivedJournals;

	public static readonly UserAccessIdSE eInvOperations;

	public static readonly UserAccessIdSE DeletedDocumentsJournal;

	public static readonly UserAccessIdSE MTIncome;

	public static readonly UserAccessIdSE MTIncomeImport;

	public static readonly UserAccessIdSE MTIncomeRetail;

	public static readonly UserAccessIdSE MTOverheadExpenses;

	public static readonly UserAccessIdSE ReturnToSupplier;

	public static readonly UserAccessIdSE Sale;

	public static readonly UserAccessIdSE MTBill;

	public static readonly UserAccessIdSE MTBillReturn;

	public static readonly UserAccessIdSE MTInvoice;

	public static readonly UserAccessIdSE MTSaleRetail;

	public static readonly UserAccessIdSE ReturnFromCustomer;

	public static readonly UserAccessIdSE CashIn;

	public static readonly UserAccessIdSE CashOut;

	public static readonly UserAccessIdSE PayOrd;

	public static readonly UserAccessIdSE BankIn;

	public static readonly UserAccessIdSE BankOut;

	public static readonly UserAccessIdSE MTMove;

	public static readonly UserAccessIdSE MTInventory;

	public static readonly UserAccessIdSE MTInventoryRetail;

	public static readonly UserAccessIdSE MTRevaluationRetail;

	public static readonly UserAccessIdSE MTExpense;

	public static readonly UserAccessIdSE MTComplectation;

	public static readonly UserAccessIdSE MTDeComplectation;

	public static readonly UserAccessIdSE Revaluation;

	public static readonly UserAccessIdSE PartRemCorrect;

	public static readonly UserAccessIdSE PartRemGroupCorrect;

	public static readonly UserAccessIdSE PartRemMove;

	public static readonly UserAccessIdSE BonusGroupCorrect;

	public static readonly UserAccessIdSE MTIncomeRem;

	public static readonly UserAccessIdSE PartnersGroupRevaluation;

	public static readonly UserAccessIdSE BoughtServices;

	public static readonly UserAccessIdSE MTPurchaseOrder;

	public static readonly UserAccessIdSE InvoiceForPay;

	public static readonly UserAccessIdSE MTRevaluation;

	public static readonly UserAccessIdSE ReconciliationStatementDoc;

	public static readonly UserAccessIdSE MTMoveOrder;

	public static readonly UserAccessIdSE EMarkReceiptDoc;

	public static readonly UserAccessIdSE MTMoveWithConfirmation;

	public static readonly UserAccessIdSE AssignTaxInvNums;

	public static readonly UserAccessIdSE AssignAccDocInvNums;

	public static readonly UserAccessIdSE ImportTaxInvoice;

	public static readonly UserAccessIdSE ImportPurchaseOrders;

	public static readonly UserAccessIdSE PrintECRReport;

	public static readonly UserAccessIdSE PrintECRReportPOS;

	public static readonly UserAccessIdSE OpenCashierShift;

	public static readonly UserAccessIdSE SyncECR;

	public static readonly UserAccessIdSE SyncECRPOS;

	public static readonly UserAccessIdSE CloseCashierShift;

	public static readonly UserAccessIdSE ExportForElScalesAndTerminals;

	public static readonly UserAccessIdSE ECRConfiguration;

	public static readonly UserAccessIdSE CashierShiftReport;

	public static readonly UserAccessIdSE PurchaseDocumentsJournal;

	public static readonly UserAccessIdSE SaleDocumentsJournal;

	public static readonly UserAccessIdSE TaxInvoicesJournal;

	public static readonly UserAccessIdSE WrittenOffInvoicesForPay;

	public static readonly UserAccessIdSE ReconciliationStatementsJournal;

	public static readonly UserAccessIdSE ReceivedInvoicesJournal;

	public static readonly UserAccessIdSE WarehouseDocumentsJournal;

	public static readonly UserAccessIdSE CashDocumentsJournal;

	public static readonly UserAccessIdSE BankDocumentsJournal;

	public static readonly UserAccessIdSE TransportationDocumentsJournal;

	public static readonly UserAccessIdSE ECRCheckDocumentsJournal;

	public static readonly UserAccessIdSE LocalDBDocumnets;

	public static readonly UserAccessIdSE ECRReport;

	public static readonly UserAccessIdSE ECRCheckDocumentsListJournal;

	public static readonly UserAccessIdSE EMarkReceipListJournal;

	public static readonly UserAccessIdSE GoodsStockTakingRep;

	public static readonly UserAccessIdSE MatRepAvailabilityTreeView;

	public static readonly UserAccessIdSE MatRepAvailability;

	public static readonly UserAccessIdSE MatRepTurnOver;

	public static readonly UserAccessIdSE MatRepOperationsJournal;

	public static readonly UserAccessIdSE MatRepTAccount;

	public static readonly UserAccessIdSE MTPriceList;

	public static readonly UserAccessIdSE PartRemains;

	public static readonly UserAccessIdSE PartTurnOver;

	public static readonly UserAccessIdSE PartOpJournal;

	public static readonly UserAccessIdSE PartRepCard;

	public static readonly UserAccessIdSE BonusRemains;

	public static readonly UserAccessIdSE BonusTurnOver;

	public static readonly UserAccessIdSE BonusOpJournal;

	public static readonly UserAccessIdSE BonusTAccount;

	public static readonly UserAccessIdSE CashRemains;

	public static readonly UserAccessIdSE CashTurnOver;

	public static readonly UserAccessIdSE CashOpJournal;

	public static readonly UserAccessIdSE CashDeskTAccount;

	public static readonly UserAccessIdSE BankRemains;

	public static readonly UserAccessIdSE BankTurnOver;

	public static readonly UserAccessIdSE BankOpJournal;

	public static readonly UserAccessIdSE BankTAccount;

	public static readonly UserAccessIdSE MatRepSalesAnalysis;

	public static readonly UserAccessIdSE MatRepSalesAverage;

	public static readonly UserAccessIdSE MatRepAvailabilityRetail;

	public static readonly UserAccessIdSE MatRepTurnOverRetail;

	public static readonly UserAccessIdSE MatRepOperationsRetail;

	public static readonly UserAccessIdSE GoodsCostCalculation;

	public static readonly UserAccessIdSE ReceivedServicesJournal;

	public static readonly UserAccessIdSE DiscountLogsJournal;

	public static readonly UserAccessIdSE Goods;

	public static readonly UserAccessIdSE ImportFromUnitedProducts;

	public static readonly UserAccessIdSE B2BProductsCheck;

	public static readonly UserAccessIdSE MTGroups;

	public static readonly UserAccessIdSE SubstituteItemsGroups;

	public static readonly UserAccessIdSE PartnersMT;

	public static readonly UserAccessIdSE Storages;

	public static readonly UserAccessIdSE Partners;

	public static readonly UserAccessIdSE PartGroups;

	public static readonly UserAccessIdSE Segments;

	public static readonly UserAccessIdSE GoodsBySegment;

	public static readonly UserAccessIdSE CashDesks;

	public static readonly UserAccessIdSE CashierShift;

	public static readonly UserAccessIdSE ElScalesAndTerminals;

	public static readonly UserAccessIdSE AccountsInBank;

	public static readonly UserAccessIdSE QntUnits;

	public static readonly UserAccessIdSE PriceTypes;

	public static readonly UserAccessIdSE Countries;

	public static readonly UserAccessIdSE Makers;

	public static readonly UserAccessIdSE PropertiesTypes;

	public static readonly UserAccessIdSE StandardComments;

	public static readonly UserAccessIdSE DiscountGroups;

	public static readonly UserAccessIdSE Discounts;

	public static readonly UserAccessIdSE BonusScale;

	public static readonly UserAccessIdSE Employees;

	public static readonly UserAccessIdSE MTQntUnits;

	public static readonly UserAccessIdSE MTBarCodes;

	public static readonly UserAccessIdSE MTPrices;

	public static readonly UserAccessIdSE OrderMinMax;

	public static readonly UserAccessIdSE MTComplectsParts;

	public static readonly UserAccessIdSE PartContracts;

	public static readonly UserAccessIdSE MTComplects;

	public static readonly UserAccessIdSE PropertiesListsValues;

	public static readonly UserAccessIdSE PartnerAccountsInBank;

	public static readonly UserAccessIdSE IBImportTransactions;

	public static readonly UserAccessIdSE CBAImportExchangeRates;

	public static readonly UserAccessIdSE ImportBanksFromB2B;

	public static readonly UserAccessIdSE DocumentLinkedList;

	public static readonly UserAccessIdSE PaymentOrdersJournal;

	public static readonly UserAccessIdSE IBTransactions;

	public static readonly UserAccessIdSE PurchaseOrdersDocsJournal;

	public static readonly UserAccessIdSE DocumentsJournal;

	public static readonly UserAccessIdSE ChangeActualnessDate;

	public static readonly UserAccessIdSE B2BDocuments;

	public static readonly UserAccessIdSE B2BProducts;

	public static readonly UserAccessIdSE B2BInvoicesForPay;

	public static readonly UserAccessIdSE B2BReceivedDocs;

	public static readonly UserAccessIdSE B2BSentDocs;

	public static readonly UserAccessIdSE B2BOrganizationData;

	public static readonly UserAccessIdSE DiscountServerBonusCards;

	public static readonly UserAccessIdSE DiscountServerBonusTransactions;

	public static readonly UserAccessIdSE DiscountServerBonusSumComparison;

	public static readonly UserAccessIdSE OnlineBankQuering;

	public static readonly UserAccessIdSE OnlineAccountStatement;

	public static readonly UserAccessIdSE OnlineAccountsRemains;

	public static readonly UserAccessIdSE MerchantStatements;

	public static readonly UserAccessIdSE PublishSettings;

	public static readonly UserAccessIdSE b2bReports;

	public static readonly UserAccessIdSE EMarkReceiptApp;

	public static readonly UserAccessIdSE AnalysisDashBoard;

	public static readonly UserAccessIdSE SalesAnalysisDiagram;

	public static readonly UserAccessIdSE SalesAnalysisBestSellersReport;

	public static readonly UserAccessIdSE AnalysisDebtorsCreditorsRems;

	public static readonly UserAccessIdSE CashBankAnalysisReport;

	public static readonly UserAccessIdSE GoodsABCAnalysisReport;

	public static readonly UserAccessIdSE PartnersABCAnalysisReport;

	public static readonly UserAccessIdSE DebugDialogTest;

	public static readonly UserAccessIdSE DebugReportTest;

	public static readonly UserAccessIdSE DebugCreateStoredProcedures;

	public static readonly UserAccessIdSE DebugTransactionsControl;

	public static readonly UserAccessIdSE DebugFinalizeTracer;

	public static readonly UserAccessIdSE DebugExportMetadata;

	public static readonly UserAccessIdSE DebugAppTokenViewer;

	public static readonly UserAccessIdSE DebugRunDiscountServerTests;

	public static readonly UserAccessIdSE DebugNotificationTest;

	public static readonly UserAccessIdSE UserCashesAccess;

	public static readonly UserAccessIdSE UserStorageOptions;

	public static readonly UserAccessIdSE ImportDataFromASAccountantWindow;

	public static readonly UserAccessIdSE DatabasesOperations;

	public static readonly UserAccessIdSE Currencies;

	public static readonly UserAccessIdSE ExchangeRates;

	public static readonly UserAccessIdSE Banks;

	public static readonly UserAccessIdSE DocLogs;

	public static readonly UserAccessIdSE Templates;

	public static readonly UserAccessIdSE DatabaseLog;

	public static readonly UserAccessIdSE Databases;

	public static readonly UserAccessIdSE DatabaseConnections;

	public static readonly UserAccessIdSE Users;

	public static readonly UserAccessIdSE RefreshServiceCaches;

	public static readonly UserAccessIdSE CPAClassifications;

	public static readonly UserAccessIdSE Integrations;

	public static readonly UserAccessIdSE ApiKeys;

	public static readonly UserAccessIdSE Backups;

	public static readonly UserAccessIdSE CheckIntegrity;

	public static readonly UserAccessIdSE Archive;

	public static readonly UserAccessIdSE ImportData;

	public static readonly UserAccessIdSE ExportData;

	public static readonly UserAccessIdSE DirsLog;

	public static readonly UserAccessIdSE Extensions;

	public static readonly UserAccessIdSE ImportExtensions;

	public static readonly UserAccessIdSE DocumentsExtensions;

	public static readonly UserAccessIdSE DirectoriesExtensions;

	public static readonly UserAccessIdSE DataViewsExtensions;

	public static readonly UserAccessIdSE TreesDefinitions;

	public static readonly UserAccessIdSE UserScriptsDefinitions;

	public static readonly UserAccessIdSE AWPDefinitions;

	public static readonly UserAccessIdSE ContextFunctionDefinitions;

	public static readonly UserAccessIdSE UserResources;

	public static readonly UserAccessIdSE DialogValueTemplates;

	public static readonly UserAccessIdSE MakeAssemblyFromExtensions;

	public static readonly UserAccessIdSE UserReportsDefinitions;

	public static readonly UserAccessIdSE UserRepTemplates;

	public static readonly UserAccessIdSE DebugCalls;

	public static readonly UserAccessIdSE OnlineLicenses;

	public static readonly UserAccessIdSE DataViewLayouts;

	public static readonly UserAccessIdSE TaxService;

	public static readonly UserAccessIdSE TaxServiceTaxInvoices;

	public static readonly UserAccessIdSE TaxServiceTaxInvoicesSubmitted;

	public static readonly UserAccessIdSE TaxServiceTaxInvoicesReceived;

	public static readonly UserAccessIdSE TaxServiceTaxInvoicesDraft;

	public static readonly UserAccessIdSE TaxServiceAccDocs;

	public static readonly UserAccessIdSE TaxServiceAccDocsSubmitted;

	public static readonly UserAccessIdSE TaxServiceAccDocsReceived;

	public static readonly UserAccessIdSE TaxServiceAccDocsDraft;

	public static readonly UserAccessIdSE TaxServiceTaxInvoicesCancelled;

	public static readonly UserAccessIdSE TaxServiceAccDocsCancelled;

	public static readonly UserAccessIdSE DebugChangeDBVersion;

	public static readonly UserAccessIdSE DebugExpPrintSubstitutions;

	public NavigationCommandRefObjectType? RefObjectType { get; private set; }

	public short RefObjectId { get; private set; }

	public List<short> SubsystemIds { get; set; }

	static UserAccessIdSE()
	{
		None = new UserAccessIdSE("None", 0, null, 0);
		PurchaseDocuments = new UserAccessIdSE("PurchaseDocuments", 7, null, 0);
		PurchaseSubDocuments = new UserAccessIdSE("PurchaseSubDocuments", 8, null, 0);
		PurchaseJournals = new UserAccessIdSE("PurchaseJournals", 9, null, 0);
		PurchaseOperations = new UserAccessIdSE("PurchaseOperations", 10, null, 0);
		PurchaseReports = new UserAccessIdSE("PurchaseReports", 11, null, 0);
		PurchasePartnersReports = new UserAccessIdSE("PurchasePartnersReports", 12, null, 0);
		PurchaseBonusReports = new UserAccessIdSE("PurchaseBonusReports", 13, null, 0);
		PurchaseCashBankReports = new UserAccessIdSE("PurchaseCashBankReports", 14, null, 0);
		PurchaseDirectories = new UserAccessIdSE("PurchaseDirectories", 15, null, 0);
		PurchaseGoodsDirectories = new UserAccessIdSE("PurchaseGoodsDirectories", 16, null, 0);
		PurchasePartnersDirectories = new UserAccessIdSE("PurchasePartnersDirectories", 17, null, 0);
		PurchaseOtherDirectories = new UserAccessIdSE("PurchaseOtherDirectories", 18, null, 0);
		SalesSubDocuments = new UserAccessIdSE("SalesSubDocuments", 19, null, 0);
		SaleDocuments = new UserAccessIdSE("SaleDocuments", 20, null, 0);
		SaleJournals = new UserAccessIdSE("SaleJournals", 21, null, 0);
		SaleOperations = new UserAccessIdSE("SaleOperations", 22, null, 0);
		SaleReports = new UserAccessIdSE("SaleReports", 23, null, 0);
		SalesRetailReports = new UserAccessIdSE("SalesRetailReports", 24, null, 0);
		SalesPartnersReports = new UserAccessIdSE("SalesPartnersReports", 25, null, 0);
		SalesBonusReports = new UserAccessIdSE("SalesBonusReports", 26, null, 0);
		SalesCashBankReports = new UserAccessIdSE("SalesCashBankReports", 27, null, 0);
		SaleDirectories = new UserAccessIdSE("SaleDirectories", 28, null, 0);
		SalesGoodsDirectories = new UserAccessIdSE("SalesGoodsDirectories", 29, null, 0);
		SalesPartnersDirectories = new UserAccessIdSE("SalesPartnersDirectories", 30, null, 0);
		SalesOtherDirectories = new UserAccessIdSE("SalesOtherDirectories", 31, null, 0);
		PointOfSaleDocuments = new UserAccessIdSE("PointOfSaleDocuments", 61, null, 0);
		PointOfSaleOperations = new UserAccessIdSE("PointOfSaleOperations", 62, null, 0);
		PointOfSaleReports = new UserAccessIdSE("PointOfSaleReports", 63, null, 0);
		StorageDocuments = new UserAccessIdSE("StorageDocuments", 32, null, 0);
		StorageSubDocuments = new UserAccessIdSE("StorageSubDocuments", 33, null, 0);
		StorageJournals = new UserAccessIdSE("StorageJournals", 34, null, 0);
		StorageOperations = new UserAccessIdSE("StorageOperations", 35, null, 0);
		StorageReports = new UserAccessIdSE("StorageReports", 36, null, 0);
		StorageRetailReports = new UserAccessIdSE("StorageRetailReports", 37, null, 0);
		StorageDirectories = new UserAccessIdSE("StorageDirectories", 38, null, 0);
		StorageGoodsDirectories = new UserAccessIdSE("StorageGoodsDirectories", 39, null, 0);
		CashBankDocuments = new UserAccessIdSE("CashBankDocuments", 40, null, 0);
		CashBankJournals = new UserAccessIdSE("CashBankJournals", 41, null, 0);
		CashBankOperations = new UserAccessIdSE("CashBankOperations", 42, null, 0);
		CashBankReports = new UserAccessIdSE("CashBankReports", 43, null, 0);
		CashBankPartnersReports = new UserAccessIdSE("CashBankPartnersReports", 44, null, 0);
		CashBankDirectories = new UserAccessIdSE("CashBankDirectories", 45, null, 0);
		CashBankPartnersDirectories = new UserAccessIdSE("CashBankPartnersDirectories", 46, null, 0);
		AnalysisReports = new UserAccessIdSE("AnalysisReports", 47, null, 0);
		b2bJournals = new UserAccessIdSE("b2bJournals", 59, null, 0);
		b2bDirectories = new UserAccessIdSE("b2bDirectories", 60, null, 0);
		b2bOperations = new UserAccessIdSE("b2bOperations", 66, null, 0);
		b2bSynchronization = new UserAccessIdSE("b2bSynchronization", 67, null, 0);
		int num = 1;
		List<short> list = new List<short>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = 6;
		List<short> subsystemIds = list;
		b2bPublishWizard = new UserAccessIdSE("b2bPublishWizard", 68, null, 0, subsystemIds);
		eInvJournals = new UserAccessIdSE("eInvJournals", 69, null, 0);
		eInvWriteOffJournals = new UserAccessIdSE("eInvWriteOffJournals", 70, null, 0);
		eInvReceivedJournals = new UserAccessIdSE("eInvReceivedJournals", 71, null, 0);
		eInvOperations = new UserAccessIdSE("eInvOperations", 72, null, 0);
		NavigationCommandRefObjectType? refObjectType = NavigationCommandRefObjectType.DataProvider;
		short refObjectId = DataProviderTypeSE.DeletedDocumentsJournal;
		num = 1;
		List<short> list2 = new List<short>(num);
		CollectionsMarshal.SetCount(list2, num);
		CollectionsMarshal.AsSpan(list2)[0] = 9;
		DeletedDocumentsJournal = new UserAccessIdSE("DeletedDocumentsJournal", 58, refObjectType, refObjectId, list2);
		NavigationCommandRefObjectType? refObjectType2 = NavigationCommandRefObjectType.Document;
		short refObjectId2 = DocumentTypeSE.MTIncome;
		num = 2;
		List<short> list3 = new List<short>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<short> span = CollectionsMarshal.AsSpan(list3);
		span[0] = 1;
		span[1] = 3;
		MTIncome = new UserAccessIdSE("MTIncome", 130, refObjectType2, refObjectId2, list3);
		NavigationCommandRefObjectType? refObjectType3 = NavigationCommandRefObjectType.Document;
		short refObjectId3 = DocumentTypeSE.MTIncomeImport;
		num = 2;
		List<short> list4 = new List<short>(num);
		CollectionsMarshal.SetCount(list4, num);
		Span<short> span2 = CollectionsMarshal.AsSpan(list4);
		span2[0] = 1;
		span2[1] = 3;
		MTIncomeImport = new UserAccessIdSE("MTIncomeImport", 156, refObjectType3, refObjectId3, list4);
		NavigationCommandRefObjectType? refObjectType4 = NavigationCommandRefObjectType.Document;
		short refObjectId4 = DocumentTypeSE.MTIncomeRetail;
		num = 2;
		List<short> list5 = new List<short>(num);
		CollectionsMarshal.SetCount(list5, num);
		Span<short> span3 = CollectionsMarshal.AsSpan(list5);
		span3[0] = 1;
		span3[1] = 3;
		MTIncomeRetail = new UserAccessIdSE("MTIncomeRetail", 157, refObjectType4, refObjectId4, list5);
		NavigationCommandRefObjectType? refObjectType5 = NavigationCommandRefObjectType.Document;
		short refObjectId5 = DocumentTypeSE.MTOverheadExp;
		num = 1;
		List<short> list6 = new List<short>(num);
		CollectionsMarshal.SetCount(list6, num);
		CollectionsMarshal.AsSpan(list6)[0] = 1;
		MTOverheadExpenses = new UserAccessIdSE("MTOverheadExpenses", 131, refObjectType5, refObjectId5, list6);
		NavigationCommandRefObjectType? refObjectType6 = NavigationCommandRefObjectType.Document;
		short refObjectId6 = DocumentTypeSE.MTReturnToSupplier;
		num = 1;
		List<short> list7 = new List<short>(num);
		CollectionsMarshal.SetCount(list7, num);
		CollectionsMarshal.AsSpan(list7)[0] = 1;
		ReturnToSupplier = new UserAccessIdSE("ReturnToSupplier", 132, refObjectType6, refObjectId6, list7);
		num = 1;
		List<short> list8 = new List<short>(num);
		CollectionsMarshal.SetCount(list8, num);
		CollectionsMarshal.AsSpan(list8)[0] = 7;
		subsystemIds = list8;
		Sale = new UserAccessIdSE("Sale", 399, null, 0, subsystemIds);
		NavigationCommandRefObjectType? refObjectType7 = NavigationCommandRefObjectType.Document;
		short refObjectId7 = DocumentTypeSE.MTBill;
		num = 2;
		List<short> list9 = new List<short>(num);
		CollectionsMarshal.SetCount(list9, num);
		Span<short> span4 = CollectionsMarshal.AsSpan(list9);
		span4[0] = 2;
		span4[1] = 7;
		MTBill = new UserAccessIdSE("MTBill", 155, refObjectType7, refObjectId7, list9);
		NavigationCommandRefObjectType? refObjectType8 = NavigationCommandRefObjectType.Document;
		short refObjectId8 = DocumentTypeSE.MTBillReturn;
		num = 2;
		List<short> list10 = new List<short>(num);
		CollectionsMarshal.SetCount(list10, num);
		Span<short> span5 = CollectionsMarshal.AsSpan(list10);
		span5[0] = 2;
		span5[1] = 7;
		MTBillReturn = new UserAccessIdSE("MTBillReturn", 171, refObjectType8, refObjectId8, list10);
		NavigationCommandRefObjectType? refObjectType9 = NavigationCommandRefObjectType.Document;
		short refObjectId9 = DocumentTypeSE.MTInvoice;
		num = 1;
		List<short> list11 = new List<short>(num);
		CollectionsMarshal.SetCount(list11, num);
		CollectionsMarshal.AsSpan(list11)[0] = 2;
		MTInvoice = new UserAccessIdSE("MTInvoice", 133, refObjectType9, refObjectId9, list11);
		NavigationCommandRefObjectType? refObjectType10 = NavigationCommandRefObjectType.Document;
		short refObjectId10 = DocumentTypeSE.MTSaleRetail;
		num = 1;
		List<short> list12 = new List<short>(num);
		CollectionsMarshal.SetCount(list12, num);
		CollectionsMarshal.AsSpan(list12)[0] = 2;
		MTSaleRetail = new UserAccessIdSE("MTSaleRetail", 158, refObjectType10, refObjectId10, list12);
		NavigationCommandRefObjectType? refObjectType11 = NavigationCommandRefObjectType.Document;
		short refObjectId11 = DocumentTypeSE.MTReturnFromCustomer;
		num = 2;
		List<short> list13 = new List<short>(num);
		CollectionsMarshal.SetCount(list13, num);
		Span<short> span6 = CollectionsMarshal.AsSpan(list13);
		span6[0] = 2;
		span6[1] = 7;
		ReturnFromCustomer = new UserAccessIdSE("ReturnFromCustomer", 134, refObjectType11, refObjectId11, list13);
		NavigationCommandRefObjectType? refObjectType12 = NavigationCommandRefObjectType.Document;
		short refObjectId12 = DocumentTypeSE.CashIn;
		num = 2;
		List<short> list14 = new List<short>(num);
		CollectionsMarshal.SetCount(list14, num);
		Span<short> span7 = CollectionsMarshal.AsSpan(list14);
		span7[0] = 2;
		span7[1] = 4;
		CashIn = new UserAccessIdSE("CashIn", 135, refObjectType12, refObjectId12, list14);
		NavigationCommandRefObjectType? refObjectType13 = NavigationCommandRefObjectType.Document;
		short refObjectId13 = DocumentTypeSE.CashOut;
		num = 2;
		List<short> list15 = new List<short>(num);
		CollectionsMarshal.SetCount(list15, num);
		Span<short> span8 = CollectionsMarshal.AsSpan(list15);
		span8[0] = 1;
		span8[1] = 4;
		CashOut = new UserAccessIdSE("CashOut", 136, refObjectType13, refObjectId13, list15);
		NavigationCommandRefObjectType? refObjectType14 = NavigationCommandRefObjectType.Document;
		short refObjectId14 = DocumentTypeSE.PayOrd;
		num = 2;
		List<short> list16 = new List<short>(num);
		CollectionsMarshal.SetCount(list16, num);
		Span<short> span9 = CollectionsMarshal.AsSpan(list16);
		span9[0] = 1;
		span9[1] = 4;
		PayOrd = new UserAccessIdSE("PayOrd", 137, refObjectType14, refObjectId14, list16);
		NavigationCommandRefObjectType? refObjectType15 = NavigationCommandRefObjectType.Document;
		short refObjectId15 = DocumentTypeSE.BankIn;
		num = 2;
		List<short> list17 = new List<short>(num);
		CollectionsMarshal.SetCount(list17, num);
		Span<short> span10 = CollectionsMarshal.AsSpan(list17);
		span10[0] = 2;
		span10[1] = 4;
		BankIn = new UserAccessIdSE("BankIn", 138, refObjectType15, refObjectId15, list17);
		NavigationCommandRefObjectType? refObjectType16 = NavigationCommandRefObjectType.Document;
		short refObjectId16 = DocumentTypeSE.BankOut;
		num = 2;
		List<short> list18 = new List<short>(num);
		CollectionsMarshal.SetCount(list18, num);
		Span<short> span11 = CollectionsMarshal.AsSpan(list18);
		span11[0] = 1;
		span11[1] = 4;
		BankOut = new UserAccessIdSE("BankOut", 139, refObjectType16, refObjectId16, list18);
		NavigationCommandRefObjectType? refObjectType17 = NavigationCommandRefObjectType.Document;
		short refObjectId17 = DocumentTypeSE.MTMove;
		num = 1;
		List<short> list19 = new List<short>(num);
		CollectionsMarshal.SetCount(list19, num);
		CollectionsMarshal.AsSpan(list19)[0] = 3;
		MTMove = new UserAccessIdSE("MTMove", 140, refObjectType17, refObjectId17, list19);
		NavigationCommandRefObjectType? refObjectType18 = NavigationCommandRefObjectType.Document;
		short refObjectId18 = DocumentTypeSE.MTStockTaking;
		num = 1;
		List<short> list20 = new List<short>(num);
		CollectionsMarshal.SetCount(list20, num);
		CollectionsMarshal.AsSpan(list20)[0] = 3;
		MTInventory = new UserAccessIdSE("MTInventory", 141, refObjectType18, refObjectId18, list20);
		NavigationCommandRefObjectType? refObjectType19 = NavigationCommandRefObjectType.Document;
		short refObjectId19 = DocumentTypeSE.MTStockTakingRetail;
		num = 1;
		List<short> list21 = new List<short>(num);
		CollectionsMarshal.SetCount(list21, num);
		CollectionsMarshal.AsSpan(list21)[0] = 3;
		MTInventoryRetail = new UserAccessIdSE("MTInventoryRetail", 159, refObjectType19, refObjectId19, list21);
		NavigationCommandRefObjectType? refObjectType20 = NavigationCommandRefObjectType.Document;
		short refObjectId20 = DocumentTypeSE.MTRevaluationRetail;
		num = 1;
		List<short> list22 = new List<short>(num);
		CollectionsMarshal.SetCount(list22, num);
		CollectionsMarshal.AsSpan(list22)[0] = 3;
		MTRevaluationRetail = new UserAccessIdSE("MTRevaluationRetail", 160, refObjectType20, refObjectId20, list22);
		NavigationCommandRefObjectType? refObjectType21 = NavigationCommandRefObjectType.Document;
		short refObjectId21 = DocumentTypeSE.MTExpense;
		num = 1;
		List<short> list23 = new List<short>(num);
		CollectionsMarshal.SetCount(list23, num);
		CollectionsMarshal.AsSpan(list23)[0] = 3;
		MTExpense = new UserAccessIdSE("MTExpense", 142, refObjectType21, refObjectId21, list23);
		NavigationCommandRefObjectType? refObjectType22 = NavigationCommandRefObjectType.Document;
		short refObjectId22 = DocumentTypeSE.MTComplectation;
		num = 1;
		List<short> list24 = new List<short>(num);
		CollectionsMarshal.SetCount(list24, num);
		CollectionsMarshal.AsSpan(list24)[0] = 3;
		MTComplectation = new UserAccessIdSE("MTComplectation", 143, refObjectType22, refObjectId22, list24);
		NavigationCommandRefObjectType? refObjectType23 = NavigationCommandRefObjectType.Document;
		short refObjectId23 = DocumentTypeSE.MTDeComplectation;
		num = 1;
		List<short> list25 = new List<short>(num);
		CollectionsMarshal.SetCount(list25, num);
		CollectionsMarshal.AsSpan(list25)[0] = 3;
		MTDeComplectation = new UserAccessIdSE("MTDeComplectation", 144, refObjectType23, refObjectId23, list25);
		NavigationCommandRefObjectType? refObjectType24 = NavigationCommandRefObjectType.Document;
		short refObjectId24 = DocumentTypeSE.Revaluation;
		num = 1;
		List<short> list26 = new List<short>(num);
		CollectionsMarshal.SetCount(list26, num);
		CollectionsMarshal.AsSpan(list26)[0] = 4;
		Revaluation = new UserAccessIdSE("Revaluation", 145, refObjectType24, refObjectId24, list26);
		NavigationCommandRefObjectType? refObjectType25 = NavigationCommandRefObjectType.Document;
		short refObjectId25 = DocumentTypeSE.PartnerRemCorrect;
		num = 2;
		List<short> list27 = new List<short>(num);
		CollectionsMarshal.SetCount(list27, num);
		Span<short> span12 = CollectionsMarshal.AsSpan(list27);
		span12[0] = 1;
		span12[1] = 2;
		PartRemCorrect = new UserAccessIdSE("PartRemCorrect", 146, refObjectType25, refObjectId25, list27);
		NavigationCommandRefObjectType? refObjectType26 = NavigationCommandRefObjectType.Document;
		short refObjectId26 = DocumentTypeSE.PartnerRemGroupCorrect;
		num = 2;
		List<short> list28 = new List<short>(num);
		CollectionsMarshal.SetCount(list28, num);
		Span<short> span13 = CollectionsMarshal.AsSpan(list28);
		span13[0] = 1;
		span13[1] = 2;
		PartRemGroupCorrect = new UserAccessIdSE("PartRemGroupCorrect", 340, refObjectType26, refObjectId26, list28);
		NavigationCommandRefObjectType? refObjectType27 = NavigationCommandRefObjectType.Document;
		short refObjectId27 = DocumentTypeSE.PartnerRemMove;
		num = 2;
		List<short> list29 = new List<short>(num);
		CollectionsMarshal.SetCount(list29, num);
		Span<short> span14 = CollectionsMarshal.AsSpan(list29);
		span14[0] = 1;
		span14[1] = 2;
		PartRemMove = new UserAccessIdSE("PartRemMove", 339, refObjectType27, refObjectId27, list29);
		NavigationCommandRefObjectType? refObjectType28 = NavigationCommandRefObjectType.Document;
		short refObjectId28 = DocumentTypeSE.BonusGroupCorrect;
		num = 2;
		List<short> list30 = new List<short>(num);
		CollectionsMarshal.SetCount(list30, num);
		Span<short> span15 = CollectionsMarshal.AsSpan(list30);
		span15[0] = 1;
		span15[1] = 2;
		BonusGroupCorrect = new UserAccessIdSE("BonusGroupCorrect", 344, refObjectType28, refObjectId28, list30);
		NavigationCommandRefObjectType? refObjectType29 = NavigationCommandRefObjectType.Document;
		short refObjectId29 = DocumentTypeSE.MTIncomeRem;
		num = 2;
		List<short> list31 = new List<short>(num);
		CollectionsMarshal.SetCount(list31, num);
		Span<short> span16 = CollectionsMarshal.AsSpan(list31);
		span16[0] = 1;
		span16[1] = 3;
		MTIncomeRem = new UserAccessIdSE("MTIncomeRem", 165, refObjectType29, refObjectId29, list31);
		NavigationCommandRefObjectType? refObjectType30 = NavigationCommandRefObjectType.Document;
		short refObjectId30 = DocumentTypeSE.PartnersGroupRevaluation;
		num = 2;
		List<short> list32 = new List<short>(num);
		CollectionsMarshal.SetCount(list32, num);
		Span<short> span17 = CollectionsMarshal.AsSpan(list32);
		span17[0] = 1;
		span17[1] = 2;
		PartnersGroupRevaluation = new UserAccessIdSE("PartnersGroupRevaluation", 331, refObjectType30, refObjectId30, list32);
		NavigationCommandRefObjectType? refObjectType31 = NavigationCommandRefObjectType.Document;
		short refObjectId31 = DocumentTypeSE.ReceivedService;
		num = 1;
		List<short> list33 = new List<short>(num);
		CollectionsMarshal.SetCount(list33, num);
		CollectionsMarshal.AsSpan(list33)[0] = 1;
		BoughtServices = new UserAccessIdSE("BoughtServices", 341, refObjectType31, refObjectId31, list33);
		NavigationCommandRefObjectType? refObjectType32 = NavigationCommandRefObjectType.Document;
		short refObjectId32 = DocumentTypeSE.MTPurchaseOrder;
		num = 1;
		List<short> list34 = new List<short>(num);
		CollectionsMarshal.SetCount(list34, num);
		CollectionsMarshal.AsSpan(list34)[0] = 1;
		MTPurchaseOrder = new UserAccessIdSE("MTPurchaseOrder", 351, refObjectType32, refObjectId32, list34);
		NavigationCommandRefObjectType? refObjectType33 = NavigationCommandRefObjectType.Document;
		short refObjectId33 = DocumentTypeSE.InvoiceForPay;
		num = 1;
		List<short> list35 = new List<short>(num);
		CollectionsMarshal.SetCount(list35, num);
		CollectionsMarshal.AsSpan(list35)[0] = 2;
		InvoiceForPay = new UserAccessIdSE("InvoiceForPay", 411, refObjectType33, refObjectId33, list35);
		NavigationCommandRefObjectType? refObjectType34 = NavigationCommandRefObjectType.Document;
		short refObjectId34 = DocumentTypeSE.MTRevaluation;
		num = 1;
		List<short> list36 = new List<short>(num);
		CollectionsMarshal.SetCount(list36, num);
		CollectionsMarshal.AsSpan(list36)[0] = 3;
		MTRevaluation = new UserAccessIdSE("MTRevaluation", 430, refObjectType34, refObjectId34, list36);
		NavigationCommandRefObjectType? refObjectType35 = NavigationCommandRefObjectType.Document;
		short refObjectId35 = DocumentTypeSE.Reconciliation;
		num = 2;
		List<short> list37 = new List<short>(num);
		CollectionsMarshal.SetCount(list37, num);
		Span<short> span18 = CollectionsMarshal.AsSpan(list37);
		span18[0] = 1;
		span18[1] = 3;
		ReconciliationStatementDoc = new UserAccessIdSE("ReconciliationStatementDoc", 419, refObjectType35, refObjectId35, list37);
		NavigationCommandRefObjectType? refObjectType36 = NavigationCommandRefObjectType.Document;
		short refObjectId36 = DocumentTypeSE.MTMoveOrder;
		num = 1;
		List<short> list38 = new List<short>(num);
		CollectionsMarshal.SetCount(list38, num);
		CollectionsMarshal.AsSpan(list38)[0] = 3;
		MTMoveOrder = new UserAccessIdSE("MTMoveOrder", 427, refObjectType36, refObjectId36, list38);
		NavigationCommandRefObjectType? refObjectType37 = NavigationCommandRefObjectType.Document;
		short refObjectId37 = DocumentTypeSE.EMarkReceipt;
		num = 2;
		List<short> list39 = new List<short>(num);
		CollectionsMarshal.SetCount(list39, num);
		Span<short> span19 = CollectionsMarshal.AsSpan(list39);
		span19[0] = 2;
		span19[1] = 3;
		EMarkReceiptDoc = new UserAccessIdSE("EMarkReceiptDoc", 428, refObjectType37, refObjectId37, list39);
		NavigationCommandRefObjectType? refObjectType38 = NavigationCommandRefObjectType.Document;
		short refObjectId38 = DocumentTypeSE.MTMoveWithConfirmation;
		num = 1;
		List<short> list40 = new List<short>(num);
		CollectionsMarshal.SetCount(list40, num);
		CollectionsMarshal.AsSpan(list40)[0] = 3;
		MTMoveWithConfirmation = new UserAccessIdSE("MTMoveWithConfirmation", 429, refObjectType38, refObjectId38, list40);
		num = 2;
		List<short> list41 = new List<short>(num);
		CollectionsMarshal.SetCount(list41, num);
		Span<short> span20 = CollectionsMarshal.AsSpan(list41);
		span20[0] = 2;
		span20[1] = 8;
		subsystemIds = list41;
		AssignTaxInvNums = new UserAccessIdSE("AssignTaxInvNums", 335, null, 0, subsystemIds);
		num = 2;
		List<short> list42 = new List<short>(num);
		CollectionsMarshal.SetCount(list42, num);
		Span<short> span21 = CollectionsMarshal.AsSpan(list42);
		span21[0] = 3;
		span21[1] = 8;
		subsystemIds = list42;
		AssignAccDocInvNums = new UserAccessIdSE("AssignAccDocInvNums", 333, null, 0, subsystemIds);
		num = 3;
		List<short> list43 = new List<short>(num);
		CollectionsMarshal.SetCount(list43, num);
		Span<short> span22 = CollectionsMarshal.AsSpan(list43);
		span22[0] = 1;
		span22[1] = 3;
		span22[2] = 8;
		subsystemIds = list43;
		ImportTaxInvoice = new UserAccessIdSE("ImportTaxInvoice", 337, null, 0, subsystemIds);
		num = 1;
		List<short> list44 = new List<short>(num);
		CollectionsMarshal.SetCount(list44, num);
		CollectionsMarshal.AsSpan(list44)[0] = 1;
		subsystemIds = list44;
		ImportPurchaseOrders = new UserAccessIdSE("ImportPurchaseOrders", 352, null, 0, subsystemIds);
		num = 1;
		List<short> list45 = new List<short>(num);
		CollectionsMarshal.SetCount(list45, num);
		CollectionsMarshal.AsSpan(list45)[0] = 2;
		subsystemIds = list45;
		PrintECRReport = new UserAccessIdSE("PrintECRReport", 359, null, 0, subsystemIds);
		num = 1;
		List<short> list46 = new List<short>(num);
		CollectionsMarshal.SetCount(list46, num);
		CollectionsMarshal.AsSpan(list46)[0] = 7;
		subsystemIds = list46;
		PrintECRReportPOS = new UserAccessIdSE("PrintECRReportPOS", 400, null, 0, subsystemIds);
		num = 1;
		List<short> list47 = new List<short>(num);
		CollectionsMarshal.SetCount(list47, num);
		CollectionsMarshal.AsSpan(list47)[0] = 2;
		subsystemIds = list47;
		OpenCashierShift = new UserAccessIdSE("OpenCashierShift", 404, null, 0, subsystemIds);
		num = 1;
		List<short> list48 = new List<short>(num);
		CollectionsMarshal.SetCount(list48, num);
		CollectionsMarshal.AsSpan(list48)[0] = 2;
		subsystemIds = list48;
		SyncECR = new UserAccessIdSE("SyncECR", 405, null, 0, subsystemIds);
		num = 1;
		List<short> list49 = new List<short>(num);
		CollectionsMarshal.SetCount(list49, num);
		CollectionsMarshal.AsSpan(list49)[0] = 7;
		subsystemIds = list49;
		SyncECRPOS = new UserAccessIdSE("SyncECRPOS", 406, null, 0, subsystemIds);
		num = 2;
		List<short> list50 = new List<short>(num);
		CollectionsMarshal.SetCount(list50, num);
		Span<short> span23 = CollectionsMarshal.AsSpan(list50);
		span23[0] = 2;
		span23[1] = 7;
		subsystemIds = list50;
		CloseCashierShift = new UserAccessIdSE("CloseCashierShift", 401, null, 0, subsystemIds);
		num = 1;
		List<short> list51 = new List<short>(num);
		CollectionsMarshal.SetCount(list51, num);
		CollectionsMarshal.AsSpan(list51)[0] = 2;
		subsystemIds = list51;
		ExportForElScalesAndTerminals = new UserAccessIdSE("ExportForElScalesAndTerminals", 409, null, 0, subsystemIds);
		num = 1;
		List<short> list52 = new List<short>(num);
		CollectionsMarshal.SetCount(list52, num);
		CollectionsMarshal.AsSpan(list52)[0] = 7;
		subsystemIds = list52;
		ECRConfiguration = new UserAccessIdSE("ECRConfiguration", 410, null, 0, subsystemIds);
		NavigationCommandRefObjectType? refObjectType39 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId39 = DataProviderTypeSE.CashierShiftReport;
		num = 2;
		List<short> list53 = new List<short>(num);
		CollectionsMarshal.SetCount(list53, num);
		Span<short> span24 = CollectionsMarshal.AsSpan(list53);
		span24[0] = 2;
		span24[1] = 7;
		CashierShiftReport = new UserAccessIdSE("CashierShiftReport", 403, refObjectType39, refObjectId39, list53);
		NavigationCommandRefObjectType? refObjectType40 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId40 = DataProviderTypeSE.PurchaseDocumentsJournal;
		num = 2;
		List<short> list54 = new List<short>(num);
		CollectionsMarshal.SetCount(list54, num);
		Span<short> span25 = CollectionsMarshal.AsSpan(list54);
		span25[0] = 1;
		span25[1] = 8;
		PurchaseDocumentsJournal = new UserAccessIdSE("PurchaseDocumentsJournal", 320, refObjectType40, refObjectId40, list54);
		NavigationCommandRefObjectType? refObjectType41 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId41 = DataProviderTypeSE.SaleDocumentsJournal;
		num = 1;
		List<short> list55 = new List<short>(num);
		CollectionsMarshal.SetCount(list55, num);
		CollectionsMarshal.AsSpan(list55)[0] = 2;
		SaleDocumentsJournal = new UserAccessIdSE("SaleDocumentsJournal", 321, refObjectType41, refObjectId41, list55);
		NavigationCommandRefObjectType? refObjectType42 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId42 = DataProviderTypeSE.TaxInvoicesDocumentsJournal;
		num = 2;
		List<short> list56 = new List<short>(num);
		CollectionsMarshal.SetCount(list56, num);
		Span<short> span26 = CollectionsMarshal.AsSpan(list56);
		span26[0] = 2;
		span26[1] = 8;
		TaxInvoicesJournal = new UserAccessIdSE("TaxInvoicesJournal", 167, refObjectType42, refObjectId42, list56);
		NavigationCommandRefObjectType? refObjectType43 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId43 = DataProviderTypeSE.SentInvoicesForPaymentDocumentsJournal;
		num = 1;
		List<short> list57 = new List<short>(num);
		CollectionsMarshal.SetCount(list57, num);
		CollectionsMarshal.AsSpan(list57)[0] = 2;
		WrittenOffInvoicesForPay = new UserAccessIdSE("WrittenOffInvoicesForPay", 360, refObjectType43, refObjectId43, list57);
		NavigationCommandRefObjectType? refObjectType44 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId44 = DataProviderTypeSE.ReconcilationStatementsDocumentsJournal;
		num = 2;
		List<short> list58 = new List<short>(num);
		CollectionsMarshal.SetCount(list58, num);
		Span<short> span27 = CollectionsMarshal.AsSpan(list58);
		span27[0] = 1;
		span27[1] = 2;
		ReconciliationStatementsJournal = new UserAccessIdSE("ReconciliationStatementsJournal", 426, refObjectType44, refObjectId44, list58);
		NavigationCommandRefObjectType? refObjectType45 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId45 = DataProviderTypeSE.ReceivedTaxInvoicesDocumentsJournal;
		num = 2;
		List<short> list59 = new List<short>(num);
		CollectionsMarshal.SetCount(list59, num);
		Span<short> span28 = CollectionsMarshal.AsSpan(list59);
		span28[0] = 1;
		span28[1] = 8;
		ReceivedInvoicesJournal = new UserAccessIdSE("ReceivedInvoicesJournal", 336, refObjectType45, refObjectId45, list59);
		NavigationCommandRefObjectType? refObjectType46 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId46 = DataProviderTypeSE.WarehouseDocumentsJournal;
		num = 1;
		List<short> list60 = new List<short>(num);
		CollectionsMarshal.SetCount(list60, num);
		CollectionsMarshal.AsSpan(list60)[0] = 1;
		WarehouseDocumentsJournal = new UserAccessIdSE("WarehouseDocumentsJournal", 322, refObjectType46, refObjectId46, list60);
		NavigationCommandRefObjectType? refObjectType47 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId47 = DataProviderTypeSE.CashDocumentsJournal;
		num = 1;
		List<short> list61 = new List<short>(num);
		CollectionsMarshal.SetCount(list61, num);
		CollectionsMarshal.AsSpan(list61)[0] = 4;
		CashDocumentsJournal = new UserAccessIdSE("CashDocumentsJournal", 318, refObjectType47, refObjectId47, list61);
		NavigationCommandRefObjectType? refObjectType48 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId48 = DataProviderTypeSE.BankDocumentsJournal;
		num = 1;
		List<short> list62 = new List<short>(num);
		CollectionsMarshal.SetCount(list62, num);
		CollectionsMarshal.AsSpan(list62)[0] = 4;
		BankDocumentsJournal = new UserAccessIdSE("BankDocumentsJournal", 319, refObjectType48, refObjectId48, list62);
		NavigationCommandRefObjectType? refObjectType49 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId49 = DataProviderTypeSE.TransportationDocumentsJournal;
		num = 2;
		List<short> list63 = new List<short>(num);
		CollectionsMarshal.SetCount(list63, num);
		Span<short> span29 = CollectionsMarshal.AsSpan(list63);
		span29[0] = 3;
		span29[1] = 8;
		TransportationDocumentsJournal = new UserAccessIdSE("TransportationDocumentsJournal", 332, refObjectType49, refObjectId49, list63);
		NavigationCommandRefObjectType? refObjectType50 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId50 = DataProviderTypeSE.ECRCheckDocumentsJournal;
		num = 1;
		List<short> list64 = new List<short>(num);
		CollectionsMarshal.SetCount(list64, num);
		CollectionsMarshal.AsSpan(list64)[0] = 2;
		ECRCheckDocumentsJournal = new UserAccessIdSE("ECRCheckDocumentsJournal", 334, refObjectType50, refObjectId50, list64);
		NavigationCommandRefObjectType? refObjectType51 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId51 = DataProviderTypeSE.LocalDBDocuments;
		num = 1;
		List<short> list65 = new List<short>(num);
		CollectionsMarshal.SetCount(list65, num);
		CollectionsMarshal.AsSpan(list65)[0] = 2;
		LocalDBDocumnets = new UserAccessIdSE("LocalDBDocumnets", 519, refObjectType51, refObjectId51, list65);
		NavigationCommandRefObjectType? refObjectType52 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId52 = DataProviderTypeSE.ECRReport;
		num = 1;
		List<short> list66 = new List<short>(num);
		CollectionsMarshal.SetCount(list66, num);
		CollectionsMarshal.AsSpan(list66)[0] = 2;
		ECRReport = new UserAccessIdSE("ECRReport", 338, refObjectType52, refObjectId52, list66);
		NavigationCommandRefObjectType? refObjectType53 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId53 = DataProviderTypeSE.ECRCheckDocumentsListJournal;
		num = 1;
		List<short> list67 = new List<short>(num);
		CollectionsMarshal.SetCount(list67, num);
		CollectionsMarshal.AsSpan(list67)[0] = 2;
		ECRCheckDocumentsListJournal = new UserAccessIdSE("ECRCheckDocumentsListJournal", 356, refObjectType53, refObjectId53, list67);
		NavigationCommandRefObjectType? refObjectType54 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId54 = DataProviderTypeSE.EMarkReceipListJournal;
		num = 2;
		List<short> list68 = new List<short>(num);
		CollectionsMarshal.SetCount(list68, num);
		Span<short> span30 = CollectionsMarshal.AsSpan(list68);
		span30[0] = 2;
		span30[1] = 3;
		EMarkReceipListJournal = new UserAccessIdSE("EMarkReceipListJournal", 458, refObjectType54, refObjectId54, list68);
		NavigationCommandRefObjectType? refObjectType55 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId55 = DataProviderTypeSE.GoodsStockTakingRep;
		num = 1;
		List<short> list69 = new List<short>(num);
		CollectionsMarshal.SetCount(list69, num);
		CollectionsMarshal.AsSpan(list69)[0] = 3;
		GoodsStockTakingRep = new UserAccessIdSE("GoodsStockTakingRep", 510, refObjectType55, refObjectId55, list69);
		NavigationCommandRefObjectType? refObjectType56 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId56 = DataProviderTypeSE.MatRepAvailabilityTreeView;
		num = 3;
		List<short> list70 = new List<short>(num);
		CollectionsMarshal.SetCount(list70, num);
		Span<short> span31 = CollectionsMarshal.AsSpan(list70);
		span31[0] = 1;
		span31[1] = 2;
		span31[2] = 3;
		MatRepAvailabilityTreeView = new UserAccessIdSE("MatRepAvailabilityTreeView", 100, refObjectType56, refObjectId56, list70);
		NavigationCommandRefObjectType? refObjectType57 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId57 = DataProviderTypeSE.MatRepAvailability;
		num = 3;
		List<short> list71 = new List<short>(num);
		CollectionsMarshal.SetCount(list71, num);
		Span<short> span32 = CollectionsMarshal.AsSpan(list71);
		span32[0] = 1;
		span32[1] = 2;
		span32[2] = 3;
		MatRepAvailability = new UserAccessIdSE("MatRepAvailability", 101, refObjectType57, refObjectId57, list71);
		NavigationCommandRefObjectType? refObjectType58 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId58 = DataProviderTypeSE.GoodsTurnOver;
		num = 3;
		List<short> list72 = new List<short>(num);
		CollectionsMarshal.SetCount(list72, num);
		Span<short> span33 = CollectionsMarshal.AsSpan(list72);
		span33[0] = 1;
		span33[1] = 2;
		span33[2] = 3;
		MatRepTurnOver = new UserAccessIdSE("MatRepTurnOver", 102, refObjectType58, refObjectId58, list72);
		NavigationCommandRefObjectType? refObjectType59 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId59 = DataProviderTypeSE.GoodsRepOperations;
		num = 3;
		List<short> list73 = new List<short>(num);
		CollectionsMarshal.SetCount(list73, num);
		Span<short> span34 = CollectionsMarshal.AsSpan(list73);
		span34[0] = 1;
		span34[1] = 2;
		span34[2] = 3;
		MatRepOperationsJournal = new UserAccessIdSE("MatRepOperationsJournal", 103, refObjectType59, refObjectId59, list73);
		NavigationCommandRefObjectType? refObjectType60 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId60 = DataProviderTypeSE.MatRepTAccount;
		num = 3;
		List<short> list74 = new List<short>(num);
		CollectionsMarshal.SetCount(list74, num);
		Span<short> span35 = CollectionsMarshal.AsSpan(list74);
		span35[0] = 1;
		span35[1] = 2;
		span35[2] = 3;
		MatRepTAccount = new UserAccessIdSE("MatRepTAccount", 104, refObjectType60, refObjectId60, list74);
		NavigationCommandRefObjectType? refObjectType61 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId61 = DataProviderTypeSE.MTPriceList;
		num = 2;
		List<short> list75 = new List<short>(num);
		CollectionsMarshal.SetCount(list75, num);
		Span<short> span36 = CollectionsMarshal.AsSpan(list75);
		span36[0] = 1;
		span36[1] = 2;
		MTPriceList = new UserAccessIdSE("MTPriceList", 166, refObjectType61, refObjectId61, list75);
		NavigationCommandRefObjectType? refObjectType62 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId62 = DataProviderTypeSE.PartRemains;
		num = 3;
		List<short> list76 = new List<short>(num);
		CollectionsMarshal.SetCount(list76, num);
		Span<short> span37 = CollectionsMarshal.AsSpan(list76);
		span37[0] = 1;
		span37[1] = 2;
		span37[2] = 4;
		PartRemains = new UserAccessIdSE("PartRemains", 500, refObjectType62, refObjectId62, list76);
		NavigationCommandRefObjectType? refObjectType63 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId63 = DataProviderTypeSE.PartnersTurnOver;
		num = 3;
		List<short> list77 = new List<short>(num);
		CollectionsMarshal.SetCount(list77, num);
		Span<short> span38 = CollectionsMarshal.AsSpan(list77);
		span38[0] = 1;
		span38[1] = 2;
		span38[2] = 4;
		PartTurnOver = new UserAccessIdSE("PartTurnOver", 106, refObjectType63, refObjectId63, list77);
		NavigationCommandRefObjectType? refObjectType64 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId64 = DataProviderTypeSE.PartOpJournal;
		num = 3;
		List<short> list78 = new List<short>(num);
		CollectionsMarshal.SetCount(list78, num);
		Span<short> span39 = CollectionsMarshal.AsSpan(list78);
		span39[0] = 1;
		span39[1] = 2;
		span39[2] = 4;
		PartOpJournal = new UserAccessIdSE("PartOpJournal", 107, refObjectType64, refObjectId64, list78);
		NavigationCommandRefObjectType? refObjectType65 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId65 = DataProviderTypeSE.PartRepCard;
		num = 3;
		List<short> list79 = new List<short>(num);
		CollectionsMarshal.SetCount(list79, num);
		Span<short> span40 = CollectionsMarshal.AsSpan(list79);
		span40[0] = 1;
		span40[1] = 2;
		span40[2] = 4;
		PartRepCard = new UserAccessIdSE("PartRepCard", 154, refObjectType65, refObjectId65, list79);
		NavigationCommandRefObjectType? refObjectType66 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId66 = DataProviderTypeSE.BonusRemains;
		num = 2;
		List<short> list80 = new List<short>(num);
		CollectionsMarshal.SetCount(list80, num);
		Span<short> span41 = CollectionsMarshal.AsSpan(list80);
		span41[0] = 1;
		span41[1] = 2;
		BonusRemains = new UserAccessIdSE("BonusRemains", 346, refObjectType66, refObjectId66, list80);
		NavigationCommandRefObjectType? refObjectType67 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId67 = DataProviderTypeSE.BonusTurnOver;
		num = 2;
		List<short> list81 = new List<short>(num);
		CollectionsMarshal.SetCount(list81, num);
		Span<short> span42 = CollectionsMarshal.AsSpan(list81);
		span42[0] = 1;
		span42[1] = 2;
		BonusTurnOver = new UserAccessIdSE("BonusTurnOver", 347, refObjectType67, refObjectId67, list81);
		NavigationCommandRefObjectType? refObjectType68 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId68 = DataProviderTypeSE.BonusOpJournal;
		num = 2;
		List<short> list82 = new List<short>(num);
		CollectionsMarshal.SetCount(list82, num);
		Span<short> span43 = CollectionsMarshal.AsSpan(list82);
		span43[0] = 1;
		span43[1] = 2;
		BonusOpJournal = new UserAccessIdSE("BonusOpJournal", 348, refObjectType68, refObjectId68, list82);
		NavigationCommandRefObjectType? refObjectType69 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId69 = DataProviderTypeSE.BonusTAccount;
		num = 2;
		List<short> list83 = new List<short>(num);
		CollectionsMarshal.SetCount(list83, num);
		Span<short> span44 = CollectionsMarshal.AsSpan(list83);
		span44[0] = 1;
		span44[1] = 2;
		BonusTAccount = new UserAccessIdSE("BonusTAccount", 349, refObjectType69, refObjectId69, list83);
		NavigationCommandRefObjectType? refObjectType70 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId70 = DataProviderTypeSE.CashRemains;
		num = 3;
		List<short> list84 = new List<short>(num);
		CollectionsMarshal.SetCount(list84, num);
		Span<short> span45 = CollectionsMarshal.AsSpan(list84);
		span45[0] = 1;
		span45[1] = 2;
		span45[2] = 4;
		CashRemains = new UserAccessIdSE("CashRemains", 501, refObjectType70, refObjectId70, list84);
		NavigationCommandRefObjectType? refObjectType71 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId71 = DataProviderTypeSE.CashTurnOver;
		num = 3;
		List<short> list85 = new List<short>(num);
		CollectionsMarshal.SetCount(list85, num);
		Span<short> span46 = CollectionsMarshal.AsSpan(list85);
		span46[0] = 1;
		span46[1] = 2;
		span46[2] = 4;
		CashTurnOver = new UserAccessIdSE("CashTurnOver", 108, refObjectType71, refObjectId71, list85);
		NavigationCommandRefObjectType? refObjectType72 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId72 = DataProviderTypeSE.CashOperations;
		num = 3;
		List<short> list86 = new List<short>(num);
		CollectionsMarshal.SetCount(list86, num);
		Span<short> span47 = CollectionsMarshal.AsSpan(list86);
		span47[0] = 1;
		span47[1] = 2;
		span47[2] = 4;
		CashOpJournal = new UserAccessIdSE("CashOpJournal", 109, refObjectType72, refObjectId72, list86);
		NavigationCommandRefObjectType? refObjectType73 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId73 = DataProviderTypeSE.CashDeskTAccount;
		num = 3;
		List<short> list87 = new List<short>(num);
		CollectionsMarshal.SetCount(list87, num);
		Span<short> span48 = CollectionsMarshal.AsSpan(list87);
		span48[0] = 1;
		span48[1] = 2;
		span48[2] = 4;
		CashDeskTAccount = new UserAccessIdSE("CashDeskTAccount", 503, refObjectType73, refObjectId73, list87);
		NavigationCommandRefObjectType? refObjectType74 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId74 = DataProviderTypeSE.BankRemains;
		num = 3;
		List<short> list88 = new List<short>(num);
		CollectionsMarshal.SetCount(list88, num);
		Span<short> span49 = CollectionsMarshal.AsSpan(list88);
		span49[0] = 1;
		span49[1] = 2;
		span49[2] = 4;
		BankRemains = new UserAccessIdSE("BankRemains", 502, refObjectType74, refObjectId74, list88);
		NavigationCommandRefObjectType? refObjectType75 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId75 = DataProviderTypeSE.BankTurnOver;
		num = 3;
		List<short> list89 = new List<short>(num);
		CollectionsMarshal.SetCount(list89, num);
		Span<short> span50 = CollectionsMarshal.AsSpan(list89);
		span50[0] = 1;
		span50[1] = 2;
		span50[2] = 4;
		BankTurnOver = new UserAccessIdSE("BankTurnOver", 110, refObjectType75, refObjectId75, list89);
		NavigationCommandRefObjectType? refObjectType76 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId76 = DataProviderTypeSE.BankOperations;
		num = 3;
		List<short> list90 = new List<short>(num);
		CollectionsMarshal.SetCount(list90, num);
		Span<short> span51 = CollectionsMarshal.AsSpan(list90);
		span51[0] = 1;
		span51[1] = 2;
		span51[2] = 4;
		BankOpJournal = new UserAccessIdSE("BankOpJournal", 111, refObjectType76, refObjectId76, list90);
		NavigationCommandRefObjectType? refObjectType77 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId77 = DataProviderTypeSE.BankTAccount;
		num = 3;
		List<short> list91 = new List<short>(num);
		CollectionsMarshal.SetCount(list91, num);
		Span<short> span52 = CollectionsMarshal.AsSpan(list91);
		span52[0] = 1;
		span52[1] = 2;
		span52[2] = 4;
		BankTAccount = new UserAccessIdSE("BankTAccount", 504, refObjectType77, refObjectId77, list91);
		NavigationCommandRefObjectType? refObjectType78 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId78 = DataProviderTypeSE.MatRepSalesAnalysis;
		num = 1;
		List<short> list92 = new List<short>(num);
		CollectionsMarshal.SetCount(list92, num);
		CollectionsMarshal.AsSpan(list92)[0] = 2;
		MatRepSalesAnalysis = new UserAccessIdSE("MatRepSalesAnalysis", 105, refObjectType78, refObjectId78, list92);
		NavigationCommandRefObjectType? refObjectType79 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId79 = DataProviderTypeSE.MatRepSalesAverage;
		num = 1;
		List<short> list93 = new List<short>(num);
		CollectionsMarshal.SetCount(list93, num);
		CollectionsMarshal.AsSpan(list93)[0] = 2;
		MatRepSalesAverage = new UserAccessIdSE("MatRepSalesAverage", 408, refObjectType79, refObjectId79, list93);
		NavigationCommandRefObjectType? refObjectType80 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId80 = DataProviderTypeSE.MatRepAvailabilityRetail;
		num = 2;
		List<short> list94 = new List<short>(num);
		CollectionsMarshal.SetCount(list94, num);
		Span<short> span53 = CollectionsMarshal.AsSpan(list94);
		span53[0] = 2;
		span53[1] = 3;
		MatRepAvailabilityRetail = new UserAccessIdSE("MatRepAvailabilityRetail", 162, refObjectType80, refObjectId80, list94);
		NavigationCommandRefObjectType? refObjectType81 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId81 = DataProviderTypeSE.MatRepTurnOverRetail;
		num = 2;
		List<short> list95 = new List<short>(num);
		CollectionsMarshal.SetCount(list95, num);
		Span<short> span54 = CollectionsMarshal.AsSpan(list95);
		span54[0] = 2;
		span54[1] = 3;
		MatRepTurnOverRetail = new UserAccessIdSE("MatRepTurnOverRetail", 163, refObjectType81, refObjectId81, list95);
		NavigationCommandRefObjectType? refObjectType82 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId82 = DataProviderTypeSE.MatRepOperationsRetail;
		num = 2;
		List<short> list96 = new List<short>(num);
		CollectionsMarshal.SetCount(list96, num);
		Span<short> span55 = CollectionsMarshal.AsSpan(list96);
		span55[0] = 2;
		span55[1] = 3;
		MatRepOperationsRetail = new UserAccessIdSE("MatRepOperationsRetail", 164, refObjectType82, refObjectId82, list96);
		NavigationCommandRefObjectType? refObjectType83 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId83 = DataProviderTypeSE.GoodsCostCalculation;
		num = 1;
		List<short> list97 = new List<short>(num);
		CollectionsMarshal.SetCount(list97, num);
		CollectionsMarshal.AsSpan(list97)[0] = 1;
		GoodsCostCalculation = new UserAccessIdSE("GoodsCostCalculation", 508, refObjectType83, refObjectId83, list97);
		NavigationCommandRefObjectType? refObjectType84 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId84 = DataProviderTypeSE.ReceivedServicesJournal;
		num = 1;
		List<short> list98 = new List<short>(num);
		CollectionsMarshal.SetCount(list98, num);
		CollectionsMarshal.AsSpan(list98)[0] = 1;
		ReceivedServicesJournal = new UserAccessIdSE("ReceivedServicesJournal", 342, refObjectType84, refObjectId84, list98);
		NavigationCommandRefObjectType? refObjectType85 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId85 = DataProviderTypeSE.DiscountLogsJournal;
		num = 1;
		List<short> list99 = new List<short>(num);
		CollectionsMarshal.SetCount(list99, num);
		CollectionsMarshal.AsSpan(list99)[0] = 2;
		DiscountLogsJournal = new UserAccessIdSE("DiscountLogsJournal", 450, refObjectType85, refObjectId85, list99);
		NavigationCommandRefObjectType? refObjectType86 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId86 = DataProviderTypeSE.Goods;
		num = 3;
		List<short> list100 = new List<short>(num);
		CollectionsMarshal.SetCount(list100, num);
		Span<short> span56 = CollectionsMarshal.AsSpan(list100);
		span56[0] = 1;
		span56[1] = 2;
		span56[2] = 3;
		Goods = new UserAccessIdSE("Goods", 112, refObjectType86, refObjectId86, list100);
		NavigationCommandRefObjectType? refObjectType87 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId87 = DataProviderTypeSE.b2bUnitedProducts;
		num = 3;
		List<short> list101 = new List<short>(num);
		CollectionsMarshal.SetCount(list101, num);
		Span<short> span57 = CollectionsMarshal.AsSpan(list101);
		span57[0] = 1;
		span57[1] = 2;
		span57[2] = 3;
		ImportFromUnitedProducts = new UserAccessIdSE("ImportFromUnitedProducts", 446, refObjectType87, refObjectId87, list101);
		NavigationCommandRefObjectType? refObjectType88 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId88 = DataProviderTypeSE.B2BProductsCheck;
		num = 3;
		List<short> list102 = new List<short>(num);
		CollectionsMarshal.SetCount(list102, num);
		Span<short> span58 = CollectionsMarshal.AsSpan(list102);
		span58[0] = 1;
		span58[1] = 2;
		span58[2] = 3;
		B2BProductsCheck = new UserAccessIdSE("B2BProductsCheck", 479, refObjectType88, refObjectId88, list102);
		NavigationCommandRefObjectType? refObjectType89 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId89 = DataProviderTypeSE.MTGroups;
		num = 3;
		List<short> list103 = new List<short>(num);
		CollectionsMarshal.SetCount(list103, num);
		Span<short> span59 = CollectionsMarshal.AsSpan(list103);
		span59[0] = 1;
		span59[1] = 2;
		span59[2] = 3;
		MTGroups = new UserAccessIdSE("MTGroups", 113, refObjectType89, refObjectId89, list103);
		NavigationCommandRefObjectType? refObjectType90 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId90 = DataProviderTypeSE.SubstituteItemsGroups;
		num = 3;
		List<short> list104 = new List<short>(num);
		CollectionsMarshal.SetCount(list104, num);
		Span<short> span60 = CollectionsMarshal.AsSpan(list104);
		span60[0] = 1;
		span60[1] = 2;
		span60[2] = 3;
		SubstituteItemsGroups = new UserAccessIdSE("SubstituteItemsGroups", 170, refObjectType90, refObjectId90, list104);
		NavigationCommandRefObjectType? refObjectType91 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId91 = DataProviderTypeSE.PartnersMT;
		num = 3;
		List<short> list105 = new List<short>(num);
		CollectionsMarshal.SetCount(list105, num);
		Span<short> span61 = CollectionsMarshal.AsSpan(list105);
		span61[0] = 1;
		span61[1] = 2;
		span61[2] = 3;
		PartnersMT = new UserAccessIdSE("PartnersMT", 507, refObjectType91, refObjectId91, list105);
		NavigationCommandRefObjectType? refObjectType92 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId92 = DataProviderTypeSE.Storages;
		num = 2;
		List<short> list106 = new List<short>(num);
		CollectionsMarshal.SetCount(list106, num);
		Span<short> span62 = CollectionsMarshal.AsSpan(list106);
		span62[0] = 1;
		span62[1] = 3;
		Storages = new UserAccessIdSE("Storages", 114, refObjectType92, refObjectId92, list106);
		NavigationCommandRefObjectType? refObjectType93 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId93 = DataProviderTypeSE.Partners;
		num = 3;
		List<short> list107 = new List<short>(num);
		CollectionsMarshal.SetCount(list107, num);
		Span<short> span63 = CollectionsMarshal.AsSpan(list107);
		span63[0] = 1;
		span63[1] = 2;
		span63[2] = 4;
		Partners = new UserAccessIdSE("Partners", 117, refObjectType93, refObjectId93, list107);
		NavigationCommandRefObjectType? refObjectType94 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId94 = DataProviderTypeSE.PartGroups;
		num = 3;
		List<short> list108 = new List<short>(num);
		CollectionsMarshal.SetCount(list108, num);
		Span<short> span64 = CollectionsMarshal.AsSpan(list108);
		span64[0] = 1;
		span64[1] = 2;
		span64[2] = 4;
		PartGroups = new UserAccessIdSE("PartGroups", 118, refObjectType94, refObjectId94, list108);
		NavigationCommandRefObjectType? refObjectType95 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId95 = DataProviderTypeSE.Segments;
		num = 1;
		List<short> list109 = new List<short>(num);
		CollectionsMarshal.SetCount(list109, num);
		CollectionsMarshal.AsSpan(list109)[0] = 2;
		Segments = new UserAccessIdSE("Segments", 511, refObjectType95, refObjectId95, list109);
		NavigationCommandRefObjectType? refObjectType96 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId96 = DataProviderTypeSE.GoodsBySegment;
		num = 1;
		List<short> list110 = new List<short>(num);
		CollectionsMarshal.SetCount(list110, num);
		CollectionsMarshal.AsSpan(list110)[0] = 2;
		GoodsBySegment = new UserAccessIdSE("GoodsBySegment", 512, refObjectType96, refObjectId96, list110);
		NavigationCommandRefObjectType? refObjectType97 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId97 = DataProviderTypeSE.CashDesks;
		num = 4;
		List<short> list111 = new List<short>(num);
		CollectionsMarshal.SetCount(list111, num);
		Span<short> span65 = CollectionsMarshal.AsSpan(list111);
		span65[0] = 1;
		span65[1] = 2;
		span65[2] = 4;
		span65[3] = 7;
		CashDesks = new UserAccessIdSE("CashDesks", 119, refObjectType97, refObjectId97, list111);
		NavigationCommandRefObjectType? refObjectType98 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId98 = DataProviderTypeSE.CashierShift;
		num = 2;
		List<short> list112 = new List<short>(num);
		CollectionsMarshal.SetCount(list112, num);
		Span<short> span66 = CollectionsMarshal.AsSpan(list112);
		span66[0] = 2;
		span66[1] = 4;
		CashierShift = new UserAccessIdSE("CashierShift", 436, refObjectType98, refObjectId98, list112);
		NavigationCommandRefObjectType? refObjectType99 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId99 = DataProviderTypeSE.ElScalesAndTerminals;
		num = 1;
		List<short> list113 = new List<short>(num);
		CollectionsMarshal.SetCount(list113, num);
		CollectionsMarshal.AsSpan(list113)[0] = 2;
		ElScalesAndTerminals = new UserAccessIdSE("ElScalesAndTerminals", 407, refObjectType99, refObjectId99, list113);
		NavigationCommandRefObjectType? refObjectType100 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId100 = DataProviderTypeSE.AccountsInBank;
		num = 3;
		List<short> list114 = new List<short>(num);
		CollectionsMarshal.SetCount(list114, num);
		Span<short> span67 = CollectionsMarshal.AsSpan(list114);
		span67[0] = 1;
		span67[1] = 2;
		span67[2] = 4;
		AccountsInBank = new UserAccessIdSE("AccountsInBank", 122, refObjectType100, refObjectId100, list114);
		NavigationCommandRefObjectType? refObjectType101 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId101 = DataProviderTypeSE.QntUnits;
		num = 2;
		List<short> list115 = new List<short>(num);
		CollectionsMarshal.SetCount(list115, num);
		Span<short> span68 = CollectionsMarshal.AsSpan(list115);
		span68[0] = 1;
		span68[1] = 3;
		QntUnits = new UserAccessIdSE("QntUnits", 115, refObjectType101, refObjectId101, list115);
		NavigationCommandRefObjectType? refObjectType102 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId102 = DataProviderTypeSE.PriceTypes;
		num = 2;
		List<short> list116 = new List<short>(num);
		CollectionsMarshal.SetCount(list116, num);
		Span<short> span69 = CollectionsMarshal.AsSpan(list116);
		span69[0] = 1;
		span69[1] = 2;
		PriceTypes = new UserAccessIdSE("PriceTypes", 116, refObjectType102, refObjectId102, list116);
		NavigationCommandRefObjectType? refObjectType103 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId103 = DataProviderTypeSE.Countries;
		num = 2;
		List<short> list117 = new List<short>(num);
		CollectionsMarshal.SetCount(list117, num);
		Span<short> span70 = CollectionsMarshal.AsSpan(list117);
		span70[0] = 1;
		span70[1] = 3;
		Countries = new UserAccessIdSE("Countries", 123, refObjectType103, refObjectId103, list117);
		NavigationCommandRefObjectType? refObjectType104 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId104 = DataProviderTypeSE.Producers;
		num = 2;
		List<short> list118 = new List<short>(num);
		CollectionsMarshal.SetCount(list118, num);
		Span<short> span71 = CollectionsMarshal.AsSpan(list118);
		span71[0] = 1;
		span71[1] = 3;
		Makers = new UserAccessIdSE("Makers", 124, refObjectType104, refObjectId104, list118);
		NavigationCommandRefObjectType? refObjectType105 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId105 = DataProviderTypeSE.PropertiesTypes;
		num = 2;
		List<short> list119 = new List<short>(num);
		CollectionsMarshal.SetCount(list119, num);
		Span<short> span72 = CollectionsMarshal.AsSpan(list119);
		span72[0] = 1;
		span72[1] = 3;
		PropertiesTypes = new UserAccessIdSE("PropertiesTypes", 125, refObjectType105, refObjectId105, list119);
		NavigationCommandRefObjectType? refObjectType106 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId106 = DataProviderTypeSE.StandardComments;
		num = 3;
		List<short> list120 = new List<short>(num);
		CollectionsMarshal.SetCount(list120, num);
		Span<short> span73 = CollectionsMarshal.AsSpan(list120);
		span73[0] = 1;
		span73[1] = 3;
		span73[2] = 4;
		StandardComments = new UserAccessIdSE("StandardComments", 128, refObjectType106, refObjectId106, list120);
		NavigationCommandRefObjectType? refObjectType107 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId107 = DataProviderTypeSE.DiscountGroups;
		num = 1;
		List<short> list121 = new List<short>(num);
		CollectionsMarshal.SetCount(list121, num);
		CollectionsMarshal.AsSpan(list121)[0] = 2;
		DiscountGroups = new UserAccessIdSE("DiscountGroups", 448, refObjectType107, refObjectId107, list121);
		NavigationCommandRefObjectType? refObjectType108 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId108 = DataProviderTypeSE.Discounts;
		num = 1;
		List<short> list122 = new List<short>(num);
		CollectionsMarshal.SetCount(list122, num);
		CollectionsMarshal.AsSpan(list122)[0] = 2;
		Discounts = new UserAccessIdSE("Discounts", 449, refObjectType108, refObjectId108, list122);
		NavigationCommandRefObjectType? refObjectType109 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId109 = DataProviderTypeSE.BonusScale;
		num = 1;
		List<short> list123 = new List<short>(num);
		CollectionsMarshal.SetCount(list123, num);
		CollectionsMarshal.AsSpan(list123)[0] = 2;
		BonusScale = new UserAccessIdSE("BonusScale", 345, refObjectType109, refObjectId109, list123);
		NavigationCommandRefObjectType? refObjectType110 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId110 = DataProviderTypeSE.Employees;
		num = 1;
		List<short> list124 = new List<short>(num);
		CollectionsMarshal.SetCount(list124, num);
		CollectionsMarshal.AsSpan(list124)[0] = 2;
		Employees = new UserAccessIdSE("Employees", 127, refObjectType110, refObjectId110, list124);
		NavigationCommandRefObjectType? refObjectType111 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId111 = DataProviderTypeSE.MTQntUnits;
		num = 3;
		List<short> list125 = new List<short>(num);
		CollectionsMarshal.SetCount(list125, num);
		Span<short> span74 = CollectionsMarshal.AsSpan(list125);
		span74[0] = 1;
		span74[1] = 2;
		span74[2] = 3;
		MTQntUnits = new UserAccessIdSE("MTQntUnits", 147, refObjectType111, refObjectId111, list125);
		NavigationCommandRefObjectType? refObjectType112 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId112 = DataProviderTypeSE.MTBarCodes;
		num = 3;
		List<short> list126 = new List<short>(num);
		CollectionsMarshal.SetCount(list126, num);
		Span<short> span75 = CollectionsMarshal.AsSpan(list126);
		span75[0] = 1;
		span75[1] = 2;
		span75[2] = 3;
		MTBarCodes = new UserAccessIdSE("MTBarCodes", 343, refObjectType112, refObjectId112, list126);
		NavigationCommandRefObjectType? refObjectType113 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId113 = DataProviderTypeSE.MTPrices;
		num = 2;
		List<short> list127 = new List<short>(num);
		CollectionsMarshal.SetCount(list127, num);
		Span<short> span76 = CollectionsMarshal.AsSpan(list127);
		span76[0] = 1;
		span76[1] = 2;
		MTPrices = new UserAccessIdSE("MTPrices", 148, refObjectType113, refObjectId113, list127);
		NavigationCommandRefObjectType? refObjectType114 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId114 = DataProviderTypeSE.OrderMinMax;
		num = 2;
		List<short> list128 = new List<short>(num);
		CollectionsMarshal.SetCount(list128, num);
		Span<short> span77 = CollectionsMarshal.AsSpan(list128);
		span77[0] = 1;
		span77[1] = 3;
		OrderMinMax = new UserAccessIdSE("OrderMinMax", 513, refObjectType114, refObjectId114, list128);
		NavigationCommandRefObjectType? refObjectType115 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId115 = DataProviderTypeSE.MTComplectsParts;
		num = 3;
		List<short> list129 = new List<short>(num);
		CollectionsMarshal.SetCount(list129, num);
		Span<short> span78 = CollectionsMarshal.AsSpan(list129);
		span78[0] = 1;
		span78[1] = 2;
		span78[2] = 3;
		MTComplectsParts = new UserAccessIdSE("MTComplectsParts", 150, refObjectType115, refObjectId115, list129);
		NavigationCommandRefObjectType? refObjectType116 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId116 = DataProviderTypeSE.PartContracts;
		num = 3;
		List<short> list130 = new List<short>(num);
		CollectionsMarshal.SetCount(list130, num);
		Span<short> span79 = CollectionsMarshal.AsSpan(list130);
		span79[0] = 1;
		span79[1] = 2;
		span79[2] = 4;
		PartContracts = new UserAccessIdSE("PartContracts", 151, refObjectType116, refObjectId116, list130);
		NavigationCommandRefObjectType? refObjectType117 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId117 = DataProviderTypeSE.MTComplects;
		num = 3;
		List<short> list131 = new List<short>(num);
		CollectionsMarshal.SetCount(list131, num);
		Span<short> span80 = CollectionsMarshal.AsSpan(list131);
		span80[0] = 1;
		span80[1] = 2;
		span80[2] = 3;
		MTComplects = new UserAccessIdSE("MTComplects", 305, refObjectType117, refObjectId117, list131);
		NavigationCommandRefObjectType? refObjectType118 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId118 = DataProviderTypeSE.PropertiesListsValues;
		num = 2;
		List<short> list132 = new List<short>(num);
		CollectionsMarshal.SetCount(list132, num);
		Span<short> span81 = CollectionsMarshal.AsSpan(list132);
		span81[0] = 1;
		span81[1] = 3;
		PropertiesListsValues = new UserAccessIdSE("PropertiesListsValues", 329, refObjectType118, refObjectId118, list132);
		NavigationCommandRefObjectType? refObjectType119 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId119 = DataProviderTypeSE.PartnerAccountsInBank;
		num = 3;
		List<short> list133 = new List<short>(num);
		CollectionsMarshal.SetCount(list133, num);
		Span<short> span82 = CollectionsMarshal.AsSpan(list133);
		span82[0] = 1;
		span82[1] = 2;
		span82[2] = 4;
		PartnerAccountsInBank = new UserAccessIdSE("PartnerAccountsInBank", 265, refObjectType119, refObjectId119, list133);
		num = 1;
		List<short> list134 = new List<short>(num);
		CollectionsMarshal.SetCount(list134, num);
		CollectionsMarshal.AsSpan(list134)[0] = 4;
		subsystemIds = list134;
		IBImportTransactions = new UserAccessIdSE("IBImportTransactions", 257, null, 0, subsystemIds);
		num = 1;
		List<short> list135 = new List<short>(num);
		CollectionsMarshal.SetCount(list135, num);
		CollectionsMarshal.AsSpan(list135)[0] = 4;
		subsystemIds = list135;
		CBAImportExchangeRates = new UserAccessIdSE("CBAImportExchangeRates", 316, null, 0, subsystemIds);
		num = 1;
		List<short> list136 = new List<short>(num);
		CollectionsMarshal.SetCount(list136, num);
		CollectionsMarshal.AsSpan(list136)[0] = 4;
		subsystemIds = list136;
		ImportBanksFromB2B = new UserAccessIdSE("ImportBanksFromB2B", 434, null, 0, subsystemIds);
		DocumentLinkedList = new UserAccessIdSE("DocumentLinkedList", 514, null, 0);
		NavigationCommandRefObjectType? refObjectType120 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId120 = DataProviderTypeSE.PaymentOrdersJournal;
		num = 1;
		List<short> list137 = new List<short>(num);
		CollectionsMarshal.SetCount(list137, num);
		CollectionsMarshal.AsSpan(list137)[0] = 4;
		PaymentOrdersJournal = new UserAccessIdSE("PaymentOrdersJournal", 309, refObjectType120, refObjectId120, list137);
		NavigationCommandRefObjectType? refObjectType121 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId121 = DataProviderTypeSE.IBTransactions;
		num = 1;
		List<short> list138 = new List<short>(num);
		CollectionsMarshal.SetCount(list138, num);
		CollectionsMarshal.AsSpan(list138)[0] = 4;
		IBTransactions = new UserAccessIdSE("IBTransactions", 315, refObjectType121, refObjectId121, list138);
		NavigationCommandRefObjectType? refObjectType122 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId122 = DataProviderTypeSE.PurchaseOrdersDocumentsJournal;
		num = 1;
		List<short> list139 = new List<short>(num);
		CollectionsMarshal.SetCount(list139, num);
		CollectionsMarshal.AsSpan(list139)[0] = 1;
		PurchaseOrdersDocsJournal = new UserAccessIdSE("PurchaseOrdersDocsJournal", 350, refObjectType122, refObjectId122, list139);
		NavigationCommandRefObjectType? refObjectType123 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId123 = DataProviderTypeSE.AllDocumentsJournal;
		num = 1;
		List<short> list140 = new List<short>(num);
		CollectionsMarshal.SetCount(list140, num);
		CollectionsMarshal.AsSpan(list140)[0] = 1;
		DocumentsJournal = new UserAccessIdSE("DocumentsJournal", 129, refObjectType123, refObjectId123, list140);
		num = 1;
		List<short> list141 = new List<short>(num);
		CollectionsMarshal.SetCount(list141, num);
		CollectionsMarshal.AsSpan(list141)[0] = 9;
		subsystemIds = list141;
		ChangeActualnessDate = new UserAccessIdSE("ChangeActualnessDate", 97, null, 0, subsystemIds);
		B2BDocuments = new UserAccessIdSE("B2BDocuments", 353, null, 0);
		num = 1;
		List<short> list142 = new List<short>(num);
		CollectionsMarshal.SetCount(list142, num);
		CollectionsMarshal.AsSpan(list142)[0] = 6;
		subsystemIds = list142;
		B2BProducts = new UserAccessIdSE("B2BProducts", 354, null, 0, subsystemIds);
		B2BInvoicesForPay = new UserAccessIdSE("B2BInvoicesForPay", 412, null, 0);
		num = 1;
		List<short> list143 = new List<short>(num);
		CollectionsMarshal.SetCount(list143, num);
		CollectionsMarshal.AsSpan(list143)[0] = 6;
		subsystemIds = list143;
		B2BReceivedDocs = new UserAccessIdSE("B2BReceivedDocs", 355, null, 0, subsystemIds);
		num = 1;
		List<short> list144 = new List<short>(num);
		CollectionsMarshal.SetCount(list144, num);
		CollectionsMarshal.AsSpan(list144)[0] = 6;
		subsystemIds = list144;
		B2BSentDocs = new UserAccessIdSE("B2BSentDocs", 358, null, 0, subsystemIds);
		num = 1;
		List<short> list145 = new List<short>(num);
		CollectionsMarshal.SetCount(list145, num);
		CollectionsMarshal.AsSpan(list145)[0] = 6;
		subsystemIds = list145;
		B2BOrganizationData = new UserAccessIdSE("B2BOrganizationData", 443, null, 0, subsystemIds);
		NavigationCommandRefObjectType? refObjectType124 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId124 = DataProviderTypeSE.DiscountServerBonusCards;
		num = 1;
		List<short> list146 = new List<short>(num);
		CollectionsMarshal.SetCount(list146, num);
		CollectionsMarshal.AsSpan(list146)[0] = 2;
		DiscountServerBonusCards = new UserAccessIdSE("DiscountServerBonusCards", 439, refObjectType124, refObjectId124, list146);
		NavigationCommandRefObjectType? refObjectType125 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId125 = DataProviderTypeSE.DiscountServerBonusTransactions;
		num = 1;
		List<short> list147 = new List<short>(num);
		CollectionsMarshal.SetCount(list147, num);
		CollectionsMarshal.AsSpan(list147)[0] = 2;
		DiscountServerBonusTransactions = new UserAccessIdSE("DiscountServerBonusTransactions", 440, refObjectType125, refObjectId125, list147);
		NavigationCommandRefObjectType? refObjectType126 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId126 = DataProviderTypeSE.BonusSumsComparison;
		num = 1;
		List<short> list148 = new List<short>(num);
		CollectionsMarshal.SetCount(list148, num);
		CollectionsMarshal.AsSpan(list148)[0] = 2;
		DiscountServerBonusSumComparison = new UserAccessIdSE("DiscountServerBonusSumComparison", 442, refObjectType126, refObjectId126, list148);
		num = 1;
		List<short> list149 = new List<short>(num);
		CollectionsMarshal.SetCount(list149, num);
		CollectionsMarshal.AsSpan(list149)[0] = 4;
		subsystemIds = list149;
		OnlineBankQuering = new UserAccessIdSE("OnlineBankQuering", 431, null, 0, subsystemIds);
		num = 1;
		List<short> list150 = new List<short>(num);
		CollectionsMarshal.SetCount(list150, num);
		CollectionsMarshal.AsSpan(list150)[0] = 4;
		subsystemIds = list150;
		OnlineAccountStatement = new UserAccessIdSE("OnlineAccountStatement", 432, null, 0, subsystemIds);
		NavigationCommandRefObjectType? refObjectType127 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId127 = DataProviderTypeSE.IBBankAccountsRemains;
		num = 1;
		List<short> list151 = new List<short>(num);
		CollectionsMarshal.SetCount(list151, num);
		CollectionsMarshal.AsSpan(list151)[0] = 4;
		OnlineAccountsRemains = new UserAccessIdSE("OnlineAccountsRemains", 433, refObjectType127, refObjectId127, list151);
		NavigationCommandRefObjectType? refObjectType128 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId128 = DataProviderTypeSE.IBMerchantStatements;
		num = 1;
		List<short> list152 = new List<short>(num);
		CollectionsMarshal.SetCount(list152, num);
		CollectionsMarshal.AsSpan(list152)[0] = 4;
		MerchantStatements = new UserAccessIdSE("MerchantStatements", 517, refObjectType128, refObjectId128, list152);
		num = 1;
		List<short> list153 = new List<short>(num);
		CollectionsMarshal.SetCount(list153, num);
		CollectionsMarshal.AsSpan(list153)[0] = 9;
		subsystemIds = list153;
		PublishSettings = new UserAccessIdSE("PublishSettings", 456, null, 0, subsystemIds);
		num = 1;
		List<short> list154 = new List<short>(num);
		CollectionsMarshal.SetCount(list154, num);
		CollectionsMarshal.AsSpan(list154)[0] = 6;
		subsystemIds = list154;
		b2bReports = new UserAccessIdSE("b2bReports", 441, null, 0, subsystemIds);
		NavigationCommandRefObjectType? refObjectType129 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId129 = DataProviderTypeSE.EMarkReceiptApp;
		num = 1;
		List<short> list155 = new List<short>(num);
		CollectionsMarshal.SetCount(list155, num);
		CollectionsMarshal.AsSpan(list155)[0] = 9;
		EMarkReceiptApp = new UserAccessIdSE("EMarkReceiptApp", 457, refObjectType129, refObjectId129, list155);
		num = 1;
		List<short> list156 = new List<short>(num);
		CollectionsMarshal.SetCount(list156, num);
		CollectionsMarshal.AsSpan(list156)[0] = 5;
		subsystemIds = list156;
		AnalysisDashBoard = new UserAccessIdSE("AnalysisDashBoard", 382, null, 0, subsystemIds);
		NavigationCommandRefObjectType? refObjectType130 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId130 = DataProviderTypeSE.SalesAnalysisReport;
		num = 1;
		List<short> list157 = new List<short>(num);
		CollectionsMarshal.SetCount(list157, num);
		CollectionsMarshal.AsSpan(list157)[0] = 5;
		SalesAnalysisDiagram = new UserAccessIdSE("SalesAnalysisDiagram", 383, refObjectType130, refObjectId130, list157);
		NavigationCommandRefObjectType? refObjectType131 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId131 = DataProviderTypeSE.BestSellersReport;
		num = 1;
		List<short> list158 = new List<short>(num);
		CollectionsMarshal.SetCount(list158, num);
		CollectionsMarshal.AsSpan(list158)[0] = 5;
		SalesAnalysisBestSellersReport = new UserAccessIdSE("SalesAnalysisBestSellersReport", 384, refObjectType131, refObjectId131, list158);
		num = 1;
		List<short> list159 = new List<short>(num);
		CollectionsMarshal.SetCount(list159, num);
		CollectionsMarshal.AsSpan(list159)[0] = 5;
		subsystemIds = list159;
		AnalysisDebtorsCreditorsRems = new UserAccessIdSE("AnalysisDebtorsCreditorsRems", 385, null, 0, subsystemIds);
		NavigationCommandRefObjectType? refObjectType132 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId132 = DataProviderTypeSE.CashBankAnalysisReport;
		num = 1;
		List<short> list160 = new List<short>(num);
		CollectionsMarshal.SetCount(list160, num);
		CollectionsMarshal.AsSpan(list160)[0] = 5;
		CashBankAnalysisReport = new UserAccessIdSE("CashBankAnalysisReport", 386, refObjectType132, refObjectId132, list160);
		NavigationCommandRefObjectType? refObjectType133 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId133 = DataProviderTypeSE.GoodsABCAnalysisReport;
		num = 1;
		List<short> list161 = new List<short>(num);
		CollectionsMarshal.SetCount(list161, num);
		CollectionsMarshal.AsSpan(list161)[0] = 5;
		GoodsABCAnalysisReport = new UserAccessIdSE("GoodsABCAnalysisReport", 397, refObjectType133, refObjectId133, list161);
		NavigationCommandRefObjectType? refObjectType134 = NavigationCommandRefObjectType.DataProvider;
		short refObjectId134 = DataProviderTypeSE.PartnersABCAnalysisReport;
		num = 1;
		List<short> list162 = new List<short>(num);
		CollectionsMarshal.SetCount(list162, num);
		CollectionsMarshal.AsSpan(list162)[0] = 5;
		PartnersABCAnalysisReport = new UserAccessIdSE("PartnersABCAnalysisReport", 398, refObjectType134, refObjectId134, list162);
		DebugDialogTest = new UserAccessIdSE("DebugDialogTest", 388, null, 0);
		DebugReportTest = new UserAccessIdSE("DebugReportTest", 389, null, 0);
		DebugCreateStoredProcedures = new UserAccessIdSE("DebugCreateStoredProcedures", 390, null, 0);
		DebugTransactionsControl = new UserAccessIdSE("DebugTransactionsControl", 391, null, 0);
		DebugFinalizeTracer = new UserAccessIdSE("DebugFinalizeTracer", 393, null, 0);
		DebugExportMetadata = new UserAccessIdSE("DebugExportMetadata", 396, null, 0);
		DebugAppTokenViewer = new UserAccessIdSE("DebugAppTokenViewer", 437, null, 0);
		DebugRunDiscountServerTests = new UserAccessIdSE("DebugRunDiscountServerTests", 438, null, 0);
		DebugNotificationTest = new UserAccessIdSE("DebugNotificationTest", 518, null, 0);
		UserCashesAccess = new UserAccessIdSE("UserCashesAccess", 515, null, 0);
		UserStorageOptions = new UserAccessIdSE("UserStorageOptions", 516, null, 0);
		num = 1;
		List<short> list163 = new List<short>(num);
		CollectionsMarshal.SetCount(list163, num);
		CollectionsMarshal.AsSpan(list163)[0] = 9;
		subsystemIds = list163;
		ImportDataFromASAccountantWindow = new UserAccessIdSE("ImportDataFromASAccountantWindow", 57, null, 0, subsystemIds);
		num = 1;
		List<short> list164 = new List<short>(num);
		CollectionsMarshal.SetCount(list164, num);
		CollectionsMarshal.AsSpan(list164)[0] = 9;
		subsystemIds = list164;
		DatabasesOperations = new UserAccessIdSE("DatabasesOperations", 520, null, 0, subsystemIds);
		Currencies = new UserAccessIdSE("Currencies", 126, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Currencies);
		ExchangeRates = new UserAccessIdSE("ExchangeRates", 121, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.ExchangeRates);
		Banks = new UserAccessIdSE("Banks", 120, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Banks);
		DocLogs = new UserAccessIdSE("DocLogs", 357, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DocsLog);
		Templates = new UserAccessIdSE("Templates", 51, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Templates);
		DatabaseLog = new UserAccessIdSE("DatabaseLog", 65, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DatabaseLog);
		Databases = new UserAccessIdSE("Databases", 50, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Databases);
		DatabaseConnections = new UserAccessIdSE("DatabaseConnections", 53, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DatabaseConnections);
		Users = new UserAccessIdSE("Users", 48, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Users);
		RefreshServiceCaches = new UserAccessIdSE("RefreshServiceCaches", 1000, null, 0);
		CPAClassifications = new UserAccessIdSE("CPAClassifications", 509, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.CPAClassifications);
		Integrations = new UserAccessIdSE("Integrations", 1001, null, 0);
		ApiKeys = new UserAccessIdSE("ApiKeys", 1002, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.ApiKeys);
		Backups = new UserAccessIdSE("Backups", 49, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Backups);
		CheckIntegrity = new UserAccessIdSE("CheckIntegrity", 52, null, 0);
		Archive = new UserAccessIdSE("Archive", 54, null, 0);
		ImportData = new UserAccessIdSE("ImportData", 55, null, 0);
		ExportData = new UserAccessIdSE("ExportData", 56, null, 0);
		DirsLog = new UserAccessIdSE("DirsLog", 453, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DirsLog);
		Extensions = new UserAccessIdSE("Extensions", 420, null, 0);
		ImportExtensions = new UserAccessIdSE("ImportExtensions", 421, null, 0);
		DocumentsExtensions = new UserAccessIdSE("DocumentsExtensions", 422, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DocumentExtensions);
		DirectoriesExtensions = new UserAccessIdSE("DirectoriesExtensions", 1005, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DirectoryExtensions);
		DataViewsExtensions = new UserAccessIdSE("DataViewsExtensions", 423, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DataViewDefinitions);
		TreesDefinitions = new UserAccessIdSE("TreesDefinitions", 424, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.Trees);
		UserScriptsDefinitions = new UserAccessIdSE("UserScriptsDefinitions", 435, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.UserScriptsDefinitions);
		AWPDefinitions = new UserAccessIdSE("AWPDefinitions", 454, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.AWPDefinitions);
		ContextFunctionDefinitions = new UserAccessIdSE("ContextFunctionDefinitions", 1007, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.ContextFunctionDefinitions);
		UserResources = new UserAccessIdSE("UserResources", 1006, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.UserResources);
		DialogValueTemplates = new UserAccessIdSE("DialogValueTemplates", 463, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DialogValueTemplates);
		MakeAssemblyFromExtensions = new UserAccessIdSE("MakeAssemblyFromExtensions", 425, null, 0);
		UserReportsDefinitions = new UserAccessIdSE("UserReportsDefinitions", 444, null, 0);
		UserRepTemplates = new UserAccessIdSE("UserRepTemplates", 445, null, 0);
		DebugCalls = new UserAccessIdSE("DebugCalls", 460, null, 0);
		OnlineLicenses = new UserAccessIdSE("OnlineLicenses", 451, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.OnlineLicenses);
		DataViewLayouts = new UserAccessIdSE("DataViewLayouts", 452, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.DataViewLayouts);
		TaxService = new UserAccessIdSE("TaxService", 466, null, 0);
		TaxServiceTaxInvoices = new UserAccessIdSE("TaxServiceTaxInvoices", 467, null, 0);
		TaxServiceTaxInvoicesSubmitted = new UserAccessIdSE("TaxServiceTaxInvoicesSubmitted", 468, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceTaxInvoicesSubmitted);
		TaxServiceTaxInvoicesReceived = new UserAccessIdSE("TaxServiceTaxInvoicesReceived", 469, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceTaxInvoicesReceived);
		TaxServiceTaxInvoicesDraft = new UserAccessIdSE("TaxServiceTaxInvoicesDraft", 470, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceTaxInvoicesDraft);
		TaxServiceAccDocs = new UserAccessIdSE("TaxServiceAccDocs", 475, null, 0);
		TaxServiceAccDocsSubmitted = new UserAccessIdSE("TaxServiceAccDocsSubmitted", 476, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceAccDocsSubmitted);
		TaxServiceAccDocsReceived = new UserAccessIdSE("TaxServiceAccDocsReceived", 477, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceAccDocsReceived);
		TaxServiceAccDocsDraft = new UserAccessIdSE("TaxServiceAccDocsDraft", 478, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceAccDocsDraft);
		TaxServiceTaxInvoicesCancelled = new UserAccessIdSE("TaxServiceTaxInvoicesCancelled", 1003, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceTaxInvoicesCancelled);
		TaxServiceAccDocsCancelled = new UserAccessIdSE("TaxServiceAccDocsCancelled", 1004, NavigationCommandRefObjectType.DataProvider, DataProviderTypeSE.TaxServiceAccDocsCancelled);
		DebugChangeDBVersion = new UserAccessIdSE("DebugChangeDBVersion", 395, null, 0);
		DebugExpPrintSubstitutions = new UserAccessIdSE("DebugExpPrintSubstitutions", 394, null, 0);
		UserAccessIdSE currencies = Currencies;
		num = 3;
		List<short> list165 = new List<short>(num);
		CollectionsMarshal.SetCount(list165, num);
		Span<short> span83 = CollectionsMarshal.AsSpan(list165);
		span83[0] = 1;
		span83[1] = 2;
		span83[2] = 4;
		currencies.SubsystemIds = list165;
		UserAccessIdSE exchangeRates = ExchangeRates;
		num = 3;
		List<short> list166 = new List<short>(num);
		CollectionsMarshal.SetCount(list166, num);
		Span<short> span84 = CollectionsMarshal.AsSpan(list166);
		span84[0] = 1;
		span84[1] = 2;
		span84[2] = 4;
		exchangeRates.SubsystemIds = list166;
		UserAccessIdSE banks = Banks;
		num = 3;
		List<short> list167 = new List<short>(num);
		CollectionsMarshal.SetCount(list167, num);
		Span<short> span85 = CollectionsMarshal.AsSpan(list167);
		span85[0] = 1;
		span85[1] = 2;
		span85[2] = 4;
		banks.SubsystemIds = list167;
		UserAccessIdSE docLogs = DocLogs;
		num = 1;
		List<short> list168 = new List<short>(num);
		CollectionsMarshal.SetCount(list168, num);
		CollectionsMarshal.AsSpan(list168)[0] = 9;
		docLogs.SubsystemIds = list168;
		UserAccessIdSE templates = Templates;
		num = 1;
		List<short> list169 = new List<short>(num);
		CollectionsMarshal.SetCount(list169, num);
		CollectionsMarshal.AsSpan(list169)[0] = 9;
		templates.SubsystemIds = list169;
		UserAccessIdSE databaseLog = DatabaseLog;
		num = 1;
		List<short> list170 = new List<short>(num);
		CollectionsMarshal.SetCount(list170, num);
		CollectionsMarshal.AsSpan(list170)[0] = 9;
		databaseLog.SubsystemIds = list170;
		UserAccessIdSE databases = Databases;
		num = 1;
		List<short> list171 = new List<short>(num);
		CollectionsMarshal.SetCount(list171, num);
		CollectionsMarshal.AsSpan(list171)[0] = 9;
		databases.SubsystemIds = list171;
		UserAccessIdSE databaseConnections = DatabaseConnections;
		num = 1;
		List<short> list172 = new List<short>(num);
		CollectionsMarshal.SetCount(list172, num);
		CollectionsMarshal.AsSpan(list172)[0] = 9;
		databaseConnections.SubsystemIds = list172;
		UserAccessIdSE users = Users;
		num = 1;
		List<short> list173 = new List<short>(num);
		CollectionsMarshal.SetCount(list173, num);
		CollectionsMarshal.AsSpan(list173)[0] = 9;
		users.SubsystemIds = list173;
		UserAccessIdSE refreshServiceCaches = RefreshServiceCaches;
		num = 1;
		List<short> list174 = new List<short>(num);
		CollectionsMarshal.SetCount(list174, num);
		CollectionsMarshal.AsSpan(list174)[0] = 9;
		refreshServiceCaches.SubsystemIds = list174;
		UserAccessIdSE cPAClassifications = CPAClassifications;
		num = 2;
		List<short> list175 = new List<short>(num);
		CollectionsMarshal.SetCount(list175, num);
		Span<short> span86 = CollectionsMarshal.AsSpan(list175);
		span86[0] = 2;
		span86[1] = 3;
		cPAClassifications.SubsystemIds = list175;
		UserAccessIdSE backups = Backups;
		num = 1;
		List<short> list176 = new List<short>(num);
		CollectionsMarshal.SetCount(list176, num);
		CollectionsMarshal.AsSpan(list176)[0] = 9;
		backups.SubsystemIds = list176;
		UserAccessIdSE checkIntegrity = CheckIntegrity;
		num = 1;
		List<short> list177 = new List<short>(num);
		CollectionsMarshal.SetCount(list177, num);
		CollectionsMarshal.AsSpan(list177)[0] = 9;
		checkIntegrity.SubsystemIds = list177;
		UserAccessIdSE archive = Archive;
		num = 1;
		List<short> list178 = new List<short>(num);
		CollectionsMarshal.SetCount(list178, num);
		CollectionsMarshal.AsSpan(list178)[0] = 9;
		archive.SubsystemIds = list178;
		UserAccessIdSE importData = ImportData;
		num = 1;
		List<short> list179 = new List<short>(num);
		CollectionsMarshal.SetCount(list179, num);
		CollectionsMarshal.AsSpan(list179)[0] = 9;
		importData.SubsystemIds = list179;
		UserAccessIdSE exportData = ExportData;
		num = 1;
		List<short> list180 = new List<short>(num);
		CollectionsMarshal.SetCount(list180, num);
		CollectionsMarshal.AsSpan(list180)[0] = 9;
		exportData.SubsystemIds = list180;
		UserAccessIdSE dirsLog = DirsLog;
		num = 1;
		List<short> list181 = new List<short>(num);
		CollectionsMarshal.SetCount(list181, num);
		CollectionsMarshal.AsSpan(list181)[0] = 9;
		dirsLog.SubsystemIds = list181;
		UserAccessIdSE extensions = Extensions;
		num = 1;
		List<short> list182 = new List<short>(num);
		CollectionsMarshal.SetCount(list182, num);
		CollectionsMarshal.AsSpan(list182)[0] = 9;
		extensions.SubsystemIds = list182;
		UserAccessIdSE importExtensions = ImportExtensions;
		num = 1;
		List<short> list183 = new List<short>(num);
		CollectionsMarshal.SetCount(list183, num);
		CollectionsMarshal.AsSpan(list183)[0] = 9;
		importExtensions.SubsystemIds = list183;
		UserAccessIdSE documentsExtensions = DocumentsExtensions;
		num = 1;
		List<short> list184 = new List<short>(num);
		CollectionsMarshal.SetCount(list184, num);
		CollectionsMarshal.AsSpan(list184)[0] = 9;
		documentsExtensions.SubsystemIds = list184;
		UserAccessIdSE dataViewsExtensions = DataViewsExtensions;
		num = 1;
		List<short> list185 = new List<short>(num);
		CollectionsMarshal.SetCount(list185, num);
		CollectionsMarshal.AsSpan(list185)[0] = 9;
		dataViewsExtensions.SubsystemIds = list185;
		UserAccessIdSE treesDefinitions = TreesDefinitions;
		num = 1;
		List<short> list186 = new List<short>(num);
		CollectionsMarshal.SetCount(list186, num);
		CollectionsMarshal.AsSpan(list186)[0] = 9;
		treesDefinitions.SubsystemIds = list186;
		UserAccessIdSE userScriptsDefinitions = UserScriptsDefinitions;
		num = 1;
		List<short> list187 = new List<short>(num);
		CollectionsMarshal.SetCount(list187, num);
		CollectionsMarshal.AsSpan(list187)[0] = 9;
		userScriptsDefinitions.SubsystemIds = list187;
		UserAccessIdSE aWPDefinitions = AWPDefinitions;
		num = 1;
		List<short> list188 = new List<short>(num);
		CollectionsMarshal.SetCount(list188, num);
		CollectionsMarshal.AsSpan(list188)[0] = 9;
		aWPDefinitions.SubsystemIds = list188;
		UserAccessIdSE contextFunctionDefinitions = ContextFunctionDefinitions;
		num = 1;
		List<short> list189 = new List<short>(num);
		CollectionsMarshal.SetCount(list189, num);
		CollectionsMarshal.AsSpan(list189)[0] = 9;
		contextFunctionDefinitions.SubsystemIds = list189;
		UserAccessIdSE makeAssemblyFromExtensions = MakeAssemblyFromExtensions;
		num = 1;
		List<short> list190 = new List<short>(num);
		CollectionsMarshal.SetCount(list190, num);
		CollectionsMarshal.AsSpan(list190)[0] = 9;
		makeAssemblyFromExtensions.SubsystemIds = list190;
		UserAccessIdSE userReportsDefinitions = UserReportsDefinitions;
		num = 1;
		List<short> list191 = new List<short>(num);
		CollectionsMarshal.SetCount(list191, num);
		CollectionsMarshal.AsSpan(list191)[0] = 9;
		userReportsDefinitions.SubsystemIds = list191;
		UserAccessIdSE userRepTemplates = UserRepTemplates;
		num = 1;
		List<short> list192 = new List<short>(num);
		CollectionsMarshal.SetCount(list192, num);
		CollectionsMarshal.AsSpan(list192)[0] = 9;
		userRepTemplates.SubsystemIds = list192;
		UserAccessIdSE debugCalls = DebugCalls;
		num = 1;
		List<short> list193 = new List<short>(num);
		CollectionsMarshal.SetCount(list193, num);
		CollectionsMarshal.AsSpan(list193)[0] = 9;
		debugCalls.SubsystemIds = list193;
		UserAccessIdSE onlineLicenses = OnlineLicenses;
		num = 1;
		List<short> list194 = new List<short>(num);
		CollectionsMarshal.SetCount(list194, num);
		CollectionsMarshal.AsSpan(list194)[0] = 9;
		onlineLicenses.SubsystemIds = list194;
		UserAccessIdSE dataViewLayouts = DataViewLayouts;
		num = 1;
		List<short> list195 = new List<short>(num);
		CollectionsMarshal.SetCount(list195, num);
		CollectionsMarshal.AsSpan(list195)[0] = 9;
		dataViewLayouts.SubsystemIds = list195;
		UserAccessIdSE taxService = TaxService;
		num = 1;
		List<short> list196 = new List<short>(num);
		CollectionsMarshal.SetCount(list196, num);
		CollectionsMarshal.AsSpan(list196)[0] = 8;
		taxService.SubsystemIds = list196;
		UserAccessIdSE taxServiceTaxInvoices = TaxServiceTaxInvoices;
		num = 1;
		List<short> list197 = new List<short>(num);
		CollectionsMarshal.SetCount(list197, num);
		CollectionsMarshal.AsSpan(list197)[0] = 8;
		taxServiceTaxInvoices.SubsystemIds = list197;
		UserAccessIdSE taxServiceTaxInvoicesSubmitted = TaxServiceTaxInvoicesSubmitted;
		num = 1;
		List<short> list198 = new List<short>(num);
		CollectionsMarshal.SetCount(list198, num);
		CollectionsMarshal.AsSpan(list198)[0] = 8;
		taxServiceTaxInvoicesSubmitted.SubsystemIds = list198;
		UserAccessIdSE taxServiceTaxInvoicesReceived = TaxServiceTaxInvoicesReceived;
		num = 1;
		List<short> list199 = new List<short>(num);
		CollectionsMarshal.SetCount(list199, num);
		CollectionsMarshal.AsSpan(list199)[0] = 8;
		taxServiceTaxInvoicesReceived.SubsystemIds = list199;
		UserAccessIdSE taxServiceTaxInvoicesDraft = TaxServiceTaxInvoicesDraft;
		num = 1;
		List<short> list200 = new List<short>(num);
		CollectionsMarshal.SetCount(list200, num);
		CollectionsMarshal.AsSpan(list200)[0] = 8;
		taxServiceTaxInvoicesDraft.SubsystemIds = list200;
		UserAccessIdSE taxServiceAccDocs = TaxServiceAccDocs;
		num = 1;
		List<short> list201 = new List<short>(num);
		CollectionsMarshal.SetCount(list201, num);
		CollectionsMarshal.AsSpan(list201)[0] = 8;
		taxServiceAccDocs.SubsystemIds = list201;
		UserAccessIdSE taxServiceAccDocsSubmitted = TaxServiceAccDocsSubmitted;
		num = 1;
		List<short> list202 = new List<short>(num);
		CollectionsMarshal.SetCount(list202, num);
		CollectionsMarshal.AsSpan(list202)[0] = 8;
		taxServiceAccDocsSubmitted.SubsystemIds = list202;
		UserAccessIdSE taxServiceAccDocsReceived = TaxServiceAccDocsReceived;
		num = 1;
		List<short> list203 = new List<short>(num);
		CollectionsMarshal.SetCount(list203, num);
		CollectionsMarshal.AsSpan(list203)[0] = 8;
		taxServiceAccDocsReceived.SubsystemIds = list203;
		UserAccessIdSE taxServiceAccDocsDraft = TaxServiceAccDocsDraft;
		num = 1;
		List<short> list204 = new List<short>(num);
		CollectionsMarshal.SetCount(list204, num);
		CollectionsMarshal.AsSpan(list204)[0] = 8;
		taxServiceAccDocsDraft.SubsystemIds = list204;
		UserAccessIdSE debugChangeDBVersion = DebugChangeDBVersion;
		num = 1;
		List<short> list205 = new List<short>(num);
		CollectionsMarshal.SetCount(list205, num);
		CollectionsMarshal.AsSpan(list205)[0] = 9;
		debugChangeDBVersion.SubsystemIds = list205;
		UserAccessIdSE debugExpPrintSubstitutions = DebugExpPrintSubstitutions;
		num = 1;
		List<short> list206 = new List<short>(num);
		CollectionsMarshal.SetCount(list206, num);
		CollectionsMarshal.AsSpan(list206)[0] = 9;
		debugExpPrintSubstitutions.SubsystemIds = list206;
	}

	protected UserAccessIdSE(string name, short value, NavigationCommandRefObjectType? refObjectType = null, short refObjectId = 0, List<short> subsystemIds = null)
		: base(name, value)
	{
		RefObjectType = refObjectType;
		RefObjectId = refObjectId;
		SubsystemIds = subsystemIds;
	}
}
