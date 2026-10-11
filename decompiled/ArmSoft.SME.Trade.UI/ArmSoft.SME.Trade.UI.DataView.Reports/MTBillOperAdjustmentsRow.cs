using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Trade.UI.DataView.Reports;

public class MTBillOperAdjustmentsRow : DocumentGridRow
{
	public string Operation { get; set; }

	public string HotKey { get; set; }

	public bool ShowButton { get; set; } = true;

	public bool AllowEditBooleanCell { get; set; }
}
