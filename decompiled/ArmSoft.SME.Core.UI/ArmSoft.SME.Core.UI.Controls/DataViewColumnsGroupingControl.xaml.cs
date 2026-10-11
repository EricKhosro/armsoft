using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Validation;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class DataViewColumnsGroupingControl : UserControl, IComponentConnector
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

	private const int _maxGroups = 2;

	private const int _maxColumnsPerGroup = 3;

	private readonly ComboBoxEdit[,] _cbColumns = new ComboBoxEdit[2, 3];

	private List<ColumnDesc> _columns;

	private bool _handleSelectedIndexChanged;

	public IDataView IDV { get; set; }

	public DataViewColumnsGroupingControl()
	{
		InitializeComponent();
		int count = lcOrderBy.Children.Count;
		for (int i = 0; i <= 1; i++)
		{
			LayoutGroup layoutGroup = new LayoutGroup
			{
				Orientation = Orientation.Vertical,
				Margin = new Thickness(-5.0, (i != 0) ? 5 : 0, -5.0, 0.0),
				View = LayoutGroupView.GroupBox,
				Header = ArmSoft.SME.Common.Resources.GroupLevel + " " + (i + 1)
			};
			for (int j = 0; j <= 2; j++)
			{
				LayoutItem layoutItem = new LayoutItem();
				if (j == 0)
				{
					layoutItem.Label = ArmSoft.SME.Common.Resources.GroupBy1;
				}
				else
				{
					layoutItem.Label = ArmSoft.SME.Common.Resources.GroupBy2;
				}
				layoutGroup.Children.Add(layoutItem);
				_cbColumns[i, j] = new ComboBoxEdit();
				layoutItem.Content = _cbColumns[i, j];
				_cbColumns[i, j].Name = "CB_Columns" + i + j;
				_cbColumns[i, j].Tag = i + " " + j;
				_cbColumns[i, j].IsTextEditable = false;
				_cbColumns[i, j].InvalidValueBehavior = InvalidValueBehavior.AllowLeaveEditor;
				_cbColumns[i, j].SelectedIndexChanged += new SafeEventHandler<RoutedEventArgs>(ColumnsSelectedIndexChanged).Handler;
			}
			lcOrderBy.Children.Insert(lcOrderBy.Children.Count - count, layoutGroup);
		}
		ButtonClearAll.Content = ArmSoft.SME.Common.Resources.ClearWord;
		ButtonReset.Content = ArmSoft.SME.Common.Resources.Reset;
		_handleSelectedIndexChanged = true;
		ButtonClearAll.Click += new SafeEventHandler<RoutedEventArgs>(ButtonClearAll_Click).Handler;
		ButtonReset.Click += new SafeEventHandler<RoutedEventArgs>(ButtonReset_Click).Handler;
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
						Name = potentiallyColumn.Name.ToString(),
						Caption = potentiallyColumn.Caption.ToString()
					});
				}
			}
		}
		for (int i = 0; i <= 1; i++)
		{
			for (int j = 0; j <= 2; j++)
			{
				_cbColumns[i, j].ItemsSource = _columns;
			}
		}
		_handleSelectedIndexChanged = true;
		ButtonClearAll.IsEnabled = IDV.AllowChangeGrouping;
		ButtonReset.IsEnabled = IDV.AllowChangeGrouping;
	}

	public void ReConfigure(List<string> columns)
	{
		List<List<string>> grouping = GetGrouping();
		Configure(columns);
		Configure(grouping);
	}

	public void Configure(List<List<string>> groupBy)
	{
		_handleSelectedIndexChanged = false;
		int num = 0;
		foreach (List<string> item in groupBy)
		{
			int num2 = 0;
			foreach (string item2 in item)
			{
				int num3 = ColumnIndex(item2);
				if (num3 >= 0)
				{
					_cbColumns[num, num2].SelectedIndex = num3;
					_cbColumns[num, num2].IsEnabled = IDV.AllowChangeGrouping;
					num2++;
				}
			}
			for (int i = num2; i <= 2; i++)
			{
				_cbColumns[num, i].SelectedIndex = 0;
				_cbColumns[num, i].IsEnabled = num2 == i && IDV.AllowChangeGrouping;
			}
			num++;
		}
		for (int j = num; j <= 1; j++)
		{
			for (int k = 0; k <= 2; k++)
			{
				_cbColumns[j, k].SelectedIndex = 0;
				_cbColumns[j, k].IsEnabled = j == num && k == 0 && IDV.AllowChangeGrouping;
			}
		}
		_handleSelectedIndexChanged = true;
	}

	public void Reset()
	{
		_handleSelectedIndexChanged = false;
		int num = 0;
		foreach (List<ViewColumn> item in IDV.Columns.GroupBy())
		{
			int num2 = 0;
			foreach (ViewColumn item2 in (IEnumerable)item)
			{
				int num3 = ColumnIndex(item2.Name.ToString());
				if (num3 >= 0)
				{
					_cbColumns[num, num2].SelectedIndex = num3;
					_cbColumns[num, num2].IsEnabled = IDV.AllowChangeGrouping;
					num2++;
				}
			}
			for (int i = num2; i <= 2; i++)
			{
				_cbColumns[num, i].SelectedIndex = 0;
				_cbColumns[num, i].IsEnabled = num2 == i && IDV.AllowChangeGrouping;
			}
			num++;
		}
		for (int j = num; j <= 1; j++)
		{
			for (int k = 0; k <= 2; k++)
			{
				_cbColumns[j, k].SelectedIndex = 0;
				_cbColumns[j, k].IsEnabled = j == num && k == 0 && IDV.AllowChangeGrouping;
			}
		}
		_handleSelectedIndexChanged = true;
	}

	public List<List<string>> GetGrouping()
	{
		List<List<string>> list = new List<List<string>>();
		for (int i = 0; i <= 1; i++)
		{
			List<string> list2 = new List<string>();
			for (int j = 0; j <= 2; j++)
			{
				if (_cbColumns[i, j].SelectedIndex <= 0)
				{
					continue;
				}
				string name = _columns[_cbColumns[i, j].SelectedIndex].Name;
				bool flag = false;
				foreach (List<string> item in list)
				{
					foreach (string item2 in item)
					{
						if (item2 == name)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					foreach (string item3 in list2)
					{
						if (item3 == name)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					list2.Add(name);
				}
			}
			if (list2.Count > 0)
			{
				list.Add(list2);
			}
		}
		return list;
	}

	private void ButtonClearAll_Click(object sender, RoutedEventArgs e)
	{
		Configure(new List<List<string>>());
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

	private void ButtonReset_Click(object sender, RoutedEventArgs e)
	{
		Reset();
	}

	private void ColumnsSelectedIndexChanged(object sender, RoutedEventArgs e)
	{
		if (!_handleSelectedIndexChanged)
		{
			return;
		}
		string[] array = ((ComboBoxEdit)e.Source).Tag.ToString().Split(' ');
		int num = int.Parse(array[0]);
		int num2 = int.Parse(array[1]);
		_handleSelectedIndexChanged = false;
		if (_cbColumns[num, num2].SelectedIndex == 0)
		{
			_cbColumns[num, num2].IsEnabled = IDV.AllowChangeGrouping;
			for (int i = num2 + 1; i <= 2; i++)
			{
				_cbColumns[num, i].SelectedIndex = 0;
				_cbColumns[num, i].IsEnabled = false;
			}
			if (num2 == 0)
			{
				for (int j = num + 1; j <= 1; j++)
				{
					for (int k = 0; k <= 2; k++)
					{
						_cbColumns[j, k].SelectedIndex = 0;
						_cbColumns[j, k].IsEnabled = false;
					}
				}
			}
		}
		else
		{
			if (num2 < 2 && !_cbColumns[num, num2 + 1].IsEnabled)
			{
				_cbColumns[num, num2 + 1].IsEnabled = IDV.AllowChangeGrouping;
			}
			if (num < 1 && !_cbColumns[num + 1, 0].IsEnabled)
			{
				_cbColumns[num + 1, 0].IsEnabled = IDV.AllowChangeGrouping;
			}
		}
		_handleSelectedIndexChanged = true;
	}
}
