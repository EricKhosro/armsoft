using System;
using System.ComponentModel.DataAnnotations;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class GoodsRemsDataProvider : DataProvider<GoodsRemsDataProvider.DataRow, GoodsRemsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string Caption { get; set; }

		public string FullCaption { get; set; }

		public string Description { get; set; }

		public bool VAT { get; set; }

		public decimal Qty { get; set; }

		public string UnitCode { get; set; }

		public string UnitBrief { get; set; }

		public string AltUnitCode { get; set; }

		public string AltUnitBrief { get; set; }

		public decimal CostSumm { get; set; }

		public decimal CostSummVAT { get; set; }

		public decimal CurrCostSumm { get; set; }

		public decimal CurrCost { get; set; }

		public decimal Price { get; set; }

		public decimal Summ { get; set; }
	}

	public class Param
	{
		[Required]
		public DateOnly RemDate { get; set; }

		public string Storage { get; set; }

		public string MTGroup { get; set; }

		public string MTCode { get; set; }

		public string PriceListTypes { get; set; }

		public string PriceListType { get; set; }

		public bool ShowAltUnits { get; set; }

		public bool ShowZeroRows { get; set; }
	}

	public override string Name => "GoodsRems";

	public GoodsRemsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
