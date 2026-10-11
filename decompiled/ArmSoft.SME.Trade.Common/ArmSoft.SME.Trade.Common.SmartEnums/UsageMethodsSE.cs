using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(2, "UsageMethods")]
public class UsageMethodsSE : ASSmartEnum<UsageMethodsSE, string>
{
	public static readonly UsageMethodsSE GoodDiscountPercent = new UsageMethodsSE("GoodDiscountPercent", "1", () => Resources.UseSelectedPercentDiscount);

	public static readonly UsageMethodsSE Scale = new UsageMethodsSE("Scale", "2", () => Resources.BonusAccumulationMethodScale);

	public static readonly UsageMethodsSE PartnerCard = new UsageMethodsSE("PartnerCard", "3", () => Resources.BonusAccumulationMethodPartnerCard);

	public static readonly UsageMethodsSE DiscountPriceType = new UsageMethodsSE("DiscountPriceType", "4", () => Resources.UseDiscountPriceType);

	public static readonly UsageMethodsSE DiscountPercent = new UsageMethodsSE("DiscountPercent", "5", () => Resources.UseDiscountPercent);

	public static readonly UsageMethodsSE ShowMessage = new UsageMethodsSE("ShowMessage", "6", () => Resources.ShowMessage);

	public static readonly UsageMethodsSE FixedDiscountAmount = new UsageMethodsSE("FixedDiscountAmount", "7", () => Resources.FixedDiscountAmount);

	public static readonly UsageMethodsSE GiftWithLowestPrice = new UsageMethodsSE("GiftWithLowestPrice", "8", () => Resources.GiftWithLowestPrice);

	public static readonly UsageMethodsSE Gift = new UsageMethodsSE("Gift", "9", () => Resources.Gift + " " + Resources.DiscountByGiftSumm);

	public static readonly UsageMethodsSE GiftSet = new UsageMethodsSE("GiftSet", "10", () => Resources.GiftSet + " " + Resources.DiscountByGiftSumm);

	public const string GoodDiscountPercentConst = "1";

	public const string ScaleConst = "2";

	public const string PartnerCardConst = "3";

	public const string DiscountPriceTypeConst = "4";

	public const string DiscountPercentConst = "5";

	public const string ShowMessageConst = "6";

	public const string FixedDiscountAmountConst = "7";

	public const string GiftWithLowestPriceConst = "8";

	public const string GiftConst = "9";

	public const string GiftSetConst = "10";

	public UsageMethodsSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
