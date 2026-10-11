using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("4C95C6A0-F355-4A81-85F4-086B63ED2087")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTInvoiceRetail : ExchangeBase
{
	public string DocumentNumber { get; set; } = string.Empty;

	public DateTime DocumentDate { get; set; }

	public string Storage { get; set; } = string.Empty;

	public string BuyerCode { get; set; } = string.Empty;

	public string Contract { get; set; } = string.Empty;

	public decimal PartnerAmount { get; set; }

	public string TaxExportType { get; set; } = string.Empty;

	public string TaxInvoiceSeries { get; set; } = string.Empty;

	public string TaxInvoiceNumber { get; set; } = string.Empty;

	public DateTime? TaxInvoiceDate { get; set; }

	public string SalesConsultant { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;

	public string CurrencyCode { get; set; } = string.Empty;

	public string VATType { get; set; } = string.Empty;

	public decimal DiscountPercent { get; set; }

	public decimal TotalAmount { get; set; }

	public decimal AmountVAT { get; set; }

	public string SupplierName { get; set; } = string.Empty;

	public string SupplierLegalAddress { get; set; } = string.Empty;

	public string SupplierBusinessAddress { get; set; } = string.Empty;

	public string SupplierSettlementAccount { get; set; } = string.Empty;

	public string SupplierTaxCode { get; set; } = string.Empty;

	public string SupplierManagerPost { get; set; } = string.Empty;

	public string SupplierManagerName { get; set; } = string.Empty;

	public string SupplierChiefAccountantPost { get; set; } = string.Empty;

	public string SupplierChiefAccountantName { get; set; } = string.Empty;

	public string BuyerName { get; set; } = string.Empty;

	public string BuyerLegalAddress { get; set; } = string.Empty;

	public string BuyerBusinessAddress { get; set; } = string.Empty;

	public string BuyerSettlementAccount { get; set; } = string.Empty;

	public string BuyerTaxCode { get; set; } = string.Empty;

	public string BuyerManagerPost { get; set; } = string.Empty;

	public string BuyerManagerName { get; set; } = string.Empty;

	public string BuyerChiefAccountantPost { get; set; } = string.Empty;

	public string BuyerChiefAccountantName { get; set; } = string.Empty;

	public string BuyerStateRegisterNumber { get; set; } = string.Empty;

	public string BuyerIDDocType { get; set; } = "";

	public string BuyerPassportNumber { get; set; } = string.Empty;

	public string BaseISN { get; set; } = string.Empty;

	public string ISN { get; set; } = string.Empty;

	public List<MTInvoiceSpecificationRow> Specification { get; set; }

	public string SupplierAdditionalData { get; set; } = string.Empty;

	public string BuyerAdditionalData { get; set; } = string.Empty;

	public string BuyerShippingMethod { get; set; } = string.Empty;

	public string ByWhomFIO { get; set; } = string.Empty;

	public string CredentialNumber { get; set; } = string.Empty;

	public DateTime? CredentialDate { get; set; }

	public MTInvoiceSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<MTInvoiceSpecificationRow>();
		}
		MTInvoiceSpecificationRow mTInvoiceSpecificationRow = new MTInvoiceSpecificationRow();
		Specification.Add(mTInvoiceSpecificationRow);
		return mTInvoiceSpecificationRow;
	}
}
