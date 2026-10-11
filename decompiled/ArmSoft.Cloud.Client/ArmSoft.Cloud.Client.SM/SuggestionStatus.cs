using System.Runtime.Serialization;

namespace ArmSoft.Cloud.Client.SM;

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
