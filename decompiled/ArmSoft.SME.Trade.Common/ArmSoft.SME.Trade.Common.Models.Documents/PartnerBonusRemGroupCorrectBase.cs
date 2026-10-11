using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class PartnerBonusRemGroupCorrectBase : TradeDocument
{
	public class PartnerBonusRemSpecificationRow
	{
		public int? PartID { get; set; }

		public string PartCode { get; set; }

		public string PartName { get; set; }

		public string Contract { get; set; }

		public decimal? Summa { get; set; }

		public decimal? SummaNCC { get; set; }

		public short? RowNum { get; set; }

		public Guid? TransactionISN { get; set; }
	}

	public List<PartnerBonusRemSpecificationRow> PartRemList { get; set; }
}
