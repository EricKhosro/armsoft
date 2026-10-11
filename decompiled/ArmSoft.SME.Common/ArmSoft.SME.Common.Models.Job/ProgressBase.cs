using System;
using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Common.Models.Job;

public class ProgressBase<T> : IProgressBase where T : ProgressPhaseBase, new()
{
	public JobState State { get; set; }

	public string StateName { get; set; }

	public DateTime? StartDate { get; set; }

	public List<T> Details { get; set; } = new List<T>();

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

	List<ProgressPhaseBase> IProgressBase.Details => Details.Cast<ProgressPhaseBase>().ToList();

	public ProgressBase()
	{
	}

	public ProgressBase(ExecutionProgress progress)
	{
		if (progress == null)
		{
			return;
		}
		StartDate = progress.Start;
		EndDate = progress.End;
		Details = new List<T>();
		foreach (ExecutionProgress.Phase detail in progress.Details)
		{
			T item = new T
			{
				Id = detail.Id,
				Name = detail.Name,
				StartDate = detail.StartDate,
				EndDate = detail.EndDate,
				Row = detail.Row,
				Total = detail.Total
			};
			Details.Add(item);
		}
	}
}
