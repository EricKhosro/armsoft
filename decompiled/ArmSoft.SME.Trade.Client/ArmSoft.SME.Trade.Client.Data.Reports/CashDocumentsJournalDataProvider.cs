namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class CashDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "CashDocumentsJournal";

	public CashDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
