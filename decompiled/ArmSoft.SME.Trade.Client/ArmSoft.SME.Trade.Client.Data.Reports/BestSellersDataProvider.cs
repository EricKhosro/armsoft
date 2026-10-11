using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class BestSellersDataProvider : DataProvider<BestSellersDataProvider.DataRow, BestSellersDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int ItemID { get; set; }

		public string ItemCode { get; set; }

		public decimal Summ { get; set; }

		public string Caption { get; set; }
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public byte CompareMode { get; set; }

		public int TopsCount { get; set; }

		public byte SalesSumType { get; set; }

		public string ItemGroup { get; set; }
	}

	public override string Name => "BestSellers";

	public BestSellersDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
