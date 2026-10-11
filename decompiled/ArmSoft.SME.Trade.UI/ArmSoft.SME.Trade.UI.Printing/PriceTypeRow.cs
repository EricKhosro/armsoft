using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class PriceTypeRow : CodeCaption
{
	[PrintDataMember("Curr")]
	public string Curr { get; set; }
}
