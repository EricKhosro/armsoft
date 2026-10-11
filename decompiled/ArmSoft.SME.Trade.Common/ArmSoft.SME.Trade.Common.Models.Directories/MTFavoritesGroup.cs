namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class MTFavoritesGroup : TradeDirectory
{
	public string HotKey { get; set; }

	public string HotKeyOld { get; set; }

	public int UserId { get; set; }

	public int OrderNum { get; set; }

	public byte[] Img { get; set; }

	public bool ImageEdited { get; set; }
}
