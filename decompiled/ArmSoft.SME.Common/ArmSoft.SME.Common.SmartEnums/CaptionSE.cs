using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "ASECRCheckNamesHeader")]
public class CaptionSE : ASSmartEnum<CaptionSE, string>
{
	public static readonly CaptionSE ItemCaption = new CaptionSE("ItemCaption", "1", () => Resources.Name);

	public static readonly CaptionSE ItemFullCaption = new CaptionSE("ItemFullCaption", "2", () => Resources.FullCaption);

	private CaptionSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
