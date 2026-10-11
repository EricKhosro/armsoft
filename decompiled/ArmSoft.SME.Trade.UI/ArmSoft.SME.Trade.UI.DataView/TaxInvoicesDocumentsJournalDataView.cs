using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10107)]
public class TaxInvoicesDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public TaxInvoicesDocumentsJournalDataView()
		: base(ReportTypesSE.TaxInvoices, (DocumentTypeSE)null)
	{
		DataProvider = new TaxInvoicesDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
