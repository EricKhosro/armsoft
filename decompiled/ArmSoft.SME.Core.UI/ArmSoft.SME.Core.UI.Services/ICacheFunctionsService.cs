using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Core.UI.Services;

public interface ICacheFunctionsService
{
	Dictionary<short, ICacheDefinition> CacheDefinitions { get; }

	CacheResponse<T> Refresh<T>(CacheRequest request) where T : CacheRowBase, new();

	void RefreshServiceCaches();

	void ReloadServiceCaches();

	void RefreshCaches();

	bool RefreshCache(out string error, List<short> cacheIds = null, List<short> exceptCacheIds = null, bool checkChanging = true, bool ignoreDeleted = false);

	List<short> GetChangedCaches();

	List<short> GetChangedCaches(List<ChangedCachesRequestRow> tsItems);

	byte[] GetCachesMetadataFile(List<short> cacheIds);

	byte[] GetCachesMetadataZipFile(List<short> cacheIds);
}
