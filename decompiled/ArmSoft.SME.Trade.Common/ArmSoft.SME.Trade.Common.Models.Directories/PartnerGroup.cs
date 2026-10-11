namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class PartnerGroup : TradeDirectory
{
	public string NewCode { get; set; }

	public string CodeForTree { get; set; }

	public string Parent { get; set; }

	public string Path { get; set; }
}
