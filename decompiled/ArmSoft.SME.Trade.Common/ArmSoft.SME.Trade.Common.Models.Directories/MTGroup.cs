namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class MTGroup : TradeDirectory
{
	public string NewCode { get; set; } = string.Empty;

	public string CodeForTree { get; set; } = string.Empty;

	public string CaptionEn { get; set; } = string.Empty;

	public string CaptionRu { get; set; } = string.Empty;

	public string Path { get; set; } = string.Empty;

	public string Parent { get; set; } = string.Empty;

	public string Body { get; set; }

	public bool IsLeaf { get; set; }
}
