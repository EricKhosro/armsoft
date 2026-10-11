using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GetAccDocsRequest : GenericAuthenticatedRequest
{
	private AccDocFilter accDocFilterField;

	[XmlElement(Order = 0)]
	public AccDocFilter AccDocFilter
	{
		get
		{
			return accDocFilterField;
		}
		set
		{
			accDocFilterField = value;
		}
	}
}
