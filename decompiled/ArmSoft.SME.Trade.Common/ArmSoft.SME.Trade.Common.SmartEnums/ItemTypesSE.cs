using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "TypeWord")]
public class ItemTypesSE : ASSmartEnum<ItemTypesSE, string>
{
	public static readonly ItemTypesSE All = new ItemTypesSE("All", "0", () => ArmSoft.SME.Common.Resources.All);

	public static readonly ItemTypesSE Goods = new ItemTypesSE("Goods", "1", () => Resources.Goods);

	public static readonly ItemTypesSE Services = new ItemTypesSE("Services", "2", () => ArmSoft.SME.Common.Resources.Services);

	public const string AllConst = "0";

	public const string GoodsConst = "1";

	public const string ServicesConst = "2";

	public ItemTypesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
