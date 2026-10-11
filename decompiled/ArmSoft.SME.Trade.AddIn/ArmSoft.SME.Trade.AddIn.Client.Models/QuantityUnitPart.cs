namespace ArmSoft.SME.Trade.AddIn.Client.Models;

public class QuantityUnitPart : ExchangeBase
{
	public string GoodsCode { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal? Coef { get; set; }

	public bool IsDefault { get; set; }

	public string Image { get; set; } = string.Empty;

	public bool? IsAltUnit { get; set; }
}
