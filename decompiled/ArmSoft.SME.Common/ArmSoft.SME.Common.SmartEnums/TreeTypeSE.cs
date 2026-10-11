using System;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(8, "TreeDefs")]
[SmartEnumPartial(SmartEnumType.Base)]
public class TreeTypeSE : ASSmartEnum<TreeTypeSE, string>
{
	[JsonIgnore]
	public readonly Func<string> fullCaptionDelegate;

	public string FullCaption => fullCaptionDelegate();

	protected TreeTypeSE(string name, string value, Func<string> caption, Func<string> fullCaption = null)
		: base(name, value, caption)
	{
		fullCaptionDelegate = ((fullCaption == null) ? caption : fullCaption);
	}
}
