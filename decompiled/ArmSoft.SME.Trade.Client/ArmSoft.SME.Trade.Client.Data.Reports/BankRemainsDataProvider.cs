namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class BankRemainsDataProvider : CashBankRemainsDataProvider
{
	public override string Name => "BankRemains";

	public BankRemainsDataProvider(TradeClient client)
		: base(client)
	{
	}
}
