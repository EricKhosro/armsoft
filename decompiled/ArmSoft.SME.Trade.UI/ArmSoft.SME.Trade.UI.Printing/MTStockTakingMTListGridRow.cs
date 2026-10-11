using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTStockTakingMTListGridRow : CodeCaption
{
	[PrintDataMember("RowN")]
	public int RowNumber { get; set; }

	[PrintDataMember("FullCaption")]
	public string FullName { get; set; }

	[PrintDataMember("Unit")]
	public string UnitBrief { get; set; }

	[PrintDataMember("ActualQTY")]
	public decimal ActualQty { get; set; }

	[PrintDataMember("ActualSumm")]
	public decimal ActualSumm { get; set; }

	[PrintDataMember("AccQTY")]
	public decimal AccQty { get; set; }

	[PrintDataMember("AccSumm")]
	public decimal AccSumm { get; set; }

	[PrintDataMember("MTTAccSalePrice")]
	public decimal SalePrice { get; set; }
}
