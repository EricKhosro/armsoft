using System.Collections.Generic;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common.Models.Documents;

namespace ArmSoft.SME.Trade.UI.Models;

public class MTBillDocumentReceipt : BaseMTBillDocumentReceipt
{
	public decimal? CreditSum { get; set; }

	public decimal? ChangeSum { get; set; }

	public decimal? DiscountSum { get; set; }

	public decimal? CashSum { get; set; }

	public decimal? NonCashSum { get; set; }

	public decimal? PrePaymentSum { get; set; }

	public string BuyerName { get; set; }

	public string ECRCheckNumber { get; set; }

	public string CashierName { get; set; }

	public string SalesConsultantName { get; set; }

	public List<MTBillReceiptRowItem> MTList { get; set; }

	public MTBillDocumentReceipt(MTBill mTBill)
		: base(mTBill)
	{
		CashSum = mTBill.SummaCash;
		NonCashSum = mTBill.SummaPlCard;
		PrePaymentSum = mTBill.SummaPrePayment;
		CreditSum = mTBill.SummaCredit;
		ChangeSum = mTBill.SummaChange;
		ECRCheckNumber = mTBill.ECRCheckNum;
		if (mTBill.PartnerID != -1 && !string.IsNullOrWhiteSpace(mTBill.PartnerContractGroup?.ContractCode))
		{
			BuyerCardCode = TradeSettings.TradeCacheService.Partner.GetContract(mTBill.PartnerID, mTBill.PartnerContractGroup.ContractCode)?.DiscountCard;
		}
		BuyerName = mTBill.BuyerName;
		CashierName = ((!mTBill.OwnerID.HasValue) ? "" : (Settings.CoreCacheService.User.GetItem(mTBill.OwnerID.Value)?.Description ?? ""));
		if (!string.IsNullOrWhiteSpace(mTBill.SalesConsultant))
		{
			SalesConsultantName = TradeSettings.TradeCacheService.Employee.GetItem(mTBill.SalesConsultant)?.Caption;
		}
		List<MTBillReceiptRowItem> list = new List<MTBillReceiptRowItem>();
		decimal value = 0m;
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT in mTBill.MTList)
		{
			if (!mT.IsDeleted)
			{
				MTBillReceiptRowItem item = new MTBillReceiptRowItem(mT);
				list.Add(item);
				value += mT.DiscountSumma;
			}
		}
		MTList = list;
		DiscountSum = value;
	}

	public MTBillDocumentReceipt(MTBillReturn mTBillReturn)
		: base(mTBillReturn)
	{
		CashSum = mTBillReturn.ECRReturnByCashSum;
		NonCashSum = mTBillReturn.ECRReturnByCardSum;
		PrePaymentSum = mTBillReturn.ECRReturnByPrePaymentSum;
		CreditSum = mTBillReturn.SummaCredit;
		ChangeSum = mTBillReturn.SummaChange;
		ECRCheckNumber = mTBillReturn.ECRCheckNum;
		if (mTBillReturn.PartnerID != -1 && !string.IsNullOrWhiteSpace(mTBillReturn.PartnerContractGroup?.ContractCode))
		{
			BuyerCardCode = TradeSettings.TradeCacheService.Partner.GetContract(mTBillReturn.PartnerID, mTBillReturn.PartnerContractGroup.ContractCode)?.DiscountCard;
		}
		CashierName = ((!mTBillReturn.OwnerID.HasValue) ? "" : (Settings.CoreCacheService.User.GetItem(mTBillReturn.OwnerID.Value)?.Description ?? ""));
		if (!string.IsNullOrWhiteSpace(mTBillReturn.SalesConsultant))
		{
			SalesConsultantName = TradeSettings.TradeCacheService.Employee.GetItem(mTBillReturn.SalesConsultant)?.Caption;
		}
		List<MTBillReceiptRowItem> list = new List<MTBillReceiptRowItem>();
		decimal value = 0m;
		foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT in mTBillReturn.MTList)
		{
			if (mT.IsDeleted != true)
			{
				MTBillReceiptRowItem item = new MTBillReceiptRowItem(mT);
				list.Add(item);
				value += mT.DiscountSumma.GetValueOrDefault();
			}
		}
		MTList = list;
		DiscountSum = value;
	}

	public MTBillDocumentReceipt(MTReturnFromCustomer mTReturnFromCustomer)
		: base(mTReturnFromCustomer)
	{
		CashSum = mTReturnFromCustomer.ECRReturnByCashSum;
		NonCashSum = mTReturnFromCustomer.ECRReturnByCardSum;
		PrePaymentSum = mTReturnFromCustomer.ECRReturnByPrePaymentSum;
		ECRCheckNumber = mTReturnFromCustomer.ECRCheckNum;
		if (mTReturnFromCustomer.PartnerID != -1 && !string.IsNullOrWhiteSpace(mTReturnFromCustomer.PartnerContractGroup?.ContractCode))
		{
			BuyerCardCode = TradeSettings.TradeCacheService.Partner.GetContract(mTReturnFromCustomer.PartnerID, mTReturnFromCustomer.PartnerContractGroup.ContractCode)?.DiscountCard;
		}
		BuyerName = mTReturnFromCustomer.BuyerName;
		CashierName = ((!mTReturnFromCustomer.OwnerID.HasValue) ? "" : (Settings.CoreCacheService.User.GetItem(mTReturnFromCustomer.OwnerID.Value)?.Description ?? ""));
		if (!string.IsNullOrWhiteSpace(mTReturnFromCustomer.SalesConsultant))
		{
			SalesConsultantName = TradeSettings.TradeCacheService.Employee.GetItem(mTReturnFromCustomer.SalesConsultant)?.Caption;
		}
		List<MTBillReceiptRowItem> list = new List<MTBillReceiptRowItem>();
		decimal value = 0m;
		foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT in mTReturnFromCustomer.MTList)
		{
			if (mT.IsDeleted != true)
			{
				MTBillReceiptRowItem item = new MTBillReceiptRowItem(mT);
				list.Add(item);
				value += mT.DiscountSumma.GetValueOrDefault();
			}
		}
		MTList = list;
		DiscountSum = value;
	}

	public MTBillDocumentReceipt()
	{
	}
}
