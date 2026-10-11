using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArmSoft.ECRConnector.Helpers;

namespace ArmSoft.ECRConnector;

public static class Utilities
{
	public class ECRException : Exception
	{
		public int ErrCode { get; set; }

		public ECRException(int errCode)
			: base(ErrorHelper.GetErrorMessage(errCode))
		{
			ErrCode = errCode;
		}

		public ECRException(string message, int errCode = -1)
			: base(message)
		{
			ErrCode = errCode;
		}
	}

	public enum ErrMessageLanguage : short
	{
		Armenian = 1,
		English
	}

	public enum PrintMode
	{
		Simple = 1,
		Product,
		PrePayment
	}

	public enum ProductDiscountType
	{
		Percent = 1,
		Price = 2,
		Total = 4
	}

	public enum ProductAdditionalDiscountType
	{
		Percent = 8,
		Price = 0x10
	}

	public enum FiscalReportTypes
	{
		X = 1,
		Z
	}

	public enum FiscalReportTransactionTypes
	{
		ByCash = 1,
		ByCard
	}

	public enum PaymentSystemTypes
	{
		CardPayment = 1,
		Telcel = 10,
		EasyPay = 11,
		MobiDram = 12,
		IDram = 13,
		StakProcessing = 14,
		FastShift = 15,
		TeamPay = 16,
		PayX = 17,
		InecoPay = 18
	}

	public enum DepartmentTypes : short
	{
		WithVAT = 1,
		WithOutVAT,
		TurnOver,
		Patent,
		Fixed,
		Family,
		Micro
	}

	[DataContract]
	public class OperatorLoginRequest
	{
		[DataMember(Order = 1)]
		public string password { get; set; } = "";

		[DataMember(Order = 2)]
		public int cashier { get; set; } = -1;

		[DataMember(Order = 3)]
		public string pin { get; set; } = "";
	}

	[DataContract]
	public class ResponseBase
	{
		[DataMember]
		public int status { get; set; } = -1;
	}

	[DataContract]
	public class OperatorLoginResponse : CommonResponse
	{
		[DataMember]
		public string key { get; set; } = "";

		public byte[] GetKeyBytes()
		{
			try
			{
				return Convert.FromBase64String(key);
			}
			catch (Exception ex)
			{
				throw new ECRException(ex.Message);
			}
		}
	}

	[DataContract]
	public class SessionRequest
	{
		[DataMember]
		public int seq { get; set; } = -1;
	}

	[DataContract]
	public class OperatorLogoutRequest : SessionRequest
	{
	}

	[DataContract]
	public class OperatorLogoutResponse
	{
	}

	[DataContract]
	public class GetOperatorsAndDepsResponse : CommonResponse
	{
		[DataMember(Order = 1)]
		public List<Cashier> c { get; set; }

		[DataMember(Order = 2)]
		public List<Department> d { get; set; }
	}

	[DataContract]
	public class Department
	{
		[DataMember(Order = 1)]
		public int id { get; set; } = -1;

		[DataMember(Order = 2)]
		public string name { get; set; } = "";

		[DataMember(Order = 3)]
		public int type { get; set; } = -1;

		[IgnoreDataMember]
		public string DepartmentType => type switch
		{
			1 => "ԱԱՀ-ով հարկվող", 
			2 => "ԱԱՀ-ով չհարկվող", 
			3 => "Շրջ. հարկ", 
			4 => "Արտոն. վճար", 
			5 => "Հաստատգ. վճար", 
			6 => "Ընտանեկան ձեռն.", 
			7 => "Միկրո․ ձեռն.", 
			_ => type.ToString(), 
		};
	}

	[DataContract]
	public class Cashier
	{
		[DataMember(Order = 1)]
		public int id { get; set; } = -1;

		[DataMember(Order = 2)]
		public string name { get; set; } = "";

		[DataMember(Order = 3)]
		public int[] deps { get; set; }
	}

	[DataContract]
	public class GetOperatorsAndDepsRequest
	{
		[DataMember]
		public string password { get; set; } = "";
	}

	[DataContract]
	public class CommonResponse
	{
		[DataMember]
		public int responseCode { get; set; } = -1;
	}

	[DataContract]
	public class PrintReceiptResponse : CommonResponse
	{
		[DataMember(Order = 1)]
		public int rseq { get; set; }

		[DataMember(Order = 2)]
		public string crn { get; set; } = "";

		[DataMember(Order = 3)]
		public string sn { get; set; } = "";

		[DataMember(Order = 4)]
		public string tin { get; set; } = "";

		[DataMember(Order = 5)]
		public string taxpayer { get; set; } = "";

		[DataMember(Order = 6)]
		public string address { get; set; } = "";

		[DataMember(Order = 7)]
		public double time { get; set; }

		[DataMember(Order = 8)]
		public string fiscal { get; set; } = "";

		[DataMember(Order = 9)]
		public string lottery { get; set; } = "";

		[DataMember(Order = 10)]
		public int prize { get; set; }

		[DataMember(Order = 11)]
		public double total { get; set; }

		[DataMember(Order = 12)]
		public double change { get; set; }

		[DataMember(Order = 13)]
		public string qr { get; set; } = "";

		[DataMember(Order = 14)]
		public string emarksCount { get; set; } = "";

		[DataMember(Order = 15)]
		public string verificationNumber { get; set; } = "";
	}

	[DataContract]
	public class PrintReceiptRequest : SessionRequest
	{
		public static readonly int PRINT_MODE_SIMPLE = 1;

		public static readonly int PRINT_MODE_PRODUCT = 2;

		public static readonly int PRINT_MODE_PREPAYMENT = 3;

		[DataMember(Order = 1)]
		public double paidAmount { get; set; }

		[DataMember(Order = 2)]
		public double paidAmountCard { get; set; }

		[DataMember(Order = 3)]
		public double partialAmount { get; set; }

		[DataMember(Order = 4)]
		public double prePaymentAmount { get; set; }

		[DataMember(Order = 5)]
		public required int mode { get; set; }

		[DataMember(Order = 6)]
		public string partnerTin { get; set; }

		[DataMember(Order = 7, EmitDefaultValue = false)]
		public int dep { get; set; }

		[DataMember(Order = 8)]
		public bool useExtPOS { get; set; } = true;

		[DataMember(Order = 9)]
		public int? PaymentSystem { get; set; }

		[DataMember(Order = 10, EmitDefaultValue = false)]
		public string rrn { get; set; }

		[DataMember(Order = 11, EmitDefaultValue = false)]
		public string terminalId { get; set; }

		[DataMember(Order = 12)]
		public List<string> eMarks { get; set; } = new List<string>();

		[DataMember(Order = 13)]
		public List<Item> items { get; set; } = new List<Item>();

		public void addItem(Item item)
		{
			items.Add(item);
		}

		public void addEMark(string item)
		{
			eMarks.Add(item);
		}
	}

	[DataContract]
	public class PrintLastReceiptRequest : SessionRequest
	{
	}

	[DataContract]
	public class PrintReturnReceiptRequest : SessionRequest
	{
		[DataMember(Order = 1)]
		public string crn { get; set; } = "";

		[DataMember(Order = 2)]
		public int returnTicketId { get; set; } = -1;

		[DataMember(Order = 3)]
		public decimal cashAmountForReturn { get; set; }

		[DataMember(Order = 4)]
		public decimal cardAmountForReturn { get; set; }

		[DataMember(Order = 5)]
		public decimal prePaymentAmountForReturn { get; set; }

		[DataMember(Order = 6, EmitDefaultValue = false)]
		public string rrn { get; set; }

		[DataMember(Order = 7, EmitDefaultValue = false)]
		public string terminalId { get; set; }

		[DataMember(Order = 8)]
		public List<string> eMarks { get; set; } = new List<string>();

		[DataMember(Order = 9)]
		public List<ReturnItem> returnItemList { get; set; } = new List<ReturnItem>();

		public void addReturnItem(ReturnItem returnItem)
		{
			returnItemList.Add(returnItem);
		}

		public void addEMark(string item)
		{
			eMarks.Add(item);
		}
	}

	[DataContract]
	public class ReturnItem
	{
		[DataMember(Order = 1)]
		public long rpid { get; set; } = -1L;

		[DataMember(Order = 2)]
		public double quantity { get; set; }
	}

	[DataContract]
	public class PrintFiscalReportRequest : SessionRequest
	{
		public static readonly int PRINT_FISCAL_REPORT_X = 1;

		public static readonly int PRINT_FISCAL_REPORT_Z = 2;

		[DataMember(Order = 1)]
		public int reportType { get; set; } = 1;

		[DataMember(Order = 2, EmitDefaultValue = false)]
		public int deptId { get; set; }

		[DataMember(Order = 3, EmitDefaultValue = false)]
		public int cashierId { get; set; }

		[DataMember(Order = 4, EmitDefaultValue = false)]
		public int transactionTypeId { get; set; }

		[DataMember(Order = 5)]
		public long startDate { get; set; }

		[DataMember(Order = 6)]
		public long endDate { get; set; }
	}

	[DataContract]
	public class PrintCashInOutRequest : SessionRequest
	{
		[DataMember(Order = 1)]
		public double amount { get; set; }

		[DataMember(Order = 2)]
		public bool isCashIn { get; set; }

		[DataMember(Order = 3)]
		public int cashierId { get; set; }

		[DataMember(Order = 4)]
		public string description { get; set; }
	}

	[DataContract]
	public class GetReceiptRequest : SessionRequest
	{
		[DataMember(Order = 1)]
		public string receiptId { get; set; } = "";

		[DataMember(Order = 2)]
		public string crn { get; set; } = "";
	}

	[DataContract]
	public class SyncRequest : SessionRequest
	{
	}

	[DataContract]
	public class EMarkCheckRequest : SessionRequest
	{
		[DataMember(Order = 1)]
		public string eMark { get; set; } = "";
	}

	[DataContract]
	public class GetReceiptResponse : CommonResponse
	{
		[DataMember(Order = 1)]
		public string cid { get; set; } = "";

		[DataMember(Order = 2)]
		public string time { get; set; } = "";

		[DataMember(Order = 3)]
		public string ta { get; set; } = "";

		[DataMember(Order = 4)]
		public string cash { get; set; } = "";

		[DataMember(Order = 5)]
		public string card { get; set; } = "";

		[DataMember(Order = 6)]
		public string ppa { get; set; } = "";

		[DataMember(Order = 7)]
		public string ppu { get; set; } = "";

		[DataMember(Order = 8)]
		public string lot { get; set; } = "";

		[DataMember(Order = 9)]
		public string type { get; set; } = "";

		[DataMember(Order = 10)]
		public string @ref { get; set; } = "";

		[DataMember(Order = 11)]
		public string refcrn { get; set; } = "";

		[DataMember(Order = 12)]
		public string saleType { get; set; } = "";

		[DataMember(Order = 13)]
		public string rrt { get; set; } = "";

		[DataMember(Order = 14)]
		public int rseq { get; set; }

		[DataMember(Order = 15)]
		public string rTin { get; set; } = "";

		[DataMember(Order = 16)]
		public string fiscal { get; set; } = "";

		[DataMember(Order = 17)]
		public List<SubTotal> totals { get; set; }

		[DataMember(Order = 18)]
		public int subType { get; set; }

		[DataMember(Order = 19)]
		public int? did { get; set; }

		[DataMember(Order = 20)]
		public string pTin { get; set; }

		[DataMember(Order = 21)]
		public List<string> eMarks { get; set; }
	}

	[DataContract]
	public class SubTotal
	{
		[DataMember(Order = 1)]
		public string did { get; set; } = "";

		[DataMember(Order = 2)]
		public string dt { get; set; } = "";

		[DataMember(Order = 3)]
		public string dtm { get; set; } = "";

		[DataMember(Order = 4)]
		public string t { get; set; } = "";

		[DataMember(Order = 5)]
		public string tt { get; set; } = "";

		[DataMember(Order = 6)]
		public string id { get; set; } = "";

		[DataMember(Order = 7)]
		public string gc { get; set; } = "";

		[DataMember(Order = 8)]
		public string gn { get; set; } = "";

		[DataMember(Order = 9)]
		public string qty { get; set; } = "";

		[DataMember(Order = 10)]
		public string p { get; set; } = "";

		[DataMember(Order = 11)]
		public string adg { get; set; } = "";

		[DataMember(Order = 12)]
		public string mu { get; set; } = "";

		[DataMember(Order = 13)]
		public string dsc { get; set; } = "";

		[DataMember(Order = 14)]
		public string adsc { get; set; } = "";

		[DataMember(Order = 15)]
		public string dsct { get; set; } = "";

		[DataMember(Order = 16)]
		public string rpid { get; set; } = "";
	}

	[DataContract]
	public class HeaderFooter
	{
		public static readonly int ALIGNMENT_LEFT = 3;

		public static readonly int ALIGNMENT_CENTER = 1;

		public static readonly int ALIGNMENT_RIGHT = 2;

		public static readonly int HEADER_STYLE_NORMAL = 0;

		public static readonly int HEADER_STYLE_BOLD = 1;

		public static readonly int FONT_SIZE_TINY = 1;

		public static readonly int FONT_SIZE_SMALL = 2;

		public static readonly int FONT_SIZE_MEDIUM = 3;

		public static readonly int FONT_SIZE_LARGE = 4;

		public static readonly int FONT_SIZE_HUGE = 5;

		private const int FONT_SIZE_MULTIPLIER = 8;

		[DataMember(Order = 1)]
		public int align { get; set; }

		[DataMember(Order = 2)]
		public int bold { get; set; }

		[DataMember(Order = 3)]
		public int fsize { get; set; }

		[DataMember(Order = 4)]
		public string text { get; set; } = "";
	}

	[DataContract]
	public class SetupHeaderFooterRequest : SessionRequest
	{
		[DataMember(Order = 1)]
		public List<HeaderFooter> headers { get; set; } = new List<HeaderFooter>();

		[DataMember(Order = 2)]
		public List<HeaderFooter> footers { get; set; } = new List<HeaderFooter>();
	}

	[DataContract]
	public class SetupHeaderLogoRequest : SessionRequest
	{
		[DataMember(Order = 1)]
		public string headerLogo = "";
	}

	[DataContract]
	public class Item
	{
		public const short gLenName = 50;

		public const string ErrLenName = "{0} կոդով ապրանքի անվանումը գերազանցում է {1} նիշը";

		[DataMember(Order = 1)]
		public int dep { get; set; } = -1;

		[DataMember(Order = 2)]
		public decimal qty { get; set; }

		[DataMember(Order = 3)]
		public decimal price { get; set; }

		[DataMember(Order = 4)]
		public string productCode { get; set; } = "";

		[DataMember(Order = 5, EmitDefaultValue = false)]
		public decimal discount { get; set; }

		[DataMember(Order = 6, EmitDefaultValue = false)]
		public int discountType { get; set; }

		[DataMember(Order = 7, EmitDefaultValue = false)]
		public decimal additionalDiscount { get; set; }

		[DataMember(Order = 8, EmitDefaultValue = false)]
		public int additionalDiscountType { get; set; }

		[DataMember(Order = 9)]
		public string productName { get; set; } = "";

		[DataMember(Order = 10)]
		public string adgCode { get; set; } = "";

		[DataMember(Order = 11)]
		public string unit { get; set; } = "";
	}

	[DataContract]
	public class GetDateResponse
	{
		[DataMember(Order = 1)]
		public string dt { get; set; } = "";
	}

	[DataContract]
	public class GetPaymentSystemsResponse
	{
		[DataMember(Order = 1)]
		public List<PaymentSystemsItem> PaymentSystems { get; set; } = new List<PaymentSystemsItem>();
	}

	[DataContract]
	public class PaymentSystemsItem
	{
		[DataMember(Order = 1)]
		public int code { get; set; }

		[DataMember(Order = 2)]
		public string name { get; set; } = "";
	}

	public const int SUCCESS = 200;

	public const int INTERNAL_ERROR = 500;

	public const int ERROR_BAD_REQUEST = 400;

	public const int ERROR_WRONG_PROTOCOL_VERSION = 402;

	public const int ERROR_UNAUTHORIZED_CONNECTION = 403;

	public const int ERROR_BAD_OP_CODE = 404;

	public const int SIMPLE_RECEIPT_NOT_ALLOWED = 410;

	public const int LOGIN_ERROR_WRONG_PASS = 101;

	public const int ERROR_WRONG_SESSION = 102;

	public const int ERROR_WRONG_REQ_HEADER = 103;

	public const int ERROR_WRONG_RSEQ = 104;

	public const int ERROR_WRONG_JSON = 105;

	public const int LOGIN_ERROR_WRONG_USER = 111;

	public const int LOGIN_ERROR_NO_SUCH_USER = 112;

	public const int LOGIN_ERROR_INACTIVE_USER = 113;

	public const int ERROR_NO_USER = 121;

	public const int LAST_RECEIPT_NO_FILE = 141;

	public const int LAST_RECEIPT_ANOTHER_CASHIER = 142;

	public const int LAST_RECEIPT_PRN_FAIL = 143;

	public const int LAST_RECEIPT_PRN_INIT = 144;

	public const int LAST_RECEIPT_PRN_NO_PAPER = 145;

	public const int PRINT_RECEIPT_NO_DEPT = 151;

	public const int PRINT_RECEIPT_PAID_LESS = 152;

	public const int PRINT_RECEIPT_LIMIT = 153;

	public const int PRINT_RECEIPT_AMOUNT_POS = 154;

	public const int PRINT_RECEIPT_SYNC_NEEDED = 155;

	public const int PRINT_RECEIPT_SYNC_ERROR = 156;

	public const int RECEIPT_RETURN_INVALID_ID = 157;

	public const int RECEIPT_RETURN_ALREADY_RETURNED = 158;

	public const int RECEIPT_PRICE_QTY_NONZERO = 159;

	public const int RECEIPT_PRICE_INVALID_DISCOUNT_PERCENT = 160;

	public const int PRINT_RECEIPT_ERROR_EMPTY_CODE = 161;

	public const int PRINT_RECEIPT_ERROR_EMPTY_NAME = 162;

	public const int PRINT_RECEIPT_AMOUNT_CASH = 163;

	public const int PAYMENT_FAILED = 164;

	public const int RECEIPT_ITEM_FULL_PRICE_NONZERO = 165;

	public const int RECEIPT_PRICE_QTY_NOT_EQUAL = 166;

	public const int RECEIPT_PAYMENT_AMOUNT_MORE_THAN_TOTAL = 167;

	public const int RECEIPT_PAYMENT_AMOUNT_REDUNDANT_CASH = 168;

	public const int PRINT_FISCAL_REPORT_WRONG_FILTERS = 169;

	public const int PRINT_FISCAL_REPORT_INVALID_PERIOD = 170;

	public const int RECEIPT_PRICE_INVALID_ITEM_TOTAL_PRICE = 171;

	public const int GET_RECEIPT_TYPE_NOT_A_PRODUCT_RECEIPT = 172;

	public const int INVALID_DISCOUNT_TYPES = 173;

	public const int RETURN_RECEIPT_DOES_NOT_EXIST = 174;

	public const int RETURN_RECEIPT_INVALID_CRN = 175;

	public const int ERROR_LAST_RECEIPT_DOES_NOT_EXIST = 176;

	public const int RETURN_RECEIPT_TYPE_NOT_SUPPORTED = 177;

	public const int RECEIPT_RETURN_BAD_REQUESTED_AMOUNT = 178;

	public const int ERROR_PARTIAL_PAYMENT_RECEIPT_MUST_BE_RETURNED_FULLY = 179;

	public const int RECEIPT_RETURN_MISSING_PRODUCT_LIST = 180;

	public const int RECEIPT_RETURN_INVALID_QUANTITY_FOR_ITEM = 181;

	public const int ERROR_RETURN_RECEIPT_IS_RETURN_TYPE = 182;

	public const int ERROR_INVALID_CPA_CODE = 183;

	public const int PREPAYMENT_RETURN_INVALID_REQUEST = 184;

	public const int RECEIPT_HAS_NOT_SYNCED_REFERENCED_RECEIPTS = 185;

	public const int INVALID_SUM_DURING_PREPAYMENT = 186;

	public const int INVALID_LIST_DURING_PREPAYMENT = 187;

	public const int INVALID_SUMS = 188;

	public const int INVALID_ROUNDING = 189;

	public const int PAYMENT_IS_UNAVAILABLE = 190;

	public const int SUM_MUST_BE_GREATER_THAN_ZERO_DURING_CASH_IN_OUT = 191;

	public const int CPA_IS_REQUIRED = 192;

	public const int PARTNER_TIN_INVALID_FORMAT = 193;

	public const int ERROR_PREPAYMENT_CONTAINS_EMARKS = 194;

	public const int WRONG_EMARK_FORMAT = 195;

	public const int WRONG_COUNTRY_CODE = 196;

	public const int SRC_SERVER_CONNECTION_ERROR = 501;

	public const int NO_INTERNET_ACCESS = 502;

	public const int REQUEST_VOLUME_EXCEEDS_THE_LIMIT = 612;

	public const int ONLY_NONCASH_PAYMENT_ALLOWED_WITH_BUYERS_TIN = 613;

	public const int ONLY_NONCASH_PAYMENT_ALLOWED = 614;

	public const int ONLY_NONCASH_PAYMENT_ALLOWED_AMOUNT_EXCEEDS_CASH_LIMIT = 615;

	public const int UNKNOWN_RESPONSE_CODE = -1;

	public const int EMarkMinLength = 29;

	public const int EMarkMaxLength = 110;

	public const string ErrLenEMark = "{0} eMark կոդի նիշերի քանակը պետք է լինի {1} - {2} միջակայքում";
}
