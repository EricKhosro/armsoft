using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "TimeMrsUnit")]
public class DatePeriodTypeSE : ASSmartEnum<DatePeriodTypeSE, short>
{
	public static readonly DatePeriodTypeSE Day = new DatePeriodTypeSE("Day", 0, () => ArmSoft.SME.Common.Resources.Day);

	public static readonly DatePeriodTypeSE Month = new DatePeriodTypeSE("Month", 1, () => ArmSoft.SME.Common.Resources.Month);

	public static readonly DatePeriodTypeSE Year = new DatePeriodTypeSE("Year", 2, () => ArmSoft.SME.Common.Resources.Year);

	public static readonly DatePeriodTypeSE Week = new DatePeriodTypeSE("Week", 3, () => ArmSoft.SME.Common.Resources.Week);

	public DatePeriodTypeSE(string name, short value, Func<string> captionDelegate)
		: base(name, value, captionDelegate)
	{
	}
}
