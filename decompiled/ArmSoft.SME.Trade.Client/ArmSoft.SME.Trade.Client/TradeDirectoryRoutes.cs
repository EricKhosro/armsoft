using System.Text.Json;
using ArmSoft.SME.Client;
using ArmSoft.SME.Trade.Client.JsonSerialization;

namespace ArmSoft.SME.Trade.Client;

public class TradeDirectoryRoutes : DirectoryRoutes
{
	protected override string ControllerName => "TradeDirectory";

	public TradeDirectoryRoutes(TradeClient client)
		: base(client)
	{
	}

	protected override void UpdateJsonSerializerOptions(JsonSerializerOptions options)
	{
		options.TypeInfoResolver = new TradeDirectoryTypeResolver();
	}
}
