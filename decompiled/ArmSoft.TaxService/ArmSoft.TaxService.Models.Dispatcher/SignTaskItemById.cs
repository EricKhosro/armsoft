using System;
using System.Text.Json.Serialization;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService.Models.Dispatcher;

public class SignTaskItemById
{
	public Guid Id { get; set; }

	public Guid SignTaskId { get; set; }

	public Guid InvoiceId { get; set; }

	public string Status { get; set; } = string.Empty;

	public string InvoiceType { get; set; } = string.Empty;

	[JsonIgnore]
	public string InvoiceTypeCaption => EncodeDecodeTypes.GetInvoiceTypeCaption(InvoiceType);

	public string FailMessage { get; set; } = string.Empty;

	public string Warnings { get; set; } = string.Empty;

	public string SerialNo { get; set; } = string.Empty;
}
