using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(20, "DocumentTypes")]
[SmartEnumPartial(SmartEnumType.Base)]
public class PrintTemplateDocumentTypeSE : ASSmartEnum<PrintTemplateDocumentTypeSE, string>
{
	public PrintTemplateDocumentTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}

	public PrintTemplateDocumentTypeSE(DocumentTypeSE docType)
		: base(docType.Name, docType.Value.ToString().PadLeft(2, '0'), docType.FullCaptionDelegate)
	{
	}
}
