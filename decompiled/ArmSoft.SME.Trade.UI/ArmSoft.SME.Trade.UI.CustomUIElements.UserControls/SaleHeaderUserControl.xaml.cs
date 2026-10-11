using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public partial class SaleHeaderUserControl : UserControl, IComponentConnector
{
	public delegate void ButtonClickEventHandler(object sender, SaleWindowHelper.ButtonClickEventArgs e);

	public event ButtonClickEventHandler ButtonClick;

	public SaleHeaderUserControl()
	{
		InitializeComponent();
		if (Settings.IsOffline)
		{
			ConnectionIndicatorBorder.BorderBrush = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326")));
			ConnectionIndicatorBorder.Background = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326")));
			ConnectionIndicatorTextBlock.Text = ArmSoft.SME.Common.Resources.Offline.ToUpper();
		}
		else
		{
			ConnectionIndicatorBorder.BorderBrush = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#4fad77")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#3E7337")));
			ConnectionIndicatorBorder.Background = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#4fad77")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#3E7337")));
			ConnectionIndicatorTextBlock.Text = ArmSoft.SME.Common.Resources.Online.ToUpper();
		}
		ASFontHelper.SetFontSettings(this);
	}

	private void CloseTitleButton_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Exit));
	}
}
