using System;

namespace ArmSoft.SME.Common.Models.ASReport;

public class InvoiceDetails
{
	public Guid ISN { get; set; }

	public DateTime Date { get; set; }

	public string TaxCode { get; set; }

	public string Passport { get; set; }

	public decimal Sum { get; set; }

	public string DocNum { get; set; }

	public short DocType { get; set; }

	public string BuyerBusinessAddress { get; set; }

	public string SupplierBusinessAddress { get; set; }

	public string Currency { get; set; }

	public short InvoiceType { get; set; }

	public string InvoiceNumber { get; set; }
}
