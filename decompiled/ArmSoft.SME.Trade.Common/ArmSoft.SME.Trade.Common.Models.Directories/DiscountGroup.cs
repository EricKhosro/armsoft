namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class DiscountGroup : TradeDirectory
{
	public string Type { get; set; } = string.Empty;

	public string TypeCaption { get; set; } = string.Empty;

	public byte Priority { get; set; }
}
