using System.Linq;
using DevExpress.Spreadsheet;

namespace ArmSoft.PrintTemplates;

public class SpreadsheetTemplateSubstitutorXlsm : SpreadsheetTemplateSubstitutorBase
{
	private const int maxColumnCount = 250;

	public SpreadsheetTemplateSubstitutorXlsm(IPrintTemplateSubstitution printTemplateSubstitution, string outputFormat = "", bool check = false)
		: base(printTemplateSubstitution, "XLSM", outputFormat, "", check)
	{
	}

	protected override void SubstituteTables(Workbook workbook)
	{
		foreach (Worksheet item in workbook.Sheets.Cast<Worksheet>())
		{
			foreach (PrintTemplateSubstitutionGrid value in PrintTemplateSubstitution.GridSubstitutions.Substitutions.Values)
			{
				if (string.Compare(item.Name, value.Name, ignoreCase: true) == 0)
				{
					SubstituteToTable(value, item);
				}
			}
		}
	}

	private void SubstituteToTable(IPrintTemplateSubstitutionGrid grid, Worksheet sheet)
	{
		if (grid.RowCount == 0)
		{
			sheet[0, 0].Value = 0;
			return;
		}
		for (int i = 0; i <= 250; i++)
		{
			Cell cell = sheet[0, i];
			foreach (GridColumn sortedByLengthColum in grid.GetSortedByLengthColums())
			{
				if (cell.Value.IsText)
				{
					if (string.Compare(cell.Value.TextValue, "%" + grid.Name + ".RowCount", ignoreCase: true) == 0)
					{
						cell.Value = grid.RowCount.ToString();
						break;
					}
					if (string.Compare(cell.Value.TextValue, "%" + grid.Name + "." + sortedByLengthColum.Name, ignoreCase: true) == 0)
					{
						SubstituteToColumn(grid, sheet, sortedByLengthColum.Name, i);
						break;
					}
				}
			}
		}
	}

	private void SubstituteToColumn(IPrintTemplateSubstitutionGrid grid, Worksheet sheet, string colName, int colInd)
	{
		if (grid.RowCount > 0)
		{
			Cell source = sheet[0, colInd];
			int i = 0;
			for (int rowCount = grid.RowCount; i < rowCount; i++)
			{
				sheet[i, colInd].CopyFrom(source, PasteSpecial.Formats);
				sheet[i, colInd].SetValue(grid.GetValueConsideringFormatter(i, colName));
			}
		}
	}
}
