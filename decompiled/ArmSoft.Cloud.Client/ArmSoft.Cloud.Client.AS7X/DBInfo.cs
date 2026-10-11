using System;

namespace ArmSoft.Cloud.Client.AS7X;

public class DBInfo
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string TaxCode { get; set; }

	public string CompanyName { get; set; }

	public int ActorId { get; set; }

	public decimal SizeMB { get; set; }

	public decimal SpaceUsedMB { get; set; }
}
