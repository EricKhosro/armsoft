using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Common.Models.ASReport;

public class MTAccountReportResult
{
	public class TurnRow
	{
		public decimal QtyIn { get; set; }

		public decimal SumIn { get; set; }

		public decimal SumVATIn { get; set; }

		public decimal SumCurrIn { get; set; }

		public decimal QtyOut { get; set; }

		public decimal SumOut { get; set; }

		public decimal SumVATOut { get; set; }

		public decimal SumCurrOut { get; set; }

		public decimal QtyBegin { get; set; }

		public decimal SumBegin { get; set; }

		public decimal SumVATBegin { get; set; }

		public decimal SumCurrBegin { get; set; }

		public decimal SaleSumBegin { get; set; }

		public decimal SaleSumVATBegin { get; set; }

		public decimal SaleSumCurrBegin { get; set; }
	}

	public class OperationRow : IRowWithISN
	{
		public Guid Base { get; set; }

		public short DocType { get; set; }

		public int DocPartID { get; set; }

		public string DocNum { get; set; } = string.Empty;

		public string DocTypeCaption { get; set; } = string.Empty;

		public string Comment { get; set; } = string.Empty;

		public string Storage { get; set; } = string.Empty;

		public string GoodCode { get; set; } = string.Empty;

		public DateOnly OpDate { get; set; }

		public string CorrObjectCode { get; set; } = string.Empty;

		public string CorrObjectCaption { get; set; } = string.Empty;

		public decimal Quantity { get; set; }

		public decimal QuantityDb { get; set; }

		public decimal QuantityCr { get; set; }

		public decimal Cost { get; set; }

		public decimal CostSumm { get; set; }

		public decimal CostSummDb { get; set; }

		public decimal CostSummCr { get; set; }

		public decimal CostWithVat { get; set; }

		public decimal CostSummWithVat { get; set; }

		public decimal CostSummWithVATDb { get; set; }

		public decimal CostSummWithVATCr { get; set; }

		public decimal CurrCost { get; set; }

		public decimal CurrCostSumm { get; set; }

		public decimal CurrCostSummDb { get; set; }

		public decimal CurrCostSummCr { get; set; }

		public decimal SalePrice { get; set; }

		public decimal SaleCurPrice { get; set; }

		public decimal SaleSumm { get; set; }

		public decimal VATSalePrice { get; set; }

		public decimal VATSaleSumm { get; set; }

		public decimal SaleCurSumm { get; set; }

		public string PartyPartCaption { get; set; } = string.Empty;

		public string PartyContractCaption { get; set; } = string.Empty;

		public string PartyDocNum { get; set; } = string.Empty;

		public short PartyDocType { get; set; }

		public DateOnly? PartyDocUsableDate { get; set; }

		public DateOnly IncomeDate { get; set; }

		public string PartyDocCountry { get; set; } = string.Empty;

		public string PartyDocProducer { get; set; } = string.Empty;

		public string PartyDocPropertyType1 { get; set; } = string.Empty;

		public string PartyDocPropertyValue1 { get; set; } = string.Empty;

		public string PartyDocPropertyType2 { get; set; } = string.Empty;

		public string PartyDocPropertyValue2 { get; set; } = string.Empty;

		public Guid GetDocISN()
		{
			return Base;
		}

		public short GetDocType()
		{
			return DocType;
		}
	}

	public List<OperationRow> OperationRows { get; set; }

	public List<TurnRow> TurnRows { get; set; }
}
