using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DiscountsMergeMethod")]
public class DiscountsMergeMethodsSE : ASSmartEnum<DiscountsMergeMethodsSE, string>
{
	public static readonly DiscountsMergeMethodsSE Summation = new DiscountsMergeMethodsSE("Summation", "1", () => Resources.Summation);

	public static readonly DiscountsMergeMethodsSE SequentialUsage = new DiscountsMergeMethodsSE("SequentialUsage", "2", () => Resources.SequentialUsage);

	public static readonly DiscountsMergeMethodsSE MaxPercent = new DiscountsMergeMethodsSE("MaxPercent", "3", () => Resources.MaxPercent);

	public static readonly DiscountsMergeMethodsSE Priority = new DiscountsMergeMethodsSE("Priority", "4", () => Resources.Priority);

	public DiscountsMergeMethodsSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
