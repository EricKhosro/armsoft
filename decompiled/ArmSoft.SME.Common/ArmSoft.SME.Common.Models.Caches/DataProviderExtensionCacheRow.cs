using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common.Models.Caches;

public class DataProviderExtensionCacheRow : CacheRowBase<short>
{
	public string Name { get; }

	public short Type { get; }

	public string Xml { get; }

	[JsonIgnore]
	public Type HandlerType { get; set; }

	[JsonIgnore]
	public string Script { get; }

	public override string KeyJSON => JsonSerializer.Serialize(Type);

	public DataProviderExtensionCacheRow()
	{
	}

	[JsonConstructor]
	public DataProviderExtensionCacheRow(string name, short type, string xml, byte[] ts)
	{
		Name = name;
		Type = type;
		Xml = xml;
		TS = ts;
	}

	public DataProviderExtensionCacheRow(string name, short type, string xml, string script, byte[] ts)
	{
		Name = name;
		Type = type;
		Xml = xml;
		TS = ts;
		Script = script;
		HandlerType = null;
	}
}
