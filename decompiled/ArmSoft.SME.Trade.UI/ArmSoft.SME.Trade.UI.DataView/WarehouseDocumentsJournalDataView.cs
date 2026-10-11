using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10106)]
public class WarehouseDocumentsJournalDataView : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public WarehouseDocumentsJournalDataView()
		: base(ReportTypesSE.WarehouseDocument, (DocumentTypeSE)null)
	{
		DataProvider = new WarehouseDocumentsJournalDataProvider(TradeSettings.CreateTradeClient());
	}
}
