using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class ReceivedService : TradeDocument
{
	public class ReceivedServiceSpecificationRow
	{
		public string ServCode { get; set; } = string.Empty;

		public string ServName { get; set; } = string.Empty;

		public int ServID { get; set; } = -1;

		public decimal Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal PriceVAT { get; set; }

		public bool VAT { get; set; }

		public decimal Summa { get; set; }

		public short RowNum { get; set; }

		public string FullName { get; set; } = string.Empty;

		public string UnitCode { get; set; } = string.Empty;

		public string UnitBrief { get; set; } = string.Empty;

		public bool UnitFraction { get; set; }
	}

	public string PriceType { get; set; } = string.Empty;

	public decimal SummVAT { get; set; }

	public List<ReceivedServiceSpecificationRow> ReceivedServiceList { get; set; } = new List<ReceivedServiceSpecificationRow>();
}
