using ArmSoft.SME.Core.UI.SmartEnums;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.VersionManagement;

public abstract class UpgradeTo
{
	public static void AddWin11ThemeBase()
	{
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey, writable: true);
		if (registryKey != null && string.IsNullOrWhiteSpace(Settings.ApplicationTheme()))
		{
			registryKey.SetValue("ApplicationTheme", ApplicationThemeSE.Office);
		}
	}
}
