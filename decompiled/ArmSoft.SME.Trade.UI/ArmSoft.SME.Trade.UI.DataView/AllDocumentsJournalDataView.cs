using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10101)]
public class AllDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public AllDocumentsJournalDataView()
		: base(ReportTypesSE.AllDocument, (DocumentTypeSE)null)
	{
		DataProvider = new AllDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
