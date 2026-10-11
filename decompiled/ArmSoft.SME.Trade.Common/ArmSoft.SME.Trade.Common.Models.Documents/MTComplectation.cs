using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTComplectation : TradeDocument
{
	public class MTComplectationSpecificationRow
	{
		public string ItemType { get; set; }

		public string ItemCode { get; set; }

		public int? ItemID { get; set; }

		public short RowNum { get; set; }

		public decimal? Quantity { get; set; }

		public string UnitCode { get; set; }

		public decimal? Summa { get; set; }

		public bool? IsPartySelected { get; set; }

		public int? RowID { get; set; }

		public string ItemName { get; set; }

		public string UnitBrief { get; set; }
	}

	public List<PartyRow> MTExpDocSelectedPartiesList { get; set; } = new List<PartyRow>();

	public int? MaxRowID { get; set; }

	public string StorageExpense { get; set; }

	public string StorageIncome { get; set; }

	public string MTComplectCode { get; set; }

	public decimal? Quantity { get; set; }

	public string QuantityUnit { get; set; }

	public DateOnly? UseLife { get; set; }

	public string Country { get; set; }

	public string Producer { get; set; }

	public string Property1 { get; set; }

	public string Property2 { get; set; }

	public List<MTComplectationSpecificationRow> ComplectPartsList { get; set; } = new List<MTComplectationSpecificationRow>();
}
