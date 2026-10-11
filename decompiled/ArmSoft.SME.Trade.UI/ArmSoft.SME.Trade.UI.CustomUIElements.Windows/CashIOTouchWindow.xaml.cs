using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.Windows;

public partial class CashIOTouchWindow : ThemedWindow, IComponentConnector
{
	public decimal Amount { get; set; }

	public bool Print { get; set; } = true;

	public CashIOTouchWindow(Window owner)
	{
		InitializeComponent();
		Owner = owner;
		CashAmount.Focus();
		ASFontHelper.SetFontSettings(this);
	}

	private void ExitButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void PrintButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (!CashAmount.Text.Trim().IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
			}
			Amount = Convert.ToDecimal(CashAmount.Text.Trim());
			DialogResult = true;
			Close();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			CashAmount.Text = "";
			CashAmount.Focus();
		}
	}

	private void OKButton_Click(object sender, RoutedEventArgs e)
	{
		Print = false;
		PrintButton_Click(sender, e);
	}

	private void NumericTouchPadControlButton_Click(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.C)
		{
			ReleaseCButton();
		}
		else if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.BackSpace)
		{
			ReleaseBackButton();
		}
		else if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.Numeric)
		{
			ForwardSymbolToSearchPanel(((SaleWindowHelper.ButtonNumericClickEventArgs)e).ButtonText);
		}
	}

	private void ReleaseCButton()
	{
		CashAmount.Focus();
		CashAmount.Text = "";
	}

	private void ReleaseBackButton()
	{
		if (CashAmount.Text.Length > 0)
		{
			string text = CashAmount.Text;
			CashAmount.Text = "";
			CashAmount.Focus();
			CashAmount.Text = text.Left(text.Length - 1);
		}
	}

	private void ForwardSymbolToSearchPanel(string value)
	{
		string text = CashAmount.Text;
		CashAmount.Text = "";
		CashAmount.Focus();
		CashAmount.Text = text + value;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
