using System;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

public class Prices : ExchangeBase
{
	public string Code { get; set; } = string.Empty;

	public string PriceType { get; set; } = string.Empty;

	public string Unit { get; set; } = string.Empty;

	public DateTime Date { get; set; }

	public string CurrencyCode { get; set; } = string.Empty;

	public decimal Price { get; set; }

	public string Comment { get; set; } = "";
}
