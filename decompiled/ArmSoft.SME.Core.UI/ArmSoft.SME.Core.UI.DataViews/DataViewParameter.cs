namespace ArmSoft.SME.Core.UI.DataViews;

public class DataViewParameter
{
	public string Name { get; set; } = "";

	public string Caption { get; set; } = "";

	public object Value { get; set; }

	public string Format { get; set; } = "";

	public bool IncludeCaptionToolTip { get; set; } = true;

	public IDropDownView DataView { get; set; }
}
