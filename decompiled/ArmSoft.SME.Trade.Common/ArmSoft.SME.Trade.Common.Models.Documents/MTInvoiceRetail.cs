using System;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTInvoiceRetail : MTInvoiceBase
{
	public string SupplierName { get; set; }

	public string SupplierLegalAddress { get; set; }

	public string SupplierBusinessAddress { get; set; }

	public string SupplierSettlAcc { get; set; }

	public string SupplierTaxCode { get; set; }

	public string SupplierMngPost { get; set; }

	public string SupplierMngName { get; set; }

	public string SupplierChiefAccPost { get; set; }

	public string SupplierChiefAccName { get; set; }

	public string SupplierAdditionalData { get; set; }

	public string BuyerLegalAddress { get; set; }

	public string BuyerBusinessAddress { get; set; }

	public string BuyerSettlAcc { get; set; }

	public string BuyerTaxCode { get; set; }

	public string BuyerMngPost { get; set; }

	public string BuyerMngName { get; set; }

	public string BuyerChiefAccPost { get; set; }

	public string BuyerChiefAccName { get; set; }

	public string BuyerStateRegNum { get; set; }

	public string BuyerIDDocType { get; set; }

	public string BuyerPassport { get; set; }

	public string BuyerAdditionalData { get; set; }

	public string BuyerShippingMethod { get; set; }

	public string ByWhomFio { get; set; }

	public string AuthorizationNumber { get; set; }

	public DateOnly? AuthorizationDate { get; set; }
}
