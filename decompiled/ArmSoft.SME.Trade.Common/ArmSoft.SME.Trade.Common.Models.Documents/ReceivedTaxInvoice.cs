using System;
using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class ReceivedTaxInvoice : TaxDocument
{
	public string CurrCode { get; set; }

	public int DocOrderNum { get; set; }

	public DateOnly? OldDocDate { get; set; }

	public DateOnly? InvoiceDate { get; set; }

	public string Owner { get; set; }
}
