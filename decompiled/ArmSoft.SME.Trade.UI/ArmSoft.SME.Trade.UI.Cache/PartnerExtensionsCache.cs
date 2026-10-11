using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class PartnerExtensionsCache : CacheDefinition<DirectoryExtensionValuesCacheRow, string>
{
	public override short CacheId => CacheIdSE.PartnerExtensions.Value;
}
