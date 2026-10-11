using ArmSoft.SME.Core.UI.Controls;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Directories;

public class DaysHoursDiscountControls
{
	public LayoutGroup Lg { get; set; }

	public CheckEditExt Active { get; set; }

	public TimeEditControl FirstStartTime { get; set; }

	public TimeEditControl FirstEndTime { get; set; }

	public TimeEditControl SecondStartTime { get; set; }

	public TimeEditControl SecondEndTime { get; set; }
}
