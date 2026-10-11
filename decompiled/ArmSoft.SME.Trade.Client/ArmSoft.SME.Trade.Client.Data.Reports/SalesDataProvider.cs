using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class SalesDataProvider : DataProvider<SalesDataProvider.DataRow, SalesDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public DateOnly PeriodStartDate { get; set; }

		public string PeriodCaption { get; set; }

		public decimal Cost { get; set; }

		public decimal Income { get; set; }

		public decimal Profit { get; set; }
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public short PeriodMode { get; set; }

		public string ItemType { get; set; }

		public short SalesSumType { get; set; }

		public string ItemGroup { get; set; }

		public string Storage { get; set; }
	}

	public override string Name => "Sales";

	public SalesDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
