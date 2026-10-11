using ArmSoft.SME.Trade.Common.Models.Documents;

namespace ArmSoft.SME.Trade.UI.Models;

public class MTBillReceiptRowItem
{
	public string ItemCode { get; }

	public string ItemName { get; }

	public decimal? Quantity { get; }

	public decimal? Price { get; }

	public decimal? Sum { get; }

	public MTBillReceiptRowItem(MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow)
	{
		ItemCode = mTInvoiceSpecificationRow.ItemCode;
		ItemName = mTInvoiceSpecificationRow.ItemName;
		Quantity = mTInvoiceSpecificationRow.Quantity;
		Price = mTInvoiceSpecificationRow.Price;
		Sum = mTInvoiceSpecificationRow.Summa;
	}

	public MTBillReceiptRowItem(MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow)
	{
		ItemCode = mTRetFromCustSpecificationRow.ItemCode;
		ItemName = mTRetFromCustSpecificationRow.ItemName;
		Quantity = mTRetFromCustSpecificationRow.Quantity;
		Price = mTRetFromCustSpecificationRow.Price;
		Sum = mTRetFromCustSpecificationRow.Summa;
	}
}
