using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.Documents.Base;
using ArmSoft.SME.Trade.UI.Documents.Goods;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.Windows;

public partial class PrintPrePaymentReturnTouchWindow : ThemedWindow, IComponentConnector
{
	private PartnerCacheRow _partnerCacheRow;

	private PartnerCacheRow.ContractRow _partnerContractRow;

	private decimal _amount;

	public CashIn BaseDocument;

	private BasePOSSaleWindow _owner;

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

	public PrintPrePaymentReturnTouchWindow(BasePOSSaleWindow saleWindow)
	{
		InitializeComponent();
		_owner = saleWindow;
		Owner = _owner;
		TE_Code.Focus();
		ASFontHelper.SetFontSettings(this);
	}

	private void PrintButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (BaseDocument == null || BaseDocument.ISN == Guid.Empty)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.BaseDocNeedsForECRC);
			}
			if (!TE_Code.Text.Trim().IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
			}
			_amount = Convert.ToDecimal(TE_Code.Text.Trim());
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

	private void CardButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			ReadMagneticCard(TE_Code.Text.Trim(), byPrefixSuffix: false);
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

	private void ExitButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void ReturnECRNumberButton_Click(object sender, RoutedEventArgs e)
	{
		Guid guid = Guid.Empty;
		try
		{
			if (string.IsNullOrWhiteSpace(TE_Code.Text))
			{
				return;
			}
			if (!TE_Code.Text.Trim().IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
			}
			if (TE_Code.Text.Length < 8)
			{
				TE_Code.Text = new string('0', 8 - TE_Code.Text.Length) + TE_Code.Text;
			}
			if (_owner is SaleWindow saleWindow)
			{
				List<Guid> docISNByECRN = TradeSettings.CreateTradeClient().CommonServices.GetDocISNByECRN(TE_Code.Text.Trim(), Convert.ToDateTime(saleWindow.StatusBar.TB_Time.Text).Date.ToDateOnly(), DocumentTypeSE.CashIn.Value);
				if (docISNByECRN.Count == 1)
				{
					guid = docISNByECRN[0];
				}
				else
				{
					BaseECRCheckDocuments dropDownView = new BaseECRCheckDocuments(TE_Code.Text.Trim(), Convert.ToDateTime(saleWindow.StatusBar.TB_Time.Text).ToDateOnly(), DocumentTypeSE.CashIn.Value.ToString());
					TouchGridControl<BaseECRCheckDocumentsDataProvider.DataRow> touchGridControl = new TouchGridControl<BaseECRCheckDocumentsDataProvider.DataRow>(this, dropDownView, "BaseECRCheckDocumentsGrid");
					if (touchGridControl.ShowDialog().Value && touchGridControl.SelectedRow != null)
					{
						guid = touchGridControl.SelectedRow.ISN;
					}
				}
			}
			BaseDocument = (CashIn)Settings.DocumentService.Load(guid, withoutAccessCheck: true);
			TB_Base.Text = ArmSoft.SME.Trade.Common.Resources.PrePayBase + ":";
			TB_ReturnInfo1.Text = ArmSoft.SME.Trade.Common.Resources.PrePaymentECRN + " " + TE_Code.Text.Trim();
			TB_ReturnInfo2.Text = $"{ArmSoft.SME.Common.Resources.DateShort} {BaseDocument.DocDate.ToShortDateString()} {BaseDocument.Summ.Value.RoundAs(2)} {ArmSoft.SME.Common.Resources.DramWordSmallLetter}";
			TB_ReturnInfo1.Foreground = (Settings.IsDarkTheme ? Brushes.White : Brushes.Black);
			TB_ReturnInfo2.Foreground = (Settings.IsDarkTheme ? Brushes.White : Brushes.Black);
		}
		catch (Exception ex)
		{
			if (guid == Guid.Empty)
			{
				TB_Base.Text = "";
				TB_ReturnInfo1.Text = ArmSoft.SME.Trade.Common.Resources.FilECRNforPrePaymentReturn;
				TB_ReturnInfo2.Text = ArmSoft.SME.Trade.Common.Resources.number;
				TB_ReturnInfo1.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#fb736f" : "#dd0326");
				TB_ReturnInfo2.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#fb736f" : "#dd0326");
			}
			Settings.ErrorHandler(ex);
		}
		finally
		{
			TE_Code.Text = "";
			TE_Code.Focus();
		}
	}

	private void NumericTouchPadButton_Click(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.C)
		{
			RelaseCButton();
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

	private void ReadMagneticCard(string cardData, bool byPrefixSuffix = true)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(cardData))
			{
				return;
			}
			string cardNumber;
			if (!byPrefixSuffix)
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
			PartnerContractCacheRow = TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(cardNumber, out var partnerCacheRow);
			PartnerCacheRow = partnerCacheRow;
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

	private void RelaseCButton()
	{
		TE_Code.Focus();
		TE_Code.Text = "";
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
