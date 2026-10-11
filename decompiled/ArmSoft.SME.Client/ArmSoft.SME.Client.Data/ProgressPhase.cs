using System;

namespace ArmSoft.SME.Client.Data;

public class ProgressPhase
{
	public int Id { get; set; }

	public string Name { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public double? Duration { get; set; }

	public int? Row { get; set; }

	public int? Total { get; set; }
}
