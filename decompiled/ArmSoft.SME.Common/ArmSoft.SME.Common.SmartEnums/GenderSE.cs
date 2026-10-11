using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "Sex")]
public class GenderSE : ASSmartEnum<GenderSE, string>
{
	public static readonly GenderSE Male = new GenderSE("Male", "1", () => Resources.SexMale);

	public static readonly GenderSE Female = new GenderSE("Female", "2", () => Resources.SexFemale);

	public const string MaleConst = "1";

	public const string FemaleConst = "2";

	public GenderSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
