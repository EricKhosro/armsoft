using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.SmartEnums;

namespace ArmSoft.SME.Trade.UI;

public partial class DeviceSettings : System.Windows.Controls.UserControl, IComponentConnector
{
	public class MonitorRow
	{
		public string Name { get; set; } = "";

		public string DisplayDevice { get; set; } = "";
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	private struct DISPLAY_DEVICE
	{
		public int cb;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string DeviceName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceString;

		[MarshalAs(UnmanagedType.U4)]
		public DisplayDeviceStateFlags StateFlags;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceKey;
	}

	[Flags]
	private enum DisplayDeviceStateFlags : uint
	{
		AttachedToDesktop = 1u,
		MultiDriver = 2u,
		PrimaryDevice = 4u,
		MirroringDriver = 8u,
		VGACompatible = 0x10u,
		Removable = 0x20u,
		ModesPruned = 0x8000000u,
		Remote = 0x4000000u,
		Disconnect = 0x2000000u
	}

	private readonly List<MonitorRow> _monitorsList;

	private readonly List<CodeCaptionPairsCacheRow> _touchModesList;

	private readonly List<CodeCaptionPairsCacheRow> _scaleTypesList;

	private readonly System.Windows.Controls.Button _scannerConnect;

	private readonly TextBlock _scannerConnectionResult;

	private bool _scannerConnected;

	private readonly System.Windows.Controls.Button _mcrConnect;

	private readonly TextBlock _mcrConnectionResult;

	private bool _mcrConnected;

	private readonly System.Windows.Controls.Button _scaleConnect;

	private readonly TextBlock _scaleConnectionResult;

	private bool _scaleConnected;

	public DeviceSettings()
	{
		InitializeComponent();
		_monitorsList = GetMonitorsList();
		_touchModesList = GetTouchModeTypesList();
		_scaleTypesList = GetElScaleTypesList();
		gbScanerOptions.Header = ArmSoft.SME.Common.Resources.ScannerOptions;
		gbMagneticCardReader.Header = ArmSoft.SME.Trade.Common.Resources.MCROptions;
		gbCheckPrinter.Header = ArmSoft.SME.Trade.Common.Resources.CheckPrinter;
		gbLabelPrinter.Header = ArmSoft.SME.Trade.Common.Resources.LabelPrinter;
		CheckPrinter.ItemsSource = GetPrintersList();
		LabelPrinter.ItemsSource = CheckPrinter.ItemsSource;
		COMPort.ItemsSource = GetComPortList();
		BaudRate.ItemsSource = GetBaudRateList();
		StopBits.ItemsSource = GetStopBitsList();
		Parity.ItemsSource = GetParityList();
		DataBits.ItemsSource = GetDataBitList();
		StackPanel stackPanel = new StackPanel
		{
			Orientation = System.Windows.Controls.Orientation.Horizontal,
			Margin = new Thickness(0.0, 5.0, 0.0, 5.0),
			HorizontalAlignment = System.Windows.HorizontalAlignment.Left
		};
		_scannerConnect = new System.Windows.Controls.Button
		{
			HorizontalAlignment = System.Windows.HorizontalAlignment.Left
		};
		_scannerConnect.Click += new SafeEventHandler<RoutedEventArgs>(ScannerConnect_Click).Handler;
		_scannerConnectionResult = new TextBlock();
		stackPanel.Children.Add(_scannerConnect);
		_scannerConnectionResult.Margin = new Thickness(10.0, 0.0, 0.0, 0.0);
		_scannerConnectionResult.VerticalAlignment = VerticalAlignment.Center;
		stackPanel.Children.Add(_scannerConnectionResult);
		LayoutItemWithRequired element = new LayoutItemWithRequired
		{
			Content = stackPanel
		};
		gbScanerOptions.Children.Add(element);
		COMPort.PreviewKeyDown += new SafeEventHandler<System.Windows.Input.KeyEventArgs>(COMPort_PreviewKeyDown).Handler;
		MCRCOMPort.ItemsSource = COMPort.ItemsSource;
		MCRBaudRate.ItemsSource = BaudRate.ItemsSource;
		MCRStopBits.ItemsSource = StopBits.ItemsSource;
		MCRParity.ItemsSource = Parity.ItemsSource;
		MCRDataBits.ItemsSource = DataBits.ItemsSource;
		stackPanel = new StackPanel
		{
			Orientation = System.Windows.Controls.Orientation.Horizontal,
			Margin = new Thickness(0.0, 5.0, 0.0, 5.0),
			HorizontalAlignment = System.Windows.HorizontalAlignment.Left
		};
		_mcrConnect = new System.Windows.Controls.Button
		{
			HorizontalAlignment = System.Windows.HorizontalAlignment.Left
		};
		_mcrConnect.Click += new SafeEventHandler<RoutedEventArgs>(MCRConnect_Click).Handler;
		_mcrConnectionResult = new TextBlock();
		stackPanel.Children.Add(_mcrConnect);
		_mcrConnectionResult.Margin = new Thickness(10.0, 0.0, 0.0, 0.0);
		_mcrConnectionResult.VerticalAlignment = VerticalAlignment.Center;
		stackPanel.Children.Add(_mcrConnectionResult);
		element = new LayoutItemWithRequired
		{
			Content = stackPanel
		};
		gbMagneticCardReader.Children.Insert(gbMagneticCardReader.Children.IndexOf(lCtrMCRSeperator), element);
		lCtrMCRPrefix.Label = ArmSoft.SME.Trade.Common.Resources.Prefix;
		lCtrMCRSuffix.Label = ArmSoft.SME.Trade.Common.Resources.Suffix;
		gbPoleDisplayOptions.Header = ArmSoft.SME.Trade.Common.Resources.PoleDisplayOptions;
		gbCashierDisplayOptions.Header = ArmSoft.SME.Trade.Common.Resources.CashierDisplayOptions;
		liResolution.Label = ArmSoft.SME.Common.Resources.Resolution;
		ShowBarCodeInAvailList.Label = ArmSoft.SME.Trade.Common.Resources.ShowBarCodeInGoodsSelectList;
		AllowSaleWithQRCodesLayoutItem.Label = ArmSoft.SME.Trade.Common.Resources.AllowSaleWithQRCodes;
		AllowChangeCardSize.Label = ArmSoft.SME.Trade.Common.Resources.AllowChangeCardViewSize;
		InFavoritesFirtsGroups.Label = ArmSoft.SME.Trade.Common.Resources.InFavoritesFirtsGroups;
		liTouchMode.Label = ArmSoft.SME.Common.Resources.TypeWord;
		Resolutions.ItemsSource = GetResolutions();
		TouchModesComboBox.ItemsSource = _touchModesList;
		PDMonitors.ItemsSource = _monitorsList;
		liPDMonitors.Label = ArmSoft.SME.Trade.Common.Resources.PoleDisplayOptionsMonitor;
		liPDHeader.Label = ArmSoft.SME.Trade.Common.Resources.PoleDisplayOptionsHeader;
		liPDShowPicture.Label = ArmSoft.SME.Trade.Common.Resources.PoleDisplayOptionsPicture;
		liPDShowBackGround.Label = ArmSoft.SME.Trade.Common.Resources.PoleDisplayOptionsBackground;
		ScaleCOMPort.ItemsSource = COMPort.ItemsSource;
		ScaleBaudRate.ItemsSource = BaudRate.ItemsSource;
		ScaleDataBits.ItemsSource = DataBits.ItemsSource;
		ScaleParity.ItemsSource = Parity.ItemsSource;
		ScaleStopBits.ItemsSource = StopBits.ItemsSource;
		stackPanel = new StackPanel
		{
			Orientation = System.Windows.Controls.Orientation.Horizontal,
			Margin = new Thickness(0.0, 5.0, 0.0, 5.0),
			HorizontalAlignment = System.Windows.HorizontalAlignment.Left
		};
		_scaleConnect = new System.Windows.Controls.Button
		{
			Content = ArmSoft.SME.Common.Resources.CheckConnection,
			HorizontalAlignment = System.Windows.HorizontalAlignment.Left
		};
		_scaleConnect.Click += new SafeEventHandler<RoutedEventArgs>(ScaleConnect_Click).Handler;
		_scaleConnectionResult = new TextBlock();
		stackPanel.Children.Add(_scaleConnect);
		_scaleConnectionResult.Margin = new Thickness(10.0, 0.0, 0.0, 0.0);
		_scaleConnectionResult.VerticalAlignment = VerticalAlignment.Center;
		stackPanel.Children.Add(_scaleConnectionResult);
		element = new LayoutItemWithRequired
		{
			Content = stackPanel
		};
		gbElScaleOptions.Children.Add(element);
		ScaleTypes.ItemsSource = _scaleTypesList;
		ScaleTypes.SelectionChanged += new SafeEventHandler<EventArgs>(ScaleTypes_DropDownClosed).Handler;
		liScaleType.Label = ArmSoft.SME.Trade.Common.Resources.ScaleType;
		gbElScaleOptions.Header = ArmSoft.SME.Trade.Common.Resources.ElScaleOptions;
		ASFontHelper.SetFontSettings(this);
	}

	public void FillSettings()
	{
		ScannerOn.IsChecked = ScannerInfo.GetInstance().ScannerOn;
		COMPort.Text = ScannerInfo.GetInstance().COMPort;
		BaudRate.Text = ScannerInfo.GetInstance().BaudRate;
		Parity.Text = ScannerInfo.GetInstance().Parity;
		StopBits.Text = ScannerInfo.GetInstance().StopBits;
		DataBits.Text = ScannerInfo.GetInstance().DataBits;
		ScannerOptionsEnable(ScannerInfo.GetInstance().ScannerOn);
		UpdateScannerInfo();
		MCROn.IsChecked = MCRInfo.GetInstance().MCROn;
		MCRCOMPort.Text = MCRInfo.GetInstance().COMPort;
		MCRBaudRate.Text = MCRInfo.GetInstance().BaudRate;
		MCRParity.Text = MCRInfo.GetInstance().Parity;
		MCRStopBits.Text = MCRInfo.GetInstance().StopBits;
		MCRDataBits.Text = MCRInfo.GetInstance().DataBits;
		MCRPrefix.Text = MCRInfo.GetInstance().Prefix;
		MCRSuffix.Text = MCRInfo.GetInstance().Suffix;
		MCROptionsEnable(MCRInfo.GetInstance().MCROn);
		UpdateMCRInfo();
		CheckPrinter.Text = CheckPrinterInfo.GetInstance().CheckPrinterName;
		LabelPrinter.Text = LabelPrinterInfo.GetInstance().LabelPrinterName;
		Resolutions.SelectedIndex = Resolutions.Items.IndexOf(CashierDisplayInfo.GetInstance().Resolution);
		if (Settings.IsOffline)
		{
			InFavAndShowBarCodeLayoutGroup.Visibility = Visibility.Collapsed;
			ShowBarCodeInAvailList.Visibility = Visibility.Collapsed;
			AllowSaleWithQRCodesCheckEdit.Visibility = Visibility.Collapsed;
			AllowChangeCardSize.Visibility = Visibility.Collapsed;
			InFavoritesFirtsGroups.Visibility = Visibility.Collapsed;
		}
		else
		{
			InFavAndShowBarCodeLayoutGroup.Visibility = Visibility.Visible;
			ShowBarCodeInAvailList.Visibility = Visibility.Visible;
			AllowSaleWithQRCodesCheckEdit.Visibility = Visibility.Visible;
			AllowChangeCardSize.Visibility = Visibility.Visible;
			InFavoritesFirtsGroups.Visibility = Visibility.Visible;
			ShowBarCodeInAvailListCheck.IsChecked = Settings.ParametersService.ShowBarCodeInAvailList();
			AllowSaleWithQRCodesCheckEdit.IsChecked = Settings.ParametersService.AllowSaleWithQRCodes();
			AllowChangeCardSizeCheck.IsChecked = Settings.ParametersService.AllowCardViewSizeChange();
			InFavoritesFirtsGroupsCheck.IsChecked = Settings.ParametersService.InFavoritesFirtsGroups();
		}
		int i = 0;
		for (int num = _touchModesList.Count - 1; i <= num; i++)
		{
			if (_touchModesList[i].Code == CashierDisplayInfo.GetInstance().TouchMode)
			{
				TouchModesComboBox.SelectedIndex = i;
			}
		}
		for (int j = 0; j <= _monitorsList.Count - 1; j++)
		{
			if (_monitorsList[j].Name == PoleDisplayInfo.GetInstance().Monitor.Name && _monitorsList[j].DisplayDevice == PoleDisplayInfo.GetInstance().Monitor.DisplayDevice)
			{
				PDMonitors.SelectedIndex = j;
			}
		}
		PDShowHeader.IsChecked = PoleDisplayInfo.GetInstance().ShowHeader;
		PDHeaderText.Text = PoleDisplayInfo.GetInstance().HeaderText;
		PDHeaderText.IsEnabled = PoleDisplayInfo.GetInstance().ShowHeader;
		PDShowItemsPicture.IsChecked = PoleDisplayInfo.GetInstance().ShowImage;
		PDBackgroundImage.Text = PoleDisplayInfo.GetInstance().BackgroundImage;
		PDShowBackground.IsChecked = PoleDisplayInfo.GetInstance().ShowBackgroundImage;
		PDBackgroundImage.IsEnabled = PoleDisplayInfo.GetInstance().ShowBackgroundImage;
		int k = 0;
		for (int num2 = _scaleTypesList.Count - 1; k <= num2; k++)
		{
			if (_scaleTypesList[k].Code == ElScaleInfo.GetInstance().ElScaleType)
			{
				ScaleTypes.SelectedIndex = k;
			}
		}
		ScaleCOMPort.Text = ElScaleInfo.GetInstance().COMPort;
		ScaleBaudRate.Text = ElScaleInfo.GetInstance().BaudRate;
		ScaleParity.Text = ElScaleInfo.GetInstance().Parity;
		ScaleDataBits.Text = ElScaleInfo.GetInstance().DataBits;
		ScaleStopBits.Text = ElScaleInfo.GetInstance().StopBits;
		ElScaleOptionsEnable(ElScaleInfo.GetInstance().ElScaleType);
	}

	public void StoreSettings()
	{
		ScannerInfo.GetInstance().ScannerOn = Convert.ToBoolean(ScannerOn.IsChecked);
		ScannerInfo.GetInstance().COMPort = COMPort.Text;
		ScannerInfo.GetInstance().BaudRate = BaudRate.Text;
		ScannerInfo.GetInstance().Parity = Parity.Text;
		ScannerInfo.GetInstance().StopBits = StopBits.Text;
		ScannerInfo.GetInstance().DataBits = DataBits.Text;
		ScannerInfo.GetInstance().StoreSettings();
		MCRInfo.GetInstance().MCROn = Convert.ToBoolean(MCROn.IsChecked);
		MCRInfo.GetInstance().COMPort = MCRCOMPort.Text;
		MCRInfo.GetInstance().BaudRate = MCRBaudRate.Text;
		MCRInfo.GetInstance().Parity = MCRParity.Text;
		MCRInfo.GetInstance().StopBits = MCRStopBits.Text;
		MCRInfo.GetInstance().DataBits = MCRDataBits.Text;
		MCRInfo.GetInstance().Prefix = MCRPrefix.Text;
		MCRInfo.GetInstance().Suffix = MCRSuffix.Text;
		MCRInfo.GetInstance().StoreSettings();
		CheckPrinterInfo.GetInstance().CheckPrinterName = CheckPrinter.Text;
		CheckPrinterInfo.GetInstance().StoreSettings();
		LabelPrinterInfo.GetInstance().LabelPrinterName = LabelPrinter.Text;
		LabelPrinterInfo.GetInstance().StoreSettings();
		CashierDisplayInfo.GetInstance().TouchMode = ((CodeCaptionPairsCacheRow)TouchModesComboBox.SelectedItem).Code;
		CashierDisplayInfo.GetInstance().Resolution = Convert.ToString(Resolutions.SelectedValue);
		if (!Settings.IsOffline)
		{
			Settings.ParametersService.ShowBarCodeInAvailList_set(ShowBarCodeInAvailListCheck.IsChecked.Value, Settings.LoginService.UserId);
			Settings.ParametersService.AllowSaleWithQRCodes_set(AllowSaleWithQRCodesCheckEdit.IsChecked.Value, Settings.LoginService.UserId);
			Settings.ParametersService.AllowCardViewSizeChange_set(AllowChangeCardSizeCheck.IsChecked.Value, Settings.LoginService.UserId);
			Settings.ParametersService.InFavoritesFirtsGroups_set(InFavoritesFirtsGroupsCheck.IsChecked.Value, Settings.LoginService.UserId);
		}
		CashierDisplayInfo.GetInstance().StoreSettings();
		if (PDMonitors.SelectedItem != null)
		{
			MonitorRow monitorRow = (MonitorRow)PDMonitors.SelectedItem;
			if (monitorRow != null)
			{
				PoleDisplayInfo.GetInstance().Monitor.Name = monitorRow.Name;
				PoleDisplayInfo.GetInstance().Monitor.DisplayDevice = monitorRow.DisplayDevice;
			}
		}
		PoleDisplayInfo.GetInstance().ShowHeader = Convert.ToBoolean(PDShowHeader.IsChecked);
		PoleDisplayInfo.GetInstance().HeaderText = PDHeaderText.Text;
		PoleDisplayInfo.GetInstance().ShowImage = Convert.ToBoolean(PDShowItemsPicture.IsChecked);
		PoleDisplayInfo.GetInstance().BackgroundImage = PDBackgroundImage.Text;
		PoleDisplayInfo.GetInstance().ShowBackgroundImage = Convert.ToBoolean(PDShowBackground.IsChecked);
		PoleDisplayInfo.GetInstance().StoreSettings();
		CodeCaptionPairsCacheRow codeCaptionPairsCacheRow = (CodeCaptionPairsCacheRow)ScaleTypes.SelectedValue;
		if (codeCaptionPairsCacheRow != null)
		{
			ElScaleInfo.GetInstance().ElScaleType = codeCaptionPairsCacheRow.Code;
		}
		ElScaleInfo.GetInstance().COMPort = ScaleCOMPort.Text;
		ElScaleInfo.GetInstance().Parity = ScaleParity.Text;
		ElScaleInfo.GetInstance().BaudRate = ScaleBaudRate.Text;
		ElScaleInfo.GetInstance().StopBits = ScaleStopBits.Text;
		ElScaleInfo.GetInstance().DataBits = ScaleDataBits.Text;
		ElScaleInfo.GetInstance().StoreSettings();
	}

	private static ArrayList GetComPortList()
	{
		try
		{
			ArrayList arrayList = new ArrayList();
			string[] portNames = SerialPort.GetPortNames();
			foreach (string value in portNames)
			{
				arrayList.Add(value);
			}
			return arrayList;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			return null;
		}
	}

	private static ArrayList GetBaudRateList()
	{
		return new ArrayList
		{
			"110", "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "28800",
			"38400", "56000", "57600", "115200", "128000", "256000"
		};
	}

	private static ArrayList GetDataBitList()
	{
		return new ArrayList { "5", "6", "7", "8" };
	}

	private static ArrayList GetParityList()
	{
		return new ArrayList { "NO", "ODD", "EVEN", "MARK", "SPACE" };
	}

	private static ArrayList GetStopBitsList()
	{
		return new ArrayList { "1", "1.5", "2" };
	}

	private static List<CodeCaptionPairsCacheRow> GetElScaleTypesList()
	{
		return new List<CodeCaptionPairsCacheRow>
		{
			new CodeCaptionPairsCacheRow
			{
				Name = "",
				Code = ""
			},
			new CodeCaptionPairsCacheRow
			{
				Name = "CAS PR-II",
				Code = "3"
			},
			new CodeCaptionPairsCacheRow
			{
				Name = "Shtrih slim(POS2)",
				Code = "4"
			}
		};
	}

	private static IEnumerable<string> GetResolutions()
	{
		return from x in typeof(DevicesConst.Resolutions).GetFields()
			select x.GetRawConstantValue().ToString();
	}

	private void COMPort_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.Key == 20)
		{
			COMPort.IsDropDownOpen = true;
		}
	}

	private List<CodeCaptionPairsCacheRow> GetTouchModeTypesList()
	{
		return (from x in SmartEnum<POSScreenModesSE, string>.List
			where x.ShowInDropDown
			select new CodeCaptionPairsCacheRow
			{
				Name = x.Caption,
				Code = x.Value
			}).ToList();
	}

	private static ArrayList GetPrintersList()
	{
		ArrayList arrayList = new ArrayList();
		try
		{
			for (int i = 0; i <= PrinterSettings.InstalledPrinters.Count - 1; i++)
			{
				arrayList.Add(PrinterSettings.InstalledPrinters[i]);
			}
			return arrayList;
		}
		catch (Win32Exception ex)
		{
			Settings.ErrorHandler(new ASException((ex.NativeErrorCode == 1722) ? ArmSoft.SME.Trade.Common.Resources.ErrPrintSpoolerStopped : ex.Message, ex));
			return null;
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
			return null;
		}
	}

	private void ScannerOn_Checked(object sender, RoutedEventArgs e)
	{
		ScannerOptionsEnable(enable: true);
	}

	private void ScannerOn_Unchecked(object sender, RoutedEventArgs e)
	{
		ScannerOptionsEnable(enable: false);
	}

	private void ScannerOptionsEnable(bool enable)
	{
		BaudRate.IsEnabled = enable;
		COMPort.IsEnabled = enable;
		Parity.IsEnabled = enable;
		StopBits.IsEnabled = enable;
		DataBits.IsEnabled = enable;
		_scannerConnect.IsEnabled = enable;
		_scannerConnectionResult.Visibility = ((!enable) ? Visibility.Collapsed : Visibility.Visible);
	}

	private void MCROn_Checked(object sender, RoutedEventArgs e)
	{
		MCROptionsEnable(enable: true);
	}

	private void MCROn_Unchecked(object sender, RoutedEventArgs e)
	{
		MCROptionsEnable(enable: false);
	}

	private void MCROptionsEnable(bool enable)
	{
		MCRBaudRate.IsEnabled = enable;
		MCRCOMPort.IsEnabled = enable;
		MCRParity.IsEnabled = enable;
		MCRStopBits.IsEnabled = enable;
		MCRDataBits.IsEnabled = enable;
		_mcrConnect.IsEnabled = enable;
		_mcrConnectionResult.Visibility = ((!enable) ? Visibility.Collapsed : Visibility.Visible);
	}

	private void ElScaleOptionsEnable(string value)
	{
		bool flag = !string.IsNullOrWhiteSpace(value);
		ScaleCOMPort.IsEnabled = flag;
		ScaleBaudRate.IsEnabled = flag;
		ScaleStopBits.IsEnabled = flag;
		ScaleDataBits.IsEnabled = flag;
		ScaleParity.IsEnabled = flag;
		_scaleConnect.IsEnabled = flag;
		_scaleConnectionResult.Visibility = ((!flag) ? Visibility.Collapsed : Visibility.Visible);
	}

	private void PDShowHeader_Checked(object sender, RoutedEventArgs e)
	{
		PDHeaderText.IsEnabled = true;
	}

	private void PDShowHeader_Unchecked(object sender, RoutedEventArgs e)
	{
		PDHeaderText.IsEnabled = false;
	}

	private static List<MonitorRow> GetMonitorsList()
	{
		List<MonitorRow> list = new List<MonitorRow>();
		try
		{
			DISPLAY_DEVICE lpDisplayDevice = default;
			DISPLAY_DEVICE lpDisplayDevice2 = default;
			lpDisplayDevice.cb = Marshal.SizeOf(lpDisplayDevice);
			lpDisplayDevice2.cb = Marshal.SizeOf(lpDisplayDevice2);
			uint num = 0u;
			while (EnumDisplayDevices(null, num, ref lpDisplayDevice, 0u))
			{
				num = Convert.ToUInt32(num + 1);
				if (EnumDisplayDevices(lpDisplayDevice.DeviceName, 0u, ref lpDisplayDevice2, 1u) && !string.IsNullOrWhiteSpace(lpDisplayDevice2.DeviceString))
				{
					list.Add(new MonitorRow
					{
						Name = lpDisplayDevice2.DeviceString,
						DisplayDevice = lpDisplayDevice2.DeviceName
					});
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			return null;
		}
	}

	[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
	private static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

	private void PDBackgroundImage_DefaultButtonClick(object sender, RoutedEventArgs e)
	{
		string text = BrowseBackgroundImage();
		if (!string.IsNullOrWhiteSpace(text))
		{
			PDBackgroundImage.Text = text;
		}
	}

	private string BrowseBackgroundImage()
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (!string.IsNullOrWhiteSpace(PDBackgroundImage.Text) && !System.IO.Directory.Exists(Path.GetDirectoryName(PDBackgroundImage.Text)))
			{
				openFileDialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
			}
			openFileDialog.DefaultExt = "";
			openFileDialog.Filter = "All Picture files|*.jpeg;*.png;*.jpg;*.gif|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
			openFileDialog.FilterIndex = 1;
			if (Convert.ToBoolean(openFileDialog.ShowDialog()))
			{
				string fileName = openFileDialog.FileName;
				if (!string.IsNullOrWhiteSpace(fileName))
				{
					PoleDisplayInfo.GetInstance().BackgroundImage = fileName;
				}
				return fileName;
			}
			return "";
		}
		catch (Exception)
		{
			return "";
		}
	}

	private void PDShowBackground_Checked(object sender, RoutedEventArgs e)
	{
		PDBackgroundImage.IsEnabled = true;
	}

	private void PDShowBackground_Unchecked(object sender, RoutedEventArgs e)
	{
		PDBackgroundImage.IsEnabled = false;
	}

	private void ScaleTypes_DropDownClosed(object sender, EventArgs e)
	{
		CodeCaptionPairsCacheRow codeCaptionPairsCacheRow = (CodeCaptionPairsCacheRow)ScaleTypes.SelectedValue;
		ElScaleOptionsEnable(codeCaptionPairsCacheRow.Name);
	}

	private void ScannerConnect_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			Settings.HostingEnvironment.ScannerDisconnect();
			if (!_scannerConnected)
			{
				SerialPort port = new SerialPort
				{
					PortName = COMPort.Text,
					BaudRate = Convert.ToInt32(BaudRate.Text),
					DataBits = Convert.ToInt32(DataBits.Text),
					Parity = (Enum.TryParse<Parity>(Parity.Text, out var result) ? result : System.IO.Ports.Parity.None),
					StopBits = ((!Enum.TryParse<StopBits>(StopBits.Text, out var result2)) ? System.IO.Ports.StopBits.One : result2)
				};
				Settings.HostingEnvironment.ScannerConnect(port);
				if (Settings.HostingEnvironment.FailedToOpenComPort)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(Settings.HostingEnvironment.FailedToOpenComPortError, ArmSoft.SME.Common.Resources.ErrorWord, MessageBoxButton.OK, MessageBoxImage.Hand);
				}
			}
			UpdateScannerInfo();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void UpdateScannerInfo()
	{
		_scannerConnected = Settings.HostingEnvironment.Scanner?.IsOpen ?? false;
		_scannerConnect.Content = (_scannerConnected ? ArmSoft.SME.Common.Resources.Disconnect : ArmSoft.SME.Common.Resources.Connect);
		_scannerConnectionResult.Text = (_scannerConnected ? ArmSoft.SME.Common.Resources.Connected : ArmSoft.SME.Common.Resources.Disconnected);
		_scannerConnectionResult.Foreground = (_scannerConnected ? Brushes.Green : Brushes.Red);
	}

	private void MCRConnect_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			Settings.HostingEnvironment.MCRDisconnect();
			if (!_mcrConnected)
			{
				SerialPort port = new SerialPort
				{
					PortName = MCRCOMPort.Text,
					BaudRate = Convert.ToInt32(MCRBaudRate.Text),
					DataBits = Convert.ToInt32(MCRDataBits.Text),
					Parity = (Enum.TryParse<Parity>(MCRParity.Text, out var result) ? result : System.IO.Ports.Parity.None),
					StopBits = ((!Enum.TryParse<StopBits>(MCRStopBits.Text, out var result2)) ? System.IO.Ports.StopBits.One : result2)
				};
				Settings.HostingEnvironment.MCRConnect(port);
				if (Settings.HostingEnvironment.FailedToOpenComPort)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(Settings.HostingEnvironment.FailedToOpenComPortError, ArmSoft.SME.Common.Resources.ErrorWord, MessageBoxButton.OK, MessageBoxImage.Hand);
				}
			}
			UpdateMCRInfo();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void UpdateMCRInfo()
	{
		_mcrConnected = Settings.HostingEnvironment.MCR?.IsOpen ?? false;
		_mcrConnect.Content = (_mcrConnected ? ArmSoft.SME.Common.Resources.Disconnect : ArmSoft.SME.Common.Resources.Connect);
		_mcrConnectionResult.Text = (_mcrConnected ? ArmSoft.SME.Common.Resources.Connected : ArmSoft.SME.Common.Resources.Disconnected);
		_mcrConnectionResult.Foreground = (_mcrConnected ? Brushes.Green : Brushes.Red);
	}

	private void ScaleConnect_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			ScaleConnector.ScaleDisconnect();
			if (!_scaleConnected)
			{
				ScaleConnector.ScaleConnect(new SerialPort
				{
					PortName = ScaleCOMPort.Text,
					BaudRate = Convert.ToInt32(ScaleBaudRate.Text),
					DataBits = Convert.ToInt32(ScaleDataBits.Text),
					Parity = (Enum.TryParse<Parity>(ScaleParity.Text, out var result) ? result : System.IO.Ports.Parity.None),
					StopBits = ((!Enum.TryParse<StopBits>(ScaleStopBits.Text, out var result2)) ? System.IO.Ports.StopBits.One : result2),
					ReadTimeout = 3000
				});
				if (ScaleTypes.SelectedValue is CodeCaptionPairsCacheRow codeCaptionPairsCacheRow && !string.IsNullOrWhiteSpace(codeCaptionPairsCacheRow.Code))
				{
					ScaleConnector.ChooseProtocol(codeCaptionPairsCacheRow.Code);
				}
			}
			UpdateScaleInfo();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void UpdateScaleInfo()
	{
		_scaleConnected = ScaleConnector.Scale?.IsOpen ?? false;
		_scaleConnectionResult.Text = (_scaleConnected ? ArmSoft.SME.Common.Resources.Connected : ArmSoft.SME.Common.Resources.Disconnected);
		_scaleConnectionResult.Foreground = (_scaleConnected ? Brushes.Green : Brushes.Red);
		if (_scaleConnected)
		{
			_scaleConnected = false;
			ScaleConnector.ClosePort();
		}
	}
}
