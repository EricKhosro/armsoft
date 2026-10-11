using System;
using System.Collections.Generic;

namespace ArmSoft.TaxService.Models.Dispatcher;

public class FailedSignTaskItems
{
	public Guid Id { get; set; }

	public Guid SignTaskId { get; set; }

	public Guid InvoiceId { get; set; }

	public string Status { get; set; } = string.Empty;

	public string InvoiceType { get; set; } = string.Empty;

	public string SerialNo { get; set; } = string.Empty;

	public Dictionary<string, string> Errors { get; set; }

	public List<string> Warnings { get; set; }
}
