using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("4641B6F2-EC6A-4697-AD5F-A11F3ABE3FB7")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTReturnFromCustomer : ExchangeBase
{
	public DateTime DocumentDate { get; set; }

	public string DocumentNumber { get; set; } = string.Empty;

	public string CurrencyCode { get; set; } = string.Empty;

	public decimal CurrencyExchangeRate { get; set; }

	public decimal CurrencyExchangeBase { get; set; }

	public string VATType { get; set; } = string.Empty;

	public string PriceType { get; set; } = string.Empty;

	public string Storage { get; set; } = string.Empty;

	public string PartnerCode { get; set; } = string.Empty;

	public string Contract { get; set; } = string.Empty;

	public decimal PartnerAmount { get; set; }

	public string SalesConsultant { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;

	public string BaseISN { get; set; } = string.Empty;

	public string Trust { get; set; } = string.Empty;

	public string Mediator { get; set; } = string.Empty;

	public string Machine { get; set; } = string.Empty;

	public string ChiefAccountant { get; set; } = string.Empty;

	public string Allower { get; set; } = string.Empty;

	public string Response { get; set; } = string.Empty;

	[ComVisible(false)]
	public List<MTReturnFromCustomerSpecificationRow> Specification { get; set; }

	public string ECRBaseCheckNumber { get; set; } = string.Empty;

	public string ECRReturnCheckNumber { get; set; } = string.Empty;

	public string ECRCheckNumber { get; set; } = string.Empty;

	public decimal ECRReturnByCardSum { get; set; }

	public decimal ECRReturnByCashSum { get; set; }

	public decimal ECRReturnByPrePaymentSum { get; set; }

	public string TaxExportType { get; set; } = string.Empty;

	public string TaxInvoiceSeries { get; set; } = string.Empty;

	public string TaxInvoiceNumber { get; set; } = string.Empty;

	public DateTime? TaxInvoiceDate { get; set; }

	public string BaseDocTaxInvoiceSeries { get; set; } = string.Empty;

	public string BaseDocTaxInvoiceNumber { get; set; } = string.Empty;

	public DateTime? BaseDocTaxInvoiceDate { get; set; }

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

	public string BuyerStateRegisterNumber { get; set; } = string.Empty;

	public string BuyerIDDocType { get; set; } = "";

	public string BuyerPassportNumber { get; set; } = string.Empty;

	public string BuyerChiefAccountantName { get; set; } = string.Empty;

	public string BuyerAdditionalData { get; set; } = string.Empty;

	public string BuyerShippingMethod { get; set; } = string.Empty;

	public string ByWhomFIO { get; set; } = string.Empty;

	public string CredentialNumber { get; set; } = string.Empty;

	public DateTime? CredentialDate { get; set; }

	public bool BuyerForeign { get; set; }

	public string CashDesk { get; set; } = string.Empty;

	public string CashierShiftNumber { get; set; } = string.Empty;

	public string BankAcquiringPartner { get; set; } = string.Empty;

	public string BankAcquiringContract { get; set; } = string.Empty;

	public bool PaymentRefundAccepted { get; set; }

	public string ISN { get; set; } = string.Empty;

	public MTReturnFromCustomerSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<MTReturnFromCustomerSpecificationRow>();
		}
		MTReturnFromCustomerSpecificationRow mTReturnFromCustomerSpecificationRow = new MTReturnFromCustomerSpecificationRow();
		Specification.Add(mTReturnFromCustomerSpecificationRow);
		return mTReturnFromCustomerSpecificationRow;
	}
}
