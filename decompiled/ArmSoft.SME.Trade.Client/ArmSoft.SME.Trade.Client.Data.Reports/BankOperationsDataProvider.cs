namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class BankOperationsDataProvider : CashBankOperationsDataProvider
{
	public override string Name => "BankOperations";

	public BankOperationsDataProvider(TradeClient client)
		: base(client)
	{
	}
}
