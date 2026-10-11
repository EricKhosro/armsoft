using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTGridRow : CodeCaption
{
	[PrintDataMember("Party")]
	public PartyInfo Party { get; set; }

	[PrintDataMember("FullCaption")]
	public string FullName { get; set; }

	[PrintDataMember("Unit")]
	public string UnitBrief { get; set; }

	[PrintDataMember("Quantity", DecimalPrecisionType.Quantity)]
	public decimal Quantity { get; set; }
}
