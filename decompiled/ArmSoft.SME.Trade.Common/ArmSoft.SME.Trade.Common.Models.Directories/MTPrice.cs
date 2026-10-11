using System;

namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class MTPrice : TradeDirectory
{
	public int MTId { get; set; }

	public decimal Price { get; set; }

	public string Unit { get; set; }

	public string PriceType { get; set; }

	public DateOnly Date { get; set; }

	public bool HasPriceType { get; set; }

	public string Curr { get; set; }
}
