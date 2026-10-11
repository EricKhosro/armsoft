using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "DaysOfWeek")]
public class DaysOfWeekSE : ASSmartEnum<DaysOfWeekSE, int>
{
	public static readonly DaysOfWeekSE Monday = new DaysOfWeekSE("Monday", 1, () => Resources.DayOfWeekString1);

	public static readonly DaysOfWeekSE Tuesday = new DaysOfWeekSE("Tuesday", 2, () => Resources.DayOfWeekString2);

	public static readonly DaysOfWeekSE Wednesday = new DaysOfWeekSE("Wednesday", 3, () => Resources.DayOfWeekString3);

	public static readonly DaysOfWeekSE Thursday = new DaysOfWeekSE("Thursday", 4, () => Resources.DayOfWeekString4);

	public static readonly DaysOfWeekSE Friday = new DaysOfWeekSE("Friday", 5, () => Resources.DayOfWeekString5);

	public static readonly DaysOfWeekSE Saturday = new DaysOfWeekSE("Saturday", 6, () => Resources.DayOfWeekString6);

	public static readonly DaysOfWeekSE Sunday = new DaysOfWeekSE("Sunday", 7, () => Resources.DayOfWeekString7);

	public DaysOfWeekSE(string name, int value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
