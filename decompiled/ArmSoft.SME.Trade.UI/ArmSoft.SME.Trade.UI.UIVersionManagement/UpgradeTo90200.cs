using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.VersionManagement;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.UIVersionManagement;

public class UpgradeTo90200 : UpgradeTo
{
	[Change(1)]
	public void DeleteOldRegistryKeysForVersion()
	{
		string registryKey = Settings.RegistryKey;
		using RegistryKey registryKey2 = Registry.CurrentUser.CreateSubKey(registryKey);
		if (registryKey2.GetValue("UIVersion") != null)
		{
			registryKey2.DeleteValue("UIVersion");
			registryKey2.DeleteValue("UIVersionChangeNum");
		}
	}
}
