using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTIncomeBase : TradeDocument
{
	public class MTIncomeSpecificationRow
	{
		public string MTCode { get; set; }

		public short RowNum { get; set; }

		public int RowID { get; set; }

		public int MTID { get; set; }

		public string UnitCode { get; set; }

		public decimal Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal Discount { get; set; }

		public decimal DiscountPrice { get; set; }

		public decimal Summa { get; set; }

		public decimal ExtraPercent { get; set; }

		public decimal SalePrice { get; set; }

		public decimal SaleSumma { get; set; }

		public string VATCodeForConvert { get; set; }

		public bool? VAT { get; set; }

		public DateOnly? UseLife { get; set; }

		public string Country { get; set; }

		public string Producer { get; set; }

		public string Property1 { get; set; }

		public string Property1Caption { get; set; }

		public string Property2 { get; set; }

		public string Property2Caption { get; set; }

		public string MTName { get; set; }

		public string FullName { get; set; }

		public string UnitBrief { get; set; }

		public string ProducerCaption { get; set; }

		public decimal LastPurchasePrice { get; set; }

		public decimal VariancePercent { get; set; }

		public string RowDescription { get; set; }
	}

	public int MaxRowID { get; set; }

	public decimal Discount { get; set; }

	public decimal SummVAT { get; set; }

	public string PriceType { get; set; }

	public string Trust { get; set; }

	public string Mediator { get; set; }

	public string Machine { get; set; }

	public string Chief { get; set; }

	public string Allower { get; set; }

	public string Response { get; set; }

	public bool IncludeVATInCost { get; set; }

	public List<MTIncomeSpecificationRow> MTList { get; set; } = new List<MTIncomeSpecificationRow>();
}
