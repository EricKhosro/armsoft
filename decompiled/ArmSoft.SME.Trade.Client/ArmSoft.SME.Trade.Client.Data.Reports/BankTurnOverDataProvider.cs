namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class BankTurnOverDataProvider : CashBankTurnOverDataProvider
{
	public override string Name => "BankTurnOver";

	public BankTurnOverDataProvider(TradeClient client)
		: base(client)
	{
	}
}
