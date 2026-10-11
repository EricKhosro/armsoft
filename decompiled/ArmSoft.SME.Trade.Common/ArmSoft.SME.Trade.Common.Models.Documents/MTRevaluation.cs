using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTRevaluation : TradeDocument
{
	public class MTRevaluationSpecificationRow
	{
		public short RowNum { get; set; }

		public string Storage { get; set; } = "";

		public string MTCode { get; set; } = string.Empty;

		public int MTID { get; set; } = -1;

		public string UnitCode { get; set; } = string.Empty;

		public decimal Quantity { get; set; }

		public decimal Cost { get; set; }

		public decimal CostWithVAT { get; set; }

		public decimal CurCostWithVAT { get; set; }

		public decimal NewCost { get; set; }

		public decimal NewCostWithVAT { get; set; }

		public decimal NewCurCostWithVAT { get; set; }

		public DateOnly IncomeDate { get; set; }

		public string IncomeDocDescr { get; set; } = "";

		public int RowID { get; set; }

		public Guid PartyBase { get; set; }

		public int PartyRow { get; set; }

		public int PartID { get; set; }

		public string PartnerName { get; set; }

		public string ContractName { get; set; }

		public string ContractCode { get; set; }

		public string ExpenseMethod { get; set; } = "";

		public DateOnly? UseLife { get; set; }

		public string CountryName { get; set; } = "";

		public string CountryCode { get; set; } = "";

		public string ProducerCode { get; set; } = "";

		public string ProducerName { get; set; } = "";

		public string Property1Code { get; set; } = "";

		public string Property2Code { get; set; } = "";

		public string Property1Value { get; set; } = "";

		public string Property2Value { get; set; } = "";

		public bool UnitFraction { get; set; }

		public string MTName { get; set; } = "";

		public bool MTVAT { get; set; }

		public string UnitBrief { get; set; } = "";

		public decimal CostSumm { get; set; }

		public decimal CostSummWithVAT { get; set; }

		public decimal CurCostSummWithVAT { get; set; }

		public decimal VATSumm { get; set; }
	}

	public List<PartyRow> MTExpDocSelectedPartiesList { get; set; } = new List<PartyRow>();

	public string PriceFormationOpType { get; set; } = "";

	public decimal Coefficient { get; set; }

	public string PriceType { get; set; } = "";

	public decimal DistrSum { get; set; }

	public string DistrSumBy { get; set; } = "";

	public string GridViewColsType { get; set; } = "";

	public decimal Price { get; set; }

	public List<MTRevaluationSpecificationRow> MTList { get; set; } = new List<MTRevaluationSpecificationRow>();
}
