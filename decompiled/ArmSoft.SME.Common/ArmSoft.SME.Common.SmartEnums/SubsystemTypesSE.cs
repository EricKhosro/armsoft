using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(8, "TreeDefs")]
[SmartEnumPartial(SmartEnumType.Base)]
public class SubsystemTypesSE : ASSmartEnum<SubsystemTypesSE, short>
{
	protected SubsystemTypesSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
