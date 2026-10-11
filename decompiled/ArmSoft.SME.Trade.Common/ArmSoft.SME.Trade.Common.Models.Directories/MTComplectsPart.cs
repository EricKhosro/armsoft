namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class MTComplectsPart : TradeDirectory
{
	public int MTID { get; set; } = -1;

	public string MTCode { get; set; } = string.Empty;

	public string MTCaption { get; set; } = string.Empty;

	public string ItemType { get; set; } = string.Empty;

	public string ItemTypeCaption { get; set; } = string.Empty;

	public string QntUnit { get; set; } = string.Empty;

	public string QntUnitBrief { get; set; } = string.Empty;

	public decimal Quantity { get; set; }

	public decimal ComplectQuantity { get; set; }

	public decimal SummaPart { get; set; }

	public decimal Summa { get; set; }

	public bool VAT { get; set; }
}
