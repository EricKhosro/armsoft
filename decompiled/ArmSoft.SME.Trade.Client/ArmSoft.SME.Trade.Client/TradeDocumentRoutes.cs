using System.Text.Json;
using ArmSoft.SME.Client;
using ArmSoft.SME.Trade.Client.JsonSerialization;

namespace ArmSoft.SME.Trade.Client;

public class TradeDocumentRoutes : DocumentRoutes
{
	protected override string ControllerName => "TradeDocument";

	public TradeDocumentRoutes(TradeClient client)
		: base(client)
	{
	}

	protected override void UpdateJsonSerializerOptions(JsonSerializerOptions options)
	{
		options.TypeInfoResolver = new TradeDocumentTypeResolver();
	}
}
