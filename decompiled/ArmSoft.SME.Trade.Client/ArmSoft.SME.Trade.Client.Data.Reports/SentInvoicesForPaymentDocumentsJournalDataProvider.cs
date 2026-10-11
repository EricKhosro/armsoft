namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class SentInvoicesForPaymentDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "SentInvoicesForPaymentDocumentsJournal";

	public SentInvoicesForPaymentDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
