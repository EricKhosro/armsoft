using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "State")]
public class TransitStatesSE : ASSmartEnum<TransitStatesSE, short>
{
	public static readonly TransitStatesSE InTransit = new TransitStatesSE("InTransit", 0, () => Resources.InTransit);

	public static readonly TransitStatesSE Completed = new TransitStatesSE("Completed", 1, () => Resources.Completed);

	public TransitStatesSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
