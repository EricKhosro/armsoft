using System;

namespace ArmSoft.SME.Common.Models.Job;

public abstract class ProgressPhaseBase
{
	public int Id { get; set; }

	public string Name { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public double? Duration
	{
		get
		{
			if (StartDate.HasValue)
			{
				if (EndDate.HasValue)
				{
					return (EndDate.Value - StartDate.Value).TotalSeconds;
				}
				return (DateTime.Now - StartDate.Value).TotalSeconds;
			}
			return null;
		}
	}

	public int? Row { get; set; }

	public int? Total { get; set; }

	public bool DisableCancel { get; set; }

	public bool Overwritable { get; set; } = true;

	public abstract ProgressPhaseBase ToModel();
}
