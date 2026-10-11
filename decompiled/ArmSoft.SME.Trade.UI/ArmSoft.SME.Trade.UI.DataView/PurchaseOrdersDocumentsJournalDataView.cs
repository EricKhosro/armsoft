using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10112)]
public class PurchaseOrdersDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public PurchaseOrdersDocumentsJournalDataView()
		: base(ReportTypesSE.PurchaseOrderDocument, (DocumentTypeSE)null)
	{
		DataProvider = new PurchaseOrdersDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
