using System.Text.Json;

namespace ArmSoft.SME.Common.Models.Caches;

public class ParamsCacheRow : CacheRowBase<short>
{
	public short Id { get; set; }

	public string Value { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Id);

	public ParamsCacheRow()
	{
	}

	public ParamsCacheRow(short id, string value)
	{
		Id = id;
		Value = value;
	}
}
