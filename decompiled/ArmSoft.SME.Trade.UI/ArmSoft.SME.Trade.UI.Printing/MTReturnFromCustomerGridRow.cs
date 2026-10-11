using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class MTReturnFromCustomerGridRow : MTIncomeGridRow
{
	[PrintDataMember("Description")]
	public string RowDescription { get; set; }
}
