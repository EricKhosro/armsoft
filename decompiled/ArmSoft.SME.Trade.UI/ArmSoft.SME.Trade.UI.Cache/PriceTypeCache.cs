using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class PriceTypeCache : CacheDefinition<PriceTypeCacheRow, string>
{
	public override short CacheId => CacheIdSE.PriceTypes.Value;

	public override bool StoreInLocalStorage => true;
}
