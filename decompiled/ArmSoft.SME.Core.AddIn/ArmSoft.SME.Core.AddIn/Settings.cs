using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.AddIn;

public static class Settings
{
	public enum SystemTypes : short
	{
		Accountant = 1,
		Trade = 3,
		BookKeeper = 2,
		MarketPlace = 5,
		SalesManagement = 6,
		InternetTradeAdmin = 7
	}

	public enum Localizations : short
	{
		Armenia = 1,
		Georgia
	}

	private static string b2bCloudBaseAddress;

	private static string token;

	[CompilerGenerated]
	private static Localizations Localization__BackingField;

	private static int? processId;

	private static string tempPath;

	public static string CultureName;

	private const string _configFilePathRegKey = "ConfigFilePath";

	public static string CultureRegistryKey;

	public static SystemTypes SystemType { get; set; }

	public static Localizations Localization
	{
		[CompilerGenerated]
		get
		{
			return Localization__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			Localization__BackingField = value;
		}
	}

	public static string ServiceAddress { get; set; }

	public static string IdToken { get; set; }

	public static string UserName { get; set; }

	public static string Database { get; set; }

	public static int DatabaseId { get; set; }

	public static bool IsLoggedIn { get; set; }

	public static bool DemoMode { get; set; }

	public static string ASAdminB2CToken { get; set; }

	public static Version AppVersion => Assembly.GetExecutingAssembly().GetName().Version;

	public static string AppVersionString => $"{AppVersion?.Major}.{AppVersion?.Minor}.{AppVersion?.Build}";

	public static string ConfigFileFullName { get; set; }

	public static bool ASAdminLogin { get; set; }

	public static string ASAdminLoginBaseURL { get; set; }

	public static string RegistryKey
	{
		get
		{
			string text = "Software\\Armenian Software\\";
			switch (SystemType)
			{
			case SystemTypes.Trade:
				text += "AS Trade 9";
				break;
			case SystemTypes.Accountant:
				text += "AS Accountant 9";
				break;
			case SystemTypes.SalesManagement:
				text += "AS SalesManagement 9";
				break;
			}
			return text;
		}
	}

	public static string ExcelAddInRegistryKey => RegistryKey + "\\ExcelAddIn";

	public static string B2BCloudBaseAddress
	{
		get
		{
			if (string.IsNullOrWhiteSpace(b2bCloudBaseAddress))
			{
				b2bCloudBaseAddress = "https://cloud.armsoft.am/";
			}
			return b2bCloudBaseAddress;
		}
	}

	public static string Token
	{
		get
		{
			if (string.IsNullOrWhiteSpace(token))
			{
				GetToken();
			}
			return token;
		}
	}

	static Settings()
	{
		Localization__BackingField = Localizations.Armenia;
		CultureRegistryKey = "Culture";
		tempPath = Path.GetTempPath() + tempPath + Path.DirectorySeparatorChar;
	}

	public static string GetCurrentCultureName(bool getFromRegistry = false)
	{
		if (string.IsNullOrWhiteSpace(CultureName) | getFromRegistry)
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RegistryKey, writable: false);
			if (registryKey == null)
			{
				CultureName = "hy-AM";
			}
			else
			{
				CultureName = (string)registryKey.GetValue(CultureRegistryKey, "");
				if (string.IsNullOrWhiteSpace(CultureName))
				{
					CultureName = "hy-AM";
				}
			}
		}
		return CultureName;
	}

	public static int ProcessId()
	{
		if (!processId.HasValue)
		{
			using Process process = Process.GetCurrentProcess();
			processId = process.Id;
		}
		return processId.Value;
	}

	private static void GetToken()
	{
		ApiClient apiClient = new ApiClient(ServiceAddress, "", new HttpClient());
		AuthenticateResponse authenticateResponse = null;
		if (ASAdminLogin)
		{
			authenticateResponse = apiClient.Authentication.AuthenticateACS(ASAdminB2CToken, DatabaseId);
		}
		else if (!string.IsNullOrWhiteSpace(IdToken))
		{
			authenticateResponse = apiClient.Authentication.AuthenticateB2C(IdToken, DatabaseId, AppVersionString, DemoMode);
		}
		if (authenticateResponse != null)
		{
			if (ASAdminLogin)
			{
				SetUserInfoForASAdmin(authenticateResponse);
			}
			token = authenticateResponse.AccessToken;
		}
		else
		{
			token = "";
		}
	}

	public static void ClearToken()
	{
		token = "";
	}

	public static void ClearLoginInfo()
	{
		ServiceAddress = "";
		IdToken = "";
		UserName = "";
		Database = "";
		DatabaseId = 0;
		token = "";
	}

	public static void InitData()
	{
		ConfigFileFullName = GetConfigFileFullName();
		ASAdminLogin = Convert.ToBoolean(GetConfigValue("ASAdminLogin"));
		ASAdminLoginBaseURL = GetConfigValue("ASAdminLoginBaseURL") ?? "";
	}

	private static string GetConfigFileFullName()
	{
		string result = null;
		using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RegistryKey))
		{
			if (registryKey != null)
			{
				string text = (string)registryKey.GetValue("ConfigFilePath", "");
				if (!string.IsNullOrWhiteSpace(text))
				{
					if (text.Contains("Debug"))
					{
						text = Directory.GetParent(text).FullName;
						text = Directory.GetParent(text).FullName;
						text = Directory.GetParent(text).FullName;
						result = Path.Combine(text, "appsettings.Development.json");
					}
					else
					{
						result = Path.Combine(text, "appsettings.json");
					}
				}
				else
				{
					result = "";
				}
			}
			else
			{
				result = "";
			}
		}
		return result;
	}

	public static void SetUserInfoForASAdmin(AuthenticateResponse userLoginResponse)
	{
		UserName = userLoginResponse.UserName;
	}

	public static string GetTempPath()
	{
		if (!Directory.Exists(tempPath))
		{
			Directory.CreateDirectory(tempPath);
		}
		return tempPath;
	}

	public static string GetConfigValue(string settingName)
	{
		try
		{
			if (File.Exists(ConfigFileFullName))
			{
				using JsonDocument jsonDocument = JsonDocument.Parse(Regex.Replace(File.ReadAllText(ConfigFileFullName), "^\\s*//.*$", "", RegexOptions.Multiline));
				if (jsonDocument.RootElement.TryGetProperty(settingName, out var value))
				{
					return value.ToString();
				}
			}
		}
		catch (Exception)
		{
		}
		return null;
	}
}
