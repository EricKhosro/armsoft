using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ArmSoft.AS8X.Common.Language.Extensions;
using ArmSoft.PrintTemplates.Text;
using Armsoft.PrintTemplates2.Helper;
using Armsoft.PrintTemplates2.Resources;
using Microsoft.VisualBasic;
using Spread.SheetCellStyle;

namespace ArmSoft.PrintTemplates;

public abstract class PrintTemplateSubstitutionGrid<T> : IEnumerable<T>, IEnumerable, IPrintTemplateSubstitutionGrid where T : IGridRow
{
	private const int colMaxCount = 100;

	public T this[int index] => Rows[index];

	public string Name { get; }

	private int LastIndex { get; set; }

	internal Dictionary<string, GridColumn> Columns { get; private set; } = new Dictionary<string, GridColumn>(StringComparer.OrdinalIgnoreCase);

	protected List<T> Rows { get; set; } = new List<T>();

	private List<SpreadSheetCellStyle[]> Styles { get; set; } = new List<SpreadSheetCellStyle[]>();

	public int RowCount
	{
		get
		{
			if (Rows == null)
			{
				return 0;
			}
			return Rows.Count;
		}
	}

	public bool UseStyles { get; set; }

	object IPrintTemplateSubstitutionGrid.this[int index] => this[index];

	Dictionary<string, GridColumn> IPrintTemplateSubstitutionGrid.Columns => Columns;

	public PrintTemplateSubstitutionGrid(string name)
	{
		Name = name;
		LastIndex = -1;
	}

	public ReadOnlyDictionary<string, GridColumn> GetColumns()
	{
		return new ReadOnlyDictionary<string, GridColumn>(Columns);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected R InnerGetValue<R>(int row, string column)
	{
		if (Columns.ContainsKey(column))
		{
			return Rows[row][column].ToGeneric<R>();
		}
		throw new InvalidOperationException(string.Format(Resource.InvalidColumn, Name, column));
	}

	public void SetValue<R>(int row, string column, R value)
	{
		CheckColumn(column);
		T val = Rows[row];
		val[column] = value;
	}

	public SpreadSheetCellStyle GetStyle(int row, string column)
	{
		if (Columns.ContainsKey(column))
		{
			return Styles[row][Columns[column].Index];
		}
		throw new InvalidOperationException(string.Format(Resource.InvalidColumn, Name, column));
	}

	public void SetStyle(int row, string column, SpreadSheetCellStyle value)
	{
		if (Columns.ContainsKey(column))
		{
			Styles[row][Columns[column].Index] = value;
			return;
		}
		throw new InvalidOperationException(string.Format(Resource.InvalidColumn, Name, column));
	}

	public void SetStyle(int row, int colIndex, SpreadSheetCellStyle value)
	{
		if (colIndex <= LastIndex)
		{
			Styles[row][colIndex] = value;
			return;
		}
		throw new InvalidOperationException(string.Format(Resource.InvalidColumn, Name, colIndex));
	}

	public void SetRowStyle(int row, SpreadSheetCellStyle styleValue)
	{
		int i = Information.LBound(Styles[row]);
		for (int num = Information.UBound(Styles[row]); i <= num; i++)
		{
			Styles[row][i] = styleValue;
		}
	}

	public void SetColumnStyle(string column, SpreadSheetCellStyle styleValue)
	{
		if (Columns.ContainsKey(column))
		{
			int index = Columns[column].Index;
			int i = 0;
			for (int num = Styles.Count - 1; i <= num; i++)
			{
				Styles[i][index] = styleValue;
			}
			return;
		}
		throw new InvalidOperationException(string.Format(Resource.InvalidColumn, Name, column));
	}

	public void AddColumn(string name, string caption, Func<object, string> formtter = null)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new InvalidOperationException(Resource.ColumnNameEmpty);
		}
		if (Columns.ContainsKey(name))
		{
			throw new InvalidOperationException(string.Format(Resource.ColumnExists, name));
		}
		if (LastIndex > 100)
		{
			throw new InvalidOperationException(string.Format(Resource.AdditionalColumnExceed, 100));
		}
		LastIndex++;
		Columns.Add(name, new GridColumn(name, caption, LastIndex, formtter));
	}

	public void AddRow()
	{
		T item = (T)Activator.CreateInstance(typeof(T), Columns, 100);
		Rows.Add(item);
		if (UseStyles)
		{
			SpreadSheetCellStyle[] item2 = new SpreadSheetCellStyle[100];
			Styles.Add(item2);
		}
	}

	public void RemoveRow(int index)
	{
		Rows.RemoveAt(index);
		if (UseStyles)
		{
			Styles.RemoveAt(index);
		}
	}

	private void CheckColumn(string column)
	{
		if (!Columns.ContainsKey(column))
		{
			throw new InvalidOperationException(string.Format(Resource.InvalidColumn, Name, column));
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		return Rows.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return Rows.GetEnumerator();
	}

	public void SetOrderedRows(IOrderedEnumerable<T> gridRows)
	{
		Rows = gridRows.ToList();
	}

	object IPrintTemplateSubstitutionGrid.GetValue(int row, string column)
	{
		return InnerGetValue<object>(row, column);
	}

	void IPrintTemplateSubstitutionGrid.ConvertToArmenianUnicode()
	{
		foreach (KeyValuePair<string, GridColumn> column in Columns)
		{
			column.Value.Caption = column.Value.Caption.ToArmenianUnicode();
		}
		for (int i = 0; i < Rows.Count; i++)
		{
			Rows[i].ConvertToArmenianUnicode();
		}
	}

	List<GridColumn> IPrintTemplateSubstitutionGrid.GetSortedByLengthColums()
	{
		List<GridColumn> list = new List<GridColumn>();
		if (Columns.Count > 0)
		{
			GridColumn[] array = new GridColumn[Columns.Count];
			Columns.Values.CopyTo(array, 0);
			Array.Sort(array);
			list.AddRange(array);
		}
		return list;
	}

	object IPrintTemplateSubstitutionGrid.GetValueConsideringFormatter(int row, string column)
	{
		if (Columns[column].Formatter != null)
		{
			return this[row].GetFormattedValue(Columns[column]);
		}
		return InnerGetValue<object>(row, column);
	}
}
public sealed class PrintTemplateSubstitutionGrid : PrintTemplateSubstitutionGrid<GridRow>
{
	public PrintTemplateSubstitutionGrid(string name)
		: base(name)
	{
	}

	public string GetValue(int row, string column)
	{
		return InnerGetValue<string>(row, column);
	}

	public void SetValue(int row, string column, string value)
	{
		base.SetValue(row, column, StringConverter.RemoveXMLRestrictedCharacters(value));
	}
}
