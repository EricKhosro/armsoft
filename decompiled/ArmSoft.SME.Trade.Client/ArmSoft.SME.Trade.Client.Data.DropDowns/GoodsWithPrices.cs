using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.DropDowns;

public class GoodsWithPrices : DataProvider<GoodsWithPrices.DataRow, GoodsWithPrices.Param>
{
	public class DataRow : ExtendableRow, IMatchedFilterKey
	{
		public string GoodCode { get; set; }

		public string Caption { get; set; }

		public string CPACode { get; set; }

		public string Group { get; set; }

		public string GroupPath { get; set; }

		public decimal Price { get; set; }

		public string UnitBrief { get; set; }

		public string Description { get; set; }

		public object FilterKey => GoodCode;

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
		public DateOnly Date { get; set; }

		public string PriceType { get; set; }

		public string LikeName { get; set; }

		public string PartCode { get; set; }

		public string ItemType { get; set; }
	}

	public override string Name => "GoodsWithPrices";

	public GoodsWithPrices(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
