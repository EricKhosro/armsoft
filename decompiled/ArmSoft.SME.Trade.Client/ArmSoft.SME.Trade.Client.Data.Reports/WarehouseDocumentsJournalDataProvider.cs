namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class WarehouseDocumentsJournalDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "WarehouseDocumentsJournal";

	public WarehouseDocumentsJournalDataProvider(TradeClient client)
		: base(client)
	{
	}
}
