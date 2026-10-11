using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.Caches;

public class CurrentExchangeRatesCache : CacheDefinition<CurrentExchangeRatesCacheRow, string>
{
	public override short CacheId => CacheIdSE.CurrentExchangeRates.Value;

	public override bool StoreInLocalStorage => true;
}
