using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10104)]
public class PurchaseDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public PurchaseDocumentsJournalDataView()
		: base(ReportTypesSE.PurchaseDocument, (DocumentTypeSE)null)
	{
		DataProvider = new PurchaseDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
