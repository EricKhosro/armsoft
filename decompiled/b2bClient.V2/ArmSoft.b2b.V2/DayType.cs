using System.Runtime.Serialization;

namespace ArmSoft.b2b.V2;

public enum DayType
{
	[EnumMember(Value = "Undefined")]
	Undefined,
	[EnumMember(Value = "Holiday")]
	Holiday,
	[EnumMember(Value = "PreHoliday")]
	PreHoliday,
	[EnumMember(Value = "Workday")]
	Workday,
	[EnumMember(Value = "WorkHoliday")]
	WorkHoliday
}
