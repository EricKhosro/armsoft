using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common.Models.Caches;

public class DirectoryExtensionValuesCacheRow : CacheRowBase<string>
{
	public string Code { get; }

	public List<KeyValuePair<string, string>> Values { get; } = new List<KeyValuePair<string, string>>();

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public DirectoryExtensionValuesCacheRow()
	{
	}

	[JsonConstructor]
	public DirectoryExtensionValuesCacheRow(string code, List<KeyValuePair<string, string>> values, byte[] ts)
	{
		Code = code;
		Values = values;
		TS = ts;
	}
}
