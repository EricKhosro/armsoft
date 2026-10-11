using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.Caches;

public class CPACodeCache : CacheDefinition<CPACodesCacheRow, string>
{
	public override bool StoreInLocalStorage => true;

	public override short CacheId => CacheIdSE.CPAClassifiers.Value;
}
