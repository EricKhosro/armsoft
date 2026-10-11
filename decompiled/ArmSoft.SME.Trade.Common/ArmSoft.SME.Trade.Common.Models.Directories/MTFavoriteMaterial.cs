namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class MTFavoriteMaterial : TradeDirectory
{
	public int MTIDOld { get; set; }

	public string GroupCode { get; set; }

	public string GroupCaption { get; set; }

	public string QntUnit { get; set; }

	public string QntUnitOld { get; set; }

	public string QntUnitBrief { get; set; }

	public string HotKey { get; set; }

	public string HotKeyOld { get; set; }

	public int UserId { get; set; }

	public int OrderNum { get; set; }
}
