using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class OnlineContentPermissionDTO
{
	private string contentCodeField;

	private string descField;

	private string permissionCodeField;

	private string typeField;

	[XmlElement(Order = 0)]
	public string ContentCode
	{
		get
		{
			return contentCodeField;
		}
		set
		{
			contentCodeField = value;
		}
	}

	[XmlElement(Order = 1)]
	public string Desc
	{
		get
		{
			return descField;
		}
		set
		{
			descField = value;
		}
	}

	[XmlElement(Order = 2)]
	public string PermissionCode
	{
		get
		{
			return permissionCodeField;
		}
		set
		{
			permissionCodeField = value;
		}
	}

	[XmlElement(Order = 3)]
	public string Type
	{
		get
		{
			return typeField;
		}
		set
		{
			typeField = value;
		}
	}
}
