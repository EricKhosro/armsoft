using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Client.Models.NavigatorTree;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NavigatorTreeNodeType8X
{
	Folder,
	[EnumMember(Value = "Document")]
	Document,
	[EnumMember(Value = "DataView")]
	DataView,
	[EnumMember(Value = "TreeBrowser")]
	TreeBrowser,
	[EnumMember(Value = "TreeRef")]
	TreeRef,
	[EnumMember(Value = "UserReport")]
	UserReport,
	[EnumMember(Value = "ModuleCall")]
	ModuleCall
}
