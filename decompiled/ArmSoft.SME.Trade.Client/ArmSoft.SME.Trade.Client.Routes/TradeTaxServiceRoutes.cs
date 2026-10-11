using ArmSoft.SME.Client;

namespace ArmSoft.SME.Trade.Client.Routes;

public class TradeTaxServiceRoutes : TaxServiceRoutes
{
	protected override string ControllerName => "TradeTaxService";

	public TradeTaxServiceRoutes(TradeClient client)
		: base(client)
	{
	}
}
