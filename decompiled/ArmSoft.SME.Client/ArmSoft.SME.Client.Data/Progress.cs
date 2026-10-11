using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Client.Data;

public class Progress
{
	public DateTime? StartDate { get; set; }

	public List<ProgressPhase> Details { get; set; }

	public DateTime? EndDate { get; set; }

	public double? Duration { get; set; }
}
