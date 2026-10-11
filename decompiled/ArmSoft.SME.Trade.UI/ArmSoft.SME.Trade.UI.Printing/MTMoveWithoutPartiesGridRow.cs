using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTMoveWithoutPartiesGridRow : MTExpenseWithoutPartiesGridRow
{
	[PrintDataMember("RowN")]
	public int RowNumber { get; set; }

	[PrintDataMember("OptionsVAT")]
	public bool VAT { get; set; }

	[PrintDataMember("CPAClassification")]
	public string CPACode { get; set; }
}
