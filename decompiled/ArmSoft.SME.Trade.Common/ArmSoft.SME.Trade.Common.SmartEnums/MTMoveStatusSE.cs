using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "State")]
public class MTMoveStatusSE : ASSmartEnum<MTMoveStatusSE, short>
{
	public static readonly MTMoveStatusSE None = new MTMoveStatusSE("None", 0, () => "");

	public static readonly MTMoveStatusSE OnWay = new MTMoveStatusSE("OnWay", 1, () => Resources.MTMoveStatusOnWay);

	public static readonly MTMoveStatusSE Completed = new MTMoveStatusSE("Completed", 2, () => Resources.MTMoveStatusCompleted);

	public MTMoveStatusSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
