using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTReturnToSupplierGridRow : InvoiceGridRow
{
	[PrintDataMember("DiscountPercent")]
	public decimal DiscountPercent { get; set; }

	[PrintDataMember("DiscountedPrice")]
	public decimal DiscountPrice { get; set; }

	[PrintDataMember("MTTAccSalePrice")]
	public decimal SalePrice { get; set; }

	[PrintDataMember("SaleSum")]
	public decimal SaleSum { get; set; }

	[PrintDataMember("Party")]
	public PartyInfo Party { get; set; }

	[PrintDataMember("Storage")]
	public StorageRow Storage { get; set; }
}
