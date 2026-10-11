using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DisplayableCols")]
public class MTOverheadExpGridColsViewSE : ASSmartEnum<MTOverheadExpGridColsViewSE, short>
{
	public static readonly MTOverheadExpGridColsViewSE NCCSummWithoutVAT = new MTOverheadExpGridColsViewSE("NCCSummWithoutVAT", 1, () => Resources.DistributedSumma);

	public static readonly MTOverheadExpGridColsViewSE NCCSummWithVAT = new MTOverheadExpGridColsViewSE("NCCSummWithVAT", 2, () => Resources.DistributedVATSumma);

	public static readonly MTOverheadExpGridColsViewSE CurrSummWithVAT = new MTOverheadExpGridColsViewSE("CurrSummWithVAT", 3, () => Resources.DistributedCurrSumma);

	public static readonly MTOverheadExpGridColsViewSE All = new MTOverheadExpGridColsViewSE("All", 4, () => ArmSoft.SME.Common.Resources.All);

	public const short NCCSummWithoutVATConst = 1;

	public const short NCCSummWithVATConst = 2;

	public const short CurrSummWithVATConst = 3;

	public const short AllConst = 4;

	public MTOverheadExpGridColsViewSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
