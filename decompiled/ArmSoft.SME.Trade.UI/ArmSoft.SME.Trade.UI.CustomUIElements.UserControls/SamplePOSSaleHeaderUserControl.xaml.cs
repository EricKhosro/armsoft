using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public partial class SamplePOSSaleHeaderUserControl : UserControl, IComponentConnector
{
	public delegate void ButtonClickEventHandler(object sender, SaleWindowHelper.ButtonClickEventArgs e);

	public event ButtonClickEventHandler ButtonClick;

	public SamplePOSSaleHeaderUserControl()
	{
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		if (Settings.IsOffline)
		{
			ConnectionIndicatorBorder.BorderBrush = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326")));
			ConnectionIndicatorBorder.Background = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326")));
			ConnectionIndicatorTextBlock.Text = ArmSoft.SME.Common.Resources.Offline.ToUpper();
			ConnectionIndicatorImage.Source = ImageService.GetImage("pos_disconnect_32.svg");
		}
		else
		{
			ConnectionIndicatorBorder.BorderBrush = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#4fad77")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#217346")));
			ConnectionIndicatorBorder.Background = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#4fad77")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#217346")));
			ConnectionIndicatorTextBlock.Text = ArmSoft.SME.Common.Resources.Online.ToUpper();
			ConnectionIndicatorImage.Source = ImageService.GetImage("pos_connect_32.svg");
		}
		ASFontHelper.SetFontSettings(this);
	}

	private void CloseTitleButton_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Exit));
	}
}
