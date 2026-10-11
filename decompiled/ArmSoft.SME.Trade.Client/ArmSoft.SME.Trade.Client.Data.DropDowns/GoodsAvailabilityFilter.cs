using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.DropDowns;

public class GoodsAvailabilityFilter : DataProvider<GoodsAvailabilityFilter.DataRow, GoodsAvailabilityFilter.Param>
{
	public class DataRow : ExtendableRow, IMatchedFilterKey
	{
		public string ItemType { get; set; }

		public string ItemTypeCaption { get; set; }

		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string Caption { get; set; }

		public string CPACode { get; set; }

		public string Unit { get; set; }

		public string UnitBrief { get; set; }

		public string Group { get; set; }

		public string GroupPath { get; set; }

		public string GroupCaption { get; set; }

		public string Description { get; set; }

		public decimal Quantity { get; set; }

		public decimal Price { get; set; }

		public bool VAT { get; set; }

		public string SubstituteItemsGroup { get; set; }

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
		public DateOnly AsOfDate { get; set; }

		public string Storage { get; set; }

		public bool ForRetail { get; set; }

		public bool IncludeServices { get; set; }

		public bool ShowAll { get; set; }

		public string Group { get; set; }

		public string PriceListType { get; set; }

		public string MTCode { get; set; }

		public bool ShowPrice { get; set; }
	}

	public override string Name => "GoodsAvailabilityFilter";

	public GoodsAvailabilityFilter(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
