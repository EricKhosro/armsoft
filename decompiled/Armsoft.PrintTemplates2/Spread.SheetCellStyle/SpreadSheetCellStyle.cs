namespace Spread.SheetCellStyle;

public class SpreadSheetCellStyle
{
	public const string BooleanNotDefined = "2";

	public string Color { get; set; }

	public string BackColor { get; set; }

	public string Bold { get; set; }

	public string Italic { get; set; }

	public string Underline { get; set; }

	public SpreadSheetCellStyle()
	{
		Bold = "2";
		Italic = "2";
		Underline = "2";
	}
}
