using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.HelpClasses;

public static class RegistryHelper
{
	private const string _windowDefaultValue = "0";

	private const string _exportDirectoryName = "ExportDirectory";

	private const string _importDirectoryName = "ImportDirectory";

	public static bool RestoreWindowMainSettings(Window window, string source, double defaultWidth = 0.0, double defaultHeight = 0.0, bool restoreState = true)
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\" + source, writable: false);
			return RestoreWindowMainSettings(window, registryKey, defaultWidth, defaultHeight, restoreState);
		}
		catch
		{
			return false;
		}
	}

	public static bool RestoreWindowMainSettings(Window window, RegistryKey registryKey, double defaultWidth = 0.0, double defaultHeight = 0.0, bool restoreState = true)
	{
		try
		{
			if (registryKey != null)
			{
				if (restoreState && Enum.TryParse(typeof(WindowState), registryKey.GetValue("WindowState", "0").ToString(), out object result))
				{
					window.WindowState = (WindowState)result;
				}
				if (window.WindowState == WindowState.Normal)
				{
					if (double.TryParse(registryKey.GetValue("Width", defaultWidth).ToString(), out var result2))
					{
						window.Width = result2;
					}
					if (double.TryParse(registryKey.GetValue("Height", defaultHeight).ToString(), out var result3))
					{
						window.Height = result3;
					}
				}
				return true;
			}
			return false;
		}
		catch
		{
			return false;
		}
	}

	public static bool StoreWindowMainSettings(Window window, string source, bool writeState = true)
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\" + source);
			return StoreWindowMainSettings(window, registryKey, writeState);
		}
		catch
		{
			return false;
		}
	}

	public static bool StoreWindowMainSettings(Window window, RegistryKey registryKey, bool writeState = true)
	{
		try
		{
			if (registryKey != null && window != null)
			{
				if (window.WindowState == WindowState.Normal)
				{
					registryKey.SetValue("Width", window.Width);
					registryKey.SetValue("Height", window.Height);
				}
				if (writeState)
				{
					registryKey.SetValue("WindowState", window.WindowState);
				}
				return true;
			}
		}
		catch
		{
			return false;
		}
		return false;
	}

	public static void DeleteWindowsSettings()
	{
		Registry.CurrentUser.DeleteSubKeyTree(Settings.RegistryKey + "\\DialogWindow", throwOnMissingSubKey: false);
		Registry.CurrentUser.DeleteSubKeyTree(Settings.RegistryKey + "\\DocumentWindow", throwOnMissingSubKey: false);
		Registry.CurrentUser.DeleteSubKeyTree(Settings.RegistryKey + "\\DocumentAttachment", throwOnMissingSubKey: false);
		Registry.CurrentUser.DeleteSubKeyTree(Settings.RegistryKey + "\\UserReportWindow", throwOnMissingSubKey: false);
		Registry.CurrentUser.DeleteSubKeyTree(Settings.RegistryKey + "\\ImportWindow", throwOnMissingSubKey: false);
		Registry.CurrentUser.DeleteSubKeyTree(Settings.RegistryKey + "\\NavigatorDesignerWindow", throwOnMissingSubKey: false);
		Registry.CurrentUser.DeleteSubKeyTree(Settings.RegistryKey + "\\AccessManagementWindow", throwOnMissingSubKey: false);
	}

	public static string ReadExportDirectory(string defaultDirectory = null)
	{
		return ReadDirectory("ExportDirectory", defaultDirectory);
	}

	public static void WriteExportDirectory(string exportDirectory)
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
		registryKey?.SetValue("ExportDirectory", exportDirectory);
	}

	public static string ReadImportDirectory(string defaultDirectory = null)
	{
		return ReadDirectory("ImportDirectory", defaultDirectory);
	}

	public static void WriteImportDirectory(string importDirectory)
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
		registryKey?.SetValue("ImportDirectory", importDirectory);
	}

	private static string ReadDirectory(string registryName, string defaultDirectory = null)
	{
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey, writable: false);
		if (defaultDirectory == null)
		{
			defaultDirectory = Directory.GetCurrentDirectory();
		}
		if (registryKey != null)
		{
			string text = (string)registryKey.GetValue(registryName, defaultDirectory);
			if (Directory.Exists(text))
			{
				return text;
			}
		}
		return defaultDirectory;
	}

	public static void StoreToRegistry(string key, object value, string subKeyName = "")
	{
		string text = Settings.RegistryKey;
		if (!string.IsNullOrWhiteSpace(subKeyName))
		{
			text = text + "\\" + subKeyName;
		}
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(text);
		registryKey?.SetValue(key, Convert.ToString(value));
	}

	public static void StoreToRegistry(string key, Dictionary<string, string> value, string subKeyName = "")
	{
		string text = Settings.RegistryKey;
		if (!string.IsNullOrWhiteSpace(subKeyName))
		{
			text = text + "\\" + subKeyName;
		}
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(text);
		registryKey?.SetValue(key, JsonSerializer.Serialize(value));
	}

	public static bool ReadBoolFromRegistry(string key, string subKeyName = "", bool defaultValue = false)
	{
		string text = Settings.RegistryKey;
		if (!string.IsNullOrWhiteSpace(subKeyName))
		{
			text = text + "\\" + subKeyName;
		}
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(text, writable: false);
		if (registryKey == null)
		{
			return defaultValue;
		}
		object value = registryKey.GetValue(key, null);
		if (value == null)
		{
			return defaultValue;
		}
		return Convert.ToBoolean(value);
	}

	public static string ReadStringFromRegistry(string key, string subKeyName = "", string defaultValue = "")
	{
		string text = Settings.RegistryKey;
		if (!string.IsNullOrWhiteSpace(subKeyName))
		{
			text = text + "\\" + subKeyName;
		}
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(text, writable: false);
		if (registryKey == null)
		{
			return defaultValue;
		}
		object value = registryKey.GetValue(key);
		if (value == null)
		{
			return defaultValue;
		}
		return Convert.ToString(value);
	}

	public static Dictionary<string, string> ReadDictionaryFromRegistry(string key, string subKeyName = "")
	{
		string text = Settings.RegistryKey;
		if (!string.IsNullOrWhiteSpace(subKeyName))
		{
			text = text + "\\" + subKeyName;
		}
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(text, writable: false);
		if (registryKey == null)
		{
			return new Dictionary<string, string>();
		}
		string text2 = Convert.ToString(registryKey.GetValue(key));
		if (string.IsNullOrWhiteSpace(text2))
		{
			return new Dictionary<string, string>();
		}
		return JsonSerializer.Deserialize<Dictionary<string, string>>(text2);
	}

	public static int ReadIntFromRegistry(string key, string subKeyName = "", int defaultValue = 0)
	{
		string text = Settings.RegistryKey;
		if (!string.IsNullOrWhiteSpace(subKeyName))
		{
			text = text + "\\" + subKeyName;
		}
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(text, writable: false);
		if (registryKey == null)
		{
			return defaultValue;
		}
		object value = registryKey.GetValue(key);
		if (value == null)
		{
			return defaultValue;
		}
		return Convert.ToInt32(value);
	}

	public static double ReadDoubleFromRegistry(string key, string subKeyName = "", double defaultValue = 0.0)
	{
		string text = Settings.RegistryKey;
		if (!string.IsNullOrWhiteSpace(subKeyName))
		{
			text = text + "\\" + subKeyName;
		}
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(text, writable: false);
		if (registryKey == null)
		{
			return defaultValue;
		}
		object value = registryKey.GetValue(key);
		if (value == null)
		{
			return defaultValue;
		}
		return Convert.ToDouble(value);
	}

	public static void StoreStringArrayToRegistry(string key, string[] values, string subKeyName = "")
	{
		string text = Settings.RegistryKey;
		if (!string.IsNullOrWhiteSpace(subKeyName))
		{
			text = text + "\\" + subKeyName;
		}
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(text);
		registryKey?.SetValue(key, values, RegistryValueKind.MultiString);
	}

	public static string[] ReadStringArrayFromRegistry(string key, string subKeyName = "")
	{
		string text = Settings.RegistryKey;
		if (!string.IsNullOrWhiteSpace(subKeyName))
		{
			text = text + "\\" + subKeyName;
		}
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(text, writable: false);
		if (registryKey == null)
		{
			return Array.Empty<string>();
		}
		object value = registryKey.GetValue(key);
		if ((value == null || !(value is string[])) ? true : false)
		{
			return Array.Empty<string>();
		}
		return (string[])value;
	}
}
