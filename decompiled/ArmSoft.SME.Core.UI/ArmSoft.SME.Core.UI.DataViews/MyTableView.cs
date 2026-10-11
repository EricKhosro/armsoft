using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.DataViews;

public class MyTableView : TableView, IPanelView
{
	public DataViewPanel DVP { get; set; }

	public IDataView IDV { get; set; }

	public bool ShowFilterRow
	{
		get
		{
			return ShowAutoFilterRow;
		}
		set
		{
			ShowAutoFilterRow = value;
			if (!value)
			{
				return;
			}
			GridColumn col = null;
			if (VisibleColumns.Count > 1)
			{
				col = VisibleColumns[1];
			}
			for (int i = 0; i <= VisibleColumns.Count - 1; i++)
			{
				if (VisibleColumns[i].SortIndex == 0)
				{
					col = VisibleColumns[i];
					break;
				}
			}
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
			{
				FocusedRowHandle = -2147483645;
				Grid.CurrentColumn = col;
				ShowEditor();
			}), Array.Empty<object>());
		}
	}

	public int GroupingLevel
	{
		get
		{
			if (Grid.GroupCount == 0)
			{
				return 0;
			}
			int num = 1;
			foreach (GridColumn column in Grid.Columns)
			{
				if (column.IsGrouped && column.GroupLevel + 1 > num)
				{
					num = Convert.ToInt32(column.GroupLevel + 1);
				}
			}
			return num;
		}
	}

	public MyTableView()
	{
		ShowFilterPopup += new SafeEventHandler<FilterPopupEventArgs>(MyTableView_ShowFilterPopup).Handler;
	}

	public void ExpandGroups()
	{
		Grid.ExpandAllGroups();
	}

	public void ExpandGroupLevel(int level)
	{
		Grid.CollapseAllGroups();
		List<int> list = new List<int>();
		Grid.BeginDataUpdate();
		for (int i = 0; i <= level - 1; i++)
		{
			for (int j = 0; j <= Grid.VisibleRowCount - 1; j++)
			{
				int rowHandleByVisibleIndex = Grid.GetRowHandleByVisibleIndex(j);
				if (Grid.GetRowLevelByRowHandle(rowHandleByVisibleIndex) == i && Grid.IsGroupRowHandle(rowHandleByVisibleIndex))
				{
					list.Add(rowHandleByVisibleIndex);
				}
			}
			foreach (int item in list)
			{
				Grid.ExpandGroupRow(item);
			}
		}
		Grid.EndDataUpdate();
		Grid.RefreshData();
	}

	public void CollapseGroups()
	{
		Grid.CollapseAllGroups();
	}

	public object[] MyGetGroupRowDisplayText(int rowHandle)
	{
		object[] groupDisplayValues = GetGroupDisplayValues(rowHandle);
		if (groupDisplayValues != null)
		{
			return groupDisplayValues;
		}
		return new object[1] { GetGroupDisplayValue(rowHandle) };
	}

	public string[] MyGetGroupRowCaptions(int rowHandle)
	{
		string[] groupRowHeadersCaptions = GetGroupRowHeadersCaptions(rowHandle);
		if (groupRowHeadersCaptions != null)
		{
			return groupRowHeadersCaptions;
		}
		return new string[1] { GetGroupRowHeaderCaption(rowHandle) };
	}

	private void MyTableView_ShowFilterPopup(object sender, FilterPopupEventArgs e)
	{
		if (e.Column.Name == "EntryExists" || e.ComboBoxEdit == null)
		{
			return;
		}
		Grid.ImplyNullLikeEmptyStringWhenFiltering = true;
		BaseEditSettings editSettings = e.Column.EditSettings;
		if (editSettings != null && (editSettings.DisplayFormat == "{0:dd/MM/yy H:mm:ss}" || editSettings.DisplayFormat == "{0:dd/MM/yyyy}" || editSettings.DisplayFormat == "{0:dd/MM/yy}" || editSettings.DisplayFormat == "{0:dd/MM/yy HH:mm}"))
		{
			return;
		}
		List<object> list = new List<object>();
		Dictionary<object, object> uniqueColumnValues = GetUniqueColumnValues(e.Column);
		if (e.Column.IsFiltered)
		{
			e.Handled = false;
			return;
		}
		foreach (CustomComboBoxItem item in ((List<object>)e.ComboBoxEdit.ItemsSource).Cast<CustomComboBoxItem>())
		{
			if (item.EditValue != null && uniqueColumnValues.ContainsKey(item.EditValue))
			{
				list.Add(item);
			}
		}
		if (uniqueColumnValues.ContainsKey("BlankValue"))
		{
			list.Insert(0, "");
		}
		e.ComboBoxEdit.ItemsSource = list;
		e.Handled = true;
	}

	private Dictionary<object, object> GetUniqueColumnValues(ColumnBase columnBase)
	{
		Dictionary<object, object> dictionary = new Dictionary<object, object>();
		for (int i = 0; i <= Grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = Grid.GetRowHandleByVisibleIndex(i);
			if (rowHandleByVisibleIndex >= 0)
			{
				object cellValue = Grid.GetCellValue(rowHandleByVisibleIndex, columnBase.Name);
				if (cellValue != null && !dictionary.ContainsKey(cellValue))
				{
					dictionary.Add(cellValue, cellValue);
				}
				else if (cellValue == null && !dictionary.ContainsKey("BlankValue"))
				{
					dictionary.Add("BlankValue", null);
				}
			}
		}
		return dictionary;
	}

	public IEnumerable<ColumnBase> GetVisibleColumns()
	{
		return VisibleColumns;
	}

	public void FitColumn(GridColumn col)
	{
		BestFitColumn(col);
	}

	void IPanelView.BestFitColumn(GridColumn col)
	{
		FitColumn(col);
	}
}
