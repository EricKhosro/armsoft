using System;

namespace ArmSoft.SME.Common.Models.Job;

public class JobResultRequest
{
	public Guid Id { get; set; }

	public bool Delete { get; set; }
}
