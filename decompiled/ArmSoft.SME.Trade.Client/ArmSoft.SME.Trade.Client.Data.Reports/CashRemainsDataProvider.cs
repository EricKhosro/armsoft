namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class CashRemainsDataProvider : CashBankRemainsDataProvider
{
	public override string Name => "CashRemains";

	public CashRemainsDataProvider(TradeClient client)
		: base(client)
	{
	}
}
