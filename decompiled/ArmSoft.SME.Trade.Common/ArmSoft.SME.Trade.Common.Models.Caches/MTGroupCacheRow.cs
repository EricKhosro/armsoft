using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class MTGroupCacheRow : CacheRowBase<string>
{
	public string Code { get; set; }

	public string Caption { get; set; }

	public string CodeForTree { get; set; }

	public string Path { get; set; }

	public string Parent { get; set; }

	public string CaptionEn { get; set; }

	public string CaptionRu { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public MTGroupCacheRow()
	{
	}

	public MTGroupCacheRow(string code, string caption, string codeForTree, string path, string parent, string captionEn, string captionRu, byte[] ts)
	{
		Code = code;
		Caption = caption;
		CodeForTree = codeForTree;
		Path = path;
		Parent = parent;
		CaptionEn = captionEn;
		CaptionRu = captionRu;
		TS = ts;
	}
}
