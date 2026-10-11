using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DiscountTypes")]
public class ECRRowDiscountTypesSE : ASSmartEnum<ECRRowDiscountTypesSE, string>
{
	public static readonly ECRRowDiscountTypesSE Percent = new ECRRowDiscountTypesSE("Percent", "1", () => Resources.ECRRowDiscountTypePercent);

	public static readonly ECRRowDiscountTypesSE Price = new ECRRowDiscountTypesSE("Price", "2", () => Resources.ECRRowDiscountTypePrice);

	public static readonly ECRRowDiscountTypesSE Total = new ECRRowDiscountTypesSE("Total", "3", () => Resources.ECRRowDiscountTypeTotal);

	public const string PercentConst = "1";

	public const string PriceConst = "2";

	public const string TotalConst = "3";

	public ECRRowDiscountTypesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
