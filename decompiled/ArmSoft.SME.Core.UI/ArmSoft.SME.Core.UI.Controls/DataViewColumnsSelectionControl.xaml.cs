using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI.DataViews;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class DataViewColumnsSelectionControl : UserControl, IComponentConnector
{
	private class TVColumn : TreeViewItem
	{
		public ViewColumn Column;

		public bool IsLeaf;
	}

	private class LVColumn : ListViewItem
	{
		public ViewColumn Column;
	}

	public delegate void ColumnAddedEventHandler();

	public delegate void ColumnRemovedEventHandler();

	public delegate void ResetClickEventHandler();

	private readonly Dictionary<string, TVColumn> _groups;

	private int _draggedValue;

	public IDataView IDV { get; set; }

	public DataViewSetting DVS { get; set; }

	public event ColumnAddedEventHandler ColumnAdded;

	public event ColumnRemovedEventHandler ColumnRemoved;

	public event ResetClickEventHandler ResetClick;

	public DataViewColumnsSelectionControl()
	{
		InitializeComponent();
		btAdd.Content = ArmSoft.SME.Common.Resources.NewAdd + " >";
		btRemove.Content = "< " + ArmSoft.SME.Common.Resources.Delete;
		btMoveDown.Content = ArmSoft.SME.Common.Resources.Down;
		btMoveUp.Content = ArmSoft.SME.Common.Resources.Up;
		btReset.Content = ArmSoft.SME.Common.Resources.Reset;
		lblFilter.Text = ArmSoft.SME.Common.Resources.FilterString;
		lblFilter.Visibility = ((Settings.SystemType != SystemTypes.SalesManagement) ? Visibility.Collapsed : Visibility.Visible);
		txtFilter.Visibility = lblFilter.Visibility;
		TBAvailableColumns.Text = ArmSoft.SME.Common.Resources.AvailableColumns;
		TBColumns.Text = ArmSoft.SME.Common.Resources.ShowColumns;
		_groups = new Dictionary<string, TVColumn>();
		Style itemContainerStyle = new Style(typeof(ListBoxItem))
		{
			Setters = 
			{
				(SetterBase)new Setter(UIElement.AllowDropProperty, true),
				(SetterBase)new EventSetter(UIElement.PreviewMouseMoveEvent, new MouseEventHandler(MoveListBoxItem)),
				(SetterBase)new EventSetter(UIElement.DropEvent, new DragEventHandler(DropListBoxItem))
			}
		};
		SelectedColumns.AllowDrop = true;
		SelectedColumns.DragOver += new SafeEventHandler<DragEventArgs>(ScrollOnDrag).Handler;
		SelectedColumns.ItemContainerStyle = itemContainerStyle;
		SelectedColumns.SelectionChanged += new SafeEventHandler<SelectionChangedEventArgs>(SelectedColumns_SelectionChanged).Handler;
		AvailableColumns.SelectedItemChanged += new SafeEventHandler<RoutedPropertyChangedEventArgs<object>>(AvailableColumns_SelectedItemChanged).Handler;
		btAdd.Click += new SafeEventHandler<RoutedEventArgs>(ButtonAdd_Click).Handler;
		btRemove.Click += new SafeEventHandler<RoutedEventArgs>(ButtonRemove_Click).Handler;
		btReset.Click += new SafeEventHandler<RoutedEventArgs>(ButtonReset_Click).Handler;
		btMoveUp.Click += new SafeEventHandler<RoutedEventArgs>(ButtonMoveUp_Click).Handler;
		btMoveDown.Click += new SafeEventHandler<RoutedEventArgs>(ButtonMoveDown_Click).Handler;
	}

	private void MoveListBoxItem(object sender, MouseEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed && sender is ListBoxItem)
		{
			ListBoxItem listBoxItem = (ListBoxItem)sender;
			listBoxItem.IsSelected = true;
			_draggedValue = SelectedColumns.Items.IndexOf(listBoxItem);
			DragDrop.DoDragDrop((DependencyObject)(object)listBoxItem, ((LVColumn)listBoxItem).Column.Caption, DragDropEffects.Move);
		}
	}

	private void ScrollOnDrag(object sender, DragEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		ScrollViewer scrollViewer = DataViewPanel.FindScrollViewer((DependencyObject)(object)SelectedColumns);
		if (scrollViewer != null)
		{
			int num = 10;
			Point position = e.GetPosition(SelectedColumns);
			double y = position.Y;
			int num2 = 3;
			if (y < (double)num)
			{
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - (double)num2);
			}
			else if (y > SelectedColumns.ActualHeight - (double)num)
			{
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + (double)num2);
			}
		}
	}

	private void DropListBoxItem(object sender, DragEventArgs e)
	{
		object obj = SelectedColumns.Items[_draggedValue];
		if (e.Source is ListBoxItem)
		{
			ListBoxItem item = (ListBoxItem)e.Source;
			int draggedValue = _draggedValue;
			int num = SelectedColumns.Items.IndexOf(item);
			if (draggedValue > num)
			{
				SelectedColumns.Items.RemoveAt(draggedValue);
				SelectedColumns.Items.Insert(num, obj);
			}
			else
			{
				SelectedColumns.Items.RemoveAt(draggedValue);
				SelectedColumns.Items.Insert(num, obj);
			}
			SelectedColumns.SelectedItem = obj;
			SelectedColumns.ScrollIntoView(obj);
			_draggedValue = 0;
		}
	}

	public void Configure()
	{
		SelectedColumns.Items.Clear();
		AvailableColumns.Items.Clear();
		_groups.Clear();
		if (DVS == null || DVS.Columns.Count == 0)
		{
			foreach (ViewColumn view in IDV.Columns.ViewList)
			{
				if (view.Visible)
				{
					AddToSelectedColumns(view);
				}
			}
		}
		else
		{
			foreach (string column in DVS.Columns)
			{
				foreach (ViewColumn potentiallyColumn in IDV.PotentiallyColumns)
				{
					if (potentiallyColumn.Visible && potentiallyColumn.Name == column)
					{
						AddToSelectedColumns(potentiallyColumn);
					}
				}
			}
		}
		if (DVS != null)
		{
			txtFilter.Text = DVS.FilterString;
		}
		foreach (ViewColumn item in IDV.PotentiallyColumns.Where((ViewColumn p) => p.Visible))
		{
			TVColumn tVColumn = null;
			if (string.IsNullOrWhiteSpace(item.Group))
			{
				tVColumn = new TVColumn
				{
					IsLeaf = true,
					Header = item.Caption,
					Column = item
				};
				tVColumn.MouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(AvailableColumns_MouseDoubleClick).Handler;
				AvailableColumns.Items.Add(tVColumn);
			}
			else
			{
				TVColumn tVColumn2;
				if (_groups.TryGetValue(item.Group, out var value))
				{
					tVColumn2 = value;
				}
				else
				{
					tVColumn2 = new TVColumn
					{
						IsLeaf = false,
						Header = item.Group,
						IsExpanded = true
					};
					AvailableColumns.Items.Add(tVColumn2);
					_groups.Add(item.Group, tVColumn2);
				}
				tVColumn = new TVColumn
				{
					IsLeaf = true,
					Header = item.Caption,
					Column = item
				};
				tVColumn.MouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(AvailableColumns_MouseDoubleClick).Handler;
				tVColumn2.Items.Add(tVColumn);
			}
			if (SelectedColumnsIndexOf(item) == -1)
			{
				tVColumn.Visibility = Visibility.Visible;
			}
			else
			{
				tVColumn.Visibility = Visibility.Collapsed;
			}
		}
		foreach (TVColumn value2 in _groups.Values)
		{
			bool flag = false;
			foreach (TVColumn item2 in (IEnumerable)value2.Items)
			{
				if (item2.Visibility == Visibility.Visible)
				{
					flag = true;
					break;
				}
			}
			value2.Visibility = ((!flag) ? Visibility.Collapsed : Visibility.Visible);
		}
		if (SelectedColumns.Items.Count > 0)
		{
			SelectedColumns.SelectedIndex = 0;
		}
		foreach (TVColumn item3 in (IEnumerable)AvailableColumns.Items)
		{
			if (item3.Visibility == Visibility.Visible)
			{
				item3.IsSelected = true;
				break;
			}
		}
	}

	private LVColumn AddToSelectedColumns(ViewColumn col)
	{
		LVColumn lVColumn = ((!col.IsSystem) ? new LVColumn
		{
			Content = col.Caption,
			Column = col
		} : new LVColumn
		{
			Column = col,
			Content = new TextBlock
			{
				Text = col.Caption,
				Foreground = Brushes.Gray
			},
			ToolTip = string.Format(ArmSoft.SME.Common.Resources.VisibilityControlledBySystem, col.Caption)
		});
		lVColumn.MouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(SelectedColumns_MouseDoubleClick).Handler;
		SelectedColumns.Items.Add(lVColumn);
		return lVColumn;
	}

	private int SelectedColumnsIndexOf(ViewColumn col)
	{
		int result = -1;
		for (int i = 0; i <= SelectedColumns.Items.Count - 1; i++)
		{
			if (((LVColumn)SelectedColumns.Items[i]).Column == col)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	private void AvailableColumns_MouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		AddColumn();
	}

	private void ButtonAdd_Click(object sender, RoutedEventArgs e)
	{
		AddColumn();
	}

	private void ButtonRemove_Click(object sender, RoutedEventArgs e)
	{
		RemoveColumn();
	}

	private void SelectedColumns_MouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		RemoveColumn();
	}

	private void AddColumn()
	{
		if (AvailableColumns.SelectedItem != null)
		{
			TVColumn tVColumn = (TVColumn)AvailableColumns.SelectedItem;
			if (tVColumn.IsLeaf && tVColumn.IsVisible)
			{
				LVColumn selectedItem = AddToSelectedColumns(tVColumn.Column);
				SelectedColumns.SelectedItem = selectedItem;
				tVColumn.Visibility = Visibility.Collapsed;
				if (string.IsNullOrWhiteSpace(tVColumn.Column.Group))
				{
					SetSelected(AvailableColumns.Items, tVColumn, null);
				}
				else
				{
					SetSelected(_groups[tVColumn.Column.Group].Items, tVColumn, _groups[tVColumn.Column.Group]);
				}
			}
		}
		RefreshButtonsState();
		ColumnAdded?.Invoke();
	}

	private void SetSelected(ItemCollection items, TVColumn item, TVColumn groupItem)
	{
		short num = 0;
		TVColumn tVColumn = null;
		foreach (TVColumn item2 in (IEnumerable)items)
		{
			if (num == 0)
			{
				if (item2 == item)
				{
					num = 1;
				}
				else if (item2.IsVisible)
				{
					tVColumn = item2;
				}
			}
			else if (item2.IsVisible)
			{
				item2.IsSelected = true;
				num = 2;
				break;
			}
		}
		if (num == 2)
		{
			return;
		}
		if (tVColumn != null)
		{
			tVColumn.IsSelected = true;
			return;
		}
		if (groupItem != null)
		{
			groupItem.IsSelected = true;
			return;
		}
		foreach (TVColumn item3 in (IEnumerable)AvailableColumns.Items)
		{
			if (item3.IsVisible)
			{
				item3.IsSelected = true;
				break;
			}
		}
	}

	private void RemoveColumn()
	{
		if (SelectedColumns.SelectedItem != null)
		{
			LVColumn lVColumn = (LVColumn)SelectedColumns.SelectedItem;
			if (lVColumn.Column.IsSystem)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(lVColumn.Column.Group))
			{
				foreach (TVColumn item in (IEnumerable)AvailableColumns.Items)
				{
					if (item.Column == lVColumn.Column)
					{
						item.Visibility = Visibility.Visible;
						item.IsSelected = true;
						break;
					}
				}
			}
			else
			{
				_groups[lVColumn.Column.Group].Visibility = Visibility.Visible;
				_groups[lVColumn.Column.Group].IsExpanded = true;
				foreach (TVColumn item2 in (IEnumerable)_groups[lVColumn.Column.Group].Items)
				{
					if (item2.Column == lVColumn.Column)
					{
						item2.Visibility = Visibility.Visible;
						item2.IsSelected = true;
						break;
					}
				}
			}
			int selectedIndex = SelectedColumns.SelectedIndex;
			SelectedColumns.Items.Remove(lVColumn);
			if (SelectedColumns.Items.Count > selectedIndex)
			{
				SelectedColumns.SelectedIndex = selectedIndex;
			}
			else
			{
				SelectedColumns.SelectedIndex = SelectedColumns.Items.Count - 1;
			}
		}
		RefreshButtonsState();
		ColumnRemoved?.Invoke();
	}

	private void RefreshButtonsState()
	{
		if (AvailableColumns.SelectedItem != null)
		{
			TVColumn tVColumn = (TVColumn)AvailableColumns.SelectedItem;
			btAdd.IsEnabled = tVColumn.IsLeaf;
		}
		else
		{
			btAdd.IsEnabled = false;
		}
		int count = SelectedColumns.Items.Count;
		if (count == 0)
		{
			btRemove.IsEnabled = false;
		}
		else
		{
			LVColumn lVColumn = (LVColumn)SelectedColumns.SelectedItem;
			if (lVColumn == null)
			{
				btRemove.IsEnabled = false;
			}
			else
			{
				btRemove.IsEnabled = !lVColumn.Column.IsSystem;
			}
		}
		btMoveUp.IsEnabled = SelectedColumns.SelectedIndex > 0;
		btMoveDown.IsEnabled = SelectedColumns.SelectedIndex < count - 1;
	}

	private void SelectedColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		RefreshButtonsState();
	}

	private void ButtonReset_Click(object sender, RoutedEventArgs e)
	{
		DVS.Clear();
		Configure();
		ResetClick?.Invoke();
	}

	private void ButtonMoveUp_Click(object sender, RoutedEventArgs e)
	{
		int selectedIndex = SelectedColumns.SelectedIndex;
		if (selectedIndex > 0)
		{
			LVColumn lVColumn = (LVColumn)SelectedColumns.SelectedItem;
			SelectedColumns.Items.Remove(lVColumn);
			SelectedColumns.Items.Insert(selectedIndex - 1, lVColumn);
			SelectedColumns.SelectedItem = lVColumn;
			SelectedColumns.ScrollIntoView(lVColumn);
		}
	}

	private void ButtonMoveDown_Click(object sender, RoutedEventArgs e)
	{
		int selectedIndex = SelectedColumns.SelectedIndex;
		if (selectedIndex < SelectedColumns.Items.Count - 1)
		{
			LVColumn lVColumn = (LVColumn)SelectedColumns.SelectedItem;
			SelectedColumns.Items.Remove(lVColumn);
			SelectedColumns.Items.Insert(selectedIndex + 1, lVColumn);
			SelectedColumns.SelectedItem = lVColumn;
			SelectedColumns.ScrollIntoView(lVColumn);
		}
	}

	public void Fill(DataViewSetting dvs)
	{
		dvs.Columns.Clear();
		for (int i = 0; i <= SelectedColumns.Items.Count - 1; i++)
		{
			LVColumn lVColumn = (LVColumn)SelectedColumns.Items[i];
			dvs.Columns.Add(lVColumn.Column.Name);
		}
		dvs.FilterString = txtFilter.Text?.Trim();
	}

	public List<string> GetColumns()
	{
		List<string> list = new List<string>();
		for (int i = 0; i <= SelectedColumns.Items.Count - 1; i++)
		{
			LVColumn lVColumn = (LVColumn)SelectedColumns.Items[i];
			list.Add(lVColumn.Column.Name);
		}
		return list;
	}

	private void AvailableColumns_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		RefreshButtonsState();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
