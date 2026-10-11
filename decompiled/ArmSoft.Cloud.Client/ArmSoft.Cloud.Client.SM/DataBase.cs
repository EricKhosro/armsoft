using System;

namespace ArmSoft.Cloud.Client.SM;

public class DataBase
{
	public int DBid { get; set; }

	public Guid DBguid { get; set; }

	public string Name { get; set; }

	public string AppKeyCode { get; set; }

	public int ActorId { get; set; }

	public string TaxCode { get; set; }

	public string Phone { get; set; }

	public DateTime? LastCSUpdate { get; set; }
}
