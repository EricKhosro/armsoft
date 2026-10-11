using System.Text.Json;

namespace ArmSoft.SME.Common.Models.Caches;

public class UserAccessesCacheRow : CacheRowBase<short>
{
	public short AccessId { get; set; }

	public bool AllowView { get; set; }

	public bool AllowOperation { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(AccessId);

	public UserAccessesCacheRow(short accessId, bool allowView, bool allowOperation)
	{
		AccessId = accessId;
		AllowView = allowView;
		AllowOperation = allowOperation;
	}

	public UserAccessesCacheRow()
	{
	}
}
