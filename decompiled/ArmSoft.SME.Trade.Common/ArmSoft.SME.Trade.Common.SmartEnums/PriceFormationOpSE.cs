using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "Operation")]
public class PriceFormationOpSE : ASSmartEnum<PriceFormationOpSE, string>
{
	public static readonly PriceFormationOpSE Plus = new PriceFormationOpSE("Plus", "+", () => Resources.PricesInputing);

	public static readonly PriceFormationOpSE Minus = new PriceFormationOpSE("Minus", "-", () => Resources.AutoForming);

	public static readonly PriceFormationOpSE Multiply = new PriceFormationOpSE("Multiply", "*", () => ArmSoft.SME.Common.Resources.Delete);

	public PriceFormationOpSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
