using System.Text.Json;
using ArmSoft.SME.Client;
using ArmSoft.SME.Trade.Client.JsonSerialization;

namespace ArmSoft.SME.Trade.Client;

public class TradeCacheRoutes : CacheRoutes
{
	protected override string ControllerName => "TradeCache";

	public TradeCacheRoutes(TradeClient client)
		: base(client)
	{
	}

	protected override void UpdateJsonSerializerOptions(JsonSerializerOptions options)
	{
		options.TypeInfoResolver = new TradeCacheTypeResolver();
	}
}
