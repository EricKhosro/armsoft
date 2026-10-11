namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class DocumentLinkedListDataProvider : DocumentsJournalDataProvider
{
	public override string Name => "DocumentLinkedList";

	public DocumentLinkedListDataProvider(TradeClient client)
		: base(client)
	{
	}
}
