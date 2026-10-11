using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class QntUnitCache : CacheDefinition<QntUnitCacheRow, string>
{
	public override bool StoreInLocalStorage => true;

	public override short CacheId => CacheIdSE.QntUnits.Value;
}
