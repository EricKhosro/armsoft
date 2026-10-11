using System;
using System.Collections.Generic;
using System.Linq;
using ArmSoft.AS8X.Common.Language.Extensions;
using Armsoft.PrintTemplates2.Helper;

namespace ArmSoft.PrintTemplates;

public abstract class GridRow<T> : IGridRow
{
	public object[] Row { get; private set; }

	internal string[] FormattedElements { get; private set; }

	protected Dictionary<string, GridColumn> Columns { get; set; }

	public T this[string columnName]
	{
		get
		{
			return Row[Columns[columnName].Index].ToGeneric<T>();
		}
		set
		{
			Row[Columns[columnName].Index] = value;
			SetFormatted(Columns[columnName].Index, value);
		}
	}

	public T this[int index]
	{
		get
		{
			return Row[index].ToGeneric<T>();
		}
		set
		{
			Row[index] = value;
			SetFormatted(index, value);
		}
	}

	object IGridRow.this[string columnName]
	{
		get
		{
			return Row[Columns[columnName].Index];
		}
		set
		{
			Row[Columns[columnName].Index] = value;
			SetFormatted(Columns[columnName].Index, value);
		}
	}

	object IGridRow.this[int index]
	{
		get
		{
			return Row[index];
		}
		set
		{
			Row[index] = value;
			SetFormatted(index, value);
		}
	}

	public GridRow(Dictionary<string, GridColumn> columns, int columnMaxCount)
	{
		Row = new object[columnMaxCount];
		FormattedElements = new string[columnMaxCount];
		for (int i = 0; i < columnMaxCount; i++)
		{
			Row[i] = null;
			FormattedElements[i] = string.Empty;
		}
		Columns = columns;
	}

	string IGridRow.GetFormattedValue(GridColumn gridColumn)
	{
		return FormattedElements[gridColumn.Index];
	}

	void IGridRow.ConvertToArmenianUnicode()
	{
		for (int i = 0; i < Row.Length; i++)
		{
			if (Row[i] is string value)
			{
				Row[i] = value.ToArmenianUnicode();
			}
			if (!string.IsNullOrWhiteSpace(FormattedElements[i]))
			{
				FormattedElements[i] = FormattedElements[i].ToArmenianUnicode();
			}
		}
	}

	private void SetFormatted(int columnIndex, object value)
	{
		GridColumn value2 = Columns.ElementAt(columnIndex).Value;
		Func<object, string> formatter = value2.Formatter;
		if (formatter != null)
		{
			FormattedElements[value2.Index] = formatter(value);
		}
	}
}
public class GridRow : GridRow<string>
{
	public GridRow(Dictionary<string, GridColumn> columns, int columnMaxCount)
		: base(columns, columnMaxCount)
	{
	}
}
