using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Models.Cloud;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.BackgroundWorks;
using ArmSoft.SME.Core.UI.Caches;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DataViews.Directories;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Navigation;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.StandartWindow;
using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet.Functions;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Docking.Base;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Layout.Core;
using DevExpress.Xpf.Ribbon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Serilog;

namespace ArmSoft.SME.Core.UI;

public abstract class MainWindowBase : ThemedWindow, INotifyPropertyChanged
{
	protected delegate void DelegateSerialPortDataReceived(string serialData);

	public delegate void ResponseOnLogOutEventHandler(object sender);

	protected internal bool SyncVar;

	protected BarContainerControl _leftDockControl;

	protected BarContainerControl _bottomDockControl;

	protected RibbonStatusBarControl _ribbonStatusBar;

	protected BarStaticItem _statusBarCurrentRowInner;

	protected BarStaticItem _statusBarSelectedRowsInner;

	protected BarStaticItem _statusBarCompanyNameInner;

	protected BarStaticItem _statusBarWorkPeriod;

	protected BarStaticItem _statusBarKeySerial;

	protected BarButtonItem _statusBarNewNotifications;

	protected BarStaticItem _statusBarCloudMode;

	protected HeaderItemControl _helpHeaderItem;

	protected HeaderItemControl _cloudUserHeaderItem;

	protected int _virtualProcessId;

	protected UpdateResponse _appUpdateResponse;

	protected string _buffer = "";

	public BarStaticItem StatusBarTaxServiceConnectionState;

	public MainRibbonSupportedPanel MainRibbonPanel;

	private short _onlineLicenseFailedRenewInH = 3;

	private DateTime _onlineLicenseRenewDate;

	public bool OnlineLicenseRenewResult;

	public string OnlineLicenseRenewMessage;

	public static readonly DependencyProperty RibbonEditModeProperty = DependencyProperty.Register("RibbonEditMode", typeof(bool), typeof(MainWindowBase));

	public static readonly DependencyProperty CurrentDateProperty = DependencyProperty.Register("CurrentDate", typeof(DateOnly), typeof(MainWindowBase));

	public DockLayoutManager LayoutManager;

	public LayoutGroup MainGroup;

	public DocumentGroup DocGroupPanels;

	public TabbedGroup NavigatorGroup;

	public HistoryPanel RecentsPanel;

	public HistoryController RecentsController;

	public SolidColorBrush gUserColor;

	public SerialPort Scanner;

	public SerialPort MCR;

	public bool FailedToOpenComPort;

	public string FailedToOpenComPortError = "";

	private LayoutPanel _emptyContent;

	private SimpleButton _emptyContentNavigatorButton;

	private SimpleButton _emptyContentHomeButton;

	private TextBlock _cloudUserName;

	private DateOnly _currentDate;

	private DateOnly _dateBegin;

	private DateOnly _dateEnd;

	private object _currentPanel;

	private RibbonPage _activeRibbonPage;

	private bool _mainWindowBase_Activated_FirstCall = true;

	private DXImage _userImage;

	private Timer _notificationsTimer;

	private SemaphoreSlim _notificationsSemaphore;

	private Timer _statisticsTimer;

	private SemaphoreSlim _statisticsSemaphore;

	private Timer _synchronizationsTimer;

	private Timer _offlineDocsSynchronizationsTimer;

	private SemaphoreSlim _synchronizationsSemaphore;

	private SemaphoreSlim _offlineDocsSynchronizationsSemaphore;

	private readonly IAppUpdateService _appUpdateService;

	private readonly DispatcherTimer _timerPing;

	private Timer _onlineLicenseTimer;

	private NotificationService _notificationService;

	protected ContractInformationWindowDelegate CallBackForCreatePayOrderDelegate { get; set; }

	protected List<Type> PanelsForClosing { get; set; }

	protected internal BackgroundBackupWindow AutoBackupWindow { get; set; }

	public AutoHideGroup AutoHideLeft { get; set; }

	public virtual Dictionary<short, string> Subsystems => new Dictionary<short, string>();

	protected abstract DocumentPanel HomePanel { get; }

	protected abstract bool AllowHomePanel { get; }

	public abstract UserAccessManagerBase UserAccessManager { get; }

	public abstract NavigationBarsControllerBase NavigationBarsController { get; }

	public abstract NotificationsProcessorBase NotificationsProcessor { get; }

	public abstract SynchronizationsProcessorBase SynchronizationsProcessor { get; }

	public virtual OfflineSynchronizationsProcessorBase OfflineSynchronizationsProcessor => new OfflineSynchronizationsProcessorBase();

	public abstract string AddInPath { get; }

	public abstract Uri AdsUri { get; }

	public bool RibbonEditMode
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(RibbonEditModeProperty);
		}
		set
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RibbonEditMode"));
			((DependencyObject)this).SetValue(RibbonEditModeProperty, (object)value);
		}
	}

	public DateOnly CurrentDate
	{
		get
		{
			return _currentDate;
		}
		set
		{
			_currentDate = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentDate"));
			((DependencyObject)this).SetValue(CurrentDateProperty, (object)value);
		}
	}

	public object CurrentPanel
	{
		get
		{
			return _currentPanel;
		}
		set
		{
			_currentPanel = value;
		}
	}

	public DataViewPanel CurrentViewPanel { get; set; }

	public object StatusBarCompanyName
	{
		get
		{
			return _statusBarCompanyNameInner.Content;
		}
		set
		{
			_statusBarCompanyNameInner.Content = value;
		}
	}

	public SolidColorBrush UserColor
	{
		get
		{
			return gUserColor;
		}
		set
		{
			gUserColor = value;
		}
	}

	public virtual DateOnly DateBegin
	{
		get
		{
			return _dateBegin;
		}
		set
		{
			_dateBegin = value;
		}
	}

	public virtual DateOnly DateEnd
	{
		get
		{
			return _dateEnd;
		}
		set
		{
			_dateEnd = value;
		}
	}

	public RibbonPage ActiveRibbonPage
	{
		get
		{
			return _activeRibbonPage;
		}
		set
		{
			_activeRibbonPage = value;
		}
	}

	private bool DisconnectedAfterLoggingIntoCloud { get; set; }

	public List<HistoryCategory> HistoryCategories { get; set; }

	protected virtual Type NotificationActivatorType => null;

	public event PropertyChangedEventHandler PropertyChanged;

	public event ResponseOnLogOutEventHandler ResponseOnLogOut;

	public MainWindowBase()
	{
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bce: Expected Obj, but got Unknown
		_appUpdateService = Settings.ServiceProvider.GetRequiredService<IAppUpdateService>();
		Height = 430.0;
		Width = 635.0;
		_virtualProcessId = Settings.ProcessId();
		WindowStyle = WindowStyle.SingleBorderWindow;
		WindowState = WindowState.Maximized;
		WindowKind = WindowKind.Ribbon;
		UseLayoutRounding = true;
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		CurrentDate = DateTime.Today.ToDateOnly();
		DateBegin = new DateOnly(CurrentDate.Year, CurrentDate.Month, 1);
		DateEnd = CurrentDate;
		PropertyChanged += new SafeEventHandler<PropertyChangedEventArgs>(MainWindow_PropertyChanged).Handler;
		RecentsController = new HistoryController();
		MainRibbonPanel = new MainRibbonSupportedPanel(AllowHomePanel);
		MainRibbonPanel.LoginBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BLogin_ItemClick).Handler;
		MainRibbonPanel.LoginOffLineBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BLoginOffline_ItemClick).Handler;
		MainRibbonPanel.LogoutBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BLogout_ItemClick).Handler;
		MainRibbonPanel.OptionsWindowBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(OptionsWindow_ItemClick).Handler;
		MainRibbonPanel.ExitBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(Exit_ItemClick).Handler;
		MainRibbonPanel.ExplorerPanelBarItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(Explorer).Handler;
		MainRibbonPanel.RecentPanelBarItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(RecentPanelBarItem_ItemClick).Handler;
		MainRibbonPanel.DatesBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ShowDates).Handler;
		MainRibbonPanel.CloseWindowsBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(CloseAllWindows).Handler;
		MainRibbonPanel.ConfigureChangeOnlineStatusButton(DisconnectedAfterLoggingIntoCloud);
		MainRibbonPanel.ChangeOnlineStatusBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ButtonChangeOnlineStatus_ItemClick).Handler;
		MainRibbonPanel.AddToFavoritesBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(AddToFavorites_ItemClick).Handler;
		if (ConfigHelper.CallsRecordingIsAvailable)
		{
			MainRibbonPanel.BarButtonRecordUserCalls.CheckedChanged += new SafeEventHandler<ItemClickEventArgs>(BarButtonRecordUserCalls_CheckedChanged).Handler;
		}
		_statusBarCurrentRowInner = new BarStaticItem
		{
			Content = "",
			ItemMinWidth = 150.0,
			AutoSizeMode = BarItemAutoSizeMode.Fill,
			ContentAlignment = HorizontalAlignment.Center,
			Focusable = false
		};
		_statusBarSelectedRowsInner = new BarStaticItem
		{
			Content = "",
			ItemMinWidth = 50.0,
			AutoSizeMode = BarItemAutoSizeMode.Fill,
			ContentAlignment = HorizontalAlignment.Center,
			Focusable = false
		};
		_statusBarCompanyNameInner = new BarStaticItem
		{
			Content = "",
			ItemMinWidth = 100.0
		};
		_statusBarWorkPeriod = new BarStaticItem
		{
			Content = "",
			ItemMinWidth = 100.0,
			AutoSizeMode = BarItemAutoSizeMode.Content
		};
		_statusBarNewNotifications = new BarButtonItem
		{
			Content = "",
			ToolTip = ArmSoft.SME.Common.Resources.Notifications,
			Glyph = ImageService.GetImage("notification_16.svg")
		};
		_statusBarNewNotifications.ItemClick += new SafeEventHandler<ItemClickEventArgs>(NewNotifications_ItemClick).Handler;
		DockPanel dockPanel = new DockPanel
		{
			UseLayoutRounding = true
		};
		_leftDockControl = new BarContainerControl
		{
			Orientation = Orientation.Vertical,
			UseLayoutRounding = true,
			Focusable = false
		};
		DockPanel.SetDock(_leftDockControl, Dock.Left);
		dockPanel.Children.Add(_leftDockControl);
		_bottomDockControl = new BarContainerControl
		{
			UseLayoutRounding = true,
			Focusable = false
		};
		DockPanel.SetDock(_bottomDockControl, Dock.Bottom);
		dockPanel.Children.Add(_bottomDockControl);
		_ribbonStatusBar = new RibbonStatusBarControl
		{
			UseLayoutRounding = true,
			Focusable = false
		};
		DockPanel.SetDock(_ribbonStatusBar, Dock.Bottom);
		dockPanel.Children.Add(_ribbonStatusBar);
		BarItemLink barItemLink = _statusBarCurrentRowInner.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		barItemLink.Focusable = false;
		_ribbonStatusBar.LeftItemLinks.AddWithSeparator(barItemLink);
		barItemLink = _statusBarSelectedRowsInner.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		barItemLink.Focusable = false;
		_ribbonStatusBar.LeftItemLinks.AddWithSeparator(barItemLink);
		_ribbonStatusBar.LeftItemLinks.AddWithSeparator(_statusBarCompanyNameInner.CreateLink());
		_ribbonStatusBar.LeftItemLinks.AddWithSeparator(_statusBarWorkPeriod.CreateLink());
		_statusBarKeySerial = new BarStaticItem
		{
			Content = "",
			ItemMinWidth = 75.0,
			AutoSizeMode = BarItemAutoSizeMode.Fill,
			ContentAlignment = HorizontalAlignment.Center,
			IsVisible = false
		};
		if (Settings.SystemType != SystemTypes.SalesManagement)
		{
			_statusBarKeySerial.ItemDoubleClick += new SafeEventHandler<ItemClickEventArgs>(CopyKeySerial).Handler;
		}
		_statusBarCloudMode = new BarStaticItem
		{
			Content = "",
			ItemMinWidth = 25.0,
			AutoSizeMode = BarItemAutoSizeMode.Fill,
			ContentAlignment = HorizontalAlignment.Center,
			IsVisible = false
		};
		StatusBarTaxServiceConnectionState = new BarStaticItem
		{
			Content = ArmSoft.SME.Common.Resources.EInvoicing,
			Glyph = ImageService.GetImage("TaxService_line_16.svg"),
			AutoSizeMode = BarItemAutoSizeMode.Fill,
			ContentAlignment = HorizontalAlignment.Center,
			ShowBorder = false,
			IsVisible = false
		};
		StatusBarTaxServiceConnectionState.ItemClick += new SafeEventHandler<ItemClickEventArgs>(StatusBarTaxServiceConnectionState_ItemClick).Handler;
		_ribbonStatusBar.RightItemLinks.AddWithSeparator(StatusBarTaxServiceConnectionState.CreateLink());
		_ribbonStatusBar.RightItemLinks.AddWithSeparator(_statusBarCloudMode);
		_ribbonStatusBar.RightItemLinks.AddWithSeparator(_statusBarKeySerial);
		_ribbonStatusBar.RightItemLinks.AddWithSeparator(_statusBarNewNotifications);
		Binding binding = new Binding("NewItemsMode")
		{
			Source = RecentsController,
			Mode = BindingMode.OneWay,
			UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
		};
		_statusBarNewNotifications.SetBinding(BarItem.IsVisibleProperty, binding);
		MainRibbonPanel.RibbonControl.Drop += new SafeEventHandler<DragEventArgs>(Ribbon_Drop).Handler;
		MainRibbonPanel.RibbonControl.MouseLeftButtonDown += new SafeEventHandler<MouseButtonEventArgs>(Ribbon_MouseLeftButtonDown).Handler;
		MainRibbonPanel.RibbonControl.RibbonPopupMenuShowing += new SafeEventHandler<RibbonPopupMenuShowingEventArgs>(Ribbon_RibbonPopupMenuShowing).Handler;
		dockPanel.Children.Add(MainRibbonPanel.RibbonControl);
		LayoutManager = new DockLayoutManager
		{
			MDIMergeStyle = MDIMergeStyle.WhenChildActivated,
			FloatingMode = DevExpress.Xpf.Docking.FloatingMode.Desktop,
			ClosingBehavior = ClosingBehavior.ImmediatelyRemove
		};
		MainGroup = new LayoutGroup
		{
			AllowDrop = true,
			ClosingBehavior = ClosingBehavior.ImmediatelyRemove
		};
		LayoutManager.LayoutRoot = MainGroup;
		LayoutManager.DockItemHidden += new SafeEventHandler<ItemEventArgs>(LayoutManager_DockItemHidden).Handler;
		LayoutManager.UnMerge += new SafeEventHandler<BarMergeEventArgs>(LayoutManager_UnMerge).Handler;
		LayoutManager.DockItemActivating += new SafeEventHandler<ItemCancelEventArgs>(LayoutManager_DockItemActivating).Handler;
		LayoutManager.DockItemClosing += new SafeEventHandler<ItemCancelEventArgs>(LayoutManager_DockItemClosing).Handler;
		LayoutManager.DockItemActivated += new SafeEventHandler<DockItemActivatedEventArgs>(LayoutManager_DockItemActivated).Handler;
		LayoutManager.DockItemClosed += new SafeEventHandler<DockItemClosedEventArgs>(LayoutManager_DockItemClosed).Handler;
		NavigatorGroup = new TabbedGroup
		{
			AllowDrag = false,
			AllowDrop = false,
			ClosingBehavior = ClosingBehavior.ImmediatelyRemove,
			UseLayoutRounding = true
		};
		MainGroup.Items.Insert(0, NavigatorGroup);
		DocGroupPanels = new DocumentGroup
		{
			Name = "MyDocumentGroup",
			TabHeaderLayoutType = TabHeaderLayoutType.Trim,
			CaptionAlignMode = CaptionAlignMode.Default,
			ItemWidth = new GridLength(1.0, GridUnitType.Star),
			ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPageHeaders,
			ShowTabForSinglePage = true,
			DestroyOnClosingChildren = false,
			MDIStyle = MDIStyle.Tabbed,
			ClosingBehavior = ClosingBehavior.ImmediatelyRemove
		};
		DocGroupPanels.SelectedItemChanged += new SafeEventHandler<DevExpress.Xpf.Docking.Base.SelectedItemChangedEventArgs>(DocGroupPanels_SelectedItemChanged).Handler;
		MainGroup.Items.Add(DocGroupPanels);
		AutoHideLeft = new AutoHideGroup
		{
			DockType = Dock.Left,
			Name = "AutoHideLeft",
			ClosingBehavior = ClosingBehavior.Default
		};
		LayoutManager.AutoHideGroups.Add(AutoHideLeft);
		dockPanel.Children.Add(LayoutManager);
		MainRibbonPanel.BarManager.Child = dockPanel;
		LayoutManager.DockController.Dock(CreateEmptyContent());
		Content = MainRibbonPanel.BarManager;
		int num = 3;
		List<Type> list = new List<Type>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<Type> span = CollectionsMarshal.AsSpan(list);
		span[0] = typeof(DataViewPanel);
		span[1] = typeof(AsReportViewer);
		span[2] = typeof(AsSpreadSheet);
		PanelsForClosing = list;
		AutoBackupWindow = null;
		Activated += new SafeEventHandler<EventArgs>(MainWindow_Activated).Handler;
		Loaded += new SafeEventHandler<RoutedEventArgs>(MainWindow_Loaded).Handler;
		Closed += new SafeEventHandler<EventArgs>(MainWindow_Closed).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(MainWindow_PreviewKeyDown).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(MainWindowBase_Closing).Handler;
		AddCloudUserHeaderInfo();
		AddHelpHeaderItem();
		_timerPing = new DispatcherTimer(new TimeSpan(0, 0, 2, 0, 0), (DispatcherPriority)9, (EventHandler)TimerEventHandler, ((DispatcherObject)this).Dispatcher)
		{
			IsEnabled = false
		};
		InitCOMPorts();
		MainRibbonPanel.ExplorerPanelBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(Explorer).Handler;
		MainRibbonPanel.HomePanelBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(Home).Handler;
		MainRibbonPanel.HomePanelItemBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(HomePanelItem_ItemClick).Handler;
		MainRibbonPanel.ExplorerPanelBarItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ExplorerPanelBarItem_ItemClick).Handler;
		MainRibbonPanel.WebDownloadBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonWebDownload_ItemClick).Handler;
		MainRibbonPanel.LiveChatBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonLiveChat_ItemClick).Handler;
		MainRibbonPanel.UpdateAvailableBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonUpdateAvailable_ItemClick).Handler;
		MainRibbonPanel.MobileBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonMobile_ItemClick).Handler;
		MainRibbonPanel.BarButtonCheckForUpdate.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonCheckForUpdate_ItemClick).Handler;
		MainRibbonPanel.RegExcelBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonRegExcel_ItemClick).Handler;
		MainRibbonPanel.AboutBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonAbout_ItemClick).Handler;
		MainRibbonPanel.HelpBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonHelp_ItemClick).Handler;
		_helpHeaderItem.Click += new SafeEventHandler<RoutedEventArgs>(HelpHeaderItem_Click).Handler;
		_cloudUserHeaderItem.Click += new SafeEventHandler<RoutedEventArgs>(CloudUserHeaderItem_Click).Handler;
		RegisterNotificationActivator();
	}

	protected virtual void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
	}

	private void LayoutManager_DockItemActivated(object sender, DockItemActivatedEventArgs ea)
	{
		UpdateEmptyContentVisibility();
	}

	private void LayoutManager_DockItemClosed(object sender, DockItemClosedEventArgs e)
	{
		UpdateEmptyContentVisibility();
	}

	private void CloseAllWindows(object sender, ItemClickEventArgs e)
	{
		ClosePanels();
	}

	private void Explorer(object sender, ItemClickEventArgs e)
	{
		ShowNavigatorPanel();
	}

	private void Home(object sender, ItemClickEventArgs e)
	{
		if (AllowHomePanel)
		{
			ShowHomePanel();
		}
	}

	public virtual void ReloadControlCaptions()
	{
		MainRibbonPanel.ReloadControlCaptions();
		MainRibbonPanel.ConfigureChangeOnlineStatusButton(DisconnectedAfterLoggingIntoCloud);
		_statusBarNewNotifications.ToolTip = ArmSoft.SME.Common.Resources.Notifications;
		if (RecentsPanel != null)
		{
			RecentsPanel.Caption = ArmSoft.SME.Common.Resources.History;
		}
		_emptyContentNavigatorButton.Content = ArmSoft.SME.Common.Resources.Explorer;
		_emptyContentNavigatorButton.Tag = ArmSoft.SME.Common.Resources.ExpolorerTag;
		_emptyContentHomeButton.Content = ArmSoft.SME.Common.Resources.HomePanel;
		_emptyContentHomeButton.Tag = ArmSoft.SME.Common.Resources.HomePanelTag;
	}

	public RibbonControl MainWindowRibbon()
	{
		return MainRibbonPanel.RibbonControl;
	}

	public void AddPanelToDocumentGroup(DocumentPanel newPanel, bool bOpenAsChild = true)
	{
		if (!DocGroupPanels.IsVisible)
		{
			DocGroupPanels.Visibility = Visibility.Visible;
		}
		if (DocGroupPanels.Items.Contains(newPanel))
		{
			return;
		}
		BaseLayoutItem selectedItem = DocGroupPanels.SelectedItem;
		if (selectedItem == null || !bOpenAsChild)
		{
			DocGroupPanels.Add(newPanel);
			return;
		}
		for (int i = 0; i <= DocGroupPanels.Items.Count - 1; i++)
		{
			if (DocGroupPanels.Items[i] == selectedItem)
			{
				DocGroupPanels.Insert(i + 1, newPanel);
				break;
			}
		}
	}

	public virtual bool CheckOperationAccess()
	{
		throw new NotImplementedException();
	}

	public void ClosePanel(DocumentPanel panel)
	{
		LayoutManager.DockController.Close(panel);
	}

	public void ClosePanels()
	{
		if (MainFunctionsHelper.CheckOpenWindows && MessageBox.Show(ArmSoft.SME.Common.Resources.CloseAllWnds, ArmSoft.SME.Common.Resources.Windows, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
		{
			return;
		}
		List<BaseLayoutItem> list = new List<BaseLayoutItem>();
		PopulatePanelsForClosing(LayoutManager.LayoutRoot, list);
		foreach (FloatGroup floatGroup in LayoutManager.FloatGroups)
		{
			PopulatePanelsForClosing(floatGroup, list);
		}
		foreach (BaseLayoutItem item in list)
		{
			LayoutManager.DockController.Close(item);
		}
		foreach (Window window in Application.Current.Windows)
		{
			bool flag = window != this;
			if (flag)
			{
				flag = ((object)window).GetType().Name != "AdornerLayerWindow";
			}
			if (flag)
			{
				window.Close();
			}
		}
	}

	public DataViewPanel FindDataViewPanel(int id)
	{
		return FindDataViewPanelInner(id);
	}

	public virtual string GetDocumentDescription(Guid exceptionDocISN)
	{
		throw new NotImplementedException();
	}

	public virtual void InitSpreadsheetFunctions(CustomFunctionCollection customFunctionCollection)
	{
		throw new NotImplementedException();
	}

	protected virtual void SetTitle(string dbName)
	{
		if (string.IsNullOrWhiteSpace(dbName))
		{
			Title = Settings.ProgramName;
		}
		else
		{
			Title = dbName + " - " + Settings.ProgramName + " " + Settings.AppVersionString;
		}
		if (Settings.IsOffline)
		{
			Title = Title + " (" + ArmSoft.SME.Common.Resources.Offline + ")";
		}
		if (!System.Environment.Is64BitProcess)
		{
			Title += " (x86)";
		}
	}

	public virtual void ShowDocument(Guid docISN, short docType = 0, bool isReadOnly = false, bool forCreateCopy = false, int dvpId = -1)
	{
		throw new NotImplementedException();
	}

	public virtual void ShowHelp(string helpId = "")
	{
		throw new NotImplementedException();
	}

	public virtual void ShowOptionsWindow()
	{
		throw new NotImplementedException();
	}

	public void StatusBarCurrentRow(object msg)
	{
		_statusBarCurrentRowInner.Content = msg;
	}

	public void StatusBarSelectedRows(object msg)
	{
		_statusBarSelectedRowsInner.Content = msg;
	}

	public void PopulatePanelsForClosing(LayoutGroup lg, List<BaseLayoutItem> itemsList)
	{
		foreach (BaseLayoutItem item in lg.Items)
		{
			if (AllowHomePanel && (object)((object)item).GetType() == HomePanel)
			{
				itemsList.Add(item);
				continue;
			}
			if (item is LayoutGroup lg2)
			{
				PopulatePanelsForClosing(lg2, itemsList);
				continue;
			}
			foreach (Type item2 in PanelsForClosing)
			{
				if ((object)((object)item).GetType() == item2)
				{
					itemsList.Add(item);
				}
			}
		}
	}

	public virtual bool IsActiveWindowOptionsWindow(string callingSubName)
	{
		throw new NotImplementedException();
	}

	protected virtual void MainWindowActivated()
	{
		if (_mainWindowBase_Activated_FirstCall)
		{
			Height = ActualHeight;
			Width = ActualWidth;
			_mainWindowBase_Activated_FirstCall = false;
		}
	}

	protected virtual void MainWindowLoaded()
	{
		SetTitle("");
		CommandBinding commandBinding = new CommandBinding(new RoutedCommand
		{
			InputGestures = { (InputGesture)new KeyGesture((Key)95, (ModifierKeys)2) }
		}, ShowNextWindow);
		CommandBindings.Add(commandBinding);
		CommandManager.RegisterClassCommandBinding(typeof(Window), commandBinding);
		commandBinding = new CommandBinding(new RoutedCommand
		{
			InputGestures = { (InputGesture)new KeyGesture((Key)95, (ModifierKeys)6) }
		}, ShowPrevWindow);
		CommandBindings.Add(commandBinding);
		CommandManager.RegisterClassCommandBinding(typeof(Window), commandBinding);
	}

	protected void RegisterNavigatorCommandHotKey(KeyGesture keyGesture, short commandId)
	{
		CommandBinding commandBinding = new CommandBinding(new RoutedCommand
		{
			InputGestures = { (InputGesture)keyGesture }
		}, (object s, ExecutedRoutedEventArgs e) =>
		{
			ExecuteNavigatorCommand(e, commandId);
		});
		CommandBindings.Add(commandBinding);
		CommandManager.RegisterClassCommandBinding(typeof(Window), commandBinding);
	}

	private void ExecuteNavigatorCommand(ExecutedRoutedEventArgs e, short commandId)
	{
		try
		{
			if (!SyncVar && !IsActiveWindowOptionsWindow("ExecuteNavigatorCommand") && Settings.LoginService.IsLoggedIn)
			{
				SyncVar = true;
				if (!Settings.UserAccessService.CanViewNavigatorNode(commandId))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.AccessDenied, 20008);
				}
				NavigationBarsController.ExecuteCommand(commandId);
				SyncVar = false;
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			SyncVar = false;
			e.Handled = true;
		}
	}

	private DataViewPanel FindDataViewPanelInner(int id)
	{
		if (id == -1)
		{
			return null;
		}
		DataViewPanel dataViewPanel = FindDataViewPanelInner(id, LayoutManager.LayoutRoot);
		if (dataViewPanel == null)
		{
			foreach (FloatGroup floatGroup in LayoutManager.FloatGroups)
			{
				dataViewPanel = FindDataViewPanelInner(id, floatGroup);
				if (dataViewPanel != null)
				{
					break;
				}
			}
		}
		return dataViewPanel;
	}

	private static DataViewPanel FindDataViewPanelInner(int id, LayoutGroup lg)
	{
		DataViewPanel dataViewPanel = null;
		foreach (BaseLayoutItem item in lg.Items)
		{
			if (item is DataViewPanel dataViewPanel2)
			{
				if (dataViewPanel2.Id == id)
				{
					return dataViewPanel2;
				}
			}
			else if (item is LayoutGroup lg2)
			{
				dataViewPanel = FindDataViewPanelInner(id, lg2);
				if (dataViewPanel != null)
				{
					return dataViewPanel;
				}
			}
		}
		return dataViewPanel;
	}

	private void CloseAllPanels()
	{
		List<BaseLayoutItem> list = new List<BaseLayoutItem>();
		PopulatePanelsForClosing(LayoutManager.LayoutRoot, list);
		foreach (FloatGroup floatGroup in LayoutManager.FloatGroups)
		{
			PopulatePanelsForClosing(floatGroup, list);
		}
		foreach (BaseLayoutItem item in list)
		{
			LayoutManager.DockController.Close(item);
		}
		foreach (Window window in Application.Current.Windows)
		{
			bool flag = window != this;
			if (flag)
			{
				flag = ((object)window).GetType().Name != "AdornerLayerWindow";
			}
			if (flag)
			{
				window.Close();
			}
		}
		NavigatorPanel.Close();
		LayoutManager.AutoHideGroups.Clear();
		if (RecentsPanel != null)
		{
			RecentsPanel.ClearList(null, null);
			LayoutManager.DockController.Close(RecentsPanel);
		}
		if (AllowHomePanel)
		{
			LayoutManager.DockController.Close(HomePanel);
		}
	}

	private LayoutPanel CreateEmptyContent()
	{
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		_emptyContent = new LayoutPanel
		{
			Name = "EmptyContent",
			IsActive = true,
			AllowDock = true,
			AllowDockToDocumentGroup = true,
			ShowCaption = false,
			ShowCaptionImage = false,
			ShowExpandButton = false,
			MinWidth = 500.0,
			Visibility = Visibility.Collapsed,
			ShowCloseButton = false,
			ShowBorder = true,
			AllowFloat = false,
			AllowDrop = false,
			AllowDrag = false,
			AllowHide = false,
			AllowMove = false,
			AllowMaximize = false,
			AllowMinimize = false,
			AllowClose = false,
			AllowContextMenu = false
		};
		Grid grid = new Grid
		{
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalAlignment = HorizontalAlignment.Center,
			Visibility = Visibility.Visible,
			Width = 450.0
		};
		grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition
		{
			Height = new GridLength(1.0, GridUnitType.Star)
		});
		_emptyContentNavigatorButton = new SimpleButton
		{
			IsTabStop = false,
			Glyph = ImageService.GetImage("pane_expand_32.svg"),
			GlyphWidth = 0.0,
			ContentTemplate = (DataTemplate)Settings.GenericResources["EmptyContentTemplate"],
			Margin = new Thickness(0.0, 0.0, 0.0, 12.0)
		};
		Grid.SetRow(_emptyContentNavigatorButton, 0);
		grid.Children.Add(_emptyContentNavigatorButton);
		_emptyContentNavigatorButton.Click += new SafeEventHandler<RoutedEventArgs>(EmptyContentPanel_NavigatorButton_Click).Handler;
		if (AllowHomePanel)
		{
			_emptyContentHomeButton = new SimpleButton
			{
				IsTabStop = false,
				GlyphWidth = 0.0,
				Glyph = ImageService.GetImage("home_32.svg"),
				ContentTemplate = (DataTemplate)Settings.GenericResources["EmptyContentTemplate"]
			};
			Grid.SetRow(_emptyContentHomeButton, 1);
			grid.Children.Add(_emptyContentHomeButton);
			_emptyContentHomeButton.Click += new SafeEventHandler<RoutedEventArgs>(EmptyContentPanel_HomeButton_Click).Handler;
		}
		_emptyContent.Content = grid;
		return _emptyContent;
	}

	private void EmptyContentPanel_HomeButton_Click(object sender, RoutedEventArgs e)
	{
		ShowHomePanel();
		e.Handled = true;
	}

	private void EmptyContentPanel_NavigatorButton_Click(object sender, RoutedEventArgs e)
	{
		ShowNavigatorPanel();
		e.Handled = true;
	}

	private void ShowDates(object sender, ItemClickEventArgs e)
	{
		ShowReportDates();
	}

	private void MainWindow_Activated(object sender, EventArgs e)
	{
		MainWindowActivated();
	}

	private void MainWindow_Loaded(object sender, RoutedEventArgs e)
	{
		MainWindowLoaded();
	}

	private void ShowNextWindow(object sender, ExecutedRoutedEventArgs e)
	{
		if (!SyncVar && !IsActiveWindowOptionsWindow("ShowNextWindow"))
		{
			SyncVar = true;
			ShowNextPrevWindow(moveForward: true);
			SyncVar = false;
			e.Handled = true;
		}
	}

	private void ShowPrevWindow(object sender, ExecutedRoutedEventArgs e)
	{
		if (!SyncVar && !IsActiveWindowOptionsWindow("ShowPrevWindow"))
		{
			SyncVar = true;
			ShowNextPrevWindow(moveForward: false);
			SyncVar = false;
			e.Handled = true;
		}
	}

	private void ShowNextPrevWindow(bool moveForward)
	{
		List<BaseLayoutItem> list = new List<BaseLayoutItem>();
		PopulatePanelsForClosing(LayoutManager.LayoutRoot, list);
		foreach (FloatGroup floatGroup in LayoutManager.FloatGroups)
		{
			PopulatePanelsForClosing(floatGroup, list);
		}
		if (AllowHomePanel && HomePanel != null && HomePanel.IsVisible)
		{
			list.Add(HomePanel);
		}
		if (RecentsPanel != null)
		{
			list.Add(RecentsPanel);
		}
		for (int i = 0; i <= list.Count - 1; i++)
		{
			if (list[i].IsActive)
			{
				BaseLayoutItem baseLayoutItem;
				if (moveForward)
				{
					baseLayoutItem = ((i != list.Count - 1) ? list[i + 1] : list[0]);
				}
				else if (i == 0)
				{
					baseLayoutItem = list[list.Count - 1];
				}
				else
				{
					baseLayoutItem = list[i - 1];
				}
				if (baseLayoutItem.IsFloating)
				{
					baseLayoutItem.IsActive = true;
				}
				else
				{
					LayoutManager.DockController.Activate(baseLayoutItem);
				}
				if (baseLayoutItem is DataViewPanel dataViewPanel)
				{
					dataViewPanel.SetFocus();
					break;
				}
				FocusManager.SetFocusedElement((DependencyObject)(object)this, baseLayoutItem);
				Keyboard.Focus(baseLayoutItem);
				break;
			}
		}
	}

	private void AutoBackup()
	{
		if (AutoBackupWindow != null)
		{
			return;
		}
		List<string> list = new List<string>();
		DatabaseInfo cloudDatabase = DatabaseHelper.GetCloudDatabase(Settings.LoginService.DatabaseId);
		if (cloudDatabase != null)
		{
			if (cloudDatabase.LastBackup.HasValue)
			{
				if (!(DateTime.Now.Date - cloudDatabase.LastBackup.Value.Date < new TimeSpan(Settings.ParametersService.AutoBackUpInterval(), 0, 0, 0)))
				{
					list.Add(cloudDatabase.DbName);
				}
			}
			else
			{
				list.Add(cloudDatabase.DbName);
			}
		}
		if (list.Count > 0)
		{
			BackgroundBackupWindow.TransferModes mode = (Settings.ParametersService.OnlineBackUp() ? BackgroundBackupWindow.TransferModes.DropBox : ((!Settings.ParametersService.LocalBackUp()) ? BackgroundBackupWindow.TransferModes.None : BackgroundBackupWindow.TransferModes.LocalDirectory));
			AutoBackupWindow = new BackgroundBackupWindow(list, mode);
			AutoBackupWindow.Show();
		}
	}

	private void BLogin_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (AutoBackupWindow != null)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.BackupIsNotYetComplete);
		}
		if (Settings.LoginService.IsLoggedIn)
		{
			if (MainFunctionsHelper.CheckOpenWindows && MessageBox.Show(ArmSoft.SME.Common.Resources.ConfirmLogout, ArmSoft.SME.Common.Resources.Logout, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
			{
				e.Handled = true;
				return;
			}
			LogOut(bSaveSettings: true);
		}
		LogIn(isOnline: true);
	}

	private void BLoginOffline_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (AutoBackupWindow != null)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.BackupIsNotYetComplete);
		}
		if (Settings.LoginService.IsLoggedIn)
		{
			if (MainFunctionsHelper.CheckOpenWindows && MessageBox.Show(ArmSoft.SME.Common.Resources.ConfirmLogout, ArmSoft.SME.Common.Resources.Logout, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
			{
				e.Handled = true;
				return;
			}
			LogOut(bSaveSettings: false);
		}
		LogIn(isOnline: false);
	}

	private void BLogout_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (AutoBackupWindow != null)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.BackupIsNotYetComplete);
		}
		if (!MainFunctionsHelper.CheckOpenWindows || MessageBox.Show(ArmSoft.SME.Common.Resources.ConfirmLogout, ArmSoft.SME.Common.Resources.Logout, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
		{
			LogOut(bSaveSettings: true);
		}
	}

	public virtual void LogOut(bool bSaveSettings)
	{
		if (!Settings.LoginService.IsLoggedIn)
		{
			return;
		}
		try
		{
			StatisticsProcessorBase.SendData();
		}
		catch (Exception)
		{
		}
		StatisticsProcessorDispose();
		Settings.InternetBankService.Dispose();
		Settings.OpenBankingService.Dispose();
		Settings.TaxService.Dispose();
		Settings.ServiceProvider.GetRequiredService<SQLiteStorageService>().Dispose();
		NotificationsProcessorDispose();
		SynchronizationsProcessorDispose();
		OfflineSynchronizationsProcessorDispose();
		if (bSaveSettings)
		{
			SaveSettings();
			string value = NavigatorPanel.CollectNodesState();
			if (!string.IsNullOrWhiteSpace(value) && !Settings.IsOffline)
			{
				Settings.ParametersService.NavigatorState_set(value, Settings.LoginService.UserId);
			}
			MainRibbonPanel.SaveFavoritesRibbon();
			ResponseOnLogOut?.Invoke(this);
		}
		if (!Settings.IsOffline && (Settings.SystemType == SystemTypes.Trade || Settings.SystemType == SystemTypes.Accountant || Settings.SystemType == SystemTypes.SalesManagement))
		{
			if (ConfigHelper.ASAdminLogin)
			{
				CloudDBUserLogCreationRequest request = new CloudDBUserLogCreationRequest
				{
					AdToken = ADHelper.ASAdminB2CToken,
					ProcessId = Settings.ProcessId(),
					Station = System.Environment.MachineName,
					DatabaseId = Settings.LoginService.DatabaseId,
					Key = Settings.LoginService.Key,
					UserId = Settings.LoginService.UserId,
					ASUser = Settings.LoginService.ASAdminUserName,
					Action = CloudDBUserLogAction.Logout
				};
				Settings.CreateApiClient().AdminCloud.AddCloudDatabaseUserLog(request);
			}
			else
			{
				using CloudClient cloudClient = Settings.CloudClient;
				UserLogoutRequest request2 = new UserLogoutRequest
				{
					ProcessId = Settings.ProcessId(),
					Workstation = System.Environment.MachineName,
					DBId = Settings.LoginService.DatabaseId,
					Key = Settings.LoginService.Key
				};
				cloudClient.AS7X.UserLogout(request2);
			}
		}
		_statusBarCompanyNameInner.Content = "";
		_statusBarWorkPeriod.Content = "";
		Settings.ConfigureLogger();
		Settings.TaxServiceHelper.LogOut();
		LogOutApp();
		ClearUserData();
		Settings.ClearApplicationToken();
		Settings.LoginService.Logout();
		DisconnectedAfterLoggingIntoCloud = false;
		SetTitle("");
		CloseAllPanels();
		SetBarsStates();
		MainRibbonPanel.RibbonPageFavorites.IsEnabled = false;
	}

	private static void LoadUserData()
	{
		new CacheProgress().ShowMultilineProgressWindow();
		Settings.ServiceProvider.GetRequiredService<SQLiteStorageService>().StoreInUserBasedLocalStorage = true;
		List<short> cacheIds = (from p in Settings.CacheFunctionsService.CacheDefinitions.Values
			where p.StoreInUserBasedLocalStorage
			select p.CacheId).ToList();
		Settings.CacheFunctionsService.RefreshCache(out var _, cacheIds);
	}

	protected virtual void ClearUserData()
	{
		Settings.ParametersService.Clear();
		Settings.UserAccessService.Clear();
		Settings.CoreCacheService.Clear();
		CallsRecordingHelper.Clear();
	}

	protected virtual string GetKey()
	{
		return string.Empty;
	}

	public virtual void RefreshWorkPeriodStatusBar()
	{
	}

	public virtual void RefreshStatusBarBankStatuses()
	{
	}

	public virtual string GetKeyInfo()
	{
		return "";
	}

	public virtual void InnerLogIn()
	{
	}

	public virtual void LogIn(bool isOnline)
	{
		try
		{
			if (isOnline)
			{
				CloudLoginWindow cloudLoginWindow = new CloudLoginWindow();
				cloudLoginWindow.ShowDialog();
				if (cloudLoginWindow.DialogResult != true)
				{
					SetBarsStates();
					Settings.ConfigureLogger();
					return;
				}
				Settings.ConfigureLogger();
			}
			else
			{
				OfflineLoginWindow offlineLoginWindow = new OfflineLoginWindow();
				offlineLoginWindow.ShowDialog();
				if (!offlineLoginWindow.DialogResult.Value)
				{
					SetBarsStates();
					Settings.ConfigureLogger();
					return;
				}
			}
			if (ConfigHelper.ASAdminLogin && Settings.IsBetaVersion && !Settings.LoginService.EdgeServiceAddress.StartsWith("https://smetest.armsoft.am"))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.LoginNotAllowedInBeta);
			}
			if (Settings.LoginService.DemoMode && Settings.LoginService.ContractExpirationDate.HasValue && (Settings.LoginService.ContractExpirationDate.Value - DateTime.Today).Days <= 0)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.DemoTimeExpired);
			}
			Mouse.OverrideCursor = Cursors.Wait;
			if (!Settings.IsOffline)
			{
				SQLiteStorageService requiredService = Settings.ServiceProvider.GetRequiredService<SQLiteStorageService>();
				requiredService.StoreInUserBasedLocalStorage = false;
				if (Settings.LoginService.CacheStructChanged)
				{
					requiredService.DeleteDatabase();
				}
				_appUpdateService.TryUpdateApp();
				LoadUserData();
			}
			Settings.HostingEnvironment.StatusBarCompanyName = Settings.ParametersService.CompanyName();
			Settings.HostingEnvironment.SetTitle(Settings.LoginService.Database);
			SetKeySerialStatusBarMode();
			_timerPing.IsEnabled = true;
			OnlineLicenseRenewResult = true;
			OnlineLicenseRenewMessage = "";
			InnerLogIn();
			SetPanelsState();
			try
			{
				MainRibbonPanel.LoadFavoriteRibbon(UserAccessManager, NavigationBarsController);
			}
			catch (Exception)
			{
			}
			finally
			{
				MainRibbonPanel.RibbonPageFavorites.IsEnabled = true;
			}
			if (NavigatorPanel.NavPanel != null)
			{
				LayoutManager.Activate(NavigatorPanel.NavPanel);
				try
				{
					NavigatorPanel.LoadNavigatorNodes();
				}
				catch (Exception)
				{
				}
			}
			if (!Settings.IsOffline && !InformationWindow.InfoIsActive())
			{
				OpenContractWindow();
			}
			try
			{
				if (!Settings.IsOffline && Settings.ParametersService.AutoBackUp())
				{
					AutoBackup();
				}
			}
			catch (Exception)
			{
			}
			SetBarsStates();
			_onlineLicenseTimer = new Timer(OnlineLicenseTimerEventHandler, null, TimerStart.OnlineLicense, TimerInterval.OnlineLicense);
			_notificationsTimer = new Timer(NotificationsTimerEventHandler, null, TimerStart.Notifications, new TimeSpan(0, 0, 0, Settings.ParametersService.NotificationsCheckInterval()));
			_notificationsSemaphore = new SemaphoreSlim(1);
			_synchronizationsTimer = new Timer(SynchronizationsTimerEventHandler, null, TimerStart.Synchronization, TimerInterval.Synchronization);
			_synchronizationsSemaphore = new SemaphoreSlim(1);
			if (Settings.OfflineModeIsAvailable)
			{
				_offlineDocsSynchronizationsTimer = new Timer(OfflineDocsSynchronizationsTimerEventHandler, null, TimerStart.OfflineDocsSynchronization, TimerInterval.OfflineDocsSynchronization);
				_offlineDocsSynchronizationsSemaphore = new SemaphoreSlim(1);
			}
			_statisticsTimer = new Timer(StatisticsTimerEventHandler, null, TimerStart.Statistics, TimerInterval.Statistics);
			_statisticsSemaphore = new SemaphoreSlim(1);
			UpdateEmptyContentVisibility();
		}
		catch (Exception ex4)
		{
			LogOut(bSaveSettings: false);
			Settings.ErrorHandler(ex4);
		}
		Mouse.OverrideCursor = null;
	}

	protected void OpenContractWindow()
	{
		try
		{
			if (Settings.SystemType != SystemTypes.Trade)
			{
				return;
			}
			Mouse.OverrideCursor = null;
			if (!Settings.LoginService.ContractExpirationDate.HasValue)
			{
				return;
			}
			Settings.LoginService.ContractRemainingSupportDays = (Settings.LoginService.ContractExpirationDate.Value - DateTime.Today).Days;
			using CloudClient cloudClient = Settings.CloudClient;
			cloudClient.Token = Settings.GetApplicationToken();
			ContractBalance contractBalance = cloudClient.AS7X.GetContractBalance();
			if (contractBalance != null)
			{
				Settings.LoginService.ContractNexPayment = contractBalance.NextPayment - contractBalance.Balance;
			}
			decimal? contractNexPayment = Settings.LoginService.ContractNexPayment;
			if (((contractNexPayment.GetValueOrDefault() > 0m) & contractNexPayment.HasValue) && Settings.LoginService.ContractRemainingSupportDays <= 9 && Settings.LoginService.ContractRemainingSupportDays > 0)
			{
				new ContractInformationWindow(Settings.LoginService.ContractRemainingSupportDays, Settings.LoginService.ContractNexPayment.Value, CallBackForCreatePayOrderDelegate).ShowDialog();
			}
		}
		catch (Exception exception)
		{
			Log.Error(exception, "Contract balance error");
		}
	}

	public virtual void LogInOffline()
	{
	}

	public virtual void LogOutApp()
	{
		if (_onlineLicenseTimer != null)
		{
			_onlineLicenseTimer.Dispose();
			_onlineLicenseTimer = null;
		}
		if (Settings.LoginService != null)
		{
			if (!Settings.IsOffline && !ConfigHelper.ASAdminLogin)
			{
				using CloudClient cloudClient = Settings.CloudClient;
				cloudClient.AS7X.FreeOnlineLicense(Settings.LoginService.LicenseId, Settings.ProcessId(), out var _);
			}
			_timerPing.IsEnabled = false;
		}
		_emptyContent.Visibility = Visibility.Collapsed;
	}

	private void TimerEventHandler(object sender, EventArgs e)
	{
		try
		{
			if (OnlineLicenseRenewResult)
			{
				return;
			}
			bool flag = false;
			if ((DateTime.Now - _onlineLicenseRenewDate).TotalHours > (double)_onlineLicenseFailedRenewInH)
			{
				try
				{
					Guid licenseId = Guid.Empty;
					string message = "";
					int status = 0;
					int sessionID = 0;
					Settings.ProcessIdToSessionId(Settings.ProcessId(), ref sessionID);
					using CloudClient cloudClient = Settings.CloudClient;
					cloudClient.AS7X.GetOnlineLicense(Settings.LoginService.Key, Settings.LoginService.ServerName, Settings.LoginService.Database, System.Environment.MachineName + "/" + Settings.GetHostName(), Settings.AppVersionString, sessionID, Settings.ProcessId(), ref licenseId, ref message, ref status);
					Settings.LoginService.LicenseId = licenseId;
					flag = true;
				}
				catch
				{
				}
			}
			if (!flag)
			{
				_timerPing.IsEnabled = false;
				MessageBox.Show(OnlineLicenseRenewMessage, ArmSoft.SME.Common.Resources.ErrorWord, MessageBoxButton.OK, MessageBoxImage.Hand);
				LogOut(bSaveSettings: false);
				Close();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OnlineLicenseTimerEventHandler(object stateInfo)
	{
		try
		{
			if (Settings.IsOffline || ConfigHelper.ASAdminLogin)
			{
				return;
			}
			using CloudClient cloudClient = Settings.CloudClient;
			OnlineLicenseRenewResult = cloudClient.AS7X.RenewOnlineLicense(Settings.LoginService.LicenseId, Settings.ProcessId(), out OnlineLicenseRenewMessage);
			if (OnlineLicenseRenewResult)
			{
				_onlineLicenseRenewDate = DateTime.Now;
			}
		}
		catch (Exception)
		{
		}
	}

	protected void SetPanelsState()
	{
		using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey, writable: false))
		{
			string value = "1";
			string tag = "Home";
			bool isMinimized;
			bool flag;
			bool isSimplified;
			if (registryKey == null)
			{
				isMinimized = false;
				flag = true;
				isSimplified = false;
			}
			else
			{
				if (AllowHomePanel)
				{
					value = Convert.ToString(registryKey.GetValue("HomePageState", "1"));
					tag = Convert.ToString(registryKey.GetValue("HomePageTag", "Home"));
				}
				isMinimized = (string)registryKey.GetValue("RibbonMinimized", "0") == "1";
				flag = (string)registryKey.GetValue("RibbonToolbarMode", "1") == "1";
				isSimplified = (string)registryKey.GetValue("RibbonSimplified", "0") == "1";
			}
			MainRibbonPanel.RibbonControl.IsMinimized = isMinimized;
			MainRibbonPanel.RibbonControl.ToolbarShowMode = ((!flag) ? RibbonQuickAccessToolbarShowMode.ShowBelow : RibbonQuickAccessToolbarShowMode.Default);
			MainRibbonPanel.RibbonControl.IsSimplified = isSimplified;
			if (AllowHomePanel)
			{
				HomePanel.Tag = tag;
				if (Convert.ToBoolean(Convert.ToInt16(value)))
				{
					ShowHomePanel();
				}
				else
				{
					HomePanel.Visibility = Visibility.Collapsed;
				}
			}
		}
		if (Settings.SystemType == SystemTypes.Cabinet)
		{
			return;
		}
		NavigatorPanel.FirstCall = true;
		ShowNavigatorPanel();
		ShowHistoryPanel();
		SetBarsStates();
		List<BaseLayoutItem> list = NavigatorGroup.Items.ToList();
		NavigatorGroup.Items.Clear();
		foreach (BaseLayoutItem item in list)
		{
			NavigatorGroup.Items.Add(item);
		}
	}

	private void SaveSettings()
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
		if (registryKey == null)
		{
			return;
		}
		registryKey.SetValue("RibbonMinimized", MainRibbonPanel.RibbonControl.IsMinimized ? "1" : "0");
		if (MainRibbonPanel.RibbonControl.ToolbarShowMode == RibbonQuickAccessToolbarShowMode.Default)
		{
			registryKey.SetValue("RibbonToolbarMode", "1");
		}
		else if (MainRibbonPanel.RibbonControl.ToolbarShowMode == RibbonQuickAccessToolbarShowMode.ShowBelow)
		{
			registryKey.SetValue("RibbonToolbarMode", "0");
		}
		registryKey.SetValue("RibbonSimplified", MainRibbonPanel.RibbonControl.IsSimplified ? "1" : "0");
		if (AllowHomePanel && HomePanel != null)
		{
			registryKey.SetValue("HomePageState", (HomePanel.Visibility == Visibility.Visible) ? "1" : "0");
			if (HomePanel.Tag != null)
			{
				registryKey.SetValue("HomePageTag", HomePanel.Tag.ToString());
			}
		}
		NavigatorPanel.SaveSettings();
		SaveHistoryPanelState();
	}

	private void Exit_ItemClick(object sender, ItemClickEventArgs e)
	{
		Close();
	}

	private void MainWindow_Closed(object sender, EventArgs e)
	{
		Settings.InitHostingEnvironment(null);
	}

	private void OptionsWindow_ItemClick(object sender, ItemClickEventArgs e)
	{
		MainRibbonPanel.AppMenu.Visibility = Visibility.Collapsed;
		ShowOptionsWindow();
	}

	private void ExplorerPanelBarItem_ItemClick(object sender, ItemClickEventArgs e)
	{
		ShowNavigatorPanel();
		e.Handled = true;
	}

	private void RecentPanelBarItem_ItemClick(object sender, ItemClickEventArgs e)
	{
		ShowRecentPanel();
		e.Handled = true;
	}

	private void HomePanelItem_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (AllowHomePanel)
		{
			ShowHomePanel();
		}
	}

	private void ButtonChangeOnlineStatus_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (!Settings.IsOffline)
		{
			DisconnectedAfterLoggingIntoCloud = true;
		}
		if (OnOnlineStatusChange(!Settings.IsOffline))
		{
			Settings.IsOffline = !Settings.IsOffline;
		}
		SetBarsStates();
	}

	protected virtual bool OnOnlineStatusChange(bool newOnlineStatus)
	{
		return true;
	}

	public virtual void ShowHomePanel()
	{
		if (!DocGroupPanels.IsVisible)
		{
			DocGroupPanels.Visibility = Visibility.Visible;
		}
	}

	private void ShowNavigatorPanel()
	{
		AddNavigatorGroup();
		if (NavigatorPanel.NavPanel == null)
		{
			NavigatorPanel.Restore(considerPanelState: false);
		}
		else if (NavigatorPanel.NavPanel.Parent == null)
		{
			if (RecentsPanel?.Parent != null)
			{
				LayoutManager.DockController.Dock(NavigatorPanel.NavPanel, RecentsPanel, DockType.Fill);
			}
			else
			{
				NavigatorGroup.Items.Add(NavigatorPanel.NavPanel);
			}
		}
		TreeViewControl value = (TreeViewControl)NavigatorPanel.NavPanel.NavBarControl.ActiveGroup.Content;
		FocusManager.SetFocusedElement((DependencyObject)(object)this, value);
		LayoutManager.DockController.Activate(NavigatorPanel.NavPanel);
	}

	private void ShowRecentPanel()
	{
		if (!MainRibbonPanel.RecentPanelBarItemLink.IsVisible)
		{
			return;
		}
		if (RecentsPanel == null)
		{
			RecentsPanel = new HistoryPanel(RecentsController)
			{
				Categories = InitHistoryCategories(),
				AllowFloat = false,
				AllowMove = false,
				AllowClose = true
			};
		}
		if (RecentsPanel.Parent != null)
		{
			LayoutManager.Activate(RecentsPanel);
			if (RecentsPanel.xGrid.VisibleRowCount > 0)
			{
				FocusManager.SetFocusedElement((DependencyObject)(object)RecentsPanel, RecentsPanel.xGrid);
				RecentsPanel.xGrid.SelectedItem = RecentsPanel.xGrid.CurrentItem;
			}
			return;
		}
		AddNavigatorGroup();
		if (NavigatorPanel.NavPanel != null && NavigatorPanel.NavPanel.Parent != null)
		{
			LayoutManager.DockController.Dock(RecentsPanel, NavigatorPanel.NavPanel, DockType.Fill);
		}
		else
		{
			NavigatorGroup.Items.Add(RecentsPanel);
		}
		LayoutManager.Activate(RecentsPanel);
		if (RecentsPanel.xGrid.VisibleRowCount > 0)
		{
			FocusManager.SetFocusedElement((DependencyObject)(object)RecentsPanel, RecentsPanel.xGrid);
			RecentsPanel.xGrid.SelectedItem = RecentsPanel.xGrid.CurrentItem;
		}
	}

	private void AddNavigatorGroup()
	{
		List<BaseLayoutItem> list = NavigatorGroup.Items.ToList();
		NavigatorGroup.Items.Clear();
		foreach (BaseLayoutItem item in list)
		{
			NavigatorGroup.Items.Add(item);
		}
		if (NavigatorGroup == null)
		{
			NavigatorGroup = new TabbedGroup
			{
				AllowDrop = false,
				AllowMove = false,
				ClosingBehavior = ClosingBehavior.ImmediatelyRemove
			};
		}
		if (NavigatorGroup.Parent == null)
		{
			MainGroup.Items.Insert(0, NavigatorGroup);
		}
		if (NavigatorPanel.NavPanel == null && RecentsPanel == null)
		{
			NavigatorGroup.ItemWidth = new GridLength(200.0, GridUnitType.Pixel);
		}
		NavigatorGroup.AllowFloat = false;
		NavigatorGroup.AllowMove = false;
		if (AutoHideLeft.Parent == null && !LayoutManager.AutoHideGroups.Any((AutoHideGroup p) => p.Name == "AutoHideLeft"))
		{
			AutoHideLeft = new AutoHideGroup
			{
				DockType = Dock.Left,
				AllowFloat = false,
				AllowMove = false,
				Name = "AutoHideLeft",
				ClosingBehavior = ClosingBehavior.ImmediatelyRemove
			};
			LayoutManager.AutoHideGroups.Add(AutoHideLeft);
		}
	}

	private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Invalid comparison between Unknown and I4
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Invalid comparison between Unknown and I4
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Invalid comparison between Unknown and I4
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Invalid comparison between Unknown and I4
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Invalid comparison between Unknown and I4
		if (!Settings.LoginService.IsLoggedIn && ((int)Keyboard.Modifiers != 2 || ((int)e.Key != 101 && (int)e.Key != 98)))
		{
			e.Handled = true;
		}
		else if ((int)Keyboard.Modifiers == 2 && (int)e.Key == 58)
		{
			ShowOptionsWindow();
			e.Handled = true;
		}
		else if ((int)Keyboard.Modifiers == 2 && (int)e.Key == 99)
		{
			if (MainRibbonPanel.RecentPanelBarItemLink.IsVisible)
			{
				ShowRecentPanel();
			}
			e.Handled = true;
		}
		else if ((int)Keyboard.Modifiers == 2 && (int)e.Key == 100)
		{
			if (MainRibbonPanel.ExplorerPanelBarItemLink.IsVisible)
			{
				ShowNavigatorPanel();
			}
			e.Handled = true;
		}
		else if ((int)Keyboard.Modifiers == 0 && (int)e.Key == 100)
		{
			if (AllowHomePanel && MainRibbonPanel.HomePanelItemLink.IsVisible)
			{
				ShowHomePanel();
			}
			e.Handled = true;
		}
		else if ((int)Keyboard.Modifiers == 2 && (int)e.Key == 3)
		{
			e.Handled = true;
		}
	}

	public virtual void SetBarsStates(object sender = null)
	{
		MainRibbonPanel.SetBarsStates(AllowHomePanel, _appUpdateService);
		MainRibbonPanel.ConfigureChangeOnlineStatusButton(DisconnectedAfterLoggingIntoCloud);
		SetTitle(Settings.LoginService.Database);
	}

	private void LayoutManager_DockItemActivating(object sender, ItemCancelEventArgs e)
	{
		_currentPanel = e.Item;
		if (e.Item is DataViewPanel dataViewPanel)
		{
			CurrentViewPanel = dataViewPanel;
			_currentPanel = dataViewPanel;
			SetBarsStates(dataViewPanel);
		}
		else if (e.Item is AsReportViewer asReportViewer)
		{
			CurrentViewPanel = null;
			_currentPanel = asReportViewer;
			SetBarsStates(asReportViewer);
		}
		else if (e.Item is HistoryPanel)
		{
			RecentsController.NewItemsMode = false;
		}
	}

	private void LayoutManager_DockItemClosing(object sender, ItemCancelEventArgs e)
	{
		try
		{
			if (AllowHomePanel && e.Item == HomePanel)
			{
				e.Item.Visibility = Visibility.Collapsed;
				e.Cancel = false;
				return;
			}
			if (e.Item == NavigatorPanel.NavPanel)
			{
				NavigatorPanel.NavPanel = null;
				return;
			}
			if (e.Item == RecentsPanel)
			{
				RecentsPanel.UpdateItems();
			}
			if (_currentPanel == e.Item)
			{
				_currentPanel = null;
			}
			if (CurrentViewPanel == e.Item)
			{
				CurrentViewPanel.IDV.StopExecution();
				CurrentViewPanel = null;
			}
			SetBarsStates();
		}
		catch (Exception exception)
		{
			Log.Warning(exception, "LayoutManager.DockItemClosing: ");
		}
	}

	private void UpdateEmptyContentVisibility()
	{
		if (Settings.LoginService.IsLoggedIn)
		{
			bool flag = (from p in LayoutManager.GetItems()
				where !(p is LayoutGroup) && !(p is TabbedGroup) && p.Name != "EmptyContent"
				select p).Any((BaseLayoutItem p) => !(p is HistoryPanel) && !(p is NavigatorPanel));
			_emptyContent.Visibility = (flag ? Visibility.Collapsed : Visibility.Visible);
			DocGroupPanels.Visibility = ((!flag) ? Visibility.Collapsed : Visibility.Visible);
		}
	}

	private void CopyKeySerial(object sender, ItemClickEventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (_statusBarKeySerial.IsVisible)
		{
			string key = Settings.LoginService.Key;
			Clipboard.SetDataObject(key);
			ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
			{
				Image = ImageService.GetImage("accept_32.svg"),
				HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification),
				Caption = ArmSoft.SME.Common.Resources.KeyNumCopedToBufer,
				Text = ArmSoft.SME.Common.Resources.Key + " - " + key
			};
			toastNotificationViewModel.HistoryItem.Description = toastNotificationViewModel.Text;
			toastNotificationViewModel.HistoryItem.Details = toastNotificationViewModel.Text;
			toastNotificationViewModel.HistoryItem.Icon = "accept_16.svg";
			toastNotificationViewModel.HistoryItem.Id = "Key";
			ShowNotificationAsync(toastNotificationViewModel);
		}
	}

	private void StatusBarTaxServiceConnectionState_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (StatusBarTaxServiceConnectionState.IsVisible && MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.DisconnectEInvoicingSystem, Settings.TaxService.Client.Tin), "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
		{
			Settings.TaxServiceHelper.LogOut();
			ArmSoft.SME.Core.UI.Services.TaxService.AuthenticateWithLoginWindow = true;
		}
	}

	public void TestNotification()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
		{
			Image = ImageService.GetImage("accept_32.svg"),
			HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification),
			Caption = "Test notification caption",
			Text = "Test notification text"
		};
		toastNotificationViewModel.HistoryItem.Description = toastNotificationViewModel.Text;
		toastNotificationViewModel.HistoryItem.Details = toastNotificationViewModel.Text;
		toastNotificationViewModel.HistoryItem.Icon = "accept_16.svg";
		toastNotificationViewModel.HistoryItem.Id = "TestNotification";
		ShowNotificationAsync(toastNotificationViewModel);
	}

	private void AddToFavorites_ItemClick(object sender, ItemClickEventArgs e)
	{
		BarButtonItem barButtonItem = (BarButtonItem)sender;
		NavigationCommand ti = (NavigationCommand)barButtonItem.Tag;
		MainRibbonPanel.AddToFavorites_ItemClick(barButtonItem, ti, UserAccessManager);
	}

	private void NewNotifications_ItemClick(object sender, ItemClickEventArgs e)
	{
		ShowRecentPanel();
	}

	private void Ribbon_Drop(object sender, DragEventArgs e)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		if (!e.Data.GetDataPresent("MyTreeViewItem"))
		{
			return;
		}
		TreeListNode treeListNode = (TreeListNode)e.Data.GetData("MyTreeViewItem");
		if (treeListNode.HasChildren)
		{
			e.Handled = true;
			return;
		}
		if (treeListNode != null)
		{
			AsNavigationTreeNode asNavigationTreeNode = (AsNavigationTreeNode)treeListNode.Content;
			NavigationCommand navigationCommand = new NavigationCommand
			{
				Command = asNavigationTreeNode.Command,
				Subsystem = asNavigationTreeNode.Subsystem,
				CommandType = asNavigationTreeNode.CommandType,
				CommandTag = asNavigationTreeNode.CommandTag
			};
			if (navigationCommand.CommandType == AWPCommandType.SystemCall)
			{
				MainRibbonPanel.AddCommandToFavorites(asNavigationTreeNode.FullHeader.ToString(), navigationCommand, ImageService.GetImage(asNavigationTreeNode.Image), UserAccessManager, NavigationBarsController);
			}
		}
		e.Handled = true;
	}

	private void Ribbon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		_activeRibbonPage = MainRibbonPanel.RibbonControl.SelectedPage;
	}

	private void Ribbon_RibbonPopupMenuShowing(object sender, RibbonPopupMenuShowingEventArgs e)
	{
		if (MainRibbonPanel.RibbonPageFavorites.IsSelected)
		{
			BarItemLinkBase link = e.Link;
			if (link != null)
			{
				MainRibbonPanel.RemoveFromFavoritesBarButtonItem.Tag = link;
				e.Menu.ItemLinks.Add(MainRibbonPanel.RemoveFromFavoritesBarButtonItem.CreateLink());
			}
		}
	}

	private void LayoutManager_DockItemHidden(object sender, ItemEventArgs e)
	{
		if (sender is NavigatorPanel navigatorPanel)
		{
			if (navigatorPanel.Parent != AutoHideLeft)
			{
				AutoHideLeft.Insert(0, navigatorPanel);
			}
			if (RecentsPanel != null && RecentsPanel.Parent != null && RecentsPanel.Parent != AutoHideLeft)
			{
				AutoHideLeft.Add(RecentsPanel);
				RecentsPanel.ItemWidth = NavigatorPanel.NavPanel.ItemWidth;
			}
			e.Handled = true;
		}
		else if (sender is HistoryPanel historyPanel)
		{
			if (historyPanel.Parent != AutoHideLeft)
			{
				AutoHideLeft.Add(historyPanel);
			}
			if (NavigatorPanel.NavPanel != null && NavigatorPanel.NavPanel.Parent != null && NavigatorPanel.NavPanel.Parent != AutoHideLeft)
			{
				AutoHideLeft.Insert(0, NavigatorPanel.NavPanel);
				RecentsPanel.ItemWidth = NavigatorPanel.NavPanel.ItemWidth;
			}
			e.Handled = true;
		}
	}

	private void LayoutManager_UnMerge(object sender, BarMergeEventArgs e)
	{
		_statusBarCurrentRowInner.Content = "";
		_statusBarSelectedRowsInner.Content = "";
	}

	private void DocGroupPanels_SelectedItemChanged(object sender, DevExpress.Xpf.Docking.Base.SelectedItemChangedEventArgs e)
	{
		if (e.Item == null)
		{
			return;
		}
		if (e.Item is DataViewPanel dataViewPanel)
		{
			_statusBarSelectedRowsInner.Content = dataViewPanel.StatusBarSelectedRowsContent();
			_statusBarCurrentRowInner.Content = dataViewPanel.StatusBarCurrentRowContent();
			if (dataViewPanel.InnerGrid().VisibleRowCount > 0)
			{
				FocusManager.SetFocusedElement((DependencyObject)(object)this, dataViewPanel.InnerGrid());
			}
		}
		else if (DocGroupPanels.SelectedItem is AsReportViewer asReportViewer)
		{
			_statusBarSelectedRowsInner.Content = asReportViewer.StatusBarSelectedRowsContent();
			_statusBarCurrentRowInner.Content = asReportViewer.StatusBarCurrentRowContent();
		}
	}

	public void WriteInHistory(HistoryItem itm)
	{
		RecentsController.AddItem(itm);
	}

	public virtual void HistoryItemClick(HistoryItem item)
	{
		try
		{
			if (item.Category == HistoryItem.ItemCategory.Notification)
			{
				switch (item.Type)
				{
				case 2:
					_appUpdateService.TryUpdateAppFiles();
					break;
				case 100:
					new RatesOfExchange().Show();
					break;
				}
			}
			else if (item.Category == HistoryItem.ItemCategory.Document)
			{
				switch (item.Type)
				{
				case 101:
					Settings.DocumentService.ShowWindow(Guid.Parse(item.Id), WindowViewMode.Edit);
					break;
				case 102:
					Settings.DirectoryService.ShowWindow(Convert.ToInt16(item.Id.Substring(0, 2)), item.Id.Substring(2), isPOWindow: true, WindowViewMode.Edit);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public List<HistoryCategory> InitHistoryCategories()
	{
		HistoryCategories = new List<HistoryCategory>();
		HistoryCategories.Add(new HistoryCategory
		{
			ID = HistoryItem.ItemCategory.Document,
			Icon = "doc_16.svg",
			ActionName = "View",
			Caption = ArmSoft.SME.Common.Resources.Documents
		});
		HistoryCategories.Add(new HistoryCategory
		{
			ID = HistoryItem.ItemCategory.Notification,
			Icon = "notification_16.svg",
			ActionName = "View",
			Caption = ArmSoft.SME.Common.Resources.Notifications
		});
		return HistoryCategories;
	}

	public virtual void ShowNotificationAsync(ToastNotificationViewModel viewModel)
	{
		if (Settings.ParametersService.UseWindowsNotifications() && System.Environment.OSVersion.Version.Major >= 10)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action<ToastNotificationViewModel>(ShowWindowNotificationAsync), new object[1] { viewModel });
		}
		else
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action<ToastNotificationViewModel>(ShowProgramNotification), new object[1] { viewModel });
		}
	}

	protected void ShowProgramNotification(ToastNotificationViewModel viewModel)
	{
		if (Settings.LoginService.IsLoggedIn)
		{
			if (viewModel.HistoryItem != null)
			{
				WriteInHistory(viewModel.HistoryItem);
			}
			ToastNotificationService.ProgramNotificationService.CreateCustomNotification(viewModel).ShowAsync().ContinueWith((Task<NotificationResult> t) =>
			{
				OnProgramNotificationShown(t, viewModel);
			});
		}
	}

	private void OnProgramNotificationShown(Task<NotificationResult> task, ToastNotificationViewModel model)
	{
		if (!Settings.LoginService.IsLoggedIn)
		{
			return;
		}
		try
		{
			if (task.Result == NotificationResult.Activated && !Convert.ToBoolean(((DispatcherObject)this).Dispatcher.Invoke((Delegate)(Func<bool>)(() => ComponentDispatcher.IsThreadModal), Array.Empty<object>())) && model.HistoryItem != null)
			{
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action<HistoryItem>(HistoryItemClick), new object[1] { model.HistoryItem });
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ShowWindowNotificationAsync(ToastNotificationViewModel viewModel)
	{
		try
		{
			INotification notification = NotificationService().CreatePredefinedNotification(viewModel.Caption, viewModel.Text, "", viewModel.Image);
			ShowWindowNotification(notification, viewModel);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ShowWindowNotification(INotification notification, ToastNotificationViewModel viewModel)
	{
		if (Settings.LoginService.IsLoggedIn)
		{
			if (viewModel.HistoryItem != null)
			{
				WriteInHistory(viewModel.HistoryItem);
			}
			notification.ShowAsync().ContinueWith((Task<NotificationResult> t) =>
			{
				OnWindowNotificationShown(t, viewModel);
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}
	}

	private void OnWindowNotificationShown(Task<NotificationResult> task, ToastNotificationViewModel viewModel)
	{
		if (!Settings.LoginService.IsLoggedIn)
		{
			return;
		}
		try
		{
			if (!task.IsFaulted && task.Result == NotificationResult.Activated && !(bool)((DispatcherObject)this).Dispatcher.Invoke((Delegate)(Func<bool>)(() => ComponentDispatcher.IsThreadModal), Array.Empty<object>()) && viewModel.HistoryItem != null)
			{
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action<HistoryItem>(HistoryItemClick), new object[1] { viewModel.HistoryItem });
			}
		}
		catch (AggregateException)
		{
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
		}
	}

	private NotificationService NotificationService()
	{
		if (_notificationService == null)
		{
			_notificationService = CreateNotificationService();
		}
		return _notificationService;
	}

	private NotificationService CreateNotificationService()
	{
		return new NotificationService
		{
			PredefinedNotificationTemplate = NotificationTemplate.LongHeaderAndShortText,
			PredefinedNotificationDuration = PredefinedNotificationDuration.Default,
			UseWin8NotificationsIfAvailable = true,
			ApplicationActivator = NotificationActivatorType,
			CreateApplicationShortcut = false,
			ApplicationName = Settings.ProgramName,
			ApplicationId = Settings.ApplicationId
		};
	}

	private void RegisterNotificationActivator()
	{
		if (NotificationActivatorType != null && !ShellHelper.IsApplicationShortcutExist(Settings.ProgramName))
		{
			ShellHelper.TryCreateShortcut(Settings.ApplicationId, Settings.ProgramName);
		}
	}

	private void ShowReportDates()
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Common.Resources.Dates,
			Name = "RepDates"
		};
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		DateEditControl dateEditControl = dialogWindow.AddDateControl(container, "DateCurrent", ArmSoft.SME.Common.Resources.CurrentDate, useLongDate: false, isRequired: true);
		dateEditControl.EditValue = CurrentDate;
		dialogWindow.AddSpecialDateButton(container, dateEditControl);
		container = dialogWindow.AddHorizontalLayoutGroup();
		dateEditControl = dialogWindow.AddDateControl(container, "DateStart", ArmSoft.SME.Common.Resources.BeginOfPeriod, useLongDate: false, isRequired: true);
		dateEditControl.EditValue = DateBegin;
		container = dialogWindow.AddHorizontalLayoutGroup();
		DateEditControl dateEditControl2 = dialogWindow.AddDateControl(container, "DateEnd", ArmSoft.SME.Common.Resources.EndOfPeriod, useLongDate: false, isRequired: true);
		dateEditControl2.EditValue = DateEnd;
		dialogWindow.AddSpecialDateButton(container, dateEditControl, dateEditControl2);
		dialogWindow.WindowValidation += ReportDatesValidation;
		if (dialogWindow.ShowDialog().Value)
		{
			CurrentDate = (DateOnly)dialogWindow["DateCurrent"];
			DateBegin = (DateOnly)dialogWindow["DateStart"];
			DateEnd = (DateOnly)dialogWindow["DateEnd"];
		}
	}

	private void ReportDatesValidation(DialogWindow dialog, DialogWindowValidationArgs args)
	{
		try
		{
			ValidationService.ValidateWrongPeriod((DateOnly)dialog["DateStart"], (DateOnly)dialog["DateEnd"]);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
			args.SetFocusWhenInvalidSave = dialog.GetUIElement("DateCurrent");
		}
	}

	public virtual IDataView GetMeasurementUnitsDataView()
	{
		return null;
	}

	public virtual string GetAppKeyCode()
	{
		return string.Empty;
	}

	private void BarButtonRecordUserCalls_CheckedChanged(object sender, ItemClickEventArgs e)
	{
		if (((BarCheckItem)sender).IsChecked.Value)
		{
			if (!CallsRecordingHelper.EditRecordCallsConfig())
			{
				MainRibbonPanel.BarButtonRecordUserCalls.IsChecked = false;
			}
		}
		else
		{
			CallsRecordingHelper.Config = null;
		}
	}

	public virtual void RefreshActualnessDateStatusBar(bool isActualnessDateCorrect, bool isActualnessDateChanged = true)
	{
	}

	private void ShowHistoryPanel()
	{
		if (RegistryHelper.ReadStringFromRegistry("HistoryPanelState", "", "1") == "1")
		{
			ShowRecentPanel();
		}
	}

	private void SaveHistoryPanelState()
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
		if (RecentsPanel != null && RecentsPanel.Parent != null)
		{
			registryKey.SetValue("HistoryPanelState", "1");
		}
		else
		{
			registryKey.SetValue("HistoryPanelState", "0");
		}
	}

	private void MainWindowBase_Closing(object sender, CancelEventArgs e)
	{
		if (AutoBackupWindow != null)
		{
			e.Cancel = true;
			return;
		}
		if (DXSplashScreen.IsActive)
		{
			DXSplashScreen.Close();
		}
		if (Settings.LoginService.IsLoggedIn)
		{
			if (MainFunctionsHelper.CheckOpenWindows)
			{
				if (MessageBox.Show(ArmSoft.SME.Common.Resources.ConfirmCloseAndExit, ArmSoft.SME.Common.Resources.Logout, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
				{
					e.Cancel = true;
					return;
				}
			}
			else if (MessageBox.Show(ArmSoft.SME.Common.Resources.ConfirmExit, ArmSoft.SME.Common.Resources.Logout, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
			{
				e.Cancel = true;
				return;
			}
		}
		try
		{
			LogOut(bSaveSettings: true);
		}
		catch (Exception ex)
		{
			if (!ex.Message.Contains("No connection could be made") && !(ex is ASException) && !(ex is ArmSoft.SME.Common.Exceptions.RESTException) && !(ex is ApiException { IsRestException: not false }))
			{
				Log.Error(ex, ex.Message);
			}
		}
		if (_timerPing != null)
		{
			_timerPing.IsEnabled = false;
		}
		CloseCOMPorts();
	}

	public virtual void InitCOMPorts()
	{
	}

	public virtual void ScannerConnect(SerialPort port = null)
	{
	}

	public virtual void ScannerDisconnect()
	{
	}

	public virtual void MCRConnect(SerialPort port = null)
	{
	}

	public virtual void MCRDisconnect()
	{
	}

	public virtual void CloseCOMPorts()
	{
	}

	public virtual void CloudRemTimerEventHandler(object stateInfo)
	{
	}

	private void SetKeySerialStatusBarMode()
	{
		if (Settings.LoginService.DemoMode && _statusBarKeySerial.ContentTemplate == null)
		{
			_statusBarKeySerial.ContentTemplate = (DataTemplate)Settings.GenericResources["DemoTemplate"];
		}
		else if (!Settings.LoginService.DemoMode && _statusBarKeySerial.ContentTemplate != null)
		{
			_statusBarKeySerial.ContentTemplate = null;
		}
	}

	private void AddHelpHeaderItem()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		DXImage content = new DXImage
		{
			Source = (Settings.IsWin11Theme ? ImageService.GetImage("help_16.svg") : ImageService.GetImage("help_16_black.svg")),
			Width = 16.0,
			Height = 16.0,
			SnapsToDevicePixels = true,
			Stretch = Stretch.Uniform
		};
		_helpHeaderItem = new HeaderItemControl
		{
			Content = content
		};
		HeaderItems.Add(_helpHeaderItem);
	}

	private void AddCloudUserHeaderInfo()
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		StackPanel stackPanel = new StackPanel
		{
			Orientation = Orientation.Horizontal
		};
		_cloudUserName = new TextBlock
		{
			Text = "",
			Margin = new Thickness(0.0, 0.0, 5.0, 0.0)
		};
		stackPanel.Children.Add(_cloudUserName);
		_userImage = new DXImage
		{
			Source = ImageService.GetImage("user_16.svg"),
			Width = 16.0,
			Height = 16.0,
			UseLayoutRounding = true,
			Stretch = Stretch.Uniform
		};
		stackPanel.Children.Add(_userImage);
		_cloudUserHeaderItem = new HeaderItemControl
		{
			Content = stackPanel,
			Visibility = Visibility.Hidden
		};
		HeaderItems.Add(_cloudUserHeaderItem);
	}

	public void SetCloudUserHeaderInfo(string userDescription, string userName)
	{
		if (string.IsNullOrWhiteSpace(userDescription) && string.IsNullOrWhiteSpace(userName))
		{
			_cloudUserHeaderItem.Visibility = Visibility.Hidden;
			return;
		}
		_cloudUserHeaderItem.Visibility = Visibility.Visible;
		if (string.IsNullOrWhiteSpace(userDescription))
		{
			_cloudUserName.Text = userName;
			_cloudUserHeaderItem.ToolTip = userName;
		}
		else
		{
			_cloudUserName.Text = userDescription;
			_cloudUserHeaderItem.ToolTip = userDescription + " (" + userName + ")";
		}
		_userImage.Source = ImageService.GetUserImage(userDescription, userName);
	}

	protected virtual void OfflineDocsSynchronizationsTimerEventHandler(ref string traceLog)
	{
	}

	private void OfflineDocsSynchronizationsTimerEventHandler(object stateInfo)
	{
		try
		{
			string traceLog = "Unknown";
			if (Settings.LoginService.IsLoggedIn && !ConfigHelper.ASAdminLogin && (!Settings.IsOffline || NetworkInterface.GetIsNetworkAvailable()) && OfflineSynchronizationsProcessor != null && AutoBackupWindow == null)
			{
				if (!OfflineSynchronizationsProcessor.IsSynchronizationsEnabled())
				{
					OfflineSynchronizationsProcessorDispose();
				}
				else if (_offlineDocsSynchronizationsSemaphore.Wait(0))
				{
					OfflineDocsSynchronizationsTimerEventHandler(ref traceLog);
				}
			}
		}
		catch
		{
		}
		finally
		{
			if (_offlineDocsSynchronizationsSemaphore != null)
			{
				_offlineDocsSynchronizationsSemaphore.Release();
			}
		}
	}

	protected virtual void SynchronizationsTimerEventHandler(ref string traceLog)
	{
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		if (!SynchronizationsProcessor.TaxServiceAuthenticationDone && !StatusBarTaxServiceConnectionState.IsVisible)
		{
			traceLog = "TaxServiceAuthenticationWithSavedUser";
			Settings.TaxServiceHelper.TaxServiceAuthenticationWithSavedUser(out var _);
			SynchronizationsProcessor.TaxServiceAuthenticationDone = true;
		}
		if (Settings.SystemType != SystemTypes.SalesManagement && SynchronizationsProcessor.ImportExchangeRate && !SynchronizationsProcessor.ExchnageRatesChecked)
		{
			SynchronizationsProcessor.ExchnageRatesChecked = true;
			traceLog = "ImportExchangeRates";
			string message = string.Empty;
			DateOnly dateOnly = DateTime.Today.ToDateOnly();
			DateOnly dateOnly2 = dateOnly.AddDays(-SynchronizationsProcessor.NotificationsCheckInterval);
			short num = SynchronizationsProcessorBase.TryImportExchangeRates(dateOnly2, dateOnly, ref message);
			if (num != 0)
			{
				ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
				{
					Caption = ArmSoft.SME.Common.Resources.CBExchangeRatesShort,
					HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification)
				};
				switch (num)
				{
				case -1:
					toastNotificationViewModel.Image = ImageService.GetImage("error_32.svg");
					toastNotificationViewModel.Text = message;
					toastNotificationViewModel.HistoryItem.Icon = "error_32.svg";
					break;
				case 2:
					toastNotificationViewModel.Image = ImageService.GetImage("accept_32.svg");
					toastNotificationViewModel.Text = ArmSoft.SME.Common.Resources.SuccessfullyImported + " " + dateOnly2.FormatDDMMYY() + " - " + dateOnly.FormatDDMMYY();
					toastNotificationViewModel.HistoryItem.Icon = "accept_32.svg";
					break;
				default:
					toastNotificationViewModel.Image = ImageService.GetImage("warning_32.svg");
					toastNotificationViewModel.Text = ArmSoft.SME.Common.Resources.CBAImportExchangeRatesWarning;
					toastNotificationViewModel.HistoryItem.Icon = "wand_16.svg";
					break;
				}
				toastNotificationViewModel.HistoryItem.Type = 100;
				toastNotificationViewModel.HistoryItem.Description = toastNotificationViewModel.Caption;
				toastNotificationViewModel.HistoryItem.Details = toastNotificationViewModel.Text;
				ShowNotificationAsync(toastNotificationViewModel);
			}
		}
		if (!SynchronizationsProcessor.CPACodesUpdatedChecked)
		{
			SynchronizationsProcessor.CPACodesUpdatedChecked = true;
			if (!SynchronizationsProcessor.CPACodesLastSyncDate.HasValue || SynchronizationsProcessor.CPACodesLastSyncDate < DateTime.Today.ToDateOnly())
			{
				Settings.ParametersService.CPACodesLastSyncDate_set(DateTime.Today.ToDateOnly());
				traceLog = "CPACodesUpdate";
				short num2 = SynchronizationsProcessorBase.TryUpdateCPACodesFromB2B(SynchronizationsProcessor.CPACodesLastSyncDate.Value, out var message2);
				if (num2 != 0)
				{
					ToastNotificationViewModel toastNotificationViewModel2 = new ToastNotificationViewModel
					{
						Caption = ArmSoft.SME.Common.Resources.RefreshCPACodes,
						HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification)
					};
					switch (num2)
					{
					case -1:
						toastNotificationViewModel2.Image = ImageService.GetImage("error_32.svg");
						toastNotificationViewModel2.Text = message2;
						toastNotificationViewModel2.HistoryItem.Icon = "error_32.svg";
						break;
					case 1:
						toastNotificationViewModel2.Image = ImageService.GetImage("accept_32.svg");
						toastNotificationViewModel2.Text = message2;
						toastNotificationViewModel2.HistoryItem.Icon = "accept_32.svg";
						break;
					}
					toastNotificationViewModel2.HistoryItem.Type = 103;
					toastNotificationViewModel2.HistoryItem.Description = toastNotificationViewModel2.Caption;
					toastNotificationViewModel2.HistoryItem.Details = toastNotificationViewModel2.Text;
					ShowNotificationAsync(toastNotificationViewModel2);
				}
			}
		}
		if (SynchronizationsProcessor.BanksUpdatedChecked)
		{
			return;
		}
		SynchronizationsProcessor.BanksUpdatedChecked = true;
		if (SynchronizationsProcessor.BanksLastSyncDate.HasValue && !(SynchronizationsProcessor.BanksLastSyncDate < DateTime.Today.ToDateOnly()))
		{
			return;
		}
		Settings.ParametersService.BanksLastSyncDate_set(DateTime.Today.ToDateOnly());
		traceLog = "BanksUpdate";
		short num3 = SynchronizationsProcessorBase.TryUpdateBanksFromB2B(out var message3);
		if (num3 != 0)
		{
			ToastNotificationViewModel toastNotificationViewModel3 = new ToastNotificationViewModel
			{
				Caption = ArmSoft.SME.Common.Resources.RefreshBanks,
				HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification)
			};
			switch (num3)
			{
			case -1:
				toastNotificationViewModel3.Image = ImageService.GetImage("error_32.svg");
				toastNotificationViewModel3.Text = message3;
				toastNotificationViewModel3.HistoryItem.Icon = "error_32.svg";
				break;
			case 1:
				toastNotificationViewModel3.Image = ImageService.GetImage("accept_32.svg");
				toastNotificationViewModel3.Text = message3;
				toastNotificationViewModel3.HistoryItem.Icon = "accept_32.svg";
				break;
			}
			toastNotificationViewModel3.HistoryItem.Type = 108;
			toastNotificationViewModel3.HistoryItem.Description = toastNotificationViewModel3.Caption;
			toastNotificationViewModel3.HistoryItem.Details = toastNotificationViewModel3.Text;
			ShowNotificationAsync(toastNotificationViewModel3);
		}
	}

	private void SynchronizationsTimerEventHandler(object stateInfo)
	{
		try
		{
			string traceLog = "Unknown";
			if (Settings.LoginService.IsLoggedIn && !ConfigHelper.ASAdminLogin && !Settings.IsOffline && !ConfigHelper.QATestMode && SynchronizationsProcessor != null && AutoBackupWindow == null)
			{
				if (!SynchronizationsProcessor.IsSynchronizationsEnabled())
				{
					SynchronizationsProcessorDispose();
				}
				else if (_synchronizationsSemaphore.Wait(0))
				{
					SynchronizationsTimerEventHandler(ref traceLog);
				}
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			if (_synchronizationsSemaphore != null)
			{
				_synchronizationsSemaphore.Release();
			}
		}
	}

	protected virtual void StatisticsTimerEventHandler(ref string traceLog)
	{
		StatisticsProcessorBase.SendData();
	}

	private void StatisticsTimerEventHandler(object stateInfo)
	{
		try
		{
			string traceLog = "Unknown";
			if (Settings.LoginService.IsLoggedIn && !ConfigHelper.ASAdminLogin && !Settings.IsOffline && AutoBackupWindow == null && _statisticsSemaphore.Wait(0))
			{
				StatisticsTimerEventHandler(ref traceLog);
			}
		}
		catch
		{
		}
		finally
		{
			if (_statisticsSemaphore != null)
			{
				_statisticsSemaphore.Release();
			}
		}
	}

	protected virtual void NotificationsTimerEventHandler(ref string traceLog)
	{
		if (!NotificationsProcessor.IsCheckedUpdateVersion)
		{
			NotificationsProcessor.IsCheckedUpdateVersion = true;
			traceLog = "CheckUpdateAvailable";
			RemainingSupportDaysCount();
			string version = Settings.CreateApiClient().AppManager.GetUIRecommendedVersion(ADHelper.ADB2CToken, Settings.LoginService.DatabaseId).Version;
			if (Settings.AppVersion < new Version(version))
			{
				_appUpdateService.UpdateAvailable = true;
				ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
				{
					Caption = ArmSoft.SME.Common.Resources.UpdateAvailable
				};
				toastNotificationViewModel.Text = toastNotificationViewModel.Caption;
				toastNotificationViewModel.HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification)
				{
					Icon = "software_update_available_16.svg",
					Type = 2,
					Description = toastNotificationViewModel.Caption,
					Details = toastNotificationViewModel.Caption
				};
				ShowNotificationAsync(toastNotificationViewModel);
			}
		}
	}

	protected virtual void RemainingSupportDaysCount()
	{
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		if (!Settings.LoginService.ContractExpirationDate.HasValue)
		{
			return;
		}
		Settings.LoginService.ContractRemainingSupportDays = (Settings.LoginService.ContractExpirationDate.Value - DateTime.Today).Days;
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		ContractBalance contractBalance = cloudClient.AS7X.GetContractBalance();
		if (contractBalance != null)
		{
			Settings.LoginService.ContractNexPayment = contractBalance.NextPayment - contractBalance.Balance;
		}
		decimal? contractNexPayment = Settings.LoginService.ContractNexPayment;
		if (!((contractNexPayment.GetValueOrDefault() > 0m) & contractNexPayment.HasValue) || Settings.LoginService.ContractRemainingSupportDays > 9 || Settings.LoginService.ContractRemainingSupportDays <= 0)
		{
			return;
		}
		if (Settings.LoginService.ContractRemainingSupportDays <= 0)
		{
			((DispatcherObject)Settings.HostingEnvironment).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
			{
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				_statusBarKeySerial.IsVisible = true;
				_statusBarKeySerial.Content = ArmSoft.SME.Common.Resources.ProgramNotSupported;
				_statusBarKeySerial.Glyph = ImageService.GetImage("warning_circle_16.svg");
			}), (DispatcherPriority)6, Array.Empty<object>());
		}
		else if (Settings.LoginService.ContractRemainingSupportDays <= 10)
		{
			ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
			{
				Image = ImageService.GetImage("warning_32.svg"),
				Caption = ArmSoft.SME.Common.Resources.SupportContract,
				Text = ((Settings.LoginService.ContractRemainingSupportDays == 1) ? ArmSoft.SME.Common.Resources.SupportContractExpiresToday : string.Format(ArmSoft.SME.Common.Resources.SupportContractExpiresDays, Settings.LoginService.ContractRemainingSupportDays))
			};
			toastNotificationViewModel.HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification)
			{
				Icon = "warning_16.svg",
				Type = 3,
				Description = toastNotificationViewModel.Caption,
				Details = toastNotificationViewModel.Text
			};
			ShowNotificationAsync(toastNotificationViewModel);
		}
	}

	private void NotificationsTimerEventHandler(object stateInfo)
	{
		try
		{
			string traceLog = "Unknown";
			if (Settings.LoginService.IsLoggedIn && !Settings.IsOffline && !ConfigHelper.ASAdminLogin && NotificationsProcessor != null)
			{
				if (!NotificationsProcessor.IsNotificationsEnabled())
				{
					NotificationsProcessorDispose();
				}
				else if (_notificationsSemaphore.Wait(0))
				{
					LanguageService.RestoreCulture(Settings.GetCurrentCultureName());
					NotificationsProcessor.NotificationsCheckDate = DateTime.Now;
					NotificationsTimerEventHandler(ref traceLog);
					NotificationsProcessor.PreviousNotificationsCheckDate = NotificationsProcessor.NotificationsCheckDate;
				}
			}
		}
		catch
		{
		}
		finally
		{
			if (_notificationsSemaphore != null)
			{
				_notificationsSemaphore.Release();
			}
		}
	}

	protected virtual void SynchronizationsProcessorDispose()
	{
		if (_synchronizationsTimer != null)
		{
			_synchronizationsTimer.Dispose();
			_synchronizationsTimer = null;
		}
		if (_synchronizationsSemaphore != null)
		{
			_synchronizationsSemaphore.Dispose();
			_synchronizationsSemaphore = null;
		}
	}

	protected virtual void OfflineSynchronizationsProcessorDispose()
	{
		if (_offlineDocsSynchronizationsTimer != null)
		{
			_offlineDocsSynchronizationsTimer.Dispose();
			_offlineDocsSynchronizationsTimer = null;
		}
		if (_offlineDocsSynchronizationsSemaphore != null)
		{
			_offlineDocsSynchronizationsSemaphore.Dispose();
			_offlineDocsSynchronizationsSemaphore = null;
		}
	}

	protected virtual void NotificationsProcessorDispose()
	{
		if (_notificationsTimer != null)
		{
			_notificationsTimer.Dispose();
			_notificationsTimer = null;
		}
	}

	protected virtual void StatisticsProcessorDispose()
	{
		if (_statisticsTimer != null)
		{
			_statisticsTimer.Dispose();
			_statisticsTimer = null;
		}
		if (_statisticsSemaphore != null)
		{
			_statisticsSemaphore.Dispose();
			_statisticsSemaphore = null;
		}
	}

	protected abstract void BarButtonWebDownload_ItemClick(object sender, ItemClickEventArgs e);

	protected abstract void BarButtonLiveChat_ItemClick(object sender, ItemClickEventArgs e);

	protected abstract void BarButtonMobile_ItemClick(object sender, ItemClickEventArgs e);

	private void BarButtonUpdateAvailable_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (!Settings.LoginService.IsLoggedIn)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.MustLogin);
		}
		_appUpdateService.TryUpdateAppFiles();
	}

	private void BarButtonCheckForUpdate_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (!Settings.LoginService.IsLoggedIn)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.MustLogin);
		}
		_appUpdateService.TryUpdateAppFiles();
	}

	public void DisableTimerPing()
	{
		if (_timerPing != null)
		{
			_timerPing.IsEnabled = false;
		}
	}

	private void BarButtonRegExcel_ItemClick(object sender, ItemClickEventArgs e)
	{
		MainRibbonSupportedPanel.BarButtonRegExcel_ItemClick(AddInPath);
	}

	protected abstract void BarButtonAbout_ItemClick(object sender, ItemClickEventArgs e);

	protected void BarButtonHelp_ItemClick(object sender, ItemClickEventArgs e)
	{
		ShowHelp();
	}

	protected void HelpHeaderItem_Click(object sender, RoutedEventArgs e)
	{
		ShowHelp();
	}

	private void CloudUserHeaderItem_Click(object sender, RoutedEventArgs e)
	{
		new CloudUserPropertiesWindow().Invoke();
	}
}
