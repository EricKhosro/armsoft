using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class Cash : TradeDirectory
{
	public class NonCashInfo
	{
		public int Type { get; set; }

		public string Token { get; set; }

		public string MerchantID { get; set; }

		public string Partner { get; set; }

		public string Contract { get; set; }

		public string Caption { get; set; }

		public string Device { get; set; }

		public bool StaticQR { get; set; }

		public string Branch { get; set; }

		public string Cashier { get; set; }

		public bool IsSellerScans { get; set; }

		public string POSTerminalID { get; set; }

		public string POSTerminalAddress { get; set; }

		public short ECRPaymentMethod { get; set; }
	}

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

	public string CashSummLimitType { get; set; }

	public bool ShowECRTracking { get; set; }

	public bool B2BPublish { get; set; }

	public Dictionary<int, NonCashInfo> NonCashInfoList { get; set; } = new Dictionary<int, NonCashInfo>();
}
