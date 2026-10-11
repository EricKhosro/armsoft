using System.Collections.Generic;
using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class CashCacheRow : CacheRowBase<string>
{
	public class NonCashInfo
	{
		public int Type { get; set; }

		public string Token { get; set; } = string.Empty;

		public string MerchantID { get; set; } = string.Empty;

		public string Partner { get; set; } = string.Empty;

		public string Contract { get; set; } = string.Empty;

		public string Caption { get; set; } = string.Empty;

		public string Device { get; set; } = "0";

		public bool StaticQR { get; set; }

		public string Branch { get; set; } = string.Empty;

		public string Cashier { get; set; } = string.Empty;

		public bool IsSellerScans { get; set; }

		public string POSTerminalID { get; set; } = string.Empty;

		public string POSTerminalAddress { get; set; } = string.Empty;

		public short ECRPaymentMethod { get; set; }

		public NonCashInfo Clone()
		{
			return new NonCashInfo
			{
				Type = Type,
				Token = Token,
				MerchantID = MerchantID,
				Partner = Partner,
				Contract = Contract,
				Caption = Caption,
				Device = Device,
				StaticQR = StaticQR,
				Branch = Branch,
				Cashier = Cashier,
				IsSellerScans = IsSellerScans,
				POSTerminalID = POSTerminalID,
				POSTerminalAddress = POSTerminalAddress,
				ECRPaymentMethod = ECRPaymentMethod
			};
		}
	}

	public string Code { get; set; }

	public string Caption { get; set; }

	public bool IsDefault { get; set; }

	public string NextNumIn { get; set; }

	public string NextNumOut { get; set; }

	public string NextMTBillNum { get; set; }

	public string NextMTBillReturnNum { get; set; }

	public string NextCashierShiftNum { get; set; }

	public bool IsECR { get; set; }

	public string ECRSN { get; set; }

	public string ECRCrn { get; set; }

	public string ECRIP { get; set; }

	public string ECRPort { get; set; }

	public string ECRMacAddress { get; set; }

	public string ECRPassword { get; set; }

	public string ECROperator { get; set; }

	public string ECROperatorPassword { get; set; }

	public string ECRDepWithVAT { get; set; }

	public string ECRDepWithOutVAT { get; set; }

	public string ECRPrintMode { get; set; }

	public string CashSummLimitType { get; set; } = string.Empty;

	public bool ShowECRTracking { get; set; }

	public bool B2BPublish { get; set; }

	public Dictionary<int, NonCashInfo> NonCashInfoDict { get; set; }

	public List<NonCashInfo> NonCashInfoList { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public CashCacheRow()
	{
		Code = string.Empty;
		Caption = string.Empty;
		IsDefault = false;
		NextNumIn = string.Empty;
		NextNumOut = string.Empty;
		NextMTBillNum = string.Empty;
		NextMTBillReturnNum = string.Empty;
		IsECR = false;
		ECRSN = string.Empty;
		ECRIP = string.Empty;
		ECRPort = string.Empty;
		ECRMacAddress = string.Empty;
		ECRPassword = string.Empty;
		ECRDepWithVAT = string.Empty;
		ECRDepWithOutVAT = string.Empty;
		ECRPrintMode = string.Empty;
		ECRCrn = string.Empty;
		ShowECRTracking = false;
		B2BPublish = false;
		NonCashInfoDict = new Dictionary<int, NonCashInfo>();
		NonCashInfoList = new List<NonCashInfo>();
	}

	public CashCacheRow(string code, string caption, bool isDefault, string nextNumIn, string nextNumOut, string nextMTBillNum, string nextMTBillReturnNum, string nextCashierShiftNum, bool isECR, string ecrSN, string ecrIP, string ecrPort, string ecrMacAddress, string ecrPassword, string ecrDepWithVat, string ECRDepWithOutVat, string ecrPrintMode, string ecrCrn, bool showECRTracking, string cashSummLimitType, bool b2bPublish, byte[] ts, Dictionary<int, NonCashInfo> nonCashInfoDict, List<NonCashInfo> nonCashInfos)
	{
		Code = code;
		Caption = caption;
		IsDefault = isDefault;
		NextNumIn = nextNumIn;
		NextNumOut = nextNumOut;
		NextMTBillNum = nextMTBillNum;
		NextMTBillReturnNum = nextMTBillReturnNum;
		NextCashierShiftNum = nextCashierShiftNum;
		IsECR = isECR;
		ECRSN = ecrSN;
		ECRIP = ecrIP;
		ECRPort = ecrPort;
		ECRMacAddress = ecrMacAddress;
		ECRPassword = ecrPassword;
		ECRDepWithVAT = ecrDepWithVat;
		ECRDepWithOutVAT = ECRDepWithOutVat;
		ECRPrintMode = ecrPrintMode;
		ECRCrn = ecrCrn;
		ShowECRTracking = showECRTracking;
		CashSummLimitType = cashSummLimitType;
		B2BPublish = b2bPublish;
		TS = ts;
		NonCashInfoDict = nonCashInfoDict;
		NonCashInfoList = nonCashInfos;
	}
}
