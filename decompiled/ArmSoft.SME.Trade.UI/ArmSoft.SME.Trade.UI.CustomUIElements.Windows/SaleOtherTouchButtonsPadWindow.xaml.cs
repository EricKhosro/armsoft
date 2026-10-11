using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.Windows;

public partial class SaleOtherTouchButtonsPadWindow : ThemedWindow, IComponentConnector
{
	public delegate void ButtonClickEventHandler(object sender, SaleWindowHelper.ButtonNumericClickEventArgs e);

	public delegate void TaxCodeButtonClickEventHandler(object sender, SaleWindowHelper.ButtonClickEventArgs e);

	public delegate void FindBuyerButtonClickEventHandler(object sender, SaleWindowHelper.ButtonClickEventArgs e);

	public delegate void LastCheckECRButtonClickEventHandler(object sender, SaleWindowHelper.ButtonClickEventArgs e);

	public event ButtonClickEventHandler ButtonClick;

	public event TaxCodeButtonClickEventHandler TaxCodeButtonClick;

	public event FindBuyerButtonClickEventHandler FindBuyerButtonClick;

	public event LastCheckECRButtonClickEventHandler LastCheckECRButtonClick;

	public SaleOtherTouchButtonsPadWindow(Window ownerWindow)
	{
		InitializeComponent();
		Owner = ownerWindow;
		SetConnectionDependedFields();
		ASFontHelper.SetFontSettings(this);
	}

	private void SetConnectionDependedFields()
	{
		if (Settings.IsOffline)
		{
			CashInButton.IsEnabled = false;
			CashOutButton.IsEnabled = false;
			SalesDocumentsHistoryButton.IsEnabled = false;
			NewBuyer.IsEnabled = false;
			EMarkReceiptButton.IsEnabled = false;
			PrePayECRButton.IsEnabled = false;
			PrePayReturnECRButton.IsEnabled = false;
		}
		else
		{
			if (Settings.ParametersService.ShowCashDeskAmount())
			{
				SummCurrSummNccPair accRem = TradeSettings.CreateTradeClient().CommonServices.GetAccRem(CommonConst.AccountingsTypes.Cash, Settings.ParametersService.DefaultCash(), -1, "AMD", TradeSettings.CreateTradeClient().CommonServices.GetServerDate().ToDateOnly());
				AvailableCash.Text = ArmSoft.SME.Trade.Common.Resources.InCashDesk + " = ";
				AvailableCashAmount.Text = accRem.SummNcc.CurrencyFormat(16, 2) + " ";
				CurCode.Text = ArmSoft.SME.Common.Resources.DramWordSmallLetter;
			}
			if (!Settings.ParametersService.AllowPOSCashInOut())
			{
				CashInButton.IsEnabled = false;
				CashOutButton.IsEnabled = false;
			}
			NewBuyer.IsEnabled = Settings.ParametersService.AllowToRegisterNewBuyer();
			if (!Settings.ParametersService.AllowPOSPrePaymentECRCheck())
			{
				PrePayECRButton.IsEnabled = false;
				PrePayReturnECRButton.IsEnabled = false;
			}
		}
		if (!Settings.ParametersService.AllowPOSChooseBuyer())
		{
			FindBuyerButton.IsEnabled = false;
			TaxCodeButton.IsEnabled = false;
		}
		if (!Settings.ParametersService.UseMarking() || !Settings.ParametersService.EMarkReceiptShowButton())
		{
			EMarkReceiptButton.Visibility = Visibility.Collapsed;
		}
	}

	private void NewBuyer_Click(object sender, RoutedEventArgs e)
	{
		if (Settings.IsOffline)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.InvalidOperationForBrokenConnection);
		}
		Close();
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs(SaleWindowHelper.ButtonCommands.NewBuyer, ""));
	}

	private void EMarkReceiptButton_Click(object sender, RoutedEventArgs e)
	{
		if (Settings.IsOffline)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.InvalidOperationForBrokenConnection);
		}
		Close();
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs(SaleWindowHelper.ButtonCommands.EMarkReceipt, ""));
	}

	private void SalesDocumentsHistoryButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs(SaleWindowHelper.ButtonCommands.SalesDocumentsHistory, ""));
	}

	private void PrePayECRButton_Click(object sender, RoutedEventArgs e)
	{
		if (Settings.IsOffline)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.InvalidOperationForBrokenConnection);
		}
		Close();
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs(SaleWindowHelper.ButtonCommands.PrePayECR, ""));
	}

	private void CashInButton_Click(object sender, RoutedEventArgs e)
	{
		if (Settings.IsOffline)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.InvalidOperationForBrokenConnection);
		}
		Close();
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs(SaleWindowHelper.ButtonCommands.CashIn, ""));
	}

	private void CashOutButton_Click(object sender, RoutedEventArgs e)
	{
		if (Settings.IsOffline)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.InvalidOperationForBrokenConnection);
		}
		Close();
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs(SaleWindowHelper.ButtonCommands.CashOut, ""));
	}

	private void PrePayReturnECRButton_Click(object sender, RoutedEventArgs e)
	{
		if (Settings.IsOffline)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.InvalidOperationForBrokenConnection);
		}
		Close();
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonNumericClickEventArgs(SaleWindowHelper.ButtonCommands.PrePayRetECR, ""));
	}

	private void TaxCodeButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
		TaxCodeButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.TaxCode));
	}

	private void FindBuyerButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
		FindBuyerButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.FindBuyer));
	}

	private void LastECRCheckButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
		LastCheckECRButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.LastECRCheck));
	}

	private void SaleOtherTouchButtonsPadWindow_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.Key == 13)
			{
				DialogResult = false;
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
