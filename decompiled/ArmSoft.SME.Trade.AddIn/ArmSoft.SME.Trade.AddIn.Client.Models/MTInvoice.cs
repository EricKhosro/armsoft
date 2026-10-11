using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("43B17EE2-F9BD-44F7-A799-381818F60E0A")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTInvoice : ExchangeBase
{
	public string DocumentNumber { get; set; } = string.Empty;

	public DateTime DocumentDate { get; set; }

	public string BuyerCode { get; set; } = string.Empty;

	public string Contract { get; set; } = string.Empty;

	public decimal PartnerAmount { get; set; }

	public string SupplierBookNumber { get; set; } = string.Empty;

	public int SupplierBookPage { get; set; }

	public int SupplierBookRow { get; set; }

	public string TaxExportType { get; set; } = string.Empty;

	public string TaxInvoiceSeries { get; set; } = string.Empty;

	public string TaxInvoiceNumber { get; set; } = string.Empty;

	public DateTime? TaxInvoiceDate { get; set; }

	public string SalesConsultant { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;

	public string PriceType { get; set; } = string.Empty;

	public string DiscountPriceType { get; set; } = string.Empty;

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

	public string CurrencyCode { get; set; } = string.Empty;

	public decimal CurrencyExchangeRate { get; set; }

	public decimal CurrencyExchangeBase { get; set; }

	public string VATType { get; set; } = string.Empty;

	public string PricesTypes { get; set; } = string.Empty;

	public string Storage { get; set; } = string.Empty;

	public decimal DiscountPercent { get; set; }

	public decimal TotalAmount { get; set; }

	public decimal AmountVAT { get; set; }

	public string ISN { get; set; } = string.Empty;

	public string BaseISN { get; set; } = string.Empty;

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
