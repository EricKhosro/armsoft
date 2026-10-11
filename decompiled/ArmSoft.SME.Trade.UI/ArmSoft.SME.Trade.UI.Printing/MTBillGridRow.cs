using System;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTBillGridRow : InvoiceGridRow
{
	[PrintDataMember("DiscountPercent")]
	public decimal DiscountPercent { get; set; }

	[PrintDataMember("DiscountedPrice")]
	public decimal DiscountPrice { get; set; }

	[PrintDataMember("DiscountShort")]
	public decimal DiscountSum { get; set; }

	[PrintDataMember("Party")]
	public PartyInfo Party { get; set; }

	[PrintDataMember("BonusUse")]
	public decimal UsedBonus { get; set; }

	[PrintDataMember("AccumulateBonus")]
	public decimal AccumulatedBonus { get; set; }

	[PrintDataMember("Storage")]
	public string Storage { get; set; }

	[PrintDataMember("Summa1")]
	public decimal Summ1 { get; set; }

	[PrintDataMember("Gift")]
	public bool Gift { get; set; }

	[PrintDataMember("RowISN")]
	public Guid RowISN { get; set; }
}
