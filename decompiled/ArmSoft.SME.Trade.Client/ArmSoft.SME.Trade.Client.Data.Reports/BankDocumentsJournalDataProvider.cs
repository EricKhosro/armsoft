namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class BankDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "BankDocumentsJournal";

	public BankDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
