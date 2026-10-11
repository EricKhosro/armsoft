using System;

namespace ArmSoft.SME.Common.Models.Job;

public class JobStopRequest
{
	public Guid Id { get; set; }

	public bool SuppressPostProcessing { get; set; }
}
