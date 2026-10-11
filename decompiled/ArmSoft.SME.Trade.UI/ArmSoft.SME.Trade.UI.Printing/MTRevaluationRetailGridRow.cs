using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTRevaluationRetailGridRow : CodeCaption
{
	[PrintDataMember("RowN")]
	public int RowNumber { get; set; }

	[PrintDataMember("FullCaption")]
	public string FullName { get; set; }

	[PrintDataMember("Quantity", DecimalPrecisionType.Quantity)]
	public decimal Quantity { get; set; }

	[PrintDataMember("Unit")]
	public string UnitBrief { get; set; }

	[PrintDataMember("OldCost")]
	public decimal OldCost { get; set; }

	[PrintDataMember("NewPrice")]
	public decimal NewCost { get; set; }
}
