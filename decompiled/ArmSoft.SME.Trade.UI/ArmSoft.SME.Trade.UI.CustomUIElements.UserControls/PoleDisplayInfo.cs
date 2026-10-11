using System;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public class PoleDisplayInfo
{
	private static PoleDisplayInfo _instance;

	public DeviceSettings.MonitorRow Monitor { get; set; } = new DeviceSettings.MonitorRow();

	public bool ShowHeader { get; set; }

	public string HeaderText { get; set; }

	public bool ShowImage { get; set; }

	public string BackgroundImage { get; set; }

	public bool ShowBackgroundImage { get; set; }

	public static PoleDisplayInfo GetInstance()
	{
		if (_instance == null)
		{
			_instance = new PoleDisplayInfo();
		}
		return _instance;
	}

	private PoleDisplayInfo()
	{
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\PoleDisplay", writable: false);
		if (registryKey == null)
		{
			ShowHeader = true;
			HeaderText = Settings.ParametersService.CompanyName();
			ShowImage = true;
			ShowBackgroundImage = false;
			BackgroundImage = string.Empty;
		}
		else
		{
			Monitor.Name = Convert.ToString(registryKey.GetValue("Monitor"));
			Monitor.DisplayDevice = Convert.ToString(registryKey.GetValue("DisplayDevice"));
			ShowHeader = Convert.ToBoolean(registryKey.GetValue("ShowHeader"));
			HeaderText = Convert.ToString(registryKey.GetValue("HeaderText"));
			ShowImage = Convert.ToBoolean(registryKey.GetValue("ShowImage"));
			BackgroundImage = Convert.ToString(registryKey.GetValue("BackgroundImage"));
			ShowBackgroundImage = Convert.ToBoolean(registryKey.GetValue("ShowBackgroundImage"));
		}
	}

	public void StoreSettings()
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\PoleDisplay");
		if (registryKey != null)
		{
			registryKey.SetValue("Monitor", Monitor.Name);
			registryKey.SetValue("DisplayDevice", Monitor.DisplayDevice);
			registryKey.SetValue("ShowHeader", ShowHeader);
			registryKey.SetValue("HeaderText", HeaderText);
			registryKey.SetValue("ShowImage", ShowImage);
			registryKey.SetValue("BackgroundImage", BackgroundImage);
			registryKey.SetValue("ShowBackgroundImage", ShowBackgroundImage);
		}
	}
}
