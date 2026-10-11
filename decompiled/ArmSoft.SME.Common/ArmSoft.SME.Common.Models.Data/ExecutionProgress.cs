using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Data;

public class ExecutionProgress
{
	public class Phase
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public int Row { get; set; } = -1;

		public int Total { get; set; } = -1;
	}

	public DateTime? Start { get; set; }

	public DateTime? End { get; set; }

	public List<Phase> Details { get; private set; } = new List<Phase>();

	public Phase CurrentPhase { get; private set; }

	public void Add(int id, string name)
	{
		if (CurrentPhase != null)
		{
			if (id <= CurrentPhase.Id)
			{
				throw new ArgumentException($"It is not possible to add the {id} code after the {CurrentPhase.Id} code, as the progress phase codes must be added in ascending order.");
			}
			CurrentPhase.EndDate = DateTime.Now;
		}
		CurrentPhase = new Phase
		{
			Id = id,
			Name = name,
			StartDate = DateTime.Now
		};
		Details.Add(CurrentPhase);
	}
}
