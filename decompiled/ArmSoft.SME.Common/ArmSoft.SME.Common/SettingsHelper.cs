using System;
using System.Globalization;
using System.Threading;

namespace ArmSoft.SME.Common;

public static class SettingsHelper
{
	private static bool mIsOSVersionWindows8OrGreater;

	private static bool mIsOSVersionWindows8OrGreaterInitialize;

	public static bool InList<T>(T sValue, params T[] lValues)
	{
		if (sValue != null)
		{
			foreach (T val in lValues)
			{
				if (sValue.Equals(val))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static CultureInfo GetResourcesCulture(bool isPrintMode = true)
	{
		switch (Thread.CurrentThread.CurrentUICulture.Name.ToUpper())
		{
		case "EN-US":
			return new CultureInfo("en-US");
		case "HY-AM":
			return new CultureInfo("hy-AM");
		case "AR-SY":
			return new CultureInfo("ar-SY");
		case "KA-GE":
			return new CultureInfo("ka-GE");
		case "RU-RU":
			return new CultureInfo("ru-RU");
		default:
			if (isPrintMode)
			{
				return new CultureInfo("hy-AM");
			}
			return new CultureInfo("");
		}
	}

	public static bool IsOSVersionWindows8OrGreater()
	{
		if (!mIsOSVersionWindows8OrGreaterInitialize)
		{
			OperatingSystem oSVersion = Environment.OSVersion;
			mIsOSVersionWindows8OrGreater = oSVersion.Platform == PlatformID.Win32NT && (oSVersion.Version.Major > 6 || (oSVersion.Version.Major == 6 && oSVersion.Version.Minor >= 2));
			mIsOSVersionWindows8OrGreaterInitialize = true;
		}
		return mIsOSVersionWindows8OrGreater;
	}

	public static bool IsOSVersionWindows7OrGreater()
	{
		OperatingSystem oSVersion = Environment.OSVersion;
		if (oSVersion.Platform == PlatformID.Win32NT)
		{
			if (oSVersion.Version.Major <= 6)
			{
				if (oSVersion.Version.Major == 6)
				{
					return oSVersion.Version.Minor >= 1;
				}
				return false;
			}
			return true;
		}
		return false;
	}
}
