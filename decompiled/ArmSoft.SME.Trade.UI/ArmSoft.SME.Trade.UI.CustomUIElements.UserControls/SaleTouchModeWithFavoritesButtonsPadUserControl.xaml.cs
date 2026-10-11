using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public partial class SaleTouchModeWithFavoritesButtonsPadUserControl : UserControl, IComponentConnector
{
	public delegate void ButtonClickEventHandler(object sender, SaleWindowHelper.ButtonClickEventArgs e);

	public event ButtonClickEventHandler ButtonClick;

	public SaleTouchModeWithFavoritesButtonsPadUserControl()
	{
		InitializeComponent();
		SetDefaultValuesAndStates();
		ASFontHelper.SetFontSettings(this);
	}

	private void SetDefaultValuesAndStates()
	{
		ButtonSaveCaption.Text = (Settings.ParametersService.SaveAndPrintECR() ? ArmSoft.SME.Trade.Common.Resources.SaveAndPrintECR : ArmSoft.SME.Common.Resources.SetDocProcess);
		ButtonNotCash.IsEnabled = Settings.ParametersService.MTBillPlCardPaymentShowButton();
		PrintECRCheckButton.IsEnabled = Settings.ParametersService.MTBillPrintECRCheckShowButton();
		ButtonReturn.IsEnabled = Settings.ParametersService.MTBillReturnBillShowButton();
		ButtonContinue.IsEnabled = Settings.ParametersService.POSContinueShowButton();
		ButtonDefer.IsEnabled = Settings.ParametersService.POSDeferSaleShowButton();
		ButtonCard.IsEnabled = Settings.ParametersService.POSPartnerCardShowButton();
		if (!string.IsNullOrWhiteSpace(ElScaleInfo.GetInstance().ElScaleType))
		{
			ButtonQTYCaption.Text = ArmSoft.SME.Trade.Common.Resources.QuantityWeightShort;
		}
	}

	private void PrintECRCheckButton_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.PrintCheck));
	}

	private void ButtonReturn_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Return));
	}

	private void ButtonDefer_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Defer));
	}

	private void ButtonContinue_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Continue));
	}

	private void ButtonCard_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Card));
	}

	private void ButtonClearPayment_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.ClearPayment));
	}

	private void ButtonNotCash_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.NotCash));
	}

	private void ButtonCash_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Cash));
	}

	private void ButtonSave_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Save));
	}

	private void ButtonSelectMT_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.SelectMT));
	}

	private void ButtonOther_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Other));
	}

	private void ButtonQTY_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.QTY));
	}

	private void NumericTouchPadButton_Click(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		ButtonClick?.Invoke(sender, e);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
