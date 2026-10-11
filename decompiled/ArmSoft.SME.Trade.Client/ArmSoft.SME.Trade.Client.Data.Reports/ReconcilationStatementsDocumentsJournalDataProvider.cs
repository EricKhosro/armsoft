namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class ReconcilationStatementsDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "ReconcilationStatementsDocumentsJournal";

	public ReconcilationStatementsDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
