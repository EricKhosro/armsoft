using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTPurchaseOrder : TradeDocument
{
	public class MTPurchaseOrderSpecificationRow
	{
		public string MTCode { get; set; }

		public short RowNum { get; set; }

		public int MTID { get; set; }

		public string UnitCode { get; set; }

		public decimal Quantity { get; set; }

		public int RowID { get; set; }

		public string MTName { get; set; }

		public string FullName { get; set; }

		public string UnitBrief { get; set; }
	}

	public int MaxRowId { get; set; }

	public string PartnerName { get; set; }

	public List<MTPurchaseOrderSpecificationRow> MTList { get; set; } = new List<MTPurchaseOrderSpecificationRow>();

	public string POStates { get; set; }

	public string SupplierEMail { get; set; }

	public string BuyerPassport { get; set; }
}
