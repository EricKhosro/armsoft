using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTColStListGridRow
{
	[PrintDataMember("RowN")]
	public int RowNumber { get; set; }

	[PrintDataMember("Name")]
	public string Name { get; set; }

	[PrintDataMember("FullCaption")]
	public string FullName { get; set; }

	[PrintDataMember("Code")]
	public string Code { get; set; }

	[PrintDataMember("Unit")]
	public string UnitBrief { get; set; }

	[PrintDataMember("DeficitQty")]
	public decimal DeficitQTY { get; set; }

	[PrintDataMember("DeficitSumm")]
	public decimal DeficitSumm { get; set; }

	[PrintDataMember("SurplusQty")]
	public decimal SurplusQTY { get; set; }

	[PrintDataMember("SurplusSumm")]
	public decimal SurplusSumm { get; set; }
}
