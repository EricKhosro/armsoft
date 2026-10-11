using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class CurrentAvailGoodsByPartiesDataProvider : DataProvider<CurrentAvailGoodsByPartiesDataProvider.DataRow, CurrentAvailGoodsByPartiesDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public decimal Qty { get; set; }

		public DateOnly IncomeDate { get; set; }

		public decimal Cost { get; set; }

		public decimal Summa { get; set; }

		public string Storage { get; set; }

		public string Comment { get; set; }

		public DateOnly? UseableDate { get; set; }

		public string PartnerCode { get; set; }

		public string PartnerCaption { get; set; }

		public string Contract { get; set; }

		public string Country { get; set; }

		public string Producer { get; set; }

		public string Property1 { get; set; }

		public string Property2 { get; set; }
	}

	public class Param
	{
		public string Storage { get; set; }

		public int MTID { get; set; }

		public int SupplierID { get; set; }
	}

	public override string Name => "CurrentAvailGoodsByParties";

	public CurrentAvailGoodsByPartiesDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
