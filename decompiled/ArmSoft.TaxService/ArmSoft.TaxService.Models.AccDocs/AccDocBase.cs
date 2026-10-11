using System;
using System.Text.Json.Serialization;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService.Models.AccDocs;

public class AccDocBase
{
	private string statusName = string.Empty;

	private string sourceName;

	private Source source;

	private Status status;

	public Guid Id { get; set; }

	public DateTime? DealtAt { get; set; }

	public string DealNo { get; set; } = string.Empty;

	public string SerialNo { get; set; } = string.Empty;

	public string SupplierTin { get; set; } = string.Empty;

	public string SupplierVatTin { get; set; } = string.Empty;

	public string SupplierName { get; set; } = string.Empty;

	public string SupplierAddress { get; set; } = string.Empty;

	public string SupplierAdditionalInfo { get; set; } = string.Empty;

	public string SupplierBank { get; set; } = string.Empty;

	public string SupplierAccNo { get; set; } = string.Empty;

	public string SupplierUser { get; set; } = string.Empty;

	public string SupplierCn { get; set; } = string.Empty;

	public DateTime? IssuedAt { get; set; }

	public Guid? SupplierSignId { get; set; }

	public bool IsBatch { get; set; }

	[JsonInclude]
	[JsonPropertyName("totalValue")]
	public decimal TotalWithoutTaxes { get; set; }

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

	public int? Version { get; set; }

	public XMLAccDocType AccountingDocumentType { get; set; }
}
