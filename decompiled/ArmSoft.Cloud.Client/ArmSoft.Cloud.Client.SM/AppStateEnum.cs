using System.Runtime.Serialization;

namespace ArmSoft.Cloud.Client.SM;

public enum AppStateEnum
{
	[EnumMember(Value = "Undefined")]
	Undefined = 0,
	[EnumMember(Value = "Proposed")]
	Proposed = 1,
	[EnumMember(Value = "Sent")]
	Sent = 2,
	[EnumMember(Value = "Confirmed")]
	Confirmed = 4,
	[EnumMember(Value = "Rejected")]
	Rejected = 8,
	[EnumMember(Value = "Pending")]
	Pending = 0x10
}
