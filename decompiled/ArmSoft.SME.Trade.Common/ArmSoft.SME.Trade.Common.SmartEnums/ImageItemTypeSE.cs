using Ardalis.SmartEnum;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Special)]
[SmartEnumPartial(SmartEnumType.Base)]
public class ImageItemTypeSE : SmartEnum<ImageItemTypeSE, short>
{
	public static readonly ImageItemTypeSE Materials = new ImageItemTypeSE("Materials", 1);

	public static readonly ImageItemTypeSE FavGroups = new ImageItemTypeSE("FavGroups", 2);

	private ImageItemTypeSE(string name, short value)
		: base(name, value)
	{
	}
}
