using System;
using System.IO.Ports;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public class ElScaleInfo
{
	private static ElScaleInfo _instance;

	public string ElScaleType;

	public string COMPort;

	public string BaudRate;

	public string Parity;

	public string StopBits;

	public string DataBits;

	public static ElScaleInfo GetInstance()
	{
		if (_instance == null)
		{
			_instance = new ElScaleInfo();
		}
		return _instance;
	}

	private ElScaleInfo()
	{
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\ElScale", writable: true);
		if (registryKey == null)
		{
			ElScaleType = string.Empty;
			COMPort = "COM1";
			BaudRate = "9600";
			Parity = "";
			StopBits = "1";
			DataBits = "8";
		}
		else
		{
			ElScaleType = Convert.ToString(registryKey.GetValue("ElScaleType"));
			COMPort = Convert.ToString(registryKey.GetValue("COMPort"));
			BaudRate = Convert.ToString(registryKey.GetValue("Baud Rate"));
			Parity = Convert.ToString(registryKey.GetValue("Parity"));
			StopBits = Convert.ToString(registryKey.GetValue("Stop Bits"));
			DataBits = Convert.ToString(registryKey.GetValue("Data Bits"));
		}
	}

	internal void StoreSettings()
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\ElScale");
		if (registryKey != null)
		{
			registryKey.SetValue("ElScaleType", ElScaleType);
			registryKey.SetValue("COMPort", COMPort);
			registryKey.SetValue("Baud Rate", BaudRate);
			registryKey.SetValue("Parity", Parity);
			registryKey.SetValue("Stop Bits", StopBits);
			registryKey.SetValue("Data Bits", DataBits);
		}
	}

	public Parity GetParity()
	{
		if (SmartEnum<ParitiesSE, string>.TryFromValue(Parity, out var result))
		{
			return result.Parity;
		}
		return ParitiesSE.None.Parity;
	}
}
