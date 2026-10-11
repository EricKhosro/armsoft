using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DistributionBases")]
public class DistrSumByTypeSE : ASSmartEnum<DistrSumByTypeSE, string>
{
	public static readonly DistrSumByTypeSE Sum = new DistrSumByTypeSE("Sum", "1", () => ArmSoft.SME.Common.Resources.Summarized);

	public static readonly DistrSumByTypeSE Quantity = new DistrSumByTypeSE("Quantity", "2", () => ArmSoft.SME.Common.Resources.Quantity);

	public static readonly DistrSumByTypeSE Coeff = new DistrSumByTypeSE("Coeff", "3", () => ArmSoft.SME.Common.Resources.Quantity + " x " + ArmSoft.SME.Common.Resources.Coefficient);

	public const string SumConst = "1";

	public const string QuantityConst = "2";

	public const string CoeffConst = "3";

	public DistrSumByTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
