using System;
using System.ComponentModel;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

[PrintModel(36)]
public class MTMoveOrder : MTMoveBase
{
	[PrintDataMember]
	public DateOnly SupplyDate { get; set; }

	[PrintDataMember]
	public bool IsMade { get; set; }

	[PrintDataMember("MTListWithAll")]
	public BindingList<MTMoveWithoutPartiesGridRow> MTListUncompressed { get; set; }
}
