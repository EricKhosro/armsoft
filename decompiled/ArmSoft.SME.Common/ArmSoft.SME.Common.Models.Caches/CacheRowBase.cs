using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Common.Models.Caches;

public class CacheRowBase<TKey> : CacheRowBase
{
	[JsonIgnore]
	public TKey Key => JsonSerializer.Deserialize<TKey>(KeyJSON);
}
public class CacheRowBase
{
	public virtual string KeyJSON { get; }

	public byte[] TS { get; set; } = TimeStamp.NewTS();

	public virtual int GetId()
	{
		throw new NotImplementedException();
	}
}
