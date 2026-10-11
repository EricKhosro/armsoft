using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "ShowSubTotals")]
public class SubTotalTypesSE : ASSmartEnum<SubTotalTypesSE, string>
{
	public static readonly SubTotalTypesSE ByStorages = new SubTotalTypesSE("ByStorages", "1", () => ArmSoft.SME.Common.Resources.Storage);

	public static readonly SubTotalTypesSE ByGroups = new SubTotalTypesSE("ByGroups", "2", () => ArmSoft.SME.Common.Resources.Group);

	public static readonly SubTotalTypesSE ByGoods = new SubTotalTypesSE("ByGoods", "3", () => Resources.MaterialValue);

	public const string SubTotalByStorage = "1";

	public const string SubTotalByGroup = "2";

	public const string SubTotalByMT = "3";

	public SubTotalTypesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
