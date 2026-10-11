using System;

namespace ArmSoft.SME.Common.Models.Statistics;

public class StatisticsDeleteRequest
{
	public DateOnly? StartDate { get; set; }

	public DateOnly? EndDate { get; set; }
}
