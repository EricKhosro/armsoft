using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class OnlineUserPermissionDTO
{
	private long userIdField;

	private OnlineContentPermissionDTO[] contentPermissionListField;

	[XmlElement(Order = 0)]
	public long userId
	{
		get
		{
			return userIdField;
		}
		set
		{
			userIdField = value;
		}
	}

	[XmlElement("ContentPermissionList", Order = 1)]
	public OnlineContentPermissionDTO[] ContentPermissionList
	{
		get
		{
			return contentPermissionListField;
		}
		set
		{
			contentPermissionListField = value;
		}
	}
}
