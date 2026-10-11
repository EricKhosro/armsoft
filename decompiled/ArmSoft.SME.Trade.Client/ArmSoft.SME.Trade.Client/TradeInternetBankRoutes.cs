namespace ArmSoft.SME.Trade.Client;

public class TradeInternetBankRoutes
{
	private readonly TradeClient _client;

	private string ControllerName => "TradeInternetBank";

	public TradeInternetBankRoutes(TradeClient client)
	{
		_client = client;
	}
}
