namespace ArmSoft.SME.Common.Models.Caches;

public class CacheRowBaseWithIdName<TKey> : CacheRowBaseWithName<TKey>
{
	public int Id { get; set; }

	public override int GetId()
	{
		return Id;
	}
}
