namespace ArmSoft.SME.Core.UI.Report;

public class AsReportColumn
{
	public string Caption { get; set; }

	public int Length { get; set; }

	public string BindingField { get; set; }

	public bool AllowDecreaseWidth { get; set; }

	public ColumnsType ColType { get; set; }

	public float WidthF { get; set; }
}
