using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Core.UI.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public abstract class TradeDocumentPrintWrapper : DocumentPrintWrapper
{
	public TradeDocumentPrintWrapper()
	{
	}

	public TradeDocumentPrintWrapper(TemplatesCacheRow template)
		: base(template)
	{
	}
}
