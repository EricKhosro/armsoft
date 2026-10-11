using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DiscountConditionTypes")]
public class DiscountConditionTypesSE : ASSmartEnum<DiscountConditionTypesSE, string>
{
	public static readonly DiscountConditionTypesSE OneTimeSaleSumma = new DiscountConditionTypesSE("OneTimeSaleSumma", "1", () => Resources.BonusAccumulationBaseOneTimeSaleSum);

	public static readonly DiscountConditionTypesSE AccumulativeSaleSumma = new DiscountConditionTypesSE("AccumulativeSaleSumma", "2", () => Resources.BonusAccumulationBaseAccumulatedSalesAmount);

	public static readonly DiscountConditionTypesSE OneTimeSaleRowQuantity = new DiscountConditionTypesSE("OneTimeSaleRowQuantity", "3", () => Resources.OneTimeSaleRowQuantity);

	public static readonly DiscountConditionTypesSE OneTimeSaleTotalQuantity = new DiscountConditionTypesSE("OneTimeSaleTotalQuantity", "4", () => Resources.OneTimeSaleTotalQuantity);

	public const string OneTimeSaleSummaConst = "1";

	public const string AccumulativeSaleSummaConst = "2";

	public const string OneTimeSaleRowQuantityConst = "3";

	public const string OneTimeSaleTotalQuantityConst = "4";

	public DiscountConditionTypesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
