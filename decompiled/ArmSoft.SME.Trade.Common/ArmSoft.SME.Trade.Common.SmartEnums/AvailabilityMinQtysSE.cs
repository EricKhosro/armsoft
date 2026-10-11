using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "MinQtys")]
public class AvailabilityMinQtysSE : ASSmartEnum<AvailabilityMinQtysSE, string>
{
	public static readonly AvailabilityMinQtysSE ShowMinQty = new AvailabilityMinQtysSE("ShowMinQty", "1", () => Resources.ShowMinCounts);

	public static readonly AvailabilityMinQtysSE ShowAll = new AvailabilityMinQtysSE("ShowAll", "2", () => ArmSoft.SME.Common.Resources.ShowAll);

	public const string ShowMinQtyConst = "1";

	public const string ShowAllConst = "2";

	public AvailabilityMinQtysSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
