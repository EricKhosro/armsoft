using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTDeComplectationResultGridRow : MTGridRow
{
	[PrintDataMember("Price")]
	public decimal Price { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }
}
