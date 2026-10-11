using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ServiceReference1;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.taxservice.am/tp3/invoice/definitions")]
public class RegisterInvalidationDeclRequest : GenericAuthenticatedRequest
{
	private byte[] xmlFileField;

	[XmlElement(DataType = "base64Binary", Order = 0)]
	public byte[] xmlFile
	{
		get
		{
			return xmlFileField;
		}
		set
		{
			xmlFileField = value;
		}
	}
}
