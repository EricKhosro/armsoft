using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "Residency")]
public class ResidencyTypeSE : ASSmartEnum<ResidencyTypeSE, string>
{
	public static readonly ResidencyTypeSE Resident = new ResidencyTypeSE("Resident", "1", () => Resources.Resident);

	public static readonly ResidencyTypeSE NotResident = new ResidencyTypeSE("NotResident", "2", () => Resources.NotResident);

	public const string ResidentConst = "1";

	public const string NotResidentConst = "2";

	public ResidencyTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
