namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class GoodsBySegment : TradeDirectory
{
	public int MTID { get; set; } = -1;

	public string MTCode { get; set; } = string.Empty;

	public string OldCode { get; set; } = string.Empty;

	public string OldMTCode { get; set; } = string.Empty;

	public string MTCaption { get; set; } = string.Empty;
}
