using System;

namespace ArmSoft.Cloud.Client;

public class JobProgressPhase
{
	public int Id { get; set; }

	public string Name { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public double? Duration { get; set; }

	public int Row { get; set; }

	public int Total { get; set; }

	public bool DisableCancel { get; set; }

	public bool Overwritable { get; set; }
}
