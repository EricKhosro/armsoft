using System;
using System.IO.Ports;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public class MCRInfo
{
	private static MCRInfo _instance;

	public bool MCROn;

	public string COMPort;

	public string BaudRate;

	public string Parity;

	public string StopBits;

	public string DataBits;

	public string Prefix;

	public string Suffix;

	public static MCRInfo GetInstance()
	{
		if (_instance == null)
		{
			_instance = new MCRInfo();
		}
		return _instance;
	}

	private MCRInfo()
	{
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\MCR", writable: true);
		if (registryKey == null)
		{
			MCROn = false;
			COMPort = "COM1";
			BaudRate = "9600";
			Parity = "";
			StopBits = "1";
			DataBits = "8";
			Prefix = ";";
			Suffix = "?";
		}
		else
		{
			MCROn = Convert.ToBoolean(registryKey.GetValue("MCR On"));
			COMPort = Convert.ToString(registryKey.GetValue("COMPort"));
			BaudRate = Convert.ToString(registryKey.GetValue("Baud Rate"));
			Parity = Convert.ToString(registryKey.GetValue("Parity"));
			StopBits = Convert.ToString(registryKey.GetValue("Stop Bits"));
			DataBits = Convert.ToString(registryKey.GetValue("Data Bits"));
			Prefix = Convert.ToString(registryKey.GetValue("Prefix"));
			Suffix = Convert.ToString(registryKey.GetValue("Suffix"));
		}
	}

	internal void StoreSettings()
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\MCR");
		if (registryKey != null)
		{
			Prefix = Prefix.Trim();
			Suffix = Suffix.Trim();
			registryKey.SetValue("MCR On", MCROn);
			registryKey.SetValue("COMPort", COMPort);
			registryKey.SetValue("Baud Rate", BaudRate);
			registryKey.SetValue("Parity", Parity);
			registryKey.SetValue("Stop Bits", StopBits);
			registryKey.SetValue("Data Bits", DataBits);
			registryKey.SetValue("Prefix", Prefix);
			registryKey.SetValue("Suffix", Suffix);
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

	public bool IsValidCard(string cardData)
	{
		if (cardData.Length >= Prefix.Length + Suffix.Length + 1 && cardData.Mid(1, Prefix.Length) == Prefix)
		{
			return cardData.Mid(cardData.Length - Suffix.Length + 1, Suffix.Length) == Suffix;
		}
		return false;
	}

	public string GetCardNumber(string cardData)
	{
		int length = cardData.Length - Suffix.Length - Prefix.Length;
		return cardData.Mid(Prefix.Length + 1, length);
	}
}
