using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class GoodsOperationsDataProvider : DataProvider<GoodsOperationsDataProvider.DataRow, GoodsOperationsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithISN
	{
		public DateOnly OpDate { get; set; }

		public string DocNum { get; set; }

		public Guid Base { get; set; }

		public short DocType { get; set; }

		public string DocTypeCaption { get; set; }

		public int DocPartID { get; set; }

		public int GoodId { get; set; }

		public string GoodCode { get; set; }

		public string Caption { get; set; }

		public string Storage { get; set; }

		public string StorageCaption { get; set; }

		public string Unit { get; set; }

		public string UnitBrief { get; set; }

		public int PartIDCorrObject { get; set; }

		public string CorrObjectCode { get; set; }

		public string CorrObjectCaption { get; set; }

		public string CorrObjectTaxCode { get; set; }

		public string StorageCorrCode { get; set; }

		public string StorageCorrCaption { get; set; }

		public decimal Quantity { get; set; }

		public decimal Cost { get; set; }

		public decimal CostSumm { get; set; }

		public decimal CostWithVat { get; set; }

		public decimal CostSummWithVat { get; set; }

		public decimal VATSumm { get; set; }

		public decimal CurrCost { get; set; }

		public decimal CurrCostSumm { get; set; }

		public decimal SalePrice { get; set; }

		public decimal SaleSumm { get; set; }

		public decimal VATSalePrice { get; set; }

		public decimal VATSaleSumm { get; set; }

		public decimal SaleVATSumm { get; set; }

		public decimal SaleCurPrice { get; set; }

		public decimal SaleCurSumm { get; set; }

		public string Comment { get; set; }

		public string Group { get; set; }

		public string GroupCaption { get; set; }

		public string GroupCodeCaption { get; set; }

		public string Body { get; set; }

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

		public string RowDescription { get; set; }

		public string PartyContractCode { get; set; }

		public string PartyDocProducerCode { get; set; }

		public string PartyDocCountryCode { get; set; }

		public string PartyProperty1TypeCode { get; set; }

		public string PartyProperty2TypeCode { get; set; }

		public Guid PartyBase { get; set; }

		public int PartyRow { get; set; }

		public decimal QuantityDb { get; set; }

		public decimal CostSummDb { get; set; }

		public decimal CostSummWithVATDb { get; set; }

		public decimal CurrCostSummDb { get; set; }

		public decimal QuantityCr { get; set; }

		public decimal CostSummCr { get; set; }

		public decimal CostSummWithVATCr { get; set; }

		public decimal CurrCostSummCr { get; set; }

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

		public Guid GetDocISN()
		{
			return Base;
		}

		public short GetDocType()
		{
			return DocType;
		}

		public bool Matched(object key)
		{
			return (Guid)key == Base;
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

		public bool ShowSumms { get; set; }

		public bool ShowSummsWithVAT { get; set; }

		public bool ShowCurrSumms { get; set; }

		public bool ShowSalePrice { get; set; }

		public bool ShowParties { get; set; }

		public Guid DocISN { get; set; }

		public bool ForByPartiesJournal { get; set; }
	}

	public override string Name => "GoodsOperations";

	public GoodsOperationsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
