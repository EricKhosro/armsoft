using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTOverheadExp : TradeDocument
{
	public class MTOverheadExpSpecificationRow
	{
		public string MTCode { get; set; }

		public int MTID { get; set; } = -1;

		public short RowNum { get; set; }

		public string UnitCode { get; set; }

		public decimal Quantity { get; set; }

		public decimal Summa { get; set; }

		public decimal DistributedSumma { get; set; }

		public decimal DistributedVATSumma { get; set; }

		public decimal DistributedCurrSumma { get; set; }

		public bool Include { get; set; }

		public bool VAT { get; set; }

		public int RowID { get; set; }

		public string MTName { get; set; } = "";

		public string FullName { get; set; } = "";

		public string UnitBrief { get; set; } = "";

		public decimal Coeff { get; set; }
	}

	public DateOnly? OverheadExpDate { get; set; }

	public decimal SummWithoutVAT { get; set; }

	public decimal SummVAT { get; set; }

	public bool AllowMTListEdit { get; set; }

	public string DistrType { get; set; }

	public string DistrSumBy { get; set; } = "";

	public string GridViewColsType { get; set; }

	public List<MTOverheadExpSpecificationRow> MTList { get; set; } = new List<MTOverheadExpSpecificationRow>();

	public List<MTIncomeBase.MTIncomeSpecificationRow> MTIncomeMTList { get; set; }
}
