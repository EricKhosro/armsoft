namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class BillDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "BillDocumentsJournal";

	public BillDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
