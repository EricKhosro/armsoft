using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "BonusAccumulationMethods")]
public class BonusAccumulationMethodSE : ASSmartEnum<BonusAccumulationMethodSE, string>
{
	public static readonly BonusAccumulationMethodSE Simple = new BonusAccumulationMethodSE("Simple", "1", () => Resources.BonusAccumulationMethodSimple);

	public static readonly BonusAccumulationMethodSE Scale = new BonusAccumulationMethodSE("Scale", "2", () => Resources.BonusAccumulationMethodScale);

	public static readonly BonusAccumulationMethodSE PartnerCard = new BonusAccumulationMethodSE("PartnerCard", "3", () => Resources.BonusAccumulationMethodPartnerCard);

	public const string SimpleConst = "1";

	public const string ScaleConst = "2";

	public const string PartnerCardConst = "3";

	public BonusAccumulationMethodSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
