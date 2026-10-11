using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.DropDowns;

public class GoodsAvailabilityFilterForInvoices : DataProvider<GoodsAvailabilityFilterForInvoices.DataRow, GoodsAvailabilityFilterForInvoices.Param>
{
	public class DataRow : ExtendableRow, IMatchedFilterKey
	{
		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string Caption { get; set; }

		public string CPACode { get; set; }

		public string Group { get; set; }

		public string GroupPath { get; set; }

		public string GroupCaption { get; set; }

		public string UnitBrief { get; set; }

		public decimal Quantity { get; set; }

		public decimal Price { get; set; }

		public string BarCode { get; set; }

		public string Description { get; set; }

		public string ItemType { get; set; }

		public string SubstituteItemsGroup { get; set; }

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

		public bool Retail { get; set; }

		public string PriceType { get; set; }

		public string LikeName { get; set; }

		public string SubstituteItemsGroup { get; set; }

		public bool IsFilteredSearchView { get; set; }

		public bool ExtendedView { get; set; }

		public bool ShowAll { get; set; }
	}

	public override string Name => "GoodsAvailabilityFilterForInvoices";

	public GoodsAvailabilityFilterForInvoices(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
