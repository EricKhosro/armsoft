using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ArmSoft.TaxService.Models.Invoices;

public class VatRefund : InvoiceBase
{
	public class Item : ItemBase
	{
		public Guid ClassifierId { get; set; }

		[JsonConverter(typeof(DiscountConverter))]
		public decimal? Discount { get; set; }
	}

	public string SupplierTin { get; set; } = string.Empty;

	public string SupplierVatTin { get; set; } = string.Empty;

	public string SupplierName { get; set; } = string.Empty;

	public string SupplierAddress { get; set; } = string.Empty;

	public string SupplierAdditionalInfo { get; set; } = string.Empty;

	public Guid? SourceRegionId { get; set; }

	public Guid? SourceCommunityId { get; set; }

	public Guid? SourceResidenceId { get; set; }

	public string SourceRegion { get; set; } = string.Empty;

	public string SourceCommunity { get; set; } = string.Empty;

	public string SourceResidence { get; set; } = string.Empty;

	public string SourceStreet { get; set; } = string.Empty;

	public string SourceHouse { get; set; } = string.Empty;

	public string SourceApartment { get; set; } = string.Empty;

	public string SourceFullAddress { get; set; } = string.Empty;

	public string BuyerFullName { get; set; } = string.Empty;

	public string BuyerPassport { get; set; } = string.Empty;

	public string BuyerEmail { get; set; } = string.Empty;

	public Guid BuyerCountryId { get; set; }

	public string BuyerAddress { get; set; } = string.Empty;

	public string BuyerBank { get; set; } = string.Empty;

	public string BuyerAccNo { get; set; } = string.Empty;

	public string SupplierCn { get; set; } = string.Empty;

	public string SupplierUser { get; set; } = string.Empty;

	public string SerialNo { get; set; } = string.Empty;

	public decimal? TotalVatAmount { get; set; }

	public Guid? SupplierSignId { get; set; }

	public string Tin { get; set; } = string.Empty;

	public List<Item> Items { get; set; }
}
