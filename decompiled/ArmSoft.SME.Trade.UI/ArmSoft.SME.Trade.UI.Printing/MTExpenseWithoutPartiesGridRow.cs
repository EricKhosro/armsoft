using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTExpenseWithoutPartiesGridRow : MTGridWithoutPartiesRow
{
	[PrintDataMember("Price")]
	public decimal Price { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }

	[PrintDataMember("BarCode")]
	public string BarCode { get; set; }

	[PrintDataMember("MTTAccSalePrice")]
	public decimal SalePrice { get; set; }

	[PrintDataMember("SaleSum")]
	public decimal SaleSum { get; set; }
}
