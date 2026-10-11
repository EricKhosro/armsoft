using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10111)]
public class ECRCheckDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public ECRCheckDocumentsJournalDataView()
		: base(ReportTypesSE.ECRCheckDocument, (DocumentTypeSE)null)
	{
		DataProvider = new ECRCheckDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
