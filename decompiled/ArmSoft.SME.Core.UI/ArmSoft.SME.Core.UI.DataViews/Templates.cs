using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Documents;

namespace ArmSoft.SME.Core.UI.DataViews;

[DataView(6)]
public class Templates : TemplatesBase
{
	public Templates()
		: base(PrintTemplateTypeSE.Doc)
	{
	}
}
