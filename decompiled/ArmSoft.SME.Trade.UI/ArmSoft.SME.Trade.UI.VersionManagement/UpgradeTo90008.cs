using System;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.VersionManagement;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.VersionManagement;

public class UpgradeTo90008 : UpgradeTo
{
	[Change(1)]
	public void Change1()
	{
		MoveExportDirectory();
	}

	private static void MoveExportDirectory()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Armenian Software\\AS Trade 7\\", writable: false);
			if (registryKey != null)
			{
				string text = Convert.ToString(registryKey.GetValue("ExportDirectory"));
				if (!string.IsNullOrWhiteSpace(text))
				{
					RegistryHelper.WriteExportDirectory(text);
				}
			}
		}
		catch
		{
		}
	}
}
