namespace ArmSoft.SME.Trade.Common.CustomTypes;

public class DiscountGroupResult
{
	public byte GroupPriority { get; set; }

	public string GroupCode { get; set; } = string.Empty;

	public byte DiscountPriority { get; set; }

	public string DiscountCode { get; set; } = string.Empty;

	public decimal DiscountedSum { get; set; }
}
