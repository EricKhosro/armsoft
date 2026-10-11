using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using ArmSoft.SME.Core.AddIn;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.AddIn;

public static class TradeSettings
{
	public static TradeExcelConnector Connector;

	public static Application ExcelApplication;

	public static string AddInPath;

	public static string DateShortFormat;

	public static string DateLongFormat;

	public static DateTime gCurrentDate;

	public static DateTime gDateBegin;

	public static DateTime gDateEnd;

	public static int CalcMode;

	public static CultureInfo AM_Culture;

	public static CultureInfo EN_Culture;

	public static CultureInfo AR_Culture;

	public static CultureInfo GE_Culture;

	public static int gLang;

	public static int gPrecision;

	public static int gDivider;

	public static bool CalcAll;

	private static HttpClient httpClient;

	public static string ProgramName
	{
		get
		{
			if (gLang == 1)
			{
				return "ՀԾ Առևտուր";
			}
			return "AS Trade";
		}
	}

	static TradeSettings()
	{
		gLang = 1;
		gPrecision = 2;
		gDivider = 1;
		httpClient = new HttpClient();
	}

	public static TradeAddInClient CreateApiClient()
	{
		return new TradeAddInClient(Settings.ServiceAddress, Settings.Token, httpClient);
	}

	public static void SetCultures()
	{
		RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.ExcelAddInRegistryKey);
		Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
		string[] valueNames = registryKey.GetValueNames();
		bool flag = false;
		if (valueNames.Length != 0)
		{
			for (int i = 0; i < valueNames.Length; i++)
			{
				if (valueNames[i] == "Lang")
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			if (int.TryParse(registryKey.GetValue("Lang").ToString(), out var result))
			{
				gLang = result;
			}
			else
			{
				gLang = 1;
			}
		}
		else
		{
			gLang = 1;
		}
		registryKey.Close();
		AM_Culture = new CultureInfo("HY-AM");
		AM_Culture.DateTimeFormat.DateSeparator = "/";
		AM_Culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
		AM_Culture.DateTimeFormat.LongTimePattern = "H:mm:ss";
		AM_Culture.DateTimeFormat.ShortTimePattern = "H:mm";
		EN_Culture = new CultureInfo("");
		EN_Culture.DateTimeFormat.ShortDatePattern = AM_Culture.DateTimeFormat.ShortDatePattern;
		EN_Culture.DateTimeFormat.LongTimePattern = AM_Culture.DateTimeFormat.LongTimePattern;
		EN_Culture.DateTimeFormat.ShortTimePattern = AM_Culture.DateTimeFormat.ShortTimePattern;
		SetCurrentThreadCulture();
		DateShortFormat = "dd/MM/yy";
		DateLongFormat = "dd/MM/yyyy";
		gCurrentDate = DateTime.Today;
		gDateBegin = new DateTime(Thread.CurrentThread.CurrentCulture.Calendar.GetYear(gCurrentDate), Thread.CurrentThread.CurrentCulture.Calendar.GetMonth(gCurrentDate), 1);
		gDateEnd = gCurrentDate;
	}

	public static void SetCurrentThreadCulture()
	{
		try
		{
			Thread currentThread = Thread.CurrentThread;
			if (gLang == 1)
			{
				currentThread.CurrentUICulture = AM_Culture;
				currentThread.CurrentCulture = AM_Culture;
				CultureInfo.DefaultThreadCurrentCulture = AM_Culture;
				CultureInfo.DefaultThreadCurrentUICulture = AM_Culture;
			}
			else
			{
				currentThread.CurrentCulture = EN_Culture;
				currentThread.CurrentUICulture = EN_Culture;
				CultureInfo.DefaultThreadCurrentCulture = EN_Culture;
				CultureInfo.DefaultThreadCurrentUICulture = EN_Culture;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool IsConnected()
	{
		return Settings.IsLoggedIn;
	}
}
