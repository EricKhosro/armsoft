using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class PurchasesByPartiesDataProvider : DataProvider<PurchasesByPartiesDataProvider.DataRow, PurchasesByPartiesDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public DateOnly Date { get; set; }

		public string DocNum { get; set; }

		public string DocTypeCaption { get; set; }

		public string Storage { get; set; }

		public string CorrObjectCode { get; set; }

		public string CorrObjectCaption { get; set; }

		public decimal Quantity { get; set; }

		public decimal Cost { get; set; }

		public decimal CostSummWithVAT { get; set; }

		public string Comment { get; set; }

		public string PartyContractCaption { get; set; }

		public DateOnly? PartyDocUsableDate { get; set; }

		public string PartyDocCountryCode { get; set; }

		public string PartyDocCountry { get; set; }

		public string PartyDocProducerCode { get; set; }

		public string PartyDocProducer { get; set; }

		public string PartyDocPropertyType1 { get; set; }

		public string PartyDocPropertyValue1 { get; set; }

		public string PartyDocPropertyType2 { get; set; }

		public string PartyDocPropertyValue2 { get; set; }
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string Storage { get; set; }

		public int MTID { get; set; } = -1;

		public int SupplierID { get; set; } = -1;
	}

	public override string Name => "PurchasesByParties";

	public PurchasesByPartiesDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
