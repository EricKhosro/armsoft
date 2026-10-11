namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class FilteredSearchGridColumn
{
	public string Header { get; set; } = string.Empty;

	public string FieldName { get; set; } = string.Empty;

	public bool AllowEditing { get; set; }

	public string NumericFormat { get; set; } = string.Empty;

	public bool ShowTotalSummary { get; set; } = true;

	public double CustomWidth { get; set; }
}
