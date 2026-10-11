using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(4, "Operation")]
public class PriceFormationPercentUnitSE : ASSmartEnum<PriceFormationPercentUnitSE, string>
{
	public static readonly PriceFormationPercentUnitSE Unit = new PriceFormationPercentUnitSE("Unit", "Unit", () => ArmSoft.SME.Common.Resources.Unit);

	public static readonly PriceFormationPercentUnitSE Percent = new PriceFormationPercentUnitSE("Percent", "%", () => "%");

	public PriceFormationPercentUnitSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
