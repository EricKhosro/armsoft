namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class BonusScale : TradeDirectory
{
	public decimal BeginSumma { get; set; }

	public decimal EndSumma { get; set; }

	public decimal Percent { get; set; }

	public decimal Point { get; set; }
}
