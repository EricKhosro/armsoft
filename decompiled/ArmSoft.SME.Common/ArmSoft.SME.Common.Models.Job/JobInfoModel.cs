using System;

namespace ArmSoft.SME.Common.Models.Job;

public class JobInfoModel
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string State { get; set; }

	public int DatabaseId { get; set; }
}
