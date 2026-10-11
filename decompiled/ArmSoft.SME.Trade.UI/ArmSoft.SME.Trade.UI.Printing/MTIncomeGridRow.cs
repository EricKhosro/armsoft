using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTIncomeGridRow : MTIncomeImportGridRow
{
	[PrintDataMember("Price")]
	public decimal Cost { get; set; }

	[PrintDataMember("DiscountPercentSymbol")]
	public decimal Discount { get; set; }

	[PrintDataMember("PriceListCost")]
	public decimal InitCost { get; set; }

	[PrintDataMember("PriceListCostWithoutVAT")]
	public decimal InitCostWithOutVAT { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }
}
