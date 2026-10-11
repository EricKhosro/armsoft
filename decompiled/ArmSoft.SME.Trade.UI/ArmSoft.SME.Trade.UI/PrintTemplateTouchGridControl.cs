using System;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.CustomWindowControls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.DropDownsData;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.Helpers;

namespace ArmSoft.SME.Trade.UI;

public class PrintTemplateTouchGridControl : TouchGridControl<TemplateDataProvider.DataRow>
{
	public delegate void ButtonClickEventHandler(object sender, SaleWindowHelper.PrintButtonClickEventArgs e);

	private CWCSimpleButton _qtyPlusButton;

	private CWCSimpleButton _qtyMinusButton;

	private TextBlock _printCountTextBlock;

	public event ButtonClickEventHandler ButtonClick;

	public PrintTemplateTouchGridControl(Window windowOwner, Template dropDownView)
		: base(windowOwner, (IDropDownView)dropDownView, "PrintTeplateSelectTouchDialog")
	{
	}

	protected override void InitializeComponent()
	{
		base.InitializeComponent();
		AddPrintCount();
		AddButtonQtyPlus();
		AddButtonQtyMinus();
		EmptyRow.Height = new GridLength(30.0);
	}

	private void AddPrintCount()
	{
		TextBlock element = new TextBlock
		{
			Text = ArmSoft.SME.Trade.Common.Resources.PrintCopies,
			FontSize = 14.0,
			Margin = new Thickness(5.0, 10.0, 5.0, 0.0)
		};
		Grid.SetColumn(element, 1);
		Grid.SetRow(element, 1);
		ButtonsPanelGrid.Children.Add(element);
		_printCountTextBlock = new TextBlock
		{
			Text = "1",
			FontSize = 14.0,
			Margin = new Thickness(80.0, 10.0, 5.0, 0.0)
		};
		Grid.SetColumn(_printCountTextBlock, 1);
		Grid.SetRow(_printCountTextBlock, 1);
		ButtonsPanelGrid.Children.Add(_printCountTextBlock);
	}

	private void AddButtonQtyPlus()
	{
		_qtyPlusButton = new CWCSimpleButton
		{
			Name = "QtyPlusButton",
			Height = 40.0,
			Margin = new Thickness(5.0, 10.0, 5.0, 0.0),
			UseLayoutRounding = true,
			Content = ArmSoft.SME.Trade.Common.Resources.QtyPlus
		};
		_qtyPlusButton.Click += QtyPlusButton_Click;
		Grid.SetColumn(_qtyPlusButton, 1);
		Grid.SetRow(_qtyPlusButton, 2);
		ButtonsPanelGrid.Children.Add(_qtyPlusButton);
	}

	private void AddButtonQtyMinus()
	{
		_qtyMinusButton = new CWCSimpleButton
		{
			Name = "QtyMinusButton",
			Height = 40.0,
			Margin = new Thickness(5.0, 10.0, 5.0, 0.0),
			UseLayoutRounding = true,
			Content = ArmSoft.SME.Trade.Common.Resources.QtyMinus
		};
		_qtyMinusButton.Click += QtyMinusButton_Click;
		Grid.SetColumn(_qtyMinusButton, 1);
		Grid.SetRow(_qtyMinusButton, 3);
		ButtonsPanelGrid.Children.Add(_qtyMinusButton);
	}

	private void QtyPlusButton_Click(object sender, RoutedEventArgs e)
	{
		_printCountTextBlock.Text = (Convert.ToInt16(_printCountTextBlock.Text) + 1).ToString();
	}

	private void QtyMinusButton_Click(object sender, RoutedEventArgs e)
	{
		short num = Convert.ToInt16(_printCountTextBlock.Text);
		if (num > 1)
		{
			_printCountTextBlock.Text = (num - 1).ToString();
		}
	}

	protected override void ProcessOK()
	{
		try
		{
			base.ProcessOK();
			ButtonClick?.Invoke(this, new SaleWindowHelper.PrintButtonClickEventArgs(SelectedRow.Name, Convert.ToInt16(_printCountTextBlock.Text)));
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
