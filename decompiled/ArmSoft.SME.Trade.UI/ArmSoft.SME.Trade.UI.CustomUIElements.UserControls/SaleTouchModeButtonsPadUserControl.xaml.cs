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

public partial class SaleTouchModeButtonsPadUserControl : UserControl, IComponentConnector
{
	public delegate void ButtonClickEventHandler(object sender, SaleWindowHelper.ButtonClickEventArgs e);

	public event ButtonClickEventHandler ButtonClick;

	public SaleTouchModeButtonsPadUserControl()
	{
		InitializeComponent();
		SetDefaultValuesAndStates();
		SetConnectionDependedFields();
		ASFontHelper.SetFontSettings(this);
	}

	private void SetDefaultValuesAndStates()
	{
		if (!string.IsNullOrWhiteSpace(ElScaleInfo.GetInstance().ElScaleType))
		{
			ButtonQTYCaption.Text = ArmSoft.SME.Trade.Common.Resources.QuantityWeightShort;
		}
		ButtonSaveCaption.Text = (Settings.ParametersService.SaveAndPrintECR() ? ArmSoft.SME.Trade.Common.Resources.SaveAndPrintECR : ArmSoft.SME.Common.Resources.SetDocProcess);
		ButtonNotCash.IsEnabled = Settings.ParametersService.MTBillPlCardPaymentShowButton();
		ButtonFromPrePay.IsEnabled = Settings.ParametersService.MTBillUsePrePaymentShowButton();
		ButtonCreditSale.IsEnabled = Settings.ParametersService.MTBillCreditShowButton();
		PrintButton.IsEnabled = Settings.ParametersService.AllowPrintMTCheck() && Settings.ParametersService.MTBillPrintBillShowButton();
		PrintECRCheckButton.IsEnabled = Settings.ParametersService.MTBillPrintECRCheckShowButton();
		ButtonReturn.IsEnabled = Settings.ParametersService.MTBillReturnBillShowButton();
		ButtonMTFavorites.IsEnabled = Settings.ParametersService.MTBillFavoritesShowButton();
		ButtonContinue.IsEnabled = Settings.ParametersService.POSContinueShowButton();
		ButtonDefer.IsEnabled = Settings.ParametersService.POSDeferSaleShowButton();
		ButtonCard.IsEnabled = Settings.ParametersService.POSPartnerCardShowButton();
	}

	private void SetConnectionDependedFields()
	{
		if (Settings.IsOffline)
		{
			ButtonUseBonus.IsEnabled = false;
		}
		else
		{
			ButtonUseBonus.IsEnabled = true;
		}
	}

	private void ButtonUseBonus_Click(object sender, RoutedEventArgs e)
	{
		if (Settings.IsOffline)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.InvalidOperationForBrokenConnection);
		}
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.UseBonus));
	}

	private void ExitButton_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Exit));
	}

	private void PrintButton_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.PrintWithCount));
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

	private void ButtonAddToBonus_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.AddToBonus));
	}

	private void ButtonCode_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Code));
	}

	private void ButtonBarCode_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.BarCode));
	}

	private void ButtonQTY_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.QTY));
	}

	private void ButtonPrice_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Price));
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

	private void ButtonMTFavorites_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.MTFavorites));
	}

	private void ButtonSelectMT_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.SelectMT));
	}

	private void ButtonFromPrePay_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.FromPrePay));
	}

	private void ButtonOther_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Other));
	}

	private void ButtonCreditSale_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.CreditSale));
	}

	private void ButtonSaleConsultant_click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.SaleConsultant));
	}

	private void NumericTouchPad_ButtonClick(object sender, SaleWindowHelper.ButtonClickEventArgs e)
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
