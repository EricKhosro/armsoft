namespace ArmSoft.SME.Common.Models.Caches;

public class CacheRowBaseWithName<TKey> : CacheRowBase<TKey>
{
	public string Name { get; set; }
}
