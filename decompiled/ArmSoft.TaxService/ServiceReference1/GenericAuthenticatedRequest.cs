using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[XmlInclude(typeof(GetBookDeclsRequest))]
[XmlInclude(typeof(GetAccDocsRequest))]
[XmlInclude(typeof(GetInvoicesRequest))]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class GenericAuthenticatedRequest
{
	private AuthToken authTokenField;

	[XmlElement(Order = 0)]
	public AuthToken AuthToken
	{
		get
		{
			return authTokenField;
		}
		set
		{
			authTokenField = value;
		}
	}
}
