using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class SalesAveragePriceDataProvider : DataProvider<SalesAveragePriceDataProvider.DataRow, SalesAveragePriceDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string ItemCode { get; set; } = string.Empty;

		public string ItemName { get; set; } = string.Empty;

		public string UnitCode { get; set; } = string.Empty;

		public string UnitBrief { get; set; } = string.Empty;

		public decimal SaleAvgPrice { get; set; }

		public string ItemGroup { get; set; } = string.Empty;

		public decimal Quantity { get; set; }
	}

	public class Param
	{
		public DateOnly? StartDate { get; set; }

		public DateOnly? EndDate { get; set; }

		public string Storage { get; set; } = string.Empty;

		public string GoodCode { get; set; } = string.Empty;

		public string GoodGroup { get; set; } = string.Empty;

		public string DocType { get; set; } = string.Empty;

		public string PriceListType { get; set; } = string.Empty;
	}

	public override string Name => "SalesAveragePrice";

	public SalesAveragePriceDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
