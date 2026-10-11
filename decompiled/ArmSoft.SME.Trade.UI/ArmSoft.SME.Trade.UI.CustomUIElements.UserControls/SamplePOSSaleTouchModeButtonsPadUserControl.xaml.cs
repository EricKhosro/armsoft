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

public partial class SamplePOSSaleTouchModeButtonsPadUserControl : UserControl, IComponentConnector
{
	public delegate void ButtonClickEventHandler(object sender, SaleWindowHelper.ButtonClickEventArgs e);

	public event ButtonClickEventHandler ButtonClick;

	public SamplePOSSaleTouchModeButtonsPadUserControl(SamplePOSSalePaymentMehtodsPadUserControl modernPOSSalePaymentMehtodsPadUserControl)
	{
		InitializeComponent();
		ConstructUIControlsPlaces(modernPOSSalePaymentMehtodsPadUserControl);
		SetDefaultValuesAndStates();
		ASFontHelper.SetFontSettings(this);
	}

	private void ConstructUIControlsPlaces(SamplePOSSalePaymentMehtodsPadUserControl modernPOSSalePaymentMehtodsPadUserControl)
	{
		if (modernPOSSalePaymentMehtodsPadUserControl != null)
		{
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = GridLength.Auto
			});
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Style = (Style)Application.Current.Resources["SamplePOSSaleWindowPrimaryButtonsBlocksSeparatorRowsDefintionStyle"]
			});
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(1.0, GridUnitType.Star)
			});
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Style = (Style)Application.Current.Resources["SamplePOSSaleWindowPrimaryButtonsBlocksSeparatorRowsDefintionStyle"]
			});
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = GridLength.Auto
			});
			Grid.SetRow(OperationButtonsGrid, 0);
			Grid.SetRow(modernPOSSalePaymentMehtodsPadUserControl, 2);
			Grid.SetRow(DocumentSavingButtonsGrid, 4);
			MainGrid.Children.Add(modernPOSSalePaymentMehtodsPadUserControl);
		}
		else
		{
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = GridLength.Auto
			});
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(1.0, GridUnitType.Star)
			});
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Style = (Style)Application.Current.Resources["SamplePOSSaleWindowPrimaryButtonsBlocksSeparatorRowsDefintionStyle"]
			});
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = GridLength.Auto
			});
			Grid.SetRow(OperationButtonsGrid, 0);
			Grid.SetRow(DocumentSavingButtonsGrid, 3);
		}
	}

	private void SetDefaultValuesAndStates()
	{
		ButtonCardCaption.Text = ArmSoft.SME.Trade.Common.Resources.Buyers + " " + ArmSoft.SME.Trade.Common.Resources.Card;
		if (Settings.ParametersService.SaveAndPrintECR())
		{
			ButtonSaveCaption.Text = $"{ArmSoft.SME.Common.Resources.SetDocProcess} + {ArmSoft.SME.Common.Resources.Print} {ArmSoft.SME.Common.Resources.ECRC}";
			PrintECRCheckButton.Visibility = Visibility.Collapsed;
			Grid.SetColumnSpan(PrintButton, 3);
		}
		else
		{
			ButtonSaveCaption.Text = ArmSoft.SME.Common.Resources.SetDocProcess;
			PrintECRCheckButton.Visibility = Visibility.Visible;
			Grid.SetColumnSpan(PrintButton, 1);
		}
		PrintButton.IsEnabled = Settings.ParametersService.AllowPrintMTCheck() && Settings.ParametersService.MTBillPrintBillShowButton();
		PrintECRCheckButton.IsEnabled = Settings.ParametersService.MTBillPrintECRCheckShowButton();
		ButtonReturn.IsEnabled = Settings.ParametersService.MTBillReturnBillShowButton();
		ButtonMTFavorites.IsEnabled = Settings.ParametersService.MTBillFavoritesShowButton();
		ButtonContinue.IsEnabled = Settings.ParametersService.POSContinueShowButton();
		ButtonDefer.IsEnabled = Settings.ParametersService.POSDeferSaleShowButton();
		ButtonCard.IsEnabled = Settings.ParametersService.POSPartnerCardShowButton();
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

	private void ButtonOther_Click(object sender, RoutedEventArgs e)
	{
		ButtonClick?.Invoke(this, new SaleWindowHelper.ButtonClickEventArgs(SaleWindowHelper.ButtonCommands.Other));
	}
}
