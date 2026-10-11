using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.DropDowns;

public class MTRetailQtyAndPriceDataProvider : DataProvider<MTRetailQtyAndPriceDataProvider.DataRow, MTRetailQtyAndPriceDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public decimal Quantity { get; set; }

		public decimal Price { get; set; }
	}

	public class Param
	{
		public string MTCode { get; set; }

		public string Storage { get; set; }

		public DateOnly? Date { get; set; }

		public decimal UnitCoef { get; set; }
	}

	public override string Name => "MTRetailQtyAndPrice";

	public MTRetailQtyAndPriceDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
