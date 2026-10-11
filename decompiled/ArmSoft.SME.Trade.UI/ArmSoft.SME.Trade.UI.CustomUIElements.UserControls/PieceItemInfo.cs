using System;
using ArmSoft.SME.Core.UI;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public class PieceItemInfo : WeightOrPieceItem
{
	private static PieceItemInfo _instance;

	public static PieceItemInfo GetInstance()
	{
		if (_instance == null)
		{
			_instance = new PieceItemInfo();
		}
		return _instance;
	}

	private PieceItemInfo()
	{
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\Piece Item", writable: false);
		if (registryKey == null)
		{
			Prefix = "26";
			CodeLength = 5;
			Precision = 0;
			Unit = "001";
		}
		else
		{
			Prefix = Convert.ToString(registryKey.GetValue("Piece Item Prefix"));
			CodeLength = Convert.ToInt16(registryKey.GetValue("Piece Item Code Length"));
			Precision = Convert.ToInt16(registryKey.GetValue("Piece Item Precision"));
			Unit = Convert.ToString(registryKey.GetValue("Piece Item Unit"));
		}
	}

	public override void StoreSettings()
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\Piece Item");
		if (registryKey != null)
		{
			registryKey.SetValue("Piece Item Prefix", Prefix);
			registryKey.SetValue("Piece Item Code Length", CodeLength);
			registryKey.SetValue("Piece Item Precision", Precision);
			registryKey.SetValue("Piece Item Unit", Unit);
		}
	}
}
