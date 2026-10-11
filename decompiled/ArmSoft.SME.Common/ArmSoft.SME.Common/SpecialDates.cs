using System;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Common;

public class SpecialDates
{
	public static DateTime WEEK_BEGIN()
	{
		return DateTime.Today.WeekStart();
	}

	public static DateTime WEEK_END()
	{
		return DateTime.Today.WeekEnd();
	}

	public static DateTime MONTH_BEGIN()
	{
		DateTime today = DateTime.Today;
		return today.AddDays(1 - today.Day);
	}

	public static DateTime MONTH_END()
	{
		return MONTH_BEGIN().AddMonths(1).AddDays(-1.0);
	}

	public static DateTime PREVIOUS_MONTH_BEGIN()
	{
		return MONTH_BEGIN().AddMonths(-1);
	}

	public static DateTime PREVIOUS_MONTH_END()
	{
		return MONTH_BEGIN().AddDays(-1.0);
	}

	public static DateTime QUARTE_BEGIN()
	{
		DateTime result = MONTH_BEGIN();
		while (result.Month % 3 != 1)
		{
			result = result.AddMonths(-1);
		}
		return result;
	}

	public static DateTime QUARTE_END()
	{
		return QUARTE_BEGIN().AddMonths(3).AddDays(-1.0);
	}

	public static DateTime PREVIOUS_QUARTE_BEGIN()
	{
		return QUARTE_BEGIN().AddMonths(-3);
	}

	public static DateTime PREVIOUS_QUARTE_END()
	{
		return QUARTE_BEGIN().AddDays(-1.0);
	}

	public static DateTime YEAR_BEGIN()
	{
		return new DateTime(DateTime.Today.Year, 1, 1);
	}

	public static DateTime YEAR_END()
	{
		return new DateTime(DateTime.Today.Year, 12, 31);
	}

	public static DateTime PREVIOUS_YEAR_BEGIN()
	{
		return new DateTime(DateTime.Today.AddYears(-1).Year, 1, 1);
	}

	public static DateTime PREVIOUS_YEAR_END()
	{
		return new DateTime(DateTime.Today.AddYears(-1).Year, 12, 31);
	}

	public static DateTime PREVIOUS_WEEK_BEGIN()
	{
		return WEEK_BEGIN().AddDays(-7.0);
	}

	public static DateTime PREVIOUS_WEEK_END()
	{
		return WEEK_END().AddDays(-7.0);
	}

	public static DateTime LAST_X_DAY_INTERVAL_BEGIN(double x)
	{
		return DateTime.Today.AddDays(0.0 - x + 1.0);
	}
}
