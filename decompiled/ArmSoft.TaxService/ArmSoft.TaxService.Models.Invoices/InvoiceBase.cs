using System;
using System.Text.Json.Serialization;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService.Models.Invoices;

public class InvoiceBase
{
	private string sourceName = string.Empty;

	private Source source;

	private string statusName = string.Empty;

	private Status status;

	public Guid Id { get; set; }

	public string UserName { get; set; }

	[JsonInclude]
	[JsonPropertyName("totalValue")]
	public decimal TotalWithoutTaxes { get; set; }

	public decimal Total { get; set; }

	public bool IsBatch { get; set; }

	public DateTime? IssuedAt { get; set; }

	[JsonInclude]
	[JsonPropertyName("source")]
	private string SourceName
	{
		get
		{
			return sourceName;
		}
		set
		{
			sourceName = value;
			source = EncodeDecodeTypes.DecodeSource(sourceName);
		}
	}

	[JsonIgnore]
	public Source Source
	{
		get
		{
			return source;
		}
		set
		{
			source = value;
			sourceName = EncodeDecodeTypes.EncodeSource(source);
		}
	}

	[JsonInclude]
	[JsonPropertyName("status")]
	private string StatusName
	{
		get
		{
			return statusName;
		}
		set
		{
			statusName = value;
			status = EncodeDecodeTypes.DecodeStatus(statusName);
		}
	}

	[JsonIgnore]
	public Status Status
	{
		get
		{
			return status;
		}
		set
		{
			status = value;
			statusName = EncodeDecodeTypes.EncodeStatus(status);
		}
	}

	public DateTime? CreatedAt { get; set; }

	public DateTime DeliveredAt { get; set; }

	public int? Version { get; set; }

	public XMLInvoiceDocType InvoiceType { get; set; }
}
