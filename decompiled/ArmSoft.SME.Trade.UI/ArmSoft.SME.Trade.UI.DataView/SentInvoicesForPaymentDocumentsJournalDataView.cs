using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10113)]
public class SentInvoicesForPaymentDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public SentInvoicesForPaymentDocumentsJournalDataView()
		: base(ReportTypesSE.SentInvoicesForPayment, (DocumentTypeSE)null)
	{
		DataProvider = new SentInvoicesForPaymentDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
