using System;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.UI.SmartEnums;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public class CashierDisplayInfo
{
	private static CashierDisplayInfo _instance;

	public string Resolution { get; set; }

	public string TouchMode { get; set; }

	public bool FullScreen => Resolution.Equals("Full screen");

	public double Width
	{
		get
		{
			if (!FullScreen)
			{
				return double.Parse(Resolution.Split((ReadOnlySpan<char>)new char[1] { 'x' })[0]);
			}
			return 0.0;
		}
	}

	public double Height
	{
		get
		{
			if (!FullScreen)
			{
				return double.Parse(Resolution.Split((ReadOnlySpan<char>)new char[1] { 'x' })[1]);
			}
			return 0.0;
		}
	}

	public static CashierDisplayInfo GetInstance()
	{
		if (_instance == null)
		{
			_instance = new CashierDisplayInfo();
		}
		return _instance;
	}

	private CashierDisplayInfo()
	{
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\CashierDisplay", writable: false);
		if (registryKey == null)
		{
			TouchMode = POSScreenModesSE.SampleScreen.Value;
		}
		else
		{
			if ((string)registryKey.GetValue("Touch Mode") == "False")
			{
				TouchMode = POSScreenModesSE.Keyboard.Value;
			}
			else if ((string)registryKey.GetValue("Touch Mode") == "True")
			{
				TouchMode = POSScreenModesSE.TouchScreen.Value;
			}
			else
			{
				TouchMode = (string)registryKey.GetValue("Touch Mode");
			}
			Resolution = Convert.ToString(registryKey.GetValue("Resolution"));
		}
		if (Resolution == null)
		{
			string text = (Resolution = "Full screen");
		}
	}

	public void StoreSettings()
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\CashierDisplay");
		if (registryKey != null)
		{
			registryKey.SetValue("Touch Mode", TouchMode);
			registryKey.SetValue("Resolution", Resolution);
		}
	}
}
