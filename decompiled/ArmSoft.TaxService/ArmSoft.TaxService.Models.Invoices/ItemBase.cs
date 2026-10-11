using System;
using System.Text.Json.Serialization;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService.Models.Invoices;

public class ItemBase
{
	private string vatRateName = string.Empty;

	private VatRate vatRate;

	public Guid Id { get; set; }

	public Guid InvoiceId { get; set; }

	public int SeqNo { get; set; }

	public string Name { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal UnitPrice { get; set; }

	[JsonInclude]
	[JsonPropertyName("totalValue")]
	public decimal TotalWithoutTaxes { get; set; }

	[JsonInclude]
	[JsonPropertyName("vatRate")]
	private string VatRateName
	{
		get
		{
			return vatRateName;
		}
		set
		{
			vatRateName = value;
			vatRate = EncodeDecodeTypes.DecodeVatRate(vatRateName);
		}
	}

	[JsonIgnore]
	public VatRate VatRate
	{
		get
		{
			return vatRate;
		}
		set
		{
			vatRate = value;
			vatRateName = EncodeDecodeTypes.EncodeVatRate(vatRate);
		}
	}

	public decimal? VatAmount { get; set; }

	public decimal Total { get; set; }
}
