using System;
using System.Text.Json.Serialization;

namespace ArmSoft.TaxService.Models.AccDocs;

public class ItemBase
{
	public Guid Id { get; set; }

	public Guid InvoiceId { get; set; }

	public int SeqNo { get; set; }

	public string Name { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	[JsonInclude]
	[JsonPropertyName("totalValue")]
	public decimal TotalWithoutTaxes { get; set; }
}
