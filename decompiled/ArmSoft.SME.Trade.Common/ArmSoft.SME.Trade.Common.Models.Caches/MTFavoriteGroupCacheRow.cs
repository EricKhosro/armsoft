using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class MTFavoriteGroupCacheRow : CacheRowBase<int>
{
	public int Id { get; }

	public string Code { get; }

	public string Name { get; }

	public string HotKey { get; }

	public int OrderNum { get; }

	public bool ImageExists { get; }

	public Guid? ImageGUID { get; }

	public override string KeyJSON => JsonSerializer.Serialize(Id);

	public MTFavoriteGroupCacheRow()
	{
	}

	[JsonConstructor]
	public MTFavoriteGroupCacheRow(int id, string code, string name, string hotKey, int orderNum, bool imageExists, Guid? imageGuid, byte[] ts)
	{
		Id = id;
		Code = code;
		Name = name;
		HotKey = hotKey;
		OrderNum = orderNum;
		ImageExists = imageExists;
		ImageGUID = imageGuid;
		TS = ts;
	}
}
