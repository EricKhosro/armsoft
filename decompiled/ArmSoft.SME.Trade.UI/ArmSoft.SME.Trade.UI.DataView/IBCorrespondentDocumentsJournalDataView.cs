using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10109)]
public class IBCorrespondentDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public IBCorrespondentDocumentsJournalDataView()
		: base(ReportTypesSE.IBCorrespondent, (DocumentTypeSE)null)
	{
		DataProvider = new IBCorrespondentDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
