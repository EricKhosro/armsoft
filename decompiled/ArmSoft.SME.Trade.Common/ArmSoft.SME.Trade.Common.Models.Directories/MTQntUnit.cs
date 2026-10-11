namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class MTQntUnit : TradeDirectory
{
	public string QntUnitBrief { get; set; } = string.Empty;

	public int MTID { get; set; } = -1;

	public string MTCode { get; set; } = string.Empty;

	public string MTCaption { get; set; } = string.Empty;

	public decimal Coef { get; set; }

	public bool IsDefault { get; set; }

	public byte[] Image { get; set; }

	public bool ImageEdited { get; set; }
}
