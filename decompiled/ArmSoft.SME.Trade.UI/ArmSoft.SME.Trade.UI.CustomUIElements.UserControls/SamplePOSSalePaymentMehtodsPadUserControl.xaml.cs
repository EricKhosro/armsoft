using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.SmartEnums;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public partial class SamplePOSSalePaymentMehtodsPadUserControl : UserControl, IComponentConnector
{
	public delegate void ButtonClickEventHandler(object sender, SaleWindowHelper.ButtonClickEventArgs e);

	public event ButtonClickEventHandler ButtonClick;

	public SamplePOSSalePaymentMehtodsPadUserControl(string posViewMode)
	{
		InitializeComponent();
		SetPaymentMethodsGrid(posViewMode);
		ASFontHelper.SetFontSettings(this);
	}

	private void SetPaymentMethodsGrid(string posViewMode)
	{
		if (Settings.ParametersService.AllowSaleWithQRCodes() && posViewMode == POSScreenModesSE.SampleScreen.Value)
		{
			CashButton.Visibility = Visibility.Collapsed;
			CashQRButton.Visibility = Visibility.Visible;
			CashQRButtonCaption.Visibility = Visibility.Visible;
			NonCashButton.Visibility = Visibility.Collapsed;
			NonCashQRButton.Visibility = Visibility.Visible;
			NonCashQRButtonCaption.Visibility = Visibility.Visible;
			CashInfoGrid.VerticalAlignment = VerticalAlignment.Center;
			NonCashInfoGrid.VerticalAlignment = VerticalAlignment.Center;
			Grid.SetRow(CashSummTextBorder, 3);
			Grid.SetRowSpan(CashSummTextBorder, 1);
			CashSummTextBorder.VerticalAlignment = VerticalAlignment.Top;
			Grid.SetRow(NonCashSummTextBorder, 3);
			Grid.SetRowSpan(NonCashSummTextBorder, 1);
			NonCashSummTextBorder.VerticalAlignment = VerticalAlignment.Top;
			NonCashQRButton.IsEnabled = Settings.ParametersService.MTBillPlCardPaymentShowButton();
		}
		else
		{
			CashButton.Visibility = Visibility.Visible;
			CashQRButton.Visibility = Visibility.Collapsed;
			CashQRButtonCaption.Visibility = Visibility.Hidden;
			NonCashButton.Visibility = Visibility.Visible;
			NonCashQRButton.Visibility = Visibility.Collapsed;
			NonCashQRButtonCaption.Visibility = Visibility.Hidden;
			CashInfoGrid.VerticalAlignment = VerticalAlignment.Bottom;
			NonCashInfoGrid.VerticalAlignment = VerticalAlignment.Top;
			Grid.SetRow(CashSummTextBorder, 0);
			Grid.SetRowSpan(CashSummTextBorder, 4);
			CashSummTextBorder.VerticalAlignment = VerticalAlignment.Stretch;
			Grid.SetRow(NonCashSummTextBorder, 0);
			Grid.SetRowSpan(NonCashSummTextBorder, 4);
			NonCashSummTextBorder.VerticalAlignment = VerticalAlignment.Stretch;
			NonCashButton.IsEnabled = Settings.ParametersService.MTBillPlCardPaymentShowButton();
		}
	}

	private void ButtonNotCash_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.NotCash));
	}

	private void ButtonCash_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Cash));
	}
}
