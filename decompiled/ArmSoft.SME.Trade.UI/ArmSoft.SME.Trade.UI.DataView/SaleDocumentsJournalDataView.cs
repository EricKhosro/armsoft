using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10105)]
public class SaleDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public SaleDocumentsJournalDataView()
		: base(ReportTypesSE.SaleDocument, (DocumentTypeSE)null)
	{
		DataProvider = new SaleDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
