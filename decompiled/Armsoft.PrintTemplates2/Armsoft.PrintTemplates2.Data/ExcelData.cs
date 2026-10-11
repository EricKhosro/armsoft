using System.Collections.Generic;
using System.Data;

namespace Armsoft.PrintTemplates2.Data;

public class ExcelData
{
	public ExcelSettings Settings { get; } = new ExcelSettings();

	public DataTable Data { get; } = new DataTable();

	public Dictionary<string, DataColumnOptions> ColumnsOptions { get; } = new Dictionary<string, DataColumnOptions>();

	public ExcelData(DataTable dataTable, ExcelSettings excelSettings, Dictionary<string, DataColumnOptions> options)
	{
		Data = dataTable;
		Settings = excelSettings;
		ColumnsOptions = options;
	}
}
