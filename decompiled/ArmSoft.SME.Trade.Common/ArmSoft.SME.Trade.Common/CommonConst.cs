using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.Common;

public static class CommonConst
{
	public enum SubSystemTypes : short
	{
		PurchasesSS = 1,
		SalesSS = 2,
		StorageSS = 3,
		CashBankSS = 4,
		AnalysisReports = 5,
		b2bSS = 6,
		PointOfSaleSS = 7,
		eInvoicingSS = 8,
		AdminTools = 9,
		UserAWP = 100
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct POSTerminalTypes
	{
		public const string None = "0";

		public const string ECRPOS = "1";

		public const string IngenicoACRUS = "2";

		public const string VerifoneINPAS = "3";

		public const string FeitianPOSSOFT = "4";
	}

	public enum CashDeskNonCashTye
	{
		None,
		Idram,
		InecoPay,
		TelCell,
		PayX,
		POS1,
		POS2,
		POS3,
		POS4
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct CodeCaptionPairsTableNames
	{
		public const string StandardComments = "STDCOMM";

		public const string Countries = "COUNTRIES";

		public const string Producers = "PRODUCER";

		public const string SubstituteItemsGroups = "SUBSTITUTEITEMSGROUP";

		public const string Segments = "SEGMENTS";

		public const string CashInfoList = "CASHINFOLIST";
	}

	public enum ChartOfAccountsNodeType : byte
	{
		Leaf = 1,
		InnerNode,
		All
	}

	public enum MTIncomeRemCreateTypes : short
	{
		Manual = 1,
		ClosingYear,
		ClosingYearRetailStorage
	}

	public enum AccountingsTypes : byte
	{
		Cash = 1,
		Bank,
		PartnerContract,
		Bonus,
		PrePayment
	}

	public enum ChartOfAccountsPartnerIdentType
	{
		PartnerIdent,
		Analytics,
		SyntAcc,
		AllExceptPartnerIdent,
		AllExceptAnalytics,
		NotSyntAcc,
		All
	}

	public enum FavoriteTypes : byte
	{
		ReturnButtonAsc = 0,
		Material = 1,
		Group = 2,
		ReturnButtonDesc = 100
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct PriceTypeCheck
	{
		public const string CheckedOnly = "1";

		public const string NotCheckedOnly = "2";

		public const string All = "3";
	}

	public enum GoodsTypes : short
	{
		All,
		Goods,
		Services
	}

	public enum PriceList : short
	{
		IncludedInPriceList = 1,
		NotIncludedInPriceList,
		All
	}

	public enum SalesSumType : short
	{
		SumNCC,
		WithVAT,
		SumCur
	}

	public enum ItemType : byte
	{
		MT = 1,
		Service
	}

	public enum BestSellersCompareMode : byte
	{
		Quantity,
		Income,
		Profit,
		ProfitPercent
	}

	public enum ElSupportSystem : short
	{
		InternetBank = 1,
		ClientBank
	}

	public enum XMLInvoiceDocType : short
	{
		Goods = 1,
		Excise,
		Services,
		Leasing
	}

	public enum SummaTypes : short
	{
		Unknown,
		NCCSummWithoutVAT,
		NCCSummWithVAT,
		CurrSummWithVAT
	}

	public enum SaleSumTypes : short
	{
		Unknown,
		SaleSum,
		SaleCostSum,
		SaleDiscountSum
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct CashInOpTypes
	{
		public const string FromPartner = "1";

		public const string RevenueInput = "2";

		public const string FromSettlementAcc = "3";

		public const string ByPlCard = "4";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct CashOutOpTypes
	{
		public const string ToPartner = "1";

		public const string AmountOutput = "2";

		public const string ToSettlementAcc = "3";

		public const string ByPlCard = "4";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct BankInOutOpTypes
	{
		public const string FromPartner = "1";

		public const string FromSettlementAcc = "2";
	}

	public enum BestSellersCompareModeShort : byte
	{
		Income,
		Profit
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ABCCategories
	{
		public const string A = "A";

		public const string B = "B";

		public const string C = "C";
	}

	public enum ReceivedTaxInvSource : short
	{
		EInvoicing = 1,
		Email
	}

	public enum b2bComparisonResult
	{
		Equal,
		FromB2BNotFound,
		NotFoundInB2B,
		NotEqual
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ScannerMode
	{
		public const string MergeWithFirstMatchedRow = "1";

		public const string MergeWithCurrentRow = "2";

		public const string AddNewRow = "3";
	}

	public enum JSDiscType
	{
		None,
		DiscountPercent,
		DiscountPrice,
		DiscountSumm,
		ProgCalc
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct InclusionExclusion
	{
		public const string Include = "0";

		public const string Exclude = "1";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct HotKeys
	{
		public const string F2 = "F2";

		public const string F3 = "F3";

		public const string F4 = "F4";

		public const string F6 = "F6";

		public const string F7 = "F7";

		public const string F8 = "F8";

		public const string F9 = "F9";

		public const string F11 = "F11";

		public const string Ctrl_F2 = "Ctrl+F2";

		public const string Ctrl_F3 = "Ctrl+F3";

		public const string Ctrl_F5 = "Ctrl+F5";

		public const string Ctrl_F9 = "Ctrl+F9";

		public const string Ctrl_1 = "Ctrl+1";

		public const string Ctrl_2 = "Ctrl+2";

		public const string Ctrl_3 = "Ctrl+3";

		public const string Ctrl_4 = "Ctrl+4";

		public const string Ctrl_5 = "Ctrl+5";

		public const string Ctrl_6 = "Ctrl+6";

		public const string Ctrl_7 = "Ctrl+7";

		public const string Ctrl_8 = "Ctrl+8";

		public const string Ctrl_9 = "Ctrl+9";

		public const string Ctrl_F11 = "Ctrl+F11";

		public const string Shift_F2 = "Shift+F2";

		public const string Shift_F3 = "Shift+F3";

		public const string Shift_F5 = "Shift+F5";

		public const string Shift_F6 = "Shift+F6";

		public const string Shift_F7 = "Shift+F7";

		public const string Shift_F8 = "Shift+F8";

		public const string Shift_F9 = "Shift+F9";

		public const string Shift_F11 = "Shift+F11";

		public const string Shift_F12 = "Shift+F12";

		public const string CtrlShift_M = "Ctrl+Shift+M";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct HelpIDs
	{
		public const string POS_Sale = "POS_Sale";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ScannedQRValues
	{
		public const string MakePaymentByCash = "AS$#PayByCash";

		public const string MakePaymentByNonCash = "AS$#PayByNonCash";
	}

	public const string NCOnly = "1";

	public const string FCOnly = "2";

	public const string NCAndFC = "3";

	public const string IngenicoDefaultValuesSettingsNamePrefix = "ARCUS";

	public const string IngenicoCaption = "ARCUS 2 POS";

	public const string IngenicoPOSCaption = "ARCUS 2 POS (Ingenico/SUNMI)";

	public const string FeitianPOSCaption = "Possoft Pay POS (Feitian)";

	public const string PossoftPayCaption = "Possoft Pay POS";

	public const string WeightItemRegSectionName = "\\Weight Item\\";

	public const string WeightItemCodeLengthRegKey = "Weight Item Code Length";

	public const string WeightItemPrefixRegKey = "Weight Item Prefix";

	public const string WeightItemPrecisionRegKey = "Weight Item Precision";

	public const string WeightItemUnitRegKey = "Weight Item Unit";

	public const string PieceItemRegSectionName = "\\Piece Item\\";

	public const string PieceItemCodeLengthRegKey = "Piece Item Code Length";

	public const string PieceItemPrefixRegKey = "Piece Item Prefix";

	public const string PieceItemPrecisionRegKey = "Piece Item Precision";

	public const string PieceItemUnitRegKey = "Piece Item Unit";

	public const string PieceItemUnitDefaultValue = "001";

	public const string WeightItemUnitDefaultValue = "303";

	public const string EncryptOldKey = "#Lock#";

	public const string OurData = "1";

	public const short LenPartner = 20;

	public const short LenPartnerGroup = 5;

	public const short LenPartnerSettlAcc = 50;

	public const short LenPartMTCode = 60;

	private const short DepthLevelPartnerGroup = 8;

	public const short LenPartnerGroupPath = 48;

	public const short LenTaxCode = 20;

	public const short LenFormulaCode = 20;

	public const short LenExternalCode = 20;

	public const short LenCountry = 3;

	public const short LenProducer = 4;

	public const short LenProperty = 4;

	public const short LenPropertyList = 20;

	public const short LenStorage = 5;

	public const short LenMTGroup = 5;

	public const short LenSubstituteItemsGroup = 5;

	public const short LenQntUnit = 3;

	public const short DepthLevelMtGroup = 8;

	public const short LenMTGroupPath = 48;

	public const short LenPriceType = 2;

	public const short LenRound = 6;

	public const short LenSpec = 100;

	public const short LenStandardComment = 3;

	public const short LenFullCaption = 255;

	public const short LenCaption = 50;

	public const int LenQntUnitCoeffPrecision = 10;

	public const int LenQntUnitCoeff = 19;

	public const short LenPartMTCoeff = 10;

	public const short LenPartPassport = 32;

	public const short LenPartyMethod = 19;

	public const double LenGridGoodCodeColWidth = 125.0;

	public const short LenTrust = 6;

	public const short LenMachine = 9;

	public const short LenFADepartment = 3;

	public const short LenFeesHolds = 3;

	public const short LenAcc = 8;

	public const short LenCashDeskCode = 8;

	public const short LenDiscountCard = 50;

	public const short LenBriefQNTUNIT = 6;

	public const short LenPLUCode = 9;

	public const short LenPLUCodePrecision = 0;

	public const short LenSettlAcc = 22;

	public const short LenPrintPartnerTinOnCheck = 1;

	public const short LenPOSTerminalType = 1;

	public const int DialogGoodCodeWidth = 8;

	public const short LenEmplPassportNum = 25;

	public const short LenPhone = 50;

	public const short LenDocNum = 12;

	public const int LenDocOrderNumber = 7;

	public const string WholeSalePriceWithNCC = "1";

	public const string RetailSalePriceWithNCC = "2";

	public const short LenInvN = 20;

	public const short LenBarCode = 20;

	public const short LenWeightBarCode = 13;

	public const short LenPrefix = 2;

	public const int LenDiscountGroupCode = 5;

	public const int LenDiscountGroupMergeMethodCode = 1;

	public const int LenUsageMethod = 2;

	public const int LenRoundDegreeCode = 4;

	public const int LenDiscountCode = 5;

	public const int LenGoodCode = 20;

	public const int LenDialogGoodCode = 8;

	public const int LenSegmentCode = 5;

	public const int LenGroupByMethod = 2;

	public const int LenNonCashPaymentMethod = 2;

	public const int LenFilteredSearchBtn = 130;

	public const int LenButton90 = 90;

	public const short LenSearchModeType = 1;

	public const string OpMTHI_Income = "I";

	public const string OpMTHI_Expense = "E";

	public const string OpMTHI_Move = "M";

	public const string OpMTHI_Reval = "P";

	public const string OpMTHI_Realization = "R";

	public const string OpMTHI_RealizationRetail = "T";

	public const string OpMTHI_ToQWExploit = "Q";

	public const string OpMTHI_Complectation = "K";

	public const string OpMTHI_DeComplectation = "D";

	public const string OpMTHI_OverheadExp = "O";

	public const string OpMTHI_Return = "V";

	public const string OpMTHI_RetToSup = "W";

	public const string OpMTHI_IncomeRem = "A";

	public const string OpMTHI_CustomsClearance = "B";

	public const string OpMTHI_VATCorrection = "F";

	public const string IDPassport = "1";

	public const string IDBirthCertificate = "2";

	public const string IDResidenceCertificate = "3";

	public const string IDTravelDocument = "4";

	public const string IDIdentificationCard = "5";

	public const string IDBiometricPassport = "6";

	public const string IDRussianPassport = "7";

	public const string PrimeCostNCCWithoutVAT = "S1";

	public const string PrimeCostNCCWithVAT = "S2";

	public const string PrimeCostCurr = "S3";

	public const string PurchasePrice = "S4";

	public const string SalePrice = "S5";

	public const string RetailAvailabilityPrice = "S6";

	public const string DiscountPrice = "S7";

	public const int LenEMarkReceiptCode = 3;

	public const int LenEMarkReceiptNumber = 20;

	public const string PriceListTypeDynamicColPrefix = "PT_";

	public const string PriceListSumDynamicColPrefix = "PS_";

	public const string PriceListDateDynamicColPrefix = "PD_";

	public const string DelimiterForTerminalExport = ";";

	public const string InvoiceSeries = "Ա";

	public const string AccDocSeries = "Բ";

	public const string AccDocSeriesNew = "B";

	public const string TaxServiceURLAddress = "http://www.taxservice.am/tp3/invoice/definitions";

	public const short LenCarMake = 256;

	public const short LenCarPlateNumber = 20;

	public const double GridGoodCodeColWidth = 125.0;

	public const decimal InvoiceAgreeTotalDiffSum = 100m;

	public const int MinDocOrderNumber = 10;

	public const string SwitchToAltUnitPrefix = "/";

	public const string Interrogation = "?";

	public const string ZeroWithTab = "0\t";

	public const string DecimalZeroWithTab = "0,0\t";

	public const string OneWithTab = "1\t";

	public const string Semicolon = ";";

	public const string ZeroWithSemicolon = "0;";

	public const string DefaultUnitCode = "001";

	public const string ASPrefix = "AS_";

	public const string DraftOrProcessedState = "0";

	public const short LenMTDescr = 255;

	public const string PartnerEmployeeCardCodePrefixDelimited = "||";

	public const char PartnerEmployeeCardCodeSeparator = '|';

	public const int PartnerEmployeeCardCodeLength = 7;

	public const int MinPartnerEmployeeCardCodeLength = 1;

	public const int MaxPartnerEmployeeCardCodeLength = 50;

	public const string OpENTRIES_Miscellaneous = "MSC";

	public const string OpENTRIES_Revaluation = "RVL";

	public const string OpENTRIES_MTBill = "MTB";

	public const string OpENTRIES_MTBill_Cash = "BCA";

	public const string OpENTRIES_MTBill_Bank = "BPL";

	public const string OpENTRIES_MTBill_PrePay = "BPP";

	public const string OpENTRIES_MTBill_Bonus = "BBO";

	public const string ErrEntriesEngineLbl = "AccEngine";

	public const string MTFavoritesHotKeyPattern = "^(?!(Ctrl\\+0|Shift\\+((NumPad)[0-9])))(Ctrl\\+|Shift\\+){0,1}((NumPad){0,1}[0-9]|F([2356789]|11|12)|[A-Z])$";

	public const string MTFavoriteCardTemplate = "MTFavoriteCardTemplate";

	public const string GoodsCardPrintTemplate = "GoodsCardPrintTemplate";

	public const string GoodsCardHeaderTemplate = "GoodsCardHeaderTemplate";

	public const int SequenceDefaultLength = 100;

	public const string CostWithVAT = "1";

	public const string CostWithInVAT = "2";

	public const string CostWithoutVAT = "3";

	public const string CostZeroVAT = "4";

	public const string CostWithInVATRetail = "5";

	public const string fCostWithVAT = "1";

	public const string fCostWithInVAT = "5";

	public const string fCostWithoutVAT = "3";

	public const string fCostZeroVAT = "4";

	public const string nCostWithOrWithInVAT = "1";

	public const string nCostWithoutVAT = "2";

	public const string nCostZeroVAT = "3";

	public const string MTExpMethodIdentFIFO = "1";

	public const string MTExpMethodIdentLIFO = "2";

	public const string MTExpMethodAverage = "3";

	public const double LenBetweenDateAndDocNum = 10.0;

	public const string BodyIDForCashDeskCode = "Cash";

	public const string BodyIDForCashOutDeskCode = "CashOut";

	public const string BodyIDForCashInDeskCode = "CashIn";

	public const string BodyIDForAccBankCode = "AccBankCode";

	public const string BodyIDForSummaReceived = "SummaReceived";

	public const string BodyIDForSummaPlCart = "SummaPlCart";

	public const string BodyIDForSummaCredit = "SummaCredit";

	public const string BodyIDForSummaPrePayment = "SummaPrePayment";

	public const string BodyIDForSummaVAT = "SummaVAT";

	public const string BodyIDForECRReturnCheckNum = "ECRReturnCheckNum";

	public const string BodyIDForSummaNCC = "SummaNCC";

	public const string BodyIDForOperType = "OperType";

	public const string BodyIDForECRBaseCheckNum = "ECRBaseCheckNum";

	public const string BodyIDForBankAcquiringContract = "BankAcquiringContract";

	public const string BodyIDForSummaBonus = "SummaBonus";

	public const string BodyIDForPaymentId = "QRPaymentId";

	public const string BodyIDForPaymentAccepted = "QRPaymentAccepted";

	public const string BodyIDForBuyerBusinessAddress = "BuyBusAddress";

	public const string BodyIDForIncludeVATInCost = "IncludeVATInCost";

	public const string POSTerminalAddress = "POSTerminalAddress";

	public const string BodyIdForPOSCRN = "POSCRN";

	public const string BodyIDForToken = "Token";

	public const string BodyIDForRefreshToken = "RefreshToken";

	public const string BodyIDForEMarkReceiptNumber = "EMarkReceiptNumber";

	public const string BodyIDForMaxRowid = "MaxRowID";

	public const string BodyIDForContract = "Contract";

	public const string BodyIdForReceiverLegalStatus = "RECEIVERLEGALSTATUS";

	public const int MTMoveDocIncomeRowsStep = 15000;

	public const string DiscountDefaultCode = "001";

	public const string Percent = "%";

	public const decimal MinExtraPercent = -99.99m;

	public const decimal MaxExtraPercent = 999.99m;

	public const short LenComplectQuantityPrecision = 6;

	public const short LenComplectSummPartPrecision = 3;

	public const decimal MinDiscountPercent = -999.99m;

	public const decimal MaxDiscountPercent = 100m;

	public const short LenDiscountPercent = 6;

	public const short LenCustomPercent = 5;

	public const short LenVATType = 1;

	public const int TouchModeButtonHeight = 40;

	public const int TouchModeRowHeight = 40;

	public const string SubTotalByGroupTreeView = "4";

	public const int LenCompanyName = 50;

	public const int LenCompanyAddress = 100;

	public const short LenScaleCode = 3;

	public const short LenScaleType = 1;

	public const short LenScaleModel = 2;

	public const short LenNameInterpretation = 1;

	public const short LenExportElScaleNext = 1;

	public const short DocNotExist = 1;

	public const short LenPLUCodeInCas = 6;

	public const short LenPLUCodeInTMA = 5;

	public const short LenPLUCodeInMassaK = 8;

	public const short LenPLUCodeInDigi = 8;

	public const short LenPLUCodeInMertech = 8;

	public const string InvFormPrincipal = "4";

	public const int MinDiscountServerSyncInterval = 5;

	public const int MaxDiscountServerSyncInterval = 999999999;

	public const int MinBarCodePrefix = 20;

	public const int MaxBarCodePrefix = 29;

	public const int MinElScaleItemCodeLength = 5;

	public const int MaxElScaleItemCodeLength = 7;

	public const int MinElScaleItemPrecision = 0;

	public const int MaxElScaleItemPrecision = 3;

	public const int InternalBarcodeNextLength = 10;
}
