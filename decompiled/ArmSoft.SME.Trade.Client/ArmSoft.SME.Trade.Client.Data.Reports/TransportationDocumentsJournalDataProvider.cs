namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class TransportationDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "TransportationDocumentsJournal";

	public TransportationDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
