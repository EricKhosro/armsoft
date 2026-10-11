using System;
using System.Collections.Generic;

namespace ArmSoft.b2b.V2;

public class Calendar
{
	public Dictionary<DateTime, DayType> SpecialDates { get; set; }

	public List<TimeSheet> TimeSheet { get; set; }
}
