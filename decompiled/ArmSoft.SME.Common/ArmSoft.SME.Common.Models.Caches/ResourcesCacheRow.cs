using System.Text.Json;

namespace ArmSoft.SME.Common.Models.Caches;

public class ResourcesCacheRow : CacheRowBase<string>
{
	public string Name { get; set; }

	public string Caption { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Name);

	public ResourcesCacheRow()
	{
	}

	public ResourcesCacheRow(string name, string caption)
	{
		Name = name;
		Caption = caption;
	}
}
