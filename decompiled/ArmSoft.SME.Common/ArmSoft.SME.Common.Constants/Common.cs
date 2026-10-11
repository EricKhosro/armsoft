using System;
using System.Runtime.InteropServices;
using ArmSoft.SME.Common.Parameters;

namespace ArmSoft.SME.Common.Constants;

public static class Common
{
	public enum BankCodeAM : short
	{
		AEB = 163,
		ACBA = 220,
		HSBC = 217,
		BP = 9
	}

	public enum Classifiers : byte
	{
		CNFEA = 1,
		CPA = 2,
		All = 4
	}

	public enum B2bDocStates : short
	{
		Proposed = 10,
		Pending = 20,
		Sent = 30,
		Confirmed = 40,
		Rejected = 50,
		Undefined = 90
	}

	public enum Partnerb2bStatus : byte
	{
		Active = 0,
		NewRegistered = 1,
		TmpInactive = 2,
		Liquidated = 3,
		Liquidating = 4,
		LiquidatedAffiliated = 5,
		LiquidatedMerged = 6,
		Bankrupt = 7,
		Dead = 8,
		ProvidedByMistake = 9,
		ExclusionFromRegister = 10,
		Reorganized = 11,
		SpinOff = 12,
		Bankrupting = 13,
		WithdrawnFromAccountingHMK = 14,
		NotFound = 254,
		NotExist = byte.MaxValue
	}

	public enum NonCashPaymentType : short
	{
		None = 0,
		CardPayment = 11,
		ARCA = 1,
		VISA = 2,
		MASTER = 3,
		AMEX = 4,
		MIR = 10,
		PayX = 5,
		Idram = 6,
		InecoPay = 7,
		EasyPay = 8,
		Telcell = 9
	}

	public enum NonCashPaymentTypeForSaleWindow : short
	{
		None = 0,
		BankCardWithNoPOS = 10,
		BankCardWithECRPOS = 11,
		BankCardWithIngenicoPOS = 12,
		BankCardWithFeitianPOS = 13
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ECRPaymentTypes
	{
		public const string None = "0";

		public const string CardPayment = "1";

		public const string Telcell = "10";

		public const string EasyPay = "11";

		public const string MobiDram = "12";

		public const string IDram = "13";

		public const string StakProcessing = "14";

		public const string FastShift = "15";

		public const string TeamPay = "16";

		public const string PayX = "17";

		public const string InecoPay = "18";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct NonCashPaymentStringTypes
	{
		public const string PayX = "PayX";

		public const string Idram = "Idram";

		public const string EasyPay = "EasyPay";

		public const string InecoPay = "InecoPay";

		public const string Telcell = "Telcell";
	}

	public enum KeyApplication : byte
	{
		Accountant = 1,
		BookKeeper = 2,
		Trade = 3,
		MarketPlace = 5,
		SalesManagment = 6
	}

	[Flags]
	public enum DocumentDescriptionMode
	{
		TypeNum = 0,
		TypeDateNum = 1,
		DateNum = 2,
		ChangeDayOrderNumber = TypeDateNum | DateNum,
		ChangeActualnessDay = 4
	}

	public const short LockTimeout = 30000;

	public const string SourceContext = "SourceContext";

	public const string ContextFunctions = "ContextFunctions";

	public const int ProgressCodeMaxCount = 25;

	public const int ExtendLifetime = 10;

	public const int ExtendPOLifetime = 5;

	public const int TileSizeLarge = 298;

	public const int TileSizeSmall = 144;

	public const short LenTimeStamp = 8;

	public const int LongQueryTimeout = 3600;

	public const int ReportQueryTimeout = 300;

	public const int QueryTimeout = 30;

	public const short LenDirectoryCodeMax = 50;

	public const short LenDirectoryNameMax = 500;

	public const string VBCrLf = "\r\n";

	public const string VBTab = "\t";

	public const short UserAWPSubSystemId = 100;

	public const short LenDateFormatWithMilliseconds = 23;

	public const short SqlDateISOFormat = 112;

	public const short SQlDateISOFormatWithMilliseconds = 121;

	public const short LenLogOp = 4;

	public const short LenDocState = 2;

	public const short LenUserId = 7;

	public const short LenParamId = 5;

	public const short LenOnlineReportParamCode = 30;

	public const short LenUserName = 50;

	public const string LockKeyword = "#Lock#";

	public const string UnDefinedValueForKeyOfInnerDirectories = "???";

	public const short LenMaxUsrPasswd = 16;

	public const short LenServerName = 32;

	public const short LenDBName = 20;

	public const short LenFullCaption = 255;

	public const short LenCaption = 50;

	public const short LenDoubleCaption = 100;

	public const short LenMTCaption = 200;

	public const short LenItemDescription = 255;

	public const short LenPost = 50;

	public const string Exchange = "Exchange";

	public const string AdminUserName = "ADMIN";

	public const int ASAdminUserId = 27335;

	public const string ASAdminUserName = "ArmSoft Admin";

	public const string TestServiceAddressStart = "https://smetest.armsoft.am";

	public const string MasterDB = "master";

	public const short LenCurrency = 3;

	public const short LenCurrUnitCentName = 10;

	public const string Unknown = "Unknown";

	public const short LenIBANCODE = 2;

	public const short LenSWIFTCODE = 11;

	public const short LenTemplateName = 20;

	public const short LenTemplateCaption = 50;

	public const short LenTemplateFile = 512;

	public const string XmlNS = "http://www.armsoft.am/Accountant/6.0";

	public const short LenExchangeRate = 11;

	public const short LenExchangeRatePrecision = 4;

	public const short LenExchangeBase = 5;

	public const short LenCompName = 32;

	public const short LenAttachDescription = 50;

	public const short LenAttachLink = 256;

	public const short SpinControlMaxValue = 99;

	public const int BestFitMaxRowCount = 50;

	public const short LenComment = 50;

	public const short LenLongComment = 255;

	public const short LenResourceName = 50;

	public const short LenResourceCulture = 6;

	public const short LenResourceValue = 500;

	public const short LenString = 512;

	public const short LenDialogName = 30;

	public const short LenDialogTemplateCaption = 100;

	public const short LenDataViewName = 30;

	public const short LenDataViewGroup = 30;

	public const short LenUserReportName = 30;

	public const short LenAWPName = 20;

	public const short LenContextFunctionDefName = 20;

	public const short LenUnboundColumnName = 20;

	public const short LenDataProviderName = 50;

	public const short DialogDataViewNameCodeWidth = 10;

	public const short AppKeyCodeLength = 8;

	public const short LenUserScriptNameLength = 32;

	public const short LenUserScriptCaptionLength = 150;

	public const short LenColor = 9;

	public const short LenUserCardNumber = 50;

	public const short LenDocType = 2;

	public const short LenDirType = 2;

	public const short LenIBFieldLength = 32;

	public const short LenCoeff = 3;

	public const short LenCoeffForMTReval = 8;

	public const short LenCoeffPrecision = 4;

	public const int LenFieldSelectionMethod = 1;

	public const int LenLegilativeParamSelectionMethod = 3;

	public const short LenHotKey = 15;

	public const int LenStatisticType = 20;

	public const int LenStatisticKey = 100;

	public const int MinDurationInSForRecord = 10;

	public const int MinDurationForDocInSForRecord = 5;

	public const int MinAutoSaveInterval = 1;

	public const int MaxAutoSaveInterval = 60;

	public const short TimeToLiveMsBoxInSeconds = 60;

	public const short LenAddress = 255;

	public const short LenEmail = 129;

	public const short LenPhone = 16;

	public const short LenUserRole = 3;

	public const double ImageWidthm = 31.5;

	public const double ImageHeightm = 31.5;

	public const double ImageResizeCoef = 31.5;

	public const short WeekLong = 7;

	public const short MonthLong = 30;

	public const short Quarter = 90;

	public const short YearLong = 365;

	public const string NCC = "AMD";

	public const short ImageWidthp = 119;

	public const short ImageHeightp = 119;

	public const string BaseUri = "pack://application:,,,/ArmSoft.SME.Core.UI;component/Images/";

	public const string AppUri = "pack://application:,,,/Images/";

	public const string PathDelimiter = "!";

	public static readonly DateTime minOPDate = new DateTime(1951, 1, 1);

	public static readonly DateTime maxOPDate = new DateTime(2050, 1, 1);

	public static readonly DateOnly MinOPDateOnly = new DateOnly(1951, 1, 1);

	public static readonly DateOnly MaxOPDateOnly = new DateOnly(2079, 1, 1);

	public static readonly DateTime B2BDateNothing = new DateTime(1900, 1, 1);

	public static readonly DateOnly DefaultDate = new DateOnly(2001, 1, 1);

	public const int LenHour = 6;

	public const int LenDay = 4;

	public const int LenMonth = 2;

	public const string CollateCondition = " COLLATE Latin1_General_BIN ";

	public const string MaleCode = "1";

	public const string FemaleCode = "2";

	public const string CaptionCode = "1";

	public const string FullCaptionCode = "2";

	public const string Culture_EN = "EN-US";

	public const string Culture_AM = "HY-AM";

	public const string Culture_GE = "KA-GE";

	public const string Culture_RU = "RU-RU";

	public const string Culture_AR = "AR-SY";

	public const string TeamViewer = "TeamViewer";

	public const string AnyDesk = "AnyDesk";

	public const string Telegram = "Telegram";

	public const string householdandgroceryCode = "1";

	public const string medicineCode = "2";

	public const string OSMTileUriTemplate = "https://maps.armsoft.am/tile/{tileLevel}/{tileX}/{tileY}.png";

	public const string OpenStreetMapUri = "https://maps.armsoft.am/";

	public const string MapWebRequestCoreUserAgent = "core.armsoft.am";

	public const string DateTimeFormat = "dd/MM/yy HH:mm:ss";

	public const string DateTimeFormatWithoutSeconds = "dd/MM/yy HH:mm";

	public const int ByteToMBConvertRate = 1048576;

	public const int LenDocumentState = 2;

	public const short LenBank = 5;

	public const short LenBankMain = 3;

	public const string ArtsakhTINCode = "9";

	public const char SpaceChar = ' ';

	public const short DropDownTypeLen = 10;

	public const short LenTreeName = 8;

	public const string TreeNamePrefix = "Tree_";

	public const short LenTreeCaption = 150;

	public const short LenTreeCode = 20;

	public const short LenTreePath = 255;

	public const short LenTreeSpec = 255;

	public const short LenTreeBody = 3000;

	public const string TreeName = "Tree";

	public const string MyTreeViewItemName = "MyTreeViewItem";

	public const short LenCPAClassifier = 12;

	public const short LenCPAClassifierBase = 4;

	public static readonly DateOnly MinDateForCPACodes = new DateOnly(2024, 1, 1);

	public const short LenMaxUserName = 50;

	public const short LenLayoutName = 50;

	public const short LenLayoutCaption = 50;

	public const short LenGeoCoordLength = 10;

	public const short LenGeoCoordPrecision = 6;

	public const double YerevanHeartLatitude = 40.177624;

	public const double YerevanHeartLongitude = 44.512646;

	public const double TbilisiHeartLatitude = 41.693443;

	public const double TbilisiHeartLongitude = 44.801499;

	public const short StartUserAWPNavigationId = 5000;

	public const string Image = "Image";

	public const string UserAWPNavigationName = "UserAPW";

	public const short LenAssemblyName = 50;

	public const short LenAppKeyVersion = 20;

	public const string b2bArmSoftServiceBaseAddress = "https://services.armsoft.am:4443/";

	public const string b2bSite = "https://b2b.armsoft.am";

	public const short LenExactTaxCode = 8;

	public const short LenTaxAreaCode = 2;

	public const short Lenb2bProduct = 20;

	public const short Lenb2bProductName = 50;

	public const short Lenb2bProductFullName = 250;

	public const short Lenb2bDescription = 4000;

	public const short Lenb2bCPACode = 8;

	public const short Lenb2bCategoryCode = 50;

	public const short Lenb2bCategoryCaption = 100;

	public const short Lenb2bCategoryComment = 100;

	public const string b2bCategoryPathDelimiter = "/";

	public const string ArmenianPhoneMask = "+374(00)00-00-00";

	public const string IPAddressMask = "([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}";

	public const string MacAddressMask = "([0-9A-Fa-f]{2}-){5}[0-9A-Fa-f]{2}";

	public const string TCPPortMask = "([1-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])";

	public const string IPAddressWithPortMask = "([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}(:([0-9]|[1-9][0-9]{1,3}|[1-5][0-9]{4}|6([0-4][0-9]{3}|5([0-4][0-9]{2}|5([0-2][0-9]|3[0-5])))))";

	public const short LenBarCode = 20;

	public const short LenTaxCode = 20;

	public const short LenSocCode = 11;

	public const short LenContract = 3;

	public const short LenSettlAcc = 22;

	public const short LenTaxDocumentSettlAcc = 34;

	public const short LenIBSettlAcc = 50;

	public const short LenBankAcc = 8;

	public const short LenExternalCode = 60;

	public const short LenItemCode = 20;

	public const short LenItemType = 1;

	public const short LenTaxInvoiceType = 1;

	public const short LenExpType = 1;

	public const short LenTaxDocumentSerial = 1;

	public const short LenTaxDocumentNumber = 10;

	public const short LenPurchasedDocN = 20;

	public const short LenTaxDocumentDocStatus = 2;

	public const short LenTaxDocumentName = 512;

	public const short LenTaxDocumentComment = 256;

	public const short TaxVATRate20 = 20;

	public const short LenSupBookN = 10;

	public const short LenSupBookPageN = 6;

	public const short LenSupBookRowN = 6;

	public const short LenCustomerPassport = 50;

	public const short LenPartPassport = 32;

	public const short LenShowComm = 50;

	public const short LenShortComment = 32;

	public const short LenDocNum = 12;

	public const short LenGuid = 36;

	public const short LenQuantity = 13;

	public const short LenQuantityPrecision = 3;

	public const short LenPercent = 6;

	public const short LenPercentPrecision = 2;

	public const short LenSum = 16;

	public const short LenSumPrecision = 2;

	public const short LenPrice = 16;

	public const short LenPricePrecision = 2;

	public const short LenFieldType = 15;

	public const short LenMaxPriceString = 19;

	public const short LenSumIntegerPart = 14;

	public const short MaxLenPricePrecision = 4;

	public const short MaxLenPrice = 18;

	public const short MaxLenSumPrecision = 4;

	public const short MaxLenSum = 18;

	public const short LenPoint = 3;

	public const short LenPriceForTax = 18;

	public const short LenPricePrecisionForTax = 5;

	public const short LenSumPrecisionForTax = 2;

	public const short LenPercentForTax = 7;

	public const short LenPercentPrecisionForTax = 4;

	public const short LenQuantityForTax = 17;

	public const short LenQuantityPrecisionForTax = 4;

	public const short LenExcisePrecisionForTax = 3;

	public const short LenTaxesPrecisionForTax = 2;

	public const double ClickDropDownLenCoefficient = 9.5;

	public const double StringLenCoefficient = 11.0;

	public const double LongStringLenCoefficient = 4.5;

	public const double LongStringLenTimesOnePointTwo = 1.2;

	public const double LongStringLenTimesOnePointThree = 1.3;

	public const double LongStringLenTimesOnePointSix = 1.6;

	public const double LongStringLenTimesFour = 4.0;

	public const double ShortStringLenCoefficient = 2.5;

	public const double NumberLenCoefficient = 9.0;

	public const short LenMeasureUnit = 6;

	public const short LenOverHeadExpCoeff = 7;

	public const string ArmSoftTIN = "00807555";

	public const int LenMaximumArrayQuotaB2B = 1000000;

	public const int LenHashSize = 64;

	public const int LenUserPIN = 4;

	public const decimal CashSummLimit = 300000m;

	public const short LenEmployeeCode = 12;

	public const short LenDepartmentCode = 3;

	public const short LenDepartmentName = 255;

	public const short MerchantIdLength = 100;

	public const short TokenLength = 250;

	public const short NonCashPaymentIdLength = 100;

	public const short POSTerminalIdLength = 12;

	public const string ECREmptyNum = "XXXXXXXX";

	public const int ECRCheckNumLen = 8;

	public const int ECRPrintDelay = 180;

	public const int LenECRCRN = 12;

	public const short LenECRDeviceNumber = 20;

	public const short LenIP4Address = 15;

	public const short LenIP4WithPort = 21;

	public const short LenTCPPortNumber = 5;

	public const short LenMacAddress = 17;

	public const short LenECRPasswordField = 255;

	public const short LenECRPassword = 20;

	public const short LenECRDepartment = 20;

	public const short LenECROperator = 12;

	public const short LenECROperatorPasswordField = 255;

	public const short LenECROperatorPassword = 16;

	public const short LenMark = 200;

	public const short LenEMarkMax = 110;

	public const short LenEMarkMin = 29;

	public const short LenInternalBarcodeEAN = 10;

	public const short LenInternalBarcodeMin = 4;

	public const short LenIBComment = 140;

	public const short LenIBCorrName = 140;

	public const short LenIBDocId = 20;

	public const short LenIBDocNum = 6;

	public const short LenIBOpCode = 5;

	public const short LenIBPassport = 100;

	public const short LenIBAddress = 33;

	public const short UserParamImageDefaultActualHeight = 20;

	public const string SqliteDBExtension = ".db3";

	public const string ZipExtension = ".zip";

	public const string ApiKey = "apiKey";

	public const string Token = "token";

	public const string AcceptLanguage = "Accept-Language";

	public const int MinDay = 1;

	public const int MaxDay = 31;

	public const int MinMonth = 1;

	public const int MaxMonth = 12;

	public const int AutoBackupMinCount = 1;

	public const int DailyAutoBackupMaxCount = 45;

	public const int MonthlyAutoBackupMaxCount = 15;

	public const int AnnualAutoBackupMaxCount = 5;

	public const int DailyBackupDefaultCount = 35;

	public const int MonthlyBackupDefaultCount = 11;

	public const int AnnualBackupDefaultCount = 5;

	public const int MonthlyBackupDefaultDay = 1;

	public const int AnnualBackupDefaultDay = 15;

	public const int AnnualBackupDefaultMonth = 1;

	public const short DemoModeDocsCount = 200;

	public const short DefaultNotificationsCheckInterval = 300;

	public const short NotificationsCheckIntervalMinValue = 60;

	public const short NotificationsCheckIntervalMaxValue = 900;

	public const short NotificationsCheckIntervalIncrement = 30;

	public static short LenSumFromParam(IParametersService parametersService)
	{
		return Convert.ToInt16(14 + parametersService.SumPrecisionDigitsCount());
	}

	public static short LenSumPrecisionFromParam(IParametersService parametersService)
	{
		return (short)parametersService.SumPrecisionDigitsCount();
	}

	public static short LenPriceFromParam(IParametersService parametersService)
	{
		return Convert.ToInt16(14 + parametersService.SumPrecisionDigitsCount());
	}

	public static short LenPricePrecisionFromParam(IParametersService parametersService)
	{
		return (short)parametersService.SumPrecisionDigitsCount();
	}
}
