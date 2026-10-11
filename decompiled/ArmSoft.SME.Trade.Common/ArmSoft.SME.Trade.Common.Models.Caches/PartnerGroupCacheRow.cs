using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class PartnerGroupCacheRow : CacheRowBase<string>
{
	public const string PartnerGroupCache = "PartnerGroupCache";

	public string Code { get; set; }

	public string Caption { get; set; }

	public string CodeForTree { get; set; }

	public string Path { get; set; } = string.Empty;

	public string Parent { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public PartnerGroupCacheRow()
	{
	}

	public PartnerGroupCacheRow(string code, string caption, string codeForTree, string path, string parent, byte[] ts)
	{
		Code = code;
		Caption = caption;
		CodeForTree = codeForTree;
		Path = path;
		Parent = parent;
		TS = ts;
	}

	public object GetReferenceValue(string name)
	{
		if (name == "PartnerGroupCacheCaption")
		{
			return Caption;
		}
		return null;
	}
}
