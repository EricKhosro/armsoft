using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Models;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class PayOrdJournalDataProvider : DataProvider<PayOrdJournalDataProvider.DataRow, PayOrdJournalDataProvider.Param>
{
	public class DataRow : TradeDocumentDataRow
	{
		public string PayerAcc { get; set; }

		public string ReceiverAcc { get; set; }

		public string Receiver { get; set; }

		public string PaymentAim { get; set; }

		public bool ExportedToBank { get; set; }

		public int PartID { get; set; }

		public string PartCode { get; set; }

		public string PartTaxCode { get; set; }
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string PartnerCode { get; set; }
	}

	public override string Name => "PayOrdJournal";

	public PayOrdJournalDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
