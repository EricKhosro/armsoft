using System;

namespace ArmSoft.SME.Common.Models;

public class WorkingPeriod
{
	public DateOnly? StartDate { get; set; }

	public DateOnly? EndDate { get; set; }

	public bool IsUserPeriod { get; set; }

	public bool IsUseCurrentDate { get; set; }
}
