using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "BonusAccumulationBases")]
public class BonusAccumulationBaseSE : ASSmartEnum<BonusAccumulationBaseSE, string>
{
	public static readonly BonusAccumulationBaseSE OneTimeSaleSumm = new BonusAccumulationBaseSE("OneTimeSaleSumm", "1", () => Resources.BonusAccumulationBaseOneTimeSaleSum);

	public static readonly BonusAccumulationBaseSE AccumulativeSaleSumma = new BonusAccumulationBaseSE("AccumulativeSaleSumma", "2", () => Resources.BonusAccumulationBaseAccumulatedSalesAmount);

	public static readonly BonusAccumulationBaseSE WithoutCondition = new BonusAccumulationBaseSE("WithoutCondition", "3", () => Resources.BonusAccumulationBaseWithoutCondition);

	public const string OneTimeSaleSummaConst = "1";

	public const string AccumulativeSaleSummaConst = "2";

	public const string WithoutConditionConst = "3";

	public BonusAccumulationBaseSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
