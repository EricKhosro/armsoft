using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Core.UI.Services;

namespace ArmSoft.SME.Core.UI.Caches;

public class TreeNodesCache : CacheDefinition<TreeNodesCacheRow, string>
{
	public TreeNodesCache()
	{
	}

	public TreeNodesCache(short cacheId, string caption)
		: base(cacheId, caption)
	{
	}
}
