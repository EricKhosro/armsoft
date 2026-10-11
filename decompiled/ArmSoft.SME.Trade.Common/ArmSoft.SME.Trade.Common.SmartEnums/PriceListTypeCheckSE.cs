using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "Show")]
public class PriceListTypeCheckSE : ASSmartEnum<PriceListTypeCheckSE, byte>
{
	public static readonly PriceListTypeCheckSE CheckedOnly = new PriceListTypeCheckSE("CheckedOnly", 1, () => Resources.ShowOnlyInPrice);

	public static readonly PriceListTypeCheckSE NotCheckedOnly = new PriceListTypeCheckSE("NotCheckedOnly", 2, () => Resources.ShowOnlyNotInPrice);

	public static readonly PriceListTypeCheckSE All = new PriceListTypeCheckSE("All", 3, () => ArmSoft.SME.Common.Resources.All);

	public PriceListTypeCheckSE(string name, byte value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
