using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Caches;
using ArmSoft.SME.Core.UI.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Services;

public class CacheDefinition<TRow, TKey> : ICacheDefinition where TRow : CacheRowBase<TKey>, new()
{
	protected Dictionary<TKey, TRow> _itemsByKey = new Dictionary<TKey, TRow>();

	private Dictionary<int, TRow> _itemsById;

	private List<TRow> _items;

	protected bool _isLoadedAtLeastOnce;

	protected string _caption;

	public virtual short CacheId { get; }

	public virtual bool StoreInLocalStorage { get; }

	public virtual bool StoreInUserBasedLocalStorage { get; }

	protected virtual bool CacheAlsoWithId { get; }

	public byte[] TS { get; private set; }

	public byte[] RowsMaxTS { get; private set; } = TimeStamp.NewTS();

	public IReadOnlyDictionary<int, TRow> ItemsWithIds
	{
		get
		{
			if (CacheAlsoWithId)
			{
				if (_itemsById == null)
				{
					_itemsById = GetList().ToDictionary((TRow p) => p.GetId(), (TRow p) => p);
				}
				return _itemsById;
			}
			throw new NotImplementedException();
		}
	}

	public IReadOnlyDictionary<TKey, TRow> ItemsWithKeys
	{
		get
		{
			if (!_isLoadedAtLeastOnce)
			{
				Refresh();
			}
			return _itemsByKey;
		}
	}

	public byte[] GetTS()
	{
		return TS;
	}

	public CacheDefinition()
	{
		_caption = SmartEnumHelper.GetSmartEnumRowCaption(typeof(CacheIdSE), CacheId.ToString());
	}

	public CacheDefinition(short cacheId, string caption)
	{
		CacheId = cacheId;
		_caption = caption;
	}

	public void Refresh(bool checkChanging = true, bool ignoreDeleted = false)
	{
		InnerRefresh(checkChanging, ignoreDeleted, out var _);
	}

	protected virtual bool InnerRefresh(bool checkChanging, bool ignoreDeleted, out List<TKey> updatedKeys)
	{
		updatedKeys = new List<TKey>();
		List<CacheRequestRow> list = null;
		SQLiteStorageService requiredService = Settings.ServiceProvider.GetRequiredService<SQLiteStorageService>();
		requiredService.StoreInUserBasedLocalStorage = StoreInUserBasedLocalStorage;
		if (checkChanging && Settings.IsOffline && (StoreInLocalStorage || StoreInUserBasedLocalStorage))
		{
			checkChanging = false;
		}
		if (checkChanging)
		{
			ICacheFunctionsService cacheFunctionsService = Settings.CacheFunctionsService;
			int num = 1;
			List<ChangedCachesRequestRow> list2 = new List<ChangedCachesRequestRow>(num);
			CollectionsMarshal.SetCount(list2, num);
			CollectionsMarshal.AsSpan(list2)[0] = new ChangedCachesRequestRow(CacheId, TS);
			List<short> changedCaches = cacheFunctionsService.GetChangedCaches(list2);
			if (changedCaches != null && changedCaches.Count == 0)
			{
				_isLoadedAtLeastOnce = true;
				return false;
			}
		}
		if (_itemsByKey.Count == 0 && (StoreInLocalStorage || StoreInUserBasedLocalStorage))
		{
			List<CacheRowJSON> cacheData = requiredService.GetCacheData(CacheId);
			if (cacheData != null && cacheData.Count > 0)
			{
				TS = requiredService.GetCacheTS(CacheId);
				list = new List<CacheRequestRow>(cacheData.Count);
				foreach (CacheRowJSON item in cacheData)
				{
					if (!ignoreDeleted)
					{
						list.Add(new CacheRequestRow(item.KeyJSON, item.TS));
					}
					_itemsByKey.Add(JsonSerializer.Deserialize<TKey>(item.KeyJSON, JsonSerializerFactory.JsonOptions), JsonSerializer.Deserialize<TRow>(item.Value, JsonSerializerFactory.JsonOptions));
					RowsMaxTS = TimeStamp.Max(item.TS, RowsMaxTS);
				}
				_items?.Clear();
				_items = null;
				if (CacheAlsoWithId)
				{
					_itemsById?.Clear();
					_itemsById = null;
				}
				if (Settings.IsOffline)
				{
					_isLoadedAtLeastOnce = true;
					return false;
				}
				ICacheFunctionsService cacheFunctionsService2 = Settings.CacheFunctionsService;
				int num = 1;
				List<ChangedCachesRequestRow> list3 = new List<ChangedCachesRequestRow>(num);
				CollectionsMarshal.SetCount(list3, num);
				CollectionsMarshal.AsSpan(list3)[0] = new ChangedCachesRequestRow(CacheId, TS);
				List<short> changedCaches2 = cacheFunctionsService2.GetChangedCaches(list3);
				if (changedCaches2 != null && changedCaches2.Count == 0)
				{
					_isLoadedAtLeastOnce = true;
					return false;
				}
			}
			else if (Settings.IsOffline)
			{
				_isLoadedAtLeastOnce = true;
				return false;
			}
		}
		else if (_itemsByKey.Count > 0 && !ignoreDeleted)
		{
			list = new List<CacheRequestRow>(_itemsByKey.Count);
			list.AddRange(_itemsByKey.Select((KeyValuePair<TKey, TRow> p) => new CacheRequestRow(p.Value.KeyJSON, p.Value.TS)));
		}
		CacheRequest request = new CacheRequest
		{
			TS = TS,
			CacheId = CacheId,
			TSItems = list,
			RowsMaxTS = RowsMaxTS
		};
		if (!Settings.IsOffline)
		{
			CacheResponse<TRow> cacheResponse = Settings.CacheFunctionsService.Refresh<TRow>(request);
			if (cacheResponse != null && (TS == null || MemoryExtensions.SequenceEqual<byte>((ReadOnlySpan<byte>)TS, (ReadOnlySpan<byte>)TimeStamp.NewTS()) || !MemoryExtensions.SequenceEqual<byte>((ReadOnlySpan<byte>)TS, (ReadOnlySpan<byte>)cacheResponse.TS)))
			{
				if (!ignoreDeleted)
				{
					TS = cacheResponse.TS;
				}
				RowsMaxTS = cacheResponse.RowsMaxTS;
				if (StoreInLocalStorage || StoreInUserBasedLocalStorage)
				{
					requiredService.AddOrUpdateCacheTS(CacheId, cacheResponse.TS);
				}
				List<string> delete = cacheResponse.Delete;
				if (delete != null && delete.Count > 0)
				{
					foreach (string item2 in cacheResponse.Delete)
					{
						_itemsByKey.Remove(JsonSerializer.Deserialize<TKey>(item2));
					}
					if (StoreInLocalStorage || StoreInUserBasedLocalStorage)
					{
						requiredService.DeleteCacheData(CacheId, cacheResponse.Delete);
					}
				}
				List<TRow> merge = cacheResponse.Merge;
				if (merge != null && merge.Count > 0)
				{
					foreach (TRow item3 in cacheResponse.Merge)
					{
						updatedKeys.Add(item3.Key);
						if (_itemsByKey.TryAdd(item3.Key, item3))
						{
							if (!(CacheAlsoWithId & ignoreDeleted))
							{
								continue;
							}
							TRow itemById = GetItemById(item3.GetId());
							if (itemById != null && itemById.KeyJSON != item3.KeyJSON)
							{
								_itemsByKey.Remove(itemById.Key);
								if (StoreInLocalStorage || StoreInUserBasedLocalStorage)
								{
									short cacheId = CacheId;
									int num = 1;
									List<string> list4 = new List<string>(num);
									CollectionsMarshal.SetCount(list4, num);
									CollectionsMarshal.AsSpan(list4)[0] = itemById.KeyJSON;
									requiredService.DeleteCacheData(cacheId, list4);
								}
							}
						}
						else
						{
							_itemsByKey[item3.Key] = item3;
						}
					}
					if (StoreInLocalStorage || StoreInUserBasedLocalStorage)
					{
						requiredService.AddOrUpdateCacheData(CacheId, cacheResponse.Merge);
					}
				}
				_items?.Clear();
				_items = null;
				if (CacheAlsoWithId)
				{
					_itemsById?.Clear();
					_itemsById = null;
				}
			}
		}
		_isLoadedAtLeastOnce = true;
		return true;
	}

	public virtual IReadOnlyList<TRow> GetList(Func<TRow, bool> predicate = null)
	{
		if (_items == null)
		{
			_items = ItemsWithKeys.Values.ToList();
		}
		if (predicate == null)
		{
			return _items;
		}
		List<TRow> list = new List<TRow>();
		list.AddRange(_items.Where(predicate));
		return new _003C_003Ez__ReadOnlyList<TRow>(list);
	}

	public virtual List<TRow> GetClonedList(Func<TRow, bool> predicate = null)
	{
		return GetList(predicate).Select(Clone).ToList();
	}

	private static TRow Clone(TRow sourceRow)
	{
		TRow val = new TRow();
		PropertyInfo[] properties = sourceRow.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (propertyInfo.CanRead && propertyInfo.CanWrite)
			{
				propertyInfo.SetValue(val, propertyInfo.GetValue(sourceRow, null), null);
			}
		}
		return val;
	}

	public TRow GetItem(TKey key, bool throwExceptionNotExits = false)
	{
		TRow value = null;
		TKey key2 = key;
		if (key != null)
		{
			if (key is string text)
			{
				string text2 = text.Trim();
				if (throwExceptionNotExits && string.IsNullOrWhiteSpace(text2))
				{
					throw new RESTException(string.Format(Resources.FieldIsEmpty, _caption));
				}
				if (!string.Equals(text, text2, StringComparison.Ordinal))
				{
					key2 = (TKey)(object)text2;
				}
			}
			ItemsWithKeys.TryGetValue(key2, out value);
		}
		string elemWithCodeNotExists;
		object arg;
		object message;
		if ((value == null) & throwExceptionNotExits)
		{
			TKey val;
			if (typeof(TKey).IsPrimitive || typeof(TKey) == typeof(string))
			{
				elemWithCodeNotExists = Resources.ElemWithCodeNotExists;
				ref TKey reference = ref key2;
				val = default;
				if (val == null)
				{
					val = reference;
					reference = ref val;
					if (val == null)
					{
						arg = null;
						goto IL_00de;
					}
				}
				arg = reference.ToString();
				goto IL_00de;
			}
			ref TKey reference2 = ref key;
			val = default;
			if (val == null)
			{
				val = reference2;
				reference2 = ref val;
				if (val == null)
				{
					message = null;
					goto IL_0133;
				}
			}
			message = reference2.ToString();
			goto IL_0133;
		}
		return value;
		IL_00de:
		throw new ASException(string.Format(elemWithCodeNotExists, arg, _caption))
		{
			ErrorCode = 20004
		};
		IL_0133:
		throw new ASException((string)message);
	}

	public TRow GetItemById(int id, bool throwExceptionNotExits = false)
	{
		ItemsWithIds.TryGetValue(id, out var value);
		if ((value == null) & throwExceptionNotExits)
		{
			throw new ASException(string.Format(Resources.ElemWithISNNotExists, id.ToString(), _caption));
		}
		return value;
	}

	public string GetItemName(string key)
	{
		if (typeof(TKey).IsPrimitive || typeof(TKey) == typeof(string))
		{
			TKey key2 = (TKey)Convert.ChangeType(key, typeof(TKey));
			return (GetItem(key2) as CacheRowBaseWithName<TKey>)?.Name;
		}
		return null;
	}

	public virtual void Clear()
	{
		TS = null;
		RowsMaxTS = null;
		_itemsByKey?.Clear();
		if (CacheAlsoWithId)
		{
			_itemsById?.Clear();
			_itemsById = null;
		}
		_items?.Clear();
		_items = null;
		_isLoadedAtLeastOnce = false;
	}

	protected object ExtensionUIResolverFunction(DirectoryExtensionValuesCacheRow cacheRow, string fieldName)
	{
		if (cacheRow == null)
		{
			return null;
		}
		int num = fieldName.IndexOf("_TreeNodeName_");
		if (num >= 0)
		{
			string codeFieldName = fieldName.Substring(0, num);
			string value = cacheRow.Values.FirstOrDefault((KeyValuePair<string, string> p) => p.Key == codeFieldName).Value;
			if (!string.IsNullOrWhiteSpace(value))
			{
				return ((TreeNodesCache)Settings.CoreCacheService.Tree.GetItem(fieldName.Substring(num + "_TreeNodeName_".Length), throwExceptionNotExits: true).TreeNodeCache).GetItem(value)?.Name;
			}
			return null;
		}
		num = fieldName.IndexOf("_DropDownName_");
		if (num >= 0)
		{
			string codeFieldName2 = fieldName.Substring(0, num);
			string value2 = cacheRow.Values.FirstOrDefault((KeyValuePair<string, string> p) => p.Key == codeFieldName2).Value;
			if (!string.IsNullOrWhiteSpace(value2))
			{
				return Settings.CacheFunctionsService.CacheDefinitions[Convert.ToInt16(fieldName.Substring(num + "_DropDownName_".Length))].GetItemName(value2);
			}
			return null;
		}
		return cacheRow.Values.FirstOrDefault((KeyValuePair<string, string> p) => p.Key == fieldName).Value;
	}
}
