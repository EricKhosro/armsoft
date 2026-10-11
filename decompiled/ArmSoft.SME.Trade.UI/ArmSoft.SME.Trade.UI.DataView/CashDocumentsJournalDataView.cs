using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10102)]
public class CashDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public CashDocumentsJournalDataView()
		: base(ReportTypesSE.CashDocument, (DocumentTypeSE)null)
	{
		DataProvider = new CashDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
