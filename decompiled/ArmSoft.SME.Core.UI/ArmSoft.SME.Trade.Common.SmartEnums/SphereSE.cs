using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "TypeWord")]
public class SphereSE : ASSmartEnum<SphereSE, string>
{
	public static readonly SphereSE HouseholdAndGrocery = new SphereSE("HouseholdAndGrocery", "1", () => Resources.HouseholdAndGrocery);

	public static readonly SphereSE PharmacyProducts = new SphereSE("PharmacyProducts", "2", () => Resources.PharmacyProducts);

	public const string HouseholdAndGroceryConst = "1";

	public const string PharmacyProductsConst = "2";

	public SphereSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
