using System;

namespace ArmSoft.Cloud.Client.SM;

public class SalesPoint
{
	public int SalesPointId { get; set; }

	public string Name { get; set; }

	public string Address { get; set; }

	public DateTime? LastLogin { get; set; }

	public DateTime? DBListUpdated { get; set; }
}
