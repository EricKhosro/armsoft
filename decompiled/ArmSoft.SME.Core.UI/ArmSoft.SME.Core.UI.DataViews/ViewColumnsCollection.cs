using System;
using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Data;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Core.UI.DataViews;

public class ViewColumnsCollection
{
	private readonly Dictionary<string, ViewColumn> _columnsDictionary;

	private readonly List<ViewColumn> _columnsList;

	private readonly List<ViewColumnSorting> _columnsSortOrder;

	private readonly List<List<ViewColumn>> _columnsGroupBy;

	private List<DirectoryLinkColumnData> _directoryLinkColumns;

	public ViewColumn this[string name] => _columnsDictionary[name];

	public List<ViewColumn> ViewList => _columnsList;

	public IEnumerable<ViewColumn> ReferenceColumns => _columnsList.Where((ViewColumn p) => p.IsReference);

	public IEnumerable<ViewColumn> ExtendedColumns => _columnsList.Where((ViewColumn p) => p.IsExtended);

	public List<DirectoryLinkColumnData> DirectoryLinkColumns => _directoryLinkColumns;

	public ViewColumnsCollection()
	{
		_columnsDictionary = new Dictionary<string, ViewColumn>();
		_columnsList = new List<ViewColumn>();
		_columnsSortOrder = new List<ViewColumnSorting>();
		_columnsGroupBy = new List<List<ViewColumn>>();
	}

	public ViewColumn Add(string name)
	{
		return Add(name, "", name);
	}

	public ViewColumn Add(string name, string caption)
	{
		return Add(name, caption, name);
	}

	public ViewColumn Add(string name, string caption, string source)
	{
		ViewColumn viewColumn = new ViewColumn(name, caption, source);
		Add(viewColumn);
		return viewColumn;
	}

	public ViewColumn Add(Column item)
	{
		ViewColumn viewColumn = new ViewColumn(item.Name, item.Caption, item.Source)
		{
			Format = item.Format,
			Type = item.Type.CastToDataColumnType(),
			IsDynamic = (item.SourceType == ColumnSourceType.Dynamic),
			FooterSum = item.FooterSum,
			Before = item.Before,
			After = item.After
		};
		Add(viewColumn);
		return viewColumn;
	}

	public void Add(ViewColumn item)
	{
		if (_columnsDictionary.ContainsKey(item.Name))
		{
			throw new InvalidOperationException("Column " + item.Name + " already exists.");
		}
		_columnsDictionary.Add(item.Name, item);
		_columnsList.Add(item);
	}

	public ViewColumn AddDynamic(string name, string caption, string source, DataColumnType type, bool footerSum = false, string after = "", string before = "", bool isExtended = false, string format = null)
	{
		ViewColumn viewColumn = new ViewColumn(name, caption, source)
		{
			IsDynamic = true,
			Type = type,
			FooterSum = footerSum,
			Before = before,
			After = after,
			IsExtended = isExtended,
			Format = format
		};
		Add(viewColumn);
		return viewColumn;
	}

	public void Insert(int index, ViewColumn item)
	{
		if (_columnsDictionary.ContainsKey(item.Name))
		{
			throw new InvalidOperationException("Column " + item.Name + " already exists.");
		}
		_columnsList.Insert(index, item);
		_columnsDictionary.Add(item.Name, item);
	}

	public void RemoveAllDynamicColumns()
	{
		for (int num = _columnsList.Count - 1; num >= 0; num--)
		{
			if (_columnsList[num].IsDynamic)
			{
				Remove(_columnsList[num].Name);
			}
		}
	}

	public void Remove(string name)
	{
		int num = ViewListIndex(name);
		if (num == -1)
		{
			throw new ASException(string.Format(Resources.InvalidColumnName, name));
		}
		ViewColumn viewColumn = _columnsList[num];
		_columnsList.Remove(viewColumn);
		_columnsDictionary.Remove(name);
		ViewColumnSorting viewColumnSorting = null;
		foreach (ViewColumnSorting item in _columnsSortOrder)
		{
			if (item.Column == viewColumn)
			{
				viewColumnSorting = item;
				break;
			}
		}
		if (viewColumnSorting != null)
		{
			_columnsSortOrder.Remove(viewColumnSorting);
		}
		bool flag = false;
		for (int i = 0; i <= _columnsGroupBy.Count - 1; i++)
		{
			for (int j = 0; j <= _columnsGroupBy[i].Count - 1; j++)
			{
				if (_columnsGroupBy[i][j] == viewColumn)
				{
					flag = true;
					_columnsGroupBy[i].RemoveAt(j);
					if (_columnsGroupBy[i].Count == 0)
					{
						_columnsGroupBy.RemoveAt(i);
					}
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
	}

	public void Move(string name1, string name2, bool after)
	{
		if (name1 == name2)
		{
			throw new InvalidOperationException("Can't move the same column");
		}
		int num = ViewListIndex(name1);
		if (num == -1)
		{
			throw new ASException(string.Format(Resources.InvalidColumnName, name1));
		}
		int num2 = ViewListIndex(name2);
		if (num2 != -1)
		{
			ViewColumn item = _columnsList[num];
			_columnsList.Remove(item);
			if (num > num2)
			{
				_columnsList.Insert(after ? (num2 + 1) : num2, item);
			}
			else
			{
				_columnsList.Insert(after ? num2 : (num2 - 1), item);
			}
		}
	}

	public void Move(string name, int index)
	{
		int num = ViewListIndex(name);
		if (num == -1)
		{
			throw new ASException(string.Format(Resources.InvalidColumnName, name));
		}
		if (num != index)
		{
			ViewColumn item = _columnsList[num];
			_columnsList.Remove(item);
			if (num <= index)
			{
				_columnsList.Insert(index - 1, item);
			}
			else
			{
				_columnsList.Insert(index, item);
			}
		}
	}

	private int ViewListIndex(string name)
	{
		for (int i = 0; i <= _columnsList.Count - 1; i++)
		{
			if (_columnsList[i].Name == name)
			{
				return i;
			}
		}
		return -1;
	}

	public bool Contains(string name)
	{
		return _columnsDictionary.ContainsKey(name);
	}

	public void AddSorting(string name, bool descending = false)
	{
		int num = ViewListIndex(name);
		if (num == -1)
		{
			throw new ASException(string.Format(Resources.InvalidColumnName, name));
		}
		ViewColumn viewColumn = _columnsList[num];
		foreach (ViewColumnSorting item in _columnsSortOrder)
		{
			if (item.Column == viewColumn)
			{
				throw new InvalidOperationException("The column '" + name + "' is already sorted.");
			}
		}
		_columnsSortOrder.Add(new ViewColumnSorting
		{
			Column = viewColumn,
			Descending = descending
		});
	}

	public List<ViewColumnSorting> SortOrder()
	{
		return _columnsSortOrder;
	}

	public void ClearSorting()
	{
		_columnsSortOrder.Clear();
	}

	public void AddDirectoryLinkColumn(short directoryType, string codeColumnName, string captionColumnName = "", string directoryCaptionColumnName = "", string directoryCaption = "", bool showAsLink = true, bool showInContextMenu = true)
	{
		if (_directoryLinkColumns == null)
		{
			_directoryLinkColumns = new List<DirectoryLinkColumnData>();
		}
		_directoryLinkColumns.Add(new DirectoryLinkColumnData(codeColumnName, captionColumnName, directoryType, directoryCaptionColumnName, directoryCaption, showAsLink, showInContextMenu));
	}

	public void ClearDirectoryLinkColumns()
	{
		if (_directoryLinkColumns != null)
		{
			_directoryLinkColumns.Clear();
		}
	}

	public void AddGroup()
	{
		_columnsGroupBy.Add(new List<ViewColumn>());
	}

	public void AddToLastGroup(string name)
	{
		int num = ViewListIndex(name);
		if (num == -1)
		{
			throw new ASException(string.Format(Resources.InvalidColumnName, name));
		}
		ViewColumn viewColumn = _columnsList[num];
		for (int i = 0; i <= _columnsGroupBy.Count - 1; i++)
		{
			for (int j = 0; j <= _columnsGroupBy[i].Count - 1; j++)
			{
				if (_columnsGroupBy[i][j] == viewColumn)
				{
					throw new InvalidOperationException("The column '" + name + "' is already grouped.");
				}
			}
		}
		if (_columnsGroupBy.Count == 0)
		{
			AddGroup();
		}
		List<List<ViewColumn>> columnsGroupBy = _columnsGroupBy;
		columnsGroupBy[columnsGroupBy.Count - 1].Add(viewColumn);
	}

	public List<List<ViewColumn>> GroupBy()
	{
		return _columnsGroupBy;
	}

	public void ClearGroupBy()
	{
		_columnsGroupBy.Clear();
	}

	public void Clear()
	{
		_columnsDictionary.Clear();
		_columnsList.Clear();
		_columnsSortOrder.Clear();
		_columnsGroupBy.Clear();
	}

	public ViewColumnsCollection Clone()
	{
		ViewColumnsCollection viewColumnsCollection = new ViewColumnsCollection();
		foreach (ViewColumn columns in _columnsList)
		{
			viewColumnsCollection.Add(columns);
		}
		return viewColumnsCollection;
	}
}
