using System;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTReturnFromCustomer : MTReturnFromCustomerBase
{
	public string Trust { get; set; } = "";

	public string Mediator { get; set; } = "";

	public string Machine { get; set; } = "";

	public string Chief { get; set; } = "";

	public string Allower { get; set; } = "";

	public string Response { get; set; } = "";

	public string BuyerName { get; set; } = "";

	public string BuyerLegalAddress { get; set; } = "";

	public string BuyerBusinessAddress { get; set; } = "";

	public string BuyerSettlAcc { get; set; } = "";

	public string BuyerTaxCode { get; set; } = "";

	public string BuyerMngPost { get; set; } = "";

	public string BuyerMngName { get; set; } = "";

	public string BuyerChiefAccPost { get; set; } = "";

	public string BuyerChiefAccName { get; set; } = "";

	public string BuyerStateRegNum { get; set; } = "";

	public string BuyerIDDocType { get; set; } = "";

	public string BuyerPassport { get; set; } = "";

	public string SupplierName { get; set; } = "";

	public string SupplierLegalAddress { get; set; } = "";

	public string SupplierBusinessAddress { get; set; } = "";

	public string SupplierSettlAcc { get; set; } = "";

	public string SupplierTaxCode { get; set; } = "";

	public string SupplierMngPost { get; set; } = "";

	public string SupplierMngName { get; set; } = "";

	public string SupplierChiefAccPost { get; set; } = "";

	public string SupplierChiefAccName { get; set; } = "";

	public string BuyerAdditionalData { get; set; } = "";

	public string BuyerShippingMethod { get; set; } = "";

	public string ByWhomFio { get; set; } = "";

	public string AuthorizationNumber { get; set; } = "";

	public DateOnly? AuthorizationDate { get; set; }

	public bool BuyerForeign { get; set; }

	public string BaseDocTaxInvoiceSeries { get; set; } = "";

	public string BaseDocTaxInvoiceNumber { get; set; } = "";

	public DateOnly? BaseDocTaxInvoiceDate { get; set; }

	public string TaxInvoiceSeries { get; set; } = "";

	public string TaxInvoiceNumber { get; set; } = "";

	public decimal BaseECRByCardSum { get; set; }

	public decimal BaseECRByCashSum { get; set; }

	public decimal BaseECRByPrePaymentSum { get; set; }
}
