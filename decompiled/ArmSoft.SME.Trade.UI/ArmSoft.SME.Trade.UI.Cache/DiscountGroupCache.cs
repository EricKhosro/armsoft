using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class DiscountGroupCache : CacheDefinition<DiscountGroupCacheRow, string>
{
	public override short CacheId => CacheIdSE.DiscountGroups.Value;
}
