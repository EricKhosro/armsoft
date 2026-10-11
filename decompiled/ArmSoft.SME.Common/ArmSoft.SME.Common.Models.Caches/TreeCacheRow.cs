using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common.Models.Caches;

public class TreeCacheRow : CacheRowBase<string>
{
	public string Id { get; set; }

	public string Caption { get; set; }

	public int NodesCount { get; set; }

	public short CodeLen { get; set; }

	public short CaptionLen { get; set; }

	public bool Multilevel { get; set; }

	public bool PrefixBased { get; set; }

	public bool HasExtension { get; set; }

	public bool DocumentBased { get; set; }

	public bool System { get; set; }

	public string CodeCaption { get; set; }

	public string NodeCaption { get; set; }

	public string SpecCaption { get; set; }

	public short SpecLen { get; set; }

	public bool IsLogAvailable { get; set; }

	public bool IsClosedAvailable { get; set; }

	public short? NextCodeParamId { get; set; }

	public short CacheId { get; set; }

	public string Extension { get; set; }

	[JsonIgnore]
	public object TreeNodeCache { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Id);
}
