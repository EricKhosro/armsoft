using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ArmSoft.SME.Common;
using DevExpress.Xpf.Bars;

namespace ArmSoft.SME.Core.UI.Extensions;

public static class ListExtensions
{
	public static void CheckIfDuplicateFound<T, TKey>(this ObservableCollection<T> list, Func<T, TKey> keySelector, string columnCaption, string listCaption, string listName = "", bool isCompositeKey = false)
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
					throw new ASException(string.Format(colNumRepeatedInTable, (ReadOnlySpan<object?>)buffer), listName);
				}
				throw new ASException(string.Format(Resources.RowsRepeatedInGrid, listCaption, dictionary[key] + 1, i + 1), listName);
			}
		}
	}

	public static void AddWithSeparator(this BarItemLinkCollection list, BarItemLinkBase barItem)
	{
		list.Add(barItem);
		list.Add(new BarItemSeparator());
	}

	public static void AddWithSeparator(this BarItemLinkCollection list, BarStaticItem barItem)
	{
		list.Add(barItem);
		list.Add(new BarItemSeparator());
	}

	public static void AddWithSeparator(this BarItemLinkCollection list, BarButtonItem barItem)
	{
		list.Add(barItem);
		list.Add(new BarItemSeparator());
	}
}
