using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Documents;

namespace ArmSoft.SME.Core.UI.DataViews;

[DataView(34)]
public class OnlineReportTemplates : TemplatesBase
{
	public OnlineReportTemplates()
		: base(PrintTemplateTypeSE.OnlineReport)
	{
	}
}
