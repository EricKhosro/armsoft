namespace ArmSoft.SME.Common.Models.Caches;

public class CacheRequestRow
{
	public string Key { get; set; }

	public byte[] TS { get; set; }

	public CacheRequestRow()
	{
	}

	public CacheRequestRow(string key, byte[] ts)
	{
		Key = key;
		TS = ts;
	}
}
