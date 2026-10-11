using System;

namespace ArmSoft.Cloud.Client.SM;

public class SalesPointDataBasePair
{
	public int SalesPointId { get; set; }

	public string SalesPointName { get; set; }

	public string Address { get; set; }

	public DateTime? LastLogin { get; set; }

	public int DBid { get; set; }

	public Guid DBguid { get; set; }

	public string DBName { get; set; }

	public string AppKeyCode { get; set; }

	public string TaxCode { get; set; }

	public DateTime? LastCSUpdate { get; set; }

	public int UserId { get; set; }

	public string Phone { get; set; } = "";

	public string FirstName { get; set; } = "";

	public string LastName { get; set; } = "";

	public string Email { get; set; } = "";

	public DateTime? LastSeen { get; set; }

	public int? TelegramId { get; set; }
}
