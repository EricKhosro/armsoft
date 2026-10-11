using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class SalesByPartiesDataProvider : DataProvider<SalesByPartiesDataProvider.DataRow, SalesByPartiesDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public DateOnly? DocDate { get; set; }

		public string DocNum { get; set; }

		public string OpName { get; set; }

		public string Storage { get; set; }

		public string PartnerCode { get; set; }

		public string PartnerCaption { get; set; }

		public decimal Quantity { get; set; }

		public decimal SalePrice { get; set; }

		public decimal SaleSumm { get; set; }

		public decimal SalePriceWithVAT { get; set; }

		public decimal SaleSummWithVAT { get; set; }

		public string Comment { get; set; }

		public DateOnly? IncomeDate { get; set; }

		public string PartyPartCaption { get; set; } = string.Empty;

		public string PartyContractCaption { get; set; } = string.Empty;

		public string PartyDocNum { get; set; } = string.Empty;

		public string PartyDocType { get; set; } = string.Empty;

		public DateOnly? PartyDocUsableDate { get; set; }

		public string PartyDocCountryCode { get; set; } = string.Empty;

		public string PartyDocCountry { get; set; } = string.Empty;

		public string PartyDocProducerCode { get; set; } = string.Empty;

		public string PartyDocProducer { get; set; } = string.Empty;

		public string PartyDocPropertyType1 { get; set; } = string.Empty;

		public string PartyDocPropertyValue1 { get; set; } = string.Empty;

		public string PartyDocPropertyType2 { get; set; } = string.Empty;

		public string PartyDocPropertyValue2 { get; set; } = string.Empty;
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string Storage { get; set; }

		public int MTID { get; set; } = -1;

		public int SupplierID { get; set; } = -1;

		public int BuyerId { get; set; } = -1;
	}

	public override string Name => "SalesByParties";

	public SalesByPartiesDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
