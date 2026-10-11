using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.Windows;

public partial class CustomSearchWindow : ThemedWindow, IComponentConnector
{
	public CustomSearchWindow(Window ownerWindow)
	{
		InitializeComponent();
		Owner = ownerWindow;
		ASFontHelper.SetFontSettings(this);
	}

	private void OKButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void BackButton_Click(object sender, RoutedEventArgs e)
	{
		if (SearchControl.SearchText.Length > 0)
		{
			string searchText = SearchControl.SearchText;
			SearchControl.SearchText = "";
			SearchControl.Focus();
			SearchControl.SearchText = searchText.Left(searchText.Length - 1);
		}
	}

	private void SlashButton_Click(object sender, RoutedEventArgs e)
	{
		ForwardSymbolToSearchPanel("/");
	}

	private void NumericTouchPadButton_Click(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.C)
		{
			SearchControl.Focus();
			SearchControl.SearchText = "";
		}
		else if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.Numeric)
		{
			ForwardSymbolToSearchPanel(((SaleWindowHelper.ButtonNumericClickEventArgs)e).ButtonText);
		}
	}

	private void CustomSearchControl_Activated(object sender, EventArgs e)
	{
		try
		{
			SearchControl.Focus();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CustomSearchControl_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (HotKeys.Single.Escape.Matches(this, e))
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

	private void ForwardSymbolToSearchPanel(string value)
	{
		string searchText = SearchControl.SearchText;
		SearchControl.SearchText = "";
		SearchControl.Focus();
		SearchControl.SearchText = searchText + value;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
