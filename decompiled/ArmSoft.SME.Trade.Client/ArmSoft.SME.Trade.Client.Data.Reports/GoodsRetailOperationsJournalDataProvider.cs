using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class GoodsRetailOperationsJournalDataProvider : DataProvider<GoodsRetailOperationsJournalDataProvider.DataRow, GoodsRetailOperationsJournalDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithISN
	{
		public Guid ISN { get; set; }

		public DateOnly Date { get; set; }

		public string DocNum { get; set; }

		public Guid Base { get; set; }

		public short DocType { get; set; }

		public string DocTypeCaption { get; set; }

		public string Storage { get; set; }

		public string StorageCaption { get; set; }

		public int GoodId { get; set; }

		public string GoodCode { get; set; }

		public string Caption { get; set; }

		public string Unit { get; set; }

		public string UnitBrief { get; set; }

		public decimal Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal PriceSum { get; set; }

		public decimal Cost { get; set; }

		public decimal CostSum { get; set; }

		public decimal CostWithVAT { get; set; }

		public decimal CostSumWithVAT { get; set; }

		public decimal VATSum { get; set; }

		public decimal CurCost { get; set; }

		public decimal CurrCostSumm { get; set; }

		public string Comment { get; set; }

		public string Group { get; set; }

		public string GroupCaption { get; set; }

		public string GroupCodeCaption { get; set; }

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
		public DateOnly? StartDate { get; set; }

		public DateOnly? EndDate { get; set; }

		public string Storage { get; set; }

		public string GoodGroup { get; set; }

		public string GoodCode { get; set; }

		public string DocTypes { get; set; }

		public bool ShowAltUnit { get; set; }

		public bool ShowSums { get; set; }

		public bool ShowSumsWithVat { get; set; }

		public bool ShowCurrSums { get; set; }

		public string SubTotalType { get; set; }

		public Guid DocISN { get; set; }
	}

	public override string Name => "GoodsRetailOperationsJournal";

	public GoodsRetailOperationsJournalDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
