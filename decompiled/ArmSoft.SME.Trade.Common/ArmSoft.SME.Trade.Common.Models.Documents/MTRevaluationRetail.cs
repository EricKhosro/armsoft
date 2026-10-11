using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTRevaluationRetail : TradeDocument
{
	public class MTRevaluationRetailSpecificationRow
	{
		public string MTCode { get; set; } = string.Empty;

		public int MTID { get; set; } = -1;

		public string UnitCode { get; set; } = string.Empty;

		public decimal Quantity { get; set; }

		public decimal Cost { get; set; }

		public decimal NewCost { get; set; }

		public int RowID { get; set; }

		public short RowNum { get; set; }

		public string MTName { get; set; } = string.Empty;

		public string FullName { get; set; } = string.Empty;

		public string UnitBrief { get; set; } = string.Empty;
	}

	public List<PartyRow> MTExpDocSelectedPartiesList { get; set; } = new List<PartyRow>();

	public List<MTRevaluationRetailSpecificationRow> MTList { get; set; } = new List<MTRevaluationRetailSpecificationRow>();

	public int MaxRowID { get; set; }

	public string PriceType { get; set; } = string.Empty;
}
