using System;
using System.Text.Json.Serialization;

namespace ArmSoft.TaxService.Models.Dispatcher;

public class InvoiceLockStatus
{
	public bool IsLock { get; set; }

	[JsonInclude]
	[JsonPropertyName("taskId")]
	public Guid SignTaskId { get; set; }
}
