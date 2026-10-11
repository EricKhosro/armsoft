using System;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "Dates")]
public class SpecialPeriodsSE : ASSmartEnum<SpecialPeriodsSE, string>
{
	public static readonly SpecialPeriodsSE Today = new SpecialPeriodsSE("1", DateTime.Today.FormatDDMMYY() + " - " + DateTime.Today.FormatDDMMYY(), () => Resources.Today);

	public static readonly SpecialPeriodsSE CurrentWeek = new SpecialPeriodsSE("2", SpecialDates.WEEK_BEGIN().FormatDDMMYY() + " - " + SpecialDates.WEEK_END().FormatDDMMYY(), () => Resources.CurrentWeek);

	public static readonly SpecialPeriodsSE CurrentWeekToDate = new SpecialPeriodsSE("3", SpecialDates.WEEK_BEGIN().FormatDDMMYY() + " - " + DateTime.Today.FormatDDMMYY(), () => Resources.CurrentWeekToDate);

	public static readonly SpecialPeriodsSE CurrentMonth = new SpecialPeriodsSE("4", SpecialDates.MONTH_BEGIN().FormatDDMMYY() + " - " + SpecialDates.MONTH_END().FormatDDMMYY(), () => Resources.CurrentMonth);

	public static readonly SpecialPeriodsSE CurrentMonthToDate = new SpecialPeriodsSE("5", SpecialDates.MONTH_BEGIN().FormatDDMMYY() + " - " + DateTime.Today.FormatDDMMYY(), () => Resources.CurrentMonthToDate);

	public static readonly SpecialPeriodsSE CurrentQuarter = new SpecialPeriodsSE("6", SpecialDates.QUARTE_BEGIN().FormatDDMMYY() + " - " + SpecialDates.QUARTE_END().FormatDDMMYY(), () => Resources.CurrentQuarter);

	public static readonly SpecialPeriodsSE CurrentQuarterToDate = new SpecialPeriodsSE("7", SpecialDates.QUARTE_BEGIN().FormatDDMMYY() + " - " + DateTime.Today.FormatDDMMYY(), () => Resources.CurrentQuarterToDate);

	public static readonly SpecialPeriodsSE CurrentYear = new SpecialPeriodsSE("8", SpecialDates.YEAR_BEGIN().FormatDDMMYY() + " - " + SpecialDates.YEAR_END().FormatDDMMYY(), () => Resources.CurrentYear);

	public static readonly SpecialPeriodsSE CurrentYearToDate = new SpecialPeriodsSE("9", SpecialDates.YEAR_BEGIN().FormatDDMMYY() + " - " + DateTime.Today.FormatDDMMYY(), () => Resources.CurrentYearToDate);

	public static readonly SpecialPeriodsSE Yesterday = new SpecialPeriodsSE("10", DateTime.Today.AddDays(-1.0).FormatDDMMYY() + " - " + DateTime.Today.AddDays(-1.0).FormatDDMMYY(), () => Resources.Yesterday);

	public static readonly SpecialPeriodsSE PreviousWeek = new SpecialPeriodsSE("11", SpecialDates.PREVIOUS_WEEK_BEGIN().FormatDDMMYY() + " - " + SpecialDates.PREVIOUS_WEEK_END().FormatDDMMYY(), () => Resources.PreviousWeek);

	public static readonly SpecialPeriodsSE PreviousMonth = new SpecialPeriodsSE("12", SpecialDates.PREVIOUS_MONTH_BEGIN().FormatDDMMYY() + " - " + SpecialDates.PREVIOUS_MONTH_END().FormatDDMMYY(), () => Resources.PreviousMonth);

	public static readonly SpecialPeriodsSE PreviousQuarter = new SpecialPeriodsSE("13", SpecialDates.PREVIOUS_QUARTE_BEGIN().FormatDDMMYY() + " - " + SpecialDates.PREVIOUS_QUARTE_END().FormatDDMMYY(), () => Resources.PreviousQuarter);

	public static readonly SpecialPeriodsSE PreviousYear = new SpecialPeriodsSE("14", SpecialDates.PREVIOUS_YEAR_BEGIN().FormatDDMMYY() + " - " + SpecialDates.PREVIOUS_YEAR_END().FormatDDMMYY(), () => Resources.PreviousYear);

	public static readonly SpecialPeriodsSE LastWeek = new SpecialPeriodsSE("15", SpecialDates.LAST_X_DAY_INTERVAL_BEGIN(7.0).FormatDDMMYY() + " - " + DateTime.Today.FormatDDMMYY(), () => string.Format(Resources.LastXDays, (short)7));

	public static readonly SpecialPeriodsSE LastMonth = new SpecialPeriodsSE("16", SpecialDates.LAST_X_DAY_INTERVAL_BEGIN(30.0).FormatDDMMYY() + " - " + DateTime.Today.FormatDDMMYY(), () => string.Format(Resources.LastXDays, (short)30));

	public static readonly SpecialPeriodsSE LastQuarter = new SpecialPeriodsSE("17", SpecialDates.LAST_X_DAY_INTERVAL_BEGIN(90.0).FormatDDMMYY() + " - " + DateTime.Today.FormatDDMMYY(), () => string.Format(Resources.LastXDays, (short)90));

	public static readonly SpecialPeriodsSE LastYear = new SpecialPeriodsSE("18", SpecialDates.LAST_X_DAY_INTERVAL_BEGIN(365.0).FormatDDMMYY() + " - " + DateTime.Today.FormatDDMMYY(), () => string.Format(Resources.LastXDays, (short)365));

	public SpecialPeriodsSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
