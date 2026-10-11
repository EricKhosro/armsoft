using System;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Common.Models.Job;

public class JobEnqueueWithSchemaResponse
{
	public Guid Id { get; set; }

	public Schema Schema { get; set; }
}
