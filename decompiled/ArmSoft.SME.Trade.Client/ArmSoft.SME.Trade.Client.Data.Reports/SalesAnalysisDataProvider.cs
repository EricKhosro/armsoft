using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class SalesAnalysisDataProvider : DataProvider<SalesAnalysisDataProvider.DataRow, SalesAnalysisDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithISN
	{
		public DateOnly? DocDate { get; set; }

		public Guid DocISN { get; set; }

		public string DocNum { get; set; }

		public short DocType { get; set; }

		public string DocTypeCaption { get; set; }

		public string DocCurrCode { get; set; }

		public string Contract { get; set; }

		public int RowId { get; set; }

		public string DocVATType { get; set; }

		public string DocVATTypeCaption { get; set; }

		public string ECRCheckNum { get; set; }

		public string OpName { get; set; }

		public string ItemTypeCode { get; set; }

		public string ItemType { get; set; }

		public int GoodID { get; set; }

		public string GoodCode { get; set; }

		public string GoodCaption { get; set; }

		public string GoodFullCaption { get; set; }

		public string GoodDescription { get; set; }

		public string UnitCode { get; set; }

		public string UnitBrief { get; set; }

		public string Storage { get; set; }

		public string StorageCaption { get; set; }

		public string CashDesk { get; set; }

		public string CashDeskCaption { get; set; }

		public int PartnerId { get; set; }

		public string PartnerCode { get; set; }

		public string PartnerCaption { get; set; }

		public decimal Quantity { get; set; }

		public decimal Summ { get; set; }

		public decimal CostPrice { get; set; }

		public decimal SalePrice { get; set; }

		public decimal SaleSumm { get; set; }

		public decimal Profit { get; set; }

		public decimal ProfitPercentBySale { get; set; }

		public decimal ProfitPercentByCost { get; set; }

		public decimal DiscountSum { get; set; }

		public decimal DiscountPercent { get; set; }

		public decimal SummWithVAT { get; set; }

		public decimal CostPriceWithVAT { get; set; }

		public decimal SalePriceWithVAT { get; set; }

		public decimal SaleSummWithVAT { get; set; }

		public decimal ProfitWithVAT { get; set; }

		public decimal ProfitPercentBySaleWithVAT { get; set; }

		public decimal ProfitPercentByCostWithVAT { get; set; }

		public decimal DiscountSummWithVAT { get; set; }

		public decimal DiscountPercentWithVAT { get; set; }

		public decimal CostVATSumm { get; set; }

		public decimal CostVATPrice { get; set; }

		public decimal SaleVATSumm { get; set; }

		public decimal DiscountVATSumm { get; set; }

		public decimal CurSumm { get; set; }

		public decimal CurrCostPrice { get; set; }

		public decimal SaleCurPrice { get; set; }

		public decimal SaleCurSumm { get; set; }

		public decimal CurProfit { get; set; }

		public decimal CurProfitPercentBySale { get; set; }

		public decimal CurProfitPercentByCost { get; set; }

		public decimal DiscountCurSum { get; set; }

		public decimal DiscountCurPercent { get; set; }

		public decimal InitialPrice { get; set; }

		public decimal InitialPriceVAT { get; set; }

		public decimal InitialPriceCurr { get; set; }

		public decimal InitialSumm { get; set; }

		public decimal InitialSummVAT { get; set; }

		public decimal InitialSummCurr { get; set; }

		public decimal UsedBonus { get; set; }

		public decimal AccumulatedBonus { get; set; }

		public string Group { get; set; }

		public string SalesConsultantCode { get; set; }

		public string SalesConsultantCaption { get; set; }

		public string Comment { get; set; }

		public DateOnly? IncomeDate { get; set; }

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

		public string PartyDocPropertyValue1 { get; set; }

		public string PartyDocPropertyType2 { get; set; }

		public string PartyDocPropertyValue2 { get; set; }

		public Guid PartyBase { get; set; }

		public int PartyRow { get; set; }

		public decimal EnvFeeSumma { get; set; }

		public string RowDescription { get; set; }

		public DateOnly? BaseDocDate { get; set; }

		public string TaxExportCaption { get; set; }

		public string BaseDocNum { get; set; }

		public string BuyerBussAddress { get; set; }

		public string ShiftNumber { get; set; } = "";

		public MaterialCacheRow MaterialCacheRow { get; set; }

		public PartnerCacheRow PartnerCacheRow { get; set; }

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
			else if (source.StartsWith("Partner"))
			{
				if (PartnerCacheRow != null)
				{
					return PartnerCacheRow.GetReferenceValue(source);
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
			return DocISN;
		}

		public bool Matched(object Key)
		{
			return (Guid)Key == DocISN;
		}
	}

	public class Param
	{
		public DateOnly? StartDate { get; set; }

		public DateOnly? EndDate { get; set; }

		public string Storage { get; set; }

		public string GoodGroup { get; set; }

		public string GoodCode { get; set; }

		public string PartnerType { get; set; }

		public string PartnerCode { get; set; }

		public string ItemType { get; set; }

		public bool ShowAltUnit { get; set; }

		public bool ShowNccSumm { get; set; }

		public bool ShowCurrSumm { get; set; }

		public bool ShowNCCVATSumm { get; set; }

		public bool ShowParties { get; set; }

		public bool ShowPrices { get; set; }

		public bool ShowDiscount { get; set; }

		public bool ShowBonusPoints { get; set; }

		public bool ShowVATSumms { get; set; }

		public string GroupBy { get; set; }

		public Guid DocISN { get; set; }
	}

	public override string Name => "SalesAnalysis";

	public SalesAnalysisDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
