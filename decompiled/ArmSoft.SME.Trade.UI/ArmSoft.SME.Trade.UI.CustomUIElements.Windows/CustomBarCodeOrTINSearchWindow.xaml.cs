using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.Enums;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.Windows;

public partial class CustomBarCodeOrTINSearchWindow : ThemedWindow, IComponentConnector
{
	private SearchModes _searchMode;

	public string SearchText { get; set; } = "";

	public CustomBarCodeOrTINSearchWindow(Window ownerWindow, SearchModes searchMode)
	{
		InitializeComponent();
		_searchMode = searchMode;
		Owner = ownerWindow;
		if (_searchMode == SearchModes.BarCode)
		{
			Title = ArmSoft.SME.Common.Resources.SearchByBarcode;
		}
		else if (_searchMode == SearchModes.TaxCode)
		{
			Title = ArmSoft.SME.Trade.Common.Resources.FindByTaxCode;
		}
		ASFontHelper.SetFontSettings(this);
	}

	private void OKButton_Click(object sender, RoutedEventArgs e)
	{
		SearchText = SearchTextEdit.Text.Trim();
		if (_searchMode == SearchModes.TaxCode)
		{
			if (!SearchText.IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
			}
			if (SearchText.Length != 8)
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldLengthMustBeEqualTo, ArmSoft.SME.Common.Resources.OptionsTaxCode, (short)8) + ": " + SearchText);
			}
		}
		DialogResult = true;
		Close();
	}

	private void BackButton_Click(object sender, RoutedEventArgs e)
	{
		if (SearchTextEdit.Text.Length > 0)
		{
			string text = SearchTextEdit.Text;
			SearchTextEdit.Text = "";
			SearchTextEdit.Focus();
			SearchTextEdit.Text = text.Left(text.Length - 1);
		}
	}

	private void SlashButton_Click(object sender, RoutedEventArgs e)
	{
		ForwardSymbolToSearchPanel("/");
	}

	private void NumericTouchPad_ButtonClick(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.C)
		{
			SearchTextEdit.Focus();
			SearchTextEdit.Text = "";
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
			SearchTextEdit.Focus();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CustomBarCodeOrTINSearchControl_KeyDown(object sender, KeyEventArgs e)
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
		string text = SearchTextEdit.Text;
		SearchTextEdit.Text = "";
		SearchTextEdit.Focus();
		SearchTextEdit.Text = text + value;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
