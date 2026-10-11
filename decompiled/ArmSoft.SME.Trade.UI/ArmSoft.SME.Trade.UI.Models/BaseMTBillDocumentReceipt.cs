using System;
using Ardalis.SmartEnum;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Models;

public class BaseMTBillDocumentReceipt
{
	public string DocNum { get; set; }

	public DateOnly DocDate { get; set; }

	public short DocTypeCode { get; set; }

	public string DocTypeName { get; set; }

	public decimal? TotalSum { get; set; }

	public string BuyerCardCode { get; set; }

	public BaseMTBillDocumentReceipt(TradeDocument tradeDocument)
	{
		DocNum = tradeDocument.DocNum;
		DocDate = tradeDocument.DocDate;
		DocTypeCode = tradeDocument.POType;
		DocTypeName = SmartEnum<DocumentTypeSE, short>.FromValue(DocTypeCode)?.Caption;
		TotalSum = tradeDocument.Summ;
		if (tradeDocument.PartnerID != -1 && !string.IsNullOrWhiteSpace(tradeDocument.PartnerContractGroup?.ContractCode))
		{
			BuyerCardCode = TradeSettings.TradeCacheService.Partner.GetContract(tradeDocument.PartnerID, tradeDocument.PartnerContractGroup.ContractCode)?.DiscountCard;
		}
	}

	public BaseMTBillDocumentReceipt()
	{
	}
}
