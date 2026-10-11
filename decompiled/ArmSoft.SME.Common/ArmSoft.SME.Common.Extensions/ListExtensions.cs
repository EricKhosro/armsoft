using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ArmSoft.SME.Common.Exceptions;

namespace ArmSoft.SME.Common.Extensions;

public static class ListExtensions
{
	public static bool TryGetFirstOrDefault<T>(this List<T> value, Func<T, bool> predicate, out T result)
	{
		result = default;
		T val = value.FirstOrDefault(predicate);
		if (val != null)
		{
			result = val;
			return true;
		}
		return false;
	}

	public static bool HasDuplicates<T, TKey>(this List<T> list, Func<T, TKey> keySelector)
	{
		if (list == null || list.Count <= 1)
		{
			return false;
		}
		HashSet<TKey> hashSet = new HashSet<TKey>();
		foreach (T item2 in list)
		{
			TKey item = keySelector(item2);
			if (!hashSet.Add(item))
			{
				return true;
			}
		}
		return false;
	}

	public static void CheckIfDuplicateFound<T, TKey>(this List<T> list, Func<T, TKey> keySelector, string columnCaption, string listCaption, string listName = "", bool isCompositeKey = false)
	{
		if (list == null || list.Count <= 1)
		{
			return;
		}
		Dictionary<TKey, int> dictionary = new Dictionary<TKey, int>();
		for (int i = 0; i < list.Count; i++)
		{
			TKey key = keySelector(list[i]);
			if (!dictionary.TryAdd(key, i))
			{
				if (!isCompositeKey)
				{
					string colNumRepeatedInTable = Resources.ColNumRepeatedInTable;
					InlineArray4<object> buffer = default;
					buffer[0] = columnCaption;
					buffer[1] = listCaption;
					buffer[2] = dictionary[key] + 1;
					buffer[3] = i + 1;
					throw new RESTException(string.Format(colNumRepeatedInTable, (ReadOnlySpan<object?>)buffer), listName);
				}
				throw new RESTException(string.Format(Resources.RowsRepeatedInGrid, listCaption, dictionary[key] + 1, i + 1), listName);
			}
		}
	}
}
