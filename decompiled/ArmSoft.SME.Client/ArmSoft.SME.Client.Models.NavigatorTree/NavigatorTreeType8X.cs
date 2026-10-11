using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Client.Models.NavigatorTree;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NavigatorTreeType8X
{
	[EnumMember(Value = "Standard")]
	Standard,
	[EnumMember(Value = "Ref")]
	Ref,
	[EnumMember(Value = "User")]
	User
}
