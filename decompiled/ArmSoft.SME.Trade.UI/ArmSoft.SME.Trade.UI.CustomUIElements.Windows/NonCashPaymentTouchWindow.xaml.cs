using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.CustomWindowControls;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.Windows;

public partial class NonCashPaymentTouchWindow : ThemedWindow, IComponentConnector
{
	private string _cashDesk = "";

	public ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow NonCashPaymentType { get; private set; }

	public short NonCashPaymentMethod { get; private set; }

	public string AcquaringContract { get; private set; }

	public string AcquaringPartner { get; private set; }

	public decimal Amount { get; private set; }

	public NonCashPaymentTouchWindow(Window windowOwner, bool isSaleDocument, decimal amount, short nonCashPaymentMethod, ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow nonCashPaymentTypeForSaleWindow, string bankAcquiringPartner, string bankAcquiringContract)
	{
		InitializeComponent();
		Owner = windowOwner;
		Amount = amount;
		_cashDesk = Settings.ParametersService.DefaultCash();
		NonCashPaymentType = nonCashPaymentTypeForSaleWindow;
		NonCashPaymentMethod = nonCashPaymentMethod;
		AcquaringContract = bankAcquiringContract;
		AcquaringPartner = bankAcquiringPartner;
		SumPopupCalcEdit.Text = amount.ToString();
		CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk);
		if (item != null)
		{
			CashCacheRow.NonCashInfo value = null;
			if (item.NonCashInfoDict.TryGetValue(5, out value))
			{
				POS1Caption.Text = value.Caption;
				value = null;
			}
			if (item.NonCashInfoDict.TryGetValue(6, out value))
			{
				POS2Caption.Text = value.Caption;
				value = null;
			}
			if (item.NonCashInfoDict.TryGetValue(7, out value))
			{
				POS3Caption.Text = value.Caption;
				value = null;
			}
			if (item.NonCashInfoDict.TryGetValue(8, out value))
			{
				POS4Caption.Text = value.Caption;
				value = null;
			}
		}
		PartnerCacheRow item2 = TradeSettings.TradeCacheService.Partner.GetItem(AcquaringPartner);
		string text = "";
		switch (NonCashPaymentType)
		{
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithECRPOS:
			text = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode("1");
			break;
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithIngenicoPOS:
			text = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode("2");
			break;
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithFeitianPOS:
			text = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode("4");
			break;
		}
		if (item2 != null)
		{
			POSTerminal.Text = ArmSoft.SME.Trade.Common.Resources.POSTerminal;
			AcquaringInfo.Text = item2.Caption;
			if (!string.IsNullOrWhiteSpace(text))
			{
				TextBlock acquaringInfo = AcquaringInfo;
				acquaringInfo.Text = acquaringInfo.Text + " (" + text + ")";
			}
		}
		else
		{
			AcquaringInfo.Text = "";
			POSTerminal.Text = "";
		}
		SumPopupCalcEdit.Focus();
		ASFontHelper.SetFontSettings(this);
	}

	private void ExitButton_Click(object sender, RoutedEventArgs e)
	{
		DialogResult = false;
	}

	private void POSTerminal_Click(object sender, RoutedEventArgs e)
	{
		OnPOSTerminalButtonPress(sender);
	}

	private void OKButton_Click(object sender, RoutedEventArgs e)
	{
		OnConfirmButtonPress();
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
		else
		{
			UIHelper.ForwardSymbolFromPadToTextEditPanel(SumPopupCalcEdit, ((SaleWindowHelper.ButtonNumericClickEventArgs)e).ButtonText, 2);
		}
	}

	private void POSTerminal_DoubleClick(object sender, MouseButtonEventArgs e)
	{
		OnPOSTerminalButtonPress(sender);
		OnConfirmButtonPress();
	}

	private void NonCashPaymentTouchWindow_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (HotKeys.Single.Escape.Matches(this, e))
			{
				DialogResult = false;
				e.Handled = true;
			}
			else if (HotKeys.Single.Enter.Matches(this, e))
			{
				if (NonCashPaymentType != ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.None)
				{
					ValidateAmount();
				}
				else
				{
					Amount = 0m;
				}
				e.Handled = true;
				DialogResult = true;
			}
			else if (HotKeys.WithModifier.Control_Enter.Matches(this, e))
			{
				e.Handled = true;
				OnConfirmButtonPress();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OnConfirmButtonPress()
	{
		if (!decimal.TryParse(SumPopupCalcEdit.Text.Trim(), out var result))
		{
			Amount = result;
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
		}
		Amount = result;
		if (Amount == 0m || NonCashPaymentType == ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.None)
		{
			NonCashPaymentType = ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.None;
			AcquaringContract = "";
			AcquaringPartner = "";
			Amount = 0m;
		}
		DialogResult = true;
	}

	private void OnPOSTerminalButtonPress(object sender)
	{
		CWCSimpleButton cWCSimpleButton = (CWCSimpleButton)sender;
		CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk);
		if (cWCSimpleButton != null && item != null)
		{
			string text = "0";
			string text2 = "";
			CashCacheRow.NonCashInfo value = null;
			switch (cWCSimpleButton.Name)
			{
			case "BankCardWithPOS1":
				item.NonCashInfoDict.TryGetValue(5, out value);
				break;
			case "BankCardWithPOS2":
				item.NonCashInfoDict.TryGetValue(6, out value);
				break;
			case "BankCardWithPOS3":
				item.NonCashInfoDict.TryGetValue(7, out value);
				break;
			case "BankCardWithPOS4":
				item.NonCashInfoDict.TryGetValue(8, out value);
				break;
			}
			if (value != null)
			{
				PartnerCacheRow item2 = TradeSettings.TradeCacheService.Partner.GetItem(value.Partner);
				if (item2 != null)
				{
					text2 = item2.Caption;
					AcquaringPartner = item2.PartCode;
					AcquaringContract = value.Contract;
					text = value.Device;
					NonCashPaymentMethod = value.ECRPaymentMethod;
				}
				else
				{
					AcquaringPartner = "";
					AcquaringContract = "";
				}
			}
			switch (text)
			{
			case "0":
				NonCashPaymentType = ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithNoPOS;
				break;
			case "1":
				NonCashPaymentType = ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithECRPOS;
				break;
			case "2":
				NonCashPaymentType = ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithIngenicoPOS;
				break;
			case "4":
				NonCashPaymentType = ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithFeitianPOS;
				break;
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				POSTerminal.Text = ArmSoft.SME.Trade.Common.Resources.POSTerminal;
				AcquaringInfo.Text = text2;
				if (text != "0")
				{
					TextBlock acquaringInfo = AcquaringInfo;
					acquaringInfo.Text = acquaringInfo.Text + " (" + POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode(text) + ")";
				}
			}
			else
			{
				AcquaringInfo.Text = "";
				POSTerminal.Text = "";
			}
		}
		ValidateAmount();
	}

	private void ReleaseBackButton()
	{
		if (SumPopupCalcEdit.Text.Length > 0)
		{
			string text = SumPopupCalcEdit.Text;
			SumPopupCalcEdit.Text = "";
			SumPopupCalcEdit.Focus();
			SumPopupCalcEdit.Text = text.Left(text.Length - 1);
		}
	}

	private void ReleaseCButton()
	{
		SumPopupCalcEdit.Focus();
		SumPopupCalcEdit.Text = "";
	}

	private void ValidateAmount()
	{
		try
		{
			if (!decimal.TryParse(SumPopupCalcEdit.Text.Trim(), out var result))
			{
				Amount = result;
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
			}
			Amount = result;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SetAcquiringInfo(ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow nonCashType)
	{
		CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk);
		CashCacheRow.NonCashInfo value = null;
		if (item != null)
		{
			item.NonCashInfoDict.TryGetValue(0, out value);
			if (value != null)
			{
				AcquaringContract = value.Contract;
				AcquaringPartner = value.Partner;
			}
		}
		if (value == null)
		{
			AcquaringContract = "";
			AcquaringPartner = "";
		}
		PartnerCacheRow item2 = TradeSettings.TradeCacheService.Partner.GetItem(AcquaringPartner);
		if (item2 != null)
		{
			AcquaringInfo.Text = item2.Caption;
		}
		else
		{
			AcquaringInfo.Text = "";
		}
		POSTerminal.Text = "";
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
