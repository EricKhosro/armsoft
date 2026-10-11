namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class SaleDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "SaleDocumentsJournal";

	public SaleDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
