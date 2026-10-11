using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(20, "DocumentTypes")]
[SmartEnumPartial(SmartEnumType.Base)]
public class OnlineReportTemplateTypeSE : ASSmartEnum<OnlineReportTemplateTypeSE, string>
{
	public OnlineReportTemplateTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
