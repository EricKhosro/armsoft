using System.Text.Json;
using ArmSoft.SME.Client;
using ArmSoft.SME.Trade.Client.JsonSerialization;

namespace ArmSoft.SME.Trade.Client;

public class TradeExchangeRoutes : ExchangeRoutes
{
	protected override string ControllerName => "TradeExchange";

	public TradeExchangeRoutes(TradeClient client)
		: base(client)
	{
	}

	protected override void UpdateJsonSerializerOptions(JsonSerializerOptions options)
	{
		options.TypeInfoResolver = new TradeExchangeTypeResolver();
	}
}
