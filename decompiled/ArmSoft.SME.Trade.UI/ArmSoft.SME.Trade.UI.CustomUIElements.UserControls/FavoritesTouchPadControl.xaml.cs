using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.Models;
using ArmSoft.SME.Trade.UI.SmartEnums;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public partial class FavoritesTouchPadControl : UserControl, IComponentConnector
{
	private readonly List<FavoriteItem> _favoriteItemsList;

	private readonly List<FavoriteItem> _favoriteMaterialsList;

	private readonly Action<string, string> _funcAddFromFavorites;

	public double CardViewWidth { get; private set; }

	public bool IsSearchPanelFocused
	{
		get
		{
			if (CardView.ActualShowSearchPanel)
			{
				return CardView.SearchControl?.IsKeyboardFocusWithin ?? false;
			}
			return false;
		}
	}

	public FavoritesTouchPadControl(double screenWidth, Action<string, string> funcAddFromFavorites, string priceListType, string vATType, DateTime documentDate)
	{
		InitializeComponent();
		_favoriteItemsList = new List<FavoriteItem>();
		_favoriteMaterialsList = new List<FavoriteItem>();
		if (Settings.IsOffline)
		{
			IReadOnlyList<MTFavoriteGroupCacheRow> list = TradeSettings.TradeCacheService.MTFavoriteGroup.GetList();
			IEnumerable<MTFavoriteCacheRow> enumerable = TradeSettings.TradeCacheService.MTFavorite.GetList();
			if (enumerable != null && enumerable.Count() > 0)
			{
				enumerable = enumerable.OrderBy((MTFavoriteCacheRow mf) => mf.OrderNum);
			}
			if (Settings.ParametersService.InFavoritesFirtsGroups())
			{
				foreach (MTFavoriteGroupCacheRow item in list)
				{
					_favoriteItemsList.Add(new FavoriteItem(item));
				}
				foreach (MTFavoriteCacheRow item2 in enumerable)
				{
					_favoriteMaterialsList.Add(new FavoriteItem(item2, priceListType, vATType));
					if (string.IsNullOrWhiteSpace(item2.Group))
					{
						_favoriteItemsList.Add(new FavoriteItem(item2, priceListType, vATType));
					}
				}
			}
			else
			{
				foreach (MTFavoriteCacheRow item3 in enumerable)
				{
					_favoriteMaterialsList.Add(new FavoriteItem(item3, priceListType, vATType));
					if (string.IsNullOrWhiteSpace(item3.Group))
					{
						_favoriteItemsList.Add(new FavoriteItem(item3, priceListType, vATType));
					}
				}
				foreach (MTFavoriteGroupCacheRow item4 in list)
				{
					_favoriteItemsList.Add(new FavoriteItem(item4));
				}
			}
		}
		else
		{
			MTFavoritesDataProvider.Param param = new MTFavoritesDataProvider.Param
			{
				UserId = Settings.LoginService.UserId,
				Date = documentDate.ToDateOnly(),
				VatType = vATType,
				OnlyMaterials = true,
				OnlyGroups = false,
				PriceType = priceListType
			};
			MTFavoritesDataProvider.Param param2 = new MTFavoritesDataProvider.Param
			{
				UserId = Settings.LoginService.UserId,
				Date = documentDate.ToDateOnly(),
				VatType = vATType,
				OnlyGroups = false,
				OnlyMaterials = false,
				PriceType = priceListType
			};
			foreach (MTFavoritesDataProvider.DataRow row in new MTFavoritesDataProvider(TradeSettings.CreateTradeClient()).GetRows(param2))
			{
				_favoriteItemsList.Add(new FavoriteItem(row));
			}
			foreach (MTFavoritesDataProvider.DataRow row2 in new MTFavoritesDataProvider(TradeSettings.CreateTradeClient()).GetRows(param))
			{
				_favoriteMaterialsList.Add(new FavoriteItem(row2));
			}
		}
		FavoritsTouchGridControl.ItemsSource = _favoriteItemsList;
		_funcAddFromFavorites = funcAddFromFavorites;
		SetDefaultValuesAndStates(screenWidth);
		ASFontHelper.SetFontSettings(this);
	}

	private void SetDefaultValuesAndStates(double screenWidth)
	{
		double num = RegistryHelper.ReadDoubleFromRegistry("CardSize", "Document Windows\\POS");
		if (num <= 0.0)
		{
			if (screenWidth >= 1024.0 && screenWidth < 1280.0)
			{
				num = 100.0;
			}
			else
			{
				num = ((!(screenWidth >= 1280.0) || !(screenWidth < 1440.0)) ? 150.0 : 140.0);
			}
		}
		CardView.CardLayout = CardLayout.Rows;
		string touchMode = CashierDisplayInfo.GetInstance().TouchMode;
		if (touchMode == POSScreenModesSE.TouchScreenWithFavorites.Value)
		{
			if (screenWidth >= 1024.0 && screenWidth < 1280.0)
			{
				CardViewWidth = 102.0;
				CardView.MaxCardCountInRow = 3;
			}
			else if (screenWidth >= 1280.0 && screenWidth < 1600.0)
			{
				CardViewWidth = 137.0;
				CardView.MaxCardCountInRow = 4;
			}
			else
			{
				CardViewWidth = 148.0;
				CardView.MaxCardCountInRow = 5;
			}
		}
		else if (touchMode == POSScreenModesSE.SampleScreenWithFavorites.Value)
		{
			if (screenWidth >= 1024.0 && screenWidth < 1280.0)
			{
				CardViewWidth = 106.0;
				CardView.MaxCardCountInRow = 3;
			}
			else if (screenWidth >= 1280.0 && screenWidth < 1600.0)
			{
				CardViewWidth = 140.0;
				CardView.MaxCardCountInRow = 4;
			}
			else
			{
				CardViewWidth = 150.0;
				CardView.MaxCardCountInRow = 5;
			}
		}
		CardView.FixedSize = num;
		CardView.CardHeaderTemplate = (DataTemplate)Application.Current.FindResource("GoodsCardHeaderTemplate");
		if (Settings.ParametersService.AllowCardViewSizeChange())
		{
			CardView.CardMargin = new Thickness(1.0, 3.0, 1.0, 3.0);
			CardView.AllowCardResizing = true;
			CardView.SeparatorThickness = 1.0;
		}
		else
		{
			CardView.CardMargin = new Thickness(1.0, 1.0, 1.0, 1.0);
			CardView.AllowCardResizing = false;
			CardView.SeparatorThickness = 0.0;
		}
	}

	private void BackButtonEdit_Click(object sender, RoutedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(GroupName.Text))
		{
			FavoritsTouchGridControl.ItemsSource = _favoriteItemsList;
			GroupName.Text = "";
		}
	}

	private void DownButtonEdit_Click(object sender, RoutedEventArgs e)
	{
		FavoritsTouchGridControl.View.MoveNextPage();
	}

	private void UpButtonEdit_Click(object sender, RoutedEventArgs e)
	{
		FavoritsTouchGridControl.View.MovePrevPage();
	}

	private void FavoritsTouchGridControlPreview_MouseDown(object sender, MouseButtonEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected Obj, but got Unknown
		try
		{
			CardViewHitInfo hitInfo = CardView.CalcHitInfo((DependencyObject)e.OriginalSource);
			if (hitInfo.InRow)
			{
				e.Handled = false;
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
				{
					ProcessEnter(hitInfo.RowHandle);
				}), (DispatcherPriority)2, Array.Empty<object>());
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ProcessEnter(int rowHnd)
	{
		if (rowHnd == int.MinValue || rowHnd == -2147483645)
		{
			return;
		}
		List<FavoriteItem> list = (List<FavoriteItem>)FavoritsTouchGridControl.ItemsSource;
		if (list.Count == 0)
		{
			return;
		}
		if (rowHnd < 0 || rowHnd >= list.Count)
		{
			return;
		}
		FavoriteItem mTFavoriteProduct = list[rowHnd];
		switch (mTFavoriteProduct.FavType)
		{
		case 2:
			FavoritsTouchGridControl.ItemsSource = _favoriteMaterialsList.Where((FavoriteItem x) => x.GroupCode == mTFavoriteProduct.MTCode).ToList();
			GroupName.Text = mTFavoriteProduct.Caption;
			break;
		case 1:
			_funcAddFromFavorites(mTFavoriteProduct.MTCode, mTFavoriteProduct.UnitCode);
			break;
		}
	}
}
