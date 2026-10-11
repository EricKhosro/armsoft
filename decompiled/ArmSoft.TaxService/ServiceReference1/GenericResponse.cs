using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[XmlInclude(typeof(GetBookDeclsCountResponse))]
[XmlInclude(typeof(GetBookDeclsResponse))]
[XmlInclude(typeof(GetAccDocListCountResponse))]
[XmlInclude(typeof(GetListCountResponse))]
[XmlInclude(typeof(GetAccDocsResponse))]
[XmlInclude(typeof(GetInvoicesResponse))]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GenericResponse
{
	private Status statusField;

	[XmlElement(Order = 0)]
	public Status Status
	{
		get
		{
			return statusField;
		}
		set
		{
			statusField = value;
		}
	}
}
