namespace ArmSoft.SME.Trade.Common.Models;

public class TerminalRow
{
	public int LineNumber { get; set; }

	public int Id { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public string BarCode { get; set; }

	public string Unit { get; set; }

	public string AlternativeUnit { get; set; }

	public decimal? Quantity { get; set; }
}
