using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Trade.UI.DataView.Reports;

public class MTFillRowFilds : DocumentGridRow
{
	public string Row { get; set; }

	public bool Show { get; set; } = true;

	public bool Edit { get; set; }

	public bool AllowEditBooleanCell { get; set; }

	public bool AllowEditBooleanCellEdit { get; set; } = true;
}
