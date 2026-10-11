using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "TypeWord")]
public class POTypeSE : ASSmartEnum<POTypeSE, short>
{
	public static readonly POTypeSE Directory = new POTypeSE("Directory", 1, () => Resources.Directory);

	public static readonly POTypeSE Document = new POTypeSE("Document", 2, () => Resources.Document);

	public POTypeSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
