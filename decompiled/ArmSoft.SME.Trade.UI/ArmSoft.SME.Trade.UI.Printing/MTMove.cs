using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(10)]
public class MTMove : MTMoveBase
{
	public class MTMoveGridWithSalePriceRow : MTMoveGridRow
	{
		[PrintDataMember("MTTAccSalePrice")]
		public decimal SalePrice { get; set; }

		[PrintDataMember("SaleSum")]
		public decimal SaleSum { get; set; }
	}

	[PrintDataMember("SupBookN")]
	public string SupBookN { get; set; }

	[PrintDataMember("PageN")]
	public string SupBookPageN { get; set; }

	[PrintDataMember("RowN")]
	public string SupBookRowN { get; set; }

	[PrintDataMember("SupBookNPageNRowN")]
	public string SupBookNPageNRowN { get; set; }

	[PrintDataMember("Summa")]
	public decimal Summ { get; set; }

	[PrintDataMember("SaleSum")]
	public decimal SaleSum { get; set; }

	[PrintDataMember("MTListWithSalePrice", typeof(MTMoveGridWithSalePriceRow))]
	public BindingList<MTMoveGridWithSalePriceRow> MTListWithSalePrice { get; set; }

	[PrintDataMember("MTPartyList", typeof(MTMoveGridWithSalePriceRow))]
	public BindingList<MTMoveGridWithSalePriceRow> MTPartyList { get; set; }

	[PrintDataMember("MTListWithoutParties", typeof(MTMoveWithoutPartiesGridRow))]
	public BindingList<MTMoveWithoutPartiesGridRow> MTListWithoutParties { get; set; }
}
