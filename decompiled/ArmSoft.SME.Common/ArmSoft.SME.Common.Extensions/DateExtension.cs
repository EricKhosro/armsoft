using System;
using System.Globalization;
using System.Threading;

namespace ArmSoft.SME.Common.Extensions;

public static class DateExtension
{
	public enum Interval
	{
		Year,
		Quarter,
		Month,
		DayOfYear,
		Day,
		WeekOfYear,
		Weekday,
		Hour,
		Minute,
		Second
	}

	public static DateOnly? ToDateOnly(this DateTime? value)
	{
		if (value.HasValue)
		{
			return DateOnly.FromDateTime(value.Value);
		}
		return null;
	}

	public static DateOnly ToDateOnly(this DateTime value)
	{
		return DateOnly.FromDateTime(value);
	}

	public static string FormatYYYYMMDD(this DateTime value)
	{
		return value.Year.ToString("0000") + value.Month.ToString("00") + value.Day.ToString("00");
	}

	public static string FormatDDMMYY(this DateTime value)
	{
		return value.Day.ToString("00") + "/" + value.Month.ToString("00") + "/" + ("00" + value.Year).Right(2);
	}

	public static string FormatDDMMYYYY(this DateTime value)
	{
		return value.Day.ToString("00") + "/" + value.Month.ToString("00") + "/" + ("0000" + value.Year).Right(4);
	}

	public static string FormatYYYYMMDD(this DateTime? value)
	{
		if (value.HasValue)
		{
			return value.Value.FormatYYYYMMDD();
		}
		return "00000000";
	}

	public static string FormatDDMMYY(this DateTime? value)
	{
		if (value.HasValue)
		{
			return value.Value.FormatDDMMYY();
		}
		return "  /  /  ";
	}

	public static string FormatDDMMYYYY(this DateTime? value)
	{
		if (value.HasValue)
		{
			return value.Value.FormatDDMMYYYY();
		}
		return "  /  /    ";
	}

	public static string FormatHHMMSS(this DateTime value)
	{
		return value.Hour.ToString("00") + value.Minute.ToString("00") + value.Second.ToString("00");
	}

	public static DateTime WeekStart(this DateTime value)
	{
		int num = (int)(value.DayOfWeek - 1);
		if (num < 0)
		{
			num += 7;
		}
		return value.AddDays(-num).Date;
	}

	public static DateTime WeekEnd(this DateTime value)
	{
		return value.WeekStart().AddDays(6.0);
	}

	public static string DateToSymbol(DateTime value, bool shortMode = false, bool isPrintMode = true)
	{
		string text = ("0" + value.Day).Right(2);
		string text2 = Thread.CurrentThread.CurrentUICulture.Name.ToUpper();
		if (!(text2 == "HY-AM") && !(text2 == "KA-GE") && !isPrintMode)
		{
			return MonthName(value, shortMode, isPrintMode) + " " + text + ", " + value.Year;
		}
		CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
		return "\"" + text + "\" " + MonthName(value.Month, currentUICulture, shortMode) + " " + value.Year + Resources.DtocShortYear;
	}

	public static string MonthName(DateTime date, bool shortMode = false, bool isPrintMode = true)
	{
		CultureInfo resourcesCulture = SettingsHelper.GetResourcesCulture(isPrintMode);
		return MonthName(date.Month, resourcesCulture, shortMode);
	}

	public static string MonthName(int month, bool shortMode = false, bool isPrintMode = true)
	{
		CultureInfo resourcesCulture = SettingsHelper.GetResourcesCulture(isPrintMode);
		return MonthName(month, resourcesCulture, shortMode);
	}

	private static string MonthName(int month, CultureInfo resourcesCulture, bool shortMode)
	{
		string text = (shortMode ? "DtocShortString" : "DtocString");
		if (month >= 1 && month <= 9)
		{
			text = text + "0" + month;
		}
		else if (month >= 10 && month <= 12)
		{
			text += month;
		}
		else if (month >= 13 || month <= 0)
		{
			throw new ArgumentException("Argument 'Month' must be bettween 1 and 12.");
		}
		return (string)typeof(Resources).GetProperty(text)?.GetValue(text);
	}

	public static string DayOfWeekName(int dayOfWeek, bool shortMode = false, bool isPrintMode = false)
	{
		CultureInfo resourcesCulture = SettingsHelper.GetResourcesCulture(isPrintMode);
		return DayOfWeekName(dayOfWeek, resourcesCulture, shortMode);
	}

	private static string DayOfWeekName(int dayOfWeek, CultureInfo resourcesCulture, bool shortMode)
	{
		string text = (shortMode ? "DayOfWeekShortString" : "DayOfWeekString");
		text += dayOfWeek;
		return Convert.ToString(typeof(Resources).GetProperty(text)?.GetValue(text));
	}

	public static long DateDiff(Interval interval, DateTime date1, DateTime date2, DayOfWeek dayOfWeek = DayOfWeek.Sunday)
	{
		TimeSpan timeSpan = date2.Subtract(date1);
		Calendar calendar = Thread.CurrentThread.CurrentCulture.Calendar;
		switch (interval)
		{
		case Interval.Year:
			return calendar.GetYear(date2) - calendar.GetYear(date1);
		case Interval.Month:
			return (calendar.GetYear(date2) - calendar.GetYear(date1)) * 12 + calendar.GetMonth(date2) - calendar.GetMonth(date1);
		case Interval.DayOfYear:
		case Interval.Day:
			return (long)timeSpan.TotalDays;
		case Interval.Hour:
			return (long)timeSpan.TotalHours;
		case Interval.Minute:
			return (long)timeSpan.TotalMinutes;
		case Interval.Second:
			return (long)timeSpan.TotalSeconds;
		case Interval.WeekOfYear:
			date1 = date1.AddDays(-GetDayOfWeek(date1, dayOfWeek));
			date2 = date2.AddDays(-GetDayOfWeek(date2, dayOfWeek));
			return (long)date2.Subtract(date1).TotalDays / 7;
		case Interval.Weekday:
			return (long)timeSpan.TotalDays / 7;
		case Interval.Quarter:
			return (calendar.GetYear(date2) - calendar.GetYear(date1)) * 4 + (calendar.GetMonth(date2) - 1) / 3 - (calendar.GetMonth(date1) - 1) / 3;
		default:
			throw new ArgumentException("Argument_InvalidValue", "interval");
		}
	}

	private static int GetDayOfWeek(DateTime dt, DayOfWeek? weekdayFirst = null)
	{
		DayOfWeek valueOrDefault = weekdayFirst.GetValueOrDefault();
		if (!weekdayFirst.HasValue)
		{
			valueOrDefault = Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 1;
			weekdayFirst = valueOrDefault;
		}
		return (dt.DayOfWeek - weekdayFirst.Value + 8) % 7 + 1;
	}

	public static int WeekDayFromMonday(this DateTime date)
	{
		if (date.DayOfWeek != DayOfWeek.Sunday)
		{
			return Convert.ToInt32(date.DayOfWeek);
		}
		return 7;
	}

	public static DateTime? DDMMYYToNullableDate(this string source)
	{
		string value = source.Substring(0, 2);
		string value2 = source.Substring(3, 2);
		string value3 = source.Substring(6, 2);
		if (string.IsNullOrWhiteSpace(value) | string.IsNullOrWhiteSpace(value2) | string.IsNullOrWhiteSpace(value3))
		{
			return null;
		}
		return new DateTime(Convert.ToInt32(value3) + 2000, Convert.ToInt32(value2), Convert.ToInt32(value));
	}

	public static DateTime YYYYMMDDHHMMSSToDate(this string source)
	{
		string value = source.Substring(0, 4);
		string value2 = source.Substring(4, 2);
		string value3 = source.Substring(6, 2);
		string value4 = source.Substring(8, 2);
		string value5 = source.Substring(10, 2);
		return new DateTime(second: Convert.ToInt32(source.Substring(12, 2)), year: Convert.ToInt32(value), month: Convert.ToInt32(value2), day: Convert.ToInt32(value3), hour: Convert.ToInt32(value4), minute: Convert.ToInt32(value5));
	}

	public static DateTime? YYYYMMDDHHMMSSToNullableDate(this string source)
	{
		if (source == "00000000000000")
		{
			return null;
		}
		return source.YYYYMMDDHHMMSSToDate();
	}

	public static string NullableDateToYYYYMMDDHHMMSS(this DateTime? source)
	{
		if (source.HasValue)
		{
			return source.FormatYYYYMMDD() + source.Value.FormatHHMMSS();
		}
		return "00000000000000";
	}

	public static DateTime MonthStart(this DateTime source)
	{
		return new DateTime(source.Year, source.Month, 1);
	}

	public static DateTime MonthEnd(this DateTime source)
	{
		return source.AddMonths(1).AddDays(-1.0);
	}

	public static string FormatDDMMYYHHMM(this DateTime date)
	{
		return date.FormatDDMMYY() + " " + date.Hour.ToString("00") + ":" + date.Minute.ToString("00");
	}

	public static string FormatYYYYMMDDHHMMSS(this DateTime date)
	{
		return date.FormatYYYYMMDD() + date.FormatHHMMSS();
	}
}
