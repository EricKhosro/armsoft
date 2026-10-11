using System.Text.Json;
using System.Text.Json.Serialization;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Common.Models.Caches;

public class TreeNodesCacheRow : CacheRowBase<string>
{
	public string Code { get; }

	public string Name { get; }

	public string Parent { get; }

	public string Path { get; }

	public bool IsLeaf { get; }

	public bool IsClosed { get; }

	public string Spec { get; }

	[JsonIgnore]
	public DocumentExtension Extension { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public TreeNodesCacheRow()
	{
	}

	[JsonConstructor]
	public TreeNodesCacheRow(string code, string name, string parent, string path, string spec, bool isLeaf, bool isClosed, byte[] ts, DocumentExtension extension = null)
	{
		Code = code;
		Name = name;
		Parent = parent;
		Path = path;
		Spec = spec;
		IsLeaf = isLeaf;
		IsClosed = isClosed;
		TS = ts;
		Extension = extension;
	}

	public TreeNodesCacheRow Clone()
	{
		return new TreeNodesCacheRow(Code, Name, Parent, Path, Spec, IsLeaf, IsClosed, TS, Extension);
	}
}
