using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTRevaluationGridRow : MTGridRow
{
	[PrintDataMember("Storage")]
	public string Storage { get; set; }

	[PrintDataMember("OldCost")]
	public decimal OldCost { get; set; }

	[PrintDataMember("NewPrice")]
	public decimal NewCost { get; set; }

	[PrintDataMember]
	public decimal OldCostVAT { get; set; }

	[PrintDataMember]
	public decimal NewCostVAT { get; set; }

	[PrintDataMember]
	public decimal OldCurCost { get; set; }

	[PrintDataMember]
	public decimal NewCurCost { get; set; }
}
