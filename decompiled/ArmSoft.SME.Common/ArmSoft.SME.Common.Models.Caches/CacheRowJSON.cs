namespace ArmSoft.SME.Common.Models.Caches;

public class CacheRowJSON : CacheRowBase
{
	public string Value;

	public override string KeyJSON { get; }

	public CacheRowJSON(string keyJSON, byte[] ts, string value)
	{
		KeyJSON = keyJSON;
		TS = ts;
		Value = value;
	}
}
