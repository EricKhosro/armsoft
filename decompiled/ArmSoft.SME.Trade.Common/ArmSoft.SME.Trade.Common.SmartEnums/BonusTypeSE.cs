using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "BonusTypes")]
public class BonusTypeSE : ASSmartEnum<BonusTypeSE, string>
{
	public static readonly BonusTypeSE ByPercent = new BonusTypeSE("ByPercent", "1", () => Resources.BonusTypeByPercent);

	public static readonly BonusTypeSE BySum = new BonusTypeSE("BySum", "2", () => Resources.BonusTypeBySum);

	public const string ByPercentConst = "1";

	public const string BySumConst = "2";

	public BonusTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
