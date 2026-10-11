using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class ExistInvalidationApprovedAccDocsRequest : GenericAuthenticatedRequest
{
	private string tINField;

	private string usernameField;

	[XmlElement(Order = 0)]
	public string TIN
	{
		get
		{
			return tINField;
		}
		set
		{
			tINField = value;
		}
	}

	[XmlElement(Order = 1)]
	public string Username
	{
		get
		{
			return usernameField;
		}
		set
		{
			usernameField = value;
		}
	}
}
