using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public partial class PoleDisplayWindow : ThemedWindow, IComponentConnector
{
	private int mGoodID = -1;

	private string qntUnit = "";

	public PoleDisplayWindow()
	{
		Loaded += new SafeEventHandler<RoutedEventArgs>(PoleDisplayWindow_Loaded).Handler;
		InitializeComponent();
		if (RedesignPoleDisplay())
		{
			SpecificDesignConfigs();
		}
	}

	private void SpecificDesignConfigs()
	{
		RowInfoLayoutItem.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
		txtBonus_lbl.Visibility = Visibility.Collapsed;
		txtRowCount.Visibility = Visibility.Collapsed;
		BuyerAndTotalLayout.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
		txtRowCount.Visibility = Visibility.Collapsed;
		txtHeader.FontSize = 72.0;
		txtBuyerName.FontSize = 70.0;
		txtAccumulatedBonus_lbl.FontSize = 70.0;
		txtBonusRemain_lbl.FontSize = 70.0;
		txtAccumulatedBonus.FontSize = 70.0;
		txtBonusRemain.FontSize = 70.0;
		RowInfoTextBlock.FontSize = 70.0;
		TotalCaptionTextBlock.FontSize = 60.0;
		DiscountCaptionTextBlock.FontSize = 60.0;
		CashCaptionTextBlock.FontSize = 60.0;
		NonCashCaptionTextBlock.FontSize = 60.0;
		UsedBonusCaptionTextBlock.FontSize = 60.0;
		FromPrepayCaptionTextBlock.FontSize = 60.0;
		CreditSaleCaptionTextBlock.FontSize = 60.0;
		ForPayCaptionTextBlock.FontSize = 60.0;
		TotalTextBlock.FontSize = 60.0;
		DiscountTextBlock.FontSize = 60.0;
		CashTextBlock.FontSize = 60.0;
		NonCashTextBlock.FontSize = 60.0;
		UsedBonusTextBlock.FontSize = 60.0;
		FromPrepayTextBlock.FontSize = 60.0;
		CreditSaleTextBlock.FontSize = 60.0;
		ForPayTextBlock.FontSize = 60.0;
	}

	public void ClearScreen(bool clearAll = true, string messageForPoleDispaly = "")
	{
		txtRowCount.Text = "";
		if (string.IsNullOrWhiteSpace(messageForPoleDispaly))
		{
			RowInfoTextBlock.Text = string.Empty;
		}
		else
		{
			RowInfoTextBlock.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#ff0000")));
			RowInfoTextBlock.Text = messageForPoleDispaly;
		}
		ClearTotalsInfo();
		if (clearAll)
		{
			ClearBuyerInfo();
		}
		imgGoods.Source = null;
		mGoodID = -1;
		qntUnit = "";
	}

	public void SetRowInfo(ISaleRow row = null)
	{
		try
		{
			if (row == null || string.IsNullOrWhiteSpace(row.ItemName))
			{
				RowInfoTextBlock.Text = "";
				imgGoods.Source = null;
				return;
			}
			decimal num = (row.Quantity * row.DiscountPrice).RoundAs(2);
			RowInfoTextBlock.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#212121")));
			RowInfoTextBlock.Text = row.ItemName.Trim() + Environment.NewLine;
			TextBlock rowInfoTextBlock = RowInfoTextBlock;
			string text = rowInfoTextBlock.Text;
			InlineArray4<object> buffer = default;
			buffer[0] = row.Quantity.CurrencyFormat(16, (short)((!(row.Quantity == decimal.Floor(row.Quantity))) ? 3 : 0));
			buffer[1] = row.UnitBrief.Trim();
			buffer[2] = row.DiscountPrice.CurrencyFormat(16, (short)((!(row.DiscountPrice == decimal.Floor(row.DiscountPrice))) ? 2 : 0));
			buffer[3] = num.CurrencyFormat(16, (short)((!(num == decimal.Floor(num))) ? 2 : 0));
			rowInfoTextBlock.Text = text + string.Format("{0} {1} x {2} = {3}", (ReadOnlySpan<object?>)buffer);
			if (PoleDisplayInfo.GetInstance().ShowImage && mGoodID != row.ItemID)
			{
				mGoodID = row.ItemID;
				qntUnit = row.UnitCode;
				imgGoods.Source = null;
				LoadImage();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public void SetTotalInfo(decimal total = 0m, decimal discountSum = 0m, decimal cashSum = 0m, decimal nonCashSum = 0m, decimal usedBonusSum = 0m, decimal fromPrePaySumm = 0m, decimal creditSaleSumm = 0m, decimal sumForPay = 0m)
	{
		if (total + discountSum == 0m)
		{
			ClearTotalsInfo();
		}
		string text = (total + discountSum).CurrencyFormat(16, 2);
		TotalTextBlock.Text = text;
		DiscountTextBlock.Text = discountSum.CurrencyFormat(16, 2);
		CashTextBlock.Text = cashSum.CurrencyFormat(16, 2);
		NonCashTextBlock.Text = nonCashSum.CurrencyFormat(16, 2);
		UsedBonusTextBlock.Text = usedBonusSum.CurrencyFormat(16, 2);
		FromPrepayTextBlock.Text = fromPrePaySumm.CurrencyFormat(16, 2);
		CreditSaleTextBlock.Text = creditSaleSumm.CurrencyFormat(16, 2);
		if (sumForPay <= 0m)
		{
			ForPayCaptionTextBlock.Text = ArmSoft.SME.Trade.Common.Resources.ForPayShort;
			ForPayTextBlock.Text = Math.Abs(sumForPay).CurrencyFormat(16, 2);
		}
		else
		{
			ForPayCaptionTextBlock.Text = ArmSoft.SME.Trade.Common.Resources.SummaChange;
			ForPayTextBlock.Text = sumForPay.CurrencyFormat(16, 2);
		}
	}

	public void SetBuyerInfo(string buyerCode, string buyerName, string contractCode, decimal currentAccumulatedBonus, decimal currentBonusRemain)
	{
		if (string.IsNullOrWhiteSpace(buyerCode))
		{
			ClearBuyerInfo();
			return;
		}
		txtBuyerName.Text = buyerName;
		if (!string.IsNullOrWhiteSpace(contractCode) && Settings.ParametersService.UseBonus())
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(buyerCode, contractCode);
			if (contract != null && contract.IsBonusCard)
			{
				txtBonus_lbl.Text = ArmSoft.SME.Trade.Common.Resources.BonusOnly;
				txtAccumulatedBonus_lbl.Text = ArmSoft.SME.Trade.Common.Resources.CurrentAccumulate;
				txtAccumulatedBonus.Text = currentAccumulatedBonus.CurrencyFormat(16, 2);
				txtBonusRemain_lbl.Text = ArmSoft.SME.Trade.Common.Resources.CurrentBonusRemain;
				txtBonusRemain.Text = currentBonusRemain.CurrencyFormat(16, 2);
			}
		}
	}

	public void SetRowCountInfo(int RowCount = 0)
	{
		if (RowCount != 0)
		{
			txtRowCount.Text = $"{ArmSoft.SME.Trade.Common.Resources.GoodsCount} {RowCount}";
		}
	}

	private void PoleDisplayWindow_Loaded(object sender, RoutedEventArgs e)
	{
		if (PoleDisplayInfo.GetInstance().ShowHeader)
		{
			txtHeader.Text = PoleDisplayInfo.GetInstance().HeaderText;
		}
		else
		{
			grpHeader.Visibility = Visibility.Collapsed;
		}
		if (!PoleDisplayInfo.GetInstance().ShowImage)
		{
			grpImage.Visibility = Visibility.Collapsed;
		}
		SetBackgroundImage();
		ShowToCertainDisplay();
	}

	private void ShowToCertainDisplay()
	{
		Screen[] allScreens = Screen.AllScreens;
		foreach (Screen screen in allScreens)
		{
			if (PoleDisplayInfo.GetInstance().Monitor.DisplayDevice.Contains(screen.DeviceName))
			{
				Left = (double)screen.Bounds.Location.X + ((double)screen.Bounds.Width - Width) / 2.0;
				Top = (double)screen.Bounds.Location.Y + ((double)screen.Bounds.Height - Height) / 2.0;
				WindowState = WindowState.Maximized;
			}
		}
	}

	private void LoadImage()
	{
		try
		{
			byte[] itemImage = Settings.ImagesService.GetItemImage(ImageItemTypeSE.Materials.Value, TradeSettings.TradeCacheService.Material.GetQntUnit(mGoodID, qntUnit)?.ImageGUID.ToString());
			if (itemImage != null && itemImage != null && itemImage.Length != 0)
			{
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.CreateOptions = BitmapCreateOptions.None;
				bitmapImage.CacheOption = BitmapCacheOption.Default;
				bitmapImage.StreamSource = new MemoryStream(itemImage);
				bitmapImage.EndInit();
				imgGoods.Source = bitmapImage;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SetBackgroundImage()
	{
		try
		{
			if (PoleDisplayInfo.GetInstance().ShowBackgroundImage && !string.IsNullOrWhiteSpace(PoleDisplayInfo.GetInstance().BackgroundImage) && File.Exists(PoleDisplayInfo.GetInstance().BackgroundImage))
			{
				ImageBrush background = new ImageBrush
				{
					ImageSource = (ImageSource)new ImageSourceConverter().ConvertFromString(PoleDisplayInfo.GetInstance().BackgroundImage),
					Stretch = Stretch.UniformToFill,
					TileMode = TileMode.None
				};
				Background = background;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SetBackground(Brush xBrush)
	{
		Background = xBrush;
	}

	private void SetFontColor(Brush xBrush)
	{
	}

	private void SetHeaderForeground(Brush xForeground)
	{
		txtHeader.Foreground = xForeground;
	}

	private void ClearTotalsInfo()
	{
		TotalTextBlock.Text = "";
		DiscountTextBlock.Text = "";
		CashTextBlock.Text = "";
		NonCashTextBlock.Text = "";
		UsedBonusTextBlock.Text = "";
		FromPrepayTextBlock.Text = "";
		CreditSaleTextBlock.Text = "";
		ForPayTextBlock.Text = "";
	}

	private void ClearBuyerInfo()
	{
		txtBuyerName.Text = "";
		txtBonus_lbl.Text = "";
		txtAccumulatedBonus_lbl.Text = "";
		txtAccumulatedBonus.Text = "";
		txtBonusRemain_lbl.Text = "";
		txtBonusRemain.Text = "";
	}

	private static bool RedesignPoleDisplay()
	{
		return RegistryHelper.ReadBoolFromRegistry("Redesign", "PoleDisplay");
	}
}
