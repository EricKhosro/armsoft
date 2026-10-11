namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class IBCorrespondentDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "IBCorrespondentDocumentsJournal";

	public IBCorrespondentDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
