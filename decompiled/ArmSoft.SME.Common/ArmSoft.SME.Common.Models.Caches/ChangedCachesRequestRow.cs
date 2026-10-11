namespace ArmSoft.SME.Common.Models.Caches;

public class ChangedCachesRequestRow
{
	public short CacheId { get; set; }

	public byte[] TS { get; set; }

	public ChangedCachesRequestRow()
	{
	}

	public ChangedCachesRequestRow(short cacheId, byte[] ts)
	{
		CacheId = cacheId;
		TS = ts;
	}
}
