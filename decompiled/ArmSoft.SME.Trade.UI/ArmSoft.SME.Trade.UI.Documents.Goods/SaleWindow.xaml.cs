using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using Ardalis.SmartEnum;
using ArmSoft.Arcus2Client;
using ArmSoft.Cloud.Client.Discount;
using ArmSoft.ECRConnector;
using ArmSoft.PossoftPayClient;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Models;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.SmartEnums;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Data.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.CustomTypes;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.CustomUIElements.Windows;
using ArmSoft.SME.Trade.UI.DataView;
using ArmSoft.SME.Trade.UI.DataViewDataProvider.Offline;
using ArmSoft.SME.Trade.UI.Documents.Base;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Enums;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.Models;
using ArmSoft.SME.Trade.UI.OtherWindows;
using ArmSoft.SME.Trade.UI.Services;
using ArmSoft.SME.Trade.UI.SmartEnums;
using ArmSoft.SME.Trade.UI.Views;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.Documents.Goods;

public partial class SaleWindow : BasePOSSaleWindow, IScanable, IMagneticCardReadable, IComponentConnector, IStyleConnector
{
	private delegate void RowInfoChangedEventHandler(SaleRow saleRow);

	private delegate void DelegateSetRowInfoToInfoPanel(SaleRow row);

	private delegate void DelegateSetRowInfoToPoleDisplay(SaleRow row);

	private delegate void DelegateSetTotalInfoToPoleDisplay(decimal total, decimal discountSum, decimal cashSum, decimal nonCashSum, decimal usedBonusSum, decimal fromPrePaySumm, decimal creditSaleSumm, decimal sumForPay);

	private delegate void DelegateSetRowCountInfoToPoleDisplay(int rowCount);

	private delegate void DelegateSetBuyerInfoToPoleDisplay(string buyerCode, string buyerName, string contractCode, decimal currentAccumulatedBonus, decimal currentBonusRemain);

	private delegate void DelegateSetChangeInfoToPoleDisplay(decimal change);

	private delegate void DelegateClearInfoPanel();

	private delegate void DelegateClearPoleDisplay(bool clearAll, string messageForPoleDispaly);

	private readonly SaleTouchModeButtonsPadUserControl _saleTouchModeButtonsPadUserControl;

	private readonly SaleTouchModeWithFavoritesButtonsPadUserControl _saleTouchModeWithFavoritesButtonsPadUserControl;

	private readonly SaleTouchModeWithFavoritesAltModeButtonsPadUserControl _saleTouchModeWithFavoritesAltModeButtonsPadUserControl;

	private readonly FavoritesTouchPadControl _favoritesTouchPadControl;

	private string _posTerminalID = "";

	private string _posTerminalAddress = "";

	private decimal _partContractBonusPercentOrPoint;

	private decimal _currentBonusRemain;

	private bool _baseButtonsPad = true;

	private bool _isBonusCard;

	private decimal _addedBonusValue;

	private decimal _summaPayByBonusValue;

	private decimal _summaCreditValue;

	private decimal _summaPrePaymentValue;

	private SolidColorBrush _remainBackgroundColor = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(61, 68, 63)) : new SolidColorBrush(Color.FromRgb(222, 240, 224)));

	private decimal _calculatedBonusValue;

	private decimal _summaCashSubtractBonus;

	private decimal _summaPlCardSubtractBonus;

	private decimal _summaPrePaymentSubtractBonus;

	public string HelpID => "POS_Sale";

	private event RowInfoChangedEventHandler RowInfoChangedEvent;

	public SaleWindow(Window owner, string touchMode, bool fullScreen, double width, double height)
	{
		//IL_1274: Unknown result type (might be due to invalid IL or missing references)
		//IL_127e: Expected Obj, but got Unknown
		//IL_1608: Unknown result type (might be due to invalid IL or missing references)
		//IL_1612: Expected Obj, but got Unknown
		if (Settings.IsOffline)
		{
			TradeSettings.TradeCacheService.Material.Refresh();
			if (!TradeSettings.TradeCacheService.CashierShift.GetList((CashierShiftCacheRow p) => p.CashDesk == _defaultCash).Any())
			{
				Dictionary<short, string> dictionary = new Dictionary<short, string>();
				if (!Settings.ParametersService.AllowPOSRecordReturnFromCustomer() && (Settings.UserAccessService.GetDocumentPermission(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value).AllowAdd || Settings.UserAccessService.GetDocumentPermission(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value).AllowAdd))
				{
					dictionary.Add(ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSRecordReturnFromCustomer.Id, true.ToString());
				}
				if (!Settings.ParametersService.AllowPOSDeleteRow())
				{
					dictionary.Add(ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSDeleteRow.Id, true.ToString());
				}
				if (!Settings.ParametersService.AllowPOSDecreaseQuantity() && Settings.ParametersService.AllowChangeMTBillGoodRow())
				{
					dictionary.Add(ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSDecreaseQuantity.Id, true.ToString());
				}
				if (!Settings.ParametersService.AllowPOSInputFromKeyboard())
				{
					dictionary.Add(ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSInputFromKeyboard.Id, true.ToString());
				}
				if (!Settings.ParametersService.AllowPOSRemoveCard())
				{
					dictionary.Add(ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSRemoveCard.Id, true.ToString());
				}
				if (!Settings.ParametersService.AllowPOSChooseBuyer())
				{
					dictionary.Add(ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSChooseBuyer.Id, true.ToString());
				}
				if (!Settings.ParametersService.AllowPrintMTCheck())
				{
					dictionary.Add(ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPrintMTCheck.Id, true.ToString());
				}
				bool flag = Settings.ParametersService.AllowPOSChangePrice();
				bool flag2 = Settings.ParametersService.DiscountUse();
				bool flag3 = Settings.ParametersService.AllowChangeSalePrices();
				bool flag4 = Settings.ParametersService.AllowChangeDiscountPrice();
				bool flag5 = Settings.ParametersService.AlwaysCalcDiscountAuto() || Settings.ParametersService.CalcDiscountAuto();
				if (!flag && ((!flag2 & flag3) || ((flag2 & flag4) && !flag5)))
				{
					dictionary.Add(ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSChangePrice.Id, true.ToString());
				}
				if (dictionary.Count != 0)
				{
					ApiParametersService.StoreValuesInLocalDB(dictionary);
					Settings.ParametersService.Refresh();
				}
			}
		}
		else
		{
			Settings.CommonCoreHelper.CheckWorkingPeriod(DateTime.Today.Date.ToDateOnly());
		}
		if (string.IsNullOrWhiteSpace(Settings.ParametersService.DefaultStorage()))
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Storage));
		}
		if (string.IsNullOrWhiteSpace(Settings.ParametersService.DefaultCash()))
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Cash));
		}
		InitializeComponent();
		Owner = owner;
		TouchMode = touchMode;
		FullScreen = fullScreen;
		if (FullScreen)
		{
			Left = 0.0;
			Top = 0.0;
			WindowStartupLocation = WindowStartupLocation.Manual;
			WindowState = WindowState.Maximized;
		}
		else
		{
			Width = width;
			Height = height;
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
		}
		StatusBar.SetTurnedOnb2barmsoftDiscountServerIsVisibility(_connectTob2barmsoftDiscountServer);
		CodeTextEdit.Background = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(73, 80, 75)) : Settings.RequiredBrush);
		WindowStyle = WindowStyle.None;
		if (TouchMode == POSScreenModesSE.TouchScreen.Value)
		{
			SaleHeader.CloseTitleButton.Visibility = Visibility.Collapsed;
			MainGrid.ColumnDefinitions[3].Width = new GridLength(300.0);
			_saleTouchModeButtonsPadUserControl = new SaleTouchModeButtonsPadUserControl();
			Grid.SetRow(_saleTouchModeButtonsPadUserControl, 3);
			Grid.SetRowSpan(_saleTouchModeButtonsPadUserControl, 3);
			Grid.SetColumn(_saleTouchModeButtonsPadUserControl, 3);
			Grid.SetColumnSpan(TotalInfo, 3);
			MainGrid.RowDefinitions[5].Height = new GridLength(160.0);
			TotalInfo.Totals.RowDefinitions[7].Height = new GridLength(0.0);
			_saleTouchModeButtonsPadUserControl.Margin = new Thickness(0.0, 0.0, 12.0, 0.0);
			_saleTouchModeButtonsPadUserControl.VerticalAlignment = VerticalAlignment.Top;
			MainGrid.Children.Add(_saleTouchModeButtonsPadUserControl);
			BarCode.Visible = false;
			InnerView.ShowVerticalLines = false;
			InnerView.RowMinHeight = 35.0;
			BE_TouchPadChange.Visibility = Visibility.Collapsed;
		}
		else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
		{
			SaleHeader.CloseTitleButton.Visibility = Visibility.Visible;
			double num = ((!fullScreen) ? width : ((double)Screen.PrimaryScreen.Bounds.Width));
			Grid.SetColumn(GridControl, 0);
			Grid.SetColumnSpan(GridControl, 3);
			GridControl.Margin = new Thickness(GridControl.Margin.Left, 10.0, GridControl.Margin.Right, GridControl.Margin.Bottom);
			Grid.SetRow(BonusInfo, 4);
			Grid.SetColumn(BonusInfo, 0);
			BonusInfo.Margin = new Thickness(12.0, 5.0, 0.0, -50.0);
			Grid.SetRow(CodeTextEdit, 4);
			Grid.SetColumn(CodeTextEdit, 1);
			CodeTextEdit.Height = 40.0;
			CodeTextEdit.Margin = new Thickness(6.0, 8.0, 6.0, 10.0);
			BE_TouchPadChange.MinWidth = 70.0;
			BE_TouchPadChange.Height = 40.0;
			BE_TouchPadChange.Margin = new Thickness(-4.0, 8.0, 8.0, 2.0);
			Grid.SetRow(BE_TouchPadChange, 4);
			Grid.SetColumn(BE_TouchPadChange, 2);
			if (num > 1680.0)
			{
				TotalInfo.Totals.ColumnDefinitions[0].Width = GridLength.Auto;
				TotalInfo.Totals.ColumnDefinitions[2].Width = new GridLength(170.0);
				Grid.SetRow(TotalInfo.TB_Total, 6);
				TotalInfo.TB_Total.Margin = new Thickness(0.0, 0.0, 20.0, 0.0);
				TotalInfo.Totals.RowDefinitions[7].Height = new GridLength(0.0);
				Grid.SetColumn(TotalInfo.TB_TotalCaption, 3);
				Grid.SetRow(TotalInfo.TB_TotalCaption, 2);
				TotalInfo.TB_TotalCaption.Visibility = Visibility.Visible;
			}
			else
			{
				TotalInfo.Totals.ColumnDefinitions[0].Width = new GridLength(150.0);
				TotalInfo.Totals.ColumnDefinitions[1].Width = new GridLength(0.0);
				TotalInfo.Totals.ColumnDefinitions[2].Width = GridLength.Auto;
				TotalInfo.Totals.RowDefinitions[7].Height = new GridLength(50.0);
				Grid.SetColumnSpan(TotalInfo.TB_Discount, 2);
				Grid.SetColumn(TotalInfo.TB_Discount, 2);
				Grid.SetColumnSpan(TotalInfo.TB_Cash, 2);
				Grid.SetColumn(TotalInfo.TB_Cash, 2);
				Grid.SetColumnSpan(TotalInfo.TB_NoCash, 2);
				Grid.SetColumn(TotalInfo.TB_NoCash, 2);
				Grid.SetColumnSpan(TotalInfo.TB_CreditSale, 2);
				Grid.SetColumn(TotalInfo.TB_CreditSale, 2);
				Grid.SetColumnSpan(TotalInfo.TB_MustPay, 2);
				Grid.SetColumn(TotalInfo.TB_MustPay, 2);
				Grid.SetColumnSpan(TotalInfo.TB_FromPrePay, 2);
				Grid.SetColumn(TotalInfo.TB_FromPrePay, 2);
				Grid.SetColumnSpan(TotalInfo.TB_FromBonus, 2);
				Grid.SetColumn(TotalInfo.TB_FromBonus, 2);
				Grid.SetColumn(TotalInfo.TB_Total, 0);
				Grid.SetColumnSpan(TotalInfo.TB_Total, 4);
				Grid.SetRow(TotalInfo.TB_Total, 7);
				TotalInfo.TB_Total.Margin = new Thickness(0.0, 0.0, 7.0, 0.0);
				TotalInfo.TB_TotalCaption.Visibility = Visibility.Collapsed;
			}
			Grid.SetColumn(TotalInfo, 0);
			Grid.SetRow(TotalInfo, 5);
			TotalInfo.Margin = new Thickness(10.0, 54.0, 0.0, 10.0);
			TotalInfo.TB_Total.VerticalAlignment = VerticalAlignment.Center;
			TotalInfo.TB_Total.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
			TotalInfo.TB_Discount.Margin = new Thickness(0.0, 0.0, 7.0, 0.0);
			TotalInfo.DiscountCaptionStack.Margin = new Thickness(7.0, 0.0, 0.0, 0.0);
			TotalInfo.TB_CashCaption.Margin = new Thickness(7.0, 0.0, 0.0, 0.0);
			TotalInfo.TB_NoCashCaption.Margin = new Thickness(7.0, 0.0, 0.0, 0.0);
			TotalInfo.TB_CreditSaleCaption.Margin = new Thickness(7.0, 0.0, 0.0, 0.0);
			TotalInfo.TB_FromPrepayCaption.Margin = new Thickness(7.0, 0.0, 0.0, 0.0);
			TotalInfo.TB_FromBonusCaption.Margin = new Thickness(7.0, 0.0, 0.0, 0.0);
			TotalInfo.TB_MustPayCaption.Margin = new Thickness(7.0, 0.0, 0.0, 0.0);
			TotalInfo.TB_Cash.Margin = new Thickness(0.0, 0.0, 7.0, 0.0);
			TotalInfo.TB_NoCash.Margin = new Thickness(0.0, 0.0, 7.0, 0.0);
			TotalInfo.TB_CreditSale.Margin = new Thickness(0.0, 0.0, 7.0, 0.0);
			TotalInfo.TB_FromPrePay.Margin = new Thickness(0.0, 0.0, 7.0, 0.0);
			TotalInfo.TB_FromBonus.Margin = new Thickness(0.0, 0.0, 7.0, 0.0);
			TotalInfo.TB_MustPay.Margin = new Thickness(0.0, 0.0, 7.0, 0.0);
			_saleTouchModeWithFavoritesButtonsPadUserControl = new SaleTouchModeWithFavoritesButtonsPadUserControl();
			Grid.SetRow(_saleTouchModeWithFavoritesButtonsPadUserControl, 5);
			Grid.SetColumn(_saleTouchModeWithFavoritesButtonsPadUserControl, 1);
			Grid.SetColumnSpan(_saleTouchModeWithFavoritesButtonsPadUserControl, 2);
			_saleTouchModeWithFavoritesButtonsPadUserControl.Margin = new Thickness(6.0, 0.0, 8.0, 12.0);
			MainGrid.Children.Add(_saleTouchModeWithFavoritesButtonsPadUserControl);
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl = new SaleTouchModeWithFavoritesAltModeButtonsPadUserControl();
			Grid.SetRow(_saleTouchModeWithFavoritesAltModeButtonsPadUserControl, 5);
			Grid.SetColumn(_saleTouchModeWithFavoritesAltModeButtonsPadUserControl, 1);
			Grid.SetColumnSpan(_saleTouchModeWithFavoritesAltModeButtonsPadUserControl, 2);
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.Margin = new Thickness(6.0, 0.0, 8.0, 12.0);
			MainGrid.Children.Add(_saleTouchModeWithFavoritesAltModeButtonsPadUserControl);
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.Visibility = Visibility.Collapsed;
			_favoritesTouchPadControl = new FavoritesTouchPadControl(num, AddFromFavorites, _priceListType, _vatType, DateTime.Now.Date);
			Grid.SetColumn(_favoritesTouchPadControl, 3);
			Grid.SetRow(_favoritesTouchPadControl, 1);
			Grid.SetRowSpan(_favoritesTouchPadControl, 5);
			_favoritesTouchPadControl.Margin = new Thickness(5.0, 25.0, 7.0, 15.0);
			MainGrid.Children.Add(_favoritesTouchPadControl);
			BarCode.Visible = false;
			ItemCode.Visible = false;
			AccumulatedBonus.Visible = false;
			InnerView.ShowVerticalLines = false;
			InnerView.RowMinHeight = 35.0;
			if (num < 1024.0)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.TouchScreenWithFavoritesMinSizeMustBe1024);
			}
			if (num >= 1024.0 && num < 1280.0)
			{
				MainGrid.ColumnDefinitions[3].Width = new GridLength(350.0);
			}
			else if (Screen.PrimaryScreen.Bounds.Width >= 1280 && num < 1600.0)
			{
				MainGrid.ColumnDefinitions[3].Width = new GridLength(600.0);
			}
			else
			{
				MainGrid.ColumnDefinitions[3].Width = new GridLength(800.0);
			}
		}
		foreach (FormatConditionBase item6 in ConditionalFormattingHelper.ConvertConditions(CreateFormatingList()))
		{
			InnerView.AddFormatCondition(item6);
		}
		MaterialInfo.BE_Discount.Visibility = Visibility.Collapsed;
		MaterialInfo.BE_QtyMinus.Visibility = Visibility.Collapsed;
		MaterialInfo.BE_QtyPlus.Visibility = Visibility.Collapsed;
		_docType = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill;
		string text = Settings.ParametersService.VATTypeRetail();
		switch (text)
		{
		default:
			if (text.Length != 0)
			{
				break;
			}
			goto case "1";
		case "3":
		case "4":
			_vatType = "3";
			break;
		case "1":
		case "2":
		case "5":
			_vatType = "5";
			break;
		case null:
			break;
		}
		if (!string.IsNullOrWhiteSpace(_defaultCash))
		{
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_defaultCash);
			if (item != null)
			{
				_ecrCRN = item.ECRCrn;
			}
		}
		SetNextDocumentNumber();
		StatusBar.TB_User.Text = Settings.LoginService.UserName;
		_currentShiftNum = CashierShiftHelper.GetOpenCashierShiftNumber();
		_shiftBeginDate = null;
		if (!string.IsNullOrWhiteSpace(_currentShiftNum))
		{
			_shiftBeginDate = CashierShiftHelper.GetOpenCashierShiftBeginDate(_currentShiftNum);
		}
		if (_shiftBeginDate.HasValue)
		{
			StatusBar.TB_Shift.Text = _currentShiftNum + ", " + ArmSoft.SME.Trade.Common.Resources.Start + ": " + _shiftBeginDate.Value.FormatDDMMYY() + " " + _shiftBeginDate.Value.ToLongTimeString();
		}
		else
		{
			StatusBar.TB_Shift.Text = _currentShiftNum + " : - : ";
		}
		SetStatusBar_SaleConsultant();
		StatusBar.RefreshTime();
		ModifySaleConsultantCardButtonText(_saleConsultantCode);
		_timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 50), (DispatcherPriority)9, (EventHandler)TimerEventHandler, ((DispatcherObject)this).Dispatcher);
		if (string.IsNullOrWhiteSpace(_currCode) || _currCode == "AMD")
		{
			_currCode = "AMD";
			_exchangeRate.Rate = 1m;
			_exchangeRate.Base = 1m;
		}
		else if (Settings.IsOffline)
		{
			CurrentExchangeRatesCacheRow item2 = Settings.CoreCacheService.CurrentExchangeRates.GetItem(_currCode);
			if (item2 != null)
			{
				_exchangeRate.Rate = item2.Rate;
				_exchangeRate.Base = item2.Base;
			}
			else
			{
				_exchangeRate.Rate = 0m;
				_exchangeRate.Base = 1m;
			}
		}
		else
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode, StatusBar.TB_Time.Text.ToDateOnly());
			if (exchangeRates != null)
			{
				_exchangeRate.Rate = exchangeRates.Rate;
				_exchangeRate.Base = exchangeRates.Base;
			}
		}
		_discountsCalculator = new DiscountsCalculator(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill);
		_discountsCalculator.ShowMessageOnDiscount += ShowMessageOnDiscountButtonClick;
		_discountsCalculator.AddGiftGoodsToDoc += AddGiftGoodsToDoc;
		_discountsCalculator.SelectAndAddGiftGoodsToDoc += SelectAndAddGiftGoodsToDoc;
		if (!string.IsNullOrWhiteSpace(_discountPriceListType))
		{
			PriceTypeCacheRow item3 = TradeSettings.TradeCacheService.PriceType.GetItem(_discountPriceTypeRoundDegree);
			if (item3 != null)
			{
				_discountPriceTypeRoundDegree = item3.Round;
			}
			ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_discountPriceListType, out _discountPriceListVATType, out _discountPriceListCurr);
			_discountPriceListExchangeRate.Rate = 1m;
			_discountPriceListExchangeRate.Base = 1m;
			if (_discountPriceListCurr != _currCode && _discountPriceListCurr != "AMD")
			{
				if (Settings.IsOffline)
				{
					CurrentExchangeRatesCacheRow item4 = Settings.CoreCacheService.CurrentExchangeRates.GetItem(_discountPriceListCurr);
					if (item4 != null)
					{
						_discountPriceListExchangeRate.Rate = item4.Rate;
						_discountPriceListExchangeRate.Base = item4.Base;
					}
				}
				else if (_discountPriceListCurr != _currCode && _discountPriceListCurr != "AMD")
				{
					ExchangeRate exchangeRates2 = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_discountPriceListCurr, StatusBar.TB_Time.Text.ToDateOnly());
					if (exchangeRates2 != null)
					{
						_discountPriceListExchangeRate.Rate = exchangeRates2.Rate;
						_discountPriceListExchangeRate.Base = exchangeRates2.Base;
					}
				}
			}
		}
		if (!string.IsNullOrWhiteSpace(_priceListType))
		{
			PriceTypeCacheRow item5 = TradeSettings.TradeCacheService.PriceType.GetItem(_priceListType);
			if (item5 != null)
			{
				_priceListTypeRoundDgr = item5.Round;
			}
		}
		_isFirstActivate = true;
		_materialsRowsList = new BindingList<SaleRow>();
		GridControl.ItemsSource = _materialsRowsList;
		InitPoleDisplay();
		KeyGestureConfigs();
		AddNotXamlEventHandlers();
		if (Settings.ParametersService.AutoSave())
		{
			_lastAutoSavedDocument = (TradeDocument)Settings.LocalDBDocumentService.GetLastAutoSavedDocument();
			if (_lastAutoSavedDocument != null)
			{
				ContinueAutoSavedSale();
			}
			_autoSaveTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, Settings.ParametersService.AutoSaveInterval(), 0), (DispatcherPriority)4, (EventHandler)AutoSaveEventHandler, ((DispatcherObject)this).Dispatcher);
		}
		ASFontHelper.SetFontSettings(this);
	}

	private void AddNotXamlEventHandlers()
	{
		RowInfoChangedEvent += OnRowInfoChanged;
		InnerView.FocusedRowHandleChanged += InnerView_FocusedRowHandleChanged;
		InnerView.GotFocus += InnerView_GotFocus;
		InnerView.MouseLeftButtonUp += InnerView_MouseLeftButtonUp;
		if (TouchMode == POSScreenModesSE.TouchScreen.Value)
		{
			_saleTouchModeButtonsPadUserControl.ButtonClick += SaleTouchButtonsPadUserControl_ButtonClick;
		}
		else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
		{
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonClick += SaleTouchButtonsPad1BaseUserControl_ButtonClick;
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.ButtonClick += SaleTouchButtonsPad1AltUserControl_ButtonClick;
		}
	}

	private List<AsFormatting> CreateFormatingList()
	{
		UserDefinedFormat customFormat = new UserDefinedFormat
		{
			IconUri = "gift_small_16.svg"
		};
		AsCondititionFormat item = new AsCondititionFormat
		{
			Expression = "[IsGift] = True",
			FieldName = "ItemCode",
			FormatName = "Custom",
			ApplyToRow = false,
			CustomFormat = customFormat,
			IsFixed = false
		};
		List<AsFormatting> list = new List<AsFormatting> { item };
		if (_useMarking)
		{
			customFormat = new UserDefinedFormat
			{
				IconUri = "datamatrix_blue_16.svg"
			};
			AsCondititionFormat item2 = new AsCondititionFormat
			{
				Expression = "[IsRowMarkable] = True",
				FieldName = "Quantity",
				FormatName = "Custom",
				ApplyToRow = false,
				CustomFormat = customFormat,
				IsFixed = true
			};
			list.Add(item2);
			customFormat = new UserDefinedFormat
			{
				IconUri = "datamatrix_red_16.svg"
			};
			item2 = new AsCondititionFormat
			{
				Expression = "[MarkingHasMismatch] = True",
				FieldName = "Quantity",
				FormatName = "Custom",
				ApplyToRow = false,
				CustomFormat = customFormat,
				IsFixed = true
			};
			list.Add(item2);
		}
		return list;
	}

	private void KeyGestureConfigs()
	{
		FillButtonCommandByKeyGesture();
		UpdateKeyGestures();
	}

	private void FillButtonCommandByKeyGesture()
	{
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillInputValueHotKey()] = SaleWindowHelper.ButtonCommands.FocusOnInputField;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillSummaCashHotKey()] = SaleWindowHelper.ButtonCommands.Cash;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillSalePriceHotKey()] = SaleWindowHelper.ButtonCommands.Price;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillBarCodeEnterHotKey()] = SaleWindowHelper.ButtonCommands.BarCode;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillCodeEnterHotKey()] = SaleWindowHelper.ButtonCommands.Code;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillMTListLikeNameHotKey()] = SaleWindowHelper.ButtonCommands.SelectMT;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillQtyEnterHotKey()] = SaleWindowHelper.ButtonCommands.QTY;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillPrintBillHotKey()] = SaleWindowHelper.ButtonCommands.PrintWithCount;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillPrintECRCheckHotKey()] = SaleWindowHelper.ButtonCommands.PrintCheck;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillPlCardPaymentHotKey()] = SaleWindowHelper.ButtonCommands.NotCash;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillUsePrePaymentHotKey()] = SaleWindowHelper.ButtonCommands.FromPrePay;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillUseBonusHotKey()] = SaleWindowHelper.ButtonCommands.UseBonus;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillAddToBonusHotKey()] = SaleWindowHelper.ButtonCommands.AddToBonus;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillFavoritesHotKey()] = SaleWindowHelper.ButtonCommands.MTFavorites;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillErasePaymentsHotKey()] = SaleWindowHelper.ButtonCommands.ClearPayment;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.POSContinueHotKey()] = SaleWindowHelper.ButtonCommands.Continue;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.POSDeferSaleHotKey()] = SaleWindowHelper.ButtonCommands.Defer;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.POSReturnHotKey()] = SaleWindowHelper.ButtonCommands.Return;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.POSPartnerCardHotKey()] = SaleWindowHelper.ButtonCommands.Card;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillCreditHotKey()] = SaleWindowHelper.ButtonCommands.CreditSale;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.UnitCodeSwitchHotKey()] = SaleWindowHelper.ButtonCommands.QtyUnit;
	}

	private void UpdateKeyGestures()
	{
		if (TouchMode == POSScreenModesSE.TouchScreen.Value)
		{
			_saleTouchModeButtonsPadUserControl.ButtonCashKeyGesture.Text = "(" + Settings.ParametersService.MTBillSummaCashHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonPriceKeyGesture.Text = "(" + Settings.ParametersService.MTBillSalePriceHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonFromPrePayKeyGesture.Text = "(" + Settings.ParametersService.MTBillUsePrePaymentHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonBarCodeKeyGesture.Text = "(" + Settings.ParametersService.MTBillBarCodeEnterHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonCodeKeyGesture.Text = "(" + Settings.ParametersService.MTBillCodeEnterHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonSelectMTKeyGesture.Text = "(" + Settings.ParametersService.MTBillMTListLikeNameHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonQTYKeyGesture.Text = "(" + Settings.ParametersService.MTBillQtyEnterHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.PrintButtonKeyGesture.Text = "(" + Settings.ParametersService.MTBillPrintBillHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.PrintECRCheckButtonKeyGesture.Text = "(" + Settings.ParametersService.MTBillPrintECRCheckHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonNoCashKeyGesture.Text = "(" + Settings.ParametersService.MTBillPlCardPaymentHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonUseBonusKeyGesture.Text = "(" + Settings.ParametersService.MTBillUseBonusHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonAddToBonusKeyGesture.Text = "(" + Settings.ParametersService.MTBillAddToBonusHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonMTFavoritesKeyGesture.Text = "(" + Settings.ParametersService.MTBillFavoritesHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonClearPaymentKeyGesture.Text = "(" + Settings.ParametersService.MTBillErasePaymentsHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonContinueKeyGesture.Text = "(" + Settings.ParametersService.POSContinueHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonDeferKeyGesture.Text = "(" + Settings.ParametersService.POSDeferSaleHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonReturnKeyGesture.Text = "(" + Settings.ParametersService.POSReturnHotKey() + ")";
			_saleTouchModeButtonsPadUserControl.ButtonCardKeyGesture.Text = "(" + Settings.ParametersService.POSPartnerCardHotKey() + ")";
		}
		else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
		{
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonQTYKeyGesture.Text = "(" + Settings.ParametersService.MTBillQtyEnterHotKey() + ")";
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCashKeyGesture.Text = "(" + Settings.ParametersService.MTBillSummaCashHotKey() + ")";
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonNameKeyGesture.Text = "(" + Settings.ParametersService.MTBillMTListLikeNameHotKey() + ")";
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonNoCashKeyGesture.Text = "(" + Settings.ParametersService.MTBillPlCardPaymentHotKey() + ")";
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardKeyGesture.Text = "(" + Settings.ParametersService.POSPartnerCardHotKey() + ")";
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonClearPaymentKeyGesture.Text = "(" + Settings.ParametersService.MTBillErasePaymentsHotKey() + ")";
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonReturnKeyGesture.Text = "(" + Settings.ParametersService.POSReturnHotKey() + ")";
			_saleTouchModeWithFavoritesButtonsPadUserControl.PrintECRCheckButtonKeyGesture.Text = "(" + Settings.ParametersService.MTBillPrintECRCheckHotKey() + ")";
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.ButtonCodeKeyGesture.Text = "(" + Settings.ParametersService.MTBillCodeEnterHotKey() + ")";
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.ButtonFromPrePayKeyGesture.Text = "(" + Settings.ParametersService.MTBillUsePrePaymentHotKey() + ")";
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.ButtonBarCodeKeyGesture.Text = "(" + Settings.ParametersService.MTBillBarCodeEnterHotKey() + ")";
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.ButtonCreditSaleHotKey.Text = "(" + Settings.ParametersService.MTBillCreditHotKey() + ")";
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.ButtonUseBonusKeyGesture.Text = "(" + Settings.ParametersService.MTBillUseBonusHotKey() + ")";
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.ButtonClearPaymentKeyGesture.Text = "(" + Settings.ParametersService.MTBillErasePaymentsHotKey() + ")";
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.ButtonAddToBonusKeyGesture.Text = "(" + Settings.ParametersService.MTBillAddToBonusHotKey() + ")";
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.PrintButtonKeyGesture.Text = "(" + Settings.ParametersService.MTBillPrintBillHotKey() + ")";
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.ButtonContinueKeyGesture.Text = "(" + Settings.ParametersService.POSContinueHotKey() + ")";
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.ButtonDeferKeyGesture.Text = "(" + Settings.ParametersService.POSDeferSaleHotKey() + ")";
		}
	}

	private void OnDeferButtonClick()
	{
		if (Settings.ParametersService.POSDeferSaleShowButton())
		{
			if (_materialsRowsList.Count == 0)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidateEmptyMtList);
			}
			InnerSave(new StoreRequest
			{
				State = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft,
				WithoutAccessCheck = true
			});
			TradeNotificationHelper.ShowTradeDefaultNotification(ArmSoft.SME.Trade.Common.Resources.Defer, string.Format(_docIsSale ? ArmSoft.SME.Trade.Common.Resources.MTBillDeferd : ArmSoft.SME.Trade.Common.Resources.MTRetFromCustomerDeferd, SaleHeader.TB_SaleNum.Text));
			NewAfterStore(isDeferClick: true);
		}
	}

	private void OnSaveSaleClick()
	{
		if (_saveAndPrintECR)
		{
			PrintCheck();
		}
		else
		{
			SaveSaleDocument();
		}
	}

	private void OnContinueSaleButtonClick()
	{
		if (!Settings.ParametersService.POSContinueShowButton() || !ValidateIsCashierShiftOpen())
		{
			return;
		}
		if (_materialsRowsList.Count > 0)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.BeforContinueStoreOrDefer);
		}
		StopAutoSaveTimer();
		DeleteAutoSavedDocument();
		DateOnly dateOnly = StatusBar.TB_Time.Text.ToDateOnly();
		short num = 0;
		_deferredISN = Guid.Empty;
		if (Settings.IsOffline)
		{
			LocalDBDocuments localDBDocuments = new LocalDBDocuments
			{
				StartDate = ((!_shiftBeginDate.HasValue) ? dateOnly : Convert.ToDateTime(_shiftBeginDate).ToDateOnly()),
				EndDate = dateOnly,
				CurrCode = "AMD",
				CashDeskCode = _defaultCash,
				CashierShiftNumber = _currentShiftNum,
				DocumentState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value,
				OrderByAscedning = false
			};
			TouchGridControlWithExtraButtons<LocalDBDocumentsDataProvider.DataRow> touchGridControlWithExtraButtons = new TouchGridControlWithExtraButtons<LocalDBDocumentsDataProvider.DataRow>(this, localDBDocuments, "DeferedDocsTouchDialog");
			if (TouchMode != POSScreenModesSE.Keyboard.Value)
			{
				if (touchGridControlWithExtraButtons.ShowDialog().Value && touchGridControlWithExtraButtons.SelectedRow != null)
				{
					_deferredISN = touchGridControlWithExtraButtons.SelectedRow.ISN;
					num = touchGridControlWithExtraButtons.SelectedRow.DocType;
				}
			}
			else
			{
				ModalBrowser modalBrowser = new ModalBrowser(localDBDocuments, showTree: false, isShowInTaskbar: false)
				{
					Owner = Owner,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};
				if (modalBrowser.Show())
				{
					LocalDBDocumentsDataProvider.DataRow dataRow = (LocalDBDocumentsDataProvider.DataRow)modalBrowser.DataView.Data[modalBrowser.SelectedRowIndex];
					if (dataRow != null)
					{
						num = dataRow.DocType;
						_deferredISN = dataRow.ISN;
					}
				}
			}
		}
		else
		{
			DocumentsJournal documentsJournal = new DocumentsJournal(this, ReportTypesSE.DeferredDocument, null)
			{
				StartDate = ((!_shiftBeginDate.HasValue) ? dateOnly : Convert.ToDateTime(_shiftBeginDate).ToDateOnly()),
				EndDate = dateOnly,
				CurrCode = "AMD",
				CashCode = _defaultCash,
				ShiftNumber = _currentShiftNum,
				DocState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.ValueUI
			};
			if (TouchMode != POSScreenModesSE.Keyboard.Value)
			{
				TouchGridControlWithExtraButtons<DocumentsJournalDataProvider.DataRow> touchGridControlWithExtraButtons2 = new TouchGridControlWithExtraButtons<DocumentsJournalDataProvider.DataRow>(this, documentsJournal, "DeferedDocsTouchDialog");
				if (touchGridControlWithExtraButtons2.ShowDialog().Value && touchGridControlWithExtraButtons2.SelectedRow != null)
				{
					_deferredISN = touchGridControlWithExtraButtons2.SelectedRow.ISN;
					num = touchGridControlWithExtraButtons2.SelectedRow.DocType;
				}
			}
			else
			{
				ModalBrowser modalBrowser2 = new ModalBrowser(documentsJournal, showTree: false, isShowInTaskbar: false)
				{
					Owner = Owner,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};
				if (modalBrowser2.Show())
				{
					DocumentsJournalDataProvider.DataRow dataRow2 = (DocumentsJournalDataProvider.DataRow)modalBrowser2.DataView.Data[modalBrowser2.SelectedRowIndex];
					if (dataRow2 != null)
					{
						num = dataRow2.DocType;
						_deferredISN = dataRow2.ISN;
					}
				}
			}
		}
		if (_deferredISN == Guid.Empty)
		{
			return;
		}
		_sumFromRound = 0m;
		if (num == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
		{
			if (!_docIsSale)
			{
				SaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.SaleNumber + " N ";
				SaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#4fad77")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#3E7337")));
				SaleHeader.TB_SaleNum.Foreground = SaleHeader.TB_SaleNumCaption.Foreground;
				Title = SaleHeader.TB_SaleNumCaption.Text + SaleHeader.TB_SaleNum.Text;
				if (TouchMode == POSScreenModesSE.TouchScreen.Value)
				{
					_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord;
				}
				else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
				{
					_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord;
				}
				TotalInfo.TB_FromBonusCaption.Text = ArmSoft.SME.Trade.Common.Resources.PaidByBonus;
				_docIsSale = true;
			}
			MTBill mTBill = ((!Settings.IsOffline) ? Settings.DocumentService.Load<MTBill>(_deferredISN, withoutAccessCheck: true) : Settings.LocalDBDocumentService.Load<MTBill>(_deferredISN));
			if (mTBill != null)
			{
				LoadMTBillDocument(mTBill);
				_ecrCheckNum = mTBill.ECRCheckNum;
				_prePaymentECRCheckNum = mTBill.PrePaymentECRCheckNum;
				_isDeferred = true;
			}
			AutoSaveDocumentInLocalFile(mTBill);
		}
		else
		{
			if (_docIsSale)
			{
				SaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord + " N ";
				SaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326")));
				SaleHeader.TB_SaleNum.Foreground = SaleHeader.TB_SaleNumCaption.Foreground;
				Title = SaleHeader.TB_SaleNumCaption.Text + SaleHeader.TB_SaleNum.Text;
				if (TouchMode == POSScreenModesSE.TouchScreen.Value)
				{
					_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.Sale;
				}
				else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
				{
					_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.Sale;
				}
				TotalInfo.TB_FromBonusCaption.Text = ArmSoft.SME.Trade.Common.Resources.BonusRestore;
				_docIsSale = false;
			}
			MTReturnFromCustomerBase mTReturnFromCustomerBase = ((!Settings.IsOffline) ? ((MTReturnFromCustomerBase)Settings.DocumentService.Load(_deferredISN, withoutAccessCheck: true)) : ((MTReturnFromCustomerBase)Settings.LocalDBDocumentService.Load(_deferredISN)));
			if (mTReturnFromCustomerBase != null)
			{
				TotalInfo.TotalFill.Fill = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#49504b")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#ffc7ce")));
				_ecrCheckNum = mTReturnFromCustomerBase.ECRCheckNum;
				LoadMTReturnFromCustomerDocument(mTReturnFromCustomerBase);
				_isDeferred = true;
				if (SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>.TryFromValue(mTReturnFromCustomerBase.POType, out var result))
				{
					_docType = result;
				}
			}
			AutoSaveDocumentInLocalFile(mTReturnFromCustomerBase);
		}
		if (!string.IsNullOrWhiteSpace(BonusInfo.TB_CardNumber.Text))
		{
			ModifyPartnerCardButtonText(BonusInfo.TB_CardNumber.Text);
			ModifySaleConsultantCardButtonText(_saleConsultantCode);
		}
		if (_materialsRowsList.Count > 0)
		{
			GridControl gridControl = GridControl;
			BindingList<SaleRow> materialsRowsList = _materialsRowsList;
			gridControl.CurrentItem = materialsRowsList[materialsRowsList.Count - 1];
		}
		UpdateChangeOrPaySum();
		SetPaymentsVisibility();
		StartAutoSaveTimer();
	}

	private void AutoSaveDocumentInLocalFile(TradeDocument tradeDocument)
	{
		if (Settings.ParametersService.AutoSave())
		{
			_lastAutoSavedDocument = tradeDocument;
		}
	}

	private void ContinueAutoSavedSale()
	{
		if (!ValidateIsCashierShiftOpen())
		{
			return;
		}
		_sumFromRound = 0m;
		if (_lastAutoSavedDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
		{
			if (!_docIsSale)
			{
				SaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.SaleNumber + " N ";
				SaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#4fad77")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#3E7337")));
				SaleHeader.TB_SaleNum.Foreground = SaleHeader.TB_SaleNumCaption.Foreground;
				Title = SaleHeader.TB_SaleNumCaption.Text + SaleHeader.TB_SaleNum.Text;
				if (TouchMode == POSScreenModesSE.TouchScreen.Value)
				{
					_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord;
				}
				else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
				{
					_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord;
				}
				TotalInfo.TB_FromBonusCaption.Text = ArmSoft.SME.Trade.Common.Resources.PaidByBonus;
				_docIsSale = true;
			}
			MTBill mTBill = (MTBill)_lastAutoSavedDocument;
			if (mTBill != null)
			{
				LoadMTBillDocument(mTBill);
				_ecrCheckNum = mTBill.ECRCheckNum;
				_prePaymentECRCheckNum = mTBill.PrePaymentECRCheckNum;
				_isDeferred = true;
			}
		}
		else
		{
			if (_docIsSale)
			{
				SaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord + " N ";
				SaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326")));
				SaleHeader.TB_SaleNum.Foreground = SaleHeader.TB_SaleNumCaption.Foreground;
				Title = SaleHeader.TB_SaleNumCaption.Text + SaleHeader.TB_SaleNum.Text;
				if (TouchMode == POSScreenModesSE.TouchScreen.Value)
				{
					_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.Sale;
				}
				else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
				{
					_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.Sale;
				}
				TotalInfo.TB_FromBonusCaption.Text = ArmSoft.SME.Trade.Common.Resources.BonusRestore;
				_docIsSale = false;
			}
			MTReturnFromCustomerBase mTReturnFromCustomerBase = (MTReturnFromCustomerBase)_lastAutoSavedDocument;
			if (mTReturnFromCustomerBase != null)
			{
				TotalInfo.TotalFill.Fill = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#49504b")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#ffc7ce")));
				_ecrCheckNum = mTReturnFromCustomerBase.ECRCheckNum;
				LoadMTReturnFromCustomerDocument(mTReturnFromCustomerBase);
				_isDeferred = true;
			}
		}
		if (!string.IsNullOrWhiteSpace(BonusInfo.TB_CardNumber.Text))
		{
			ModifyPartnerCardButtonText(BonusInfo.TB_CardNumber.Text);
			ModifySaleConsultantCardButtonText(_saleConsultantCode);
		}
		if (_materialsRowsList.Count > 0)
		{
			GridControl gridControl = GridControl;
			BindingList<SaleRow> materialsRowsList = _materialsRowsList;
			gridControl.CurrentItem = materialsRowsList[materialsRowsList.Count - 1];
		}
		UpdateChangeOrPaySum();
		SetPaymentsVisibility();
	}

	private TradeDocument SaveSaleDocument(bool newAfterStore = true)
	{
		ValidateSaleFromUI();
		TradeDocument tradeDocument = InnerSave(new StoreRequest
		{
			WithoutAccessCheck = true
		});
		if ((tradeDocument != null) & newAfterStore)
		{
			NewAfterStore();
		}
		return tradeDocument;
	}

	private void ValidateSaleFromUI()
	{
		if (!_materialsRowsList.Any((SaleRow x) => !x.IsDeleted))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidateEmptyMtList);
		}
		if (!Settings.ParametersService.AllowUse100PercentSale() && Convert.ToDecimal(TotalInfo.TB_Total.Text) == 0m)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.UserCanNotSellWith100PercentDiscount);
		}
		if (GetPaidSum() < Convert.ToDecimal(TotalInfo.TB_Total.Text))
		{
			throw new ASException(ArmSoft.SME.Common.Resources.PaymentsSmallerThanTotal);
		}
		if (GetWithoutCashPaidSum() > Convert.ToDecimal(TotalInfo.TB_Total.Text))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.SummMustBeGreater);
		}
		if (Convert.ToDecimal(TotalInfo.TB_NoCash.Text) != 0m && string.IsNullOrWhiteSpace(_bankAcquiringPartner))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrBankAquaringPart);
		}
		if (Convert.ToDecimal(TotalInfo.TB_CreditSale.Text) != 0m && string.IsNullOrWhiteSpace(_partnerCode))
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Buyer));
		}
		if (!_docIsSale && _baseDocISN == Guid.Empty)
		{
			if (Convert.ToDecimal(TotalInfo.TB_NoCash.Text) > 0m)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ReturnByCardSumMustBeZero);
			}
			if (Convert.ToDecimal(TotalInfo.TB_FromPrePay.Text) > 0m)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ReturnByPrePaySumMustBeZero);
			}
		}
	}

	private void ValidateGrid()
	{
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			materialsRows.ValidateWithoutResponse();
			if (Convert.ToBoolean(materialsRows.HasValidationError))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ErrorInSave);
			}
		}
	}

	private void InnerAutoSave()
	{
		if (!Settings.ParametersService.AutoSave())
		{
			return;
		}
		if (_docIsSale)
		{
			if (_lastAutoSavedDocument == null)
			{
				_lastAutoSavedDocument = OfflineModeHelper.CreateOfflineModeMTBillDocument();
			}
			else if (_lastAutoSavedDocument.POType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
			{
				_lastAutoSavedDocument = OfflineModeHelper.CreateOfflineModeMTBillDocument();
			}
			FillMTBillDocumentCommonFields((MTBill)_lastAutoSavedDocument);
		}
		else if (_baseDocISN == Guid.Empty)
		{
			if (_lastAutoSavedDocument == null)
			{
				_lastAutoSavedDocument = OfflineModeHelper.CreateOfflineModeMTReturnFromCustomerDocument();
			}
			else if (_lastAutoSavedDocument.POType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value)
			{
				_lastAutoSavedDocument = OfflineModeHelper.CreateOfflineModeMTReturnFromCustomerDocument();
			}
			FillRetFromCustomerDocumentCommonFields((MTReturnFromCustomer)_lastAutoSavedDocument, ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value);
		}
		else
		{
			if (_lastAutoSavedDocument == null)
			{
				_lastAutoSavedDocument = OfflineModeHelper.CreateOfflineModeMTBillReturnDocument();
			}
			else if (_lastAutoSavedDocument.POType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value)
			{
				_lastAutoSavedDocument = OfflineModeHelper.CreateOfflineModeMTBillReturnDocument();
			}
			FillMTBillReturnDocumentCommonFields((MTBillReturn)_lastAutoSavedDocument);
		}
		Settings.LocalDBDocumentService.AutoStore(_lastAutoSavedDocument, new StoreRequest
		{
			DocNumOfferSource = _docNumOfferSource,
			WithoutAccessCheck = true,
			State = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft
		}, storedInDb: false, LocalDBDocumentSyncStateSE.AutoSaved);
	}

	private void DeleteAutoSavedDocument()
	{
		try
		{
			if (Settings.ParametersService.AutoSave())
			{
				_lastAutoSavedDocument = null;
				Settings.LocalDBDocumentService.DeleteAutoSavedDocuments();
			}
		}
		catch
		{
		}
	}

	private TradeDocument InnerSave(StoreRequest sr)
	{
		if (!ValidateIsCashierShiftOpen())
		{
			return null;
		}
		TradeDocument tradeDocument;
		if (_docIsSale)
		{
			tradeDocument = FillMTBillDocument();
		}
		else
		{
			tradeDocument = ((!(_baseDocISN == Guid.Empty)) ? ((MTReturnFromCustomerBase)FillMTBillReturnDocument()) : ((MTReturnFromCustomerBase)FillRetFromCustomerDocument(sr.State)));
		}
		if (BeforeStore(sr, tradeDocument.ISN, out var checkLimitAnswerYes))
		{
			ValidateGrid();
			if (_docIsSale)
			{
				((MTBill)tradeDocument).IsCheckLimitAnswerYes = checkLimitAnswerYes;
			}
			sr.DocNumOfferSource = _docNumOfferSource;
			if (Settings.IsOffline)
			{
				Settings.LocalDBDocumentService.Store(tradeDocument, sr, tradeDocument.StoredInDb, LocalDBDocumentSyncStateSE.NotSent);
			}
			else
			{
				if (tradeDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
				{
					tradeDocument = Settings.DocumentService.StoreAndLoad((MTBill)tradeDocument, sr);
				}
				else if (tradeDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value)
				{
					tradeDocument = Settings.DocumentService.StoreAndLoad((MTBillReturn)tradeDocument, sr);
				}
				else if (tradeDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value)
				{
					tradeDocument = Settings.DocumentService.StoreAndLoad((MTReturnFromCustomer)tradeDocument, sr);
				}
				PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(tradeDocument.PartnerID, _contract);
				if (tradeDocument.PartnerID != -1 && contract != null && contract.IsBonusCard && !string.IsNullOrWhiteSpace(contract.DiscountCard) && _connectTob2barmsoftDiscountServer && tradeDocument.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && tradeDocument.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value)
				{
					CardTransactionRequest cardTransactionRequest = new CardTransactionRequest
					{
						CardNumber = contract.DiscountCard,
						DocInfo = new CardTransactionRequestDocInfo
						{
							ISN = tradeDocument.ISN,
							Type = (_docIsSale ? DocType.Sale : DocType.Return),
							Date = tradeDocument.DocDate.ToDateTime(),
							Number = tradeDocument.DocNum,
							Amount = tradeDocument.Summ.Value,
							CashCode = tradeDocument.CashDesk,
							ShiftNumber = tradeDocument.CashierShiftNumber,
							User = Settings.LoginService.UserName
						},
						Rows = new List<CardTransactionRow>()
					};
					if (_docIsSale)
					{
						if (Convert.ToDecimal(BonusInfo.TB_AccumulatedBonus.Text) != 0m)
						{
							cardTransactionRequest.Rows.Add(new CardTransactionRow
							{
								Op = OpType.Accumulate,
								Amount = Convert.ToDecimal(BonusInfo.TB_AccumulatedBonus.Text)
							});
						}
						if (Convert.ToDecimal(BonusInfo.TB_AddedBonus.Text) != 0m)
						{
							cardTransactionRequest.Rows.Add(new CardTransactionRow
							{
								Op = OpType.Accumulate,
								Amount = Convert.ToDecimal(BonusInfo.TB_AddedBonus.Text)
							});
						}
						if (Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) != 0m)
						{
							cardTransactionRequest.Rows.Add(new CardTransactionRow
							{
								Op = OpType.Use,
								Amount = -Convert.ToDecimal(TotalInfo.TB_FromBonus.Text)
							});
						}
					}
					else
					{
						if (Convert.ToDecimal(BonusInfo.TB_AccumulatedBonus.Text) != 0m)
						{
							cardTransactionRequest.Rows.Add(new CardTransactionRow
							{
								Op = OpType.Accumulate,
								Amount = -Convert.ToDecimal(BonusInfo.TB_AccumulatedBonus.Text)
							});
						}
						if (Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) != 0m)
						{
							cardTransactionRequest.Rows.Add(new CardTransactionRow
							{
								Op = OpType.Use,
								Amount = Convert.ToDecimal(TotalInfo.TB_FromBonus.Text)
							});
						}
					}
					CardTransaction cardTransaction = DiscountServerService.RecordDiscountServerTransaction(cardTransactionRequest);
					if (cardTransaction != null)
					{
						tradeDocument.TransactionISN = cardTransaction.ISN;
						int num = 1;
						List<PropertyWithValue> list = new List<PropertyWithValue>(num);
						CollectionsMarshal.SetCount(list, num);
						CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("TransactionISN", tradeDocument.TransactionISN.ToString());
						List<PropertyWithValue> properties = list;
						sr.LogComment = $"{ArmSoft.SME.Trade.Common.Resources.RecordInDiscountServer} - {tradeDocument.TransactionISN}";
						tradeDocument.TS = Settings.DocumentService.EditFields(tradeDocument.ISN, properties, sr);
					}
				}
			}
			return tradeDocument;
		}
		return null;
	}

	private void NewAfterStore(bool isDeferClick = false)
	{
		StopAutoSaveTimer();
		DeleteAutoSavedDocument();
		_docIsSale = true;
		_docType = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill;
		_isDeferred = false;
		_priceListType = Settings.ParametersService.DefaultMTCheckPriceType();
		_priceListTypeRoundDgr = TradeSettings.TradeCacheService.PriceType.GetItem(_priceListType)?.Round ?? "";
		_temporaryDiscountSumm = 0m;
		SaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.SaleNumber + " ";
		SaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#4fad77")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#3E7337")));
		SaleHeader.TB_SaleNum.Foreground = SaleHeader.TB_SaleNumCaption.Foreground;
		SaleHeader.TB_BaseDoc.Text = "";
		if (!string.IsNullOrWhiteSpace(_defaultCash))
		{
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_defaultCash);
			if (item != null)
			{
				_ecrCRN = item.ECRCrn;
			}
			ClearNonCashPayment();
		}
		if (TouchMode == POSScreenModesSE.TouchScreen.Value)
		{
			_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord;
		}
		else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
		{
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord;
		}
		if (TouchMode == POSScreenModesSE.TouchScreen.Value)
		{
			_saleTouchModeButtonsPadUserControl.ButtonCardCaption.Text = ArmSoft.SME.Trade.Common.Resources.Buyers;
			_saleTouchModeButtonsPadUserControl.ButtonCardCaption.Foreground = _saleTouchModeButtonsPadUserControl.ButtonCash.Foreground;
			_saleTouchModeButtonsPadUserControl.ButtonCardCaption1.Text = ArmSoft.SME.Trade.Common.Resources.Card;
			_saleTouchModeButtonsPadUserControl.ButtonCardCaption1.Foreground = _saleTouchModeButtonsPadUserControl.ButtonCash.Foreground;
		}
		else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
		{
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardCaption.Text = ArmSoft.SME.Trade.Common.Resources.Buyers;
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardCaption.Foreground = _saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCash.Foreground;
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardCaption1.Text = ArmSoft.SME.Trade.Common.Resources.Card;
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardCaption1.Foreground = _saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCash.Foreground;
		}
		MaterialInfo.BE_Discount.Visibility = Visibility.Collapsed;
		MaterialInfo.BE_QtyMinus.Visibility = Visibility.Collapsed;
		MaterialInfo.BE_QtyPlus.Visibility = Visibility.Collapsed;
		CodeTextEdit.Text = "";
		_realText = "";
		SetNextDocumentNumber();
		string text = "";
		bool flag = _nonCashPaymentMethod.InList(5, 6, 7, 8, 9);
		if (!isDeferClick)
		{
			new CurrencyDataProvider.Param().Currency = _currCode;
			string unitName = Settings.CoreCacheService.Currency.GetItem(_currCode).UnitName;
			if (Convert.ToDecimal(TotalInfo.TB_Cash.Text) != 0m.RoundAs(2))
			{
				if ((Convert.ToDecimal(TotalInfo.TB_NoCash.Text) == 0m.RoundAs(2)) | flag)
				{
					if (Convert.ToDecimal(TotalInfo.TB_MustPay.Text) != 0m.RoundAs(2))
					{
						MaterialInfo.TB_GoodName.Text = $"{ArmSoft.SME.Trade.Common.Resources.ReturnBuyerChange} {TotalInfo.TB_MustPay.Text} {unitName}";
						InlineArray4<object> buffer = default;
						buffer[0] = Environment.NewLine;
						buffer[1] = ArmSoft.SME.Trade.Common.Resources.DontForgetYourChange;
						buffer[2] = TotalInfo.TB_MustPay.Text;
						buffer[3] = unitName;
						text = string.Format("{0} {1} {2} {3}", (ReadOnlySpan<object?>)buffer);
					}
				}
				else if (Convert.ToDecimal(TotalInfo.TB_NoCash.Text) != 0m.RoundAs(2) && !flag)
				{
					if (Convert.ToDecimal(TotalInfo.TB_MustPay.Text) == 0m.RoundAs(2))
					{
						MaterialInfo.TB_GoodName.Text = $"{ArmSoft.SME.Trade.Common.Resources.ReturnBuyerCard}";
						MaterialInfo.TB_GoodRowDetail.Text = " ";
						text = $"{Environment.NewLine} {ArmSoft.SME.Trade.Common.Resources.DontForgetYourCard}";
					}
					else
					{
						MaterialInfo.TB_GoodName.Text = $"{ArmSoft.SME.Trade.Common.Resources.ReturnBuyerChangeAndCard} {TotalInfo.TB_MustPay.Text} {unitName} ";
						InlineArray4<object> buffer2 = default;
						buffer2[0] = Environment.NewLine;
						buffer2[1] = ArmSoft.SME.Trade.Common.Resources.DontForgetYourChangeAndCard;
						buffer2[2] = TotalInfo.TB_MustPay.Text;
						buffer2[3] = unitName;
						text = string.Format("{0} {1} {2} {3} ", (ReadOnlySpan<object?>)buffer2);
					}
				}
			}
			else if (Convert.ToDecimal(TotalInfo.TB_NoCash.Text) != 0m.RoundAs(2) && !flag)
			{
				MaterialInfo.TB_GoodName.Text = $"{ArmSoft.SME.Trade.Common.Resources.ReturnBuyerCard}";
				MaterialInfo.TB_GoodRowDetail.Text = " ";
				text = $"{Environment.NewLine} {ArmSoft.SME.Trade.Common.Resources.DontForgetYourCard}";
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				MaterialInfo.TB_GoodName.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376")) : Brushes.Red);
			}
			else
			{
				MaterialInfo.TB_GoodName.Text = "";
				MaterialInfo.TB_GoodRowDetail.Text = "";
			}
		}
		else
		{
			MaterialInfo.TB_GoodName.Text = "";
			MaterialInfo.TB_GoodRowDetail.Text = "";
		}
		MaterialInfo.TB_GoodName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
		MaterialInfo.TB_GoodRowDetail.Text = "";
		MaterialInfo.TB_GoodCode.Text = "";
		MaterialInfo.TB_GoodRemInfo.Text = "";
		MaterialInfo.TB_GoodRemInfo.Background = MaterialInfo.TB_GoodName.Background;
		BonusInfo.TB_CardNumber.Text = "";
		BonusInfo.TB_CardHolderName.Text = "";
		BonusInfo.TB_Bonus.Text = 0m.CurrencyFormat(16, 2);
		BonusInfo.TB_AccumulatedBonus.Text = 0m.CurrencyFormat(16, 2);
		BonusInfo.TB_AddedBonus.Text = 0m.CurrencyFormat(16, 2);
		BonusInfo.TB_DiscountPercent.Text = 0m.CurrencyFormat(6, 2);
		TotalInfo.TB_Discount.Text = 0m.CurrencyFormat(16, 2);
		TotalInfo.TB_Total.Text = 0m.CurrencyFormat(16, 2);
		TotalInfo.TB_Cash.Text = 0m.CurrencyFormat(16, 2);
		TotalInfo.TB_MustPay.Text = 0m.CurrencyFormat(16, 2);
		TotalInfo.TB_NoCash.Text = 0m.CurrencyFormat(16, 2);
		TotalInfo.TB_FromPrePay.Text = 0m.CurrencyFormat(16, 2);
		TotalInfo.TB_CreditSale.Text = 0m.CurrencyFormat(16, 2);
		TotalInfo.TB_FromBonus.Text = 0m.CurrencyFormat(16, 2);
		TotalInfo.TB_FromBonusCaption.Text = ArmSoft.SME.Trade.Common.Resources.PaidByBonus;
		TotalInfo.TB_MustPayCaption.Text = ArmSoft.SME.Trade.Common.Resources.ForPay;
		TotalInfo.TB_MustPayCaption.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#fb7376" : "#dd0326");
		TotalInfo.TB_MustPay.Foreground = TotalInfo.TB_MustPayCaption.Foreground;
		TotalInfo.TotalFill.Fill = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#49504b")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#def0e0")));
		SetPaymentsVisibility();
		StatusBar.TB_QTY.Text = 0m.CurrencyFormat(13, 3);
		_sumFromRound = 0m;
		_partnerTaxCode = null;
		_ecrCheckNum = "";
		_prePaymentECRCheckNum = "";
		_contract = "";
		_partnerID = -1;
		_partnerCode = "";
		_prepaymentPartnerCode = "";
		_prepaymentContractCode = "";
		_isBonusCard = false;
		_partContractBonusPercentOrPoint = 0m;
		_deferredISN = Guid.Empty;
		_baseDocISN = Guid.Empty;
		_nonCashPaymentId = "";
		_paymentAccepted = false;
		SetStatusBar_SaleConsultant();
		ModifySaleConsultantCardButtonText(_saleConsultantCode);
		_maxRowID = 0;
		_materialsRowsList.Clear();
		_discountsCalculator.Log?.Clear();
		SetBuyerInfoToPoleDisplay();
		InitPoleDisplay(text);
		CodeTextEdit.Focus();
		StartAutoSaveTimer();
	}

	private bool BeforeStore(StoreRequest sr, Guid isn, out bool checkLimitAnswerYes)
	{
		checkLimitAnswerYes = false;
		if (_docIsSale && sr.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && !string.IsNullOrWhiteSpace(_partnerCode) && !string.IsNullOrWhiteSpace(_contract))
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partnerID, _contract);
			if (contract != null)
			{
				if (contract.IsBonusCard && Settings.ParametersService.BonusUseMaxPercent() < 100m && (Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) * 100m / Convert.ToDecimal(TotalInfo.TB_Total.Text)).RoundAs(2) > Settings.ParametersService.BonusUseMaxPercent())
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.UsedMaxBonusValidate, (Convert.ToDecimal(TotalInfo.TB_Total.Text) * Settings.ParametersService.BonusUseMaxPercent() / 100m).RoundAs(2)));
				}
				if (Convert.ToDecimal(TotalInfo.TB_CreditSale.Text) + Convert.ToDecimal(TotalInfo.TB_FromPrePay.Text) > 0m)
				{
					if (Settings.IsOffline)
					{
						return true;
					}
					if (contract.ControlReceivables)
					{
						if ((TradeSettings.CreateTradeClient().CommonServices.GetCurrentPartnerRemByContract(_partnerID, contract.Contract, "", isn).SummCurr + Convert.ToDecimal(TotalInfo.TB_CreditSale.Text) + Convert.ToDecimal(TotalInfo.TB_FromPrePay.Text)).RoundAs(2) > contract.ReceivablesLimit)
						{
							if (!Settings.ParametersService.AllowExceedDebtLimit())
							{
								ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ReceivablesExceedDenied, _contract), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
								return false;
							}
							if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ReceivablesConfirmExceed, contract.Contract), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
							{
								return false;
							}
							checkLimitAnswerYes = true;
						}
					}
					else
					{
						checkLimitAnswerYes = true;
					}
				}
			}
		}
		return true;
	}

	private MTBill FillMTBillDocument()
	{
		MTBill mtBillDocument;
		if (Settings.IsOffline)
		{
			mtBillDocument = ((!(_deferredISN == Guid.Empty)) ? Settings.LocalDBDocumentService.Load<MTBill>(_deferredISN) : OfflineModeHelper.CreateOfflineModeMTBillDocument());
		}
		else
		{
			mtBillDocument = ((!(_deferredISN == Guid.Empty)) ? Settings.DocumentService.Load<MTBill>(_deferredISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill, withoutAccessCheck: true) : Settings.DocumentService.Create<MTBill>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill));
		}
		return FillMTBillDocumentCommonFields(mtBillDocument);
	}

	private MTBill FillMTBillDocumentCommonFields(MTBill mtBillDocument)
	{
		mtBillDocument.PrintTemplateDocumentType = 18;
		mtBillDocument.IsDeferred = _isDeferred;
		mtBillDocument.DocNum = SaleHeader.TB_SaleNum.Text;
		mtBillDocument.ECRCheckNum = _ecrCheckNum;
		if (!string.IsNullOrWhiteSpace(StatusBar.TB_Time.Text))
		{
			mtBillDocument.DocDate = Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly();
		}
		mtBillDocument.VATType = Methods.VATAndRetailVATMapping(_vatType, oppositeMapping: false);
		mtBillDocument.CurrCode = _currCode;
		mtBillDocument.Storage = _defaultStorage;
		mtBillDocument.CashDesk = _defaultCash;
		mtBillDocument.CashierShiftNumber = _currentShiftNum;
		mtBillDocument.PartnerContractGroup.PartnerCode = _partnerCode;
		mtBillDocument.PartnerContractGroup.ContractCode = _contract;
		mtBillDocument.PrePaymentECRCheckNum = _prePaymentECRCheckNum;
		mtBillDocument.PrintPartnerTinOnCheck = !string.IsNullOrWhiteSpace(_partnerTaxCode);
		mtBillDocument.PrepaymentGroup.PartnerCode = _prepaymentPartnerCode;
		mtBillDocument.PrepaymentGroup.ContractCode = _prepaymentContractCode;
		mtBillDocument.SalesConsultant = _saleConsultantCode;
		mtBillDocument.Discount = Convert.ToDecimal(BonusInfo.TB_DiscountPercent.Text);
		mtBillDocument.BankAcquiring.PartnerCode = _bankAcquiringPartner;
		mtBillDocument.BankAcquiring.ContractCode = _bankAcquiringContract;
		mtBillDocument.NonCashPaymentMethod = _nonCashPaymentMethod;
		mtBillDocument.PaymentId = _nonCashPaymentId;
		mtBillDocument.PaymentAccepted = _paymentAccepted;
		mtBillDocument.POSTerminalType = _posTerminalType;
		mtBillDocument.POSTerminalID = _posTerminalID;
		mtBillDocument.POSTerminalAddress = _posTerminalAddress;
		mtBillDocument.ECRCRN = _ecrCRN;
		mtBillDocument.Summ = Convert.ToDecimal(TotalInfo.TB_Total.Text);
		mtBillDocument.SummaCash = Convert.ToDecimal(TotalInfo.TB_Cash.Text);
		mtBillDocument.SummaChange = Convert.ToDecimal(TotalInfo.TB_MustPay.Text);
		mtBillDocument.SumFromRound = _sumFromRound;
		mtBillDocument.SummaPlCard = Convert.ToDecimal(TotalInfo.TB_NoCash.Text);
		decimal? summaPlCard = mtBillDocument.SummaPlCard;
		if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
		{
			mtBillDocument.BankAcquiring.PartnerCode = "";
			mtBillDocument.BankAcquiring.ContractCode = "";
			mtBillDocument.POSTerminalType = "";
			mtBillDocument.POSTerminalID = "";
			mtBillDocument.POSTerminalAddress = "";
			mtBillDocument.NonCashPaymentMethod = 0;
		}
		mtBillDocument.SummaCredit = Convert.ToDecimal(TotalInfo.TB_CreditSale.Text);
		mtBillDocument.SummaPayByBonus = Convert.ToDecimal(TotalInfo.TB_FromBonus.Text);
		mtBillDocument.SummaPrePayment = Convert.ToDecimal(TotalInfo.TB_FromPrePay.Text);
		mtBillDocument.SumAddedBonus = Convert.ToDecimal(BonusInfo.TB_AddedBonus.Text);
		mtBillDocument.SummVAT = 0m;
		mtBillDocument.Comment = "";
		mtBillDocument.Owner = Settings.LoginService.UserName;
		mtBillDocument.MaxRowID = _maxRowID;
		mtBillDocument.SalesConsultant = _saleConsultantCode;
		mtBillDocument.PrePaymentCashAmount = _summaCashSubtractBonus;
		mtBillDocument.PrePaymentCardAmount = _summaPlCardSubtractBonus;
		mtBillDocument.MTList.Clear();
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			MTInvoiceBase.MTInvoiceSpecificationRow item = new MTInvoiceBase.MTInvoiceSpecificationRow
			{
				ItemType = materialsRows.ItemType,
				Storage = materialsRows.Storage,
				ItemCode = materialsRows.ItemCode,
				ItemID = materialsRows.ItemID,
				ItemName = materialsRows.ItemName,
				UnitCode = materialsRows.UnitCode,
				UnitBrief = materialsRows.UnitBrief,
				Quantity = materialsRows.Quantity,
				Price = materialsRows.Price,
				Discount = materialsRows.Discount,
				DiscountPrice = materialsRows.DiscountPrice,
				UsedBonus = materialsRows.UsedBonus,
				AccumulatedBonus = materialsRows.AccumulatedBonus,
				Summa = materialsRows.Summa,
				DiscountSumma = materialsRows.DiscountSumma,
				BeginSumma = materialsRows.BeginSumma,
				BarCode = materialsRows.BarCode,
				VAT = materialsRows.VAT,
				RowID = materialsRows.RowID,
				IsPartySelected = materialsRows.IsPartySelected,
				RowDescription = materialsRows.RowDescription,
				IsDeleted = materialsRows.IsDeleted,
				IsGift = materialsRows.IsGift,
				IsStorageRequired = materialsRows.IsStorageRequired,
				MarkingList = materialsRows.MarkingList,
				IsRowMarkable = materialsRows.IsRowMarkable
			};
			mtBillDocument.MTList.Add(item);
		}
		if (_discountsCalculator.Log != null)
		{
			mtBillDocument.DiscountLogsList = _discountsCalculator.Log;
		}
		return mtBillDocument;
	}

	private MTBillReturn FillMTBillReturnDocument()
	{
		MTBillReturn mTBillReturnDocument;
		if (Settings.IsOffline)
		{
			mTBillReturnDocument = ((!(_deferredISN == Guid.Empty)) ? Settings.LocalDBDocumentService.Load<MTBillReturn>(_deferredISN) : OfflineModeHelper.CreateOfflineModeMTBillReturnDocument());
		}
		else
		{
			mTBillReturnDocument = ((!(_deferredISN == Guid.Empty)) ? Settings.DocumentService.Load<MTBillReturn>(_deferredISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value, withoutAccessCheck: true) : Settings.DocumentService.Create<MTBillReturn>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value));
		}
		return FillMTBillReturnDocumentCommonFields(mTBillReturnDocument);
	}

	private MTBillReturn FillMTBillReturnDocumentCommonFields(MTBillReturn mTBillReturnDocument)
	{
		mTBillReturnDocument.PrintTemplateDocumentType = 38;
		mTBillReturnDocument.IsDeferred = _isDeferred;
		mTBillReturnDocument.BaseISN = _baseDocISN;
		if (Settings.IsOffline)
		{
			mTBillReturnDocument.ECRBaseCheckNum = Settings.LocalDBDocumentService.Load<MTBill>(_baseDocISN).ECRCheckNum;
		}
		else
		{
			mTBillReturnDocument.ECRBaseCheckNum = Settings.DocumentService.Load<MTBill>(_baseDocISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value, withoutAccessCheck: true, withoutGrids: true).ECRCheckNum;
		}
		mTBillReturnDocument.DocNum = SaleHeader.TB_SaleNum.Text;
		mTBillReturnDocument.ECRCheckNum = _ecrCheckNum;
		if (!string.IsNullOrWhiteSpace(StatusBar.TB_Time.Text))
		{
			mTBillReturnDocument.DocDate = Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly();
		}
		mTBillReturnDocument.PriceType = _priceListType;
		mTBillReturnDocument.VATType = Methods.VATAndRetailVATMapping(_vatType, oppositeMapping: false);
		mTBillReturnDocument.CurrCode = _currCode;
		mTBillReturnDocument.Storage = _defaultStorage;
		mTBillReturnDocument.CashDesk = _defaultCash;
		mTBillReturnDocument.CashierShiftNumber = _currentShiftNum;
		mTBillReturnDocument.PartnerContractGroup.PartnerCode = _partnerCode;
		mTBillReturnDocument.PartnerContractGroup.ContractCode = _contract;
		mTBillReturnDocument.SalesConsultant = _defaultSalesConsultant;
		mTBillReturnDocument.Summ = Convert.ToDecimal(TotalInfo.TB_Total.Text);
		mTBillReturnDocument.SumFromRound = _sumFromRound;
		mTBillReturnDocument.PrePaymentECRCheckNum = _prePaymentECRCheckNum;
		mTBillReturnDocument.ECRReturnByCashSum = Convert.ToDecimal(TotalInfo.TB_Cash.Text) - Convert.ToDecimal(TotalInfo.TB_MustPay.Text);
		mTBillReturnDocument.ECRReturnByCardSum = Convert.ToDecimal(TotalInfo.TB_NoCash.Text);
		mTBillReturnDocument.ECRReturnByPrePaymentSum = Convert.ToDecimal(TotalInfo.TB_FromPrePay.Text);
		mTBillReturnDocument.SummaCredit = Convert.ToDecimal(TotalInfo.TB_CreditSale.Text);
		mTBillReturnDocument.BankAcquiring.PartnerCode = _bankAcquiringPartner;
		mTBillReturnDocument.BankAcquiring.ContractCode = _bankAcquiringContract;
		mTBillReturnDocument.PaymentRefundAccepted = PaymentRefundAccepted;
		mTBillReturnDocument.POSTerminalType = _posTerminalType;
		mTBillReturnDocument.POSTerminalAddress = _posTerminalAddress;
		mTBillReturnDocument.NonCashPaymentMethod = _nonCashPaymentMethod;
		mTBillReturnDocument.SummVAT = 0m;
		mTBillReturnDocument.Comment = "";
		mTBillReturnDocument.Owner = Settings.LoginService.UserName;
		mTBillReturnDocument.PrePaymentCashOutAmount = _summaCashSubtractBonus;
		mTBillReturnDocument.PrePaymentCardOutAmount = _summaPlCardSubtractBonus;
		mTBillReturnDocument.MTList.Clear();
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			mTBillReturnDocument.MTList.Add(FillMTRetFromCustSpecificationRow(materialsRows));
		}
		mTBillReturnDocument.MaxRowID = _maxRowID;
		return mTBillReturnDocument;
	}

	private MTReturnFromCustomer FillRetFromCustomerDocument(byte documentState)
	{
		MTReturnFromCustomer mTReturnFromCustomerDocument;
		if (Settings.IsOffline)
		{
			mTReturnFromCustomerDocument = ((!(_deferredISN == Guid.Empty)) ? Settings.LocalDBDocumentService.Load<MTReturnFromCustomer>(_deferredISN) : OfflineModeHelper.CreateOfflineModeMTReturnFromCustomerDocument());
		}
		else
		{
			mTReturnFromCustomerDocument = ((!(_deferredISN == Guid.Empty)) ? Settings.DocumentService.Load<MTReturnFromCustomer>(_deferredISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer, withoutAccessCheck: true) : Settings.DocumentService.Create<MTReturnFromCustomer>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer));
		}
		return FillRetFromCustomerDocumentCommonFields(mTReturnFromCustomerDocument, documentState);
	}

	private MTReturnFromCustomer FillRetFromCustomerDocumentCommonFields(MTReturnFromCustomer mTReturnFromCustomerDocument, byte documentState)
	{
		mTReturnFromCustomerDocument.IsDeferred = _isDeferred;
		mTReturnFromCustomerDocument.DocNum = SaleHeader.TB_SaleNum.Text;
		if (!string.IsNullOrWhiteSpace(StatusBar.TB_Time.Text))
		{
			mTReturnFromCustomerDocument.DocDate = Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly();
		}
		mTReturnFromCustomerDocument.PrintTemplateDocumentType = 15;
		mTReturnFromCustomerDocument.ECRCheckNum = _ecrCheckNum;
		mTReturnFromCustomerDocument.PriceType = _priceListType;
		mTReturnFromCustomerDocument.VATType = _vatType;
		mTReturnFromCustomerDocument.CurrCode = _currCode;
		mTReturnFromCustomerDocument.Storage = _defaultStorage;
		mTReturnFromCustomerDocument.CashDesk = _defaultCash;
		mTReturnFromCustomerDocument.CashierShiftNumber = _currentShiftNum;
		mTReturnFromCustomerDocument.PartnerContractGroup.PartnerCode = _partnerCode;
		mTReturnFromCustomerDocument.PartnerContractGroup.ContractCode = _contract;
		mTReturnFromCustomerDocument.SalesConsultant = _defaultSalesConsultant;
		mTReturnFromCustomerDocument.Summ = Convert.ToDecimal(TotalInfo.TB_Total.Text);
		mTReturnFromCustomerDocument.SumFromRound = _sumFromRound;
		mTReturnFromCustomerDocument.ECRReturnByCashSum = Convert.ToDecimal(TotalInfo.TB_Cash.Text) - Convert.ToDecimal(TotalInfo.TB_MustPay.Text);
		mTReturnFromCustomerDocument.ECRReturnByCardSum = Convert.ToDecimal(TotalInfo.TB_NoCash.Text);
		mTReturnFromCustomerDocument.ECRReturnByPrePaymentSum = Convert.ToDecimal(TotalInfo.TB_FromPrePay.Text);
		mTReturnFromCustomerDocument.BankAcquiring.PartnerCode = _bankAcquiringPartner;
		mTReturnFromCustomerDocument.BankAcquiring.ContractCode = _bankAcquiringContract;
		mTReturnFromCustomerDocument.PaymentRefundAccepted = PaymentRefundAccepted;
		mTReturnFromCustomerDocument.SummVAT = 0m;
		mTReturnFromCustomerDocument.Comment = "";
		mTReturnFromCustomerDocument.Owner = Settings.LoginService.UserName;
		mTReturnFromCustomerDocument.MTList.Clear();
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			if (!materialsRows.IsDeleted || documentState == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value)
			{
				mTReturnFromCustomerDocument.MTList.Add(FillMTRetFromCustSpecificationRow(materialsRows));
			}
		}
		mTReturnFromCustomerDocument.MaxRowID = _maxRowID;
		return mTReturnFromCustomerDocument;
	}

	private static MTReturnFromCustomerBase.MTRetFromCustSpecificationRow FillMTRetFromCustSpecificationRow(SaleRow saleRow)
	{
		MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = new MTReturnFromCustomerBase.MTRetFromCustSpecificationRow
		{
			ItemType = saleRow.ItemType,
			UnitCode = saleRow.UnitCode,
			UnitBrief = saleRow.UnitBrief,
			ItemID = saleRow.ItemID,
			ItemCode = saleRow.ItemCode,
			ItemName = saleRow.ItemName,
			Quantity = saleRow.Quantity,
			Discount = saleRow.Discount,
			Summa = saleRow.Summa,
			BeginSumma = saleRow.BeginSumma,
			BeginPrice = saleRow.Price,
			DiscountPrice = saleRow.DiscountPrice,
			Price = saleRow.DiscountPrice,
			VAT = saleRow.VAT,
			UsedBonus = saleRow.UsedBonus,
			AccumulatedBonus = saleRow.AccumulatedBonus,
			IsDeleted = saleRow.IsDeleted,
			IsGift = saleRow.IsGift,
			IsStorageRequired = saleRow.IsStorageRequired,
			RowDescription = saleRow.RowDescription,
			BarCode = saleRow.BarCode,
			RowID = saleRow.RowID,
			MarkingList = saleRow.MarkingList,
			IsRowMarkable = saleRow.IsRowMarkable
		};
		if (mTRetFromCustSpecificationRow.ItemType == ItemTypesSE.Goods.Value)
		{
			mTRetFromCustSpecificationRow.Storage = saleRow.Storage;
		}
		return mTRetFromCustSpecificationRow;
	}

	private void OnReturnButtonClick()
	{
		if (!ValidateIsCashierShiftOpen())
		{
			return;
		}
		if (_materialsRowsList.Count > 0)
		{
			if (_docIsSale)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.BeforReturnStoreOrDefer);
			}
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.BeforSaleStoreOrDefer);
		}
		if (_docIsSale)
		{
			if (Settings.ParametersService.AllowPOSRecordReturnFromCustomer())
			{
				MakeReturnDocument();
			}
			else if (new UserAuthenticationDialogWindow(this, ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSRecordReturnFromCustomer.Id).ShowDialog().Value)
			{
				MakeReturnDocument();
			}
			return;
		}
		if (!string.IsNullOrWhiteSpace(_defaultCash))
		{
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_defaultCash);
			if (item != null)
			{
				_ecrCRN = item.ECRCrn;
			}
		}
		_docType = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill;
		TotalInfo.TotalFill.Fill = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#49504b")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#def0e0")));
		SaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.SaleNumber + " ";
		SaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#4fad77")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#3E7337")));
		SaleHeader.TB_SaleNum.Foreground = SaleHeader.TB_SaleNumCaption.Foreground;
		SaleHeader.TB_BaseDoc.Text = "";
		SetNextDocumentNumber();
		if (TouchMode == POSScreenModesSE.TouchScreen.Value)
		{
			_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord;
		}
		else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
		{
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord;
		}
		_docIsSale = true;
	}

	private void MakeReturnDocument()
	{
		_baseDocCash = "";
		if (string.IsNullOrWhiteSpace(CodeTextEdit.Text) && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnWithoutBase, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			if (Settings.IsOffline && !Settings.UserAccessService.GetDocumentPermission(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value).AllowAdd)
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.DontHavePermissionToCreatePO, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Caption));
			}
			_docIsSale = false;
			_docType = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer;
		}
		else
		{
			if (Settings.IsOffline && !Settings.UserAccessService.GetDocumentPermission(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value).AllowAdd)
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.DontHavePermissionToCreatePO, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Caption));
			}
			if (!SelectBaseSaleForReturn())
			{
				_docIsSale = true;
				_docType = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill;
				return;
			}
		}
		TotalInfo.TotalFill.Fill = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#49504b")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#ffc7ce")));
		SaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord + " N ";
		SaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326")));
		SaleHeader.TB_SaleNum.Foreground = SaleHeader.TB_SaleNumCaption.Foreground;
		SetNextDocumentNumber();
		MaterialInfo.TB_GoodName.Text = "";
		MaterialInfo.TB_GoodRowDetail.Text = "";
		if (!string.IsNullOrWhiteSpace(CodeTextEdit.Text.Trim()))
		{
			SaleHeader.TB_BaseDoc.Text = $" ({ArmSoft.SME.Trade.Common.Resources.Base} {CodeTextEdit.Text.Trim()}) ";
			SaleHeader.TB_BaseDoc.Foreground = SaleHeader.TB_SaleNumCaption.Foreground;
			CodeTextEdit.Text = "";
			_realText = "";
		}
		else
		{
			SaleHeader.TB_BaseDoc.Text = "";
		}
		Title = SaleHeader.TB_SaleNumCaption.Text + SaleHeader.TB_SaleNum.Text;
		TotalInfo.TB_FromBonusCaption.Text = ArmSoft.SME.Trade.Common.Resources.BonusRestore;
		if (TouchMode == POSScreenModesSE.TouchScreen.Value)
		{
			_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.Sale;
		}
		else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
		{
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.Sale;
		}
	}

	private bool SelectBaseSaleForReturn()
	{
		DateOnly dateOnly = Convert.ToDateTime(StatusBar.TB_Time.Text).Date.ToDateOnly();
		if (!string.IsNullOrWhiteSpace(CodeTextEdit.Text))
		{
			List<Guid> list = (Settings.IsOffline ? Settings.LocalDBDocumentService.GetDocISNByECRN(CodeTextEdit.Text.Trim(), dateOnly, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value) : TradeSettings.CreateTradeClient().CommonServices.GetDocISNByECRN(CodeTextEdit.Text.Trim(), dateOnly, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value));
			if (list.Count == 1)
			{
				_baseDocISN = list[0];
			}
			else if (Settings.IsOffline)
			{
				_baseDocISN = list[0];
			}
			else
			{
				BaseECRCheckDocuments baseECRCheckDocuments = new BaseECRCheckDocuments(CodeTextEdit.Text.Trim(), Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value.ToString());
				if (TouchMode == POSScreenModesSE.Keyboard.Value)
				{
					ModalBrowser modalBrowser = new ModalBrowser(baseECRCheckDocuments)
					{
						Owner = Owner,
						WindowStartupLocation = WindowStartupLocation.CenterOwner
					};
					if (modalBrowser.Show())
					{
						BaseECRCheckDocumentsDataProvider.DataRow dataRow = (BaseECRCheckDocumentsDataProvider.DataRow)modalBrowser.DataView.Data[modalBrowser.SelectedRowIndex];
						if (dataRow != null)
						{
							_baseDocISN = dataRow.ISN;
						}
					}
				}
				else
				{
					TouchGridControl<BaseECRCheckDocumentsDataProvider.DataRow> touchGridControl = new TouchGridControl<BaseECRCheckDocumentsDataProvider.DataRow>(this, baseECRCheckDocuments, "BaseECRCheckDocumentsGrid");
					if (touchGridControl.ShowDialog().Value && touchGridControl.SelectedRow != null)
					{
						_baseDocISN = touchGridControl.SelectedRow.ISN;
					}
				}
			}
		}
		else
		{
			IDropDownView dropDownView = ((!Settings.IsOffline) ? ((IDropDownView)new DocumentsJournal(this, ReportTypesSE.BillDocument, new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value }, forPosReturn: true)
			{
				StartDate = dateOnly.AddDays(-30),
				EndDate = dateOnly,
				PartnerCode = _partnerCode,
				DocState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Processed.ValueUI
			}) : ((IDropDownView)new LocalDBDocuments
			{
				StartDate = dateOnly.AddDays(-30),
				EndDate = dateOnly,
				CurrCode = "AMD",
				DocumentState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Processed.Value,
				DocType = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value.ToString(),
				OrderByAscedning = false
			}));
			if (TouchMode == POSScreenModesSE.Keyboard.Value)
			{
				ModalBrowser modalBrowser2 = new ModalBrowser(dropDownView)
				{
					Owner = Owner,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};
				if (modalBrowser2.Show())
				{
					_baseDocISN = (Guid)dropDownView.KeyMember(modalBrowser2.SelectedRowIndex);
				}
			}
			else
			{
				TouchGridControl<TradeDocumentDataRow> touchGridControl2 = new TouchGridControl<TradeDocumentDataRow>(this, dropDownView, dropDownView.Name);
				if (touchGridControl2.ShowDialog().Value && touchGridControl2.SelectedRow != null)
				{
					_baseDocISN = touchGridControl2.SelectedRow.ISN;
				}
			}
		}
		if (_baseDocISN == Guid.Empty)
		{
			return false;
		}
		_docIsSale = false;
		_docType = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn;
		_sumFromRound = 0m;
		MTBill mTBill = ((!Settings.IsOffline) ? Settings.DocumentService.Load<MTBill>(_baseDocISN, withoutAccessCheck: true) : Settings.LocalDBDocumentService.Load<MTBill>(_baseDocISN));
		if (mTBill != null)
		{
			mTBill.SumAddedBonus = 0m;
			LoadMTBillDocument(mTBill, loadBaseDoc: true);
			TotalInfo.TB_Cash.Text = (mTBill.SummaCash.Value - mTBill.SummaChange.Value).CurrencyFormat(16, 2);
		}
		_baseDocCash = _defaultCash;
		_defaultStorage = Settings.ParametersService.DefaultStorage();
		_defaultCash = Settings.ParametersService.DefaultCash();
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			materialsRows.Storage = _defaultStorage;
		}
		if (_materialsRowsList.Count > 0)
		{
			GridControl gridControl = GridControl;
			BindingList<SaleRow> materialsRowsList = _materialsRowsList;
			gridControl.CurrentItem = materialsRowsList[materialsRowsList.Count - 1];
		}
		UpdateChangeOrPaySum();
		SetPaymentsVisibility();
		return true;
	}

	private void LoadMTBillDocument(MTBill mTBillDocument, bool loadBaseDoc = false)
	{
		SaleHeader.TB_SaleNum.Text = mTBillDocument.DocNum;
		if (!loadBaseDoc)
		{
			_ecrCheckNum = mTBillDocument.ECRCheckNum;
			_prePaymentECRCheckNum = mTBillDocument.PrePaymentECRCheckNum;
		}
		StatusBar.TB_Time.Text = mTBillDocument.DocDate.ToString();
		_vatType = mTBillDocument.VATType;
		_currCode = mTBillDocument.CurrCode;
		_defaultStorage = mTBillDocument.Storage;
		_defaultCash = mTBillDocument.CashDesk;
		_partnerCode = mTBillDocument.PartnerContractGroup.PartnerCode;
		_partnerID = mTBillDocument.PartnerID;
		_contract = mTBillDocument.PartnerContractGroup.ContractCode;
		_defaultSalesConsultant = mTBillDocument.SalesConsultant;
		BonusInfo.TB_DiscountPercent.Text = mTBillDocument.Discount.CurrencyFormat(6, 2);
		_bankAcquiringPartner = mTBillDocument.BankAcquiring.PartnerCode;
		_bankAcquiringContract = mTBillDocument.BankAcquiring.ContractCode;
		_prepaymentPartnerCode = mTBillDocument.PrepaymentGroup.PartnerCode;
		_prepaymentContractCode = mTBillDocument.PrepaymentGroup.ContractCode;
		_posTerminalType = mTBillDocument.POSTerminalType;
		_posTerminalID = mTBillDocument.POSTerminalID;
		_posTerminalAddress = mTBillDocument.POSTerminalAddress;
		_ecrCRN = mTBillDocument.ECRCRN;
		_nonCashPaymentMethod = mTBillDocument.NonCashPaymentMethod;
		_nonCashPaymentId = mTBillDocument.PaymentId;
		_paymentAccepted = mTBillDocument.PaymentAccepted;
		decimal? summaPlCard = mTBillDocument.SummaPlCard;
		_nonCashPaymentMethodSelected = (summaPlCard.GetValueOrDefault() > 0m) & summaPlCard.HasValue;
		ChangeTotalInfoNOCashCaption();
		if (mTBillDocument.PrintPartnerTinOnCheck.Value)
		{
			PartnerCacheRow itemById = TradeSettings.TradeCacheService.Partner.GetItemById(mTBillDocument.PartnerID);
			if (itemById != null)
			{
				_partnerTaxCode = itemById.TaxCode;
			}
		}
		_sumFromRound = mTBillDocument.SumFromRound;
		TotalInfo.TB_Total.Text = mTBillDocument.Summ.Value.CurrencyFormat(16, 2);
		TotalInfo.TB_Cash.Text = mTBillDocument.SummaCash.Value.CurrencyFormat(16, 2);
		TotalInfo.TB_MustPay.Text = mTBillDocument.SummaChange.Value.CurrencyFormat(16, 2);
		TotalInfo.TB_NoCash.Text = mTBillDocument.SummaPlCard.Value.CurrencyFormat(16, 2);
		TotalInfo.TB_FromBonus.Text = mTBillDocument.SummaPayByBonus.Value.CurrencyFormat(16, 2);
		TotalInfo.TB_FromPrePay.Text = mTBillDocument.SummaPrePayment.Value.CurrencyFormat(16, 2);
		TotalInfo.TB_CreditSale.Text = mTBillDocument.SummaCredit.Value.CurrencyFormat(16, 2);
		_maxRowID = mTBillDocument.MaxRowID;
		decimal num = 0m;
		_materialsRowsList.Clear();
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT in mTBillDocument.MTList)
		{
			if (loadBaseDoc)
			{
				if (Convert.ToBoolean(!mT.IsDeleted))
				{
					mT.MarkingList.Clear();
					SaleRow saleRow = new SaleRow(mT);
					_materialsRowsList.Add(saleRow);
					num = Convert.ToDecimal(num + mT.DiscountSumma);
					ChangeMarkingStatus(saleRow);
				}
			}
			else
			{
				SaleRow saleRow2 = new SaleRow(mT);
				_materialsRowsList.Add(saleRow2);
				num = Convert.ToDecimal(num + mT.DiscountSumma);
				ChangeMarkingStatus(saleRow2);
			}
		}
		TotalInfo.TB_Discount.Text = num.CurrencyFormat(16, 2);
		SetStatusBar_Quantity();
		_saleConsultantCode = mTBillDocument.SalesConsultant;
		SetStatusBar_SaleConsultant(_saleConsultantCode);
		ModifySaleConsultantCardButtonText(_saleConsultantCode);
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partnerCode, _contract, out var partnerCacheRow);
		if (contract != null)
		{
			if (string.IsNullOrWhiteSpace(contract.DiscountCard))
			{
				FillPartnerCardInfo(_partnerCode, partnerCacheRow, contract);
				BonusInfo.TB_CardNumber.Text = "";
			}
			else
			{
				FillPartnerCardInfo(contract.DiscountCard, partnerCacheRow, contract);
			}
			BonusInfo.TB_AddedBonus.Text = mTBillDocument.SumAddedBonus.Value.CurrencyFormat(16, 2);
			UpdateAccumulatedBonusText();
		}
		SetBuyerInfoToPoleDisplay();
		RowChanged();
	}

	private void LoadMTReturnFromCustomerDocument(MTReturnFromCustomerBase mtReturnFromCustomerBase)
	{
		SaleHeader.TB_SaleNum.Text = mtReturnFromCustomerBase.DocNum;
		StatusBar.TB_Time.Text = mtReturnFromCustomerBase.DocDate.ToString();
		_baseDocISN = mtReturnFromCustomerBase.BaseISN;
		_vatType = mtReturnFromCustomerBase.VATType;
		_currCode = mtReturnFromCustomerBase.CurrCode;
		_defaultStorage = mtReturnFromCustomerBase.Storage;
		_defaultCash = mtReturnFromCustomerBase.CashDesk;
		_partnerCode = mtReturnFromCustomerBase.PartnerContractGroup.PartnerCode;
		_partnerID = mtReturnFromCustomerBase.PartnerID;
		_contract = mtReturnFromCustomerBase.PartnerContractGroup.ContractCode;
		_sumFromRound = mtReturnFromCustomerBase.SumFromRound;
		_bankAcquiringContract = mtReturnFromCustomerBase.BankAcquiring.ContractCode;
		_bankAcquiringPartner = mtReturnFromCustomerBase.BankAcquiring.PartnerCode;
		_ecrCheckNum = mtReturnFromCustomerBase.ECRCheckNum;
		if (mtReturnFromCustomerBase.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value)
		{
			MTBillReturn mTBillReturn = (MTBillReturn)mtReturnFromCustomerBase;
			_nonCashPaymentMethod = mTBillReturn.NonCashPaymentMethod;
			_posTerminalType = mTBillReturn.POSTerminalType;
			_prePaymentECRCheckNum = mTBillReturn.PrePaymentECRCheckNum;
			_nonCashPaymentMethodSelected = mTBillReturn.ECRReturnByCardSum > 0m;
			TotalInfo.TB_CreditSale.Text = mTBillReturn.SummaCredit.CurrencyFormat(16, 2);
		}
		PaymentRefundAccepted = mtReturnFromCustomerBase.PaymentRefundAccepted;
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partnerID, _contract, out var partnerCacheRow);
		if (contract != null)
		{
			if (string.IsNullOrWhiteSpace(contract.DiscountCard))
			{
				FillPartnerCardInfo(_partnerCode, partnerCacheRow, contract);
				BonusInfo.TB_CardNumber.Text = "";
			}
			else
			{
				FillPartnerCardInfo(contract.DiscountCard, partnerCacheRow, contract);
			}
			UpdateAccumulatedBonusText();
		}
		TotalInfo.TB_Total.Text = mtReturnFromCustomerBase.Summ.Value.CurrencyFormat(16, 2);
		TotalInfo.TB_FromBonus.Text = mtReturnFromCustomerBase.SummaPayByBonus.CurrencyFormat(16, 2);
		TotalInfo.TB_Cash.Text = mtReturnFromCustomerBase.ECRReturnByCashSum.CurrencyFormat(16, 2);
		TotalInfo.TB_NoCash.Text = mtReturnFromCustomerBase.ECRReturnByCardSum.CurrencyFormat(16, 2);
		TotalInfo.TB_FromPrePay.Text = mtReturnFromCustomerBase.ECRReturnByPrePaymentSum.CurrencyFormat(16, 2);
		_maxRowID = mtReturnFromCustomerBase.MaxRowID;
		decimal num = 0m;
		_materialsRowsList.Clear();
		foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT in mtReturnFromCustomerBase.MTList)
		{
			SaleRow saleRow = new SaleRow(mT);
			_materialsRowsList.Add(saleRow);
			if (Convert.ToBoolean(!mT.IsDeleted))
			{
				decimal value = num;
				decimal? discountSumma = mT.DiscountSumma;
				num = Convert.ToDecimal((decimal?)value + discountSumma);
			}
			ChangeMarkingStatus(saleRow);
		}
		TotalInfo.TB_Discount.Text = num.CurrencyFormat(16, 2);
		SetStatusBar_Quantity();
		if (string.IsNullOrWhiteSpace(_contract))
		{
			BonusInfo.TB_CardNumber.Text = "";
			BonusInfo.TB_CardHolderName.Text = "";
			BonusInfo.TB_DiscountPercent.Text = 0m.CurrencyFormat(6, 2);
			BonusInfo.TB_Bonus.Text = 0m.CurrencyFormat(16, 2);
			BonusInfo.TB_AccumulatedBonus.Text = 0m.CurrencyFormat(16, 2);
		}
		SetBuyerInfoToPoleDisplay();
		RowChanged();
	}

	private decimal CalculateBonusUsingExtension()
	{
		CallExternalFunction("BonusCalculationForSaleWindow", calculateDiscount: false);
		return _materialsRowsList.Sum((SaleRow r) => r.AccumulatedBonus).RoundAs(2);
	}

	private void CalculateDiscountUsingExtension()
	{
		CallExternalFunction("DiscountCalculationForSaleWindow", calculateDiscount: true);
	}

	private void CallExternalFunction(string functionName, bool calculateDiscount)
	{
	}

	private void CalculateDiscount(CommonConst.JSDiscType discType = CommonConst.JSDiscType.ProgCalc)
	{
		decimal value = 0m;
		switch (discType)
		{
		case CommonConst.JSDiscType.DiscountPercent:
			foreach (SaleRow materialsRows in _materialsRowsList)
			{
				materialsRows.DiscountPrice = Methods.RoundPrice((materialsRows.Price * (1m - materialsRows.Discount / 100m)).RoundAs(2), _priceListTypeRoundDgr);
				materialsRows.Summa = (materialsRows.Quantity * materialsRows.DiscountPrice).RoundAs(2);
				materialsRows.DiscountSumma = (materialsRows.Quantity * materialsRows.Price).RoundAs(2) - materialsRows.Summa;
				if (!materialsRows.IsDeleted)
				{
					value += materialsRows.DiscountSumma;
				}
			}
			break;
		case CommonConst.JSDiscType.DiscountPrice:
			foreach (SaleRow materialsRows2 in _materialsRowsList)
			{
				if (materialsRows2.Price == 0m)
				{
					materialsRows2.Discount = 0m;
				}
				else
				{
					materialsRows2.Discount = (100m * (materialsRows2.Price - materialsRows2.DiscountPrice) / materialsRows2.Price).RoundAs(2);
				}
				materialsRows2.Summa = (materialsRows2.Quantity * materialsRows2.DiscountPrice).RoundAs(2);
				materialsRows2.DiscountSumma = (materialsRows2.Quantity * materialsRows2.Price).RoundAs(2) - materialsRows2.Summa;
				if (!materialsRows2.IsDeleted)
				{
					value += materialsRows2.DiscountSumma;
				}
			}
			break;
		case CommonConst.JSDiscType.DiscountSumm:
			foreach (SaleRow materialsRows3 in _materialsRowsList)
			{
				if (materialsRows3.Quantity != 0m)
				{
					materialsRows3.DiscountPrice = Methods.RoundPrice((materialsRows3.Summa / materialsRows3.Quantity).RoundAs(2), _priceListTypeRoundDgr);
					if (materialsRows3.Price == 0m)
					{
						materialsRows3.Discount = 0m;
					}
					else
					{
						materialsRows3.Discount = (100m * (materialsRows3.Price - materialsRows3.DiscountPrice) / materialsRows3.Price).RoundAs(2);
					}
					materialsRows3.DiscountSumma = (materialsRows3.Quantity * materialsRows3.Price).RoundAs(2) - materialsRows3.Summa;
					if (!materialsRows3.IsDeleted)
					{
						value += materialsRows3.DiscountSumma;
					}
				}
			}
			break;
		case CommonConst.JSDiscType.ProgCalc:
		{
			decimal num = Convert.ToDecimal(BonusInfo.TB_DiscountPercent.Text);
			for (int i = 0; i < _materialsRowsList.Count; i++)
			{
				_materialsRowsList[i].Discount = num;
				if (num != 0m)
				{
					_materialsRowsList[i].DiscountPrice = Methods.RoundPrice((_materialsRowsList[i].Price * (1m - num / 100m)).RoundAs(2), _priceListTypeRoundDgr);
				}
				else
				{
					SaleRow saleRow = _materialsRowsList[i];
					CalculateSaleRowDiscountPrice(ref saleRow);
					_materialsRowsList[i] = saleRow;
					if (_materialsRowsList[i].DiscountPrice == 0m)
					{
						_materialsRowsList[i].DiscountPrice = _materialsRowsList[i].Price;
					}
				}
				_materialsRowsList[i].Summa = (_materialsRowsList[i].Quantity * _materialsRowsList[i].DiscountPrice).RoundAs(2);
				_materialsRowsList[i].DiscountSumma = (_materialsRowsList[i].Quantity * _materialsRowsList[i].Price).RoundAs(2) - _materialsRowsList[i].Summa;
				if (!_materialsRowsList[i].IsDeleted)
				{
					value += _materialsRowsList[i].DiscountSumma;
				}
			}
			break;
		}
		}
		TotalInfo.TB_Discount.Text = value.CurrencyFormat(16, 2);
		GridControl.RefreshData();
	}

	private void AutoCalculateDiscount()
	{
		try
		{
			if (Settings.IsOffline)
			{
				return;
			}
			for (int num = _materialsRowsList.Count - 1; num >= 0; num--)
			{
				if (_materialsRowsList[num].IsGift)
				{
					_materialsRowsList.RemoveAt(num);
				}
			}
			_discountsCalculator.DocInfo = new DocumentInfo
			{
				DocDate = Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(),
				PartnerCode = _partnerCode,
				PartnerContract = _contract,
				CurrBase = _exchangeRate.Base,
				CurrRate = _exchangeRate.Rate,
				CurrCode = _currCode,
				TotalAmount = (_materialsRowsList.Where((SaleRow r) => !r.IsDeleted && r.ItemID != -1).Sum((SaleRow r) => r.Quantity * r.Price).RoundAs(2) * _exchangeRate.Rate / _exchangeRate.Base).RoundAs(2),
				VATType = _vatType,
				PriceType = _priceListType,
				PriceListTypeRoundDgr = _priceListTypeRoundDgr,
				Storage = _defaultStorage,
				Cash = _defaultCash,
				SaleConsultant = _saleConsultantCode,
				Rows = _materialsRowsList
			};
			_discountsCalculator.InitializeAndCalculate();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ShowMessageOnDiscountButtonClick(string msg)
	{
		ArmSoft.SME.Core.UI.MessageBox.Show(msg, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
	}

	private void SelectAndAddGiftGoodsToDoc(List<DiscountCacheRow> docDiscounts, Dictionary<string, List<MTComplectsPartsDataProvider.DataRow>> docGiftSets)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		List<GiftSetRowsDataProvider.DataRow> list = new List<GiftSetRowsDataProvider.DataRow>();
		foreach (DiscountCacheRow docDiscount in docDiscounts)
		{
			if (!(docDiscount.UseType == UsageMethodsSE.GiftSet.Value) || !docDiscount.SelectGift || !docGiftSets.TryGetValue(docDiscount.Code, out var value))
			{
				continue;
			}
			GiftSetRows giftSetRows = new GiftSetRows(value);
			if (TouchMode != POSScreenModesSE.Keyboard.Value)
			{
				TouchGridControlWithExtraButtons<GiftSetRowsDataProvider.DataRow> touchGridControlWithExtraButtons = new TouchGridControlWithExtraButtons<GiftSetRowsDataProvider.DataRow>(this, giftSetRows, "SelectAndAddGiftGoods");
				touchGridControlWithExtraButtons.Title = touchGridControlWithExtraButtons.Title + " (" + docDiscount.Caption + ")";
				while (touchGridControlWithExtraButtons.SelectedRow == null)
				{
					if (touchGridControlWithExtraButtons.ShowDialog().Value)
					{
						MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(touchGridControlWithExtraButtons.SelectedRow.MTCode);
						if (item != null)
						{
							dictionary.Add(docDiscount.Code, item.Id);
							list.Add(touchGridControlWithExtraButtons.SelectedRow);
						}
					}
				}
				continue;
			}
			ModalBrowser modalBrowser = new ModalBrowser(giftSetRows)
			{
				Owner = this
			};
			modalBrowser.Caption = modalBrowser.Caption + " (" + docDiscount.Caption + ")";
			modalBrowser.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			while (modalBrowser.SelectedRowIndex == -1)
			{
				if (!modalBrowser.Show())
				{
					continue;
				}
				GiftSetRowsDataProvider.DataRow rowByValue = giftSetRows.GetRowByValue(giftSetRows.ValueMember(modalBrowser.SelectedRowIndex));
				if (rowByValue != null)
				{
					MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(rowByValue.MTCode);
					if (item2 != null)
					{
						dictionary.Add(docDiscount.Code, item2.Id);
						list.Add(rowByValue);
					}
				}
			}
		}
		foreach (GiftSetRowsDataProvider.DataRow item4 in list)
		{
			_discountsCalculator.GenerateNewGiftDiscountsForDocument(dictionary);
			_discountsCalculator.CalculateNewGeneratedGiftDiscount(dictionary);
			MaterialCacheRow item3 = TradeSettings.TradeCacheService.Material.GetItem(item4.MTCode);
			if (item3 != null)
			{
				AddGiftItemToList(item3, item4.Summ, item4.Quantity);
			}
		}
	}

	private void AddGiftGoodsToDoc(List<DiscountCacheRow> docDiscounts, List<MTComplectsPartsDataProvider.DataRow> docGiftSets)
	{
		foreach (DiscountCacheRow docDiscount in docDiscounts)
		{
			if (docDiscount.UseType == UsageMethodsSE.Gift)
			{
				MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(docDiscount.GiftGoodID);
				if (itemById != null)
				{
					AddGiftItemToList(itemById, docDiscount.Amount, docDiscount.GiftQuantity);
				}
			}
		}
		foreach (MTComplectsPartsDataProvider.DataRow docGiftSet in docGiftSets)
		{
			MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(docGiftSet.MTSourceID);
			if (itemById != null)
			{
				AddGiftItemToList(itemById, docGiftSet.Summ, docGiftSet.QTY);
			}
		}
	}

	private void AddGiftItemToList(MaterialCacheRow materialCacheRow, decimal Price, decimal qty)
	{
		SaleRow saleRow = new SaleRow
		{
			IsGift = true,
			RowID = _maxRowID
		};
		FillSaleWindowSaleRow(saleRow, materialCacheRow, materialCacheRow.Unit, Price, qty);
		_materialsRowsList.Add(saleRow);
		_maxRowID++;
	}

	private void ClearPartnerRelatedSumms()
	{
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			materialsRows.AccumulatedBonus = 0m;
		}
		TotalInfo.TB_CreditSale.Text = 0m.CurrencyFormat(16, 2);
		UpdateChangeOrPaySum();
		SetPaymentsVisibility();
		OnFocusedRowHandleChanged();
		GridControl.RefreshData();
	}

	private decimal CalcBonuses()
	{
		decimal num = 0m;
		switch (Settings.ParametersService.BonusAccumulationMethod())
		{
		case "1":
			foreach (SaleRow item in _materialsRowsList.Where((SaleRow r) => !r.IsDeleted && !r.IsGift))
			{
				MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(item.ItemID);
				if (itemById == null)
				{
					continue;
				}
				if ((Settings.ParametersService.AccumulateBonusForDiscountedGoods() || item.Discount == 0m) && !itemById.IgnoreBonus)
				{
					if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent)
					{
						item.AccumulatedBonus = (item.Summa * itemById.Bonus / 100m).RoundAs(2);
					}
					else
					{
						item.AccumulatedBonus = (item.Quantity * itemById.Point).RoundAs(2);
					}
					num = Convert.ToDecimal(num + item.AccumulatedBonus);
				}
				else
				{
					num = Convert.ToDecimal(num - item.AccumulatedBonus);
					item.AccumulatedBonus = 0m;
				}
			}
			break;
		case "2":
		{
			decimal num5 = 0m;
			decimal num6 = 0m;
			if (Settings.ParametersService.BonusAccumulationBase() == BonusAccumulationBaseSE.AccumulativeSaleSumma.Value && _partnerID > 0)
			{
				DateOnly startDate = Settings.ParametersService.BonusAccumulationConditionPeriodStart() ?? ArmSoft.SME.Common.Constants.Common.MinOPDateOnly;
				DateOnly endDate = Settings.ParametersService.BonusAccumulationConditionPeriodEnd() ?? Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly();
				num6 = TradeSettings.CreateTradeClient().CommonServices.GetMTSaleSumByPartnerAndContract(CommonConst.SummaTypes.NCCSummWithVAT, startDate, endDate, _partnerID, _contract, Guid.Empty);
			}
			num = TradeSettings.CreateTradeClient().CommonServices.CalculateBonusByScale(Settings.ParametersService.BonusType(), (num6 > 0m) ? num6 : _saleTotalAmmount);
			if (num != -1m)
			{
				foreach (SaleRow item2 in _materialsRowsList.Where((SaleRow r) => !r.IsDeleted && !r.IsGift))
				{
					if ((Settings.ParametersService.AccumulateBonusForDiscountedGoods() || item2.Discount == 0m) && !TradeSettings.TradeCacheService.Material.IsBonusIgnoredForItem(item2.ItemID))
					{
						if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
						{
							item2.AccumulatedBonus = (item2.Summa * num / 100m).RoundAs(2);
						}
						else
						{
							item2.AccumulatedBonus = (item2.Summa / _saleTotalAmmount * num).RoundAs(2);
							num5 += item2.AccumulatedBonus;
						}
					}
					else if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
					{
						item2.AccumulatedBonus = 0m;
					}
					else
					{
						num5 -= item2.AccumulatedBonus;
						item2.AccumulatedBonus = 0m;
					}
					if (!(Settings.ParametersService.BonusType() != BonusTypeSE.ByPercent.Value))
					{
						continue;
					}
					decimal num7 = num - num5;
					if (!(num7 != 0m) || _materialsRowsList.Count <= 0)
					{
						continue;
					}
					for (int num8 = 0; num8 <= _materialsRowsList.Count - 1; num8++)
					{
						if (!_materialsRowsList[num8].IsDeleted && !_materialsRowsList[num8].IsGift && _materialsRowsList[num8].AccumulatedBonus > 0m)
						{
							BindingList<SaleRow> materialsRowsList2 = _materialsRowsList;
							materialsRowsList2[materialsRowsList2.Count - 1].AccumulatedBonus += num7;
						}
					}
				}
			}
			num = _materialsRowsList.Where((SaleRow r) => !r.IsDeleted && !r.IsGift).Sum((SaleRow r) => r.AccumulatedBonus);
			break;
		}
		case "3":
			if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent)
			{
				foreach (SaleRow item3 in _materialsRowsList.Where((SaleRow r) => !r.IsDeleted && !r.IsGift))
				{
					if ((Settings.ParametersService.AccumulateBonusForDiscountedGoods() || item3.Discount == 0m) && !TradeSettings.TradeCacheService.Material.IsBonusIgnoredForItem(item3.ItemID))
					{
						item3.AccumulatedBonus = (item3.Summa * _partContractBonusPercentOrPoint / 100m).RoundAs(2);
						num = Convert.ToDecimal(num + item3.AccumulatedBonus);
					}
					else
					{
						num = Convert.ToDecimal(num - item3.AccumulatedBonus);
						item3.AccumulatedBonus = 0m;
					}
				}
			}
			else
			{
				if (!(_partContractBonusPercentOrPoint != 0m))
				{
					break;
				}
				decimal num2 = 0m;
				decimal num3 = 0m;
				decimal saleTotalAmmount = _saleTotalAmmount;
				foreach (SaleRow materialsRows in _materialsRowsList)
				{
					if ((Settings.ParametersService.AccumulateBonusForDiscountedGoods() || materialsRows.Discount == 0m) && !TradeSettings.TradeCacheService.Material.IsBonusIgnoredForItem(materialsRows.ItemID))
					{
						if (saleTotalAmmount != 0m)
						{
							materialsRows.AccumulatedBonus = (materialsRows.Summa / saleTotalAmmount * _partContractBonusPercentOrPoint).RoundAs(2);
							num3 = Convert.ToDecimal(num3 + materialsRows.AccumulatedBonus);
						}
					}
					else if (saleTotalAmmount != 0m)
					{
						num = Convert.ToDecimal(num - materialsRows.AccumulatedBonus);
						materialsRows.AccumulatedBonus = 0m;
					}
				}
				num2 = _partContractBonusPercentOrPoint - num3;
				if (!(num2 != 0m) || CalcMTBillListRowCount(includeDeleted: false) <= 0)
				{
					break;
				}
				for (int num4 = _materialsRowsList.Count - 1; num4 >= 0; num4--)
				{
					if (!_materialsRowsList[num4].IsDeleted && !_materialsRowsList[num4].IsGift && _materialsRowsList[num4].AccumulatedBonus > 0m)
					{
						BindingList<SaleRow> materialsRowsList = _materialsRowsList;
						materialsRowsList[materialsRowsList.Count - 1].AccumulatedBonus += num2;
						break;
					}
				}
			}
			break;
		}
		return num;
	}

	private void UpdateAccumulatedBonusText()
	{
		decimal value = 0m;
		if (Settings.ParametersService.UseBonus() && _isBonusCard)
		{
			value = _materialsRowsList.Sum((SaleRow r) => r.AccumulatedBonus).RoundAs(2);
		}
		else
		{
			_currentBonusRemain = 0m;
		}
		BonusInfo.TB_AccumulatedBonus.Text = value.CurrencyFormat(16, 2);
	}

	private void UnitEdit(SaleRow row, string unitCodeOld)
	{
		UnitChanged(row, unitCodeOld);
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(row.UnitCode);
		row.UnitBrief = item.Brief;
		if (!Settings.ParametersService.PriceRecalcFromSumm())
		{
			ChangePriceAndRefreshGrid(row, row.Price, fromEditRowWindow: true);
		}
		else
		{
			CalculateSaleRowSummaAndDiscountSumma(row, calcDiscountPrice: true, fromEditRowWindow: true);
		}
		if (_docIsSale && Settings.ParametersService.DiscountUse())
		{
			if (Settings.ParametersService.DiscountExtensionUse() && !Settings.IsOffline)
			{
				CalculateDiscountUsingExtension();
			}
			else if (Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.IsOffline)
			{
				AutoCalculateDiscount();
			}
		}
		GridControl.RefreshData();
		UpdateChangeOrPaySum();
		if (_materialsRowsList.Count == 0)
		{
			RowInfoChangedEvent?.Invoke(null);
		}
		CalculateDocumentSumma();
		SetStatusBar_Quantity();
	}

	private void EditRow(SaleRow row, SaleWindowHelper.CurrentFocusedField focusedField)
	{
		SaleEditRowWindow saleEditRowWindow = new SaleEditRowWindow(this, row, !_docIsSale, TouchMode != POSScreenModesSE.Keyboard.Value, _isRetail, _priceListType, _priceListTypeRoundDgr, _vatType, Convert.ToDateTime(StatusBar.TB_Time.Text), _currCode, focusedField, _materialsRowsList.SelectMany((SaleRow saleRow) => saleRow.MarkingList).ToList(), fromSamplePOS: false);
		if (saleEditRowWindow.ShowDialog().Value)
		{
			row.UnitCode = saleEditRowWindow.UnitCode;
			row.UnitBrief = saleEditRowWindow.TE_Unit.Text;
			MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(row.ItemID, row.UnitCode);
			row.BarCode = ((barCode == null) ? "" : barCode.BarCode);
			row.MarkingList.Clear();
			if (!Settings.ParametersService.PriceRecalcFromSumm())
			{
				if (row.Quantity < saleEditRowWindow.Quantity)
				{
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(row.ItemCode);
					if (item == null || !item.IsWeight)
					{
						ChangeQuantityAndRefreshGrid(row, saleEditRowWindow.Quantity);
					}
				}
				ChangePriceAndRefreshGrid(row, saleEditRowWindow.Price, fromEditRowWindow: true);
			}
			else
			{
				CalculateSaleRowSummaAndDiscountSumma(row, calcDiscountPrice: true, fromEditRowWindow: true);
			}
			foreach (string marking in saleEditRowWindow.MarkingList)
			{
				row.MarkingList.Add(marking);
			}
		}
		if (_docIsSale && Settings.ParametersService.DiscountUse())
		{
			if (Settings.ParametersService.DiscountExtensionUse() && !Settings.IsOffline)
			{
				CalculateDiscountUsingExtension();
			}
			else if (Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.IsOffline)
			{
				AutoCalculateDiscount();
			}
		}
		GridControl.RefreshData();
		UpdateChangeOrPaySum();
		if (_materialsRowsList.Count == 0)
		{
			RowInfoChangedEvent?.Invoke(null);
		}
		CalculateDocumentSumma();
		SetStatusBar_Quantity();
	}

	private void IncreaseDecreaseQuantity(bool increase = true)
	{
		if (GridControl.CurrentItem == null)
		{
			return;
		}
		try
		{
			SaleRow saleRow = (SaleRow)GridControl.CurrentItem;
			if (!saleRow.IsDeleted)
			{
				if (increase)
				{
					saleRow.Quantity += 1m;
				}
				else if (saleRow.Quantity != 0m)
				{
					saleRow.Quantity = Math.Max(0m, saleRow.Quantity - 1m);
				}
				ChangeQuantityAndRefreshGrid(saleRow, saleRow.Quantity);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ChangeQuantityAndRefreshGrid(SaleRow saleRow, decimal quantity)
	{
		if (saleRow != null)
		{
			TotalInfo.TB_FromBonus.Text = (Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) - saleRow.UsedBonus).CurrencyFormat(16, 2);
			if (saleRow.Quantity != 0m)
			{
				saleRow.UsedBonus = (quantity * saleRow.UsedBonus / saleRow.Quantity).RoundAs(2);
			}
			TotalInfo.TB_FromBonus.Text = (Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) + saleRow.UsedBonus).CurrencyFormat(16, 2);
			saleRow.Quantity = quantity;
			if (Settings.ParametersService.DiscountUse())
			{
				if (Settings.ParametersService.DiscountExtensionUse() && _docIsSale && !Settings.IsOffline)
				{
					CalculateDiscountUsingExtension();
				}
				else if (Settings.ParametersService.AlwaysCalcDiscountAuto() && _docIsSale && !Settings.IsOffline)
				{
					AutoCalculateDiscount();
				}
				else
				{
					CalculateSaleRowSummaAndDiscountSumma(saleRow, calcDiscountPrice: false);
				}
			}
			else
			{
				CalculateSaleRowSumma(saleRow);
			}
			GridControl.RefreshData();
			CalculateDocumentSumma();
			SetStatusBar_Quantity();
			RowInfoChangedEvent?.Invoke(saleRow);
		}
		CodeTextEdit.Text = "";
		_realText = "";
		CodeTextEdit.Focus();
	}

	private void ChangePriceAndRefreshGrid(SaleRow saleRow, decimal price, bool fromEditRowWindow = false)
	{
		saleRow.Price = price;
		saleRow.BeginSumma = (saleRow.Quantity * saleRow.Price).RoundAs(2);
		if (Settings.ParametersService.DiscountUse())
		{
			if (Settings.ParametersService.DiscountExtensionUse() && _docIsSale && !Settings.IsOffline)
			{
				saleRow.DiscountPrice = saleRow.Price;
				CalculateDiscountUsingExtension();
			}
			else if (Settings.ParametersService.AlwaysCalcDiscountAuto() && _docIsSale && !Settings.IsOffline)
			{
				AutoCalculateDiscount();
			}
			else
			{
				if (_docIsSale && !string.IsNullOrWhiteSpace(_discountPriceListCurr) && saleRow.Price != 0m)
				{
					saleRow.Discount = (100m * (saleRow.Price - saleRow.DiscountPrice) / saleRow.Price).RoundAs(2);
				}
				CalculateSaleRowSummaAndDiscountSumma(saleRow, !_docIsSale || string.IsNullOrWhiteSpace(_discountPriceListCurr) || saleRow.DiscountPrice == 0m, fromEditRowWindow);
			}
		}
		else
		{
			CalculateSaleRowSumma(saleRow);
		}
		GridControl.RefreshData();
		CalculateDocumentSumma();
		RowInfoChangedEvent?.Invoke(saleRow);
		CodeTextEdit.Text = "";
		_realText = "";
		CodeTextEdit.Focus();
	}

	private void SetStatusBar_Quantity()
	{
		StatusBar.TB_QTY.Text = _materialsRowsList.Where((SaleRow r) => !r.IsDeleted && !r.IsGift).Sum((SaleRow r) => r.Quantity).CurrencyFormat(13, 3);
	}

	private void OnPriceButtonClick()
	{
		CodeTextEdit.DoValidate();
		if (!_docIsSale && _baseDocISN != Guid.Empty)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowChangePriceForReturn);
		}
		if (GridControl.CurrentItem == null || ((SaleRow)GridControl.CurrentItem).IsGift)
		{
			return;
		}
		try
		{
			if (string.IsNullOrWhiteSpace(CodeTextEdit.Text))
			{
				EditRow((SaleRow)GridControl.CurrentItem, SaleWindowHelper.CurrentFocusedField.SalePrice);
				return;
			}
			if (!CodeTextEdit.Text.Trim().IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
			}
			if (decimal.TryParse(CodeTextEdit.Text.Trim(), out var result))
			{
				decimal num = Convert.ToDecimal(result).RoundAs(2);
				if (num < 0m)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidatePriceIsPositive);
				}
				if (decimal.Truncate(num).ToString().Length > 9)
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ErrNumberOfDigitsBeforeDecimalPart);
				}
				if (Settings.ParametersService.AllowPOSChangePrice())
				{
					ChangePriceAndRefreshGrid((SaleRow)GridControl.CurrentItem, num);
				}
				else if (new UserAuthenticationDialogWindow(this, ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSChangePrice.Id).ShowDialog().Value)
				{
					ChangePriceAndRefreshGrid((SaleRow)GridControl.CurrentItem, num);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			CodeTextEdit.Text = "";
			_realText = "";
		}
	}

	private void DeleteRowAndRefreshDocumentFields(SaleRow saleRow)
	{
		TotalInfo.TB_FromBonus.Text = (Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) - saleRow.UsedBonus).CurrencyFormat(16, 2);
		if (Settings.ParametersService.AllowPOSFullDeleteRow())
		{
			_materialsRowsList.Remove(saleRow);
		}
		else
		{
			saleRow.IsDeleted = !saleRow.IsDeleted;
			saleRow.AccumulatedBonus = 0m;
			saleRow.UsedBonus = 0m;
			saleRow.MarkingList.Clear();
		}
		if (_docIsSale && Settings.ParametersService.DiscountUse())
		{
			if (Settings.ParametersService.DiscountExtensionUse() && !Settings.IsOffline)
			{
				CalculateDiscountUsingExtension();
			}
			else if (Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.IsOffline)
			{
				AutoCalculateDiscount();
			}
		}
		GridControl.RefreshData();
		UpdateChangeOrPaySum();
		if (_materialsRowsList.Count == 0)
		{
			RowInfoChangedEvent?.Invoke(null);
		}
		CalculateDocumentSumma();
		SetStatusBar_Quantity();
	}

	private void OnFavoriteMaterialsButtonClick()
	{
		if (!_docIsSale && _baseDocISN != Guid.Empty)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowAddRowForReturn);
		}
		_qty_Weight = 1m;
		if (Settings.IsOffline)
		{
			if (TradeSettings.TradeCacheService.MTFavorite.GetList().Count == 0)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.MTFavoriteInstruction, ArmSoft.SME.Common.Resources.RibbonPageFavorites, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			MTFavoritesBrowser mTFavoritesBrowser = new MTFavoritesBrowser(new CachedMTFavorites(Settings.LoginService.UserId, onlyMaterials: false, onlyGroups: false, _priceListType, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), _vatType, _currCode), AddFromFavorites, _priceListType, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), showTree: false, showCardView: true, TouchMode != POSScreenModesSE.Keyboard.Value);
			mTFavoritesBrowser.Owner = this;
			mTFavoritesBrowser.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			mTFavoritesBrowser.Show();
		}
		else
		{
			if (!TradeSettings.CreateTradeClient().CommonServices.FavoriteMTsExists(Settings.LoginService.UserId))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.MTFavoriteInstruction, ArmSoft.SME.Common.Resources.RibbonPageFavorites, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			MTFavoritesBrowser mTFavoritesBrowser2 = new MTFavoritesBrowser(new ArmSoft.SME.Trade.UI.DropDown.MTFavorites(Settings.LoginService.UserId, onlyMaterials: false, onlyGroups: false, _priceListType, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), _vatType, _currCode), AddFromFavorites, _priceListType, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), showTree: false, showCardView: true, TouchMode != POSScreenModesSE.Keyboard.Value);
			mTFavoritesBrowser2.Owner = this;
			mTFavoritesBrowser2.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			mTFavoritesBrowser2.Show();
		}
		RowChanged();
	}

	private void AddFromFavorites(string materialCode, string materialUnitCode)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(materialCode);
		if (item == null)
		{
			GridControl.Focus();
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, materialCode));
		}
		_qty_Weight = 1m;
		SaleRow saleRow = AddItemToList(item, materialCode, barCode: false, 0m, materialUnitCode);
		if (_useMarking)
		{
			bool flag = MarkingHelpers.IsGoodMarkable(saleRow.ItemCode, _useMarking, saleRow.MarkingList.Count != 0, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), out var isRowMarkable);
			saleRow.IsRowMarkable = isRowMarkable;
			if (_immediatlyOpenMarkingWindow & flag)
			{
				EditRow(saleRow, SaleWindowHelper.CurrentFocusedField.Quantity);
			}
		}
	}

	private void OnOpenProductsListButtonClick()
	{
		if (!_docIsSale && _baseDocISN != Guid.Empty)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowAddRowForReturn);
		}
		_qty_Weight = 1m;
		string likeName = CodeTextEdit.Text.Trim();
		string text = "";
		CodeTextEdit.Text = "";
		_realText = "";
		if (Settings.IsOffline)
		{
			OfflineGoodsFilter offlineGoodsFilter = new OfflineGoodsFilter(_priceListType, likeName);
			if (TouchMode != POSScreenModesSE.Keyboard.Value)
			{
				TouchGridControlWithExtraButtons<OfflineGoodsFilterDataProvider.DataRow> touchGridControlWithExtraButtons = new TouchGridControlWithExtraButtons<OfflineGoodsFilterDataProvider.DataRow>(this, offlineGoodsFilter, "MaterialSelectTouchDialog")
				{
					AllowFindAndFilterByBarCode = true
				};
				if (touchGridControlWithExtraButtons.ShowDialog().Value && touchGridControlWithExtraButtons.SelectedRow != null)
				{
					text = touchGridControlWithExtraButtons.SelectedRow.MTCode;
					RowChanged();
				}
			}
			else
			{
				ModalBrowser modalBrowser = new ModalBrowser(offlineGoodsFilter)
				{
					Owner = Owner,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};
				if (!modalBrowser.Show())
				{
					CodeTextEdit.Focus();
					RowChanged();
					return;
				}
				text = offlineGoodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
				RowChanged();
			}
		}
		else
		{
			ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices goodsAvailabilityFilterForInvoices = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices(StatusBar.TB_Time.Text.ToDateOnly(), _defaultStorage, _priceListType, _isRetail, likeName, "", extendedView: true, isFilteredSearchView: false, Settings.ParametersService.AllowPOSShowAvailability());
			if (TouchMode != POSScreenModesSE.Keyboard.Value)
			{
				TouchGridControlWithExtraButtons<ArmSoft.SME.Trade.Client.Data.DropDowns.GoodsAvailabilityFilterForInvoices.DataRow> touchGridControlWithExtraButtons2 = new TouchGridControlWithExtraButtons<ArmSoft.SME.Trade.Client.Data.DropDowns.GoodsAvailabilityFilterForInvoices.DataRow>(this, goodsAvailabilityFilterForInvoices, "MaterialSelectTouchDialog")
				{
					AllowFindAndFilterByBarCode = true
				};
				if (touchGridControlWithExtraButtons2.ShowDialog().Value && touchGridControlWithExtraButtons2.SelectedRow != null)
				{
					text = touchGridControlWithExtraButtons2.SelectedRow.MTCode;
					RowChanged();
				}
			}
			else
			{
				ModalBrowser modalBrowser2 = new ModalBrowser(goodsAvailabilityFilterForInvoices)
				{
					Owner = Owner,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};
				if (!modalBrowser2.Show())
				{
					CodeTextEdit.Focus();
					RowChanged();
					return;
				}
				text = goodsAvailabilityFilterForInvoices.ValueMember(modalBrowser2.SelectedRowIndex);
				RowChanged();
			}
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
		if (item == null)
		{
			CodeTextEdit.Focus();
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text));
		}
		SaleRow saleRow = AddItemToList(item, text, barCode: false);
		if (_useMarking)
		{
			bool flag = MarkingHelpers.IsGoodMarkable(saleRow.ItemCode, _useMarking, saleRow.MarkingList.Count != 0, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), out var isRowMarkable);
			saleRow.IsRowMarkable = isRowMarkable;
			if (_immediatlyOpenMarkingWindow & flag)
			{
				EditRow(saleRow, SaleWindowHelper.CurrentFocusedField.Quantity);
			}
		}
	}

	private SaleRow AddItemToList(MaterialCacheRow materialCacheRow, string identificValue, bool barCode = true, decimal retailPrice = 0m, string qntUnit = "", bool tryCallDiscountExtension = true)
	{
		SaleRow saleRow = null;
		int count = _materialsRowsList.Count;
		int num = ((!(Settings.ParametersService.ScannerMode() == "3")) ? AddNewRowOrMergeInner(materialCacheRow, identificValue, ref saleRow, barCode, retailPrice, Settings.ParametersService.ScannerMode() == "2", qntUnit) : AddNewRowInner(materialCacheRow, identificValue, ref saleRow, barCode, retailPrice, qntUnit));
		if (num == -1)
		{
			return null;
		}
		GridControl.CurrentItem = saleRow;
		if (Settings.ParametersService.DiscountUse())
		{
			if (_docIsSale && Settings.ParametersService.DiscountExtensionUse() && !Settings.IsOffline)
			{
				if (tryCallDiscountExtension)
				{
					CalculateDiscountUsingExtension();
				}
			}
			else if (_docIsSale && Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.IsOffline)
			{
				AutoCalculateDiscount();
			}
			else if (string.IsNullOrWhiteSpace(_priceListType) || (count != _materialsRowsList.Count && (!_isRetail || saleRow.ItemType == GoodsTypesSE.Service.Value)))
			{
				CalculateSaleRowDiscountPrice(ref saleRow);
				CalculateSaleRowSummaAndDiscountSumma(saleRow, !_docIsSale || saleRow.DiscountPrice == 0m);
			}
			else
			{
				CalculateSaleRowSummaAndDiscountSumma(saleRow, calcDiscountPrice: false);
			}
		}
		else
		{
			CalculateSaleRowSumma(saleRow);
		}
		GridControl.RefreshData();
		GridControl.RefreshRow(num);
		CalculateDocumentSumma();
		SetStatusBar_Quantity();
		CodeTextEdit.Text = "";
		_realText = "";
		CodeTextEdit.Focus();
		RowInfoChangedEvent?.Invoke(saleRow);
		return saleRow;
	}

	private int AddNewRowInner(MaterialCacheRow materialCacheRow, string identificValue, ref SaleRow saleRow, bool isBarCode = true, decimal retailPrice = 0m, string qntUnit = "")
	{
		decimal price = 0m;
		string qntUnit2 = "";
		string text = null;
		if (isBarCode)
		{
			if (TradeSettings.TradeCacheService.Material.GetItemByBarCode(identificValue, out qntUnit2) != null)
			{
				text = identificValue;
			}
		}
		else
		{
			qntUnit2 = (string.IsNullOrWhiteSpace(qntUnit) ? materialCacheRow.Unit : qntUnit);
			text = TradeSettings.TradeCacheService.Material.GetBarCode(materialCacheRow.Id, qntUnit2)?.BarCode;
		}
		if (!_isRetail || materialCacheRow.Type == GoodsTypesSE.Service.Value)
		{
			if (Settings.IsOffline)
			{
				price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForGivenPriceListAndVATTypeFromCache(_priceListType, _vatType, materialCacheRow.MTCode, qntUnit2);
				if (price == 0m && materialCacheRow.Unit != materialCacheRow.AltUnit)
				{
					price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceCalcForUnitByPriceListFromCache(_priceListType, _vatType, materialCacheRow.MTCode, qntUnit2, StatusBar.TB_Time.Text.ToDateOnly(), _currCode);
				}
			}
			else
			{
				price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, materialCacheRow.Id, qntUnit2, StatusBar.TB_Time.Text.ToDateOnly(), _currCode);
				if (price == 0m && materialCacheRow.Unit != materialCacheRow.AltUnit)
				{
					price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceCalcForUnitByPriceList(_priceListType, _vatType, materialCacheRow.MTCode, qntUnit2, StatusBar.TB_Time.Text.ToDateOnly(), _currCode);
				}
			}
			price = Methods.RoundPrice(price, _priceListTypeRoundDgr);
		}
		else
		{
			MaterialCacheRow.QntUnitRow qntUnit3 = TradeSettings.TradeCacheService.Material.GetQntUnit(materialCacheRow.Id, qntUnit2);
			if (qntUnit3 != null)
			{
				if (Settings.IsOffline)
				{
					price = retailPrice;
				}
				else if (retailPrice == 0m)
				{
					MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(materialCacheRow.MTCode, qntUnit3.Coef, _defaultStorage, StatusBar.TB_Time.Text.ToDateOnly());
					mTRetailQtyAndPrice.LoadData();
					if (mTRetailQtyAndPrice.RowsCount == 1)
					{
						price = Convert.ToDecimal(mTRetailQtyAndPrice.CommentMember(0));
					}
					else if (mTRetailQtyAndPrice.RowsCount > 1)
					{
						ModalBrowser modalBrowser = new ModalBrowser(mTRetailQtyAndPrice)
						{
							Owner = this,
							WindowStartupLocation = WindowStartupLocation.CenterOwner
						};
						if (!modalBrowser.Show())
						{
							CodeTextEdit.Text = "";
							_realText = "";
							CodeTextEdit.Focus();
							RowChanged();
							return -1;
						}
						price = Convert.ToDecimal(mTRetailQtyAndPrice.CommentMember(modalBrowser.SelectedRowIndex));
						RowChanged();
					}
				}
				else
				{
					price = retailPrice;
				}
			}
		}
		saleRow = new SaleRow();
		if (_qty_Weight == 0m)
		{
			_qty_Weight = 1m;
		}
		FillSaleWindowSaleRow(saleRow, materialCacheRow, qntUnit2, price, _qty_Weight);
		if (text != null)
		{
			saleRow.BarCode = text;
		}
		saleRow.RowID = _maxRowID;
		_materialsRowsList.Add(saleRow);
		_maxRowID++;
		return GridControl.GetRowHandleByListIndex(_materialsRowsList.Count - 1);
	}

	private int AddNewRowOrMergeInner(MaterialCacheRow materialCacheRow, string identificValue, ref SaleRow saleRow, bool barCode = true, decimal retailPrice = 0m, bool forFocusedRow = false, string qntUnit = "")
	{
		decimal num = 0m;
		int result = -1;
		SaleRow saleRow2 = null;
		if (forFocusedRow && CalcMTBillListRowCount(includeDeleted: true) != 0)
		{
			saleRow2 = (SaleRow)GridControl.CurrentItem;
			if (saleRow2 == null)
			{
				return -1;
			}
		}
		for (int i = 0; i <= _materialsRowsList.Count - 1; i++)
		{
			if ((forFocusedRow && saleRow2.RowID != _materialsRowsList[i].RowID) || _materialsRowsList[i].IsDeleted || _materialsRowsList[i].IsGift || ((!barCode || !(_materialsRowsList[i].BarCode == identificValue)) && (barCode || !(_materialsRowsList[i].ItemCode == identificValue) || (!string.IsNullOrWhiteSpace(qntUnit) && !(_materialsRowsList[i].UnitCode == qntUnit.Trim())))))
			{
				continue;
			}
			if (!_isRetail || materialCacheRow.Type == GoodsTypesSE.Service.Value)
			{
				_materialsRowsList[i].Quantity += _qty_Weight;
				saleRow = _materialsRowsList[i];
				result = GridControl.GetRowHandleByListIndex(i);
				break;
			}
			MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(materialCacheRow.Id, materialCacheRow.Unit);
			if (qntUnit2 == null)
			{
				break;
			}
			if (retailPrice == 0m)
			{
				MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(materialCacheRow.MTCode, qntUnit2.Coef, _defaultStorage, StatusBar.TB_Time.Text.ToDateOnly());
				mTRetailQtyAndPrice.LoadData();
				if (mTRetailQtyAndPrice.RowsCount == 1)
				{
					num = Convert.ToDecimal(mTRetailQtyAndPrice.CommentMember(0));
				}
				else if (mTRetailQtyAndPrice.RowsCount > 1)
				{
					ModalBrowser modalBrowser = new ModalBrowser(mTRetailQtyAndPrice)
					{
						Owner = this,
						WindowStartupLocation = WindowStartupLocation.CenterOwner
					};
					if (!modalBrowser.Show())
					{
						CodeTextEdit.Text = "";
						_realText = "";
						CodeTextEdit.Focus();
						RowChanged();
						return -1;
					}
					num = Convert.ToDecimal(mTRetailQtyAndPrice.CommentMember(modalBrowser.SelectedRowIndex));
					RowChanged();
				}
			}
			else
			{
				num = retailPrice;
			}
			if (num == _materialsRowsList[i].Price)
			{
				_materialsRowsList[i].Quantity += _qty_Weight;
				saleRow = _materialsRowsList[i];
				result = GridControl.GetRowHandleByListIndex(i);
				break;
			}
			for (int j = 0; j <= _materialsRowsList.Count - 1; j++)
			{
				if (_materialsRowsList[j].ItemID == materialCacheRow.Id && _materialsRowsList[j].UnitCode == materialCacheRow.Unit && _materialsRowsList[j].Price == num)
				{
					_materialsRowsList[j].Quantity += _qty_Weight;
					saleRow = _materialsRowsList[j];
					result = GridControl.GetRowHandleByListIndex(j);
					break;
				}
			}
			if (saleRow == null)
			{
				saleRow = new SaleRow();
				FillSaleWindowSaleRow(saleRow, materialCacheRow, materialCacheRow.Unit, num, _qty_Weight);
				MaterialCacheRow.BarCodeRow barCode2 = TradeSettings.TradeCacheService.Material.GetBarCode(materialCacheRow.Id, materialCacheRow.Unit);
				if (barCode2 != null)
				{
					saleRow.BarCode = barCode2.BarCode;
				}
				saleRow.RowID = _maxRowID;
				_materialsRowsList.Add(saleRow);
				_maxRowID++;
				result = GridControl.GetRowHandleByListIndex(_materialsRowsList.Count - 1);
			}
			break;
		}
		if (saleRow == null || saleRow.IsDeleted || saleRow.IsGift)
		{
			result = AddNewRowInner(materialCacheRow, identificValue, ref saleRow, barCode, retailPrice, qntUnit);
		}
		return result;
	}

	private void FillSaleWindowSaleRow(SaleRow saleRow, MaterialCacheRow materialRow, string unitCode, decimal price, decimal quantity)
	{
		saleRow.ItemType = materialRow.Type;
		if (materialRow.Type == GoodsTypesSE.Good.Value)
		{
			saleRow.Storage = _defaultStorage;
			saleRow.IsPartySelected = false;
		}
		else
		{
			saleRow.Storage = "";
			saleRow.IsPartySelected = null;
		}
		saleRow.ItemID = materialRow.Id;
		saleRow.ItemCode = materialRow.MTCode;
		saleRow.ItemName = materialRow.Caption;
		saleRow.BarCode = materialRow.BarCode;
		saleRow.UnitCode = unitCode;
		saleRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(unitCode)?.Brief ?? "";
		saleRow.VAT = materialRow.VAT;
		saleRow.Quantity = quantity;
		saleRow.Price = price;
		if (Settings.ParametersService.DiscountUse() && !Settings.ParametersService.DiscountExtensionUse() && (!Settings.IsOffline || !Settings.ParametersService.CalcDiscountAuto()))
		{
			saleRow.Discount = Convert.ToDecimal(BonusInfo.TB_DiscountPercent.Text);
			if (saleRow.Discount == 0m && (!_docIsSale || string.IsNullOrWhiteSpace(_discountPriceListType)))
			{
				saleRow.Discount = materialRow.Discount;
			}
			CalculateSaleRowDiscountPrice(ref saleRow);
			CalculateSaleRowSummaAndDiscountSumma(saleRow, !_docIsSale || saleRow.DiscountPrice == 0m);
			saleRow.BeginSumma = (saleRow.Quantity * saleRow.Price).RoundAs(2);
		}
		else
		{
			CalculateSaleRowSumma(saleRow);
		}
	}

	private void CalculateSaleRowDiscountPrice(ref SaleRow saleRow)
	{
		if (!_docIsSale || !Settings.ParametersService.DiscountUse())
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(_discountPriceListType))
		{
			saleRow.DiscountPrice = 0m;
			return;
		}
		decimal? num = ((!Settings.IsOffline) ? TradeSettings.CreateTradeClient().CommonServices.GetPrice(_discountPriceListType, saleRow.ItemID, saleRow.UnitCode, StatusBar.TB_Time.Text.ToDateOnly())?.Price : TradeSettings.TradeCacheService.CurrentPrices.GetItem(new CurrentPricesCacheRow.RowKey
		{
			MTID = saleRow.ItemID,
			PriceType = _discountPriceListType,
			Unit = saleRow.UnitCode
		})?.Price);
		if (num.HasValue && num.Value != 0m)
		{
			if (_discountPriceListCurr == _currCode)
			{
				saleRow.DiscountPrice = num.Value;
			}
			else if (_discountPriceListCurr == "AMD")
			{
				saleRow.DiscountPrice = (num.Value / _exchangeRate.Rate * _exchangeRate.Base).RoundAs(2);
			}
			else
			{
				saleRow.DiscountPrice = (num.Value * _discountPriceListExchangeRate.Rate / _discountPriceListExchangeRate.Base / _exchangeRate.Rate * _exchangeRate.Base).RoundAs(2);
			}
			if (Methods.VATAndRetailVATMapping(_discountPriceListVATType, oppositeMapping: true) != _vatType && saleRow.VAT.Value)
			{
				switch (_discountPriceListVATType)
				{
				case "1":
					if (_vatType.InList("2", "5"))
					{
						saleRow.DiscountPrice += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(saleRow.DiscountPrice, "1", saleRow.VAT.Value);
					}
					break;
				case "2":
					if (_vatType.InList("1", "3", "4"))
					{
						saleRow.DiscountPrice -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(saleRow.DiscountPrice, "5", saleRow.VAT.Value);
					}
					break;
				case "5":
					if (_vatType.InList("1", "3", "4"))
					{
						saleRow.DiscountPrice -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(saleRow.DiscountPrice, "5", saleRow.VAT.Value);
					}
					break;
				}
			}
			saleRow.DiscountPrice = Methods.RoundPrice(saleRow.DiscountPrice, _discountPriceTypeRoundDegree);
			if (saleRow.Price == 0m)
			{
				saleRow.Price = saleRow.DiscountPrice;
			}
			else
			{
				saleRow.Discount = (100m * (saleRow.Price - saleRow.DiscountPrice) / saleRow.Price).RoundAs(2);
			}
		}
		else if (Convert.ToDecimal(BonusInfo.TB_DiscountPercent.Text) != 0m)
		{
			saleRow.Discount = Convert.ToDecimal(BonusInfo.TB_DiscountPercent.Text);
			saleRow.DiscountPrice = Methods.RoundPrice((saleRow.Price * (100m - saleRow.Discount) / 100m).RoundAs(2), _priceListTypeRoundDgr);
		}
		else
		{
			saleRow.DiscountPrice = saleRow.Price;
			saleRow.Discount = 0m;
		}
	}

	private void CalculateSaleRowSummaAndDiscountSumma(SaleRow row, bool calcDiscountPrice = true, bool fromEditRowWindow = false)
	{
		if (Settings.ParametersService.DiscountUse())
		{
			if (calcDiscountPrice)
			{
				if (fromEditRowWindow)
				{
					row.DiscountPrice = Methods.RoundPrice((row.Summa / row.Quantity).RoundAs(2), _priceListTypeRoundDgr);
					row.Discount = ((row.Price > 0m) ? (100m * (row.Price - row.DiscountPrice) / row.Price).RoundAs(2) : 0m);
				}
				else
				{
					row.DiscountPrice = Methods.RoundPrice((row.Price * (100m - row.Discount) / 100m).RoundAs(2), _priceListTypeRoundDgr);
				}
			}
			if (!fromEditRowWindow)
			{
				row.Summa = (row.Quantity * row.DiscountPrice).RoundAs(2);
			}
			row.DiscountSumma = ((row.Quantity * row.Price).RoundAs(2) - row.Summa).RoundAs(2);
		}
		else
		{
			CalculateSaleRowSumma(row);
		}
	}

	private static void CalculateSaleRowSumma(SaleRow row)
	{
		row.DiscountPrice = row.Price;
		row.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(row.DiscountPrice, row.Quantity);
		row.Discount = 0m;
		row.DiscountSumma = 0m;
		row.BeginSumma = row.Summa;
	}

	private void CalculateDocumentSumma()
	{
		_saleTotalAmmount = _materialsRowsList.Where((SaleRow r) => !r.IsDeleted).Sum((SaleRow r) => r.Summa);
		if (!Settings.IsOffline)
		{
			if (Settings.ParametersService.UseBonus() && _isBonusCard && _docIsSale)
			{
				decimal value = 0m;
				if (!Settings.ParametersService.NoCalcBonusesWhenRePay())
				{
					value = ((Settings.ParametersService.BonusExtensionUse() && _docIsSale) ? CalculateBonusUsingExtension() : CalcBonuses());
				}
				else if (Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) == 0m)
				{
					value = ((Settings.ParametersService.BonusExtensionUse() && _docIsSale) ? CalculateBonusUsingExtension() : CalcBonuses());
				}
				BonusInfo.TB_AccumulatedBonus.Text = value.CurrencyFormat(16, 2);
			}
			if (Settings.ParametersService.UseBonus() && !_docIsSale && _baseDocISN != Guid.Empty)
			{
				decimal num = CalcBonusesReturn();
				BonusInfo.TB_AccumulatedBonus.Text = num.ToString("N" + (short)2);
			}
		}
		CalculateTotalAmount();
		UpdateChangeOrPaySum();
		OnFocusedRowHandleChanged();
		GridControl.RefreshData();
	}

	private decimal CalcBonusesReturn()
	{
		MTBill mTBill = (MTBill)Settings.DocumentService.Load(_baseDocISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill, withoutAccessCheck: true);
		decimal result = 0m;
		foreach (SaleRow row in _materialsRowsList)
		{
			MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow = mTBill.MTList.FirstOrDefault((MTInvoiceBase.MTInvoiceSpecificationRow x) => x.ItemCode == row.ItemCode);
			if (mTInvoiceSpecificationRow != null)
			{
				row.AccumulatedBonus = Math.Round(mTInvoiceSpecificationRow.AccumulatedBonus / mTInvoiceSpecificationRow.Quantity * row.Quantity, 2);
				result += row.AccumulatedBonus;
			}
		}
		return result;
	}

	private void CalculateTotalAmount()
	{
		decimal num = 0m;
		_temporaryDiscountSumm = 0m;
		foreach (SaleRow item in _materialsRowsList.Where((SaleRow r) => !r.IsDeleted))
		{
			num = Convert.ToDecimal(num + item.Summa);
			_temporaryDiscountSumm += item.DiscountSumma;
		}
		decimal num2 = num;
		if (!string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillRoundDegree()))
		{
			num2 = Methods.RoundPrice(num, Settings.ParametersService.MTBillRoundDegree(), Settings.ParametersService.MTBillSummRoundUp());
		}
		_sumFromRound = num2 - num;
		TotalInfo.TB_MustPay.Text = num2.CurrencyFormat(16, 2);
		TotalInfo.TB_Total.Text = num2.CurrencyFormat(16, 2);
		TotalInfo.TB_Discount.Text = _temporaryDiscountSumm.CurrencyFormat(16, 2);
		SetTotalInfoToPoleDisplay();
	}

	private int CalcMTBillListRowCount(bool includeDeleted)
	{
		if (!includeDeleted)
		{
			return _materialsRowsList.Count((SaleRow r) => !r.IsDeleted);
		}
		return _materialsRowsList.Count;
	}

	private void OnTaxCodeButtonClick()
	{
		CustomBarCodeOrTINSearchWindow customBarCodeOrTINSearchWindow = new CustomBarCodeOrTINSearchWindow(this, SearchModes.TaxCode);
		if (customBarCodeOrTINSearchWindow.ShowDialog().Value)
		{
			if (!string.IsNullOrWhiteSpace(_partnerCode))
			{
				ClearPartnerCard();
			}
			GetPartnerByTaxCode(customBarCodeOrTINSearchWindow.SearchText);
		}
	}

	private void OnFindBuyerButtonClick()
	{
		string text = "";
		CodeTextEdit.Text = "";
		_realText = "";
		ArmSoft.SME.Trade.UI.DropDown.PartnersFilter partnersFilter = new ArmSoft.SME.Trade.UI.DropDown.PartnersFilter(ArmSoft.SME.Trade.UI.DropDown.PartnersFilter.ShowPartnersModeType.FindBuyerPOS, "", isFilterBySuppliers: false, isFilterByCustomers: true);
		if (TouchMode != POSScreenModesSE.Keyboard.Value)
		{
			TouchGridControlWithExtraButtons<PartnersFilterDataProvider.DataRow> touchGridControlWithExtraButtons = new TouchGridControlWithExtraButtons<PartnersFilterDataProvider.DataRow>(this, partnersFilter, "FindBuyerTouchDialog");
			if (touchGridControlWithExtraButtons.ShowDialog().Value && touchGridControlWithExtraButtons.SelectedRow != null)
			{
				if (!string.IsNullOrWhiteSpace(_partnerCode))
				{
					ClearPartnerCard();
				}
				text = touchGridControlWithExtraButtons.SelectedRow.PartCode;
				RowChanged();
			}
		}
		else
		{
			ModalBrowser modalBrowser = new ModalBrowser(partnersFilter)
			{
				Owner = Owner,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};
			if (!modalBrowser.Show())
			{
				RowChanged();
				return;
			}
			if (!string.IsNullOrWhiteSpace(_partnerCode))
			{
				ClearPartnerCard();
			}
			text = partnersFilter.ValueMember(modalBrowser.SelectedRowIndex);
			RowChanged();
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			GetBuyerByCode(text);
		}
		SetBuyerInfoToPoleDisplay();
	}

	private void OnDiscountButtonClickBonusCardButtonClick()
	{
		try
		{
			if (!_docIsSale && _baseDocISN != Guid.Empty)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowChangeCardForReturn);
			}
			CodeTextEdit.DoValidate();
			bool flag = false;
			bool byCleanButtonClick = false;
			short num = 0;
			if (string.IsNullOrWhiteSpace(_partnerCode))
			{
				flag = Settings.ParametersService.AllowPOSInputFromKeyboard();
				Settings.ParametersService.AllowPOSInputFromKeyboard();
				num = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSInputFromKeyboard.Id;
			}
			else
			{
				flag = Settings.ParametersService.AllowPOSRemoveCard();
				Settings.ParametersService.AllowPOSRemoveCard();
				num = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSRemoveCard.Id;
				byCleanButtonClick = true;
			}
			if (flag)
			{
				ReadMagneticCard(CodeTextEdit.Text.Trim(), byMagneticCardPrefixSuffix: false, byCleanButtonClick);
			}
			else if (new UserAuthenticationDialogWindow(this, num).ShowDialog().Value)
			{
				ReadMagneticCard(CodeTextEdit.Text.Trim(), byMagneticCardPrefixSuffix: false, byCleanButtonClick);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			CodeTextEdit.Text = "";
			_realText = "";
		}
	}

	private void EMarkReceiptClick()
	{
		MarkingHelpers.ExportEMarkReceipts(this, viewMode: false, forPOS: true, _defaultStorage);
	}

	private void NewBuyerClick()
	{
		NewBuyerWindow newBuyerWindow = new NewBuyerWindow(this);
		if (newBuyerWindow.ShowDialog().Value && !string.IsNullOrWhiteSpace(newBuyerWindow.PartnerCode))
		{
			if (!string.IsNullOrWhiteSpace(_partnerCode))
			{
				ClearPartnerCard();
			}
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(newBuyerWindow.PartnerCode, out var partnerCacheRow);
			FillBuyerFields(defaultContract.DiscountCard, partnerCacheRow, defaultContract);
		}
	}

	private void GetPartnerByTaxCode(string taxCode)
	{
		try
		{
			if (!_docIsSale && _baseDocISN != Guid.Empty)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowChangeCardForReturn);
			}
			_partnerTaxCode = taxCode;
			PartnerCacheRow partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetPartnersByTaxCode(taxCode).FirstOrDefault();
			if (partnerCacheRow == null)
			{
				if (Settings.IsOffline)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.CurrentOperationIsNotAvailableInOfflineMode, ArmSoft.SME.Trade.Common.Resources.NewBuyer));
				}
				Partner partner = Settings.DirectoryService.Create<Partner>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Partner.Value);
				partner.Code = Settings.ParametersService.NextPartner();
				partner.Name = ArmSoft.SME.Trade.Common.Resources.BuyerAddedFromPOS;
				partner.FullCaption = ArmSoft.SME.Trade.Common.Resources.BuyerAddedFromPOS;
				partner.TaxCode = taxCode;
				partner.Supplier = false;
				partner.Customer = true;
				Settings.DirectoryService.Store(partner, new DirStoreRequest
				{
					LogComment = ArmSoft.SME.Trade.Common.Resources.BuyerAddedFromPOS,
					WithoutAccessCheck = Settings.ParametersService.AllowToRegisterNewBuyer()
				});
				PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(partner.Code, out var partnerCacheRow2);
				if (defaultContract != null)
				{
					_contract = defaultContract.Contract;
					_partnerID = partnerCacheRow2.Id;
					_partnerCode = partnerCacheRow2.PartCode;
					_isBonusCard = defaultContract.IsBonusCard;
					DateOnly value = StatusBar.TB_Time.Text.ToDateOnly();
					_isPartBonusCardValid = (!defaultContract.BonusCardValidPeriodStart.HasValue || (defaultContract.BonusCardValidPeriodStart.HasValue && defaultContract.BonusCardValidPeriodStart <= value)) && (!defaultContract.BonusCardValidPeriodEnd.HasValue || (defaultContract.BonusCardValidPeriodEnd.HasValue && defaultContract.BonusCardValidPeriodEnd >= value));
					if (_connectTob2barmsoftDiscountServer)
					{
						_isPartBonusCardValid = _isPartBonusCardValid && defaultContract.BonusCardValidPeriodStart.HasValue;
					}
					_ = defaultContract.BonusCardCanceled;
					if (_docIsSale)
					{
						if (defaultContract.BonusCardCanceled)
						{
							ClearPartnerCard();
							if (Settings.ParametersService.UseBonus() && _isBonusCard)
							{
								throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsCanceled);
							}
							throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsCanceled);
						}
						if (!_isPartBonusCardValid)
						{
							ClearPartnerCard();
							if (Settings.ParametersService.UseBonus() && _isBonusCard && _connectTob2barmsoftDiscountServer)
							{
								throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsNotValidForBSUser);
							}
							if (Settings.ParametersService.UseBonus() && _isBonusCard)
							{
								throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsNotValid);
							}
							throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsNotValid);
						}
					}
					BonusInfo.TB_CardNumber.Text = ArmSoft.SME.Common.Resources.OptionsTaxCode + " - " + taxCode;
					BonusInfo.TB_CardHolderName.Text = partner.Name + " (" + ArmSoft.SME.Trade.Common.Resources.DefaultContract + ")";
					BonusInfo.TB_DiscountPercent.Text = 0m.CurrencyFormat(6, 2);
					BonusInfo.TB_Bonus.Text = 0m.CurrencyFormat(16, 2);
					BonusInfo.TB_AccumulatedBonus.Text = 0m.CurrencyFormat(16, 2);
					BonusInfo.TB_AddedBonus.Text = 0m.CurrencyFormat(16, 2);
					ModifyPartnerCardButtonText(taxCode);
				}
				if (_docIsSale && Settings.ParametersService.DiscountUse() && Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.IsOffline)
				{
					AutoCalculateDiscount();
				}
			}
			else
			{
				PartnerCacheRow.ContractRow defaultContract2 = TradeSettings.TradeCacheService.Partner.GetDefaultContract(partnerCacheRow);
				if (defaultContract2 != null)
				{
					if (string.IsNullOrWhiteSpace(defaultContract2.DiscountCard))
					{
						FillPartnerCardInfo(taxCode, partnerCacheRow, defaultContract2, byTaxCode: true);
					}
					else
					{
						FillPartnerCardInfo(defaultContract2.DiscountCard, partnerCacheRow, defaultContract2);
					}
					if (_docIsSale && !string.IsNullOrWhiteSpace(defaultContract2.PriceType) && _priceListType != defaultContract2.PriceType)
					{
						PriceListTypeEditValueChanged(defaultContract2.PriceType);
					}
				}
			}
			CalculateDocumentSumma();
			SetBuyerInfoToPoleDisplay();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			CodeTextEdit.Text = "";
			_realText = "";
		}
	}

	private void GetBuyerByCode(string code)
	{
		try
		{
			_partnerCode = code;
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(code);
			if (item != null)
			{
				PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item);
				if (defaultContract != null)
				{
					if (string.IsNullOrWhiteSpace(defaultContract.DiscountCard))
					{
						FillPartnerCardInfo(code, item, defaultContract);
						BonusInfo.TB_CardNumber.Text = "";
					}
					else
					{
						FillPartnerCardInfo(defaultContract.DiscountCard, item, defaultContract);
					}
					if (_docIsSale)
					{
						if (Settings.ParametersService.DiscountUse())
						{
							if (Settings.ParametersService.DiscountExtensionUse() && !Settings.IsOffline)
							{
								CalculateDiscountUsingExtension();
							}
							else if (!string.IsNullOrWhiteSpace(defaultContract.PriceType) && _priceListType != defaultContract.PriceType)
							{
								PriceListTypeEditValueChanged(defaultContract.PriceType);
							}
							else if (!Settings.ParametersService.AlwaysCalcDiscountAuto() && (!Settings.IsOffline || !Settings.ParametersService.CalcDiscountAuto()))
							{
								CalculateDiscount();
							}
						}
						else if (!string.IsNullOrWhiteSpace(defaultContract.PriceType) && _priceListType != defaultContract.PriceType)
						{
							PriceListTypeEditValueChanged(defaultContract.PriceType);
						}
					}
				}
			}
			CalculateDocumentSumma();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			CodeTextEdit.Text = "";
			_realText = "";
		}
	}

	private void ReadMagneticCard(string cardData, bool byMagneticCardPrefixSuffix = true, bool byCleanButtonClick = false)
	{
		try
		{
			string text = "";
			text = BasePOSSaleWindow.GetCardNumberWithoutPrefixSuffix(cardData, byMagneticCardPrefixSuffix);
			if (byCleanButtonClick)
			{
				if (!string.IsNullOrWhiteSpace(_partnerCode))
				{
					CodeTextEdit.Text = "";
					_realText = "";
					CodeTextEdit.Focus();
					ClearPartnerCard();
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(text))
				{
					return;
				}
				if (!ReadSaleConsultantMagneticCard(text))
				{
					AddNewBuyerWindow(text);
				}
				PartnerCacheRow.ContractRow contractByDiscountCard = TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(text, out var partnerCacheRow);
				FillPartnerCardInfo(text, partnerCacheRow, contractByDiscountCard);
				if (_docIsSale)
				{
					if (Settings.ParametersService.DiscountUse())
					{
						if (Settings.ParametersService.DiscountExtensionUse() && !Settings.IsOffline)
						{
							CalculateDiscountUsingExtension();
						}
						else if (!string.IsNullOrWhiteSpace(contractByDiscountCard?.PriceType) && _priceListType != contractByDiscountCard.PriceType)
						{
							PriceListTypeEditValueChanged(contractByDiscountCard.PriceType);
						}
						else if (!Settings.ParametersService.AlwaysCalcDiscountAuto() && (!Settings.IsOffline || !Settings.ParametersService.CalcDiscountAuto()))
						{
							CalculateDiscount();
						}
					}
					else if (!string.IsNullOrWhiteSpace(contractByDiscountCard?.PriceType) && _priceListType != contractByDiscountCard.PriceType)
					{
						PriceListTypeEditValueChanged(contractByDiscountCard.PriceType);
					}
				}
				CalculateDocumentSumma();
				UpdateAccumulatedBonusText();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FillBuyerFields(string cardNumber, PartnerCacheRow partnerCacheRow, PartnerCacheRow.ContractRow partnerContract, bool byTaxCode = false)
	{
		ModifyPartnerCardButtonText(partnerContract.Contract);
		_contract = partnerContract.Contract;
		_partnerID = partnerCacheRow.Id;
		_partnerCode = partnerCacheRow.PartCode;
		_isBonusCard = partnerContract.IsBonusCard;
		DateOnly value = Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly();
		_isPartBonusCardValid = (((!partnerContract.BonusCardValidPeriodStart.HasValue || (partnerContract.BonusCardValidPeriodStart.HasValue && partnerContract.BonusCardValidPeriodStart <= value)) && (!partnerContract.BonusCardValidPeriodEnd.HasValue || (partnerContract.BonusCardValidPeriodEnd.HasValue && partnerContract.BonusCardValidPeriodEnd >= value))) ? true : false);
		if (_connectTob2barmsoftDiscountServer)
		{
			_isPartBonusCardValid = _isPartBonusCardValid && partnerContract.BonusCardValidPeriodStart.HasValue;
		}
		bool bonusCardCanceled = partnerContract.BonusCardCanceled;
		if (_docIsSale)
		{
			if (bonusCardCanceled)
			{
				ClearPartnerCard();
				if (Settings.ParametersService.UseBonus() && _isBonusCard)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsCanceled);
				}
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsCanceled);
			}
			if (!_isPartBonusCardValid)
			{
				ClearPartnerCard();
				if (Settings.ParametersService.UseBonus() && _isBonusCard && _connectTob2barmsoftDiscountServer)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsNotValidForBSUser);
				}
				if (Settings.ParametersService.UseBonus() && _isBonusCard)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsNotValid);
				}
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsNotValid);
			}
		}
		if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent)
		{
			_partContractBonusPercentOrPoint = partnerContract.Bonus;
		}
		else
		{
			_partContractBonusPercentOrPoint = partnerContract.Point;
		}
		if (Settings.ParametersService.DiscountUse())
		{
			if (_docIsSale && Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.IsOffline)
			{
				AutoCalculateDiscount();
				CalculateDocumentSumma();
			}
			BonusInfo.TB_DiscountPercent.Text = partnerContract.Percent.CurrencyFormat(6, 2);
		}
		if (byTaxCode)
		{
			BonusInfo.TB_CardNumber.Text = ArmSoft.SME.Common.Resources.OptionsTaxCode + " - " + cardNumber;
		}
		else
		{
			BonusInfo.TB_CardNumber.Text = cardNumber;
		}
		BonusInfo.TB_CardHolderName.Text = partnerCacheRow.Caption + " (" + partnerContract.Caption + ")";
		if (Settings.IsOffline)
		{
			_currentBonusRemain = 0m;
		}
		else if (Settings.ParametersService.UseBonus() && _isBonusCard && !string.IsNullOrWhiteSpace(_contract))
		{
			if (_connectTob2barmsoftDiscountServer)
			{
				_currentBonusRemain = DiscountServerService.GetCardBalance(cardNumber);
			}
			else
			{
				SummCurrSummNccPair accRem = TradeSettings.CreateTradeClient().CommonServices.GetAccRem(CommonConst.AccountingsTypes.Bonus, _contract, _partnerID, _currCode, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly());
				_currentBonusRemain = accRem.SummNcc;
			}
		}
		else
		{
			_currentBonusRemain = 0m;
		}
		BonusInfo.TB_Bonus.Text = _currentBonusRemain.CurrencyFormat(16, 2);
		SetBuyerInfoToPoleDisplay();
	}

	private void FillPartnerCardInfo(string cardNumber, PartnerCacheRow partnerCacheRow, PartnerCacheRow.ContractRow partnerContract, bool byTaxCode = false)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(cardNumber))
			{
				ClearPartnerCard();
			}
			else if (partnerContract != null)
			{
				FillBuyerFields(cardNumber, partnerCacheRow, partnerContract, byTaxCode);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ClearPartnerCard()
	{
		BonusInfo.TB_CardNumber.Text = "";
		BonusInfo.TB_CardHolderName.Text = "";
		BonusInfo.TB_DiscountPercent.Text = 0m.CurrencyFormat(6, 2);
		BonusInfo.TB_Bonus.Text = 0m.CurrencyFormat(16, 2);
		BonusInfo.TB_AccumulatedBonus.Text = 0m.CurrencyFormat(16, 2);
		BonusInfo.TB_AddedBonus.Text = 0m.CurrencyFormat(16, 2);
		ModifyPartnerCardButtonText(CodeTextEdit.Text);
		_contract = "";
		_partnerID = -1;
		_partnerCode = "";
		_isBonusCard = false;
		_partContractBonusPercentOrPoint = 0m;
		_priceListType = Settings.ParametersService.DefaultMTCheckPriceType();
		_priceListTypeRoundDgr = TradeSettings.TradeCacheService.PriceType.GetItem(_priceListType)?.Round ?? "";
		if (_docIsSale && Settings.ParametersService.DiscountUse() && Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.IsOffline)
		{
			AutoCalculateDiscount();
			GridControl.RefreshData();
			CalculateDocumentSumma();
		}
		else
		{
			ClearPartnerRelatedSumms();
		}
		SetBuyerInfoToPoleDisplay();
	}

	private void ModifyPartnerCardButtonText(string cardNum)
	{
		if (!string.IsNullOrWhiteSpace(cardNum))
		{
			if (TouchMode == POSScreenModesSE.TouchScreen.Value)
			{
				_saleTouchModeButtonsPadUserControl.ButtonCardCaption.Text = ArmSoft.SME.Common.Resources.ClearWord;
				_saleTouchModeButtonsPadUserControl.ButtonCardCaption.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#fb7376" : "#dd0326");
				_saleTouchModeButtonsPadUserControl.ButtonCardCaption1.Text = ArmSoft.SME.Trade.Common.Resources.TheCard;
				_saleTouchModeButtonsPadUserControl.ButtonCardCaption1.Foreground = _saleTouchModeButtonsPadUserControl.ButtonCardCaption.Foreground;
			}
			else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
			{
				_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardCaption.Text = ArmSoft.SME.Common.Resources.ClearWord;
				_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardCaption.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#fb7376" : "#dd0326");
				_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardCaption1.Text = ArmSoft.SME.Trade.Common.Resources.TheCard;
				_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardCaption1.Foreground = _saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardCaption.Foreground;
			}
		}
		else if (TouchMode == POSScreenModesSE.TouchScreen.Value)
		{
			_saleTouchModeButtonsPadUserControl.ButtonCardCaption.Text = ArmSoft.SME.Trade.Common.Resources.Buyers;
			_saleTouchModeButtonsPadUserControl.ButtonCardCaption.Foreground = _saleTouchModeButtonsPadUserControl.ButtonCash.Foreground;
			_saleTouchModeButtonsPadUserControl.ButtonCardCaption1.Text = ArmSoft.SME.Trade.Common.Resources.Card;
			_saleTouchModeButtonsPadUserControl.ButtonCardCaption1.Foreground = _saleTouchModeButtonsPadUserControl.ButtonCash.Foreground;
		}
		else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
		{
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardCaption.Text = ArmSoft.SME.Trade.Common.Resources.Buyers;
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardCaption.Foreground = _saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCash.Foreground;
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardCaption1.Text = ArmSoft.SME.Trade.Common.Resources.Card;
			_saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCardCaption1.Foreground = _saleTouchModeWithFavoritesButtonsPadUserControl.ButtonCash.Foreground;
		}
	}

	private void PriceListTypeEditValueChanged(string priceListType)
	{
		bool flag = false;
		if (_saleTotalAmmount != 0m)
		{
			if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		_priceListType = priceListType;
		_priceListTypeRoundDgr = TradeSettings.TradeCacheService.PriceType.GetItem(_priceListType)?.Round ?? "";
		ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_priceListType, out var priceListRowVATType, out var priceListRowCurr);
		UpdateSaleRowPrices(StatusBar.TB_Time.Text.ToDateOnly(), priceListRowCurr, _priceListTypeRoundDgr);
		if (priceListRowVATType != Methods.VATAndRetailVATMapping(_vatType, oppositeMapping: true))
		{
			VATTypeValidate(priceListRowVATType);
		}
		if (!_isRetail && string.IsNullOrWhiteSpace(_discountPriceListType))
		{
			CalculateDiscountsAndTotals();
			if (!Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.ParametersService.CalcDiscountAuto() && Convert.ToDecimal(BonusInfo.TB_DiscountPercent.Text) != 0m)
			{
				CalculateDiscount();
			}
		}
	}

	protected void CalculateDiscountsAndTotals()
	{
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			materialsRows.DiscountPrice = (materialsRows.Price * (1m - materialsRows.Discount / 100m)).RoundAs(2);
			materialsRows.Summa = (materialsRows.Quantity * materialsRows.DiscountPrice).RoundAs(2);
			materialsRows.DiscountSumma = ((materialsRows.Quantity * materialsRows.Price).RoundAs(2) - materialsRows.Summa).RoundAs(2);
		}
	}

	private void UpdateSaleRowPrices(DateOnly docDate, string priceTypeCurr, string priceTypeRoundDegree)
	{
		if (_materialsRowsList.Count == 0)
		{
			return;
		}
		if (priceTypeCurr != "AMD")
		{
			if (Settings.IsOffline)
			{
				CurrentExchangeRatesCacheRow item = Settings.CoreCacheService.CurrentExchangeRates.GetItem(_currCode);
				if (item != null)
				{
					_exchangeRate.Rate = item.Rate;
					_exchangeRate.Base = item.Base;
				}
			}
			else
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode, StatusBar.TB_Time.Text.ToDateOnly());
				if (exchangeRates != null)
				{
					_exchangeRate.Rate = exchangeRates.Rate;
					_exchangeRate.Base = exchangeRates.Base;
				}
			}
		}
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			PricesDataProvider.DataRow dataRow = new PricesDataProvider.DataRow();
			if (Settings.IsOffline)
			{
				dataRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForGivenPriceListAndVATTypeFromCache(_priceListType, _vatType, materialsRows.ItemCode, materialsRows.UnitCode);
			}
			else
			{
				dataRow = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_priceListType, materialsRows.ItemID, materialsRows.UnitCode, docDate);
			}
			if (priceTypeCurr == _currCode)
			{
				if (_isRetail)
				{
					if (string.IsNullOrWhiteSpace(_discountPriceListType))
					{
						materialsRows.DiscountPrice = dataRow.Price;
					}
				}
				else
				{
					materialsRows.Price = dataRow.Price;
				}
			}
			else if (priceTypeCurr == "AMD")
			{
				if (_isRetail)
				{
					if (string.IsNullOrWhiteSpace(_discountPriceListType))
					{
						materialsRows.DiscountPrice = (dataRow.Price / _exchangeRate.Rate * _exchangeRate.Base).RoundAs(2);
					}
				}
				else
				{
					materialsRows.Price = (dataRow.Price / _exchangeRate.Rate * _exchangeRate.Base).RoundAs(2);
				}
			}
			else if (_isRetail)
			{
				if (string.IsNullOrWhiteSpace(_discountPriceListType))
				{
					materialsRows.DiscountPrice = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(dataRow.Price, _exchangeRate);
				}
			}
			else
			{
				materialsRows.Price = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(dataRow.Price, _exchangeRate);
			}
			if (_isRetail)
			{
				if (Settings.ParametersService.DiscountUse())
				{
					materialsRows.DiscountPrice = Methods.RoundPrice(materialsRows.DiscountPrice, priceTypeRoundDegree);
					if (materialsRows.Price == 0m && string.IsNullOrWhiteSpace(_discountPriceListType))
					{
						MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(materialsRows.ItemCode);
						if (materialsRows.Discount == 0m)
						{
							materialsRows.Discount = item2.Discount;
						}
						else if (item2.Discount != 0m)
						{
							materialsRows.Discount = item2.Discount;
						}
						if (materialsRows.Discount != 0m)
						{
							materialsRows.Price = (materialsRows.DiscountPrice * 100m / (100m - materialsRows.Discount)).RoundAs(2);
						}
						else
						{
							materialsRows.Price = materialsRows.DiscountPrice;
						}
					}
					else
					{
						materialsRows.Discount = (100m * (materialsRows.Price - materialsRows.DiscountPrice) / materialsRows.Price).RoundAs(2);
					}
				}
				materialsRows.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(materialsRows.DiscountPrice, materialsRows.Quantity);
				materialsRows.DiscountSumma = ((materialsRows.Quantity * materialsRows.Price).RoundAs(2) - materialsRows.Summa).RoundAs(2);
			}
			else
			{
				materialsRows.Price = Methods.RoundPrice(materialsRows.Price, priceTypeRoundDegree);
				materialsRows.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(materialsRows.Price, materialsRows.Quantity);
			}
		}
	}

	private void VATTypeValidate(string priceListRowVATType)
	{
		decimal saleTotalAmmount = 0m;
		if (_materialsRowsList.Count <= 0)
		{
			return;
		}
		if (!(priceListRowVATType == "1"))
		{
			if (priceListRowVATType == "2")
			{
				foreach (SaleRow materialsRows in _materialsRowsList)
				{
					saleTotalAmmount += materialsRows.Summa;
					if (!_isRetail)
					{
						if (_vatType.InList("1", "3", "4"))
						{
							materialsRows.Price -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(materialsRows.Price, "5", materialsRows.VAT.Value);
							materialsRows.Price = Methods.RoundPrice(materialsRows.Price, _priceListTypeRoundDgr);
						}
						materialsRows.Summa = (materialsRows.Quantity * materialsRows.Price).RoundAs(2);
					}
				}
				_saleTotalAmmount = saleTotalAmmount;
			}
		}
		else
		{
			foreach (SaleRow materialsRows2 in _materialsRowsList)
			{
				if (!_isRetail)
				{
					if ((_vatType == "2") | (_vatType == "5"))
					{
						materialsRows2.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(materialsRows2.Price, "1", materialsRows2.VAT.Value);
						materialsRows2.Price = Methods.RoundPrice(materialsRows2.Price, _priceListTypeRoundDgr);
					}
					materialsRows2.Summa = (materialsRows2.Quantity * materialsRows2.Price).RoundAs(2);
				}
				saleTotalAmmount += materialsRows2.Summa;
			}
			_saleTotalAmmount = saleTotalAmmount;
		}
		if (Settings.ParametersService.DiscountUse() && Settings.ParametersService.AlwaysCalcDiscountAuto())
		{
			AutoCalculateDiscount();
		}
	}

	private void OnSaleConsultantCardButtonClick()
	{
		try
		{
			if (!_docIsSale && _baseDocISN != Guid.Empty)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowChangeCardForReturn);
			}
			CodeTextEdit.DoValidate();
			if (!string.IsNullOrWhiteSpace(_saleConsultantCode))
			{
				_saleConsultantCode = "";
				StatusBar.TB_SaleConsultant.Text = "";
			}
			else if (!string.IsNullOrWhiteSpace(CodeTextEdit.Text))
			{
				EmployeeCacheRow employeeCacheRow = TradeSettings.TradeCacheService.Employee.GetEmployeeByCard(CodeTextEdit.Text.Trim()) ?? throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidCardNotExist, CodeTextEdit.Text));
				_saleConsultantCode = employeeCacheRow.EmployeeCode;
				StatusBar.TB_SaleConsultant.Text = employeeCacheRow.Caption;
			}
			ModifySaleConsultantCardButtonText(_saleConsultantCode);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			CodeTextEdit.Text = "";
			_realText = "";
		}
	}

	private bool ReadSaleConsultantMagneticCard(string cardNumber)
	{
		try
		{
			PartnerCacheRow partnerCacheRow;
			if (string.IsNullOrWhiteSpace(cardNumber))
			{
				_saleConsultantCode = "";
				StatusBar.TB_SaleConsultant.Text = "";
			}
			else if (TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(cardNumber, out partnerCacheRow) == null)
			{
				EmployeeCacheRow employeeByCard = TradeSettings.TradeCacheService.Employee.GetEmployeeByCard(cardNumber);
				if (employeeByCard == null)
				{
					return false;
				}
				_saleConsultantCode = employeeByCard.EmployeeCode;
				StatusBar.TB_SaleConsultant.Text = employeeByCard.Caption;
				ModifySaleConsultantCardButtonText(_saleConsultantCode);
			}
			return true;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		return true;
	}

	private void ModifySaleConsultantCardButtonText(string saleConsultantCode)
	{
		if (!string.IsNullOrWhiteSpace(saleConsultantCode))
		{
			if (TouchMode == POSScreenModesSE.TouchScreen.Value)
			{
				_saleTouchModeButtonsPadUserControl.ButtonSaleConsultantCaption.Text = ArmSoft.SME.Trade.Common.Resources.SaleConsultantCaption;
				_saleTouchModeButtonsPadUserControl.ButtonSaleConsultantCaption1.Text = ArmSoft.SME.Trade.Common.Resources.SaleConsultantCaption1;
				_saleTouchModeButtonsPadUserControl.ButtonSaleConsultantCaption1.Visibility = Visibility.Visible;
				_saleTouchModeButtonsPadUserControl.ButtonSaleConsultantCaption.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#fb7376" : "#dd0326");
				_saleTouchModeButtonsPadUserControl.ButtonSaleConsultantCaption1.Foreground = _saleTouchModeButtonsPadUserControl.ButtonSaleConsultantCaption.Foreground;
			}
		}
		else if (TouchMode == POSScreenModesSE.TouchScreen.Value)
		{
			_saleTouchModeButtonsPadUserControl.ButtonSaleConsultantCaption.Text = ArmSoft.SME.Trade.Common.Resources.SalesConsultant;
			_saleTouchModeButtonsPadUserControl.ButtonSaleConsultantCaption1.Text = "";
			_saleTouchModeButtonsPadUserControl.ButtonSaleConsultantCaption1.Visibility = Visibility.Collapsed;
			_saleTouchModeButtonsPadUserControl.ButtonSaleConsultantCaption.Foreground = _saleTouchModeButtonsPadUserControl.ButtonCash.Foreground;
		}
	}

	private void SetStatusBar_SaleConsultant(string saleConsultant = null)
	{
		if (string.IsNullOrWhiteSpace(saleConsultant))
		{
			saleConsultant = Settings.ParametersService.DefaultSalesConsultant();
		}
		EmployeeCacheRow item = TradeSettings.TradeCacheService.Employee.GetItem(saleConsultant);
		if (item == null)
		{
			_saleConsultantCode = "";
			StatusBar.TB_SaleConsultant.Text = "";
		}
		else
		{
			_saleConsultantCode = item.EmployeeCode;
			StatusBar.TB_SaleConsultant.Text = item.Caption;
		}
	}

	private void TimerEventHandler(object sender, EventArgs e)
	{
		StatusBar.RefreshTime();
	}

	private void AutoSaveEventHandler(object sender, EventArgs e)
	{
		try
		{
			InnerAutoSave();
		}
		catch
		{
		}
	}

	private void SaleHeader_ButtonClick(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		ButtonClick(e.ButtonCommand);
	}

	private void MaterialInfo_ButtonClick(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		ButtonClick(e.ButtonCommand);
	}

	private void SaleButtonsUserControl_ButtonClick(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		ButtonClick(e.ButtonCommand);
	}

	private void SaleTouchButtonsPadUserControl_ButtonClick(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.Numeric)
		{
			CodeTextEdit.EditValue = CodeTextEdit.Text + ((SaleWindowHelper.ButtonNumericClickEventArgs)e).ButtonText;
			CodeTextEdit_FocusAndMoveCursorToEnd();
		}
		else
		{
			ButtonClick(e.ButtonCommand);
		}
	}

	private void SaleTouchButtonsPad1BaseUserControl_ButtonClick(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.Numeric)
		{
			CodeTextEdit.EditValue = CodeTextEdit.Text + ((SaleWindowHelper.ButtonNumericClickEventArgs)e).ButtonText;
			CodeTextEdit_FocusAndMoveCursorToEnd();
		}
		else
		{
			ButtonClick(e.ButtonCommand);
		}
	}

	private void SaleTouchButtonsPad1AltUserControl_ButtonClick(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.Numeric)
		{
			CodeTextEdit.EditValue = CodeTextEdit.Text + ((SaleWindowHelper.ButtonNumericClickEventArgs)e).ButtonText;
			CodeTextEdit_FocusAndMoveCursorToEnd();
		}
		else
		{
			ButtonClick(e.ButtonCommand);
		}
	}

	private void ButtonClick(SaleWindowHelper.ButtonCommands buttonCommand)
	{
		try
		{
			switch (buttonCommand)
			{
			case SaleWindowHelper.ButtonCommands.PrePayECR:
				OnPrintPrepaymentCheck();
				break;
			case SaleWindowHelper.ButtonCommands.UseBonus:
				OnUseBonuseButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.BarCode:
				OnBarCodeButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.Code:
				OnCodeButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.QTY:
				OnQuantityButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.Price:
				OnPriceButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.Cash:
				OnCashPaymentButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.NotCash:
				if (Settings.ParametersService.MTBillPlCardPaymentShowButton())
				{
					OnPlCardPaymentButtonClick();
				}
				break;
			case SaleWindowHelper.ButtonCommands.ClearPayment:
				OnClearPaymentButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.Card:
				OnDiscountButtonClickBonusCardButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.PrintCheck:
				if (Settings.ParametersService.MTBillPrintECRCheckShowButton())
				{
					OnPrintCheckButtonClick();
				}
				break;
			case SaleWindowHelper.ButtonCommands.PrintWithCount:
				if (Settings.ParametersService.AllowPrintMTCheck() & Settings.ParametersService.MTBillPrintBillShowButton())
				{
					OnPrintWithCountButtonClick();
				}
				break;
			case SaleWindowHelper.ButtonCommands.Other:
				OnOtherButtonsButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.DeleteRow:
				OnDeleteRowButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.Defer:
				OnDeferButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.Continue:
				OnContinueSaleButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.AddToBonus:
				OnAddBonuseButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.Return:
				if (Settings.ParametersService.MTBillReturnBillShowButton())
				{
					OnReturnButtonClick();
				}
				break;
			case SaleWindowHelper.ButtonCommands.MTFavorites:
				if (Settings.ParametersService.MTBillFavoritesShowButton())
				{
					OnFavoriteMaterialsButtonClick();
				}
				break;
			case SaleWindowHelper.ButtonCommands.SelectMT:
				OnOpenProductsListButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.QtyPlus:
				OnQuantityPlusButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.QtyMinus:
				OnQuantityMinusButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.Discount:
				OnDiscountButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.FromPrePay:
				if (Settings.ParametersService.MTBillUsePrePaymentShowButton())
				{
					OnFromPrePaymentButtonClick();
				}
				break;
			case SaleWindowHelper.ButtonCommands.CreditSale:
				if (Settings.ParametersService.MTBillCreditShowButton())
				{
					OnCreditSaleButtonClick();
				}
				break;
			case SaleWindowHelper.ButtonCommands.SaleConsultant:
				OnSaleConsultantCardButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.Save:
				OnSaveSaleClick();
				break;
			case SaleWindowHelper.ButtonCommands.Exit:
				if (_materialsRowsList.Count > 0)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.BeforExitStoreOrDefer);
				}
				DialogResult = true;
				DeleteAutoSavedDocument();
				break;
			case SaleWindowHelper.ButtonCommands.BackSpace:
				if (CodeTextEdit.Text.Length > 0)
				{
					CodeTextEdit.EditValue = CodeTextEdit.Text.Left(CodeTextEdit.Text.Length - 1);
					CodeTextEdit_FocusAndMoveCursorToEnd();
				}
				break;
			case SaleWindowHelper.ButtonCommands.C:
				CodeTextEdit.EditValue = "";
				CodeTextEdit_FocusAndMoveCursorToEnd();
				break;
			case SaleWindowHelper.ButtonCommands.FocusOnInputField:
				CodeTextEdit.Focus();
				break;
			case SaleWindowHelper.ButtonCommands.TaxCode:
				OnTaxCodeButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.FindBuyer:
				OnFindBuyerButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.LastECRCheck:
				OnLastECRCheckButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.QtyUnit:
				OnSwitchUnitButtonClick();
				break;
			default:
				ArmSoft.SME.Core.UI.MessageBox.Show($"{buttonCommand} call", ArmSoft.SME.Trade.Common.Resources.ProgramName);
				break;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OnSwitchUnitButtonClick()
	{
		SaleRow saleRow = (SaleRow)GridControl.CurrentItem;
		if (saleRow == null)
		{
			return;
		}
		List<MaterialCacheRow.QntUnitRow> qntUnits = TradeSettings.TradeCacheService.Material.GetItem(saleRow.ItemCode).QntUnits;
		if (qntUnits.Count <= 1)
		{
			return;
		}
		int num = 0;
		using (List<MaterialCacheRow.QntUnitRow>.Enumerator enumerator = qntUnits.GetEnumerator())
		{
			while (enumerator.MoveNext() && !(enumerator.Current.QntUnit == saleRow.UnitCode))
			{
				num++;
			}
		}
		string unitCode = saleRow.UnitCode;
		if (num + 1 == qntUnits.Count)
		{
			saleRow.UnitCode = qntUnits[0].QntUnit;
		}
		else
		{
			saleRow.UnitCode = qntUnits[num + 1].QntUnit;
		}
		UnitEdit(saleRow, unitCode);
		GridControl.RefreshData();
	}

	private void OnSalesDocumentsHistoryButtonClick()
	{
		DateOnly dateOnly = StatusBar.TB_Time.Text.ToDateOnly();
		ECRChecksJournalInsidePOS dropDownView = new ECRChecksJournalInsidePOS(this)
		{
			StartDate = ((!_shiftBeginDate.HasValue) ? dateOnly : Convert.ToDateTime(_shiftBeginDate).ToDateOnly()),
			EndDate = dateOnly,
			CurrCode = "AMD",
			CashCode = _defaultCash,
			ShiftNumber = _currentShiftNum,
			DocState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Processed.ValueUI
		};
		string value = (_shiftAlreadyClosed ? ArmSoft.SME.Common.Resources.IsClosed : ArmSoft.SME.Common.Resources.IsOpened);
		string title = $"{ArmSoft.SME.Common.Resources.ECRChecksJournal} - {ArmSoft.SME.Trade.Common.Resources.CashierShift} N {_currentShiftNum}, {_shiftBeginDate} ({value})";
		new SalesDocumentsHistoryWindow(this, title, dropDownView).ShowDialog();
	}

	private void SaleWindow_Activated(object sender, EventArgs e)
	{
		if (_isFirstActivate)
		{
			double num = ((!FullScreen) ? Width : ((double)Screen.PrimaryScreen.Bounds.Width));
			if (TouchMode == POSScreenModesSE.TouchScreen.Value)
			{
				CodeTextEdit.Height = 40.0;
				if (num == 1024.0)
				{
					TotalInfo.TB_Total.FontSize = 44.0;
				}
				else if (num == 1152.0)
				{
					TotalInfo.TB_Total.FontSize = 59.0;
				}
				else if (num == 1280.0)
				{
					TotalInfo.TB_Total.FontSize = 70.0;
				}
				else if (num >= 1360.0)
				{
					TotalInfo.TB_Total.FontSize = 80.0;
				}
			}
			else if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
			{
				TotalInfo.TB_Discount.FontSize = 18.0;
				TotalInfo.TB_Cash.FontSize = 18.0;
				TotalInfo.TB_NoCash.FontSize = 18.0;
				TotalInfo.TB_CreditSale.FontSize = 18.0;
				TotalInfo.TB_MustPay.FontSize = 18.0;
				TotalInfo.TB_FromPrePay.FontSize = 18.0;
				TotalInfo.TB_FromBonus.FontSize = 18.0;
				TotalInfo.TB_DiscountCaption.FontSize = 16.0;
				TotalInfo.TB_DiscountCaption.FontWeight = FontWeights.Regular;
				TotalInfo.TB_CashCaption.FontSize = 16.0;
				TotalInfo.TB_CashCaption.FontWeight = FontWeights.Regular;
				TotalInfo.TB_NoCashCaption.FontSize = 16.0;
				TotalInfo.TB_NoCashCaption.FontWeight = FontWeights.Regular;
				TotalInfo.TB_CreditSaleCaption.FontSize = 16.0;
				TotalInfo.TB_CreditSaleCaption.FontWeight = FontWeights.Regular;
				TotalInfo.TB_MustPayCaption.FontSize = 16.0;
				TotalInfo.TB_MustPayCaption.FontWeight = FontWeights.Regular;
				TotalInfo.TB_FromPrepayCaption.FontSize = 16.0;
				TotalInfo.TB_FromPrepayCaption.FontWeight = FontWeights.Regular;
				TotalInfo.TB_FromBonusCaption.FontSize = 16.0;
				TotalInfo.TB_FromBonusCaption.FontWeight = FontWeights.Regular;
				TotalInfo.TB_Total.FontSize = 40.0;
				TotalInfo.TB_Total.FontWeight = FontWeights.Regular;
			}
			_isFirstActivate = false;
		}
		CodeTextEdit.Focus();
	}

	private void SaleWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
	{
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (HotKeys.WithModifier.Control_D.Matches(this, e))
			{
				e.Handled = true;
				OnDeleteRowButtonClick();
			}
			else if (HotKeys.WithModifier.Control_Shift_D.Matches(this, e))
			{
				if (Settings.ParametersService.POSDeferSaleHotKey() == HotKeysSE.Ctrl_Shift_D)
				{
					e.Handled = true;
					OnDeferButtonClick();
					return;
				}
			}
			else if (HotKeys.WithModifier.Control_Enter.Matches(this, e) || HotKeys.WithModifier.Control_K.Matches(this, e))
			{
				e.Handled = true;
				OnSaveSaleClick();
			}
			else if (HotKeys.WithModifier.Alt_F4.Matches(this, e))
			{
				e.Handled = true;
				if (_materialsRowsList != null && _materialsRowsList.Count > 0)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.BeforExitStoreOrDefer);
				}
				DeleteAutoSavedDocument();
			}
			else if (HotKeys.Functional.F12.Matches(this, e) || HotKeys.WithModifier.Shift_F12.Matches(this, e) || HotKeys.WithModifier.Control_L.Matches(this, e) || HotKeys.WithModifier.Control_Shift_L.Matches(this, e) || HotKeys.WithModifier.Control_B.Matches(this, e) || HotKeys.WithModifier.Control_Shift_B.Matches(this, e) || HotKeys.WithModifier.Control_J.Matches(this, e))
			{
				e.Handled = true;
			}
			string key = KeysHelper.ToKeyGesture(e.Key);
			if (_buttonCommandByKeyGesturesDictionary.ContainsKey(key))
			{
				e.Handled = true;
				ButtonClick(_buttonCommandByKeyGesturesDictionary[key]);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OnRowInfoChanged(SaleRow row)
	{
		CleanInformationPanel();
		if (row == null)
		{
			if (Settings.ParametersService.ShowMTBillInfoPanel())
			{
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearInfoPanel(CleanInformationPanel), Array.Empty<object>());
			}
			if (Settings.ParametersService.ShowPoleDisplay() && TradeSettings.PoleDisplay != null)
			{
				ClearPoleDisplay(_partnerID == -1);
				int num = CalcMTBillListRowCount(includeDeleted: false);
				if (num != 0)
				{
					SetTotalInfoToPoleDisplay();
					SetRowCountInfoToPoleDisplay(num);
				}
			}
			MaterialInfo.BE_Discount.Visibility = Visibility.Collapsed;
			MaterialInfo.BE_QtyMinus.Visibility = Visibility.Collapsed;
			MaterialInfo.BE_QtyPlus.Visibility = Visibility.Collapsed;
		}
		else
		{
			MaterialInfo.BE_QtyMinus.Visibility = Visibility.Visible;
			MaterialInfo.BE_QtyPlus.Visibility = Visibility.Visible;
			if (Settings.ParametersService.DiscountUse())
			{
				if (Settings.ParametersService.CalcDiscountAuto() || Settings.ParametersService.AlwaysCalcDiscountAuto() || Settings.ParametersService.DiscountExtensionUse() || !_docIsSale)
				{
					MaterialInfo.BE_Discount.Visibility = Visibility.Collapsed;
				}
				else if (Settings.ParametersService.AllowPOSChangePrice())
				{
					MaterialInfo.BE_Discount.Visibility = Visibility.Visible;
				}
			}
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToInfoPanel(SetRowInformationToPanel), new object[1] { row });
			if (Settings.ParametersService.ShowPoleDisplay() && TradeSettings.PoleDisplay != null)
			{
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToPoleDisplay(TradeSettings.PoleDisplay.SetRowInfo), new object[1] { row });
			}
		}
		ChangeMarkingStatus(row);
	}

	private void InnerView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		RowChanged();
		OnFocusedRowHandleChanged();
	}

	private void InnerView_GotFocus(object sender, RoutedEventArgs e)
	{
		RowChanged();
		OnFocusedRowHandleChanged();
	}

	private void InnerView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		TableView innerView = InnerView;
		object originalSource = e.OriginalSource;
		TableViewHitInfo tableViewHitInfo = innerView.CalcHitInfo((DependencyObject)((originalSource is DependencyObject) ? originalSource : null));
		if (tableViewHitInfo.InRowCell)
		{
			SaleWindowHelper.CurrentFocusedField currentFocusedField = SaleWindowHelper.CurrentFocusedField.None;
			if (tableViewHitInfo.Column == Quantity)
			{
				currentFocusedField = SaleWindowHelper.CurrentFocusedField.Quantity;
			}
			else if (tableViewHitInfo.Column == Price)
			{
				currentFocusedField = SaleWindowHelper.CurrentFocusedField.SalePrice;
			}
			else if (tableViewHitInfo.Column == Summa)
			{
				currentFocusedField = SaleWindowHelper.CurrentFocusedField.SaleSumm;
			}
			SaleRow saleRow = (SaleRow)GridControl.GetRow(tableViewHitInfo.RowHandle);
			if (!saleRow.IsDeleted && currentFocusedField != SaleWindowHelper.CurrentFocusedField.None)
			{
				EditRow(saleRow, currentFocusedField);
			}
		}
	}

	public void OnScanner(string scannerData)
	{
		CodeTextEdit.Text = MarkingHelper.CleanScannerSymbols(scannerData);
		ReadBarCode(CodeTextEdit.Text);
		CodeTextEdit.Text = "";
		_realText = "";
	}

	public void OnMagneticCardReader(string cardData)
	{
		try
		{
			bool flag = true;
			if (!string.IsNullOrWhiteSpace(_partnerCode))
			{
				flag = Settings.ParametersService.AllowPOSRemoveCard();
			}
			if (flag)
			{
				ReadMagneticCard(cardData);
			}
			else if (new UserAuthenticationDialogWindow(this, ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSRemoveCard.Id).ShowDialog().Value)
			{
				ReadMagneticCard(cardData);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		try
		{
			if (_materialsRowsList != null)
			{
				if (!_shiftAlreadyClosed & (_materialsRowsList.Count > 0))
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.BeforExitStoreOrDefer);
				}
				e.Cancel = false;
			}
			else
			{
				e.Cancel = false;
			}
			StopDocumentDateTimer();
			StopAutoSaveTimer();
		}
		catch (Exception ex)
		{
			e.Cancel = true;
			Settings.ErrorHandler(ex);
		}
	}

	private void CodeTextEdit_PreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		_realText += e.Text;
	}

	private void CodeTextEdit_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
	{
		try
		{
			if (HotKeys.Single.Enter.Matches(this, e) && !CodeTextEdit.IsReadOnly && !string.IsNullOrWhiteSpace(CodeTextEdit.Text))
			{
				e.Handled = true;
				CodeTextEdit.Text = MarkingHelper.CleanScannerSymbols(_realText.Contains('\u001d') ? _realText : CodeTextEdit.Text);
				OnBarCodeButtonClick();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			_realText = "";
		}
	}

	private void SaleOtherButtonsPadWindowButton_Click(object sender, SaleWindowHelper.ButtonNumericClickEventArgs e)
	{
		try
		{
			switch (e.ButtonCommand)
			{
			case SaleWindowHelper.ButtonCommands.PrePayECR:
				OnPrintPrepaymentCheck();
				break;
			case SaleWindowHelper.ButtonCommands.PrePayRetECR:
				PrintPrePaymentReturnECR();
				break;
			case SaleWindowHelper.ButtonCommands.CashIn:
				CashIOClick(isCashIn: true);
				break;
			case SaleWindowHelper.ButtonCommands.CashOut:
				CashIOClick(isCashIn: false);
				break;
			case SaleWindowHelper.ButtonCommands.NewBuyer:
				NewBuyerClick();
				break;
			case SaleWindowHelper.ButtonCommands.SalesDocumentsHistory:
				OnSalesDocumentsHistoryButtonClick();
				break;
			case SaleWindowHelper.ButtonCommands.EMarkReceipt:
				EMarkReceiptClick();
				break;
			case SaleWindowHelper.ButtonCommands.Card:
				GetPartnerByTaxCode(e.ButtonText);
				break;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OnDeleteRowButtonClick(object sender, RoutedEventArgs e)
	{
		OnDeleteRowButtonClick();
	}

	private void SaleWindow_Closed(object sender, EventArgs e)
	{
		try
		{
			if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
			{
				RegistryHelper.StoreToRegistry("CardSize", _favoritesTouchPadControl.CardView.FixedSize, "Document Windows\\POS");
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void BE_TouchPadChange_Click(object sender, RoutedEventArgs e)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (_baseButtonsPad)
		{
			_saleTouchModeWithFavoritesButtonsPadUserControl.Visibility = Visibility.Collapsed;
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.Visibility = Visibility.Visible;
			ButtonTouchPadImage.Source = ImageService.GetImage("keyboard_switch_2_32.svg");
			_baseButtonsPad = false;
		}
		else
		{
			_saleTouchModeWithFavoritesButtonsPadUserControl.Visibility = Visibility.Visible;
			_saleTouchModeWithFavoritesAltModeButtonsPadUserControl.Visibility = Visibility.Collapsed;
			ButtonTouchPadImage.Source = ImageService.GetImage("keyboard_switch_1_32.svg");
			_baseButtonsPad = true;
		}
		CodeTextEdit.Focus();
	}

	private void PrintTemplateSelectTouchDialogButton_Click(object sender, SaleWindowHelper.PrintButtonClickEventArgs e)
	{
		OnPrintClick(e.PrintCopies, e.PrintTemplate);
	}

	private void InitPoleDisplay(string messageForPoleDispaly = "")
	{
		if (Settings.ParametersService.ShowPoleDisplay())
		{
			if (TradeSettings.PoleDisplay == null || PresentationSource.FromVisual(TradeSettings.PoleDisplay) == null)
			{
				TradeSettings.PoleDisplay = new PoleDisplayWindow();
			}
			else
			{
				ClearPoleDisplay(clearAll: true, messageForPoleDispaly);
			}
			TradeSettings.PoleDisplay.Show();
		}
	}

	private void ClearPoleDisplay(bool clearAll, string messageForPoleDispaly = "")
	{
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearPoleDisplay(TradeSettings.PoleDisplay.ClearScreen), new object[2] { clearAll, messageForPoleDispaly });
	}

	private void SetTotalInfoToPoleDisplay()
	{
		if (TradeSettings.PoleDisplay != null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetTotalInfoToPoleDisplay(TradeSettings.PoleDisplay.SetTotalInfo), new object[8]
			{
				Convert.ToDecimal(TotalInfo.TB_Total.Text),
				_temporaryDiscountSumm,
				Convert.ToDecimal(TotalInfo.TB_Cash.Text),
				Convert.ToDecimal(TotalInfo.TB_NoCash.Text),
				Convert.ToDecimal(TotalInfo.TB_FromBonus.Text),
				Convert.ToDecimal(TotalInfo.TB_FromPrePay.Text),
				Convert.ToDecimal(TotalInfo.TB_CreditSale.Text),
				GetPaidSum() - Convert.ToDecimal(TotalInfo.TB_Total.Text)
			});
		}
	}

	private void SetRowCountInfoToPoleDisplay(int rowCount = 0)
	{
		if (TradeSettings.PoleDisplay != null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowCountInfoToPoleDisplay(TradeSettings.PoleDisplay.SetRowCountInfo), new object[1] { rowCount });
		}
	}

	private void SetBuyerInfoToPoleDisplay()
	{
		if (TradeSettings.PoleDisplay == null)
		{
			return;
		}
		decimal num = 0m;
		decimal num2 = 0m;
		if (!Settings.IsOffline && Settings.ParametersService.UseBonus())
		{
			num = _materialsRowsList.Sum((SaleRow row) => row.AccumulatedBonus).RoundAs(2);
			if (!string.IsNullOrWhiteSpace(_contract))
			{
				decimal num3 = 0m;
				if (_connectTob2barmsoftDiscountServer)
				{
					PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partnerCode, _contract);
					if (contract != null)
					{
						num3 = DiscountServerService.GetCardBalance(contract.DiscountCard);
					}
				}
				else
				{
					TradeSettings.TradeCacheService.Partner.GetContract(_partnerCode, _contract, out var partnerCacheRow);
					num3 = TradeSettings.CreateTradeClient().CommonServices.GetAccRem(CommonConst.AccountingsTypes.Bonus, _contract, partnerCacheRow.Id, _currCode, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly()).SummNcc;
				}
				num2 = num3;
			}
		}
		string text = "";
		if (!string.IsNullOrWhiteSpace(_partnerCode))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partnerCode);
			if (item != null)
			{
				text = item.Caption;
			}
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetBuyerInfoToPoleDisplay(TradeSettings.PoleDisplay.SetBuyerInfo), new object[5] { _partnerCode, text, _contract, num, num2 });
	}

	private void OnLastECRCheckButtonClick()
	{
		ECR ecr = null;
		bool flag = false;
		bool flag2 = false;
		try
		{
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_defaultCash);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NotInKassa, _defaultCash));
			}
			if (string.IsNullOrWhiteSpace(item.ECROperator) || string.IsNullOrWhiteSpace(item.ECROperatorPassword))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidateECROperatorOrECROperatorPin, _defaultCash), ArmSoft.SME.Common.Resources.ECRPrintLastCheckDuplicate, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			if (!item.ECROperator.IsNumeric())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ECRPrintLastCheckDuplicate, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			ECRHelpers.InitECR(ref ecr, _defaultCash);
			if (ecr == null)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ECRPrintLastCheckDuplicate, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			ecr.Connect();
			flag = true;
			ecr.OperatorLogin(Convert.ToInt32(item.ECROperator), item.ECROperatorPassword);
			flag2 = true;
			ecr.PrintLastReceipt();
		}
		catch (Utilities.ECRException ex)
		{
			Settings.ErrorHandler(new ASException(ex.Message));
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
		}
		finally
		{
			try
			{
				if (flag)
				{
					if (flag2)
					{
						ecr.OperatorLogout();
					}
					ecr.DisConnect();
				}
			}
			catch
			{
			}
		}
	}

	private void OnPrintWithCountButtonClick()
	{
		if ((_docIsSale && !string.IsNullOrWhiteSpace(Settings.ParametersService.MTCheckTemplate())) || (_docType.Value == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value && !string.IsNullOrWhiteSpace(Settings.ParametersService.RetCheckTemplate())))
		{
			OnPrintClick(1);
		}
		else
		{
			PrintWithCount();
		}
	}

	private void PrintWithCount()
	{
		try
		{
			PrintTemplateTouchGridControl printTemplateTouchGridControl = new PrintTemplateTouchGridControl(this, new ArmSoft.SME.Core.UI.DropDowns.Template(_docType, PrintTemplateFileFormatSE.XML));
			printTemplateTouchGridControl.ButtonClick += PrintTemplateSelectTouchDialogButton_Click;
			printTemplateTouchGridControl.ShowDialog();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OnPrintClick(short printCopies = 1, string templateCodeTextEdit = "")
	{
		try
		{
			TradeDocument tradeDocument;
			if (Settings.ParametersService.PrintMTCheckAndNew() && !_saveAndPrintECR)
			{
				tradeDocument = SaveSaleDocument(newAfterStore: false);
			}
			else if (_docIsSale)
			{
				tradeDocument = FillMTBillDocument();
			}
			else
			{
				tradeDocument = ((_docType.Value != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value) ? ((MTReturnFromCustomerBase)FillRetFromCustomerDocument(ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value)) : ((MTReturnFromCustomerBase)FillMTBillReturnDocument()));
			}
			if (tradeDocument == null)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(templateCodeTextEdit))
			{
				if (tradeDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
				{
					templateCodeTextEdit = Settings.ParametersService.MTCheckTemplate();
				}
				else if (tradeDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value)
				{
					templateCodeTextEdit = Settings.ParametersService.RetCheckTemplate();
				}
			}
			if (string.IsNullOrWhiteSpace(templateCodeTextEdit))
			{
				string s = string.Empty;
				if (tradeDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
				{
					s = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTBill.Value;
				}
				else if (tradeDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value)
				{
					s = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTBillReturn.Value;
				}
				else if (tradeDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value)
				{
					s = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTReturnFromCustomer.Value;
				}
				templateCodeTextEdit = Settings.CoreCacheService.Template.GetDefaultPrintTemplate(short.Parse(s), throwExceptionNotExits: true).Name;
			}
			if (Settings.IsOffline)
			{
				try
				{
					Settings.ServiceProvider.GetRequiredService<ITemplateService>().LoadTemplateFileOffline(templateCodeTextEdit);
				}
				catch (Exception innerException)
				{
					throw new ASException(ArmSoft.SME.Common.Resources.PrintTemplateNotFound, innerException);
				}
			}
			Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(tradeDocument, PrintMode.PrintDirect, templateCodeTextEdit, CheckPrinterInfo.GetInstance().CheckPrinterName, printCopies);
			if (Settings.ParametersService.PrintMTCheckAndNew())
			{
				if (_saveAndPrintECR)
				{
					PrintCheck();
				}
				else
				{
					NewAfterStore();
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OnPrintCheckButtonClick()
	{
		PrintCheck();
		if (Settings.ParametersService.PrintECRAndProductCheck() && !Settings.ParametersService.PrintECRCheckOnCheckPrinter())
		{
			OnPrintClick(1);
		}
	}

	private void PrintCheck()
	{
		PosTerminalResponse posTerminalResponse = null;
		long poscrn = 0L;
		MTBill baseDoc = null;
		if (_ecrCheckNum == "XXXXXXXX" || _prePaymentECRCheckNum == "XXXXXXXX")
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRMessageForXXXXXXXX, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return;
		}
		if (!_docIsSale)
		{
			if (_baseDocISN == Guid.Empty)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.BaseDocNeedsForECRC, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			if (Settings.IsOffline)
			{
				baseDoc = Settings.LocalDBDocumentService.Load<MTBill>(_baseDocISN);
			}
			else
			{
				baseDoc = Settings.DocumentService.Load<MTBill>(_baseDocISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill, withoutAccessCheck: true);
			}
			if (string.IsNullOrWhiteSpace(baseDoc.ECRCheckNum))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyECRNumForBaseDoc, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			if (baseDoc.ECRCheckNum == "XXXXXXXX" || baseDoc.PrePaymentECRCheckNum == "XXXXXXXX")
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRMessageForXXXXXXXX, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
		}
		object obj = null;
		CashCacheRow cashesCacheRow = TradeSettings.TradeCacheService.Cash.GetItem(_defaultCash);
		if (!string.IsNullOrWhiteSpace(_partnerTaxCode))
		{
			if (Settings.ParametersService.ECRCheckPrintCreditAsPartial())
			{
				if (Convert.ToDecimal(TotalInfo.TB_Cash.Text) != 0m || Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) != 0m)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
					return;
				}
			}
			else if (Convert.ToDecimal(TotalInfo.TB_Cash.Text) != 0m || Convert.ToDecimal(TotalInfo.TB_CreditSale.Text) != 0m || Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) != 0m)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
		}
		else if (cashesCacheRow.CashSummLimitType == CashLimitsSE.OnlyNonCash.Value)
		{
			if (Settings.ParametersService.ECRCheckPrintCreditAsPartial())
			{
				if (Convert.ToDecimal(TotalInfo.TB_Cash.Text) != 0m || Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) != 0m)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
					return;
				}
			}
			else if (Convert.ToDecimal(TotalInfo.TB_Cash.Text) != 0m || Convert.ToDecimal(TotalInfo.TB_CreditSale.Text) != 0m || Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) != 0m)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
		}
		else if (cashesCacheRow.CashSummLimitType == CashLimitsSE.WithLimit.Value && Convert.ToDecimal(TotalInfo.TB_Total.Text) > 300000m)
		{
			if (Settings.ParametersService.ECRCheckPrintCreditAsPartial())
			{
				if (Convert.ToDecimal(TotalInfo.TB_Cash.Text) != 0m || Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) != 0m)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
					return;
				}
			}
			else if (Convert.ToDecimal(TotalInfo.TB_Cash.Text) != 0m || Convert.ToDecimal(TotalInfo.TB_CreditSale.Text) != 0m || Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) != 0m)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
		}
		if (!ECRPreliminaryChecksAndInitialization(cashesCacheRow, prepaymentCheck: false, _ecrCRN))
		{
			return;
		}
		_addedBonusValue = Convert.ToDecimal(BonusInfo.TB_AddedBonus.Text);
		_summaCashValue = Convert.ToDecimal(TotalInfo.TB_Cash.Text);
		if (!_docIsSale || (Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.DiscountAndPrePayment.Value && CalcBonusFromRows() + _addedBonusValue > 0m))
		{
			_summaCashValue -= Convert.ToDecimal(TotalInfo.TB_MustPay.Text);
		}
		_summaPlCardValue = Convert.ToDecimal(TotalInfo.TB_NoCash.Text);
		_summaPayByBonusValue = Convert.ToDecimal(TotalInfo.TB_FromBonus.Text);
		_summaCreditValue = Convert.ToDecimal(TotalInfo.TB_CreditSale.Text);
		_summaPrePaymentValue = Convert.ToDecimal(TotalInfo.TB_FromPrePay.Text);
		_calculatedBonusValue = 0m;
		_summaCashSubtractBonus = 0m;
		_summaPlCardSubtractBonus = 0m;
		_summaPrePaymentSubtractBonus = 0m;
		bool bonus = false;
		if (!Settings.IsOffline)
		{
			if (_docIsSale)
			{
				if (Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.DiscountAndPrePayment.Value)
				{
					CalcAllSummaBasedFromCalculatedBonuses();
				}
				bonus = Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.DiscountAndPrePayment.Value && _calculatedBonusValue != 0m && (_summaPlCardSubtractBonus > 0m || _summaCashSubtractBonus > 0m);
			}
			else
			{
				bonus = (!string.IsNullOrWhiteSpace(baseDoc.PrePaymentECRCheckNum) && baseDoc.PrePaymentECRCheckNum.Trim() != "XXXXXXXX") || Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.DiscountAndPrePayment.Value;
				if (bonus)
				{
					CalcAllSummaBasedFromCalculatedBonuses();
				}
				bonus = bonus && (_summaCashSubtractBonus != 0m || _summaPlCardSubtractBonus != 0m);
			}
		}
		TradeDocument tradeDocument = SaveSaleDocument(newAfterStore: false);
		if (tradeDocument == null)
		{
			return;
		}
		ECRPrintCheckWindow.ECRCheckType checkType;
		try
		{
			if (_docIsSale)
			{
				if (Settings.IsOffline)
				{
					_clonedBill = Settings.LocalDBDocumentService.Load<MTBill>(tradeDocument.ISN);
				}
				else
				{
					_clonedBill = Settings.DocumentService.Load<MTBill>(tradeDocument.ISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill, withoutAccessCheck: true);
				}
				_clonedBill.SumAddedBonus = _addedBonusValue;
				MTBill clonedBill = _clonedBill;
				decimal summaPrePaymentValue = _summaPrePaymentValue;
				decimal? summaPayByBonus = _clonedBill.SummaPayByBonus;
				clonedBill.SummaPrePayment = (decimal?)summaPrePaymentValue - summaPayByBonus;
				obj = CreatePrintRequest(cashesCacheRow);
				checkType = ECRPrintCheckWindow.ECRCheckType.Standard;
			}
			else
			{
				if (Settings.IsOffline)
				{
					_clonedBillReturn = Settings.LocalDBDocumentService.Load<MTBillReturn>(tradeDocument.ISN);
				}
				else
				{
					_clonedBillReturn = Settings.DocumentService.Load<MTBillReturn>(tradeDocument.ISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn, withoutAccessCheck: true);
				}
				foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT in _clonedBillReturn.MTList)
				{
					mT.DiscountPrice = (mT.DiscountPrice - mT.AccumulatedBonus / mT.Quantity).Value.RoundAs(2);
					mT.Discount = 100m - (mT.DiscountPrice / mT.Price * (decimal?)100).Value.RoundAs(2);
					mT.Summa = (mT.Summa - mT.AccumulatedBonus).Value.RoundAs(2);
					mT.DiscountSumma = (mT.DiscountSumma + mT.AccumulatedBonus).Value.RoundAs(2);
				}
				obj = CreateReturnRequest(cashesCacheRow);
				checkType = ECRPrintCheckWindow.ECRCheckType.Return;
			}
			if (_posTerminalType == "2" || _posTerminalType == "4")
			{
				if (_docIsSale)
				{
					posTerminalResponse = PayWithPOSTerminalIfChosen((MTBill)tradeDocument, ref poscrn);
					if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN))
					{
						Utilities.PrintReceiptRequest printReceiptRequest = (Utilities.PrintReceiptRequest)obj;
						printReceiptRequest.rrn = posTerminalResponse.RRN;
						if (!string.IsNullOrWhiteSpace(posTerminalResponse.TerminalId))
						{
							printReceiptRequest.terminalId = posTerminalResponse.TerminalId;
						}
					}
				}
				else
				{
					posTerminalResponse = RefundPOSTerminalIfChosen((MTBillReturn)tradeDocument);
					if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN))
					{
						Utilities.PrintReturnReceiptRequest printReturnReceiptRequest = (Utilities.PrintReturnReceiptRequest)obj;
						printReturnReceiptRequest.rrn = posTerminalResponse.RRN;
						if (!string.IsNullOrWhiteSpace(posTerminalResponse.TerminalId))
						{
							printReturnReceiptRequest.terminalId = posTerminalResponse.TerminalId;
						}
					}
				}
			}
		}
		catch (Exception ex) when ((ex is Arcus2Exception || ex is PossoftPayClientException) ? true : false)
		{
			MakeDocumentDraftAndReloadIt(tradeDocument.ISN);
			Settings.ErrorHandler(new ASException(ex.Message));
			return;
		}
		catch (Exception ex2)
		{
			MakeDocumentDraftAndReloadIt(tradeDocument.ISN);
			Settings.ErrorHandler(ex2);
			return;
		}
		bool flag = false;
		try
		{
			if (((_posTerminalType == "1") & bonus) && _summaPlCardSubtractBonus != 0m)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.CanNotPrintBonusAsPerPaymentWithECRPOS);
			}
			ECRPrintCheckWindow eCRPrintCheckWindow = new ECRPrintCheckWindow(this, _ecr, cashesCacheRow.ECROperator, cashesCacheRow.ECROperatorPassword, cashesCacheRow.ShowECRTracking, obj, checkType, (ECRPrintCheckWindow window, Utilities.PrintReceiptResponse response) =>
			{
				if (_docIsSale)
				{
					if (!bonus || Settings.ParametersService.PrintPrepaymentCheckSeparately())
					{
						ECRHelpers.PrintOnOtherPrinter(window, response, cashesCacheRow, bonus ? _clonedBill : tradeDocument, $"{ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value}.3");
					}
				}
				else if (!bonus || Settings.ParametersService.PrintPrepaymentCheckSeparately())
				{
					ECRHelpers.PrintOnOtherPrinter(window, response, cashesCacheRow, bonus ? _clonedBillReturn : tradeDocument, $"{ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value}.3");
				}
			}, (string checkNumber, string errorMessage) =>
			{
				int num = 1;
				List<PropertyWithValue> list = new List<PropertyWithValue>(num);
				CollectionsMarshal.SetCount(list, num);
				CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("ECRCheckNum", checkNumber);
				List<PropertyWithValue> list2 = list;
				if (bonus)
				{
					list2.Add(new PropertyWithValue("PrePaymentECRCheckNum", string.IsNullOrWhiteSpace(checkNumber) ? checkNumber : "XXXXXXXX"));
				}
				tradeDocument.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, errorMessage, tradeDocument.ISN, list2, tradeDocument.TS);
				_ecrCheckNum = checkNumber;
			});
			flag = eCRPrintCheckWindow.Succeeded;
			if (flag)
			{
				UpdateDocumentAfterCheckPrint(tradeDocument, eCRPrintCheckWindow.CheckNumber, eCRPrintCheckWindow.ECRCrn);
				if (!Settings.IsOffline)
				{
					TradeSettings.CreateTradeClient().CommonServices.SetCompanyData(tradeDocument.CashDesk, eCRPrintCheckWindow.TaxCode, eCRPrintCheckWindow.CompanyName, eCRPrintCheckWindow.CompanyAddress, eCRPrintCheckWindow.ECRCrn);
				}
				if (_docIsSale && _nonCashPaymentMethod == 0 && (_posTerminalType == "2" || _posTerminalType == "4"))
				{
					ConfirmPOSTerminalPaymentIfDone(posTerminalResponse, (MTBill)tradeDocument);
				}
			}
			if (flag & bonus)
			{
				object request;
				if (_docIsSale)
				{
					request = CreatePrePaymentRequest();
					((MTBill)tradeDocument).PrePaymentCashAmount = _summaCashSubtractBonus;
					((MTBill)tradeDocument).PrePaymentCardAmount = _summaPlCardSubtractBonus;
				}
				else
				{
					request = CreateReturnRequestForBonus(cashesCacheRow);
					((MTBillReturn)tradeDocument).PrePaymentCashOutAmount = _summaCashSubtractBonus;
					((MTBillReturn)tradeDocument).PrePaymentCardOutAmount = _summaPlCardSubtractBonus;
				}
				ECRPrintCheckWindow eCRPrintCheckWindow2 = new ECRPrintCheckWindow(this, _ecr, cashesCacheRow.ECROperator, cashesCacheRow.ECROperatorPassword, cashesCacheRow.ShowECRTracking, request, (!_docIsSale) ? ECRPrintCheckWindow.ECRCheckType.Return : ECRPrintCheckWindow.ECRCheckType.PrePayment, (ECRPrintCheckWindow window, Utilities.PrintReceiptResponse response) =>
				{
					if (_docIsSale)
					{
						if (!Settings.ParametersService.PrintPrepaymentCheckSeparately())
						{
							MTBill mTBill = (MTBill)tradeDocument;
							_clonedBill.PrePaymentECRCheckNum = mTBill.PrePaymentECRCheckNum;
							_clonedBill.ECRCheckNum = mTBill.ECRCheckNum;
						}
						ECRHelpers.PrintOnOtherPrinter(window, response, cashesCacheRow, Settings.ParametersService.PrintPrepaymentCheckSeparately() ? tradeDocument : _clonedBill, Settings.ParametersService.PrintPrepaymentCheckSeparately() ? $"{ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value}.5" : $"{ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value}.3");
					}
					else
					{
						if (!Settings.ParametersService.PrintPrepaymentCheckSeparately())
						{
							MTBillReturn mTBillReturn = (MTBillReturn)tradeDocument;
							_clonedBillReturn.PrePaymentECRCheckNum = mTBillReturn.PrePaymentECRCheckNum;
							_clonedBillReturn.ECRCheckNum = mTBillReturn.ECRCheckNum;
							_clonedBillReturn.ECRBaseCheckNum = mTBillReturn.ECRBaseCheckNum + "/" + baseDoc.PrePaymentECRCheckNum;
						}
						ECRHelpers.PrintOnOtherPrinter(window, response, cashesCacheRow, Settings.ParametersService.PrintPrepaymentCheckSeparately() ? tradeDocument : _clonedBillReturn, Settings.ParametersService.PrintPrepaymentCheckSeparately() ? $"{ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value}.5" : $"{ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value}.3");
					}
				}, (string checkNumber, string errorMessage) =>
				{
					if (checkNumber != "XXXXXXXX")
					{
						int num = 1;
						List<PropertyWithValue> list = new List<PropertyWithValue>(num);
						CollectionsMarshal.SetCount(list, num);
						CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("PrePaymentECRCheckNum", checkNumber);
						List<PropertyWithValue> editedProperties = list;
						tradeDocument.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, errorMessage, tradeDocument.ISN, editedProperties, tradeDocument.TS);
						_ecrCheckNum = checkNumber;
					}
				});
				if (eCRPrintCheckWindow2.Succeeded)
				{
					UpdateDocumentAfterCheckPrint(tradeDocument, eCRPrintCheckWindow2.CheckNumber, eCRPrintCheckWindow2.ECRCrn, forBonus: true);
				}
			}
			_addedBonusValue = Convert.ToDecimal(BonusInfo.TB_AddedBonus.Text);
			_summaCashValue = Convert.ToDecimal(TotalInfo.TB_Cash.Text);
			_summaPlCardValue = Convert.ToDecimal(TotalInfo.TB_NoCash.Text);
			_summaPayByBonusValue = Convert.ToDecimal(TotalInfo.TB_FromBonus.Text);
			_summaCreditValue = Convert.ToDecimal(TotalInfo.TB_CreditSale.Text);
			_summaPrePaymentValue = Convert.ToDecimal(TotalInfo.TB_FromPrePay.Text);
			_calculatedBonusValue = 0m;
			_summaCashSubtractBonus = 0m;
			_summaPlCardSubtractBonus = 0m;
		}
		finally
		{
			if (flag)
			{
				NewAfterStore();
			}
			else if (_docIsSale && _nonCashPaymentMethod == 0 && (_posTerminalType == "2" || _posTerminalType == "4"))
			{
				try
				{
					CancelPOSTerminalPaymentIfDone(posTerminalResponse, (MTBill)tradeDocument, poscrn);
					MakeDocumentDraftAndReloadIt(tradeDocument.ISN);
				}
				catch (Exception)
				{
					MakeDocumentDraftAndReloadIt(tradeDocument.ISN);
					throw;
				}
			}
			else
			{
				MakeDocumentDraftAndReloadIt(tradeDocument.ISN);
			}
		}
	}

	private void MakeDocumentDraftAndReloadIt(Guid docISN)
	{
		if (_docIsSale)
		{
			MTBill mTBill = ((!Settings.IsOffline) ? Settings.DocumentService.Load<MTBill>(docISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill, withoutAccessCheck: true) : Settings.LocalDBDocumentService.Load<MTBill>(docISN));
			if (mTBill != null)
			{
				if (Settings.IsOffline)
				{
					Settings.LocalDBDocumentService.Store(mTBill, new StoreRequest
					{
						State = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value,
						WithoutAccessCheck = true
					}, storedInDb: false, LocalDBDocumentSyncStateSE.NotSent);
				}
				else
				{
					mTBill = Settings.DocumentService.StoreAndLoad(mTBill, new StoreRequest
					{
						State = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value,
						WithoutAccessCheck = true
					});
				}
				_deferredISN = docISN;
				LoadMTBillDocument(mTBill);
			}
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(mTBill.PartnerID, _contract);
			if (mTBill.PartnerID != -1 && contract != null && contract.IsBonusCard && !string.IsNullOrWhiteSpace(contract.DiscountCard) && _connectTob2barmsoftDiscountServer)
			{
				DiscountServerService.DeleteTransactionsByISN(mTBill.TransactionISN);
				mTBill.TransactionISN = Guid.Empty;
				int num = 1;
				List<PropertyWithValue> list = new List<PropertyWithValue>(num);
				CollectionsMarshal.SetCount(list, num);
				CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("TransactionISN", mTBill.TransactionISN.ToString());
				List<PropertyWithValue> properties = list;
				mTBill.TS = Settings.DocumentService.EditFields(mTBill.ISN, properties, new StoreRequest
				{
					State = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value,
					WithoutAccessCheck = true,
					LogComment = ArmSoft.SME.Trade.Common.Resources.DeleteFromDiscountServer
				});
			}
			return;
		}
		MTBillReturn mTBillReturn = ((!Settings.IsOffline) ? Settings.DocumentService.Load<MTBillReturn>(docISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn, withoutAccessCheck: true) : Settings.LocalDBDocumentService.Load<MTBillReturn>(docISN));
		if (mTBillReturn != null)
		{
			if (Settings.IsOffline)
			{
				Settings.LocalDBDocumentService.Store(mTBillReturn, new StoreRequest
				{
					State = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value,
					WithoutAccessCheck = true
				}, storedInDb: false, LocalDBDocumentSyncStateSE.NotSent);
			}
			else
			{
				mTBillReturn = Settings.DocumentService.StoreAndLoad(mTBillReturn, new StoreRequest
				{
					State = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value,
					WithoutAccessCheck = true
				});
			}
			_deferredISN = docISN;
			LoadMTReturnFromCustomerDocument(mTBillReturn);
		}
		PartnerCacheRow.ContractRow contract2 = TradeSettings.TradeCacheService.Partner.GetContract(mTBillReturn.PartnerID, _contract);
		if (mTBillReturn.PartnerID != -1 && contract2 != null && contract2.IsBonusCard && !string.IsNullOrWhiteSpace(contract2.DiscountCard) && _connectTob2barmsoftDiscountServer)
		{
			DiscountServerService.DeleteTransactionsByISN(mTBillReturn.TransactionISN);
			mTBillReturn.TransactionISN = Guid.Empty;
			int num = 1;
			List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list2, num);
			CollectionsMarshal.AsSpan(list2)[0] = new PropertyWithValue("TransactionISN", mTBillReturn.TransactionISN.ToString());
			List<PropertyWithValue> properties2 = list2;
			mTBillReturn.TS = Settings.DocumentService.EditFields(mTBillReturn.ISN, properties2, new StoreRequest
			{
				State = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value,
				WithoutAccessCheck = true,
				LogComment = ArmSoft.SME.Trade.Common.Resources.DeleteFromDiscountServer
			});
		}
	}

	private bool ECRPreliminaryChecksAndInitialization(CashCacheRow cashesCacheRow, bool prepaymentCheck = false, string baseDocECRCRN = "")
	{
		if (!prepaymentCheck && (CalcMTBillListRowCount(includeDeleted: false) <= 0 || (!_docIsSale && _baseDocISN == Guid.Empty)))
		{
			return false;
		}
		string eCROperator = cashesCacheRow.ECROperator;
		string eCROperatorPassword = cashesCacheRow.ECROperatorPassword;
		if (Settings.IsOffline)
		{
			if (StatusBar.TB_Time.Text.ToDateOnly() != DateTime.Now.ToDateOnly())
			{
				throw new ASException(ArmSoft.SME.Common.Resources.DocDateNotCurrent);
			}
		}
		else if (StatusBar.TB_Time.Text.ToDateOnly() != TradeSettings.CreateTradeClient().CommonServices.GetServerDate().ToDateOnly())
		{
			throw new ASException(ArmSoft.SME.Common.Resources.DocDateNotCurrent);
		}
		if (cashesCacheRow == null)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.ValidEmptyCash);
		}
		if (string.IsNullOrWhiteSpace(eCROperator) || string.IsNullOrWhiteSpace(eCROperatorPassword))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidateECROperatorOrECROperatorPin, cashesCacheRow.Code), (prepaymentCheck || _docIsSale) ? ArmSoft.SME.Common.Resources.PrintECRCheck : ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (!eCROperator.IsNumeric())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, (prepaymentCheck || _docIsSale) ? ArmSoft.SME.Common.Resources.PrintECRCheck : ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (!_docIsSale && !IsValidReturnSums())
		{
			return false;
		}
		if (!prepaymentCheck && !_docIsSale && string.IsNullOrWhiteSpace(baseDocECRCRN) && string.IsNullOrWhiteSpace(cashesCacheRow.ECRCrn))
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.ECRCrn));
		}
		ECRHelpers.InitECR(ref _ecr, _defaultCash);
		if (_ecr == null)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, (prepaymentCheck || _docIsSale) ? ArmSoft.SME.Common.Resources.PrintECRCheck : ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		return true;
	}

	private bool IsValidReturnSums()
	{
		decimal num = Convert.ToDecimal(TotalInfo.TB_NoCash.Text);
		decimal num2 = Convert.ToDecimal(TotalInfo.TB_Cash.Text) - Convert.ToDecimal(TotalInfo.TB_MustPay.Text);
		decimal num3 = Convert.ToDecimal(TotalInfo.TB_FromPrePay.Text);
		decimal num4 = Convert.ToDecimal(TotalInfo.TB_CreditSale.Text);
		MTBill mTBill = ((!Settings.IsOffline) ? Settings.DocumentService.Load<MTBill>(_baseDocISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill, withoutAccessCheck: true) : Settings.LocalDBDocumentService.Load<MTBill>(_baseDocISN));
		decimal num5 = 0m;
		if (!string.IsNullOrWhiteSpace(Settings.ParametersService.ECRCheckPrintBonusType()))
		{
			num5 = CalcUsedBonusSumm();
		}
		decimal num6 = num + num2 + num5 + num4 + num3;
		if (num6 == 0m)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnSumsNotSet, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (num6 < Convert.ToDecimal(TotalInfo.TB_Total.Text))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnSumLessThanTotalSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (num6 > Convert.ToDecimal(TotalInfo.TB_Total.Text))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnSumGreaterThanTotalSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		decimal? summaPlCard = mTBill.SummaPlCard;
		if ((num > summaPlCard.GetValueOrDefault()) & summaPlCard.HasValue)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnByCardSumGreaterThanBaseDocByCardSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		summaPlCard = mTBill.SummaPrePayment;
		if ((num3 > summaPlCard.GetValueOrDefault()) & summaPlCard.HasValue)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnByPrePaymentSumGreaterThanBaseDocByPrePaymentSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		summaPlCard = mTBill.SummaCash;
		if ((num2 > summaPlCard.GetValueOrDefault()) & summaPlCard.HasValue)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnByCashSumGreaterThanBaseDocByCashSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		summaPlCard = mTBill.SummaCredit;
		if ((num4 > summaPlCard.GetValueOrDefault()) & summaPlCard.HasValue)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnByCreditSumGreaterThanBaseDocByCreditSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		return true;
	}

	private (string departmentWithVAT, string departmentWithoutVAT) GetECRDepartments(CashCacheRow cashesCacheRow, bool isDocumentWithVAT)
	{
		bool flag = false;
		bool flag2 = false;
		string item = "";
		string item2 = "";
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			if (!materialsRows.IsDeleted)
			{
				flag = flag || materialsRows.VAT.Value;
				flag2 = flag2 || !materialsRows.VAT.Value;
				if (flag & flag2)
				{
					break;
				}
			}
		}
		if (flag & isDocumentWithVAT)
		{
			if (string.IsNullOrWhiteSpace(cashesCacheRow.ECRDepWithVAT))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ECRDepWithVATNotSet);
			}
			if (!cashesCacheRow.ECRDepWithVAT.IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ECRInitFailed);
			}
			item = cashesCacheRow.ECRDepWithVAT;
		}
		if (flag2 || !isDocumentWithVAT)
		{
			if (string.IsNullOrWhiteSpace(cashesCacheRow.ECRDepWithOutVAT))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ECRDepWithOutVATNotSet);
			}
			if (!cashesCacheRow.ECRDepWithOutVAT.IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ECRInitFailed);
			}
			item2 = cashesCacheRow.ECRDepWithOutVAT;
		}
		return (departmentWithVAT: item, departmentWithoutVAT: item2);
	}

	private List<Utilities.Item> CreateECRItemsForGoodsMode(bool isDocumentWithVAT, string departmentWithVAT, string departmentWithoutVAT)
	{
		List<Utilities.Item> list = new List<Utilities.Item>();
		for (int i = 0; i < _materialsRowsList.Count; i++)
		{
			SaleRow saleRow = _materialsRowsList[i];
			if (saleRow.IsDeleted)
			{
				continue;
			}
			Utilities.Item item = new Utilities.Item
			{
				dep = Convert.ToInt32((saleRow.VAT.Value & isDocumentWithVAT) ? departmentWithVAT : departmentWithoutVAT),
				qty = saleRow.Quantity
			};
			decimal num = saleRow.DiscountPrice;
			decimal discount = saleRow.Discount;
			decimal num2 = saleRow.Summa;
			decimal num3 = saleRow.DiscountSumma;
			if (Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.DiscountAndPrePayment)
			{
				num = (saleRow.DiscountPrice - saleRow.AccumulatedBonus / saleRow.Quantity).RoundAs(2);
				discount = 100m - (num / saleRow.Price * 100m).RoundAs(2);
				num2 = (saleRow.Summa - saleRow.AccumulatedBonus).RoundAs(2);
				num3 = (saleRow.DiscountSumma + saleRow.AccumulatedBonus).RoundAs(2);
			}
			_clonedBill.MTList[i].DiscountPrice = num;
			_clonedBill.MTList[i].Discount = discount;
			_clonedBill.MTList[i].Summa = num2;
			_clonedBill.MTList[i].DiscountSumma = num3;
			if (Settings.ParametersService.PrintECRCheckDiscount())
			{
				item.price = saleRow.Price;
				if (num3 != 0m)
				{
					if (Settings.ParametersService.ECRDiscountType() == ECRRowDiscountTypesSE.Percent.Value)
					{
						item.discount = discount;
						item.discountType = 1;
					}
					else if (Settings.ParametersService.ECRDiscountType() == ECRRowDiscountTypesSE.Price.Value)
					{
						item.discount = saleRow.Price - ((Settings.ParametersService.AllowChangeDiscountPrice() || saleRow.Quantity == 0m) ? num : ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculatePrice(num2, saleRow.Quantity));
						item.discountType = 2;
					}
					else
					{
						item.discount = num3;
						item.discountType = 4;
					}
				}
			}
			else if (Settings.ParametersService.AllowChangeDiscountPrice() || saleRow.Quantity == 0m)
			{
				item.price = num;
			}
			else
			{
				item.price = (num2 / saleRow.Quantity).RoundAs(2);
			}
			MaterialCacheRow materialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(saleRow.ItemCode) ?? throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NotInMaterials, saleRow.ItemCode));
			item.productCode = ECRHelpers.GetItemCode(Settings.ParametersService.ASECRCheckItemCode(), materialCacheRow).Trim();
			item.productName = ECRHelpers.GetItemName(Settings.ParametersService.ASECRCheckItemName(), materialCacheRow, saleRow.ItemName).Trim();
			item.adgCode = materialCacheRow.CPACode;
			if (string.IsNullOrWhiteSpace(item.adgCode))
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.CPAClassification));
			}
			if (materialCacheRow.CPACode.Replace(".", "").Length == materialCacheRow.CPACode.Length)
			{
				if (materialCacheRow.CPACode.Length >= 4)
				{
					item.adgCode = materialCacheRow.CPACode.Substring(0, 4);
				}
			}
			else if (materialCacheRow.CPACode.Length >= 5)
			{
				item.adgCode = materialCacheRow.CPACode.Substring(0, 5);
			}
			item.unit = saleRow.UnitBrief.Trim();
			if (_useMarking && !materialCacheRow.IsWeight && materialCacheRow.CheckMarking && (decimal)saleRow.MarkingList.Count != saleRow.Quantity && MarkingHelpers.IsGoodMarkable(saleRow.ItemCode, _useMarking, saleRow.MarkingList.Count != 0, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), out var _))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MarkingHasMismatch, saleRow.ItemCode));
			}
			list.Add(item);
		}
		return list;
	}

	private Utilities.PrintReceiptRequest CreatePrintRequest(CashCacheRow cashesCacheRow)
	{
		string text = "";
		string text2 = "";
		bool isDocumentWithVAT = _vatType.InList("1", "2", "5");
		decimal summaCashValue = _summaCashValue;
		decimal summaPlCardValue = _summaPlCardValue;
		decimal summaPrePaymentValue = _summaPrePaymentValue;
		decimal d = 0m;
		if (Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.Cash.Value)
		{
			summaCashValue += _summaPayByBonusValue;
		}
		if (Settings.ParametersService.ECRCheckPrintCreditAsCash())
		{
			summaCashValue += _summaCreditValue;
		}
		else if (Settings.ParametersService.ECRCheckPrintCreditAsPartial())
		{
			d = _summaCreditValue;
		}
		(string departmentWithVAT, string departmentWithoutVAT) eCRDepartments = GetECRDepartments(cashesCacheRow, isDocumentWithVAT);
		text = eCRDepartments.departmentWithVAT;
		text2 = eCRDepartments.departmentWithoutVAT;
		List<Utilities.Item> items = CreateECRItemsForGoodsMode(isDocumentWithVAT, text, text2);
		int? paymentSystem = null;
		if (_posTerminalType == POSTerminalTypesSE.ECRPOS)
		{
			paymentSystem = NonCashPaymentHelpers.GetECRPaymentSystem((ArmSoft.SME.Common.Constants.Common.NonCashPaymentType)(_nonCashPaymentMethodSelected ? _nonCashPaymentMethod : _nonCashPaymentMethodForECRPOS));
		}
		return new Utilities.PrintReceiptRequest
		{
			mode = 2,
			paidAmount = decimal.ToDouble(summaCashValue),
			partialAmount = decimal.ToDouble(d),
			prePaymentAmount = decimal.ToDouble(summaPrePaymentValue),
			paidAmountCard = decimal.ToDouble(summaPlCardValue),
			useExtPOS = (_posTerminalType != POSTerminalTypesSE.ECRPOS.Value),
			PaymentSystem = paymentSystem,
			partnerTin = _partnerTaxCode,
			items = items,
			eMarks = CreateECREMarksList()
		};
	}

	private void CalcAllSummaBasedFromCalculatedBonuses()
	{
		_summaPrePaymentValue += _summaPayByBonusValue;
		_summaPayByBonusValue = 0m;
		_calculatedBonusValue += _addedBonusValue;
		_addedBonusValue = 0m;
		_calculatedBonusValue += CalcBonusFromRows();
		if (_summaCashValue >= _calculatedBonusValue)
		{
			_summaCashSubtractBonus = _calculatedBonusValue;
			_summaCashValue -= _calculatedBonusValue;
			return;
		}
		_summaCashSubtractBonus = _summaCashValue;
		_calculatedBonusValue -= _summaCashValue;
		_summaCashValue = 0m;
		if (_summaPlCardValue >= _calculatedBonusValue)
		{
			_summaPlCardSubtractBonus = _calculatedBonusValue;
			_summaPlCardValue -= _calculatedBonusValue;
			return;
		}
		_summaPlCardSubtractBonus = _summaPlCardValue;
		_calculatedBonusValue -= _summaPlCardValue;
		_summaPlCardValue = 0m;
		if (_summaPrePaymentValue >= _calculatedBonusValue)
		{
			_summaPrePaymentSubtractBonus = _calculatedBonusValue;
			_summaPrePaymentValue -= _calculatedBonusValue;
		}
		else
		{
			_summaCashSubtractBonus += _calculatedBonusValue;
			_summaCreditValue -= _calculatedBonusValue;
		}
	}

	private decimal CalcBonusFromRows()
	{
		decimal result = 0m;
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			if (!materialsRows.IsDeleted)
			{
				result += materialsRows.AccumulatedBonus;
			}
		}
		return result;
	}

	private Utilities.PrintReceiptRequest CreatePrePaymentRequest()
	{
		return new Utilities.PrintReceiptRequest
		{
			mode = 3,
			paidAmount = (double)_summaCashSubtractBonus,
			partialAmount = 0.0,
			prePaymentAmount = 0.0,
			paidAmountCard = (double)_summaPlCardSubtractBonus,
			PaymentSystem = null,
			useExtPOS = true
		};
	}

	private Utilities.PrintReturnReceiptRequest CreateReturnRequestForBonus(CashCacheRow cashRow)
	{
		MTBill mTBill = (MTBill)Settings.DocumentService.Load(_baseDocISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill, withoutAccessCheck: true);
		Utilities.PrintReturnReceiptRequest printReturnReceiptRequest = new Utilities.PrintReturnReceiptRequest();
		if (string.IsNullOrWhiteSpace(mTBill.ECRCRN))
		{
			if (!string.IsNullOrWhiteSpace(mTBill.CashDesk) && cashRow.Code.Trim() != mTBill.CashDesk.Trim())
			{
				printReturnReceiptRequest.crn = TradeSettings.TradeCacheService.Cash.GetItem(mTBill.CashDesk)?.ECRCrn ?? cashRow.ECRCrn;
			}
			else
			{
				printReturnReceiptRequest.crn = cashRow.ECRCrn;
			}
		}
		else
		{
			printReturnReceiptRequest.crn = mTBill.ECRCRN;
		}
		int.TryParse(mTBill.PrePaymentECRCheckNum, out var result);
		printReturnReceiptRequest.returnTicketId = result;
		printReturnReceiptRequest.cardAmountForReturn = _summaPlCardSubtractBonus;
		printReturnReceiptRequest.cashAmountForReturn = _summaCashSubtractBonus;
		return printReturnReceiptRequest;
	}

	private List<string> CreateECREMarksList()
	{
		List<string> list = new List<string>();
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			if (materialsRows.IsDeleted)
			{
				continue;
			}
			foreach (string marking in materialsRows.MarkingList)
			{
				list.Add(marking);
			}
		}
		return list;
	}

	private Utilities.PrintReturnReceiptRequest CreateReturnRequest(CashCacheRow cashesCacheRow)
	{
		List<Utilities.ReturnItem> list = new List<Utilities.ReturnItem>();
		MTBill mTBill = ((!Settings.IsOffline) ? Settings.DocumentService.Load<MTBill>(_baseDocISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill, withoutAccessCheck: true) : Settings.LocalDBDocumentService.Load<MTBill>(_baseDocISN));
		Dictionary<int, int> eCRRowNumByRowIdDictionary = ECRHelpers.GetECRRowNumByRowIdDictionary(mTBill.MTList);
		decimal cashAmountForReturn = 0m;
		if (Settings.ParametersService.ECRCheckPrintCreditAsCash())
		{
			cashAmountForReturn += _summaCreditValue;
		}
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			if (materialsRows.IsDeleted)
			{
				continue;
			}
			Utilities.ReturnItem item = new Utilities.ReturnItem
			{
				rpid = eCRRowNumByRowIdDictionary[materialsRows.RowID],
				quantity = decimal.ToDouble(materialsRows.Quantity)
			};
			list.Add(item);
			if (_useMarking)
			{
				MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(materialsRows.ItemCode);
				if (!item2.IsWeight && item2.CheckMarking && (decimal)materialsRows.MarkingList.Count != materialsRows.Quantity && MarkingHelpers.IsGoodMarkable(materialsRows.ItemCode, _useMarking, materialsRows.MarkingList.Count != 0, StatusBar.TB_Time.Text.ToDateOnly(), out var _))
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MarkingHasMismatch, materialsRows.ItemCode));
				}
			}
		}
		cashAmountForReturn += _summaCashValue + _summaPayByBonusValue;
		Utilities.PrintReturnReceiptRequest printReturnReceiptRequest = new Utilities.PrintReturnReceiptRequest
		{
			returnTicketId = Convert.ToInt32(mTBill.ECRCheckNum),
			cardAmountForReturn = _summaPlCardValue,
			cashAmountForReturn = cashAmountForReturn,
			prePaymentAmountForReturn = _summaPrePaymentValue,
			returnItemList = list,
			eMarks = CreateECREMarksList()
		};
		if (string.IsNullOrWhiteSpace(mTBill.ECRCRN))
		{
			if (!string.IsNullOrWhiteSpace(_baseDocCash) && cashesCacheRow.Code.Trim() != _baseDocCash.Trim())
			{
				CashCacheRow item3 = TradeSettings.TradeCacheService.Cash.GetItem(_baseDocCash);
				printReturnReceiptRequest.crn = ((item3 == null) ? cashesCacheRow.ECRCrn : item3.ECRCrn);
			}
			else
			{
				printReturnReceiptRequest.crn = cashesCacheRow.ECRCrn;
			}
		}
		else
		{
			printReturnReceiptRequest.crn = mTBill.ECRCRN;
		}
		return printReturnReceiptRequest;
	}

	private static void UpdateDocumentAfterCheckPrint(TradeDocument tradeDocument, string checkNumber, string ecrCRN, bool forBonus = false, bool fillXInBonus = false)
	{
		List<PropertyWithValue> list = new List<PropertyWithValue>();
		switch (tradeDocument.POType)
		{
		default:
			return;
		case 18:
			if (forBonus)
			{
				list.Add(new PropertyWithValue("PrePaymentECRCheckNum", checkNumber));
			}
			else
			{
				list.Add(new PropertyWithValue("ECRCheckNum", checkNumber));
			}
			if (fillXInBonus)
			{
				list.Add(new PropertyWithValue("PrePaymentECRCheckNum", "XXXXXXXX"));
			}
			list.Add(new PropertyWithValue("ECRCRN", ecrCRN));
			list.Add(new PropertyWithValue("IsCheckLimitAnswerYes", Convert.ToString(value: true)));
			break;
		case 38:
			if (forBonus)
			{
				list.Add(new PropertyWithValue("PrePaymentECRCheckNum", checkNumber));
			}
			else
			{
				list.Add(new PropertyWithValue("ECRCheckNum", checkNumber));
			}
			if (fillXInBonus)
			{
				list.Add(new PropertyWithValue("PrePaymentECRCheckNum", "XXXXXXXX"));
			}
			break;
		case 2:
			list.Add(new PropertyWithValue("ECRCheckNum", checkNumber));
			list.Add(new PropertyWithValue("ECRCRN", ecrCRN));
			break;
		case 3:
			list.Add(new PropertyWithValue("ECRReturnCheckNum", checkNumber));
			break;
		}
		tradeDocument.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, "", tradeDocument.ISN, list, tradeDocument.TS);
	}

	private void OnPrintPrepaymentCheck()
	{
		PrintPrePaymentTouchWindow printPrePaymentTouchWindow = new PrintPrePaymentTouchWindow(this)
		{
			PartnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(_partnerCode),
			PartnerContractCacheRow = TradeSettings.TradeCacheService.Partner.GetContract(_partnerID, _contract)
		};
		if (!printPrePaymentTouchWindow.ShowDialog().Value)
		{
			return;
		}
		CashIn cashIn = (CashIn)Settings.DocumentService.Create(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn);
		cashIn.CashDesk = _defaultCash;
		CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(cashIn.CashDesk);
		if (printPrePaymentTouchWindow.Mode == SaleWindowHelper.PrintMode.Cash)
		{
			cashIn.OpType = "1";
		}
		else
		{
			cashIn.OpType = "4";
			if (item != null)
			{
				cashIn.BankAcquiringPartnerGroup.PartnerCode = printPrePaymentTouchWindow.AcquaringPartner;
				cashIn.BankAcquiringPartnerGroup.ContractCode = printPrePaymentTouchWindow.AcquaringContract;
				cashIn.POSTerminalType = printPrePaymentTouchWindow.POSTerminalType;
				cashIn.NonCashPaymentMethod = printPrePaymentTouchWindow.NonCashPaymentMethod;
				cashIn.POSTerminalID = printPrePaymentTouchWindow.PosTerminalID;
				cashIn.POSTerminalAddress = printPrePaymentTouchWindow.PosTerminalAddress;
			}
		}
		if (!string.IsNullOrWhiteSpace(StatusBar.TB_Time.Text))
		{
			cashIn.DocDate = Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly();
		}
		cashIn.CashierShiftNumber = _currentShiftNum;
		cashIn.CurrCode = "AMD";
		cashIn.Summ = printPrePaymentTouchWindow.Amount;
		cashIn.SummNCC = printPrePaymentTouchWindow.Amount;
		if (printPrePaymentTouchWindow.PartnerContractCacheRow != null)
		{
			cashIn.PartnerContractGroup.PartnerCode = printPrePaymentTouchWindow.PartnerCacheRow.PartCode;
			cashIn.PartnerContractGroup.ContractCode = printPrePaymentTouchWindow.PartnerContractCacheRow.Contract;
			cashIn.PayerName = printPrePaymentTouchWindow.PartnerCacheRow.Caption;
			if (printPrePaymentTouchWindow.PartnerContractCacheRow.CurrCode == "AMD")
			{
				cashIn.PartnerContractGroup.SummaPart = cashIn.SummNCC.Value;
			}
			else
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(printPrePaymentTouchWindow.PartnerContractCacheRow.CurrCode, cashIn.DocDate);
				cashIn.PartnerContractGroup.SummaPart = (cashIn.SummNCC.Value * exchangeRates.Base / exchangeRates.Rate).RoundAs(2);
			}
		}
		if (string.IsNullOrWhiteSpace(cashIn.PartnerContractGroup.PartnerCode))
		{
			if (cashIn.OpType == "1")
			{
				cashIn.PayerName = ArmSoft.SME.Trade.Common.Resources.PrepaymentCash;
			}
			else
			{
				cashIn.PayerName = ArmSoft.SME.Trade.Common.Resources.PayWithPlasticCart;
			}
		}
		cashIn.Owner = Settings.LoginService.UserName;
		cashIn = Settings.DocumentService.StoreAndLoad(cashIn, new StoreRequest
		{
			WithoutAccessCheck = true
		});
		TradeNotificationHelper.ShowTradeDefaultNotification(ArmSoft.SME.Trade.Common.Resources.UsePrePaymentPart1 + " " + ArmSoft.SME.Common.Resources.ECRC, string.Format(ArmSoft.SME.Trade.Common.Resources.CashInCreatedNotification, cashIn.DocNum));
		long poscrn = 0L;
		PosTerminalResponse posTerminalResponse = PayWithPOSTerminalIfChosen(cashIn, ref poscrn);
		PrintPrepaymentCheck(cashIn, item, posTerminalResponse, poscrn);
	}

	private void PrintPrePaymentReturnECR()
	{
		PrintPrePaymentReturnTouchWindow printPrePaymentReturnTouchWindow = new PrintPrePaymentReturnTouchWindow(this)
		{
			PartnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(_partnerCode),
			PartnerContractCacheRow = TradeSettings.TradeCacheService.Partner.GetContract(_partnerID, _contract)
		};
		if (!printPrePaymentReturnTouchWindow.ShowDialog().Value)
		{
			return;
		}
		CashOut cashOutDocument = (CashOut)Settings.DocumentService.Create(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut);
		cashOutDocument.OpType = "1";
		cashOutDocument.BaseISN = printPrePaymentReturnTouchWindow.BaseDocument.ISN;
		if (!string.IsNullOrWhiteSpace(StatusBar.TB_Time.Text))
		{
			cashOutDocument.DocDate = Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly();
		}
		cashOutDocument.CashDesk = printPrePaymentReturnTouchWindow.BaseDocument.CashDesk;
		cashOutDocument.ReceiverName = printPrePaymentReturnTouchWindow.BaseDocument.PayerName;
		cashOutDocument.CashierShiftNumber = _currentShiftNum;
		cashOutDocument.CurrCode = "AMD";
		cashOutDocument.Summ = printPrePaymentReturnTouchWindow.Amount;
		cashOutDocument.SummNCC = printPrePaymentReturnTouchWindow.Amount;
		if (printPrePaymentReturnTouchWindow.PartnerContractCacheRow != null)
		{
			cashOutDocument.PartnerContractGroup.PartnerCode = printPrePaymentReturnTouchWindow.PartnerCacheRow.Caption;
			cashOutDocument.PartnerContractGroup.ContractCode = printPrePaymentReturnTouchWindow.PartnerContractCacheRow.Contract;
			if (string.IsNullOrWhiteSpace(cashOutDocument.ReceiverName))
			{
				cashOutDocument.ReceiverName = printPrePaymentReturnTouchWindow.PartnerCacheRow.Caption;
			}
			if (printPrePaymentReturnTouchWindow.PartnerContractCacheRow.CurrCode == "AMD")
			{
				cashOutDocument.PartnerContractGroup.SummaPart = cashOutDocument.SummNCC.Value;
			}
			else
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(printPrePaymentReturnTouchWindow.PartnerContractCacheRow.CurrCode, cashOutDocument.DocDate);
				cashOutDocument.PartnerContractGroup.SummaPart = (cashOutDocument.SummNCC.Value * exchangeRates.Base / exchangeRates.Rate).RoundAs(2);
			}
		}
		cashOutDocument.Owner = Settings.LoginService.UserName;
		cashOutDocument = Settings.DocumentService.StoreAndLoad(cashOutDocument, new StoreRequest
		{
			WithoutAccessCheck = true
		});
		TradeNotificationHelper.ShowTradeDefaultNotification(ArmSoft.SME.Trade.Common.Resources.UsePrePaymentPart1 + " " + ArmSoft.SME.Common.Resources.ECRC, string.Format(ArmSoft.SME.Trade.Common.Resources.CashOutCreatedNotification, cashOutDocument.DocNum));
		CashCacheRow cashRow = TradeSettings.TradeCacheService.Cash.GetItem(cashOutDocument.CashDesk);
		if (ECRPreliminaryChecksAndInitialization(cashRow, prepaymentCheck: true, printPrePaymentReturnTouchWindow.BaseDocument.ECRCRN))
		{
			string templateCode = PrintTemplateHelper.CombinePrintTemplateName(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut.Value, 3);
			ECRPrintCheckWindow eCRPrintCheckWindow = new ECRPrintCheckWindow(this, _ecr, cashRow.ECROperator, cashRow.ECROperatorPassword, cashRow.ShowECRTracking, CreatePrePaymentReturnRequest(cashRow, printPrePaymentReturnTouchWindow.BaseDocument, cashOutDocument.SummNCC.Value), ECRPrintCheckWindow.ECRCheckType.Return, (ECRPrintCheckWindow window, Utilities.PrintReceiptResponse response) =>
			{
				ECRHelpers.PrintOnOtherPrinter(window, response, cashRow, cashOutDocument, templateCode);
			}, (string checkNumber, string errorMessage) =>
			{
				int num = 1;
				List<PropertyWithValue> list = new List<PropertyWithValue>(num);
				CollectionsMarshal.SetCount(list, num);
				CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("ECRReturnCheckNum", checkNumber);
				List<PropertyWithValue> editedProperties = list;
				cashOutDocument.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, errorMessage, cashOutDocument.ISN, editedProperties, cashOutDocument.TS);
				_ecrCheckNum = checkNumber;
				cashOutDocument.ECRReturnCheckNum = checkNumber;
			});
			if (eCRPrintCheckWindow.Succeeded)
			{
				UpdateDocumentAfterCheckPrint(cashOutDocument, eCRPrintCheckWindow.CheckNumber, eCRPrintCheckWindow.ECRCrn);
			}
		}
	}

	private void PrintPrepaymentCheck(CashIn cashInDocument, CashCacheRow cashesCacheRow, PosTerminalResponse posTerminalResponse, long poscrn)
	{
		if (ECRPreliminaryChecksAndInitialization(cashesCacheRow, prepaymentCheck: true))
		{
			string templateCode = PrintTemplateHelper.CombinePrintTemplateName(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn.Value, 3);
			ECRPrintCheckWindow eCRPrintCheckWindow = new ECRPrintCheckWindow(this, _ecr, cashesCacheRow.ECROperator, cashesCacheRow.ECROperatorPassword, cashesCacheRow.ShowECRTracking, CreatePrePaymentRequest(cashInDocument, posTerminalResponse), ECRPrintCheckWindow.ECRCheckType.PrePayment, (ECRPrintCheckWindow window, Utilities.PrintReceiptResponse response) =>
			{
				ECRHelpers.PrintOnOtherPrinter(window, response, cashesCacheRow, cashInDocument, templateCode);
			}, (string checkNumber, string errorMessage) =>
			{
				int num = 1;
				List<PropertyWithValue> list = new List<PropertyWithValue>(num);
				CollectionsMarshal.SetCount(list, num);
				CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("ECRCheckNum", checkNumber);
				List<PropertyWithValue> editedProperties = list;
				cashInDocument.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, errorMessage, cashInDocument.ISN, editedProperties, cashInDocument.TS);
				cashInDocument.ECRCheckNum = checkNumber;
			});
			if (eCRPrintCheckWindow.Succeeded)
			{
				UpdateDocumentAfterCheckPrint(cashInDocument, eCRPrintCheckWindow.CheckNumber, eCRPrintCheckWindow.ECRCrn);
				ConfirmPOSTerminalPaymentIfDone(posTerminalResponse, cashInDocument);
			}
			else
			{
				CancelPOSTerminalPaymentIfDone(posTerminalResponse, cashInDocument, poscrn);
			}
		}
	}

	private Utilities.PrintReceiptRequest CreatePrePaymentRequest(CashIn cashInDocument, PosTerminalResponse posTerminalResponse)
	{
		decimal d = 0m;
		decimal d2 = 0m;
		if (cashInDocument.OpType == "4")
		{
			d2 = cashInDocument.SummNCC.Value;
		}
		else
		{
			d = cashInDocument.SummNCC.Value;
		}
		int? paymentSystem = null;
		if (cashInDocument.POSTerminalType == POSTerminalTypesSE.ECRPOS.Value)
		{
			paymentSystem = NonCashPaymentHelpers.GetECRPaymentSystem((ArmSoft.SME.Common.Constants.Common.NonCashPaymentType)cashInDocument.NonCashPaymentMethod.Value);
		}
		Utilities.PrintReceiptRequest printReceiptRequest = new Utilities.PrintReceiptRequest
		{
			mode = 3,
			paidAmount = decimal.ToDouble(d),
			partialAmount = 0.0,
			prePaymentAmount = 0.0,
			paidAmountCard = decimal.ToDouble(d2),
			PaymentSystem = paymentSystem,
			useExtPOS = (cashInDocument.POSTerminalType != POSTerminalTypesSE.ECRPOS.Value)
		};
		if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN))
		{
			printReceiptRequest.rrn = posTerminalResponse.RRN;
			if (!string.IsNullOrWhiteSpace(posTerminalResponse.TerminalId))
			{
				printReceiptRequest.terminalId = posTerminalResponse.TerminalId;
			}
		}
		return printReceiptRequest;
	}

	private void PrintCashIOCheck(TradeDocument tradeDocument, CashCacheRow cashesCacheRow, bool isCashIn)
	{
		if (!ECRPreliminaryChecksAndInitialization(cashesCacheRow, prepaymentCheck: true))
		{
			return;
		}
		ECRPrintCheckWindow eCRPrintCheckWindow = new ECRPrintCheckWindow(this, _ecr, cashesCacheRow.ECROperator, cashesCacheRow.ECROperatorPassword, cashesCacheRow.ShowECRTracking, CreateCashIORequest(tradeDocument, isCashIn, cashesCacheRow.ECROperator), ECRPrintCheckWindow.ECRCheckType.CashInOut, null, (string checkNumber, string errorMessage) =>
		{
			List<PropertyWithValue> list = new List<PropertyWithValue>();
			if (isCashIn)
			{
				list.Add(new PropertyWithValue("ECRCheckNum", ((CashIn)tradeDocument).ECRCheckNum));
			}
			else
			{
				list.Add(new PropertyWithValue("ECRReturnCheckNum", ((CashOut)tradeDocument).ECRReturnCheckNum));
			}
			tradeDocument.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, errorMessage, tradeDocument.ISN, list, tradeDocument.TS);
			_ecrCheckNum = checkNumber;
		});
		if (eCRPrintCheckWindow.Succeeded)
		{
			UpdateDocumentAfterCheckPrint(tradeDocument, eCRPrintCheckWindow.CheckNumber, eCRPrintCheckWindow.ECRCrn);
		}
	}

	private Utilities.PrintCashInOutRequest CreateCashIORequest(TradeDocument tradeDocument, bool isCashIn, string ecrOperator)
	{
		return new Utilities.PrintCashInOutRequest
		{
			amount = decimal.ToDouble(tradeDocument.SummNCC.Value),
			isCashIn = isCashIn,
			cashierId = Convert.ToInt32(ecrOperator),
			description = (isCashIn ? ArmSoft.SME.Trade.Common.Resources.CashIn : ArmSoft.SME.Trade.Common.Resources.CashOut)
		};
	}

	private Utilities.PrintReturnReceiptRequest CreatePrePaymentReturnRequest(CashCacheRow cashesCacheRow, CashIn cashInBaseDocument, decimal summNCC)
	{
		Utilities.PrintReturnReceiptRequest printReturnReceiptRequest = new Utilities.PrintReturnReceiptRequest();
		if (string.IsNullOrWhiteSpace(cashInBaseDocument.ECRCRN))
		{
			if (!string.IsNullOrWhiteSpace(cashInBaseDocument.CashDesk) && cashesCacheRow.Code.Trim() != cashInBaseDocument.CashDesk.Trim())
			{
				CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(cashInBaseDocument.CashDesk);
				printReturnReceiptRequest.crn = ((item == null) ? cashesCacheRow.ECRCrn : item.ECRCrn);
			}
			else
			{
				printReturnReceiptRequest.crn = cashesCacheRow.ECRCrn;
			}
		}
		else
		{
			printReturnReceiptRequest.crn = cashInBaseDocument.ECRCRN;
		}
		int.TryParse(cashInBaseDocument.ECRCheckNum, out var result);
		printReturnReceiptRequest.returnTicketId = result;
		printReturnReceiptRequest.cardAmountForReturn = ((cashInBaseDocument.OpType == "4") ? summNCC : 0m);
		printReturnReceiptRequest.cashAmountForReturn = summNCC - printReturnReceiptRequest.cardAmountForReturn;
		return printReturnReceiptRequest;
	}

	private static PosTerminalResponse PayWithPOSTerminalIfChosen(CashIn cashInDocument, ref long poscrn)
	{
		if (cashInDocument.SummNCC.Value == 0m)
		{
			return null;
		}
		if (cashInDocument.POSTerminalType == "2")
		{
			return Arcus2Helpers.CheckAndPurchase(cashInDocument.POSTerminalID, cashInDocument.SummNCC.Value);
		}
		if (cashInDocument.POSTerminalType == "4")
		{
			return PossoftPayClientHelper.Purchase(out poscrn, cashInDocument.POSTerminalAddress, cashInDocument.SummNCC.Value);
		}
		return null;
	}

	private static PosTerminalResponse PayWithPOSTerminalIfChosen(MTBill mtBillDocument, ref long poscrn)
	{
		decimal? summaPlCard = mtBillDocument.SummaPlCard;
		if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
		{
			return null;
		}
		if (mtBillDocument.POSTerminalType == "2")
		{
			return Arcus2Helpers.CheckAndPurchase(mtBillDocument.POSTerminalID, mtBillDocument.SummaPlCard.Value);
		}
		if (mtBillDocument.POSTerminalType == "4")
		{
			return PossoftPayClientHelper.Purchase(out poscrn, mtBillDocument.POSTerminalAddress, mtBillDocument.SummaPlCard.Value);
		}
		return null;
	}

	private static void CancelPOSTerminalPaymentIfDone(PosTerminalResponse posTerminalResponse, CashIn doc, long poscrn)
	{
		if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN))
		{
			if (doc.POSTerminalType == "2")
			{
				Arcus2Helpers.Cancel(doc.POSTerminalID, doc.SummNCC.Value, posTerminalResponse.RRN);
			}
			else if (doc.POSTerminalType == "4")
			{
				PossoftPayClientHelper.Cancel(poscrn, doc.POSTerminalAddress);
			}
		}
	}

	private static void CancelPOSTerminalPaymentIfDone(PosTerminalResponse posTerminalResponse, MTBill mtBillDocuemnt, long poscrn)
	{
		if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN))
		{
			if (mtBillDocuemnt.POSTerminalType == "2")
			{
				Arcus2Helpers.Cancel(mtBillDocuemnt.POSTerminalID, mtBillDocuemnt.SummNCC.Value, posTerminalResponse.RRN);
			}
			else if (mtBillDocuemnt.POSTerminalType == "4")
			{
				PossoftPayClientHelper.Cancel(poscrn, mtBillDocuemnt.POSTerminalAddress);
			}
		}
	}

	private void ConfirmPOSTerminalPaymentIfDone(PosTerminalResponse posTerminalResponse, CashIn cashInDocument)
	{
		if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN) && (!(cashInDocument.POSTerminalType != "2") || !(cashInDocument.POSTerminalType != "4")))
		{
			UpdatePaymentId(posTerminalResponse.RRN, cashInDocument);
		}
	}

	private void ConfirmPOSTerminalPaymentIfDone(PosTerminalResponse posTerminalResponse, MTBill mtBillDocument)
	{
		if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN) && (!(mtBillDocument.POSTerminalType != "2") || !(mtBillDocument.POSTerminalType != "4")))
		{
			UpdatePaymentId(posTerminalResponse.RRN, mtBillDocument);
		}
	}

	private void UpdatePaymentId(string paymentId, CashIn cashInDocument)
	{
		try
		{
			string text = (SmartEnum<POSTerminalTypesSE, string>.TryFromValue(cashInDocument.POSTerminalType, out var result) ? (result.CaptionShort + ": ") : "");
			_nonCashPaymentId = text + paymentId;
			_paymentAccepted = true;
			cashInDocument.PaymentId = _nonCashPaymentId;
			cashInDocument.PaymentAccepted = true;
			int num = 2;
			List<PropertyWithValue> list = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list, num);
			Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
			span[0] = new PropertyWithValue("PaymentId", cashInDocument.PaymentId);
			span[1] = new PropertyWithValue("PaymentAccepted", Convert.ToString(cashInDocument.PaymentAccepted));
			List<PropertyWithValue> properties = list;
			StoreRequest sr = new StoreRequest
			{
				IsDataChanged = true,
				LogComment = _nonCashPaymentId,
				WithoutGrids = true,
				WithoutValidation = true,
				WithoutAccessCheck = true,
				RegenerateEntries = false
			};
			cashInDocument.TS = Settings.DocumentService.EditFields(cashInDocument.ISN, properties, sr, cashInDocument.TS);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void UpdatePaymentId(string paymentId, MTBill mtBillDocument)
	{
		try
		{
			string text = (SmartEnum<POSTerminalTypesSE, string>.TryFromValue(mtBillDocument.POSTerminalType, out var result) ? (result.CaptionShort + ": ") : "");
			_nonCashPaymentId = text + paymentId;
			_paymentAccepted = true;
			mtBillDocument.PaymentId = _nonCashPaymentId;
			mtBillDocument.PaymentAccepted = true;
			int num = 2;
			List<PropertyWithValue> list = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list, num);
			Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
			span[0] = new PropertyWithValue("PaymentId", mtBillDocument.PaymentId);
			span[1] = new PropertyWithValue("PaymentAccepted", Convert.ToString(mtBillDocument.PaymentAccepted));
			List<PropertyWithValue> properties = list;
			StoreRequest sr = new StoreRequest
			{
				IsDataChanged = true,
				LogComment = _nonCashPaymentId,
				WithoutGrids = true,
				WithoutValidation = true,
				WithoutAccessCheck = true,
				RegenerateEntries = false
			};
			mtBillDocument.TS = Settings.DocumentService.EditFields(mtBillDocument.ISN, properties, sr, mtBillDocument.TS);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void UpdateReturnPaymentStatus(MTBillReturn mTBillReturnDocument)
	{
		try
		{
			mTBillReturnDocument.PaymentRefundAccepted = true;
			int num = 1;
			List<PropertyWithValue> list = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list, num);
			CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("PaymentRefundAccepted", Convert.ToString(mTBillReturnDocument.PaymentRefundAccepted));
			List<PropertyWithValue> properties = list;
			StoreRequest sr = new StoreRequest
			{
				IsDataChanged = true,
				LogComment = ArmSoft.SME.Common.Resources.IsRefunded,
				WithoutGrids = true,
				WithoutValidation = true,
				WithoutAccessCheck = true,
				RegenerateEntries = false
			};
			mTBillReturnDocument.TS = Settings.DocumentService.EditFields(mTBillReturnDocument.ISN, properties, sr, mTBillReturnDocument.TS);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private PosTerminalResponse RefundPOSTerminalIfChosen(MTBillReturn mtBillReturnDocument)
	{
		PosTerminalResponse result = null;
		MTBill mTBill = (MTBill)Settings.DocumentService.Load(_baseDocISN, withoutAccessCheck: true);
		if (mTBill != null)
		{
			LoadMTBillDocument(mTBill);
		}
		if (mTBill.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && !mtBillReturnDocument.PaymentRefundAccepted)
		{
			decimal? summaPlCard = mTBill.SummaPlCard;
			if (!((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue) && (mTBill.POSTerminalType == "2" || mTBill.POSTerminalType == "4"))
			{
				string text;
				if (string.IsNullOrWhiteSpace(mTBill.PaymentId))
				{
					text = "";
				}
				else
				{
					text = (mTBill.PaymentId.Contains(':') ? mTBill.PaymentId.Split(':')[1].Trim() : mTBill.PaymentId.Trim());
				}
				if (mTBill.POSTerminalType == "2")
				{
					result = Arcus2Helpers.CheckAndRefund(mTBill.POSTerminalID, mtBillReturnDocument.ECRReturnByCardSum, mTBill.SummaPlCard.Value, text);
				}
				if (mTBill.POSTerminalType == "4")
				{
					result = PossoftPayClientHelper.Refund(text, mtBillReturnDocument.POSTerminalAddress, mtBillReturnDocument.ECRReturnByCardSum);
				}
				UpdateReturnPaymentStatus(mtBillReturnDocument);
			}
		}
		return result;
	}

	private void OnCashPaymentButtonClick()
	{
		try
		{
			decimal num = GetPaidSum() - Convert.ToDecimal(TotalInfo.TB_Total.Text);
			if (string.IsNullOrWhiteSpace(CodeTextEdit.Text) && Convert.ToDecimal(TotalInfo.TB_Cash.Text) == 0m && num < 0m)
			{
				TotalInfo.TB_Cash.Text = Convert.ToDecimal(TotalInfo.TB_MustPay.Text).CurrencyFormat(16, 2);
			}
			else
			{
				if (!CodeTextEdit.Text.Trim().IsNumeric())
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
				}
				if (Convert.ToDecimal(CodeTextEdit.Text.Trim()) < 0m)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.IncrementSumma, 0));
				}
				TotalInfo.TB_Cash.Text = Convert.ToDecimal(CodeTextEdit.Text).CurrencyFormat(16, 2);
			}
			UpdateChangeOrPaySum();
			CodeTextEdit.Focus();
			SetPaymentsVisibility();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			CodeTextEdit.Text = "";
			_realText = "";
		}
	}

	private void OnFromPrePaymentButtonClick()
	{
		try
		{
			FromPrePaymentTouchWindow fromPrePaymentTouchWindow;
			if (string.IsNullOrWhiteSpace(_prepaymentPartnerCode))
			{
				fromPrePaymentTouchWindow = new FromPrePaymentTouchWindow(this)
				{
					PartnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(_partnerCode),
					PartnerContractCacheRow = TradeSettings.TradeCacheService.Partner.GetContract(_partnerID, _contract)
				};
			}
			else
			{
				PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_prepaymentPartnerCode);
				if (string.IsNullOrWhiteSpace(_prepaymentContractCode))
				{
					PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item);
					fromPrePaymentTouchWindow = new FromPrePaymentTouchWindow(this)
					{
						PartnerCacheRow = item,
						PartnerContractCacheRow = TradeSettings.TradeCacheService.Partner.GetContract(item.Id, defaultContract.Contract)
					};
				}
				else
				{
					fromPrePaymentTouchWindow = new FromPrePaymentTouchWindow(this)
					{
						PartnerCacheRow = item,
						PartnerContractCacheRow = TradeSettings.TradeCacheService.Partner.GetContract(item.Id, _prepaymentContractCode)
					};
				}
			}
			decimal num = Convert.ToDecimal(TotalInfo.TB_FromPrePay.Text.Trim());
			if (num == 0m)
			{
				decimal num2 = GetPaidSum() - Convert.ToDecimal(TotalInfo.TB_Total.Text);
				if (num2 < 0m)
				{
					fromPrePaymentTouchWindow.Amount = -num2;
				}
			}
			else
			{
				fromPrePaymentTouchWindow.Amount = num;
			}
			if (fromPrePaymentTouchWindow.ShowDialog().Value)
			{
				if (fromPrePaymentTouchWindow.PartnerContractCacheRow == null)
				{
					_prepaymentPartnerCode = "";
					_prepaymentContractCode = "";
				}
				else
				{
					_prepaymentPartnerCode = fromPrePaymentTouchWindow.PartnerCacheRow.PartCode;
					_prepaymentContractCode = fromPrePaymentTouchWindow.PartnerContractCacheRow.Contract;
				}
				TotalInfo.TB_FromPrePay.Text = fromPrePaymentTouchWindow.Amount.CurrencyFormat(16, 2);
				UpdateChangeOrPaySum();
				CodeTextEdit.Focus();
				SetPaymentsVisibility();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OnCreditSaleButtonClick()
	{
		try
		{
			if (_partnerID <= 0 || string.IsNullOrWhiteSpace(_partnerCode))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.CreditSaleBuyer);
			}
			decimal num = GetPaidSum() - Convert.ToDecimal(TotalInfo.TB_Total.Text);
			if (string.IsNullOrWhiteSpace(CodeTextEdit.Text) && Convert.ToDecimal(TotalInfo.TB_CreditSale.Text) == 0m && num < 0m)
			{
				TotalInfo.TB_CreditSale.Text = Convert.ToDecimal(TotalInfo.TB_MustPay.Text).CurrencyFormat(16, 2);
			}
			else
			{
				if (!CodeTextEdit.Text.Trim().IsNumeric())
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
				}
				if (Convert.ToDecimal(CodeTextEdit.Text.Trim()) < 0m)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.IncrementSumma);
				}
				TotalInfo.TB_CreditSale.Text = Convert.ToDecimal(CodeTextEdit.Text).CurrencyFormat(16, 2);
			}
			UpdateChangeOrPaySum();
			CodeTextEdit.Focus();
			SetPaymentsVisibility();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			CodeTextEdit.Text = "";
			_realText = "";
		}
	}

	private void OnClearPaymentButtonClick()
	{
		try
		{
			TotalInfo.TB_Cash.Text = 0m.CurrencyFormat(16, 2);
			TotalInfo.TB_NoCash.Text = 0m.CurrencyFormat(16, 2);
			TotalInfo.TB_FromPrePay.Text = 0m.CurrencyFormat(16, 2);
			TotalInfo.TB_CreditSale.Text = 0m.CurrencyFormat(16, 2);
			if (_docIsSale || (!_docIsSale && _baseDocISN == Guid.Empty))
			{
				TotalInfo.TB_FromBonus.Text = 0m.CurrencyFormat(16, 2);
			}
			_nonCashPaymentId = "";
			_paymentAccepted = false;
			ClearNonCashPayment();
			if (_docIsSale)
			{
				RemoveUsedBonusFromRows();
			}
			UpdateChangeOrPaySum();
			SetPaymentsVisibility();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ClearNonCashPayment()
	{
		_nonCashPaymentMethodSelected = false;
		_bankAcquiringPartner = "";
		_bankAcquiringContract = "";
		_posTerminalType = "";
		_posTerminalID = "";
		_posTerminalAddress = "";
		_nonCashPaymentMethodForECRPOS = 0;
		_nonCashPaymentMethod = 0;
		ChangeTotalInfoNOCashCaption();
	}

	private void OnPlCardPaymentButtonClick()
	{
		try
		{
			decimal num = GetPaidSum() - Convert.ToDecimal(TotalInfo.TB_Total.Text);
			decimal num2 = 0m;
			num2 = ((!(Convert.ToDecimal(TotalInfo.TB_NoCash.Text) == 0m) || !(num < 0m)) ? Convert.ToDecimal(TotalInfo.TB_NoCash.Text) : Convert.ToDecimal(TotalInfo.TB_MustPay.Text));
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_defaultCash);
			List<CashCacheRow.NonCashInfo> list = new List<CashCacheRow.NonCashInfo>();
			if (!_nonCashPaymentMethodSelected)
			{
				CashCacheRow.NonCashInfo nonCashInfo = item.NonCashInfoList.FirstOrDefault((CashCacheRow.NonCashInfo p) => !string.IsNullOrWhiteSpace(p.Partner));
				list = item.NonCashInfoList.Where((CashCacheRow.NonCashInfo p) => !string.IsNullOrWhiteSpace(p.Partner)).ToList();
				if (nonCashInfo != null)
				{
					_bankAcquiringPartner = nonCashInfo.Partner;
					_bankAcquiringContract = nonCashInfo.Contract;
					_posTerminalType = nonCashInfo.Device;
					_posTerminalID = nonCashInfo.POSTerminalID;
					_posTerminalAddress = nonCashInfo.POSTerminalAddress;
					_nonCashPaymentMethodForECRPOS = nonCashInfo.ECRPaymentMethod;
					_nonCashPaymentMethod = nonCashInfo.ECRPaymentMethod;
					_nonCashPaymentId = "";
					_paymentAccepted = false;
				}
			}
			ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow nonCashPaymentTypeForSaleWindow = ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.None;
			switch (_posTerminalType)
			{
			case "0":
				nonCashPaymentTypeForSaleWindow = ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithNoPOS;
				break;
			case "1":
				nonCashPaymentTypeForSaleWindow = ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithECRPOS;
				break;
			case "2":
				nonCashPaymentTypeForSaleWindow = ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithIngenicoPOS;
				break;
			case "4":
				nonCashPaymentTypeForSaleWindow = ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithFeitianPOS;
				break;
			}
			if (list.Count != 1 || num == 0m)
			{
				NonCashPaymentTouchWindow nonCashPaymentTouchWindow = new NonCashPaymentTouchWindow(this, _docIsSale, num2, _nonCashPaymentMethod, nonCashPaymentTypeForSaleWindow, _bankAcquiringPartner, _bankAcquiringContract);
				if (nonCashPaymentTouchWindow.ShowDialog().Value)
				{
					TotalInfo.TB_NoCash.Text = nonCashPaymentTouchWindow.Amount.CurrencyFormat(16, 2);
					switch (nonCashPaymentTouchWindow.NonCashPaymentType)
					{
					case ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithIngenicoPOS:
						_posTerminalType = "2";
						break;
					case ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithFeitianPOS:
						_posTerminalType = "4";
						break;
					case ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithECRPOS:
						_posTerminalType = "1";
						break;
					default:
						_posTerminalType = "0";
						break;
					}
					_bankAcquiringPartner = nonCashPaymentTouchWindow.AcquaringPartner;
					_bankAcquiringContract = nonCashPaymentTouchWindow.AcquaringContract;
					_nonCashPaymentMethod = nonCashPaymentTouchWindow.NonCashPaymentMethod;
					_nonCashPaymentId = "";
					_paymentAccepted = false;
					_nonCashPaymentMethodSelected = true;
					ChangeTotalInfoNOCashCaption();
				}
			}
			else
			{
				TotalInfo.TB_NoCash.Text = num2.CurrencyFormat(16, 2);
				ChangeTotalInfoNOCashCaption();
			}
			CodeTextEdit.Focus();
			UpdateChangeOrPaySum();
			SetPaymentsVisibility();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			CodeTextEdit.Text = "";
			_realText = "";
		}
	}

	private void ChangeTotalInfoNOCashCaption()
	{
		switch (_nonCashPaymentMethod)
		{
		case 5:
			TotalInfo.TB_NoCashCaption.Text = "PayX";
			break;
		case 6:
			TotalInfo.TB_NoCashCaption.Text = "Idram";
			break;
		case 8:
			TotalInfo.TB_NoCashCaption.Text = "EasyPay";
			break;
		case 7:
			TotalInfo.TB_NoCashCaption.Text = "InecoPay";
			break;
		case 9:
			TotalInfo.TB_NoCashCaption.Text = "Telcell";
			break;
		default:
			TotalInfo.TB_NoCashCaption.Text = ArmSoft.SME.Common.Resources.PlCardSum;
			break;
		}
	}

	private decimal GetPaidSum()
	{
		return Convert.ToDecimal(TotalInfo.TB_Cash.Text) + GetWithoutCashPaidSum();
	}

	private decimal GetWithoutCashPaidSum()
	{
		return Convert.ToDecimal(TotalInfo.TB_FromBonus.Text) + Convert.ToDecimal(TotalInfo.TB_NoCash.Text) + Convert.ToDecimal(TotalInfo.TB_FromPrePay.Text) + Convert.ToDecimal(TotalInfo.TB_CreditSale.Text) - Convert.ToDecimal(BonusInfo.TB_AddedBonus.Text);
	}

	private void UpdateChangeOrPaySum()
	{
		decimal num = GetPaidSum() - Convert.ToDecimal(TotalInfo.TB_Total.Text);
		TotalInfo.TB_MustPay.Text = Math.Abs(num).CurrencyFormat(16, 2);
		SetTotalInfoToPoleDisplay();
		if (num < 0m)
		{
			TotalInfo.TB_MustPayCaption.Text = ArmSoft.SME.Trade.Common.Resources.ForPay;
			TotalInfo.TB_MustPayCaption.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#fb7376" : "#dd0326");
			TotalInfo.TB_MustPay.Foreground = TotalInfo.TB_MustPayCaption.Foreground;
		}
		else if (num > 0m)
		{
			TotalInfo.TB_MustPayCaption.Text = ArmSoft.SME.Trade.Common.Resources.SummaChange;
			TotalInfo.TB_MustPayCaption.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#4fad77" : "#3E7337");
			TotalInfo.TB_MustPay.Foreground = TotalInfo.TB_MustPayCaption.Foreground;
		}
		else
		{
			TotalInfo.TB_MustPayCaption.Text = ArmSoft.SME.Trade.Common.Resources.SummaChange;
			TotalInfo.TB_MustPayCaption.Foreground = TotalInfo.TB_Cash.Foreground;
			TotalInfo.TB_MustPay.Foreground = TotalInfo.TB_Cash.Foreground;
		}
	}

	private void SetPaymentsVisibility()
	{
		double fontSize = 16.0;
		short num = 0;
		if (Convert.ToDecimal(TotalInfo.TB_FromBonus.Text.Trim()) == 0m)
		{
			TotalInfo.Totals.RowDefinitions[3].Height = new GridLength(0.0);
		}
		else
		{
			num++;
			TotalInfo.Totals.RowDefinitions[3].Height = new GridLength(1.0, GridUnitType.Star);
		}
		if (Convert.ToDecimal(TotalInfo.TB_FromPrePay.Text.Trim()) == 0m)
		{
			TotalInfo.Totals.RowDefinitions[4].Height = new GridLength(0.0);
		}
		else
		{
			num++;
			TotalInfo.Totals.RowDefinitions[4].Height = new GridLength(1.0, GridUnitType.Star);
		}
		if (Convert.ToDecimal(TotalInfo.TB_CreditSale.Text.Trim()) == 0m)
		{
			TotalInfo.Totals.RowDefinitions[5].Height = new GridLength(0.0);
		}
		else
		{
			num++;
			TotalInfo.Totals.RowDefinitions[5].Height = new GridLength(1.0, GridUnitType.Star);
		}
		switch (num)
		{
		case 3:
			fontSize = 12.0;
			break;
		case 2:
			fontSize = 14.0;
			break;
		}
		TotalInfo.TB_DiscountCaption.FontSize = fontSize;
		TotalInfo.TB_CashCaption.FontSize = fontSize;
		TotalInfo.TB_NoCashCaption.FontSize = fontSize;
		TotalInfo.TB_MustPayCaption.FontSize = fontSize;
		TotalInfo.TB_FromBonusCaption.FontSize = fontSize;
		TotalInfo.TB_FromPrepayCaption.FontSize = fontSize;
		TotalInfo.TB_CreditSaleCaption.FontSize = fontSize;
	}

	private void OnUseBonuseButtonClick()
	{
		try
		{
			if (!_docIsSale && _baseDocISN != Guid.Empty)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowChangeBonusForReturn);
			}
			if (_partnerID <= 0)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.CreditSaleBuyer);
			}
			if (!_isBonusCard)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ContractIsNotBonusCard, _partnerCode, _contract), "PartRemList");
			}
			if (!string.IsNullOrWhiteSpace(CodeTextEdit.Text))
			{
				string text = CodeTextEdit.Text.Trim();
				if (!text.IsNumeric())
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
				}
				if (Convert.ToDecimal(text) < 0m)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.IncrementSumma, 0));
				}
				if (Convert.ToDecimal(text) > _currentBonusRemain)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.UsedBonusValidate);
				}
			}
			decimal num = 0m;
			if (string.IsNullOrWhiteSpace(CodeTextEdit.Text))
			{
				num = ((!(Settings.ParametersService.BonusUseMaxPercent() < 100m)) ? Convert.ToDecimal(TotalInfo.TB_MustPay.Text) : (Convert.ToDecimal(TotalInfo.TB_Total.Text) * Settings.ParametersService.BonusUseMaxPercent() / 100m).RoundAs(2));
			}
			else
			{
				string value = CodeTextEdit.Text.Trim();
				num = ((!(Settings.ParametersService.BonusUseMaxPercent() < 100m)) ? Convert.ToDecimal(value) : (Convert.ToDecimal(value) * Settings.ParametersService.BonusUseMaxPercent() / 100m).RoundAs(2));
			}
			TotalInfo.TB_FromBonus.Text = Math.Min(num, _currentBonusRemain).CurrencyFormat(16, 2);
			decimal num2 = Convert.ToDecimal(TotalInfo.TB_FromBonus.Text);
			decimal num3 = Convert.ToDecimal(TotalInfo.TB_Total.Text);
			decimal num4 = 0m;
			decimal num5 = 0m;
			foreach (SaleRow materialsRows in _materialsRowsList)
			{
				if (!materialsRows.IsDeleted && !materialsRows.IsGift && num3 != 0m)
				{
					materialsRows.UsedBonus = (materialsRows.Summa / num3 * num2).RoundAs(2);
					if (Settings.ParametersService.NoCalcBonusesWhenRePay())
					{
						materialsRows.AccumulatedBonus = 0m;
					}
					num4 += materialsRows.UsedBonus;
				}
			}
			if (Settings.ParametersService.NoCalcBonusesWhenRePay())
			{
				GridControl.RefreshData();
				BonusInfo.TB_AccumulatedBonus.Text = 0m.CurrencyFormat(16, 2);
			}
			num5 = num2 - num4;
			if (num5 != 0m && CalcMTBillListRowCount(includeDeleted: false) > 0)
			{
				for (int num6 = _materialsRowsList.Count - 1; num6 >= 0; num6--)
				{
					if (!_materialsRowsList[num6].IsDeleted && !_materialsRowsList[num6].IsGift)
					{
						BindingList<SaleRow> materialsRowsList = _materialsRowsList;
						materialsRowsList[materialsRowsList.Count - 1].UsedBonus += num5;
						break;
					}
				}
			}
			UpdateChangeOrPaySum();
			CodeTextEdit.Focus();
			SetPaymentsVisibility();
			SetBuyerInfoToPoleDisplay();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			CodeTextEdit.Text = "";
			_realText = "";
		}
	}

	private void OnAddBonuseButtonClick()
	{
		try
		{
			if (!_docIsSale && _baseDocISN != Guid.Empty)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowChangeBonusForReturn);
			}
			if (_partnerID <= 0)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.CreditSaleBuyer);
			}
			if (!_isBonusCard)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ContractIsNotBonusCard, _partnerCode, _contract), "PartRemList");
			}
			if (!CodeTextEdit.Text.Trim().IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
			}
			if (Convert.ToDecimal(CodeTextEdit.Text.Trim()) < 0m)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.IncrementSumma, 0));
			}
			BonusInfo.TB_AddedBonus.Text = Convert.ToDecimal(CodeTextEdit.Text.Trim()).CurrencyFormat(16, 2);
			UpdateChangeOrPaySum();
			CodeTextEdit.Focus();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			CodeTextEdit.Text = "";
			_realText = "";
		}
	}

	private void OnDiscountButtonClick()
	{
		SaleRow saleRow = (SaleRow)GridControl.GetFocusedRow();
		if (saleRow != null && !saleRow.IsDeleted)
		{
			SaleDiscountsEditWindow saleDiscountsEditWindow = new SaleDiscountsEditWindow(this, saleRow, isReturn: false, TouchMode != POSScreenModesSE.Keyboard.Value, _priceListTypeRoundDgr, Convert.ToDateTime(StatusBar.TB_Time.Text), _currCode);
			if (saleDiscountsEditWindow.ShowDialog().Value)
			{
				saleRow.Discount = saleDiscountsEditWindow.DiscountPercent;
				saleRow.DiscountSumma = saleDiscountsEditWindow.DiscountSumm;
				saleRow.Summa = saleDiscountsEditWindow.DiscountedSumm;
			}
			GridControl.RefreshData();
			UpdateChangeOrPaySum();
			if (_materialsRowsList.Count == 0)
			{
				RowInfoChangedEvent?.Invoke(null);
			}
			CalculateSaleRowSummaAndDiscountSumma(saleRow, calcDiscountPrice: true, fromEditRowWindow: true);
			CalculateDocumentSumma();
			SetStatusBar_Quantity();
		}
	}

	private void OnQuantityButtonClick()
	{
		if (GridControl.CurrentItem == null)
		{
			return;
		}
		try
		{
			SaleRow saleRow = (SaleRow)GridControl.CurrentItem;
			bool flag = false;
			if (string.IsNullOrWhiteSpace(CodeTextEdit.Text))
			{
				if (TradeSettings.TradeCacheService.Material.GetItemById(saleRow.ItemID).IsWeight)
				{
					decimal weight = ScaleConnector.GetWeight();
					if (weight != 0m)
					{
						CodeTextEdit.Text = weight.ToString();
						flag = true;
					}
				}
				if (string.IsNullOrWhiteSpace(CodeTextEdit.Text))
				{
					EditRow(saleRow, SaleWindowHelper.CurrentFocusedField.Quantity);
					return;
				}
			}
			CodeTextEdit.DoValidate();
			if (saleRow.IsGift || (flag && saleRow.UnitCode != WeightItemInfo.GetInstance().Unit))
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(CodeTextEdit.Text))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.InputBoxIsEmpty);
			}
			if (!CodeTextEdit.Text.Trim().IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
			}
			if (!decimal.TryParse(CodeTextEdit.Text.Trim(), out var result))
			{
				return;
			}
			decimal num = Convert.ToDecimal(result).RoundAs(3);
			if (num <= 0m)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidateQuantIsPositive);
			}
			if (decimal.Truncate(num).ToString().Length > 9)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ErrNumberOfDigitsBeforeDecimalPart);
			}
			if (num < saleRow.Quantity)
			{
				if (Settings.ParametersService.AllowPOSDecreaseQuantity())
				{
					ChangeQuantityAndRefreshGrid(saleRow, num);
				}
				else if (new UserAuthenticationDialogWindow(this, ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSDecreaseQuantity.Id).ShowDialog().Value)
				{
					ChangeQuantityAndRefreshGrid(saleRow, num);
				}
			}
			else
			{
				ChangeQuantityAndRefreshGrid(saleRow, num);
			}
			if (flag)
			{
				TradeNotificationHelper.ShowTradeDefaultNotification(ArmSoft.SME.Common.Resources.Quantity, ArmSoft.SME.Trade.Common.Resources.QuantityIsFromElScale);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			CodeTextEdit.Text = "";
			_realText = "";
		}
	}

	private void OnQuantityPlusButtonClick()
	{
		IncreaseDecreaseQuantity();
	}

	private void OnQuantityMinusButtonClick()
	{
		if (Settings.ParametersService.AllowPOSDecreaseQuantity())
		{
			IncreaseDecreaseQuantity(increase: false);
		}
		else if (new UserAuthenticationDialogWindow(this, ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSDecreaseQuantity.Id).ShowDialog().Value)
		{
			IncreaseDecreaseQuantity(increase: false);
		}
	}

	private void OnFocusedRowHandleChanged()
	{
		int focusedRowHandle = InnerView.FocusedRowHandle;
		SaleRow saleRow = (SaleRow)GridControl.GetRow(focusedRowHandle);
		if (saleRow != null)
		{
			RowInfoChangedEvent?.Invoke(saleRow);
		}
	}

	private void RowChanged()
	{
		int focusedRowHandle = InnerView.FocusedRowHandle;
		if (CommonEventHandlers.IsDataRow(focusedRowHandle, GridControl))
		{
			StatusBar.GridRowInfo = $"{GridControl.GetRowVisibleIndexByHandle(focusedRowHandle) + 1}/{GridControl.VisibleRowCount}";
		}
		else
		{
			StatusBar.GridRowInfo = $"0/{GridControl.VisibleRowCount}";
		}
	}

	private void OnBarCodeButtonClick()
	{
		if (!_docIsSale && _baseDocISN != Guid.Empty)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowAddRowForReturn);
		}
		if (!string.IsNullOrWhiteSpace(CodeTextEdit.Text))
		{
			CodeTextEdit.DoValidate();
			ReadBarCode(CodeTextEdit.Text.Trim());
			CodeTextEdit.Text = "";
		}
	}

	private void OnCodeButtonClick()
	{
		if (!_docIsSale && _baseDocISN != Guid.Empty)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowAddRowForReturn);
		}
		if (string.IsNullOrWhiteSpace(CodeTextEdit.Text))
		{
			return;
		}
		CodeTextEdit.DoValidate();
		_qty_Weight = 1m;
		try
		{
			string text = CodeTextEdit.Text.Trim();
			if (text.Length > 20)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrMTCodeLen, 20, text));
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
			if (item == null)
			{
				CodeTextEdit.Focus();
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text));
			}
			SaleRow saleRow = AddItemToList(item, text, barCode: false);
			if (_useMarking && _immediatlyOpenMarkingWindow)
			{
				bool flag = MarkingHelpers.IsGoodMarkable(saleRow.ItemCode, _useMarking, saleRow.MarkingList.Count != 0, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), out var isRowMarkable);
				saleRow.IsRowMarkable = isRowMarkable;
				if (flag)
				{
					EditRow(saleRow, SaleWindowHelper.CurrentFocusedField.Quantity);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			CodeTextEdit.Text = "";
			_realText = "";
		}
	}

	private void CleanInformationPanel()
	{
		MaterialInfo.TB_GoodName.Text = "";
		MaterialInfo.TB_GoodRowDetail.Text = "";
		MaterialInfo.TB_GoodCode.Text = "";
		MaterialInfo.TB_GoodRemInfo.Text = "";
	}

	private void SetRowInformationToPanel(SaleRow saleRow)
	{
		if (string.IsNullOrWhiteSpace(saleRow.ItemName))
		{
			CleanInformationPanel();
			return;
		}
		decimal num = (saleRow.Quantity * saleRow.DiscountPrice).RoundAs(2);
		MaterialInfo.TB_GoodName.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff")) : Brushes.DarkSlateGray);
		MaterialInfo.TB_GoodName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
		MaterialInfo.TB_GoodName.Text = saleRow.ItemName.Trim();
		if (saleRow.Discount == 0m)
		{
			TextEdit tB_GoodRowDetail = MaterialInfo.TB_GoodRowDetail;
			InlineArray4<object> buffer = default;
			buffer[0] = saleRow.Quantity.CurrencyFormat(16, (short)((!(saleRow.Quantity == decimal.Floor(saleRow.Quantity))) ? 3 : 0));
			buffer[1] = saleRow.UnitBrief.Trim();
			buffer[2] = saleRow.DiscountPrice.CurrencyFormat(16, (short)((!(saleRow.DiscountPrice == decimal.Floor(saleRow.DiscountPrice))) ? 2 : 0));
			buffer[3] = num.CurrencyFormat(16, (short)((!(num == decimal.Floor(num))) ? 2 : 0));
			tB_GoodRowDetail.Text = string.Format("{0} {1} x {2} = {3}", (ReadOnlySpan<object?>)buffer);
		}
		else
		{
			decimal value = Math.Abs(saleRow.DiscountSumma);
			string text = ((!(saleRow.DiscountSumma >= 0m)) ? "+" : "-");
			TextEdit tB_GoodRowDetail2 = MaterialInfo.TB_GoodRowDetail;
			InlineArray6<object> buffer2 = default;
			buffer2[0] = saleRow.Quantity.CurrencyFormat(16, (short)((!(saleRow.Quantity == decimal.Floor(saleRow.Quantity))) ? 3 : 0));
			buffer2[1] = saleRow.UnitBrief.Trim();
			buffer2[2] = saleRow.Price.CurrencyFormat(16, (short)((!(saleRow.Price == decimal.Floor(saleRow.Price))) ? 2 : 0));
			buffer2[3] = text;
			buffer2[4] = value.CurrencyFormat(16, (short)((!(saleRow.DiscountSumma == decimal.Floor(saleRow.DiscountSumma))) ? 2 : 0)) + "(" + saleRow.Discount.CurrencyFormat(16, (short)((!(saleRow.Discount == decimal.Floor(saleRow.Discount))) ? 2 : 0)) + "%)";
			buffer2[5] = num.CurrencyFormat(16, (short)((!(num == decimal.Floor(num))) ? 2 : 0));
			tB_GoodRowDetail2.Text = string.Format("{0} {1} x {2} {3} {4} = {5}", (ReadOnlySpan<object?>)buffer2);
		}
		if (TouchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
		{
			MaterialInfo.TB_GoodCode.Text = ArmSoft.SME.Common.Resources.Code + ": " + saleRow.ItemCode;
		}
		if (Settings.ParametersService.ShowStorageAvailabilityOnInfoPanel() || Settings.ParametersService.ShowWholeAvailabilityOnInfoPanel())
		{
			SetAvailabilityInformationToPanel(saleRow);
		}
		else
		{
			MaterialInfo.TB_GoodRemInfo.Height = 0.0;
		}
	}

	private void SetAvailabilityInformationToPanel(SaleRow saleRow)
	{
		MaterialInfo.TB_GoodRemInfo.Background = _remainBackgroundColor;
		MaterialInfo.TB_GoodRemInfo.Text = "";
		if (Settings.IsOffline || !(saleRow.ItemType == GoodsTypesSE.Good.Value))
		{
			return;
		}
		if (Settings.ParametersService.ShowStorageAvailabilityOnInfoPanel())
		{
			MaterialInfo.TB_GoodRemInfo.Text = $"{ArmSoft.SME.Trade.Common.Resources.GoodsRem}: {MaterialHelpers.GetGoodRemains(update: true, saleRow.ItemID, saleRow.UnitCode, ref _availabilityByGoodIdsDictionary, saleRow.Storage)} {saleRow.UnitBrief.Trim()} ";
		}
		if (Settings.ParametersService.ShowWholeAvailabilityOnInfoPanel())
		{
			if (!string.IsNullOrWhiteSpace(MaterialInfo.TB_GoodRemInfo.Text))
			{
				MaterialInfo.TB_GoodRemInfo.Text += ", ";
			}
			MaterialInfo.TB_GoodRemInfo.Text += $"{ArmSoft.SME.Trade.Common.Resources.Whole}: {MaterialHelpers.GetGoodRemains(update: true, saleRow.ItemID, saleRow.UnitCode, ref _availabilityByGoodIdsDictionary)} {saleRow.UnitBrief.Trim()}";
		}
	}

	private void ReadBarCode(string barCode)
	{
		try
		{
			_qty_Weight = 1m;
			if (string.IsNullOrWhiteSpace(barCode))
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(StatusBar.TB_Time.Text))
			{
				CodeTextEdit.Focus();
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			bool flag = false;
			if ((!Settings.ParametersService.UsePartnerEmployeeSearchNewMode()) ? MCRInfo.GetInstance().IsValidCard(barCode) : ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.IsValidPartnerEmployeeCardCode(barCode, Settings.ParametersService.PartnerEmployeeCardCodePrefixes(), Settings.ParametersService.UsePartnerCardLenght(), Settings.ParametersService.PartnerCardLenght()))
			{
				bool flag2 = true;
				string text = barCode;
				if (!Settings.ParametersService.UsePartnerEmployeeSearchNewMode())
				{
					text = MCRInfo.GetInstance().GetCardNumber(barCode);
				}
				if (TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(text, out var _) == null)
				{
					if (!ReadSaleConsultantMagneticCard(text))
					{
						AddNewBuyerWindow(text);
					}
					return;
				}
				short userParameterID = 0;
				if (!string.IsNullOrWhiteSpace(_partnerCode))
				{
					flag2 = Settings.ParametersService.AllowPOSRemoveCard();
					userParameterID = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSRemoveCard.Id;
				}
				else if (!string.IsNullOrWhiteSpace(CodeTextEdit.Text.Trim()))
				{
					flag2 = Settings.ParametersService.AllowPOSInputFromKeyboard();
					userParameterID = ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSInputFromKeyboard.Id;
				}
				if (flag2)
				{
					ReadMagneticCard(text, byMagneticCardPrefixSuffix: false);
				}
				else if (new UserAuthenticationDialogWindow(this, userParameterID).ShowDialog().Value)
				{
					ReadMagneticCard(text, byMagneticCardPrefixSuffix: false);
				}
				return;
			}
			if (barCode.Length < 29)
			{
				MaterialCacheRow materialCacheRow = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.TryGetItemByBarCode(ref barCode, out _qty_Weight, out var _, out var _);
				if (materialCacheRow == null)
				{
					CodeTextEdit.Text = "";
					_realText = "";
					CodeTextEdit.Focus();
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeNotExist, barCode));
				}
				if (_useMarking && !materialCacheRow.IsWeight && materialCacheRow.CheckMarking)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ScanDataMatrix);
				}
				SaleRow saleRow = AddItemToList(materialCacheRow, barCode);
				if (_useMarking && _immediatlyOpenMarkingWindow && MarkingHelpers.IsGoodMarkable(saleRow.ItemCode, _useMarking, saleRow.MarkingList.Count != 0, StatusBar.TB_Time.Text.ToDateOnly(), out var isRowMarkable))
				{
					saleRow.IsRowMarkable = isRowMarkable;
					EditRow(saleRow, SaleWindowHelper.CurrentFocusedField.Quantity);
				}
				return;
			}
			if (!_useMarking)
			{
				CodeTextEdit.Text = "";
				_realText = "";
				CodeTextEdit.Focus();
				throw new ASException(ArmSoft.SME.Common.Resources.MarkingNotActive);
			}
			string eanBarCode = "";
			MaterialCacheRow materialFromScannedMark = GetMaterialFromScannedMark(ref eanBarCode, out var skipMarking);
			if (skipMarking)
			{
				return;
			}
			if (materialFromScannedMark == null)
			{
				CodeTextEdit.Text = "";
				_realText = "";
				CodeTextEdit.Focus();
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodesNotExist, eanBarCode));
			}
			if (!MarkingHelpers.IsGoodMarkable(materialFromScannedMark.MTCode, _useMarking, markingListHasValue: false, StatusBar.TB_Time.Text.ToDateOnly(), out var _))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.CPAEmptyOrNotMarkable);
			}
			if (!string.IsNullOrWhiteSpace(eanBarCode) && _useMarking)
			{
				string text2 = CodeTextEdit.Text;
				SaleRow saleRow2 = AddItemToList(materialFromScannedMark, eanBarCode);
				if (MarkingHelpers.IsGoodMarkable(saleRow2.ItemCode, _useMarking, saleRow2.MarkingList.Count != 0, StatusBar.TB_Time.Text.ToDateOnly(), out var isRowMarkable3))
				{
					saleRow2.IsRowMarkable = isRowMarkable3;
					saleRow2.MarkingList.Add(text2);
					ChangeMarkingStatus(saleRow2);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private MaterialCacheRow GetMaterialFromScannedMark(ref string eanBarCode, out bool skipMarking)
	{
		skipMarking = false;
		if (_useMarking)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(CodeTextEdit.Text);
			for (int i = 0; i <= bytes.Length - 1; i += 2)
			{
				if (((bytes[i + 1] << 8) | bytes[i]) > 127)
				{
					CodeTextEdit.Text = "";
					_realText = "";
					CodeTextEdit.Focus();
					throw new ASException(ArmSoft.SME.Common.Resources.KeyboardLanguageForMarkingScanShouldBeEnglish);
				}
			}
			SaleRow saleRow = _materialsRowsList.FirstOrDefault((SaleRow r) => r.MarkingList.Contains(CodeTextEdit.Text));
			if (saleRow != null)
			{
				ScannerMessageBoxWindow scannerMessageBoxWindow = new ScannerMessageBoxWindow(ArmSoft.SME.Trade.Common.Resources.ConfirmDeleteDataMatrix, this);
				if (scannerMessageBoxWindow.ShowDialog().Value)
				{
					saleRow.MarkingList.Remove(CodeTextEdit.Text);
					if (saleRow.Quantity >= 1m && !saleRow.IsDeleted && !saleRow.IsGift)
					{
						MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(saleRow.ItemCode);
						if (item == null || !item.IsWeight)
						{
							bool flag = false;
							bool flag2 = Settings.ParametersService.AllowPOSDecreaseQuantity();
							if (!flag2)
							{
								flag = new UserAuthenticationDialogWindow(this, ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSDecreaseQuantity.Id).ShowDialog().Value;
								flag2 = flag;
							}
							if (flag2)
							{
								saleRow.Quantity--;
								if (saleRow.Quantity == 0m && ((Settings.ParametersService.AllowPOSDeleteRow() | flag) || new UserAuthenticationDialogWindow(this, ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSDeleteRow.Id).ShowDialog().Value))
								{
									DeleteRowAndRefreshDocumentFields(saleRow);
									saleRow = null;
								}
								else
								{
									ChangeQuantityAndRefreshGrid(saleRow, saleRow.Quantity);
								}
							}
						}
					}
					ChangeMarkingStatus(saleRow);
				}
				CodeTextEdit.Text = "";
				_realText = "";
				CodeTextEdit.Focus();
				if (!string.IsNullOrWhiteSpace(scannerMessageBoxWindow.ScannerData))
				{
					CodeTextEdit.Text = scannerMessageBoxWindow.ScannerData;
					ReadBarCode(scannerMessageBoxWindow.ScannerData);
				}
				skipMarking = true;
				return null;
			}
			MarkingHelper.ValidateMarkingCode(CodeTextEdit.Text);
			eanBarCode = MarkingHelpers.ExtractGTINFromDataMatrix(CodeTextEdit.Text, out var good);
			return good;
		}
		return null;
	}

	private void OnOtherButtonsButtonClick()
	{
		SaleOtherTouchButtonsPadWindow saleOtherTouchButtonsPadWindow = new SaleOtherTouchButtonsPadWindow(this);
		saleOtherTouchButtonsPadWindow.ButtonClick += SaleOtherButtonsPadWindowButton_Click;
		saleOtherTouchButtonsPadWindow.TaxCodeButtonClick += SaleButtonsUserControl_ButtonClick;
		saleOtherTouchButtonsPadWindow.FindBuyerButtonClick += SaleButtonsUserControl_ButtonClick;
		saleOtherTouchButtonsPadWindow.LastCheckECRButtonClick += SaleButtonsUserControl_ButtonClick;
		saleOtherTouchButtonsPadWindow.ShowDialog();
	}

	private void CashIOClick(bool isCashIn)
	{
		try
		{
			CashIOTouchWindow cashIOTouchWindow = new CashIOTouchWindow(this);
			cashIOTouchWindow.Title = (isCashIn ? ArmSoft.SME.Trade.Common.Resources.CashIn : ArmSoft.SME.Trade.Common.Resources.CashOut);
			if (!cashIOTouchWindow.ShowDialog().Value)
			{
				return;
			}
			string text = Settings.ParametersService.DefaultTransitCash();
			TradeDocument tradeDocument;
			if (isCashIn)
			{
				CashIn cashIn = Settings.DocumentService.Create<CashIn>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashIn);
				if (string.IsNullOrWhiteSpace(text))
				{
					cashIn.OpType = "1";
					cashIn.PayerName = ArmSoft.SME.Trade.Common.Resources.CashIn;
					if (!string.IsNullOrWhiteSpace(_partnerCode))
					{
						cashIn.PartnerContractGroup.PartnerCode = _partnerCode;
						cashIn.PartnerContractGroup.ContractCode = _contract;
					}
				}
				else
				{
					cashIn.CashOutDesk = text;
					cashIn.PayerName = ArmSoft.SME.Trade.Common.Resources.CashIn;
					cashIn.OpType = "2";
				}
				tradeDocument = cashIn;
			}
			else
			{
				CashOut cashOut = Settings.DocumentService.Create<CashOut>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut);
				if (string.IsNullOrWhiteSpace(text))
				{
					cashOut.OpType = "1";
					cashOut.ReceiverName = ArmSoft.SME.Trade.Common.Resources.CashOut;
					if (!string.IsNullOrWhiteSpace(_partnerCode))
					{
						cashOut.PartnerContractGroup.PartnerCode = _partnerCode;
						cashOut.PartnerContractGroup.ContractCode = _contract;
					}
				}
				else
				{
					cashOut.CashInDesk = text;
					cashOut.ReceiverName = ArmSoft.SME.Trade.Common.Resources.CashOut;
					cashOut.OpType = "2";
				}
				tradeDocument = cashOut;
			}
			tradeDocument.CashDesk = _defaultCash;
			tradeDocument.DocDate = DateTime.Now.Date.ToDateOnly();
			tradeDocument.CurrCode = _currCode;
			tradeDocument.Summ = cashIOTouchWindow.Amount;
			tradeDocument.CashierShiftNumber = _currentShiftNum;
			if (tradeDocument.CurrCode == "AMD")
			{
				tradeDocument.SummNCC = tradeDocument.Summ;
			}
			else
			{
				tradeDocument.ExchangeRate = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(tradeDocument.CurrCode, tradeDocument.DocDate);
				tradeDocument.SummNCC = (tradeDocument.Summ.Value * tradeDocument.ExchangeRate.Rate / tradeDocument.ExchangeRate.Base).RoundAs(2);
			}
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(tradeDocument.CashDesk);
			tradeDocument.Owner = Settings.LoginService.UserName;
			tradeDocument.OwnerID = Settings.LoginService.UserId;
			tradeDocument = (isCashIn ? ((TradeDocument)Settings.DocumentService.StoreAndLoad((CashIn)tradeDocument, new StoreRequest
			{
				WithoutAccessCheck = true
			})) : ((TradeDocument)Settings.DocumentService.StoreAndLoad((CashOut)tradeDocument, new StoreRequest
			{
				WithoutAccessCheck = true
			})));
			TradeNotificationHelper.ShowTradeDefaultNotification(isCashIn ? ArmSoft.SME.Trade.Common.Resources.CashIn : ArmSoft.SME.Trade.Common.Resources.CashOut, isCashIn ? string.Format(ArmSoft.SME.Trade.Common.Resources.CashInCreatedNotification, tradeDocument.DocNum) : string.Format(ArmSoft.SME.Trade.Common.Resources.CashOutCreatedNotification, tradeDocument.DocNum));
			if (!cashIOTouchWindow.Print && ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.PrintDoc, isCashIn ? ArmSoft.SME.Trade.Common.Resources.CashIn : ArmSoft.SME.Trade.Common.Resources.CashOut), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				string currentUserPrintTemplate = Settings.ParametersService.GetCurrentUserPrintTemplate(tradeDocument.POType);
				if (string.IsNullOrWhiteSpace(currentUserPrintTemplate))
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.PrintTemplateIsMissing, Settings.LoginService.UserName, isCashIn ? ArmSoft.SME.Trade.Common.Resources.CashIn : ArmSoft.SME.Trade.Common.Resources.CashOut), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
				}
				else
				{
					Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(tradeDocument, PrintMode.PrintDirect, currentUserPrintTemplate, CheckPrinterInfo.GetInstance().CheckPrinterName);
				}
			}
			if (cashIOTouchWindow.Print)
			{
				PrintCashIOCheck(tradeDocument, item, isCashIn);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OnDeleteRowButtonClick()
	{
		int focusedRowHandle = InnerView.FocusedRowHandle;
		SaleRow saleRow = (SaleRow)GridControl.GetRow(focusedRowHandle);
		if (saleRow != null && !saleRow.IsGift)
		{
			if (Settings.ParametersService.AllowPOSDeleteRow())
			{
				DeleteRowAndRefreshDocumentFields(saleRow);
			}
			else if (new UserAuthenticationDialogWindow(this, ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSDeleteRow.Id).ShowDialog().Value)
			{
				DeleteRowAndRefreshDocumentFields(saleRow);
			}
		}
	}

	private void SetNextDocumentNumber()
	{
		KeyValuePair<string, DocNumOfferSource> nextDocumentNumberAndRefreshOfflineIfNeeded = SaleWindowHelper.GetNextDocumentNumberAndRefreshOfflineIfNeeded(_docType);
		SaleHeader.TB_SaleNum.Text = nextDocumentNumberAndRefreshOfflineIfNeeded.Key;
		_docNumOfferSource = nextDocumentNumberAndRefreshOfflineIfNeeded.Value;
		Title = SaleHeader.TB_SaleNumCaption.Text + SaleHeader.TB_SaleNum.Text;
	}

	private void CodeTextEdit_FocusAndMoveCursorToEnd()
	{
		CodeTextEdit.Focus();
		CodeTextEdit.SelectionStart = CodeTextEdit.Text.Length;
	}

	private void ChangeMarkingStatus(SaleRow row)
	{
		if (row != null)
		{
			MarkingHelpers.IsGoodMarkable(row.ItemCode, _useMarking, row.MarkingList.Count != 0, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), out var isRowMarkable);
			row.IsRowMarkable = isRowMarkable;
			row.MarkingStatus = (isRowMarkable ? MarkingHelper.GetMarkingStatus(row.MarkingList.Count, row.Quantity) : "");
		}
	}

	private void AddNewBuyerWindow(string cardNumber)
	{
		try
		{
			CodeTextEdit.Clear();
			if (!Settings.ParametersService.AllowToRegisterNewBuyer())
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidCardNotExist, cardNumber));
			}
			if (!Settings.ParametersService.AllowToAddNewBuyerWithNotRegisteredBuyerCard())
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidCardNotExist, cardNumber));
			}
			if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.AcceptToAddNewBuyerWithCardCode, cardNumber), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
			{
				return;
			}
			NewBuyerWindow newBuyerWindow = new NewBuyerWindow(this, cardNumber);
			if (!newBuyerWindow.ShowDialog().Value || string.IsNullOrWhiteSpace(newBuyerWindow.PartnerCode))
			{
				return;
			}
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(newBuyerWindow.PartnerCode);
			if (item.Contracts.Count > 0)
			{
				if (!string.IsNullOrWhiteSpace(_partnerCode))
				{
					ClearPartnerCard();
				}
				PartnerCacheRow.ContractRow contractRow = item.Contracts.First((PartnerCacheRow.ContractRow row) => row.IsDefault);
				FillBuyerFields(contractRow.DiscountCard, item, contractRow);
			}
		}
		catch (ASException ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private decimal CalcUsedBonusSumm()
	{
		decimal num = 0m;
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			num += num + materialsRows.UsedBonus;
		}
		return num;
	}

	private void RemoveUsedBonusFromRows()
	{
		foreach (SaleRow materialsRows in _materialsRowsList)
		{
			materialsRows.UsedBonus = 0m;
		}
	}

	private void UnitChanged(SaleRow saleRow, string oldUnitCode)
	{
		string unitCode = saleRow.UnitCode;
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(unitCode);
		if (item != null)
		{
			MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(saleRow.ItemID);
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(saleRow.ItemCode, item.Code);
			MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(saleRow.ItemID, item.Code);
			if (!_isRetail || itemById.Type == ItemTypesSE.Services.Value)
			{
				if (!string.IsNullOrWhiteSpace(_priceListType))
				{
					decimal num = ((!Settings.IsOffline) ? ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, saleRow.ItemID, item.Code, StatusBar.TB_Time.Text.ToDateOnly(), _currCode) : ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForGivenPriceListAndVATTypeFromCache(_priceListType, _vatType, saleRow.ItemCode, item.Code));
					if (num == 0m)
					{
						num = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(oldUnitCode, unitCode, saleRow.Price, saleRow.ItemCode);
					}
					saleRow.Price = Methods.RoundPrice(num, _priceListTypeRoundDgr);
				}
				else
				{
					MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(saleRow.ItemCode, oldUnitCode);
					if (qntUnit2 != null)
					{
						saleRow.Price = (saleRow.Price / qntUnit2.Coef * qntUnit.Coef).RoundAs(2);
						saleRow.Price = Methods.RoundPrice(saleRow.Price, _priceListTypeRoundDgr);
					}
				}
			}
			else
			{
				saleRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(oldUnitCode, unitCode, saleRow.Price, saleRow.ItemCode);
			}
			saleRow.BarCode = ((barCode == null) ? "" : barCode.BarCode);
		}
		saleRow.UnitCode = unitCode;
		decimal num2 = saleRow.Quantity * saleRow.Price;
		saleRow.Summa = (num2 - (num2 * saleRow.Discount / 100m).RoundAs(2)).RoundAs(2);
		saleRow.DiscountSumma = (num2 - saleRow.Summa).RoundAs(2);
		saleRow.BeginSumma = num2.RoundAs(2);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
