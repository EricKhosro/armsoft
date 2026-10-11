using System;
using ArmSoft.SME.Core.UI;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public class CheckPrinterInfo
{
	private static CheckPrinterInfo _instance;

	public string CheckPrinterName;

	public short FontLoadMode;

	public static CheckPrinterInfo GetInstance()
	{
		if (_instance == null)
		{
			_instance = new CheckPrinterInfo();
		}
		return _instance;
	}

	private CheckPrinterInfo()
	{
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\CheckPrinter", writable: false);
		if (registryKey == null)
		{
			CheckPrinterName = string.Empty;
			FontLoadMode = 1;
		}
		else
		{
			CheckPrinterName = Convert.ToString(registryKey.GetValue("Check Printer"));
			FontLoadMode = Convert.ToInt16(registryKey.GetValue("Fonts"));
		}
	}

	public void StoreSettings()
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\CheckPrinter");
		if (registryKey != null)
		{
			registryKey.SetValue("Check Printer", CheckPrinterName);
			registryKey.SetValue("Fonts", FontLoadMode);
		}
	}
}
