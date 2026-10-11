using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTExpense : TradeDocument
{
	public class MTExpenseSpecificationRow
	{
		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string MTName { get; set; }

		public short RowNum { get; set; }

		public decimal Quantity { get; set; }

		public string UnitCode { get; set; }

		public string UnitBrief { get; set; }

		public decimal SalePrice { get; set; }

		public decimal Summa { get; set; }

		public int RowID { get; set; }

		public bool IsPartySelected { get; set; }
	}

	public List<PartyRow> MTExpDocSelectedPartiesList { get; set; } = new List<PartyRow>();

	public int MaxRowID { get; set; }

	public string Trust { get; set; }

	public string Mediator { get; set; }

	public string Machine { get; set; }

	public string Chief { get; set; }

	public string Allower { get; set; }

	public string Demand { get; set; }

	public List<MTExpenseSpecificationRow> MTList { get; set; } = new List<MTExpenseSpecificationRow>();
}
