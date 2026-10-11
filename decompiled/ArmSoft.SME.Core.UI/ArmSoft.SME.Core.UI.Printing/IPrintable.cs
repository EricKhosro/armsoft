using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Core.UI.Printing;

public interface IPrintable
{
	short PrintTemplateDocumentType { get; }

	DocumentPrintWrapper GetDocumentPrintWrapper(TemplatesCacheRow template);
}
