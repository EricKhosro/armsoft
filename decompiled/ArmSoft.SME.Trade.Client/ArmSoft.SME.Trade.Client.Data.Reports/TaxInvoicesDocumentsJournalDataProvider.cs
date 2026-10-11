namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class TaxInvoicesDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "TaxInvoicesDocumentsJournal";

	public TaxInvoicesDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
