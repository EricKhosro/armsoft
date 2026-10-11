using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class MTQntUnitsDataProvider : DataProvider<MTQntUnitsDataProvider.DataRow, MTQntUnitsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public class MTQntUnitsRowKey
		{
			public int MTID { get; set; }

			public string QntUnit { get; set; }
		}

		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string MTCaption { get; set; }

		public bool MTShowInPriceList { get; set; }

		public string QntUnit { get; set; }

		public string QntUnitCaption { get; set; }

		public string QntUnitBrief { get; set; }

		public decimal Coef { get; set; }

		public bool IsDefault { get; set; }

		public byte[] Image { get; set; }

		public bool ImageExists { get; set; }

		public string ImgStr => MTID + "," + QntUnit.Trim();

		public object GetKey()
		{
			return new MTQntUnitsRowKey
			{
				MTID = MTID,
				QntUnit = QntUnit
			};
		}

		public string GetCode()
		{
			return "";
		}

		public string GetName()
		{
			return "";
		}

		public bool Matched(object key)
		{
			MTQntUnitsRowKey mTQntUnitsRowKey = (MTQntUnitsRowKey)key;
			if (mTQntUnitsRowKey.MTID == MTID)
			{
				return mTQntUnitsRowKey.QntUnit == QntUnit;
			}
			return false;
		}
	}

	public class Param
	{
		public string MTCode { get; set; }
	}

	public override string Name => "MTQntUnits";

	public MTQntUnitsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
