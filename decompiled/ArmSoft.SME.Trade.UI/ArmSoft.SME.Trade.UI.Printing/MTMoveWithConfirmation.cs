using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(39)]
public class MTMoveWithConfirmation : MTMoveBase
{
	public class MTMoveWithConfirmationGridWithSalePriceRow : MTMoveGridRow
	{
		[PrintDataMember("MTTAccSalePrice")]
		public decimal SalePrice { get; set; }

		[PrintDataMember("SaleSum")]
		public decimal SaleSum { get; set; }
	}

	public class MTMoveWithConfirmationWithoutPartiesGridRow : MTMoveWithoutPartiesGridRow
	{
		[PrintDataMember("ActualQTY")]
		public decimal ActualQuantity { get; set; }
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

	[PrintDataMember("EntryConfirmation")]
	public bool Confirmed { get; set; }

	[PrintDataMember("Status")]
	public string Status { get; set; }

	[PrintDataMember("MTListWithSalePrice", typeof(MTMoveWithConfirmationGridWithSalePriceRow))]
	public BindingList<MTMoveWithConfirmationGridWithSalePriceRow> MTListWithSalePrice { get; set; }

	[PrintDataMember("MTPartyList", typeof(MTMoveWithConfirmationGridWithSalePriceRow))]
	public BindingList<MTMoveWithConfirmationGridWithSalePriceRow> MTPartyList { get; set; }

	[PrintDataMember("MTListWithoutParties", typeof(MTMoveWithConfirmationWithoutPartiesGridRow))]
	public BindingList<MTMoveWithConfirmationWithoutPartiesGridRow> MTListWithoutParties { get; set; }
}
