namespace ArmSoft.SME.Trade.Common.CustomTypes;

public class DiscountLogInfo
{
	public string DiscountCode { get; set; } = string.Empty;

	public byte Priority { get; set; }

	public decimal Percent { get; set; } = -1m;

	public decimal DiscountedSum { get; set; }

	public decimal DiscountPrice { get; set; } = -1m;
}
