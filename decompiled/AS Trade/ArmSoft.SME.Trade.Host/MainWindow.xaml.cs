using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.Discount;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.UserAccess;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.BackgroundWorks;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Converters;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Navigation;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.StandartWindow;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DataView.Reports.Analysis;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.MetroNavigation;
using ArmSoft.SME.Trade.UI.Navigation;
using ArmSoft.SME.Trade.UI.Notifications;
using ArmSoft.SME.Trade.UI.Services;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet.Functions;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Docking;
using Serilog;

namespace ArmSoft.SME.Trade.Host;

public partial class MainWindow : MainWindowBase, IComponentConnector
{
	[Guid("DC19A1A0-35A6-43A5-853E-552FABDBDF7D")]
	[ComVisible(true)]
	public class CustomNotificationActivator : ToastNotificationActivator
	{
		public override void OnActivate(string arguments, Dictionary<string, string> data)
		{
			((DispatcherObject)Application.Current).Dispatcher.Invoke((Action)(() =>
			{
				MainFunctionsHelper.Activator();
			}));
		}
	}

	private BarStaticItem StatusBarActualnessDateCorrect;

	private BarStaticItem StatusBarActualnessDate;

	private bool _firstActivated = true;

	internal NavigationBarsController NavBarController = new NavigationBarsController();

	private DispatcherTimer TimerChangeCurrentDate;

	private Timer _discountServerSyncTimer;

	private DocumentPanel homePanel;

	private NavigationBarsController _navBarController = new NavigationBarsController();

	private NotificationsProcessor _notificationsProcessor;

	private SynchronizationsProcessor _synchronizationsProcessor;

	private OfflineSynchronizationsProcessor _offlineSynchronizationsProcessor;

	private UserAccessManager userAccessManager { get; set; }

	public override string AddInPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AddIn", "ArmSoft.SME.Trade.AddIn.dll");

	public override Uri AdsUri
	{
		get
		{
			if (Settings.LoginService.ContractType != 1)
			{
				return new Uri("https://www.armsoft.am/banners/trade_9_0_0_am.htm");
			}
			return new Uri("https://www.armsoft.am/banners/store_9_0_0_am.htm");
		}
	}

	public InformationWindowCloseDelegate CallBackForInfoWindowClosing { get; }

	protected override bool AllowHomePanel => true;

	protected override DocumentPanel HomePanel => homePanel;

	public override NavigationBarsControllerBase NavigationBarsController => _navBarController;

	public override UserAccessManagerBase UserAccessManager => userAccessManager;

	public override NotificationsProcessorBase NotificationsProcessor => _notificationsProcessor;

	public override SynchronizationsProcessorBase SynchronizationsProcessor => _synchronizationsProcessor;

	public override OfflineSynchronizationsProcessorBase OfflineSynchronizationsProcessor => _offlineSynchronizationsProcessor;

	public override Dictionary<short, string> Subsystems => new Dictionary<short, string>
	{
		{
			1,
			ArmSoft.SME.Trade.Common.Resources.Purchases
		},
		{
			2,
			ArmSoft.SME.Common.Resources.Sales
		},
		{
			7,
			ArmSoft.SME.Trade.Common.Resources.PointOfSalePOS
		},
		{
			3,
			ArmSoft.SME.Common.Resources.Storage
		},
		{
			4,
			ArmSoft.SME.Trade.Common.Resources.CashBank
		},
		{
			5,
			ArmSoft.SME.Common.Resources.AnalysisCaption
		},
		{
			8,
			ArmSoft.SME.Common.Resources.EInvoicing
		}
	};

	protected override Type NotificationActivatorType => typeof(CustomNotificationActivator);

	public MainWindow()
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		CallBackForInfoWindowClosing = DoWorkForInfoWindowClosing;
		CallBackForCreatePayOrderDelegate = CreatePayOrder;
		Settings.InitHostingEnvironment(this);
		StatusBarActualnessDateCorrect = new BarStaticItem
		{
			Content = "",
			AutoSizeMode = BarItemAutoSizeMode.Fill,
			Name = "StatusBarActualnessDateCorrect",
			Glyph = ImageService.GetImage("warning_16.svg"),
			LargeGlyph = ImageService.GetImage("warning_32.svg"),
			IsVisible = false
		};
		StatusBarActualnessDateCorrect.ItemDoubleClick += new SafeEventHandler<ItemClickEventArgs>(StatusBarActualnessDateCorrect_ItemDoubleClick).Handler;
		_ribbonStatusBar.LeftItemLinks.AddWithSeparator(StatusBarActualnessDateCorrect.CreateLink());
		StatusBarActualnessDate = new BarStaticItem
		{
			Content = "",
			ItemMinWidth = 100.0,
			AutoSizeMode = BarItemAutoSizeMode.Content,
			Name = "StatusBarActualnessDate"
		};
		_ribbonStatusBar.LeftItemLinks.AddWithSeparator(StatusBarActualnessDate.CreateLink());
		homePanel = new DocumentPanel
		{
			Name = "HomePage",
			CaptionImage = ImageService.GetImage("t_home_16.svg"),
			Caption = ArmSoft.SME.Common.Resources.HomePanel,
			AllowFloat = false,
			AllowMaximize = false,
			AllowDock = false
		};
		MainRibbonPanel.RecentPanelBarItemLink.IsVisible = true;
		PanelsForClosing.Add(typeof(AnalysisDashBoardWindow));
		MainRibbonPanel.ShowTransactionsCheckItem.IsVisible = false;
		MainRibbonPanel.BarButtonKB.IsVisible = false;
		MainRibbonPanel.WebDownloadBarButtonItem.IsVisible = false;
		BarItemLink barItemLink = MainRibbonPanel.MobileBarButtonItem.CreateLink();
		barItemLink.RibbonStyle = RibbonItemStyles.SmallWithText;
		MainRibbonPanel.RibbonControl.PageHeaderItems.Add(barItemLink);
		MainRibbonPanel.LoginOfflineBarItemLink.IsVisible = true;
		MainRibbonPanel.ChangePinBarItemLink.IsVisible = true;
		MainRibbonPanel.ChangeOnlineStatusBarItemLink.IsVisible = true;
		SetBarsStates();
		_statusBarKeySerial.IsVisible = true;
		ImageSource icon = new ImageSourceConverter().ConvertFromString(Settings.AppIconPath) as ImageSource;
		Icon = icon;
		MainRibbonPanel.AboutBarButtonItem.Content = ArmSoft.SME.Trade.Common.Resources.HlpAbout;
		Binding binding = new Binding
		{
			Converter = new ArmSoft.SME.Core.UI.Converters.FontSizeConverter()
		};
		SetBinding(Control.FontSizeProperty, binding);
		SetTitle("");
		ASFontHelper.SetFontSettings(this);
	}

	protected override void SetTitle(string dbName)
	{
		if (string.IsNullOrWhiteSpace(dbName))
		{
			Title = ((Settings.LoginService.ContractType == 1) ? ArmSoft.SME.Trade.Common.Resources.ProgramNameStart : Settings.ProgramName);
		}
		else
		{
			Title = dbName + " - " + ((Settings.LoginService.ContractType == 1) ? ArmSoft.SME.Trade.Common.Resources.ProgramNameStart : Settings.ProgramName) + " " + Settings.AppVersionString;
		}
		if (Settings.IsOffline)
		{
			Title = Title + " (" + ArmSoft.SME.Common.Resources.Offline + ")";
		}
		if (!Environment.Is64BitProcess)
		{
			Title += " (x86)";
		}
	}

	private void StatusBarActualnessDateCorrect_ItemDoubleClick(object sender, ItemClickEventArgs e)
	{
		ActualnessDateHelpers.ChangeActualnessDate();
	}

	private void CloudUserHeaderItem_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			new CloudUserPropertiesWindow().Invoke();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public override void ReloadControlCaptions()
	{
		MainRibbonPanel.AboutBarButtonItem.Content = ArmSoft.SME.Trade.Common.Resources.HlpAbout;
		homePanel.Caption = ArmSoft.SME.Common.Resources.HomePanel;
		base.ReloadControlCaptions();
	}

	public override void ShowHomePanel()
	{
		base.ShowHomePanel();
		if (homePanel != null && homePanel.Parent == null)
		{
			if (homePanel.Tag != null)
			{
				MainNavigationWindow window = (MainNavigationWindow)homePanel.Content;
				if (window != null && Settings.UserAccessService.GetAccessibleSubsystems().Any((SubsystemAccess p) => p.SubsystemId == window.GetSubSystemByKey(homePanel.Tag.ToString())))
				{
					if (Settings.IsOffline)
					{
						window.NavigateToCommandByKey("PointOfSaleModel");
					}
					else if (!Settings.ParametersService.ShowOnlyUserAWP() || homePanel.Tag.ToString() == "UserAWPModel" || homePanel.Tag.ToString() == "PointOfSaleModel")
					{
						window.NavigateToCommandByKey(homePanel.Tag.ToString());
					}
				}
				homePanel.Tag = null;
			}
			DocGroupPanels.Insert(0, homePanel);
		}
		homePanel.Visibility = Visibility.Visible;
		homePanel.IsActive = true;
	}

	public override void InitCOMPorts()
	{
		try
		{
			ScannerConnect();
			MCRConnect();
			ScaleConnect();
		}
		catch (Exception ex)
		{
			FailedToOpenComPort = true;
			FailedToOpenComPortError = ex.Message;
		}
	}

	public override void ScannerConnect(SerialPort port = null)
	{
		if (Scanner != null)
		{
			return;
		}
		if (port != null)
		{
			Scanner = port;
		}
		else
		{
			ScannerInfo instance = ScannerInfo.GetInstance();
			if (!instance.ScannerOn)
			{
				return;
			}
			Scanner = new SerialPort
			{
				PortName = instance.COMPort,
				BaudRate = Convert.ToInt32(instance.BaudRate),
				DataBits = Convert.ToInt32(instance.DataBits),
				Parity = instance.GetParity(),
				StopBits = ((!Enum.TryParse<StopBits>(instance.StopBits, out var result)) ? StopBits.One : result)
			};
		}
		if (MemoryExtensions.Contains<string>((ReadOnlySpan<string>)SerialPort.GetPortNames(), Scanner.PortName) && !Scanner.IsOpen)
		{
			Scanner.Open();
			Scanner.DataReceived += OnScannerDataReceived;
			FailedToOpenComPort = false;
		}
	}

	public override void MCRConnect(SerialPort port = null)
	{
		if (MCR != null)
		{
			return;
		}
		if (port != null)
		{
			MCR = port;
		}
		else
		{
			MCRInfo instance = MCRInfo.GetInstance();
			if (!instance.MCROn)
			{
				return;
			}
			MCR = new SerialPort
			{
				PortName = instance.COMPort,
				BaudRate = Convert.ToInt32(instance.BaudRate),
				DataBits = Convert.ToInt32(instance.DataBits),
				Parity = instance.GetParity()
			};
			Enum.TryParse<StopBits>(instance.StopBits, out var result);
			MCR.StopBits = result;
		}
		if (MemoryExtensions.Contains<string>((ReadOnlySpan<string>)SerialPort.GetPortNames(), MCR.PortName) && !MCR.IsOpen)
		{
			MCR.Open();
			MCR.DataReceived += OnMCRDataReceived;
			FailedToOpenComPort = false;
		}
	}

	private void ScaleConnect()
	{
		ScaleConnector.Initialize();
		FailedToOpenComPort = false;
	}

	public override void CloseCOMPorts()
	{
		try
		{
			ScannerDisconnect();
			MCRDisconnect();
			ScaleConnector.ScaleDisconnect();
		}
		catch
		{
		}
	}

	public override void ScannerDisconnect()
	{
		if (Scanner != null)
		{
			Scanner.Close();
			Scanner.Dispose();
			Scanner = null;
		}
	}

	public override void MCRDisconnect()
	{
		if (MCR != null)
		{
			MCR.Close();
			MCR.Dispose();
			MCR = null;
		}
	}

	private void OnScannerDataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		if (Scanner == null || e.EventType != SerialData.Chars)
		{
			return;
		}
		try
		{
			_buffer += Scanner.ReadExisting();
			int num = _buffer.IndexOf('\r');
			int num2 = _buffer.IndexOf('\n');
			if (num >= 0 || num2 >= 0)
			{
				int length = Math.Min((num >= 0) ? num : int.MaxValue, (num2 >= 0) ? num2 : int.MaxValue);
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSerialPortDataReceived(ScannerDataReceived), new object[1] { _buffer.Substring(0, length) });
				_buffer = "";
				Scanner.BaseStream.Flush();
			}
		}
		catch (Exception exception)
		{
			Log.Error(exception, "An unexpected error occurred while handling scanner input.");
		}
	}

	private void ScannerDataReceived(string scannerData)
	{
		try
		{
			scannerData = scannerData.Trim();
			foreach (Window window in Application.Current.Windows)
			{
				if (window.IsActive && window is IScanable scanable)
				{
					scanable.OnScanner(scannerData);
					return;
				}
			}
			if (CurrentDataViewPanel() != null && CurrentDataViewPanel().IsActive)
			{
				CurrentDataViewPanel().OnScanner(scannerData);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private DataViewPanel CurrentDataViewPanel()
	{
		if (CurrentPanel is DataViewPanel result)
		{
			return result;
		}
		return Settings.HostingEnvironment.CurrentViewPanel;
	}

	private void OnMCRDataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		if (MCR == null || e.EventType != SerialData.Chars)
		{
			return;
		}
		try
		{
			_buffer += MCR.ReadExisting();
			int num = _buffer.IndexOf('\r');
			int num2 = _buffer.IndexOf('\n');
			if (num >= 0 || num2 >= 0)
			{
				int length = Math.Min((num >= 0) ? num : int.MaxValue, (num2 >= 0) ? num2 : int.MaxValue);
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSerialPortDataReceived(MCRDataReceived), new object[1] { _buffer.Substring(0, length) });
			}
			_buffer = "";
			MCR.BaseStream.Flush();
		}
		catch (Exception exception)
		{
			Log.Error(exception, "An unexpected error occurred while handling MCR input.");
		}
	}

	private void MCRDataReceived(string cardData)
	{
		try
		{
			cardData = cardData.Trim();
			foreach (Window window in Application.Current.Windows)
			{
				if (window.IsActive && window is IMagneticCardReadable magneticCardReadable)
				{
					magneticCardReadable.OnMagneticCardReader(cardData);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public override void InitSpreadsheetFunctions(CustomFunctionCollection customFunctionCollection)
	{
	}

	public override bool IsActiveWindowOptionsWindow(string callingSubName)
	{
		try
		{
			foreach (Window window in Application.Current.Windows)
			{
				if (window != Settings.HostingEnvironment && window.IsActive && (window is OptionsWindow || window is DialogWindow || window is MessageBoxWindow))
				{
					return true;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Warning("IsActiveWindowOptionsWindow: " + callingSubName, ex.Message);
			return true;
		}
		return false;
	}

	protected override void MainWindowLoaded()
	{
		base.MainWindowLoaded();
		RegisterNavigatorCommandHotKey(HotKeys.WithModifier.Control_B, UserAccessIdSE.PartRepCard.Value);
		RegisterNavigatorCommandHotKey(HotKeys.WithModifier.Control_Shift_B, UserAccessIdSE.Partners.Value);
		RegisterNavigatorCommandHotKey(HotKeys.WithModifier.Control_L, UserAccessIdSE.MatRepTAccount.Value);
		RegisterNavigatorCommandHotKey(HotKeys.WithModifier.Control_Shift_L, UserAccessIdSE.Goods.Value);
		RegisterNavigatorCommandHotKey(HotKeys.Functional.F12, UserAccessIdSE.MTBill.Value);
		RegisterNavigatorCommandHotKey(HotKeys.WithModifier.Shift_F12, UserAccessIdSE.MTBillReturn.Value);
		RegisterNavigatorCommandHotKey(HotKeys.WithModifier.Control_J, UserAccessIdSE.DocumentsJournal.Value);
	}

	protected override void MainWindowActivated()
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected Obj, but got Unknown
		base.MainWindowActivated();
		if (!_firstActivated)
		{
			return;
		}
		_firstActivated = false;
		try
		{
			CloseSplashScreen();
			Activate();
			if (!Settings.OfflineModeIsAvailable || MainFunctionsHelper.IsArmSoftCloudAvailable() || ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.LoginModeSelectionPrompt, null, MessageBoxButton.YesNo) == MessageBoxResult.No)
			{
				LogIn(isOnline: true);
			}
			else
			{
				LogIn(isOnline: false);
			}
			if (CurrentDate != DateTime.Today.ToDateOnly())
			{
				ConfirmCurDateChange();
			}
			TimerChangeCurrentDate = new DispatcherTimer(DateTime.Today.AddDays(1.0) - DateTime.Now, (DispatcherPriority)9, (EventHandler)TimerChangeCurDateEventHandler, ((DispatcherObject)this).Dispatcher);
			if (FailedToOpenComPort)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(FailedToOpenComPortError, ArmSoft.SME.Common.Resources.ErrorWord, MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}
		catch (Exception ex)
		{
			CloseSplashScreen();
			Settings.ErrorHandler(ex);
		}
	}

	private static void CloseSplashScreen()
	{
		if (App.SplashScreen.State != SplashScreenState.Closing && App.SplashScreen.State != SplashScreenState.Closed)
		{
			App.SplashScreen.Close();
		}
	}

	protected override void SynchronizationsTimerEventHandler(ref string traceLog)
	{
		base.SynchronizationsTimerEventHandler(ref traceLog);
		if (!_synchronizationsProcessor.PartnerStatisticsChecked)
		{
			_synchronizationsProcessor.PartnerStatisticsChecked = true;
			if (!_synchronizationsProcessor.PartnerStatisticsLastDate.HasValue || _synchronizationsProcessor.PartnerStatisticsLastDate < DateTime.Today.ToDateOnly())
			{
				Settings.ParametersService.PartnerDataReceivedDateFromb2b_set(DateTime.Today.ToDateOnly());
				traceLog = ArmSoft.SME.Common.Resources.PartnerStatisticFromb2b;
				string message = string.Empty;
				short num = _synchronizationsProcessor.TryGetPartnersStatisticsFromb2b(Settings.HostingEnvironment.CurrentDate, ref message);
				string imageUri = string.Empty;
				string historyItemIconUri = string.Empty;
				if (num != 0)
				{
					switch (num)
					{
					case -1:
						imageUri = "error_32.svg";
						historyItemIconUri = "edit_16.svg";
						break;
					case 1:
						imageUri = "accept_32.svg";
						historyItemIconUri = "accept_16.svg";
						break;
					}
					TradeNotificationHelper.ShowTradeNotification(ArmSoft.SME.Common.Resources.PartnerStatisticFromb2b, imageUri, message, 10106, ArmSoft.SME.Common.Resources.PartnerStatisticFromb2b, message, historyItemIconUri);
				}
			}
		}
		if (!_synchronizationsProcessor.OfflineDBInfoUpdatedChecked)
		{
			traceLog = "Send Offline DB Info";
			_synchronizationsProcessor.OfflineDBInfoUpdatedChecked = true;
			ArmSoft.SME.Trade.UI.Notifications.SynchronizationsProcessor.SendOfflineDBInfo();
		}
		if (!_synchronizationsProcessor.EMarkReceiptUpdateChecked)
		{
			_synchronizationsProcessor.EMarkReceiptUpdateChecked = true;
			if (Settings.ParametersService.UseMarking() && (!_synchronizationsProcessor.EMarkReceiptUpdateLastDate.HasValue || _synchronizationsProcessor.EMarkReceiptUpdateLastDate < DateTime.Today.ToDateOnly()))
			{
				Settings.ParametersService.EMarkReceiptLastSyncDate_set(DateTime.Today.ToDateOnly());
				traceLog = "EMark Receipts update";
				ArmSoft.SME.Trade.UI.Notifications.SynchronizationsProcessor.TryUpdateEMarkReceipts(_synchronizationsProcessor.EMarkReceiptUpdateLastDate.Value);
			}
		}
	}

	protected override void OfflineDocsSynchronizationsTimerEventHandler(ref string traceLog)
	{
		base.OfflineDocsSynchronizationsTimerEventHandler(ref traceLog);
		if (!_offlineSynchronizationsProcessor.OfflineDocumentsDeleteChecked)
		{
			traceLog = "Delete Old Offline Documents";
			_offlineSynchronizationsProcessor.OfflineDocumentsDeleteChecked = true;
			_offlineSynchronizationsProcessor.DeleteOldOfflineDocuments();
		}
		if (_offlineSynchronizationsProcessor.OfflineDocumentsSendLastDate.HasValue && !(_offlineSynchronizationsProcessor.OfflineDocumentsSendLastDate.Value.AddMinutes(_offlineSynchronizationsProcessor.OfflineDocumentsSendPeriodInM) <= DateTime.Now))
		{
			return;
		}
		_offlineSynchronizationsProcessor.OfflineDocumentsSendLastDate = DateTime.Now;
		traceLog = ArmSoft.SME.Common.Resources.OfflineDocsSending;
		_offlineSynchronizationsProcessor.SendOfflineDocuments(out var successCount, out var errorCount);
		if (successCount > 0 || errorCount > 0)
		{
			_ = string.Empty;
			string empty = string.Empty;
			string details;
			if (errorCount > 0)
			{
				details = ArmSoft.SME.Common.Resources.WorkCompletedWithErrors;
				empty = "error_16.svg";
			}
			else
			{
				details = ArmSoft.SME.Common.Resources.WorkCompletedSuccess;
				empty = "accept_16.svg";
			}
			HistoryItem historyItem = new HistoryItem(HistoryItem.ItemCategory.Notification)
			{
				Type = 104,
				Description = ArmSoft.SME.Common.Resources.OfflineDocsSending,
				Details = details,
				Icon = empty
			};
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action<HistoryItem>(WriteInHistory), new object[1] { historyItem });
		}
	}

	protected override void NotificationsTimerEventHandler(ref string traceLog)
	{
		base.NotificationsTimerEventHandler(ref traceLog);
	}

	protected override void RemainingSupportDaysCount()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		if (Settings.LoginService.ContractType == 1 || Settings.LoginService.ContractType == 2)
		{
			if (Settings.LoginService.KeyExpirationDate.HasValue)
			{
				int days = (Settings.LoginService.KeyExpirationDate.Value - DateTime.Today).Days;
				ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel();
				toastNotificationViewModel.Image = ImageService.GetImage("warning_32.svg");
				toastNotificationViewModel.Caption = Settings.ProgramName;
				toastNotificationViewModel.Text = ((days > 0) ? (ArmSoft.SME.Trade.Common.Resources.TrialModeTimeEndDay + " " + days + " " + ArmSoft.SME.Common.Resources.Day) : ArmSoft.SME.Trade.Common.Resources.TrialModeTimeEnded);
				ToastNotificationViewModel toastNotificationViewModel2 = toastNotificationViewModel;
				toastNotificationViewModel2.HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification)
				{
					Icon = "warning_16.svg",
					Type = 3,
					Description = toastNotificationViewModel2.Caption,
					Details = toastNotificationViewModel2.Text
				};
				ShowNotificationAsync(toastNotificationViewModel2);
				return;
			}
			decimal? contractNexPayment = Settings.LoginService.ContractNexPayment;
			if (!((contractNexPayment.GetValueOrDefault() > 0m) & contractNexPayment.HasValue))
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
			else if (Settings.LoginService.ContractRemainingSupportDays <= 9)
			{
				ToastNotificationViewModel toastNotificationViewModel3 = new ToastNotificationViewModel
				{
					Image = ImageService.GetImage("warning_32.svg"),
					Caption = ArmSoft.SME.Common.Resources.SupportContract,
					Text = ((Settings.LoginService.ContractRemainingSupportDays == 1) ? ArmSoft.SME.Common.Resources.SupportContractExpiresToday : string.Format(ArmSoft.SME.Common.Resources.SupportContractExpiresDays, Settings.LoginService.ContractRemainingSupportDays))
				};
				toastNotificationViewModel3.HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification)
				{
					Icon = "warning_16.svg",
					Type = 3,
					Description = toastNotificationViewModel3.Caption,
					Details = toastNotificationViewModel3.Text
				};
				ShowNotificationAsync(toastNotificationViewModel3);
			}
		}
		else
		{
			base.RemainingSupportDaysCount();
		}
	}

	public override void InnerLogIn()
	{
		base.InnerLogIn();
		if (!Settings.ParametersService.AllowLogin())
		{
			throw new ASException(ArmSoft.SME.Common.Resources.DBLoginNotAllowed);
		}
		_notificationsProcessor = new NotificationsProcessor();
		if (!Settings.IsOffline && Settings.OfflineModeIsAvailable)
		{
			StoreDataInLocalDBAfterLogin();
		}
		_offlineSynchronizationsProcessor = new OfflineSynchronizationsProcessor();
		if (!Settings.IsOffline && !ConfigHelper.ASAdminLogin)
		{
			_synchronizationsProcessor = new SynchronizationsProcessor();
			MainFunctionsHelper.CheckCloudDatabaseProperties();
		}
		SetCloudUserHeaderInfo(Settings.LoginService.UserDescription, Settings.LoginService.UserName);
		RefreshWorkPeriodStatusBar();
		userAccessManager = new UserAccessManager();
		if (!Settings.IsOffline)
		{
			ActualnessDateHelpers.IsActualnessDateCorrect();
		}
		if (Settings.ParametersService.ShowPoleDisplay())
		{
			TradeSettings.PoleDisplay = new PoleDisplayWindow();
		}
		homePanel.Content = new MainNavigationWindow();
		if (!Settings.IsOffline)
		{
			if (Settings.LoginService.ContractExpirationDate.HasValue)
			{
				InformationWindow.ShowIfActive(CallBackForInfoWindowClosing);
			}
			if (Settings.ParametersService.UseBonus() && Settings.ParametersService.ConnectTob2barmsoftDiscountServer() && Settings.ParametersService.SyncContractsWithb2barmsoftDiscountServer())
			{
				_discountServerSyncTimer = new Timer(DiscountServerTimerEventHandler, null, new TimeSpan(0, 0, 2, 0), new TimeSpan(0, 0, Settings.ParametersService.DiscountServerSyncInterval(), 0));
			}
		}
		RefreshWorkPeriodStatusBar();
	}

	public override void LogOut(bool bSaveSettings)
	{
		SetCloudUserHeaderInfo(string.Empty, string.Empty);
		TradeSettings.PoleDisplay = null;
		if (homePanel != null)
		{
			MainNavigationWindow mainNavigationWindow = (MainNavigationWindow)homePanel.Content;
			if (mainNavigationWindow != null)
			{
				homePanel.Tag = mainNavigationWindow.GetCurrentModuleCode();
			}
		}
		TradeSettings.LocalDBService.Close();
		base.LogOut(bSaveSettings);
	}

	public override void LogOutApp()
	{
		base.LogOutApp();
		StatusBarActualnessDateCorrect.IsVisible = false;
		StatusBarActualnessDate.Content = string.Empty;
		_statusBarKeySerial.Content = string.Empty;
		_statusBarKeySerial.Glyph = null;
		if (_discountServerSyncTimer != null)
		{
			_discountServerSyncTimer.Dispose();
			_discountServerSyncTimer = null;
		}
	}

	private void TimerChangeCurDateEventHandler(object sender, EventArgs e)
	{
		ConfirmCurDateChange();
		TimerChangeCurrentDate.Interval = new TimeSpan(1, 0, 0, 0, 0);
	}

	private void ConfirmCurDateChange()
	{
		if (!SyncVar && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ConfirmCurDateChange, "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
		{
			CurrentDate = DateTime.Today.ToDateOnly();
			DateBegin = new DateOnly(CurrentDate.Year, CurrentDate.Month, 1);
			DateEnd = CurrentDate;
		}
	}

	private void DiscountServerTimerEventHandler(object stateInfo)
	{
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
		{
			Caption = ArmSoft.SME.Trade.Common.Resources.DiscountServer,
			HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification)
		};
		toastNotificationViewModel.HistoryItem.Type = 10107;
		toastNotificationViewModel.HistoryItem.Description = toastNotificationViewModel.Caption;
		try
		{
			using CloudClient cloudClient = Settings.DiscountServerClient;
			List<string> updatedCards = new List<string>();
			foreach (CardInfo card in cloudClient.Discount.GetCards(false))
			{
				DiscountServerService.ImportCardInfo(card, ref updatedCards);
			}
			if (updatedCards.Count > 0)
			{
				cloudClient.Discount.UpdateCardsSyncState(updatedCards);
				toastNotificationViewModel.Image = ImageService.GetImage("accept_32.svg");
				toastNotificationViewModel.Text = ArmSoft.SME.Trade.Common.Resources.DiscountServerCardsSynced;
				toastNotificationViewModel.HistoryItem.Icon = "accept_16.svg";
				toastNotificationViewModel.HistoryItem.Details = toastNotificationViewModel.Text;
				ShowNotificationAsync(toastNotificationViewModel);
			}
		}
		catch (Exception ex)
		{
			Log.Warning($"DiscountServerTimerEventHandler (Database - {Settings.LoginService.Database}, TraceLog - {ArmSoft.SME.Trade.Common.Resources.DiscountServer})", $"Exception - {ex.Message}");
			toastNotificationViewModel.Image = ImageService.GetImage("error_32.svg");
			toastNotificationViewModel.Text = ex.Message;
			toastNotificationViewModel.HistoryItem.Icon = "error_16.svg";
			toastNotificationViewModel.HistoryItem.Details = toastNotificationViewModel.Text;
			ShowNotificationAsync(toastNotificationViewModel);
		}
	}

	protected override void ClearUserData()
	{
		base.ClearUserData();
		TradeSettings.TradeCacheService.Clear();
	}

	public override string GetDocumentDescription(Guid exceptionDocISN)
	{
		return TradeSettings.CreateTradeClient().CommonServices.GetDocumentDescription(exceptionDocISN, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum);
	}

	public override void ShowOptionsWindow()
	{
		if (!Settings.LoginService.IsLoggedIn)
		{
			return;
		}
		Mouse.OverrideCursor = Cursors.Wait;
		try
		{
			OptionsWindow optionsWindow = new OptionsWindow();
			if (Convert.ToBoolean(optionsWindow.Invoke()))
			{
				RefreshWorkPeriodStatusBar();
				Settings.HostingEnvironment.StatusBarCompanyName = Settings.ParametersService.CompanyName();
				if (optionsWindow.FontOrThemeChanged)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OptionsChgInfo, ArmSoft.SME.Common.Resources.Options, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public override void RefreshWorkPeriodStatusBar()
	{
		WorkingPeriod workingPeriod;
		if (Settings.IsOffline)
		{
			workingPeriod = TradeSettings.LocalDBService.WorkingPeriod;
		}
		else
		{
			workingPeriod = Settings.CommonCoreHelper.GetWorkingPeriod();
			TradeSettings.LocalDBService.WorkingPeriod = workingPeriod;
		}
		_statusBarWorkPeriod.Content = ArmSoft.SME.Common.Resources.WorkPeriodShort + " " + workingPeriod.StartDate.FormatDDMMYY() + " - " + workingPeriod.EndDate.FormatDDMMYY();
		_statusBarKeySerial.Content = GetKeyInfo();
	}

	public override string GetKeyInfo()
	{
		return ArmSoft.SME.Common.Resources.Key + " R " + Settings.LoginService.WSLimit + "  " + Settings.LoginService.Key;
	}

	public override void RefreshActualnessDateStatusBar(bool isActualnessDateCorrect, bool isActualnessDateChanged = true)
	{
		if (isActualnessDateChanged)
		{
			DateOnly value = Settings.ParametersService.ActualnessDate().Value;
			StatusBarActualnessDate.Content = ArmSoft.SME.Trade.Common.Resources.ActualnessDateShort + value.FormatDDMMYY();
		}
		StatusBarActualnessDateCorrect.IsVisible = isActualnessDateCorrect;
	}

	protected override void BarButtonWebDownload_ItemClick(object sender, ItemClickEventArgs e)
	{
		Process.Start(new ProcessStartInfo("http://support.armsoft.am/knowledgeBase/ViewKBArticle.aspx?kbarticleid=36beca6a-d742-e311-bd19-00155d640a02")
		{
			UseShellExecute = true
		});
	}

	protected override void BarButtonLiveChat_ItemClick(object sender, ItemClickEventArgs e)
	{
		StringBuilder stringBuilder = new StringBuilder("https://secure.livechatinc.com/licence/9876745/v2/open_chat.cgi?groups=0");
		stringBuilder.Append("&params=App%3DTrade%20");
		stringBuilder.Append(Settings.AppVersionString);
		Process.Start(new ProcessStartInfo(stringBuilder.ToString())
		{
			UseShellExecute = true
		});
	}

	protected override void BarButtonMobile_ItemClick(object sender, ItemClickEventArgs e)
	{
		Process.Start(new ProcessStartInfo("https://online.armsoft.am/trade7/index.html?mobileapplication.htm")
		{
			UseShellExecute = true
		});
	}

	protected override void BarButtonAbout_ItemClick(object sender, ItemClickEventArgs e)
	{
		new About().ShowDialog();
	}

	public override void ShowHelp(string helpId = "")
	{
		if (string.IsNullOrWhiteSpace(helpId))
		{
			Process.Start(new ProcessStartInfo("https://online.armsoft.am/trade7/index.html")
			{
				UseShellExecute = true
			});
		}
		else
		{
			Process.Start(new ProcessStartInfo("https://online.armsoft.am/trade7/index.html?" + helpId + ".htm")
			{
				UseShellExecute = true
			});
		}
	}

	protected override void NotificationsProcessorDispose()
	{
		base.NotificationsProcessorDispose();
		_notificationsProcessor = null;
	}

	protected override void SynchronizationsProcessorDispose()
	{
		base.SynchronizationsProcessorDispose();
		_synchronizationsProcessor = null;
	}

	protected override void OfflineSynchronizationsProcessorDispose()
	{
		base.OfflineSynchronizationsProcessorDispose();
		_offlineSynchronizationsProcessor = null;
	}

	private void StoreDataInLocalDBAfterLogin()
	{
		Settings.LoginService.StoreToLocalDB();
		TradeSettings.LocalDBService.Token = ApplicationServiceConfig.Token;
		TradeSettings.LocalDBService.ADB2CToken = ADHelper.ADB2CToken;
		TradeSettings.LocalDBService.PIN = Settings.ParametersService.UserPIN();
		string openCashierShiftNumber = CashierShiftHelper.GetOpenCashierShiftNumber();
		if (!string.IsNullOrWhiteSpace(openCashierShiftNumber))
		{
			CashierShiftHelper.GetOpenCashierShiftBeginDate(openCashierShiftNumber);
		}
		SaleWindowHelper.IsDefaultStorageRetail();
		SaleWindowHelper.GetNextDocumentNumberAndRefreshOfflineIfNeeded(DocumentTypeSE.MTBill);
		SaleWindowHelper.GetNextDocumentNumberAndRefreshOfflineIfNeeded(DocumentTypeSE.MTReturnFromCustomer);
		SaleWindowHelper.GetNextDocumentNumberAndRefreshOfflineIfNeeded(DocumentTypeSE.MTBillReturn);
	}

	protected override bool OnOnlineStatusChange(bool newOnlineStatus)
	{
		if (newOnlineStatus)
		{
			_ = ADHelper.ADB2CToken;
			_ = ApplicationServiceConfig.Token;
		}
		return true;
	}

	public object DoWorkForInfoWindowClosing()
	{
		OpenContractWindow();
		return true;
	}

	public static object CreatePayOrder()
	{
		DocumentHelpers.CreatePaymentOrderDoc();
		return true;
	}

	public override void HistoryItemClick(HistoryItem item)
	{
		base.HistoryItemClick(item);
		try
		{
			if (item.Category == HistoryItem.ItemCategory.Notification && item.Type == 3)
			{
				OpenContractWindow();
			}
			else if (item.Type == 104 && _offlineSynchronizationsProcessor.OfflineDocumentsSendErrorRepViewer.HasErrors)
			{
				_offlineSynchronizationsProcessor.OfflineDocumentsSendErrorRepViewer.Show();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		base.MainWindow_PropertyChanged(sender, e);
		if (e.PropertyName == "CurrentDate")
		{
			Settings.CoreCacheService.CurrentExchangeRates.Refresh();
			TradeSettings.TradeCacheService.CurrentPrices.Refresh();
		}
	}
}
