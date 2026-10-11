using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTIncomeImportGridRow : MTGridRow
{
	[PrintDataMember("PriceWithoutVAT")]
	public decimal CostWithOutVAT { get; set; }

	[PrintDataMember("SumWithoutVAT")]
	public decimal SummWithOutVAT { get; set; }

	[PrintDataMember("BarCode")]
	public string BarCode { get; set; }

	[PrintDataMember("OptionsVAT")]
	public bool VAT { get; set; }

	[PrintDataMember("CPAClassification")]
	public string CPACode { get; set; }

	[PrintDataMember("Specification")]
	public string Description { get; set; }
}
