using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.Caches;

public class StringResourceCache : CacheDefinition<ResourcesCacheRow, string>
{
	public override short CacheId => CacheIdSE.StringResources.Value;
}
