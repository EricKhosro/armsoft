using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DistributionType")]
public class DistrTypeSE : ASSmartEnum<DistrTypeSE, string>
{
	public static readonly DistrTypeSE Automatically = new DistrTypeSE("Automatically", "1", () => ArmSoft.SME.Common.Resources.Automatically);

	public static readonly DistrTypeSE Manually = new DistrTypeSE("Manually", "2", () => ArmSoft.SME.Common.Resources.Manually);

	public const string AutomaticallyConst = "1";

	public const string ManuallyConst = "2";

	public DistrTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
