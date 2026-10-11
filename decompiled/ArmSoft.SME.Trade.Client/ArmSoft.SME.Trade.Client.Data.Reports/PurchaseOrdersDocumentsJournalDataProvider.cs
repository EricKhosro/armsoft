namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class PurchaseOrdersDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "PurchaseOrdersDocumentsJournal";

	public PurchaseOrdersDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
