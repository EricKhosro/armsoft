namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class DiscountsDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "DiscountsDocumentsJournal";

	public DiscountsDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
