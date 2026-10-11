namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class ECRCheckDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "ECRCheckDocumentsJournal";

	public ECRCheckDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
