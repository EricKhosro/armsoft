using System;
using System.Globalization;
using System.Threading;

namespace ArmSoft.SME.Common.Extensions;

public static class DateOnlyExtension
{
	public static DateTime ToDateTime(this DateOnly value)
	{
		return value.ToDateTime(TimeOnly.Parse("12:00:00 AM"));
	}

	public static DateTime? ToDateTime(this DateOnly? value)
	{
		if (!value.HasValue)
		{
			return null;
		}
		return value.Value.ToDateTime(TimeOnly.Parse("12:00:00 AM"));
	}

	public static string FormatYYYYMMDD(this DateOnly value)
	{
		return value.Year.ToString("0000") + value.Month.ToString("00") + value.Day.ToString("00");
	}

	public static string FormatDDMMYY(this DateOnly value)
	{
		return value.Day.ToString("00") + "/" + value.Month.ToString("00") + "/" + ("00" + value.Year).Right(2);
	}

	public static string FormatDDMMYYYY(this DateOnly value)
	{
		return value.Day.ToString("00") + "/" + value.Month.ToString("00") + "/" + ("0000" + value.Year).Right(4);
	}

	public static string FormatYYYYMMDD(this DateOnly? value)
	{
		if (value.HasValue)
		{
			return value.Value.FormatYYYYMMDD();
		}
		return "00000000";
	}

	public static string FormatDDMMYY(this DateOnly? value)
	{
		if (value.HasValue)
		{
			return value.Value.FormatDDMMYY();
		}
		return "  /  /  ";
	}

	public static string FormatDDMMYYYY(this DateOnly? value)
	{
		if (value.HasValue)
		{
			return value.Value.FormatDDMMYYYY();
		}
		return "  /  /    ";
	}

	public static DateOnly WeekStart(this DateOnly value)
	{
		int num = (int)(value.DayOfWeek - 1);
		if (num < 0)
		{
			num += 7;
		}
		return value.AddDays(-num);
	}

	public static DateOnly WeekEnd(this DateOnly value)
	{
		return value.WeekStart().AddDays(6);
	}

	public static DateOnly MonthStart(this DateOnly source)
	{
		return new DateOnly(source.Year, source.Month, 1);
	}

	public static DateOnly MonthEnd(this DateOnly source)
	{
		return new DateOnly(source.Year, source.Month, DateTime.DaysInMonth(source.Year, source.Month));
	}

	public static DateOnly YearEnd(this DateOnly source)
	{
		return new DateOnly(source.Year, 12, 31);
	}

	public static DateOnly YearStart(this DateOnly source)
	{
		return new DateOnly(source.Year, 1, 1);
	}

	public static string DateToSymbol(DateOnly value, bool shortMode = false, bool isPrintMode = true)
	{
		string text = ("0" + value.Day).Right(2);
		string text2 = Thread.CurrentThread.CurrentUICulture.Name.ToUpper();
		if (!(text2 == "HY-AM") && !(text2 == "KA-GE") && !isPrintMode)
		{
			return value.MonthName(shortMode, isPrintMode) + " " + text + ", " + value.Year;
		}
		CultureInfo resourcesCulture = SettingsHelper.GetResourcesCulture(isPrintMode);
		return "\"" + text + "\" " + MonthName(value.Month, resourcesCulture, shortMode) + " " + value.Year + Resources.DtocShortYear;
	}

	public static string MonthName(this DateOnly date, bool shortMode = false, bool isPrintMode = true)
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

	public static long DateDiff(DateExtension.Interval interval, DateOnly date1, DateOnly date2, DayOfWeek dayOfWeek = DayOfWeek.Sunday)
	{
		return DateExtension.DateDiff(interval, date1.ToDateTime(), date2.ToDateTime(), dayOfWeek);
	}

	public static DateOnly DateSerial(int year, int month, int day)
	{
		return new DateOnly(year, 1, 1).AddMonths(month - 1).AddDays(day - 1);
	}

	public static int WeekDayFromMonday(this DateOnly date)
	{
		if (date.DayOfWeek != DayOfWeek.Sunday)
		{
			return Convert.ToInt32(date.DayOfWeek);
		}
		return 7;
	}
}
