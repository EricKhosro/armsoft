using System;
using System.ComponentModel.DataAnnotations;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class GoodsRemainsDataProvider : DataProvider<GoodsRemainsDataProvider.DataRow, GoodsRemainsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string Caption { get; set; }

		public string FullCaption { get; set; }

		public string UnitCode { get; set; }

		public string UnitBrief { get; set; }

		public string Storage { get; set; }

		public string StorageCaption { get; set; }

		public string Group { get; set; }

		public string GroupCaption { get; set; }

		public string GroupCodeCaption { get; set; }

		public string Description { get; set; }

		public decimal Quantity { get; set; }

		public decimal QuantityInTransit { get; set; }

		public decimal? Cost { get; set; }

		public decimal CostSumm { get; set; }

		public decimal? CostWithVAT { get; set; }

		public decimal CostSummWithVAT { get; set; }

		public decimal? CurrCost { get; set; }

		public decimal CurrCostSumm { get; set; }

		public string MTExpMethod { get; set; }

		public decimal MinQty { get; set; }

		public decimal DiffQty { get; set; }

		public decimal MaxQty { get; set; }

		public decimal OrderQty { get; set; }

		public string OrderComment { get; set; }

		public DateOnly IncomeDate { get; set; }

		public int PartyPartID { get; set; } = -1;

		public string PartyPartCaption { get; set; }

		public string PartyPartTaxCode { get; set; }

		public string PartyContractCaption { get; set; }

		public string PartyDocNum { get; set; }

		public string PartyDocType { get; set; }

		public DateOnly? PartyDocUsableDate { get; set; }

		public string PartyDocCountry { get; set; }

		public string PartyDocProducer { get; set; }

		public string PartyDocPropertyType1 { get; set; }

		public string PartyDocPropertyValue1Code { get; set; }

		public string PartyDocPropertyValue1 { get; set; }

		public string PartyDocPropertyType2 { get; set; }

		public string PartyDocPropertyValue2Code { get; set; }

		public string PartyDocPropertyValue2 { get; set; }

		public string PartyContractCode { get; set; }

		public string PartyDocProducerCode { get; set; }

		public string PartyDocCountryCode { get; set; }

		public string PartyProperty1TypeCode { get; set; }

		public string PartyProperty2TypeCode { get; set; }

		public byte[] Image { get; set; }

		public Guid PartyBase { get; set; }

		public int PartyRow { get; set; }

		public bool ImageExists { get; set; }

		public string ImgStr => MTID + "," + UnitCode.Trim();

		public string Parent { get; set; }

		public bool Leaf { get; set; } = true;

		public string KeyFieldForTree { get; set; }

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
		[Required]
		public DateOnly RemDate { get; set; }

		public string Storage { get; set; }

		public string MTGroup { get; set; }

		public string MTCode { get; set; }

		public string PriceListTypes { get; set; }

		public bool ShowByAditionalQntUnits { get; set; }

		public bool ShowSumms { get; set; }

		public bool ShowSummsWithVAT { get; set; }

		public bool ShowCurrSumms { get; set; }

		public bool ShowByDifferentParties { get; set; }

		public bool DetailByStorage { get; set; }

		public string MinQtys { get; set; }

		public string SubTotalType { get; set; }
	}

	public override string Name => "GoodsRemains";

	public GoodsRemainsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
