using System;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class InvoiceForPay : MTInvoiceRetail
{
	public DateOnly? ContractDate { get; set; }

	public string PriceListType { get; set; }

	public string DiscountPriceListType { get; set; }

	public string InvForPayStates { get; set; }

	public Guid? IdAsGuid { get; set; }

	public string SupplierEMail { get; set; }
}
