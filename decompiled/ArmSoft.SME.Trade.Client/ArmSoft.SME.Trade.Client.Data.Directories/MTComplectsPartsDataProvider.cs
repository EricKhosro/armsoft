using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class MTComplectsPartsDataProvider : DataProvider<MTComplectsPartsDataProvider.DataRow, MTComplectsPartsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public class MTComplectsRowKey
		{
			public int MTDestID { get; set; }

			public int MTSourceID { get; set; }
		}

		public int MTDestID { get; set; }

		public string MTDestCode { get; set; }

		public string MTDestCaption { get; set; }

		public string ItemType { get; set; }

		public string ItemTypeCaption { get; set; }

		public int MTSourceID { get; set; }

		public string MTSourceCode { get; set; }

		public string MTSourceCaption { get; set; }

		public string QntUnit { get; set; }

		public string QntUnitBrief { get; set; }

		public decimal QTY { get; set; }

		public decimal ComplectQTY { get; set; }

		public decimal SummPart { get; set; }

		public decimal Summ { get; set; }

		public bool VAT { get; set; }

		public object GetKey()
		{
			return new MTComplectsRowKey
			{
				MTDestID = MTDestID,
				MTSourceID = MTSourceID
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
			MTComplectsRowKey mTComplectsRowKey = (MTComplectsRowKey)key;
			if (mTComplectsRowKey.MTDestID == MTDestID)
			{
				return mTComplectsRowKey.MTSourceID == MTSourceID;
			}
			return false;
		}
	}

	public class Param
	{
		public string ComplectCode { get; set; } = string.Empty;
	}

	public override string Name => "MTComplectsParts";

	public MTComplectsPartsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
