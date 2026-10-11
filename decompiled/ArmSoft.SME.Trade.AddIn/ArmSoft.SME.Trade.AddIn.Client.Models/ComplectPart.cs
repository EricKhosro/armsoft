namespace ArmSoft.SME.Trade.AddIn.Client.Models;

public class ComplectPart : ExchangeBase
{
	public string DestCode { get; set; } = string.Empty;

	public string ItemType { get; set; } = string.Empty;

	public string SourceCode { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal ComplectQuantity { get; set; }

	public decimal SummPart { get; set; }

	public decimal Summ { get; set; }
}
