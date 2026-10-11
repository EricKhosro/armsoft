using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "ManagerPost")]
public class ManagerPositionsSE : ASSmartEnum<ManagerPositionsSE, string>
{
	public static readonly ManagerPositionsSE ManagerPost1 = new ManagerPositionsSE("ManagerPost1", "01", () => Resources.ManagerPost1);

	public static readonly ManagerPositionsSE ManagerPost2 = new ManagerPositionsSE("ManagerPost2", "02", () => Resources.ManagerPost2);

	public static readonly ManagerPositionsSE ManagerPost3 = new ManagerPositionsSE("ManagerPost3", "03", () => Resources.ManagerPost3);

	public ManagerPositionsSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
