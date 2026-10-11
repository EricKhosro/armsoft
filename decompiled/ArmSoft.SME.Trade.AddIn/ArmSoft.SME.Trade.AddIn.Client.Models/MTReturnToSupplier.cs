using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("415AFA9B-48E0-41AB-A75F-6B5E94CE624C")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTReturnToSupplier : ExchangeBase
{
	public DateTime DocumentDate { get; set; }

	public string DocumentNumber { get; set; } = string.Empty;

	public string PartnerCode { get; set; } = string.Empty;

	public string Contract { get; set; } = string.Empty;

	public decimal PartnerAmount { get; set; }

	public string SupplierBookNumber { get; set; } = string.Empty;

	public int SupplierBookPage { get; set; }

	public int SupplierBookRow { get; set; }

	public string SalesConsultant { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;

	public string CurrencyCode { get; set; } = string.Empty;

	public decimal CurrencyExchangeRate { get; set; }

	public decimal CurrencyExchangeBase { get; set; }

	public string VATType { get; set; } = string.Empty;

	public string PriceType { get; set; } = string.Empty;

	public string Storage { get; set; } = string.Empty;

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

	public decimal DiscountPercent { get; set; }

	public string BaseISN { get; set; } = string.Empty;

	public List<MTRetToSupSpecificationRow> Specification { get; set; }

	public MTRetToSupSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<MTRetToSupSpecificationRow>();
		}
		MTRetToSupSpecificationRow mTRetToSupSpecificationRow = new MTRetToSupSpecificationRow();
		Specification.Add(mTRetToSupSpecificationRow);
		return mTRetToSupSpecificationRow;
	}
}
