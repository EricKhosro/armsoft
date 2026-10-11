using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.Data.Reports;

public class AccRestRow
{
	public class AccRestRowKey
	{
		public CommonConst.AccountingsTypes AccType { get; set; }

		public string AccCode { get; set; } = "";

		public int PartID { get; set; }

		public string CurrCode { get; set; }
	}

	public CommonConst.AccountingsTypes AccType { get; set; }

	public string AccCode { get; set; } = "";

	public string PartnerCode { get; set; } = "";

	public int PartID { get; set; } = -1;

	public string CurrCode { get; set; } = "";

	public decimal Remain { get; set; }

	public decimal CurrRemain { get; set; }

	public bool Matched(object Key)
	{
		AccRestRowKey accRestRowKey = (AccRestRowKey)Key;
		if (accRestRowKey.AccType == AccType && accRestRowKey.PartID == PartID && accRestRowKey.AccCode == AccCode)
		{
			return accRestRowKey.CurrCode == CurrCode;
		}
		return false;
	}
}
