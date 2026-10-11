using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DisplayableCols")]
public class GridViewColsTypeSE : ASSmartEnum<GridViewColsTypeSE, string>
{
	public static readonly GridViewColsTypeSE NCCSummWithoutVAT = new GridViewColsTypeSE("NCCSummWithoutVAT", "1", () => Resources.MatRepCurrentCost + Resources.WithoutNDS);

	public static readonly GridViewColsTypeSE NCCSummWithVAT = new GridViewColsTypeSE("NCCSummWithVAT", "2", () => Resources.MatRepCurrentCost + Resources.WithNDS);

	public static readonly GridViewColsTypeSE CurrSummWithVAT = new GridViewColsTypeSE("CurrSummWithVAT", "3", () => Resources.PriceCurr);

	public static readonly GridViewColsTypeSE All = new GridViewColsTypeSE("All", "4", () => ArmSoft.SME.Common.Resources.All);

	public const string NCCSummWithoutVATConst = "1";

	public const string NCCSummWithVATConst = "2";

	public const string CurrSummWithVATConst = "3";

	public const string AllConst = "4";

	public GridViewColsTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
