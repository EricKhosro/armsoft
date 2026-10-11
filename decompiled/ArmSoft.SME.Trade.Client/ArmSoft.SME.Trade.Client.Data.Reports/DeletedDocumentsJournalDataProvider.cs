namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class DeletedDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "DeletedDocumentsJournal";

	public DeletedDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
