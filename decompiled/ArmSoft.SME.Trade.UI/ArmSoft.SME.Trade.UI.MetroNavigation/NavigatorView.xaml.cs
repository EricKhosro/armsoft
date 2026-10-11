using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.MetroNavigation;

public partial class NavigatorView : UserControl, IComponentConnector
{
	public NavigatorView()
	{
		InitializeComponent();
		ASFontHelper.SetFontSettings(this);
	}
}
