using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Special)]
[SmartEnumPartial(SmartEnumType.Base)]
public class DataProviderTypeSE : ASSmartEnum<DataProviderTypeSE, short>
{
	public static readonly DataProviderTypeSE AccountsInBank = new DataProviderTypeSE("AccountsInBank", 10002, () => ArmSoft.SME.Common.Resources.SettlementAccounts);

	public static readonly DataProviderTypeSE Goods = new DataProviderTypeSE("Goods", 10003, () => Resources.MaterialsAndServices);

	public static readonly DataProviderTypeSE MTGroups = new DataProviderTypeSE("MTGroups", 10004, () => Resources.MaterGrps);

	public static readonly DataProviderTypeSE SubstituteItemsGroups = new DataProviderTypeSE("SubstituteItemsGroups", 10005, () => Resources.SubstituteItemsGroups);

	public static readonly DataProviderTypeSE PartnersMT = new DataProviderTypeSE("PartnersMT", 10007, () => Resources.PartnersMTAndServices);

	public static readonly DataProviderTypeSE Storages = new DataProviderTypeSE("Storages", 10008, () => ArmSoft.SME.Common.Resources.StoragesAndShops);

	public static readonly DataProviderTypeSE Partners = new DataProviderTypeSE("Partners", 10009, () => ArmSoft.SME.Common.Resources.Partners);

	public static readonly DataProviderTypeSE PartGroups = new DataProviderTypeSE("PartGroups", 10010, () => ArmSoft.SME.Common.Resources.PartnersGroups);

	public static readonly DataProviderTypeSE Segments = new DataProviderTypeSE("Segments", 10011, () => Resources.GoodsSegments);

	public static readonly DataProviderTypeSE GoodsBySegment = new DataProviderTypeSE("GoodsBySegment", 10012, () => Resources.GoodsBySegment);

	public static readonly DataProviderTypeSE CashDesks = new DataProviderTypeSE("CashDesks", 10013, () => ArmSoft.SME.Common.Resources.Cashes);

	public static readonly DataProviderTypeSE CashierShift = new DataProviderTypeSE("CashierShift", 10014, () => Resources.CashierShifts);

	public static readonly DataProviderTypeSE QntUnits = new DataProviderTypeSE("QntUnits", 10015, () => ArmSoft.SME.Common.Resources.MeasureUnits);

	public static readonly DataProviderTypeSE PriceTypes = new DataProviderTypeSE("PriceTypes", 10016, () => ArmSoft.SME.Common.Resources.PriceListTypes);

	public static readonly DataProviderTypeSE Countries = new DataProviderTypeSE("Countries", 10019, () => ArmSoft.SME.Common.Resources.Countries);

	public static readonly DataProviderTypeSE PropertiesTypes = new DataProviderTypeSE("PropertiesTypes", 10021, () => Resources.MTPropTypes);

	public static readonly DataProviderTypeSE StandardComments = new DataProviderTypeSE("StandardComments", 10022, () => ArmSoft.SME.Common.Resources.StandartComments);

	public static readonly DataProviderTypeSE DiscountGroups = new DataProviderTypeSE("DiscountGroups", 10023, () => Resources.DiscountGroups);

	public static readonly DataProviderTypeSE Discounts = new DataProviderTypeSE("Discounts", 10024, () => Resources.Discounts);

	public static readonly DataProviderTypeSE BonusScale = new DataProviderTypeSE("BonusScale", 10025, () => Resources.BonusScale);

	public static readonly DataProviderTypeSE Employees = new DataProviderTypeSE("Employees", 10026, () => ArmSoft.SME.Common.Resources.Employees);

	public static readonly DataProviderTypeSE MTQntUnits = new DataProviderTypeSE("MTQntUnits", 10027, () => Resources.MTQntUnits);

	public static readonly DataProviderTypeSE MTBarCodes = new DataProviderTypeSE("MTBarCodes", 10028, () => ArmSoft.SME.Common.Resources.BarCodes);

	public static readonly DataProviderTypeSE MTPrices = new DataProviderTypeSE("MTPrices", 10029, () => ArmSoft.SME.Common.Resources.Prices);

	public static readonly DataProviderTypeSE OrderMinMax = new DataProviderTypeSE("OrderMinMax", 10030, () => Resources.MTOrderMinMax);

	public static readonly DataProviderTypeSE MTComplectsParts = new DataProviderTypeSE("MTComplectsParts", 10032, () => Resources.MTComplectsParts);

	public static readonly DataProviderTypeSE PartContracts = new DataProviderTypeSE("PartContracts", 10033, () => ArmSoft.SME.Common.Resources.Contracts);

	public static readonly DataProviderTypeSE MTComplects = new DataProviderTypeSE("MTComplects", 10034, () => ArmSoft.SME.Common.Resources.Complects);

	public static readonly DataProviderTypeSE PropertiesListsValues = new DataProviderTypeSE("PropertiesListsValues", 10035, () => Resources.PropertiesListsValues);

	public static readonly DataProviderTypeSE PartnerAccountsInBank = new DataProviderTypeSE("PartnerAccountsInBank", 10036, () => Resources.PartnersAccountsInBank);

	public static readonly DataProviderTypeSE MTFavorites = new DataProviderTypeSE("MTFavorites", 10037, () => Resources.MTFavorites);

	public static readonly DataProviderTypeSE Producers = new DataProviderTypeSE("Producers", 10039, () => ArmSoft.SME.Common.Resources.Producers);

	public static readonly DataProviderTypeSE GoodsTurnOver = new DataProviderTypeSE("GoodsTurnOver", 10041, () => Resources.MTTurnOver);

	public static readonly DataProviderTypeSE PaymentOrdersJournal = new DataProviderTypeSE("PaymentOrdersJournal", 10042, () => ArmSoft.SME.Common.Resources.PaymentOrders);

	public static readonly DataProviderTypeSE GoodsCostCalculation = new DataProviderTypeSE("GoodsCostCalculation", 10043, () => Resources.GoodsCostCalculationHeader);

	public static readonly DataProviderTypeSE ECRCheckDocumentsListJournal = new DataProviderTypeSE("ECRCheckDocumentsListJournal", 10044, () => Resources.ECRChecksListJournal);

	public static readonly DataProviderTypeSE MatRepAvailabilityRetail = new DataProviderTypeSE("GoodsRemsRetail", 10045, () => Resources.MTAvailabilityRetail);

	public static readonly DataProviderTypeSE GoodsStockTakingRep = new DataProviderTypeSE("GoodsStockTakingRep", 10046, () => Resources.GoodsStockTakingRep);

	public static readonly DataProviderTypeSE DiscountLogsJournal = new DataProviderTypeSE("DiscountLogsJournal", 10047, () => Resources.DiscountLogsJournal);

	public static readonly DataProviderTypeSE MatRepTurnOverRetail = new DataProviderTypeSE("GoodsRetailTurnOver", 10048, () => Resources.MTRepRetailTurn);

	public static readonly DataProviderTypeSE MatRepAvailability = new DataProviderTypeSE("GoodsRemains", 10049, () => Resources.MatRepAvail);

	public static readonly DataProviderTypeSE MatRepAvailabilityTreeView = new DataProviderTypeSE("GoodsTreeViewRemains", 10050, () => Resources.MatRepAvailTreeView);

	public static readonly DataProviderTypeSE PartRemains = new DataProviderTypeSE("PartnersRemains", 10051, () => Resources.PartnerRemains);

	public static readonly DataProviderTypeSE PartOpJournal = new DataProviderTypeSE("PartnersOperations", 10052, () => Resources.PartnerOpJournal);

	public static readonly DataProviderTypeSE BonusRemains = new DataProviderTypeSE("BonusRemains", 10053, () => Resources.BonusRemains);

	public static readonly DataProviderTypeSE BonusOpJournal = new DataProviderTypeSE("BonusOpJournal", 10054, () => Resources.BonusOpJournal);

	public static readonly DataProviderTypeSE CashOperations = new DataProviderTypeSE("CashOperations", 10055, () => Resources.KassaOpJournal);

	public static readonly DataProviderTypeSE BankOperations = new DataProviderTypeSE("BankOperations", 10065, () => Resources.BankOpJournal);

	public static readonly DataProviderTypeSE CashRemains = new DataProviderTypeSE("CashRemains", 10056, () => Resources.KassaRemains);

	public static readonly DataProviderTypeSE BankRemains = new DataProviderTypeSE("BankRemains", 10064, () => Resources.BankRemains);

	public static readonly DataProviderTypeSE MatRepSalesAnalysis = new DataProviderTypeSE("SalesAnalysis", 10057, () => Resources.MTRepSalesAnalysis);

	public static readonly DataProviderTypeSE MatRepSalesAverage = new DataProviderTypeSE("MatRepSalesAverage", 10068, () => Resources.SalesAveragePrice);

	public static readonly DataProviderTypeSE ReceivedServicesJournal = new DataProviderTypeSE("ReceivedServicesJournal", 10058, () => Resources.ReceivedServicesJournal);

	public static readonly DataProviderTypeSE MatRepOperationsRetail = new DataProviderTypeSE("GoodOperationsRetail", 10059, () => Resources.MTRepRetailOpJournal);

	public static readonly DataProviderTypeSE MTPriceList = new DataProviderTypeSE("MTPriceList", 10060, () => ArmSoft.SME.Common.Resources.PriceList);

	public static readonly DataProviderTypeSE BonusTurnOver = new DataProviderTypeSE("BonusTurnOver", 10061, () => Resources.BonusTurnOver);

	public static readonly DataProviderTypeSE CashTurnOver = new DataProviderTypeSE("CashTurnOver", 10062, () => Resources.KassaTurn);

	public static readonly DataProviderTypeSE BankTurnOver = new DataProviderTypeSE("BankTurnOver", 10063, () => Resources.BankTurn);

	public static readonly DataProviderTypeSE PartnersTurnOver = new DataProviderTypeSE("PartnersTurnOver", 10066, () => Resources.PartnerTurn);

	public static readonly DataProviderTypeSE GoodsRepOperations = new DataProviderTypeSE("GoodsOperations", 10067, () => Resources.MatOperations);

	public static readonly DataProviderTypeSE PartRepCard = new DataProviderTypeSE("PartRepCard", 10070, () => Resources.PartnerCard);

	public static readonly DataProviderTypeSE BonusTAccount = new DataProviderTypeSE("BonusTAccount", 10071, () => Resources.BonusTAccount);

	public static readonly DataProviderTypeSE CashDeskTAccount = new DataProviderTypeSE("CashDeskTAccount", 10072, () => Resources.CashDeskTAccount);

	public static readonly DataProviderTypeSE BankTAccount = new DataProviderTypeSE("BankTAccount", 10073, () => Resources.BankTAccount);

	public static readonly DataProviderTypeSE SalesAnalysisReport = new DataProviderTypeSE("SalesAnalysisReport", 10080, () => ArmSoft.SME.Common.Resources.Sales);

	public static readonly DataProviderTypeSE BestSellersReport = new DataProviderTypeSE("BestSellersReport", 10081, () => ArmSoft.SME.Common.Resources.BestSellers);

	public static readonly DataProviderTypeSE CashBankAnalysisReport = new DataProviderTypeSE("CashBankAnalysisReport", 10082, () => ArmSoft.SME.Common.Resources.CashBankReport);

	public static readonly DataProviderTypeSE ECRReport = new DataProviderTypeSE("ECRReport", 10083, () => Resources.ECRCrnReport);

	public static readonly DataProviderTypeSE CashierShiftReport = new DataProviderTypeSE("CashierShiftReport", 10084, () => Resources.CashierShifts);

	public static readonly DataProviderTypeSE MatRepTAccount = new DataProviderTypeSE("MatRepTAccount", 10085, () => Resources.TAccount);

	public static readonly DataProviderTypeSE GoodsABCAnalysisReport = new DataProviderTypeSE("GoodsABCAnalysisReport", 10086, () => Resources.ABCAnalysisByGoods);

	public static readonly DataProviderTypeSE PartnersABCAnalysisReport = new DataProviderTypeSE("PartnersABCAnalysisReport", 10087, () => Resources.ABCAnalysisByPartners);

	public static readonly DataProviderTypeSE DebitorsCreditorsAnalysisReport = new DataProviderTypeSE("DebitorsCreditorsAnalysisReport", 10088, () => ArmSoft.SME.Common.Resources.CashBankReport);

	public static readonly DataProviderTypeSE UserCashesAccess = new DataProviderTypeSE("UserCashesAccess", 10089, () => Resources.CashesAccess);

	public static readonly DataProviderTypeSE UserStorageOptionsAccess = new DataProviderTypeSE("UserStorageOptionsAccess", 10090, () => Resources.UserStorageAvailability);

	public static readonly DataProviderTypeSE UsersWorkingPeriods = new DataProviderTypeSE("UsersWorkingPeriods", 10091, () => ArmSoft.SME.Common.Resources.OptionWorkPeriod);

	public static readonly DataProviderTypeSE PurchasesByParties = new DataProviderTypeSE("PurchasesByParties", 10092, () => Resources.MatRepAvail);

	public static readonly DataProviderTypeSE BaseECRCheckDocuments = new DataProviderTypeSE("BaseECRCheckDocuments", 10093, () => Resources.BaseECRCheckDocuments);

	public static readonly DataProviderTypeSE CurrentAvailGoodsByParties = new DataProviderTypeSE("CurrentAvailGoodsByParties", 10094, () => Resources.MatRepAvail);

	public static readonly DataProviderTypeSE SalesByParties = new DataProviderTypeSE("SalesByParties", 10095, () => Resources.MatRepAvail);

	public static readonly DataProviderTypeSE GoodsRemsDetailedByStorage = new DataProviderTypeSE("GoodsRemsDetailedByStorage", 10096, () => Resources.MatRepAvail);

	public static readonly DataProviderTypeSE EmployeesWithLog = new DataProviderTypeSE("EmployeesWithLog", 10097, () => ArmSoft.SME.Common.Resources.Employees);

	public static readonly DataProviderTypeSE MTBarCodesCached = new DataProviderTypeSE("MTBarCodesCached", 10098, () => ArmSoft.SME.Common.Resources.BarCodes);

	public static readonly DataProviderTypeSE PartnersWithLog = new DataProviderTypeSE("PartnersWithLog", 10099, () => ArmSoft.SME.Common.Resources.Partners);

	public static readonly DataProviderTypeSE GoodsRems = new DataProviderTypeSE("GoodsRems", 10100, () => Resources.MatRepAvail);

	public static readonly DataProviderTypeSE AllDocumentsJournal = new DataProviderTypeSE("AllDocumentsJournal", 10101, () => ReportTypesSE.AllDocument.Caption);

	public static readonly DataProviderTypeSE CashDocumentsJournal = new DataProviderTypeSE("CashDocumentsJournal", 10102, () => ReportTypesSE.CashDocument.Caption);

	public static readonly DataProviderTypeSE BankDocumentsJournal = new DataProviderTypeSE("BankDocumentsJournal", 10103, () => ReportTypesSE.BankDocument.Caption);

	public static readonly DataProviderTypeSE PurchaseDocumentsJournal = new DataProviderTypeSE("PurchaseDocumentsJournal", 10104, () => ReportTypesSE.PurchaseDocument.Caption);

	public static readonly DataProviderTypeSE SaleDocumentsJournal = new DataProviderTypeSE("SaleDocumentsJournal", 10105, () => ReportTypesSE.SaleDocument.Caption);

	public static readonly DataProviderTypeSE WarehouseDocumentsJournal = new DataProviderTypeSE("WarehouseDocumentsJournal", 10106, () => ReportTypesSE.WarehouseDocument.Caption);

	public static readonly DataProviderTypeSE TaxInvoicesDocumentsJournal = new DataProviderTypeSE("TaxInvoicesDocumentsJournal", 10107, () => ReportTypesSE.TaxInvoices.Caption);

	public static readonly DataProviderTypeSE ReceivedTaxInvoicesDocumentsJournal = new DataProviderTypeSE("ReceivedTaxInvoicesDocumentsJournal", 10108, () => ReportTypesSE.ReceivedTaxInvoices.Caption);

	public static readonly DataProviderTypeSE IBCorrespondentDocumentsJournal = new DataProviderTypeSE("IBCorrespondentDocumentsJournal", 10109, () => ReportTypesSE.IBCorrespondent.Caption);

	public static readonly DataProviderTypeSE TransportationDocumentsJournal = new DataProviderTypeSE("TransportationDocumentsJournal", 10110, () => ReportTypesSE.TransportationDocument.Caption);

	public static readonly DataProviderTypeSE ECRCheckDocumentsJournal = new DataProviderTypeSE("ECRCheckDocumentsJournal", 10111, () => ReportTypesSE.ECRCheckDocument.Caption);

	public static readonly DataProviderTypeSE PurchaseOrdersDocumentsJournal = new DataProviderTypeSE("PurchaseOrdersDocumentsJournal", 10112, () => ReportTypesSE.PurchaseOrderDocument.Caption);

	public static readonly DataProviderTypeSE SentInvoicesForPaymentDocumentsJournal = new DataProviderTypeSE("SentInvoicesForPaymentDocumentsJournal", 10113, () => ReportTypesSE.SentInvoicesForPayment.Caption);

	public static readonly DataProviderTypeSE ReconcilationStatementsDocumentsJournal = new DataProviderTypeSE("ReconcilationStatementsDocumentsJournal", 10114, () => ReportTypesSE.ReconciliationStatements.Caption);

	public static readonly DataProviderTypeSE BillDocumentsJournal = new DataProviderTypeSE("BillDocumentsJournal", 10115, () => ReportTypesSE.ReconciliationStatements.Caption);

	public static readonly DataProviderTypeSE DiscountsDocumentsJournal = new DataProviderTypeSE("DiscountsDocumentsJournal", 10116, () => ReportTypesSE.DiscountDocument.Caption);

	public static readonly DataProviderTypeSE DeletedDocumentsJournal = new DataProviderTypeSE("DeletedDocumentsJournal", 10117, () => ReportTypesSE.DeletedDocument.Caption);

	public static readonly DataProviderTypeSE b2bUnitedProducts = new DataProviderTypeSE("b2bUnitedProducts", 10119, () => ArmSoft.SME.Common.Resources.b2bSystem);

	public static readonly DataProviderTypeSE DiscountServerBonusCards = new DataProviderTypeSE("DiscountServerBonusCards", 10120, () => Resources.BonusCards);

	public static readonly DataProviderTypeSE DiscountServerBonusTransactions = new DataProviderTypeSE("DiscountServerBonusTransactions", 10121, () => Resources.BonusTransactions);

	public static readonly DataProviderTypeSE BonusSumsComparison = new DataProviderTypeSE("BonusSumsComparison", 10122, () => Resources.CompareWithDiscountServer);

	public static readonly DataProviderTypeSE ElScalesAndTerminals = new DataProviderTypeSE("ElScalesAndTerminals", 10123, () => Resources.ElScalesAndTerminals);

	public static readonly DataProviderTypeSE LocalDBDocuments = new DataProviderTypeSE("LocalDBDocuments", 10124, () => ArmSoft.SME.Common.Resources.OfflineECRChecksJournal);

	public static readonly DataProviderTypeSE IBMerchantStatements = new DataProviderTypeSE("IBMerchantStatements", 10125, () => Resources.MerchantStatements);

	public static readonly DataProviderTypeSE B2BProductsCheck = new DataProviderTypeSE("B2BProductsCheck", 10126, () => Resources.DetectedVariations);

	public static readonly DataProviderTypeSE EMarkReceiptApp = new DataProviderTypeSE("EMarkReceiptApp", 10127, () => Resources.EMarkReceiptApp);

	public static readonly DataProviderTypeSE EMarkReceipListJournal = new DataProviderTypeSE("EMarkReceipListJournal", 10128, () => Resources.EMarkReceipListJournal);

	public static readonly DataProviderTypeSE PartnerBusinessAddresses = new DataProviderTypeSE("PartnerBusinessAddresses", 10129, () => ArmSoft.SME.Common.Resources.AdditionalBusAddrList);

	public const short SaleJournalsConst = 10001;

	public const short AccountsInBankConst = 10002;

	public const short GoodsConst = 10003;

	public const short MTGroupsConst = 10004;

	public const short SubstituteItemsGroupsConst = 10005;

	public const short PartnersMTConst = 10007;

	public const short StoragesConst = 10008;

	public const short PartnersConst = 10009;

	public const short PartGroupsConst = 10010;

	public const short SegmentsConst = 10011;

	public const short GoodsBySegmentConst = 10012;

	public const short CashDesksConst = 10013;

	public const short CashierShiftConst = 10014;

	public const short QntUnitsConst = 10015;

	public const short PriceTypesConst = 10016;

	public const short CountriesConst = 10019;

	public const short PropertiesTypesConst = 10021;

	public const short StandardCommentsConst = 10022;

	public const short DiscountGroupsConst = 10023;

	public const short DiscountsConst = 10024;

	public const short BonusScaleConst = 10025;

	public const short EmployeesConst = 10026;

	public const short MTQntUnitsConst = 10027;

	public const short MTBarCodesConst = 10028;

	public const short MTPricesConst = 10029;

	public const short OrderMinMaxConst = 10030;

	public const short MTComplectsPartsConst = 10032;

	public const short PartContractsConst = 10033;

	public const short MTComplectsConst = 10034;

	public const short PropertiesListsValuesConst = 10035;

	public const short PartnerAccountsInBankConst = 10036;

	public const short MTFavoritesConst = 10037;

	public const short ProducersConst = 10039;

	public const short DocumentJournalConst = 10040;

	public const short GoodsTurnOverConst = 10041;

	public const short PaymentOrdersJournalConst = 10042;

	public const short GoodsCostCalculationConst = 10043;

	public const short ECRCheckDocumentsListJournalConst = 10044;

	public const short MatRepAvailabilityRetailConst = 10045;

	public const short GoodsStockTakingRepConst = 10046;

	public const short DiscountLogsJournalConst = 10047;

	public const short MatRepTurnOverRetailConst = 10048;

	public const short MatRepAvailabilityConst = 10049;

	public const short MatRepAvailabilityTreeViewConst = 10050;

	public const short PartRemainsConst = 10051;

	public const short PartOpJournalConst = 10052;

	public const short BonusRemainsConst = 10053;

	public const short BonusOpJournalConst = 10054;

	public const short CashOperationsConst = 10055;

	public const short CashRemainsConst = 10056;

	public const short MatRepSalesAnalysisConst = 10057;

	public const short ReceivedServicesJournalConst = 10058;

	public const short MatRepOperationsRetailConst = 10059;

	public const short MTPriceListConst = 10060;

	public const short BonusTurnOverConst = 10061;

	public const short CashTurnOverConst = 10062;

	public const short BankTurnOverConst = 10063;

	public const short BankRemainsConst = 10064;

	public const short BankOperationsConst = 10065;

	public const short PartnersTurnOverConst = 10066;

	public const short GoodsRepOperationsConst = 10067;

	public const short MatRepSalesAverageConst = 10068;

	public const short DraftInvoicesConst = 10069;

	public const short PartRepCardConst = 10070;

	public const short BonusTAccountConst = 10071;

	public const short CashDeskTAccountConst = 10072;

	public const short BankTAccountConst = 10073;

	public const short SalesAnalysisReportConst = 10080;

	public const short BestSellersReportConst = 10081;

	public const short CashBankAnalysisReportConst = 10082;

	public const short ECRReportConst = 10083;

	public const short CashierShiftReportConst = 10084;

	public const short MatRepTAccountConst = 10085;

	public const short GoodsABCAnalysisReportConst = 10086;

	public const short PartnersABCAnalysisReportConst = 10087;

	public const short DebitorsCreditorsAnalysisReportConst = 10088;

	public const short UserCashesAccessConst = 10089;

	public const short UserStorageOptionsAccessConst = 10090;

	public const short UsersWorkingPeriodsConst = 10091;

	public const short PurchasesByPartiesConst = 10092;

	public const short BaseECRCheckDocumentsConst = 10093;

	public const short CurrentAvailGoodsByPartiesConst = 10094;

	public const short SalesByPartiesConst = 10095;

	public const short GoodsRemsDetailedByStorageConst = 10096;

	public const short EmployeesWithLogConst = 10097;

	public const short MTBarCodesCachedConst = 10098;

	public const short PartnersWithLogConst = 10099;

	public const short GoodsRemsConst = 10100;

	public const short AllDocumentsJournalConst = 10101;

	public const short CashDocumentsJournalConst = 10102;

	public const short BankDocumentsJournalConst = 10103;

	public const short PurchaseDocumentsJournalConst = 10104;

	public const short SaleDocumentsJournalConst = 10105;

	public const short WarehouseDocumentsJournalConst = 10106;

	public const short TaxInvoicesDocumentsJournalConst = 10107;

	public const short ReceivedTaxInvoicesDocumentsJournalConst = 10108;

	public const short IBCorrespondentDocumentsJournalConst = 10109;

	public const short TransportationDocumentsJournalConst = 10110;

	public const short ECRCheckDocumentsJournalConst = 10111;

	public const short PurchaseOrdersDocumentsJournalConst = 10112;

	public const short SentInvoicesForPaymentDocumentsJournalConst = 10113;

	public const short ReconcilationStatementsDocumentsJournalConst = 10114;

	public const short BillDocumentsJournalConst = 10115;

	public const short DiscountsDocumentsJournalConst = 10116;

	public const short DeletedDocumentsJournalConst = 10117;

	public const short GoodsPartiesAvailibilityListByProperty1Const = 10118;

	public const short b2bUnitedProductsConst = 10119;

	public const short DiscountServerBonusCardsConst = 10120;

	public const short DiscountServerBonusTransactionsConst = 10121;

	public const short BonusSumsComparisonConst = 10122;

	public const short ElScalesAndTerminalsConst = 10123;

	public const short LocalDBDocumentsConst = 10124;

	public const short IBMerchantStatementsConst = 10125;

	public const short B2BProductsCheckConst = 10126;

	public const short EMarkReceiptAppConst = 10127;

	public const short EMarkReceipListJournalConst = 10128;

	public const short PartnerBusinessAddressesConst = 10129;

	public static readonly DataProviderTypeSE Currencies = new DataProviderTypeSE("Currencies", 2, () => ArmSoft.SME.Common.Resources.Curres);

	public static readonly DataProviderTypeSE ExchangeRates = new DataProviderTypeSE("ExchangeRates", 3, () => ArmSoft.SME.Common.Resources.ExchangeRates);

	public static readonly DataProviderTypeSE Banks = new DataProviderTypeSE("Banks", 1, () => ArmSoft.SME.Common.Resources.Banks);

	public static readonly DataProviderTypeSE DocsLog = new DataProviderTypeSE("DocsLog", 4, () => ArmSoft.SME.Common.Resources.DocumentsLog);

	public static readonly DataProviderTypeSE DirsLog = new DataProviderTypeSE("DirsLog", 5, () => ArmSoft.SME.Common.Resources.DirectoriesLog);

	public static readonly DataProviderTypeSE Templates = new DataProviderTypeSE("Templates", 6, () => ArmSoft.SME.Common.Resources.PrintTemplates);

	public static readonly DataProviderTypeSE OnlineReportTemplates = new DataProviderTypeSE("OnlineReportTemplates", 34, () => ArmSoft.SME.Common.Resources.OnlineReportsTemplates);

	public static readonly DataProviderTypeSE DatabaseLog = new DataProviderTypeSE("DatabaseLog", 7, () => ArmSoft.SME.Common.Resources.UsersLoginHistory);

	public static readonly DataProviderTypeSE Databases = new DataProviderTypeSE("Databases", 8, () => ArmSoft.SME.Common.Resources.Databases);

	public static readonly DataProviderTypeSE Backups = new DataProviderTypeSE("Backups", 23, () => ArmSoft.SME.Common.Resources.Backups);

	public static readonly DataProviderTypeSE Users = new DataProviderTypeSE("Users", 12, () => ArmSoft.SME.Common.Resources.Users);

	public static readonly DataProviderTypeSE UsersAccess = new DataProviderTypeSE("UsersAccess", 13, () => ArmSoft.SME.Common.Resources.UserAccess);

	public static readonly DataProviderTypeSE DatabaseConnections = new DataProviderTypeSE("DatabaseConnections", 9, () => ArmSoft.SME.Common.Resources.DatabaseConnections);

	public static readonly DataProviderTypeSE DirectoryLog = new DataProviderTypeSE("DirectoryLog", 10, () => ArmSoft.SME.Common.Resources.History);

	public static readonly DataProviderTypeSE DocumentLinkedList = new DataProviderTypeSE("DocumentLinkedList", 11, () => ArmSoft.SME.Common.Resources.LinkedDoc);

	public static readonly DataProviderTypeSE CPAClassifications = new DataProviderTypeSE("CPAClassifications", 14, () => ArmSoft.SME.Common.Resources.CPAClassifications);

	public static readonly DataProviderTypeSE DataViewLayouts = new DataProviderTypeSE("DataViewLayouts", 24, () => ArmSoft.SME.Common.Resources.DataViewLayouts);

	public static readonly DataProviderTypeSE OnlineLicenses = new DataProviderTypeSE("OnlineLicenses", 25, () => ArmSoft.SME.Common.Resources.OnlineLicenses);

	public static readonly DataProviderTypeSE DocumentLog = new DataProviderTypeSE("DocumentLog", 26, () => ArmSoft.SME.Common.Resources.History);

	public static readonly DataProviderTypeSE ApiKeys = new DataProviderTypeSE("ApiKeys", 38, () => ArmSoft.SME.Common.Resources.ApiKeys);

	public static readonly DataProviderTypeSE TaxServiceTaxInvoicesSubmitted = new DataProviderTypeSE("TaxServiceTaxInvoicesSubmitted", 19, () => ArmSoft.SME.Common.Resources.TaxInvoices + " (" + ArmSoft.SME.Common.Resources.EInvoicingSubmitted + ")");

	public static readonly DataProviderTypeSE TaxServiceTaxInvoicesReceived = new DataProviderTypeSE("TaxServiceTaxInvoicesReceived", 17, () => ArmSoft.SME.Common.Resources.TaxInvoices + " (" + ArmSoft.SME.Common.Resources.EInvoicingReceived + ")");

	public static readonly DataProviderTypeSE TaxServiceTaxInvoicesDraft = new DataProviderTypeSE("TaxServiceTaxInvoicesDraft", 15, () => ArmSoft.SME.Common.Resources.TaxInvoices + " (" + ArmSoft.SME.Common.Resources.EInvoicingDraft + ")");

	public static readonly DataProviderTypeSE TaxServiceTaxInvoicesCancelled = new DataProviderTypeSE("TaxServiceTaxInvoicesCancelled", 36, () => ArmSoft.SME.Common.Resources.TaxInvoices + " (" + ArmSoft.SME.Common.Resources.EInvoicingCancelled + ")");

	public static readonly DataProviderTypeSE TaxServiceAccDocsSubmitted = new DataProviderTypeSE("TaxServiceAccDocsSubmitted", 20, () => ArmSoft.SME.Common.Resources.TaxAccDocs + " (" + ArmSoft.SME.Common.Resources.EInvoicingSubmitted + ")");

	public static readonly DataProviderTypeSE TaxServiceAccDocsReceived = new DataProviderTypeSE("TaxServiceAccDocsReceived", 18, () => ArmSoft.SME.Common.Resources.TaxAccDocs + " (" + ArmSoft.SME.Common.Resources.EInvoicingReceived + ")");

	public static readonly DataProviderTypeSE TaxServiceAccDocsDraft = new DataProviderTypeSE("TaxServiceAccDocsDraft", 16, () => ArmSoft.SME.Common.Resources.TaxAccDocs + " (" + ArmSoft.SME.Common.Resources.EInvoicingDraft + ")");

	public static readonly DataProviderTypeSE TaxServiceAccDocsCancelled = new DataProviderTypeSE("TaxServiceAccDocsCancelled", 37, () => ArmSoft.SME.Common.Resources.TaxAccDocs + " (" + ArmSoft.SME.Common.Resources.EInvoicingCancelled + ")");

	public static readonly DataProviderTypeSE IBTransactions = new DataProviderTypeSE("IBTransactions", 21, () => ArmSoft.SME.Common.Resources.IBTransactions);

	public static readonly DataProviderTypeSE IBBankAccountsRemains = new DataProviderTypeSE("IBBankAccountsRemains", 22, () => ArmSoft.SME.Common.Resources.AccountsBalance);

	public static readonly DataProviderTypeSE DataViewDefinitions = new DataProviderTypeSE("DataViewDefinitions", 30, () => ArmSoft.SME.Common.Resources.DataViewsExtensions);

	public static readonly DataProviderTypeSE AWPDefinitions = new DataProviderTypeSE("AWPDefinitions", 27, () => ArmSoft.SME.Common.Resources.AWPDefinitions);

	public static readonly DataProviderTypeSE UserScriptsDefinitions = new DataProviderTypeSE("UserScriptsDefinitions", 28, () => ArmSoft.SME.Common.Resources.UserScriptsDefinitions);

	public static readonly DataProviderTypeSE Trees = new DataProviderTypeSE("Trees", 29, () => ArmSoft.SME.Common.Resources.TreeDefs);

	public static readonly DataProviderTypeSE TreeNodes = new DataProviderTypeSE("TreeNodes", 31, () => ArmSoft.SME.Common.Resources.TreeDefs);

	public static readonly DataProviderTypeSE TreeLog = new DataProviderTypeSE("TreeLog", 33, () => ArmSoft.SME.Common.Resources.History);

	public static readonly DataProviderTypeSE ContextFunctionDefinitions = new DataProviderTypeSE("ContextFunctionDefinitions", 32, () => ArmSoft.SME.Common.Resources.UserOperations);

	public static readonly DataProviderTypeSE DocumentExtensions = new DataProviderTypeSE("DocumentExtensions", 35, () => ArmSoft.SME.Common.Resources.DocumentsExtensions);

	public static readonly DataProviderTypeSE DirectoryExtensions = new DataProviderTypeSE("DirectoryExtensions", 41, () => ArmSoft.SME.Common.Resources.DirectoriesExtensions);

	public static readonly DataProviderTypeSE UserResources = new DataProviderTypeSE("UserResources", 39, () => ArmSoft.SME.Common.Resources.UserResources);

	public static readonly DataProviderTypeSE DialogValueTemplates = new DataProviderTypeSE("DialogValueTemplates", 40, () => ArmSoft.SME.Common.Resources.DialogValuesGroups);

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
