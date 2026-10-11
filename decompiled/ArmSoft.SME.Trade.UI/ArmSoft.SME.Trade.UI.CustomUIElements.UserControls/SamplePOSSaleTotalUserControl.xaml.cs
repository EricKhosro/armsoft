using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.UI.SmartEnums;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public partial class SamplePOSSaleTotalUserControl : UserControl, IComponentConnector
{
	public TextEdit TB_Total;

	private TotalSumChangedForOutsideDelegate _TotalSumChangedForOutsideDelegate;

	public SamplePOSSaleTotalUserControl(string posViewMode, TotalSumChangedForOutsideDelegate TotalSumChangedForOutsideDelegate)
	{
		InitializeComponent();
		_TotalSumChangedForOutsideDelegate = TotalSumChangedForOutsideDelegate;
		BuildMainGrid(posViewMode);
		ASFontHelper.SetFontSettings(this);
	}

	public void BuildMainGrid(string posMode)
	{
		if (posMode == POSScreenModesSE.SampleScreenWithFavorites.Value)
		{
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = GridLength.Auto
			});
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Style = (Style)Application.Current.Resources["TouchModePrimaryFieldsSeparatorLineColumnDefintionStyle"]
			});
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Style = (Style)Application.Current.Resources["TouchModePrimaryFieldsSeparatorLineColumnDefintionStyle"]
			});
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = new GridLength(1.0, GridUnitType.Star)
			});
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(1.0, GridUnitType.Star)
			});
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Style = (Style)Application.Current.Resources["SamplePOSInsideControlMainGridSeparatorRowDefinition"]
			});
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(1.0, GridUnitType.Star)
			});
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Style = (Style)Application.Current.Resources["SamplePOSInsideControlMainGridSeparatorRowDefinition"]
			});
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = GridLength.Auto
			});
			Grid.SetRow(TB_DiscountCaption, 0);
			Grid.SetColumn(TB_DiscountCaption, 0);
			Grid.SetRow(TB_Discount, 0);
			Grid.SetColumn(TB_Discount, 3);
			Grid.SetRow(TB_MustPayCaption, 2);
			Grid.SetColumn(TB_MustPayCaption, 0);
			Grid.SetRow(TB_MustPay, 2);
			Grid.SetColumn(TB_MustPay, 3);
			TB_TotalCaption.Visibility = Visibility.Collapsed;
			Grid.SetRow(TotalGrid, 4);
			Grid.SetColumn(TotalGrid, 0);
			Grid.SetColumnSpan(TotalGrid, 4);
		}
		else
		{
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = GridLength.Auto
			});
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Style = (Style)Application.Current.Resources["TouchModePrimaryFieldsSeparatorLineColumnDefintionStyle"]
			});
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Style = (Style)Application.Current.Resources["TouchModePrimaryFieldsSeparatorLineColumnDefintionStyle"]
			});
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Style = (Style)Application.Current.Resources["TouchModePrimaryFieldsSeparatorLineColumnDefintionStyle"]
			});
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = GridLength.Auto
			});
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Style = (Style)Application.Current.Resources["TouchModePrimaryFieldsSeparatorLineColumnDefintionStyle"]
			});
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = new GridLength(1.0, GridUnitType.Star)
			});
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
				Style = (Style)Application.Current.Resources["SamplePOSInsideControlMainGridSeparatorRowDefinition"]
			});
			MainGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(1.0, GridUnitType.Star)
			});
			Grid.SetRow(TB_DiscountCaption, 1);
			Grid.SetColumn(TB_DiscountCaption, 0);
			Grid.SetRow(TB_Discount, 1);
			Grid.SetColumn(TB_Discount, 4);
			Grid.SetRow(TB_MustPayCaption, 3);
			Grid.SetColumn(TB_MustPayCaption, 0);
			Grid.SetRow(TB_MustPay, 3);
			Grid.SetColumn(TB_MustPay, 4);
			Grid.SetRow(TB_TotalCaption, 0);
			Grid.SetColumn(TB_TotalCaption, 6);
			Grid.SetRow(TotalGrid, 1);
			Grid.SetRowSpan(TotalGrid, 3);
			Grid.SetColumn(TotalGrid, 6);
		}
		TB_Total = new TextEdit
		{
			Text = "0.00",
			Visibility = Visibility.Collapsed
		};
		TB_Total.EditValueChanged += TB_Total_EditValueChanged;
	}

	private void TB_Total_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		decimal num = Convert.ToDecimal(TB_Total.Text);
		long num2 = (long)num;
		decimal num3 = num - (decimal)num2;
		TotalIntPartOfTextBlock.Text = num2.ToString("#,##0");
		TotalDecimalPartOfTextBlock.Text = "." + (num3 * 100m).ToString("00");
		_TotalSumChangedForOutsideDelegate();
	}
}
