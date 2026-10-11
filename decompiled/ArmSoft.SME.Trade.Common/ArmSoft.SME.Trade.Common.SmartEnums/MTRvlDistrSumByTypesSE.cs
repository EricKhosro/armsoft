using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DistributionBases")]
public class MTRvlDistrSumByTypesSE : ASSmartEnum<MTRvlDistrSumByTypesSE, string>
{
	public static readonly MTRvlDistrSumByTypesSE Sum = new MTRvlDistrSumByTypesSE("Sum", "1", () => ArmSoft.SME.Common.Resources.Summarized, () => ArmSoft.SME.Common.Resources.Summarized);

	public static readonly MTRvlDistrSumByTypesSE Quantity = new MTRvlDistrSumByTypesSE("Quantity", "2", () => Resources.MTTAccQuantitative, () => Resources.MTTAccQuantitative);

	public const string SumConst = "1";

	public const string QuantityConst = "2";

	private readonly Func<string> fullcaptionDelegate;

	public string FullCaption => fullcaptionDelegate();

	public MTRvlDistrSumByTypesSE(string name, string value, Func<string> caption, Func<string> fullCaption)
		: base(name, value, caption)
	{
		fullcaptionDelegate = fullCaption;
	}
}
