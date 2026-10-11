using Ardalis.SmartEnum;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Base)]
public class ImageItemTypeSE : SmartEnum<ImageItemTypeSE, short>
{
	private ImageItemTypeSE(string name, short value)
		: base(name, value)
	{
	}
}
