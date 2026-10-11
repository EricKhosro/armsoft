using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class GoodsRetailTurnOverDataProvider : DataProvider<GoodsRetailTurnOverDataProvider.DataRow, GoodsRetailTurnOverDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int GoodID { get; set; }

		public string GoodCode { get; set; }

		public string Caption { get; set; }

		public string Unit { get; set; }

		public string UnitBrief { get; set; }

		public string Storage { get; set; }

		public string StorageCaption { get; set; }

		public decimal Price { get; set; }

		public decimal QtyBegin { get; set; }

		public decimal SumBegin { get; set; }

		public decimal QtyIn { get; set; }

		public decimal SumIn { get; set; }

		public decimal QtyOut { get; set; }

		public decimal SumOut { get; set; }

		public decimal QtyEnd
		{
			get
			{
				return QtyBegin + QtyIn - QtyOut;
			}
			set
			{
			}
		}

		public decimal SumEnd
		{
			get
			{
				return SumBegin + SumIn - SumOut;
			}
			set
			{
			}
		}

		public string GroupCodeCaption { get; set; }

		public decimal QTYInIncome { get; set; }

		public decimal SumInIncome { get; set; }

		public decimal QTYInReturnFromCustomer { get; set; }

		public decimal SumInReturnFromCustomer { get; set; }

		public decimal QTYInBillReturn { get; set; }

		public decimal SumInBillReturn { get; set; }

		public decimal QTYInMove { get; set; }

		public decimal SumInMove { get; set; }

		public decimal QTYInMTRetailRevaluation { get; set; }

		public decimal SumInMTRetailRevaluation { get; set; }

		public decimal QTYInMTRem { get; set; }

		public decimal SumInMTRem { get; set; }

		public decimal QTYOutSaleRetail { get; set; }

		public decimal SumOutSaleRetail { get; set; }

		public decimal QTYOutMTExpense { get; set; }

		public decimal SumOutMTExpense { get; set; }

		public decimal QTYOutMove { get; set; }

		public decimal SumOutMove { get; set; }

		public decimal QTYOutMTRetailRevaluation { get; set; }

		public decimal SumOutMTRetailRevaluation { get; set; }

		public MaterialCacheRow MaterialCacheRow { get; set; }

		public ProducerCacheRow ProducerCacheRow { get; set; }

		public override object ReferenceValue(string name, string source)
		{
			if (source.StartsWith("Material"))
			{
				if (MaterialCacheRow != null)
				{
					return MaterialCacheRow.GetReferenceValue(source);
				}
			}
			else if (source.StartsWith("Producer") && ProducerCacheRow != null)
			{
				return ProducerCacheRow.GetReferenceValue(source);
			}
			return null;
		}
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string Storage { get; set; }

		public string Group { get; set; }

		public string GoodCode { get; set; }

		public bool ShowAltUnit { get; set; }

		public bool ShowByPrices { get; set; }

		public bool DetailByStorage { get; set; }

		public bool DetailByOper { get; set; }

		public string SubTotalType { get; set; }
	}

	public override string Name => "GoodsRetailTurnOver";

	public GoodsRetailTurnOverDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
