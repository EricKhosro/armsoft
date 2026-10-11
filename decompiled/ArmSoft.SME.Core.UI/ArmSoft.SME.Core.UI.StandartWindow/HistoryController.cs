using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace ArmSoft.SME.Core.UI.StandartWindow;

public class HistoryController : DependencyObject
{
	public delegate void ItemsChangedEventHandler();

	public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

	private const int _maxCount = 200;

	public static readonly DependencyProperty NewItemsModeProperty = DependencyProperty.Register("NewItemsMode", typeof(bool), typeof(HistoryController));

	public List<HistoryItem> ItemsList { get; }

	public bool NewItemsMode
	{
		get
		{
			return Convert.ToBoolean(((DependencyObject)this).GetValue(NewItemsModeProperty));
		}
		set
		{
			((DependencyObject)this).SetValue(NewItemsModeProperty, (object)value);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewItemsMode"));
		}
	}

	public event ItemsChangedEventHandler ItemsChanged;

	public event PropertyChangedEventHandler PropertyChanged;

	public HistoryController()
	{
		ItemsList = new List<HistoryItem>();
	}

	public void RefreshItems()
	{
		foreach (HistoryItem items in ItemsList)
		{
			items.IsNewItem = false;
		}
		NewItemsMode = false;
	}

	public void AddItem(HistoryItem item)
	{
		if (item.Mode == HistoryItem.RemoveMode.RemoveLast)
		{
			if (ItemsList.Count > 1 && IsSame(ItemsList[0], item))
			{
				ItemsList.RemoveAt(0);
			}
		}
		else if (item.Mode == HistoryItem.RemoveMode.RemoveOld)
		{
			for (int i = 0; i <= ItemsList.Count - 1; i++)
			{
				if (IsSame(ItemsList[i], item))
				{
					ItemsList.RemoveAt(i);
					break;
				}
			}
		}
		if (ItemsList.Count == 200)
		{
			ItemsList.RemoveAt(199);
		}
		if (item.Category == HistoryItem.ItemCategory.Notification)
		{
			item.IsNewItem = true;
			NewItemsMode = true;
		}
		else
		{
			item.IsNewItem = false;
		}
		ItemsList.Insert(0, item);
		ItemsChanged?.Invoke();
	}

	private static bool IsSame(HistoryItem item1, HistoryItem item2)
	{
		if (item1 == null || item2 == null)
		{
			return false;
		}
		if (item1.Category == item2.Category && item1.Id == item2.Id)
		{
			return item1.Type == item2.Type;
		}
		return false;
	}

	public void Clear()
	{
		ItemsList.Clear();
		ItemsChanged?.Invoke();
	}
}
