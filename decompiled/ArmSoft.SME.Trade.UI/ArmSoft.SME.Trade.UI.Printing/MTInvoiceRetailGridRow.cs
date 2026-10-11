using System;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTInvoiceRetailGridRow : InvoiceGridRow
{
	[PrintDataMember("DiscountPercent")]
	public decimal DiscountPercent { get; set; }

	[PrintDataMember("DiscountedPrice")]
	public decimal DiscountPrice { get; set; }

	[PrintDataMember("DiscountShort")]
	public decimal DiscountSum { get; set; }

	[PrintDataMember("Party")]
	public PartyInfo Party { get; set; }

	[PrintDataMember("EnvFeePercent")]
	public decimal EnvFeePercent { get; set; }

	[PrintDataMember("EnvFeeSumma")]
	public decimal EnvFeeSumma { get; set; }

	[PrintDataMember("RowISN")]
	public Guid RowISN { get; set; }
}
