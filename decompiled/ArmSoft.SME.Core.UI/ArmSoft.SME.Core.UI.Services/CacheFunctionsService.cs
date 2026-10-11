using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Core.UI.Caches;
using Serilog;

namespace ArmSoft.SME.Core.UI.Services;

public class CacheFunctionsService : ICacheFunctionsService
{
	private Dictionary<short, ICacheDefinition> _cacheDefinitions;

	protected virtual ApiClient Client => Settings.CreateApiClient();

	public Dictionary<short, ICacheDefinition> CacheDefinitions
	{
		get
		{
			if (_cacheDefinitions == null)
			{
				_cacheDefinitions = new Dictionary<short, ICacheDefinition>();
				AddCacheDefinitions();
			}
			return _cacheDefinitions;
		}
	}

	protected virtual void AddCacheDefinitions()
	{
		AddCacheDefinitions<CoreCacheService>(Settings.CoreCacheService);
	}

	protected void AddCacheDefinitions<T>(object obj)
	{
		PropertyInfo[] properties = typeof(T).GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<, >))
			{
				if (!(propertyInfo.GetValue(obj) is Dictionary<short, TreeNodesCache> dictionary))
				{
					continue;
				}
				foreach (KeyValuePair<short, TreeNodesCache> item in dictionary)
				{
					_cacheDefinitions.Add(item.Key, item.Value);
				}
			}
			else
			{
				ICacheDefinition cacheDefinition = (ICacheDefinition)propertyInfo.GetValue(obj);
				_cacheDefinitions.Add(cacheDefinition.CacheId, cacheDefinition);
			}
		}
	}

	public CacheResponse<T> Refresh<T>(CacheRequest request) where T : CacheRowBase, new()
	{
		return Client.CacheRoutes.Refresh<T>(request);
	}

	public void RefreshServiceCaches()
	{
		Client.CacheRoutes.RefreshServiceCaches();
	}

	public void ReloadServiceCaches()
	{
		Client.CacheRoutes.ReloadServiceCaches();
	}

	public List<short> GetChangedCaches()
	{
		List<ChangedCachesRequestRow> list = new List<ChangedCachesRequestRow>();
		foreach (ICacheDefinition value in CacheDefinitions.Values)
		{
			if (value.GetTS() != null)
			{
				list.Add(new ChangedCachesRequestRow(value.CacheId, value.GetTS()));
			}
		}
		return GetChangedCaches(list);
	}

	public List<short> GetChangedCaches(List<ChangedCachesRequestRow> tsItems)
	{
		return Client.CacheRoutes.GetChangedCaches(new ChangedCachesRequest
		{
			TSItems = tsItems
		});
	}

	public void RefreshCaches()
	{
		if (!Settings.IsOffline)
		{
			List<short> changedCaches = GetChangedCaches();
			if (changedCaches != null && changedCaches.Count > 0)
			{
				Mouse.OverrideCursor = Cursors.Wait;
				RefreshCache(out var _, GetChangedCaches(), null, checkChanging: false);
				Mouse.OverrideCursor = null;
			}
			Settings.ParametersService.Refresh();
		}
	}

	public bool RefreshCache(out string error, List<short> cacheIds = null, List<short> exceptCacheIds = null, bool checkChanging = true, bool ignoreDeleted = false)
	{
		bool result = true;
		string text = "";
		foreach (KeyValuePair<short, ICacheDefinition> cacheDefinition in CacheDefinitions)
		{
			try
			{
				bool flag;
				if ((cacheIds != null && cacheIds.Count > 0) || (exceptCacheIds != null && exceptCacheIds.Count > 0))
				{
					flag = false;
					if (cacheIds != null && cacheIds.Count > 0)
					{
						flag = cacheIds.Contains(cacheDefinition.Key);
					}
					if (exceptCacheIds != null && exceptCacheIds.Count > 0 && (flag || (cacheIds != null && cacheIds.Count == 0)))
					{
						flag = !exceptCacheIds.Contains(cacheDefinition.Key);
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					cacheDefinition.Value.Refresh(checkChanging, ignoreDeleted);
				}
			}
			catch (Exception ex)
			{
				text = ex.Message;
				result = false;
				Log.ForContext<CacheFunctionsService>().Error(ex, ex.Message);
			}
		}
		error = text;
		return result;
	}

	public byte[] GetCachesMetadataFile(List<short> cacheIds)
	{
		return Client.CacheRoutes.GetCachesMetadataFile(cacheIds);
	}

	public byte[] GetCachesMetadataZipFile(List<short> cacheIds)
	{
		return Client.CacheRoutes.GetCachesMetadataZipFile(cacheIds);
	}
}
