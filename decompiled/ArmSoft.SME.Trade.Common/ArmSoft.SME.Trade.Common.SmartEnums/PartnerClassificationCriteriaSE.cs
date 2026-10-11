using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "CategorizationCriteria")]
public class PartnerClassificationCriteriaSE : ASSmartEnum<PartnerClassificationCriteriaSE, int>
{
	public static readonly PartnerClassificationCriteriaSE Income = new PartnerClassificationCriteriaSE("Income", 0, () => ArmSoft.SME.Common.Resources.Income);

	public static readonly PartnerClassificationCriteriaSE Profit = new PartnerClassificationCriteriaSE("Profit", 1, () => ArmSoft.SME.Common.Resources.ProfitWord);

	public PartnerClassificationCriteriaSE(string name, int value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
