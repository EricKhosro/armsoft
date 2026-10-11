using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class UnitPriceRow
{
	[PrintDataMember("UnitPrice")]
	public decimal Price { get; set; }

	[PrintDataMember("Name")]
	public string Name { get; set; }

	[PrintDataMember("Code")]
	public string Code { get; set; }

	[PrintDataMember("Brief")]
	public string Brief { get; set; }

	[PrintDataMember("UnitPriceWithBrief")]
	public string PriceWithBrief { get; set; }

	[PrintDataMember("UnitPriceWithBriefWithoutRounding")]
	public string PriceWithBriefWithoutRounding { get; set; }
}
