using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTReturnToSupplier : TradeDocument
{
	public class MTRetToSupSpecificationRow
	{
		public string Storage { get; set; }

		public string MTCode { get; set; }

		public int MTID { get; set; }

		public string UnitCode { get; set; }

		public string UnitBrief { get; set; }

		public decimal Quantity { get; set; }

		public decimal Discount { get; set; }

		public decimal DiscountPrice { get; set; }

		public decimal Price { get; set; }

		public decimal Summa { get; set; }

		public decimal SalePrice { get; set; }

		public decimal SaleSumma { get; set; }

		public string VATCodeForConvert { get; set; }

		public bool VAT { get; set; }

		public short RowNum { get; set; }

		public int RowID { get; set; }

		public bool IsPartySelected { get; set; }

		public string MTName { get; set; }

		public string FullName { get; set; }
	}

	public List<PartyRow> MTExpDocSelectedPartiesList { get; set; } = new List<PartyRow>();

	public int MaxRowID { get; set; }

	public decimal Discount { get; set; }

	public string PriceListType { get; set; }

	public decimal SummVAT { get; set; }

	public string BookNum { get; set; }

	public int BookPage { get; set; }

	public int BookLine { get; set; }

	public string BaseDocInvoiceSeries { get; set; }

	public string BaseDocInvoiceNumber { get; set; }

	public DateOnly? BaseDocInvoiceDate { get; set; }

	public string TaxInvoiceSeries { get; set; }

	public string TaxInvoiceNumber { get; set; }

	public string SalesConsultant { get; set; }

	public string BuyerName { get; set; }

	public string BuyerLegalAddress { get; set; }

	public string BuyerBusinessAddress { get; set; }

	public string BuyerSettlAcc { get; set; }

	public string BuyerTaxCode { get; set; }

	public string BuyerMngPost { get; set; }

	public string BuyerMngName { get; set; }

	public string BuyerChiefAccPost { get; set; }

	public string BuyerChiefAccName { get; set; }

	public string BuyerStateRegNum { get; set; }

	public string BuyerPassport { get; set; }

	public string SupplierName { get; set; }

	public string SupplierLegalAddress { get; set; }

	public string SupplierBusinessAddress { get; set; }

	public string SupplierSettlAcc { get; set; }

	public string SupplierTaxCode { get; set; }

	public string SupplierMngPost { get; set; }

	public string SupplierMngName { get; set; }

	public string SupplierChiefAccPost { get; set; }

	public string SupplierChiefAccName { get; set; }

	public List<MTRetToSupSpecificationRow> MTList { get; set; }
}
