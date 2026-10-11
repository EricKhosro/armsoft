using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTIncomeRemGridRow : MTIncomeGridRow
{
	[PrintDataMember("Storage")]
	public StorageRow Storage { get; set; }

	[PrintDataMember("PriceWithCurrency")]
	public decimal CurrPrice { get; set; }

	[PrintDataMember("SumWithCurrency")]
	public decimal CurrSumma { get; set; }

	[PrintDataMember("PriceWithVAT")]
	public decimal VATPrice { get; set; }
}
