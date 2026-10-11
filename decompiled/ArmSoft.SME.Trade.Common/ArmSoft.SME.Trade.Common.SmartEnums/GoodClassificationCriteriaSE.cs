using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "CategorizationCriteria")]
public class GoodClassificationCriteriaSE : ASSmartEnum<GoodClassificationCriteriaSE, int>
{
	public static readonly GoodClassificationCriteriaSE Quantity = new GoodClassificationCriteriaSE("Quantity", 0, () => ArmSoft.SME.Common.Resources.Quantity);

	public static readonly GoodClassificationCriteriaSE Income = new GoodClassificationCriteriaSE("Income", 1, () => ArmSoft.SME.Common.Resources.Income);

	public static readonly GoodClassificationCriteriaSE Profit = new GoodClassificationCriteriaSE("Profit", 2, () => ArmSoft.SME.Common.Resources.ProfitWord);

	public GoodClassificationCriteriaSE(string name, int value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
