using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class CountryCache : CacheDefinition<CodeCaptionPairsCacheRow, string>
{
	public override short CacheId => CacheIdSE.Countries.Value;
}
