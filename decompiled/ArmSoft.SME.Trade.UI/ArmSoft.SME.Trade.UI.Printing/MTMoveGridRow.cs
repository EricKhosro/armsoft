using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTMoveGridRow : MTExpenseGridRow
{
	[PrintDataMember("RowN")]
	public int RowNumber { get; set; }

	[PrintDataMember("OptionsVAT")]
	public bool VAT { get; set; }

	[PrintDataMember("Description")]
	public string RowDescription { get; set; }

	[PrintDataMember("CPAClassification")]
	public string CPACode { get; set; }

	[PrintDataMember("ActualQTY")]
	public decimal ActualQuantity { get; set; }
}
