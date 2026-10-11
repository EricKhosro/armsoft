using System;

namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class Price : TradeDirectory
{
	public DateOnly PriceDate { get; set; }

	public string PriceType { get; set; }

	public string QntUnit { get; set; }

	public string Curr { get; set; }

	public decimal ItemPrice { get; set; }

	public string Comment { get; set; }

	public string GoodCodeOld { get; set; }

	public string PriceTypeOld { get; set; }

	public string UnitCodeOld { get; set; }

	public DateOnly PriceDateOld { get; set; }
}
