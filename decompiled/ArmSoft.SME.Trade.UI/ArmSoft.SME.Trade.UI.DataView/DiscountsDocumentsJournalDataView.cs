using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10116)]
public class DiscountsDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public DiscountsDocumentsJournalDataView()
		: base(ReportTypesSE.DiscountDocument, (DocumentTypeSE)null)
	{
		DataProvider = new DiscountsDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
