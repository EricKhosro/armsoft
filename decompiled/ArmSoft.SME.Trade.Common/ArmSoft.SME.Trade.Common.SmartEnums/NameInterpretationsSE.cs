using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "NameInterpretation")]
public class NameInterpretationsSE : ASSmartEnum<NameInterpretationsSE, string>
{
	public static readonly NameInterpretationsSE InterpretationName = new NameInterpretationsSE("InterpretationName", "1", () => ArmSoft.SME.Common.Resources.Name);

	public static readonly NameInterpretationsSE FullName = new NameInterpretationsSE("FullName", "2", () => ArmSoft.SME.Common.Resources.FullName);

	public static readonly NameInterpretationsSE Specification = new NameInterpretationsSE("Specification", "3", () => ArmSoft.SME.Common.Resources.Specification);

	public const string InterpretationNameConst = "1";

	public const string FullNameConst = "2";

	public const string SpecificationConst = "3";

	public NameInterpretationsSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
