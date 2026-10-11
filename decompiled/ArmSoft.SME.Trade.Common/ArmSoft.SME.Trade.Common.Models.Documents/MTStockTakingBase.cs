using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTStockTakingBase : TradeDocument
{
	public class MTStockTakingSpecificationRow
	{
		public string MTCode { get; set; }

		public int MTID { get; set; }

		public string UnitCode { get; set; }

		public decimal AccQuantity { get; set; }

		public decimal ActualQuantity { get; set; }

		public decimal VarianceQuantity { get; set; }

		public decimal AccSum { get; set; }

		public decimal Price { get; set; }

		public decimal SalePrice { get; set; }

		public decimal ActualSum { get; set; }

		public decimal SaleSum { get; set; }

		public short RowNum { get; set; }

		public string MTName { get; set; }

		public string FullName { get; set; }

		public string UnitBrief { get; set; }

		public bool VAT { get; set; }
	}

	public string GoodGroup { get; set; }

	public string PriceType { get; set; }

	public List<MTStockTakingSpecificationRow> MTList { get; set; } = new List<MTStockTakingSpecificationRow>();
}
