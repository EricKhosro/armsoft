using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.CustomWindowControls;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.Windows;

public partial class PrintPrePaymentTouchWindow : ThemedWindow, IComponentConnector
{
	private readonly string _cashDeskCode = "";

	private decimal _amount;

	private PartnerCacheRow _partnerCacheRow;

	private PartnerCacheRow.ContractRow _partnerContractRow;

	public string POSTerminalType { get; private set; } = "0";

	public string AcquaringContract { get; private set; }

	public string AcquaringPartner { get; private set; }

	public short NonCashPaymentMethod { get; private set; }

	public string PosTerminalID { get; set; }

	public string PosTerminalAddress { get; set; }

	public decimal Amount
	{
		get
		{
			return _amount;
		}
		set
		{
			_amount = value;
			TE_Code.Text = _amount.ToString();
			TE_Code.Focus();
		}
	}

	public PartnerCacheRow.ContractRow PartnerContractCacheRow
	{
		get
		{
			return _partnerContractRow;
		}
		set
		{
			_partnerContractRow = value;
			FillParnerCardInfo();
		}
	}

	public PartnerCacheRow PartnerCacheRow
	{
		get
		{
			return _partnerCacheRow;
		}
		set
		{
			_partnerCacheRow = value;
		}
	}

	public SaleWindowHelper.PrintMode Mode { get; set; }

	public PrintPrePaymentTouchWindow(Window owner)
	{
		InitializeComponent();
		Owner = owner;
		Mode = SaleWindowHelper.PrintMode.Cash;
		_cashDeskCode = Settings.ParametersService.DefaultCash();
		CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_cashDeskCode);
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
			}
		}
		TE_Code.Focus();
		ASFontHelper.SetFontSettings(this);
	}

	private void OKButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			string text = TE_Code.Text.Trim();
			if (!text.IsNumeric())
			{
				if (!MCRInfo.GetInstance().IsValidCard(text))
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
				}
				ReadMagneticCard(text);
				return;
			}
			if (Convert.ToDecimal(text) <= 0m)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.IncrementSumma, 0));
			}
			_amount = Convert.ToDecimal(text);
			DialogResult = true;
			Close();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			TE_Code.Text = "";
			TE_Code.Focus();
		}
	}

	private void ExitButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void CardButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			ReadMagneticCard(TE_Code.Text.Trim(), ByPrefixSuffix: false);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			TE_Code.Text = "";
			TE_Code.Focus();
		}
	}

	private void ClearCardButton_Click(object sender, RoutedEventArgs e)
	{
		PartnerContractCacheRow = null;
	}

	private void POSTerminalButton_Click(object sender, RoutedEventArgs e)
	{
		CWCSimpleButton cWCSimpleButton = (CWCSimpleButton)sender;
		CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_cashDeskCode);
		if (cWCSimpleButton == null || item == null)
		{
			return;
		}
		string text = "0";
		string text2 = "";
		CashCacheRow.NonCashInfo nonCashInfo = null;
		switch (cWCSimpleButton.Name)
		{
		case "BankCardWithPOS1":
			nonCashInfo = item.NonCashInfoDict[5];
			break;
		case "BankCardWithPOS2":
			nonCashInfo = item.NonCashInfoDict[6];
			break;
		case "BankCardWithPOS3":
			nonCashInfo = item.NonCashInfoDict[7];
			break;
		case "BankCardWithPOS4":
			nonCashInfo = item.NonCashInfoDict[8];
			break;
		}
		if (nonCashInfo != null)
		{
			PartnerCacheRow item2 = TradeSettings.TradeCacheService.Partner.GetItem(nonCashInfo.Partner);
			if (item2 != null)
			{
				text2 = item2.Caption;
				AcquaringPartner = item2.PartCode;
				AcquaringContract = nonCashInfo.Contract;
				text = nonCashInfo.Device;
				NonCashPaymentMethod = nonCashInfo.ECRPaymentMethod;
				PosTerminalID = nonCashInfo.POSTerminalID;
				PosTerminalAddress = nonCashInfo.POSTerminalAddress;
			}
			else
			{
				AcquaringPartner = "";
				AcquaringContract = "";
			}
		}
		switch (text ?? "")
		{
		case "0":
			POSTerminalType = "0";
			break;
		case "1":
			POSTerminalType = "1";
			break;
		case "2":
			POSTerminalType = "2";
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
			NonCashMode.IsChecked = true;
			CashMode.IsChecked = false;
		}
		else
		{
			AcquaringInfo.Text = "";
			POSTerminal.Text = "";
		}
	}

	private void NumericTouchPadButton_Click(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.C)
		{
			ReleaseCButton();
		}
		else if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.BackSpace)
		{
			ReleaseBackSpaceButton();
		}
		else if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.Numeric)
		{
			ForwardSymbolToSearchPanel(((SaleWindowHelper.ButtonNumericClickEventArgs)e).ButtonText);
		}
	}

	private void PrintPrePaymentTouchWindow_KeyDown(object sender, KeyEventArgs e)
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

	private void CashMode_Checked(object sender, RoutedEventArgs e)
	{
		Mode = SaleWindowHelper.PrintMode.Cash;
		if (AcquaringInfo != null)
		{
			AcquaringInfo.Text = "";
		}
		if (POSTerminal != null)
		{
			POSTerminal.Text = "";
		}
	}

	private void NonCashMode_Checked(object sender, RoutedEventArgs e)
	{
		Mode = SaleWindowHelper.PrintMode.ByCard;
	}

	private void ReleaseBackSpaceButton()
	{
		if (TE_Code.Text.Length > 0)
		{
			string text = TE_Code.Text;
			TE_Code.Text = "";
			TE_Code.Focus();
			TE_Code.Text = text.Left(text.Length - 1);
		}
	}

	private void ReleaseCButton()
	{
		TE_Code.Focus();
		TE_Code.Text = "";
	}

	private void ForwardSymbolToSearchPanel(string value)
	{
		string text = TE_Code.Text;
		TE_Code.Text = "";
		TE_Code.Focus();
		TE_Code.Text = text + value;
	}

	private void FillParnerCardInfo()
	{
		try
		{
			if (PartnerContractCacheRow == null || PartnerCacheRow == null)
			{
				TB_CardNumber.Text = "";
				TB_CardHolderName.Text = "";
				TB_PartSumma.Text = 0m.CurrencyFormat(16, 2);
				TB_PartSumma.Foreground = (Settings.IsDarkTheme ? Brushes.White : Brushes.Black);
				return;
			}
			TB_CardNumber.Text = PartnerContractCacheRow.DiscountCard.Trim();
			TB_CardHolderName.Text = PartnerCacheRow.Caption + " (" + PartnerContractCacheRow.Caption + ")";
			SummCurrSummNccPair currentPartnerRemByContract = TradeSettings.CreateTradeClient().CommonServices.GetCurrentPartnerRemByContract(PartnerCacheRow.Id, PartnerContractCacheRow.Contract, "");
			TB_PartSumma.Text = currentPartnerRemByContract.SummCurr.CurrencyFormat(16, 2);
			if (currentPartnerRemByContract.SummCurr < 0m)
			{
				TB_PartSumma.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#fb7376" : "#dd0326");
			}
			else
			{
				TB_PartSumma.Foreground = (Settings.IsDarkTheme ? Brushes.White : Brushes.Black);
			}
		}
		catch
		{
		}
	}

	private void ReadMagneticCard(string cardData, bool ByPrefixSuffix = true)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(cardData))
			{
				return;
			}
			string cardNumber;
			if (!ByPrefixSuffix)
			{
				cardNumber = cardData;
			}
			else
			{
				if (!MCRInfo.GetInstance().IsValidCard(cardData))
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.InvalidCardCode);
				}
				cardNumber = MCRInfo.GetInstance().GetCardNumber(cardData);
			}
			PartnerContractCacheRow = TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(cardNumber, out var _);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
