using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using Armsoft.PrintTemplates2.Data;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet.Model;

namespace Armsoft.PrintTemplates2;

public class ExcelDataExporter
{
	public ExcelData ExcelData { get; }

	public event ExportProgressEventHandler ExportProgress;

	public ExcelDataExporter(ExcelData excelExportData)
	{
		ExcelData = excelExportData;
	}

	public void ExportToExcel()
	{
		ExcelSettings settings = ExcelData.Settings;
		if (string.IsNullOrWhiteSpace(settings.File))
		{
			throw new FileNotFoundException();
		}
		byte[] buffer = File.ReadAllBytes(settings.File);
		using Workbook workbook = new Workbook();
		DocumentFormat fileFormat = GetFileFormat(settings.File);
		workbook.LoadDocument(buffer, fileFormat);
		DevExpress.Spreadsheet.Worksheet worksheet = ((!string.IsNullOrWhiteSpace(settings.WorkSheet)) ? workbook.Worksheets[settings.WorkSheet] : workbook.Worksheets.First());
		if (int.TryParse(settings.PositionColumn, out var result) && result > 0)
		{
			result--;
		}
		if (int.TryParse(settings.PositionRow, out var result2) && result2 > 0)
		{
			result2--;
		}
		if (!workbook.Styles.Contains("BoldStyle"))
		{
			workbook.Styles.Add("BoldStyle");
		}
		workbook.Styles["BoldStyle"].Font.Bold = true;
		int count = ExcelData.Data.Columns.Count;
		int count2 = ExcelData.Data.Rows.Count;
		if (settings.FillCaption)
		{
			DevExpress.Spreadsheet.CellRange cellRange = worksheet.Range.FromLTRB(result, result2, count + result - 1, result2);
			cellRange.Merge();
			cellRange.Font.Bold = true;
			Formatting formatting = cellRange.BeginUpdateFormatting();
			formatting.Alignment.Horizontal = SpreadsheetHorizontalAlignment.CenterContinuous;
			formatting.Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
			formatting.EndUpdate();
			cellRange.Value = settings.Caption;
			result2++;
		}
		if (settings.FillColumnsCaptions)
		{
			for (int i = 0; i < count; i++)
			{
				DevExpress.Spreadsheet.Cell cell = worksheet.Cells[result2, i + result];
				cell.Value = ExcelData.Data.Columns[i].Caption;
				cell.Font.Bold = true;
				cell.Borders.SetOutsideBorders(Color.Black, BorderLineStyle.Thin);
			}
			result2++;
		}
		if (settings.InsertMode && count2 > 1)
		{
			DevExpress.Spreadsheet.Row row = worksheet.Rows[result2];
			worksheet.Rows.Insert(result2 + 1, count2 - 1, RowFormatMode.FormatAsPrevious);
			int bottomRowIndex = result2 + count2 - 1;
			worksheet.Range.FromLTRB(row.LeftColumnIndex, result2 + 1, row.RightColumnIndex, bottomRowIndex).CopyFrom(worksheet.Rows[result2], PasteSpecial.All);
		}
		ExportProgressEventArgs e = new ExportProgressEventArgs();
		for (int j = 0; j < count2; j++)
		{
			for (int k = 0; k < count; k++)
			{
				DevExpress.Spreadsheet.Cell cell2 = worksheet[j + result2, k + result];
				cell2.SetValue(ExcelData.Data.Rows[j][k]);
				if (settings.DrawBorders)
				{
					cell2.Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
				}
			}
			e.Row = j + 1;
			ExportProgress(this, e);
		}
		for (int l = 0; l < count; l++)
		{
			if (ExcelData.ColumnsOptions[ExcelData.Data.Columns[l].ColumnName].HasSum)
			{
				string text = CellReferenceParser.ColumnIndexToString(l + result);
				DevExpress.Spreadsheet.Cell cell2 = worksheet[count2 + result2, l + result];
				cell2.Formula = "=SUM(" + text + (result2 + 1) + ":" + text + (count2 + result2) + ")";
			}
		}
		worksheet.Calculate();
		workbook.SaveDocument(settings.File, fileFormat);
		using (Process.Start(new ProcessStartInfo(settings.File)
		{
			UseShellExecute = true
		}))
		{
		}
	}

	private DocumentFormat GetFileFormat(string fileName)
	{
		if (string.IsNullOrWhiteSpace(fileName))
		{
			throw new Exception("File name is empty");
		}
		return Path.GetExtension(fileName).ToLower() switch
		{
			".xls" => DocumentFormat.Xls, 
			".xlsx" => DocumentFormat.Xlsx, 
			".xlsm" => DocumentFormat.Xlsm, 
			".xml" => DocumentFormat.XmlSpreadsheet2003, 
			_ => throw new ArgumentException("File format is incorrect"), 
		};
	}
}
