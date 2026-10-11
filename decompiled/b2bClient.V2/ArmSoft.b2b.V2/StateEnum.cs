using System.Runtime.Serialization;

namespace ArmSoft.b2b.V2;

public enum StateEnum
{
	[EnumMember(Value = "Undefined")]
	Undefined = 0,
	[EnumMember(Value = "Proposed")]
	Proposed = 1,
	[EnumMember(Value = "Base")]
	Base = 2,
	[EnumMember(Value = "New")]
	New = 4,
	[EnumMember(Value = "Closed")]
	Closed = 8,
	[EnumMember(Value = "Pending")]
	Pending = 0x10
}
