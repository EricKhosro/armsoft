using System;
using System.Windows;
using ArmSoft.SME.Core.UI.DataViews;

namespace ArmSoft.SME.Trade.UI.DataView;

public class MTFavoritesBrowser : ModalBrowser
{
	public bool IsCardView;

	public readonly bool TouchMode;

	private Action<string, string> _funcAddFromFavorites;

	public readonly bool IsShowInTaskBar;

	private readonly string _priceType;

	private readonly DateOnly _date;

	public MTFavoritesBrowser(IDropDownView idv, Action<string, string> funcAddFromFavorites, string priceType, DateOnly date, bool showTree = false, bool showCardView = false, bool touchMode = false, bool isShowInTaskbar = true)
		: base(idv, showTree)
	{
		_funcAddFromFavorites = funcAddFromFavorites;
		IsShowInTaskBar = isShowInTaskbar;
		IsCardView = showCardView;
		TouchMode = touchMode;
		_priceType = priceType;
		_date = date;
	}

	public override bool Show(bool upClick = false)
	{
		MTFavoritesBrowserWindow mTFavoritesBrowserWindow = new MTFavoritesBrowserWindow(this, MultiSelect, Owner, _funcAddFromFavorites, _priceType, _date, upClick)
		{
			Left = Left,
			Top = Top,
			WindowStartupLocation = WindowStartupLocation,
			LookUpValue = LookUpValue,
			SelectedRowsIndexes = SelectedRows,
			ShowInTaskbar = IsShowInTaskBar
		};
		if (mTFavoritesBrowserWindow.WindowStartupLocation != WindowStartupLocation.Manual)
		{
			mTFavoritesBrowserWindow.LoadRegistrySettings();
		}
		ModalBrowser.MoveWindowIntoWorkArea(mTFavoritesBrowserWindow);
		mTFavoritesBrowserWindow.ShowDialog();
		return true;
	}
}
