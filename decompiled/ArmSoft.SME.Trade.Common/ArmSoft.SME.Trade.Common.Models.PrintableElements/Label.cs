using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;

namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class Label : PrintableElement
{
	public class GoodLabelRow
	{
		public MaterialCacheRow MaterialeRow { get; set; }

		public string BarCode { get; set; } = string.Empty;

		public string BarTitle { get; set; } = string.Empty;

		public string AltUnitBarCode { get; set; } = string.Empty;

		public decimal Quantity { get; set; }

		public CodeName Property1Value { get; set; }

		public CodeName Property2Value { get; set; }
	}

	public class PrintLabelWindowGridRow
	{
		public bool Include { get; set; } = true;

		public int MTID { get; set; } = -1;

		public int RowID { get; set; } = -1;

		public string MTCode { get; set; } = string.Empty;

		public string MTName { get; set; } = string.Empty;

		public string Unit { get; set; } = string.Empty;

		public string UnitBrief { get; set; } = string.Empty;

		public string UnitFromDoc { get; set; } = string.Empty;

		public string UnitBriefFromDoc { get; set; } = string.Empty;

		public decimal Price { get; set; }

		public decimal Quantity { get; set; }

		public DateOnly? UseLife { get; set; }

		public string Country { get; set; } = string.Empty;

		public string Producer { get; set; } = string.Empty;

		public string Property1 { get; set; } = string.Empty;

		public string Property1Caption { get; set; } = string.Empty;

		public string Property2 { get; set; } = string.Empty;

		public string Property2Caption { get; set; } = string.Empty;
	}

	public string Caption { get; set; }

	public List<GoodLabelRow> SelectedGoods { get; set; }

	public int Qty { get; set; }

	public DateOnly Date { get; set; }

	public string PriceType { get; set; }

	public string DiscountPriceType { get; set; }

	public string AvgPriceType { get; set; }

	public string TemplateName { get; set; }

	public bool PrintAltUnit { get; set; }

	public bool GroupByCode { get; set; }

	public bool TakeRowQty { get; set; }

	public List<PrintLabelWindowGridRow> GoodsList { get; set; }

	public TradeDocument Doc { get; set; }
}
