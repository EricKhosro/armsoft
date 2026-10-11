using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.DropDowns;

public class GoodsFilter : DataProvider<GoodsFilter.DataRow, GoodsFilter.Param>
{
	public class DataRow : ExtendableRow, IMatchedFilterKey
	{
		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string Caption { get; set; }

		public string FullCaption { get; set; }

		public string Unit { get; set; }

		public string UnitBrief { get; set; }

		public string AltUnit { get; set; }

		public string AltUnitBrief { get; set; }

		public string Group { get; set; }

		public string GroupCaption { get; set; }

		public string GroupPath { get; set; }

		public string BarCode { get; set; }

		public string CPACode { get; set; }

		public string Type { get; set; }

		public string TypeName { get; set; }

		public string Description { get; set; }

		public string ExternalCode { get; set; }

		public string ExpenseMethod { get; set; }

		public string ExpenseMethodName { get; set; }

		public bool VAT { get; set; }

		public decimal EcoFeeRate { get; set; }

		public string Producer { get; set; }

		public string Country { get; set; }

		public string Property1 { get; set; }

		public string Property2 { get; set; }

		public string SubstituteItemsGroup { get; set; }

		public bool IsWeight { get; set; }

		public bool CheckMarking { get; set; }

		public bool ShowInPriceList { get; set; }

		public decimal Coeff { get; set; }

		public decimal Extra { get; set; }

		public decimal Bonus { get; set; }

		public decimal Point { get; set; }

		public decimal Discount { get; set; }

		public int? PLUCode { get; set; }

		public decimal MinQty { get; set; }

		public decimal MaxQty { get; set; }

		public string Body { get; set; }

		public string CaptionEN { get; set; }

		public string CaptionRU { get; set; }

		public string AdditionalDescr1 { get; set; }

		public string AdditionalDescr2 { get; set; }

		public byte[] TS { get; }

		public object FilterKey => MTCode;

		public bool FilterMatched(object key, object path)
		{
			if ((string)key == Group)
			{
				return true;
			}
			return GroupPath.StartsWith((string)path + "!");
		}
	}

	public class Param
	{
		public string ItemType { get; set; } = "";

		public string Group { get; set; } = "";

		public bool IsShowOnlyInPriceList { get; set; }

		public string LikeName { get; set; } = "";

		public string PartnerCode { get; set; } = "";
	}

	public override string Name => "GoodsFilter";

	public GoodsFilter(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
