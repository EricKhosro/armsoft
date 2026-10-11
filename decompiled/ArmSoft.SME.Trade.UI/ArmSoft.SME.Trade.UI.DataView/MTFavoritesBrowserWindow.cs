using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.DataView;

public class MTFavoritesBrowserWindow : ModalBrowserWindow
{
	private CardView _cardView;

	private readonly bool _touchMode;

	private bool rowHandleChangedFirstTime = true;

	private bool _workingWithRows;

	private CheckEditExt _closeAfterSelect;

	private Action<string, string> _funcAddFromFavorites;

	private readonly string _priceType;

	private readonly DateOnly _date;

	public MTFavoritesBrowserWindow(MTFavoritesBrowser mb, bool multiSelect, Window owner, Action<string, string> funcAddFromFavorites, string priceType, DateOnly date, bool modeSearchPanel = false)
		: base(mb, multiSelect, owner, modeSearchPanel)
	{
		_funcAddFromFavorites = funcAddFromFavorites;
		_priceType = priceType;
		_date = date;
		_touchMode = mb.TouchMode;
		_cardView = new CardView
		{
			CardLayout = CardLayout.Rows,
			CardRowTemplateSelector = (DataTemplateSelector)Resources["mySelector"],
			PrintCellStyle = (Style)Resources["MyPrintEditStyle"],
			AllowCardResizing = true,
			ShowGroupPanel = false,
			ShowColumnHeaders = false,
			AllowColumnFiltering = false,
			AllowColumnMoving = false,
			AllowSorting = false,
			AllowGrouping = false,
			ShowCardExpandButton = !_touchMode,
			ScrollingMode = ScrollingMode.Normal,
			NavigationStyle = GridViewNavigationStyle.Row,
			AllowMoveColumnToDropArea = false,
			PrintAutoCardWidth = false,
			MaxCardCountInRow = 3,
			PrintCardMargin = new Thickness(5.0)
		};
		DataTemplate dataTemplate = (DataTemplate)Application.Current.FindResource("MTFavoriteCardTemplate");
		if (dataTemplate != null)
		{
			_cardView.CardTemplate = dataTemplate;
		}
		DataTemplate dataTemplate2 = (DataTemplate)Application.Current.FindResource("GoodsCardPrintTemplate");
		if (dataTemplate2 != null)
		{
			_cardView.PrintCardTemplate = dataTemplate2;
		}
		DataTemplate dataTemplate3 = (DataTemplate)Application.Current.FindResource("GoodsCardHeaderTemplate");
		if (dataTemplate3 != null)
		{
			_cardView.CardHeaderTemplate = dataTemplate3;
		}
		_cardView.MouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(View_MouseDoubleClick).Handler;
		_cardView.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(CardView_PreviewKeyDown).Handler;
		_cardView.KeyDown += new SafeEventHandler<KeyEventArgs>(CardView_KeyDown).Handler;
		_cardView.FocusedRowHandleChanged += new SafeEventHandler<FocusedRowHandleChangedEventArgs>(CardView_FocusedRowHandleChanged).Handler;
		_closeAfterSelect = new CheckEditExt
		{
			Name = "CloseAfterSelect",
			Margin = new Thickness(5.0, 5.0, 15.0, 5.0),
			HorizontalAlignment = HorizontalAlignment.Right,
			VerticalAlignment = VerticalAlignment.Center,
			UseLayoutRounding = true,
			Content = ArmSoft.SME.Trade.Common.Resources.CloseAfterSelect,
			IsChecked = (RegistryHelper.ReadStringFromRegistry("CloseAfterSelect", "MB\\" + mb.DataView.Name, "0") == "1")
		};
		_closeAfterSelect.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CloseAfterSelect_EditValueChanged).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(ModalBrowserWindow_Closing).Handler;
		Grid.SetRow(_closeAfterSelect, 0);
		Grid.SetColumn(_closeAfterSelect, 3);
		_buttonsPanel.Children.Add(_closeAfterSelect);
		if (mb.IsCardView)
		{
			_grid.View = _cardView;
			Width = 635.0;
			Height = 520.0;
		}
	}

	private void CloseAfterSelect_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\MB\\" + _mb.DataView.Name);
		registryKey.SetValue("CloseAfterSelect", _closeAfterSelect.IsChecked.Value ? "1" : "0");
	}

	private void CardView_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		View_PreviewKeyDown(sender, e);
		if (e.Handled)
		{
			return;
		}
		string text = string.Empty;
		ModifierKeys modifiers = Keyboard.Modifiers;
		if ((int)modifiers != 2)
		{
			if ((int)modifiers == 4)
			{
				text = "Shift+";
			}
		}
		else
		{
			text = "Ctrl+";
		}
		string text2 = ((object)e.Key/*cast due to constrained. prefix*/).ToString();
		text = ((!Regex.IsMatch(text2, "^D[0-9]$")) ? (text + text2) : (text + text2.Substring(1)));
		if (!Regex.IsMatch(text, "^(?!(Ctrl\\+0|Shift\\+((NumPad)[0-9])))(Ctrl\\+|Shift\\+){0,1}((NumPad){0,1}[0-9]|F([2356789]|11|12)|[A-Z])$"))
		{
			return;
		}
		for (int i = 0; i <= _grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = _grid.GetRowHandleByVisibleIndex(i);
			int listIndexByRowHandle = _grid.GetListIndexByRowHandle(rowHandleByVisibleIndex);
			if (text == _mb.DataView.CommentMember(listIndexByRowHandle))
			{
				_grid.View.FocusedRowHandle = rowHandleByVisibleIndex;
				if (!_touchMode)
				{
					ProcessEnter();
				}
				e.Handled = true;
				break;
			}
		}
	}

	private void CardView_KeyDown(object sender, KeyEventArgs e)
	{
		View_KeyDown(sender, e);
	}

	private void CardView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		if (_grid.View.FocusedRowHandle == int.MinValue)
		{
			return;
		}
		if (_workingWithRows)
		{
			_grid.View.FocusedRowHandle = int.MinValue;
			return;
		}
		if (_touchMode)
		{
			if (rowHandleChangedFirstTime)
			{
				_cardView.DataControl.CurrentItem = null;
				_cardView.DataControl.SelectedItem = null;
				rowHandleChangedFirstTime = false;
				_txtStatus.Visibility = Visibility.Hidden;
			}
			else
			{
				ProcessEnter();
				_grid.View.FocusedRowHandle = int.MinValue;
			}
		}
		View_FocusedRowHandleChanged(sender, e);
	}

	protected new void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		if (!_touchMode)
		{
			ProcessEnter();
		}
	}

	protected void ProcessEnter()
	{
		int focusedRowHandle = _grid.View.FocusedRowHandle;
		if (focusedRowHandle != int.MinValue && focusedRowHandle != -2147483645)
		{
			if (Settings.IsOffline)
			{
				ProccesEnterInOfflineMode(focusedRowHandle);
			}
			else
			{
				ProccesEnterInOnlineMode(focusedRowHandle);
			}
		}
	}

	private void ProccesEnterInOnlineMode(int rowHnd)
	{
		IList<MTFavoritesDataProvider.DataRow> list = (IList<MTFavoritesDataProvider.DataRow>)_grid.ItemsSource;
		if (list.Count == 0)
		{
			return;
		}
		MTFavoritesDataProvider.DataRow dataRow = list[rowHnd];
		switch (dataRow.FavType)
		{
		case 2:
			_workingWithRows = _touchMode;
			list.Clear();
			list.Add(new MTFavoritesDataProvider.DataRow
			{
				MTID = -1,
				ImgStr = $"{-1},",
				MTCode = ArmSoft.SME.Common.Resources.ExitWord,
				Caption = ArmSoft.SME.Trade.Common.Resources.ReturnMainLevel,
				FavType = (byte)(Settings.ParametersService.InFavoritesFirtsGroups() ? 100 : 0)
			});
			foreach (MTFavoritesDataProvider.DataRow row in new MTFavoritesDataProvider(TradeSettings.CreateTradeClient()).GetRows(new MTFavoritesDataProvider.Param
			{
				UserId = Settings.LoginService.UserId,
				GroupCode = dataRow.MTCode,
				OnlyMaterials = true,
				PriceType = _priceType,
				Date = _date
			}))
			{
				list.Add(row);
			}
			_workingWithRows = false;
			break;
		case 0:
		case 100:
			_workingWithRows = _touchMode;
			list.Clear();
			foreach (MTFavoritesDataProvider.DataRow row2 in new MTFavoritesDataProvider(TradeSettings.CreateTradeClient()).GetRows(new MTFavoritesDataProvider.Param
			{
				OnlyMaterials = false,
				UserId = Settings.LoginService.UserId,
				PriceType = _priceType,
				Date = _date
			}))
			{
				list.Add(row2);
			}
			_workingWithRows = false;
			break;
		case 1:
			_funcAddFromFavorites(dataRow.MTCode, dataRow.UnitCode);
			if (_closeAfterSelect.IsChecked.Value)
			{
				DialogResult = true;
				RaiseClosed();
			}
			break;
		}
	}

	private void ProccesEnterInOfflineMode(int rowHnd)
	{
		IList<MTCachedFavoritesDataProvider.DataRow> list = (IList<MTCachedFavoritesDataProvider.DataRow>)_grid.ItemsSource;
		if (list.Count == 0)
		{
			return;
		}
		MTCachedFavoritesDataProvider.DataRow dataRow = list[rowHnd];
		switch (dataRow.FavType)
		{
		case 2:
			_workingWithRows = _touchMode;
			list.Clear();
			list.Add(new MTCachedFavoritesDataProvider.DataRow
			{
				MTID = -1,
				ImgStr = $"{-1},",
				MTCode = ArmSoft.SME.Common.Resources.ExitWord,
				Caption = ArmSoft.SME.Trade.Common.Resources.ReturnMainLevel,
				FavType = (byte)(Settings.ParametersService.InFavoritesFirtsGroups() ? 100 : 0)
			});
			foreach (MTCachedFavoritesDataProvider.DataRow row in new MTCachedFavoritesDataProvider().GetRows(new MTCachedFavoritesDataProvider.Param
			{
				UserId = Settings.LoginService.UserId,
				GroupCode = dataRow.MTCode,
				OnlyMaterials = true
			}))
			{
				list.Add(row);
			}
			_workingWithRows = false;
			break;
		case 0:
		case 100:
			_workingWithRows = _touchMode;
			list.Clear();
			foreach (MTCachedFavoritesDataProvider.DataRow row2 in new MTCachedFavoritesDataProvider().GetRows(new MTCachedFavoritesDataProvider.Param
			{
				OnlyMaterials = false,
				UserId = Settings.LoginService.UserId
			}))
			{
				list.Add(row2);
			}
			_workingWithRows = false;
			break;
		case 1:
			_funcAddFromFavorites(dataRow.MTCode, dataRow.UnitCode);
			if (_closeAfterSelect.IsChecked.Value)
			{
				DialogResult = true;
				RaiseClosed();
			}
			break;
		}
	}

	public void LoadRegistrySettings(bool onlyPosition = false)
	{
		if (!onlyPosition)
		{
			base.LoadRegistrySettings();
		}
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey($"{Settings.RegistryKey}\\{"MB"}\\{_mb.DataView.Name}", writable: false);
		if (registryKey != null)
		{
			if (double.TryParse(registryKey.GetValue("Left", "0").ToString(), out var result))
			{
				Left = result;
			}
			if (double.TryParse(registryKey.GetValue("Top", "0").ToString(), out var result2))
			{
				Top = result2;
			}
		}
	}

	protected new void ModalBrowserWindow_Closing(object sender, CancelEventArgs e)
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey($"{Settings.RegistryKey}\\{"MB"}\\{_mb.DataView.Name}");
		if (registryKey != null)
		{
			registryKey.SetValue("Left", Left);
			registryKey.SetValue("Top", Top);
		}
		base.ModalBrowserWindow_Closing(sender, e);
	}
}
