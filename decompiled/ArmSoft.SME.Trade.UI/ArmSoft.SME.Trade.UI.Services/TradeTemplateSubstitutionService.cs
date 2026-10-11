using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.PrintableElements;
using ArmSoft.SME.Core.UI.Printing;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Printing;

namespace ArmSoft.SME.Trade.UI.Services;

public class TradeTemplateSubstitutionService : TemplateSubstitutionService
{
	protected override ApiClient Client => TradeSettings.CreateTradeClient();

	public override DocumentPrintWrapper GetDocumentPrintWrapperOffline(PrintableElement printableElement, TemplatesCacheRow template)
	{
		if (printableElement.PrintTemplateDocumentType == DocumentTypeSE.MTBill.Value)
		{
			return new ArmSoft.SME.Trade.UI.Printing.MTBill((ArmSoft.SME.Trade.Common.Models.Documents.MTBill)printableElement, null, template).ConstructTemplate();
		}
		if (printableElement is MTBillWithECRResponse)
		{
			MTBillWithECRResponse mTBillWithECRResponse = (MTBillWithECRResponse)printableElement;
			return new ArmSoft.SME.Trade.UI.Printing.MTBill(mTBillWithECRResponse.MTBillDoc, mTBillWithECRResponse.MTBillECRCheck, template).ConstructTemplate();
		}
		if (printableElement is MTBillReturnWithECRResponse)
		{
			MTBillReturnWithECRResponse mTBillReturnWithECRResponse = (MTBillReturnWithECRResponse)printableElement;
			return new ArmSoft.SME.Trade.UI.Printing.MTBillReturn(mTBillReturnWithECRResponse.MTBillReturnDoc, mTBillReturnWithECRResponse.MTBillReturnECRCheck, template).ConstructTemplate();
		}
		if (printableElement.PrintTemplateDocumentType == DocumentTypeSE.MTBillReturn.Value)
		{
			return new ArmSoft.SME.Trade.UI.Printing.MTBillReturn((ArmSoft.SME.Trade.Common.Models.Documents.MTBillReturn)printableElement, null, template).ConstructTemplate();
		}
		if (printableElement.PrintTemplateDocumentType == DocumentTypeSE.MTReturnFromCustomer.Value)
		{
			return new ArmSoft.SME.Trade.UI.Printing.MTReturnFromCustomer((ArmSoft.SME.Trade.Common.Models.Documents.MTReturnFromCustomer)printableElement, template).ConstructTemplate();
		}
		if (printableElement.PrintTemplateDocumentType.ToString() == PrintTemplateDocumentTypeSE.PosTerminalReceipt.Value)
		{
			return new ArmSoft.SME.Trade.UI.Printing.PosTerminalReceipt((ArmSoft.SME.Trade.Common.Models.PrintableElements.PosTerminalReceipt)printableElement, template).ConstructTemplate();
		}
		return base.GetDocumentPrintWrapperOffline(printableElement, template);
	}
}
