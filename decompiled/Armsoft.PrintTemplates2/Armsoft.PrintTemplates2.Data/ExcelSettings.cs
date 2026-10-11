namespace Armsoft.PrintTemplates2.Data;

public class ExcelSettings
{
	public string File { get; set; } = string.Empty;

	public string Caption { get; set; } = string.Empty;

	public string Template { get; set; } = string.Empty;

	public string WorkSheet { get; set; } = string.Empty;

	public string PositionRow { get; set; } = string.Empty;

	public string PositionColumn { get; set; } = string.Empty;

	public string PositionEndRow { get; set; } = string.Empty;

	public string PositionEndColumn { get; set; } = string.Empty;

	public bool InsertMode { get; set; }

	public bool DrawBorders { get; set; }

	public bool FillCaption { get; set; }

	public bool FillColumnsCaptions { get; set; }
}
