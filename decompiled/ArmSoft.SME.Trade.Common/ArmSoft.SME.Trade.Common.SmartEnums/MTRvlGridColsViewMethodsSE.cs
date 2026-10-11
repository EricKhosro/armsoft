using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DisplayableCols")]
public class MTRvlGridColsViewMethodsSE : ASSmartEnum<MTRvlNewPriceFormationTypesSE, string>
{
	public static readonly MTRvlGridColsViewMethodsSE NCCSummWithoutVAT = new MTRvlGridColsViewMethodsSE("NCCSummWithoutVAT", "1", () => Resources.MatRepCurrentCost + Resources.WithoutNDS, () => Resources.MatRepCurrentCost + Resources.WithoutNDS);

	public static readonly MTRvlGridColsViewMethodsSE NCCSummWithVAT = new MTRvlGridColsViewMethodsSE("NCCSummWithVAT", "2", () => Resources.MatRepCurrentCost + Resources.WithNDS, () => Resources.MatRepCurrentCost + Resources.WithNDS);

	public static readonly MTRvlGridColsViewMethodsSE CurrSummWithVAT = new MTRvlGridColsViewMethodsSE("CurrSummWithVAT", "3", () => Resources.CurrPrice, () => Resources.CurrPrice);

	public static readonly MTRvlGridColsViewMethodsSE All = new MTRvlGridColsViewMethodsSE("All", "4", () => ArmSoft.SME.Common.Resources.All, () => ArmSoft.SME.Common.Resources.All);

	public const string NCCSummWithoutVATConst = "1";

	public const string NCCSummWithVATConst = "2";

	public const string CurrSummWithVATConst = "3";

	public const string AllConst = "4";

	private readonly Func<string> fullcaptionDelegate;

	public string FullCaption => fullcaptionDelegate();

	public MTRvlGridColsViewMethodsSE(string name, string value, Func<string> caption, Func<string> fullCaption)
		: base(name, value, caption)
	{
		fullcaptionDelegate = fullCaption;
	}
}
