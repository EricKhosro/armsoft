using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "CategorizationCriteria")]
public class BestSellersClassificationCriteriaSE : GoodClassificationCriteriaSE
{
	public static readonly BestSellersClassificationCriteriaSE ProfitPercent = new BestSellersClassificationCriteriaSE("ProfitPercent", 3, () => ArmSoft.SME.Common.Resources.ProfitPercent);

	public BestSellersClassificationCriteriaSE(string name, int value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
