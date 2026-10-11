using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "Months")]
public class MonthsSE : ASSmartEnum<MonthsSE, string>
{
	public static readonly MonthsSE January = new MonthsSE("January", "01", () => Resources.January);

	public static readonly MonthsSE February = new MonthsSE("February", "02", () => Resources.February);

	public static readonly MonthsSE March = new MonthsSE("March", "03", () => Resources.March);

	public static readonly MonthsSE April = new MonthsSE("April", "04", () => Resources.April);

	public static readonly MonthsSE May = new MonthsSE("May", "05", () => Resources.May);

	public static readonly MonthsSE June = new MonthsSE("June", "06", () => Resources.June);

	public static readonly MonthsSE July = new MonthsSE("July", "07", () => Resources.July);

	public static readonly MonthsSE August = new MonthsSE("August", "08", () => Resources.August);

	public static readonly MonthsSE September = new MonthsSE("September", "09", () => Resources.September);

	public static readonly MonthsSE October = new MonthsSE("October", "10", () => Resources.October);

	public static readonly MonthsSE November = new MonthsSE("November", "11", () => Resources.November);

	public static readonly MonthsSE December = new MonthsSE("December", "12", () => Resources.December);

	public MonthsSE(string name, string value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
