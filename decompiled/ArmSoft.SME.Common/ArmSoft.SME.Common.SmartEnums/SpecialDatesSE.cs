using System;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "Dates")]
public class SpecialDatesSE : ASSmartEnum<SpecialDatesSE, string>
{
	public static readonly SpecialDatesSE Today = new SpecialDatesSE("1", DateTime.Today.FormatDDMMYY(), () => Resources.Today);

	public static readonly SpecialDatesSE CurrentWeekBegin = new SpecialDatesSE("2", SpecialDates.WEEK_BEGIN().FormatDDMMYY(), () => Resources.CurrentWeekBegin);

	public static readonly SpecialDatesSE CurrentWeekEnd = new SpecialDatesSE("3", SpecialDates.WEEK_END().FormatDDMMYY(), () => Resources.CurrentWeekEnd);

	public static readonly SpecialDatesSE CurrentMonthBegin = new SpecialDatesSE("4", SpecialDates.MONTH_BEGIN().FormatDDMMYY(), () => Resources.CurrentMonthBegin);

	public static readonly SpecialDatesSE CurrentMonthEnd = new SpecialDatesSE("5", SpecialDates.MONTH_END().FormatDDMMYY(), () => Resources.CurrentMonthEnd);

	public static readonly SpecialDatesSE CurrentQuarterBegin = new SpecialDatesSE("6", SpecialDates.QUARTE_BEGIN().FormatDDMMYY(), () => Resources.CurrentQuarterBegin);

	public static readonly SpecialDatesSE CurrentQuarterEnd = new SpecialDatesSE("7", SpecialDates.QUARTE_END().FormatDDMMYY(), () => Resources.CurrentQuarterEnd);

	public static readonly SpecialDatesSE CurrentYearBegin = new SpecialDatesSE("8", SpecialDates.YEAR_BEGIN().FormatDDMMYY(), () => Resources.CurrentYearBegin);

	public static readonly SpecialDatesSE CurrentYearEnd = new SpecialDatesSE("9", SpecialDates.YEAR_END().FormatDDMMYY(), () => Resources.CurrentYearEnd);

	public static readonly SpecialDatesSE Yesterday = new SpecialDatesSE("10", DateTime.Today.AddDays(-1.0).FormatDDMMYY(), () => Resources.Yesterday);

	public static readonly SpecialDatesSE PreviousWeekBegin = new SpecialDatesSE("11", SpecialDates.PREVIOUS_WEEK_BEGIN().FormatDDMMYY(), () => Resources.PreviousWeekBegin);

	public static readonly SpecialDatesSE PreviousWeekEnd = new SpecialDatesSE("12", SpecialDates.PREVIOUS_WEEK_END().FormatDDMMYY(), () => Resources.PreviousWeekEnd);

	public static readonly SpecialDatesSE PreviousMonthBegin = new SpecialDatesSE("13", SpecialDates.PREVIOUS_MONTH_BEGIN().FormatDDMMYY(), () => Resources.PreviousMonthBegin);

	public static readonly SpecialDatesSE PreviousMonthEnd = new SpecialDatesSE("14", SpecialDates.PREVIOUS_MONTH_END().FormatDDMMYY(), () => Resources.PreviousMonthEnd);

	public static readonly SpecialDatesSE PreviousQuarterBegin = new SpecialDatesSE("15", SpecialDates.PREVIOUS_QUARTE_BEGIN().FormatDDMMYY(), () => Resources.PreviousQuarterBegin);

	public static readonly SpecialDatesSE PreviousQuarterEnd = new SpecialDatesSE("16", SpecialDates.PREVIOUS_QUARTE_END().FormatDDMMYY(), () => Resources.PreviousQuarterEnd);

	public static readonly SpecialDatesSE PreviousYearBegin = new SpecialDatesSE("17", SpecialDates.PREVIOUS_YEAR_BEGIN().FormatDDMMYY(), () => Resources.PreviousYearBegin);

	public static readonly SpecialDatesSE PreviousYearEnd = new SpecialDatesSE("18", SpecialDates.PREVIOUS_YEAR_END().FormatDDMMYY(), () => Resources.PreviousYearEnd);

	public SpecialDatesSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
