using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10110)]
public class TransportationDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public TransportationDocumentsJournalDataView()
		: base(ReportTypesSE.TransportationDocument, (DocumentTypeSE)null)
	{
		DataProvider = new IBCorrespondentDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
