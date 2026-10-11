using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTExpenseGridRow : MTGridRow
{
	[PrintDataMember("UnitCode")]
	public string UnitCode { get; set; }

	[PrintDataMember("Price")]
	public decimal Price { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }

	[PrintDataMember("BarCode")]
	public string BarCode { get; set; }
}
