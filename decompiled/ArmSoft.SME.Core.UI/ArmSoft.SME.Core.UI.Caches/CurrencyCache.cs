using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.Caches;

public class CurrencyCache : CacheDefinition<CurrenciesCacheRow, string>
{
	public override short CacheId => CacheIdSE.Currencies.Value;

	public override bool StoreInLocalStorage => true;
}
