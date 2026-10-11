using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "ChAccFIO")]
public class ChiefAccPositionsSE : ASSmartEnum<ChiefAccPositionsSE, string>
{
	public static readonly ChiefAccPositionsSE ChAccPost1 = new ChiefAccPositionsSE("ChAccPost1", "10", () => Resources.ChAccPost1);

	public static readonly ChiefAccPositionsSE ChAccPost2 = new ChiefAccPositionsSE("ChAccPost2", "11", () => Resources.ChAccPost2);

	public static readonly ChiefAccPositionsSE ChAccPost3 = new ChiefAccPositionsSE("ChAccPost3", "12", () => Resources.ChAccPost3);

	public ChiefAccPositionsSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
