using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTIncomeRetailGridRow : MTIncomeGridRow
{
	[PrintDataMember("MTTAccSalePrice")]
	public decimal SalePrice { get; set; }

	[PrintDataMember("SaleSum")]
	public decimal SaleSum { get; set; }

	[PrintDataMember("Extra")]
	public decimal ExtraPercent { get; set; }
}
