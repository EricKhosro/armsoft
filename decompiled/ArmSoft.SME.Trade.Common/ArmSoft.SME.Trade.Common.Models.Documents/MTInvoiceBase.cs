using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTInvoiceBase : TradeDocument
{
	public class MTInvoiceSpecificationRow : ISaleRow, IMarkableRow
	{
		public string ItemType { get; set; }

		public string Storage { get; set; }

		public string ItemCode { get; set; }

		public int ItemID { get; set; }

		public short RowNum { get; set; }

		public string ItemName { get; set; }

		public string UnitCode { get; set; }

		public string UnitBrief { get; set; }

		public decimal Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal Discount { get; set; }

		public decimal DiscountPrice { get; set; }

		public decimal UsedBonus { get; set; }

		public decimal AccumulatedBonus { get; set; }

		public decimal Summa { get; set; }

		public decimal DiscountSumma { get; set; }

		public decimal BeginSumma { get; set; }

		public string BarCode { get; set; }

		public decimal EnvFeePercent { get; set; }

		public decimal EnvFeeSumma { get; set; }

		public bool? VAT { get; set; }

		public string DealType { get; set; }

		public int RowID { get; set; }

		public Guid RowISN { get; set; }

		public bool? IsPartySelected { get; set; }

		public string RowDescription { get; set; }

		public string ExternalCode { get; set; }

		public string RowSpecField { get; set; }

		public bool IsDeleted { get; set; }

		public bool IsGift { get; set; }

		public string ShortName { get; set; }

		public string FullName { get; set; }

		public bool IsStorageRequired { get; set; } = true;

		public HashSet<string> MarkingList { get; set; } = new HashSet<string>();

		public bool IsRowMarkable { get; set; }

		public string MarkingStatus { get; set; }

		public string CPACode { get; set; } = "";
	}

	public int MaxRowID { get; set; }

	public string DealType { get; set; }

	public decimal SummVAT { get; set; }

	public decimal Discount { get; set; }

	public string SalesConsultant { get; set; }

	public string BuyerName { get; set; }

	public bool IsCheckLimitAnswerYes { get; set; }

	public decimal SumFromRound { get; set; }

	public string TaxInvoiceSeries { get; set; }

	public string TaxInvoiceNumber { get; set; }

	public bool IsDeferred { get; set; }

	public List<MTInvoiceSpecificationRow> MTList { get; set; } = new List<MTInvoiceSpecificationRow>();

	public List<PartyRow> MTExpDocSelectedPartiesList { get; set; } = new List<PartyRow>();

	public Dictionary<int, Guid> ROWISNDict { get; set; } = new Dictionary<int, Guid>();

	public bool MarkingsExported { get; set; }
}
