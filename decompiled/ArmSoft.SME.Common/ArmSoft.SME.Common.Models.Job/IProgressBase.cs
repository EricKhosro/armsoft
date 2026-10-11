using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Enums;

namespace ArmSoft.SME.Common.Models.Job;

public interface IProgressBase
{
	double? Duration { get; }

	DateTime? EndDate { get; set; }

	DateTime? StartDate { get; set; }

	JobState State { get; set; }

	string StateName { get; set; }

	List<ProgressPhaseBase> Details { get; }
}
