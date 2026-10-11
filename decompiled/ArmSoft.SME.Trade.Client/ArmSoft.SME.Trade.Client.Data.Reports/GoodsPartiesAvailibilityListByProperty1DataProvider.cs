using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class GoodsPartiesAvailibilityListByProperty1DataProvider : DataProvider<GoodsPartiesAvailibilityListByProperty1DataProvider.DataRow, GoodsPartiesAvailibilityListByProperty1DataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string ItemType { get; set; } = "";

		public int GoodID { get; set; } = -1;

		public string GoodCode { get; set; } = "";

		public string Caption { get; set; } = "";

		public string Unit { get; set; } = "";

		public string UnitBrief { get; set; } = "";

		public string Group { get; set; } = "";

		public string GroupCaption { get; set; } = "";

		public string Description { get; set; } = "";

		public decimal Quantity { get; set; }

		public DateOnly? IncomeDate { get; set; }

		public decimal Cost { get; set; }

		public string PartnerCode { get; set; }

		public string Contract { get; set; }

		public string Property1Code { get; set; }

		public string Property1Value { get; set; }

		public DateOnly? UseableDate { get; set; }

		public string Country { get; set; } = "";

		public string Producer { get; set; } = "";

		public string Comment { get; set; } = "";

		public Guid PartyBase { get; set; }

		public int PartyRow { get; set; }

		public decimal? Price { get; set; }

		public string SubstituteItemsGroup { get; set; } = "";

		public bool Matched(object key)
		{
			return (string)key == GoodCode;
		}
	}

	public class Param
	{
		public string PartiesStorage { get; set; }

		public string PartiesProperty1Value { get; set; }

		public DateOnly PartiesDate { get; set; }
	}

	public override string Name => "GoodsPartiesAvailibilityListByProperty1";

	public GoodsPartiesAvailibilityListByProperty1DataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
