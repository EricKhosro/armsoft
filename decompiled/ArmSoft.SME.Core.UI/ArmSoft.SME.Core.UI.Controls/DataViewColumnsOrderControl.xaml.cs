using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI.DataViews;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Validation;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class DataViewColumnsOrderControl : UserControl, IComponentConnector
{
	private class ColumnDesc
	{
		public string Name { get; set; }

		public string Caption { get; set; }

		public override string ToString()
		{
			return Caption;
		}
	}

	private const int _rowCount = 8;

	private readonly ComboBoxEdit[] _cbColumns;

	private readonly ComboBoxEdit[] _cbColumnsOrder;

	private List<ColumnDesc> _columns;

	private bool _handleSelectedIndexChanged;

	public IDataView IDV { get; set; }

	public DataViewColumnsOrderControl()
	{
		InitializeComponent();
		int count = lcOrderBy.Children.Count;
		_cbColumns = new ComboBoxEdit[8];
		_cbColumnsOrder = new ComboBoxEdit[8];
		for (int i = 0; i <= 7; i++)
		{
			LayoutGroup layoutGroup = new LayoutGroup
			{
				Orientation = Orientation.Horizontal,
				View = LayoutGroupView.Group
			};
			LayoutItem layoutItem = new LayoutItem();
			if (i == 0)
			{
				layoutItem.Label = ArmSoft.SME.Common.Resources.SortBy1;
			}
			else
			{
				layoutItem.Label = ArmSoft.SME.Common.Resources.SortBy2;
			}
			layoutGroup.Children.Add(layoutItem);
			_cbColumns[i] = new ComboBoxEdit();
			layoutItem.Content = _cbColumns[i];
			_cbColumns[i].Name = "CB_Columns" + i;
			_cbColumns[i].Tag = i;
			_cbColumns[i].IsTextEditable = false;
			_cbColumns[i].InvalidValueBehavior = InvalidValueBehavior.AllowLeaveEditor;
			_cbColumns[i].SelectedIndexChanged += new SafeEventHandler<RoutedEventArgs>(ColumnsSelectedIndexChanged).Handler;
			layoutItem = new LayoutItem
			{
				Width = 150.0
			};
			layoutGroup.Children.Add(layoutItem);
			_cbColumnsOrder[i] = new ComboBoxEdit();
			layoutItem.Content = _cbColumnsOrder[i];
			_cbColumnsOrder[i].Name = "CB_ColumnsOrder" + i;
			_cbColumnsOrder[i].Tag = i;
			_cbColumnsOrder[i].Items.Add(ArmSoft.SME.Common.Resources.Ascending);
			_cbColumnsOrder[i].Items.Add(ArmSoft.SME.Common.Resources.Descending);
			_cbColumnsOrder[i].SelectedIndex = 0;
			_cbColumnsOrder[i].IsTextEditable = false;
			lcOrderBy.Children.Insert(lcOrderBy.Children.Count - count, layoutGroup);
		}
		ButtonClearAll.Content = ArmSoft.SME.Common.Resources.ClearWord;
		ButtonReset.Content = ArmSoft.SME.Common.Resources.Reset;
		_handleSelectedIndexChanged = true;
		ButtonReset.Click += new SafeEventHandler<RoutedEventArgs>(ButtonReset_Click).Handler;
		ButtonClearAll.Click += new SafeEventHandler<RoutedEventArgs>(ButtonClearAll_Click).Handler;
	}

	public void Configure(List<string> columns)
	{
		_handleSelectedIndexChanged = false;
		int num = 1;
		List<ColumnDesc> list = new List<ColumnDesc>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = new ColumnDesc
		{
			Name = "",
			Caption = "(" + ArmSoft.SME.Common.Resources.NotSelected + ")"
		};
		_columns = list;
		foreach (string column in columns)
		{
			foreach (ViewColumn potentiallyColumn in IDV.PotentiallyColumns)
			{
				if (column == potentiallyColumn.Name)
				{
					_columns.Add(new ColumnDesc
					{
						Name = potentiallyColumn.Name,
						Caption = potentiallyColumn.Caption
					});
				}
			}
		}
		for (int i = 0; i <= 7; i++)
		{
			_cbColumns[i].ItemsSource = _columns;
		}
		_handleSelectedIndexChanged = true;
		ButtonClearAll.IsEnabled = IDV.AllowChangeSorting;
		ButtonReset.IsEnabled = IDV.AllowChangeSorting;
	}

	public void ReConfigure(List<string> columns)
	{
		List<ColumnOrder> order = GetOrder();
		Configure(columns);
		Configure(order);
	}

	public void Configure(List<ColumnOrder> sortOrder)
	{
		_handleSelectedIndexChanged = false;
		int num = 0;
		foreach (ColumnOrder item in sortOrder)
		{
			int num2 = ColumnIndex(item.Name);
			if (num2 >= 0)
			{
				_cbColumns[num].SelectedIndex = num2;
				_cbColumnsOrder[num].SelectedIndex = (item.Descending ? 1 : 0);
				_cbColumns[num].IsEnabled = IDV.AllowChangeSorting;
				_cbColumnsOrder[num].IsEnabled = IDV.AllowChangeSorting;
				num++;
			}
		}
		for (int i = num; i <= 7; i++)
		{
			_cbColumns[i].SelectedIndex = 0;
			_cbColumnsOrder[i].SelectedIndex = 0;
			_cbColumns[i].IsEnabled = num == i && IDV.AllowChangeSorting;
			_cbColumnsOrder[i].IsEnabled = num == i && IDV.AllowChangeSorting;
		}
		_handleSelectedIndexChanged = true;
	}

	private void ButtonReset_Click(object sender, RoutedEventArgs e)
	{
		Reset();
	}

	public void Reset()
	{
		_handleSelectedIndexChanged = false;
		int num = 0;
		foreach (ViewColumnSorting item in IDV.Columns.SortOrder())
		{
			int num2 = ColumnIndex(item.Column.Name);
			if (num2 >= 0)
			{
				_cbColumns[num].SelectedIndex = num2;
				_cbColumnsOrder[num].SelectedIndex = (item.Descending ? 1 : 0);
				_cbColumns[num].IsEnabled = IDV.AllowChangeSorting;
				_cbColumnsOrder[num].IsEnabled = IDV.AllowChangeSorting;
				num++;
			}
		}
		for (int i = num; i <= 7; i++)
		{
			_cbColumns[i].SelectedIndex = 0;
			_cbColumnsOrder[i].SelectedIndex = 0;
			_cbColumns[i].IsEnabled = num == i && IDV.AllowChangeSorting;
			_cbColumnsOrder[i].IsEnabled = num == i && IDV.AllowChangeSorting;
		}
		_handleSelectedIndexChanged = true;
	}

	public List<ColumnOrder> GetOrder()
	{
		List<ColumnOrder> list = new List<ColumnOrder>();
		for (int i = 0; i <= 7; i++)
		{
			if (_cbColumns[i].SelectedIndex <= 0)
			{
				continue;
			}
			string name = _columns[_cbColumns[i].SelectedIndex].Name;
			bool flag = false;
			foreach (ColumnOrder item2 in list)
			{
				if (item2.Name == name)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				ColumnOrder item = new ColumnOrder
				{
					Name = name,
					Descending = (_cbColumnsOrder[i].SelectedIndex == 1)
				};
				list.Add(item);
			}
		}
		return list;
	}

	private int ColumnIndex(string name)
	{
		for (int i = 0; i <= _columns.Count - 1; i++)
		{
			if (_columns[i].Name == name)
			{
				return i;
			}
		}
		return -1;
	}

	private void ButtonClearAll_Click(object sender, RoutedEventArgs e)
	{
		Configure(new List<ColumnOrder>());
	}

	private void ColumnsSelectedIndexChanged(object sender, RoutedEventArgs e)
	{
		if (!_handleSelectedIndexChanged)
		{
			return;
		}
		int num = Convert.ToInt32(((ComboBoxEdit)e.Source).Tag);
		_handleSelectedIndexChanged = false;
		if (_cbColumns[num].SelectedIndex == 0)
		{
			_cbColumns[num].IsEnabled = IDV.AllowChangeSorting;
			_cbColumnsOrder[num].IsEnabled = IDV.AllowChangeSorting;
			for (int i = num + 1; i <= 7; i++)
			{
				_cbColumns[i].SelectedIndex = 0;
				_cbColumnsOrder[i].SelectedIndex = 0;
				_cbColumns[i].IsEnabled = false;
				_cbColumnsOrder[i].IsEnabled = false;
			}
		}
		else if (num < 7 && !_cbColumns[num + 1].IsEnabled)
		{
			_cbColumns[num + 1].IsEnabled = IDV.AllowChangeSorting;
			_cbColumnsOrder[num + 1].IsEnabled = IDV.AllowChangeSorting;
		}
		_handleSelectedIndexChanged = true;
	}
}
