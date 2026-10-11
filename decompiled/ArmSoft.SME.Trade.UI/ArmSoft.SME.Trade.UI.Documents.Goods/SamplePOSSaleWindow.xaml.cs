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

public partial class SamplePOSSaleWindow : BasePOSSaleWindow, IScanable, IMagneticCardReadable, IComponentConnector, IStyleConnector
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

	private SamplePOSSaleTouchModeButtonsPadUserControl _saleTouchModeButtonsPadUserControl;

	private SamplePOSSalePaymentMehtodsPadUserControl _modernPOSSalePaymentMehtodsPadUserControl;

	private FavoritesTouchPadControl _favoritesTouchPadControl;

	private SamplePOSSaleableGoodInfoUserControl _modernPOSSaleableGoodInfoUserControl = new SamplePOSSaleableGoodInfoUserControl();

	private SamplePOSSaleDiscountInfoUserControl _modernPOSSaleDiscountInfoUserControl = new SamplePOSSaleDiscountInfoUserControl();

	private POSSearchPanelControl _posSearchPanelControl;

	private SamplePOSSaleTotalUserControl _modernPOSSaleTotalUserControl;

	private event RowInfoChangedEventHandler RowInfoChangedEvent;

	private event TotalSumChangedForOutsideDelegate TotalSumChangedForOutsideEvent;

	public SamplePOSSaleWindow(Window owner, string touchMode, bool fullScreen, double width, double height)
	{
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Expected Obj, but got Unknown
		//IL_093f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Expected Obj, but got Unknown
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
		RegisterBeforeConstructingUIEventHandlers();
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
		double screenWidth = ((!FullScreen) ? Width : ((double)Screen.PrimaryScreen.Bounds.Width));
		if (TouchMode == POSScreenModesSE.SampleScreen.Value)
		{
			ConstructSaleTouchModeUI(screenWidth);
		}
		else if (TouchMode == POSScreenModesSE.SampleScreenWithFavorites.Value)
		{
			ConstructSaleTouchModeWithFavoritesUI(screenWidth);
		}
		foreach (FormatConditionBase item6 in ConditionalFormattingHelper.ConvertConditions(CreateFormatingList()))
		{
			MaterialsGridTableView.AddFormatCondition(item6);
		}
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
		SamplePOSSaleHeader.UserNameTextBlock.Text = Settings.LoginService.UserDescription;
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
		MaterialsGridControl.ItemsSource = _materialsRowsList;
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

	private void RegisterBeforeConstructingUIEventHandlers()
	{
		TotalSumChangedForOutsideEvent += OnClearPaymentButtonClick;
	}

	private void ConstructSaleTouchModeUI(double screenWidth)
	{
		MainGrid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		MainGrid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(_columnMainSpacing)
		});
		MainGrid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(400.0)
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(_rowMainSpacing)
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(_rowMainSpacing)
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(_rowMainSpacing)
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(_rowMainSpacing)
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		ConstructCommonUIControls();
		_saleTouchModeButtonsPadUserControl = new SamplePOSSaleTouchModeButtonsPadUserControl(_modernPOSSalePaymentMehtodsPadUserControl);
		SetCommonPlacesOfControls();
		Grid.SetRow(_modernPOSSaleDiscountInfoUserControl, 2);
		Grid.SetColumn(_modernPOSSaleDiscountInfoUserControl, 2);
		MainGrid.Children.Add(_modernPOSSaleDiscountInfoUserControl);
		Grid.SetRow(_posSearchPanelControl, 4);
		Grid.SetColumn(_posSearchPanelControl, 2);
		MainGrid.Children.Add(_posSearchPanelControl);
		Grid.SetRowSpan(MaterialsGridControl, 3);
		_modernPOSSaleTotalUserControl.MainGrid.ColumnDefinitions[1].Width = new GridLength(4.0);
		Grid.SetRow(_modernPOSSaleTotalUserControl, 8);
		Grid.SetColumn(_modernPOSSaleTotalUserControl, 0);
		MainGrid.Children.Add(_modernPOSSaleTotalUserControl);
		Grid.SetRow(_saleTouchModeButtonsPadUserControl, 6);
		Grid.SetRowSpan(_saleTouchModeButtonsPadUserControl, 3);
		Grid.SetColumn(_saleTouchModeButtonsPadUserControl, 2);
		MainGrid.Children.Add(_saleTouchModeButtonsPadUserControl);
		SetScreenDependedControlsProperties(screenWidth);
	}

	private void ConstructSaleTouchModeWithFavoritesUI(double screenWidth)
	{
		MainGrid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		MainGrid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(_columnMainSpacing)
		});
		MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
		if (screenWidth < 1024.0)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.TouchScreenWithFavoritesMinSizeMustBe1024);
		}
		if (screenWidth >= 1024.0 && screenWidth < 1280.0)
		{
			MainGrid.ColumnDefinitions[2].Width = new GridLength(350.0);
		}
		else if (Screen.PrimaryScreen.Bounds.Width >= 1280 && screenWidth < 1600.0)
		{
			MainGrid.ColumnDefinitions[2].Width = new GridLength(600.0);
		}
		else
		{
			MainGrid.ColumnDefinitions[2].Width = new GridLength(800.0);
		}
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(_rowMainSpacing)
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(_rowMainSpacing)
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(_rowMainSpacing)
		});
		MainGrid.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		ConstructCommonUIControls();
		_favoritesTouchPadControl = new FavoritesTouchPadControl(screenWidth, AddFromFavorites, _priceListType, _vatType, DateTime.Now.Date);
		_saleTouchModeButtonsPadUserControl = new SamplePOSSaleTouchModeButtonsPadUserControl(null);
		SetCommonPlacesOfControls();
		Grid grid = new Grid();
		Grid.SetRow(grid, 6);
		Grid.SetColumn(grid, 0);
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(1.0, GridUnitType.Star)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(_columnMainSpacing)
		});
		grid.ColumnDefinitions.Add(new ColumnDefinition
		{
			Width = new GridLength(356.0)
		});
		MainGrid.Children.Add(grid);
		Grid grid2 = new Grid();
		Grid.SetColumn(grid2, 0);
		grid2.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid2.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(_rowMainSpacing)
		});
		grid2.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid2.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(_rowMainSpacing)
		});
		grid2.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.Children.Add(grid2);
		Grid.SetRow(_modernPOSSaleDiscountInfoUserControl, 0);
		grid2.Children.Add(_modernPOSSaleDiscountInfoUserControl);
		Grid.SetRow(_modernPOSSalePaymentMehtodsPadUserControl, 2);
		grid2.Children.Add(_modernPOSSalePaymentMehtodsPadUserControl);
		_modernPOSSaleTotalUserControl.MainGrid.ColumnDefinitions[1].Width = new GridLength(4.0);
		Grid.SetRow(_modernPOSSaleTotalUserControl, 4);
		grid2.Children.Add(_modernPOSSaleTotalUserControl);
		Grid grid3 = new Grid();
		Grid.SetColumn(grid3, 2);
		grid3.RowDefinitions.Add(new RowDefinition
		{
			Height = GridLength.Auto
		});
		grid3.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(_rowMainSpacing)
		});
		grid3.RowDefinitions.Add(new RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.Children.Add(grid3);
		Grid.SetRow(_posSearchPanelControl, 0);
		grid3.Children.Add(_posSearchPanelControl);
		Grid.SetRow(_saleTouchModeButtonsPadUserControl, 2);
		grid3.Children.Add(_saleTouchModeButtonsPadUserControl);
		Grid.SetRow(_favoritesTouchPadControl, 2);
		Grid.SetRowSpan(_favoritesTouchPadControl, 5);
		Grid.SetColumn(_favoritesTouchPadControl, 2);
		MainGrid.Children.Add(_favoritesTouchPadControl);
		SetScreenDependedControlsPropertiesForFavorites(screenWidth);
		_saleTouchModeButtonsPadUserControl.ButtonMTFavorites.Visibility = Visibility.Hidden;
		ItemCode.Visible = false;
		Grid.SetColumnSpan(_saleTouchModeButtonsPadUserControl.ButtonOther, 3);
		_modernPOSSaleDiscountInfoUserControl.VerticalAlignment = VerticalAlignment.Top;
		_modernPOSSalePaymentMehtodsPadUserControl.VerticalAlignment = VerticalAlignment.Center;
		_modernPOSSaleTotalUserControl.VerticalAlignment = VerticalAlignment.Bottom;
	}

	private void ConstructCommonUIControls()
	{
		_posSearchPanelControl = new POSSearchPanelControl(Owner);
		_modernPOSSaleTotalUserControl = new SamplePOSSaleTotalUserControl(TouchMode, TotalSumChangedForOutsideEvent);
		_modernPOSSalePaymentMehtodsPadUserControl = new SamplePOSSalePaymentMehtodsPadUserControl(TouchMode);
		_posSearchPanelControl.VerticalAlignment = VerticalAlignment.Top;
		_modernPOSSaleableGoodInfoUserControl.ButtonClick += MaterialInfo_ButtonClick;
		_posSearchPanelControl.CodeSearchTextEdit.KeyDown += CodeTextEdit_KeyDown;
		_posSearchPanelControl.CodeSearchTextEdit.PreviewTextInput += CodeTextEdit_PreviewTextInput;
		_modernPOSSaleableGoodInfoUserControl.BE_Discount.Visibility = Visibility.Collapsed;
		_modernPOSSaleableGoodInfoUserControl.BE_QtyMinus.Visibility = Visibility.Collapsed;
		_modernPOSSaleableGoodInfoUserControl.BE_QtyPlus.Visibility = Visibility.Collapsed;
		_modernPOSSaleableGoodInfoUserControl.ButtonQTY.Visibility = Visibility.Collapsed;
	}

	private void SetCommonPlacesOfControls()
	{
		Grid.SetRow(SamplePOSSaleHeader, 0);
		Grid.SetColumn(SamplePOSSaleHeader, 0);
		Grid.SetColumnSpan(SamplePOSSaleHeader, 3);
		Grid.SetRow(_modernPOSSaleableGoodInfoUserControl, 2);
		Grid.SetColumn(_modernPOSSaleableGoodInfoUserControl, 0);
		MainGrid.Children.Add(_modernPOSSaleableGoodInfoUserControl);
		Grid.SetRow(MaterialsGridControl, 4);
		Grid.SetColumn(MaterialsGridControl, 0);
	}

	private void SetScreenDependedControlsProperties(double screenWidth)
	{
		if (screenWidth < 1024.0)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.POSWindowMinimumSizeMustBe1024);
		}
		if (screenWidth >= 1024.0 && screenWidth < 1152.0)
		{
			_modernPOSSaleTotalUserControl.TB_MustPay.FontSize = 20.0;
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.FontSize = 20.0;
			_modernPOSSaleTotalUserControl.TB_Discount.FontSize = 20.0;
			_modernPOSSaleTotalUserControl.TB_DiscountCaption.FontSize = 20.0;
			_modernPOSSaleTotalUserControl.TotalIntPartOfTextBlock.FontSize = 42.0;
			_modernPOSSaleTotalUserControl.TotalDecimalPartOfTextBlock.FontSize = 24.0;
			_columnMainSpacing = 5;
			_rowMainSpacing = 7;
		}
		else if (screenWidth >= 1152.0 && screenWidth <= 1280.0)
		{
			_modernPOSSaleTotalUserControl.TB_MustPay.FontSize = 24.0;
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.FontSize = 24.0;
			_modernPOSSaleTotalUserControl.TB_Discount.FontSize = 24.0;
			_modernPOSSaleTotalUserControl.TB_DiscountCaption.FontSize = 24.0;
			_modernPOSSaleTotalUserControl.TotalIntPartOfTextBlock.FontSize = 54.0;
			_modernPOSSaleTotalUserControl.TotalDecimalPartOfTextBlock.FontSize = 36.0;
			_columnMainSpacing = 5;
			_rowMainSpacing = 7;
		}
		else if (screenWidth > 1280.0 && screenWidth <= 1366.0)
		{
			_modernPOSSaleTotalUserControl.TB_MustPay.FontSize = 26.0;
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.FontSize = 26.0;
			_modernPOSSaleTotalUserControl.TB_Discount.FontSize = 26.0;
			_modernPOSSaleTotalUserControl.TB_DiscountCaption.FontSize = 26.0;
			_modernPOSSaleTotalUserControl.TotalIntPartOfTextBlock.FontSize = 64.0;
			_modernPOSSaleTotalUserControl.TotalDecimalPartOfTextBlock.FontSize = 42.0;
			_columnMainSpacing = 6;
			_rowMainSpacing = 8;
		}
		else if (screenWidth > 1366.0)
		{
			_modernPOSSaleTotalUserControl.TB_MustPay.FontSize = 30.0;
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.FontSize = 30.0;
			_modernPOSSaleTotalUserControl.TB_Discount.FontSize = 30.0;
			_modernPOSSaleTotalUserControl.TB_DiscountCaption.FontSize = 30.0;
			_modernPOSSaleTotalUserControl.TotalIntPartOfTextBlock.FontSize = 76.0;
			_modernPOSSaleTotalUserControl.TotalDecimalPartOfTextBlock.FontSize = 54.0;
			_columnMainSpacing = 10;
			_rowMainSpacing = 10;
		}
	}

	private void SetScreenDependedControlsPropertiesForFavorites(double screenWidth)
	{
		if (screenWidth < 1024.0)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.TouchScreenWithFavoritesMinSizeMustBe1024);
		}
		if (screenWidth >= 1024.0 && screenWidth < 1152.0)
		{
			_modernPOSSaleTotalUserControl.TB_MustPay.FontSize = 20.0;
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.FontSize = 20.0;
			_modernPOSSaleTotalUserControl.TB_Discount.FontSize = 20.0;
			_modernPOSSaleTotalUserControl.TB_DiscountCaption.FontSize = 20.0;
			_modernPOSSaleTotalUserControl.TotalIntPartOfTextBlock.FontSize = 42.0;
			_modernPOSSaleTotalUserControl.TotalDecimalPartOfTextBlock.FontSize = 24.0;
			_columnMainSpacing = 4;
			_rowMainSpacing = 7;
		}
		else if (screenWidth >= 1152.0 && screenWidth < 1280.0)
		{
			_modernPOSSaleTotalUserControl.TB_MustPay.FontSize = 24.0;
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.FontSize = 24.0;
			_modernPOSSaleTotalUserControl.TB_Discount.FontSize = 24.0;
			_modernPOSSaleTotalUserControl.TB_DiscountCaption.FontSize = 24.0;
			_modernPOSSaleTotalUserControl.TotalIntPartOfTextBlock.FontSize = 54.0;
			_modernPOSSaleTotalUserControl.TotalDecimalPartOfTextBlock.FontSize = 36.0;
			_columnMainSpacing = 5;
			_rowMainSpacing = 7;
		}
		else if (screenWidth >= 1280.0 && screenWidth < 1360.0)
		{
			_modernPOSSaleTotalUserControl.TB_MustPay.FontSize = 20.0;
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.FontSize = 20.0;
			_modernPOSSaleTotalUserControl.TB_Discount.FontSize = 20.0;
			_modernPOSSaleTotalUserControl.TB_DiscountCaption.FontSize = 20.0;
			_modernPOSSaleTotalUserControl.TotalIntPartOfTextBlock.FontSize = 45.0;
			_modernPOSSaleTotalUserControl.TotalDecimalPartOfTextBlock.FontSize = 28.0;
			_columnMainSpacing = 4;
			_rowMainSpacing = 7;
		}
		else if (screenWidth >= 1360.0 && screenWidth < 1440.0)
		{
			_modernPOSSaleTotalUserControl.TB_MustPay.FontSize = 22.0;
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.FontSize = 22.0;
			_modernPOSSaleTotalUserControl.TB_Discount.FontSize = 22.0;
			_modernPOSSaleTotalUserControl.TB_DiscountCaption.FontSize = 22.0;
			_modernPOSSaleTotalUserControl.TotalIntPartOfTextBlock.FontSize = 50.0;
			_modernPOSSaleTotalUserControl.TotalDecimalPartOfTextBlock.FontSize = 32.0;
			_columnMainSpacing = 5;
			_rowMainSpacing = 7;
		}
		else if (screenWidth > 1440.0 && screenWidth < 1680.0)
		{
			_modernPOSSaleTotalUserControl.TB_MustPay.FontSize = 24.0;
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.FontSize = 24.0;
			_modernPOSSaleTotalUserControl.TB_Discount.FontSize = 24.0;
			_modernPOSSaleTotalUserControl.TB_DiscountCaption.FontSize = 24.0;
			_modernPOSSaleTotalUserControl.TotalIntPartOfTextBlock.FontSize = 54.0;
			_modernPOSSaleTotalUserControl.TotalDecimalPartOfTextBlock.FontSize = 36.0;
			_columnMainSpacing = 10;
			_rowMainSpacing = 10;
		}
		else if (screenWidth >= 1680.0)
		{
			_modernPOSSaleTotalUserControl.TB_MustPay.FontSize = 30.0;
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.FontSize = 30.0;
			_modernPOSSaleTotalUserControl.TB_Discount.FontSize = 30.0;
			_modernPOSSaleTotalUserControl.TB_DiscountCaption.FontSize = 30.0;
			_modernPOSSaleTotalUserControl.TotalIntPartOfTextBlock.FontSize = 76.0;
			_modernPOSSaleTotalUserControl.TotalDecimalPartOfTextBlock.FontSize = 54.0;
			_columnMainSpacing = 10;
			_rowMainSpacing = 10;
		}
	}

	private void AddNotXamlEventHandlers()
	{
		RowInfoChangedEvent += OnRowInfoChanged;
		MaterialsGridTableView.FocusedRowHandleChanged += InnerView_FocusedRowHandleChanged;
		MaterialsGridTableView.GotFocus += InnerView_GotFocus;
		MaterialsGridTableView.MouseLeftButtonUp += InnerView_MouseLeftButtonUp;
		_saleTouchModeButtonsPadUserControl.ButtonClick += SaleTouchButtonsPadUserControl_ButtonClick;
		_modernPOSSalePaymentMehtodsPadUserControl.ButtonClick += SaleTouchButtonsPadUserControl_ButtonClick;
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
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillPlCardPaymentHotKey()] = SaleWindowHelper.ButtonCommands.NotCash;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillSalePriceHotKey()] = SaleWindowHelper.ButtonCommands.Price;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillBarCodeEnterHotKey()] = SaleWindowHelper.ButtonCommands.BarCode;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillCodeEnterHotKey()] = SaleWindowHelper.ButtonCommands.Code;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillMTListLikeNameHotKey()] = SaleWindowHelper.ButtonCommands.SelectMT;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillQtyEnterHotKey()] = SaleWindowHelper.ButtonCommands.QTY;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillPrintBillHotKey()] = SaleWindowHelper.ButtonCommands.PrintWithCount;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillPrintECRCheckHotKey()] = SaleWindowHelper.ButtonCommands.PrintCheck;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillFavoritesHotKey()] = SaleWindowHelper.ButtonCommands.MTFavorites;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.MTBillErasePaymentsHotKey()] = SaleWindowHelper.ButtonCommands.ClearPayment;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.POSContinueHotKey()] = SaleWindowHelper.ButtonCommands.Continue;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.POSDeferSaleHotKey()] = SaleWindowHelper.ButtonCommands.Defer;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.POSReturnHotKey()] = SaleWindowHelper.ButtonCommands.Return;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.POSPartnerCardHotKey()] = SaleWindowHelper.ButtonCommands.Card;
		_buttonCommandByKeyGesturesDictionary[Settings.ParametersService.UnitCodeSwitchHotKey()] = SaleWindowHelper.ButtonCommands.QtyUnit;
	}

	private void UpdateKeyGestures()
	{
		_modernPOSSalePaymentMehtodsPadUserControl.CashQRButton.ToolTip = SaleWindowHelper.TooltipOrNull(() => Settings.ParametersService.MTBillSummaCashHotKey());
		_modernPOSSalePaymentMehtodsPadUserControl.CashButton.ToolTip = SaleWindowHelper.TooltipOrNull(() => Settings.ParametersService.MTBillSummaCashHotKey());
		_modernPOSSalePaymentMehtodsPadUserControl.NonCashQRButton.ToolTip = SaleWindowHelper.TooltipOrNull(() => Settings.ParametersService.MTBillPlCardPaymentHotKey());
		_modernPOSSalePaymentMehtodsPadUserControl.NonCashButton.ToolTip = SaleWindowHelper.TooltipOrNull(() => Settings.ParametersService.MTBillPlCardPaymentHotKey());
		_saleTouchModeButtonsPadUserControl.ButtonContinue.ToolTip = SaleWindowHelper.TooltipOrNull(() => Settings.ParametersService.POSContinueHotKey());
		_saleTouchModeButtonsPadUserControl.ButtonDefer.ToolTip = SaleWindowHelper.TooltipOrNull(() => Settings.ParametersService.POSDeferSaleHotKey());
		_saleTouchModeButtonsPadUserControl.ButtonSelectMT.ToolTip = SaleWindowHelper.TooltipOrNull(() => Settings.ParametersService.MTBillMTListLikeNameHotKey());
		_saleTouchModeButtonsPadUserControl.ButtonCard.ToolTip = SaleWindowHelper.TooltipOrNull(() => Settings.ParametersService.POSPartnerCardHotKey());
		_saleTouchModeButtonsPadUserControl.ButtonReturn.ToolTip = SaleWindowHelper.TooltipOrNull(() => Settings.ParametersService.POSReturnHotKey());
		_saleTouchModeButtonsPadUserControl.PrintButton.ToolTip = SaleWindowHelper.TooltipOrNull(() => Settings.ParametersService.MTBillPrintBillHotKey());
		_saleTouchModeButtonsPadUserControl.PrintECRCheckButton.ToolTip = SaleWindowHelper.TooltipOrNull(() => Settings.ParametersService.MTBillPrintECRCheckHotKey());
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
			TradeNotificationHelper.ShowTradeDefaultNotification(ArmSoft.SME.Trade.Common.Resources.Defer, string.Format(_docIsSale ? ArmSoft.SME.Trade.Common.Resources.MTBillDeferd : ArmSoft.SME.Trade.Common.Resources.MTRetFromCustomerDeferd, SamplePOSSaleHeader.TB_SaleNum.Text));
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
			LocalDBDocuments dropDownView = new LocalDBDocuments
			{
				StartDate = ((!_shiftBeginDate.HasValue) ? dateOnly : Convert.ToDateTime(_shiftBeginDate).ToDateOnly()),
				EndDate = dateOnly,
				CurrCode = "AMD",
				CashDeskCode = _defaultCash,
				CashierShiftNumber = _currentShiftNum,
				DocumentState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value,
				OrderByAscedning = false
			};
			TouchGridControlWithExtraButtons<LocalDBDocumentsDataProvider.DataRow> touchGridControlWithExtraButtons = new TouchGridControlWithExtraButtons<LocalDBDocumentsDataProvider.DataRow>(this, dropDownView, "DeferedDocsTouchDialog");
			if (touchGridControlWithExtraButtons.ShowDialog().Value && touchGridControlWithExtraButtons.SelectedRow != null)
			{
				_deferredISN = touchGridControlWithExtraButtons.SelectedRow.ISN;
				num = touchGridControlWithExtraButtons.SelectedRow.DocType;
			}
		}
		else
		{
			DocumentsJournal dropDownView2 = new DocumentsJournal(this, ReportTypesSE.DeferredDocument, null)
			{
				StartDate = ((!_shiftBeginDate.HasValue) ? dateOnly : Convert.ToDateTime(_shiftBeginDate).ToDateOnly()),
				EndDate = dateOnly,
				CurrCode = "AMD",
				CashCode = _defaultCash,
				ShiftNumber = _currentShiftNum,
				DocState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.ValueUI
			};
			TouchGridControlWithExtraButtons<DocumentsJournalDataProvider.DataRow> touchGridControlWithExtraButtons2 = new TouchGridControlWithExtraButtons<DocumentsJournalDataProvider.DataRow>(this, dropDownView2, "DeferedDocsTouchDialog");
			if (touchGridControlWithExtraButtons2.ShowDialog().Value && touchGridControlWithExtraButtons2.SelectedRow != null)
			{
				_deferredISN = touchGridControlWithExtraButtons2.SelectedRow.ISN;
				num = touchGridControlWithExtraButtons2.SelectedRow.DocType;
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
				SamplePOSSaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.SaleNumber + " N ";
				SamplePOSSaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#4fad77")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#217346")));
				SamplePOSSaleHeader.TB_SaleNum.Foreground = SamplePOSSaleHeader.TB_SaleNumCaption.Foreground;
				Title = SamplePOSSaleHeader.TB_SaleNumCaption.Text + SamplePOSSaleHeader.TB_SaleNum.Text;
				_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord;
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
				SamplePOSSaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord + " N ";
				SamplePOSSaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326")));
				SamplePOSSaleHeader.TB_SaleNum.Foreground = SamplePOSSaleHeader.TB_SaleNumCaption.Foreground;
				Title = SamplePOSSaleHeader.TB_SaleNumCaption.Text + SamplePOSSaleHeader.TB_SaleNum.Text;
				_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.Sale;
				_docIsSale = false;
			}
			MTReturnFromCustomerBase mTReturnFromCustomerBase = ((!Settings.IsOffline) ? ((MTReturnFromCustomerBase)Settings.DocumentService.Load(_deferredISN, withoutAccessCheck: true)) : ((MTReturnFromCustomerBase)Settings.LocalDBDocumentService.Load(_deferredISN)));
			if (mTReturnFromCustomerBase != null)
			{
				_modernPOSSaleTotalUserControl.SamplePOSTotalsControl.Background = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#49504b")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#ffc7ce")));
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
		if (!string.IsNullOrWhiteSpace(_modernPOSSaleDiscountInfoUserControl.TB_CardNumber.Text))
		{
			ModifyPartnerCardButtonText(_modernPOSSaleDiscountInfoUserControl.TB_CardNumber.Text);
		}
		if (_materialsRowsList.Count > 0)
		{
			GridControl materialsGridControl = MaterialsGridControl;
			BindingList<SaleRow> materialsRowsList = _materialsRowsList;
			materialsGridControl.CurrentItem = materialsRowsList[materialsRowsList.Count - 1];
		}
		UpdateChangeOrPaySum();
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
				SamplePOSSaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.SaleNumber + " N ";
				SamplePOSSaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#4fad77")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#217346")));
				SamplePOSSaleHeader.TB_SaleNum.Foreground = SamplePOSSaleHeader.TB_SaleNumCaption.Foreground;
				Title = SamplePOSSaleHeader.TB_SaleNumCaption.Text + SamplePOSSaleHeader.TB_SaleNum.Text;
				_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord;
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
				SamplePOSSaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord + " N ";
				SamplePOSSaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326")));
				SamplePOSSaleHeader.TB_SaleNum.Foreground = SamplePOSSaleHeader.TB_SaleNumCaption.Foreground;
				Title = SamplePOSSaleHeader.TB_SaleNumCaption.Text + SamplePOSSaleHeader.TB_SaleNum.Text;
				_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.Sale;
				_docIsSale = false;
			}
			MTReturnFromCustomerBase mTReturnFromCustomerBase = (MTReturnFromCustomerBase)_lastAutoSavedDocument;
			if (mTReturnFromCustomerBase != null)
			{
				_modernPOSSaleTotalUserControl.SamplePOSTotalsControl.Background = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#49504b")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#ffc7ce")));
				_ecrCheckNum = mTReturnFromCustomerBase.ECRCheckNum;
				LoadMTReturnFromCustomerDocument(mTReturnFromCustomerBase);
				_isDeferred = true;
			}
		}
		if (!string.IsNullOrWhiteSpace(_modernPOSSaleDiscountInfoUserControl.TB_CardNumber.Text))
		{
			ModifyPartnerCardButtonText(_modernPOSSaleDiscountInfoUserControl.TB_CardNumber.Text);
		}
		if (_materialsRowsList.Count > 0)
		{
			GridControl materialsGridControl = MaterialsGridControl;
			BindingList<SaleRow> materialsRowsList = _materialsRowsList;
			materialsGridControl.CurrentItem = materialsRowsList[materialsRowsList.Count - 1];
		}
		UpdateChangeOrPaySum();
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
		if (!Settings.ParametersService.AllowUse100PercentSale() && Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text) == 0m)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.UserCanNotSellWith100PercentDiscount);
		}
		if (GetPaidSum() < Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text))
		{
			throw new ASException(ArmSoft.SME.Common.Resources.PaymentsSmallerThanTotal);
		}
		if (GetWithoutCashPaidSum() > Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.SummMustBeGreater);
		}
		if (Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text) != 0m && string.IsNullOrWhiteSpace(_bankAcquiringPartner))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrBankAquaringPart);
		}
		if (!_docIsSale && _baseDocISN == Guid.Empty && Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text) > 0m)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ReturnByCardSumMustBeZero);
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
					CardTransaction cardTransaction = DiscountServerService.RecordDiscountServerTransaction(new CardTransactionRequest
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
					});
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
		SamplePOSSaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.SaleNumber + " ";
		SamplePOSSaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#4fad77")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#217346")));
		SamplePOSSaleHeader.TB_SaleNum.Foreground = SamplePOSSaleHeader.TB_SaleNumCaption.Foreground;
		SamplePOSSaleHeader.TB_BaseDoc.Text = "";
		if (!string.IsNullOrWhiteSpace(_defaultCash))
		{
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_defaultCash);
			if (item != null)
			{
				_ecrCRN = item.ECRCrn;
			}
			ClearNonCashPayment();
		}
		_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord;
		_saleTouchModeButtonsPadUserControl.ButtonCardCaption.Text = ArmSoft.SME.Trade.Common.Resources.Buyers + " " + ArmSoft.SME.Trade.Common.Resources.Card;
		_saleTouchModeButtonsPadUserControl.ButtonCardCaption.Foreground = _saleTouchModeButtonsPadUserControl.ButtonMTFavoritesCaption.Foreground;
		_modernPOSSaleableGoodInfoUserControl.BE_Discount.Visibility = Visibility.Collapsed;
		_modernPOSSaleableGoodInfoUserControl.BE_QtyMinus.Visibility = Visibility.Collapsed;
		_modernPOSSaleableGoodInfoUserControl.BE_QtyPlus.Visibility = Visibility.Collapsed;
		_modernPOSSaleableGoodInfoUserControl.ButtonQTY.Visibility = Visibility.Collapsed;
		_posSearchPanelControl.CodeSearchTextEdit.Text = "";
		_realText = "";
		SetNextDocumentNumber();
		string text = "";
		bool flag = _nonCashPaymentMethod.InList(5, 6, 7, 8, 9);
		if (!isDeferClick)
		{
			new CurrencyDataProvider.Param().Currency = _currCode;
			string unitName = Settings.CoreCacheService.Currency.GetItem(_currCode).UnitName;
			if (Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text) != 0m.RoundAs(2))
			{
				if ((Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text) == 0m.RoundAs(2)) | flag)
				{
					if (Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_MustPay.Text) != 0m.RoundAs(2))
					{
						_modernPOSSaleableGoodInfoUserControl.TB_GoodName.Text = $"{ArmSoft.SME.Trade.Common.Resources.ReturnBuyerChange} {_modernPOSSaleTotalUserControl.TB_MustPay.Text} {unitName}";
						_modernPOSSaleableGoodInfoUserControl.TB_GoodName.ToolTip = $"{ArmSoft.SME.Trade.Common.Resources.ReturnBuyerChange} {_modernPOSSaleTotalUserControl.TB_MustPay.Text} {unitName}";
						InlineArray4<object> buffer = default;
						buffer[0] = Environment.NewLine;
						buffer[1] = ArmSoft.SME.Trade.Common.Resources.DontForgetYourChange;
						buffer[2] = _modernPOSSaleTotalUserControl.TB_MustPay.Text;
						buffer[3] = unitName;
						text = string.Format("{0} {1} {2} {3}", (ReadOnlySpan<object?>)buffer);
					}
				}
				else if (Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text) != 0m.RoundAs(2) && !flag)
				{
					if (Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_MustPay.Text) == 0m.RoundAs(2))
					{
						_modernPOSSaleableGoodInfoUserControl.TB_GoodName.Text = $"{ArmSoft.SME.Trade.Common.Resources.ReturnBuyerCard}";
						_modernPOSSaleableGoodInfoUserControl.TB_GoodName.ToolTip = $"{ArmSoft.SME.Trade.Common.Resources.ReturnBuyerCard}";
						_modernPOSSaleableGoodInfoUserControl.TB_GoodRowDetail.Text = " ";
						text = $"{Environment.NewLine} {ArmSoft.SME.Trade.Common.Resources.DontForgetYourCard}";
					}
					else
					{
						_modernPOSSaleableGoodInfoUserControl.TB_GoodName.Text = $"{ArmSoft.SME.Trade.Common.Resources.ReturnBuyerChangeAndCard} {_modernPOSSaleTotalUserControl.TB_MustPay.Text} {unitName} ";
						_modernPOSSaleableGoodInfoUserControl.TB_GoodName.ToolTip = $"{ArmSoft.SME.Trade.Common.Resources.ReturnBuyerChangeAndCard} {_modernPOSSaleTotalUserControl.TB_MustPay.Text} {unitName} ";
						InlineArray4<object> buffer2 = default;
						buffer2[0] = Environment.NewLine;
						buffer2[1] = ArmSoft.SME.Trade.Common.Resources.DontForgetYourChangeAndCard;
						buffer2[2] = _modernPOSSaleTotalUserControl.TB_MustPay.Text;
						buffer2[3] = unitName;
						text = string.Format("{0} {1} {2} {3} ", (ReadOnlySpan<object?>)buffer2);
					}
				}
			}
			else if (Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text) != 0m.RoundAs(2) && !flag)
			{
				_modernPOSSaleableGoodInfoUserControl.TB_GoodName.Text = $"{ArmSoft.SME.Trade.Common.Resources.ReturnBuyerCard}";
				_modernPOSSaleableGoodInfoUserControl.TB_GoodName.ToolTip = $"{ArmSoft.SME.Trade.Common.Resources.ReturnBuyerCard}";
				_modernPOSSaleableGoodInfoUserControl.TB_GoodRowDetail.Text = " ";
				text = $"{Environment.NewLine} {ArmSoft.SME.Trade.Common.Resources.DontForgetYourCard}";
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				_modernPOSSaleableGoodInfoUserControl.TB_GoodName.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376")) : Brushes.Red);
			}
			else
			{
				_modernPOSSaleableGoodInfoUserControl.TB_GoodName.Text = "";
				_modernPOSSaleableGoodInfoUserControl.TB_GoodName.ToolTip = "";
				_modernPOSSaleableGoodInfoUserControl.TB_GoodRowDetail.Text = "";
			}
		}
		else
		{
			_modernPOSSaleableGoodInfoUserControl.TB_GoodName.Text = "";
			_modernPOSSaleableGoodInfoUserControl.TB_GoodName.ToolTip = "";
			_modernPOSSaleableGoodInfoUserControl.TB_GoodRowDetail.Text = "";
		}
		_modernPOSSaleableGoodInfoUserControl.TB_GoodName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
		_modernPOSSaleableGoodInfoUserControl.TB_GoodRowDetail.Text = "";
		_modernPOSSaleableGoodInfoUserControl.TB_GoodCode.Text = "";
		_modernPOSSaleableGoodInfoUserControl.TB_GoodRemInfo.Text = "";
		_modernPOSSaleableGoodInfoUserControl.GoodRemInfoGrid.Background = _modernPOSSaleableGoodInfoUserControl.TB_GoodName.Background;
		_modernPOSSaleDiscountInfoUserControl.TB_CardNumber.Text = "";
		_modernPOSSaleDiscountInfoUserControl.TB_CardHolderName.Text = "";
		_modernPOSSaleDiscountInfoUserControl.DiscountGrid.Visibility = Visibility.Hidden;
		_modernPOSSaleDiscountInfoUserControl.TB_DiscountPercent.Text = 0m.CurrencyFormat(6, 2);
		_modernPOSSaleTotalUserControl.TB_Discount.Text = 0m.CurrencyFormat(16, 2);
		_modernPOSSaleTotalUserControl.TB_Total.Text = 0m.CurrencyFormat(16, 2);
		_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text = 0m.CurrencyFormat(16, 2);
		_modernPOSSaleTotalUserControl.TB_MustPay.Text = 0m.CurrencyFormat(16, 2);
		_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text = 0m.CurrencyFormat(16, 2);
		_modernPOSSaleTotalUserControl.TB_MustPayCaption.Text = ArmSoft.SME.Trade.Common.Resources.ForPay;
		_modernPOSSaleTotalUserControl.TB_MustPayCaption.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#fb7376" : "#dd0326");
		_modernPOSSaleTotalUserControl.TB_MustPay.Foreground = _modernPOSSaleTotalUserControl.TB_MustPayCaption.Foreground;
		_modernPOSSaleTotalUserControl.SamplePOSTotalsControl.Background = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#49504b")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#def0e0")));
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
		_deferredISN = Guid.Empty;
		_baseDocISN = Guid.Empty;
		_nonCashPaymentId = "";
		_paymentAccepted = false;
		SetStatusBar_SaleConsultant();
		_maxRowID = 0;
		_materialsRowsList.Clear();
		_saleTouchModeButtonsPadUserControl.ButtonDefer.Visibility = Visibility.Hidden;
		_saleTouchModeButtonsPadUserControl.ButtonContinue.Visibility = Visibility.Visible;
		_discountsCalculator.Log?.Clear();
		SetBuyerInfoToPoleDisplay();
		InitPoleDisplay(text);
		_posSearchPanelControl.CodeSearchTextEdit.Focus();
		StartAutoSaveTimer();
	}

	private bool BeforeStore(StoreRequest sr, Guid isn, out bool checkLimitAnswerYes)
	{
		checkLimitAnswerYes = false;
		if (_docIsSale && sr.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && !string.IsNullOrWhiteSpace(_partnerCode) && !string.IsNullOrWhiteSpace(_contract))
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partnerID, _contract);
			if (contract != null && contract.IsBonusCard && Settings.ParametersService.BonusUseMaxPercent() < 100m && 0m > Settings.ParametersService.BonusUseMaxPercent())
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.UsedMaxBonusValidate, (Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text) * Settings.ParametersService.BonusUseMaxPercent() / 100m).RoundAs(2)));
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
		mtBillDocument.DocNum = SamplePOSSaleHeader.TB_SaleNum.Text;
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
		mtBillDocument.Discount = Convert.ToDecimal(_modernPOSSaleDiscountInfoUserControl.TB_DiscountPercent.Text);
		mtBillDocument.BankAcquiring.PartnerCode = _bankAcquiringPartner;
		mtBillDocument.BankAcquiring.ContractCode = _bankAcquiringContract;
		mtBillDocument.NonCashPaymentMethod = _nonCashPaymentMethod;
		mtBillDocument.PaymentId = _nonCashPaymentId;
		mtBillDocument.PaymentAccepted = _paymentAccepted;
		mtBillDocument.POSTerminalType = _posTerminalType;
		mtBillDocument.ECRCRN = _ecrCRN;
		mtBillDocument.Summ = Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text);
		mtBillDocument.SummaCash = Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text);
		mtBillDocument.SummaPlCard = Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text);
		mtBillDocument.SummaChange = Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_MustPay.Text);
		mtBillDocument.SumFromRound = _sumFromRound;
		decimal? summaPlCard = mtBillDocument.SummaPlCard;
		if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
		{
			mtBillDocument.BankAcquiring.PartnerCode = "";
			mtBillDocument.BankAcquiring.ContractCode = "";
			mtBillDocument.POSTerminalType = "";
			mtBillDocument.NonCashPaymentMethod = 0;
		}
		mtBillDocument.SummaCredit = 0m;
		mtBillDocument.SummaPayByBonus = 0m;
		mtBillDocument.SummaPrePayment = 0m;
		mtBillDocument.SumAddedBonus = 0m;
		mtBillDocument.SummVAT = 0m;
		mtBillDocument.Comment = "";
		mtBillDocument.Owner = Settings.LoginService.UserName;
		mtBillDocument.MaxRowID = _maxRowID;
		mtBillDocument.SalesConsultant = _saleConsultantCode;
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
		mTBillReturnDocument.DocNum = SamplePOSSaleHeader.TB_SaleNum.Text;
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
		mTBillReturnDocument.Summ = Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text);
		mTBillReturnDocument.SumFromRound = _sumFromRound;
		mTBillReturnDocument.PrePaymentECRCheckNum = _prePaymentECRCheckNum;
		mTBillReturnDocument.ECRReturnByCashSum = Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text) - Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_MustPay.Text);
		mTBillReturnDocument.ECRReturnByCardSum = Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text);
		mTBillReturnDocument.ECRReturnByPrePaymentSum = 0m;
		mTBillReturnDocument.BankAcquiring.PartnerCode = _bankAcquiringPartner;
		mTBillReturnDocument.BankAcquiring.ContractCode = _bankAcquiringContract;
		mTBillReturnDocument.PaymentRefundAccepted = PaymentRefundAccepted;
		mTBillReturnDocument.POSTerminalType = _posTerminalType;
		mTBillReturnDocument.NonCashPaymentMethod = _nonCashPaymentMethod;
		mTBillReturnDocument.SummVAT = 0m;
		mTBillReturnDocument.Comment = "";
		mTBillReturnDocument.Owner = Settings.LoginService.UserName;
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
		mTReturnFromCustomerDocument.DocNum = SamplePOSSaleHeader.TB_SaleNum.Text;
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
		mTReturnFromCustomerDocument.Summ = Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text);
		mTReturnFromCustomerDocument.SumFromRound = _sumFromRound;
		mTReturnFromCustomerDocument.ECRReturnByCashSum = Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text) - Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_MustPay.Text);
		mTReturnFromCustomerDocument.ECRReturnByCardSum = Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text);
		mTReturnFromCustomerDocument.ECRReturnByPrePaymentSum = 0m;
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
		_modernPOSSaleTotalUserControl.SamplePOSTotalsControl.Background = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#49504b")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#def0e0")));
		SamplePOSSaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.SaleNumber + " ";
		SamplePOSSaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#4fad77")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#217346")));
		SamplePOSSaleHeader.TB_SaleNum.Foreground = SamplePOSSaleHeader.TB_SaleNumCaption.Foreground;
		SamplePOSSaleHeader.TB_BaseDoc.Text = "";
		SetNextDocumentNumber();
		_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord;
		_docIsSale = true;
	}

	private void MakeReturnDocument()
	{
		_baseDocCash = "";
		if (string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text) && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnWithoutBase, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
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
				return;
			}
			_docIsSale = false;
			_docType = ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn;
		}
		_modernPOSSaleTotalUserControl.SamplePOSTotalsControl.Background = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#49504b")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#ffc7ce")));
		SamplePOSSaleHeader.TB_SaleNumCaption.Text = ArmSoft.SME.Trade.Common.Resources.ReturnWord + " N ";
		SamplePOSSaleHeader.TB_SaleNumCaption.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#fb7376")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326")));
		SamplePOSSaleHeader.TB_SaleNum.Foreground = SamplePOSSaleHeader.TB_SaleNumCaption.Foreground;
		SetNextDocumentNumber();
		_modernPOSSaleableGoodInfoUserControl.TB_GoodName.Text = "";
		_modernPOSSaleableGoodInfoUserControl.TB_GoodName.ToolTip = "";
		_modernPOSSaleableGoodInfoUserControl.TB_GoodRowDetail.Text = "";
		if (!string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim()))
		{
			SamplePOSSaleHeader.TB_BaseDoc.Text = $" ({ArmSoft.SME.Trade.Common.Resources.Base} {_posSearchPanelControl.CodeSearchTextEdit.Text.Trim()}) ";
			SamplePOSSaleHeader.TB_BaseDoc.Foreground = SamplePOSSaleHeader.TB_SaleNumCaption.Foreground;
			_posSearchPanelControl.CodeSearchTextEdit.Text = "";
			_realText = "";
		}
		else
		{
			SamplePOSSaleHeader.TB_BaseDoc.Text = "";
		}
		Title = SamplePOSSaleHeader.TB_SaleNumCaption.Text + SamplePOSSaleHeader.TB_SaleNum.Text;
		_saleTouchModeButtonsPadUserControl.ButtonReturnCaption.Text = ArmSoft.SME.Trade.Common.Resources.Sale;
	}

	private bool SelectBaseSaleForReturn()
	{
		DateOnly dateOnly = Convert.ToDateTime(StatusBar.TB_Time.Text).Date.ToDateOnly();
		if (!string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text))
		{
			List<Guid> list = (Settings.IsOffline ? Settings.LocalDBDocumentService.GetDocISNByECRN(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim(), dateOnly, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value) : TradeSettings.CreateTradeClient().CommonServices.GetDocISNByECRN(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim(), dateOnly, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value));
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
				BaseECRCheckDocuments dropDownView = new BaseECRCheckDocuments(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim(), Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value.ToString());
				TouchGridControl<BaseECRCheckDocumentsDataProvider.DataRow> touchGridControl = new TouchGridControl<BaseECRCheckDocumentsDataProvider.DataRow>(this, dropDownView, "BaseECRCheckDocumentsGrid");
				if (touchGridControl.ShowDialog().Value && touchGridControl.SelectedRow != null)
				{
					_baseDocISN = touchGridControl.SelectedRow.ISN;
				}
			}
		}
		else
		{
			IDropDownView dropDownView2 = ((!Settings.IsOffline) ? ((IDropDownView)new DocumentsJournal(this, ReportTypesSE.BillDocument, new HashSet<short> { ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value }, forPosReturn: true)
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
			TouchGridControl<TradeDocumentDataRow> touchGridControl2 = new TouchGridControl<TradeDocumentDataRow>(this, dropDownView2, dropDownView2.Name);
			if (touchGridControl2.ShowDialog().Value && touchGridControl2.SelectedRow != null)
			{
				_baseDocISN = touchGridControl2.SelectedRow.ISN;
			}
		}
		if (_baseDocISN == Guid.Empty)
		{
			return false;
		}
		_sumFromRound = 0m;
		MTBill mTBill = ((!Settings.IsOffline) ? Settings.DocumentService.Load<MTBill>(_baseDocISN, withoutAccessCheck: true) : Settings.LocalDBDocumentService.Load<MTBill>(_baseDocISN));
		if (mTBill != null)
		{
			mTBill.SumAddedBonus = 0m;
			LoadMTBillDocument(mTBill, loadBaseDoc: true);
			_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text = (mTBill.SummaCash.Value - mTBill.SummaChange.Value).CurrencyFormat(16, 2);
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
			GridControl materialsGridControl = MaterialsGridControl;
			BindingList<SaleRow> materialsRowsList = _materialsRowsList;
			materialsGridControl.CurrentItem = materialsRowsList[materialsRowsList.Count - 1];
		}
		UpdateChangeOrPaySum();
		return true;
	}

	private void LoadMTBillDocument(MTBill mTBillDocument, bool loadBaseDoc = false)
	{
		SamplePOSSaleHeader.TB_SaleNum.Text = mTBillDocument.DocNum;
		if (!loadBaseDoc)
		{
			_ecrCheckNum = mTBillDocument.ECRCheckNum;
			_prePaymentECRCheckNum = mTBillDocument.PrePaymentECRCheckNum;
		}
		StatusBar.TB_Time.Text = mTBillDocument.DocDate.ToString();
		_sumFromRound = mTBillDocument.SumFromRound;
		_modernPOSSaleTotalUserControl.TB_Total.Text = mTBillDocument.Summ.Value.CurrencyFormat(16, 2);
		_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text = mTBillDocument.SummaCash.Value.CurrencyFormat(16, 2);
		_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text = mTBillDocument.SummaPlCard.Value.CurrencyFormat(16, 2);
		_modernPOSSaleTotalUserControl.TB_MustPay.Text = mTBillDocument.SummaChange.Value.CurrencyFormat(16, 2);
		_vatType = mTBillDocument.VATType;
		_currCode = mTBillDocument.CurrCode;
		_defaultStorage = mTBillDocument.Storage;
		_defaultCash = mTBillDocument.CashDesk;
		_partnerCode = mTBillDocument.PartnerContractGroup.PartnerCode;
		_partnerID = mTBillDocument.PartnerID;
		_contract = mTBillDocument.PartnerContractGroup.ContractCode;
		_defaultSalesConsultant = mTBillDocument.SalesConsultant;
		_modernPOSSaleDiscountInfoUserControl.TB_DiscountPercent.Text = mTBillDocument.Discount.CurrencyFormat(6, 2);
		_bankAcquiringPartner = mTBillDocument.BankAcquiring.PartnerCode;
		_bankAcquiringContract = mTBillDocument.BankAcquiring.ContractCode;
		_prepaymentPartnerCode = mTBillDocument.PrepaymentGroup.PartnerCode;
		_prepaymentContractCode = mTBillDocument.PrepaymentGroup.ContractCode;
		_posTerminalType = mTBillDocument.POSTerminalType;
		_ecrCRN = mTBillDocument.ECRCRN;
		_nonCashPaymentMethod = mTBillDocument.NonCashPaymentMethod;
		_nonCashPaymentId = mTBillDocument.PaymentId;
		_paymentAccepted = mTBillDocument.PaymentAccepted;
		decimal? summaPlCard = mTBillDocument.SummaPlCard;
		_nonCashPaymentMethodSelected = (summaPlCard.GetValueOrDefault() > 0m) & summaPlCard.HasValue;
		if (mTBillDocument.PrintPartnerTinOnCheck.Value)
		{
			PartnerCacheRow itemById = TradeSettings.TradeCacheService.Partner.GetItemById(mTBillDocument.PartnerID);
			if (itemById != null)
			{
				_partnerTaxCode = itemById.TaxCode;
			}
		}
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
		_modernPOSSaleTotalUserControl.TB_Discount.Text = num.CurrencyFormat(16, 2);
		SetStatusBar_Quantity();
		_saleConsultantCode = mTBillDocument.SalesConsultant;
		SetStatusBar_SaleConsultant(_saleConsultantCode);
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partnerCode, _contract, out var partnerCacheRow);
		if (contract != null)
		{
			if (string.IsNullOrWhiteSpace(contract.DiscountCard))
			{
				FillPartnerCardInfo(_partnerCode, partnerCacheRow, contract);
				_modernPOSSaleDiscountInfoUserControl.TB_CardNumber.Text = "";
			}
			else
			{
				FillPartnerCardInfo(contract.DiscountCard, partnerCacheRow, contract);
			}
		}
		SetBuyerInfoToPoleDisplay();
		RowChanged();
	}

	private void LoadMTReturnFromCustomerDocument(MTReturnFromCustomerBase mtReturnFromCustomerBase)
	{
		SamplePOSSaleHeader.TB_SaleNum.Text = mtReturnFromCustomerBase.DocNum;
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
		}
		PaymentRefundAccepted = mtReturnFromCustomerBase.PaymentRefundAccepted;
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partnerID, _contract, out var partnerCacheRow);
		if (contract != null)
		{
			if (string.IsNullOrWhiteSpace(contract.DiscountCard))
			{
				FillPartnerCardInfo(_partnerCode, partnerCacheRow, contract);
				_modernPOSSaleDiscountInfoUserControl.TB_CardNumber.Text = "";
			}
			else
			{
				FillPartnerCardInfo(contract.DiscountCard, partnerCacheRow, contract);
			}
		}
		_modernPOSSaleTotalUserControl.TB_Total.Text = mtReturnFromCustomerBase.Summ.Value.CurrencyFormat(16, 2);
		_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text = mtReturnFromCustomerBase.ECRReturnByCashSum.CurrencyFormat(16, 2);
		_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text = mtReturnFromCustomerBase.ECRReturnByCardSum.CurrencyFormat(16, 2);
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
		_modernPOSSaleTotalUserControl.TB_Discount.Text = num.CurrencyFormat(16, 2);
		SetStatusBar_Quantity();
		if (string.IsNullOrWhiteSpace(_contract))
		{
			_modernPOSSaleDiscountInfoUserControl.TB_CardNumber.Text = "";
			_modernPOSSaleDiscountInfoUserControl.TB_CardHolderName.Text = "";
			_modernPOSSaleDiscountInfoUserControl.DiscountGrid.Visibility = Visibility.Hidden;
			_modernPOSSaleDiscountInfoUserControl.TB_DiscountPercent.Text = 0m.CurrencyFormat(6, 2);
		}
		SetBuyerInfoToPoleDisplay();
		RowChanged();
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
			decimal num = Convert.ToDecimal(_modernPOSSaleDiscountInfoUserControl.TB_DiscountPercent.Text);
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
		_modernPOSSaleTotalUserControl.TB_Discount.Text = value.CurrencyFormat(16, 2);
		MaterialsGridControl.RefreshData();
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
			GiftSetRows dropDownView = new GiftSetRows(value);
			TouchGridControlWithExtraButtons<GiftSetRowsDataProvider.DataRow> touchGridControlWithExtraButtons = new TouchGridControlWithExtraButtons<GiftSetRowsDataProvider.DataRow>(this, dropDownView, "SelectAndAddGiftGoods");
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
		}
		foreach (GiftSetRowsDataProvider.DataRow item3 in list)
		{
			_discountsCalculator.GenerateNewGiftDiscountsForDocument(dictionary);
			_discountsCalculator.CalculateNewGeneratedGiftDiscount(dictionary);
			MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(item3.MTCode);
			if (item2 != null)
			{
				AddGiftItemToList(item2, item3.Summ, item3.Quantity);
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
		UpdateChangeOrPaySum();
		OnFocusedRowHandleChanged();
		MaterialsGridControl.RefreshData();
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
		if (_docIsSale && Settings.ParametersService.DiscountUse() && Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.IsOffline)
		{
			AutoCalculateDiscount();
		}
		MaterialsGridControl.RefreshData();
		UpdateChangeOrPaySum();
		if (_materialsRowsList.Count == 0)
		{
			RowInfoChangedEvent?.Invoke(null);
		}
		CalculateDocumentSumma();
		SetStatusBar_Quantity();
	}

	private void EditRow(SaleRow row, SaleWindowHelper.CurrentFocusedField focusedField, bool editMode = true)
	{
		SaleEditRowWindow saleEditRowWindow = new SaleEditRowWindow(this, row, !_docIsSale, TouchMode != POSScreenModesSE.Keyboard.Value, _isRetail, _priceListType, _priceListTypeRoundDgr, _vatType, Convert.ToDateTime(StatusBar.TB_Time.Text), _currCode, focusedField, _materialsRowsList.SelectMany((SaleRow saleRow) => saleRow.MarkingList).ToList(), fromSamplePOS: true, editMode);
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
		if (_docIsSale && Settings.ParametersService.DiscountUse() && Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.IsOffline)
		{
			AutoCalculateDiscount();
		}
		MaterialsGridControl.RefreshData();
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
		if (MaterialsGridControl.CurrentItem == null)
		{
			return;
		}
		try
		{
			SaleRow saleRow = (SaleRow)MaterialsGridControl.CurrentItem;
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
			if (saleRow.Quantity != 0m)
			{
				saleRow.UsedBonus = 0m;
			}
			saleRow.Quantity = quantity;
			if (Settings.ParametersService.DiscountUse())
			{
				if (Settings.ParametersService.AlwaysCalcDiscountAuto() && _docIsSale && !Settings.IsOffline)
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
			MaterialsGridControl.RefreshData();
			CalculateDocumentSumma();
			SetStatusBar_Quantity();
			RowInfoChangedEvent?.Invoke(saleRow);
		}
		_posSearchPanelControl.CodeSearchTextEdit.Text = "";
		_realText = "";
		_posSearchPanelControl.CodeSearchTextEdit.Focus();
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
		MaterialsGridControl.RefreshData();
		CalculateDocumentSumma();
		RowInfoChangedEvent?.Invoke(saleRow);
		_posSearchPanelControl.CodeSearchTextEdit.Text = "";
		_posSearchPanelControl.CodeSearchTextEdit.Focus();
	}

	private void SetStatusBar_Quantity()
	{
		StatusBar.TB_QTY.Text = _materialsRowsList.Where((SaleRow r) => !r.IsDeleted && !r.IsGift).Sum((SaleRow r) => r.Quantity).CurrencyFormat(13, 3);
	}

	private void OnPriceButtonClick()
	{
		_posSearchPanelControl.CodeSearchTextEdit.DoValidate();
		if (!_docIsSale && _baseDocISN != Guid.Empty)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowChangePriceForReturn);
		}
		if (MaterialsGridControl.CurrentItem == null || ((SaleRow)MaterialsGridControl.CurrentItem).IsGift)
		{
			return;
		}
		try
		{
			if (string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text))
			{
				EditRow((SaleRow)MaterialsGridControl.CurrentItem, SaleWindowHelper.CurrentFocusedField.SalePrice);
				return;
			}
			if (!_posSearchPanelControl.CodeSearchTextEdit.Text.Trim().IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
			}
			if (decimal.TryParse(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim(), out var result))
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
					ChangePriceAndRefreshGrid((SaleRow)MaterialsGridControl.CurrentItem, num);
				}
				else if (new UserAuthenticationDialogWindow(this, ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowPOSChangePrice.Id).ShowDialog().Value)
				{
					ChangePriceAndRefreshGrid((SaleRow)MaterialsGridControl.CurrentItem, num);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			_posSearchPanelControl.CodeSearchTextEdit.Text = "";
			_realText = "";
		}
	}

	private void DeleteRowAndRefreshDocumentFields(SaleRow saleRow)
	{
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
		if (_docIsSale && Settings.ParametersService.DiscountUse() && Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.IsOffline)
		{
			AutoCalculateDiscount();
		}
		MaterialsGridControl.RefreshData();
		UpdateChangeOrPaySum();
		if (_materialsRowsList.Count == 0)
		{
			_modernPOSSaleableGoodInfoUserControl.GoodRemInfoGrid.Background = new SolidColorBrush(Colors.Transparent);
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
			MaterialsGridControl.Focus();
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, materialCode));
		}
		_qty_Weight = 1m;
		SaleRow saleRow = AddItemToList(item, materialCode, barCode: false, 0m, materialUnitCode);
		bool flag = item.IsWeight && _modernPOSSaleableGoodInfoUserControl.ButtonQTY.Visibility == Visibility.Visible;
		if (_useMarking | flag)
		{
			bool flag2 = MarkingHelpers.IsGoodMarkable(saleRow.ItemCode, _useMarking, saleRow.MarkingList.Count != 0, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), out var isRowMarkable);
			saleRow.IsRowMarkable = isRowMarkable;
			if ((_immediatlyOpenMarkingWindow & flag2) | flag)
			{
				EditRow(saleRow, SaleWindowHelper.CurrentFocusedField.Quantity, editMode: false);
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
		string likeName = _posSearchPanelControl.CodeSearchTextEdit.Text.Trim();
		string text = "";
		_posSearchPanelControl.CodeSearchTextEdit.Text = "";
		_realText = "";
		if (Settings.IsOffline)
		{
			OfflineGoodsFilter dropDownView = new OfflineGoodsFilter(_priceListType, likeName);
			TouchGridControlWithExtraButtons<OfflineGoodsFilterDataProvider.DataRow> touchGridControlWithExtraButtons = new TouchGridControlWithExtraButtons<OfflineGoodsFilterDataProvider.DataRow>(this, dropDownView, "MaterialSelectTouchDialog")
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
			ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices dropDownView2 = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices(StatusBar.TB_Time.Text.ToDateOnly(), _defaultStorage, _priceListType, _isRetail, likeName, "", extendedView: true, isFilteredSearchView: false, Settings.ParametersService.AllowPOSShowAvailability());
			TouchGridControlWithExtraButtons<ArmSoft.SME.Trade.Client.Data.DropDowns.GoodsAvailabilityFilterForInvoices.DataRow> touchGridControlWithExtraButtons2 = new TouchGridControlWithExtraButtons<ArmSoft.SME.Trade.Client.Data.DropDowns.GoodsAvailabilityFilterForInvoices.DataRow>(this, dropDownView2, "MaterialSelectTouchDialog")
			{
				AllowFindAndFilterByBarCode = true
			};
			if (touchGridControlWithExtraButtons2.ShowDialog().Value && touchGridControlWithExtraButtons2.SelectedRow != null)
			{
				text = touchGridControlWithExtraButtons2.SelectedRow.MTCode;
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
			_posSearchPanelControl.CodeSearchTextEdit.Focus();
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text));
		}
		SaleRow saleRow = AddItemToList(item, text, barCode: false);
		bool flag = item.IsWeight && _modernPOSSaleableGoodInfoUserControl.ButtonQTY.Visibility == Visibility.Visible;
		if (_useMarking | flag)
		{
			bool flag2 = MarkingHelpers.IsGoodMarkable(saleRow.ItemCode, _useMarking, saleRow.MarkingList.Count != 0, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), out var isRowMarkable);
			saleRow.IsRowMarkable = isRowMarkable;
			if ((_immediatlyOpenMarkingWindow & flag2) | flag)
			{
				EditRow(saleRow, SaleWindowHelper.CurrentFocusedField.Quantity, editMode: false);
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
		MaterialsGridControl.CurrentItem = saleRow;
		if (Settings.ParametersService.DiscountUse())
		{
			if (_docIsSale && Settings.ParametersService.DiscountExtensionUse() && !Settings.IsOffline)
			{
				if (!tryCallDiscountExtension)
				{
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
		MaterialsGridControl.RefreshData();
		MaterialsGridControl.RefreshRow(num);
		CalculateDocumentSumma();
		SetStatusBar_Quantity();
		_posSearchPanelControl.CodeSearchTextEdit.Text = "";
		_realText = "";
		_posSearchPanelControl.CodeSearchTextEdit.Focus();
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
							_posSearchPanelControl.CodeSearchTextEdit.Text = "";
							_realText = "";
							_posSearchPanelControl.CodeSearchTextEdit.Focus();
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
		return MaterialsGridControl.GetRowHandleByListIndex(_materialsRowsList.Count - 1);
	}

	private int AddNewRowOrMergeInner(MaterialCacheRow materialCacheRow, string identificValue, ref SaleRow saleRow, bool barCode = true, decimal retailPrice = 0m, bool forFocusedRow = false, string qntUnit = "")
	{
		decimal num = 0m;
		int result = -1;
		SaleRow saleRow2 = null;
		if (forFocusedRow && CalcMTBillListRowCount(includeDeleted: true) != 0)
		{
			saleRow2 = (SaleRow)MaterialsGridControl.CurrentItem;
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
				result = MaterialsGridControl.GetRowHandleByListIndex(i);
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
						_posSearchPanelControl.CodeSearchTextEdit.Text = "";
						_realText = "";
						_posSearchPanelControl.CodeSearchTextEdit.Focus();
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
				result = MaterialsGridControl.GetRowHandleByListIndex(i);
				break;
			}
			for (int j = 0; j <= _materialsRowsList.Count - 1; j++)
			{
				if (_materialsRowsList[j].ItemID == materialCacheRow.Id && _materialsRowsList[j].UnitCode == materialCacheRow.Unit && _materialsRowsList[j].Price == num)
				{
					_materialsRowsList[j].Quantity += _qty_Weight;
					saleRow = _materialsRowsList[j];
					result = MaterialsGridControl.GetRowHandleByListIndex(j);
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
				result = MaterialsGridControl.GetRowHandleByListIndex(_materialsRowsList.Count - 1);
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
			saleRow.Discount = Convert.ToDecimal(_modernPOSSaleDiscountInfoUserControl.TB_DiscountPercent.Text);
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
		else if (Convert.ToDecimal(_modernPOSSaleDiscountInfoUserControl.TB_DiscountPercent.Text) != 0m)
		{
			saleRow.Discount = Convert.ToDecimal(_modernPOSSaleDiscountInfoUserControl.TB_DiscountPercent.Text);
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
		CalculateTotalAmount();
		UpdateChangeOrPaySum();
		OnFocusedRowHandleChanged();
		MaterialsGridControl.RefreshData();
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
		_modernPOSSaleTotalUserControl.TB_MustPay.Text = num2.CurrencyFormat(16, 2);
		_modernPOSSaleTotalUserControl.TB_Total.Text = num2.CurrencyFormat(16, 2);
		_modernPOSSaleTotalUserControl.TB_Discount.Text = _temporaryDiscountSumm.CurrencyFormat(16, 2);
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
		_posSearchPanelControl.CodeSearchTextEdit.Text = "";
		_realText = "";
		ArmSoft.SME.Trade.UI.DropDown.PartnersFilter dropDownView = new ArmSoft.SME.Trade.UI.DropDown.PartnersFilter(ArmSoft.SME.Trade.UI.DropDown.PartnersFilter.ShowPartnersModeType.FindBuyerPOS, "", isFilterBySuppliers: false, isFilterByCustomers: true);
		TouchGridControlWithExtraButtons<PartnersFilterDataProvider.DataRow> touchGridControlWithExtraButtons = new TouchGridControlWithExtraButtons<PartnersFilterDataProvider.DataRow>(this, dropDownView, "FindBuyerTouchDialog");
		if (touchGridControlWithExtraButtons.ShowDialog().Value && touchGridControlWithExtraButtons.SelectedRow != null)
		{
			if (!string.IsNullOrWhiteSpace(_partnerCode))
			{
				ClearPartnerCard();
			}
			text = touchGridControlWithExtraButtons.SelectedRow.PartCode;
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
			_posSearchPanelControl.CodeSearchTextEdit.DoValidate();
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
				ReadMagneticCard(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim(), byMagneticCardPrefixSuffix: false, byCleanButtonClick);
			}
			else if (new UserAuthenticationDialogWindow(this, num).ShowDialog().Value)
			{
				ReadMagneticCard(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim(), byMagneticCardPrefixSuffix: false, byCleanButtonClick);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			_posSearchPanelControl.CodeSearchTextEdit.Text = "";
			_realText = "";
			CodeTextEdit_FocusAndMoveCursorToEnd();
		}
	}

	private void EMarkReceiptClick()
	{
		MarkingHelpers.ExportEMarkReceipts(this, viewMode: false, forPOS: true, _defaultStorage);
	}

	private void NewBuyerClick()
	{
		try
		{
			SamplePOSNewBuyerWindow samplePOSNewBuyerWindow = new SamplePOSNewBuyerWindow(this);
			if (samplePOSNewBuyerWindow.ShowDialog().Value && !string.IsNullOrWhiteSpace(samplePOSNewBuyerWindow.PartnerCode))
			{
				if (!string.IsNullOrWhiteSpace(_partnerCode))
				{
					ClearPartnerCard();
				}
				PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(samplePOSNewBuyerWindow.PartnerCode, out var partnerCacheRow);
				if (defaultContract.IsBonusCard)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsNotAllowedInSamplePOS);
				}
				FillBuyerFields(defaultContract.DiscountCard, partnerCacheRow, defaultContract);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
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
					DateOnly value = StatusBar.TB_Time.Text.ToDateOnly();
					_isPartBonusCardValid = (!defaultContract.BonusCardValidPeriodStart.HasValue || (defaultContract.BonusCardValidPeriodStart.HasValue && defaultContract.BonusCardValidPeriodStart <= value)) && (!defaultContract.BonusCardValidPeriodEnd.HasValue || (defaultContract.BonusCardValidPeriodEnd.HasValue && defaultContract.BonusCardValidPeriodEnd >= value));
					if (_connectTob2barmsoftDiscountServer)
					{
						_isPartBonusCardValid = _isPartBonusCardValid && defaultContract.BonusCardValidPeriodStart.HasValue;
					}
					if (_docIsSale)
					{
						if (defaultContract.BonusCardCanceled)
						{
							ClearPartnerCard();
							throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsCanceled);
						}
						if (!_isPartBonusCardValid)
						{
							ClearPartnerCard();
							throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsNotValid);
						}
					}
					_modernPOSSaleDiscountInfoUserControl.TB_CardNumber.Text = ArmSoft.SME.Common.Resources.OptionsTaxCode + " - " + taxCode;
					_modernPOSSaleDiscountInfoUserControl.TB_CardHolderName.Text = partner.Name + " (" + ArmSoft.SME.Trade.Common.Resources.DefaultContract + ")";
					_modernPOSSaleDiscountInfoUserControl.DiscountGrid.Visibility = Visibility.Visible;
					_modernPOSSaleDiscountInfoUserControl.TB_DiscountPercent.Text = 0m.CurrencyFormat(6, 2);
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
			_posSearchPanelControl.CodeSearchTextEdit.Text = "";
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
						_modernPOSSaleDiscountInfoUserControl.TB_CardNumber.Text = "";
					}
					else
					{
						FillPartnerCardInfo(defaultContract.DiscountCard, item, defaultContract);
					}
					if (_docIsSale)
					{
						if (Settings.ParametersService.DiscountUse())
						{
							if (!string.IsNullOrWhiteSpace(defaultContract.PriceType) && _priceListType != defaultContract.PriceType)
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
			_posSearchPanelControl.CodeSearchTextEdit.Text = "";
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
					_posSearchPanelControl.CodeSearchTextEdit.Text = "";
					_realText = "";
					_posSearchPanelControl.CodeSearchTextEdit.Focus();
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
						if (!string.IsNullOrWhiteSpace(contractByDiscountCard?.PriceType) && _priceListType != contractByDiscountCard.PriceType)
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
		DateOnly value = Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly();
		_isPartBonusCardValid = (((!partnerContract.BonusCardValidPeriodStart.HasValue || (partnerContract.BonusCardValidPeriodStart.HasValue && partnerContract.BonusCardValidPeriodStart <= value)) && (!partnerContract.BonusCardValidPeriodEnd.HasValue || (partnerContract.BonusCardValidPeriodEnd.HasValue && partnerContract.BonusCardValidPeriodEnd >= value))) ? true : false);
		if (_connectTob2barmsoftDiscountServer)
		{
			_isPartBonusCardValid = _isPartBonusCardValid && partnerContract.BonusCardValidPeriodStart.HasValue;
		}
		if (_docIsSale)
		{
			if (partnerContract.BonusCardCanceled)
			{
				ClearPartnerCard();
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsCanceled);
			}
			if (!_isPartBonusCardValid)
			{
				ClearPartnerCard();
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.CardIsNotValid);
			}
		}
		if (Settings.ParametersService.DiscountUse())
		{
			if (_docIsSale && Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.IsOffline)
			{
				AutoCalculateDiscount();
				CalculateDocumentSumma();
			}
			_modernPOSSaleDiscountInfoUserControl.TB_DiscountPercent.Text = partnerContract.Percent.CurrencyFormat(6, 2);
		}
		if (byTaxCode)
		{
			_modernPOSSaleDiscountInfoUserControl.TB_CardNumber.Text = ArmSoft.SME.Common.Resources.OptionsTaxCode + " - " + cardNumber;
		}
		else
		{
			_modernPOSSaleDiscountInfoUserControl.TB_CardNumber.Text = cardNumber;
		}
		_modernPOSSaleDiscountInfoUserControl.TB_CardHolderName.Text = partnerCacheRow.Caption + " (" + partnerContract.Caption + ")";
		_modernPOSSaleDiscountInfoUserControl.DiscountGrid.Visibility = Visibility.Visible;
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
				if (partnerContract.IsBonusCard)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.BonusCardIsNotAllowedInSamplePOS);
				}
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
		_modernPOSSaleDiscountInfoUserControl.TB_CardNumber.Text = "";
		_modernPOSSaleDiscountInfoUserControl.TB_CardHolderName.Text = "";
		_modernPOSSaleDiscountInfoUserControl.DiscountGrid.Visibility = Visibility.Hidden;
		_modernPOSSaleDiscountInfoUserControl.TB_DiscountPercent.Text = 0m.CurrencyFormat(6, 2);
		ModifyPartnerCardButtonText(_posSearchPanelControl.CodeSearchTextEdit.Text);
		_contract = "";
		_partnerID = -1;
		_partnerCode = "";
		_priceListType = Settings.ParametersService.DefaultMTCheckPriceType();
		_priceListTypeRoundDgr = TradeSettings.TradeCacheService.PriceType.GetItem(_priceListType)?.Round ?? "";
		if (_docIsSale && Settings.ParametersService.DiscountUse() && Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.IsOffline)
		{
			AutoCalculateDiscount();
			MaterialsGridControl.RefreshData();
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
			_saleTouchModeButtonsPadUserControl.ButtonCardCaption.Text = ArmSoft.SME.Common.Resources.ClearWord + " " + ArmSoft.SME.Trade.Common.Resources.TheCard;
			_saleTouchModeButtonsPadUserControl.ButtonCardCaption.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#fb7376" : "#dd0326");
		}
		else
		{
			_saleTouchModeButtonsPadUserControl.ButtonCardCaption.Text = ArmSoft.SME.Trade.Common.Resources.Buyers + " " + ArmSoft.SME.Trade.Common.Resources.Card;
			_saleTouchModeButtonsPadUserControl.ButtonCardCaption.Foreground = _saleTouchModeButtonsPadUserControl.ButtonMTFavoritesCaption.Foreground;
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
			if (!Settings.ParametersService.AlwaysCalcDiscountAuto() && !Settings.ParametersService.CalcDiscountAuto() && Convert.ToDecimal(_modernPOSSaleDiscountInfoUserControl.TB_DiscountPercent.Text) != 0m)
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

	private bool ReadSaleConsultantMagneticCard(string cardNumber)
	{
		try
		{
			PartnerCacheRow partnerCacheRow;
			if (string.IsNullOrWhiteSpace(cardNumber))
			{
				_saleConsultantCode = "";
			}
			else if (TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(cardNumber, out partnerCacheRow) == null)
			{
				EmployeeCacheRow employeeByCard = TradeSettings.TradeCacheService.Employee.GetEmployeeByCard(cardNumber);
				if (employeeByCard == null)
				{
					return false;
				}
				_saleConsultantCode = employeeByCard.EmployeeCode;
			}
			return true;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		return true;
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
		}
		else
		{
			_saleConsultantCode = item.EmployeeCode;
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

	private void SamplePOSSaleHeader_ButtonClick(object sender, SaleWindowHelper.ButtonClickEventArgs e)
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
		ButtonClick(e.ButtonCommand);
	}

	private void SaleTouchButtonsPad1BaseUserControl_ButtonClick(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.Numeric)
		{
			_posSearchPanelControl.CodeSearchTextEdit.EditValue = _posSearchPanelControl.CodeSearchTextEdit.Text + ((SaleWindowHelper.ButtonNumericClickEventArgs)e).ButtonText;
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
				HandleSampleCashPayment();
				break;
			case SaleWindowHelper.ButtonCommands.NotCash:
				HandleSampleNonCashPayment();
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
			case SaleWindowHelper.ButtonCommands.Save:
				OnSaveSaleClick();
				break;
			case SaleWindowHelper.ButtonCommands.Exit:
				if (_materialsRowsList != null && _materialsRowsList.Count > 0)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.BeforExitStoreOrDefer);
				}
				DialogResult = true;
				DeleteAutoSavedDocument();
				break;
			case SaleWindowHelper.ButtonCommands.FocusOnInputField:
				_posSearchPanelControl.CodeSearchTextEdit.Focus();
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
		SaleRow saleRow = (SaleRow)MaterialsGridControl.CurrentItem;
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
		MaterialsGridControl.RefreshData();
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

	private void ModernSaleWindow_Activated(object sender, EventArgs e)
	{
		_posSearchPanelControl.CodeSearchTextEdit.Focus();
	}

	private void SaleWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
	{
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Invalid comparison between Unknown and I4
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Invalid comparison between Unknown and I4
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Invalid comparison between Unknown and I4
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Invalid comparison between Unknown and I4
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Invalid comparison between Unknown and I4
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Invalid comparison between Unknown and I4
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
				return;
			}
			string key = KeysHelper.ToKeyGesture(e.Key);
			if (_buttonCommandByKeyGesturesDictionary.ContainsKey(key))
			{
				e.Handled = true;
				ButtonClick(_buttonCommandByKeyGesturesDictionary[key]);
			}
			else
			{
				if (HotKeys.Single.Enter.Matches(this, e))
				{
					return;
				}
				if (HotKeys.WithModifier.Control_A.Matches(this, e) || HotKeys.WithModifier.Control_C.Matches(this, e) || HotKeys.WithModifier.Control_V.Matches(this, e) || HotKeys.WithModifier.Control_F.Matches(this, e))
				{
					e.Handled = false;
					return;
				}
				if (((int)e.Key >= 44 && (int)e.Key <= 69) || ((int)e.Key >= 34 && (int)e.Key <= 43) || ((int)e.Key >= 74 && (int)e.Key <= 89))
				{
					if (_posSearchPanelControl.CodeSearchTextEdit.SelectedText != "")
					{
						int selectionStart = _posSearchPanelControl.CodeSearchTextEdit.SelectionStart;
						_posSearchPanelControl.CodeSearchTextEdit.SelectedText = "";
						_posSearchPanelControl.CodeSearchTextEdit.Focus();
						_posSearchPanelControl.CodeSearchTextEdit.CaretIndex = selectionStart;
					}
					else
					{
						FavoritesTouchPadControl favoritesTouchPadControl = _favoritesTouchPadControl;
						if (favoritesTouchPadControl != null && favoritesTouchPadControl.IsSearchPanelFocused)
						{
							goto IL_02ac;
						}
						if (MaterialsGridTableView.ActualShowSearchPanel)
						{
							SearchControl searchControl = MaterialsGridTableView.SearchControl;
							if (searchControl != null && searchControl.IsKeyboardFocusWithin)
							{
								goto IL_02ac;
							}
						}
						if (_posSearchPanelControl.CodeSearchTextEdit.CaretIndex == 0)
						{
							CodeTextEdit_FocusAndMoveCursorToEnd();
						}
						else
						{
							_posSearchPanelControl.CodeSearchTextEdit.Focus();
							_posSearchPanelControl.CodeSearchTextEdit.SelectionStart = _posSearchPanelControl.CodeSearchTextEdit.CaretIndex;
						}
					}
				}
				e.Handled = false;
			}
			return;
			IL_02ac:
			e.Handled = false;
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
			_modernPOSSaleableGoodInfoUserControl.BE_Discount.Visibility = Visibility.Collapsed;
			_modernPOSSaleableGoodInfoUserControl.BE_QtyMinus.Visibility = Visibility.Collapsed;
			_modernPOSSaleableGoodInfoUserControl.BE_QtyPlus.Visibility = Visibility.Collapsed;
			_modernPOSSaleableGoodInfoUserControl.ButtonQTY.Visibility = Visibility.Collapsed;
		}
		else
		{
			_modernPOSSaleableGoodInfoUserControl.BE_QtyMinus.Visibility = Visibility.Visible;
			_modernPOSSaleableGoodInfoUserControl.BE_QtyPlus.Visibility = Visibility.Visible;
			if (string.IsNullOrWhiteSpace(ElScaleInfo.GetInstance().ElScaleType))
			{
				_modernPOSSaleableGoodInfoUserControl.ButtonQTY.Visibility = Visibility.Collapsed;
			}
			else
			{
				_modernPOSSaleableGoodInfoUserControl.ButtonQTY.Visibility = Visibility.Visible;
			}
			if (Settings.ParametersService.DiscountUse())
			{
				if (Settings.ParametersService.CalcDiscountAuto() || Settings.ParametersService.AlwaysCalcDiscountAuto() || Settings.ParametersService.DiscountExtensionUse() || !_docIsSale)
				{
					_modernPOSSaleableGoodInfoUserControl.BE_Discount.Visibility = Visibility.Collapsed;
				}
				else if (Settings.ParametersService.AllowPOSChangePrice())
				{
					_modernPOSSaleableGoodInfoUserControl.BE_Discount.Visibility = Visibility.Visible;
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
		TableView materialsGridTableView = MaterialsGridTableView;
		object originalSource = e.OriginalSource;
		TableViewHitInfo tableViewHitInfo = materialsGridTableView.CalcHitInfo((DependencyObject)((originalSource is DependencyObject) ? originalSource : null));
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
			SaleRow saleRow = (SaleRow)MaterialsGridControl.GetRow(tableViewHitInfo.RowHandle);
			if (!saleRow.IsDeleted && currentFocusedField != SaleWindowHelper.CurrentFocusedField.None)
			{
				EditRow(saleRow, currentFocusedField);
			}
		}
	}

	public void OnScanner(string scannerData)
	{
		if (scannerData == "AS$#PayByCash")
		{
			ButtonClick(SaleWindowHelper.ButtonCommands.Cash);
			return;
		}
		if (scannerData == "AS$#PayByNonCash")
		{
			ButtonClick(SaleWindowHelper.ButtonCommands.NotCash);
			return;
		}
		_posSearchPanelControl.CodeSearchTextEdit.Text = MarkingHelper.CleanScannerSymbols(scannerData);
		ReadBarCode(_posSearchPanelControl.CodeSearchTextEdit.Text);
		_posSearchPanelControl.CodeSearchTextEdit.Text = "";
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
		try
		{
			_realText += e.Text;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			_realText = "";
		}
	}

	private void CodeTextEdit_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
	{
		try
		{
			if (HotKeys.Single.Enter.Matches(this, e))
			{
				if (string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text))
				{
					HandleEnterPressingWithFastSaleMode();
				}
				else if (!_posSearchPanelControl.CodeSearchTextEdit.IsReadOnly)
				{
					e.Handled = true;
					_posSearchPanelControl.CodeSearchTextEdit.Text = MarkingHelper.CleanScannerSymbols(_realText.Contains('\u001d') ? _realText : _posSearchPanelControl.CodeSearchTextEdit.Text);
					OnSearchControlEnterClick();
				}
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

	private void ModernSaleWindow_Closed(object sender, EventArgs e)
	{
		try
		{
			if (TouchMode == POSScreenModesSE.SampleScreenWithFavorites.Value)
			{
				RegistryHelper.StoreToRegistry("CardSize", _favoritesTouchPadControl.CardView.FixedSize, "Document Windows\\POS");
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
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
				Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text),
				_temporaryDiscountSumm,
				Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text),
				Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text),
				0m,
				0m,
				0m,
				GetPaidSum() - Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text)
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
		string text = "";
		if (!string.IsNullOrWhiteSpace(_partnerCode))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partnerCode);
			if (item != null)
			{
				text = item.Caption;
			}
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetBuyerInfoToPoleDisplay(TradeSettings.PoleDisplay.SetBuyerInfo), new object[5] { _partnerCode, text, _contract, 0m, 0m });
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
		MTBill mTBill = null;
		if (_ecrCheckNum == "XXXXXXXX")
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
			mTBill = ((!Settings.IsOffline) ? Settings.DocumentService.Load<MTBill>(_baseDocISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill, withoutAccessCheck: true) : Settings.LocalDBDocumentService.Load<MTBill>(_baseDocISN));
			if (string.IsNullOrWhiteSpace(mTBill.ECRCheckNum))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyECRNumForBaseDoc, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			if (mTBill.ECRCheckNum == "XXXXXXXX" || mTBill.PrePaymentECRCheckNum == "XXXXXXXX")
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
				if (Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text) != 0m)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
					return;
				}
			}
			else if (Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text) != 0m)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
		}
		else if (cashesCacheRow.CashSummLimitType == CashLimitsSE.OnlyNonCash.Value)
		{
			if (Settings.ParametersService.ECRCheckPrintCreditAsPartial())
			{
				if (Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text) != 0m)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
					return;
				}
			}
			else if (Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text) != 0m)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
		}
		else if (cashesCacheRow.CashSummLimitType == CashLimitsSE.WithLimit.Value && Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text) > 300000m)
		{
			if (Settings.ParametersService.ECRCheckPrintCreditAsPartial())
			{
				if (Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text) != 0m)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
					return;
				}
			}
			else if (Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text) != 0m)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PayOnlyNonCash, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
		}
		if (!ECRPreliminaryChecksAndInitialization(cashesCacheRow, prepaymentCheck: false, _ecrCRN))
		{
			return;
		}
		_summaCashValue = Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text);
		if (_docIsSale)
		{
			if (!(Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.DiscountAndPrePayment.Value))
			{
			}
		}
		else
		{
			_summaCashValue -= Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_MustPay.Text);
		}
		_summaPlCardValue = Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text);
		TradeDocument tradeDocument = SaveSaleDocument(newAfterStore: false);
		if (tradeDocument == null)
		{
			return;
		}
		string templateCode;
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
				_clonedBill.SumAddedBonus = 0m;
				_clonedBill.SummaPrePayment = 0m;
				obj = CreatePrintRequest(cashesCacheRow);
				templateCode = PrintTemplateHelper.CombinePrintTemplateName(18, 3);
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
				templateCode = PrintTemplateHelper.CombinePrintTemplateName(38, 3);
				checkType = ECRPrintCheckWindow.ECRCheckType.Return;
			}
			if (_posTerminalType == "2")
			{
				if (_docIsSale)
				{
					posTerminalResponse = PayWithArcus2IfChosen((MTBill)tradeDocument);
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
					posTerminalResponse = RefundArcus2IfChosen((MTBillReturn)tradeDocument);
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
		catch (Arcus2Exception ex)
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
			_ = _posTerminalType == "1";
			ECRPrintCheckWindow eCRPrintCheckWindow = new ECRPrintCheckWindow(this, _ecr, cashesCacheRow.ECROperator, cashesCacheRow.ECROperatorPassword, cashesCacheRow.ShowECRTracking, obj, checkType, (ECRPrintCheckWindow window, Utilities.PrintReceiptResponse response) =>
			{
				ECRHelpers.PrintOnOtherPrinter(window, response, cashesCacheRow, tradeDocument, templateCode);
			}, (string checkNumber, string errorMessage) =>
			{
				int num = 1;
				List<PropertyWithValue> list = new List<PropertyWithValue>(num);
				CollectionsMarshal.SetCount(list, num);
				CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("ECRCheckNum", checkNumber);
				List<PropertyWithValue> editedProperties = list;
				tradeDocument.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, errorMessage, tradeDocument.ISN, editedProperties, tradeDocument.TS);
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
				if (_docIsSale && _nonCashPaymentMethod == 0 && _posTerminalType == "2")
				{
					ConfirmArcus2PaymentIfDone(posTerminalResponse, (MTBill)tradeDocument);
				}
			}
			_summaCashValue = Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text);
			_summaPlCardValue = Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text);
		}
		finally
		{
			if (flag)
			{
				NewAfterStore();
			}
			else if (_docIsSale && _nonCashPaymentMethod == 0 && _posTerminalType == "2")
			{
				try
				{
					CancelArcus2PaymentIfDone(posTerminalResponse, (MTBill)tradeDocument);
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
		decimal num = Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text);
		decimal num2 = Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text) - Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_MustPay.Text);
		decimal num3 = 0m;
		MTBill mTBill = ((!Settings.IsOffline) ? Settings.DocumentService.Load<MTBill>(_baseDocISN, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill, withoutAccessCheck: true) : Settings.LocalDBDocumentService.Load<MTBill>(_baseDocISN));
		decimal num4 = num + num2 + num3;
		if (num4 == 0m)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnSumsNotSet, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (num4 < Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnSumLessThanTotalSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (num4 > Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text))
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
		if (Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.Cash.Value)
		{
			summaCashValue += 0m;
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
			partialAmount = 0.0,
			prePaymentAmount = 0.0,
			paidAmountCard = decimal.ToDouble(summaPlCardValue),
			useExtPOS = (_posTerminalType != POSTerminalTypesSE.ECRPOS.Value),
			PaymentSystem = paymentSystem,
			partnerTin = _partnerTaxCode,
			items = items,
			eMarks = CreateECREMarksList()
		};
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
		cashAmountForReturn += _summaCashValue;
		Utilities.PrintReturnReceiptRequest printReturnReceiptRequest = new Utilities.PrintReturnReceiptRequest
		{
			returnTicketId = Convert.ToInt32(mTBill.ECRCheckNum),
			cardAmountForReturn = _summaPlCardValue,
			cashAmountForReturn = cashAmountForReturn,
			prePaymentAmountForReturn = 0m,
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

	private static PosTerminalResponse PayWithArcus2IfChosen(MTBill mtBillDocument)
	{
		if (mtBillDocument.POSTerminalType == "2")
		{
			decimal? summaPlCard = mtBillDocument.SummaPlCard;
			if (!((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue))
			{
				PosTerminalResponse posTerminalResponse = Arcus2Helpers.CheckAndPurchase(mtBillDocument.POSTerminalID, mtBillDocument.SummaPlCard.Value);
				return new PosTerminalResponse(posTerminalResponse.RRN, posTerminalResponse.TerminalId);
			}
		}
		return null;
	}

	private static void CancelArcus2PaymentIfDone(PosTerminalResponse posTerminalResponse, MTBill mtBillDocuemnt)
	{
		if (mtBillDocuemnt.POSTerminalType == "2" && posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN))
		{
			Arcus2Helpers.Cancel(mtBillDocuemnt.POSTerminalID, mtBillDocuemnt.SummaPlCard.Value, posTerminalResponse.RRN);
		}
	}

	private void ConfirmArcus2PaymentIfDone(PosTerminalResponse posTerminalResponse, MTBill mtBillDocument)
	{
		if (mtBillDocument.POSTerminalType == "2" && posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN))
		{
			UpdatePaymentId(posTerminalResponse.RRN, mtBillDocument);
		}
	}

	private void UpdatePaymentId(string paymentId, CashIn cashInDocument)
	{
		try
		{
			_nonCashPaymentId = "ARCUS 2 POS: " + paymentId;
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
			_nonCashPaymentId = "ARCUS 2 POS: " + paymentId;
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

	private PosTerminalResponse RefundArcus2IfChosen(MTBillReturn mtBillDocument)
	{
		PosTerminalResponse result = null;
		MTBill mTBill = (MTBill)Settings.DocumentService.Load(_baseDocISN, withoutAccessCheck: true);
		if (mTBill != null)
		{
			LoadMTBillDocument(mTBill);
		}
		if (mTBill.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && mTBill.POSTerminalType == "2" && !mtBillDocument.PaymentRefundAccepted)
		{
			decimal? summaPlCard = mTBill.SummaPlCard;
			if (!((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue))
			{
				string rrn = (string.IsNullOrWhiteSpace(mTBill.PaymentId) ? "" : mTBill.PaymentId.Split(':')[1].Trim());
				PosTerminalResponse posTerminalResponse = Arcus2Helpers.CheckAndRefund(mTBill.POSTerminalID, mtBillDocument.ECRReturnByCardSum, mTBill.SummaPlCard.Value, rrn);
				result = new PosTerminalResponse(posTerminalResponse.RRN, posTerminalResponse.TerminalId);
				UpdateReturnPaymentStatus(mtBillDocument);
			}
		}
		return result;
	}

	private void OnCashPaymentButtonClick()
	{
		try
		{
			decimal num = GetPaidSum() - Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text);
			if (string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text))
			{
				if (Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text) == 0m && num < 0m)
				{
					_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text = Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_MustPay.Text).CurrencyFormat(16, 2);
				}
			}
			else
			{
				if (!_posSearchPanelControl.CodeSearchTextEdit.Text.Trim().IsNumeric())
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
				}
				if (Convert.ToDecimal(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim()) < 0m)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.IncrementSumma, 0));
				}
				_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text = Convert.ToDecimal(_posSearchPanelControl.CodeSearchTextEdit.Text).CurrencyFormat(16, 2);
			}
			UpdateChangeOrPaySum();
			_posSearchPanelControl.CodeSearchTextEdit.Focus();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			_posSearchPanelControl.CodeSearchTextEdit.Text = "";
			_realText = "";
		}
	}

	private void OnClearCashPaymentButtonClick()
	{
		try
		{
			_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text = 0m.CurrencyFormat(16, 2);
			_paymentAccepted = false;
			UpdateChangeOrPaySum();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OnClearNonCashPaymentButtonClick()
	{
		try
		{
			_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text = 0m.CurrencyFormat(16, 2);
			_nonCashPaymentId = "";
			_paymentAccepted = false;
			ClearNonCashPayment();
			UpdateChangeOrPaySum();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OnClearPaymentButtonClick()
	{
		try
		{
			_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text = 0m.CurrencyFormat(16, 2);
			_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text = 0m.CurrencyFormat(16, 2);
			_nonCashPaymentId = "";
			_paymentAccepted = false;
			ClearNonCashPayment();
			UpdateChangeOrPaySum();
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
		_nonCashPaymentMethodForECRPOS = 0;
		_nonCashPaymentMethod = 0;
	}

	private void HandleEnterPressingWithFastSaleMode()
	{
		if (Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_MustPay.Text) == 0m)
		{
			ButtonClick(SaleWindowHelper.ButtonCommands.PrintCheck);
		}
		else
		{
			OnCashPaymentButtonClick();
		}
	}

	private void OnPlCardPaymentButtonClick()
	{
		try
		{
			_ = GetPaidSum() - Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text);
			decimal num = 0m;
			if (string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text))
			{
				num = Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_MustPay.Text);
			}
			else
			{
				if (!_posSearchPanelControl.CodeSearchTextEdit.Text.Trim().IsNumeric())
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
				}
				if (Convert.ToDecimal(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim()) < 0m)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.IncrementSumma, 0));
				}
				num = Convert.ToDecimal(_posSearchPanelControl.CodeSearchTextEdit.Text);
			}
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_defaultCash);
			List<CashCacheRow.NonCashInfo> list = item.NonCashInfoList.Where((CashCacheRow.NonCashInfo p) => !string.IsNullOrWhiteSpace(p.Partner)).ToList();
			if (!_nonCashPaymentMethodSelected)
			{
				CashCacheRow.NonCashInfo nonCashInfo = item.NonCashInfoList.FirstOrDefault((CashCacheRow.NonCashInfo p) => !string.IsNullOrWhiteSpace(p.Partner));
				if (nonCashInfo != null)
				{
					_bankAcquiringPartner = nonCashInfo.Partner;
					_bankAcquiringContract = nonCashInfo.Contract;
					_posTerminalType = nonCashInfo.Device;
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
			}
			if (list.Count != 1)
			{
				NonCashPaymentTouchWindow nonCashPaymentTouchWindow = new NonCashPaymentTouchWindow(this, _docIsSale, num, _nonCashPaymentMethod, nonCashPaymentTypeForSaleWindow, _bankAcquiringPartner, _bankAcquiringContract);
				if (nonCashPaymentTouchWindow.ShowDialog().Value)
				{
					_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text = nonCashPaymentTouchWindow.Amount.CurrencyFormat(16, 2);
					switch (nonCashPaymentTouchWindow.NonCashPaymentType)
					{
					case ArmSoft.SME.Common.Constants.Common.NonCashPaymentTypeForSaleWindow.BankCardWithIngenicoPOS:
						_posTerminalType = "2";
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
				}
			}
			else
			{
				_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text = num.CurrencyFormat(16, 2);
			}
			_posSearchPanelControl.CodeSearchTextEdit.Focus();
			UpdateChangeOrPaySum();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			_posSearchPanelControl.CodeSearchTextEdit.Text = "";
			_realText = "";
		}
	}

	private decimal GetPaidSum()
	{
		return Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text) + GetWithoutCashPaidSum();
	}

	private decimal GetWithoutCashPaidSum()
	{
		return Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text);
	}

	private void UpdateChangeOrPaySum()
	{
		decimal num = GetPaidSum() - Convert.ToDecimal(_modernPOSSaleTotalUserControl.TB_Total.Text);
		_modernPOSSaleTotalUserControl.TB_MustPay.Text = Math.Abs(num).CurrencyFormat(16, 2);
		SetTotalInfoToPoleDisplay();
		if (num < 0m)
		{
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.Text = ArmSoft.SME.Trade.Common.Resources.ForPay;
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#fb7376" : "#dd0326");
			_modernPOSSaleTotalUserControl.TB_MustPay.Foreground = _modernPOSSaleTotalUserControl.TB_MustPayCaption.Foreground;
		}
		else if (num > 0m)
		{
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.Text = ArmSoft.SME.Trade.Common.Resources.SummaChange;
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#4fad77" : "#217346");
			_modernPOSSaleTotalUserControl.TB_MustPay.Foreground = _modernPOSSaleTotalUserControl.TB_MustPayCaption.Foreground;
		}
		else
		{
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.Text = ArmSoft.SME.Trade.Common.Resources.SummaChange;
			_modernPOSSaleTotalUserControl.TB_MustPayCaption.Foreground = _modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Foreground;
			_modernPOSSaleTotalUserControl.TB_MustPay.Foreground = _modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Foreground;
		}
	}

	private void HandleSampleCashPayment()
	{
		if (Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_Cash.Text) == 0m)
		{
			OnCashPaymentButtonClick();
		}
		else if (string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text) || Convert.ToDecimal(_posSearchPanelControl.CodeSearchTextEdit?.Text) == 0m)
		{
			OnClearCashPaymentButtonClick();
		}
		else
		{
			OnCashPaymentButtonClick();
		}
	}

	private void HandleSampleNonCashPayment()
	{
		if (Convert.ToDecimal(_modernPOSSalePaymentMehtodsPadUserControl.TB_NoCash.Text) == 0m)
		{
			if (Settings.ParametersService.MTBillPlCardPaymentShowButton())
			{
				OnPlCardPaymentButtonClick();
			}
		}
		else if (string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text) || Convert.ToDecimal(_posSearchPanelControl.CodeSearchTextEdit?.Text) == 0m)
		{
			OnClearNonCashPaymentButtonClick();
		}
		else if (Settings.ParametersService.MTBillPlCardPaymentShowButton())
		{
			OnPlCardPaymentButtonClick();
		}
	}

	private void OnDiscountButtonClick()
	{
		SaleRow saleRow = (SaleRow)MaterialsGridControl.GetFocusedRow();
		if (saleRow != null && !saleRow.IsDeleted)
		{
			SaleDiscountsEditWindow saleDiscountsEditWindow = new SaleDiscountsEditWindow(this, saleRow, isReturn: false, TouchMode != POSScreenModesSE.Keyboard.Value, _priceListTypeRoundDgr, Convert.ToDateTime(StatusBar.TB_Time.Text), _currCode);
			if (saleDiscountsEditWindow.ShowDialog().Value)
			{
				saleRow.Discount = saleDiscountsEditWindow.DiscountPercent;
				saleRow.DiscountSumma = saleDiscountsEditWindow.DiscountSumm;
				saleRow.Summa = saleDiscountsEditWindow.DiscountedSumm;
			}
			MaterialsGridControl.RefreshData();
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
		if (MaterialsGridControl.CurrentItem == null)
		{
			return;
		}
		try
		{
			SaleRow saleRow = (SaleRow)MaterialsGridControl.CurrentItem;
			bool flag = false;
			if (string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text))
			{
				if (TradeSettings.TradeCacheService.Material.GetItemById(saleRow.ItemID).IsWeight)
				{
					decimal weight = ScaleConnector.GetWeight();
					if (weight != 0m)
					{
						_posSearchPanelControl.CodeSearchTextEdit.Text = weight.ToString();
						flag = true;
					}
				}
				if (string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text))
				{
					EditRow(saleRow, SaleWindowHelper.CurrentFocusedField.Quantity);
					return;
				}
			}
			_posSearchPanelControl.CodeSearchTextEdit.DoValidate();
			if (saleRow.IsGift || (flag && saleRow.UnitCode != WeightItemInfo.GetInstance().Unit))
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.InputBoxIsEmpty);
			}
			if (!_posSearchPanelControl.CodeSearchTextEdit.Text.Trim().IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
			}
			if (!decimal.TryParse(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim(), out var result))
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
			_posSearchPanelControl.CodeSearchTextEdit.Text = "";
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
		int focusedRowHandle = MaterialsGridTableView.FocusedRowHandle;
		SaleRow saleRow = (SaleRow)MaterialsGridControl.GetRow(focusedRowHandle);
		if (saleRow != null)
		{
			RowInfoChangedEvent?.Invoke(saleRow);
		}
	}

	private void RowChanged()
	{
		int focusedRowHandle = MaterialsGridTableView.FocusedRowHandle;
		if (CommonEventHandlers.IsDataRow(focusedRowHandle, MaterialsGridControl))
		{
			StatusBar.GridRowInfo = $"{MaterialsGridControl.GetRowVisibleIndexByHandle(focusedRowHandle) + 1}/{MaterialsGridControl.VisibleRowCount}";
		}
		else
		{
			StatusBar.GridRowInfo = $"0/{MaterialsGridControl.VisibleRowCount}";
		}
		if (_materialsRowsList.Count > 0)
		{
			_saleTouchModeButtonsPadUserControl.ButtonDefer.Visibility = Visibility.Visible;
			_saleTouchModeButtonsPadUserControl.ButtonContinue.Visibility = Visibility.Hidden;
		}
		else
		{
			_saleTouchModeButtonsPadUserControl.ButtonDefer.Visibility = Visibility.Hidden;
			_saleTouchModeButtonsPadUserControl.ButtonContinue.Visibility = Visibility.Visible;
		}
	}

	private void OnSearchControlEnterClick()
	{
		if (!_docIsSale && _baseDocISN != Guid.Empty)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowAddRowForReturn);
		}
		if (!string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text))
		{
			_posSearchPanelControl.CodeSearchTextEdit.DoValidate();
			ReadCodeFromSearchControlWithEnter(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim());
			_posSearchPanelControl.CodeSearchTextEdit.Text = "";
			_realText = "";
		}
	}

	private void OnBarCodeButtonClick()
	{
		if (!_docIsSale && _baseDocISN != Guid.Empty)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowAddRowForReturn);
		}
		if (!string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text))
		{
			_posSearchPanelControl.CodeSearchTextEdit.DoValidate();
			ReadBarCode(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim());
			_posSearchPanelControl.CodeSearchTextEdit.Text = "";
			_realText = "";
		}
	}

	private void OnCodeButtonClick()
	{
		if (!_docIsSale && _baseDocISN != Guid.Empty)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotAllowAddRowForReturn);
		}
		if (string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text))
		{
			return;
		}
		_posSearchPanelControl.CodeSearchTextEdit.DoValidate();
		_qty_Weight = 1m;
		try
		{
			string text = _posSearchPanelControl.CodeSearchTextEdit.Text.Trim();
			if (text.Length > 20)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrMTCodeLen, 20, text));
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
			if (item == null)
			{
				_posSearchPanelControl.CodeSearchTextEdit.Focus();
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text));
			}
			SaleRow saleRow = AddItemToList(item, text, barCode: false);
			bool flag = item.IsWeight && _modernPOSSaleableGoodInfoUserControl.ButtonQTY.Visibility == Visibility.Visible;
			if ((_useMarking && _immediatlyOpenMarkingWindow) | flag)
			{
				bool flag2 = MarkingHelpers.IsGoodMarkable(saleRow.ItemCode, _useMarking, saleRow.MarkingList.Count != 0, Convert.ToDateTime(StatusBar.TB_Time.Text).ToDateOnly(), out var isRowMarkable);
				saleRow.IsRowMarkable = isRowMarkable;
				if (flag2 | flag)
				{
					EditRow(saleRow, SaleWindowHelper.CurrentFocusedField.Quantity, editMode: false);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			_posSearchPanelControl.CodeSearchTextEdit.Text = "";
			_realText = "";
		}
	}

	private void CleanInformationPanel()
	{
		_modernPOSSaleableGoodInfoUserControl.TB_GoodName.Text = "";
		_modernPOSSaleableGoodInfoUserControl.TB_GoodName.ToolTip = "";
		_modernPOSSaleableGoodInfoUserControl.TB_GoodRowDetail.Text = "";
		_modernPOSSaleableGoodInfoUserControl.TB_GoodCode.Text = "";
		_modernPOSSaleableGoodInfoUserControl.TB_GoodRemInfo.Text = "";
	}

	private void SetRowInformationToPanel(SaleRow saleRow)
	{
		if (string.IsNullOrWhiteSpace(saleRow.ItemName))
		{
			CleanInformationPanel();
			return;
		}
		decimal num = (saleRow.Quantity * saleRow.DiscountPrice).RoundAs(2);
		_modernPOSSaleableGoodInfoUserControl.TB_GoodName.Foreground = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff")) : Brushes.DarkSlateGray);
		_modernPOSSaleableGoodInfoUserControl.TB_GoodName.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
		_modernPOSSaleableGoodInfoUserControl.TB_GoodName.Text = saleRow.ItemName.Trim();
		_modernPOSSaleableGoodInfoUserControl.TB_GoodName.ToolTip = saleRow.ItemName.Trim();
		if (saleRow.Discount == 0m)
		{
			TextEdit tB_GoodRowDetail = _modernPOSSaleableGoodInfoUserControl.TB_GoodRowDetail;
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
			TextEdit tB_GoodRowDetail2 = _modernPOSSaleableGoodInfoUserControl.TB_GoodRowDetail;
			InlineArray6<object> buffer2 = default;
			buffer2[0] = saleRow.Quantity.CurrencyFormat(16, (short)((!(saleRow.Quantity == decimal.Floor(saleRow.Quantity))) ? 3 : 0));
			buffer2[1] = saleRow.UnitBrief.Trim();
			buffer2[2] = saleRow.Price.CurrencyFormat(16, (short)((!(saleRow.Price == decimal.Floor(saleRow.Price))) ? 2 : 0));
			buffer2[3] = text;
			buffer2[4] = value.CurrencyFormat(16, (short)((!(saleRow.DiscountSumma == decimal.Floor(saleRow.DiscountSumma))) ? 2 : 0)) + "(" + saleRow.Discount.CurrencyFormat(16, (short)((!(saleRow.Discount == decimal.Floor(saleRow.Discount))) ? 2 : 0)) + "%)";
			buffer2[5] = num.CurrencyFormat(16, (short)((!(num == decimal.Floor(num))) ? 2 : 0));
			tB_GoodRowDetail2.Text = string.Format("{0} {1} x {2} {3} {4} = {5}", (ReadOnlySpan<object?>)buffer2);
		}
		if (TouchMode == POSScreenModesSE.SampleScreenWithFavorites.Value)
		{
			_modernPOSSaleableGoodInfoUserControl.TB_GoodCode.Text = ArmSoft.SME.Common.Resources.Code + ": " + saleRow.ItemCode;
		}
		if (Settings.ParametersService.ShowStorageAvailabilityOnInfoPanel() || Settings.ParametersService.ShowWholeAvailabilityOnInfoPanel())
		{
			SetAvailabilityInformationToPanel(saleRow);
		}
		else
		{
			_modernPOSSaleableGoodInfoUserControl.TB_GoodRemInfo.Height = 0.0;
		}
	}

	private void SetAvailabilityInformationToPanel(SaleRow saleRow)
	{
		if (_materialsRowsList != null && _materialsRowsList.Count > 0)
		{
			_modernPOSSaleableGoodInfoUserControl.GoodRemInfoGrid.Background = (Settings.IsDarkTheme ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3D443F")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDEF0E0")));
		}
		else
		{
			_modernPOSSaleableGoodInfoUserControl.GoodRemInfoGrid.Background = new SolidColorBrush(Colors.Transparent);
		}
		_modernPOSSaleableGoodInfoUserControl.TB_GoodRemInfo.Text = "";
		if (Settings.IsOffline || !(saleRow.ItemType == GoodsTypesSE.Good.Value))
		{
			return;
		}
		if (Settings.ParametersService.ShowStorageAvailabilityOnInfoPanel())
		{
			_modernPOSSaleableGoodInfoUserControl.TB_GoodRemInfo.Text = $"{ArmSoft.SME.Trade.Common.Resources.GoodsRem}: {MaterialHelpers.GetGoodRemains(update: true, saleRow.ItemID, saleRow.UnitCode, ref _availabilityByGoodIdsDictionary, saleRow.Storage)} {saleRow.UnitBrief.Trim()} ";
		}
		if (Settings.ParametersService.ShowWholeAvailabilityOnInfoPanel())
		{
			if (!string.IsNullOrWhiteSpace(_modernPOSSaleableGoodInfoUserControl.TB_GoodRemInfo.Text))
			{
				_modernPOSSaleableGoodInfoUserControl.TB_GoodRemInfo.Text += ", ";
			}
			_modernPOSSaleableGoodInfoUserControl.TB_GoodRemInfo.Text += $"{ArmSoft.SME.Trade.Common.Resources.Whole}: {MaterialHelpers.GetGoodRemains(update: true, saleRow.ItemID, saleRow.UnitCode, ref _availabilityByGoodIdsDictionary)} {saleRow.UnitBrief.Trim()}";
		}
	}

	private void ReadCodeFromSearchControlWithEnter(string code)
	{
		try
		{
			_qty_Weight = 1m;
			if (string.IsNullOrWhiteSpace(code))
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(StatusBar.TB_Time.Text))
			{
				_posSearchPanelControl.CodeSearchTextEdit.Focus();
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			bool flag = false;
			if ((!Settings.ParametersService.UsePartnerEmployeeSearchNewMode()) ? MCRInfo.GetInstance().IsValidCard(code) : ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.IsValidPartnerEmployeeCardCode(code, Settings.ParametersService.PartnerEmployeeCardCodePrefixes(), Settings.ParametersService.UsePartnerCardLenght(), Settings.ParametersService.PartnerCardLenght()))
			{
				bool flag2 = true;
				string text = code;
				if (!Settings.ParametersService.UsePartnerEmployeeSearchNewMode())
				{
					text = MCRInfo.GetInstance().GetCardNumber(code);
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
				else if (!string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim()))
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
			string codeSearchButtonMode = _posSearchPanelControl.CodeSearchButtonMode;
			string unitBrief;
			string unitCode;
			bool isRowMarkable2;
			if (codeSearchButtonMode == POSCodeSearchModesSE.ViaBarCode.Value)
			{
				if (code.Length < 29)
				{
					MaterialCacheRow materialCacheRow = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.TryGetItemByBarCode(ref code, out _qty_Weight, out unitBrief, out unitCode);
					if (materialCacheRow == null)
					{
						_posSearchPanelControl.CodeSearchTextEdit.Text = "";
						_realText = "";
						_posSearchPanelControl.CodeSearchTextEdit.Focus();
						throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeNotExist, code));
					}
					if (_useMarking && !materialCacheRow.IsWeight && materialCacheRow.CheckMarking)
					{
						throw new ASException(ArmSoft.SME.Trade.Common.Resources.ScanDataMatrix);
					}
					SaleRow saleRow = AddItemToList(materialCacheRow, code);
					bool flag3 = materialCacheRow.IsWeight && _modernPOSSaleableGoodInfoUserControl.ButtonQTY.Visibility == Visibility.Visible;
					if (((_useMarking && _immediatlyOpenMarkingWindow) | flag3) && (MarkingHelpers.IsGoodMarkable(saleRow.ItemCode, _useMarking, saleRow.MarkingList.Count != 0, StatusBar.TB_Time.Text.ToDateOnly(), out var isRowMarkable) | flag3))
					{
						saleRow.IsRowMarkable = isRowMarkable;
						EditRow(saleRow, SaleWindowHelper.CurrentFocusedField.Quantity, editMode: false);
					}
					return;
				}
				if (!_useMarking)
				{
					_posSearchPanelControl.CodeSearchTextEdit.Text = "";
					_realText = "";
					_posSearchPanelControl.CodeSearchTextEdit.Focus();
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
					_posSearchPanelControl.CodeSearchTextEdit.Text = "";
					_realText = "";
					_posSearchPanelControl.CodeSearchTextEdit.Focus();
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodesNotExist, eanBarCode));
				}
				if (!MarkingHelpers.IsGoodMarkable(materialFromScannedMark.MTCode, _useMarking, markingListHasValue: false, StatusBar.TB_Time.Text.ToDateOnly(), out isRowMarkable2))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.CPAEmptyOrNotMarkable);
				}
				if (!string.IsNullOrWhiteSpace(eanBarCode) && _useMarking)
				{
					string text2 = _posSearchPanelControl.CodeSearchTextEdit.Text;
					SaleRow saleRow2 = AddItemToList(materialFromScannedMark, eanBarCode);
					if (MarkingHelpers.IsGoodMarkable(saleRow2.ItemCode, _useMarking, saleRow2.MarkingList.Count != 0, StatusBar.TB_Time.Text.ToDateOnly(), out var isRowMarkable3))
					{
						saleRow2.IsRowMarkable = isRowMarkable3;
						saleRow2.MarkingList.Add(text2);
					}
				}
				return;
			}
			if (codeSearchButtonMode == POSCodeSearchModesSE.ViaProductCode.Value)
			{
				OnCodeButtonClick();
				return;
			}
			if (code.Length < 29)
			{
				MaterialCacheRow materialCacheRow2 = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.TryGetItemByBarCode(ref code, out _qty_Weight, out unitCode, out unitBrief);
				if (materialCacheRow2 == null)
				{
					OnCodeButtonClick();
					return;
				}
				if (_useMarking && !materialCacheRow2.IsWeight && materialCacheRow2.CheckMarking)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ScanDataMatrix);
				}
				SaleRow saleRow3 = AddItemToList(materialCacheRow2, code);
				bool flag4 = materialCacheRow2.IsWeight && _modernPOSSaleableGoodInfoUserControl.ButtonQTY.Visibility == Visibility.Visible;
				if (((_useMarking && _immediatlyOpenMarkingWindow) | flag4) && (MarkingHelpers.IsGoodMarkable(saleRow3.ItemCode, _useMarking, saleRow3.MarkingList.Count != 0, StatusBar.TB_Time.Text.ToDateOnly(), out var isRowMarkable4) | flag4))
				{
					saleRow3.IsRowMarkable = isRowMarkable4;
					EditRow(saleRow3, SaleWindowHelper.CurrentFocusedField.Quantity, editMode: false);
				}
				return;
			}
			if (!_useMarking)
			{
				_posSearchPanelControl.CodeSearchTextEdit.Text = "";
				_realText = "";
				_posSearchPanelControl.CodeSearchTextEdit.Focus();
				throw new ASException(ArmSoft.SME.Common.Resources.MarkingNotActive);
			}
			string eanBarCode2 = "";
			MaterialCacheRow materialFromScannedMark2 = GetMaterialFromScannedMark(ref eanBarCode2, out var skipMarking2);
			if (skipMarking2)
			{
				return;
			}
			if (materialFromScannedMark2 == null)
			{
				OnCodeButtonClick();
				return;
			}
			if (!MarkingHelpers.IsGoodMarkable(materialFromScannedMark2.MTCode, _useMarking, markingListHasValue: false, StatusBar.TB_Time.Text.ToDateOnly(), out isRowMarkable2))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.CPAEmptyOrNotMarkable);
			}
			if (!string.IsNullOrWhiteSpace(eanBarCode2) && _useMarking)
			{
				string text3 = _posSearchPanelControl.CodeSearchTextEdit.Text;
				SaleRow saleRow4 = AddItemToList(materialFromScannedMark2, eanBarCode2);
				if (MarkingHelpers.IsGoodMarkable(saleRow4.ItemCode, _useMarking, saleRow4.MarkingList.Count != 0, StatusBar.TB_Time.Text.ToDateOnly(), out var isRowMarkable5))
				{
					saleRow4.IsRowMarkable = isRowMarkable5;
					saleRow4.MarkingList.Add(text3);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
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
				_posSearchPanelControl.CodeSearchTextEdit.Focus();
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
				else if (!string.IsNullOrWhiteSpace(_posSearchPanelControl.CodeSearchTextEdit.Text.Trim()))
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
					_posSearchPanelControl.CodeSearchTextEdit.Text = "";
					_realText = "";
					_posSearchPanelControl.CodeSearchTextEdit.Focus();
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeNotExist, barCode));
				}
				if (_useMarking && !materialCacheRow.IsWeight && materialCacheRow.CheckMarking)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ScanDataMatrix);
				}
				SaleRow saleRow = AddItemToList(materialCacheRow, barCode);
				bool flag3 = materialCacheRow.IsWeight && _modernPOSSaleableGoodInfoUserControl.ButtonQTY.Visibility == Visibility.Visible;
				if (((_useMarking && _immediatlyOpenMarkingWindow) | flag3) && (MarkingHelpers.IsGoodMarkable(saleRow.ItemCode, _useMarking, saleRow.MarkingList.Count != 0, StatusBar.TB_Time.Text.ToDateOnly(), out var isRowMarkable) | flag3))
				{
					saleRow.IsRowMarkable = isRowMarkable;
					EditRow(saleRow, SaleWindowHelper.CurrentFocusedField.Quantity, editMode: false);
				}
				return;
			}
			if (!_useMarking)
			{
				_posSearchPanelControl.CodeSearchTextEdit.Text = "";
				_realText = "";
				_posSearchPanelControl.CodeSearchTextEdit.Focus();
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
				_posSearchPanelControl.CodeSearchTextEdit.Text = "";
				_realText = "";
				_posSearchPanelControl.CodeSearchTextEdit.Focus();
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodesNotExist, eanBarCode));
			}
			if (!MarkingHelpers.IsGoodMarkable(materialFromScannedMark.MTCode, _useMarking, markingListHasValue: false, StatusBar.TB_Time.Text.ToDateOnly(), out var _))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.CPAEmptyOrNotMarkable);
			}
			if (!string.IsNullOrWhiteSpace(eanBarCode) && _useMarking)
			{
				string text2 = _posSearchPanelControl.CodeSearchTextEdit.Text;
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
			byte[] bytes = Encoding.Unicode.GetBytes(_posSearchPanelControl.CodeSearchTextEdit.Text);
			for (int i = 0; i <= bytes.Length - 1; i += 2)
			{
				if (((bytes[i + 1] << 8) | bytes[i]) > 127)
				{
					_posSearchPanelControl.CodeSearchTextEdit.Text = "";
					_realText = "";
					_posSearchPanelControl.CodeSearchTextEdit.Focus();
					throw new ASException(ArmSoft.SME.Common.Resources.KeyboardLanguageForMarkingScanShouldBeEnglish);
				}
			}
			SaleRow saleRow = _materialsRowsList.FirstOrDefault((SaleRow r) => r.MarkingList.Contains(_posSearchPanelControl.CodeSearchTextEdit.Text));
			if (saleRow != null)
			{
				ScannerMessageBoxWindow scannerMessageBoxWindow = new ScannerMessageBoxWindow(ArmSoft.SME.Trade.Common.Resources.ConfirmDeleteDataMatrix, this);
				if (scannerMessageBoxWindow.ShowDialog().Value)
				{
					saleRow.MarkingList.Remove(_posSearchPanelControl.CodeSearchTextEdit.Text);
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
				_posSearchPanelControl.CodeSearchTextEdit.Text = "";
				_realText = "";
				_posSearchPanelControl.CodeSearchTextEdit.Focus();
				if (!string.IsNullOrWhiteSpace(scannerMessageBoxWindow.ScannerData))
				{
					_posSearchPanelControl.CodeSearchTextEdit.Text = scannerMessageBoxWindow.ScannerData;
					ReadBarCode(scannerMessageBoxWindow.ScannerData);
				}
				skipMarking = true;
				return null;
			}
			MarkingHelper.ValidateMarkingCode(_posSearchPanelControl.CodeSearchTextEdit.Text);
			eanBarCode = MarkingHelpers.ExtractGTINFromDataMatrix(_posSearchPanelControl.CodeSearchTextEdit.Text, out var good);
			return good;
		}
		return null;
	}

	private void OnOtherButtonsButtonClick()
	{
		SamplePOSSaleOthersButtonsPadWindow samplePOSSaleOthersButtonsPadWindow = new SamplePOSSaleOthersButtonsPadWindow(this);
		samplePOSSaleOthersButtonsPadWindow.ButtonClick += SaleOtherButtonsPadWindowButton_Click;
		samplePOSSaleOthersButtonsPadWindow.TaxCodeButtonClick += SaleButtonsUserControl_ButtonClick;
		samplePOSSaleOthersButtonsPadWindow.FindBuyerButtonClick += SaleButtonsUserControl_ButtonClick;
		samplePOSSaleOthersButtonsPadWindow.LastCheckECRButtonClick += SaleButtonsUserControl_ButtonClick;
		samplePOSSaleOthersButtonsPadWindow.ShowDialog();
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
		int focusedRowHandle = MaterialsGridTableView.FocusedRowHandle;
		SaleRow saleRow = (SaleRow)MaterialsGridControl.GetRow(focusedRowHandle);
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
		SamplePOSSaleHeader.TB_SaleNum.Text = nextDocumentNumberAndRefreshOfflineIfNeeded.Key;
		_docNumOfferSource = nextDocumentNumberAndRefreshOfflineIfNeeded.Value;
		Title = SamplePOSSaleHeader.TB_SaleNumCaption.Text + SamplePOSSaleHeader.TB_SaleNum.Text;
	}

	private void CodeTextEdit_FocusAndMoveCursorToEnd()
	{
		_posSearchPanelControl.CodeSearchTextEdit.Focus();
		_posSearchPanelControl.CodeSearchTextEdit.SelectionStart = _posSearchPanelControl.CodeSearchTextEdit.Text.Length;
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
			_posSearchPanelControl.CodeSearchTextEdit.Clear();
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
			SamplePOSNewBuyerWindow samplePOSNewBuyerWindow = new SamplePOSNewBuyerWindow(this, cardNumber);
			if (!samplePOSNewBuyerWindow.ShowDialog().Value || string.IsNullOrWhiteSpace(samplePOSNewBuyerWindow.PartnerCode))
			{
				return;
			}
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(samplePOSNewBuyerWindow.PartnerCode);
			if (item.Contracts.Count > 0)
			{
				if (!string.IsNullOrWhiteSpace(_partnerCode))
				{
					ClearPartnerCard();
				}
				PartnerCacheRow.ContractRow contractRow = item.Contracts.First((PartnerCacheRow.ContractRow row) => row.IsDefault);
				FillPartnerCardInfo(contractRow.DiscountCard, item, contractRow);
			}
		}
		catch (ASException ex)
		{
			Settings.ErrorHandler(ex);
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
