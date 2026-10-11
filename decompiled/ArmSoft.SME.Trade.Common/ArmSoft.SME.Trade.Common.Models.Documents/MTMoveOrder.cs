using System;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTMoveOrder : MTMoveBase
{
	public DateOnly? SupplyDate { get; set; }

	public bool? IsMade { get; set; }
}
