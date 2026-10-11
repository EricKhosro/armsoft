namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class CashOperationsDataProvider : CashBankOperationsDataProvider
{
	public override string Name => "CashOperations";

	public CashOperationsDataProvider(TradeClient client)
		: base(client)
	{
	}
}
