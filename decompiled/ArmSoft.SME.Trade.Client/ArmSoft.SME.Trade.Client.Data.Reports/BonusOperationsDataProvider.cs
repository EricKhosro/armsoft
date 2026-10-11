using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class BonusOperationsDataProvider : DataProvider<BonusOperationsDataProvider.DataRow, BonusOperationsDataProvider.Param>
{
	public class DataRow : TradeOperationDataRow
	{
		public string OpCode { get; set; }

		public DateOnly Date { get; set; }

		public string DocNum { get; set; }

		public string DocTypeCaption { get; set; }

		public int PartId { get; set; }

		public string PartCode { get; set; }

		public string PartName { get; set; }

		public string TaxCode { get; set; }

		public string ContractName { get; set; }

		public string PartnerGroup { get; set; }

		public bool DbCr { get; set; }

		public decimal Summ { get; set; }

		public decimal SummIn { get; set; }

		public decimal SummOut { get; set; }

		public string Comment { get; set; }

		public string UserName { get; set; }

		public string Owner { get; set; }
	}

	public class Param
	{
		public DateOnly? StartDate { get; set; }

		public DateOnly? EndDate { get; set; }

		public string PartnerGroup { get; set; }

		public string PartnerCode { get; set; }

		public string Contract { get; set; }

		public string DocType { get; set; }

		public Guid DocISN { get; set; }
	}

	public override string Name => "BonusOperations";

	public BonusOperationsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
