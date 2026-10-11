using ArmSoft.SME.Client;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.UI.Cache;

namespace ArmSoft.SME.Trade.UI.Services;

public class TradeCacheFunctionsService : CacheFunctionsService
{
	protected override ApiClient Client => TradeSettings.CreateTradeClient();

	protected override void AddCacheDefinitions()
	{
		base.AddCacheDefinitions();
		AddCacheDefinitions<TradeCacheService>(TradeSettings.TradeCacheService);
	}
}
