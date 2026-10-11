using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10108)]
public class ReceivedTaxInvoicesDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public ReceivedTaxInvoicesDocumentsJournalDataView()
		: base(ReportTypesSE.ReceivedTaxInvoices, (DocumentTypeSE)null)
	{
		DataProvider = new ReceivedTaxInvoicesDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
