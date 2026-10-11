using System.Runtime.Serialization;

namespace ArmSoft.b2b.V2;

public enum SuggestionStatus
{
	[EnumMember(Value = "Info")]
	Info,
	[EnumMember(Value = "Warning")]
	Warning,
	[EnumMember(Value = "Critical")]
	Critical,
	[EnumMember(Value = "Auto")]
	Auto
}
