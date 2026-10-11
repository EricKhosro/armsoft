using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public partial class SamplePOSSaleDiscountInfoUserControl : UserControl, IComponentConnector
{
	public SamplePOSSaleDiscountInfoUserControl()
	{
		InitializeComponent();
		ASFontHelper.SetFontSettings(this);
	}
}
