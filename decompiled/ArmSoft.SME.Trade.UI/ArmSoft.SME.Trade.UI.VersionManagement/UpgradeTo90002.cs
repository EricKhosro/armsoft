using System;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.VersionManagement;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.SmartEnums;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.VersionManagement;

public class UpgradeTo90002 : UpgradeTo
{
	[Change(1)]
	public void Change1()
	{
		MoveScannerInfo();
		MoveMCRInfo();
		MoveCheckPrinterInfo();
		MoveLabelPrinterInfo();
		MovePoleDisplayInfo();
		MoveElScaleInfo();
		MoveCashierDisplayInfo();
		MoveWeightItemInfo();
		MovePieceItemInfo();
	}

	private static void MoveScannerInfo()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Armenian Software\\AS Trade 7\\Scanner\\", writable: false);
			if (registryKey == null)
			{
				return;
			}
			using RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\Scanner", writable: false);
			if (registryKey2 == null)
			{
				ScannerInfo instance = ScannerInfo.GetInstance();
				instance.ScannerOn = Convert.ToBoolean(registryKey.GetValue("Scanner On"));
				instance.COMPort = Convert.ToString(registryKey.GetValue("COMPort"));
				instance.BaudRate = Convert.ToString(registryKey.GetValue("Baud Rate"));
				instance.Parity = Convert.ToString(registryKey.GetValue("Parity"));
				instance.StopBits = Convert.ToString(registryKey.GetValue("Stop Bits"));
				instance.DataBits = Convert.ToString(registryKey.GetValue("Data Bits"));
				instance.StoreSettings();
			}
		}
		catch
		{
		}
	}

	private static void MoveMCRInfo()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Armenian Software\\AS Trade 7\\MCR\\", writable: false);
			if (registryKey == null)
			{
				return;
			}
			using RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\MCR", writable: false);
			if (registryKey2 == null)
			{
				MCRInfo instance = MCRInfo.GetInstance();
				instance.MCROn = Convert.ToBoolean(registryKey.GetValue("MCR On"));
				instance.COMPort = Convert.ToString(registryKey.GetValue("COMPort"));
				instance.BaudRate = Convert.ToString(registryKey.GetValue("Baud Rate"));
				instance.Parity = Convert.ToString(registryKey.GetValue("Parity"));
				instance.StopBits = Convert.ToString(registryKey.GetValue("Stop Bits"));
				instance.DataBits = Convert.ToString(registryKey.GetValue("Data Bits"));
				instance.Prefix = Convert.ToString(registryKey.GetValue("Prefix"));
				instance.Suffix = Convert.ToString(registryKey.GetValue("Suffix"));
				instance.StoreSettings();
			}
		}
		catch
		{
		}
	}

	private static void MoveCheckPrinterInfo()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Armenian Software\\AS Trade 7\\CheckPrinter\\", writable: false);
			if (registryKey == null)
			{
				return;
			}
			using RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\CheckPrinter", writable: false);
			if (registryKey2 == null)
			{
				CheckPrinterInfo instance = CheckPrinterInfo.GetInstance();
				instance.CheckPrinterName = Convert.ToString(registryKey.GetValue("Check Printer"));
				instance.FontLoadMode = Convert.ToInt16(registryKey.GetValue("Fonts"));
				instance.StoreSettings();
			}
		}
		catch
		{
		}
	}

	private static void MoveLabelPrinterInfo()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Armenian Software\\AS Trade 7\\LabelPrinter\\", writable: false);
			if (registryKey != null)
			{
				LabelPrinterInfo instance = LabelPrinterInfo.GetInstance();
				if (string.IsNullOrWhiteSpace(instance.LabelPrinterName))
				{
					instance.LabelPrinterName = Convert.ToString(registryKey.GetValue("Label Printer"));
					instance.StoreSettings();
				}
			}
		}
		catch
		{
		}
	}

	private static void MovePoleDisplayInfo()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Armenian Software\\AS Trade 7\\PoleDisplay\\", writable: false);
			if (registryKey == null)
			{
				return;
			}
			using RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\PoleDisplay", writable: false);
			if (registryKey2 == null)
			{
				PoleDisplayInfo instance = PoleDisplayInfo.GetInstance();
				instance.Monitor.Name = Convert.ToString(registryKey.GetValue("Monitor"));
				instance.Monitor.DisplayDevice = Convert.ToString(registryKey.GetValue("DisplayDevice"));
				instance.ShowHeader = Convert.ToBoolean(registryKey.GetValue("ShowHeader"));
				instance.HeaderText = Convert.ToString(registryKey.GetValue("HeaderText"));
				instance.ShowImage = Convert.ToBoolean(registryKey.GetValue("ShowImage"));
				instance.BackgroundImage = Convert.ToString(registryKey.GetValue("BackgroundImage"));
				instance.ShowBackgroundImage = Convert.ToBoolean(registryKey.GetValue("ShowBackgroundImage"));
				instance.StoreSettings();
			}
		}
		catch
		{
		}
	}

	private static void MoveElScaleInfo()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Armenian Software\\AS Trade 7\\ElScale\\", writable: false);
			if (registryKey == null)
			{
				return;
			}
			using RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\ElScale", writable: false);
			if (registryKey2 == null)
			{
				ElScaleInfo instance = ElScaleInfo.GetInstance();
				instance.ElScaleType = Convert.ToString(registryKey.GetValue("ElScaleType"));
				instance.COMPort = Convert.ToString(registryKey.GetValue("COMPort"));
				instance.BaudRate = Convert.ToString(registryKey.GetValue("Baud Rate"));
				instance.Parity = Convert.ToString(registryKey.GetValue("Parity"));
				instance.StopBits = Convert.ToString(registryKey.GetValue("Stop Bits"));
				instance.DataBits = Convert.ToString(registryKey.GetValue("Data Bits"));
				instance.StoreSettings();
			}
		}
		catch
		{
		}
	}

	private static void MoveCashierDisplayInfo()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Armenian Software\\AS Trade 7\\CashierDisplay\\", writable: false);
			if (registryKey == null)
			{
				return;
			}
			using RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\CashierDisplay", writable: false);
			if (registryKey2 == null)
			{
				CashierDisplayInfo instance = CashierDisplayInfo.GetInstance();
				string text = (string)registryKey.GetValue("Touch Mode");
				if (text == "False")
				{
					instance.TouchMode = POSScreenModesSE.SampleScreen.Value;
				}
				else if (text == "True")
				{
					instance.TouchMode = POSScreenModesSE.TouchScreen.Value;
				}
				else
				{
					instance.TouchMode = text;
				}
				instance.Resolution = Convert.ToString(registryKey.GetValue("Resolution"));
				CashierDisplayInfo cashierDisplayInfo = instance;
				if (cashierDisplayInfo.Resolution == null)
				{
					string text2 = (cashierDisplayInfo.Resolution = "Full screen");
				}
				instance.StoreSettings();
			}
		}
		catch
		{
		}
	}

	private static void MoveWeightItemInfo()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Armenian Software\\AS Trade 7\\Weight Item\\", writable: false);
			if (registryKey == null)
			{
				return;
			}
			using RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\Weight Item", writable: false);
			if (registryKey2 == null)
			{
				WeightItemInfo instance = WeightItemInfo.GetInstance();
				instance.Prefix = Convert.ToString(registryKey.GetValue("Weight Item Prefix"));
				instance.CodeLength = Convert.ToInt16(registryKey.GetValue("Weight Item Code Length"));
				instance.Precision = Convert.ToInt16(registryKey.GetValue("Weight Item Precision"));
				instance.Unit = Convert.ToString(registryKey.GetValue("Weight Item Unit"));
				instance.StoreSettings();
			}
		}
		catch
		{
		}
	}

	private static void MovePieceItemInfo()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Armenian Software\\AS Trade 7\\Piece Item\\", writable: false);
			if (registryKey == null)
			{
				return;
			}
			using RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\Piece Item", writable: false);
			if (registryKey2 == null)
			{
				PieceItemInfo instance = PieceItemInfo.GetInstance();
				instance.Prefix = Convert.ToString(registryKey.GetValue("Piece Item Prefix"));
				instance.CodeLength = Convert.ToInt16(registryKey.GetValue("Piece Item Code Length"));
				instance.Precision = Convert.ToInt16(registryKey.GetValue("Piece Item Precision"));
				instance.Unit = Convert.ToString(registryKey.GetValue("Piece Item Unit"));
				instance.StoreSettings();
			}
		}
		catch
		{
		}
	}
}
