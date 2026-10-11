using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class CurrentPricesCache : CacheDefinition<CurrentPricesCacheRow, CurrentPricesCacheRow.RowKey>
{
	public override short CacheId => CacheIdSE.CurrentPrices.Value;

	public override bool StoreInLocalStorage => true;
}
