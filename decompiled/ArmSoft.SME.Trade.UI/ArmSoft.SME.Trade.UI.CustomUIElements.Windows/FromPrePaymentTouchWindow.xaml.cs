using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.Windows;

public partial class FromPrePaymentTouchWindow : ThemedWindow, IComponentConnector
{
	private decimal _amount;

	private PartnerCacheRow _partnerCacheRow;

	private PartnerCacheRow.ContractRow _partnerContractRow;

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

	public FromPrePaymentTouchWindow(Window owner)
	{
		InitializeComponent();
		Owner = owner;
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
			}
			else
			{
				_amount = Convert.ToDecimal(text);
				DialogResult = true;
				Close();
			}
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
			PartnerCacheRow.ContractRow contractByDiscountCard = TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(cardNumber, out var partnerCacheRow);
			PartnerCacheRow = partnerCacheRow;
			PartnerContractCacheRow = contractByDiscountCard;
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
