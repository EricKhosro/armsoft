using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Models;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class ReceivedServicesJournalDataProvider : DataProvider<ReceivedServicesJournalDataProvider.DataRow, ReceivedServicesJournalDataProvider.Param>
{
	public class DataRow : TradeDocumentDataRow
	{
		public string ItemCode { get; set; }

		public string ItemCaption { get; set; }

		public string Unit { get; set; }

		public string UnitBrief { get; set; }

		public string PartnerCode { get; set; }

		public string PartnerCaption { get; set; }

		public string PartnerTaxCode { get; set; }

		public decimal Quantity { get; set; }

		public decimal Cost { get; set; }

		public bool VAT { get; set; }

		public string VATType { get; set; }

		public string Storage { get; set; }

		public string StorageCaption { get; set; }

		public decimal Discount { get; set; }

		public decimal DiscountPrice { get; set; }

		public DateOnly? UseLife { get; set; }

		public string Country { get; set; }

		public string Producer { get; set; }

		public string Property1 { get; set; }

		public string Property2 { get; set; }

		public decimal ExtraPercent { get; set; }

		public decimal SalePrice { get; set; }

		public decimal SaleSumma { get; set; }
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string DocType { get; set; }

		public string PartnerCode { get; set; }

		public string ItemCode { get; set; }

		public string DocState { get; set; }
	}

	public override string Name => "ReceivedServicesJournal";

	public ReceivedServicesJournalDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
