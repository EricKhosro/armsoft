using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(2, "GroupBy1")]
public class GroupBySE : ASSmartEnum<GroupBySE, string>
{
	public static readonly GroupBySE ByGood = new GroupBySE("ByGood", "01", () => Resources.GrpByMTAndServices);

	public static readonly GroupBySE ByPartner = new GroupBySE("ByPartner", "02", () => Resources.GrpByPart);

	public GroupBySE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
