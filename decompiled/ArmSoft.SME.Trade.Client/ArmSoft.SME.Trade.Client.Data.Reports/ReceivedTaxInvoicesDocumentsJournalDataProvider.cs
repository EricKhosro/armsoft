namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class ReceivedTaxInvoicesDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "ReceivedTaxInvoicesDocumentsJournal";

	public ReceivedTaxInvoicesDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
