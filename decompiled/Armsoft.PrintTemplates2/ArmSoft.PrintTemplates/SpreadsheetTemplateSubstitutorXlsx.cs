using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Armsoft.PrintTemplates2.Resources;
using DevExpress.Spreadsheet;
using Spread.SheetCellStyle;

namespace ArmSoft.PrintTemplates;

public class SpreadsheetTemplateSubstitutorXlsx(IPrintTemplateSubstitution printTemplateSubstitution, string outputFormat = "", string outputPassword = "", bool protect = false, bool check = false) : SpreadsheetTemplateSubstitutorBase(printTemplateSubstitution, "XLSX", outputFormat, outputPassword, check)
{
	private bool Protect { get; set; } = protect;

	protected override void SubstituteTables(Workbook workbook)
	{
		string password = null;
		if (Protect)
		{
			password = Guid.NewGuid().ToString();
			workbook.Protect(password, lockStructure: true, lockWindows: true);
		}
		foreach (Worksheet item in workbook.Sheets.Cast<Worksheet>())
		{
			if (Protect)
			{
				item.Protect(password, WorksheetProtectionPermissions.Default);
			}
			int findStartFromRowIndex = item.Rows.LastUsedIndex;
			string foundGridName;
			while (FindTableDefinition(item, ref findStartFromRowIndex, out foundGridName))
			{
				if (!PrintTemplateSubstitution.GridSubstitutions.Substitutions.ContainsKey(foundGridName))
				{
					throw new InvalidTemplateException(string.Format(Resource.InvalidGridSubstitution, foundGridName));
				}
				if (!IsValidTableDefinition(item, findStartFromRowIndex))
				{
					throw new InvalidTemplateException(Resource.SideBySideTables);
				}
				SubstituteTable(item, findStartFromRowIndex, foundGridName);
				findStartFromRowIndex--;
			}
		}
	}

	protected bool FindTableDefinition(Worksheet sheet, ref int findStartFromRowIndex, out string foundGridName)
	{
		foundGridName = string.Empty;
		while (findStartFromRowIndex >= 0)
		{
			foreach (Cell existingCell in sheet.Rows[findStartFromRowIndex].ExistingCells)
			{
				if (!existingCell.Value.IsText)
				{
					continue;
				}
				string columnName;
				if (existingCell.HasRichText)
				{
					foreach (RichTextRun run in existingCell.GetRichText().Runs)
					{
						if (TryParseTableColumnDefinition(run.Text, out foundGridName, out columnName))
						{
							return true;
						}
					}
				}
				else if (TryParseTableColumnDefinition(existingCell.Value.TextValue, out foundGridName, out columnName))
				{
					return true;
				}
			}
			findStartFromRowIndex--;
		}
		return false;
	}

	protected bool TryParseTableColumnDefinition(string definition, out string tableName, out string columnName)
	{
		Match match = TableRegEx.Match(definition);
		if (match.Success)
		{
			tableName = match.Groups["TableName"].Value;
			columnName = match.Groups["ColumnName"].Value;
		}
		else
		{
			tableName = string.Empty;
			columnName = string.Empty;
		}
		return match.Success;
	}

	protected bool IsValidTableDefinition(Worksheet sheet, int checkRowIndex)
	{
		string text = string.Empty;
		string tableName = null;
		bool flag = false;
		foreach (Cell existingCell in sheet.Rows[checkRowIndex].ExistingCells)
		{
			if (!existingCell.Value.IsText)
			{
				continue;
			}
			string columnName;
			if (existingCell.HasRichText)
			{
				foreach (RichTextRun run in existingCell.GetRichText().Runs)
				{
					flag = TryParseTableColumnDefinition(run.Text, out tableName, out columnName);
					if (flag)
					{
						break;
					}
				}
			}
			else
			{
				flag = TryParseTableColumnDefinition(existingCell.Value.TextValue, out tableName, out columnName);
			}
			if (flag)
			{
				if (text == string.Empty)
				{
					text = tableName;
				}
				else if (tableName != text)
				{
					return false;
				}
			}
		}
		return text != string.Empty;
	}

	protected void SubstituteTable(Worksheet sheet, int rowIndex, string gridName)
	{
		IPrintTemplateSubstitutionGrid printTemplateSubstitutionGrid = PrintTemplateSubstitution.GridSubstitutions.Substitutions[gridName];
		string columnName = string.Empty;
		if (printTemplateSubstitutionGrid.RowCount == 0)
		{
			sheet.Rows.Remove(rowIndex);
			return;
		}
		if (printTemplateSubstitutionGrid.RowCount > 1)
		{
			Row row = sheet.Rows[rowIndex];
			sheet.Rows.Insert(rowIndex + 1, printTemplateSubstitutionGrid.RowCount - 1, RowFormatMode.FormatAsPrevious);
			sheet.Range.FromLTRB(row.LeftColumnIndex, rowIndex + 1, row.RightColumnIndex, rowIndex + printTemplateSubstitutionGrid.RowCount - 1).CopyFrom(sheet.Rows[rowIndex], PasteSpecial.Formats);
		}
		foreach (Cell existingCell in sheet.Rows[rowIndex].ExistingCells)
		{
			bool flag = false;
			if (!existingCell.Value.IsText)
			{
				continue;
			}
			string tableName;
			if (existingCell.HasRichText)
			{
				foreach (RichTextRun run in existingCell.GetRichText().Runs)
				{
					if (TryParseTableColumnDefinition(run.Text, out tableName, out columnName))
					{
						flag = true;
						break;
					}
				}
			}
			else if (TryParseTableColumnDefinition(existingCell.Value.TextValue, out tableName, out columnName))
			{
				flag = true;
			}
			int columnIndex = existingCell.ColumnIndex;
			if (flag)
			{
				int i = 0;
				for (int num = printTemplateSubstitutionGrid.RowCount - 1; i <= num; i++)
				{
					sheet.Cells[rowIndex + i, columnIndex].SetValue(printTemplateSubstitutionGrid.GetValueConsideringFormatter(i, columnName));
					if (printTemplateSubstitutionGrid.UseStyles)
					{
						ApplyCellStyle(sheet.Cells[rowIndex + i, columnIndex], printTemplateSubstitutionGrid.GetStyle(i, columnName));
					}
				}
			}
			else
			{
				string textValue = existingCell.Value.TextValue;
				int j = 0;
				for (int num2 = printTemplateSubstitutionGrid.RowCount - 1; j <= num2; j++)
				{
					sheet.Cells[rowIndex + j, columnIndex].SetValue(textValue);
				}
			}
		}
	}

	protected void ApplyCellStyle(Cell cell, SpreadSheetCellStyle style)
	{
		if (style == null)
		{
			return;
		}
		if (style.Bold != "2")
		{
			cell.Font.Bold = style.Bold == "1";
		}
		if (style.Italic != "2")
		{
			cell.Font.Italic = style.Italic == "1";
		}
		if (style.Underline != "2")
		{
			if (style.Underline == "1")
			{
				cell.Font.UnderlineType = UnderlineType.Single;
			}
			else
			{
				cell.Font.UnderlineType = UnderlineType.None;
			}
		}
		if (!string.IsNullOrWhiteSpace(style.Color))
		{
			cell.Font.Color = ColorTranslator.FromHtml(style.Color);
		}
		if (!string.IsNullOrWhiteSpace(style.BackColor))
		{
			cell.Fill.PatternType = PatternType.Solid;
			cell.Fill.BackgroundColor = ColorTranslator.FromHtml(style.BackColor);
		}
	}
}
