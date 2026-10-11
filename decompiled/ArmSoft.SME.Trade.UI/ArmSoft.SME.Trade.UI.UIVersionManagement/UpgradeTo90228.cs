using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.VersionManagement;
using ArmSoft.SME.Trade.UI.SmartEnums;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.UIVersionManagement;

public class UpgradeTo90228 : UpgradeTo
{
	[Change(1)]
	public void TemporaryDeletePOSKeyboardModeFromUI()
	{
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\CashierDisplay", writable: true);
		if (registryKey != null && (!(registryKey.GetValue("Touch Mode") is string text) || text == POSScreenModesSE.Keyboard.Value))
		{
			registryKey.SetValue("Touch Mode", POSScreenModesSE.SampleScreen.Value);
		}
	}

	[Change(2)]
	public void AddWin11Theme()
	{
		UpgradeTo.AddWin11ThemeBase();
	}
}
