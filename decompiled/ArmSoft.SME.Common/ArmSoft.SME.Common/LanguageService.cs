using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ArmSoft.SME.Common;

public static class LanguageService
{
	public const string ArmenianCultureName = "hy-AM";

	public const string RussianCultureName = "ru-RU";

	public const string EnglishCultureName = "en-US";

	public const string CultureRegKey = "Culture";

	public static readonly CultureInfo AM_Culture;

	public static readonly CultureInfo RU_Culture;

	public static readonly CultureInfo EN_Culture;

	public static Language Language
	{
		get
		{
			if (CultureInfo.CurrentUICulture.Name == "en-US")
			{
				return Language.English;
			}
			if (CultureInfo.CurrentUICulture.Name == "ru-RU")
			{
				return Language.Russian;
			}
			return Language.Armenian;
		}
		set
		{
			switch (value)
			{
			case Language.English:
				Thread.CurrentThread.CurrentCulture = EN_Culture;
				Thread.CurrentThread.CurrentUICulture = EN_Culture;
				CultureInfo.DefaultThreadCurrentCulture = EN_Culture;
				CultureInfo.DefaultThreadCurrentUICulture = EN_Culture;
				break;
			case Language.Russian:
				Thread.CurrentThread.CurrentCulture = RU_Culture;
				Thread.CurrentThread.CurrentUICulture = RU_Culture;
				CultureInfo.DefaultThreadCurrentCulture = RU_Culture;
				CultureInfo.DefaultThreadCurrentUICulture = RU_Culture;
				break;
			default:
				Thread.CurrentThread.CurrentCulture = AM_Culture;
				Thread.CurrentThread.CurrentUICulture = AM_Culture;
				CultureInfo.DefaultThreadCurrentCulture = AM_Culture;
				CultureInfo.DefaultThreadCurrentUICulture = AM_Culture;
				break;
			}
		}
	}

	public static bool IsEnglish => Language == Language.English;

	public static bool IsArmenian => Language == Language.Armenian;

	public static bool IsRussian => Language == Language.Russian;

	public static string CurrentUICultureName
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return CultureInfo.CurrentUICulture.TextInfo.CultureName;
		}
	}

	static LanguageService()
	{
		AM_Culture = CultureInfo.CreateSpecificCulture("hy-AM");
		DateTimeFormatInfo dateTimeFormat = AM_Culture.DateTimeFormat;
		dateTimeFormat.DateSeparator = "/";
		dateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
		dateTimeFormat.LongTimePattern = "H:mm:ss";
		dateTimeFormat.ShortTimePattern = "H:mm";
		NumberFormatInfo numberFormat = AM_Culture.NumberFormat;
		numberFormat.CurrencyDecimalSeparator = ".";
		numberFormat.PercentDecimalSeparator = ".";
		numberFormat.NumberDecimalSeparator = ".";
		numberFormat.CurrencyGroupSeparator = ",";
		numberFormat.PercentGroupSeparator = ",";
		numberFormat.NumberGroupSeparator = ",";
		RU_Culture = CultureInfo.CreateSpecificCulture("ru-RU");
		DateTimeFormatInfo dateTimeFormat2 = RU_Culture.DateTimeFormat;
		dateTimeFormat2.DateSeparator = "/";
		dateTimeFormat2.ShortDatePattern = "dd/MM/yyyy";
		dateTimeFormat2.LongTimePattern = "H:mm:ss";
		dateTimeFormat2.ShortTimePattern = "H:mm";
		NumberFormatInfo numberFormat2 = RU_Culture.NumberFormat;
		numberFormat2.CurrencyDecimalSeparator = ".";
		numberFormat2.PercentDecimalSeparator = ".";
		numberFormat2.NumberDecimalSeparator = ".";
		numberFormat2.CurrencyGroupSeparator = ",";
		numberFormat2.PercentGroupSeparator = ",";
		numberFormat2.NumberGroupSeparator = ",";
		EN_Culture = CultureInfo.CreateSpecificCulture("en-US");
		DateTimeFormatInfo dateTimeFormat3 = EN_Culture.DateTimeFormat;
		dateTimeFormat3.ShortDatePattern = AM_Culture.DateTimeFormat.ShortDatePattern;
		dateTimeFormat3.LongTimePattern = AM_Culture.DateTimeFormat.LongTimePattern;
		dateTimeFormat3.ShortTimePattern = AM_Culture.DateTimeFormat.ShortTimePattern;
	}

	public static void RestoreCulture(string cultureName)
	{
		if (cultureName == "en-US")
		{
			CultureInfo.CurrentCulture = EN_Culture;
			CultureInfo.CurrentUICulture = EN_Culture;
		}
		else if (cultureName == "ru-RU")
		{
			CultureInfo.CurrentCulture = RU_Culture;
			CultureInfo.CurrentUICulture = RU_Culture;
		}
		else
		{
			CultureInfo.CurrentCulture = AM_Culture;
			CultureInfo.CurrentUICulture = AM_Culture;
		}
	}

	public static Dictionary<string, CultureInfo> GetSupportedCultures()
	{
		return new Dictionary<string, CultureInfo>
		{
			{ "hy-AM", AM_Culture },
			{ "ru-RU", RU_Culture },
			{ "en-US", EN_Culture }
		};
	}
}
