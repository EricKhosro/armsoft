using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.Caches;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Converters;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Navigation;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.StandartWindow;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Ribbon;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI;

public class MainRibbonSupportedPanel : RibbonSupportedPanelBase
{
	public BarButtonItem _bChangePassword;

	public BarButtonItem _btnCalc;

	private BarButtonItem _btnSpreadSheet;

	private BarButtonItem _btnChangePin;

	private BarButtonItem _btnSpreadSheetOpen;

	private BarButtonItem _barButtonRefreshUICache;

	private BarButtonItem _barButtonTeamViewer;

	private BarButtonItem _barButtonAnyDesk;

	private BarButtonItem _barButtonTelegram;

	private BarButtonItem _barButtonWebSite;

	private BarButtonItem _barButtonCheckOnlineServices;

	private BarButtonItem _barButtonYouTube;

	private RibbonPage _ribbonPageHome;

	private RibbonPageGroup _ribbonPageGroupRecords;

	private RibbonPageGroup _ribbonPageGroupDataView;

	private RibbonPageGroup _ribbonPageGroupPrint;

	private RibbonPageGroup _ribbonPageGroupDocument;

	private RibbonPageGroup _ribbonPageGroupDirectory;

	private RibbonPageGroup _ribbonPageDates;

	private RibbonPageGroup _ribbonPageWindows;

	private RibbonPage _ribbonPageHelp;

	private RibbonPageGroup _ribbonPageGroupArmsoftOnline;

	private RibbonPageGroup _ribbonPageGroupRemoteSupport;

	private RibbonPageGroup _ribbonPageGroupHelp;

	private BarButtonItem _barButtonOpenImport;

	public override MergingType BarItemMergingType => MergingType.WithMergingProperties;

	public ApplicationMenu AppMenu { get; private set; }

	public BarButtonItem LoginBarButtonItem { get; private set; }

	public BarButtonItem LoginOffLineBarButtonItem { get; private set; }

	public BarButtonItem LogoutBarButtonItem { get; private set; }

	public BarButtonItem OptionsWindowBarButtonItem { get; private set; }

	public BarButtonItem ExitBarButtonItem { get; private set; }

	public BarButtonItem ExplorerPanelBarItem { get; private set; }

	public BarButtonItem RecentPanelBarItem { get; private set; }

	public BarButtonItem HomePanelItemBarButtonItem { get; private set; }

	public BarButtonItem DatesBarButtonItem { get; private set; }

	public BarButtonItem CloseWindowsBarButtonItem { get; private set; }

	public BarButtonItem ExplorerPanelBarButtonItem { get; private set; }

	public BarButtonItem HomePanelBarButtonItem { get; private set; }

	public BarButtonItem AboutBarButtonItem { get; private set; }

	public BarButtonItem ChangeOnlineStatusBarButtonItem { get; private set; }

	public BarButtonItem LiveChatBarButtonItem { get; private set; }

	public BarButtonItem WebDownloadBarButtonItem { get; private set; }

	public BarButtonItem BarButtonCheckForUpdate { get; private set; }

	public BarButtonItem UpdateAvailableBarButtonItem { get; private set; }

	public BarStaticItem MobileBarButtonItem { get; private set; }

	public BarButtonItem AddToFavoritesBarButtonItem { get; private set; }

	public BarButtonItem RemoveFromFavoritesBarButtonItem { get; private set; }

	public BarCheckItem BarButtonRecordUserCalls { get; private set; }

	public BarItemLink ExplorerPanelBarItemLink { get; private set; }

	public BarItemLink RecentPanelBarItemLink { get; private set; }

	public BarItemLink HomePanelItemLink { get; private set; }

	public RibbonPage RibbonPageFavorites { get; private set; }

	public BarButtonItem RegExcelBarButtonItem { get; private set; }

	public BarButtonItem BarButtonKB { get; private set; }

	public BarItemLink LoginOfflineBarItemLink { get; private set; }

	public BarItemLink ChangePinBarItemLink { get; private set; }

	public BarItemLink ChangeOnlineStatusBarItemLink { get; private set; }

	public MainRibbonSupportedPanel(bool allowHomePanel)
		: base(allowHomePanel)
	{
	}

	protected override void BuildBarManagerItems()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_071d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_078b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Unknown result type (might be due to invalid IL or missing references)
		//IL_084d: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0863: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0890: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_096b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0971: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6b: Unknown result type (might be due to invalid IL or missing references)
		base.BuildBarManagerItems();
		LoginBarButtonItem = AddBarButtonItem("bLogin", ArmSoft.SME.Common.Resources.Login, ImageService.GetImage("power_button_on_16.svg"), ImageService.GetImage("power_button_on_32.svg"));
		LoginOffLineBarButtonItem = AddBarButtonItem("bLoginOffLine", ArmSoft.SME.Common.Resources.LoginOffline, ImageService.GetImage("power_button_on_16.svg"), ImageService.GetImage("power_button_on_32.svg"));
		LogoutBarButtonItem = AddBarButtonItem("bLogout", ArmSoft.SME.Common.Resources.Logout, ImageService.GetImage("power_button_off_16.svg"), ImageService.GetImage("power_button_off_32.svg"));
		_bChangePassword = AddBarButtonItem("bChangePassword", ArmSoft.SME.Common.Resources.UserProperties, ImageService.GetImage("user_16.svg"), ImageService.GetImage("user_32.svg"));
		OptionsWindowBarButtonItem = AddBarButtonItem("bOptionsWindow", ArmSoft.SME.Common.Resources.Options, ImageService.GetImage("options_16.svg"), ImageService.GetImage("options_32.svg"), new KeyGesture((Key)58, (ModifierKeys)2));
		ExitBarButtonItem = AddBarButtonItem("bExit", ArmSoft.SME.Common.Resources.ExitWord, ImageService.GetImage("close_16.svg"), ImageService.GetImage("close_32.svg"), new KeyGesture((Key)93, (ModifierKeys)1));
		ExplorerPanelBarItem = AddBarButtonItem("explorerPanelBarItem", ArmSoft.SME.Common.Resources.Explorer, ImageService.GetImage("pane_expand_16.svg"), ImageService.GetImage("pane_expand_32.svg"), new KeyGesture((Key)100, (ModifierKeys)2));
		RecentPanelBarItem = AddBarButtonItem("recentPanelBarItem", ArmSoft.SME.Common.Resources.History, ImageService.GetImage("recent_16.svg"), ImageService.GetImage("recent_32.svg"), new KeyGesture((Key)99, (ModifierKeys)2));
		HomePanelItemBarButtonItem = AddBarButtonItem("homePanelItem", ArmSoft.SME.Common.Resources.HomePanel, ImageService.GetImage("home_16.svg"), ImageService.GetImage("home_32.svg"), new KeyGesture((Key)100));
		DatesBarButtonItem = AddBarButtonItem("btnDates", ArmSoft.SME.Common.Resources.Dates, null, null);
		DatesBarButtonItem.KeyGesture = HotKeys.WithModifier.Control_F12;
		Binding binding = new Binding("CurrentDate")
		{
			RelativeSource = new RelativeSource
			{
				AncestorType = typeof(Window),
				Mode = RelativeSourceMode.FindAncestor
			},
			Converter = new ConvertDateToImageSource(),
			ConverterParameter = "Small",
			Mode = BindingMode.OneWay
		};
		DatesBarButtonItem.SetBinding(BarItem.GlyphProperty, binding);
		binding = new Binding("CurrentDate")
		{
			RelativeSource = new RelativeSource
			{
				AncestorType = typeof(Window),
				Mode = RelativeSourceMode.FindAncestor
			},
			Converter = new ConvertDateToImageSource(),
			ConverterParameter = "Large",
			Mode = BindingMode.OneWay
		};
		DatesBarButtonItem.SetBinding(BarItem.LargeGlyphProperty, binding);
		_btnCalc = AddBarButtonItem("btnCalc", ArmSoft.SME.Common.Resources.Calculator, ImageService.GetImage("Calculator_16.svg"), ImageService.GetImage("Calculator_32.svg"));
		_btnCalc.KeyGesture = HotKeys.WithModifier.Control_F9;
		CloseWindowsBarButtonItem = AddBarButtonItem("btnCloseWindows", ArmSoft.SME.Common.Resources.RibbonCloseAllWnds, ImageService.GetImage("close_all_windows_16.svg"), ImageService.GetImage("close_all_windows_32.svg"), new KeyGesture((Key)93, (ModifierKeys)4));
		ExplorerPanelBarButtonItem = AddBarButtonItem("btnExplorerPanel", ArmSoft.SME.Common.Resources.Explorer, ImageService.GetImage("pane_expand_16.svg"), ImageService.GetImage("pane_expand_32.svg"), new KeyGesture((Key)100, (ModifierKeys)2));
		HomePanelBarButtonItem = AddBarButtonItem("btnHomePanel", ArmSoft.SME.Common.Resources.HomePanel, ImageService.GetImage("home_16.svg"), ImageService.GetImage("home_32.svg"), new KeyGesture((Key)100));
		_btnSpreadSheet = AddBarButtonItem("btnSpreadSheet", ArmSoft.SME.Common.Resources.NewSpreadSheet, ImageService.GetImage("spreadsheet_16.svg"), ImageService.GetImage("spreadsheet_32.svg"));
		ChangeOnlineStatusBarButtonItem = AddBarButtonItem("btnChangeOnlineStatus", "", ImageService.GetImage("network_cloud_16.svg"), ImageService.GetImage("network_cloud_32.svg"));
		_btnChangePin = AddBarButtonItem("btnChangePin", ArmSoft.SME.Common.Resources.ChangePinCode, ImageService.GetImage("key_16.svg"), ImageService.GetImage("key_32.svg"));
		_btnSpreadSheetOpen = AddBarButtonItem("btnSpreadSheetOpen", ArmSoft.SME.Common.Resources.OpenSpreadSheet, ImageService.GetImage("open_16.svg"), ImageService.GetImage("open_32.svg"));
		_barButtonRefreshUICache = AddBarButtonItem("barButtonRefreshUICache", ArmSoft.SME.Common.Resources.Refresh, ImageService.GetImage("refresh_all_16.svg"), ImageService.GetImage("refresh_all_32.svg"));
		AboutBarButtonItem = AddBarButtonItem("barButtonAbout", "", ImageService.GetImage("information_16.svg"), ImageService.GetImage("information_32.svg"));
		BarButtonKB = AddBarButtonItem("barButtonKB", ArmSoft.SME.Common.Resources.KnowledgeBase, ImageService.GetImage("books_stack_16.svg"), ImageService.GetImage("books_stack_32.svg"));
		_barButtonTeamViewer = AddBarButtonItem("barButtonTeamViewer", "TeamViewer", ImageService.GetImage("TeamViewer_16.svg"), ImageService.GetImage("TeamViewer_32.svg"));
		_barButtonAnyDesk = AddBarButtonItem("barButtonAnyDesk", "AnyDesk", ImageService.GetImage("AnyDesk_16.svg"), ImageService.GetImage("AnyDesk_32.svg"));
		if ((Settings.SystemType != SystemTypes.MarketPlace) & (Settings.Localization != Localizations.Georgia))
		{
			_barButtonTelegram = AddBarButtonItem("barButtonTelegram", "Telegram", ImageService.GetImage("Telegram_16.svg"), ImageService.GetImage("Telegram_32.svg"));
		}
		LiveChatBarButtonItem = AddBarButtonItem("barButtonLiveChat", (Settings.Localization == Localizations.Armenia) ? ArmSoft.SME.Common.Resources.ArmSoftLiveChat : ArmSoft.SME.Common.Resources.ApricotSoftwareLiveChat, ImageService.GetImage("Chat_16.svg"), ImageService.GetImage("Chat_32.svg"));
		WebDownloadBarButtonItem = AddBarButtonItem("barButtonWebDownload", ArmSoft.SME.Common.Resources.TempSmall, ImageService.GetImage("download_16.svg"), ImageService.GetImage("download_32.svg"));
		_barButtonWebSite = AddBarButtonItem("barButtonWebSite", (Settings.Localization == Localizations.Armenia) ? ArmSoft.SME.Common.Resources.ArmSoftHomePage : ArmSoft.SME.Common.Resources.ApricotSoftwareHomePage, ImageService.GetImage("AS_Logo_32.svg"), ImageService.GetImage("AS_Logo_32.svg"));
		BarButtonCheckForUpdate = AddBarButtonItem("barButtonCheckForUpdate", ArmSoft.SME.Common.Resources.CheckForUpdates, ImageService.GetImage("update_16.svg"), ImageService.GetImage("update_32.svg"));
		_barButtonCheckOnlineServices = AddBarButtonItem("barButtonCheckOnlineServices", ArmSoft.SME.Common.Resources.CheckOnlineServices, ImageService.GetImage("acs_check_16.svg"), ImageService.GetImage("acs_check_32.svg"));
		UpdateAvailableBarButtonItem = AddBarButtonItem("barButtonUpdateAvailable", ArmSoft.SME.Common.Resources.UpdateAvailable, ImageService.GetImage("software_update_available_16.svg"), ImageService.GetImage("software_update_available_32.svg"));
		MobileBarButtonItem = AddBarStaticItem("barButtonMobile", ArmSoft.SME.Common.Resources.MobileVersion, ImageService.GetImage("smartphone_16.svg"), ImageService.GetImage("smartphone_16.svg"));
		MobileBarButtonItem.GlyphSize = GlyphSize.Small;
		MobileBarButtonItem.Alignment = BarItemAlignment.Far;
		_barButtonYouTube = AddBarButtonItem("barButtonYouTube", ArmSoft.SME.Common.Resources.YouTube, ImageService.GetImage("Youtube_16.svg"), ImageService.GetImage("Youtube_32.svg"));
		AddToFavoritesBarButtonItem = AddBarButtonItem("btnAddToFavorites", ArmSoft.SME.Common.Resources.AddToFavorites, ImageService.GetImage("favorite_16.svg"), ImageService.GetImage("favorite_32.svg"));
		RemoveFromFavoritesBarButtonItem = AddBarButtonItem("btnRemoveFromFavorites", ArmSoft.SME.Common.Resources.RemoveFromFavorites, ImageService.GetImage("favorite_delete_16.svg"), ImageService.GetImage("favorite_delete_32.svg"));
		if (ConfigHelper.CallsRecordingIsAvailable)
		{
			BarButtonRecordUserCalls = AddBarCheckItem("barButtonRecordUserCalls", string.Format(ArmSoft.SME.Common.Resources.CaptionWithHotKey, "Record calls", HotKeys.WithModifier.Alt_Shift_C.DisplayString), ImageService.GetImage("test_16.svg"), ImageService.GetImage("test_32.svg"), HotKeys.WithModifier.Alt_Shift_C);
		}
	}

	protected override void BuildBarManagerUI()
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce9: Unknown result type (might be due to invalid IL or missing references)
		base.BuildBarManagerUI();
		RibbonControl = new RibbonControl
		{
			RibbonStyle = RibbonStyle.Office2010,
			UseLayoutRounding = true,
			AllowCustomization = false,
			AllowSimplifiedRibbon = true,
			IsTabStop = false,
			AllowDrop = true
		};
		DockPanel.SetDock(RibbonControl, Dock.Top);
		AppMenu = new ApplicationMenu();
		RibbonControl.ApplicationMenu = (DependencyObject)(object)AppMenu;
		RibbonControl.ApplicationButtonText = "";
		RibbonControl.ApplicationButtonSmallIcon = ImageService.GetImage("hamburger_horizontal_16.svg");
		AppMenu.RightPaneWidth = 200.0;
		AppMenu.ItemLinks.Add(LoginBarButtonItem.CreateLink());
		LoginOfflineBarItemLink = LoginOffLineBarButtonItem.CreateLink();
		LoginOfflineBarItemLink.IsVisible = false;
		AppMenu.ItemLinks.Add(LoginOfflineBarItemLink);
		AppMenu.ItemLinks.Add(LogoutBarButtonItem.CreateLink());
		AppMenu.ItemLinks.Add(_bChangePassword.CreateLink());
		ChangePinBarItemLink = _btnChangePin.CreateLink();
		ChangePinBarItemLink.IsVisible = false;
		AppMenu.ItemLinks.Add(ChangePinBarItemLink);
		BarItemLink barItemLink = OptionsWindowBarButtonItem.CreateLink();
		barItemLink.MergeOrder = 0;
		barItemLink.MergeType = BarItemMergeType.Add;
		AppMenu.ItemLinks.Add(barItemLink);
		ExplorerPanelBarItemLink = ExplorerPanelBarItem.CreateLink();
		AppMenu.ItemLinks.Add(ExplorerPanelBarItemLink);
		RecentPanelBarItemLink = RecentPanelBarItem.CreateLink();
		AppMenu.ItemLinks.Add(RecentPanelBarItemLink);
		if (_allowHomePanel)
		{
			HomePanelItemLink = HomePanelItemBarButtonItem.CreateLink();
			AppMenu.ItemLinks.Add(HomePanelItemLink);
		}
		ChangeOnlineStatusBarItemLink = ChangeOnlineStatusBarButtonItem.CreateLink();
		ChangeOnlineStatusBarItemLink.IsVisible = false;
		AppMenu.ItemLinks.Add(ChangeOnlineStatusBarItemLink);
		AppMenu.ItemLinks.Add(new BarItemLinkSeparator());
		barItemLink = _btnSpreadSheet.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.Large;
		barItemLink.VerticalAlignment = VerticalAlignment.Center;
		AppMenu.ItemLinks.Add(barItemLink);
		barItemLink = _btnSpreadSheetOpen.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.Large;
		barItemLink.VerticalAlignment = VerticalAlignment.Center;
		AppMenu.ItemLinks.Add(barItemLink);
		AppMenu.ItemLinks.Add(new BarItemLinkSeparator());
		AppMenu.ItemLinks.Add(ExitBarButtonItem.CreateLink());
		RibbonControl.ToolbarItemLinks.Add(CloseWindowsBarButtonItem.ConvertForToolbar("t_close_all_windows_16.svg"));
		RibbonControl.ToolbarItemLinks.Add(ExplorerPanelBarButtonItem.ConvertForToolbar("t_pane_expand_16.svg"));
		if (_allowHomePanel)
		{
			RibbonControl.ToolbarItemLinks.Add(HomePanelBarButtonItem.ConvertForToolbar("t_home_16.svg"));
		}
		RibbonDefaultPageCategory = new RibbonDefaultPageCategory();
		RibbonControl.Categories.Add(RibbonDefaultPageCategory);
		_ribbonPageHome = new RibbonPage
		{
			Name = "RibbonPageHome",
			CaptionTemplate = (DataTemplate)Settings.GenericResources["RibbonPageHeaderTemplate"]
		};
		RibbonDefaultPageCategory.Pages.Add(_ribbonPageHome);
		_ribbonPageHome.Caption = ArmSoft.SME.Common.Resources.Home;
		_ribbonPageHome.MergeOrder = 0;
		_ribbonPageGroupRecords = new RibbonPageGroup
		{
			Caption = ArmSoft.SME.Common.Resources.Records,
			AllowCollapse = false,
			Glyph = ImageService.GetImage("add_small_32.svg")
		};
		MergingProperties.SetName((DependencyObject)(object)_ribbonPageGroupRecords, "RibbonPageGroupRecords");
		barItemLink = DataViewAddBarButtonItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.Large;
		_ribbonPageGroupRecords.ItemLinks.Add(barItemLink);
		barItemLink = DataViewEditBarButtonItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupRecords.ItemLinks.Add(barItemLink);
		barItemLink = DataViewDeleteBarButtonItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupRecords.ItemLinks.Add(barItemLink);
		barItemLink = DataViewCreateCopyBarButtonItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupRecords.ItemLinks.Add(barItemLink);
		_ribbonPageHome.Groups.Add(_ribbonPageGroupRecords);
		_ribbonPageGroupDataView = new RibbonPageGroup
		{
			Caption = ArmSoft.SME.Common.Resources.DataView,
			AllowCollapse = false,
			Glyph = ImageService.GetImage("table_16.svg"),
			Name = "RibbonPageGroupDataView"
		};
		barItemLink = ShowDataViewDialogBarButtonItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.Large;
		_ribbonPageGroupDataView.ItemLinks.Add(barItemLink);
		barItemLink = RefreshDataViewBarButtonItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupDataView.ItemLinks.Add(barItemLink);
		barItemLink = _bExportToExcel.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupDataView.ItemLinks.Add(barItemLink);
		barItemLink = _cmdGroupPanel.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupDataView.ItemLinks.Add(barItemLink);
		_ribbonPageGroupDataView.ItemLinks.Add(new BarItemLinkSeparator());
		barItemLink = _cmdFind.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.Large;
		_ribbonPageGroupDataView.ItemLinks.Add(barItemLink);
		barItemLink = _cmdAutoFilter.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupDataView.ItemLinks.Add(barItemLink);
		barItemLink = _btnFilterSelected.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupDataView.ItemLinks.Add(barItemLink);
		barItemLink = _cmdClearSort.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupDataView.ItemLinks.Add(barItemLink);
		_ribbonPageHome.Groups.Add(_ribbonPageGroupDataView);
		_ribbonPageGroupPrint = new RibbonPageGroup
		{
			Caption = ArmSoft.SME.Common.Resources.Print,
			AllowCollapse = false,
			Glyph = ImageService.GetImage("print_16.svg"),
			Name = "RibbonPageGroupPrint"
		};
		barItemLink = _bPrintPreview.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.Large;
		_ribbonPageGroupPrint.ItemLinks.Add(barItemLink);
		barItemLink = _bPrint.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupPrint.ItemLinks.Add(barItemLink);
		barItemLink = PrintOptionsBarButtonItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupPrint.ItemLinks.Add(barItemLink);
		_ribbonPageHome.Groups.Add(_ribbonPageGroupPrint);
		_ribbonPageGroupDocument = new RibbonPageGroup
		{
			Caption = ArmSoft.SME.Common.Resources.Documents,
			AllowCollapse = false,
			Glyph = ImageService.GetImage("doc_preview_16.svg"),
			Name = "RibbonPageGroupDocument"
		};
		barItemLink = ShowTransactionsCheckItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.Large;
		barItemLink.MergeType = BarItemMergeType.Replace;
		_ribbonPageGroupDocument.ItemLinks.Add(barItemLink);
		barItemLink = DataViewDocumentBarButtonItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupDocument.ItemLinks.Add(barItemLink);
		barItemLink = ExportDocumentExcelBarButtonItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupDocument.ItemLinks.Add(barItemLink);
		barItemLink = DocumentHistoryBarButtonItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		_ribbonPageGroupDocument.ItemLinks.Add(barItemLink);
		_ribbonPageHome.Groups.Add(_ribbonPageGroupDocument);
		_ribbonPageGroupDirectory = new RibbonPageGroup
		{
			Caption = ArmSoft.SME.Common.Resources.Directories,
			AllowCollapse = false,
			Glyph = ImageService.GetImage("doc_preview_16.svg"),
			Name = "RibbonPageGroupDirectory"
		};
		barItemLink = _barButtonRefreshUICache.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.Large;
		_ribbonPageGroupDirectory.ItemLinks.Add(barItemLink);
		_ribbonPageHome.Groups.Add(_ribbonPageGroupDirectory);
		_ribbonPageDates = new RibbonPageGroup
		{
			Caption = ArmSoft.SME.Common.Resources.Tools,
			AllowCollapse = false,
			Name = "RibbonPageDates"
		};
		barItemLink = DatesBarButtonItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.Large;
		barItemLink.VerticalAlignment = VerticalAlignment.Center;
		_ribbonPageDates.ItemLinks.Add(barItemLink);
		barItemLink = _btnCalc.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.Large;
		barItemLink.VerticalAlignment = VerticalAlignment.Center;
		_ribbonPageDates.ItemLinks.Add(barItemLink);
		_ribbonPageHome.Groups.Add(_ribbonPageDates);
		_ribbonPageWindows = new RibbonPageGroup
		{
			Caption = ArmSoft.SME.Common.Resources.RibbonPageWindows,
			AllowCollapse = false,
			Name = "RibbonPageWindows"
		};
		barItemLink = CloseWindowsBarButtonItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.Large;
		barItemLink.VerticalAlignment = VerticalAlignment.Center;
		_ribbonPageWindows.ItemLinks.Add(barItemLink);
		_ribbonPageHome.Groups.Add(_ribbonPageWindows);
		RibbonPageFavorites = new RibbonPage
		{
			Name = "RibbonPageFavorites",
			CaptionTemplate = (DataTemplate)Settings.GenericResources["RibbonFavoritePageHeaderTemplate"],
			AllowDrop = true,
			Caption = ArmSoft.SME.Common.Resources.RibbonPageFavorites
		};
		RibbonPageFavorites.Groups.Add(new RibbonPageGroup
		{
			AllowCollapse = false
		});
		RibbonDefaultPageCategory.Pages.Add(RibbonPageFavorites);
		_ribbonPageHelp = new RibbonPage
		{
			Name = "RibbonPageHelp",
			Caption = ArmSoft.SME.Common.Resources.Help,
			CaptionTemplate = (DataTemplate)Settings.GenericResources["RibbonPageHeaderTemplate"]
		};
		RibbonDefaultPageCategory.Pages.Add(_ribbonPageHelp);
		_ribbonPageGroupArmsoftOnline = new RibbonPageGroup
		{
			Caption = ((Settings.Localization == Localizations.Armenia) ? ArmSoft.SME.Common.Resources.ArmsoftOnline : ArmSoft.SME.Common.Resources.ApricotSoftwareOnline),
			AllowCollapse = false,
			Name = "RibbonPageGroupArmsoftOnline"
		};
		_ribbonPageGroupArmsoftOnline.ItemLinks.Add(BarButtonKB.CreateLink());
		_ribbonPageGroupArmsoftOnline.ItemLinks.Add(WebDownloadBarButtonItem.CreateLink());
		_ribbonPageGroupArmsoftOnline.ItemLinks.Add(_barButtonWebSite.CreateLink());
		_ribbonPageGroupArmsoftOnline.ItemLinks.Add(_barButtonYouTube.CreateLink());
		_ribbonPageGroupArmsoftOnline.ItemLinks.Add(BarButtonCheckForUpdate.CreateLink());
		_ribbonPageGroupArmsoftOnline.ItemLinks.Add(_barButtonCheckOnlineServices.CreateLink());
		_ribbonPageHelp.Groups.Add(_ribbonPageGroupArmsoftOnline);
		_ribbonPageGroupRemoteSupport = new RibbonPageGroup
		{
			Caption = ArmSoft.SME.Common.Resources.RemoteSupport,
			AllowCollapse = false,
			Name = "RibbonPageGroupRemoteSupport"
		};
		_ribbonPageGroupRemoteSupport.ItemLinks.Add(_barButtonTeamViewer.CreateLink());
		_ribbonPageGroupRemoteSupport.ItemLinks.Add(_barButtonAnyDesk.CreateLink());
		if ((Settings.SystemType != SystemTypes.MarketPlace) & (Settings.Localization != Localizations.Georgia))
		{
			_ribbonPageGroupRemoteSupport.ItemLinks.Add(_barButtonTelegram.CreateLink());
		}
		_ribbonPageHelp.Groups.Add(_ribbonPageGroupRemoteSupport);
		_ribbonPageGroupHelp = new RibbonPageGroup
		{
			Caption = ArmSoft.SME.Common.Resources.Help,
			AllowCollapse = false
		};
		_ribbonPageGroupHelp.ItemLinks.Add(HelpBarButtonItem.CreateLink());
		_ribbonPageGroupHelp.ItemLinks.Add(AboutBarButtonItem.CreateLink());
		RegExcelBarButtonItem = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.RegisterExcelAddin,
			Glyph = ImageService.GetImage("excelAddin_16.svg"),
			LargeGlyph = ImageService.GetImage("excelAddin_32.svg")
		};
		BarManager.Items.AddWithMergingProperties(RegExcelBarButtonItem, "barButtonRegExcel");
		_ribbonPageGroupHelp.ItemLinks.Insert(0, RegExcelBarButtonItem.CreateLink());
		_barButtonOpenImport = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.ImportFiles,
			Glyph = ImageService.GetImage("folder_excel_16.svg"),
			LargeGlyph = ImageService.GetImage("folder_excel_32.svg")
		};
		BarManager.Items.AddWithMergingProperties(_barButtonOpenImport, "barButtonOpenImport");
		_ribbonPageGroupHelp.ItemLinks.Insert(1, _barButtonOpenImport.CreateLink());
		if (ConfigHelper.CallsRecordingIsAvailable)
		{
			_ribbonPageGroupHelp.ItemLinks.Add(BarButtonRecordUserCalls.CreateLink());
		}
		_ribbonPageHelp.Groups.Add(_ribbonPageGroupHelp);
	}

	protected override void BarManager_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (e.Item is BarButtonItem barButtonItem)
		{
			if (barButtonItem == _bChangePassword)
			{
				ChangePassword_ItemClick();
			}
			else if (barButtonItem == _btnChangePin)
			{
				ButtonChangePin_ItemClick();
			}
			else if (barButtonItem == _barButtonRefreshUICache)
			{
				RefreshUICache();
			}
			else if (barButtonItem == _barButtonCheckOnlineServices)
			{
				BarButtonCheckOnlineServices_ItemClick();
			}
			else if (barButtonItem == RemoveFromFavoritesBarButtonItem)
			{
				RemoveFromFavorites_ItemClick();
			}
			else if (barButtonItem == _btnCalc)
			{
				ShowWindowsCalculator();
			}
			else if (barButtonItem == _btnSpreadSheet)
			{
				NewSpreadSheet();
			}
			else if (barButtonItem == _btnSpreadSheetOpen)
			{
				AsSpreadSheet.OpenFile();
			}
			else if (barButtonItem == _barButtonYouTube)
			{
				BarButtonYouTube_ItemClick();
			}
			else if (barButtonItem == _barButtonWebSite)
			{
				BarButtonWebSite_ItemClick();
			}
			else if (barButtonItem == _barButtonTeamViewer)
			{
				BarButtonTeamViewer_ItemClick();
			}
			else if (barButtonItem == _barButtonAnyDesk)
			{
				BarButtonAnyDesk_ItemClick();
			}
			else if (barButtonItem == _barButtonTelegram)
			{
				BarButtonTelegram_ItemClick();
			}
			else if (barButtonItem == _barButtonOpenImport)
			{
				BarButtonOpenImport_ItemClick();
			}
		}
	}

	private static void BarButtonOpenImport_ItemClick()
	{
		string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Import");
		if (!Directory.Exists(text))
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.IncorrectDirectoryPath, text));
		}
		Process.Start(new ProcessStartInfo("explorer.exe", text)
		{
			UseShellExecute = true
		});
	}

	public static void BarButtonRegExcel_ItemClick(string addInPath)
	{
		if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
		{
			throw new ASException(ArmSoft.SME.Common.Resources.RunAsAdministrator);
		}
		if (!File.Exists(addInPath))
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FileDoesNotExist, addInPath));
		}
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(addInPath);
		string text = addInPath + ".manifest";
		if (!File.Exists(text))
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FileDoesNotExist, text));
		}
		string text2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AddIn", fileNameWithoutExtension + ".vsto");
		if (!File.Exists(text2))
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FileDoesNotExist, text2));
		}
		using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Office\\Excel\\Addins\\" + fileNameWithoutExtension))
		{
			if (registryKey == null)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ErrorCreatingRegKeyInLocalMachine);
			}
			registryKey.SetValue("Description", fileNameWithoutExtension, RegistryValueKind.String);
			registryKey.SetValue("FriendlyName", fileNameWithoutExtension, RegistryValueKind.String);
			registryKey.SetValue("LoadBehavior", 3, RegistryValueKind.DWord);
			registryKey.SetValue("Manifest", text2 + "|vstolocal", RegistryValueKind.String);
		}
		using (RegistryKey registryKey2 = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Office\\Excel\\Addins\\" + fileNameWithoutExtension))
		{
			if (registryKey2 == null)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ErrorCreatingRegKeyInCurrentUser);
			}
			registryKey2.SetValue("Description", fileNameWithoutExtension, RegistryValueKind.String);
			registryKey2.SetValue("FriendlyName", fileNameWithoutExtension, RegistryValueKind.String);
			registryKey2.SetValue("LoadBehavior", 3, RegistryValueKind.DWord);
			registryKey2.SetValue("Manifest", text2 + "|vstolocal", RegistryValueKind.String);
		}
		if (Environment.Is64BitOperatingSystem)
		{
			using (RegistryKey registryKey3 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Office\\Excel\\Addins\\" + fileNameWithoutExtension))
			{
				if (registryKey3 == null)
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ErrorCreatingRegKeyInLocalMachine);
				}
				registryKey3.SetValue("Description", fileNameWithoutExtension, RegistryValueKind.String);
				registryKey3.SetValue("FriendlyName", fileNameWithoutExtension, RegistryValueKind.String);
				registryKey3.SetValue("LoadBehavior", 3, RegistryValueKind.DWord);
				registryKey3.SetValue("Manifest", text2 + "|vstolocal", RegistryValueKind.String);
			}
			using RegistryKey registryKey4 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Office\\Excel\\Addins\\" + fileNameWithoutExtension);
			if (registryKey4 == null)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ErrorCreatingRegKeyInCurrentUser);
			}
			registryKey4.SetValue("Description", fileNameWithoutExtension, RegistryValueKind.String);
			registryKey4.SetValue("FriendlyName", fileNameWithoutExtension, RegistryValueKind.String);
			registryKey4.SetValue("LoadBehavior", 3, RegistryValueKind.DWord);
			registryKey4.SetValue("Manifest", text2 + "|vstolocal", RegistryValueKind.String);
		}
		MessageBox.Show(ArmSoft.SME.Common.Resources.OperationSucceeded, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
	}

	private static void BarButtonTelegram_ItemClick()
	{
		Process.Start(new ProcessStartInfo("https://t.me/+J-zFNtGhRX5lNmVi")
		{
			UseShellExecute = true
		});
	}

	private static void BarButtonAnyDesk_ItemClick()
	{
		string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AnyDesk.exe");
		if (!File.Exists(text))
		{
			text = "https://download.anydesk.com/AnyDesk.exe";
		}
		Process.Start(new ProcessStartInfo(text)
		{
			UseShellExecute = true
		});
	}

	private static void BarButtonTeamViewer_ItemClick()
	{
		string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ASRemoteSupport.exe");
		if (!File.Exists(text))
		{
			text = "https://download.armsoft.am/TechSupport/AsRemoteSupport.exe";
		}
		Process.Start(new ProcessStartInfo(text)
		{
			UseShellExecute = true
		});
	}

	private static void BarButtonWebSite_ItemClick()
	{
		Process.Start(new ProcessStartInfo("http://www.armsoft.am")
		{
			UseShellExecute = true
		});
	}

	private static void BarButtonYouTube_ItemClick()
	{
		Process.Start(new ProcessStartInfo((Settings.SystemType == SystemTypes.Accountant) ? "https://www.youtube.com/playlist?list=PL0knAScOnicZpgmuasLQ0fuDJfvlpWumZ" : "http://www.youtube.com/ArmenianSoftware")
		{
			UseShellExecute = true
		});
	}

	private static void NewSpreadSheet()
	{
		new AsSpreadSheet().Show();
	}

	private static void ShowWindowsCalculator()
	{
		Process process = new Process();
		process.StartInfo.FileName = "Calc.exe";
		process.Start();
	}

	public void AddToFavorites_ItemClick(BarButtonItem itm, NavigationCommand ti, UserAccessManagerBase userAccessManager)
	{
		BarButtonItem barButtonItem = MainFunctionsHelper.FindBarItemByName(BarManager, userAccessManager.GetCommandName(ti.Command));
		if (barButtonItem != null)
		{
			BarItemLink barItemLink = barButtonItem.CreateLink();
			barItemLink.RibbonStyle = RibbonItemStyles.Default;
			RibbonPageFavorites.Groups[0].ItemLinks.Add(barItemLink);
		}
	}

	private void RemoveFromFavorites_ItemClick()
	{
		if (RemoveFromFavoritesBarButtonItem.Tag != null)
		{
			RibbonPageFavorites.Groups[0].ItemLinks.Remove((BarItemLink)RemoveFromFavoritesBarButtonItem.Tag);
		}
	}

	private static void BarButtonCheckOnlineServices_ItemClick()
	{
		new CheckOnlineServicesWindow().ShowDialog();
	}

	private static void RefreshUICache()
	{
		new CacheLoadingWindow(executeByAsking: true).ShowDialog();
	}

	private static void ChangePassword_ItemClick()
	{
		new CloudUserPropertiesWindow().Invoke();
	}

	private static void ButtonChangePin_ItemClick()
	{
		if (!Settings.IsOffline)
		{
			new PinChangeWindow().ShowDialog();
		}
	}

	public void ReloadControlCaptions()
	{
		_ribbonPageHome.Caption = ArmSoft.SME.Common.Resources.Home;
		_ribbonPageGroupRecords.Caption = ArmSoft.SME.Common.Resources.Records;
		_ribbonPageGroupDataView.Caption = ArmSoft.SME.Common.Resources.DataView;
		_ribbonPageGroupDocument.Caption = ArmSoft.SME.Common.Resources.Documents;
		_ribbonPageDates.Caption = ArmSoft.SME.Common.Resources.Tools;
		_ribbonPageWindows.Caption = ArmSoft.SME.Common.Resources.RibbonPageWindows;
		_ribbonPageGroupPrint.Caption = ArmSoft.SME.Common.Resources.Print;
		RibbonPageFavorites.Caption = ArmSoft.SME.Common.Resources.RibbonPageFavorites;
		_ribbonPageHelp.Caption = ArmSoft.SME.Common.Resources.Help;
		_ribbonPageGroupArmsoftOnline.Caption = ((Settings.Localization == Localizations.Armenia) ? ArmSoft.SME.Common.Resources.ArmsoftOnline : ArmSoft.SME.Common.Resources.ApricotSoftwareOnline);
		_ribbonPageGroupRemoteSupport.Caption = ArmSoft.SME.Common.Resources.RemoteSupport;
		_ribbonPageGroupHelp.Caption = ArmSoft.SME.Common.Resources.Help;
		_ribbonPageGroupDirectory.Caption = ArmSoft.SME.Common.Resources.Directories;
		LoginBarButtonItem.Content = ArmSoft.SME.Common.Resources.Login;
		LoginOffLineBarButtonItem.Content = ArmSoft.SME.Common.Resources.LoginOffline;
		LogoutBarButtonItem.Content = ArmSoft.SME.Common.Resources.Logout;
		_bChangePassword.Content = ArmSoft.SME.Common.Resources.UserProperties;
		OptionsWindowBarButtonItem.Content = ArmSoft.SME.Common.Resources.Options;
		ExitBarButtonItem.Content = ArmSoft.SME.Common.Resources.ExitWord;
		ExplorerPanelBarItem.Content = ArmSoft.SME.Common.Resources.Explorer;
		RecentPanelBarItem.Content = ArmSoft.SME.Common.Resources.History;
		HomePanelItemBarButtonItem.Content = ArmSoft.SME.Common.Resources.HomePanel;
		_btnCalc.Content = ArmSoft.SME.Common.Resources.Calculator;
		DatesBarButtonItem.Content = ArmSoft.SME.Common.Resources.Dates;
		CloseWindowsBarButtonItem.Content = ArmSoft.SME.Common.Resources.RibbonCloseAllWnds;
		ExplorerPanelBarButtonItem.Content = ArmSoft.SME.Common.Resources.Explorer;
		HomePanelBarButtonItem.Content = ArmSoft.SME.Common.Resources.Home;
		_btnSpreadSheet.Content = ArmSoft.SME.Common.Resources.NewSpreadSheet;
		_btnSpreadSheetOpen.Content = ArmSoft.SME.Common.Resources.OpenSpreadSheet;
		DataViewAddBarButtonItem.Content = ArmSoft.SME.Common.Resources.NewAdd;
		DataViewEditBarButtonItem.Content = ArmSoft.SME.Common.Resources.Edit;
		DataViewDeleteBarButtonItem.Content = ArmSoft.SME.Common.Resources.Delete;
		DataViewCreateCopyBarButtonItem.Content = ArmSoft.SME.Common.Resources.Copy;
		ShowDataViewDialogBarButtonItem.Content = ArmSoft.SME.Common.Resources.ShowDialog;
		RefreshDataViewBarButtonItem.Content = ArmSoft.SME.Common.Resources.Refresh;
		_bExportToExcel.Content = ArmSoft.SME.Common.Resources.ExportToExcel;
		_cmdGroupPanel.Content = ArmSoft.SME.Common.Resources.Grouping;
		_cmdFind.Content = ArmSoft.SME.Common.Resources.Find;
		_cmdClearSort.Content = ArmSoft.SME.Common.Resources.ClearSortShort;
		_btnFilterSelected.Content = ArmSoft.SME.Common.Resources.FilterSelected;
		_cmdAutoFilter.Content = ArmSoft.SME.Common.Resources.ShowFilterRow;
		_bPrintPreview.Content = ArmSoft.SME.Common.Resources.PrintPreview;
		_bPrint.Content = ArmSoft.SME.Common.Resources.Print;
		PrintOptionsBarButtonItem.Content = ArmSoft.SME.Common.Resources.PreviewOptions;
		ShowTransactionsCheckItem.Content = ((Settings.SystemType == SystemTypes.SalesManagement) ? ArmSoft.SME.Common.Resources.ViewMoreDetails : ArmSoft.SME.Common.Resources.ViewTrans);
		DataViewDocumentBarButtonItem.Content = ArmSoft.SME.Common.Resources.PrintPreview;
		ExportDocumentExcelBarButtonItem.Content = ArmSoft.SME.Common.Resources.ExportToExcel;
		DocumentHistoryBarButtonItem.Content = ArmSoft.SME.Common.Resources.History;
		_barButtonRefreshUICache.Content = ArmSoft.SME.Common.Resources.Refresh;
		HelpBarButtonItem.Content = ArmSoft.SME.Common.Resources.Help;
		BarButtonKB.Content = ArmSoft.SME.Common.Resources.KnowledgeBase;
		_barButtonTeamViewer.Content = "TeamViewer";
		_barButtonAnyDesk.Content = "AnyDesk";
		_barButtonTelegram.Content = "Telegram";
		LiveChatBarButtonItem.Content = ((Settings.Localization == Localizations.Armenia) ? ArmSoft.SME.Common.Resources.ArmSoftLiveChat : ArmSoft.SME.Common.Resources.ApricotSoftwareLiveChat);
		WebDownloadBarButtonItem.Content = ArmSoft.SME.Common.Resources.TempSmall;
		_barButtonWebSite.Content = ((Settings.Localization == Localizations.Armenia) ? ArmSoft.SME.Common.Resources.ArmSoftHomePage : ArmSoft.SME.Common.Resources.ApricotSoftwareHomePage);
		BarButtonCheckForUpdate.Content = ArmSoft.SME.Common.Resources.CheckForUpdates;
		_barButtonCheckOnlineServices.Content = ArmSoft.SME.Common.Resources.CheckOnlineServices;
		UpdateAvailableBarButtonItem.Content = ArmSoft.SME.Common.Resources.UpdateAvailable;
		MobileBarButtonItem.Content = ArmSoft.SME.Common.Resources.MobileVersion;
		_barButtonYouTube.Content = ArmSoft.SME.Common.Resources.YouTube;
		AddToFavoritesBarButtonItem.Content = ArmSoft.SME.Common.Resources.AddToFavorites;
		RemoveFromFavoritesBarButtonItem.Content = ArmSoft.SME.Common.Resources.RemoveFromFavorites;
		RegExcelBarButtonItem.Content = ArmSoft.SME.Common.Resources.RegisterExcelAddin;
		_barButtonOpenImport.Content = ArmSoft.SME.Common.Resources.ImportFiles;
	}

	public void ConfigureChangeOnlineStatusButton(bool disconnectedAfterLoggingIntoCloud)
	{
		if (Settings.IsOffline)
		{
			ChangeOnlineStatusBarButtonItem.IsEnabled = Settings.LoginService.IsLoggedIn & disconnectedAfterLoggingIntoCloud;
			ChangeOnlineStatusBarButtonItem.Content = ArmSoft.SME.Common.Resources.RestoreCloudConnection;
			_btnChangePin.IsEnabled = false;
		}
		else
		{
			if (Settings.LoginService.IsLoggedIn && Settings.OfflineModeIsAvailable)
			{
				ChangeOnlineStatusBarButtonItem.IsEnabled = true;
				_btnChangePin.IsEnabled = true;
			}
			else
			{
				ChangeOnlineStatusBarButtonItem.IsEnabled = false;
				_btnChangePin.IsEnabled = false;
			}
			ChangeOnlineStatusBarButtonItem.Content = ArmSoft.SME.Common.Resources.CloseCloudConnection;
		}
		if (Settings.OfflineModeIsAvailable)
		{
			LoginOffLineBarButtonItem.IsEnabled = true;
		}
		else
		{
			LoginOffLineBarButtonItem.IsEnabled = false;
		}
	}

	public void SetBarsStates(bool allowHomePanel, IAppUpdateService appUpdateService)
	{
		LoginBarButtonItem.IsEnabled = true;
		LogoutBarButtonItem.IsEnabled = Settings.LoginService.IsLoggedIn;
		_bChangePassword.IsEnabled = LogoutBarButtonItem.IsEnabled;
		OptionsWindowBarButtonItem.IsEnabled = LogoutBarButtonItem.IsEnabled;
		ExitBarButtonItem.IsEnabled = true;
		ExplorerPanelBarItem.IsEnabled = Settings.LoginService.IsLoggedIn;
		HomePanelItemBarButtonItem.IsEnabled = allowHomePanel && ExplorerPanelBarItem.IsEnabled;
		RecentPanelBarItem.IsEnabled = ExplorerPanelBarItem.IsEnabled;
		DataViewAddBarButtonItem.IsEnabled = false;
		DataViewEditBarButtonItem.IsEnabled = false;
		DataViewCreateCopyBarButtonItem.IsEnabled = false;
		DataViewDeleteBarButtonItem.IsEnabled = false;
		ShowDataViewDialogBarButtonItem.IsEnabled = false;
		RefreshDataViewBarButtonItem.IsEnabled = false;
		_bExportToExcel.IsEnabled = false;
		_cmdGroupPanel.IsEnabled = false;
		_cmdFind.IsEnabled = false;
		_cmdAutoFilter.IsEnabled = false;
		_btnFilterSelected.IsEnabled = false;
		_cmdClearSort.IsEnabled = false;
		_bPrintPreview.IsEnabled = false;
		_bPrint.IsEnabled = false;
		PrintOptionsBarButtonItem.IsEnabled = false;
		ShowTransactionsCheckItem.IsEnabled = false;
		DataViewDocumentBarButtonItem.IsEnabled = false;
		ExportDocumentExcelBarButtonItem.IsEnabled = false;
		DocumentHistoryBarButtonItem.IsEnabled = false;
		_barButtonRefreshUICache.IsEnabled = Settings.LoginService.IsLoggedIn;
		CloseWindowsBarButtonItem.IsEnabled = Settings.LoginService.IsLoggedIn;
		ExplorerPanelBarButtonItem.IsEnabled = Settings.LoginService.IsLoggedIn;
		HomePanelBarButtonItem.IsEnabled = allowHomePanel && ExplorerPanelBarItem.IsEnabled;
		if (appUpdateService.UpdateAvailable && RibbonControl.PageHeaderItemLinks.Count == 0)
		{
			RibbonControl.PageHeaderItemLinks.Add(UpdateAvailableBarButtonItem.CreateLink());
		}
		foreach (BarItemLinkBase toolbarItemLink in RibbonControl.ToolbarItemLinks)
		{
			toolbarItemLink.IsEnabled = Settings.LoginService.IsLoggedIn;
		}
		if (!Settings.LoginService.IsLoggedIn)
		{
			RibbonControl.ToolbarShowMode = RibbonQuickAccessToolbarShowMode.Hide;
		}
	}

	public void LoadFavoriteRibbon(UserAccessManagerBase userAccessManager, NavigationBarsControllerBase navigationBarsController)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		RibbonPageFavorites.Groups[0].ItemLinks.Clear();
		string text = Settings.ParametersService.FavoriteItems();
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		string[] array = text.Split(",");
		short subSystem = 0;
		for (int i = 0; i <= array.Length - 1; i++)
		{
			AsNavigationTreeNode treeNodeByCommand = NavigationBarsControllerBase.GetTreeNodeByCommand(short.Parse(array[i]), userAccessManager, ref subSystem);
			if (treeNodeByCommand != null)
			{
				NavigationCommand nc = new NavigationCommand
				{
					Command = treeNodeByCommand.Command,
					Subsystem = subSystem,
					CommandType = treeNodeByCommand.CommandType,
					CommandTag = treeNodeByCommand.CommandTag
				};
				AddCommandToFavorites(treeNodeByCommand.FullHeader, nc, ImageService.GetImage(treeNodeByCommand.Image), userAccessManager, navigationBarsController);
			}
		}
	}

	public void AddCommandToFavorites(string title, NavigationCommand nc, ImageSource img, UserAccessManagerBase userAccessManager, NavigationBarsControllerBase navigationBarsController)
	{
		string commandName = userAccessManager.GetCommandName(nc.Command);
		BarButtonItemLink barButtonItemLink = MainFunctionsHelper.FindBarItemByNameInFavorites(commandName);
		if (barButtonItemLink != null)
		{
			return;
		}
		BarButtonItem barButtonItem = MainFunctionsHelper.FindBarItemByName(BarManager, commandName);
		if (barButtonItem == null)
		{
			barButtonItem = new BarButtonItem
			{
				Content = title,
				Tag = nc,
				Name = commandName,
				Glyph = img
			};
			barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>((object? s, ItemClickEventArgs e) =>
			{
				Favorite_click(s, navigationBarsController);
			}).Handler;
			BarManager.Items.Add(barButtonItem);
		}
		barButtonItemLink = (BarButtonItemLink)barButtonItem.CreateLink();
		RibbonPageFavorites.Groups[0].ItemLinks.Add(barButtonItemLink);
	}

	private static void Favorite_click(object sender, NavigationBarsControllerBase navigationBarsController)
	{
		if (Settings.LoginService.IsLoggedIn)
		{
			NavigationCommand navigationCommand = (NavigationCommand)((BarButtonItem)sender).Tag;
			if (!navigationBarsController.CheckUserAccess(navigationCommand.Command))
			{
				MessageBox.Show(ArmSoft.SME.Common.Resources.AccessDenied, "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else
			{
				navigationBarsController.ExecuteCommand(navigationCommand);
			}
		}
	}

	public void SaveFavoritesRibbon()
	{
		if (!RibbonPageFavorites.IsVisible || Settings.IsOffline)
		{
			return;
		}
		string text = "";
		foreach (BarItemLinkBase itemLink in RibbonPageFavorites.Groups[0].ItemLinks)
		{
			if (itemLink is BarButtonItemLink barButtonItemLink && barButtonItemLink.Item.Tag != null && barButtonItemLink.Item.Tag is NavigationCommand navigationCommand)
			{
				text = text + "," + navigationCommand.Command;
			}
		}
		if (text.Length > 0)
		{
			text = text.Substring(1);
			if (text.Length > 255)
			{
				text = text.Substring(0, 254);
			}
		}
		Settings.ParametersService.FavoriteItems_set(text, Settings.LoginService.UserId);
		RibbonPageFavorites.Groups[0].ItemLinks.Clear();
	}
}
