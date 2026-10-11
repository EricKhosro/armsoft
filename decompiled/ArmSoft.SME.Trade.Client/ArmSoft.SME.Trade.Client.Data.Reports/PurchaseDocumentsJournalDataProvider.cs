namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class PurchaseDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "PurchaseDocumentsJournal";

	public PurchaseDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
