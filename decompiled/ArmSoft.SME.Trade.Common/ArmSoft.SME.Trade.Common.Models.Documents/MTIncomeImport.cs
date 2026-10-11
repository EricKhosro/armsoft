using System;
using System.Collections.Generic;
using ArmSoft.SME.Trade.Common.CustomTypes;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTIncomeImport : TradeDocument
{
	public class MTIncomeImportSpecificationRow
	{
		public string MTCode { get; set; }

		public short RowNum { get; set; }

		public int RowID { get; set; }

		public int MTID { get; set; }

		public string UnitCode { get; set; }

		public decimal Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal Summa { get; set; }

		public decimal CustomSumma { get; set; }

		public decimal CustomPercent { get; set; }

		public decimal CustomDuty { get; set; }

		public bool VAT { get; set; }

		public string VATCodeForConvert { get; set; }

		public decimal VATSumma { get; set; }

		public DateOnly? UseLife { get; set; }

		public string Country { get; set; }

		public string Producer { get; set; }

		public string Property1 { get; set; }

		public string Property2 { get; set; }

		public string MTName { get; set; }

		public string FullName { get; set; }

		public string UnitBrief { get; set; }

		public string ProducerCaption { get; set; }

		public string Property1Caption { get; set; }

		public string Property2Caption { get; set; }

		public string RowDescription { get; set; }

		public decimal LastPurchasePrice { get; set; }

		public decimal VariancePercent { get; set; }
	}

	public PartnerContractGroup CustomContractContainer { get; set; } = new PartnerContractGroup();

	public decimal CustomDuty { get; set; }

	public decimal CustomFee { get; set; }

	public int MaxRowID { get; set; }

	public decimal SummVAT { get; set; }

	public string PriceType { get; set; }

	public string Trust { get; set; }

	public string Mediator { get; set; }

	public string Machine { get; set; }

	public string Chief { get; set; }

	public string Allower { get; set; }

	public string Response { get; set; }

	public List<MTIncomeImportSpecificationRow> MTList { get; set; }
}
