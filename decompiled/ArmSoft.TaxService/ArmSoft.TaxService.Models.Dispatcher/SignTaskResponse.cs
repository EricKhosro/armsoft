using System;
using System.Collections.Generic;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService.Models.Dispatcher;

public class SignTaskResponse
{
	public Guid RequestId { get; set; }

	public SignState State { get; set; }

	public string FailMessage { get; set; } = string.Empty;

	public List<FailedSignTaskItems> FailedSignTaskItems { get; set; } = new List<FailedSignTaskItems>();
}
