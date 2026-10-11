using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10114)]
public class ReconcilationStatementsDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public ReconcilationStatementsDocumentsJournalDataView()
		: base(ReportTypesSE.ReconciliationStatements, (DocumentTypeSE)null)
	{
		DataProvider = new ReconcilationStatementsDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
