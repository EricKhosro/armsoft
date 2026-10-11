using System;
using ArmSoft.SME.Core.UI;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public class WeightItemInfo : WeightOrPieceItem
{
	private static WeightItemInfo _instance;

	public static WeightItemInfo GetInstance()
	{
		if (_instance == null)
		{
			_instance = new WeightItemInfo();
		}
		return _instance;
	}

	private WeightItemInfo()
	{
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\Weight Item", writable: false);
		if (registryKey == null)
		{
			Prefix = "28";
			CodeLength = 5;
			Precision = 3;
			Unit = "303";
		}
		else
		{
			Prefix = Convert.ToString(registryKey.GetValue("Weight Item Prefix"));
			CodeLength = Convert.ToInt16(registryKey.GetValue("Weight Item Code Length"));
			Precision = Convert.ToInt16(registryKey.GetValue("Weight Item Precision"));
			Unit = Convert.ToString(registryKey.GetValue("Weight Item Unit"));
		}
	}

	public override void StoreSettings()
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\Weight Item");
		if (registryKey != null)
		{
			registryKey.SetValue("Weight Item Prefix", Prefix);
			registryKey.SetValue("Weight Item Code Length", CodeLength);
			registryKey.SetValue("Weight Item Precision", Precision);
			registryKey.SetValue("Weight Item Unit", Unit);
		}
	}
}
