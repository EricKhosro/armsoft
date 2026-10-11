namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class AllDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "AllDocumentsJournal";

	public AllDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
