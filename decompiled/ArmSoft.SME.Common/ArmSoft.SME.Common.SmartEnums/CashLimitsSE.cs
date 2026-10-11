using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "CashLimits")]
public class CashLimitsSE : ASSmartEnum<CashLimitsSE, string>
{
	public static readonly CashLimitsSE OnlyNonCash = new CashLimitsSE("OnlyNonCash", "1", () => Resources.WholeSummNonCash);

	public static readonly CashLimitsSE WithLimit = new CashLimitsSE("WithLimit", "2", () => Resources.CashLimitSumm);

	public static readonly CashLimitsSE WithoutLimit = new CashLimitsSE("WithoutLimit", "3", () => Resources.NoLimit);

	public CashLimitsSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
