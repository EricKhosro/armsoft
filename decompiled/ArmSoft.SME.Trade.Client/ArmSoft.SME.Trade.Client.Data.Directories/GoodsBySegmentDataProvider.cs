using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class GoodsBySegmentDataProvider : DataProvider<GoodsBySegmentDataProvider.DataRow, GoodsBySegmentDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public class GoodsBySegmentRowKey
		{
			public int MTID { get; set; } = -1;

			public string SegmentCode { get; set; } = string.Empty;
		}

		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string Caption { get; set; }

		public string Type { get; set; }

		public string TypeName { get; set; }

		public string SegmentCode { get; set; }

		public string SegmentCaption { get; set; }

		public string FullCaption { get; set; }

		public string Group { get; set; }

		public string CPACode { get; set; }

		public string GroupCaption { get; set; }

		public string Unit { get; set; }

		public string UnitBrief { get; set; }

		public string AltUnit { get; set; }

		public string AltUnitBrief { get; set; }

		public string BarCode { get; set; }

		public string Description { get; set; }

		public string ExternalCode { get; set; }

		public bool ShowInPriceList { get; set; }

		public string Country { get; set; }

		public string Producer { get; set; }

		public string Property1 { get; set; }

		public string Property2 { get; set; }

		public decimal MinQty { get; set; }

		public decimal MaxQty { get; set; }

		public decimal Bonus { get; set; }

		public decimal Point { get; set; }

		public bool VAT { get; set; }

		public bool IsWeight { get; set; }

		public decimal Discount { get; set; }

		public string Body { get; set; }

		public object GetKey()
		{
			return new GoodsBySegmentRowKey
			{
				MTID = MTID,
				SegmentCode = SegmentCode
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
			GoodsBySegmentRowKey goodsBySegmentRowKey = (GoodsBySegmentRowKey)key;
			if (goodsBySegmentRowKey.MTID == MTID)
			{
				return goodsBySegmentRowKey.SegmentCode == SegmentCode;
			}
			return false;
		}
	}

	public class Param
	{
		public string SegmentCode { get; set; }

		public string MTCode { get; set; }
	}

	public override string Name => "GoodsBySegment";

	public GoodsBySegmentDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
