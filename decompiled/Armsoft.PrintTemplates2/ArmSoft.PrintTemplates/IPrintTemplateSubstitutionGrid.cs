using System;
using System.Collections.Generic;
using Spread.SheetCellStyle;

namespace ArmSoft.PrintTemplates;

public interface IPrintTemplateSubstitutionGrid
{
	object this[int index] { get; }

	string Name { get; }

	int RowCount { get; }

	bool UseStyles { get; }

	internal Dictionary<string, GridColumn> Columns { get; }

	void AddColumn(string name, string caption, Func<object, string> formtter = null);

	void AddRow();

	SpreadSheetCellStyle GetStyle(int row, string column);

	object GetValue(int row, string column);

	object GetValueConsideringFormatter(int row, string column);

	internal void ConvertToArmenianUnicode();

	internal List<GridColumn> GetSortedByLengthColums();

	void SetValue<R>(int row, string column, R value);
}
