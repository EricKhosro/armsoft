using System;
using System.Collections.Generic;

namespace ArmSoft.Cloud.Client;

public class JobProgress
{
	public JobState State { get; set; }

	public string StateName { get; set; }

	public DateTime? StartDate { get; set; }

	public List<JobProgressPhase> Details { get; set; }

	public DateTime? EndDate { get; set; }

	public double? Duration { get; set; }
}
