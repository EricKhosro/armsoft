using DevExpress.XtraPrinting;

namespace ArmSoft.SME.Core.UI.Report;

public class AsTableReportColumn
{
	public string FieldName { get; set; }

	public bool IsSummary { get; set; }

	public bool IsGroupColumn { get; set; }

	public int VisibleIndex { get; set; }

	public int GridIndex { get; set; }

	public float WidthF { get; set; }

	public string Header { get; set; }

	public TextAlignment ContentAlignment { get; set; }

	public string DisplayFormat { get; set; }

	public bool BooleanBased { get; set; }

	public bool IsImage { get; set; }
}
