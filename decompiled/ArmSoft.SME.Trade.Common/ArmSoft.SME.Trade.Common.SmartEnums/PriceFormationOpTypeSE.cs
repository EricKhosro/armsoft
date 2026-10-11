using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "Operation")]
public class PriceFormationOpTypeSE : ASSmartEnum<PriceFormationOpTypeSE, string>
{
	public static readonly PriceFormationOpTypeSE PricesInputing = new PriceFormationOpTypeSE("PricesInputing", "1", () => Resources.PricesInputing);

	public static readonly PriceFormationOpTypeSE AutoForming = new PriceFormationOpTypeSE("AutoForming", "2", () => Resources.AutoForming);

	public static readonly PriceFormationOpTypeSE Delete = new PriceFormationOpTypeSE("Delete", "3", () => ArmSoft.SME.Common.Resources.Delete);

	public PriceFormationOpTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
