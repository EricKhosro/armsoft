using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10115)]
public class BillDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public BillDocumentsJournalDataView()
		: base(ReportTypesSE.BillDocument, (DocumentTypeSE)null)
	{
		DataProvider = new BillDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
