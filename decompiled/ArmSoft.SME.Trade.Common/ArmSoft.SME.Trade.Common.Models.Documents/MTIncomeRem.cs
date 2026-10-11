using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTIncomeRem : TradeDocument
{
	public class MTIncomeRemSpecificationRow
	{
		public string Storage { get; set; }

		public string MTCode { get; set; }

		public short RowNum { get; set; }

		public int RowID { get; set; }

		public int MTID { get; set; }

		public string UnitCode { get; set; }

		public decimal Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal Summa { get; set; }

		public decimal CurrPrice { get; set; }

		public decimal CurrSumma { get; set; }

		public decimal SalePrice { get; set; }

		public decimal SaleSumma { get; set; }

		public string VATCodeForConvert { get; set; }

		public bool VAT { get; set; }

		public decimal VATPrice { get; set; }

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
	}

	public decimal SummVAT { get; set; }

	public decimal SummaCurr { get; set; }

	public string CreateType { get; set; }

	public string ParamMTCurr { get; set; }

	public int MaxRowID { get; set; }

	public List<MTIncomeRemSpecificationRow> MTList { get; set; } = new List<MTIncomeRemSpecificationRow>();
}
