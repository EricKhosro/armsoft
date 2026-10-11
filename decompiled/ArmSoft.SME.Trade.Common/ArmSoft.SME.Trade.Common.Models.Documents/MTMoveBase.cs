using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTMoveBase : TradeDocument
{
	public class MTMoveSpecificationRow
	{
		public string MTCode { get; set; }

		public int MTID { get; set; }

		public short RowNum { get; set; }

		public decimal Quantity { get; set; }

		public decimal ActualQuantity { get; set; }

		public string UnitCode { get; set; }

		public decimal? Price { get; set; }

		public decimal? Summa { get; set; }

		public bool IsPartySelected { get; set; }

		public int RowID { get; set; }

		public string RowDescription { get; set; }

		public string MTName { get; set; }

		public string FullName { get; set; }

		public string UnitBrief { get; set; }

		public string CPACode { get; set; } = "";
	}

	public int MaxRowID { get; set; }

	public string StorageExpense { get; set; }

	public string StorageIncome { get; set; }

	public List<MTMoveSpecificationRow> MTList { get; set; } = new List<MTMoveSpecificationRow>();

	internal string StorageIncomeCaption { get; set; }
}
