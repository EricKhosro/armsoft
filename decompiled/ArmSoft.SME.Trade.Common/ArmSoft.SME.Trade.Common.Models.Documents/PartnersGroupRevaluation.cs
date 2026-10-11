using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class PartnersGroupRevaluation : TradeDocument
{
	public class RevaluationSpecificationRow
	{
		public int PartID { get; set; } = -1;

		public string PartCode { get; set; }

		public string PartName { get; set; } = string.Empty;

		public string Contract { get; set; }

		public string CurrCode { get; set; }

		public decimal Summa { get; set; }

		public short Trans { get; set; }
	}

	public List<RevaluationSpecificationRow> RevaluationList { get; set; } = new List<RevaluationSpecificationRow>();
}
