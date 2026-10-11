using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10103)]
public class BankDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public BankDocumentsJournalDataView()
		: base(ReportTypesSE.BankDocument, (DocumentTypeSE)null)
	{
		DataProvider = new BankDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
