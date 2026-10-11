using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class GoodsCostCalculationDataProvider : DataProvider<GoodsCostCalculationDataProvider.DataRow, GoodsCostCalculationDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithISN
	{
		public int GoodID { get; set; }

		public string GoodCode { get; set; }

		public string Caption { get; set; }

		public string UnitBrief { get; set; }

		public decimal Cost { get; set; }

		public decimal VATCost { get; set; }

		public decimal CURCost { get; set; }

		public decimal Quantity { get; set; }

		public decimal Summ { get; set; }

		public decimal VATSumm { get; set; }

		public decimal CURSumm { get; set; }

		public bool VAT { get; set; }

		public decimal Total { get; set; }

		public decimal TotalVAT { get; set; }

		public decimal TotalCUR { get; set; }

		public decimal GoodUnitCost { get; set; }

		public decimal GoodUnitCostVAT { get; set; }

		public decimal GoodUnitCostCUR { get; set; }

		public int RowID { get; set; }

		public Guid ISN { get; set; }

		public Guid GetDocISN()
		{
			return ISN;
		}

		public short GetDocType()
		{
			return -1;
		}

		public bool Matched(object key)
		{
			return (Guid)key == ISN;
		}
	}

	public class Param
	{
		public Guid BaseISN { get; set; }

		public bool ShowSums { get; set; }

		public bool ShowSumsWithVAT { get; set; }

		public bool ShowCurrSums { get; set; }
	}

	public override string Name => "GoodsCostCalculation";

	public GoodsCostCalculationDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
