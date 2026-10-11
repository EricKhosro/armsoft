using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class GoodsStockTakingListDataProvider : DataProvider<GoodsStockTakingListDataProvider.DataRow, GoodsStockTakingListDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithISN
	{
		public Guid ISN { get; set; }

		public DateOnly DocDate { get; set; }

		public string DocNum { get; set; } = string.Empty;

		public byte DocState { get; set; }

		public string DocStateCaption { get; set; } = string.Empty;

		public string Comment { get; set; } = string.Empty;

		public string UserName { get; set; } = string.Empty;

		public DateTime ChangeDate { get; set; }

		public string Storage { get; set; } = string.Empty;

		public string MTCode { get; set; } = string.Empty;

		public string MTCaption { get; set; } = string.Empty;

		public string UnitCode { get; set; } = string.Empty;

		public string UnitBrief { get; set; } = string.Empty;

		public decimal AccQuantity { get; set; }

		public decimal ActualQuantity { get; set; }

		public decimal VarianceQuantity { get; set; }

		public decimal AccSumma { get; set; }

		public decimal ActualSumma { get; set; }

		public decimal Price { get; set; }

		public decimal SalePriceRetail { get; set; }

		public decimal SalePrice { get; set; }

		public decimal SaleSumma { get; set; }

		public short DocType { get; set; }

		public string DocTypeCaption { get; set; }

		public short GetDocType()
		{
			return DocType;
		}

		public Guid GetDocISN()
		{
			return ISN;
		}

		public bool Matched(object Key)
		{
			return (Guid)Key == ISN;
		}
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string Storage { get; set; } = string.Empty;

		public string DocState { get; set; } = string.Empty;
	}

	public override string Name => "GoodsStockTakingList";

	public GoodsStockTakingListDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
