namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class CashTurnOverDataProvider : CashBankTurnOverDataProvider
{
	public override string Name => "CashTurnOver";

	public CashTurnOverDataProvider(TradeClient client)
		: base(client)
	{
	}
}
