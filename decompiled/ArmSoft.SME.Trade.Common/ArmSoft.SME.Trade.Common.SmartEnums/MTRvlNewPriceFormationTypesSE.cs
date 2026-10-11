using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "NewPriceFormationMethods")]
public class MTRvlNewPriceFormationTypesSE : ASSmartEnum<MTRvlNewPriceFormationTypesSE, string>
{
	public static readonly MTRvlNewPriceFormationTypesSE Coefficent = new MTRvlNewPriceFormationTypesSE("Coefficent", "1", () => ArmSoft.SME.Common.Resources.Coefficient, () => ArmSoft.SME.Common.Resources.Coefficient);

	public static readonly MTRvlNewPriceFormationTypesSE DistrSum = new MTRvlNewPriceFormationTypesSE("DistrSum", "2", () => Resources.SumOfDistribution, () => Resources.SumOfDistribution);

	public static readonly MTRvlNewPriceFormationTypesSE Price = new MTRvlNewPriceFormationTypesSE("Price", "3", () => Resources.FixedPrice, () => Resources.FixedPrice);

	public static readonly MTRvlNewPriceFormationTypesSE PriceTypes = new MTRvlNewPriceFormationTypesSE("PriceTypes", "4", () => ArmSoft.SME.Common.Resources.PriceListType, () => ArmSoft.SME.Common.Resources.PriceListType);

	public const string CoefficentConst = "1";

	public const string DistrSumConst = "2";

	public const string PriceConst = "3";

	public const string PriceTypesConst = "4";

	private readonly Func<string> fullcaptionDelegate;

	public string FullCaption => fullcaptionDelegate();

	public MTRvlNewPriceFormationTypesSE(string name, string value, Func<string> caption, Func<string> fullCaption)
		: base(name, value, caption)
	{
		fullcaptionDelegate = fullCaption;
	}
}
