using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class EMarkReceipt : TradeDocument
{
	public class EMarkReceiptSpecificationRow
	{
		public int Id { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		public string UnitCode { get; set; }

		public string UnitBrief { get; set; }

		public decimal Quantity { get; set; }

		public short RowNum { get; set; }

		public int RowId { get; set; }

		public string Barcode { get; set; }

		public string CPACode { get; set; }

		public bool IsRowMarkable { get; set; }

		public string MarkingStatus { get; set; }

		public HashSet<string> MarkingList { get; set; }
	}

	public int MaxRowId { get; set; }

	public string EMarkReceiptNumber { get; set; }

	public List<EMarkReceiptSpecificationRow> MTList { get; set; }
}
