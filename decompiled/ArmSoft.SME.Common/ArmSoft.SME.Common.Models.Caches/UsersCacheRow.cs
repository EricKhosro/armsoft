using System.Text.Json;

namespace ArmSoft.SME.Common.Models.Caches;

public class UsersCacheRow : CacheRowBaseWithName<int>
{
	public int UserId { get; set; }

	public string Description { get; set; }

	public bool Login { get; set; }

	public bool Admin { get; set; }

	public int? LocalUserId { get; set; }

	public int CloudUserId { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(UserId);

	public UsersCacheRow()
	{
		UserId = -1;
		Name = string.Empty;
		Description = string.Empty;
		Login = false;
		Admin = false;
		LocalUserId = null;
		CloudUserId = -1;
	}

	public UsersCacheRow(int cloudUserId, string name, string description, bool login, bool admin, int? localUserId)
	{
		UserId = localUserId ?? cloudUserId;
		Name = name;
		Description = description;
		Login = login;
		Admin = admin;
		LocalUserId = localUserId;
		CloudUserId = cloudUserId;
	}
}
